using System;
using System.IO;
using AnotherFlux.Exceptions;
using AnotherFlux.Models;
using Eto.Forms;
using FluxShared;

namespace AnotherFlux.Commands
{
    public class OpenCommand : Command
    {
        private const string ReadRomSuccessMessage = "ROM opened. Open windows not refreshed.";
        private const string ReadRomErrorMessage = "Error while trying to read file.  Is file open in another program?";
        private const string HeaderedRomQuestionMessage = "This ROM appears to have a header. Remove?";
        private const string HeaderedRomErrorMessage = "Error - Temporal Flux requires a non-headered ROM";
        private const string InterleavedRomQuestionMessage = "This ROM appears to be interleaved. De-interleave?";

        private readonly FileDialog _openRom = new OpenFileDialog
        {
            Filters =
            {
                new FileFilter("ROM Files", ".smc", ".sfc"),
                new FileFilter("All Files", ".*")
            }
        };

        protected override void OnExecuted(EventArgs e)
        {
            if (_openRom.ShowDialog(Application.Instance.MainForm) != DialogResult.Ok) return;
            try
            {
                using var fileStream = File.OpenRead(_openRom.FileName);
                if (ChronoTriggerRom.IsRomHeadered(fileStream))
                {
                    if (MessageBox.Show(Application.Instance.MainForm, HeaderedRomQuestionMessage,
                        MessageBoxButtons.YesNo, MessageBoxType.Error) != DialogResult.Yes)
                    {
                        GlobalShared.PostStatus(HeaderedRomErrorMessage);
                        return;
                    }
                    ChronoTriggerRom.ScrubRomHeader(_openRom.FileName);
                    GlobalShared.PostStatus("");
                }
            }
            catch
            {
                GlobalShared.PostStatus(ReadRomErrorMessage);
            }

            try
            {
                using var fileStream = File.OpenRead(_openRom.FileName);
                if (ChronoTriggerRom.IsRomInterleaved(fileStream) && MessageBox.Show(Application.Instance.MainForm, InterleavedRomQuestionMessage,
                    MessageBoxButtons.YesNo, MessageBoxType.Error) == DialogResult.Yes)
                {
                    ChronoTriggerRom.DeinterleaveRom(_openRom.FileName);
                    GlobalShared.PostStatus("");
                }
                DataContext = new ChronoTriggerRom(_openRom.FileName);
            }
            catch (RomReadException ex)
            {
                GlobalShared.PostStatus(ex.Message);
                return;
            }

            /*
            WindowMenu.Enabled = true;
            mnuCompress.Enabled = true;
            mnuExport.Enabled = GlobalShared.nRomType != (byte) RomType.Beta;
            mnuImport.Enabled = GlobalShared.nRomType != (byte) RomType.Beta;
            mnuMarkAll.Enabled = GlobalShared.nRomType != (byte) RomType.Beta;
            PluginsMenu.Enabled = true;
            LoadPlugins();
            */
            GlobalShared.PostStatus(ReadRomSuccessMessage);
        }
    }
}