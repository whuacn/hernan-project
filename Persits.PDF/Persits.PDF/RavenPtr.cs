using System;
namespace Persits.PDF
{
	internal class RavenPtr<BaseType>
	{
		private BaseType[] m_pData;
		private int m_nPtr;
		internal BaseType this[int i]
		{
			get
			{
				return this.m_pData[this.m_nPtr + i];
			}
			set
			{
				this.m_pData[this.m_nPtr + i] = value;
			}
		}
		internal BaseType[] buffer
		{
			get
			{
				return this.m_pData;
			}
		}
		internal BaseType ptr
		{
			get
			{
				return this.m_pData[this.m_nPtr];
			}
			set
			{
				this.m_pData[this.m_nPtr] = value;
			}
		}
		internal int offset
		{
			get
			{
				return this.m_nPtr;
			}
		}
		internal RavenPtr()
		{
			this.m_pData = null;
			this.m_nPtr = 0;
		}
		internal RavenPtr(BaseType[] b, int nOffset)
		{
			this.Set(b, nOffset);
		}
		internal RavenPtr(RavenPtr<BaseType> p, int nOffset)
		{
			this.Set(p.m_pData, p.m_nPtr + nOffset);
		}
		internal RavenPtr(BaseType[] b)
		{
			this.Set(b, 0);
		}
		internal void Inc(uint offset)
		{
			this.Inc((int)offset);
		}
		internal void Inc(int offset)
		{
			this.m_nPtr += offset;
		}
		internal void Set(BaseType[] b, int nOffset)
		{
			this.m_pData = b;
			this.m_nPtr = nOffset;
		}
		public static RavenPtr<BaseType>operator ++(RavenPtr<BaseType> p)
		{
			p.m_nPtr++;
			return p;
		}
	}
}
