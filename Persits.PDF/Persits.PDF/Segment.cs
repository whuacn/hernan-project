using System;
namespace Persits.PDF
{
	internal class Segment
	{
		public char m_cType;
		public int m_nStart;
		public int m_nEnd;
		public Segment(char type, int start, int end)
		{
			this.m_cType = type;
			this.m_nStart = start;
			this.m_nEnd = end;
		}
	}
}
