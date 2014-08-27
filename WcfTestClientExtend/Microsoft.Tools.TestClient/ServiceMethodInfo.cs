using Microsoft.Tools.TestClient.Variables;
using System;
using System.Collections.Generic;

namespace Microsoft.Tools.TestClient
{
	[Serializable]
	internal class ServiceMethodInfo
	{
		private ClientEndpointInfo endpoint;

		private List<TypeMemberInfo> invalidParameters;

		private bool isOneWay;

		private string methodName;

		private List<TypeMemberInfo> otherParameters = new List<TypeMemberInfo>();

		private List<TypeMemberInfo> parameters = new List<TypeMemberInfo>();

		private List<TestCase> testCases = new List<TestCase>();

		internal ClientEndpointInfo Endpoint
		{
			get
			{
				return this.endpoint;
			}
		}

		internal IList<TypeMemberInfo> InputParameters
		{
			get
			{
				return this.parameters;
			}
		}

		internal List<TypeMemberInfo> InvalidMembers
		{
			get
			{
				if (this.invalidParameters == null)
				{
					this.invalidParameters = new List<TypeMemberInfo>();
					this.parameters.Find(new Predicate<TypeMemberInfo>(this.CheckAndSaveInvalidMembers));
					this.otherParameters.Find(new Predicate<TypeMemberInfo>(this.CheckAndSaveInvalidMembers));
				}
				return this.invalidParameters;
			}
		}

		internal bool IsOneWay
		{
			get
			{
				return this.isOneWay;
			}
		}

		internal string MethodName
		{
			get
			{
				return this.methodName;
			}
		}

		internal IList<TypeMemberInfo> OtherParameters
		{
			get
			{
				return this.otherParameters;
			}
		}

		internal IList<TestCase> TestCases
		{
			get
			{
				return this.testCases;
			}
		}

		internal bool Valid
		{
			get
			{
				return this.InvalidMembers.Count == 0;
			}
		}

		internal ServiceMethodInfo(ClientEndpointInfo endpoint, string methodName, bool isOneWay)
		{
			this.endpoint = endpoint;
			this.methodName = methodName;
			this.isOneWay = isOneWay;
		}

		private bool CheckAndSaveInvalidMembers(TypeMemberInfo member)
		{
			if (member.IsInvalid)
			{
				this.invalidParameters.Add(member);
			}
			return false;
		}

		internal TestCase CreateTestCase()
		{
			TestCase testCase = new TestCase(this);
			this.testCases.Add(testCase);
			return testCase;
		}

		internal Variable[] GetVariables()
		{
			Variable[] variableArray = new Variable[this.parameters.Count];
			int num = 0;
			foreach (TypeMemberInfo parameter in this.parameters)
			{
				variableArray[num] = VariableFactory.CreateAssociateVariable(parameter);
				variableArray[num].SetServiceMethodInfo(this);
				string[] selectionList = variableArray[num].GetSelectionList();
				if (selectionList != null && (int)selectionList.Length == 2 && selectionList[0] == "(null)")
				{
					variableArray[num].SetValue(selectionList[1]);
				}
				num++;
			}
			return variableArray;
		}
	}
}