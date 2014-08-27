using Microsoft.Tools.TestClient.UI;
using Microsoft.Tools.TestClient.Variables;
using System;

namespace Microsoft.Tools.TestClient
{
	[Serializable]
	internal class ServiceInvocationInputs
	{
		private string clientTypeName;

		private string contractTypeName;

		[NonSerialized]
		private AppDomain domain;

		private string endpointConfigurationName;

		private Variable[] inputs;

		private string methodName;

		private string proxyIdentifier;

		[NonSerialized]
		private Microsoft.Tools.TestClient.UI.ServicePage servicePage;

		private bool startNewClient;

		internal string ClientTypeName
		{
			get
			{
				return this.clientTypeName;
			}
		}

		internal string ContractTypeName
		{
			get
			{
				return this.contractTypeName;
			}
		}

		internal AppDomain Domain
		{
			get
			{
				return this.domain;
			}
		}

		internal string EndpointConfigurationName
		{
			get
			{
				return this.endpointConfigurationName;
			}
		}

		internal string MethodName
		{
			get
			{
				return this.methodName;
			}
		}

		internal string ProxyIdentifier
		{
			get
			{
				return this.proxyIdentifier;
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

		internal bool StartNewClient
		{
			get
			{
				return this.startNewClient;
			}
		}

		internal ServiceInvocationInputs(Variable[] inputs, Microsoft.Tools.TestClient.UI.ServicePage servicePage, bool startNewClient) : this(inputs, servicePage.TestCase, startNewClient)
		{
			this.servicePage = servicePage;
		}

		internal ServiceInvocationInputs(Variable[] inputs, TestCase testCase, bool startNewClient)
		{
			ServiceMethodInfo method = testCase.Method;
			ClientEndpointInfo endpoint = method.Endpoint;
			ServiceProject serviceProject = endpoint.ServiceProject;
			this.clientTypeName = endpoint.ClientTypeName;
			this.contractTypeName = endpoint.OperationContractTypeName;
			this.endpointConfigurationName = endpoint.EndpointConfigurationName;
			this.proxyIdentifier = endpoint.ProxyIdentifier;
			this.methodName = method.MethodName;
			this.inputs = inputs;
			this.startNewClient = startNewClient;
			if (serviceProject != null)
			{
				this.domain = serviceProject.ClientDomain;
			}
		}

		internal Variable[] GetInputs()
		{
			return this.inputs;
		}
	}
}