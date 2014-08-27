using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Microsoft.Tools.TestClient
{
	internal class Workspace
	{
		private ICollection<ServiceProject> serviceProjects = new List<ServiceProject>();

		internal ICollection<ServiceProject> ServiceProjects
		{
			get
			{
				return this.serviceProjects;
			}
		}

		public Workspace()
		{
		}

		internal ServiceProject AddServiceProject(string endpoint, BackgroundWorker addServiceWorker, float startProgress, float progressRange, out string error)
		{
			ServiceAnalyzer serviceAnalyzer = new ServiceAnalyzer();
			ServiceProject serviceProject = serviceAnalyzer.AnalyzeService(endpoint, addServiceWorker, startProgress, progressRange, out error);
			if (serviceProject == null)
			{
				return null;
			}
			if (addServiceWorker.CancellationPending)
			{
				serviceProject.Remove();
				return null;
			}
			this.serviceProjects.Add(serviceProject);
			error = null;
			return serviceProject;
		}

		internal void Close()
		{
			foreach (ServiceProject serviceProject in this.serviceProjects)
			{
				serviceProject.Remove();
			}
			this.serviceProjects.Clear();
			if (ApplicationSettings.GetInstance().RegenerateConfigEnabled)
			{
				ConfigFileMappingManager.GetInstance().Clear();
			}
		}

		internal ServiceProject FindServiceProject(string configPath)
		{
			ServiceProject serviceProject;
			using (IEnumerator<ServiceProject> enumerator = this.serviceProjects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ServiceProject current = enumerator.Current;
					if (!string.Equals(configPath, current.ConfigFile.FileName, StringComparison.OrdinalIgnoreCase))
					{
						continue;
					}
					serviceProject = current;
					return serviceProject;
				}
				return null;
			}
			return serviceProject;
		}

		internal void Remove(ServiceProject serviceProject)
		{
			this.serviceProjects.Remove(serviceProject);
			serviceProject.Remove();
		}
	}
}