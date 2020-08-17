using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GGRLib
{
	public class TreeViewMS : TreeView
	{
		protected List<TreeNode> nodes;

		protected TreeNode lastNode;

		protected TreeNode firstNode;

		public List<TreeNode> SelectedNodes
		{
			get
			{
				return nodes;
			}
			set
			{
				//removePaintFromNodes();
				nodes.Clear();
				nodes = value;
				//paintSelectedNodes();
			}
		}

		public TreeViewMS()
		{
			nodes = new List<TreeNode>();
		}

		/*
		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
		}

		protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
		{
			base.OnBeforeSelect(e);
			bool flag = Control.ModifierKeys == Keys.Control;
			bool flag2 = Control.ModifierKeys == Keys.Shift;
			if (!((TreeNodeExt)e.Node.Tag).bSelectable)
			{
				e.Cancel = true;
			}
			if (flag && nodes.Contains(e.Node))
			{
				e.Cancel = true;
				removePaintFromNodes();
				nodes.Remove(e.Node);
				paintSelectedNodes();
			}
			else
			{
				lastNode = e.Node;
				if (!flag2)
				{
					firstNode = e.Node;
				}
			}
		}

		protected override void OnAfterSelect(TreeViewEventArgs e)
		{
			base.OnAfterSelect(e);
			bool flag = Control.ModifierKeys == Keys.Control;
			bool flag2 = Control.ModifierKeys == Keys.Shift;
			if (flag)
			{
				if (!nodes.Contains(e.Node))
				{
					nodes.Add(e.Node);
				}
				else
				{
					removePaintFromNodes();
					nodes.Remove(e.Node);
				}
				paintSelectedNodes();
			}
			else if (flag2)
			{
				Queue<TreeNode> queue = new Queue<TreeNode>();
				TreeNode treeNode = firstNode;
				TreeNode treeNode2 = e.Node;
				bool flag3 = isParent(firstNode, e.Node);
				if (!flag3)
				{
					flag3 = isParent(treeNode2, treeNode);
					if (flag3)
					{
						TreeNode treeNode3 = treeNode;
						treeNode = treeNode2;
						treeNode2 = treeNode3;
					}
				}
				if (flag3)
				{
					for (TreeNode treeNode4 = treeNode2; treeNode4 != treeNode.Parent; treeNode4 = treeNode4.Parent)
					{
						if (!nodes.Contains(treeNode4) && ((TreeNodeExt)treeNode4.Tag).bSelectable)
						{
							queue.Enqueue(treeNode4);
						}
					}
				}
				else if ((treeNode.Parent == null && treeNode2.Parent == null) || (treeNode.Parent != null && treeNode.Parent.Nodes.Contains(treeNode2)))
				{
					int i = treeNode.Index;
					int index = treeNode2.Index;
					if (index < i)
					{
						TreeNode treeNode5 = treeNode;
						treeNode = treeNode2;
						treeNode2 = treeNode5;
						i = treeNode.Index;
						index = treeNode2.Index;
					}
					TreeNode treeNode6 = treeNode;
					for (; i <= index; i++)
					{
						if (!nodes.Contains(treeNode6) && ((TreeNodeExt)treeNode6.Tag).bSelectable)
						{
							queue.Enqueue(treeNode6);
						}
						treeNode6 = treeNode6.NextNode;
					}
				}
				else
				{
					if (!nodes.Contains(treeNode) && ((TreeNodeExt)treeNode.Tag).bSelectable)
					{
						queue.Enqueue(treeNode);
					}
					if (!nodes.Contains(treeNode2) && ((TreeNodeExt)treeNode2.Tag).bSelectable)
					{
						queue.Enqueue(treeNode2);
					}
				}
				nodes.AddRange(queue);
				paintSelectedNodes();
				firstNode = e.Node;
			}
			else
			{
				if (nodes != null && nodes.Count > 0)
				{
					removePaintFromNodes();
					nodes.Clear();
				}
				nodes.Add(e.Node);
			}
		}

		protected bool isParent(TreeNode parentNode, TreeNode childNode)
		{
			if (parentNode == childNode)
			{
				return true;
			}
			TreeNode treeNode = childNode;
			bool flag = false;
			while (!flag && treeNode != null)
			{
				treeNode = treeNode.Parent;
				flag = (treeNode == parentNode);
			}
			return flag;
		}

		protected void paintSelectedNodes()
		{
			foreach (TreeNode item in nodes)
			{
				if (item != null)
				{
					item.BackColor = SystemColors.Highlight;
					item.ForeColor = SystemColors.HighlightText;
				}
			}
		}

		protected void removePaintFromNodes()
		{
			if (nodes.Count != 0)
			{
				TreeNode treeNode = nodes[0];
				if (treeNode != null && treeNode.TreeView != null)
				{
					Color backColor = treeNode.TreeView.BackColor;
					foreach (TreeNode item in nodes)
					{
						item.BackColor = backColor;
						item.ForeColor = ((TreeNodeExt)item.Tag).DefForeColor;
					}
				}
			}
		}
		*/
	}
}
