using System;
namespace Persits.PDF
{
	internal class shapestruct
	{
		public char basechar;
		public int count;
		public char[] charshape;
		public shapestruct(uint b, int c, uint[] shape)
		{
			this.basechar = (char)b;
			this.count = c;
			this.charshape = new char[c];
			for (int i = 0; i < c; i++)
			{
				this.charshape[i] = (char)shape[i];
			}
		}
	}
}
