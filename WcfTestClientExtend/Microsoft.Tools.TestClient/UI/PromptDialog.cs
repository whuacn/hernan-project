using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Microsoft.Tools.TestClient.UI
{
	internal class PromptDialog : Form
	{
		private IContainer components;

		private Label promptLabel;

		private Button cancelButton;

		private Button okButton;

		private ComboBox inputBox;

		internal PromptDialog()
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
			componentResourceManager = new ComponentResourceManager(typeof(PromptDialog));
			this.promptLabel = new Label();
			this.cancelButton = new Button();
			this.okButton = new Button();
			this.inputBox = new ComboBox();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.promptLabel, "promptLabel");
			this.promptLabel.Name = "promptLabel";
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.cancelButton, "cancelButton");
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.okButton, "okButton");
			this.okButton.Name = "okButton";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new EventHandler(this.okButton_Click);
			this.inputBox.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.inputBox, "inputBox");
			this.inputBox.Name = "inputBox";
			base.AcceptButton = this.okButton;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.cancelButton;
			base.Controls.Add(this.inputBox);
			base.Controls.Add(this.okButton);
			base.Controls.Add(this.cancelButton);
			base.Controls.Add(this.promptLabel);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "PromptDialog";
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			base.DialogResult = System.Windows.Forms.DialogResult.OK;
		}

		internal static string Prompt(Form owner, string title, string prompt, params string[] defaultValues)
		{
			PromptDialog promptDialog = new PromptDialog()
			{
				Text = title
			};
			promptDialog.promptLabel.Text = prompt;
			promptDialog.StartPosition = FormStartPosition.CenterParent;
			promptDialog.ShowInTaskbar = false;
			string[] strArrays = defaultValues;
			for (int i = 0; i < (int)strArrays.Length; i++)
			{
				string str = strArrays[i];
				promptDialog.inputBox.Items.Add(str);
			}
			if ((int)defaultValues.Length > 0)
			{
				promptDialog.inputBox.Text = defaultValues[0];
			}
			promptDialog.ShowDialog(owner);
			if (promptDialog.DialogResult != System.Windows.Forms.DialogResult.OK)
			{
				return null;
			}
			return promptDialog.inputBox.Text;
		}
	}
}