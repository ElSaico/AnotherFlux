namespace AnotherFlux.Models
{
    public enum RomAddress : uint
    {
        MakerCode = 0xFFB0,
        GameCode = 0xFFB2,
        HeaderZeros = 0xFFB6,
        ExpansionRamSize = 0xFFBD,
        SpecialVersion,
        CartridgeType,
        Name,
        MapMode = 0xFFD5,
        Type,
        Size,
        SramSize,
        Region,
        Header33,
        Version,
        NotChecksum,
        Checksum = 0xFFDE,
        NameInterleaved = 0x7FC0
    }
}
