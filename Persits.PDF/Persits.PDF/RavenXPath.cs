using System;
using System.Collections.Generic;
namespace Persits.PDF
{
	internal class RavenXPath
	{
		internal const int ravenMaxCurveSplits = 1024;
		internal const byte ravenXPathHoriz = 1;
		internal const byte ravenXPathVert = 2;
		internal const byte ravenXPathFlip = 4;
		internal List<RavenXPathSeg> segs = new List<RavenXPathSeg>();
		internal int length
		{
			get
			{
				if (this.segs == null)
				{
					return 0;
				}
				return this.segs.Count;
			}
		}
		internal void transform(double[] matrix, double xi, double yi, ref double xo, ref double yo)
		{
			xo = xi * matrix[0] + yi * matrix[2] + matrix[4];
			yo = xi * matrix[1] + yi * matrix[3] + matrix[5];
		}
		internal RavenXPath(RavenPath path, double[] matrix, double flatness, bool closeSubpaths)
		{
			RavenXPathPoint[] array = new RavenXPathPoint[path.pts.Count];
			int i;
			for (i = 0; i < path.pts.Count; i++)
			{
				this.transform(matrix, path.pts[i].x, path.pts[i].y, ref array[i].x, ref array[i].y);
			}
			RavenXPathAdjust[] array2;
			double num;
			double num2;
			if (path.hints != null)
			{
				array2 = new RavenXPathAdjust[path.hints.Count];
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j] = new RavenXPathAdjust();
				}
				for (i = 0; i < path.hints.Count; i++)
				{
					RavenPathHint ravenPathHint = path.hints[i];
					num = array[ravenPathHint.ctrl0].x;
					num2 = array[ravenPathHint.ctrl0].y;
					double num3 = array[ravenPathHint.ctrl0 + 1].x;
					double y = array[ravenPathHint.ctrl0 + 1].y;
					double x = array[ravenPathHint.ctrl1].x;
					double y2 = array[ravenPathHint.ctrl1].y;
					double x2 = array[ravenPathHint.ctrl1 + 1].x;
					double y3 = array[ravenPathHint.ctrl1 + 1].y;
					double num4;
					double num5;
					if (num == num3 && x == x2)
					{
						array2[i].vert = true;
						num4 = num;
						num5 = x;
					}
					else
					{
						if (num2 != y || y2 != y3)
						{
							array2 = null;
							break;
						}
						array2[i].vert = false;
						num4 = num2;
						num5 = y2;
					}
					if (num4 > num5)
					{
						num = num4;
						num4 = num5;
						num5 = num;
					}
					array2[i].x0a = num4 - 0.01;
					array2[i].x0b = num4 + 0.01;
					array2[i].xma = 0.5 * (num4 + num5) - 0.01;
					array2[i].xmb = 0.5 * (num4 + num5) + 0.01;
					array2[i].x1a = num5 - 0.01;
					array2[i].x1b = num5 + 0.01;
					num = (double)RavenMath.ravenRound(num4);
					num3 = (double)RavenMath.ravenRound(num5);
					if (num3 == num)
					{
						num3 += 1.0;
					}
					array2[i].x0 = num;
					array2[i].x1 = num3 - 0.01;
					array2[i].xm = 0.5 * (array2[i].x0 + array2[i].x1);
					array2[i].firstPt = ravenPathHint.firstPt;
					array2[i].lastPt = ravenPathHint.lastPt;
				}
			}
			else
			{
				array2 = null;
			}
			if (array2 != null)
			{
				i = 0;
				RavenXPathAdjust ravenXPathAdjust = array2[0];
				while (i < path.hints.Count)
				{
					ravenXPathAdjust = array2[i];
					for (int k = ravenXPathAdjust.firstPt; k <= ravenXPathAdjust.lastPt; k++)
					{
						this.strokeAdjust(ravenXPathAdjust, ref array[k].x, ref array[k].y);
					}
					i++;
				}
			}
			this.segs = null;
			double y4;
			double x3;
			num2 = (num = (x3 = (y4 = 0.0)));
			int num6 = 0;
			i = 0;
			while (i < path.pts.Count)
			{
				if ((path.flags[i] & 1) != 0)
				{
					num = array[i].x;
					num2 = array[i].y;
					x3 = num;
					y4 = num2;
					num6 = i;
					int arg_3E9_0 = this.length;
					i++;
				}
				else
				{
					if ((path.flags[i] & 8) != 0)
					{
						double num3 = array[i].x;
						double y = array[i].y;
						double x = array[i + 1].x;
						double y2 = array[i + 1].y;
						double x2 = array[i + 2].x;
						double y3 = array[i + 2].y;
						this.addCurve(num, num2, num3, y, x, y2, x2, y3, flatness, (path.flags[i - 1] & 1) != 0, (path.flags[i + 2] & 2) != 0, !closeSubpaths && (path.flags[i - 1] & 1) != 0 && (path.flags[i - 1] & 4) == 0, !closeSubpaths && (path.flags[i + 2] & 2) != 0 && (path.flags[i + 2] & 4) == 0);
						num = x2;
						num2 = y3;
						i += 3;
					}
					else
					{
						double num3 = array[i].x;
						double y = array[i].y;
						this.addSegment(num, num2, num3, y);
						num = num3;
						num2 = y;
						i++;
					}
					if (closeSubpaths && (path.flags[i - 1] & 2) != 0 && (array[i - 1].x != array[num6].x || array[i - 1].y != array[num6].y))
					{
						this.addSegment(num, num2, x3, y4);
					}
				}
			}
		}
		private void strokeAdjust(RavenXPathAdjust adjust, ref double xp, ref double yp)
		{
			if (adjust.vert)
			{
				double num = xp;
				if (num > adjust.x0a && num < adjust.x0b)
				{
					xp = adjust.x0;
					return;
				}
				if (num > adjust.xma && num < adjust.xmb)
				{
					xp = adjust.xm;
					return;
				}
				if (num > adjust.x1a && num < adjust.x1b)
				{
					xp = adjust.x1;
					return;
				}
			}
			else
			{
				double num2 = yp;
				if (num2 > adjust.x0a && num2 < adjust.x0b)
				{
					yp = adjust.x0;
					return;
				}
				if (num2 > adjust.xma && num2 < adjust.xmb)
				{
					yp = adjust.xm;
					return;
				}
				if (num2 > adjust.x1a && num2 < adjust.x1b)
				{
					yp = adjust.x1;
				}
			}
		}
		internal RavenXPath(RavenXPath xPath)
		{
			this.segs.AddRange(xPath.segs.ToArray());
		}
		internal void addCurve(double x0, double y0, double x1, double y1, double x2, double y2, double x3, double y3, double flatness, bool first, bool last, bool end0, bool end1)
		{
			double[][] array = new double[1025][];
			double[][] array2 = new double[1025][];
			for (int i = 0; i < 1025; i++)
			{
				array[i] = new double[3];
				array2[i] = new double[3];
			}
			int[] array3 = new int[1025];
			double num = flatness * flatness;
			int j = 0;
			int num2 = 1024;
			array[j][0] = x0;
			array2[j][0] = y0;
			array[j][1] = x1;
			array2[j][1] = y1;
			array[j][2] = x2;
			array2[j][2] = y2;
			array[num2][0] = x3;
			array2[num2][0] = y3;
			array3[j] = num2;
			while (j < 1024)
			{
				double num3 = array[j][0];
				double num4 = array2[j][0];
				double num5 = array[j][1];
				double num6 = array2[j][1];
				double num7 = array[j][2];
				double num8 = array2[j][2];
				num2 = array3[j];
				double num9 = array[num2][0];
				double num10 = array2[num2][0];
				double num11 = (num3 + num9) * 0.5;
				double num12 = (num4 + num10) * 0.5;
				double num13 = num5 - num11;
				double num14 = num6 - num12;
				double num15 = num13 * num13 + num14 * num14;
				num13 = num7 - num11;
				num14 = num8 - num12;
				double num16 = num13 * num13 + num14 * num14;
				if (num2 - j == 1 || (num15 <= num && num16 <= num))
				{
					this.addSegment(num3, num4, num9, num10);
					j = num2;
				}
				else
				{
					double num17 = (num3 + num5) * 0.5;
					double num18 = (num4 + num6) * 0.5;
					double num19 = (num5 + num7) * 0.5;
					double num20 = (num6 + num8) * 0.5;
					double num21 = (num17 + num19) * 0.5;
					double num22 = (num18 + num20) * 0.5;
					double num23 = (num7 + num9) * 0.5;
					double num24 = (num8 + num10) * 0.5;
					double num25 = (num19 + num23) * 0.5;
					double num26 = (num20 + num24) * 0.5;
					double num27 = (num21 + num25) * 0.5;
					double num28 = (num22 + num26) * 0.5;
					int num29 = (j + num2) / 2;
					array[j][1] = num17;
					array2[j][1] = num18;
					array[j][2] = num21;
					array2[j][2] = num22;
					array3[j] = num29;
					array[num29][0] = num27;
					array2[num29][0] = num28;
					array[num29][1] = num25;
					array2[num29][1] = num26;
					array[num29][2] = num23;
					array2[num29][2] = num24;
					array3[num29] = num2;
				}
			}
		}
		internal void grow(int count)
		{
			if (this.segs == null)
			{
				this.segs = new List<RavenXPathSeg>();
			}
		}
		internal void addSegment(double x0, double y0, double x1, double y1)
		{
			this.grow(1);
			this.segs.Add(new RavenXPathSeg(x0, y0, x1, y1));
			int index = this.length - 1;
			this.segs[index].flags = 0u;
			if (y1 == y0)
			{
				this.segs[index].dxdy = (this.segs[index].dydx = 0.0);
				this.segs[index].flags |= 1u;
				if (x1 == x0)
				{
					this.segs[index].flags |= 2u;
				}
			}
			else
			{
				if (x1 == x0)
				{
					this.segs[index].dxdy = (this.segs[index].dydx = 0.0);
					this.segs[index].flags |= 2u;
				}
				else
				{
					this.segs[index].dxdy = (x1 - x0) / (y1 - y0);
					this.segs[index].dydx = 1.0 / this.segs[index].dxdy;
				}
			}
			if (y0 > y1)
			{
				this.segs[index].flags |= 4u;
			}
		}
		internal void aaScale()
		{
			foreach (RavenXPathSeg current in this.segs)
			{
				current.x0 *= (double)RavenConstants.ravenAASize;
				current.y0 *= (double)RavenConstants.ravenAASize;
				current.x1 *= (double)RavenConstants.ravenAASize;
				current.y1 *= (double)RavenConstants.ravenAASize;
			}
		}
		internal RavenXPath copy()
		{
			return new RavenXPath(this);
		}
		private static int cmpXPathSegs(RavenXPathSeg seg0, RavenXPathSeg seg1)
		{
			double num;
			double num2;
			if ((seg0.flags & 4u) != 0u)
			{
				num = seg0.x1;
				num2 = seg0.y1;
			}
			else
			{
				num = seg0.x0;
				num2 = seg0.y0;
			}
			double num3;
			double num4;
			if ((seg1.flags & 4u) != 0u)
			{
				num3 = seg1.x1;
				num4 = seg1.y1;
			}
			else
			{
				num3 = seg1.x0;
				num4 = seg1.y0;
			}
			if (num2 != num4)
			{
				if (num2 <= num4)
				{
					return -1;
				}
				return 1;
			}
			else
			{
				if (num == num3)
				{
					return 0;
				}
				if (num <= num3)
				{
					return -1;
				}
				return 1;
			}
		}
		internal void sort()
		{
			this.segs.Sort(new Comparison<RavenXPathSeg>(RavenXPath.cmpXPathSegs));
		}
	}
}
