using Microsoft.Tools.TestClient;
using System;
using System.Globalization;
using System.Reflection;

namespace Microsoft.Tools.TestClient.Variables
{
	[Serializable]
	internal class NumericVariable : Variable
	{
		private MethodInfo parseMethod;

		internal NumericVariable(TypeMemberInfo declaredMember) : base(declaredMember)
		{
			Type type = Type.GetType(this.currentMember.TypeName);
			Type[] typeArray = new Type[] { typeof(string), typeof(IFormatProvider) };
			this.parseMethod = type.GetMethod("Parse", typeArray);
		}

		internal override object CreateObject()
		{
			MethodInfo methodInfo = this.parseMethod;
			object[] currentUICulture = new object[] { this.@value, CultureInfo.CurrentUICulture };
			return methodInfo.Invoke(null, currentUICulture);
		}

		internal override void ValidateAndCanonicalize(string input)
		{
			Type type = Type.GetType(this.currentMember.TypeName);
			object[] objArray = new object[] { input, null };
			Type[] typeArray = new Type[] { typeof(string), Type.GetType(string.Concat(this.currentMember.TypeName, "&")) };
			if (!(bool)type.GetMethod("TryParse", typeArray).Invoke(null, objArray))
			{
				this.@value = null;
				return;
			}
			this.@value = objArray[1].ToString();
		}
	}
}