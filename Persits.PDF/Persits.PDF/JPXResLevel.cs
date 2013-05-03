using System;
namespace Persits.PDF
{
	internal class JPXResLevel
	{
		internal uint precinctWidth;
		internal uint precinctHeight;
		internal uint x0;
		internal uint y0;
		internal uint x1;
		internal uint y1;
		internal uint[] bx0 = new uint[3];
		internal uint[] by0 = new uint[3];
		internal uint[] bx1 = new uint[3];
		internal uint[] by1 = new uint[3];
		internal JPXPrecinct[] precincts;
	}
}
