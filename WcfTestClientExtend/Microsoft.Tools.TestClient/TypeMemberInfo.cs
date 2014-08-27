using System;
using System.Collections.Generic;

namespace Microsoft.Tools.TestClient
{
	[Serializable]
	internal class TypeMemberInfo : IComparable
	{
		private SerializableType type;

		private string variableName;

		internal string DataSetSchema
		{
			get
			{
				return this.type.DataSetSchema;
			}
		}

		internal Microsoft.Tools.TestClient.EditorType EditorType
		{
			get
			{
				return this.type.EditorType;
			}
		}

		internal string FriendlyTypeName
		{
			get
			{
				return this.type.FriendlyName;
			}
		}

		internal bool IsInvalid
		{
			get
			{
				return this.type.IsInvalid;
			}
		}

		internal ICollection<TypeMemberInfo> Members
		{
			get
			{
				return this.type.Members;
			}
		}

		internal ICollection<SerializableType> SubTypes
		{
			get
			{
				return this.type.SubTypes;
			}
		}

		internal string TypeName
		{
			get
			{
				return this.type.TypeName;
			}
		}

		internal Microsoft.Tools.TestClient.TypeProperty TypeProperty
		{
			get
			{
				return this.type.TypeProperty;
			}
		}

		internal string VariableName
		{
			get
			{
				return this.variableName;
			}
		}

		internal TypeMemberInfo(string variableName, SerializableType type)
		{
			this.variableName = variableName;
			this.type = type;
		}

		public int CompareTo(object obj)
		{
			TypeMemberInfo typeMemberInfo = (TypeMemberInfo)obj;
			return string.Compare(this.variableName, typeMemberInfo.variableName, StringComparison.Ordinal);
		}

		internal string GetDefaultValue()
		{
			return this.type.GetDefaultValue();
		}

		internal string[] GetSelectionList()
		{
			return this.type.GetSelectionList();
		}

		internal string GetStringRepresentation(object obj)
		{
			return this.type.GetStringRepresentation(obj);
		}

		internal bool HasMembers()
		{
			return this.type.HasMembers();
		}

		internal bool IsContainer()
		{
			return this.type.IsContainer();
		}
	}
}