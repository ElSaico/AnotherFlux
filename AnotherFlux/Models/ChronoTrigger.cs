using System.IO;
using FluxShared;
using AnotherFlux.Exceptions;

namespace AnotherFlux.Models
{
    public class ChronoTrigger
    {
        private const ushort ROM_HEADER_SIZE = 0x200;
        private const ushort ROM_BLOCK_SIZE = 0x8000;
        private const byte ROM_NAME_LENGTH = 21;
        private const string ROM_NAME = "CHRONO TRIGGER       ";
        private const uint ROM_INTERLEAVE_OFFSET = 0x200000;
        private const byte ROM_TYPE_EXHIROM = 0x35;

        private byte[] rawData;
        private RomType romType;
        private bool expandedRom;
        private bool allNlz;
        private bool dactylNlz;
        private bool startupLoc; // ???
        private bool betaEvents;

        public static bool IsRomHeadered(FileStream rom)
        {
            if ((rom.Length % ROM_BLOCK_SIZE) != 0)
            {
                if ((rom.Length % ROM_BLOCK_SIZE) != ROM_HEADER_SIZE)
                {
                    throw new ROMReadException("Incorrect ROM size");
                }
                return true;
            }
            return false;
        }

        public static void ScrubRomHeader(string filename)
        {
            byte[] romData;
            using (var file = File.OpenRead(filename))
            {
                using var reader = new BinaryReader(file);
                romData = reader.ReadBytes((int)file.Length);
            };
            using (var writer = new BinaryWriter(File.OpenWrite(filename)))
            {
                writer.Write(romData, ROM_HEADER_SIZE, romData.Length - ROM_HEADER_SIZE);
                writer.Flush();
            }
        }

        public static bool IsRomInterleaved(FileStream file)
        {
            using var reader = new BinaryReader(file);
            file.Seek((long)RomAddress.Name, SeekOrigin.Begin);
            if (reader.ReadChars(ROM_NAME_LENGTH).ToString() != ROM_NAME)
            {
                file.Seek((long)RomAddress.NameInterleaved, SeekOrigin.Begin);
                if (reader.ReadChars(ROM_NAME_LENGTH).ToString() != ROM_NAME)
                {
                    throw new ROMReadException("Incorrect ROM name");
                }
                return true;
            }
            return false;
        }

        public static void DeinterleaveRom(string filename)
        {
            byte nBlocks;
            byte[] romData;
            using (var file = File.OpenRead(filename))
            {
                using var reader = new BinaryReader(file);
                file.Seek(0L, SeekOrigin.Begin);
                romData = reader.ReadBytes((int)file.Length);
                nBlocks = (byte)(file.Length / ROM_BLOCK_SIZE);
            }
            using (var writer = new BinaryWriter(File.OpenWrite(filename)))
            {
                for (ushort i = 0; i < nBlocks; i++)
                {
                    writer.Write(romData, (int)((i * ROM_BLOCK_SIZE) | ROM_INTERLEAVE_OFFSET), ROM_BLOCK_SIZE);
                    writer.Write(romData, i * ROM_BLOCK_SIZE, ROM_BLOCK_SIZE);
                }
                writer.Flush();
            }
        }

