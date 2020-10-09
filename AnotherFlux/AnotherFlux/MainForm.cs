using Eto.Forms;
using Eto.Drawing;

namespace AnotherFlux
{
    public sealed class MainForm : Form
    {
        public MainForm()
        {
            Title = "Another Flux";
            ClientSize = new Size(400, 350);
            
            Menu = new MenuBar
            {
                Items =
                {
                    new ButtonMenuItem
                    {
                        Text = "&File",
                        Items =
                        {
                            new ButtonMenuItem {Text = "&Open", Shortcut = Keys.Control | Keys.O},
                            new ButtonMenuItem {Text = "&Save", Enabled = false, Shortcut = Keys.Control | Keys.Shift | Keys.S},
                            new ButtonMenuItem {Text = "Save as...", Enabled = false},
                            new CheckMenuItem {Text = "Auto-Archive"},
                            new ButtonMenuItem {Text = "Mark All Modified", Enabled = false},
                            new SeparatorMenuItem(),
                            new ButtonMenuItem {Text = "Compression...", Enabled = false},
                            new ButtonMenuItem {Text = "Export...", Enabled = false},
                            new ButtonMenuItem {Text = "Import...", Enabled = false},
                            new ButtonMenuItem
                            {
                                Text = "Patches",
                                Items =
                                {
                                    new ButtonMenuItem {Text = "Expand ROM", Enabled = false},
                                    new ButtonMenuItem {Text = "All Overworlds have a NLZ", Enabled = false},
                                    new ButtonMenuItem {Text = "Dactyl NLZ is not origin based", Enabled = false},
                                    new ButtonMenuItem {Text = "Startup Location", Enabled = false},
                                    new ButtonMenuItem {Text = "Beta Events", Enabled = false}
                                }
                            }
                        }
                    },
                    new ButtonMenuItem {Text = "&Window", Enabled = false},
                    new ButtonMenuItem {Text = "&Plugins", Enabled = false},
                    new ButtonMenuItem
                    {
                        Text = "&Help",
                        Items =
                        {
                            new ButtonMenuItem {Text = "Manual", Shortcut = Keys.F1},
                            new ButtonMenuItem {Text = "Acknowledgements"}
                        }
                    }
                },
                QuitItem = new Command ((sender, e) => Application.Instance.Quit())
                {
                    MenuText = "Exit",
                    Shortcut = Keys.Alt | Keys.F4
                },
                AboutItem = new Command((sender, e) => new AboutDialog().ShowDialog(this))
                {
                    MenuText = "About..."
                }
            };
        }
    }
}