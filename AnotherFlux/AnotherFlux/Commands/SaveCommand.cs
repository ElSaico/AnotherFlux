using AnotherFlux.Models;
using Eto.Forms;
using FluxShared;

namespace AnotherFlux.Commands
{
    public class SaveCommand : Command
    {
        public override bool Enabled => ((MainForm) Application.Instance.MainForm).Rom != null && GlobalShared.nRomType != (byte)RomType.Beta;

        public SaveCommand()
        {
            MenuText = "&Save";
            ToolBarText = "Save";
            Shortcut = Application.Instance.CommonModifier | Keys.Shift | Keys.S;
        }
    }
}