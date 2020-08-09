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
        private bool patchAllNlz;
        private bool patchDactylNlz;
        private bool patchStartLocation;
        private bool patchBetaEvents;
        private SaveRecord[] locationMaps;
        private SaveRecord[] locationTileAssemblyL3;
        private SaveRecord[] locationTileAssemblyL12;
        private SaveRecord locationExits;
        private SaveRecord treasure;
        private SaveRecord[] overworldPalettes;
        private SaveRecord[] overworldMaps;
        private SaveRecord[] overworldTileAssemblyL3;
        private SaveRecord[] overworldTileAssemblyL12;
        private SaveRecord[] overworldExits;
        private SaveRecord[] strings;
        private SaveRecord[] overworldTileProperties;
        private SaveRecord[] overworldEvents;
        private SaveRecord[] overworldMusicTransitionData;
        private SaveRecord[] customData;
        private SaveRecord[] locationEventsAndDialogue;
        private SaveRecord[] substrings;

        public static bool IsRomHeadered(FileStream rom)
        {
            if ((rom.Length % ROM_BLOCK_SIZE) != 0)
            {
                if ((rom.Length % ROM_BLOCK_SIZE) != ROM_HEADER_SIZE)
                {
                    throw new RomReadException("Incorrect ROM size");
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
                    throw new RomReadException("Incorrect ROM name");
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
                for (var i = 0; i < nBlocks; i++)
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
                    throw new RomReadException("Invalid ROM version; v1.0 required");
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
            patchAllNlz = romType == RomType.Beta || rawData[GlobalShared.GetRomAddr(RomAddr.NLZPatch)] != 0x64;
            patchDactylNlz = romType == RomType.Beta || rawData[GlobalShared.GetRomAddr(RomAddr.DactylPatch)] == 0x64;
            patchStartLocation = romType == RomType.Beta || rawData[0x2E1A] != 0x20;
            patchBetaEvents = romType == RomType.Beta && rawData[0x372012] != 0xA7;

            locationMaps = GetSaveRecords(256, 0x6006, RomAddr.LocMap);
            locationTileAssemblyL12 = GetSaveRecords(87, 0x1000, RomAddr.LocTileAsmL12);
            locationTileAssemblyL3 = GetSaveRecords(23, 0x1000, RomAddr.LocTileAsmL3);

            locationExits = new SaveRecord
            {
                nMaxSize = 0x700,
                nPointerType = PointerType.SizedByAddress,
                nRecords = 0x200,
                nRecSize = 7
            };
            if (romType != RomType.USA)
            {
                locationExits.Pointer = new PointerRecord[]
                {
                    new PointerRecord(GlobalShared.GetRomAddr(RomAddr.LocExit)),
                    new PointerRecord(0x9CD6L, 0, true, false),
                    new PointerRecord(0x9CDEL, 0, true, false),
                    new PointerRecord(0x9CE8L, 0, true, false),
                    new PointerRecord(0x9CF8L, 0, true, false),
                    new PointerRecord(0x9D12L, 0, true, false),
                    new PointerRecord(0x9D19L, 0, true, false),
                    new PointerRecord(0x9D20L, 0, true, false),
                    new PointerRecord(0xA6A6L),
                    new PointerRecord(0xA6BBL, 0, true, false),
                    new PointerRecord(0xA6C4L, 0, true, false),
                    new PointerRecord(0xA6E4L, 0, true, false)
                };
            }
            else
            {
                locationExits.Pointer = new PointerRecord[]
                {
                    new PointerRecord(GlobalShared.GetRomAddr(RomAddr.LocExit)),
                    new PointerRecord(0x9538L, 0, true, false),
                    new PointerRecord(0x9540L, 0, true, false),
                    new PointerRecord(0x954AL, 0, true, false),
                    new PointerRecord(0x9564L, 0, true, false),
                    new PointerRecord(0x956BL, 0, true, false),
                    new PointerRecord(0x9572L, 0, true, false),
                    new PointerRecord(0x9FBFL),
                    new PointerRecord(0x9FD4L, 0, true, false),
                    new PointerRecord(0x9FDDL, 0, true, false),
                    new PointerRecord(0x9FFDL, 0, true, false)
                };
            }
            locationExits.nOrigAddr = GlobalShared.GetFileOffset((int)locationExits.Pointer[0].nByte);
            locationExits.Get();

            treasure = new SaveRecord
            {
                nMaxSize = 0x400,
                nPointerType = PointerType.SizedByAddress,
                nRecords = 0x200,
                nRecSize = 4
            };
            if (romType != RomType.Beta)
            {
                treasure.Pointer = new PointerRecord[]
                {
                    new PointerRecord(GlobalShared.GetRomAddr(RomAddr.Treasure)),
                    new PointerRecord(0x1E1BL, 0, true, false),
                    new PointerRecord(0x1E5BL, 0, true, false),
                    new PointerRecord(0xA74BL, 2, true, true),
                    new PointerRecord(0xA751L),
                    new PointerRecord(0xA760L, 0, true, false),
                    new PointerRecord(0xA766L, 0, true, false),
                    new PointerRecord(0xA774L, 0, true, false)
                };
            }
            else
            {
                treasure.Pointer = new PointerRecord[]
                {
                    new PointerRecord(GlobalShared.GetRomAddr(RomAddr.Treasure)),
                    new PointerRecord(0x2025L, 0, true, false),
                    new PointerRecord(0x2065L, 0, true, false),
                    new PointerRecord(0xA064L),
                    new PointerRecord(0xA06CL),
                    new PointerRecord(0xA07CL, 0, true, false)
                };
            }
            treasure.nOrigAddr = GlobalShared.GetFileOffset((int)locationExits.Pointer[0].nByte);
            treasure.Get();

            overworldPalettes = GetSaveRecords(13, 0x200, RomAddr.OWPal);
            overworldMaps = GetSaveRecords(8, 0x3000, RomAddr.OWMap);
            overworldTileAssemblyL12 = GetSaveRecords(6, 0x1000, RomAddr.OWTileAsmL12);
            overworldTileAssemblyL3 = GetSaveRecords(6, 0x1000, RomAddr.OWTileAsmL3);
            overworldExits = GetSaveRecords(8, 0xB00, RomAddr.OWExit, PointerType.OWExit);

            strings = new SaveRecord[15];
            for (var i = 0; i < strings.Length; i++)
            {
                strings[i] = new StrSaveRecord();
            }
            StrForm.InitForm();
            GetLocEvents();
            TranslationOpen(false);

            overworldTileProperties = GetSaveRecords(8, 0x200, RomAddr.OWTileProps, true);
            // TODO Temporal Flux writes comments of overworld events to the filesystem,
            // we probably need to do the same
            overworldEvents = GetSaveRecords(8, 0x10000, RomAddr.OWEvent);
            overworldMusicTransitionData = GetSaveRecords(8, 0xC00, RomAddr.OWMTD, true);

            GlobalShared.PostStatus("Getting Custom Defined Data");
            for (var i = 0; i < CDRec.Count; i++)
            {
                CDRec[i].bFullFlux = true;
                CDRec[i].Get();
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

            for (var i = 0; i < PlugList.Count; i++)
            {
                if (!PlugList[i].GetRecords())
                {
                    GlobalShared.PostStatus("Error - " + PlugList[i].sPlugName + " could not load its data.");
                }
            }
        }

        public SaveRecord[] GetSaveRecords(uint length, uint maxSize, RomAddr baseAddr)
        {
            return GetSaveRecords(length, maxSize, baseAddr, false);
        }

        public SaveRecord[] GetSaveRecords(uint length, uint maxSize, RomAddr baseAddr, bool createEmpty)
        {
            return GetSaveRecords(length, maxSize, baseAddr, createEmpty, PointerType.Simple);
        }

        public SaveRecord[] GetSaveRecords(uint length, uint maxSize, RomAddr baseAddr, PointerType pointerType)
        {
            return GetSaveRecords(length, maxSize, baseAddr, false, pointerType);
        }

        public SaveRecord[] GetSaveRecords(uint length, uint maxSize, RomAddr baseAddr, bool createEmpty, PointerType pointerType)
        {
            var records = new SaveRecord[length];
            for (var i = 0; i < length; i++)
            {
                records[i] = new SaveRecord
                {
                    nPointerType = pointerType,
                    nMaxSize = maxSize,
                    Pointer = new PointerRecord[]
                    {
                        new PointerRecord(GlobalShared.GetRomAddr(baseAddr) + i * 3)
                    },
                    bCompressed = true,
                    bCreateEmpty = createEmpty
                };
                records[i].Get();
            }
            return records;
        }
    }
}
