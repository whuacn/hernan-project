using System;

namespace Microsoft.Tools.TestClient
{
	[Serializable]
	internal class ErrorItem
	{
		private string errorDetail;

		private string errorMessage;

		private Microsoft.Tools.TestClient.TestCase testCase;

		internal string ErrorDetail
		{
			get
			{
				return this.errorDetail;
			}
		}

		internal string ErrorMessage
		{
			get
			{
				return this.errorMessage;
			}
		}

		internal Microsoft.Tools.TestClient.TestCase TestCase
		{
			get
			{
				return this.testCase;
			}
		}

		internal ErrorItem(string errorMessage, string errorDetail, Microsoft.Tools.TestClient.TestCase testCase)
		{
			this.errorMessage = errorMessage;
			this.errorDetail = errorDetail;
			this.testCase = testCase;
		}
	}
}