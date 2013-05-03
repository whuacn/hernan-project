using System;
namespace Persits.PDF
{
	internal class InputStruct
	{
		public InputTypeAttribute m_nType;
		public string m_szName;
		public InputStruct(InputTypeAttribute t, string n)
		{
			this.m_nType = t;
			this.m_szName = n;
		}
	}
}
