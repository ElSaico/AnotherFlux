using System.Collections.Generic;

namespace FluxShared
{
    public interface IFluxPlugin
    {
        string sPlugName { get; }
        ushort nFluxSchema { get; }
        ushort nFluxVersion { get; }
        ushort nFluxMinSchema { get; }
        ushort nFluxMinVersion { get; }
        //MenuItem PlugMenu { get; set; }
        List<SaveRecord[]> RecList { get; set; }
        Dictionary<string, int> RecDict { get; set; }

        bool Init();
        bool GetRecords();
        bool Close();
    }
}