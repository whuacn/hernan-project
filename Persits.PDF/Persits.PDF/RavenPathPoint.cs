using System;
namespace Persits.PDF
{
	internal struct RavenPathPoint
	{
		internal double x;
		internal double y;
		internal RavenPathPoint(double xx, double yy)
		{
			this.x = xx;
			this.y = yy;
		}
		internal void Add(double dx, double dy)
		{
			this.x += dx;
			this.y += dy;
		}
	}
}
