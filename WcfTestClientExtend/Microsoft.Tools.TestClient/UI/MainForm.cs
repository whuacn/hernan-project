using Microsoft.Internal.Performance;
using Microsoft.Tools.Common;
using Microsoft.Tools.TestClient;
using Microsoft.Tools.TestClient.Variables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WcfTestClient;

namespace Microsoft.Tools.TestClient.UI
{
	internal class MainForm : Form
	{
		private const string chmName = "WcfTestClient.chm";

		private readonly static string CodeMarkersRegRoot;

		private static CodeMarkers codeMarkers;

		private readonly Color disabledColor = SystemColors.GrayText;

		private AddServiceExecutor addServiceExecutor = new AddServiceExecutor();

		private string[] endpoints;

		private bool removingTabPage;

		private StartPage startPage;

		private TabPageCloseOrderManager tabPageCloseOrderManager = new TabPageCloseOrderManager();

		private Workspace workspace = new Workspace();

		private volatile bool isCancellingInvokeServiceWorker;

		private IContainer components;

		private TreeView serviceTreeView;

		private MenuStrip menuStrip;

		private ToolStripMenuItem fileToolStripMenuItem;

		private ToolStripMenuItem exitToolStripMenuItem;

		private ToolStripMenuItem helpToolStripMenuItem;

		private ToolStripMenuItem aboutToolStripMenuItem;

		private BackgroundWorker addServiceWorker;

		private BackgroundWorker invokeServiceWorker;

		private TabControl serviceTabControl;

		private StatusStrip globalStatusStrip;

		private ToolStripStatusLabel GlobalStatusLabel;

		private SplitContainer splitContainer;

		private System.Windows.Forms.ContextMenuStrip rootContextMenu;

		private ToolStripMenuItem addServiceContextMenuItem;

		private System.Windows.Forms.ContextMenuStrip serviceProjectContextMenu;

		private ToolStripMenuItem removeServiceToolStripMenuItem;

		private System.Windows.Forms.ContextMenuStrip tabPageContextMenu;

		private ToolStripMenuItem closeToolStripMenuItem;

		private ToolStripMenuItem closeAllToolStripMenuItem;

		private ToolStripMenuItem helpToolStripMenuItem1;

		private System.Windows.Forms.ContextMenuStrip fileContextMenu;

		private ToolStripMenuItem copyFullPathToolStripMenuItem;

		private System.Windows.Forms.ContextMenuStrip endpointContextMenu;

		private ToolStripMenuItem copyEndpointToolStripMenuItem;

		private System.Windows.Forms.ContextMenuStrip operationContextMenu;

		private ToolStripMenuItem copyOperationToolStripMenuItem;

		private ToolStripMenuItem copyServiceProjectToolStripMenuItem;

		private ToolStripMenuItem refreshServiceToolStripMenuItem;

		private FileSystemWatcher fileSystemWatcher;

		private ToolStripMenuItem toolsToolStripMenuItem;

		private ToolStripMenuItem optionsToolStripMenuItem;

		private ToolStripMenuItem editWithSvcConfigEditorToolStripMenuItem;

		private ToolStripSeparator toolStripSeparator2;

		private ToolStripMenuItem restoreToDefaultConfigToolStripMenuItem;

		private ToolStripMenuItem addServiceMainMenuItem;

		private ToolStripSeparator toolStripSeparator3;

		private ToolStripMenuItem recentServicesMainMenuItem;

		private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem saveProjectToolStripMenuItem;
        private ToolStripMenuItem loadProjectToolStripMenuItem;

		private ToolStripSeparator toolStripSeparator1;

		static MainForm()
		{
			MainForm.CodeMarkersRegRoot = string.Concat("Software\\Microsoft\\VisualStudio\\", VersionNumbers.VSCurrentVersionString);
			MainForm.codeMarkers = CodeMarkers.Instance;
		}

		internal MainForm()
		{
			MainForm.codeMarkers.InitPerformanceDll(73, MainForm.CodeMarkersRegRoot);
			MainForm.codeMarkers.CodeMarker(16720);
			this.InitializeComponent();
			base.Icon = ResourceHelper.ApplicationIcon;
			this.serviceTreeView.ImageList = new ImageList();
			this.serviceTreeView.ImageList.Images.Add(ResourceHelper.ApplicationIcon);
			this.serviceTreeView.ImageList.Images.Add(ResourceHelper.ContractImage);
			this.serviceTreeView.ImageList.Images.Add(ResourceHelper.EndpointImage);
			this.serviceTreeView.ImageList.Images.Add(ResourceHelper.FileImage);
			this.serviceTreeView.ImageList.Images.Add(ResourceHelper.OperationImage);
			this.serviceTreeView.ImageList.Images.Add(ResourceHelper.ErrorImage);
			TabControl.TabPageCollection tabPages = this.serviceTabControl.TabPages;
			StartPage startPage = new StartPage();
			StartPage startPage1 = startPage;
			this.startPage = startPage;
			tabPages.Add(startPage1);
			this.ConstructRecentServiceMenuItems();
			this.SetFileWatchPath();
		}

		internal MainForm(string[] endpoints) : this()
		{
			if (endpoints == null || (int)endpoints.Length <= 0)
			{
				this.GlobalStatusLabel.Text = StringResources.StatusReady;
			}
			else
			{
				this.endpoints = endpoints;
			}
			this.serviceTreeView.Nodes.Add(StringResources.RootNodeName);
			this.serviceTreeView.Nodes[0].ContextMenuStrip = this.rootContextMenu;
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			(new AboutForm()).ShowDialog();
		}

