using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.Threading;
using System.Xml;
using WcfTestClient;

namespace Microsoft.Tools.TestClient
{
	internal class ServiceAnalyzer
	{
		internal const string DefaultConfigName = "default.config";

		private static bool isErrorStreamClosed;

		private static string svcutilError;

		private static Type customBindingType;

		private static Type standardBindingType;

		private static TimeSpan defaultSvcInvocationTimeout;

		static ServiceAnalyzer()
		{
			ServiceAnalyzer.customBindingType = typeof(CustomBindingElement);
			ServiceAnalyzer.standardBindingType = typeof(StandardBindingElement);
			ServiceAnalyzer.defaultSvcInvocationTimeout = new TimeSpan(0, 5, 0);
		}

		public ServiceAnalyzer()
		{
		}

		internal static ServiceModelSectionGroup AnalyzeConfig(IDictionary<ChannelEndpointElement, ClientEndpointInfo> services, string configPath, ref string errorMessage)
		{
			ServiceModelSectionGroup serviceModelSectionGroup;
			try
			{
				ExeConfigurationFileMap exeConfigurationFileMap = new ExeConfigurationFileMap()
				{
					MachineConfigFilename = ConfigurationManager.OpenMachineConfiguration().FilePath,
					ExeConfigFilename = configPath
				};
				ServiceModelSectionGroup sectionGroup = ServiceModelSectionGroup.GetSectionGroup(ConfigurationManager.OpenMappedExeConfiguration(exeConfigurationFileMap, ConfigurationUserLevel.None));
				foreach (ChannelEndpointElement endpoint in sectionGroup.Client.Endpoints)
				{
					services.Add(endpoint, new ClientEndpointInfo(endpoint.Contract));
				}
				serviceModelSectionGroup = sectionGroup;
			}
			catch (ConfigurationErrorsException configurationErrorsException1)
			{
				ConfigurationErrorsException configurationErrorsException = configurationErrorsException1;
				errorMessage = string.Concat(errorMessage, configurationErrorsException.Message);
				serviceModelSectionGroup = null;
			}
			return serviceModelSectionGroup;
		}

		internal static AppDomain AnalyzeProxy(IDictionary<ChannelEndpointElement, ClientEndpointInfo> services, ServiceModelSectionGroup configObject, string configPath, string assemblyPath)
		{
			AppDomain appDomain = ServiceAnalyzer.CreateAppDomain(configPath, assemblyPath);
			DataContractAnalyzer dataContractAnalyzer = (DataContractAnalyzer)appDomain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(DataContractAnalyzer).FullName);
			foreach (ChannelEndpointElement endpoint in configObject.Client.Endpoints)
			{
				ClientEndpointInfo item = services[endpoint];
				item = dataContractAnalyzer.AnalyzeDataContract(item);
				if (item != null)
				{
					services[endpoint] = item;
				}
				else
				{
					services.Remove(endpoint);
				}
			}
			foreach (KeyValuePair<ChannelEndpointElement, ClientEndpointInfo> service in services)
			{
				service.Value.EndpointConfigurationName = service.Key.Name;
			}
			return appDomain;
		}

		private AppDomain AnalyzeProxy(IDictionary<ChannelEndpointElement, ClientEndpointInfo> services, string projectPath, string proxyPath, ServiceModelSectionGroup configObject, string configPath, ref string errorMessage, out string assemblyPath)
		{
			assemblyPath = null;
			CSharpCodeProvider cSharpCodeProvider = new CSharpCodeProvider();
			CompilerParameters compilerParameter = new CompilerParameters()
			{
				OutputAssembly = Path.Combine(projectPath, "Client.dll")
			};
			compilerParameter.ReferencedAssemblies.Add("System.dll");
			compilerParameter.ReferencedAssemblies.Add(typeof(DataSet).Assembly.Location);
			compilerParameter.ReferencedAssemblies.Add(typeof(TypedTableBaseExtensions).Assembly.Location);
			compilerParameter.ReferencedAssemblies.Add(typeof(XmlReader).Assembly.Location);
			compilerParameter.ReferencedAssemblies.Add(typeof(OperationDescription).Assembly.Location);
			compilerParameter.ReferencedAssemblies.Add(typeof(DataContractAttribute).Assembly.Location);
			compilerParameter.GenerateExecutable = false;
			compilerParameter.CompilerOptions = "/platform:x86";
			string[] strArrays = new string[] { proxyPath };
			CompilerResults compilerResult = cSharpCodeProvider.CompileAssemblyFromFile(compilerParameter, strArrays);
			if (compilerResult.Errors.Count == 0)
			{
				assemblyPath = compilerResult.PathToAssembly;
				return ServiceAnalyzer.AnalyzeProxy(services, configObject, configPath, assemblyPath);
			}
			if (errorMessage == null)
			{
				errorMessage = string.Empty;
			}
			foreach (CompilerError error in compilerResult.Errors)
			{
				errorMessage = string.Concat(errorMessage, error.ToString(), Environment.NewLine);
			}
			return null;
		}

