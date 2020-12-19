using System;
using System.Collections.Generic;
using Eto;
using FluxShared;
using GGRLib;
using Newtonsoft.Json.Linq;

namespace AnotherFlux.Models
{
    internal class MainFormModel
    {
        public ChronoTriggerRom Rom { get; set; }
        private List<IFluxPlugin> _plugins;
        public static byte[][] LecSize { get; }
        public static Dictionary<string, int> RecTypes { get; }
        public static string[] LocComType { get; }
        public static string[] OWComType { get; }
        public static string[] IfOp { get; }
        public static string[] ObjFunc { get; }
        public static string[] Animations { get; }

        static MainFormModel()
        {
            var properties = JObject.Parse(Properties.Resources.RomProperties);
            LecSize = properties["LecSize"].ToObject<byte[][]>();
            RecTypes = properties["RecTypes"].ToObject<Dictionary<string, int>>();
            LocComType = properties["LocComType"].ToObject<string[]>();
            OWComType = properties["OWComType"].ToObject<string[]>();
            IfOp = properties["IfOp"].ToObject<string[]>();
            ObjFunc = properties["ObjFunc"].ToObject<string[]>();
            Animations = properties["Animations"].ToObject<string[]>();
            GlobalShared.nRomAddr = properties["nRomAddr"].ToObject<List<uint[]>>();
            GlobalShared.nRomValue = properties["nRomValue"].ToObject<List<ushort[]>>();
            GlobalShared.KnownAddrHash = properties["KnownAddrHash"].ToObject<Dictionary<uint, string>>();
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