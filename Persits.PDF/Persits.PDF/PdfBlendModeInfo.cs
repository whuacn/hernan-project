using System;
namespace Persits.PDF
{
	internal class PdfBlendModeInfo
	{
		internal string name;
		internal enumBlendMode mode;
		internal PdfBlendModeInfo(string n, enumBlendMode m)
		{
			this.name = n;
			this.mode = m;
		}
	}
}
