using System;
namespace Persits.PDF
{
	internal class RavenSolidColor : RavenPattern
	{
		private RavenColor color;
		internal RavenSolidColor(RavenColor colorA)
		{
			this.color = new RavenColor();
			RavenTypes.ravenColorCopy(this.color, colorA);
		}
		internal override RavenPattern copy()
		{
			return new RavenSolidColor(this.color);
		}
		internal override void getColor(int x, int y, ref RavenColor c)
		{
			RavenTypes.ravenColorCopy(c, this.color);
		}
		internal override bool isStatic()
		{
			return true;
		}
	}
}
