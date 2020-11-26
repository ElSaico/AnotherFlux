using System;
using System.Collections.Generic;
using Eto;
using FluxShared;
using GGRLib;

namespace AnotherFlux.Models
{
    internal class MainFormModel
    {
        public ChronoTriggerRom Rom { get; set; }
        private List<IFluxPlugin> _plugins;
        public byte[][] LecSize { get; }
        public Dictionary<string, int> RecTypes { get; }
        public string[] LocComType { get; }
        public string[] OWComType { get; }
        public string[] IfOp { get; }
        public string[] ObjFunc { get; }
        public string[] Animations { get; }

        public static void InitializeGlobalShared()
        {
            GlobalShared.PostStatus = sStatus =>
            {
                Console.Out.WriteLine(sStatus);
                //MainStatus.Text = $"{sStatus}  ({DateTime.Now.ToLongTimeString()})";
                //Update();
            };
            //GetStrFromGroup = getStrFromGroup;
        }

        protected void LoadPlugins()
        {
            _plugins = new Plugins().GetPlugins<IFluxPlugin>(EtoEnvironment.GetFolderPath(EtoSpecialFolder.EntryExecutable));
            foreach (var plugin in _plugins)
            {
                if (!plugin.Init())
                    GlobalShared.PostStatus($"Error - {plugin.sPlugName} failed to initialize.");
                else
                    ;//_pluginsMenu.Items.Add(plugin.PlugMenu.ActualItem);
            }
        }
    }
}