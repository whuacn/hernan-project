using System;
namespace Persits.PDF
{
	internal class JBIG2MMRDecoder
	{
		private PdfStream str;
		private uint buf;
		private uint bufLen;
		private uint nBytesRead;
		private uint byteCounter;
		internal JBIG2MMRDecoder()
		{
			this.str = null;
			this.byteCounter = 0u;
			this.reset();
		}
		internal void setStream(PdfStream strA)
		{
			this.str = strA;
		}
		internal void reset()
		{
			this.buf = 0u;
			this.bufLen = 0u;
			this.nBytesRead = 0u;
		}
		internal int get2DCode()
		{
			CITTCode cITTCode;
			if (this.bufLen == 0u)
			{
				this.buf = (uint)(this.str.getChar() & 255);
				this.bufLen = 8u;
				this.nBytesRead += 1u;
				this.byteCounter += 1u;
				cITTCode = PdfCITTFaxStream.twoDimTab1[(int)((UIntPtr)(this.buf >> 1 & 127u))];
			}
			else
			{
				if (this.bufLen == 8u)
				{
					cITTCode = PdfCITTFaxStream.twoDimTab1[(int)((UIntPtr)(this.buf >> 1 & 127u))];
				}
				else
				{
					cITTCode = PdfCITTFaxStream.twoDimTab1[(int)((UIntPtr)(this.buf << (int)(7u - this.bufLen) & 127u))];
					if (cITTCode.bits < 0 || (uint)cITTCode.bits > this.bufLen)
					{
						this.buf = (this.buf << 8 | (uint)(this.str.getChar() & 255));
						this.bufLen += 8u;
						this.nBytesRead += 1u;
						this.byteCounter += 1u;
						cITTCode = PdfCITTFaxStream.twoDimTab1[(int)((UIntPtr)(this.buf >> (int)(this.bufLen - 7u) & 127u))];
					}
				}
			}
			if (cITTCode.bits < 0)
			{
				AuxException.Throw("Invalid two dim code in JBIG2 MMR stream");
				return -1;
			}
			this.bufLen -= (uint)cITTCode.bits;
			return (int)cITTCode.n;
		}
		internal int getWhiteCode()
		{
			if (this.bufLen == 0u)
			{
				this.buf = (uint)(this.str.getChar() & 255);
				this.bufLen = 8u;
				this.nBytesRead += 1u;
				this.byteCounter += 1u;
			}
			CITTCode cITTCode;
			while (true)
			{
				if (this.bufLen >= 11u && (this.buf >> (int)(this.bufLen - 7u) & 127u) == 0u)
				{
					uint num;
					if (this.bufLen <= 12u)
					{
						num = this.buf << (int)(12u - this.bufLen);
					}
					else
					{
						num = this.buf >> (int)(this.bufLen - 12u);
					}
					cITTCode = PdfCITTFaxStream.whiteTab1[(int)((UIntPtr)(num & 31u))];
				}
				else
				{
					uint num;
					if (this.bufLen <= 9u)
					{
						num = this.buf << (int)(9u - this.bufLen);
					}
					else
					{
						num = this.buf >> (int)(this.bufLen - 9u);
					}
					cITTCode = PdfCITTFaxStream.whiteTab2[(int)((UIntPtr)(num & 511u))];
				}
				if (cITTCode.bits > 0 && (uint)cITTCode.bits <= this.bufLen)
				{
					break;
				}
				if (this.bufLen >= 12u)
				{
					goto IL_172;
				}
				this.buf = (this.buf << 8 | (uint)(this.str.getChar() & 255));
				this.bufLen += 8u;
				this.nBytesRead += 1u;
				this.byteCounter += 1u;
			}
			this.bufLen -= (uint)cITTCode.bits;
			return (int)cITTCode.n;
			IL_172:
			AuxException.Throw("Invalid white code in JBIG2 MMR stream.");
			this.bufLen -= 1u;
			return 1;
		}
		internal int getBlackCode()
		{
			if (this.bufLen == 0u)
			{
				this.buf = (uint)(this.str.getChar() & 255);
				this.bufLen = 8u;
				this.nBytesRead += 1u;
				this.byteCounter += 1u;
			}
			CITTCode cITTCode;
			while (true)
			{
				if (this.bufLen >= 10u && (this.buf >> (int)(this.bufLen - 6u) & 63u) == 0u)
				{
					uint num;
					if (this.bufLen <= 13u)
					{
						num = this.buf << (int)(13u - this.bufLen);
					}
					else
					{
						num = this.buf >> (int)(this.bufLen - 13u);
					}
					cITTCode = PdfCITTFaxStream.blackTab1[(int)((UIntPtr)(num & 127u))];
				}
				else
				{
					if (this.bufLen >= 7u && (this.buf >> (int)(this.bufLen - 4u) & 15u) == 0u && (this.buf >> (int)(this.bufLen - 6u) & 3u) != 0u)
					{
						uint num;
						if (this.bufLen <= 12u)
						{
							num = this.buf << (int)(12u - this.bufLen);
						}
						else
						{
							num = this.buf >> (int)(this.bufLen - 12u);
						}
						cITTCode = PdfCITTFaxStream.blackTab2[(int)((UIntPtr)((num & 255u) - 64u))];
					}
					else
					{
						uint num;
						if (this.bufLen <= 6u)
						{
							num = this.buf << (int)(6u - this.bufLen);
						}
						else
						{
							num = this.buf >> (int)(this.bufLen - 6u);
						}
						cITTCode = PdfCITTFaxStream.blackTab3[(int)((UIntPtr)(num & 63u))];
					}
				}
				if (cITTCode.bits > 0 && (uint)cITTCode.bits <= this.bufLen)
				{
					break;
				}
				if (this.bufLen >= 13u)
				{
					goto IL_1ED;
				}
				this.buf = (this.buf << 8 | (uint)(this.str.getChar() & 255));
				this.bufLen += 8u;
				this.nBytesRead += 1u;
				this.byteCounter += 1u;
			}
			this.bufLen -= (uint)cITTCode.bits;
			return (int)cITTCode.n;
			IL_1ED:
			AuxException.Throw("Invalid black code in JBIG2 MMR stream.");
			this.bufLen -= 1u;
			return 1;
		}
		internal uint get24Bits()
		{
			while (this.bufLen < 24u)
			{
				this.buf = (this.buf << 8 | (uint)(this.str.getChar() & 255));
				this.bufLen += 8u;
				this.nBytesRead += 1u;
				this.byteCounter += 1u;
			}
			return this.buf >> (int)(this.bufLen - 24u) & 16777215u;
		}
		internal void skipTo(uint length)
		{
			while (this.nBytesRead < length)
			{
				this.str.getChar();
				this.nBytesRead += 1u;
				this.byteCounter += 1u;
			}
		}
		internal void resetByteCounter()
		{
			this.byteCounter = 0u;
		}
		internal uint getByteCounter()
		{
			return this.byteCounter;
		}
	}
}
