using System.Drawing;

namespace PSVRender
{
	public class PSV
	{
		internal static byte[] BitPlaneBits = {
			1,
            2,
			2,
			2,
			3,
			4,
			4,
			4,
			8,
			8,
			8,
			8,
			15,
			16,
			24,
			32
		};

		//internal static bool IsRBGMode(BitPlanes nBitPlanes) { }

		public static void GetGreyPalette(ref Color[] WorkingPalette, ushort nColors) { }
	}

}
