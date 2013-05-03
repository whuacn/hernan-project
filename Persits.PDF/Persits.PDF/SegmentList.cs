using System;
using System.Collections.Generic;
namespace Persits.PDF
{
	internal class SegmentList
	{
		public List<Segment> m_arrList = new List<Segment>();
		~SegmentList()
		{
			this.m_arrList.Clear();
			this.m_arrList = null;
		}
		public void add(char type, int start, int end)
		{
			Segment item = new Segment(type, start, end);
			this.m_arrList.Add(item);
		}
		public Segment get(int idx)
		{
			if (idx < 0 || idx >= this.m_arrList.Count)
			{
				return null;
			}
			return this.m_arrList[idx];
		}
		public void remove(int idx)
		{
			this.m_arrList.RemoveAt(idx);
		}
		public int size()
		{
			return this.m_arrList.Count;
		}
	}
}
