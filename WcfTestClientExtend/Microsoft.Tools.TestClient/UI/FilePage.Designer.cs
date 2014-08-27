using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using WcfTestClient;

namespace Microsoft.Tools.TestClient.UI
{
	internal class FilePage : TabPage
	{
		private IContainer components;

		private TextBox fileText;

		internal FilePage(string filePath)
		{
			this.InitializeComponent();
			this.Text = Path.GetFileName(filePath);
			this.LoadTextFromFile(filePath);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void fileText_GotFocus(object sender, EventArgs e)
		{
			this.fileText.Select(0, 0);
		}

		private void fileText_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && !e.Alt && !e.Shift && e.KeyCode == Keys.A)
			{
				this.fileText.SelectAll();
			}
		}

		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FilePage));
			this.fileText = new TextBox();
			base.SuspendLayout();
			this.fileText.CausesValidation = false;
			componentResourceManager.ApplyResources(this.fileText, "fileText");
			this.fileText.Name = "fileText";
			this.fileText.ReadOnly = true;
			this.fileText.GotFocus += new EventHandler(this.fileText_GotFocus);
			this.fileText.KeyDown += new KeyEventHandler(this.fileText_KeyDown);
			base.CausesValidation = false;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.fileText);
			base.Name = "FilePage";
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		private void LoadTextFromFile(string filePath)
		{
			if (!File.Exists(filePath))
			{
				this.fileText.Text = StringResources.ConfigNotFound;
				return;
			}
			try
			{
				this.fileText.Text = File.ReadAllText(filePath);
			}
			catch (IOException oException)
			{
			}
			catch (UnauthorizedAccessException unauthorizedAccessException)
			{
			}
		}

		internal void RefreshFile(string filePath)
		{
			this.LoadTextFromFile(filePath);
		}
	}
}