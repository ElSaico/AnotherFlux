using System.Drawing;

namespace PSVRender
{
	public class GetSnesPaletteVar
	{
		public byte[] nSnesPalette;
		public Color[] WorkingPalette;
		public uint nSnesStartIndex;
		public uint nWorkStartIndex;
		public ushort nPaletteSize = 16;
		public ushort nPaletteColors = 16;
		public ushort nPaletteShift;
		public byte nNumPalettes = 1;

		public void GetSnesPalette() { }
	}
}
