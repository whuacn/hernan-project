using Microsoft.Tools.TestClient;
using System;

namespace Microsoft.Tools.TestClient.Variables
{
	[Serializable]
	internal class StringVariable : Variable
	{
		internal StringVariable(TypeMemberInfo declaredMember) : base(declaredMember)
		{
		}

		internal override object CreateObject()
		{
			if (this.@value.Equals("(null)"))
			{
				return null;
			}
			return StringFormatter.FromEscapeCode(this.@value);
		}

		internal override void ValidateAndCanonicalize(string input)
		{
			base.ValidateAndCanonicalize(input);
		}
	}
}