using Microsoft.Tools.TestClient;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WcfTestClient;

namespace Microsoft.Tools.TestClient.UI
{
	internal class ErrorDialog : Form
	{
		private int currentIndex;

		private ErrorItem[] errorItems;

		private IContainer components;

		private PictureBox errorIconPictureBox;

		private TextBox errorMessageTextBox;

		private Button prevButton;

		private Button nextButton;

		private Label errorDetailLabel;

		private Label errorBrowseLocationLabel;

		private Button closeButton;

		private TabControl errorMsgTabControl;

		private TabPage textViewTp;

		private CheckBox errorMsgWrapCb;

		private TextBox errorDetailTextBox;

		private TabPage htmlViewTp;

		private WebBrowser errorMsgWebBrowser;

		internal ErrorDialog()
		{
			this.InitializeComponent();
			this.Text = StringResources.ProductName;
			this.errorMsgWrapCb.Text = StringResources.ErrorMsgCbText;
			this.textViewTp.Text = StringResources.TextViewTpText;
			this.htmlViewTp.Text = StringResources.HtmlViewTpText;
			this.errorIconPictureBox.Image = SystemIcons.Error.ToBitmap();
			this.prevButton.Image = ResourceHelper.ArrowUpImage;
			this.nextButton.Image = ResourceHelper.ArrowDownImage;
		}

		internal ErrorDialog(ErrorItem[] errorItems) : this()
		{
			this.errorItems = errorItems;
			this.RefreshView();
		}

		private void closeButton_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		private void closeButton_KeyDown(object sender, KeyEventArgs e)
		{
			this.ProcessKeys(e);
		}

		internal static void DisplayError(Form owner, ErrorItem[] errorItems)
		{
			ErrorDialog errorDialog = new ErrorDialog(errorItems)
			{
				StartPosition = FormStartPosition.CenterParent,
				ShowInTaskbar = false
			};
			errorDialog.ShowDialog(owner);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void errorDetailTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && !e.Alt && !e.Shift && e.KeyCode == Keys.A)
			{
				this.errorDetailTextBox.SelectAll();
			}
			this.ProcessKeys(e);
		}

