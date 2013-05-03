using System;
namespace Persits.PDF
{
	internal class DCTHuffTable
	{
		internal byte[] firstSym = new byte[17];
		internal ushort[] firstCode = new ushort[17];
		internal ushort[] numCodes = new ushort[17];
		internal byte[] sym = new byte[256];
	}
}
