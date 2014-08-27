using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using WcfTestClient;

namespace Microsoft.Tools.TestClient
{
	internal class AddServiceExecutor
	{
		public AddServiceExecutor()
		{
		}

		internal AddServiceOutputs Execute(AddServiceInputs inputs, Workspace workspace, BackgroundWorker addServiceWorker)
		{
			Uri uri;
			string str;
			AddServiceOutputs addServiceOutput = new AddServiceOutputs();
			if (inputs != null && inputs.EndpointsCount > 0)
			{
				float single = 0f;
				float endpointsCount = 100f / (float)inputs.EndpointsCount;
				foreach (string endpoint in inputs.Endpoints)
				{
					if (!Uri.TryCreate(endpoint, UriKind.Absolute, out uri))
					{
						CultureInfo currentUICulture = CultureInfo.CurrentUICulture;
						string errorInvalidUrl = StringResources.ErrorInvalidUrl;
						object[] objArray = new object[] { endpoint };
						addServiceOutput.AddError(string.Format(currentUICulture, errorInvalidUrl, objArray));
					}
					else
					{
						ServiceProject serviceProject = workspace.AddServiceProject(endpoint, addServiceWorker, single, endpointsCount, out str);
						if (serviceProject != null)
						{
							addServiceOutput.AddServiceProject(serviceProject);
							ApplicationSettings.GetInstance().RecordUrl(uri.AbsoluteUri);
							addServiceOutput.IncrementSucceedCount();
						}
						else if (!string.IsNullOrEmpty(str))
						{
							addServiceOutput.AddError(str);
						}
					}
					single = single + endpointsCount;
				}
			}
			return addServiceOutput;
		}
	}
}