		private void AddNodesForServiceProject(ServiceProject serviceProject, TreeNode serviceProjectNode)
		{
			this.serviceTreeView.BeginUpdate();
			serviceProjectNode.Tag = serviceProject;
			serviceProject.ServiceProjectNode = serviceProjectNode;
			foreach (ClientEndpointInfo endpoint in serviceProject.Endpoints)
			{
				TreeNode treeNode = new TreeNode(endpoint.ToString())
				{
					ContextMenuStrip = this.endpointContextMenu,
					Tag = endpoint
				};
				serviceProjectNode.Nodes.Add(treeNode);
				if (!endpoint.Valid)
				{
					int num = 5;
					int num1 = num;
					treeNode.ImageIndex = num;
					treeNode.SelectedImageIndex = num1;
					treeNode.ForeColor = this.disabledColor;
					if (string.IsNullOrEmpty(endpoint.InvalidReason))
					{
						treeNode.ToolTipText = StringResources.ErrorUnsupportedContract;
					}
					else
					{
						treeNode.ToolTipText = endpoint.InvalidReason;
					}
				}
				else
				{
					int num2 = 1;
					int num3 = num2;
					treeNode.ImageIndex = num2;
					treeNode.SelectedImageIndex = num3;
				}
				foreach (ServiceMethodInfo method in endpoint.Methods)
				{
					TreeNode invalidReason = new TreeNode(string.Concat(method.MethodName, "()"))
					{
						ContextMenuStrip = this.operationContextMenu,
						Tag = method
					};
					treeNode.Nodes.Add(invalidReason);
					if (!endpoint.Valid || !method.Valid)
					{
						int num4 = 5;
						int num5 = num4;
						invalidReason.ImageIndex = num4;
						invalidReason.SelectedImageIndex = num5;
						invalidReason.ForeColor = this.disabledColor;
						if (!string.IsNullOrEmpty(endpoint.InvalidReason))
						{
							invalidReason.ToolTipText = endpoint.InvalidReason;
						}
						else if (method.Valid)
						{
							invalidReason.ToolTipText = StringResources.ErrorUnsupportedOperation;
						}
						else
						{
							TypeMemberInfo item = method.InvalidMembers[0];
							CultureInfo currentUICulture = CultureInfo.CurrentUICulture;
							string errorUnsupportedOperationBecauseOfUnsupportedType = StringResources.ErrorUnsupportedOperationBecauseOfUnsupportedType;
							object[] typeName = new object[] { item.TypeName };
							invalidReason.ToolTipText = string.Format(currentUICulture, errorUnsupportedOperationBecauseOfUnsupportedType, typeName);
						}
					}
					else
					{
						int num6 = 4;
						int num7 = num6;
						invalidReason.ImageIndex = num6;
						invalidReason.SelectedImageIndex = num7;
					}
				}
			}
			TreeNode configFile = new TreeNode(StringResources.ConfigFileNodeName);
			int num8 = 3;
			int num9 = num8;
			configFile.ImageIndex = num8;
			configFile.SelectedImageIndex = num9;
			configFile.Tag = serviceProject.ConfigFile;
			serviceProjectNode.Nodes.Add(configFile);
			configFile.ContextMenuStrip = this.fileContextMenu;
			serviceProjectNode.ExpandAll();
			this.serviceTreeView.EndUpdate();
		}

		private void AddService(string address)
		{
			if (!string.IsNullOrEmpty(address))
			{
				while (address.EndsWith("/", StringComparison.Ordinal))
				{
					address = address.Substring(0, address.Length - 1);
				}
				AddServiceOutputs.IsRefreshing = false;
				string[] strArrays = new string[] { address };
				this.StartAddServiceWorker(new AddServiceInputs(strArrays), StringResources.AddingAction, StringResources.StatusAddingService);
			}
		}

		private void addServiceMenuItem_Click(object sender, EventArgs e)
		{
			ICollection<string> recentUrls = ApplicationSettings.GetInstance().RecentUrls;
			string[] strArrays = new string[recentUrls.Count];
			recentUrls.CopyTo(strArrays, 0);
			string str = PromptDialog.Prompt(this, StringResources.AddServiceTitle, StringResources.AddServicePrompt, strArrays);
			this.AddService(str);
		}

		private void addServiceWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = (BackgroundWorker)sender;
			AddServiceOutputs addServiceOutput = this.addServiceExecutor.Execute((AddServiceInputs)e.Argument, this.workspace, backgroundWorker);
			if (backgroundWorker.CancellationPending)
			{
				addServiceOutput.Cancelled = true;
			}
			else
			{
				backgroundWorker.ReportProgress(100);
			}
			e.Result = addServiceOutput;
		}

