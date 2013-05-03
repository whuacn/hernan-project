using System;
namespace Persits.PDF
{
	internal abstract class JBIG2Segment
	{
		private uint segNum;
		internal JBIG2Segment(uint segNumA)
		{
			this.segNum = segNumA;
		}
		internal void setSegNum(uint segNumA)
		{
			this.segNum = segNumA;
		}
		internal uint getSegNum()
		{
			return this.segNum;
		}
		internal abstract JBIG2SegmentType getType();
	}
}
