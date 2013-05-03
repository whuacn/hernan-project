using System;
namespace Persits.PDF
{
	internal class TagFlagStruct
	{
		public TagType m_nType;
		public uint m_dwFlag;
		public TagFlagStruct(TagType t, uint f)
		{
			this.m_nType = t;
			this.m_dwFlag = f;
		}
	}
}
