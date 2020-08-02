using System.Drawing;

namespace PSVRender
{
	public class PutSubtileVar
	{
		public int dx;
		public int dy;
		public byte nPaletteShift;
		public byte nFlip;
		public unsafe byte* nWorkingImage;
		public Bitmap WorkingImage;
		public byte[] WorkingBuffer;
		public Color[] WorkingPalette;
		public BitPlanes nBitPlanes = BitPlanes.eTetraPlane;
		public uint nDecodingStart;
		public byte bDimmed;
		public bool bSwapRB;
		public bool bHalfColorRender;
		public PixelRenderMode PRMode;
		public Bitmap SubImage;
		public unsafe byte* nSubImage;

		public PutSubtileVar() { }

		public void PutSubtile() { }

		public unsafe void RenderTetraPlane() { }

		public unsafe void RenderOctaPlane() { }

		public unsafe void ProcessSpriteData() { }

		public unsafe void ClearArea(ushort nTop, ushort nBottom, ushort nLeft, ushort nRight) { }
	}

}
