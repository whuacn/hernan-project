using System;
namespace Persits.PDF
{
	internal class JBIG2HuffmanDecoder
	{
		private PdfStream str;
		private uint buf;
		private uint bufLen;
		private uint byteCounter;
		internal JBIG2HuffmanDecoder()
		{
			this.str = null;
			this.byteCounter = 0u;
			this.reset();
		}
		internal void reset()
		{
			this.buf = 0u;
			this.bufLen = 0u;
		}
		internal void setStream(PdfStream strA)
		{
			this.str = strA;
		}
		internal bool decodeInt(ref int x, JBIG2HuffmanTable[] table)
		{
			uint num = 0u;
			uint num2 = 0u;
			uint num3 = 0u;
			while (table[(int)((UIntPtr)num)].rangeLen != 4294967295u)
			{
				while (num2 < table[(int)((UIntPtr)num)].prefixLen)
				{
					num3 = (num3 << 1 | this.readBit());
					num2 += 1u;
				}
				if (num3 == table[(int)((UIntPtr)num)].prefix)
				{
					if (table[(int)((UIntPtr)num)].rangeLen == 4294967294u)
					{
						return false;
					}
					if (table[(int)((UIntPtr)num)].rangeLen == 4294967293u)
					{
						x = (int)((long)table[(int)((UIntPtr)num)].val - (long)((ulong)this.readBits(32u)));
					}
					else
					{
						if (table[(int)((UIntPtr)num)].rangeLen > 0u)
						{
							x = (int)((long)table[(int)((UIntPtr)num)].val + (long)((ulong)this.readBits(table[(int)((UIntPtr)num)].rangeLen)));
						}
						else
						{
							x = table[(int)((UIntPtr)num)].val;
						}
					}
					return true;
				}
				else
				{
					num += 1u;
				}
			}
			return false;
		}
		internal uint readBits(uint n)
		{
			uint num = (n == 32u) ? 4294967295u : ((1u << (int)n) - 1u);
			uint num2;
			if (this.bufLen >= n)
			{
				num2 = (this.buf >> (int)(this.bufLen - n) & num);
				this.bufLen -= n;
			}
			else
			{
				num2 = (uint)((ulong)this.buf & (ulong)((long)((1 << (int)this.bufLen) - 1)));
				uint num3 = n - this.bufLen;
				this.bufLen = 0u;
				while (num3 >= 8u)
				{
					num2 = (num2 << 8 | (uint)(this.str.getChar() & 255));
					this.byteCounter += 1u;
					num3 -= 8u;
				}
				if (num3 > 0u)
				{
					this.buf = (uint)this.str.getChar();
					this.byteCounter += 1u;
					this.bufLen = 8u - num3;
					num2 = (uint)((ulong)((ulong)num2 << (int)num3) | ((ulong)(this.buf >> (int)this.bufLen) & (ulong)((long)((1 << (int)num3) - 1))));
				}
			}
			return num2;
		}
		internal uint readBit()
		{
			if (this.bufLen == 0u)
			{
				this.buf = (uint)this.str.getChar();
				this.byteCounter += 1u;
				this.bufLen = 8u;
			}
			this.bufLen -= 1u;
			return this.buf >> (int)this.bufLen & 1u;
		}
		internal void buildTable(JBIG2HuffmanTable[] table, uint len)
		{
			uint num;
			for (num = 0u; num < len; num += 1u)
			{
				uint num2 = num;
				while (num2 < len && table[(int)((UIntPtr)num2)].prefixLen == 0u)
				{
					num2 += 1u;
				}
				if (num2 == len)
				{
					break;
				}
				for (uint num3 = num2 + 1u; num3 < len; num3 += 1u)
				{
					if (table[(int)((UIntPtr)num3)].prefixLen > 0u && table[(int)((UIntPtr)num3)].prefixLen < table[(int)((UIntPtr)num2)].prefixLen)
					{
						num2 = num3;
					}
				}
				if (num2 != num)
				{
					JBIG2HuffmanTable jBIG2HuffmanTable = table[(int)((UIntPtr)num2)];
					for (uint num3 = num2; num3 > num; num3 -= 1u)
					{
						table[(int)((UIntPtr)num3)] = table[(int)((UIntPtr)(num3 - 1u))];
					}
					table[(int)((UIntPtr)num)] = jBIG2HuffmanTable;
				}
			}
			table[(int)((UIntPtr)num)] = table[(int)((UIntPtr)len)];
			if (table[0].rangeLen != 4294967295u)
			{
				num = 0u;
				uint num4 = 0u;
				table[(int)((UIntPtr)(num++))].prefix = num4++;
				while (table[(int)((UIntPtr)num)].rangeLen != 4294967295u)
				{
					num4 <<= (int)(table[(int)((UIntPtr)num)].prefixLen - table[(int)((UIntPtr)(num - 1u))].prefixLen);
					table[(int)((UIntPtr)num)].prefix = num4++;
					num += 1u;
				}
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
