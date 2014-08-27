using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Microsoft.Tools.TestClient.UI
{
	internal class StartPage : TabPage
	{
		private IContainer components;

		private PictureBox hintImagePictureBox;

		private TextBox hintTextBox;

		public StartPage()
		{
			this.InitializeComponent();
			this.hintTextBox.BackColor = this.BackColor;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void hintTextBox_GotFocus(object sender, EventArgs e)
		{
			this.hintTextBox.Select(0, 0);
		}

		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StartPage));
			this.hintImagePictureBox = new PictureBox();
			this.hintTextBox = new TextBox();
			((ISupportInitialize)this.hintImagePictureBox).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.hintImagePictureBox, "hintImagePictureBox");
			this.hintImagePictureBox.Name = "hintImagePictureBox";
			this.hintImagePictureBox.TabStop = false;
			this.hintTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			componentResourceManager.ApplyResources(this.hintTextBox, "hintTextBox");
			this.hintTextBox.Name = "hintTextBox";
			this.hintTextBox.ReadOnly = true;
			this.hintTextBox.GotFocus += new EventHandler(this.hintTextBox_GotFocus);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.hintTextBox);
			base.Controls.Add(this.hintImagePictureBox);
			base.Name = "StartPage";
			((ISupportInitialize)this.hintImagePictureBox).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}