using Microsoft.Tools.TestClient.UI;
using Microsoft.Tools.TestClient.Variables;
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Microsoft.Tools.TestClient
{
	internal class DataSetUITypeEditor : UITypeEditor
	{
		private DataSetEditorForm dataSetEditorForm;

		private bool isReadOnly;

		private ParameterTreeAdapter paraTree;

		public DataSetUITypeEditor(bool isReadOnly, ParameterTreeAdapter paraTree)
		{
			this.isReadOnly = isReadOnly;
			this.paraTree = paraTree;
		}

		private void Clean()
		{
			if (this.dataSetEditorForm != null)
			{
				this.dataSetEditorForm.Dispose();
				this.dataSetEditorForm = null;
			}
		}

		public void Close()
		{
			if (this.dataSetEditorForm != null)
			{
				this.dataSetEditorForm.Close();
				this.Clean();
			}
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			DataSetVariable dataSetVariable = (value as DataSetVariable).Clone() as DataSetVariable;
			this.dataSetEditorForm = new DataSetEditorForm(value, this.isReadOnly);
			if (this.dataSetEditorForm.ShowDialog() == DialogResult.OK)
			{
				this.paraTree.PropagateValueUpdateEvent();
			}
			else if (!(value as DataSetVariable).CopyFrom(dataSetVariable))
			{
				value = null;
			}
			this.Clean();
			return null;
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}
	}
}