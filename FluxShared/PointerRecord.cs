using PSVRender;

namespace FluxShared
{
	public class PointerRecord
	{
		public uint nBank = 0x400000u;
		public uint nRange = 0x400000u;
		public uint nByte = 0x400000u;
		public short nAdjust;
		public bool bBank = true;
		public bool bAddress = true;

		public PointerRecord() { }

		public PointerRecord(uint anBank, uint anRange, uint anByte, short anAdjust, bool abBank, bool abAddress) =>
			SetData(anBank, anRange, anByte, anAdjust, abBank, abAddress);

		public PointerRecord(long nAddress) =>
			SetData((uint) nAddress + 2, (uint) nAddress + 1, (uint) nAddress, 0, true, true);

		public PointerRecord(long nAddress, short anAdjust, bool abBank, bool abAddress)
		{
			if (abBank && !abAddress)
				SetData((uint) nAddress, 0, 0, anAdjust, true, false);
			else
				SetData((uint) nAddress + 2, (uint) nAddress + 1, (uint) nAddress, anAdjust, abBank, abAddress);
		}

		private void SetData(uint anBank, uint anRange, uint anByte, short anAdjust, bool abBank, bool abAddress) {
			nBank = anBank;
			nRange = anRange;
			nByte = anByte;
			nAdjust = anAdjust;
			bBank = abBank;
			bAddress = abAddress;
		}

		public void SetData(PointerRecord Rec) => SetData(Rec.nBank, Rec.nRange, Rec.nByte, Rec.nAdjust, Rec.bBank, Rec.bAddress);

		public override bool Equals(object obj)
		{
			if (obj == null || !ReferenceEquals(GetType(), obj.GetType()))
				return false;
			var pointerRecord = (PointerRecord) obj;
			return nBank == pointerRecord.nBank && nRange == pointerRecord.nRange && nByte == pointerRecord.nByte && nAdjust == pointerRecord.nAdjust && bBank == pointerRecord.bBank && bAddress == pointerRecord.bAddress;
		}

		public uint GetFileOffset() => GlobalShared.GetFileOffset(new[] { nBank, nRange, nByte });

		public uint GetLocalFileOffset() => nByte & 0xFF0000U | SNES.GetShort(GlobalShared.WorkingData, nByte);

		public ushort SizedByAddressRecROMSize() => (ushort) (SNES.GetShort(GlobalShared.WorkingData, nByte + 2) - SNES.GetShort(GlobalShared.WorkingData, nByte));
	}
}
