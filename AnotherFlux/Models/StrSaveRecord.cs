using FluxShared;
using PSVRender;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

// TODO this and Translation (a bunch of business logic pulled from TranslationForm)
// are just _way_ too intermingled
namespace AnotherFlux.Models
{
	internal struct StringIndex
	{
		public uint nAddr;

		public byte nIndex;

		public StringIndex(uint nAddrIn, byte nIndexIn)
		{
			nAddr = nAddrIn;
			nIndex = nIndexIn;
		}
	}

	internal class StringSearchByAddress : IComparer<StringIndex>
	{
		public int Compare(StringIndex x, StringIndex y)
		{
			if (x.nAddr > y.nAddr)
			{
				return 1;
			}
			if (x.nAddr < y.nAddr)
			{
				return -1;
			}
			return 0;
		}
	}

	internal class StringSearchByIndex : IComparer<StringIndex>
	{
		public int Compare(StringIndex x, StringIndex y)
		{
			if (x.nIndex > y.nIndex)
			{
				return 1;
			}
			if (x.nIndex < y.nIndex)
			{
				return -1;
			}
			return 0;
		}
	}

	internal class StrSaveRecord : SaveRecord
	{
		public Dictionary<byte, StringRec> StrRec;

		private AlphaType nPrivAlphaType = AlphaType.ItemFont;

		public DataTable StringTable = new DataTable("String Table");

		private DataColumn IndexCol = new DataColumn("Index", Type.GetType("System.String"));

		private DataColumn ValCol = new DataColumn("Value", Type.GetType("System.String"));

		private DataColumn TranslationCol = new DataColumn("Original", Type.GetType("System.String"));

		private DataColumn CompCol = new DataColumn("Compressed", Type.GetType("System.String"));

		private DataColumn LengthCol = new DataColumn("Length", Type.GetType("System.String"));

		public bool bNullEnd;

		public bool bNoUpdate = true;

		public AlphaType nAlphaType
        {
            get => nPrivAlphaType;
            set
            {
                nPrivAlphaType = value;
                RecGet = StringsGet;
                RecImport = StringsImport;
                RecExport = StringsExport;
                switch (nPrivAlphaType)
                {
                    case AlphaType.MainFont:
                    case AlphaType.JapNulls:
                        RecSave = DynamicSave;
                        RecSize = DynamicSize;
                        RecClaim = DynamicClaim;
                        nRecSize = 0u;
                        break;
                    case AlphaType.AsciiFont:
                        RecSave = AsciiSave;
                        RecSize = AsciiSize;
                        nRecSize = 0u;
                        break;
                    case AlphaType.ItemFont:
                        RecSave = FixedLengthSave;
                        RecSize = FixedLengthSize;
                        break;
                    default:
                        RecSave = EmptySave;
                        nRecSize = 0u;
                        break;
                }
            }
        }

        public StrSaveRecord()
		{
			IndexCol.ReadOnly = true;
			ValCol.DefaultValue = "";
			StringTable.Columns.Add(IndexCol);
			StringTable.Columns.Add(ValCol);
			StringTable.Columns.Add(TranslationCol);
			StringTable.Columns.Add(LengthCol);
			StringTable.Columns.Add(CompCol);
			StringTable.DefaultView.AllowDelete = false;
			StringTable.RowChanged += UpdateRawString;
		}

		public void StringsGet()
		{
			if (StringTable.Rows.Count != 0)
			{
				StringTable.Rows.Clear();
			}
			switch (nPrivAlphaType)
			{
				case AlphaType.MainFont:
				case AlphaType.JapNulls:
					DynamicGet();
					break;
				case AlphaType.AsciiFont:
					AsciiGet();
					break;
				case AlphaType.ItemFont:
					FixedLengthGet();
					break;
			}
			if (StringTable.Rows.Count > 0)
			{
				string text = (string)StringTable.Rows[^1].ItemArray[1];
				if (text.Length > 0 && text[^1] == '\0')
				{
					bNullEnd = true;
				}
			}
			StringTable.AcceptChanges();
			bNoUpdate = false;
		}

