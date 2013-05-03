using System;
namespace Persits.PDF
{
	internal class DCTScanInfo
	{
		internal bool[] comp = new bool[4];
		internal int numComps;
		internal int[] dcHuffTable = new int[4];
		internal int[] acHuffTable = new int[4];
		internal int firstCoeff;
		internal int lastCoeff;
		internal int ah;
		internal int al;
	}
}
