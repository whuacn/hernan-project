using System;
namespace Persits.PDF
{
	internal class RavenColorPtr
	{
		private byte[] m_pData;
		private int m_nPtr;
		internal byte this[int i]
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
		internal byte[] buffer
		{
			get
			{
				return this.m_pData;
			}
		}
		internal byte ptr
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
		internal RavenColorPtr()
		{
			this.m_pData = null;
			this.m_nPtr = 0;
		}
		internal RavenColorPtr(byte[] b, int nOffset)
		{
			this.Set(b, nOffset);
		}
		internal RavenColorPtr(RavenColorPtr p, int nOffset)
		{
			this.Set(p.m_pData, p.m_nPtr + nOffset);
		}
		internal RavenColorPtr(byte[] b)
		{
			this.Set(b, 0);
		}
		internal void Inc(int offset)
		{
			this.m_nPtr += offset;
		}
		public static RavenColorPtr operator ++(RavenColorPtr p)
		{
			p.m_nPtr++;
			return p;
		}

		internal void Set(byte[] b, int nOffset)
		{
			this.m_pData = b;
			this.m_nPtr = nOffset;
		}
	}
}
