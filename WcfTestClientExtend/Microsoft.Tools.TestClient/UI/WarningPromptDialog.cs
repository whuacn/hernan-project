using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Microsoft.Tools.TestClient.UI
{
	internal class WarningPromptDialog : Form
	{
		private IContainer components;

		private Button cancelButton;

		private Button okButton;

		private CheckBox checkDonotPromptAgain;

		private TextBox txtWarningText;

		internal WarningPromptDialog() : this(string.Empty)
		{
		}

		internal WarningPromptDialog(string warningText)
		{
			this.InitializeComponent();
			this.txtWarningText.Text = warningText;
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
			componentResourceManager = new ComponentResourceManager(typeof(WarningPromptDialog));
			this.cancelButton = new Button();
			this.okButton = new Button();
			this.checkDonotPromptAgain = new CheckBox();
			this.txtWarningText = new TextBox();
			base.SuspendLayout();
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.cancelButton, "cancelButton");
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.okButton, "okButton");
			this.okButton.Name = "okButton";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new EventHandler(this.okButton_Click);
			componentResourceManager.ApplyResources(this.checkDonotPromptAgain, "checkBox1");
			this.checkDonotPromptAgain.Name = "checkBox1";
			this.checkDonotPromptAgain.UseVisualStyleBackColor = true;
			this.txtWarningText.BorderStyle = BorderStyle.None;
			componentResourceManager.ApplyResources(this.txtWarningText, "txtWarningText");
			this.txtWarningText.Name = "txtWarningText";
			this.txtWarningText.ReadOnly = true;
			base.AcceptButton = this.okButton;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.cancelButton;
			base.Controls.Add(this.txtWarningText);
			base.Controls.Add(this.checkDonotPromptAgain);
			base.Controls.Add(this.okButton);
			base.Controls.Add(this.cancelButton);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "WarningPromptDialog";
			base.ShowInTaskbar = false;
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			base.DialogResult = System.Windows.Forms.DialogResult.OK;
		}

		internal static bool Prompt(Form owner, string text, string warningText, out bool doNotPromptAgain)
		{
			WarningPromptDialog warningPromptDialog = new WarningPromptDialog(warningText)
			{
				Text = text
			};
			warningPromptDialog.ShowDialog(owner);
			doNotPromptAgain = warningPromptDialog.checkDonotPromptAgain.Checked;
			return warningPromptDialog.DialogResult == System.Windows.Forms.DialogResult.OK;
		}
	}
}