		public void DynamicGet()
		{
			StrRec = new Dictionary<byte, StringRec>();
			List<StringIndex> list = new List<StringIndex>((int)nRecords);
			byte[] workingData = GlobalShared.WorkingData;
			for (byte idx = 0; idx < nRecords; idx++)
			{
				uint ptrAddr;
				if (nID == 14)
				{
					if (idx < 8)
					{
                        ptrAddr = (uint) ((RomType) GlobalShared.nRomType switch
                        {
                            RomType.Japan => nOrigAddr + (workingData[0x26944 + idx] << 1),
                            RomType.USA => nOrigAddr + (workingData[0x269F0 + idx] << 1),
							_ => nOrigAddr + (workingData[0x26DD3 + idx] << 1),
						});
                    }
					else
                    {
                        ptrAddr = (uint) ((RomType) GlobalShared.nRomType switch
                        {
                            RomType.Japan => nOrigAddr + (workingData[0x268F0] << 1),
                            RomType.USA => nOrigAddr + (workingData[0x2699C] << 1),
							_ => nOrigAddr + (workingData[0x26D7F] << 1),
						});
                    }
				}
				else
				{
					ptrAddr = (uint)(nOrigAddr + (idx << 1));
				}
				ushort bankAddr = SNES.GetShort(workingData, ptrAddr);
				uint dataAddr = (nOrigAddr & 0xFF0000) | bankAddr;
				DataRow StringRow = StringTable.NewRow();
				StringRow["Index"] = SNES.HexStr(idx);
				StringRow["Length"] = "000";
				StringRow["Compressed"] = "";
				var found = list.BinarySearch(new StringIndex(dataAddr, 0), new StringSearchByAddress());
				if (found > -1)
				{
					StringRow["Value"] = "{dup " + SNES.HexStr(list[found].nIndex) + "}";
					StrRec.Add(idx, new StringRec("", ptrAddr, dataAddr, 0));
				}
				else
				{
					ushort nLen = 0x1FF;
					GetRawString(ref StringRow, dataAddr, nLen);
					var text = DecompressString(workingData, dataAddr, ref nLen, nAlphaType);
					nOrigSize += nLen;
					StrRec.Add(idx, new StringRec(text, ptrAddr, dataAddr, nLen));
					StringRow["Value"] = text;
					list.Add(new StringIndex(dataAddr, idx));
					list.Sort(new StringSearchByIndex());
				}
				StringTable.Rows.Add(StringRow);
			}
		}

		private void AsciiGet()
		{
			byte idx = 0;
			uint ptr = nOrigAddr;
			while (GlobalShared.WorkingData[ptr] != 0)
			{
				DataRow StringRow = StringTable.NewRow();
				StringRow["Index"] = SNES.HexStr(idx);
				idx++;
				var rawString = GetRawString(ref StringRow, ptr, 0x1FF);
				StringRow["Value"] = DecompressString(GlobalShared.WorkingData, ptr, 0x1FF, nAlphaType);
				StringTable.Rows.Add(StringRow);
				ptr += rawString;
				nRecords++;
			}
			nDataSize = (ushort)(ptr - nOrigAddr);
			nOrigSize = nDataSize;
		}

		private void FixedLengthGet()
		{
			for (byte idx = 0; idx < nRecords; idx++)
			{
				uint ptr = nOrigAddr + idx * nRecSize;
				DataRow StringRow = StringTable.NewRow();
				StringRow["Index"] = SNES.HexStr(idx);
				GetRawString(ref StringRow, ptr, (ushort)nRecSize);
				StringRow["Value"] = DecompressString(GlobalShared.WorkingData, ptr, (byte)nRecSize, nAlphaType);
				StringTable.Rows.Add(StringRow);
			}
		}

		private bool FixedLengthSave()
		{
			byte fill = GlobalShared.nRomType == (byte)RomType.USA ? (byte)0xEF : byte.MaxValue;
            for (byte idx = 0; idx < StringTable.Rows.Count; idx++)
			{
				string compressedString = GetCompressedString(idx);
				ushort strIdx = 0;
				while (strIdx < compressedString.Length && strIdx < nRecSize)
				{
					GlobalShared.WorkingData[nOrigAddr + idx * nRecSize + strIdx] = (byte)compressedString[strIdx];
					strIdx++;
				}
				while (strIdx < nRecSize)
				{
					GlobalShared.WorkingData[nOrigAddr + idx * nRecSize + strIdx] = fill;
					strIdx++;
				}
			}
			return true;
		}

