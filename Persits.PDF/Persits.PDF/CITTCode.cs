using System;
namespace Persits.PDF
{
	internal class CITTCode
	{
		internal short bits;
		internal short n;
		internal CITTCode(short b, short N)
		{
			this.bits = b;
			this.n = N;
		}
	}
}
