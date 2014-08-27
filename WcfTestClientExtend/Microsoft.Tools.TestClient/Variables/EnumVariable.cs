using Microsoft.Tools.TestClient;
using System;

namespace Microsoft.Tools.TestClient.Variables
{
	[Serializable]
	internal class EnumVariable : Variable
	{
		internal EnumVariable(TypeMemberInfo declaredMember) : base(declaredMember)
		{
		}

		internal override object CreateObject()
		{
			return Enum.Parse(ClientSettings.GetType(this.currentMember.TypeName), this.@value);
		}
	}
}