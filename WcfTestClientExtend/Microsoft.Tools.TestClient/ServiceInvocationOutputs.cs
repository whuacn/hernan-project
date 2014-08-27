using Microsoft.Tools.TestClient.UI;
using Microsoft.Tools.TestClient.Variables;
using System;

namespace Microsoft.Tools.TestClient
{
	[Serializable]
	internal class ServiceInvocationOutputs
	{
		private string[] exceptionMessages;

		private string[] exceptionStacks;

		private Microsoft.Tools.TestClient.ExceptionType exceptionType;

		private string responseXml;

		private Variable[] serviceInvocationResult;

		[NonSerialized]
		private Microsoft.Tools.TestClient.UI.ServicePage servicePage;

		internal string[] ExceptionMessages
		{
			get
			{
				return this.exceptionMessages;
			}
		}

		internal string[] ExceptionStacks
		{
			get
			{
				return this.exceptionStacks;
			}
		}

		internal Microsoft.Tools.TestClient.ExceptionType ExceptionType
		{
			get
			{
				return this.exceptionType;
			}
		}

		internal string ResponseXml
		{
			get
			{
				return this.responseXml;
			}
		}

		internal Microsoft.Tools.TestClient.UI.ServicePage ServicePage
		{
			get
			{
				return this.servicePage;
			}
			set
			{
				this.servicePage = value;
			}
		}

		internal ServiceInvocationOutputs(Variable[] serviceInvocationResult, string responseXml)
		{
			this.serviceInvocationResult = serviceInvocationResult;
			this.responseXml = responseXml;
		}

		internal ServiceInvocationOutputs(Microsoft.Tools.TestClient.ExceptionType exceptionType, string[] exceptionMessages, string[] exceptionStacks, string responseXml)
		{
			this.exceptionType = exceptionType;
			this.exceptionMessages = exceptionMessages;
			this.exceptionStacks = exceptionStacks;
			this.responseXml = responseXml;
		}

		internal Variable[] GetServiceInvocationResult()
		{
			return this.serviceInvocationResult;
		}
	}
}