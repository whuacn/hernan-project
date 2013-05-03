using System;
namespace Persits.PDF
{
	internal class PdfImageStream
	{
		private PdfDictWithStream str;
		private int width;
		private int nComps;
		private int nBits;
		private int nVals;
		private byte[] imgLine;
		private int imgIdx;
		internal PdfImageStream(PdfDictWithStream strA, int widthA, int nCompsA, int nBitsA)
		{
			this.str = strA;
			this.width = widthA;
			this.nComps = nCompsA;
			this.nBits = nBitsA;
			this.nVals = this.width * this.nComps;
			int num;
			if (this.nBits == 1)
			{
				num = (this.nVals + 7 & -8);
			}
			else
			{
				num = this.nVals;
			}
			this.imgLine = new byte[num];
			this.imgIdx = this.nVals;
		}
		~PdfImageStream()
		{
		}
		internal void reset()
		{
			this.str.reset();
		}
		private bool getPixel(byte[] pix)
		{
			if (this.imgIdx >= this.nVals)
			{
				this.getLine();
				this.imgIdx = 0;
			}
			for (int i = 0; i < this.nComps; i++)
			{
				pix[i] = this.imgLine[this.imgIdx++];
			}
			return true;
		}
		internal byte[] getLine()
		{
			if (this.nBits == 1)
			{
				for (int i = 0; i < this.nVals; i += 8)
				{
					int @char = this.str.getChar();
					this.imgLine[i] = (byte)(@char >> 7 & 1);
					this.imgLine[i + 1] = (byte)(@char >> 6 & 1);
					this.imgLine[i + 2] = (byte)(@char >> 5 & 1);
					this.imgLine[i + 3] = (byte)(@char >> 4 & 1);
					this.imgLine[i + 4] = (byte)(@char >> 3 & 1);
					this.imgLine[i + 5] = (byte)(@char >> 2 & 1);
					this.imgLine[i + 6] = (byte)(@char >> 1 & 1);
					this.imgLine[i + 7] = (byte)(@char & 1);
				}
			}
			else
			{
				if (this.nBits == 8)
				{
					for (int i = 0; i < this.nVals; i++)
					{
						this.imgLine[i] = (byte)this.str.getChar();
					}
				}
				else
				{
					ulong num = (ulong)((long)((1 << this.nBits) - 1));
					ulong num2 = 0uL;
					int num3 = 0;
					for (int i = 0; i < this.nVals; i++)
					{
						if (num3 < this.nBits)
						{
							num2 = (num2 << 8 | (ulong)((byte)(this.str.getChar() & 255)));
							num3 += 8;
						}
						this.imgLine[i] = (byte)(num2 >> num3 - this.nBits & num);
						num3 -= this.nBits;
					}
				}
			}
			return this.imgLine;
		}
		private void skipLine()
		{
			int num = this.nVals * this.nBits + 7 >> 3;
			for (int i = 0; i < num; i++)
			{
				this.str.getChar();
			}
		}
	}
}
