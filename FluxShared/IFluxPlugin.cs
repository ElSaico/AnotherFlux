using System.Collections.Generic;

namespace FluxShared
{
    public interface IFluxPlugin
    {
#pragma warning disable IDE1006 // Naming Styles
        string sPlugName { get; }
        ushort nFluxSchema { get; }
        ushort nFluxVersion { get; }
        ushort nFluxMinSchema { get; }
        ushort nFluxMinVersion { get; }
#pragma warning restore IDE1006 // Naming Styles
        MenuItem PlugMenu { get; set; }
        List<SaveRecord[]> RecList { get; set; }
        Dictionary<string, int> RecDict { get; set; }

        bool Init();
        bool GetRecords();
        bool Close();
    }
}