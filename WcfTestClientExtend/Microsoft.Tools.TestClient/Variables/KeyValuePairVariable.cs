using Microsoft.Tools.TestClient;
using System;
using System.Collections.Generic;

namespace Microsoft.Tools.TestClient.Variables
{
	[Serializable]
	internal class KeyValuePairVariable : Variable
	{
		private const string duplicateKeyMark = "[ # ]";

		private bool isValid = true;

		internal bool IsValid
		{
			get
			{
				return this.isValid;
			}
			set
			{
				this.isValid = value;
			}
		}

		internal override string Name
		{
			get
			{
				if (!this.isValid)
				{
					return "[ # ]";
				}
				return base.Name;
			}
			set
			{
				base.Name = value;
			}
		}

		internal KeyValuePairVariable(TypeMemberInfo declaredMember) : base(declaredMember)
		{
		}

		internal override object CreateObject()
		{
			base.GetChildVariables();
			Type item = DataContractAnalyzer.TypesCache[this.currentMember.TypeName];
			object[] objArray = new object[] { this.childVariables[0].CreateObject(), this.childVariables[1].CreateObject() };
			return Activator.CreateInstance(item, objArray);
		}
	}
}