		internal ServiceProject AnalyzeService(string address, BackgroundWorker addServiceWorker, float startProgress, float progressRange, out string errorMessage)
		{
			string str;
			errorMessage = string.Empty;
			IDictionary<ChannelEndpointElement, ClientEndpointInfo> channelEndpointElements = new Dictionary<ChannelEndpointElement, ClientEndpointInfo>();
			string projectBase = ApplicationSettings.GetInstance().ProjectBase;
			Guid guid = Guid.NewGuid();
			string str1 = Path.Combine(projectBase, guid.ToString());
			string str2 = Path.Combine(str1, "Client.dll.config");
			string str3 = Path.Combine(str1, "Client.cs");
			if (!ServiceAnalyzer.GenerateProxyAndConfig(str1, address, str2, str3, this.GetIntValueOfProgress(startProgress, progressRange, 0.1f), this.GetIntValueOfProgress(startProgress, progressRange, 0.5f), addServiceWorker, out errorMessage))
			{
				ServiceAnalyzer.DeleteProjectFolder(str1);
				return null;
			}
			ServiceModelSectionGroup serviceModelSectionGroup = ServiceAnalyzer.AnalyzeConfig(channelEndpointElements, str2, ref errorMessage);
			if (serviceModelSectionGroup == null || this.CancelOrReportProgress(addServiceWorker, null, this.GetIntValueOfProgress(startProgress, progressRange, 0.7f), str1))
			{
				return null;
			}
			AppDomain appDomain = this.AnalyzeProxy(channelEndpointElements, str1, str3, serviceModelSectionGroup, str2, ref errorMessage, out str);
			if (appDomain == null || this.CancelOrReportProgress(addServiceWorker, appDomain, this.GetIntValueOfProgress(startProgress, progressRange, 0.9f), str1))
			{
				return null;
			}
			return new ServiceProject(address, str1, str2, str, str3, channelEndpointElements.Values, appDomain);
		}

		private bool CancelOrReportProgress(BackgroundWorker addServiceWorker, AppDomain clientDomain, int percentProgress, string projectPath)
		{
			if (!addServiceWorker.CancellationPending)
			{
				addServiceWorker.ReportProgress(percentProgress);
				return false;
			}
			if (clientDomain != null)
			{
				ServiceAnalyzer.UnloadAppDomain(clientDomain);
			}
			ServiceAnalyzer.DeleteProjectFolder(projectPath);
			return true;
		}

		internal static void CopyConfigFile(string oldPath, string newPath)
		{
			try
			{
				File.Copy(oldPath, newPath, true);
			}
			catch (IOException oException)
			{
			}
			catch (UnauthorizedAccessException unauthorizedAccessException)
			{
			}
		}

		private static AppDomain CreateAppDomain(string configPath, string clientAssemblyPath)
		{
			AppDomainSetup appDomainSetup = new AppDomainSetup()
			{
				ConfigurationFile = configPath,
				ApplicationBase = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
			};
			AppDomain appDomain = AppDomain.CreateDomain(configPath, AppDomain.CurrentDomain.Evidence, appDomainSetup);
			appDomain.SetData("clientAssemblyPath", clientAssemblyPath);
			return appDomain;
		}

		internal static void DeleteProjectFolder(string projectDirectory)
		{
			try
			{
				Directory.Delete(projectDirectory, true);
			}
			catch (IOException oException)
			{
			}
			catch (UnauthorizedAccessException unauthorizedAccessException)
			{
			}
		}

