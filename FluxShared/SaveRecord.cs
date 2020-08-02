using System.IO;

namespace FluxShared
{
	public class SaveRecord
	{
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
		public PointerRecord[] Pointer { get; set; }
		public PointerType nPointerType { get; set; }

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

		public SaveRecord() { }

		public ushort CommonPointers(SaveRecord Rec) { }

		public void MergePointers(SaveRecord Rec) { }

		public void Get() { }

		public bool Save() { }

		public void Reseat() { }

		public bool Claim() { }

		public uint Size() { }

		public void Import(BinaryReader Bin, ushort anSchema, ushort anVersion) { }

		public void Export(BinaryWriter Bout) { }
	}
}
