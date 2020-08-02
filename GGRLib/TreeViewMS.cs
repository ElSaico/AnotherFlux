using System.Collections.Generic;
using System.Windows.Forms;

namespace GGRLib
{
	public class TreeViewMS : TreeView
	{
		public List<TreeNode> SelectedNodes { get; set; }

		public TreeViewMS() { }
	}
}
