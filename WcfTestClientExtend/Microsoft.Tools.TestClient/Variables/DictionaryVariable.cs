using Microsoft.Tools.TestClient;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.Tools.TestClient.Variables
{
	[Serializable]
	internal class DictionaryVariable : ContainerVariable
	{
		internal DictionaryVariable(TypeMemberInfo declaredMember) : base(declaredMember)
		{
		}

		internal static object CreateAndValidateDictionary(string typeName, Variable[] variables, out List<int> invalidList)
		{
			Type item = DataContractAnalyzer.TypesCache[typeName];
			object obj = Activator.CreateInstance(item);
			invalidList = new List<int>();
			if (variables != null)
			{
				MethodInfo method = item.GetMethod("Add");
				if (method == null)
				{
					return null;
				}
				int num = 0;
				Variable[] variableArray = variables;
				for (int i = 0; i < (int)variableArray.Length; i++)
				{
					KeyValuePairVariable keyValuePairVariable = (KeyValuePairVariable)variableArray[i];
					if (keyValuePairVariable != null && keyValuePairVariable.IsValid)
					{
						object[] objArray = new object[2];
						Variable[] childVariables = keyValuePairVariable.GetChildVariables();
						objArray[0] = childVariables[0].CreateObject();
						objArray[1] = childVariables[1].CreateObject();
						try
						{
							method.Invoke(obj, objArray);
						}
						catch (TargetInvocationException targetInvocationException)
						{
							invalidList.Add(num);
						}
						num++;
					}
				}
			}
			return obj;
		}

		internal override object CreateObject()
		{
			if (this.@value.Equals("(null)"))
			{
				return null;
			}
			base.GetChildVariables();
			List<int> nums = null;
			return DictionaryVariable.CreateAndValidateDictionary(this.currentMember.TypeName, this.childVariables, out nums);
		}

		private void Validate()
		{
			if (this.childVariables != null)
			{
				Variable[] variableArray = this.childVariables;
				for (int i = 0; i < (int)variableArray.Length; i++)
				{
					((KeyValuePairVariable)variableArray[i]).IsValid = true;
				}
				foreach (int num in ServiceExecutor.ValidateDictionary(this, this.serviceMethodInfo.Endpoint.ServiceProject.ClientDomain))
				{
					((KeyValuePairVariable)this.childVariables[num]).IsValid = false;
				}
			}
		}

		internal override void ValidateAndCanonicalize(string input)
		{
			base.ValidateAndCanonicalize(input);
			if (this.@value != null)
			{
				base.GetChildVariables();
				this.Validate();
			}
		}

		internal IList<int> ValidateDictionary()
		{
			List<int> nums = null;
			DictionaryVariable.CreateAndValidateDictionary(base.TypeName, this.childVariables, out nums);
			return nums;
		}
	}
}