		private static bool GenerateProxyAndConfig(string projectPath, string address, string configPath, string proxyPath, int startProgressPosition, int endProgressPostition, BackgroundWorker addServiceWorker, out string errorMessage)
		{
			int num;
			string metadataTool = ToolingEnvironment.MetadataTool;
			bool flag = true;
			string str = Path.Combine(projectPath, "default.config");
			if (!File.Exists(metadataTool))
			{
				CultureInfo currentUICulture = CultureInfo.CurrentUICulture;
				string errorMetadataToolNotFound = StringResources.ErrorMetadataToolNotFound;
				object[] objArray = new object[] { metadataTool };
				errorMessage = string.Format(currentUICulture, errorMetadataToolNotFound, objArray);
				flag = false;
				return flag;
			}
			int num1 = 0;
			do
			{
				Process process = ServiceAnalyzer.StartSvcutil(metadataTool, address, proxyPath, str);
				while (!process.HasExited)
				{
					if (addServiceWorker.CancellationPending)
					{
						if (!process.HasExited)
						{
							process.Kill();
							process.Dispose();
						}
						errorMessage = string.Empty;
						return false;
					}
					if (startProgressPosition < endProgressPostition)
					{
						int num2 = startProgressPosition;
						startProgressPosition = num2 + 1;
						addServiceWorker.ReportProgress(num2);
					}
					Thread.Sleep(50);
				}
				process.Dispose();
				if (!File.Exists(proxyPath) || !File.Exists(str))
				{
					flag = false;
					while (!ServiceAnalyzer.isErrorStreamClosed)
					{
						Thread.Sleep(50);
					}
				}
				else
				{
					flag = true;
				}
				errorMessage = ServiceAnalyzer.svcutilError;
				if (flag)
				{
					break;
				}
				num = num1;
				num1 = num + 1;
			}
			while (num < 1);
			if (flag)
			{
				if (ApplicationSettings.GetInstance().RegenerateConfigEnabled || !ConfigFileMappingManager.GetInstance().DoesConfigMappingExist(address))
				{
					ServiceAnalyzer.CopyConfigFile(str, configPath);
				}
				else
				{
					ServiceAnalyzer.CopyConfigFile(ConfigFileMappingManager.GetInstance().GetSavedConfigPath(address), configPath);
				}
				ServiceAnalyzer.SetServiceInvocationTimeout(configPath);
			}
			return flag;
		}

		private int GetIntValueOfProgress(float startProgress, float progressRange, float percent)
		{
			return (int)(startProgress + progressRange * percent);
		}

		private static void SetServiceInvocationTimeout(string configPath)
		{
			try
			{
				ExeConfigurationFileMap exeConfigurationFileMap = new ExeConfigurationFileMap()
				{
					MachineConfigFilename = ConfigurationManager.OpenMachineConfiguration().FilePath,
					ExeConfigFilename = configPath
				};
				Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(exeConfigurationFileMap, ConfigurationUserLevel.None);
				foreach (BindingCollectionElement bindingCollection in ServiceModelSectionGroup.GetSectionGroup(configuration).Bindings.BindingCollections)
				{
					foreach (IBindingConfigurationElement configuredBinding in bindingCollection.ConfiguredBindings)
					{
						if (configuredBinding.SendTimeout.CompareTo(ServiceAnalyzer.defaultSvcInvocationTimeout) >= 0)
						{
							continue;
						}
						Type type = configuredBinding.GetType();
						if (!ServiceAnalyzer.customBindingType.IsAssignableFrom(type))
						{
							if (!ServiceAnalyzer.standardBindingType.IsAssignableFrom(type))
							{
								continue;
							}
							(configuredBinding as StandardBindingElement).SendTimeout = ServiceAnalyzer.defaultSvcInvocationTimeout;
						}
						else
						{
							(configuredBinding as CustomBindingElement).SendTimeout = ServiceAnalyzer.defaultSvcInvocationTimeout;
						}
					}
				}
				configuration.Save();
			}
			catch (ConfigurationErrorsException configurationErrorsException)
			{
			}
		}

		private static Process StartSvcutil(string svcutilPath, string address, string proxyPath, string defaultConfigPath)
		{
			Process process = new Process();
			ProcessStartInfo processStartInfo = new ProcessStartInfo(svcutilPath)
			{
				UseShellExecute = false,
				RedirectStandardError = true,
				RedirectStandardOutput = true,
				CreateNoWindow = true
			};
			string[] location = new string[] { "/targetClientVersion:Version35 /r:\"", typeof(DataSet).Assembly.Location, "\" \"", address, "\" \"/out:", proxyPath, "\" \"/config:", defaultConfigPath, "\"" };
			processStartInfo.Arguments = string.Concat(location);
			if (svcutilPath.EndsWith("v8.0A\\bin\\NETFX 4.0 Tools\\SvcUtil.exe", StringComparison.OrdinalIgnoreCase))
			{
				processStartInfo.Arguments = string.Concat("/syncOnly ", processStartInfo.Arguments);
			}
			process.StartInfo = processStartInfo;
			process.ErrorDataReceived += new DataReceivedEventHandler(ServiceAnalyzer.Svcutil_ErrorDataReceived);
			ServiceAnalyzer.svcutilError = string.Empty;
			ServiceAnalyzer.isErrorStreamClosed = false;
			process.Start();
			process.BeginErrorReadLine();
			return process;
		}

		private static void Svcutil_ErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			if (e.Data == null)
			{
				ServiceAnalyzer.isErrorStreamClosed = true;
				return;
			}
			ServiceAnalyzer.svcutilError = string.Concat(ServiceAnalyzer.svcutilError, e.Data);
		}

		internal static void UnloadAppDomain(AppDomain clientDomain)
		{
			try
			{
				AppDomain.Unload(clientDomain);
			}
			catch (CannotUnloadAppDomainException cannotUnloadAppDomainException)
			{
			}
			clientDomain = null;
		}
	}
}