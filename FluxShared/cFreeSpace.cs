// ReSharper disable InconsistentNaming

using System.Collections.Generic;

namespace FluxShared
{
	public class cFreeSpace
	{
		private class pair
		{
			public uint nStart;
			public uint nEnd;
            public pair(uint anStart, uint anEnd)
            {
				nStart = anStart;
				nEnd = anEnd;
            }
		}

		private List<pair> _freeSpace = new List<pair>();

		public void AddSpace(uint nStartOffset, uint nEndOffset) { }
		public bool FitsSpace(uint nOffset, uint nSpaceNeeded) { }
		public uint AddData(uint nSpaceNeeded) { }
		public void SortAndCollapse() { }
		private static int ComparePair(pair x, pair y) { }
		public bool ClaimSpace(uint nStartOffset, uint nEndOffset) { }
	}
}
