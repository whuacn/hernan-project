using Microsoft.Tools.TestClient;
using Microsoft.Tools.TestClient.Variables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using WcfTestClient;

namespace Microsoft.Tools.TestClient.UI
{
	internal class DataSetEditorForm : Form
	{
		private DataSetVariable dataSetVariable;

		private IContainer components;

		private DataGridWrapper dataGrid;

		private Button cancelButton;

		private Button okButton;

		private Button pasteButton;

		private Button copyButton;

		public DataSetEditorForm(object value, bool isReadOnly)
		{
			this.InitializeComponent();
			this.dataSetVariable = value as DataSetVariable;
			this.SetDataSource(this.dataSetVariable.GetDataSetValue());
			this.dataGrid.ReadOnly = isReadOnly;
			if (this.dataGrid.ReadOnly)
			{
				this.pasteButton.Visible = false;
				this.copyButton.Location = this.pasteButton.Location;
			}
			if (Variable.VariablesPool.Count < 1)
			{
				this.pasteButton.Enabled = false;
			}
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			base.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		}

		private void copyButton_Click(object sender, EventArgs e)
		{
			Variable.SaveToPool(this.dataSetVariable);
			if (Variable.VariablesPool.Count > 0)
			{
				this.pasteButton.Enabled = true;
			}
		}

		private void dataGrid_Navigate(object sender, NavigateEventArgs ne)
		{
			string dataSetName = (this.dataSetVariable.GetDataSetValue() as DataSet).DataSetName;
			if (DataSetEditorForm.IsAtDataSetRoot(this.dataGrid))
			{
				this.dataGrid.CaptionText = dataSetName;
				return;
			}
			DataGridWrapper dataGridWrapper = this.dataGrid;
			CultureInfo currentUICulture = CultureInfo.CurrentUICulture;
			object[] dataMember = new object[] { dataSetName, this.dataGrid.DataMember };
			dataGridWrapper.CaptionText = string.Format(currentUICulture, "{0}: {1}", dataMember);
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
			componentResourceManager = new ComponentResourceManager(typeof(DataSetEditorForm));
			this.dataGrid = new DataGridWrapper();
			this.cancelButton = new Button();
			this.okButton = new Button();
			this.pasteButton = new Button();
			this.copyButton = new Button();
			((ISupportInitialize)this.dataGrid).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.dataGrid, "dataGrid");
			this.dataGrid.DataMember = "";
			this.dataGrid.HeaderForeColor = SystemColors.ControlText;
			this.dataGrid.Name = "dataGrid";
			this.dataGrid.Navigate += new NavigateEventHandler(this.dataGrid_Navigate);
			componentResourceManager.ApplyResources(this.cancelButton, "cancelButton");
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new EventHandler(this.cancelButton_Click);
			componentResourceManager.ApplyResources(this.okButton, "okButton");
			this.okButton.Name = "okButton";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new EventHandler(this.okButton_Click);
			componentResourceManager.ApplyResources(this.pasteButton, "pasteButton");
			this.pasteButton.Name = "pasteButton";
			this.pasteButton.UseVisualStyleBackColor = true;
			this.pasteButton.Click += new EventHandler(this.pasteButton_Click);
			componentResourceManager.ApplyResources(this.copyButton, "copyButton");
			this.copyButton.BackColor = SystemColors.Control;
			this.copyButton.Name = "copyButton";
			this.copyButton.UseVisualStyleBackColor = true;
			this.copyButton.Click += new EventHandler(this.copyButton_Click);
			base.AcceptButton = this.okButton;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.cancelButton;
			base.ControlBox = false;
			base.Controls.Add(this.copyButton);
			base.Controls.Add(this.pasteButton);
			base.Controls.Add(this.okButton);
			base.Controls.Add(this.cancelButton);
			base.Controls.Add(this.dataGrid);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "DataSetEditorForm";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			((ISupportInitialize)this.dataGrid).EndInit();
			base.ResumeLayout(false);
		}

		private static bool IsAtDataSetRoot(DataGrid dataGrid)
		{
			if (dataGrid.DataMember == null)
			{
				return true;
			}
			return dataGrid.DataMember == string.Empty;
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			base.DialogResult = System.Windows.Forms.DialogResult.OK;
		}

		private void pasteButton_Click(object sender, EventArgs e)
		{
			if (!this.dataSetVariable.IsDefaultDataSet() && !this.dataSetVariable.SchemaEquals(Variable.VariablesPool[0]) && RtlAwareMessageBox.Show(StringResources.DifferentSchemaWarning, StringResources.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, 0) != System.Windows.Forms.DialogResult.OK)
			{
				return;
			}
			if (this.dataSetVariable.CopyFrom(Variable.VariablesPool[0]))
			{
				this.dataGrid.DataSource = null;
				this.SetDataSource(this.dataSetVariable.GetDataSetValue());
			}
		}

		private void SetDataSource(object o)
		{
			DataSet dataSet = o as DataSet;
			if (dataSet != null)
			{
				this.dataGrid.DataSource = dataSet;
				this.dataGrid.Expand(-1);
				this.dataGrid.CaptionText = dataSet.DataSetName;
				if (dataSet.Tables.Count == 1)
				{
					this.dataGrid.NavigateTo(0, dataSet.Tables[0].TableName);
				}
			}
		}
	}
}