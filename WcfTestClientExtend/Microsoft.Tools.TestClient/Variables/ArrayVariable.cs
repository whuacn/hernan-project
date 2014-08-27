using Microsoft.Tools.TestClient;
using System;
using System.Globalization;

namespace Microsoft.Tools.TestClient.Variables
{
	[Serializable]
	internal class ArrayVariable : ContainerVariable
	{
		internal ArrayVariable(TypeMemberInfo declaredMember) : base(declaredMember)
		{
		}

		internal override object CreateObject()
		{
			if (this.@value.Equals("(null)"))
			{
				return null;
			}
			Type type = ClientSettings.GetType(this.currentMember.TypeName.Substring(0, this.currentMember.TypeName.Length - 2));
			Array arrays = Array.CreateInstance(type, int.Parse(this.@value.Substring("length=".Length), CultureInfo.CurrentCulture));
			int num = 0;
			base.GetChildVariables();
			if (this.childVariables != null)
			{
				Variable[] variableArray = this.childVariables;
				for (int i = 0; i < (int)variableArray.Length; i++)
				{
					Variable variable = variableArray[i];
					int num1 = num;
					num = num1 + 1;
					arrays.SetValue(variable.CreateObject(), num1);
				}
			}
			return arrays;
		}
	}
}