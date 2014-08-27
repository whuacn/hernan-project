using Microsoft.Tools.TestClient;
using System;

namespace Microsoft.Tools.TestClient.Variables
{
	[Serializable]
	internal class ContainerVariable : Variable
	{
		internal ContainerVariable(TypeMemberInfo declaredMember) : base(declaredMember)
		{
		}

		internal override void ValidateAndCanonicalize(string input)
		{
			base.ValidateAndCanonicalize(input);
			int num = -1;
			if (this.@value == null || input.Equals("(null)"))
			{
				return;
			}
			char[] chrArray = new char[] { ' ' };
			if (!input.TrimStart(chrArray).StartsWith("length", StringComparison.OrdinalIgnoreCase))
			{
				this.@value = null;
				return;
			}
			this.@value = input.Replace(" ", "");
			if (this.@value.StartsWith("length=", StringComparison.OrdinalIgnoreCase))
			{
				input = this.@value.Substring("length=".Length);
				if (int.TryParse(input, out num) && num >= 0)
				{
					this.@value = string.Concat("length=", input);
					return;
				}
			}
			this.@value = null;
		}
	}
}