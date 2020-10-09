using System.IO;

namespace FluxShared
{
	public class SaveRecord
	{
		private const ushort MaxPointers = 256;

		public delegate void GetDel();
		public delegate bool SaveDel();
		public delegate bool ClaimDel();
		public delegate uint SizeDel();
		public delegate void ImportDel(BinaryReader Bin, ushort anSchema, ushort anVersion);
		public delegate void ExportDel(BinaryWriter Bout);
		public delegate void WriteTextDel(StreamWriter Sout);
		public delegate void ReseatDel();

		public ushort nID;
		public string sName = "";
		public uint nOrigAddr;
		public uint nOrigSize;
		public bool bOverride;

		public ushort nPointers;
		private PointerRecord[] pointer = new PointerRecord[MaxPointers];
		public PointerRecord[] Pointer {
			get => pointer;
			set {
				nPointers = (ushort)value.Length;
				pointer = value;
			}
		}
		private PointerType pointerType;
		public PointerType nPointerType {
			get => pointerType;
			set {
				pointerType = value;
				/*
				switch (pointerType)
				{
				case PointerType.SizedByAddress:
					RecGet = SizedByAddressGet;
					RecSave = SizedByAddressSave;
					RecClaim = SizedByAddressClaim;
					RecSize = SizedByAddressSize;
					RecReseat = SimpleReseat;
					RecImport = SizedByAddressImport;
					RecExport = SizedByAddressExport;
					break;
				case PointerType.OWExit:
					RecGet = OWExitGet;
					RecSave = SimpleSave;
					RecClaim = SimpleClaim;
					RecSize = OWExitSize;
					RecReseat = SimpleReseat;
					RecWriteText = OWExitWriteText;
					RecImport = OWExitImport;
					RecExport = OWExitExport;
					break;
				default:
					RecGet = SimpleGet;
					RecSave = SimpleSave;
					RecClaim = SimpleClaim;
					RecSize = SimpleSize;
					RecReseat = SimpleReseat;
					RecImport = SimpleImport;
					RecExport = SimpleExport;
					break;
				}
				*/
			}
		}

		public uint nMaxSize = 65536u;
		public uint nRecords = 1u;
		public uint nRecSize = 1u;
		public SaveRecord[] LocalRec;
		public bool bFullFlux;
		public byte[] nData;
		public uint nDataSize;
		public bool bCompressed;
		public bool bCreateEmpty;

		protected GetDel RecGet;
		protected SaveDel RecSave;
		protected ClaimDel RecClaim;
		protected SizeDel RecSize;
		protected ImportDel RecImport;
		protected ExportDel RecExport;
		protected WriteTextDel RecWriteText;
		protected ReseatDel RecReseat;

		public SaveRecord() {
			nPointerType = PointerType.Simple;
		 }

		public ushort CommonPointers(SaveRecord Rec)
		{
			ushort count = 0;
			for (int i = 0; i < nPointers; i++)
			{
				for (int j = 0; j < Rec.nPointers; j++)
				{
					if (Pointer[i].Equals(Rec.Pointer[j]))
					{
						count += 1;
					}
				}
			}
			return count;
		}

		public void MergePointers(SaveRecord Rec)
		{
			if (nPointers == MaxPointers) return;

			for (int i = 0; i < Rec.nPointers; i++)
			{
				int j;
				for (j = 0; j < nPointers && !Pointer[j].Equals(Rec.Pointer[i]); j++) { }
				if (j == nPointers)
				{
					Pointer[nPointers] = new PointerRecord();
					Pointer[nPointers].SetData(Rec.Pointer[i]);
					nPointers++;
				}
				if (nPointers == MaxPointers)
				{
					break;
				}
			}
		}

		public void Get() => RecGet();

		//public bool Save() { }

		public void Reseat() { }

		//public bool Claim() { }

		public uint Size()
		{
			return 0;
		}

		public void Import(BinaryReader Bin, ushort anSchema, ushort anVersion) { }

		public void Export(BinaryWriter Bout) { }
	}
}
