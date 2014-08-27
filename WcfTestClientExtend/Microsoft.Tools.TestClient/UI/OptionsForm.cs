using Microsoft.Tools.TestClient;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Microsoft.Tools.TestClient.UI
{
	internal class OptionsForm : Form
	{
		private IContainer components;

		private TabControl optionsTabControl;

		private TabPage configPolicyTabPage;

		private CheckBox regenerateConfigCheckBox;

		private Button okButton;

		private Button cancelButton;

		public OptionsForm()
		{
			this.InitializeComponent();
			this.regenerateConfigCheckBox.Checked = ApplicationSettings.GetInstance().RegenerateConfigEnabled;
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
			componentResourceManager = new ComponentResourceManager(typeof(OptionsForm));
			this.optionsTabControl = new TabControl();
			this.configPolicyTabPage = new TabPage();
			this.regenerateConfigCheckBox = new CheckBox();
			this.okButton = new Button();
			this.cancelButton = new Button();
			this.optionsTabControl.SuspendLayout();
			this.configPolicyTabPage.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.optionsTabControl, "OptionsTabControl");
			this.optionsTabControl.Controls.Add(this.configPolicyTabPage);
			this.optionsTabControl.Name = "OptionsTabControl";
			this.optionsTabControl.SelectedIndex = 0;
			this.configPolicyTabPage.Controls.Add(this.regenerateConfigCheckBox);
			componentResourceManager.ApplyResources(this.configPolicyTabPage, "ConfigPolicyTabPage");
			this.configPolicyTabPage.Name = "ConfigPolicyTabPage";
			this.configPolicyTabPage.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.regenerateConfigCheckBox, "regenerateConfigCheckBox");
			this.regenerateConfigCheckBox.Name = "regenerateConfigCheckBox";
			this.regenerateConfigCheckBox.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.okButton, "okButton");
			this.okButton.Name = "okButton";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new EventHandler(this.okButton_Click);
			componentResourceManager.ApplyResources(this.cancelButton, "cancelButton");
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.UseVisualStyleBackColor = true;
			base.AcceptButton = this.okButton;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.cancelButton;
			base.Controls.Add(this.cancelButton);
			base.Controls.Add(this.okButton);
			base.Controls.Add(this.optionsTabControl);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "OptionsPromptDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			this.optionsTabControl.ResumeLayout(false);
			this.configPolicyTabPage.ResumeLayout(false);
			this.configPolicyTabPage.PerformLayout();
			base.ResumeLayout(false);
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			base.DialogResult = System.Windows.Forms.DialogResult.OK;
		}

		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);
			this.regenerateConfigCheckBox.Focus();
		}

		internal static bool Prompt(Form owner, out bool regenerateConfig)
		{
			OptionsForm optionsForm = new OptionsForm();
			optionsForm.ShowDialog(owner);
			regenerateConfig = optionsForm.regenerateConfigCheckBox.Checked;
			return optionsForm.DialogResult == System.Windows.Forms.DialogResult.OK;
		}
	}
}