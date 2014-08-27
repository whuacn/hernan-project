using Microsoft.Tools.TestClient;
using System;
using System.Globalization;

namespace Microsoft.Tools.TestClient.Variables
{
	[Serializable]
	internal class DateTimeOffsetVariable : Variable
	{
		internal DateTimeOffsetVariable(TypeMemberInfo declaredMember) : base(declaredMember)
		{
		}

		internal override object CreateObject()
		{
			return DateTimeOffset.Parse(this.@value, CultureInfo.CurrentCulture);
		}

		internal override void ValidateAndCanonicalize(string input)
		{
			DateTimeOffset dateTimeOffset;
			base.ValidateAndCanonicalize(input);
			if (this.@value == null)
			{
				return;
			}
			if (!DateTimeOffset.TryParse(input, out dateTimeOffset))
			{
				this.@value = null;
				return;
			}
			this.@value = dateTimeOffset.ToString();
		}
	}
}