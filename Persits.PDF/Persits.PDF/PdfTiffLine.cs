using System;
namespace Persits.PDF
{
	internal class PdfTiffLine
	{
		public byte[] m_pBuffer;
		private int m_nCurrentByte;
		private int m_nCurrentBit;
		private int m_nPixelWidth;
		public PdfTiffLine(int nPixelWidth)
		{
			this.m_pBuffer = new byte[(nPixelWidth + 7) / 8];
			this.m_nPixelWidth = nPixelWidth;
			this.Reset();
		}
		~PdfTiffLine()
		{
			this.m_pBuffer = null;
		}
		public void Reset()
		{
			this.m_nCurrentByte = 0;
			this.m_nCurrentBit = 0;
		}
		public void Clear()
		{
			for (int i = 0; i < this.m_pBuffer.Length; i++)
			{
				this.m_pBuffer[i] = 0;
			}
			this.Reset();
		}
		public void SetPointer(int nPtr)
		{
			this.m_nCurrentByte = nPtr >> 3;
			this.m_nCurrentBit = nPtr % 8;
		}
		public void Copy(PdfTiffLine line)
		{
			Array.Copy(line.m_pBuffer, this.m_pBuffer, line.m_pBuffer.Length);
			this.m_nPixelWidth = line.m_nPixelWidth;
			this.Reset();
		}
		public short GetBit()
		{
			return this.GetBitAux(this.m_nCurrentByte, this.m_nCurrentBit);
		}
		public short GetBitAt(int nPixel)
		{
			if (nPixel <= -1)
			{
				return 0;
			}
			return this.GetBitAux(nPixel / 8, nPixel % 8);
		}
		public bool NextBit(ref short pBit)
		{
			if (8 * this.m_nCurrentByte + this.m_nCurrentBit >= this.m_nPixelWidth)
			{
				return false;
			}
			pBit = this.GetBit();
			this.m_nCurrentBit++;
			if (this.m_nCurrentBit >= 8)
			{
				this.m_nCurrentBit = 0;
				this.m_nCurrentByte++;
			}
			return true;
		}
		public short GetBitAux(int nCurByte, int nCurBit)
		{
			byte b = this.m_pBuffer[nCurByte];
			if (((int)b << nCurBit & 128) != 128)
			{
				return 0;
			}
			return 1;
		}
		public int FindBit(int nFrom, int nColor)
		{
			int result = this.m_nPixelWidth;
			if (nFrom >= this.m_nPixelWidth)
			{
				return result;
			}
			int nCurrentByte = this.m_nCurrentByte;
			int nCurrentBit = this.m_nCurrentBit;
			this.m_nCurrentByte = nFrom >> 3;
			this.m_nCurrentBit = nFrom % 8;
			short num = 0;
			short num2 = this.GetBitAt(nFrom - 1);
			while (this.NextBit(ref num))
			{
				if ((int)num == nColor && num != num2)
				{
					result = this.m_nCurrentByte * 8 + this.m_nCurrentBit - 1;
					break;
				}
				num2 = num;
			}
			this.m_nCurrentByte = nCurrentByte;
			this.m_nCurrentBit = nCurrentBit;
			return result;
		}
		public void PutBits(int nHowMany, int nColor)
		{
			while (nHowMany > 0)
			{
				if (this.m_nCurrentByte >= this.m_pBuffer.Length)
				{
					return;
				}
				if (nColor != 0)
				{
					byte[] expr_27_cp_0 = this.m_pBuffer;
					int expr_27_cp_1 = this.m_nCurrentByte;
					expr_27_cp_0[expr_27_cp_1] |= (byte)(128 >> this.m_nCurrentBit);
				}
				nHowMany--;
				this.m_nCurrentBit++;
				if (this.m_nCurrentBit >= 8)
				{
					this.m_nCurrentBit = 0;
					this.m_nCurrentByte++;
				}
			}
		}
		public void PutBitsFromCurrentTo(int nUpperBit, int nColor, bool bSetLastBit)
		{
			if (nUpperBit == -1)
			{
				return;
			}
			while (this.m_nCurrentByte * 8 + this.m_nCurrentBit <= nUpperBit)
			{
				if (this.m_nCurrentByte >= this.m_pBuffer.Length)
				{
					return;
				}
				if (this.m_nCurrentByte * 8 + this.m_nCurrentBit == nUpperBit)
				{
					if (bSetLastBit && nColor == 0)
					{
						byte[] expr_42_cp_0 = this.m_pBuffer;
						int expr_42_cp_1 = this.m_nCurrentByte;
						expr_42_cp_0[expr_42_cp_1] |= (byte)(128 >> this.m_nCurrentBit);
					}
					if (!bSetLastBit)
					{
						return;
					}
				}
				else
				{
					if (nColor != 0)
					{
						byte[] expr_77_cp_0 = this.m_pBuffer;
						int expr_77_cp_1 = this.m_nCurrentByte;
						expr_77_cp_0[expr_77_cp_1] |= (byte)(128 >> this.m_nCurrentBit);
					}
				}
				this.m_nCurrentBit++;
				if (this.m_nCurrentBit >= 8)
				{
					this.m_nCurrentBit = 0;
					this.m_nCurrentByte++;
				}
			}
		}
		public void PutBitsFromCurrentTo(int nUpperBit, int nColor)
		{
			this.PutBitsFromCurrentTo(nUpperBit, nColor, true);
		}
	}
}