		private bool DynamicSave()
		{
			List<StringIndex> list = new List<StringIndex>();
			byte[] array = new byte[0x10000];
			ushort ptr = (ushort)(nRecords << 1);
			for (byte idx = 0; idx < nRecords; idx++)
			{
				string text = (string)StringTable.Rows[idx]["Value"];
				StringRec stringRec = StrRec[idx];
				if (text.IndexOf("{dup") == -1)
				{
					text = GetCompressedString(idx);
					SNES.SetShort(array, idx << 1, ptr);
					for (var strIdx = 0; strIdx < text.Length; strIdx++)
					{
						array[ptr + strIdx] = (byte)text[strIdx];
					}
					list.Add(new StringIndex(ptr, idx));
					stringRec.nStringLength = (ushort)text.Length;
					ptr += (ushort)text.Length;
				}
			}
			for (byte idx = 0; idx < nRecords; idx++)
			{
				string text = (string)StringTable.Rows[idx]["Value"];
				var dupIdxRef = text.IndexOf("{dup");
				StringRec stringRec = StrRec[idx];
				if (dupIdxRef > -1)
				{
					try
					{
						dupIdxRef = Convert.ToByte(text.Substring(dupIdxRef + 5, 2), 16);
					}
                    catch
					{
						GlobalShared.PostStatus("Error - Duplicate index must be specified in a two character hexidecimal format (eg: 0F)");
						dupIdxRef = 0;
					}
					var dupIdx = list.BinarySearch(new StringIndex(0u, (byte)dupIdxRef), new StringSearchByIndex());
					dupIdx = Math.Max(dupIdx, 0);
					if (dupIdx < list.Count)
					{
						SNES.SetShort(array, idx << 1, list[dupIdx].nAddr);
					}
					else
					{
						SNES.SetShort(array, idx << 1, 0u);
					}
					stringRec.nStringLength = 0;
				}
			}
			uint freePtr;
			if (nOrigAddr != 0 && GlobalShared.FreeSpace.FitsSpace(nOrigAddr, ptr))
			{
				freePtr = nOrigAddr;
			}
			else
			{
				freePtr = GlobalShared.FreeSpace.AddData(ptr);
				if (freePtr == 0)
				{
					GlobalShared.PostStatus("Error:  ROM out of free space.");
					return false;
				}
			}
			for (byte idx = 0; idx < nRecords; idx++)
			{
				ushort bankPtr = (ushort)(freePtr & 0xFFFF);
				ushort bankIdx = (ushort)(SNES.GetShort(array, idx << 1) + bankPtr);
				SNES.SetShort(array, (uint)(idx << 1), bankIdx);
				StringRec stringRec = StrRec[idx];
				stringRec.nStringAddress = ((freePtr & 0xFF0000) | bankIdx);
				stringRec.nStringPointerAddress = (uint)(freePtr + (idx << 1));
			}
			Array.Copy(array, 0L, GlobalShared.WorkingData, freePtr, ptr);
			nOrigAddr = freePtr;
			nOrigSize = ptr;
			PointersSave(freePtr);
			if (nID == 14)
			{
				switch ((RomType) GlobalShared.nRomType)
				{
					case RomType.Japan:
						{
							for (byte idx = 0; idx < 8; idx++)
							{
								GlobalShared.WorkingData[0x26944 + idx] = idx;
							}
							GlobalShared.WorkingData[0x268F0] = 8;
							GlobalShared.WorkingData[0x268DE] = 8;
							GlobalShared.WorkingData[0x268E5] = 1;
							GlobalShared.WorkingData[0x268D3] = 2;
							break;
						}
					case RomType.USA:
						{
							for (byte idx = 0; idx < 8; idx++)
							{
								GlobalShared.WorkingData[0x269F0 + idx] = idx;
							}
							GlobalShared.WorkingData[0x2699C] = 8;
							GlobalShared.WorkingData[0x2698A] = 8;
							GlobalShared.WorkingData[0x26991] = 1;
							GlobalShared.WorkingData[0x2697F] = 2;
							break;
						}
					case RomType.Beta:
						{
							for (byte idx = 0; idx < 8; idx++)
							{
								GlobalShared.WorkingData[0x26DD3 + idx] = idx;
							}
							GlobalShared.WorkingData[0x26D7F] = 8;
							GlobalShared.WorkingData[0x26D74] = 2;
							break;
						}
				}
			}
			nOrigSize = Size();
			return true;
		}

