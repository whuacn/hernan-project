using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using WcfTestClient;

namespace Microsoft.Tools.TestClient
{
	[Serializable]
	internal class SerializableType
	{
		internal const string LengthRepresentation = "length=";

		internal const string NullRepresentation = "(null)";

		private static List<string> numericTypes;

		private string dataSetSchema;

		private Microsoft.Tools.TestClient.EditorType editorType;

		private string[] enumChoices;

		private string friendlyName;

		private bool isInvalid;

		private ICollection<TypeMemberInfo> members = new List<TypeMemberInfo>();

		private ICollection<SerializableType> subTypes = new List<SerializableType>();

		private string typeName;

		private Microsoft.Tools.TestClient.TypeProperty typeProperty = new Microsoft.Tools.TestClient.TypeProperty();

		internal string DataSetSchema
		{
			get
			{
				return this.dataSetSchema;
			}
		}

		internal Microsoft.Tools.TestClient.EditorType EditorType
		{
			get
			{
				return this.editorType;
			}
		}

		internal string FriendlyName
		{
			get
			{
				if (this.friendlyName == null)
				{
					this.ComposeFriendlyName();
				}
				return this.friendlyName;
			}
		}

		internal bool IsInvalid
		{
			get
			{
				return this.isInvalid;
			}
		}

		internal ICollection<TypeMemberInfo> Members
		{
			get
			{
				return this.members;
			}
		}

		internal ICollection<SerializableType> SubTypes
		{
			get
			{
				return this.subTypes;
			}
		}

		internal string TypeName
		{
			get
			{
				return this.typeName;
			}
		}

		internal Microsoft.Tools.TestClient.TypeProperty TypeProperty
		{
			get
			{
				return this.typeProperty;
			}
		}

		static SerializableType()
		{
			string[] fullName = new string[] { typeof(short).FullName, typeof(int).FullName, typeof(long).FullName, typeof(ushort).FullName, typeof(uint).FullName, typeof(ulong).FullName, typeof(byte).FullName, typeof(sbyte).FullName, typeof(float).FullName, typeof(double).FullName, typeof(decimal).FullName };
			SerializableType.numericTypes = new List<string>(fullName);
		}

		internal SerializableType(Type type)
		{
			this.typeName = type.FullName;
			if (type.IsEnum)
			{
				this.enumChoices = Enum.GetNames(type);
				this.typeProperty.IsEnum = true;
			}
			else if (SerializableType.numericTypes.Contains(this.typeName))
			{
				this.typeProperty.IsNumeric = true;
			}
			else if (DataContractAnalyzer.IsDataSet(type))
			{
				this.typeProperty.IsDataSet = true;
				this.dataSetSchema = (Activator.CreateInstance(type) as DataSet).GetXmlSchema();
			}
			else if (type.IsArray)
			{
				this.typeProperty.IsArray = true;
			}
			else if (DataContractAnalyzer.IsNullableType(type))
			{
				this.typeProperty.IsNullable = true;
			}
			else if (DataContractAnalyzer.IsCollectionType(type))
			{
				this.typeProperty.IsCollection = true;
			}
			else if (DataContractAnalyzer.IsDictionaryType(type))
			{
				this.typeProperty.IsDictionary = true;
			}
			else if (DataContractAnalyzer.IsKeyValuePairType(type))
			{
				this.typeProperty.IsKeyValuePair = true;
			}
			else if (DataContractAnalyzer.IsSupportedType(type))
			{
				this.typeProperty.IsComposite = true;
				if (type.IsValueType)
				{
					this.typeProperty.IsStruct = true;
				}
			}
			if (SerializableType.numericTypes.Contains(this.typeName) || this.typeName.Equals("System.Char", StringComparison.Ordinal) || this.typeName.Equals("System.Guid", StringComparison.Ordinal) || this.typeName.Equals("System.DateTime", StringComparison.Ordinal) || this.typeName.Equals("System.DateTimeOffset", StringComparison.Ordinal) || this.typeName.Equals("System.TimeSpan", StringComparison.Ordinal))
			{
				this.editorType = Microsoft.Tools.TestClient.EditorType.TextBox;
				return;
			}
			if (this.typeName.Equals("System.String", StringComparison.Ordinal) || this.typeName.Equals("System.Uri", StringComparison.Ordinal) || this.typeName.Equals("System.Xml.XmlQualifiedName", StringComparison.Ordinal) || this.IsContainer())
			{
				this.editorType = Microsoft.Tools.TestClient.EditorType.EditableDropDownBox;
				return;
			}
			if (this.typeName.Equals("System.Boolean", StringComparison.Ordinal) || this.HasMembers() || this.enumChoices != null || this.typeProperty.IsDataSet)
			{
				this.editorType = Microsoft.Tools.TestClient.EditorType.DropDownBox;
				return;
			}
			this.isInvalid = !this.typeName.Equals(typeof(NullObject).FullName, StringComparison.Ordinal);
		}

