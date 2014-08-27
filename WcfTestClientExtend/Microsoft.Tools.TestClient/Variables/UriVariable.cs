using Microsoft.Tools.TestClient;
using System;

namespace Microsoft.Tools.TestClient.Variables
{
	[Serializable]
	internal class UriVariable : Variable
	{
		internal UriVariable(TypeMemberInfo declaredMember) : base(declaredMember)
		{
		}

		internal override object CreateObject()
		{
			if (this.@value.Equals("(null)"))
			{
				return null;
			}
			return new Uri(this.@value);
		}

		internal override void ValidateAndCanonicalize(string input)
		{
			Uri uri;
			if (input.Equals("(null)"))
			{
				base.ValidateAndCanonicalize(input);
				return;
			}
			if (!Uri.TryCreate(input, UriKind.Absolute, out uri))
			{
				this.@value = null;
				return;
			}
			this.@value = uri.ToString();
		}
	}
}