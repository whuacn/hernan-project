using System;
namespace Persits.PDF
{
	internal class JPXCodeBlock
	{
		internal uint x0;
		internal uint y0;
		internal uint x1;
		internal uint y1;
		internal bool seen;
		internal uint lBlock;
		internal uint nextPass;
		internal uint nZeroBitPlanes;
		internal uint included;
		internal uint nCodingPasses;
		internal uint[] dataLen;
		internal uint dataLenSize;
		internal RavenPtr<int> coeffs;
		internal char[] touched;
		internal ushort len;
		internal JArithmeticDecoder arithDecoder;
		internal JArithmeticDecoderStats stats;
	}
}