        public ChronoTrigger(string filename)
        {
            using (var file = File.OpenRead(filename))
            {
                using var reader = new BinaryReader(file);
                file.Seek((long)RomAddress.Version, SeekOrigin.Begin);
                if (reader.ReadByte() != 0)
                {
                    throw new ROMReadException("Invalid ROM version; v1.0 required");
                }
                file.Seek(0L, SeekOrigin.Begin);
                rawData = reader.ReadBytes((int)file.Length);
            }

            romType = (RomType)rawData[(int)RomAddress.Region];
            if (romType == RomType.Japan && rawData[0xFF05] == 0xC7)
            { // TODO figure out the logic behind this test
                romType = RomType.Beta;
            }

            expandedRom = rawData[(int)RomAddress.MapMode] == ROM_TYPE_EXHIROM;

            // TODO check if the logic of these tests is not inverted
            allNlz = romType == RomType.Beta || rawData[GlobalShared.GetRomAddr(RomAddr.NLZPatch)] != 0x64;
            dactylNlz = romType == RomType.Beta || rawData[GlobalShared.GetRomAddr(RomAddr.DactylPatch)] == 0x64;
            startupLoc = romType == RomType.Beta || rawData[0x2E1A] != 0x20;
            betaEvents = romType == RomType.Beta && rawData[0x372012] != 0xA7;

            // here be dragons
            GlobalShared.PostStatus("Decompressing Location maps");
            G.SaveRec[1] = new SaveRecord[256];
            GetSimpleRec(G.SaveRec[1], 24582u, RomAddr.LocMap);
            GlobalShared.PostStatus("Decompressing Location Tile Assembly");
            G.SaveRec[5] = new SaveRecord[87];
            GetSimpleRec(G.SaveRec[5], 4096u, RomAddr.LocTileAsmL12);
            G.SaveRec[7] = new SaveRecord[23];
            GetSimpleRec(G.SaveRec[7], 4096u, RomAddr.LocTileAsmL3);
            GlobalShared.PostStatus("Getting Location exits");
            G.SaveRec[9] = new SaveRecord[1];
            G.SaveRec[9][0] = new SaveRecord();
            SaveRecord saveRecord = G.SaveRec[9][0];
            saveRecord.nMaxSize = 1792u;
            saveRecord.nPointerType = PointerType.SizedByAddress;
            saveRecord.nRecords = 512u;
            saveRecord.nRecSize = 7u;
            if (romType != RomType.USA)
            {
                saveRecord.Pointer = new PointerRecord[12];
                saveRecord.Pointer[0] = new PointerRecord(GlobalShared.GetRomAddr(RomAddr.LocExit));
                saveRecord.Pointer[1] = new PointerRecord(0x9CD6L, 0, true, false);
                saveRecord.Pointer[2] = new PointerRecord(0x9CDEL, 0, true, false);
                saveRecord.Pointer[3] = new PointerRecord(0x9CE8L, 0, true, false);
                saveRecord.Pointer[4] = new PointerRecord(0x9CF8L, 0, true, false);
                saveRecord.Pointer[5] = new PointerRecord(0x9D12L, 0, true, false);
                saveRecord.Pointer[6] = new PointerRecord(0x9D19L, 0, true, false);
                saveRecord.Pointer[7] = new PointerRecord(0x9D20L, 0, true, false);
                saveRecord.Pointer[8] = new PointerRecord(0xA6A6L);
                saveRecord.Pointer[9] = new PointerRecord(0xA6BBL, 0, true, false);
                saveRecord.Pointer[10] = new PointerRecord(0xA6C4L, 0, true, false);
                saveRecord.Pointer[11] = new PointerRecord(0xA6E4L, 0, true, false);
            }
            else
            {
                saveRecord.Pointer = new PointerRecord[11];
                saveRecord.Pointer[0] = new PointerRecord(GlobalShared.GetRomAddr(RomAddr.LocExit));
                saveRecord.Pointer[1] = new PointerRecord(0x9538L, 0, true, false);
                saveRecord.Pointer[2] = new PointerRecord(0x9540L, 0, true, false);
                saveRecord.Pointer[3] = new PointerRecord(0x954AL, 0, true, false);
                saveRecord.Pointer[4] = new PointerRecord(0x9564L, 0, true, false);
                saveRecord.Pointer[5] = new PointerRecord(0x956BL, 0, true, false);
                saveRecord.Pointer[6] = new PointerRecord(0x9572L, 0, true, false);
                saveRecord.Pointer[7] = new PointerRecord(0x9FBFL);
                saveRecord.Pointer[8] = new PointerRecord(0x9FD4L, 0, true, false);
                saveRecord.Pointer[9] = new PointerRecord(0x9FDDL, 0, true, false);
                saveRecord.Pointer[10] = new PointerRecord(0x9FFDL, 0, true, false);
            }
            saveRecord.nOrigAddr = GlobalShared.GetFileOffset((int)saveRecord.Pointer[0].nByte);
            saveRecord.Get();
            GlobalShared.PostStatus("Getting Treasure");
            G.SaveRec[13] = new SaveRecord[1];
            G.SaveRec[13][0] = new SaveRecord();
            saveRecord = G.SaveRec[13][0];
            saveRecord.nMaxSize = 1024u;
            saveRecord.nPointerType = PointerType.SizedByAddress;
            saveRecord.nRecords = 512u;
            saveRecord.nRecSize = 4u;
            if (romType != RomType.Beta)
            {
                saveRecord.Pointer = new PointerRecord[8];
                saveRecord.Pointer[0] = new PointerRecord(GlobalShared.GetRomAddr(RomAddr.Treasure));
                saveRecord.Pointer[1] = new PointerRecord(7707L, 0, true, false);
                saveRecord.Pointer[2] = new PointerRecord(7771L, 0, true, false);
                saveRecord.Pointer[3] = new PointerRecord(42827L, 2, true, true);
                saveRecord.Pointer[4] = new PointerRecord(42833L);
                saveRecord.Pointer[5] = new PointerRecord(42848L, 0, true, false);
                saveRecord.Pointer[6] = new PointerRecord(42854L, 0, true, false);
                saveRecord.Pointer[7] = new PointerRecord(42868L, 0, true, false);
            }
            else
            {
                saveRecord.Pointer = new PointerRecord[6];
                saveRecord.Pointer[0] = new PointerRecord(GlobalShared.GetRomAddr(RomAddr.Treasure));
                saveRecord.Pointer[1] = new PointerRecord(8229L, 0, true, false);
                saveRecord.Pointer[2] = new PointerRecord(8293L, 0, true, false);
                saveRecord.Pointer[3] = new PointerRecord(41060L);
                saveRecord.Pointer[4] = new PointerRecord(41068L);
                saveRecord.Pointer[5] = new PointerRecord(41084L, 0, true, false);
            }
            saveRecord.nOrigAddr = GlobalShared.GetFileOffset((int)saveRecord.Pointer[0].nByte);
            saveRecord.Get();
            GlobalShared.PostStatus("Decompressing Overworld Palettes");
            OverworldPalette = new SaveRecord[13];
            GetSimpleRec(OverworldPalette, 512u, RomAddr.OWPal);
            GlobalShared.PostStatus("Decompressing Overworld maps");
            G.SaveRec[0] = new SaveRecord[8];
            GetSimpleRec(G.SaveRec[0], 12288u, RomAddr.OWMap);
            GlobalShared.PostStatus("Decompressing Overworld Tile Assembly");
            G.SaveRec[6] = new SaveRecord[6];
            GetSimpleRec(G.SaveRec[6], 4096u, RomAddr.OWTileAsmL12);
            G.SaveRec[8] = new SaveRecord[6];
            GetSimpleRec(G.SaveRec[8], 4096u, RomAddr.OWTileAsmL3);
            GlobalShared.PostStatus("Decompressing Overworld exits");
            G.SaveRec[3] = new SaveRecord[8];
            GetOWExitsRec(G.SaveRec[3], 2816u, RomAddr.OWExit, false);
            GlobalShared.PostStatus("Decompressing Strings");
            G.SaveRec[11] = new SaveRecord[15];
            for (ushort num = 0; num < 15; num = (ushort)(num + 1))
            {
                G.SaveRec[11][num] = new StrSaveRecord();
            }
            StrForm.InitForm();
            GetLocEvents();
            TranslationOpen(false);
            GlobalShared.PostStatus("Decompressing Overworld Tile Properties");
            G.SaveRec[10] = new SaveRecord[8];
            G.GetSimpleRec(G.SaveRec[10], 512u, RomAddr.OWTileProps, true);
            GlobalShared.PostStatus("Decompressing Overworld Events");
            G.SaveRec[2] = new OESaveRecord[8];
            for (ushort num = 0; num < G.SaveRec[2].Length; num = (ushort)(num + 1))
            {
                G.SaveRec[2][num] = new OESaveRecord(this);
                OESaveRecord obj2 = (OESaveRecord)G.SaveRec[2][num];
                obj2.nMaxSize = 65536u;
                obj2.Pointer = new PointerRecord[1];
                obj2.Pointer[0] = new PointerRecord(GlobalShared.GetRomAddr(RomAddr.OWEvent) + num * 3);
                obj2.bCompressed = true;
                obj2.Get();
            }
            GlobalShared.PostStatus("Decompressing Overworld Music Transition Data");
            G.SaveRec[4] = new SaveRecord[8];
            G.GetSimpleRec(G.SaveRec[4], 3072u, RomAddr.OWMTD, true);
            GlobalShared.PostStatus("Getting Custom Defined Data");
            for (ushort num = 0; num < CDRec.Count; num = (ushort)(num + 1))
            {
                CDRec[num].bFullFlux = true;
                CDRec[num].Get();
            }
            GlobalShared.PostStatus("Init Music Form");
            MusicForm.FillSongs();
            MusicForm.FillInstruments();
            GlobalShared.PostStatus("Getting Misc Settings");
            MSForm.InitForm();
            if (((Control)CDForm).get_Visible())
            {
                CDForm.InitForm();
            }
            for (ushort num = 0; num < PlugList.Count; num = (ushort)(num + 1))
            {
                if (!PlugList[num].GetRecords())
                {
                    GlobalShared.PostStatus("Error - " + PlugList[num].sPlugName + " could not load its data.");
                }
            }
        }
    }
}
