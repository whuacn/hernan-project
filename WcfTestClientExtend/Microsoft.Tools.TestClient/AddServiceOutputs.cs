using System;
using System.Collections.Generic;
using System.Globalization;
using WcfTestClient;

namespace Microsoft.Tools.TestClient
{
	internal class AddServiceOutputs
	{
		internal static bool IsRefreshing;

		private List<ErrorItem> addServiceErrors = new List<ErrorItem>();

		private bool cancelled;

		private List<ServiceProject> serviceProjects = new List<ServiceProject>();

		private int succeedCount;

		public bool Cancelled
		{
			get
			{
				return this.cancelled;
			}
			set
			{
				this.cancelled = value;
			}
		}

		internal List<ErrorItem> Errors
		{
			get
			{
				return this.addServiceErrors;
			}
		}

		internal List<ServiceProject> ServiceProjects
		{
			get
			{
				return this.serviceProjects;
			}
		}

		static AddServiceOutputs()
		{
		}

		public AddServiceOutputs()
		{
		}

		internal void AddError(string errorMessage)
		{
			this.addServiceErrors.Add(new ErrorItem(StringResources.ErrorFailedAddingService, errorMessage, null));
		}

		internal void AddServiceProject(ServiceProject serviceProject)
		{
			this.serviceProjects.Add(serviceProject);
		}

		internal string GetStatusMessage()
		{
			if (this.cancelled)
			{
				if (!AddServiceOutputs.IsRefreshing)
				{
					return StringResources.StatusAddServiceCancelled;
				}
				return StringResources.StatusRefreshServiceCancelled;
			}
			int count = this.addServiceErrors.Count;
			int num = this.addServiceErrors.Count + this.succeedCount;
			if (this.addServiceErrors.Count == 0)
			{
				if (!AddServiceOutputs.IsRefreshing)
				{
					return StringResources.StatusAddingServiceCompleted;
				}
				return StringResources.StatusRefreshingServiceCompleted;
			}
			if (count == 1 && num == 1)
			{
				if (!AddServiceOutputs.IsRefreshing)
				{
					return StringResources.ErrorFailedAddingService;
				}
				return StringResources.ErrorFailedRefreshingService;
			}
			CultureInfo currentCulture = CultureInfo.CurrentCulture;
			string statusAddingMultipleServicesFailed = StringResources.StatusAddingMultipleServicesFailed;
			object[] objArray = new object[] { count, num };
			return string.Format(currentCulture, statusAddingMultipleServicesFailed, objArray);
		}

		internal void IncrementSucceedCount()
		{
			AddServiceOutputs addServiceOutput = this;
			addServiceOutput.succeedCount = addServiceOutput.succeedCount + 1;
		}
	}
}