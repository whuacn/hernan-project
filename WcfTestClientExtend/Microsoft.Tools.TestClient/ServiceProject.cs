using Microsoft.Tools.TestClient.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.ServiceModel.Configuration;
using System.Windows.Forms;
using WcfTestClient;

namespace Microsoft.Tools.TestClient
{
	internal class ServiceProject
	{
		private const string CommandLineFormatString = "\"{0}\"";

		private string address;

		private string assemblyPath;

		private AppDomain clientDomain;

		private FileItem configFile;

		private ICollection<ClientEndpointInfo> endpoints;

		private FileStream fileStream;

		private string foldName;

		private bool isConfigChanged;

		private bool isWorking;

		private string projectDirectory;

		private FileItem proxyFile;

		private TreeNode serviceProjectNode;

		internal string Address
		{
			get
			{
				return this.address;
			}
		}

		internal AppDomain ClientDomain
		{
			get
			{
				return this.clientDomain;
			}
		}

		internal FileItem ConfigFile
		{
			get
			{
				return this.configFile;
			}
		}

		internal ICollection<ClientEndpointInfo> Endpoints
		{
			get
			{
				return this.endpoints;
			}
		}

		public bool IsConfigChanged
		{
			get
			{
				return this.isConfigChanged;
			}
			set
			{
				this.isConfigChanged = value;
			}
		}

		public bool IsWorking
		{
			get
			{
				return this.isWorking;
			}
			set
			{
				this.isWorking = value;
			}
		}

		internal FileItem ProxyFile
		{
			get
			{
				return this.proxyFile;
			}
		}

		internal List<FileItem> ReferencedFiles
		{
			get
			{
				return new List<FileItem>()
				{
					this.ConfigFile,
					this.ProxyFile
				};
			}
		}

		internal List<TestCase> ReferencedTestCases
		{
			get
			{
				List<TestCase> testCases = new List<TestCase>();
				foreach (ClientEndpointInfo endpoint in this.endpoints)
				{
					foreach (ServiceMethodInfo method in endpoint.Methods)
					{
						testCases.AddRange(method.TestCases);
					}
				}
				return testCases;
			}
		}

		public TreeNode ServiceProjectNode
		{
			get
			{
				return this.serviceProjectNode;
			}
			set
			{
				this.serviceProjectNode = value;
			}
		}

		internal ServiceProject(string address, string projectDirectory, string configPath, string assemblyPath, string proxyPath, ICollection<ClientEndpointInfo> endpoints, AppDomain clientDomain)
		{
			this.address = address;
			this.projectDirectory = projectDirectory;
			this.configFile = new FileItem(configPath);
			this.proxyFile = new FileItem(proxyPath);
			this.endpoints = endpoints;
			this.clientDomain = clientDomain;
			this.assemblyPath = assemblyPath;
			this.fileStream = File.Open(assemblyPath, FileMode.Open, FileAccess.Read, FileShare.Read);
			this.foldName = Path.GetFileName(this.projectDirectory);
			this.UpdateAndValidateEndpointsInfo();
			if (!ApplicationSettings.GetInstance().RegenerateConfigEnabled)
			{
				this.CreateProxiesForEndpoints();
			}
		}

		private List<ErrorItem> CreateProxiesForEndpoints()
		{
			ServiceExecutor serviceExecutor = (ServiceExecutor)this.clientDomain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(ServiceExecutor).FullName);
			List<ErrorItem> errorItems = new List<ErrorItem>();
			foreach (ClientEndpointInfo endpoint in this.endpoints)
			{
				if (!endpoint.Valid)
				{
					continue;
				}
				ServiceInvocationOutputs cache = serviceExecutor.ConstructClientToCache(endpoint.ProxyIdentifier, endpoint.ClientTypeName, endpoint.EndpointConfigurationName);
				if (cache == null)
				{
					continue;
				}
				endpoint.Valid = false;
				endpoint.InvalidReason = StringResources.EndpointError;
				errorItems.Add(new ErrorItem(StringResources.EndpointError, cache.ExceptionMessages[0], null));
			}
			return errorItems;
		}

		private void DeleteProjectFolder()
		{
			try
			{
				this.fileStream.Close();
				Directory.Delete(this.projectDirectory, true);
			}
			catch (IOException oException)
			{
			}
			catch (UnauthorizedAccessException unauthorizedAccessException)
			{
			}
		}

