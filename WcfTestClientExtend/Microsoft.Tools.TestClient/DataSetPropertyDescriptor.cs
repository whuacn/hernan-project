using System;
using System.ComponentModel;
using System.Data;

namespace Microsoft.Tools.TestClient
{
	internal class DataSetPropertyDescriptor : PropertyDescriptor
	{
		private bool isReadOnly;

		private ParameterTreeAdapter paraTree;

		private object @value;

		private DataSetUITypeEditor dataSetUITypeEditor;

		public override Type ComponentType
		{
			get
			{
				throw (new ExceptionUtility()).ThrowHelperError(new InvalidOperationException());
			}
		}

		public override bool IsReadOnly
		{
			get
			{
				return this.isReadOnly;
			}
		}

		public override Type PropertyType
		{
			get
			{
				return typeof(DataSet);
			}
		}

		internal DataSetPropertyDescriptor(object value, bool isReadOnly, ParameterTreeAdapter paraTree) : base("DataSetProperty", null)
		{
			this.@value = value;
			this.isReadOnly = isReadOnly;
			this.paraTree = paraTree;
		}

		public override bool CanResetValue(object component)
		{
			return false;
		}

		public void Close()
		{
			if (this.dataSetUITypeEditor != null)
			{
				this.dataSetUITypeEditor.Close();
			}
		}

		public override object GetEditor(Type editorBaseType)
		{
			this.dataSetUITypeEditor = new DataSetUITypeEditor(this.isReadOnly, this.paraTree);
			return this.dataSetUITypeEditor;
		}

		public override object GetValue(object component)
		{
			return this.@value;
		}

		public override void ResetValue(object component)
		{
			throw (new ExceptionUtility()).ThrowHelperError(new InvalidOperationException());
		}

		public override void SetValue(object component, object value)
		{
		}

		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}
	}
}