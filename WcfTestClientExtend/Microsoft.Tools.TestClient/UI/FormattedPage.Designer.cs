using Microsoft.Tools.Common;
using Microsoft.Tools.TestClient;
using Microsoft.Tools.TestClient.Variables;
using Microsoft.VisualStudio.VirtualTreeGrid;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WcfTestClient;

namespace Microsoft.Tools.TestClient.UI
{
	internal class FormattedPage : TabPage
	{
		private ServicePage servicePage;

		private IContainer components;

		private SplitContainer splitContainer1;

		private VirtualTreeControl inputControl;

		private VirtualTreeControl outputControl;

		private Panel requestHeaderPanel;

		private Panel requestControlPanel;

		private Button invokeButton;

		private Label requestLabel;

		private Label responseLabel;

		private CheckBox newProxyCheckBox;

		internal FormattedPage()
		{
			this.InitializeComponent();
			VirtualTreeColumnHeader[] virtualTreeColumnHeader = new VirtualTreeColumnHeader[] { new VirtualTreeColumnHeader(StringResources.NameHeader), new VirtualTreeColumnHeader(StringResources.ValueHeader), new VirtualTreeColumnHeader(StringResources.TypeHeader) };
			this.inputControl.SetColumnHeaders(virtualTreeColumnHeader, true);
			VirtualTreeColumnHeader[] virtualTreeColumnHeaderArray = new VirtualTreeColumnHeader[] { new VirtualTreeColumnHeader(StringResources.NameHeader), new VirtualTreeColumnHeader(StringResources.ValueHeader), new VirtualTreeColumnHeader(StringResources.TypeHeader) };
			this.outputControl.SetColumnHeaders(virtualTreeColumnHeaderArray, true);
			this.outputControl.KeyDown += new KeyEventHandler(this.outputControl_KeyDown);
		}

		internal FormattedPage(ServicePage servicePage) : this()
		{
			this.servicePage = servicePage;
		}

		internal void ChangeInvokeStatus(bool invokeButtonStatus)
		{
			CheckBox checkBox = this.newProxyCheckBox;
			VirtualTreeControl virtualTreeControl = this.inputControl;
			bool flag = invokeButtonStatus;
			bool flag1 = flag;
			this.invokeButton.Enabled = flag;
			bool flag2 = flag1;
			bool flag3 = flag2;
			virtualTreeControl.Enabled = flag2;
			checkBox.Enabled = flag3;
		}

		internal void Close()
		{
			this.CloseVirtualTreeControl(this.inputControl);
			this.CloseVirtualTreeControl(this.outputControl);
		}

