using FluxShared;
using PSVRender;
using System;
using System.IO;

namespace AnotherFlux.Models
{
	/*
	internal class SubStrSaveRecord : SaveRecord
	{
		public SubStrSaveRecord()
		{
			RecGet = SubStrGet;
			RecSave = SubStrSave;
			RecSize = SubStrSize;
			RecExport = SubStrExport;
			RecImport = SubStrImport;
		}

		private void SubStrGet()
		{
			SubStrSaveRecord subStrSaveRecord = (SubStrSaveRecord)G.SaveRec[14][0];
			uint nOrigAddr = subStrSaveRecord.nOrigAddr;
			for (ushort num = 0; num < GlobalShared.GetRomValue(RomValue.Substring0Count); num = (ushort)(num + 1))
			{
				uint num2 = (nOrigAddr & 0xFF0000) + SNES.GetShort(G.MForm.RomData, nOrigAddr + (num << 1));
				ushort nLen = G.MForm.RomData[num2];
				string sValue = G.MForm.TransForm.DecompressString(G.MForm.RomData, num2 + 1, ref nLen, AlphaType.MainFont);
				subStrSaveRecord.nOrigSize += nLen;
				G.MForm.TransForm.AddDictRow(0, 0, (byte)(num + GlobalShared.GetRomValue(RomValue.SubstringStart)), sValue, bSubstring: true);
			}
			for (ushort num = 0; num < GlobalShared.GetRomValue(RomValue.Substring12Count); num = (ushort)(num + 1))
			{
				uint num3 = (nOrigAddr & 0xFF0000) + SNES.GetShort(G.MForm.RomData, nOrigAddr + (num + GlobalShared.GetRomValue(RomValue.Substring0Count) << 1));
				ushort nLen2 = G.MForm.RomData[num3];
				string sValue2 = G.MForm.TransForm.DecompressString(G.MForm.RomData, num3 + 1, ref nLen2, AlphaType.MainFont);
				subStrSaveRecord.nOrigSize += nLen2;
				G.MForm.TransForm.AddDictRow(0, 18, (byte)(num + 8), sValue2, bSubstring: true);
			}
			if (subStrSaveRecord.nOrigAddr == GlobalShared.GetRomAddr(RomAddr.Substring0OrigAddr) && GlobalShared.nRomType == 1)
			{
				subStrSaveRecord.nOrigSize += 6u;
			}
		}

		public bool SubStrSave()
		{
			ushort num = (ushort)(GlobalShared.GetRomValue(RomValue.Substring0Count) + GlobalShared.GetRomValue(RomValue.Substring12Count) << 1);
			ushort num2 = 0;
			byte[] array = new byte[65536];
			for (ushort num3 = 0; num3 < GlobalShared.GetRomValue(RomValue.Substring0Count); num3 = (ushort)(num3 + 1))
			{
				string dictVal = G.MForm.TransForm.GetDictVal(0, 0, (byte)(num3 + GlobalShared.GetRomValue(RomValue.SubstringStart)));
				if (!G.MForm.TransForm.ValidateString(dictVal, bSubstring: true))
				{
					return false;
				}
				string text = G.MForm.TransForm.CompressString(dictVal, AlphaType.MainFont, StrCompMode.SpecialCharOnly, bNullEnd: false);
				dictVal = G.MForm.TransForm.CompressString(dictVal, AlphaType.MainFont, StrCompMode.NoSubstrings, bNullEnd: false);
				num = (ushort)(num + num2);
				SNES.SetShort(array, num3 << 1, num);
				array[num] = (byte)text.Length;
				for (int i = 0; i < dictVal.Length; i++)
				{
					array[num + i + 1] = (byte)dictVal[i];
				}
				num2 = (ushort)(dictVal.Length + 1);
			}
			for (ushort num3 = 0; num3 < GlobalShared.GetRomValue(RomValue.Substring12Count); num3 = (ushort)(num3 + 1))
			{
				string dictVal = G.MForm.TransForm.GetDictVal(0, 18, (byte)(num3 + 8));
				if (!G.MForm.TransForm.ValidateString(dictVal, bSubstring: true))
				{
					return false;
				}
				string text = G.MForm.TransForm.CompressString(dictVal, AlphaType.MainFont, StrCompMode.SpecialCharOnly, bNullEnd: false);
				dictVal = G.MForm.TransForm.CompressString(dictVal, AlphaType.MainFont, StrCompMode.NoSubstrings, bNullEnd: false);
				num = (ushort)(num + num2);
				SNES.SetShort(array, num3 + GlobalShared.GetRomValue(RomValue.Substring0Count) << 1, num);
				array[num] = (byte)text.Length;
				for (int i = 0; i < dictVal.Length; i++)
				{
					array[num + i + 1] = (byte)dictVal[i];
				}
				num2 = (ushort)(dictVal.Length + 1);
			}
			num = (ushort)(num + num2);
			uint num4;
			if (GlobalShared.FreeSpace.FitsSpace(GlobalShared.GetFileOffset((int)GlobalShared.GetRomAddr(RomAddr.Substring0)), num))
			{
				num4 = GlobalShared.GetFileOffset((int)GlobalShared.GetRomAddr(RomAddr.Substring0));
			}
			else
			{
				num4 = GlobalShared.FreeSpace.AddData(num);
				if (num4 == 0)
				{
					GlobalShared.PostStatus("Error:  ROM out of free space.");
					return false;
				}
			}
			for (ushort num3 = 0; num3 < (ushort)(GlobalShared.GetRomValue(RomValue.Substring0Count) + GlobalShared.GetRomValue(RomValue.Substring12Count)); num3 = (ushort)(num3 + 1))
			{
				ushort num5 = (ushort)(num4 & 0xFFFF);
				ushort @short = SNES.GetShort(array, num3 << 1);
				@short = (ushort)(@short + num5);
				SNES.SetShort(array, (uint)(num3 << 1), @short);
			}
			Array.Copy(array, 0L, GlobalShared.WorkingData, num4, num);
			G.SaveRec[14][0].nOrigAddr = num4;
			G.SaveRec[14][0].nOrigSize = num;
			num4 = SNES.GetSnesAddr(num4);
			uint romAddr = GlobalShared.GetRomAddr(RomAddr.Substring0);
			SNES.SetInt24(GlobalShared.WorkingData, romAddr, num4);
			GlobalShared.WorkingData[romAddr + 11] = GlobalShared.WorkingData[romAddr + 2];
			romAddr = GlobalShared.GetRomAddr(RomAddr.Substring12);
			SNES.SetInt24(GlobalShared.WorkingData, romAddr, num4);
			GlobalShared.WorkingData[romAddr + 11] = GlobalShared.WorkingData[romAddr + 2];
			GlobalShared.WorkingData[romAddr - 5] = (byte)GlobalShared.GetRomValue(RomValue.Substring0Count);
			nOrigSize = Size();
			return true;
		}

		public uint SubStrSize()
		{
			uint num = (uint)(GlobalShared.GetRomValue(RomValue.Substring0Count) + GlobalShared.GetRomValue(RomValue.Substring12Count) << 1);
			ushort num2 = 0;
			for (ushort num3 = 0; num3 < GlobalShared.GetRomValue(RomValue.Substring0Count); num3 = (ushort)(num3 + 1))
			{
				string dictVal = G.MForm.TransForm.GetDictVal(0, 0, (byte)(num3 + GlobalShared.GetRomValue(RomValue.SubstringStart)));
				if (!G.MForm.TransForm.ValidateString(dictVal, bSubstring: true))
				{
					return 0u;
				}
				dictVal = G.MForm.TransForm.CompressString(dictVal, AlphaType.MainFont, StrCompMode.NoSubstrings, bNullEnd: false);
				num += num2;
				num2 = (ushort)(dictVal.Length + 1);
			}
			for (ushort num3 = 0; num3 < GlobalShared.GetRomValue(RomValue.Substring12Count); num3 = (ushort)(num3 + 1))
			{
				string dictVal = G.MForm.TransForm.GetDictVal(0, 18, (byte)(num3 + 8));
				if (!G.MForm.TransForm.ValidateString(dictVal, bSubstring: true))
				{
					return 0u;
				}
				dictVal = G.MForm.TransForm.CompressString(dictVal, AlphaType.MainFont, StrCompMode.NoSubstrings, bNullEnd: false);
				num += num2;
				num2 = (ushort)(dictVal.Length + 1);
			}
			return num + num2;
		}

		private void SubStrExport(BinaryWriter Bout)
		{
			Bout.Write(GlobalShared.GetRomValue(RomValue.Substring0Count));
			for (ushort num = 0; num < GlobalShared.GetRomValue(RomValue.Substring0Count); num = (ushort)(num + 1))
			{
				Bout.Write(G.MForm.TransForm.GetDictVal(0, 0, (byte)(num + GlobalShared.GetRomValue(RomValue.SubstringStart))));
			}
			Bout.Write(GlobalShared.GetRomValue(RomValue.Substring12Count));
			for (ushort num = 0; num < GlobalShared.GetRomValue(RomValue.Substring12Count); num = (ushort)(num + 1))
			{
				Bout.Write(G.MForm.TransForm.GetDictVal(0, 18, (byte)(num + 8)));
			}
		}

		private void SubStrImport(BinaryReader Bin, ushort anSchema, ushort anVersion)
		{
			ushort num = Bin.ReadUInt16();
			ushort num2 = (num <= GlobalShared.GetRomValue(RomValue.Substring0Count)) ? num : GlobalShared.GetRomValue(RomValue.Substring0Count);
			ushort num3;
			for (num3 = 0; num3 < num2; num3 = (ushort)(num3 + 1))
			{
				G.MForm.TransForm.SetDictVal(0, 0, (byte)(num3 + GlobalShared.GetRomValue(RomValue.SubstringStart)), Bin.ReadString());
			}
			while (num3 < num)
			{
				Bin.ReadString();
				num3 = (ushort)(num3 + 1);
			}
			num = Bin.ReadUInt16();
			num2 = ((num <= GlobalShared.GetRomValue(RomValue.Substring12Count)) ? num : GlobalShared.GetRomValue(RomValue.Substring12Count));
			for (num3 = 0; num3 < num2; num3 = (ushort)(num3 + 1))
			{
				G.MForm.TransForm.SetDictVal(0, 18, (byte)(num3 + 8), Bin.ReadString());
			}
			while (num3 < num)
			{
				Bin.ReadString();
				num3 = (ushort)(num3 + 1);
			}
		}
	}
	*/
}