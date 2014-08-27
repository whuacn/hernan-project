using Microsoft.Tools.TestClient;
using System;

namespace Microsoft.Tools.TestClient.Variables
{
	[Serializable]
	internal class GuidVariable : Variable
	{
		internal GuidVariable(TypeMemberInfo declaredMember) : base(declaredMember)
		{
		}

		internal override object CreateObject()
		{
			return new Guid(this.@value);
		}

		internal override void ValidateAndCanonicalize(string input)
		{
			try
			{
				this.@value = (new Guid(input)).ToString();
				base.ValidateAndCanonicalize(this.@value);
				return;
			}
			catch (FormatException formatException)
			{
				this.@value = null;
			}
			catch (OverflowException overflowException)
			{
				this.@value = null;
			}
			catch (ArgumentException argumentException)
			{
				this.@value = null;
			}
		}
	}
}