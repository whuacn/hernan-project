using Microsoft.Tools.TestClient;
using Microsoft.Tools.TestClient.Variables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using WcfTestClient;

namespace Microsoft.Tools.TestClient.UI
{
	internal class ServicePage : TabPage
	{
		private static IDictionary<string, int> tabNumbers;

		private FormattedPage formattedPage;

		private MainForm mainForm;

		private ServiceMethodInfo method;

		private Microsoft.Tools.TestClient.TestCase testCase;

		private XmlPage xmlPage;

		private IContainer components;

		private BackgroundWorker backgroundWorker;

		private TabControl formatTabControl;

		internal Microsoft.Tools.TestClient.TestCase TestCase
		{
			get
			{
				if (this.testCase == null)
				{
					this.testCase = this.method.CreateTestCase();
					this.testCase.OnErrorReported += new Microsoft.Tools.TestClient.TestCase.ErrorHandler(this.testCase_OnErrorReported);
					this.testCase.ServicePage = this;
				}
				return this.testCase;
			}
		}

		static ServicePage()
		{
			ServicePage.tabNumbers = new Dictionary<string, int>();
		}

		internal ServicePage()
		{
			this.InitializeComponent();
		}

		internal ServicePage(MainForm mainForm, ServiceMethodInfo method) : this()
		{
			this.mainForm = mainForm;
			this.method = method;
			this.testCase = this.TestCase;
			TabControl.TabPageCollection tabPages = this.formatTabControl.TabPages;
			FormattedPage formattedPage = new FormattedPage(this);
			FormattedPage formattedPage1 = formattedPage;
			this.formattedPage = formattedPage;
			tabPages.Add(formattedPage1);
			TabControl.TabPageCollection tabPageCollections = this.formatTabControl.TabPages;
			XmlPage xmlPage = new XmlPage();
			XmlPage xmlPage1 = xmlPage;
			this.xmlPage = xmlPage;
			tabPageCollections.Add(xmlPage1);
			string str = string.Concat(method.Endpoint.OperationContractTypeName, ".", method.MethodName);
			if (!ServicePage.tabNumbers.ContainsKey(str))
			{
				this.Text = this.Wrap(method.MethodName);
				base.ToolTipText = str;
				ServicePage.tabNumbers.Add(str, 0);
			}
			else
			{
				IDictionary<string, int> strs = ServicePage.tabNumbers;
				IDictionary<string, int> strs1 = strs;
				string str1 = str;
				int item = strs1[str1] + 1;
				int num = item;
				strs[str1] = item;
				string str2 = string.Concat(" (", num, ")");
				this.Text = this.Wrap(string.Concat(method.MethodName, str2));
				base.ToolTipText = string.Concat(str, str2);
			}
			ServicePage servicePage = this;
			servicePage.ToolTipText = string.Concat(servicePage.ToolTipText, " [", method.Endpoint.ServiceProject.Address, "]");
		}

		private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			e.Result = ServiceExecutor.TranslateToXmlInClientDomain(this.testCase, this.formattedPage.GetVariables());
		}

		private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.xmlPage.RequestXml = e.Result as string;
			this.formatTabControl.Enabled = true;
			this.formatTabControl.Focus();
		}

		internal void ChangeInvokeStatus(bool invokeEnabled)
		{
			this.formattedPage.ChangeInvokeStatus(invokeEnabled);
		}

		internal void Close()
		{
			this.formattedPage.Close();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void formatTabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.formatTabControl.SelectedTab is XmlPage)
			{
				this.xmlPage.RequestXml = StringResources.Loading;
				this.formatTabControl.Enabled = false;
				this.backgroundWorker.RunWorkerAsync();
			}
		}

		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ServicePage));
			this.backgroundWorker = new BackgroundWorker();
			this.formatTabControl = new TabControl();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.backgroundWorker, "backgroundWorker");
			this.backgroundWorker.DoWork += new DoWorkEventHandler(this.backgroundWorker_DoWork);
			this.backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
			componentResourceManager.ApplyResources(this.formatTabControl, "formatTabControl");
			this.formatTabControl.Multiline = true;
			this.formatTabControl.Name = "formatTabControl";
			this.formatTabControl.SelectedIndex = 0;
			this.formatTabControl.SelectedIndexChanged += new EventHandler(this.formatTabControl_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.formatTabControl);
			base.Name = "ServicePage";
			base.ResumeLayout(false);
		}

		internal void InvokeTestCase(Variable[] variables, bool newProxy)
		{
			this.mainForm.InvokeTestCase(this, variables, newProxy);
		}

		internal void OnValueUpdated()
		{
			this.xmlPage.ResponseXml = "";
		}

		internal void PopulateOutput(Variable[] variables, string responseXml)
		{
			if (variables != null)
			{
				this.formattedPage.PopulateOutput(variables);
			}
			if (responseXml != null)
			{
				this.xmlPage.ResponseXml = responseXml;
			}
			this.testCase.Remove();
			this.testCase = null;
		}

		internal void ResetInput()
		{
			this.formattedPage.ResetInputTree();
		}

		private void testCase_OnErrorReported(ErrorItem errorItem)
		{
			this.mainForm.OnErrorReported(new ErrorItem[] { errorItem });
		}

		private string Wrap(string p)
		{
			if (p.Length <= 33)
			{
				return p;
			}
			return string.Concat(p.Substring(0, 15), "...", p.Substring(p.Length - 15));
		}
	}
}