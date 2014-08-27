using System;
using System.Collections.Generic;

namespace Microsoft.Tools.TestClient
{
	internal class AddServiceInputs
	{
		private IEnumerable<string> endpoints;

		private int endpointsCount;

		internal IEnumerable<string> Endpoints
		{
			get
			{
				return this.endpoints;
			}
		}

		public int EndpointsCount
		{
			get
			{
				return this.endpointsCount;
			}
		}

		internal AddServiceInputs(params string[] endpoints)
		{
			if (endpoints != null)
			{
				this.endpointsCount = (int)endpoints.Length;
			}
			this.endpoints = endpoints;
		}
	}
}