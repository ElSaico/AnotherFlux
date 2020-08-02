using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GGRLib
{
	public class MyDataGrid : DataGrid
	{
		public const int WM_KEYDOWN = 256;

		public const int WM_LBUTTONDOWN = 513;

		public const int WM_LBUTTONUP = 514;

		[DllImport("user32.dll")]
		private static extern bool SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

		public void ScrollToRow(int nRow) { }

		public void EditCell(int nRow, int nCol) { }
	}
}
