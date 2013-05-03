using System;
namespace Persits.PDF
{
	internal class JBIG2Bitmap : JBIG2Segment
	{
		private int w;
		private int h;
		private int line;
		private byte[] data;
		internal JBIG2Bitmap(uint segNumA, int wA, int hA) : base(segNumA)
		{
			this.w = wA;
			this.h = hA;
			this.line = wA + 7 >> 3;
			if (this.w <= 0 || this.h <= 0 || this.line <= 0 || this.h >= 2147483646 / this.line)
			{
				this.h = -1;
				this.line = 2;
			}
			this.data = new byte[this.h * this.line + 1];
			this.data[this.h * this.line] = 0;
		}
		~JBIG2Bitmap()
		{
			this.data = null;
		}
		internal override JBIG2SegmentType getType()
		{
			return JBIG2SegmentType.jbig2SegBitmap;
		}
		internal JBIG2Bitmap copy()
		{
			return new JBIG2Bitmap(0u, this);
		}
		internal JBIG2Bitmap getSlice(uint x, uint y, uint wA, uint hA)
		{
			JBIG2Bitmap jBIG2Bitmap = new JBIG2Bitmap(0u, (int)wA, (int)hA);
			jBIG2Bitmap.clearToZero();
			for (uint num = 0u; num < hA; num += 1u)
			{
				for (uint num2 = 0u; num2 < wA; num2 += 1u)
				{
					if (this.getPixel((int)(x + num2), (int)(y + num)) != 0)
					{
						jBIG2Bitmap.setPixel((int)num2, (int)num);
					}
				}
			}
			return jBIG2Bitmap;
		}
		internal void expand(int newH, uint pixel)
		{
			if (newH <= this.h || this.line <= 0 || newH >= 2147483646 / this.line)
			{
				return;
			}
			byte[] destinationArray = new byte[newH * this.line + 1];
			Array.Copy(this.data, destinationArray, this.data.Length);
			this.data = destinationArray;
			if (pixel != 0u)
			{
				for (int i = this.h * this.line; i < (newH - this.h) * this.line; i++)
				{
					this.data[i] = 255;
				}
			}
			else
			{
				for (int j = this.h * this.line; j < (newH - this.h) * this.line; j++)
				{
					this.data[j] = 0;
				}
			}
			this.h = newH;
			this.data[this.h * this.line] = 0;
		}
		internal void clearToZero()
		{
			for (int i = 0; i < this.h * this.line; i++)
			{
				this.data[i] = 0;
			}
		}
		internal void clearToOne()
		{
			for (int i = 0; i < this.h * this.line; i++)
			{
				this.data[i] = 255;
			}
		}
		internal int getWidth()
		{
			return this.w;
		}
		internal int getHeight()
		{
			return this.h;
		}
		internal int getLineSize()
		{
			return this.line;
		}
		internal int getPixel(int x, int y)
		{
			if (x >= 0 && x < this.w && y >= 0 && y < this.h)
			{
				return this.data[y * this.line + (x >> 3)] >> 7 - (x & 7) & 1;
			}
			return 0;
		}
		internal void setPixel(int x, int y)
		{
			byte[] expr_17_cp_0 = this.data;
			int expr_17_cp_1 = y * this.line + (x >> 3);
			expr_17_cp_0[expr_17_cp_1] |= (byte)(1 << 7 - (x & 7));
		}
		internal void clearPixel(int x, int y)
		{
			byte[] expr_17_cp_0 = this.data;
			int expr_17_cp_1 = y * this.line + (x >> 3);
			expr_17_cp_0[expr_17_cp_1] &= (byte)(32639 >> (x & 7));
		}
		internal void getPixelPtr(int x, int y, JBIG2BitmapPtr ptr)
		{
			if (y < 0 || y >= this.h || x >= this.w)
			{
				ptr.p = null;
				ptr.shift = 0;
				ptr.x = 0;
				return;
			}
			if (x < 0)
			{
				ptr.p = new RavenColorPtr(this.data, y * this.line);
				ptr.shift = 7;
				ptr.x = x;
				return;
			}
			ptr.p = new RavenColorPtr(this.data, y * this.line + (x >> 3));
			ptr.shift = 7 - (x & 7);
			ptr.x = x;
		}
		internal int nextPixel(JBIG2BitmapPtr ptr)
		{
			int result;
			if (ptr.p == null)
			{
				result = 0;
			}
			else
			{
				if (ptr.x < 0)
				{
					ptr.x++;
					result = 0;
				}
				else
				{
					result = (ptr.p.ptr >> ptr.shift & 1);
					if (++ptr.x == this.w)
					{
						ptr.p = null;
					}
					else
					{
						if (ptr.shift == 0)
						{
							ptr.p = ++ptr.p;
							ptr.shift = 7;
						}
						else
						{
							ptr.shift--;
						}
					}
				}
			}
			return result;
		}
		internal void duplicateRow(int yDest, int ySrc)
		{
			Array.Copy(this.data, ySrc * this.line, this.data, yDest * this.line, this.line);
		}
		internal void combine(JBIG2Bitmap bitmap, int x, int y, uint combOp)
		{
			if (y < -2147483647)
			{
				return;
			}
			int num;
			if (y < 0)
			{
				num = -y;
			}
			else
			{
				num = 0;
			}
			int num2;
			if (y + bitmap.h > this.h)
			{
				num2 = this.h - y;
			}
			else
			{
				num2 = bitmap.h;
			}
			if (num >= num2)
			{
				return;
			}
			int num3;
			if (x >= 0)
			{
				num3 = (x & -8);
			}
			else
			{
				num3 = 0;
			}
			int num4 = x + bitmap.w;
			if (num4 > this.w)
			{
				num4 = this.w;
			}
			if (num3 >= num4)
			{
				return;
			}
			uint num5 = (uint)(x & 7);
			uint num6 = 8u - num5;
			uint num7 = (uint)(255 >> (num4 & 7));
			uint num8 = 255u << (((num4 & 7) == 0) ? 0 : (8 - (num4 & 7)));
			uint num9 = (uint)((long)(255 >> (int)num5) & (long)((ulong)num8));
			bool flag = num3 == (num4 - 1 & -8);
			for (int i = num; i < num2; i++)
			{
				if (flag)
				{
					if (x >= 0)
					{
						RavenColorPtr ravenColorPtr = new RavenColorPtr(this.data, (y + i) * this.line + (x >> 3));
						RavenColorPtr ravenColorPtr2 = new RavenColorPtr(bitmap.data, i * bitmap.line);
						uint num10 = (uint)ravenColorPtr.ptr;
						uint ptr = (uint)ravenColorPtr2.ptr;
						switch (combOp)
						{
						case 0u:
							num10 |= (ptr >> (int)num5 & num8);
							break;
						case 1u:
							num10 &= ((65280u | ptr) >> (int)num5 | num7);
							break;
						case 2u:
							num10 ^= (ptr >> (int)num5 & num8);
							break;
						case 3u:
							num10 ^= ((ptr ^ 255u) >> (int)num5 & num8);
							break;
						case 4u:
							num10 = ((num10 & ~num9) | (ptr >> (int)num5 & num9));
							break;
						}
						ravenColorPtr.ptr = (byte)num10;
					}
					else
					{
						RavenColorPtr ravenColorPtr = new RavenColorPtr(this.data, (y + i) * this.line);
						RavenColorPtr ravenColorPtr2 = new RavenColorPtr(bitmap.data, i * bitmap.line + (-x >> 3));
						uint num10 = (uint)ravenColorPtr.ptr;
						uint ptr = (uint)ravenColorPtr2.ptr;
						switch (combOp)
						{
						case 0u:
							num10 |= (ptr & num8);
							break;
						case 1u:
							num10 &= (ptr | num7);
							break;
						case 2u:
							num10 ^= (ptr & num8);
							break;
						case 3u:
							num10 ^= ((ptr ^ 255u) & num8);
							break;
						case 4u:
							num10 = ((ptr & num8) | (num10 & num7));
							break;
						}
						ravenColorPtr.ptr = (byte)num10;
					}
				}
				else
				{
					RavenColorPtr ravenColorPtr;
					RavenColorPtr ravenColorPtr2;
					uint num10;
					uint ptr;
					int j;
					if (x >= 0)
					{
						ravenColorPtr = new RavenColorPtr(this.data, (y + i) * this.line + (x >> 3));
						ravenColorPtr2 = new RavenColorPtr(bitmap.data, i * bitmap.line);
						ptr = (uint)ravenColorPtr2.ptr;
						ravenColorPtr2 = ++ravenColorPtr2;
						num10 = (uint)ravenColorPtr.ptr;
						switch (combOp)
						{
						case 0u:
							num10 |= ptr >> (int)num5;
							break;
						case 1u:
							num10 &= (65280u | ptr) >> (int)num5;
							break;
						case 2u:
							num10 ^= ptr >> (int)num5;
							break;
						case 3u:
							num10 ^= (ptr ^ 255u) >> (int)num5;
							break;
						case 4u:
							num10 = (uint)(((ulong)num10 & (ulong)(255L << (int)(num6 & 31u))) | (ulong)(ptr >> (int)num5));
							break;
						}
						ravenColorPtr.ptr = (byte)num10;
						ravenColorPtr = ++ravenColorPtr;
						j = num3 + 8;
					}
					else
					{
						ravenColorPtr = new RavenColorPtr(this.data, (y + i) * this.line);
						ravenColorPtr2 = new RavenColorPtr(bitmap.data, i * bitmap.line + (-x >> 3));
						ptr = (uint)ravenColorPtr2.ptr;
						ravenColorPtr2 = ++ravenColorPtr2;
						j = num3;
					}
					uint num11;
					uint num12;
					while (j < num4 - 8)
					{
						num10 = (uint)ravenColorPtr.ptr;
						num11 = ptr;
						ptr = (uint)ravenColorPtr2.ptr;
						ravenColorPtr2 = ++ravenColorPtr2;
						num12 = ((num11 << 8 | ptr) >> (int)num5 & 255u);
						switch (combOp)
						{
						case 0u:
							num10 |= num12;
							break;
						case 1u:
							num10 &= num12;
							break;
						case 2u:
							num10 ^= num12;
							break;
						case 3u:
							num10 ^= (num12 ^ 255u);
							break;
						case 4u:
							num10 = num12;
							break;
						}
						ravenColorPtr.ptr = (byte)num10;
						ravenColorPtr = ++ravenColorPtr;
						j += 8;
					}
					num10 = (uint)ravenColorPtr.ptr;
					num11 = ptr;
					ptr = (uint)ravenColorPtr2.ptr;
					ravenColorPtr2 = ++ravenColorPtr2;
					num12 = ((num11 << 8 | ptr) >> (int)num5 & 255u);
					switch (combOp)
					{
					case 0u:
						num10 |= (num12 & num8);
						break;
					case 1u:
						num10 &= (num12 | num7);
						break;
					case 2u:
						num10 ^= (num12 & num8);
						break;
					case 3u:
						num10 ^= ((num12 ^ 255u) & num8);
						break;
					case 4u:
						num10 = ((num12 & num8) | (num10 & num7));
						break;
					}
					ravenColorPtr.ptr = (byte)num10;
				}
			}
		}
		internal byte[] getDataPtr()
		{
			return this.data;
		}
		internal int getDataSize()
		{
			return this.h * this.line;
		}
		private JBIG2Bitmap(uint segNumA, JBIG2Bitmap bitmap) : base(segNumA)
		{
			this.w = bitmap.w;
			this.h = bitmap.h;
			this.line = bitmap.line;
			if (this.w <= 0 || this.h <= 0 || this.line <= 0 || this.h >= 2147483646 / this.line)
			{
				this.h = -1;
				this.line = 2;
			}
			this.data = new byte[this.h * this.line + 1];
			Array.Copy(bitmap.data, this.data, bitmap.data.Length);
			this.data[this.h * this.line] = 0;
		}
	}
}