		private bool AsciiSave()
		{
			GC.Collect();
			byte[] array = new byte[0x10000];
			ushort size = 0;
			ushort strIdx = 0;
			for (byte idx = 0; idx < StringTable.Rows.Count; idx++)
			{
				string compressedString = GetCompressedString(idx);
				for (strIdx = 0; strIdx < compressedString.Length; strIdx++)
				{
					array[size + strIdx] = (byte)compressedString[strIdx];
				}
				size += (ushort)compressedString.Length;
			}
			array[size + strIdx] = 0;
			size += 1;

			uint romPtr;
			if (nOrigAddr != 0 && GlobalShared.FreeSpace.FitsSpace(nOrigAddr, size))
			{
				romPtr = nOrigAddr;
			}
			else
			{
				romPtr = GlobalShared.FreeSpace.AddData(size);
				if (romPtr == 0)
				{
					GlobalShared.PostStatus("Error:  ROM out of free space.");
					return false;
				}
			}
			Array.Copy(array, 0L, GlobalShared.WorkingData, romPtr, size);
			nOrigAddr = romPtr;
			nOrigSize = size;
			PointersSave(romPtr);
			return true;
		}

		private bool EmptySave()
		{
			return true;
		}

		private bool DynamicClaim()
		{
			byte[] workingData = GlobalShared.WorkingData;
			for (byte idx = 0; idx < nRecords; idx++)
			{
				uint offset;
				if (nID == 14)
				{
					if (idx < 8)
					{
                        offset = (RomType) GlobalShared.nRomType switch
                        {
                            RomType.Japan => (uint)(nOrigAddr + (workingData[0x26944 + idx] << 1)),
                            RomType.USA => (uint)(nOrigAddr + (workingData[0x269F0 + idx] << 1)),
							_ => (uint)(nOrigAddr + (workingData[0x26DD3 + idx] << 1)),
						};
                    }
					else
					{
                        offset = (RomType) GlobalShared.nRomType switch
                        {
                            RomType.Japan => (uint)(nOrigAddr + (workingData[0x268F0] << 1)),
                            RomType.USA => (uint)(nOrigAddr + (workingData[0x2699C] << 1)),
							_ => (uint)(nOrigAddr + (workingData[0x26D7F] << 1)),
						};
                    }
				}
				else
				{
					offset = (uint)(nOrigAddr + (idx << 1));
				}
				GlobalShared.FreeSpace.ClaimSpace(offset, offset + 2);
				ushort bankPtr = SNES.GetShort(workingData, offset);
				uint romAddr = (nOrigAddr & 0xFF0000) | bankPtr;
				string compressedString = GetCompressedString(idx);
				GlobalShared.FreeSpace.ClaimSpace(romAddr, (uint)(romAddr + compressedString.Length));
			}
			return true;
		}

		private uint DynamicSize()
		{
			uint size = nRecords << 1;
			for (byte idx = 0; idx < nRecords; idx++)
			{
				if (((string)StringTable.Rows[idx]["Value"]).IndexOf("{dup") == -1)
				{
					string compressedString = GetCompressedString(idx);
					size += (uint)compressedString.Length;
				}
			}
			return size;
		}

		private uint FixedLengthSize()
		{
			return nOrigSize;
		}

		private uint AsciiSize()
		{
			uint size = 0;
			for (byte idx = 0; idx < StringTable.Rows.Count; idx++)
			{
				size += (uint)GetCompressedString(idx).Length;
			}
			return size + 1;
		}

		private void StringsImport(BinaryReader Bin, ushort anSchema, ushort anVersion)
		{
			int size = Bin.ReadInt32();
			for (var idx = 0; idx < size; idx++)
			{
				StringTable.Rows[idx]["Value"] = Bin.ReadString();
			}
		}

		private void StringsExport(BinaryWriter Bout)
		{
			Bout.Write(StringTable.Rows.Count);
			for (var idx = 0; idx < StringTable.Rows.Count; idx++)
			{
				Bout.Write((string)StringTable.Rows[idx]["Value"]);
			}
		}

		public string GetCompressedString(byte nRow)
		{
			string text = "";
			string str = (string)StringTable.Rows[nRow]["Compressed"];
			for (var idx = 0; idx < str.Length; idx++)
			{
				text += ((char)Convert.ToUInt16(str.Substring(idx, 2), 16)).ToString();
			}
			return text;
		}

		public void InitTranslation()
		{
			for (int i = 0; i < StringTable.Rows.Count; i++)
			{
				bNoUpdate = true;
				StringTable.Rows[i]["Original"] = StringTable.Rows[i]["Value"];
				bNoUpdate = false;
				if (GlobalShared.nRomType == (byte)RomType.USA)
				{
					StringTable.Rows[i]["Value"] = "".PadRight((int)nRecSize, GetDictVal(0, 0, 0xEF)[0]);
				}
				else
				{
					StringTable.Rows[i]["Value"] = "".PadRight((int)nRecSize, GetDictVal(0, 0, byte.MaxValue)[0]);
				}
			}
		}