		private void addServiceWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			AddServiceOutputs result = (AddServiceOutputs)e.Result;
			this.GlobalStatusLabel.Text = result.GetStatusMessage();
			if (AddServiceOutputs.IsRefreshing)
			{
				if (result.Cancelled)
				{
					this.UpdateButtonStatus();
					this.TriggerDelayedConfigRefresh(this.serviceTreeView.SelectedNode.Tag as ServiceProject);
					return;
				}
				ServiceProject serviceProject = this.RemoveServiceProject();
				this.serviceTreeView.SelectedNode.Nodes.Clear();
				if (result.ServiceProjects.Count < 1)
				{
					this.OnServiceProjectRemoved(serviceProject);
				}
			}
			if (result.Errors.Count > 0)
			{
				ErrorItem[] errorItemArray = new ErrorItem[result.Errors.Count];
				result.Errors.CopyTo(errorItemArray);
				this.OnErrorReported(errorItemArray);
			}
			this.ConstructRecentServiceMenuItems();
			foreach (ServiceProject serviceProject1 in result.ServiceProjects)
			{
				if (!AddServiceOutputs.IsRefreshing)
				{
					this.OnServiceProjectAdded(serviceProject1);
				}
				else
				{
					this.OnServiceProjectRefreshed(serviceProject1);
				}
			}
			this.UpdateButtonStatus();
			if (this.ServicesRefreshed != null)
			{
				this.ServicesRefreshed(this, e);
			}
			MainForm.codeMarkers.CodeMarker(16721);
		}

		private void CancelInvokeServiceWorker()
		{
			if (this.invokeServiceWorker.IsBusy)
			{
				this.isCancellingInvokeServiceWorker = true;
				this.invokeServiceWorker.WorkerSupportsCancellation = true;
				this.invokeServiceWorker.CancelAsync();
			}
		}

		private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			while (this.serviceTabControl.TabCount > 0)
			{
				this.closeToolStripMenuItem_Click(sender, e);
			}
		}

		private void CloseProjectServicePages(List<TestCase> referencedTestCases)
		{
			foreach (TestCase referencedTestCase in referencedTestCases)
			{
				if (this.serviceTabControl.Contains(referencedTestCase.ServicePage))
				{
					this.serviceTabControl.SelectedTab = referencedTestCase.ServicePage;
					this.RemoveSelectedTabPage();
				}
				referencedTestCase.ServicePage.Dispose();
			}
		}

		private void CloseStartPageIfExists()
		{
			if (this.startPage != null)
			{
				this.serviceTabControl.TabPages.Remove(this.startPage);
				this.startPage.Dispose();
				this.startPage = null;
			}
		}

		private void closeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			TestCase tag = this.serviceTabControl.SelectedTab.Tag as TestCase;
			if (tag != null)
			{
				tag.Remove();
			}
			this.RemoveSelectedTabPage();
		}

		private void ConstructRecentServiceMenuItems()
		{
			ICollection<string> recentUrls = ApplicationSettings.GetInstance().RecentUrls;
			this.recentServicesMainMenuItem.DropDownItems.Clear();
			if (recentUrls.Count == 0)
			{
				this.recentServicesMainMenuItem.Enabled = false;
				return;
			}
			this.recentServicesMainMenuItem.Enabled = true;
			int num = 1;
			foreach (string recentUrl in recentUrls)
			{
				CultureInfo currentUICulture = CultureInfo.CurrentUICulture;
				object[] objArray = new object[] { num, recentUrl };
				ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(string.Format(currentUICulture, "{0} {1}", objArray));
				toolStripMenuItem.Click += new EventHandler(this.recentServiceMenuItem_Click);
				this.recentServicesMainMenuItem.DropDownItems.Add(toolStripMenuItem);
				num++;
			}
		}

		private void copyEndpointToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ClientEndpointInfo tag = (ClientEndpointInfo)this.serviceTreeView.SelectedNode.Tag;
			SafeClipboard.SetText(tag.ToString());
		}

		private void copyFullPathToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SafeClipboard.SetText(((FileItem)this.serviceTreeView.SelectedNode.Tag).FileName);
		}

		private void CopyNode(TreeNode selectedNode)
		{
			if (selectedNode == null)
			{
				return;
			}
			object tag = selectedNode.Tag;
			ServiceProject serviceProject = tag as ServiceProject;
			ClientEndpointInfo clientEndpointInfo = tag as ClientEndpointInfo;
			ServiceMethodInfo serviceMethodInfo = tag as ServiceMethodInfo;
			FileItem fileItem = tag as FileItem;
			if (serviceProject != null)
			{
				this.copyServiceProjectToolStripMenuItem_Click(null, null);
			}
			if (clientEndpointInfo != null)
			{
				this.copyEndpointToolStripMenuItem_Click(null, null);
			}
			if (serviceMethodInfo != null)
			{
				this.copyOperationToolStripMenuItem_Click(null, null);
			}
			if (fileItem != null)
			{
				this.copyFullPathToolStripMenuItem_Click(null, null);
			}
		}

		private void copyOperationToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ServiceMethodInfo tag = (ServiceMethodInfo)this.serviceTreeView.SelectedNode.Tag;
			SafeClipboard.SetText(string.Concat(tag.Endpoint.OperationContractTypeName, ".", tag.MethodName));
		}

		private void copyServiceProjectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SafeClipboard.SetText(((ServiceProject)this.serviceTreeView.SelectedNode.Tag).Address);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void editWithSvcConfigEditorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string str;
			if (!((ServiceProject)this.serviceTreeView.SelectedNode.Parent.Tag).StartSvcConfigEditor(out str))
			{
				ErrorItem[] errorItem = new ErrorItem[] { new ErrorItem(StringResources.StartSvcConfigEditorFail, str, null) };
				this.OnErrorReported(errorItem);
			}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		private void fileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
		{
			string fullPath = e.FullPath;
			if (e.ChangeType == WatcherChangeTypes.Changed)
			{
				ServiceProject serviceProject = this.workspace.FindServiceProject(fullPath);
				if (serviceProject != null)
				{
					this.TryRefreshConfig(serviceProject);
				}
			}
		}

		private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			string directoryName = Path.GetDirectoryName(base.GetType().Assembly.Location);
			string twoLetterISOLanguageName = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
			string str = Path.Combine(directoryName, twoLetterISOLanguageName, "WcfTestClient.chm");
			if (File.Exists(str))
			{
				Help.ShowHelp(this, str);
				return;
			}
			Help.ShowHelp(this, "WcfTestClient.chm");
		}
        ComponentResourceManager componentResourceManager;
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.serviceTreeView = new System.Windows.Forms.TreeView();
            this.rootContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addServiceContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addServiceMainMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.recentServicesMainMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addServiceWorker = new System.ComponentModel.BackgroundWorker();
            this.invokeServiceWorker = new System.ComponentModel.BackgroundWorker();
            this.serviceTabControl = new System.Windows.Forms.TabControl();
            this.tabPageContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.globalStatusStrip = new System.Windows.Forms.StatusStrip();
            this.GlobalStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.serviceProjectContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshServiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeServiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.copyServiceProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyFullPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editWithSvcConfigEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.restoreToDefaultConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.endpointContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyEndpointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.operationContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyOperationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileSystemWatcher = new System.IO.FileSystemWatcher();
            this.saveProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.loadProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rootContextMenu.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.tabPageContextMenu.SuspendLayout();
            this.globalStatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.serviceProjectContextMenu.SuspendLayout();
            this.fileContextMenu.SuspendLayout();
            this.endpointContextMenu.SuspendLayout();
            this.operationContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher)).BeginInit();
            this.SuspendLayout();
            // 
            // serviceTreeView
            // 
            this.serviceTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serviceTreeView.Location = new System.Drawing.Point(0, 0);
            this.serviceTreeView.Name = "serviceTreeView";
            this.serviceTreeView.ShowNodeToolTips = true;
            this.serviceTreeView.Size = new System.Drawing.Size(263, 427);
            this.serviceTreeView.TabIndex = 0;
            this.serviceTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OpenNode);
            this.serviceTreeView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.serviceTreeView_KeyDown);
            this.serviceTreeView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.serviceTreeView_MouseDown);
            // 
            // rootContextMenu
            // 
            this.rootContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addServiceContextMenuItem});
            this.rootContextMenu.Name = "rootContextMenu";
            this.rootContextMenu.Size = new System.Drawing.Size(146, 26);
            // 
            // addServiceContextMenuItem
            // 
            this.addServiceContextMenuItem.Name = "addServiceContextMenuItem";
            this.addServiceContextMenuItem.Size = new System.Drawing.Size(145, 22);
            this.addServiceContextMenuItem.Text = "&Add Service...";
            this.addServiceContextMenuItem.Click += new System.EventHandler(this.addServiceMenuItem_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(792, 24);
            this.menuStrip.TabIndex = 3;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addServiceMainMenuItem,
            this.toolStripSeparator3,
            this.recentServicesMainMenuItem,
            this.toolStripSeparator5,
            this.saveProjectToolStripMenuItem,
            this.loadProjectToolStripMenuItem,
            this.toolStripSeparator4,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // addServiceMainMenuItem
            // 
            this.addServiceMainMenuItem.Name = "addServiceMainMenuItem";
            this.addServiceMainMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.A)));
            this.addServiceMainMenuItem.Size = new System.Drawing.Size(233, 22);
            this.addServiceMainMenuItem.Text = "&Add Service...";
            this.addServiceMainMenuItem.Click += new System.EventHandler(this.addServiceMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(230, 6);
            // 
            // recentServicesMainMenuItem
            // 
            this.recentServicesMainMenuItem.Enabled = false;
            this.recentServicesMainMenuItem.Name = "recentServicesMainMenuItem";
            this.recentServicesMainMenuItem.Size = new System.Drawing.Size(233, 22);
            this.recentServicesMainMenuItem.Text = "&Recent Services";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(230, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.optionsToolStripMenuItem.Text = "&Options...";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem1,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // helpToolStripMenuItem1
            // 
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            this.helpToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.helpToolStripMenuItem1.Size = new System.Drawing.Size(248, 22);
            this.helpToolStripMenuItem1.Text = "Hel&p";
            this.helpToolStripMenuItem1.Click += new System.EventHandler(this.helpToolStripMenuItem1_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
            this.aboutToolStripMenuItem.Text = "&About Microsoft WCF Test Client";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // addServiceWorker
            // 
            this.addServiceWorker.WorkerReportsProgress = true;
            this.addServiceWorker.WorkerSupportsCancellation = true;
            this.addServiceWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.addServiceWorker_DoWork);
            this.addServiceWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.addServiceWorker_RunWorkerCompleted);
            // 
            // invokeServiceWorker
            // 
            this.invokeServiceWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.invokeServiceWorker_DoWork);
            this.invokeServiceWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.invokeServiceWorker_RunWorkerCompleted);
            // 
            // serviceTabControl
            // 
            this.serviceTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serviceTabControl.Location = new System.Drawing.Point(0, 0);
            this.serviceTabControl.Name = "serviceTabControl";
            this.serviceTabControl.SelectedIndex = 0;
            this.serviceTabControl.ShowToolTips = true;
            this.serviceTabControl.Size = new System.Drawing.Size(525, 427);
            this.serviceTabControl.TabIndex = 4;
            this.serviceTabControl.SelectedIndexChanged += new System.EventHandler(this.serviceTabControl_SelectedIndexChanged);
            this.serviceTabControl.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.serviceTabControl_ControlAdded);
            this.serviceTabControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.serviceTabControl_KeyDown);
            this.serviceTabControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.serviceTabControl_MouseDown);
            // 
            // tabPageContextMenu
            // 
            this.tabPageContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeToolStripMenuItem,
            this.closeAllToolStripMenuItem});
            this.tabPageContextMenu.Name = "tabPageContextMenu";
            this.tabPageContextMenu.Size = new System.Drawing.Size(121, 48);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.closeToolStripMenuItem.Text = "&Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // closeAllToolStripMenuItem
            // 
            this.closeAllToolStripMenuItem.Name = "closeAllToolStripMenuItem";
            this.closeAllToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.closeAllToolStripMenuItem.Text = "Close &All";
            this.closeAllToolStripMenuItem.Click += new System.EventHandler(this.closeAllToolStripMenuItem_Click);
            // 
            // globalStatusStrip
            // 
            this.globalStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.GlobalStatusLabel});
            this.globalStatusStrip.Location = new System.Drawing.Point(0, 451);
            this.globalStatusStrip.Name = "globalStatusStrip";
            this.globalStatusStrip.Size = new System.Drawing.Size(792, 22);
            this.globalStatusStrip.TabIndex = 8;
            // 
            // GlobalStatusLabel
            // 
            this.GlobalStatusLabel.Name = "GlobalStatusLabel";
            this.GlobalStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 24);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.serviceTreeView);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.serviceTabControl);
            this.splitContainer.Size = new System.Drawing.Size(792, 427);
            this.splitContainer.SplitterDistance = 263;
            this.splitContainer.TabIndex = 7;
            // 
            // serviceProjectContextMenu
            // 
            this.serviceProjectContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshServiceToolStripMenuItem,
            this.removeServiceToolStripMenuItem,
            this.toolStripSeparator1,
            this.copyServiceProjectToolStripMenuItem});
            this.serviceProjectContextMenu.Name = "serviceProjectContextMenu";
            this.serviceProjectContextMenu.Size = new System.Drawing.Size(158, 76);
            // 
            // refreshServiceToolStripMenuItem
            // 
            this.refreshServiceToolStripMenuItem.Name = "refreshServiceToolStripMenuItem";
            this.refreshServiceToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.refreshServiceToolStripMenuItem.Text = "Re&fresh Service";
            this.refreshServiceToolStripMenuItem.Click += new System.EventHandler(this.refreshServiceToolStripMenuItem_Click);
            // 
            // removeServiceToolStripMenuItem
            // 
            this.removeServiceToolStripMenuItem.Name = "removeServiceToolStripMenuItem";
            this.removeServiceToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.removeServiceToolStripMenuItem.Text = "&Remove Service";
            this.removeServiceToolStripMenuItem.Click += new System.EventHandler(this.removeServiceToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(154, 6);
            // 
            // copyServiceProjectToolStripMenuItem
            // 
            this.copyServiceProjectToolStripMenuItem.Name = "copyServiceProjectToolStripMenuItem";
            this.copyServiceProjectToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.copyServiceProjectToolStripMenuItem.Text = "&Copy Address";
            this.copyServiceProjectToolStripMenuItem.Click += new System.EventHandler(this.copyServiceProjectToolStripMenuItem_Click);
            // 
            // fileContextMenu
            // 
            this.fileContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyFullPathToolStripMenuItem,
            this.editWithSvcConfigEditorToolStripMenuItem,
            this.toolStripSeparator2,
            this.restoreToDefaultConfigToolStripMenuItem});
            this.fileContextMenu.Name = "fileContextMenu";
            this.fileContextMenu.Size = new System.Drawing.Size(209, 76);
            // 
            // copyFullPathToolStripMenuItem
            // 
            this.copyFullPathToolStripMenuItem.Name = "copyFullPathToolStripMenuItem";
            this.copyFullPathToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.copyFullPathToolStripMenuItem.Text = "Copy &Full Path";
            this.copyFullPathToolStripMenuItem.Click += new System.EventHandler(this.copyFullPathToolStripMenuItem_Click);
            // 
            // editWithSvcConfigEditorToolStripMenuItem
            // 
            this.editWithSvcConfigEditorToolStripMenuItem.Name = "editWithSvcConfigEditorToolStripMenuItem";
            this.editWithSvcConfigEditorToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.editWithSvcConfigEditorToolStripMenuItem.Text = "Edit with SvcConfigEditor";
            this.editWithSvcConfigEditorToolStripMenuItem.Click += new System.EventHandler(this.editWithSvcConfigEditorToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(205, 6);
            // 
            // restoreToDefaultConfigToolStripMenuItem
            // 
            this.restoreToDefaultConfigToolStripMenuItem.Name = "restoreToDefaultConfigToolStripMenuItem";
            this.restoreToDefaultConfigToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.restoreToDefaultConfigToolStripMenuItem.Text = "Restore to Default Config";
            this.restoreToDefaultConfigToolStripMenuItem.Click += new System.EventHandler(this.restoreToDefaultConfigToolStripMenuItem_Click);
            // 
            // endpointContextMenu
            // 
            this.endpointContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyEndpointToolStripMenuItem});
            this.endpointContextMenu.Name = "endpointContextMenu";
            this.endpointContextMenu.Size = new System.Drawing.Size(103, 26);
            // 
            // copyEndpointToolStripMenuItem
            // 
            this.copyEndpointToolStripMenuItem.Name = "copyEndpointToolStripMenuItem";
            this.copyEndpointToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.copyEndpointToolStripMenuItem.Text = "&Copy";
            this.copyEndpointToolStripMenuItem.Click += new System.EventHandler(this.copyEndpointToolStripMenuItem_Click);
            // 
            // operationContextMenu
            // 
            this.operationContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyOperationToolStripMenuItem});
            this.operationContextMenu.Name = "operationContextMenu";
            this.operationContextMenu.Size = new System.Drawing.Size(103, 26);
            // 
            // copyOperationToolStripMenuItem
            // 
            this.copyOperationToolStripMenuItem.Name = "copyOperationToolStripMenuItem";
            this.copyOperationToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.copyOperationToolStripMenuItem.Text = "&Copy";
            this.copyOperationToolStripMenuItem.Click += new System.EventHandler(this.copyOperationToolStripMenuItem_Click);
            // 
            // fileSystemWatcher
            // 
            this.fileSystemWatcher.EnableRaisingEvents = true;
            this.fileSystemWatcher.Filter = "*.config";
            this.fileSystemWatcher.IncludeSubdirectories = true;
            this.fileSystemWatcher.NotifyFilter = System.IO.NotifyFilters.LastWrite;
            this.fileSystemWatcher.SynchronizingObject = this;
            this.fileSystemWatcher.Changed += new System.IO.FileSystemEventHandler(this.fileSystemWatcher_Changed);
            // 
            // saveProjectToolStripMenuItem
            // 
            this.saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
            this.saveProjectToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.saveProjectToolStripMenuItem.Text = "Save Project";
            this.saveProjectToolStripMenuItem.Click += new System.EventHandler(this.saveProjectToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(230, 6);
            // 
            // loadProjectToolStripMenuItem
            // 
            this.loadProjectToolStripMenuItem.Name = "loadProjectToolStripMenuItem";
            this.loadProjectToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.loadProjectToolStripMenuItem.Text = "Load Project";
            this.loadProjectToolStripMenuItem.Click += new System.EventHandler(this.loadProjectToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 473);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.globalStatusStrip);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WCF Test Client";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.rootContextMenu.ResumeLayout(false);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.tabPageContextMenu.ResumeLayout(false);
            this.globalStatusStrip.ResumeLayout(false);
            this.globalStatusStrip.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.serviceProjectContextMenu.ResumeLayout(false);
            this.fileContextMenu.ResumeLayout(false);
            this.endpointContextMenu.ResumeLayout(false);
            this.operationContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		private void invokeServiceWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			ServiceInvocationInputs argument = (ServiceInvocationInputs)e.Argument;
			argument.ServicePage.TestCase.Method.Endpoint.ServiceProject.IsWorking = true;
			ServiceInvocationOutputs servicePage = ServiceExecutor.ExecuteInClientDomain(argument);
			servicePage.ServicePage = argument.ServicePage;
			e.Result = servicePage;
		}

		private void invokeServiceWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (!this.isCancellingInvokeServiceWorker)
			{
				ServiceInvocationOutputs result = (ServiceInvocationOutputs)e.Result;
				Variable[] serviceInvocationResult = result.GetServiceInvocationResult();
				if (serviceInvocationResult != null)
				{
					result.ServicePage.PopulateOutput(serviceInvocationResult, result.ResponseXml);
					this.GlobalStatusLabel.Text = StringResources.StatusInvokingServiceCompleted;
					if (result.ServicePage.TestCase.Method.IsOneWay)
					{
						RtlAwareMessageBox.Show(this, StringResources.OneWayMessageDisplay, StringResources.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, 0);
					}
				}
				else if (result.ExceptionType != ExceptionType.InvalidInput)
				{
					result.ServicePage.PopulateOutput(serviceInvocationResult, result.ResponseXml);
					this.GlobalStatusLabel.Text = StringResources.StatusInvokingServiceFailed;
					StringBuilder stringBuilder = new StringBuilder(result.ExceptionMessages[0]);
					stringBuilder.Append(Environment.NewLine);
					stringBuilder.Append(result.ExceptionStacks[0]);
					for (int i = 1; i < (int)result.ExceptionMessages.Length; i++)
					{
						stringBuilder.Append(Environment.NewLine);
						stringBuilder.Append(Environment.NewLine);
						stringBuilder.Append("Inner Exception:");
						stringBuilder.Append(Environment.NewLine);
						stringBuilder.Append(result.ExceptionMessages[i]);
						stringBuilder.Append(Environment.NewLine);
						stringBuilder.Append(result.ExceptionStacks[i]);
					}
					result.ServicePage.TestCase.SetError(new ErrorItem(StringResources.StatusInvokingServiceFailed, stringBuilder.ToString(), result.ServicePage.TestCase));
				}
				else
				{
					RtlAwareMessageBox.Show(result.ExceptionMessages[0], StringResources.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, 0);
				}
				this.TriggerDelayedConfigRefresh(result.ServicePage.TestCase.Method.Endpoint.ServiceProject);
			}
			this.isCancellingInvokeServiceWorker = false;
			this.UpdateButtonStatus();
		}

		internal void InvokeTestCase(ServicePage servicePage, Variable[] inputs, bool newClient)
		{
			bool flag;
			if (ApplicationSettings.GetInstance().SecurityPromptEnabled)
			{
				if (!WarningPromptDialog.Prompt(this, StringResources.SecurityWarningTitle, StringResources.SecurityWarning, out flag))
				{
					return;
				}
				if (flag)
				{
					ApplicationSettings.GetInstance().SecurityPromptEnabled = false;
				}
			}
			this.GlobalStatusLabel.Text = StringResources.StatusInvokingService;
			this.UpdateButtonStatus(false, true);
			this.invokeServiceWorker.RunWorkerAsync(new ServiceInvocationInputs(inputs, servicePage, newClient));
		}

		private bool IsBusy(bool startingAddServiceWorker, bool startingInvokeServiceWorker)
		{
			if (startingInvokeServiceWorker || this.invokeServiceWorker.IsBusy || startingAddServiceWorker)
			{
				return true;
			}
			return this.addServiceWorker.IsBusy;
		}

		private void MainForm_Closing(object sender, CancelEventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			this.GlobalStatusLabel.Text = StringResources.ClosingClientStatus;
			this.fileSystemWatcher.Dispose();
			this.workspace.Close();
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.CancelInvokeServiceWorker();
		}

		private void MainForm_Shown(object sender, EventArgs e)
		{
			if (this.endpoints == null)
			{
				return;
			}
			this.StartAddServiceWorker(new AddServiceInputs(this.endpoints), StringResources.AddingAction, StringResources.StatusAddingService);
		}

		internal void OnErrorReported(params ErrorItem[] errorItem)
		{
			ErrorDialog.DisplayError(this, errorItem);
		}

		private void OnServiceProjectAdded(ServiceProject serviceProject)
		{
			this.serviceTreeView.BeginUpdate();
			TreeNode item = this.serviceTreeView.Nodes[0];
			TreeNode treeNode = new TreeNode(serviceProject.Address);
			int num = 2;
			int num1 = num;
			treeNode.ImageIndex = num;
			treeNode.SelectedImageIndex = num1;
			treeNode.ContextMenuStrip = this.serviceProjectContextMenu;
			item.Nodes.Add(treeNode);
			this.AddNodesForServiceProject(serviceProject, treeNode);
			item.Expand();
			this.serviceTreeView.EndUpdate();
		}

		private void OnServiceProjectRefreshed(ServiceProject serviceProject)
		{
			this.AddNodesForServiceProject(serviceProject, this.serviceTreeView.SelectedNode);
		}

		private void OnServiceProjectRemoved(ServiceProject serviceProject)
		{
			serviceProject.ServiceProjectNode.Remove();
		}

		private void OpenMethod(ServiceMethodInfo selectedMethod)
		{
			ServicePage servicePage = new ServicePage(this, selectedMethod);
			this.serviceTabControl.TabPages.Add(servicePage);
			servicePage.ResetInput();
			this.serviceTabControl.SelectedTab = servicePage;
			this.UpdateButtonStatus();
		}

		private void OpenNode(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				this.OpenNode(this.serviceTreeView.SelectedNode);
			}
		}

		private void OpenNode(TreeNode selectedNode)
		{
			if (selectedNode == null)
			{
				return;
			}
			ServiceMethodInfo tag = selectedNode.Tag as ServiceMethodInfo;
			FileItem fileItem = selectedNode.Tag as FileItem;
			if (tag != null && tag.Endpoint.Valid && tag.Valid)
			{
				this.CloseStartPageIfExists();
				this.OpenMethod(tag);
				return;
			}
			if (fileItem != null)
			{
				this.CloseStartPageIfExists();
				this.SwitchTab(fileItem.FilePage);
			}
		}

		private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			bool flag;
			if (OptionsForm.Prompt(this, out flag))
			{
				ApplicationSettings.GetInstance().RegenerateConfigEnabled = flag;
			}
		}

		private void recentServiceMenuItem_Click(object sender, EventArgs e)
		{
			string text = ((ToolStripMenuItem)sender).Text;
			int num = text.IndexOf(' ');
			this.AddService(text.Substring(num + 1));
		}

		private void RefreshConfig(ServiceProject serviceProject)
		{
			List<ErrorItem> errorItems;
			List<TestCase> referencedTestCases = serviceProject.ReferencedTestCases;
			this.GlobalStatusLabel.Text = StringResources.StatusLoadingConfig;
			if (serviceProject.RefreshConfig(out errorItems))
			{
				this.CloseProjectServicePages(referencedTestCases);
				this.serviceTreeView.BeginUpdate();
				foreach (TreeNode node in this.serviceTreeView.Nodes[0].Nodes)
				{
					if (!object.ReferenceEquals(node.Tag as ServiceProject, serviceProject))
					{
						continue;
					}
					node.Nodes.Clear();
					this.AddNodesForServiceProject(serviceProject, node);
					break;
				}
				this.serviceTreeView.EndUpdate();
				this.GlobalStatusLabel.Text = StringResources.StatusLoadingConfigCompleted;
			}
			else
			{
				this.GlobalStatusLabel.Text = StringResources.StatusLoadingConfigFailed;
			}
			if (errorItems != null && errorItems.Count > 0)
			{
				ErrorItem[] errorItemArray = new ErrorItem[errorItems.Count];
				errorItems.CopyTo(errorItemArray);
				this.OnErrorReported(errorItemArray);
			}
		}

		private void refreshServiceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			bool flag;
			if (this.serviceTreeView.SelectedNode.Tag is ServiceProject)
			{
				if (ApplicationSettings.GetInstance().RefreshPromptEnabled)
				{
					if (!WarningPromptDialog.Prompt(this, StringResources.RefreshServiceWarningTitle, StringResources.RefreshServiceWarning, out flag))
					{
						return;
					}
					if (flag)
					{
						ApplicationSettings.GetInstance().RefreshPromptEnabled = false;
					}
				}
				ServiceProject tag = this.serviceTreeView.SelectedNode.Tag as ServiceProject;
				tag.IsWorking = true;
				AddServiceOutputs.IsRefreshing = true;
				string[] address = new string[] { tag.Address };
				this.StartAddServiceWorker(new AddServiceInputs(address), StringResources.RefreshingAction, StringResources.StatusRefreshingService);
			}
		}

		private void RemoveSelectedTabPage()
		{
			this.serviceTabControl.SelectedTab.Focus();
			this.tabPageCloseOrderManager.HandleTabClosed(this.serviceTabControl.SelectedIndex);
			this.removingTabPage = true;
			if (this.serviceTabControl.SelectedTab is ServicePage)
			{
				((ServicePage)this.serviceTabControl.SelectedTab).Close();
			}
			this.serviceTabControl.TabPages.RemoveAt(this.serviceTabControl.SelectedIndex);
			this.removingTabPage = false;
			this.serviceTabControl.SelectedIndex = this.tabPageCloseOrderManager.LastTab();
		}

		private ServiceProject RemoveServiceProject()
		{
			ServiceProject tag = (ServiceProject)this.serviceTreeView.SelectedNode.Tag;
			ICollection<TabPage> tabPages = new List<TabPage>();
			foreach (FileItem referencedFile in tag.ReferencedFiles)
			{
				tabPages.Add(referencedFile.FilePage);
			}
			foreach (TestCase referencedTestCase in tag.ReferencedTestCases)
			{
				ServicePage servicePage = referencedTestCase.ServicePage;
				if (tabPages.Contains(servicePage))
				{
					continue;
				}
				tabPages.Add(servicePage);
			}
			foreach (TabPage tabPage in tabPages)
			{
				if (this.serviceTabControl.Contains(tabPage))
				{
					this.serviceTabControl.SelectedTab = tabPage;
					this.RemoveSelectedTabPage();
				}
				tabPage.Dispose();
			}
			this.workspace.Remove(tag);
			return tag;
		}

		private void removeServiceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.serviceTreeView.SelectedNode.Tag is ServiceProject && RtlAwareMessageBox.Show(this, StringResources.RemoveServiceWarning, StringResources.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, 0) == System.Windows.Forms.DialogResult.OK)
			{
				this.GlobalStatusLabel.Text = StringResources.StatusRemovingService;
				this.UpdateButtonStatus(true, false);
				this.OnServiceProjectRemoved(this.RemoveServiceProject());
				this.GlobalStatusLabel.Text = StringResources.StatusRemovingServiceCompleted;
				this.UpdateButtonStatus();
			}
		}

		private void restoreToDefaultConfigToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string str;
			ServiceProject tag = (ServiceProject)this.serviceTreeView.SelectedNode.Parent.Tag;
			this.fileSystemWatcher.EnableRaisingEvents = false;
			tag.RestoreDefaultConfig(out str);
			if (str != null)
			{
				ErrorItem[] errorItem = new ErrorItem[] { new ErrorItem(StringResources.DefaultConfigNotFound, str, null) };
				this.OnErrorReported(errorItem);
			}
			this.fileSystemWatcher.EnableRaisingEvents = true;
			this.RefreshConfig(tag);
		}

		private void serviceTabControl_ControlAdded(object sender, ControlEventArgs e)
		{
			if (this.serviceTabControl.TabCount == 1)
			{
				this.tabPageCloseOrderManager.HandleTabAdded(this.serviceTabControl.SelectedIndex);
			}
		}

		private void serviceTabControl_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && !e.Alt && !e.Shift && e.KeyCode == Keys.F4)
			{
				if (this.serviceTabControl.TabCount > 0)
				{
					this.RemoveSelectedTabPage();
					return;
				}
			}
			else if (e.KeyCode == Keys.Apps && this.serviceTabControl.TabCount > 0 && this.serviceTabControl.Focused)
			{
				Rectangle tabRect = this.serviceTabControl.GetTabRect(this.serviceTabControl.SelectedIndex);
				this.tabPageContextMenu.Show(this.serviceTabControl, (tabRect.Left + tabRect.Right) / 2, (tabRect.Top + tabRect.Bottom) / 2);
			}
		}

		private void serviceTabControl_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button != System.Windows.Forms.MouseButtons.Right)
			{
				return;
			}
			int num = 0;
			while (num < this.serviceTabControl.TabCount)
			{
				if (!this.serviceTabControl.GetTabRect(num).Contains(e.X, e.Y))
				{
					num++;
				}
				else
				{
					this.serviceTabControl.SelectedTab = this.serviceTabControl.TabPages[num];
					break;
				}
			}
			this.tabPageContextMenu.Show(this.serviceTabControl, e.X, e.Y);
		}

		private void serviceTabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.serviceTabControl.SelectedIndex != -1 && !this.removingTabPage)
			{
				this.tabPageCloseOrderManager.HandleTabChanged(this.serviceTabControl.SelectedIndex);
			}
			this.UpdateButtonStatus();
		}

		private void serviceTreeView_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				this.removeServiceToolStripMenuItem_Click(null, null);
			}
			if (!e.Control && !e.Alt && !e.Shift && e.KeyCode == Keys.Return)
			{
				this.OpenNode(this.serviceTreeView.SelectedNode);
			}
			if (e.Control && !e.Alt && !e.Shift && e.KeyCode == Keys.C)
			{
				this.CopyNode(this.serviceTreeView.SelectedNode);
			}
		}

		private void serviceTreeView_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Right)
			{
				this.serviceTreeView.SelectedNode = this.serviceTreeView.GetNodeAt(e.X, e.Y);
			}
		}

		private void SetFileWatchPath()
		{
			try
			{
				this.fileSystemWatcher.Path = ApplicationSettings.GetInstance().ProjectBase;
			}
			catch (IOException oException)
			{
			}
		}

		private void StartAddServiceWorker(AddServiceInputs inputs, string action, string status)
		{
			this.UpdateButtonStatus(true, false);
			this.GlobalStatusLabel.Text = status;
			this.addServiceWorker.RunWorkerAsync(inputs);
			if (!ProgressDialog.Prompt(this, action, status, this.addServiceWorker))
			{
				this.addServiceWorker.CancelAsync();
			}
		}

		private void SwitchTab(TabPage tabPage)
		{
			if (!this.serviceTabControl.TabPages.Contains(tabPage))
			{
				this.serviceTabControl.TabPages.Add(tabPage);
			}
			this.serviceTabControl.SelectedTab = tabPage;
			this.UpdateButtonStatus();
		}

		private void TriggerDelayedConfigRefresh(ServiceProject project)
		{
			project.IsWorking = false;
			if (project.IsConfigChanged)
			{
				project.IsConfigChanged = false;
				this.TryRefreshConfig(project);
			}
		}

		private void TryRefreshConfig(ServiceProject serviceProject)
		{
			if (serviceProject.IsWorking)
			{
				serviceProject.IsConfigChanged = true;
				return;
			}
			if (FileChangingForm.ShouldPrompt(serviceProject.Address) && FileChangingForm.Prompt(this, serviceProject.Address))
			{
				this.RefreshConfig(serviceProject);
			}
		}

		private void UpdateButtonStatus()
		{
			this.UpdateButtonStatus(false, false);
		}

		private void UpdateButtonStatus(bool startingAddServiceWorker, bool startingInvokeServiceWorker)
		{
			bool flag = this.IsBusy(startingAddServiceWorker, startingInvokeServiceWorker);
			bool flag1 = (flag ? false : this.serviceTabControl.SelectedTab is ServicePage);
			foreach (TabPage tabPage in this.serviceTabControl.TabPages)
			{
				if (!(tabPage is ServicePage))
				{
					continue;
				}
				((ServicePage)tabPage).ChangeInvokeStatus(flag1);
			}
			this.removeServiceToolStripMenuItem.Enabled = !flag;
			this.addServiceMainMenuItem.Enabled = !flag;
			this.addServiceContextMenuItem.Enabled = this.addServiceMainMenuItem.Enabled;
			this.recentServicesMainMenuItem.Enabled = (flag ? false : ApplicationSettings.GetInstance().RecentUrls.Count > 0);
			this.refreshServiceToolStripMenuItem.Enabled = !flag;
			this.editWithSvcConfigEditorToolStripMenuItem.Enabled = !flag;
			this.restoreToDefaultConfigToolStripMenuItem.Enabled = !flag;
			this.closeToolStripMenuItem.Enabled = this.serviceTabControl.TabPages.Count != 0;
		}

		public event EventHandler ServicesRefreshed;

		private enum IconIndex
		{
			ApplicationIcon,
			EndpointImage,
			ContractImage,
			FileImage,
			OperationImage,
			ErrorImage
		}

        private void SaveProject(string file)
        {
            List<string> lista = new List<string>();
            foreach (TreeNode node in this.serviceTreeView.Nodes[0].Nodes)
            {
                if (node.Tag is ServiceProject)
                    lista.Add((node.Tag as ServiceProject).Address);
            }
            if (lista.Count > 0)
                Repository<List<string>>.Serialize(lista, file);

        }

        private void LoadProject(string file)
        {
            List<string> lista = Repository<List<string>>.Deserialize(file);

            foreach (string serviceAddress in lista)
            {
                this.AddService(serviceAddress);
            }

        }

        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "WCFCT project|*.wcfct";
            saveFileDialog1.Title = "Save the project WCF Client Test";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                this.SaveProject(saveFileDialog1.FileName);
            }
        }

        private void loadProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "";
            openFileDialog1.Filter = "WCFCT project (*.wcfct)|*.wcfct|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.LoadProject(openFileDialog1.FileName);
            }
        }
	}
}