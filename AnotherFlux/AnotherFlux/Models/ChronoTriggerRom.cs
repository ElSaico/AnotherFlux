using System.IO;
using AnotherFlux.Exceptions;
using FluxShared;

namespace AnotherFlux.Models
{
    public class ChronoTriggerRom
    {
        private const ushort RomHeaderSize = 0x200;
        private const ushort RomBlockSize = 0x8000;
        private const byte RomNameLength = 21;
        private const string RomName = "CHRONO TRIGGER       ";
        private const uint RomInterleaveOffset = 0x200000;
        private const byte RomTypeExhirom = 0x35;

        private byte[] _rawData;
        private readonly RomType _romType;

        private bool _expandedRom;
        private bool _patchAllNlz;
        private bool _patchDactylNlz;
        private bool _patchStartLocation;
        private bool _patchBetaEvents;
        private SaveRecord[] _locationMaps;
        private SaveRecord[] _locationTileAssemblyL3;
        private SaveRecord[] _locationTileAssemblyL12;
        private SaveRecord _locationExits;
        private SaveRecord _treasure;
        private SaveRecord[] _overworldPalettes;
        private SaveRecord[] _overworldMaps;
        private SaveRecord[] _overworldTileAssemblyL3;
        private SaveRecord[] _overworldTileAssemblyL12;
        private SaveRecord[] _overworldExits;
        private SaveRecord[] _strings;
        private SaveRecord[] _overworldTileProperties;
        private SaveRecord[] _overworldEvents;
        private SaveRecord[] _overworldMusicTransitionData;
        private SaveRecord[] _customData;
        private SaveRecord[] _locationEventsAndDialogue;
        private SaveRecord[] _substrings;

        public static bool IsRomHeadered(FileStream rom)
        {
            return (rom.Length % RomBlockSize) switch
            {
                0 => false,
                RomHeaderSize => true,
                _ => throw new RomReadException("Incorrect ROM size")
            };
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
                writer.Write(romData, RomHeaderSize, romData.Length - RomHeaderSize);
                writer.Flush();
            }
        }