		private void DeleteProxiesForEndpoints()
		{
			ServiceExecutor serviceExecutor = (ServiceExecutor)this.clientDomain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(ServiceExecutor).FullName);
			foreach (ClientEndpointInfo endpoint in this.endpoints)
			{
				if (!endpoint.Valid)
				{
					continue;
				}
				serviceExecutor.DeleteClient(endpoint.ProxyIdentifier);
			}
		}

		internal bool RefreshConfig(out List<ErrorItem> errorList)
		{
			IDictionary<ChannelEndpointElement, ClientEndpointInfo> channelEndpointElements = new Dictionary<ChannelEndpointElement, ClientEndpointInfo>();
			string empty = string.Empty;
			ServiceModelSectionGroup serviceModelSectionGroup = ServiceAnalyzer.AnalyzeConfig(channelEndpointElements, this.configFile.FileName, ref empty);
			if (serviceModelSectionGroup == null)
			{
				errorList = new List<ErrorItem>()
				{
					new ErrorItem(StringResources.InvalidConfig, empty, null)
				};
				return false;
			}
			this.UnloadAppDomain();
			this.clientDomain = ServiceAnalyzer.AnalyzeProxy(channelEndpointElements, serviceModelSectionGroup, this.configFile.FileName, this.assemblyPath);
			this.endpoints = channelEndpointElements.Values;
			this.UpdateAndValidateEndpointsInfo();
			this.ConfigFile.FilePage.RefreshFile(this.ConfigFile.FileName);
			errorList = this.CreateProxiesForEndpoints();
			ConfigFileMappingManager instance = ConfigFileMappingManager.GetInstance();
			instance.AddConfigFileMapping(this.address);
			ServiceAnalyzer.CopyConfigFile(this.configFile.FileName, instance.GetSavedConfigPath(this.address));
			return true;
		}

		internal void Remove()
		{
			this.UnloadAppDomain();
			this.DeleteProjectFolder();
			if (ApplicationSettings.GetInstance().RegenerateConfigEnabled)
			{
				ConfigFileMappingManager.GetInstance().DeleteConfigFileMapping(this.address);
			}
		}

		internal void RestoreDefaultConfig(out string errorMessage)
		{
			errorMessage = null;
			string str = Path.Combine(this.projectDirectory, "default.config");
			if (!File.Exists(str))
			{
				errorMessage = StringResources.DefaultConfigNotFoundDetail;
			}
			ServiceAnalyzer.CopyConfigFile(str, this.configFile.FileName);
		}

		internal bool StartSvcConfigEditor(out string errorMessage)
		{
			bool flag;
			errorMessage = null;
			if (ToolingEnvironment.SvcConfigEditorPath == null)
			{
				errorMessage = StringResources.SvcConfigEditorNotFound;
				return false;
			}
			string svcConfigEditorPath = ToolingEnvironment.SvcConfigEditorPath;
			CultureInfo currentUICulture = CultureInfo.CurrentUICulture;
			object[] fileName = new object[] { this.configFile.FileName };
			ProcessStartInfo processStartInfo = new ProcessStartInfo(svcConfigEditorPath, string.Format(currentUICulture, "\"{0}\"", fileName))
			{
				UseShellExecute = false
			};
			try
			{
				Process.Start(processStartInfo);
				return true;
			}
			catch (Win32Exception win32Exception)
			{
				errorMessage = win32Exception.Message;
				flag = false;
			}
			return flag;
		}

		private void UnloadAppDomain()
		{
			this.DeleteProxiesForEndpoints();
			ServiceAnalyzer.UnloadAppDomain(this.clientDomain);
		}

		private void UpdateAndValidateEndpointsInfo()
		{
			ClientEndpointInfo clientEndpointInfo = null;
			foreach (ClientEndpointInfo endpoint in this.endpoints)
			{
				endpoint.ProxyIdentifier = string.Concat(this.foldName, endpoint.EndpointConfigurationName);
				if (endpoint.Methods.Count >= 1)
				{
					endpoint.ServiceProject = this;
				}
				else
				{
					CultureInfo currentUICulture = CultureInfo.CurrentUICulture;
					string invalidContract = StringResources.InvalidContract;
					object[] operationContractTypeName = new object[] { endpoint.OperationContractTypeName };
					endpoint.InvalidReason = string.Format(currentUICulture, invalidContract, operationContractTypeName);
					clientEndpointInfo = endpoint;
				}
			}
			if (clientEndpointInfo != null)
			{
				CultureInfo cultureInfo = CultureInfo.CurrentUICulture;
				object[] invalidReason = new object[] { clientEndpointInfo.InvalidReason, StringResources.InvalidContractNameAction };
				RtlAwareMessageBox.Show(string.Format(cultureInfo, "{0}\n{1}", invalidReason), StringResources.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, 0);
			}
		}
	}
}