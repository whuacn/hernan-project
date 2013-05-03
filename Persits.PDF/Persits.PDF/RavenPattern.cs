using System;
namespace Persits.PDF
{
	internal abstract class RavenPattern
	{
		internal abstract RavenPattern copy();
		internal abstract void getColor(int x, int y, ref RavenColor c);
		internal abstract bool isStatic();
	}
}