        public static bool IsRomInterleaved(FileStream file)
        {
            using var reader = new BinaryReader(file);
            file.Seek((long)RomAddress.Name, SeekOrigin.Begin);
            if (reader.ReadChars(RomNameLength).ToString() == RomName) return false;
            file.Seek((long)RomAddress.NameInterleaved, SeekOrigin.Begin);
            if (reader.ReadChars(RomNameLength).ToString() == RomName) return true;
            throw new RomReadException("Incorrect ROM name");
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
                nBlocks = (byte)(file.Length / RomBlockSize);
            }
            using (var writer = new BinaryWriter(File.OpenWrite(filename)))
            {
                for (var i = 0; i < nBlocks; i++)
                {
                    writer.Write(romData, (int)((i * RomBlockSize) | RomInterleaveOffset), RomBlockSize);
                    writer.Write(romData, i * RomBlockSize, RomBlockSize);
                }
                writer.Flush();
            }
        }

        public ChronoTriggerRom(string filename)
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
                _rawData = reader.ReadBytes((int)file.Length);
            }

            _romType = _romType switch
            {
                RomType.Japan when _rawData[0xFF05] == 0xC7 => RomType.Beta, // original TF check, but what's the logic?
                _ => (RomType) _rawData[(int) RomAddress.Region]
            };

            _expandedRom = _rawData[(int)RomAddress.MapMode] == RomTypeExhirom;

            // TODO check if the logic of these tests is not inverted
            _patchAllNlz = _romType == RomType.Beta || _rawData[GlobalShared.GetRomAddr(RomAddr.NLZPatch)] != 0x64;
            _patchDactylNlz = _romType == RomType.Beta || _rawData[GlobalShared.GetRomAddr(RomAddr.DactylPatch)] == 0x64;
            _patchStartLocation = _romType == RomType.Beta || _rawData[0x2E1A] != 0x20;
            _patchBetaEvents = _romType == RomType.Beta && _rawData[0x372012] != 0xA7;

            _locationMaps = GetSaveRecords(256, 0x6006, RomAddr.LocMap);
            _locationTileAssemblyL12 = GetSaveRecords(87, 0x1000, RomAddr.LocTileAsmL12);
            _locationTileAssemblyL3 = GetSaveRecords(23, 0x1000, RomAddr.LocTileAsmL3);

            _locationExits = new SaveRecord
            {
                nMaxSize = 0x700,
                nPointerType = PointerType.SizedByAddress,
                nRecords = 0x200,
                nRecSize = 7
            };
            if (_romType != RomType.Usa)
            {
                _locationExits.Pointer = new[]
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
                _locationExits.Pointer = new[]
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
            _locationExits.nOrigAddr = GlobalShared.GetFileOffset((int)_locationExits.Pointer[0].nByte);
            _locationExits.Get();

            _treasure = new SaveRecord
            {
                nMaxSize = 0x400,
                nPointerType = PointerType.SizedByAddress,
                nRecords = 0x200,
                nRecSize = 4
            };
            if (_romType != RomType.Beta)
            {
                _treasure.Pointer = new[]
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
                _treasure.Pointer = new[]
                {
                    new PointerRecord(GlobalShared.GetRomAddr(RomAddr.Treasure)),
                    new PointerRecord(0x2025L, 0, true, false),
                    new PointerRecord(0x2065L, 0, true, false),
                    new PointerRecord(0xA064L),
                    new PointerRecord(0xA06CL),
                    new PointerRecord(0xA07CL, 0, true, false)
                };
            }
            _treasure.nOrigAddr = GlobalShared.GetFileOffset((int)_locationExits.Pointer[0].nByte);
            _treasure.Get();

            _overworldPalettes = GetSaveRecords(13, 0x200, RomAddr.OWPal);
            _overworldMaps = GetSaveRecords(8, 0x3000, RomAddr.OWMap);
            _overworldTileAssemblyL12 = GetSaveRecords(6, 0x1000, RomAddr.OWTileAsmL12);
            _overworldTileAssemblyL3 = GetSaveRecords(6, 0x1000, RomAddr.OWTileAsmL3);
            _overworldExits = GetSaveRecords(8, 0xB00, RomAddr.OWExit, PointerType.OWExit);

            _strings = new SaveRecord[15];
            /*
            for (var i = 0; i < strings.Length; i++)
            {
                strings[i] = new StrSaveRecord();
            }
            StrForm.InitForm();
            TranslationOpen(false);
            */

            /*
            locationEventsAndDialogue = GetSaveRecords<LESaveRecord>(0x201, 0x10000u, RomAddr.LocEvent);
            foreach (var record in locationEventsAndDialogue)
            {
                record.LoadDialogue();
            }
            */

            _overworldTileProperties = GetSaveRecords(8, 0x200, RomAddr.OWTileProps, true);
            // TODO Temporal Flux writes comments of overworld events to the filesystem, we probably need to do the same
            _overworldEvents = GetSaveRecords(8, 0x10000, RomAddr.OWEvent);
            _overworldMusicTransitionData = GetSaveRecords(8, 0xC00, RomAddr.OWMTD, true);

            // TODO handle custom data

            /*
            foreach (var plugin in plugins)
            {
                if (!plugin.GetRecords())
                {
                    GlobalShared.PostStatus($"Error - {plugin.sPlugName} could not load its data.");
                }
            }
            */
        }

        private static SaveRecord[] GetSaveRecords(uint length, uint maxSize, RomAddr baseAddr)
            => GetSaveRecords<SaveRecord>(length, maxSize, baseAddr, false);

        private static SaveRecord[] GetSaveRecords(uint length, uint maxSize, RomAddr baseAddr, bool createEmpty)
            => GetSaveRecords<SaveRecord>(length, maxSize, baseAddr, createEmpty);

        private static SaveRecord[] GetSaveRecords(uint length, uint maxSize, RomAddr baseAddr, PointerType pointerType)
            => GetSaveRecords<SaveRecord>(length, maxSize, baseAddr, pointerType);

        public T[] GetSaveRecords<T>(uint length, uint maxSize, RomAddr baseAddr)
            where T : SaveRecord, new() => GetSaveRecords<T>(length, maxSize, baseAddr, false, PointerType.Simple);

        private static T[] GetSaveRecords<T>(uint length, uint maxSize, RomAddr baseAddr, bool createEmpty)
            where T : SaveRecord, new() => GetSaveRecords<T>(length, maxSize, baseAddr, createEmpty, PointerType.Simple);

        private static T[] GetSaveRecords<T>(uint length, uint maxSize, RomAddr baseAddr, PointerType pointerType)
            where T : SaveRecord, new() => GetSaveRecords<T>(length, maxSize, baseAddr, false, pointerType);

        private static T[] GetSaveRecords<T>(uint length, uint maxSize, RomAddr baseAddr, bool createEmpty, PointerType pointerType)
            where T: SaveRecord, new()
        {
            var records = new T[length];
            for (ushort i = 0; i < length; i++)
            {
                records[i] = new T
                {
                    nID = i,
                    nPointerType = pointerType,
                    nMaxSize = maxSize,
                    Pointer = new[]
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
