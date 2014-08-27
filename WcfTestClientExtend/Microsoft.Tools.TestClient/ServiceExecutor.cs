using Microsoft.Tools.TestClient.Variables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Microsoft.Tools.TestClient
{
	internal class ServiceExecutor : MarshalByRefObject
	{
		private static AsyncCallback closeClientCallBack;

		private static IDictionary<string, object> cachedProxies;

		private static object lockObject;

		private bool extractingXML;

		static ServiceExecutor()
		{
			ServiceExecutor.closeClientCallBack = new AsyncCallback(ServiceExecutor.CloseClientCallBack);
			ServiceExecutor.cachedProxies = new Dictionary<string, object>();
			ServiceExecutor.lockObject = new object();
		}

		public ServiceExecutor()
		{
		}

		private static IDictionary<string, object> BuildParameters(Variable[] inputs)
		{
			IDictionary<string, object> strs = new Dictionary<string, object>();
			Variable[] variableArray = inputs;
			for (int i = 0; i < (int)variableArray.Length; i++)
			{
				Variable variable = variableArray[i];
				object obj = variable.CreateObject();
				strs.Add(variable.Name, obj);
			}
			return strs;
		}

		private static void CloseClient(ICommunicationObject client)
		{
			try
			{
				IAsyncResult asyncResult = client.BeginClose(ServiceExecutor.closeClientCallBack, client);
				if (asyncResult.CompletedSynchronously)
				{
					ServiceExecutor.ProcessClientClose(client, asyncResult);
				}
			}
			catch (TimeoutException timeoutException)
			{
				client.Abort();
			}
			catch (CommunicationException communicationException)
			{
				client.Abort();
			}
		}

		private static void CloseClientCallBack(IAsyncResult result)
		{
			if (!result.CompletedSynchronously)
			{
				ServiceExecutor.ProcessClientClose((ICommunicationObject)result.AsyncState, result);
			}
		}

		private static ServiceInvocationOutputs ConstructClient(string clientTypeName, string endpointConfigurationName, out object client)
		{
			ServiceInvocationOutputs serviceInvocationOutput;
			Type type = ClientSettings.ClientAssembly.GetType(clientTypeName);
			try
			{
				if (endpointConfigurationName != null)
				{
					ConstructorInfo constructor = type.GetConstructor(new Type[] { typeof(string) });
					object[] objArray = new object[] { endpointConfigurationName };
					client = constructor.Invoke(objArray);
				}
				else
				{
					client = type.GetConstructor(Type.EmptyTypes).Invoke(null);
				}
				return null;
			}
			catch (TargetInvocationException targetInvocationException1)
			{
				TargetInvocationException targetInvocationException = targetInvocationException1;
				if (ExceptionUtility.IsFatal(targetInvocationException))
				{
					throw;
				}
				client = null;
				string[] message = new string[] { targetInvocationException.InnerException.Message };
				string[] stackTrace = new string[] { targetInvocationException.InnerException.StackTrace };
				serviceInvocationOutput = new ServiceInvocationOutputs(ExceptionType.ProxyConstructFail, message, stackTrace, null);
			}
			return serviceInvocationOutput;
		}

		internal ServiceInvocationOutputs ConstructClientToCache(string proxyIdentifier, string clientTypeName, string endpointConfigurationName)
		{
			object obj;
			ServiceInvocationOutputs serviceInvocationOutput = ServiceExecutor.ConstructClient(clientTypeName, endpointConfigurationName, out obj);
			if (serviceInvocationOutput != null)
			{
				return serviceInvocationOutput;
			}
			if (ServiceExecutor.cachedProxies.ContainsKey(proxyIdentifier))
			{
				this.DeleteClient(proxyIdentifier);
			}
			ServiceExecutor.cachedProxies.Add(proxyIdentifier, obj);
			return null;
		}

		internal void DeleteClient(string proxyIdentifier)
		{
			if (ServiceExecutor.cachedProxies.ContainsKey(proxyIdentifier))
			{
				ServiceExecutor.CloseClient((ICommunicationObject)ServiceExecutor.cachedProxies[proxyIdentifier]);
				ServiceExecutor.cachedProxies.Remove(proxyIdentifier);
			}
		}

		private ServiceInvocationOutputs Execute(ServiceInvocationInputs inputValues)
		{
			MethodInfo methodInfo;
			ParameterInfo[] parameterInfoArray;
			object[] objArray;
			ServiceExecutor.ResponseXmlInterceptingBehavior responseXmlInterceptingBehavior;
			string[] strArrays;
			string[] strArrays1;
			ServiceInvocationOutputs serviceInvocationOutput;
			lock (ServiceExecutor.lockObject)
			{
				string clientTypeName = inputValues.ClientTypeName;
				string contractTypeName = inputValues.ContractTypeName;
				string endpointConfigurationName = inputValues.EndpointConfigurationName;
				string methodName = inputValues.MethodName;
				Variable[] inputs = inputValues.GetInputs();
				Type type = ClientSettings.ClientAssembly.GetType(contractTypeName);
				try
				{
					ServiceExecutor.PopulateInputParameters(methodName, inputs, type, out methodInfo, out parameterInfoArray, out objArray);
				}
				catch (TargetInvocationException targetInvocationException)
				{
					string[] message = new string[] { targetInvocationException.InnerException.Message };
					serviceInvocationOutput = new ServiceInvocationOutputs(ExceptionType.InvalidInput, message, null, null);
					return serviceInvocationOutput;
				}
				if (inputValues.StartNewClient || !ServiceExecutor.cachedProxies.ContainsKey(inputValues.ProxyIdentifier))
				{
					ServiceInvocationOutputs cache = this.ConstructClientToCache(inputValues.ProxyIdentifier, clientTypeName, endpointConfigurationName);
					if (cache != null)
					{
						serviceInvocationOutput = cache;
						return serviceInvocationOutput;
					}
				}
				object item = ServiceExecutor.cachedProxies[inputValues.ProxyIdentifier];
				PropertyInfo property = item.GetType().BaseType.GetProperty("Endpoint");
				ServiceEndpoint value = (ServiceEndpoint)property.GetValue(item, null);
				if (value.Behaviors.Contains(typeof(ServiceExecutor.ResponseXmlInterceptingBehavior)))
				{
					responseXmlInterceptingBehavior = value.Behaviors.Find<ServiceExecutor.ResponseXmlInterceptingBehavior>();
					responseXmlInterceptingBehavior.SetExtractingXML(this.extractingXML);
				}
				else
				{
					responseXmlInterceptingBehavior = new ServiceExecutor.ResponseXmlInterceptingBehavior(this.extractingXML);
					value.Behaviors.Add(responseXmlInterceptingBehavior);
				}
				object nullObject = null;
				try
				{
					nullObject = methodInfo.Invoke(item, objArray);
				}
				catch (TargetInvocationException targetInvocationException1)
				{
					Exception innerException = targetInvocationException1.InnerException;
					if (ExceptionUtility.IsFatal(innerException))
					{
						throw;
					}
					ServiceExecutor.ExtractExceptionInfos(innerException, out strArrays, out strArrays1);
					serviceInvocationOutput = new ServiceInvocationOutputs(ExceptionType.InvokeFail, strArrays, strArrays1, responseXmlInterceptingBehavior.InterceptedXml);
					return serviceInvocationOutput;
				}
				IDictionary<string, object> strs = new Dictionary<string, object>();
				int num = 0;
				ParameterInfo[] parameterInfoArray1 = parameterInfoArray;
				for (int i = 0; i < (int)parameterInfoArray1.Length; i++)
				{
					ParameterInfo parameterInfo = parameterInfoArray1[i];
					if (parameterInfo.ParameterType.IsByRef)
					{
						object obj = objArray[num];
						if (obj == null)
						{
							obj = new NullObject();
						}
						strs.Add(parameterInfo.Name, obj);
					}
					num++;
				}
				if (nullObject == null)
				{
					nullObject = new NullObject();
				}
				serviceInvocationOutput = new ServiceInvocationOutputs(DataContractAnalyzer.BuildVariableInfos(nullObject, strs), responseXmlInterceptingBehavior.InterceptedXml);
			}
			return serviceInvocationOutput;
		}

		internal static ServiceInvocationOutputs ExecuteInClientDomain(ServiceInvocationInputs inputs)
		{
			ServiceExecutor serviceExecutor = (ServiceExecutor)inputs.Domain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(ServiceExecutor).FullName);
			return serviceExecutor.Execute(inputs);
		}

		internal static void ExtractExceptionInfos(Exception e, out string[] messages, out string[] stackTraces)
		{
			Exception i;
			int num = 0;
			for (i = e; i != null; i = i.InnerException)
			{
				num++;
			}
			messages = new string[num];
			stackTraces = new string[num];
			int num1 = 0;
			i = e;
			while (i != null)
			{
				messages[num1] = i.Message;
				stackTraces[num1] = i.StackTrace;
				i = i.InnerException;
				num1++;
			}
		}

		private static void PopulateInputParameters(string methodName, Variable[] inputs, Type contractType, out MethodInfo method, out ParameterInfo[] parameters, out object[] parameterArray)
		{
			method = contractType.GetMethod(methodName);
			parameters = method.GetParameters();
			parameterArray = new object[(int)parameters.Length];
			IDictionary<string, object> strs = ServiceExecutor.BuildParameters(inputs);
			int num = 0;
			ParameterInfo[] parameterInfoArray = parameters;
			for (int i = 0; i < (int)parameterInfoArray.Length; i++)
			{
				ParameterInfo parameterInfo = parameterInfoArray[i];
				if (parameterInfo.IsIn || !parameterInfo.IsOut)
				{
					parameterArray[num] = strs[parameterInfo.Name];
				}
				num++;
			}
		}

		private static void ProcessClientClose(ICommunicationObject client, IAsyncResult result)
		{
			try
			{
				client.EndClose(result);
			}
			catch (TimeoutException timeoutException)
			{
				client.Abort();
			}
			catch (CommunicationException communicationException)
			{
				client.Abort();
			}
		}

		private string TranslateToXml(TestCase testCase, Variable[] inputs)
		{
			this.extractingXML = true;
			ServiceInvocationOutputs serviceInvocationOutput = this.Execute(new ServiceInvocationInputs(inputs, testCase, false));
			return serviceInvocationOutput.ExceptionMessages[0];
		}

		internal static string TranslateToXmlInClientDomain(TestCase testCase, Variable[] inputs)
		{
			AppDomain clientDomain = testCase.Method.Endpoint.ServiceProject.ClientDomain;
			ServiceExecutor serviceExecutor = (ServiceExecutor)clientDomain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(ServiceExecutor).FullName);
			return serviceExecutor.TranslateToXml(testCase, inputs);
		}

		internal static IList<int> ValidateDictionary(DictionaryVariable variable, AppDomain domain)
		{
			ServiceExecutor serviceExecutor = (ServiceExecutor)domain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(ServiceExecutor).FullName);
			return serviceExecutor.ValidateDictionary(variable);
		}

		private IList<int> ValidateDictionary(DictionaryVariable variable)
		{
			return variable.ValidateDictionary();
		}

		private class ResponseXmlInterceptingBehavior : IEndpointBehavior
		{
			private ServiceExecutor.ResponseXmlInterceptingBehavior.ResponseXmlInterceptingInspector responseXmlInterceptor;

			public string InterceptedXml
			{
				get
				{
					return this.responseXmlInterceptor.InterceptedXml;
				}
			}

			public ResponseXmlInterceptingBehavior(bool extractingXML)
			{
				this.responseXmlInterceptor = new ServiceExecutor.ResponseXmlInterceptingBehavior.ResponseXmlInterceptingInspector(extractingXML);
			}

			public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
			{
			}

			public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
			{
				clientRuntime.MessageInspectors.Add(this.responseXmlInterceptor);
			}

			public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
			{
			}

			public void SetExtractingXML(bool value)
			{
				this.responseXmlInterceptor.SetExtractingXML(value);
			}

			public void Validate(ServiceEndpoint endpoint)
			{
			}

			private class ResponseXmlInterceptingInspector : IClientMessageInspector
			{
				private bool extractingXML;

				private string interceptedXml;

				public string InterceptedXml
				{
					get
					{
						return this.interceptedXml;
					}
				}

				public ResponseXmlInterceptingInspector(bool extractingXML)
				{
					this.extractingXML = extractingXML;
				}

				public void AfterReceiveReply(ref Message reply, object correlationState)
				{
					if (reply != null)
					{
						this.interceptedXml = reply.ToString();
					}
				}

				public object BeforeSendRequest(ref Message request, IClientChannel channel)
				{
					if (this.extractingXML)
					{
						throw (new ExceptionUtility()).ThrowHelperError(new ServiceExecutor.StopInvocationException(request.ToString()));
					}
					return null;
				}

				public void SetExtractingXML(bool value)
				{
					this.extractingXML = value;
				}
			}
		}

		[Serializable]
		private class StopInvocationException : Exception
		{
			internal StopInvocationException(string message) : base(message)
			{
			}

			protected StopInvocationException(SerializationInfo info, StreamingContext context) : base(info, context)
			{
			}
		}
	}
}