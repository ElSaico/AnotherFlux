namespace System.Windows.Forms
{
    public class TreeView
    {
        private Eto.Forms.TreeGridView _actualView;
    }

    public class TreeNode
    {
        private Eto.Forms.TreeGridItem _actualNode;
    }

    public class Button
    {
        private Eto.Forms.Button _actualButton;
    }

    public class ComboBox
    {
        private Eto.Forms.ComboBox _actualBox;
    }

    public class TextBox
    {
        private Eto.Forms.TextBox _actualBox;
    }

    public class DataGrid
    {
        private Eto.Forms.Grid _actualGrid;
    }

    public class NumericUpDown
    {
        private Eto.Forms.Spinner _actualSpinner;
    }

    public class MenuItem
    {
        public readonly Eto.Forms.MenuItem ActualItem;
    }
}
