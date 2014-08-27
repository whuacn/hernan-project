using Microsoft.Tools.TestClient;
using System;
using System.ComponentModel;

namespace Microsoft.Tools.TestClient.Variables
{
	[Serializable]
	internal class TimeSpanVariable : Variable
	{
		internal TimeSpanVariable(TypeMemberInfo declaredMember) : base(declaredMember)
		{
		}

		internal override object CreateObject()
		{
			return (new TimeSpanConverter()).ConvertFrom(this.@value);
		}

		internal override void ValidateAndCanonicalize(string input)
		{
			try
			{
				this.@value = (new TimeSpanConverter()).ConvertFrom(input).ToString();
			}
			catch (FormatException formatException)
			{
				this.@value = null;
				return;
			}
			base.ValidateAndCanonicalize(this.@value);
		}
	}
}