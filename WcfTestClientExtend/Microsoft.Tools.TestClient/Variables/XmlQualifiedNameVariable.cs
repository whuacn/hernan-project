using Microsoft.Tools.TestClient;
using System;
using System.Xml;

namespace Microsoft.Tools.TestClient.Variables
{
	[Serializable]
	internal class XmlQualifiedNameVariable : Variable
	{
		internal XmlQualifiedNameVariable(TypeMemberInfo declaredMember) : base(declaredMember)
		{
		}

		internal override object CreateObject()
		{
			XmlQualifiedName xmlQualifiedName;
			if (this.@value.Equals("(null)"))
			{
				return null;
			}
			if (!XmlQualifiedNameVariable.TryParseXmlQualifiedName(this.@value, out xmlQualifiedName))
			{
				xmlQualifiedName = null;
			}
			return xmlQualifiedName;
		}

		private static bool TryParseXmlQualifiedName(string stringRepresentation, out XmlQualifiedName result)
		{
			int num = stringRepresentation.LastIndexOf(":", StringComparison.Ordinal);
			if (num == -1)
			{
				result = new XmlQualifiedName(stringRepresentation);
				return true;
			}
			string str = stringRepresentation.Substring(0, num);
			string str1 = stringRepresentation.Substring(num + 1);
			if (string.IsNullOrEmpty(str1))
			{
				result = null;
				return false;
			}
			result = new XmlQualifiedName(str1, str);
			return true;
		}

		internal override void ValidateAndCanonicalize(string input)
		{
			XmlQualifiedName xmlQualifiedName;
			if (input.Equals("(null)"))
			{
				base.ValidateAndCanonicalize(input);
				return;
			}
			if (this.@value == null)
			{
				return;
			}
			if (!XmlQualifiedNameVariable.TryParseXmlQualifiedName(input, out xmlQualifiedName))
			{
				this.@value = null;
				return;
			}
			this.@value = xmlQualifiedName.ToString();
		}
	}
}