using System;
using AnotherFlux.Models;
using Eto.Forms;
using Eto.Serialization.Xaml;

namespace AnotherFlux
{
	public class MainForm : Form
	{
		public MainForm()
		{
			XamlReader.Load(this);
			MainFormModel.InitializeGlobalShared();
		}

		protected void HandleAbout(object sender, EventArgs e)
		{
			new AboutDialog().ShowDialog(this);
		}

		protected void HandleExit(object sender, EventArgs e)
		{
			Application.Instance.Quit();
		}
	}
}
