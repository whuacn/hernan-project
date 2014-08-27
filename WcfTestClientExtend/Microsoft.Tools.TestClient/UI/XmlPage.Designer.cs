using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Microsoft.Tools.TestClient.UI
{
	internal class XmlPage : TabPage
	{
		private IContainer components;

		private SplitContainer xmlSplitContainer;

		private TextBox requestXmlTextBox;

		private TextBox responseXmlTextBox;

		private Panel requestPanel;

		private Label requestLabel;

		private Panel responsePanel;

		private Label responseLabel;

		internal string RequestXml
		{
			set
			{
				this.requestXmlTextBox.Text = value;
			}
		}

		internal string ResponseXml
		{
			set
			{
				this.responseXmlTextBox.Text = value;
			}
		}

		internal XmlPage()
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

		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(XmlPage));
			this.xmlSplitContainer = new SplitContainer();
			this.requestXmlTextBox = new TextBox();
			this.requestPanel = new Panel();
			this.requestLabel = new Label();
			this.responseXmlTextBox = new TextBox();
			this.responsePanel = new Panel();
			this.responseLabel = new Label();
			this.xmlSplitContainer.Panel1.SuspendLayout();
			this.xmlSplitContainer.Panel2.SuspendLayout();
			this.xmlSplitContainer.SuspendLayout();
			this.requestPanel.SuspendLayout();
			this.responsePanel.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.xmlSplitContainer, "xmlSplitContainer");
			this.xmlSplitContainer.Name = "xmlSplitContainer";
			this.xmlSplitContainer.Panel1.Controls.Add(this.requestXmlTextBox);
			this.xmlSplitContainer.Panel1.Controls.Add(this.requestPanel);
			this.xmlSplitContainer.Panel2.Controls.Add(this.responseXmlTextBox);
			this.xmlSplitContainer.Panel2.Controls.Add(this.responsePanel);
			componentResourceManager.ApplyResources(this.requestXmlTextBox, "requestXmlTextBox");
			this.requestXmlTextBox.Name = "requestXmlTextBox";
			this.requestXmlTextBox.ReadOnly = true;
			this.requestXmlTextBox.KeyDown += new KeyEventHandler(this.requestXmlTextBox_KeyDown);
			this.requestPanel.Controls.Add(this.requestLabel);
			componentResourceManager.ApplyResources(this.requestPanel, "requestPanel");
			this.requestPanel.Name = "requestPanel";
			componentResourceManager.ApplyResources(this.requestLabel, "requestLabel");
			this.requestLabel.Name = "requestLabel";
			componentResourceManager.ApplyResources(this.responseXmlTextBox, "responseXmlTextBox");
			this.responseXmlTextBox.Name = "responseXmlTextBox";
			this.responseXmlTextBox.ReadOnly = true;
			this.responseXmlTextBox.KeyDown += new KeyEventHandler(this.responseXmlTextBox_KeyDown);
			this.responsePanel.Controls.Add(this.responseLabel);
			componentResourceManager.ApplyResources(this.responsePanel, "responsePanel");
			this.responsePanel.Name = "responsePanel";
			componentResourceManager.ApplyResources(this.responseLabel, "responseLabel");
			this.responseLabel.Name = "responseLabel";
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.xmlSplitContainer);
			base.Name = "XmlPage";
			this.xmlSplitContainer.Panel1.ResumeLayout(false);
			this.xmlSplitContainer.Panel1.PerformLayout();
			this.xmlSplitContainer.Panel2.ResumeLayout(false);
			this.xmlSplitContainer.Panel2.PerformLayout();
			this.xmlSplitContainer.ResumeLayout(false);
			this.requestPanel.ResumeLayout(false);
			this.requestPanel.PerformLayout();
			this.responsePanel.ResumeLayout(false);
			this.responsePanel.PerformLayout();
			base.ResumeLayout(false);
		}

		private void requestXmlTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && !e.Alt && !e.Shift && e.KeyCode == Keys.A)
			{
				this.requestXmlTextBox.SelectAll();
			}
		}

		private void responseXmlTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && !e.Alt && !e.Shift && e.KeyCode == Keys.A)
			{
				this.responseXmlTextBox.SelectAll();
			}
		}
	}
}