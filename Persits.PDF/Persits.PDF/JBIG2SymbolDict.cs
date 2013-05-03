using System;
namespace Persits.PDF
{
	internal class JBIG2SymbolDict : JBIG2Segment
	{
		internal uint size;
		private JBIG2Bitmap[] bitmaps;
		private JArithmeticDecoderStats genericRegionStats;
		private JArithmeticDecoderStats refinementRegionStats;
		internal JBIG2SymbolDict(uint segNumA, uint sizeA) : base(segNumA)
		{
			this.size = sizeA;
			this.bitmaps = new JBIG2Bitmap[this.size];
			for (uint num = 0u; num < this.size; num += 1u)
			{
				this.bitmaps[(int)((UIntPtr)num)] = null;
			}
			this.genericRegionStats = null;
			this.refinementRegionStats = null;
		}
        ~JBIG2SymbolDict()
        {

            for (uint num = 0u; num < this.size; num += 1u)
            {
                if (this.bitmaps[(int)((UIntPtr)num)] != null)
                {
                    this.bitmaps[(int)((UIntPtr)num)] = null;
                }
            }
            this.bitmaps = null;
            if (this.genericRegionStats != null)
            {
                this.genericRegionStats = null;
            }
            if (this.refinementRegionStats != null)
            {
                this.refinementRegionStats = null;
            }
        }
		internal override JBIG2SegmentType getType()
		{
			return JBIG2SegmentType.jbig2SegSymbolDict;
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
		internal void setGenericRegionStats(JArithmeticDecoderStats stats)
		{
			this.genericRegionStats = stats;
		}
		internal void setRefinementRegionStats(JArithmeticDecoderStats stats)
		{
			this.refinementRegionStats = stats;
		}
		internal JArithmeticDecoderStats getGenericRegionStats()
		{
			return this.genericRegionStats;
		}
		internal JArithmeticDecoderStats getRefinementRegionStats()
		{
			return this.refinementRegionStats;
		}
	}
}
