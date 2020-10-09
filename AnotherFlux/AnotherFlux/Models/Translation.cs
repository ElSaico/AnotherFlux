using FluxShared;
using PSVRender;
using System;
using System.Data;

namespace AnotherFlux.Models
{
    public class Translation
    {
		public string sOldDefault = "";

		public DataTable Alphabet = new DataTable("Dictionary");

		private DataColumn FontCol = new DataColumn("Font", Type.GetType("System.Byte"));

		private DataColumn Index1Col = new DataColumn("Alphabet", Type.GetType("System.Byte"));

		private DataColumn Index2Col = new DataColumn("Symbol", Type.GetType("System.Byte"));

		private DataColumn SubstringCol = new DataColumn("Substring", Type.GetType("System.Boolean"));

		private DataColumn ValCol = new DataColumn("Value", Type.GetType("System.String"));

		private DataColumn SpacingCol = new DataColumn("Spacing", Type.GetType("System.Byte"));

		private DataColumn SpacingHexCol = new DataColumn("SpacingHex", Type.GetType("System.String"));

		public string GetDictVal(byte nFont, byte nAlphabet, byte nSymbol)
		{
			DataRow dataRow = Alphabet.Rows.Find(new object[] { nFont, nAlphabet, nSymbol });
			if (dataRow != null)
			{
				return (string)dataRow["Value"];
			}
			return "";
		}

		public string DecompressString(byte[] nStrIn, uint nOffset, ushort nLen, AlphaType nType)
		{
			return DecompressString(nStrIn, nOffset, ref nLen, nType);
		}

		public string DecompressString(byte[] nStrIn, uint nOffset, ref ushort nLen, AlphaType nType)
		{
			string text = "";
			uint num = 0u;
			byte b = 0;
			if (nType == AlphaType.JapNulls)
			{
				nType = AlphaType.MainFont;
				b = 9;
			}
			byte nFont = (byte)nType;
			if ((int)nType < 2)
			{
				for (num = nOffset; num < nOffset + nLen && nStrIn[num] != b; num++)
				{
					if (nStrIn[num] == 3)
					{
						text += GetDictVal(nFont, 0, nStrIn[num]);
						text = text + SNES.HexStr(nStrIn[num + 1]) + "}";
						num++;
					}
					else if (nStrIn[num] == 18 || (GlobalShared.nRomType != 1 && (nStrIn[num] == 1 || nStrIn[num] == 2)))
					{
						num++;
						text += GetDictVal(0, nStrIn[num - 1], nStrIn[num]);
						nLen++;
					}
					else
					{
						text += GetDictVal(nFont, 0, nStrIn[num]);
					}
				}
				if (num < nOffset + nLen && nStrIn[num] == b)
				{
					text += GetDictVal(nFont, 0, nStrIn[num]);
				}
			}
			else if (nType == AlphaType.AsciiFont)
			{
				for (num = nOffset; num < nOffset + nLen && nStrIn[num] != 0 && nStrIn[num] != 128; num++)
				{
					text += GetDictVal(2, 0, nStrIn[num]);
				}
				if (num < nOffset + nLen && nStrIn[num] == 128)
				{
					text += GetDictVal(2, 0, nStrIn[num]);
				}
			}
			nLen = (ushort)(num - nOffset + 1);
			return text;
		}

		public string DecompressString(string sStrIn, AlphaType nType)
		{
			string text = "";
			int num = 0;
			if (nType == AlphaType.JapNulls)
			{
				nType = AlphaType.MainFont;
			}
			byte nFont = (byte)nType;
			if ((int)nType < 2)
			{
				for (num = 0; num < sStrIn.Length; num++)
				{
					if (sStrIn[num] == '\u0003')
					{
						text += GetDictVal(nFont, 0, (byte)sStrIn[num]);
						text = text + SNES.HexStr((byte)sStrIn[num + 1]) + "}";
						num++;
					}
					else if (sStrIn[num] == '\u0012' || (GlobalShared.nRomType != 1 && (sStrIn[num] == '\u0001' || sStrIn[num] == '\u0002')))
					{
						num++;
						text += GetDictVal(0, (byte)sStrIn[num - 1], (byte)sStrIn[num]);
					}
					else
					{
						text += GetDictVal(nFont, 0, (byte)sStrIn[num]);
					}
				}
			}
			else if (nType == AlphaType.AsciiFont)
			{
				for (num = 0; num < sStrIn.Length; num++)
				{
					text += GetDictVal(2, 0, (byte)sStrIn[num]);
				}
			}
			return text;
		}

