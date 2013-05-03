using System;
namespace Persits.PDF
{
	internal struct RavenPathHint
	{
		internal int ctrl0;
		internal int ctrl1;
		internal int firstPt;
		internal int lastPt;
		internal RavenPathHint(int c0, int c1, int f, int l)
		{
			this.ctrl0 = c0;
			this.ctrl1 = c1;
			this.firstPt = f;
			this.lastPt = l;
		}
	}
}
