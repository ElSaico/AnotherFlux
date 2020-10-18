namespace PSVRender
{
	public class SNES
	{
		public static uint GetFileOffset(uint nSnesAddr)
		{
			if (nSnesAddr < 0x600000)
				return nSnesAddr;
			return nSnesAddr < 0xC00000 ? 0 : nSnesAddr - 0xC00000;
		}

		public static uint GetSnesAddr(uint nFileOffset) => nFileOffset < 0x400000 ? nFileOffset + 0xC00000 : nFileOffset;

		public static ushort GetShort(byte[] nSrc, long nOffset) => GetShort(nSrc, (uint) nOffset);

		public static ushort GetShort(byte[] nSrc, int nOffset) => GetShort(nSrc, (uint) nOffset);

		public static ushort GetShort(byte[] nSrc, uint nOffset) => (ushort) (nSrc[nOffset] | (uint) nSrc[nOffset + 1] << 8);

		public static void SetShort(byte[] nDest, long nOffset, uint nVal) => SetShort(nDest, (uint) nOffset, (ushort) nVal);

		public static void SetShort(byte[] nDest, uint nOffset, ushort nVal)
		{
			nDest[nOffset] = (byte) (nVal & byte.MaxValue);
			nDest[nOffset + 1] = (byte) ((uint) nVal >> 8);
		}

		public static uint GetInt24(byte[] nSrc, int nOffset) => GetInt24(nSrc, (uint) nOffset);

		public static uint GetInt24(byte[] nSrc, uint nOffset) => (uint) (nSrc[nOffset] | nSrc[nOffset + 1] << 8 | nSrc[nOffset + 2] << 16);

		public static void SetInt24(byte[] nDest, uint nOffset, uint nVal)
		{
			nDest[nOffset] = (byte) (nVal & byte.MaxValue);
			nDest[nOffset + 1] = (byte) ((nVal & 0xFF00) >> 8);
			nDest[nOffset + 2] = (byte) (nVal >> 16);
		}


		public static string HexStr(byte nVal) => nVal.ToString("X").PadLeft(2, '0');

		public static string HexStr(ushort nVal) => nVal.ToString("X").PadLeft(4, '0');

		public static string HexStr(int nVal, int nPad) => HexStr((uint) nVal, nPad);

		public static string HexStr(uint nVal, int nPad) => nVal.ToString("X").PadLeft(nPad, '0');
	}
}