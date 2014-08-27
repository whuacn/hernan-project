using Microsoft.Tools.TestClient;
using System;

namespace Microsoft.Tools.TestClient.Variables
{
	[Serializable]
	internal class BooleanVariable : Variable
	{
		internal BooleanVariable(TypeMemberInfo declaredMember) : base(declaredMember)
		{
		}

		internal override object CreateObject()
		{
			return bool.Parse(this.@value);
		}

		internal override void ValidateAndCanonicalize(string input)
		{
			bool flag;
			if (bool.TryParse(input, out flag))
			{
				this.@value = input;
				return;
			}
			this.@value = null;
		}
	}
}