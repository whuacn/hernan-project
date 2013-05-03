using System;
namespace Persits.PDF
{
	internal class JPXSubband
	{
		internal uint x0;
		internal uint y0;
		internal uint x1;
		internal uint y1;
		internal uint nXCBs;
		internal uint nYCBs;
		internal uint maxTTLevel;
		internal JPXTagTreeNode[] inclusion;
		internal JPXTagTreeNode[] zeroBitPlane;
		internal JPXCodeBlock[] cbs;
	}
}
