using System;
using AnotherFlux.Models;
using Eto.Forms;

namespace AnotherFlux.Commands
{
    public class SaveCommand : Command
    {
        private readonly SaveFileDialog _saveRom = new SaveFileDialog
        {
            Filters =
            {
                new FileFilter("ROM Files", ".smc", ".sfc"),
                new FileFilter("All Files", ".*")
            }
        };

        public SaveCommand(MainForm form)
        {
            MenuText = "&Save";
            ToolBarText = "Save";
            Shortcut = Application.Instance.CommonModifier | Keys.Shift | Keys.S;
            DataContext = form.Rom;
            this.BindDataContext(c => c.Enabled, (ChronoTriggerRom rom) => rom.RomType != RomType.Beta);
        }

        protected override void OnExecuted(EventArgs e)
        {
            if (_saveRom.ShowDialog(Application.Instance.MainForm) != DialogResult.Ok) return;
        }
    }
}