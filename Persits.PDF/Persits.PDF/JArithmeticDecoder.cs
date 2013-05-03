using System;
namespace Persits.PDF
{
	internal class JArithmeticDecoder
	{
		private static uint[] qeTab = new uint[]
		{
			1442906112u,
			872480768u,
			402718720u,
			180420608u,
			86048768u,
			35717120u,
			1442906112u,
			1409351680u,
			1208025088u,
			939589632u,
			805371904u,
			604045312u,
			469827584u,
			369164288u,
			1442906112u,
			1409351680u,
			1359020032u,
			1208025088u,
			939589632u,
			872480768u,
			805371904u,
			671154176u,
			604045312u,
			570490880u,
			469827584u,
			402718720u,
			369164288u,
			335609856u,
			302055424u,
			285278208u,
			180420608u,
			163643392u,
			144769024u,
			86048768u,
			71368704u,
			44105728u,
			35717120u,
			21037056u,
			17891328u,
			8716288u,
			4784128u,
			2424832u,
			1376256u,
			589824u,
			327680u,
			65536u,
			1442906112u
		};
		private static int[] nmpsTab = new int[]
		{
			1,
			2,
			3,
			4,
			5,
			38,
			7,
			8,
			9,
			10,
			11,
			12,
			13,
			29,
			15,
			16,
			17,
			18,
			19,
			20,
			21,
			22,
			23,
			24,
			25,
			26,
			27,
			28,
			29,
			30,
			31,
			32,
			33,
			34,
			35,
			36,
			37,
			38,
			39,
			40,
			41,
			42,
			43,
			44,
			45,
			45,
			46
		};
		private static int[] nlpsTab = new int[]
		{
			1,
			6,
			9,
			12,
			29,
			33,
			6,
			14,
			14,
			14,
			17,
			18,
			20,
			21,
			14,
			14,
			15,
			16,
			17,
			18,
			19,
			19,
			20,
			21,
			22,
			23,
			24,
			25,
			26,
			27,
			28,
			29,
			30,
			31,
			32,
			33,
			34,
			35,
			36,
			37,
			38,
			39,
			40,
			41,
			42,
			43,
			46
		};
		private static int[] switchTab = new int[]
		{
			1,
			0,
			0,
			0,
			0,
			0,
			1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0
		};
		private uint buf0;
		private uint buf1;
		private uint c;
		private uint a;
		private int ct;
		private uint prev;
		private PdfStream str;
		private uint nBytesRead;
		private int dataLen;
		private bool limitStream;
		internal JArithmeticDecoder()
		{
			this.str = null;
			this.dataLen = 0;
			this.limitStream = false;
			this.nBytesRead = 0u;
		}
		~JArithmeticDecoder()
		{
			this.cleanup();
		}
		internal void setStream(PdfStream strA)
		{
			this.str = strA;
			this.dataLen = 0;
			this.limitStream = false;
		}
		internal void setStream(PdfStream strA, int dataLenA)
		{
			this.str = strA;
			this.dataLen = dataLenA;
			this.limitStream = true;
		}
		internal void start()
		{
			this.buf0 = this.readByte();
			this.buf1 = this.readByte();
			this.c = (this.buf0 ^ 255u) << 16;
			this.byteIn();
			this.c <<= 7;
			this.ct -= 7;
			this.a = 2147483648u;
		}
		internal void restart(int dataLenA)
		{
			if (this.dataLen >= 0)
			{
				this.dataLen = dataLenA;
				return;
			}
			if (this.dataLen == -1)
			{
				this.dataLen = dataLenA;
				this.buf1 = this.readByte();
				return;
			}
			int i = (-this.dataLen - 1) * 8 - this.ct;
			this.dataLen = dataLenA;
			uint num = 0u;
			bool flag = false;
			while (i > 0)
			{
				this.buf0 = this.readByte();
				int num2;
				if (flag)
				{
					num += 65024u - (this.buf0 << 9);
					num2 = 7;
				}
				else
				{
					num += 65280u - (this.buf0 << 8);
					num2 = 8;
				}
				flag = (this.buf0 == 255u);
				if (i > num2)
				{
					num <<= num2;
					i -= num2;
				}
				else
				{
					num <<= i;
					this.ct = num2 - i;
					i = 0;
				}
			}
			this.c += num;
			this.buf1 = this.readByte();
		}
		internal void cleanup()
		{
			if (this.limitStream)
			{
				while (this.dataLen > 0)
				{
					this.buf0 = this.buf1;
					this.buf1 = this.readByte();
				}
			}
		}
		internal int decodeBit(uint context, JArithmeticDecoderStats stats)
		{
			int num = stats.cxTab[(int)((UIntPtr)context)] >> 1;
			int num2 = (int)(stats.cxTab[(int)((UIntPtr)context)] & 1);
			uint num3 = JArithmeticDecoder.qeTab[num];
			this.a -= num3;
			int result;
			if (this.c < this.a)
			{
				if ((this.a & 2147483648u) != 0u)
				{
					result = num2;
				}
				else
				{
					if (this.a < num3)
					{
						result = 1 - num2;
						if (JArithmeticDecoder.switchTab[num] != 0)
						{
							stats.cxTab[(int)((UIntPtr)context)] = (byte)(JArithmeticDecoder.nlpsTab[num] << 1 | 1 - num2);
						}
						else
						{
							stats.cxTab[(int)((UIntPtr)context)] = (byte)(JArithmeticDecoder.nlpsTab[num] << 1 | num2);
						}
					}
					else
					{
						result = num2;
						stats.cxTab[(int)((UIntPtr)context)] = (byte)(JArithmeticDecoder.nmpsTab[num] << 1 | num2);
					}
					do
					{
						if (this.ct == 0)
						{
							this.byteIn();
						}
						this.a <<= 1;
						this.c <<= 1;
						this.ct--;
					}
					while ((this.a & 2147483648u) == 0u);
				}
			}
			else
			{
				this.c -= this.a;
				if (this.a < num3)
				{
					result = num2;
					stats.cxTab[(int)((UIntPtr)context)] = (byte)(JArithmeticDecoder.nmpsTab[num] << 1 | num2);
				}
				else
				{
					result = 1 - num2;
					if (JArithmeticDecoder.switchTab[num] != 0)
					{
						stats.cxTab[(int)((UIntPtr)context)] = (byte)(JArithmeticDecoder.nlpsTab[num] << 1 | 1 - num2);
					}
					else
					{
						stats.cxTab[(int)((UIntPtr)context)] = (byte)(JArithmeticDecoder.nlpsTab[num] << 1 | num2);
					}
				}
				this.a = num3;
				do
				{
					if (this.ct == 0)
					{
						this.byteIn();
					}
					this.a <<= 1;
					this.c <<= 1;
					this.ct--;
				}
				while ((this.a & 2147483648u) == 0u);
			}
			return result;
		}
		private int decodeByte(uint context, JArithmeticDecoderStats stats)
		{
			int num = 0;
			for (int i = 0; i < 8; i++)
			{
				num = (num << 1 | this.decodeBit(context, stats));
			}
			return num;
		}
		internal bool decodeInt(ref int x, JArithmeticDecoderStats stats)
		{
			this.prev = 1u;
			int num = this.decodeIntBit(stats);
			uint num2;
			if (this.decodeIntBit(stats) != 0)
			{
				if (this.decodeIntBit(stats) != 0)
				{
					if (this.decodeIntBit(stats) != 0)
					{
						if (this.decodeIntBit(stats) != 0)
						{
							if (this.decodeIntBit(stats) != 0)
							{
								num2 = 0u;
								for (int i = 0; i < 32; i++)
								{
									num2 = (num2 << 1 | (uint)this.decodeIntBit(stats));
								}
								num2 += 4436u;
							}
							else
							{
								num2 = 0u;
								for (int i = 0; i < 12; i++)
								{
									num2 = (num2 << 1 | (uint)this.decodeIntBit(stats));
								}
								num2 += 340u;
							}
						}
						else
						{
							num2 = 0u;
							for (int i = 0; i < 8; i++)
							{
								num2 = (num2 << 1 | (uint)this.decodeIntBit(stats));
							}
							num2 += 84u;
						}
					}
					else
					{
						num2 = 0u;
						for (int i = 0; i < 6; i++)
						{
							num2 = (num2 << 1 | (uint)this.decodeIntBit(stats));
						}
						num2 += 20u;
					}
				}
				else
				{
					num2 = (uint)this.decodeIntBit(stats);
					num2 = (num2 << 1 | (uint)this.decodeIntBit(stats));
					num2 = (num2 << 1 | (uint)this.decodeIntBit(stats));
					num2 = (num2 << 1 | (uint)this.decodeIntBit(stats));
					num2 += 4u;
				}
			}
			else
			{
				num2 = (uint)this.decodeIntBit(stats);
				num2 = (num2 << 1 | (uint)this.decodeIntBit(stats));
			}
			if (num != 0)
			{
				if (num2 == 0u)
				{
					return false;
				}
				x = (int)(-(int)num2);
			}
			else
			{
				x = (int)num2;
			}
			return true;
		}
		internal uint decodeIAID(uint codeLen, JArithmeticDecoderStats stats)
		{
			this.prev = 1u;
			for (uint num = 0u; num < codeLen; num += 1u)
			{
				int num2 = this.decodeBit(this.prev, stats);
				this.prev = (this.prev << 1 | (uint)num2);
			}
			return (uint)((ulong)this.prev - (ulong)(1L << (int)(codeLen & 31u)));
		}
		internal void resetByteCounter()
		{
			this.nBytesRead = 0u;
		}
		internal uint getByteCounter()
		{
			return this.nBytesRead;
		}
		private uint readByte()
		{
			if (this.limitStream)
			{
				this.dataLen--;
				if (this.dataLen < 0)
				{
					return 255u;
				}
			}
			this.nBytesRead += 1u;
			return (uint)(this.str.getChar() & 255);
		}
		private int decodeIntBit(JArithmeticDecoderStats stats)
		{
			int num = this.decodeBit(this.prev, stats);
			if (this.prev < 256u)
			{
				this.prev = (this.prev << 1 | (uint)num);
			}
			else
			{
				this.prev = (((this.prev << 1 | (uint)num) & 511u) | 256u);
			}
			return num;
		}
		private void byteIn()
		{
			if (this.buf0 != 255u)
			{
				this.buf0 = this.buf1;
				this.buf1 = this.readByte();
				this.c = this.c + 65280u - (this.buf0 << 8);
				this.ct = 8;
				return;
			}
			if (this.buf1 > 143u)
			{
				if (this.limitStream)
				{
					this.buf0 = this.buf1;
					this.buf1 = this.readByte();
					this.c = this.c + 65280u - (this.buf0 << 8);
				}
				this.ct = 8;
				return;
			}
			this.buf0 = this.buf1;
			this.buf1 = this.readByte();
			this.c = this.c + 65024u - (this.buf0 << 9);
			this.ct = 7;
		}
	}
}
