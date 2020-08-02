namespace FluxShared
{
	public class PointerRecord
	{
		public uint nBank = 4194304u;
		public uint nRange = 4194304u;
		public uint nByte = 4194304u;
		public short nAdjust;
		public bool bBank = true;
		public bool bAddress = true;

		public PointerRecord() { }

		public PointerRecord(uint anBank, uint anRange, uint anByte, short anAdjust, bool abBank, bool abAddress) {
			SetData(anBank, anRange, anByte, anAdjust, abBank, abAddress);
		}

		public PointerRecord(long nAddress) { }

		public PointerRecord(long nAddress, short anAdjust, bool abBank, bool abAddress) { }

		private void SetData(uint anBank, uint anRange, uint anByte, short anAdjust, bool abBank, bool abAddress) {
			nBank = anBank;
			nRange = anRange;
			nByte = anByte;
			nAdjust = anAdjust;
			bBank = abBank;
			bAddress = abAddress;
		}

		public void SetData(PointerRecord Rec) { }

		public override bool Equals(object obj) { }

		public override int GetHashCode() { }

		public uint GetFileOffset() { }

		public uint GetLocalFileOffset() { }

		public ushort SizedByAddressRecROMSize() { }
	}
}
