using System.Collections.Generic;
using AnotherFlux.Commands;
using AnotherFlux.Models;
using Eto;
using Eto.Drawing;
using Eto.Forms;
using FluxShared;
using GGRLib;

namespace AnotherFlux
{
    public class MainForm : Form
    {
        public ChronoTriggerRom Rom { get; set; }

        private readonly ButtonMenuItem _fileMenu;
        
        private readonly ButtonMenuItem _windowMenu = new ButtonMenuItem
        {
            Text = "&Window",
            Enabled = false,
            Items = 
            {
                new ButtonMenuItem {Text = "Locations", Shortcut = Keys.Control | Keys.L},
                new ButtonMenuItem {Text = "Location Events"},
                new ButtonMenuItem {Text = "Overworlds"},
                new ButtonMenuItem {Text = "Overworld Events"},
                new ButtonMenuItem {Text = "Strings"},
                new ButtonMenuItem {Text = "Misc Settings"},
                new ButtonMenuItem {Text = "Custom Data"},
                new ButtonMenuItem {Text = "Translation"},
                new SeparatorMenuItem(),
                new ButtonMenuItem
                {
                    Text = "Location",
                    Items =
                    {
                        new ButtonMenuItem {Text = "Properties"},
                        new ButtonMenuItem {Text = "Exits"},
                        new ButtonMenuItem {Text = "Treasure"},
                        new ButtonMenuItem {Text = "Tile Properties"},
                        new ButtonMenuItem {Text = "Map Properties"},
                        new ButtonMenuItem {Text = "Music"},
                    }
                },
                new ButtonMenuItem
                {
                    Text = "Overworld",
                    Items =
                    {
                        new ButtonMenuItem {Text = "Properties"},
                        new ButtonMenuItem {Text = "Exits"},
                        new ButtonMenuItem {Text = "Tile Properties"},
                        new ButtonMenuItem {Text = "Music Transition"},
                    }
                },
                new ButtonMenuItem
                {
                    Text = "Tile Swatches",
                    Items =
                    {
                        new ButtonMenuItem {Text = "Layer 1/2 Tiles"},
                        new ButtonMenuItem {Text = "Layer 3 Tiles"},
                        new ButtonMenuItem {Text = "Layer 1/2 Subtiles"},
                        new ButtonMenuItem {Text = "Layer 3 Subtiles"},
                    }
                },
                new SeparatorMenuItem(),
                new ButtonMenuItem {Text = "Zoom In", Shortcut = Keys.Control | Keys.Insert},
                new ButtonMenuItem {Text = "Zoom Out", Shortcut = Keys.Control | Keys.Delete},
                //new SeparatorMenuItem(),
                // TODO "Active Window"; Eto doesn't support MDI children yet - https://github.com/picoe/Eto/issues/1408
            }
        };

        private List<IFluxPlugin> _plugins;
        private readonly ButtonMenuItem _pluginsMenu = new ButtonMenuItem {Text = "&Plugins", Enabled = false};

        private readonly ButtonMenuItem _helpMenu = new ButtonMenuItem
        {
            Text = "&Help",
            Items =
            {
                new ButtonMenuItem {Text = "Manual", Shortcut = Keys.F1},
                new ButtonMenuItem {Text = "Acknowledgements"}
            }
        };

        private void LoadPlugins()
        {
            _plugins = new Plugins().GetPlugins<IFluxPlugin>(EtoEnvironment.GetFolderPath(EtoSpecialFolder.EntryExecutable));
            foreach (var plugin in _plugins)
            {
                if (!plugin.Init())
                    GlobalShared.PostStatus($"Error - {plugin.sPlugName} failed to initialize.");
                else
                    _pluginsMenu.Items.Add(plugin.PlugMenu.ActualItem);
            }
        }
        
        public MainForm()
        {
            MainFormModel.InitializeGlobalShared();
            Title = "Another Flux";
            _fileMenu = new ButtonMenuItem
            {
                Text = "&File",
                Items =
                {
                    new OpenCommand(this),
                    new SaveCommand(this),
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
            };
            ClientSize = new Size(600, 400);
            WindowState = WindowState.Maximized;
            Menu = new MenuBar
            {
                Items = {_fileMenu, _windowMenu, _pluginsMenu, _helpMenu},
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
            Content = new StackLayout
            {
                Orientation = Orientation.Vertical,
                Items =
                {
                    new StackLayoutItem(new StackLayout(), true),
                    /* TODO set borders, etc.
                    new StackLayoutItem(
                        new StackLayout
                        {
                            Orientation = Orientation.Horizontal,
                            Items =
                            {
                                new StackLayoutItem(new Label(), true),
                                new StackLayoutItem(new Label {Width = 100, Text = "(400, 400)"})
                            }
                        }
                    )
                    */
                }
            };
        }
    }
}