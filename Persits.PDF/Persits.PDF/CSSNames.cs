using System;
namespace Persits.PDF
{
	internal class CSSNames
	{
		public CssProperties m_nProp;
		public string m_szName;
		public CSSNames(CssProperties p, string n)
		{
			this.m_nProp = p;
			this.m_szName = n;
		}
	}
}