		private void CloseVirtualTreeControl(VirtualTreeControl virtualTreeControl)
		{
			if (virtualTreeControl.MultiColumnTree != null)
			{
				ParameterTreeAdapter root = ((ITree)virtualTreeControl.MultiColumnTree).Root as ParameterTreeAdapter;
				if (root != null)
				{
					root.Close();
				}
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

		private void FormattedPage_OnValueUpdated()
		{
			this.outputControl.MultiColumnTree = null;
			this.servicePage.OnValueUpdated();
		}

		internal Variable[] GetVariables()
		{
			return ((ParameterTreeAdapter)((ITree)this.inputControl.MultiColumnTree).Root).GetVariables();
		}

		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FormattedPage));
			this.splitContainer1 = new SplitContainer();
			this.inputControl = new VirtualTreeControl();
			this.requestHeaderPanel = new Panel();
			this.requestLabel = new Label();
			this.outputControl = new VirtualTreeControl();
			this.requestControlPanel = new Panel();
			this.responseLabel = new Label();
			this.invokeButton = new Button();
			this.newProxyCheckBox = new CheckBox();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.requestHeaderPanel.SuspendLayout();
			this.requestControlPanel.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.splitContainer1, "splitContainer1");
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Panel1.Controls.Add(this.inputControl);
			this.splitContainer1.Panel1.Controls.Add(this.requestHeaderPanel);
			this.splitContainer1.Panel2.Controls.Add(this.outputControl);
			this.splitContainer1.Panel2.Controls.Add(this.requestControlPanel);
			componentResourceManager.ApplyResources(this.inputControl, "inputControl");
			this.inputControl.HasGridLines = true;
			this.inputControl.HasHorizontalGridLines = true;
			this.inputControl.HasVerticalGridLines = true;
			this.inputControl.LabelEditSupport = VirtualTreeLabelEditActivationStyles.ImmediateSelection;
			this.inputControl.Name = "inputControl";
			this.requestHeaderPanel.BackColor = SystemColors.Control;
			this.requestHeaderPanel.Controls.Add(this.requestLabel);
			componentResourceManager.ApplyResources(this.requestHeaderPanel, "requestHeaderPanel");
			this.requestHeaderPanel.Name = "requestHeaderPanel";
			componentResourceManager.ApplyResources(this.requestLabel, "requestLabel");
			this.requestLabel.Name = "requestLabel";
			componentResourceManager.ApplyResources(this.outputControl, "outputControl");
			this.outputControl.HasGridLines = true;
			this.outputControl.HasHorizontalGridLines = true;
			this.outputControl.HasVerticalGridLines = true;
			this.outputControl.LabelEditSupport = VirtualTreeLabelEditActivationStyles.ImmediateSelection;
			this.outputControl.Name = "outputControl";
			this.requestControlPanel.BackColor = SystemColors.Control;
			this.requestControlPanel.Controls.Add(this.responseLabel);
			this.requestControlPanel.Controls.Add(this.invokeButton);
			this.requestControlPanel.Controls.Add(this.newProxyCheckBox);
			componentResourceManager.ApplyResources(this.requestControlPanel, "requestControlPanel");
			this.requestControlPanel.Name = "requestControlPanel";
			componentResourceManager.ApplyResources(this.responseLabel, "responseLabel");
			this.responseLabel.Name = "responseLabel";
			componentResourceManager.ApplyResources(this.invokeButton, "invokeButton");
			this.invokeButton.Name = "invokeButton";
			this.invokeButton.UseVisualStyleBackColor = true;
			this.invokeButton.Click += new EventHandler(this.invokeButton_Click);
			componentResourceManager.ApplyResources(this.newProxyCheckBox, "newProxyCheckBox");
			this.newProxyCheckBox.Name = "newProxyCheckBox";
			this.newProxyCheckBox.UseVisualStyleBackColor = true;
			base.Controls.Add(this.splitContainer1);
			componentResourceManager.ApplyResources(this, "$this");
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.requestHeaderPanel.ResumeLayout(false);
			this.requestHeaderPanel.PerformLayout();
			this.requestControlPanel.ResumeLayout(false);
			this.requestControlPanel.PerformLayout();
			base.ResumeLayout(false);
		}

		private void invokeButton_Click(object sender, EventArgs e)
		{
			this.servicePage.InvokeTestCase(this.GetVariables(), this.newProxyCheckBox.Checked);
		}

		private void outputControl_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && !e.Alt && !e.Shift && e.KeyCode == Keys.C)
			{
				ColumnItemEnumerator columnItemEnumerator = this.outputControl.CreateSelectedItemEnumerator();
				if (columnItemEnumerator != null)
				{
					int rowInTree = columnItemEnumerator.RowInTree + 1;
					int columnInTree = columnItemEnumerator.ColumnInTree;
					VirtualTreeItemInfo itemInfo = this.outputControl.Tree.GetItemInfo(rowInTree, columnInTree, false);
					string text = itemInfo.Branch.GetText(itemInfo.Row, itemInfo.Column);
					if (string.IsNullOrEmpty(text))
					{
						SafeClipboard.Clear();
						return;
					}
					SafeClipboard.SetText(text);
				}
			}
		}

		internal void PopulateOutput(Variable[] variables)
		{
			this.PopulateTree(variables, this.outputControl, true);
			for (int i = 0; i < (int)variables.Length; i++)
			{
				int length = (int)variables.Length - i - 1;
				if (this.outputControl.Tree.IsExpandable(length, 0))
				{
					this.outputControl.Tree.ToggleExpansion(length, 0);
				}
			}
		}

		private void PopulateTree(Variable[] variables, VirtualTreeControl parameterTreeView, bool readOnly)
		{
			parameterTreeView.MultiColumnTree = new MultiColumnTree(3);
			ITree multiColumnTree = (ITree)parameterTreeView.MultiColumnTree;
			multiColumnTree.Root = new ParameterTreeAdapter(multiColumnTree, parameterTreeView, variables, readOnly, null);
			((ParameterTreeAdapter)((ITree)this.inputControl.MultiColumnTree).Root).OnValueUpdated += new ParameterTreeAdapter.ValueUpdatedEventHandler(this.FormattedPage_OnValueUpdated);
		}

		internal void ResetInputTree()
		{
			Variable[] variables = this.servicePage.TestCase.Method.GetVariables();
			this.PopulateTree(variables, this.inputControl, false);
			for (int i = 0; i < (int)variables.Length; i++)
			{
				int length = (int)variables.Length - i - 1;
				if (this.inputControl.Tree.IsExpandable(length, 0))
				{
					this.inputControl.Tree.ToggleExpansion(length, 0);
				}
			}
		}
	}
}