		private void errorMessageTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && !e.Alt && !e.Shift && e.KeyCode == Keys.A)
			{
				this.errorMessageTextBox.SelectAll();
			}
			this.ProcessKeys(e);
		}

		private void errorMsgWrapCb_CheckedChanged(object sender, EventArgs e)
		{
			this.errorDetailTextBox.WordWrap = this.errorMsgWrapCb.Checked;
		}
        ComponentResourceManager componentResourceManager;
		private void InitializeComponent()
		{
			componentResourceManager = new ComponentResourceManager(typeof(ErrorDialog));
			this.errorIconPictureBox = new PictureBox();
			this.errorMessageTextBox = new TextBox();
			this.prevButton = new Button();
			this.nextButton = new Button();
			this.errorDetailLabel = new Label();
			this.errorBrowseLocationLabel = new Label();
			this.closeButton = new Button();
			this.errorMsgTabControl = new TabControl();
			this.textViewTp = new TabPage();
			this.errorMsgWrapCb = new CheckBox();
			this.errorDetailTextBox = new TextBox();
			this.htmlViewTp = new TabPage();
			this.errorMsgWebBrowser = new WebBrowser();
			((ISupportInitialize)this.errorIconPictureBox).BeginInit();
			this.errorMsgTabControl.SuspendLayout();
			this.textViewTp.SuspendLayout();
			this.htmlViewTp.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.errorIconPictureBox, "errorIconPictureBox");
			this.errorIconPictureBox.Name = "errorIconPictureBox";
			this.errorIconPictureBox.TabStop = false;
			componentResourceManager.ApplyResources(this.errorMessageTextBox, "errorMessageTextBox");
			this.errorMessageTextBox.BorderStyle = BorderStyle.None;
			this.errorMessageTextBox.Name = "errorMessageTextBox";
			this.errorMessageTextBox.ReadOnly = true;
			this.errorMessageTextBox.KeyDown += new KeyEventHandler(this.errorMessageTextBox_KeyDown);
			componentResourceManager.ApplyResources(this.prevButton, "prevButton");
			this.prevButton.Name = "prevButton";
			this.prevButton.UseVisualStyleBackColor = true;
			this.prevButton.Click += new EventHandler(this.prevButton_Click);
			this.prevButton.KeyDown += new KeyEventHandler(this.prevButton_KeyDown);
			componentResourceManager.ApplyResources(this.nextButton, "nextButton");
			this.nextButton.Name = "nextButton";
			this.nextButton.UseVisualStyleBackColor = true;
			this.nextButton.Click += new EventHandler(this.nextButton_Click);
			this.nextButton.KeyDown += new KeyEventHandler(this.nextButton_KeyDown);
			componentResourceManager.ApplyResources(this.errorDetailLabel, "errorDetailLabel");
			this.errorDetailLabel.Name = "errorDetailLabel";
			componentResourceManager.ApplyResources(this.errorBrowseLocationLabel, "errorBrowseLocationLabel");
			this.errorBrowseLocationLabel.Name = "errorBrowseLocationLabel";
			componentResourceManager.ApplyResources(this.closeButton, "closeButton");
			this.closeButton.Name = "closeButton";
			this.closeButton.UseVisualStyleBackColor = true;
			this.closeButton.Click += new EventHandler(this.closeButton_Click);
			this.closeButton.KeyDown += new KeyEventHandler(this.closeButton_KeyDown);
			componentResourceManager.ApplyResources(this.errorMsgTabControl, "errorMsgTabControl");
			this.errorMsgTabControl.Controls.Add(this.textViewTp);
			this.errorMsgTabControl.Controls.Add(this.htmlViewTp);
			this.errorMsgTabControl.Name = "errorMsgTabControl";
			this.errorMsgTabControl.SelectedIndex = 0;
			this.errorMsgTabControl.SizeMode = TabSizeMode.FillToRight;
			this.textViewTp.Controls.Add(this.errorMsgWrapCb);
			this.textViewTp.Controls.Add(this.errorDetailTextBox);
			componentResourceManager.ApplyResources(this.textViewTp, "textViewTp");
			this.textViewTp.Name = "textViewTp";
			this.textViewTp.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.errorMsgWrapCb, "errorMsgWrapCb");
			this.errorMsgWrapCb.Name = "errorMsgWrapCb";
			this.errorMsgWrapCb.UseVisualStyleBackColor = true;
			this.errorMsgWrapCb.CheckedChanged += new EventHandler(this.errorMsgWrapCb_CheckedChanged);
			componentResourceManager.ApplyResources(this.errorDetailTextBox, "errorDetailTextBox");
			this.errorDetailTextBox.Name = "errorDetailTextBox";
			this.errorDetailTextBox.ReadOnly = true;
			this.errorDetailTextBox.KeyDown += new KeyEventHandler(this.errorDetailTextBox_KeyDown);
			this.htmlViewTp.Controls.Add(this.errorMsgWebBrowser);
			componentResourceManager.ApplyResources(this.htmlViewTp, "htmlViewTp");
			this.htmlViewTp.Name = "htmlViewTp";
			this.htmlViewTp.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.errorMsgWebBrowser, "errorMsgWebBrowser");
			this.errorMsgWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
			this.errorMsgWebBrowser.Name = "errorMsgWebBrowser";
			this.errorMsgWebBrowser.ScriptErrorsSuppressed = true;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.errorMsgTabControl);
			base.Controls.Add(this.closeButton);
			base.Controls.Add(this.errorBrowseLocationLabel);
			base.Controls.Add(this.errorDetailLabel);
			base.Controls.Add(this.nextButton);
			base.Controls.Add(this.prevButton);
			base.Controls.Add(this.errorMessageTextBox);
			base.Controls.Add(this.errorIconPictureBox);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ErrorDialog";
			base.ShowIcon = false;
			((ISupportInitialize)this.errorIconPictureBox).EndInit();
			this.errorMsgTabControl.ResumeLayout(false);
			this.textViewTp.ResumeLayout(false);
			this.textViewTp.PerformLayout();
			this.htmlViewTp.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		private void nextButton_Click(object sender, EventArgs e)
		{
			ErrorDialog errorDialog = this;
			errorDialog.currentIndex = errorDialog.currentIndex + 1;
			this.RefreshView();
		}

		private void nextButton_KeyDown(object sender, KeyEventArgs e)
		{
			this.ProcessKeys(e);
		}

		private void prevButton_Click(object sender, EventArgs e)
		{
			ErrorDialog errorDialog = this;
			errorDialog.currentIndex = errorDialog.currentIndex - 1;
			this.RefreshView();
		}

		private void prevButton_KeyDown(object sender, KeyEventArgs e)
		{
			this.ProcessKeys(e);
		}

		private void ProcessKeys(KeyEventArgs e)
		{
			if (!e.Control && !e.Alt && !e.Shift && e.KeyCode == Keys.Escape)
			{
				this.closeButton_Click(null, e);
			}
		}

		private void RefreshView()
		{
			Button button = this.prevButton;
			Button button1 = this.nextButton;
			bool length = (int)this.errorItems.Length > 1;
			bool flag = length;
			this.errorBrowseLocationLabel.Visible = length;
			bool flag1 = flag;
			bool flag2 = flag1;
			button1.Visible = flag1;
			button.Visible = flag2;
			Label label = this.errorBrowseLocationLabel;
			CultureInfo currentUICulture = CultureInfo.CurrentUICulture;
			string errorBrowseLocationFormat = StringResources.ErrorBrowseLocationFormat;
			object[] objArray = new object[] { this.currentIndex + 1, (int)this.errorItems.Length };
			label.Text = string.Format(currentUICulture, errorBrowseLocationFormat, objArray);
			this.errorMessageTextBox.Text = this.errorItems[this.currentIndex].ErrorMessage;
			this.errorDetailTextBox.Text = this.TrimScript(this.errorItems[this.currentIndex].ErrorDetail);
			this.errorMsgWebBrowser.DocumentText = this.errorItems[this.currentIndex].ErrorDetail;
			this.prevButton.Enabled = this.currentIndex != 0;
			this.nextButton.Enabled = this.currentIndex != (int)this.errorItems.Length - 1;
			this.errorMessageTextBox.Select(0, 0);
			this.errorDetailTextBox.Select(0, 0);
		}

		private string TrimScript(string htmlDocText)
		{
			return (new Regex("<script type=\"text/javascript\">(.*?)</script>")).Replace(htmlDocText, "");
		}
	}
}