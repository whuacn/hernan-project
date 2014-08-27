using Microsoft.Tools.TestClient;
using System;
using System.ComponentModel;
using System.Data;
using System.IO;

namespace Microsoft.Tools.TestClient.Variables
{
	[Serializable]
	internal class DataSetVariable : Variable
	{
		private DataSet dataSetValue;

		private bool IsGeneralDataSet
		{
			get
			{
				return string.Equals(this.currentMember.TypeName, "System.Data.DataSet", StringComparison.Ordinal);
			}
		}

		internal DataSetVariable(TypeMemberInfo declaredMember) : base(declaredMember)
		{
			this.CreateEmptyDataSet(declaredMember.DataSetSchema);
		}

		internal DataSetVariable(TypeMemberInfo declaredMember, object obj) : base(declaredMember, obj)
		{
			this.CreateEmptyDataSet(((DataSet)obj).GetXmlSchema());
			using (StringReader stringReader = new StringReader(((DataSet)obj).GetXml()))
			{
				this.dataSetValue.ReadXml(stringReader);
			}
		}

		internal override Variable Clone()
		{
			DataSetVariable dataSetVariable = new DataSetVariable(this.currentMember);
			if (dataSetVariable.CopyFrom(this))
			{
				return dataSetVariable;
			}
			return null;
		}

		internal override bool CopyFrom(Variable variable)
		{
			if (variable == null || object.ReferenceEquals(this, variable))
			{
				return false;
			}
			this.dataSetValue.Dispose();
			this.dataSetValue = (variable as DataSetVariable).dataSetValue.Copy();
			return true;
		}

		private void CreateEmptyDataSet(string schema)
		{
			this.dataSetValue = new DataSet();
			using (StringReader stringReader = new StringReader(schema))
			{
				this.dataSetValue.ReadXmlSchema(stringReader);
			}
			this.dataSetValue.Locale = this.dataSetValue.Locale;
		}

		internal override object CreateObject()
		{
			if (this.@value.Equals("(null)"))
			{
				return null;
			}
			if (this.IsGeneralDataSet)
			{
				return this.dataSetValue;
			}
			Type type = ClientSettings.GetType(this.currentMember.TypeName);
			object obj = Activator.CreateInstance(type);
			using (StringReader stringReader = new StringReader(this.dataSetValue.GetXml()))
			{
				((DataSet)obj).ReadXml(stringReader);
			}
			return obj;
		}

		internal object GetDataSetValue()
		{
			return this.dataSetValue;
		}

		internal string GetXmlSchema()
		{
			return this.dataSetValue.GetXmlSchema();
		}

		internal bool IsDefaultDataSet()
		{
			DataSet dataSet = new DataSet(this.dataSetValue.DataSetName);
			return this.SchemaEquals(new DataSetVariable(this.currentMember, dataSet));
		}

		internal bool SchemaEquals(Variable variable)
		{
			DataSetVariable dataSetVariable = variable as DataSetVariable;
			return string.Equals(this.GetXmlSchema(), dataSetVariable.GetXmlSchema(), StringComparison.Ordinal);
		}
	}
}