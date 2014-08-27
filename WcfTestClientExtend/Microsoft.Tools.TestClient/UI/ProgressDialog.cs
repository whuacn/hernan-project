using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Microsoft.Tools.TestClient.UI
{
	internal class ProgressDialog : Form
	{
		private BackgroundWorker backgroundWorker;

		private IContainer components;

		private ProgressBar progressBar;

		private Button cancelButton;

		private Label actionLabel;

		public ProgressDialog(string title, string labelText, BackgroundWorker backgroundWorker)
		{
			this.InitializeComponent();
			this.Text = title;
			this.actionLabel.Text = labelText;
			this.backgroundWorker = backgroundWorker;
			this.backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
		}

		private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			this.progressBar.Value = e.ProgressPercentage;
			if (this.progressBar.Value == this.progressBar.Maximum)
			{
				base.DialogResult = System.Windows.Forms.DialogResult.OK;
				base.Close();
			}
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
			componentResourceManager = new ComponentResourceManager(typeof(ProgressDialog));
			this.progressBar = new ProgressBar();
			this.cancelButton = new Button();
			this.actionLabel = new Label();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.progressBar, "progressBar");
			this.progressBar.Name = "progressBar";
			componentResourceManager.ApplyResources(this.cancelButton, "cancelButton");
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.actionLabel, "actionLabel");
			this.actionLabel.Name = "actionLabel";
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.cancelButton;
			base.Controls.Add(this.actionLabel);
			base.Controls.Add(this.cancelButton);
			base.Controls.Add(this.progressBar);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ProgressDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		internal static bool Prompt(Form owner, string title, string labelText, BackgroundWorker backgroundWorker)
		{
			ProgressDialog progressDialog = new ProgressDialog(title, labelText, backgroundWorker);
			progressDialog.ShowDialog(owner);
			return progressDialog.DialogResult == System.Windows.Forms.DialogResult.OK;
		}
	}
}