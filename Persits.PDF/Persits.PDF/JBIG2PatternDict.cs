using System;
namespace Persits.PDF
{
	internal class JBIG2PatternDict : JBIG2Segment
	{
		private uint size;
		private JBIG2Bitmap[] bitmaps;
		internal JBIG2PatternDict(uint segNumA, uint sizeA) : base(segNumA)
		{
			this.size = sizeA;
			this.bitmaps = new JBIG2Bitmap[this.size];
		}
        ~JBIG2PatternDict()
        {

            for (uint num = 0u; num < this.size; num += 1u)
            {
                this.bitmaps[(int)((UIntPtr)num)] = null;
            }
            this.bitmaps = null;

        }
		internal override JBIG2SegmentType getType()
		{
			return JBIG2SegmentType.jbig2SegPatternDict;
		}
		internal uint getSize()
		{
			return this.size;
		}
		internal void setBitmap(uint idx, JBIG2Bitmap bitmap)
		{
			this.bitmaps[(int)((UIntPtr)idx)] = bitmap;
		}
		internal JBIG2Bitmap getBitmap(uint idx)
		{
			return this.bitmaps[(int)((UIntPtr)idx)];
		}
	}
}
