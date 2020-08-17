using System;

namespace AnotherFlux.Models
{
	[Serializable]
	internal class StringRec
	{
		public string sString = "";

		public string sOriginal = "";

		public uint nStringPointerAddress;

		public uint nStringAddress;

		public ushort nStringLength;

		public short nNewIndex = -1;

		public StringRec()
		{
		}

        public StringRec(string sStr, uint nStrPtrAddr, uint nStrAddr, ushort nStrLen)
		{
			sString = sStr;
			nStringPointerAddress = nStrPtrAddr;
			nStringAddress = nStrAddr;
			nStringLength = nStrLen;
		}
	}

}
