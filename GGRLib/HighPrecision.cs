using System.Runtime.InteropServices;

namespace GGRLib
{
	public class HighPrecision
	{
		[DllImport("Kernel32.dll")]
		public static extern bool QueryPerformanceCounter(out long value);

		[DllImport("Kernel32.dll")]
		public static extern bool QueryPerformanceFrequency(out long value);
	}
}
