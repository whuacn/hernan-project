using System;
using System.Collections.Generic;
namespace Persits.PDF
{
	internal class RavenXPathScanner
	{
		internal RavenXPath xPath;
		internal bool eo;
		internal int xMin;
		internal int yMin;
		internal int xMax;
		internal int yMax;
		internal bool partialClip;
		internal List<RavenIntersect> allInter;
		internal int allInterLen;
		internal int allInterSize;
		internal int[] inter;
		internal int interY;
		internal int interIdx;
		internal int interCount;
		internal RavenXPathScanner(RavenXPath xPathA, bool eoA, int clipYMin, int clipYMax)
		{
			this.xPath = xPathA;
			this.eo = eoA;
			this.partialClip = false;
			if (this.xPath.length == 0)
			{
				this.xMin = (this.yMin = 1);
				this.xMax = (this.yMax = 0);
			}
			else
			{
				RavenXPathSeg ravenXPathSeg = this.xPath.segs[0];
				double num;
				double num2;
				if (ravenXPathSeg.x0 <= ravenXPathSeg.x1)
				{
					num = ravenXPathSeg.x0;
					num2 = ravenXPathSeg.x1;
				}
				else
				{
					num = ravenXPathSeg.x1;
					num2 = ravenXPathSeg.x0;
				}
				double x;
				double num3;
				if ((ravenXPathSeg.flags & 4u) != 0u)
				{
					x = ravenXPathSeg.y1;
					num3 = ravenXPathSeg.y0;
				}
				else
				{
					x = ravenXPathSeg.y0;
					num3 = ravenXPathSeg.y1;
				}
				for (int i = 1; i < this.xPath.length; i++)
				{
					ravenXPathSeg = this.xPath.segs[i];
					if (ravenXPathSeg.x0 < num)
					{
						num = ravenXPathSeg.x0;
					}
					else
					{
						if (ravenXPathSeg.x0 > num2)
						{
							num2 = ravenXPathSeg.x0;
						}
					}
					if (ravenXPathSeg.x1 < num)
					{
						num = ravenXPathSeg.x1;
					}
					else
					{
						if (ravenXPathSeg.x1 > num2)
						{
							num2 = ravenXPathSeg.x1;
						}
					}
					if ((ravenXPathSeg.flags & 4u) != 0u)
					{
						if (ravenXPathSeg.y0 > num3)
						{
							num3 = ravenXPathSeg.y0;
						}
					}
					else
					{
						if (ravenXPathSeg.y1 > num3)
						{
							num3 = ravenXPathSeg.y1;
						}
					}
				}
				this.xMin = RavenMath.ravenFloor(num);
				this.xMax = RavenMath.ravenFloor(num2);
				this.yMin = RavenMath.ravenFloor(x);
				this.yMax = RavenMath.ravenFloor(num3);
				if (clipYMin > this.yMin)
				{
					this.yMin = clipYMin;
					this.partialClip = true;
				}
				if (clipYMax < this.yMax)
				{
					this.yMax = clipYMax;
					this.partialClip = true;
				}
			}
			this.allInter = null;
			this.inter = null;
			this.computeIntersections();
			this.interY = this.yMin - 1;
		}
		internal void getBBoxAA(ref int xMinA, ref int yMinA, ref int xMaxA, ref int yMaxA)
		{
			xMinA = this.xMin / RavenConstants.ravenAASize;
			yMinA = this.yMin / RavenConstants.ravenAASize;
			xMaxA = this.xMax / RavenConstants.ravenAASize;
			yMaxA = this.yMax / RavenConstants.ravenAASize;
		}
		internal void getBBox(ref int xMinA, ref int yMinA, ref int xMaxA, ref int yMaxA)
		{
			xMinA = this.xMin;
			yMinA = this.yMin;
			xMaxA = this.xMax;
			yMaxA = this.yMax;
		}
		internal bool hasPartialClip()
		{
			return this.partialClip;
		}
		internal void getSpanBounds(int y, ref int spanXMin, ref int spanXMax)
		{
			int num2;
			int num;
			if (y < this.yMin || y > this.yMax)
			{
				num = (num2 = 0);
			}
			else
			{
				num2 = this.inter[y - this.yMin];
				num = this.inter[y - this.yMin + 1];
			}
			if (num2 < num)
			{
				spanXMin = this.allInter[num2].x0;
				int x = this.allInter[num2].x1;
				for (int i = num2 + 1; i < num; i++)
				{
					if (this.allInter[i].x1 > x)
					{
						x = this.allInter[i].x1;
					}
				}
				spanXMax = x;
				return;
			}
			spanXMin = this.xMax + 1;
			spanXMax = this.xMax;
		}
		internal bool test(int x, int y)
		{
			if (y < this.yMin || y > this.yMax)
			{
				return false;
			}
			int num = this.inter[y - this.yMin];
			int num2 = this.inter[y - this.yMin + 1];
			int num3 = 0;
			int num4 = num;
			while (num4 < num2 && this.allInter[num4].x0 <= x)
			{
				if (x <= this.allInter[num4].x1)
				{
					return true;
				}
				num3 += this.allInter[num4].count;
				num4++;
			}
			if (!this.eo)
			{
				return num3 != 0;
			}
			return (num3 & 1) != 0;
		}
		internal bool testSpan(int x0, int x1, int y)
		{
			if (y < this.yMin || y > this.yMax)
			{
				return false;
			}
			int num = this.inter[y - this.yMin];
			int num2 = this.inter[y - this.yMin + 1];
			int num3 = 0;
			int num4 = num;
			while (num4 < num2 && this.allInter[num4].x1 < x0)
			{
				num3 += this.allInter[num4].count;
				num4++;
			}
			int i = x0 - 1;
			while (i < x1)
			{
				if (num4 >= num2)
				{
					return false;
				}
				if (this.allInter[num4].x0 > i + 1 && !(this.eo ? ((num3 & 1) != 0) : (num3 != 0)))
				{
					return false;
				}
				if (this.allInter[num4].x1 > i)
				{
					i = this.allInter[num4].x1;
				}
				num3 += this.allInter[num4].count;
				num4++;
			}
			return true;
		}
		internal bool getNextSpan(int y, ref int x0, ref int x1)
		{
			if (y < this.yMin || y > this.yMax)
			{
				return false;
			}
			if (this.interY != y)
			{
				this.interY = y;
				this.interIdx = this.inter[y - this.yMin];
				this.interCount = 0;
			}
			int num = this.inter[y - this.yMin + 1];
			if (this.interIdx >= num)
			{
				return false;
			}
			int x2 = this.allInter[this.interIdx].x0;
			int x3 = this.allInter[this.interIdx].x1;
			this.interCount += this.allInter[this.interIdx].count;
			this.interIdx++;
			while (this.interIdx < num && (this.allInter[this.interIdx].x0 <= x3 || (this.eo ? ((this.interCount & 1) != 0) : (this.interCount != 0))))
			{
				if (this.allInter[this.interIdx].x1 > x3)
				{
					x3 = this.allInter[this.interIdx].x1;
				}
				this.interCount += this.allInter[this.interIdx].count;
				this.interIdx++;
			}
			x0 = x2;
			x1 = x3;
			return true;
		}
		private static int cmpIntersect(RavenIntersect i0, RavenIntersect i1)
		{
			int num = i0.y - i1.y;
			if (num == 0)
			{
				num = i0.x0 - i1.x0;
			}
			return num;
		}
		internal void computeIntersections()
		{
			if (this.yMin > this.yMax)
			{
				return;
			}
			this.allInterLen = 0;
			this.allInterSize = 16;
			this.allInter = new List<RavenIntersect>();
			int i;
			for (i = 0; i < this.xPath.length; i++)
			{
				RavenXPathSeg ravenXPathSeg = this.xPath.segs[i];
				double num;
				double num2;
				if ((ravenXPathSeg.flags & 4u) != 0u)
				{
					num = ravenXPathSeg.y1;
					num2 = ravenXPathSeg.y0;
				}
				else
				{
					num = ravenXPathSeg.y0;
					num2 = ravenXPathSeg.y1;
				}
				if ((ravenXPathSeg.flags & 1u) != 0u)
				{
					int j = RavenMath.ravenFloor(ravenXPathSeg.y0);
					if (j >= this.yMin && j <= this.yMax)
					{
						this.addIntersection(num, num2, ravenXPathSeg.flags, j, RavenMath.ravenFloor(ravenXPathSeg.x0), RavenMath.ravenFloor(ravenXPathSeg.x1));
					}
				}
				else
				{
					if ((ravenXPathSeg.flags & 2u) != 0u)
					{
						int num3 = RavenMath.ravenFloor(num);
						if (num3 < this.yMin)
						{
							num3 = this.yMin;
						}
						int num4 = RavenMath.ravenFloor(num2);
						if (num4 > this.yMax)
						{
							num4 = this.yMax;
						}
						int num5 = RavenMath.ravenFloor(ravenXPathSeg.x0);
						for (int j = num3; j <= num4; j++)
						{
							this.addIntersection(num, num2, ravenXPathSeg.flags, j, num5, num5);
						}
					}
					else
					{
						double num6;
						double num7;
						if (ravenXPathSeg.x0 < ravenXPathSeg.x1)
						{
							num6 = ravenXPathSeg.x0;
							num7 = ravenXPathSeg.x1;
						}
						else
						{
							num6 = ravenXPathSeg.x1;
							num7 = ravenXPathSeg.x0;
						}
						int num3 = RavenMath.ravenFloor(num);
						if (num3 < this.yMin)
						{
							num3 = this.yMin;
						}
						int num4 = RavenMath.ravenFloor(num2);
						if (num4 > this.yMax)
						{
							num4 = this.yMax;
						}
						double num8 = ravenXPathSeg.x0 + ((double)num3 - ravenXPathSeg.y0) * ravenXPathSeg.dxdy;
						for (int j = num3; j <= num4; j++)
						{
							double num9 = num8;
							num8 = ravenXPathSeg.x0 + ((double)(j + 1) - ravenXPathSeg.y0) * ravenXPathSeg.dxdy;
							if (num9 < num6)
							{
								num9 = num6;
							}
							else
							{
								if (num9 > num7)
								{
									num9 = num7;
								}
							}
							if (num8 < num6)
							{
								num8 = num6;
							}
							else
							{
								if (num8 > num7)
								{
									num8 = num7;
								}
							}
							this.addIntersection(num, num2, ravenXPathSeg.flags, j, RavenMath.ravenFloor(num9), RavenMath.ravenFloor(num8));
						}
					}
				}
			}
			this.allInter.Sort(new Comparison<RavenIntersect>(RavenXPathScanner.cmpIntersect));
			this.inter = new int[this.yMax - this.yMin + 2];
			i = 0;
			for (int j = this.yMin; j <= this.yMax; j++)
			{
				this.inter[j - this.yMin] = i;
				while (i < this.allInterLen && this.allInter[i].y <= j)
				{
					i++;
				}
			}
			this.inter[this.yMax - this.yMin + 1] = i;
		}
		private void addIntersection(double segYMin, double segYMax, uint segFlags, int y, int x0, int x1)
		{
			this.allInter.Add(new RavenIntersect());
			this.allInter[this.allInterLen].y = y;
			if (x0 < x1)
			{
				this.allInter[this.allInterLen].x0 = x0;
				this.allInter[this.allInterLen].x1 = x1;
			}
			else
			{
				this.allInter[this.allInterLen].x0 = x1;
				this.allInter[this.allInterLen].x1 = x0;
			}
			if (segYMin <= (double)y && (double)y < segYMax && (segFlags & 1u) == 0u)
			{
				this.allInter[this.allInterLen].count = (this.eo ? 1 : (((segFlags & 4u) != 0u) ? 1 : -1));
			}
			else
			{
				this.allInter[this.allInterLen].count = 0;
			}
			this.allInterLen++;
		}
		internal void renderAALine(RavenBitmap aaBuf, ref int x0, ref int x1, int y)
		{
			byte[] dataPtr = aaBuf.getDataPtr();
			for (int i = 0; i < dataPtr.Length; i++)
			{
				dataPtr[i] = 0;
			}
			int num = aaBuf.getWidth();
			int num2 = -1;
			if (this.yMin <= this.yMax)
			{
				if (RavenConstants.ravenAASize * y < this.yMin)
				{
					this.interIdx = this.inter[0];
				}
				else
				{
					if (RavenConstants.ravenAASize * y > this.yMax)
					{
						this.interIdx = this.inter[this.yMax - this.yMin + 1];
					}
					else
					{
						this.interIdx = this.inter[RavenConstants.ravenAASize * y - this.yMin];
					}
				}
				for (int j = 0; j < RavenConstants.ravenAASize; j++)
				{
					int num3;
					if (RavenConstants.ravenAASize * y + j < this.yMin)
					{
						num3 = this.inter[0];
					}
					else
					{
						if (RavenConstants.ravenAASize * y + j > this.yMax)
						{
							num3 = this.inter[this.yMax - this.yMin + 1];
						}
						else
						{
							num3 = this.inter[RavenConstants.ravenAASize * y + j - this.yMin + 1];
						}
					}
					this.interCount = 0;
					while (this.interIdx < num3)
					{
						int num4 = this.allInter[this.interIdx].x0;
						int num5 = this.allInter[this.interIdx].x1;
						this.interCount += this.allInter[this.interIdx].count;
						this.interIdx++;
						while (this.interIdx < num3 && (this.allInter[this.interIdx].x0 <= num5 || (this.eo ? ((this.interCount & 1) != 0) : (this.interCount != 0))))
						{
							if (this.allInter[this.interIdx].x1 > num5)
							{
								num5 = this.allInter[this.interIdx].x1;
							}
							this.interCount += this.allInter[this.interIdx].count;
							this.interIdx++;
						}
						if (num4 < 0)
						{
							num4 = 0;
						}
						num5++;
						if (num5 > aaBuf.getWidth())
						{
							num5 = aaBuf.getWidth();
						}
						if (num4 < num5)
						{
							int num6 = num4;
							int num7 = j * aaBuf.getRowSize() + (num6 >> 3);
							if ((num6 & 7) != 0)
							{
								byte b = (byte)(255 >> (num6 & 7));
								if ((num6 & -8) == (num5 & -8))
								{
									b &= (byte)(65280 >> (num5 & 7));
								}
								byte[] arg_2A0_0 = dataPtr;
								int expr_29B = num7++;
								arg_2A0_0[expr_29B] |= b;
								num6 = (num6 & -8) + 8;
							}
							while (num6 + 7 < num5)
							{
								byte[] arg_2C6_0 = dataPtr;
								int expr_2C1 = num7++;
								arg_2C6_0[expr_2C1] |= 255;
								num6 += 8;
							}
							if (num6 < num5)
							{
								byte[] expr_2F4_cp_0 = dataPtr;
								int expr_2F4_cp_1 = num7;
								expr_2F4_cp_0[expr_2F4_cp_1] |= (byte)(65280 >> (num5 & 7));
							}
						}
						if (num4 < num)
						{
							num = num4;
						}
						if (num5 > num2)
						{
							num2 = num5;
						}
					}
				}
			}
			x0 = num / RavenConstants.ravenAASize;
			x1 = (num2 - 1) / RavenConstants.ravenAASize;
		}
		internal void clipAALine(RavenBitmap aaBuf, ref int x0, ref int x1, int y)
		{
			byte[] dataPtr = aaBuf.getDataPtr();
			for (int i = 0; i < RavenConstants.ravenAASize; i++)
			{
				int num = x0 * RavenConstants.ravenAASize;
				int num3;
				if (this.yMin <= this.yMax)
				{
					int num2;
					if (RavenConstants.ravenAASize * y + i < this.yMin)
					{
						num2 = (this.interIdx = this.inter[0]);
					}
					else
					{
						if (RavenConstants.ravenAASize * y + i > this.yMax)
						{
							num2 = (this.interIdx = this.inter[this.yMax - this.yMin + 1]);
						}
						else
						{
							this.interIdx = this.inter[RavenConstants.ravenAASize * y + i - this.yMin];
							if (RavenConstants.ravenAASize * y + i > this.yMax)
							{
								num2 = this.inter[this.yMax - this.yMin + 1];
							}
							else
							{
								num2 = this.inter[RavenConstants.ravenAASize * y + i - this.yMin + 1];
							}
						}
					}
					this.interCount = 0;
					while (this.interIdx < num2 && num < (x1 + 1) * RavenConstants.ravenAASize)
					{
						num3 = this.allInter[this.interIdx].x0;
						int x2 = this.allInter[this.interIdx].x1;
						this.interCount += this.allInter[this.interIdx].count;
						this.interIdx++;
						while (this.interIdx < num2 && (this.allInter[this.interIdx].x0 <= x2 || (this.eo ? ((this.interCount & 1) != 0) : (this.interCount != 0))))
						{
							if (this.allInter[this.interIdx].x1 > x2)
							{
								x2 = this.allInter[this.interIdx].x1;
							}
							this.interCount += this.allInter[this.interIdx].count;
							this.interIdx++;
						}
						if (num3 > aaBuf.getWidth())
						{
							num3 = aaBuf.getWidth();
						}
						if (num < num3)
						{
							int num4 = i * aaBuf.getRowSize() + (num >> 3);
							if ((num & 7) != 0)
							{
								byte b = (byte)(65280 >> (num & 7));
								if ((num & -8) == (num3 & -8))
								{
									b |= (byte)(255 >> (num3 & 7));
								}
								byte[] arg_266_0 = dataPtr;
								int expr_261 = num4++;
								arg_266_0[expr_261] &= b;
								num = (num & -8) + 8;
							}
							while (num + 7 < num3)
							{
								dataPtr[num4++] = 0;
								num += 8;
							}
							if (num < num3)
							{
								byte[] expr_2A5_cp_0 = dataPtr;
								int expr_2A5_cp_1 = num4;
								expr_2A5_cp_0[expr_2A5_cp_1] &= (byte)(255 >> (num3 & 7));
							}
						}
						if (x2 >= num)
						{
							num = x2 + 1;
						}
					}
				}
				num3 = (x1 + 1) * RavenConstants.ravenAASize;
				if (num < num3)
				{
					int num4 = i * aaBuf.getRowSize() + (num >> 3);
					if ((num & 7) != 0)
					{
						byte b = (byte)(65280 >> (num & 7));
						if ((num & -8) == (num3 & -8))
						{
							b &= (byte)(255 >> (num3 & 7));
						}
						byte[] arg_33B_0 = dataPtr;
						int expr_336 = num4++;
						arg_33B_0[expr_336] &= b;
						num = (num & -8) + 8;
					}
					while (num + 7 < num3)
					{
						dataPtr[num4++] = 0;
						num += 8;
					}
					if (num < num3)
					{
						byte[] expr_37A_cp_0 = dataPtr;
						int expr_37A_cp_1 = num4;
						expr_37A_cp_0[expr_37A_cp_1] &= (byte)(255 >> (num3 & 7));
					}
				}
			}
		}
	}
}
