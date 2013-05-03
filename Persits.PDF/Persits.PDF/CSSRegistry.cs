using System;
namespace Persits.PDF
{
	internal class CSSRegistry
	{
		public CssProperties m_nProp;
		public CssPropertyType m_nType;
		public CssProperties[] m_arrValues;
		public bool m_bInheritable;
		public CSSRegistry(CssProperties p, CssPropertyType t, CssProperties[] values, bool inher)
		{
			this.m_nProp = p;
			this.m_nType = t;
			this.m_arrValues = new CssProperties[values.Length];
			Array.Copy(values, this.m_arrValues, values.Length);
			this.m_bInheritable = inher;
		}
	}
}
