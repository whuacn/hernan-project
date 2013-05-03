using System;
namespace Persits.PDF
{
	internal class RavenXPathSeg
	{
		internal double x0;
		internal double y0;
		internal double x1;
		internal double y1;
		internal double dxdy;
		internal double dydx;
		internal uint flags;
		internal RavenXPathSeg(double xx0, double yy0, double xx1, double yy1)
		{
			this.x0 = xx0;
			this.y0 = yy0;
			this.x1 = xx1;
			this.y1 = yy1;
		}
	}
}
