using System;
namespace Persits.PDF
{
	internal class JBIG2CodeTable : JBIG2Segment
	{
		private JBIG2HuffmanTable[] table;
		internal JBIG2CodeTable(uint segNumA, JBIG2HuffmanTable[] tableA) : base(segNumA)
		{
			this.table = tableA;
		}
		~JBIG2CodeTable()
		{
			this.table = null;
		}
		internal override JBIG2SegmentType getType()
		{
			return JBIG2SegmentType.jbig2SegCodeTable;
		}
		internal JBIG2HuffmanTable[] getHuffTable()
		{
			return this.table;
		}
	}
}
