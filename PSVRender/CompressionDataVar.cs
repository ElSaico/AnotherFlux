namespace PSVRender
{
	public class CompressionDataVar
	{
		public uint nCompressedSize;
		public uint nDecompressedSize;
		public CompressionRoutines nDecRoutine;
		public byte[] SrcBuffer;
		public uint nSrcOff;
		public byte[] WorkingBuffer;
		public uint nWorkOff;

		public void DecompressData() { }
		public void CompressData() { }
	}
}