		public short FindDefault(bool bChange)
		{
			string defaultText = Translation.DefaultText.get_Text();
			string oldDefault = Translation.sOldDefault;
			string zero = GetDictVal(0, 0, 0); // TODO uhh what does this actually mean?
			if (nRecSize != 0)
			{
				var fill = GlobalShared.nRomType == (byte)RomType.USA ? GetDictVal(0, 0, 0xEF) : GetDictVal(0, 0, byte.MaxValue);
                defaultText = defaultText.Replace(zero, "").PadRight((int)nRecSize, fill[0]);
				oldDefault = oldDefault.Replace(zero, "").PadRight((int)nRecSize, fill[0]);
			}
			else if (nAlphaType == AlphaType.JapNulls)
			{
				defaultText = defaultText.Replace(zero, GetDictVal(0, 0, 9));
				oldDefault = oldDefault.Replace(zero, GetDictVal(0, 0, 9));
			}
			for (int i = 0; i < StringTable.Rows.Count; i++)
			{
				if ((string)StringTable.Rows[i]["Value"] == oldDefault)
				{
					if (!bChange)
					{
						return (short)i;
					}
					StringTable.Rows[i]["Value"] = defaultText;
					UpdateRawString(null, new DataRowChangeEventArgs(StringTable.Rows[i], DataRowAction.Change));
				}
			}
			return -1;
		}

		private ushort GetRawString(ref DataRow StringRow, uint nStrAddr, ushort nMaxLength)
		{
			string text = "";
			ushort num = 0;
			byte b = 0;
			if (nAlphaType == AlphaType.AsciiFont)
			{
				b = 128;
			}
			else if (nAlphaType == AlphaType.JapNulls)
			{
				b = 9;
			}
			uint num2 = nStrAddr;
			while (num < nMaxLength && GlobalShared.WorkingData[num2] != b)
			{
				text += SNES.HexStr(GlobalShared.WorkingData[num2]);
				num = (ushort)(num + 1);
				if (GlobalShared.WorkingData[num2] == 18 || GlobalShared.WorkingData[num2] == 1 || GlobalShared.WorkingData[num2] == 2)
				{
					num2++;
					text += SNES.HexStr(GlobalShared.WorkingData[num2]);
					num = (ushort)(num + 1);
				}
				num2++;
			}
			if (num < nMaxLength && GlobalShared.WorkingData[num2] == b)
			{
				text += SNES.HexStr(b);
				num = (ushort)(num + 1);
			}
			StringRow["Compressed"] = text;
			StringRow["Length"] = SNES.HexStr(num, 3);
			return num;
		}

		private void GetRawString(ref DataRow StringRow, string sStr)
		{
			string text = "";
			ushort num = 0;
			int num2 = 0;
			while (num < sStr.Length && sStr[num2] != 0)
			{
				text += SNES.HexStr((byte)sStr[num2]);
				num = (ushort)(num + 1);
				if (sStr[num2] == '\u0012' || sStr[num2] == '\u0001' || sStr[num2] == '\u0002')
				{
					num2++;
					text += SNES.HexStr((byte)sStr[num2]);
					num = (ushort)(num + 1);
				}
				num2++;
			}
			if (num < sStr.Length && sStr[num2] == '\0')
			{
				text += "00";
				num = (ushort)(num + 1);
			}
			StringRow["Compressed"] = text;
			StringRow["Length"] = SNES.HexStr(num, 3);
		}

		private void UpdateRawString(object sender, DataRowChangeEventArgs e)
		{
			if (bNoUpdate)
			{
				return;
			}
			bNoUpdate = true;
			if (e.Action == DataRowAction.Add || e.Action == DataRowAction.Change)
			{
				G.SaveRec[11][(byte)nID].bModified = true;
				DataRow StringRow = e.Row;
				string sOriginal = (string)StringRow["Value"];
				if (nRecSize == 0)
				{
					e.Row["Compressed"] = CompressString(ref sOriginal, nAlphaType, StrCompMode.Normal, bNullEnd);
				}
				else
				{
					e.Row["Compressed"] = CompressString(ref sOriginal, nAlphaType, StrCompMode.NoSubstrings, bNullEnd);
				}
				GetRawString(ref StringRow, (string)e.Row["Compressed"]);
				StringRow["Value"] = sOriginal;
			}
			StringTable.AcceptChanges();
			bNoUpdate = false;
		}

		public string GetStr(ushort nIndex)
		{
			try
			{
				return (string)StringTable.Rows[nIndex].ItemArray[1];
			}
			catch
			{
				return "";
			}
		}
	}
}