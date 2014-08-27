using System;

namespace Microsoft.Tools.TestClient
{
	internal class ValidationResult
	{
		private bool refreshRequired;

		private bool valid;

		internal bool RefreshRequired
		{
			get
			{
				return this.refreshRequired;
			}
		}

		internal bool Valid
		{
			get
			{
				return this.valid;
			}
		}

		internal ValidationResult(bool valid, bool refreshRequired)
		{
			this.valid = valid;
			this.refreshRequired = refreshRequired;
		}
	}
}