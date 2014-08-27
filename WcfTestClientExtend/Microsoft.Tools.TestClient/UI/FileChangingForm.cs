using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using WcfTestClient;

namespace Microsoft.Tools.TestClient.UI
{
	internal class FileChangingForm : Form
	{
		private static IList<string> promptedFileList;

		private IContainer components;

		private Button yesButton;

		private Button noButton;

		private Label warningTextLabel;

		private TextBox fileNameTextBox;

		static FileChangingForm()
		{
			FileChangingForm.promptedFileList = new List<string>();
		}

		internal FileChangingForm()
		{
			this.InitializeComponent();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
        ComponentResourceManager componentResourceManager;
		private void InitializeComponent()
		{
			componentResourceManager = new ComponentResourceManager(typeof(FileChangingForm));
			this.yesButton = new Button();
			this.noButton = new Button();
			this.warningTextLabel = new Label();
			this.fileNameTextBox = new TextBox();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.yesButton, "yesButton");
			this.yesButton.Name = "yesButton";
			this.yesButton.UseVisualStyleBackColor = true;
			this.yesButton.Click += new EventHandler(this.yesButton_Click);
			this.noButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.noButton, "noButton");
			this.noButton.Name = "noButton";
			this.noButton.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.warningTextLabel, "warningTextLabel");
			this.warningTextLabel.Name = "warningTextLabel";
			this.fileNameTextBox.BorderStyle = BorderStyle.None;
			componentResourceManager.ApplyResources(this.fileNameTextBox, "fileNameTextBox");
			this.fileNameTextBox.Name = "fileNameTextBox";
			this.fileNameTextBox.ReadOnly = true;
			this.fileNameTextBox.TabStop = false;
			base.AcceptButton = this.yesButton;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.noButton;
			base.Controls.Add(this.fileNameTextBox);
			base.Controls.Add(this.warningTextLabel);
			base.Controls.Add(this.noButton);
			base.Controls.Add(this.yesButton);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "FileChangingForm";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		internal static bool Prompt(Form owner, string nodeName)
		{
			FileChangingForm fileChangingForm = new FileChangingForm();
			TextBox textBox = fileChangingForm.fileNameTextBox;
			CultureInfo currentUICulture = CultureInfo.CurrentUICulture;
			string configChanging = StringResources.ConfigChanging;
			object[] objArray = new object[] { nodeName };
			textBox.Text = string.Format(currentUICulture, configChanging, objArray);
			FileChangingForm.promptedFileList.Add(nodeName);
			fileChangingForm.ShowDialog(owner);
			FileChangingForm.promptedFileList.Remove(nodeName);
			return fileChangingForm.DialogResult == System.Windows.Forms.DialogResult.Yes;
		}

		internal static bool ShouldPrompt(string fileName)
		{
			if (FileChangingForm.promptedFileList.Contains(fileName))
			{
				return false;
			}
			return true;
		}

		private void yesButton_Click(object sender, EventArgs e)
		{
			base.DialogResult = System.Windows.Forms.DialogResult.Yes;
		}
	}
}