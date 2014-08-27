using Microsoft.Tools.TestClient.UI;
using System;

namespace Microsoft.Tools.TestClient
{
	internal class FileItem
	{
		private string fileName;

		private Microsoft.Tools.TestClient.UI.FilePage filePage;

		internal string FileName
		{
			get
			{
				return this.fileName;
			}
		}

		internal Microsoft.Tools.TestClient.UI.FilePage FilePage
		{
			get
			{
				if (this.filePage == null)
				{
					this.filePage = new Microsoft.Tools.TestClient.UI.FilePage(this.fileName);
				}
				return this.filePage;
			}
		}

		internal FileItem(string fileName)
		{
			this.fileName = fileName;
		}
	}
}