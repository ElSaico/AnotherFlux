﻿namespace PSVRender
{
	public class SNES
	{
		//public static uint GetFileOffset(uint nSnesAddr) { }

		public static uint GetSnesAddr(uint nFileOffset)
		{
			return 0;
		}

		public static ushort GetShort(byte[] nSrc, long nOffset)
		{
			return 0;
		}

		public static ushort GetShort(byte[] nSrc, int nOffset)
		{
			return 0;
		}

		public static ushort GetShort(byte[] nSrc, uint nOffset)
		{
			return 0;
		}

		public static void SetShort(byte[] nDest, long nOffset, uint nVal) { }

		public static void SetShort(byte[] nDest, uint nOffset, ushort nVal) { }

		//public static uint GetInt24(byte[] nSrc, int nOffset) { }

		//public static uint GetInt24(byte[] nSrc, uint nOffset) { }

		public static void SetInt24(byte[] nDest, uint nOffset, uint nVal) { }

		public static string HexStr(byte nVal)
		{
			return "";
		}

		//public static string HexStr(ushort nVal) { }

		public static string HexStr(int nVal, int nPad)
		{
			return "";
		}

		//public static string HexStr(uint nVal, int nPad) { }
	}
}