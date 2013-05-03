using System;
namespace Persits.PDF
{
	internal class RavenScreenParams
	{
		internal RavenScreenType type;
		internal int size;
		internal double gamma;
		internal double blackThreshold;
		internal double whiteThreshold;
		internal RavenScreenParams()
		{
		}
		internal RavenScreenParams(RavenScreenType t, int s, double g, double b, double w)
		{
			this.type = t;
			this.size = s;
			this.gamma = g;
			this.blackThreshold = b;
			this.whiteThreshold = w;
		}
	}
}
