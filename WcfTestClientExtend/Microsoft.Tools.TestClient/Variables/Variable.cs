using Microsoft.Tools.TestClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using WcfTestClient;

namespace Microsoft.Tools.TestClient.Variables
{
	[Serializable]
	internal class Variable
	{
		protected Variable[] childVariables;

		protected TypeMemberInfo currentMember;

		protected TypeMemberInfo declaredMember;

		protected string name;

		[NonSerialized]
		protected ServiceMethodInfo serviceMethodInfo;

		protected string @value;

		private readonly static Variable[] empty;

		private static int poolSize;

		private static IList<Variable> variablesPool;

		[NonSerialized]
		private bool isKey;

		private bool modifiable = true;

		[NonSerialized]
		private Variable parent;

		internal Microsoft.Tools.TestClient.EditorType EditorType
		{
			get
			{
				if (this.declaredMember.EditorType == Microsoft.Tools.TestClient.EditorType.EditableDropDownBox)
				{
					string[] selectionList = this.GetSelectionList();
					if (selectionList == null || (int)selectionList.Length < 1)
					{
						return Microsoft.Tools.TestClient.EditorType.TextBox;
					}
				}
				return this.declaredMember.EditorType;
			}
		}

		internal string FriendlyTypeName
		{
			get
			{
				return this.declaredMember.FriendlyTypeName;
			}
		}

		internal bool IsKey
		{
			set
			{
				this.isKey = value;
				if (this.isKey && this.@value.Equals("(null)", StringComparison.Ordinal))
				{
					if (this.declaredMember.HasMembers())
					{
						this.@value = this.TypeName;
					}
					if (this.TypeName.Equals("System.String", StringComparison.Ordinal))
					{
						this.@value = string.Empty;
					}
				}
			}
		}

		internal virtual string Name
		{
			get
			{
				if (this.name != null)
				{
					return this.name;
				}
				return this.declaredMember.VariableName;
			}
			set
			{
				this.name = value;
			}
		}

		internal string TypeName
		{
			get
			{
				return this.declaredMember.TypeName;
			}
		}

		internal static IList<Variable> VariablesPool
		{
			get
			{
				return Variable.variablesPool;
			}
		}

		static Variable()
		{
			Variable.empty = new Variable[0];
			Variable.poolSize = 1;
			Variable.variablesPool = new List<Variable>();
		}

		internal Variable(TypeMemberInfo declaredMember)
		{
			TypeMemberInfo typeMemberInfo = declaredMember;
			TypeMemberInfo typeMemberInfo1 = typeMemberInfo;
			this.currentMember = typeMemberInfo;
			this.declaredMember = typeMemberInfo1;
			this.@value = this.currentMember.GetDefaultValue();
		}

		internal Variable(TypeMemberInfo declaredMember, object obj) : this(declaredMember)
		{
			this.@value = declaredMember.GetStringRepresentation(obj);
			this.modifiable = false;
		}

		internal virtual Variable Clone()
		{
			return null;
		}

		internal virtual bool CopyFrom(Variable variable)
		{
			if (variable == null || object.ReferenceEquals(this, variable))
			{
				return false;
			}
			this.@value = variable.@value;
			return true;
		}

		internal virtual object CreateObject()
		{
			return null;
		}

		private static int GetArrayLength(string canonicalizedValue)
		{
			return int.Parse(canonicalizedValue.Substring("length=".Length), CultureInfo.CurrentCulture);
		}

		internal Variable[] GetChildVariables()
		{
			if (string.Equals(this.@value, "(null)", StringComparison.Ordinal))
			{
				return Variable.empty;
			}
			if (this.modifiable)
			{
				if (this.declaredMember.HasMembers() && (this.childVariables == null || this.@value != this.currentMember.TypeName))
				{
					this.currentMember = this.declaredMember;
					string variableName = this.declaredMember.VariableName;
					foreach (SerializableType subType in this.declaredMember.SubTypes)
					{
						if (!subType.TypeName.Equals(this.@value))
						{
							continue;
						}
						this.currentMember = new TypeMemberInfo(variableName, subType);
						break;
					}
					this.childVariables = new Variable[this.currentMember.Members.Count];
					int num = 0;
					foreach (TypeMemberInfo member in this.currentMember.Members)
					{
						this.childVariables[num] = VariableFactory.CreateAssociateVariable(member);
						if (this.currentMember.TypeProperty.IsKeyValuePair && string.Equals(member.VariableName, "Key", StringComparison.Ordinal))
						{
							this.childVariables[num].IsKey = true;
						}
						this.childVariables[num].SetServiceMethodInfo(this.serviceMethodInfo);
						if (this.parent != null)
						{
							this.childVariables[num].SetParent(this);
						}
						num++;
					}
				}
				if (this.declaredMember.IsContainer())
				{
					int arrayLength = Variable.GetArrayLength(this.@value);
					Variable[] variableArray = this.childVariables;
					this.childVariables = new Variable[arrayLength];
					TypeMemberInfo current = null;
					using (IEnumerator<TypeMemberInfo> enumerator = this.declaredMember.Members.GetEnumerator())
					{
						if (enumerator.MoveNext())
						{
							current = enumerator.Current;
						}
					}
					for (int i = 0; i < arrayLength; i++)
					{
						if (variableArray == null || i >= (int)variableArray.Length)
						{
							this.childVariables[i] = VariableFactory.CreateAssociateVariable(string.Concat("[", i, "]"), current);
							this.childVariables[i].SetServiceMethodInfo(this.serviceMethodInfo);
							if (this.declaredMember.TypeProperty.IsDictionary || this.parent != null)
							{
								this.childVariables[i].SetParent(this);
								if (this.declaredMember.TypeProperty.IsDictionary)
								{
									this.childVariables[i].GetChildVariables();
								}
							}
						}
						else
						{
							this.childVariables[i] = variableArray[i];
						}
					}
				}
			}
			return this.childVariables;
		}

