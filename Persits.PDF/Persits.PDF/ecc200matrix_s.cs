using System;
namespace Persits.PDF
{
	internal struct ecc200matrix_s
	{
		public int H;
		public int W;
		public int FH;
		public int FW;
		public int Bytes;
		public int Datablock;
		public int RSblock;
		public ecc200matrix_s(int h, int w, int fh, int fw, int bytes, int datablock, int rsblock)
		{
			this.H = h;
			this.W = w;
			this.FH = fh;
			this.FW = fw;
			this.Bytes = bytes;
			this.Datablock = datablock;
			this.RSblock = rsblock;
		}
	}
}
