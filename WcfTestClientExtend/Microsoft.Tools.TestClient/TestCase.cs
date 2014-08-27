using Microsoft.Tools.TestClient.UI;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Microsoft.Tools.TestClient
{
	[Serializable]
	internal class TestCase
	{
		private ErrorItem error;

		private ServiceMethodInfo method;

		[NonSerialized]
		private Microsoft.Tools.TestClient.UI.ServicePage servicePage;

		internal ServiceMethodInfo Method
		{
			get
			{
				return this.method;
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

		internal TestCase(ServiceMethodInfo method)
		{
			this.method = method;
		}

		internal void Remove()
		{
			this.method.TestCases.Remove(this);
		}

		internal void SetError(ErrorItem errorItem)
		{
			if (this.onErrorReported != null)
			{
				this.onErrorReported(errorItem);
			}
			this.error = errorItem;
		}

		private event TestCase.ErrorHandler onErrorReported;

		internal event TestCase.ErrorHandler OnErrorReported
		{
			add
			{
				this.onErrorReported += value;
			}
			remove
			{
				this.onErrorReported -= value;
			}
		}

		internal delegate void ErrorHandler(ErrorItem errorItem);
	}
}