		/*
		public string CompressString(string sOriginal, AlphaType nType, StrCompMode Mode, bool bNullEnd)
		{
			return CompressString(ref sOriginal, nType, Mode, bNullEnd);
		}

		public string CompressString(ref string sOriginal, AlphaType nType, StrCompMode Mode, bool bNullEnd)
		{
			StringReplacer stringReplacer = new StringReplacer();
			stringReplacer.TransForm = this;
			AlphaType alphaType = nType;
			if (sOriginal.Contains("{dup") && nType != AlphaType.Dialogue)
			{
				return "";
			}
			if (nType == AlphaType.JapNulls)
			{
				sOriginal = sOriginal.Replace(GetDictVal(0, 0, 0), GetDictVal(0, 0, 9));
			}
			stringReplacer.sOriginal = sOriginal;
			if (nType == AlphaType.Dialogue || nType == AlphaType.JapNulls)
			{
				nType = AlphaType.MainFont;
			}
			stringReplacer.nFont = (byte)nType;
			switch (nType)
			{
				case AlphaType.MainFont:
					{
						ushort nIndex = 0;
						while (nIndex < sOriginal.Length)
						{
							if (stringReplacer.Replace(ref nIndex, 0, 3))
							{
								try
								{
									stringReplacer.sCompressed += ((char)Convert.ToByte(sOriginal.Substring(nIndex, 2), 16)).ToString();
								}
								catch
								{
									stringReplacer.sCompressed += "\0";
								}
								nIndex = (ushort)(nIndex + 3);
							}
							else
							{
								if (stringReplacer.ReplaceRange(ref nIndex, 0, 6, 12) || stringReplacer.Replace(ref nIndex, 0, 0) || stringReplacer.Replace(ref nIndex, 0, 5) || stringReplacer.ReplaceRange(ref nIndex, 0, 13, 15) || stringReplacer.Replace(ref nIndex, 0, 17) || stringReplacer.ReplaceRange(ref nIndex, 18, 0, 1) || stringReplacer.ReplaceRange(ref nIndex, 0, 19, 32))
								{
									continue;
								}
								if (GlobalShared.nRomType == 1)
								{
									if (stringReplacer.ReplaceRange(ref nIndex, 0, 225, 226) || stringReplacer.Replace(ref nIndex, 0, 238) || stringReplacer.ReplaceRange(ref nIndex, 0, 240, 242))
									{
										continue;
									}
								}
								else if (stringReplacer.ReplaceRange(ref nIndex, 2, 240, 241))
								{
									continue;
								}
								if (Mode == StrCompMode.SpecialCharOnly)
								{
									stringReplacer.CopySymbol(ref nIndex);
									continue;
								}
								byte b = (byte)GlobalShared.GetRomValue(RomValue.Substring0Count);
								byte b2 = (byte)GlobalShared.GetRomValue(RomValue.SubstringStart);
								if (Mode == StrCompMode.Normal)
								{
									ushort num;
									for (num = 0; num < b; num = (ushort)(num + 1))
									{
										byte b3 = nSubID[b - num - 1];
										if (stringReplacer.Replace(ref nIndex, 0, (byte)(b2 + b3)))
										{
											break;
										}
									}
									if (num < b || (GlobalShared.nRomType == 0 && stringReplacer.ReplaceRange(ref nIndex, 18, 8, 191)))
									{
										continue;
									}
								}
								if ((GlobalShared.nRomType != 1 || !stringReplacer.Replace(ref nIndex, 0, 239)) && !stringReplacer.ReplaceRange(ref nIndex, 0, (byte)(b2 + b), byte.MaxValue) && (GlobalShared.nRomType != 0 || (!stringReplacer.ReplaceRange(ref nIndex, 1, 0, byte.MaxValue) && !stringReplacer.ReplaceRange(ref nIndex, 2, 0, byte.MaxValue))))
								{
									stringReplacer.sCompressed += "\0";
									break;
								}
							}
						}
						if (bNullEnd)
						{
							if (alphaType == AlphaType.JapNulls && !stringReplacer.sCompressed.EndsWith("\t"))
							{
								stringReplacer.sCompressed += "\t";
							}
							else if (!stringReplacer.sCompressed.EndsWith("\0"))
							{
								stringReplacer.sCompressed += "\0";
							}
						}
						break;
					}
				case AlphaType.ItemFont:
					{
						ushort nIndex = 0;
						while (nIndex < sOriginal.Length)
						{
							if (stringReplacer.Replace(ref nIndex, 0, 3))
							{
								stringReplacer.sCompressed += ((char)Convert.ToByte(sOriginal.Substring(nIndex, 2), 16)).ToString();
								nIndex = (ushort)(nIndex + 3);
							}
							else
							{
								if (stringReplacer.ReplaceRange(ref nIndex, 0, 6, 12) || stringReplacer.Replace(ref nIndex, 0, 0) || stringReplacer.Replace(ref nIndex, 0, 5) || stringReplacer.ReplaceRange(ref nIndex, 0, 13, 15) || stringReplacer.Replace(ref nIndex, 0, 17) || stringReplacer.ReplaceRange(ref nIndex, 18, 0, 1) || stringReplacer.ReplaceRange(ref nIndex, 0, 19, 31))
								{
									continue;
								}
								if (GlobalShared.nRomType == 1)
								{
									if (stringReplacer.ReplaceRange(ref nIndex, 0, 225, 226) || stringReplacer.Replace(ref nIndex, 0, 238) || stringReplacer.ReplaceRange(ref nIndex, 0, 240, 242))
									{
										continue;
									}
								}
								else if (stringReplacer.ReplaceRange(ref nIndex, 0, 240, 241))
								{
									continue;
								}
								if (!stringReplacer.ReplaceRange(ref nIndex, 0, 32, 41) && !stringReplacer.Replace(ref nIndex, 0, 46) && !stringReplacer.Replace(ref nIndex, 0, byte.MaxValue) && !stringReplacer.Replace(ref nIndex, 0, 47) && !stringReplacer.ReplaceRange(ref nIndex, 0, 64, 254) && !stringReplacer.ReplaceRange(ref nIndex, 0, 48, 63))
								{
									stringReplacer.sCompressed += "\0";
									break;
								}
							}
						}
						if (bNullEnd && !stringReplacer.sCompressed.EndsWith("\0"))
						{
							stringReplacer.sCompressed += "\0";
						}
						break;
					}
				case AlphaType.AsciiFont:
					{
						ushort nIndex = 0;
						while (nIndex < sOriginal.Length)
						{
							if (!stringReplacer.Replace(ref nIndex, 0, 32) && !stringReplacer.Replace(ref nIndex, 0, 122) && !stringReplacer.ReplaceRange(ref nIndex, 0, 46, 47) && !stringReplacer.Replace(ref nIndex, 0, 91) && !stringReplacer.Replace(ref nIndex, 0, 123) && !stringReplacer.Replace(ref nIndex, 0, 128) && !stringReplacer.ReplaceRange(ref nIndex, 0, 65, 90) && !stringReplacer.ReplaceRange(ref nIndex, 0, 96, 121))
							{
								stringReplacer.sCompressed += "\u0080";
								break;
							}
						}
						break;
					}
			}
			sOriginal = DecompressString(stringReplacer.sCompressed, nType);
			return stringReplacer.sCompressed;
		}
		*/
	}
}
