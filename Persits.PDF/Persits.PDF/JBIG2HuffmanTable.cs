using System;
namespace Persits.PDF
{
	internal class JBIG2HuffmanTable
	{
		internal int val;
		internal uint prefixLen;
		internal uint rangeLen;
		internal uint prefix;
		internal JBIG2HuffmanTable(int v, uint p, uint r, uint pr)
		{
			this.val = v;
			this.prefixLen = p;
			this.rangeLen = r;
			this.prefix = pr;
		}
	}
}
