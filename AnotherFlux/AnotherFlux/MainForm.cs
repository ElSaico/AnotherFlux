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
                            new ButtonMenuItem {Text = "&Open"},
                            new ButtonMenuItem {Text = "&Save"},
                            new ButtonMenuItem {Text = "Save as..."},
                            new ButtonMenuItem {Text = "Auto-Archive"},
                            new ButtonMenuItem {Text = "Mark All Modified"},
                            new SeparatorMenuItem(),
                            new ButtonMenuItem {Text = "Compression..."},
                            new ButtonMenuItem {Text = "Export..."},
                            new ButtonMenuItem {Text = "Import..."},
                            new ButtonMenuItem
                            {
                                Text = "Patches",
                                Items =
                                {
                                    new ButtonMenuItem {Text = "Expand ROM"},
                                    new ButtonMenuItem {Text = "All Overworlds have a NLZ"},
                                    new ButtonMenuItem {Text = "Dactyl NLZ is not origin based"},
                                    new ButtonMenuItem {Text = "Startup Location"},
                                    new ButtonMenuItem {Text = "Beta Events"}
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
                    MenuText = "Exit"
                },
                AboutItem = new Command((sender, e) => new AboutDialog().ShowDialog(this))
                {
                    MenuText = "About..."
                }
            };
        }
    }
}