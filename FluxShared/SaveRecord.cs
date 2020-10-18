using System;
using System.IO;
using Eto.Forms;
using PSVRender;

namespace FluxShared
{
	public class SaveRecord
	{
		private const ushort MaxPointers = 0x100;

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
				switch (pointerType)
				{
				case PointerType.SizedByAddress:
					/*
					RecGet = SizedByAddressGet;
					RecSave = SizedByAddressSave;
					RecClaim = SizedByAddressClaim;
					RecSize = SizedByAddressSize;
					RecReseat = SimpleReseat;
					RecImport = SizedByAddressImport;
					RecExport = SizedByAddressExport;
					*/
					break;
				case PointerType.OWExit:
					/*
					RecGet = OWExitGet;
					RecSave = SimpleSave;
					RecClaim = SimpleClaim;
					RecSize = OWExitSize;
					RecReseat = SimpleReseat;
					RecWriteText = OWExitWriteText;
					RecImport = OWExitImport;
					RecExport = OWExitExport;
					*/
					break;
				default:
					RecGet = SimpleGet;
					/*
					RecSave = SimpleSave;
					RecClaim = SimpleClaim;
					RecSize = SimpleSize;
					RecReseat = SimpleReseat;
					RecImport = SimpleImport;
					RecExport = SimpleExport;
					*/
					break;
				}
			}
		}

		public uint nMaxSize = 0x10000;
		public uint nRecords = 1;
		public uint nRecSize = 1;
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
			for (var i = 0; i < nPointers; i++)
			{
				for (var j = 0; j < Rec.nPointers; j++)
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

			for (var i = 0; i < Rec.nPointers; i++)
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

		/*
		public bool Save() { }

		public void Reseat() { }

		public bool Claim() { }

		public uint Size() { }

		public void Import(BinaryReader Bin, ushort anSchema, ushort anVersion) { }

		public void Export(BinaryWriter Bout) { }
		*/
		
		private void SimpleGet()
		{
			nData = new byte[nMaxSize];
			if (Pointer[0] != null)
			{
				var fileOffset = Pointer[0].GetFileOffset();
				if (bFullFlux && nOrigAddr != fileOffset)
				{
					if (MessageBox.Show(
						$"Pointer does not point to data for record {SNES.HexStr(nID)} {sName}.\r\nOverride data and get from pointer?", "Get Error", MessageBoxButtons.YesNo, MessageBoxType.Warning, MessageBoxDefaultButton.No) == DialogResult.Yes)
						nOrigAddr = fileOffset;
				}
				else
					nOrigAddr = fileOffset;
			}
			if (nOrigAddr + nOrigSize > GlobalShared.WorkingData.Length)
				nOrigAddr = 0;
			if (bCompressed)
			{
				if (nOrigAddr == 0 || nOrigAddr > 0xFFFFFF)
				{
					if (!bCreateEmpty)
						return;
					nDataSize = nMaxSize;
					nOrigSize = 0;
				}
				else
				{
					var compressionDataVar = new CompressionDataVar
					{
						nDecRoutine = CompressionRoutines.eChronoTrigger,
						nSrcOff = nOrigAddr,
						SrcBuffer = GlobalShared.WorkingData,
						WorkingBuffer = nData
					};
					compressionDataVar.DecompressData();
					nOrigSize = (ushort) compressionDataVar.nCompressedSize;
					nDataSize = (ushort) compressionDataVar.nDecompressedSize;
				}
			}
			else
				Array.Copy(GlobalShared.WorkingData, nOrigAddr, nData, 0, nOrigSize);
		}
	}
}
