using System;
namespace Persits.PDF
{
	internal class TagStruct
	{
		public TagType m_nType;
		public string m_szName;
		public TagStruct(TagType t, string n)
		{
			this.m_nType = t;
			this.m_szName = n;
		}
	}
}
