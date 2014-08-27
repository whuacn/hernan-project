using Microsoft.Tools.TestClient;
using System;
using System.Reflection;

namespace Microsoft.Tools.TestClient.Variables
{
	[Serializable]
	internal class CompositeVariable : Variable
	{
		internal CompositeVariable(TypeMemberInfo declaredMember) : base(declaredMember)
		{
		}

		internal override object CreateObject()
		{
			if (this.@value.Equals("(null)"))
			{
				return null;
			}
			base.GetChildVariables();
			Type type = ClientSettings.GetType(this.currentMember.TypeName);
			object obj = Activator.CreateInstance(type);
			Variable[] variableArray = this.childVariables;
			for (int i = 0; i < (int)variableArray.Length; i++)
			{
				Variable variable = variableArray[i];
				PropertyInfo property = type.GetProperty(variable.Name);
				if (property == null)
				{
					FieldInfo field = type.GetField(variable.Name);
					field.SetValue(obj, variable.CreateObject());
				}
				else
				{
					property.SetValue(obj, variable.CreateObject(), null);
				}
			}
			return obj;
		}
	}
}