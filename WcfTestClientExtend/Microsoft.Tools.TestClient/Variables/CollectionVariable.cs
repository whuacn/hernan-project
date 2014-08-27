using Microsoft.Tools.TestClient;
using System;
using System.Reflection;

namespace Microsoft.Tools.TestClient.Variables
{
	[Serializable]
	internal class CollectionVariable : ContainerVariable
	{
		internal CollectionVariable(TypeMemberInfo declaredMember) : base(declaredMember)
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
			if (this.childVariables != null)
			{
				MethodInfo method = type.GetMethod("Add");
				Variable[] variableArray = this.childVariables;
				for (int i = 0; i < (int)variableArray.Length; i++)
				{
					Variable variable = variableArray[i];
					object[] objArray = new object[] { variable.CreateObject() };
					method.Invoke(obj, objArray);
				}
			}
			return obj;
		}
	}
}