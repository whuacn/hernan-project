using Microsoft.Tools.TestClient;
using System;

namespace Microsoft.Tools.TestClient.Variables
{
	[Serializable]
	internal class CharVariable : Variable
	{
		internal CharVariable(TypeMemberInfo declaredMember) : base(declaredMember)
		{
		}

		internal override object CreateObject()
		{
			return this.@value[0];
		}

		internal override void ValidateAndCanonicalize(string input)
		{
			if (input.Length == 1)
			{
				this.@value = input;
				return;
			}
			this.@value = null;
		}
	}
}