		private void ComposeFriendlyName()
		{
			int num = this.TypeName.IndexOf('\u0060');
			if (num <= -1)
			{
				this.friendlyName = this.TypeName;
				return;
			}
			StringBuilder stringBuilder = new StringBuilder(this.TypeName.Substring(0, num));
			stringBuilder.Append("<");
			ICollection<TypeMemberInfo> members = this.members;
			if (this.typeProperty.IsDictionary)
			{
				members = ((List<TypeMemberInfo>)this.members)[0].Members;
			}
			int num1 = 0;
			foreach (TypeMemberInfo member in members)
			{
				int num2 = num1;
				num1 = num2 + 1;
				if (num2 > 0)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(member.FriendlyTypeName);
			}
			stringBuilder.Append(">");
			this.friendlyName = stringBuilder.ToString();
		}

		internal string GetDefaultValue()
		{
			if (this.enumChoices != null)
			{
				return this.enumChoices[0];
			}
			if (SerializableType.numericTypes.Contains(this.typeName))
			{
				return "0";
			}
			if (this.typeName.Equals("System.Boolean", StringComparison.Ordinal))
			{
				return bool.FalseString;
			}
			if (this.typeName.Equals("System.Char", StringComparison.Ordinal))
			{
				return "A";
			}
			if (this.typeName.Equals("System.Guid", StringComparison.Ordinal))
			{
				return Guid.NewGuid().ToString();
			}
			if (this.typeName.Equals("System.DateTime", StringComparison.Ordinal))
			{
				string shortDateString = DateTime.Now.ToShortDateString();
				DateTime now = DateTime.Now;
				return string.Concat(shortDateString, " ", now.ToShortTimeString());
			}
			if (this.typeName.Equals("System.DateTimeOffset", StringComparison.Ordinal))
			{
				return DateTimeOffset.Now.ToString();
			}
			if (this.typeName.Equals("System.TimeSpan", StringComparison.Ordinal))
			{
				return TimeSpan.Zero.ToString();
			}
			if (this.typeName.Equals("System.Uri", StringComparison.Ordinal))
			{
				return "http://localhost";
			}
			if (this.typeName.Equals("System.Xml.XmlQualifiedName", StringComparison.Ordinal))
			{
				return "namespace:name";
			}
			if (this.IsContainer())
			{
				return "length=0";
			}
			if (!this.typeProperty.IsKeyValuePair && !this.typeProperty.IsStruct)
			{
				return "(null)";
			}
			return this.typeName;
		}

		internal string[] GetSelectionList()
		{
			string[] strArrays;
			if (this.editorType != Microsoft.Tools.TestClient.EditorType.EditableDropDownBox)
			{
				if (this.editorType != Microsoft.Tools.TestClient.EditorType.DropDownBox)
				{
					return null;
				}
				if (!this.typeName.Equals("System.Boolean"))
				{
					if (this.enumChoices != null)
					{
						return this.enumChoices;
					}
					if (this.typeProperty.IsKeyValuePair || this.typeProperty.IsStruct)
					{
						strArrays = new string[] { this.typeName };
					}
					else
					{
						strArrays = (!this.typeProperty.IsDataSet ? new string[0] : new string[] { "(null)", StringResources.EditDataSet });
					}
				}
				else
				{
					strArrays = new string[] { "True", "False" };
				}
			}
			else
			{
				strArrays = new string[] { "(null)" };
			}
			if (strArrays != null && (int)strArrays.Length == 0)
			{
				List<string> strs = new List<string>()
				{
					"(null)",
					this.typeName
				};
				foreach (SerializableType subType in this.subTypes)
				{
					if (subType.IsInvalid)
					{
						continue;
					}
					strs.Add(subType.TypeName);
				}
				strArrays = new string[strs.Count];
				strs.CopyTo(strArrays);
			}
			return strArrays;
		}

		internal string GetStringRepresentation(object obj)
		{
			if (obj == null || this.typeName.Equals("Microsoft.Tools.TestClient.NullObject"))
			{
				return "(null)";
			}
			if (this.typeProperty.IsDataSet)
			{
				return StringResources.ViewDataSet;
			}
			if (this.editorType == Microsoft.Tools.TestClient.EditorType.DropDownBox)
			{
				if (!obj.GetType().Equals(typeof(bool)) && this.enumChoices == null)
				{
					return string.Empty;
				}
				return obj.ToString();
			}
			if (obj.GetType().IsArray)
			{
				return string.Concat("length=", ((Array)obj).Length);
			}
			if (DataContractAnalyzer.IsDictionaryType(obj.GetType()) || DataContractAnalyzer.IsCollectionType(obj.GetType()))
			{
				return string.Concat("length=", ((ICollection)obj).Count);
			}
			if (!(obj is string))
			{
				return obj.ToString();
			}
			return StringFormatter.ToEscapeCode(obj.ToString());
		}

		internal bool HasMembers()
		{
			if (this.typeProperty.IsComposite || this.typeProperty.IsNullable)
			{
				return true;
			}
			return this.typeProperty.IsKeyValuePair;
		}

		internal bool IsContainer()
		{
			if (this.typeProperty.IsArray || this.typeProperty.IsDictionary)
			{
				return true;
			}
			return this.typeProperty.IsCollection;
		}

		internal static bool IsNullRepresentation(string value)
		{
			return string.Equals(value, "(null)", StringComparison.Ordinal);
		}

		internal void MarkAsInvalid()
		{
			this.isInvalid = true;
		}
	}
}