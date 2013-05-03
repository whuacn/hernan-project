using System;
namespace Persits.PDF
{
	internal class PdfShadingBitBuf
	{
		private PdfDictWithStream str;
		private int bitBuf;
		private int nBits;
		internal PdfShadingBitBuf(PdfDictWithStream strA)
		{
			this.str = strA;
			this.str.reset();
			this.bitBuf = 0;
			this.nBits = 0;
		}
		internal bool getBits(int n, ref uint val)
		{
			int num;
			if (this.nBits >= n)
			{
				num = (this.bitBuf >> this.nBits - n & (1 << n) - 1);
				this.nBits -= n;
			}
			else
			{
				num = 0;
				if (this.nBits > 0)
				{
					num = (this.bitBuf & (1 << this.nBits) - 1);
					n -= this.nBits;
					this.nBits = 0;
				}
				while (n > 0)
				{
					if ((this.bitBuf = this.str.getChar()) == -1)
					{
						this.nBits = 0;
						return false;
					}
					if (n >= 8)
					{
						num = (num << 8 | this.bitBuf);
						n -= 8;
					}
					else
					{
						num = (num << n | this.bitBuf >> 8 - n);
						this.nBits = 8 - n;
						n = 0;
					}
				}
			}
			val = (uint)num;
			return true;
		}
		internal void flushBits()
		{
			this.bitBuf = 0;
			this.nBits = 0;
		}
	}
}
