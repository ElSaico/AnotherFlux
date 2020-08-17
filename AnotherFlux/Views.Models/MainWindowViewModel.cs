using System.Collections.Generic;
using System.IO;
using System.Reflection;

using FluxShared;
using GGRLib;

namespace AnotherFlux.Views.Models
{
    public class MainWindowViewModel : ViewModelBase
    {
        public List<IFluxPlugin> plugins;

        public MainWindowViewModel()
        {
            string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            var ggrPlugins = new Plugins();
            plugins = ggrPlugins.GetPlugins<IFluxPlugin>(directory);
            foreach (var plugin in plugins)
            {
                if (!plugin.Init())
                {
                    GlobalShared.PostStatus($"Error - {plugin.sPlugName} failed to initialize.");
                }
                else
                {
                    //mnuPlug.MenuItems.Add(plugin.PlugMenu);
                }
            }
        }
    }
}
