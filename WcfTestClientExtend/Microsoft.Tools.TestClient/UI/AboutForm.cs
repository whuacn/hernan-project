using Microsoft.Tools.TestClient;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;
using WcfTestClient;

namespace Microsoft.Tools.TestClient.UI
{
	internal class AboutForm : Form
	{
		private IContainer components;

		private Button buttonSystemInformation;

		private Label labelCompanyName;

		private TextBox textBoxDescription;

		private Button buttonOk;

		private Label labelProductName;

		private Label labelVersion;

		private Label labelCopyright;

		private PictureBox wcfPicture;

		internal AboutForm()
		{
			this.InitializeComponent();
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			string about = StringResources.About;
			object[] productName = new object[] { StringResources.ProductName };
			this.Text = string.Format(invariantCulture, about, productName);
			this.labelProductName.Text = StringResources.ProductName;
			Label label = this.labelVersion;
			CultureInfo currentUICulture = CultureInfo.CurrentUICulture;
			string assemblyVersion = StringResources.AssemblyVersion;
			object[] executingAssemblyFileVersion = new object[] { AboutForm.GetExecutingAssemblyFileVersion() };
			label.Text = string.Format(currentUICulture, assemblyVersion, executingAssemblyFileVersion);
			this.labelCopyright.Text = StringResources.Copyright;
			this.labelCompanyName.Text = StringResources.Company;
			this.textBoxDescription.Text = StringResources.ProductDescription;
			this.wcfPicture.Image = ResourceHelper.AboutBoxImage;
		}

		private void buttonSystemInformation_Click(object sender, EventArgs e)
		{
			try
			{
				Process.Start("Msinfo32.exe");
			}
			catch (Win32Exception win32Exception)
			{
				RtlAwareMessageBox.Show(this, StringResources.FailToRunSystemInfo, StringResources.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, 0);
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

		private static string GetExecutingAssemblyFileVersion()
		{
			return FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
		}
        ComponentResourceManager componentResourceManager;
		private void InitializeComponent()
		{
			componentResourceManager = new ComponentResourceManager(typeof(AboutForm));
			this.buttonSystemInformation = new Button();
			this.labelCompanyName = new Label();
			this.textBoxDescription = new TextBox();
			this.buttonOk = new Button();
			this.labelProductName = new Label();
			this.labelVersion = new Label();
			this.labelCopyright = new Label();
			this.wcfPicture = new PictureBox();
			((ISupportInitialize)this.wcfPicture).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.buttonSystemInformation, "buttonSystemInformation");
			this.buttonSystemInformation.Name = "buttonSystemInformation";
			this.buttonSystemInformation.UseVisualStyleBackColor = true;
			this.buttonSystemInformation.Click += new EventHandler(this.buttonSystemInformation_Click);
			componentResourceManager.ApplyResources(this.labelCompanyName, "labelCompanyName");
			this.labelCompanyName.MaximumSize = new System.Drawing.Size(0, 17);
			this.labelCompanyName.Name = "labelCompanyName";
			componentResourceManager.ApplyResources(this.textBoxDescription, "textBoxDescription");
			this.textBoxDescription.Name = "textBoxDescription";
			this.textBoxDescription.ReadOnly = true;
			this.textBoxDescription.TabStop = false;
			componentResourceManager.ApplyResources(this.buttonOk, "buttonOk");
			this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.labelProductName, "labelProductName");
			this.labelProductName.MaximumSize = new System.Drawing.Size(0, 17);
			this.labelProductName.Name = "labelProductName";
			componentResourceManager.ApplyResources(this.labelVersion, "labelVersion");
			this.labelVersion.MaximumSize = new System.Drawing.Size(0, 17);
			this.labelVersion.Name = "labelVersion";
			componentResourceManager.ApplyResources(this.labelCopyright, "labelCopyright");
			this.labelCopyright.MaximumSize = new System.Drawing.Size(0, 17);
			this.labelCopyright.Name = "labelCopyright";
			componentResourceManager.ApplyResources(this.wcfPicture, "wcfPicture");
			this.wcfPicture.Name = "wcfPicture";
			this.wcfPicture.TabStop = false;
			base.AcceptButton = this.buttonOk;
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			base.CancelButton = this.buttonOk;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.labelProductName);
			base.Controls.Add(this.buttonOk);
			base.Controls.Add(this.labelVersion);
			base.Controls.Add(this.buttonSystemInformation);
			base.Controls.Add(this.labelCopyright);
			base.Controls.Add(this.wcfPicture);
			base.Controls.Add(this.labelCompanyName);
			base.Controls.Add(this.textBoxDescription);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "AboutForm";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			((ISupportInitialize)this.wcfPicture).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}