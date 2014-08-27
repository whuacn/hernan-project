using Microsoft.Tools.TestClient;
using System;
using System.ComponentModel;

namespace Microsoft.Tools.TestClient.Variables
{
	[Serializable]
	internal class DateTimeVariable : Variable
	{
		internal DateTimeVariable(TypeMemberInfo declaredMember) : base(declaredMember)
		{
		}

		internal override object CreateObject()
		{
			return (new DateTimeConverter()).ConvertFrom(this.@value);
		}

		internal override void ValidateAndCanonicalize(string input)
		{
			try
			{
				this.@value = (new DateTimeConverter()).ConvertFrom(input).ToString();
			}
			catch (FormatException formatException)
			{
				this.@value = null;
			}
		}
	}
}