		internal string[] GetSelectionList()
		{
			string[] selectionList = this.declaredMember.GetSelectionList();
			if (this.isKey && selectionList != null)
			{
				int num = Array.FindIndex<string>(selectionList, new Predicate<string>(Variable.IsNullRepresentation));
				if (num >= 0)
				{
					string[] strArrays = new string[(int)selectionList.Length - 1];
					int num1 = 0;
					for (int i = 0; i < (int)strArrays.Length; i++)
					{
						if (num1 == num)
						{
							num1++;
						}
						strArrays[i] = selectionList[num1];
						num1++;
					}
					return strArrays;
				}
			}
			return selectionList;
		}

		internal string GetValue()
		{
			if (string.Equals(this.@value, this.TypeName, StringComparison.Ordinal) && this.currentMember.HasMembers())
			{
				return this.FriendlyTypeName;
			}
			return this.@value;
		}

		internal bool IsExpandable()
		{
			if (this.childVariables != null && (int)this.childVariables.Length > 0)
			{
				return true;
			}
			if (this.EditorType != Microsoft.Tools.TestClient.EditorType.DropDownBox)
			{
				if (!this.declaredMember.IsContainer() || this.@value.Equals("(null)"))
				{
					return false;
				}
				return Variable.GetArrayLength(this.@value) > 0;
			}
			if (this.declaredMember.TypeName.Equals("System.Boolean") || this.declaredMember.TypeProperty.IsEnum || this.declaredMember.TypeProperty.IsDataSet)
			{
				return false;
			}
			return !string.Equals(this.@value, "(null)", StringComparison.Ordinal);
		}

		private static bool IsNullRepresentation(string str)
		{
			return string.CompareOrdinal(str, "(null)") == 0;
		}

		internal static void SaveToPool(Variable variable)
		{
			if (Variable.variablesPool.Count == Variable.poolSize)
			{
				Variable.variablesPool.RemoveAt(0);
			}
			Variable variable1 = variable.Clone();
			Variable.variablesPool.Add(variable1);
		}

		internal void SetChildVariables(Variable[] value)
		{
			this.childVariables = value;
		}

		private void SetParent(Variable parent)
		{
			this.parent = parent;
		}

		internal void SetServiceMethodInfo(ServiceMethodInfo serviceMethodInfo)
		{
			this.serviceMethodInfo = serviceMethodInfo;
		}

		internal ValidationResult SetValue(string userValue)
		{
			string str = this.@value;
			this.ValidateAndCanonicalize(userValue);
			if (this.@value == null)
			{
				this.@value = str;
				CultureInfo currentUICulture = CultureInfo.CurrentUICulture;
				string typeError = StringResources.TypeError;
				object[] objArray = new object[] { userValue };
				string str1 = string.Format(currentUICulture, typeError, objArray);
				RtlAwareMessageBox.Show(str1, StringResources.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, 0);
				return new ValidationResult(false, false);
			}
			bool flag = false;
			if (this.parent != null)
			{
				Variable variable = this;
				while (variable != null && !variable.isKey)
				{
					variable = variable.parent;
				}
				if (variable != null)
				{
					flag = true;
					variable = variable.parent.parent;
					variable.ValidateAndCanonicalize(variable.@value);
				}
			}
			if (this.EditorType == Microsoft.Tools.TestClient.EditorType.EditableDropDownBox && this.declaredMember.IsContainer())
			{
				if (!string.Equals(this.@value, "(null)", StringComparison.Ordinal))
				{
					if (string.Equals(str, "(null)", StringComparison.Ordinal))
					{
						return new ValidationResult(true, true);
					}
					int arrayLength = Variable.GetArrayLength(str);
					int num = Variable.GetArrayLength(this.@value);
					return new ValidationResult(true, arrayLength != num);
				}
				if (this.declaredMember.Members.Count > 0)
				{
					return new ValidationResult(true, true);
				}
			}
			if (this.EditorType == Microsoft.Tools.TestClient.EditorType.DropDownBox)
			{
				if (!string.Equals(this.@value, "(null)", StringComparison.Ordinal))
				{
					return new ValidationResult(true, true);
				}
				if (this.currentMember.Members.Count > 0)
				{
					return new ValidationResult(true, true);
				}
			}
			return new ValidationResult(true, flag);
		}

		internal virtual void ValidateAndCanonicalize(string input)
		{
			if (input == null)
			{
				this.@value = null;
				return;
			}
			if (this.isKey && string.Equals(input, "(null)", StringComparison.Ordinal))
			{
				this.@value = null;
				return;
			}
			this.@value = input;
		}
	}
}