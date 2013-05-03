using System;
namespace Persits.PDF
{
	internal class Raven
	{
		private const int ravenAASize = 4;
		private const double ravenAAGamma = 1.5;
		internal const double bezierCircle = 0.55228475;
		internal const double bezierCircle2 = 0.276142375;
		private bool vectorAntialias;
		private bool inShading;
		private RavenBitmap bitmap;
		private RavenState state;
		private RavenBitmap aaBuf;
		private double minLineWidth;
		private byte[] aaGamma = new byte[17];
		private int modXMin;
		private int modYMin;
		private int modXMax;
		private int modYMax;
		private RavenClipResult opClipRes;
		private RavenBitmap alpha0Bitmap;
		private int alpha0X;
		private int alpha0Y;
		private int aaBufY;
		private RavenPipeResultColorCtrl[] pipeResultColorNoAlphaBlend = new RavenPipeResultColorCtrl[]
		{
			RavenPipeResultColorCtrl.ravenPipeResultColorNoAlphaBlendMono,
			RavenPipeResultColorCtrl.ravenPipeResultColorNoAlphaBlendMono,
			RavenPipeResultColorCtrl.ravenPipeResultColorNoAlphaBlendRGB,
			RavenPipeResultColorCtrl.ravenPipeResultColorNoAlphaBlendRGB
		};
		private RavenPipeResultColorCtrl[] pipeResultColorAlphaNoBlend = new RavenPipeResultColorCtrl[]
		{
			RavenPipeResultColorCtrl.ravenPipeResultColorAlphaNoBlendMono,
			RavenPipeResultColorCtrl.ravenPipeResultColorAlphaNoBlendMono,
			RavenPipeResultColorCtrl.ravenPipeResultColorAlphaNoBlendRGB,
			RavenPipeResultColorCtrl.ravenPipeResultColorAlphaNoBlendRGB
		};
		private RavenPipeResultColorCtrl[] pipeResultColorAlphaBlend = new RavenPipeResultColorCtrl[]
		{
			RavenPipeResultColorCtrl.ravenPipeResultColorAlphaBlendMono,
			RavenPipeResultColorCtrl.ravenPipeResultColorAlphaBlendMono,
			RavenPipeResultColorCtrl.ravenPipeResultColorAlphaBlendRGB,
			RavenPipeResultColorCtrl.ravenPipeResultColorAlphaBlendRGB
		};
		internal Raven(RavenBitmap bitmapA, bool vectorAntialiasA, RavenScreenParams screenParams)
		{
			this.bitmap = bitmapA;
			this.vectorAntialias = vectorAntialiasA;
			this.inShading = false;
			this.state = new RavenState(this.bitmap.width, this.bitmap.height, this.vectorAntialias, screenParams);
			if (this.vectorAntialias)
			{
				this.aaBuf = new RavenBitmap(4 * this.bitmap.width, 4, 1, RavenColorMode.ravenModeMono1, false, true);
				for (int i = 0; i <= 16; i++)
				{
					this.aaGamma[i] = (byte)RavenMath.ravenRound(RavenMath.ravenPow((double)i / 16.0, 1.5) * 255.0);
				}
			}
			else
			{
				this.aaBuf = null;
			}
			this.minLineWidth = 0.0;
			this.clearModRegion();
		}
		internal Raven(RavenBitmap bitmapA, bool vectorAntialiasA, RavenScreen screenA)
		{
			this.bitmap = bitmapA;
			this.vectorAntialias = vectorAntialiasA;
			this.inShading = false;
			this.state = new RavenState(this.bitmap.width, this.bitmap.height, this.vectorAntialias, screenA);
			if (this.vectorAntialias)
			{
				this.aaBuf = new RavenBitmap(4 * this.bitmap.width, 4, 1, RavenColorMode.ravenModeMono1, false, true);
				for (int i = 0; i <= 16; i++)
				{
					this.aaGamma[i] = (byte)RavenMath.ravenRound(RavenMath.ravenPow((double)i / 16.0, 1.5) * 255.0);
				}
			}
			else
			{
				this.aaBuf = null;
			}
			this.minLineWidth = 0.0;
			this.clearModRegion();
		}
		internal void setMinLineWidth(double w)
		{
			this.minLineWidth = w;
		}
		internal void setMatrix(double[] matrix)
		{
			Array.Copy(matrix, this.state.matrix, 6);
		}
		internal void setStrokePattern(RavenPattern strokePattern)
		{
			this.state.setStrokePattern(strokePattern);
		}
		internal void setFillPattern(RavenPattern fillPattern)
		{
			this.state.setFillPattern(fillPattern);
		}
		internal void setScreen(RavenScreen screen)
		{
			this.state.setScreen(screen);
		}
		internal void setBlendFunc(RavenBlendFunc func)
		{
			this.state.blendFunc = func;
		}
		internal void setStrokeAlpha(double alpha)
		{
			this.state.strokeAlpha = alpha;
		}
		internal void setFillAlpha(double alpha)
		{
			this.state.fillAlpha = alpha;
		}
		internal void setLineWidth(double lineWidth)
		{
			this.state.lineWidth = lineWidth;
		}
		internal void setLineCap(int lineCap)
		{
			this.state.lineCap = lineCap;
		}
		internal void setLineJoin(int lineJoin)
		{
			this.state.lineJoin = lineJoin;
		}
		internal void setMiterLimit(double miterLimit)
		{
			this.state.miterLimit = miterLimit;
		}
		internal void setFlatness(double flatness)
		{
			if (flatness < 1.0)
			{
				this.state.flatness = 1.0;
				return;
			}
			this.state.flatness = flatness;
		}
		internal void setLineDash(double[] lineDash, int lineDashLength, double lineDashPhase)
		{
			this.state.setLineDash(lineDash, lineDashLength, lineDashPhase);
		}
		internal void setStrokeAdjust(bool strokeAdjust)
		{
			this.state.strokeAdjust = strokeAdjust;
		}
		internal void clear(RavenColor color)
		{
			this.clear(color, 0);
		}
		internal void clear(RavenColor color, byte alpha)
		{
			switch (this.bitmap.mode)
			{
			case RavenColorMode.ravenModeMono1:
			{
				byte val = ((color[0] & 128) != 0) ? byte.MaxValue : (byte)0;
				if (this.bitmap.rowSize < 0)
				{
					RavenTypes.MemSet(this.bitmap.data, this.bitmap.rowSize * (this.bitmap.height - 1), val, -this.bitmap.rowSize * this.bitmap.height);
				}
				else
				{
					RavenTypes.MemSet(this.bitmap.data, 0, val, this.bitmap.rowSize * this.bitmap.height);
				}
				break;
			}
			case RavenColorMode.ravenModeMono8:
				if (this.bitmap.rowSize < 0)
				{
					RavenTypes.MemSet(this.bitmap.data, this.bitmap.rowSize * (this.bitmap.height - 1), color[0], -this.bitmap.rowSize * this.bitmap.height);
				}
				else
				{
					RavenTypes.MemSet(this.bitmap.data, 0, color[0], this.bitmap.rowSize * this.bitmap.height);
				}
				break;
			case RavenColorMode.ravenModeRGB8:
				if (color[0] == color[1] && color[1] == color[2])
				{
					if (this.bitmap.rowSize < 0)
					{
						RavenTypes.MemSet(this.bitmap.data, this.bitmap.rowSize * (this.bitmap.height - 1), color[0], -this.bitmap.rowSize * this.bitmap.height);
					}
					else
					{
						RavenTypes.MemSet(this.bitmap.data, 0, color[0], this.bitmap.rowSize * this.bitmap.height);
					}
				}
				else
				{
					int num = 0;
					for (int i = 0; i < this.bitmap.height; i++)
					{
						int num2 = num;
						for (int j = 0; j < this.bitmap.width; j++)
						{
							this.bitmap.data[num2++] = color[2];
							this.bitmap.data[num2++] = color[1];
							this.bitmap.data[num2++] = color[0];
						}
						num += this.bitmap.rowSize;
					}
				}
				break;
			case RavenColorMode.ravenModeBGR8:
				if (color[0] == color[1] && color[1] == color[2])
				{
					if (this.bitmap.rowSize < 0)
					{
						RavenTypes.MemSet(this.bitmap.data, this.bitmap.rowSize * (this.bitmap.height - 1), color[0], -this.bitmap.rowSize * this.bitmap.height);
					}
					else
					{
						RavenTypes.MemSet(this.bitmap.data, 0, color[0], this.bitmap.rowSize * this.bitmap.height);
					}
				}
				else
				{
					int num = 0;
					for (int i = 0; i < this.bitmap.height; i++)
					{
						int num2 = num;
						for (int j = 0; j < this.bitmap.width; j++)
						{
							this.bitmap.data[num2++] = color[0];
							this.bitmap.data[num2++] = color[1];
							this.bitmap.data[num2++] = color[2];
						}
						num += this.bitmap.rowSize;
					}
				}
				break;
			}
			if (this.bitmap.alpha != null)
			{
				RavenTypes.MemSet(this.bitmap.alpha, 0, alpha, this.bitmap.width * this.bitmap.height);
			}
			this.updateModX(0);
			this.updateModY(0);
			this.updateModX(this.bitmap.width - 1);
			this.updateModY(this.bitmap.height - 1);
		}
		internal RavenError stroke(RavenPath path)
		{
			this.opClipRes = RavenClipResult.ravenClipAllOutside;
			if (path.length == 0)
			{
				return RavenError.ravenErrEmptyPath;
			}
			RavenPath ravenPath = this.flattenPath(path, this.state.matrix, this.state.flatness);
			if (this.state.lineDashLength > 0)
			{
				RavenPath ravenPath2 = this.makeDashedPath(ravenPath);
				ravenPath = ravenPath2;
				if (ravenPath.length == 0)
				{
					return RavenError.ravenErrEmptyPath;
				}
			}
			double num = this.state.matrix[0] + this.state.matrix[2];
			double num2 = this.state.matrix[1] + this.state.matrix[3];
			double num3 = num * num + num2 * num2;
			num = this.state.matrix[0] - this.state.matrix[2];
			num2 = this.state.matrix[1] - this.state.matrix[3];
			double num4 = num * num + num2 * num2;
			if (num4 > num3)
			{
				num3 = num4;
			}
			num3 *= 0.5;
			if (num3 > 0.0 && num3 * this.state.lineWidth * this.state.lineWidth < this.minLineWidth * this.minLineWidth)
			{
				double w = this.minLineWidth / RavenMath.ravenSqrt(num3);
				this.strokeWide(ravenPath, w);
			}
			else
			{
				if (this.bitmap.mode == RavenColorMode.ravenModeMono1)
				{
					if (num3 <= 2.0)
					{
						this.strokeNarrow(ravenPath);
					}
					else
					{
						this.strokeWide(ravenPath, this.state.lineWidth);
					}
				}
				else
				{
					if (this.state.lineWidth == 0.0)
					{
						this.strokeNarrow(ravenPath);
					}
					else
					{
						this.strokeWide(ravenPath, this.state.lineWidth);
					}
				}
			}
			return RavenError.ravenOk;
		}
		private RavenPath flattenPath(RavenPath path, double[] matrix, double flatness)
		{
			RavenPath ravenPath = new RavenPath();
			double flatness2 = flatness * flatness;
			int i = 0;
			while (i < path.length)
			{
				byte b = path.flags[i];
				if ((b & 1) != 0)
				{
					ravenPath.moveTo(path.pts[i].x, path.pts[i].y);
					i++;
				}
				else
				{
					if ((b & 8) != 0)
					{
						this.flattenCurve(path.pts[i - 1].x, path.pts[i - 1].y, path.pts[i].x, path.pts[i].y, path.pts[i + 1].x, path.pts[i + 1].y, path.pts[i + 2].x, path.pts[i + 2].y, matrix, flatness2, ravenPath);
						i += 3;
					}
					else
					{
						ravenPath.lineTo(path.pts[i].x, path.pts[i].y);
						i++;
					}
					if ((path.flags[i - 1] & 4) != 0)
					{
						ravenPath.close();
					}
				}
			}
			return ravenPath;
		}
		private void flattenCurve(double x0, double y0, double x1, double y1, double x2, double y2, double x3, double y3, double[] matrix, double flatness2, RavenPath fPath)
		{
			double[][] array = new double[1025][];
			double[][] array2 = new double[1025][];
			for (int i = 0; i < 1025; i++)
			{
				array[i] = new double[3];
				array2[i] = new double[3];
			}
			int[] array3 = new int[1025];
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			int j = 0;
			int num5 = 1024;
			array[j][0] = x0;
			array2[j][0] = y0;
			array[j][1] = x1;
			array2[j][1] = y1;
			array[j][2] = x2;
			array2[j][2] = y2;
			array[num5][0] = x3;
			array2[num5][0] = y3;
			array3[j] = num5;
			while (j < 1024)
			{
				double num6 = array[j][0];
				double num7 = array2[j][0];
				double num8 = array[j][1];
				double num9 = array2[j][1];
				double num10 = array[j][2];
				double num11 = array2[j][2];
				num5 = array3[j];
				double num12 = array[num5][0];
				double num13 = array2[num5][0];
				this.transform(matrix, (num6 + num12) * 0.5, (num7 + num13) * 0.5, ref num, ref num2);
				this.transform(matrix, num8, num9, ref num3, ref num4);
				double num14 = num3 - num;
				double num15 = num4 - num2;
				double num16 = num14 * num14 + num15 * num15;
				this.transform(matrix, num10, num11, ref num3, ref num4);
				num14 = num3 - num;
				num15 = num4 - num2;
				double num17 = num14 * num14 + num15 * num15;
				if (num5 - j == 1 || (num16 <= flatness2 && num17 <= flatness2))
				{
					fPath.lineTo(num12, num13);
					j = num5;
				}
				else
				{
					double num18 = RavenMath.ravenAvg(num6, num8);
					double num19 = RavenMath.ravenAvg(num7, num9);
					double num20 = RavenMath.ravenAvg(num8, num10);
					double num21 = RavenMath.ravenAvg(num9, num11);
					double num22 = RavenMath.ravenAvg(num18, num20);
					double num23 = RavenMath.ravenAvg(num19, num21);
					double num24 = RavenMath.ravenAvg(num10, num12);
					double num25 = RavenMath.ravenAvg(num11, num13);
					double num26 = RavenMath.ravenAvg(num20, num24);
					double num27 = RavenMath.ravenAvg(num21, num25);
					double num28 = RavenMath.ravenAvg(num22, num26);
					double num29 = RavenMath.ravenAvg(num23, num27);
					int num30 = (j + num5) / 2;
					array[j][1] = num18;
					array2[j][1] = num19;
					array[j][2] = num22;
					array2[j][2] = num23;
					array3[j] = num30;
					array[num30][0] = num28;
					array2[num30][0] = num29;
					array[num30][1] = num26;
					array2[num30][1] = num27;
					array[num30][2] = num24;
					array2[num30][2] = num25;
					array3[num30] = num5;
				}
			}
		}
		private RavenPath makeDashedPath(RavenPath path)
		{
			double num = 0.0;
			int i;
			for (i = 0; i < this.state.lineDashLength; i++)
			{
				num += this.state.lineDash[i];
			}
			if (num == 0.0)
			{
				return new RavenPath();
			}
			double num2 = this.state.lineDashPhase;
			i = RavenMath.ravenFloor(num2 / num);
			num2 -= (double)i * num;
			bool flag = true;
			int num3 = 0;
			if (num2 > 0.0)
			{
				while (num2 >= this.state.lineDash[num3])
				{
					flag = !flag;
					num2 -= this.state.lineDash[num3];
					num3++;
				}
			}
			RavenPath ravenPath = new RavenPath();
			int num4;
			for (i = 0; i < path.length; i = num4 + 1)
			{
				num4 = i;
				while (num4 < path.length - 1 && (path.flags[num4] & 2) == 0)
				{
					num4++;
				}
				bool flag2 = flag;
				int num5 = num3;
				double num6 = this.state.lineDash[num5] - num2;
				bool flag3 = true;
				for (int j = i; j < num4; j++)
				{
					double num7 = path.pts[j].x;
					double num8 = path.pts[j].y;
					double x = path.pts[j + 1].x;
					double y = path.pts[j + 1].y;
					double num9 = RavenMath.ravenDist(num7, num8, x, y);
					while (num9 > 0.0)
					{
						if (num6 >= num9)
						{
							if (flag2)
							{
								if (flag3)
								{
									ravenPath.moveTo(num7, num8);
									flag3 = false;
								}
								ravenPath.lineTo(x, y);
							}
							num6 -= num9;
							num9 = 0.0;
						}
						else
						{
							double num10 = num7 + num6 / num9 * (x - num7);
							double num11 = num8 + num6 / num9 * (y - num8);
							if (flag2)
							{
								if (flag3)
								{
									ravenPath.moveTo(num7, num8);
									flag3 = false;
								}
								ravenPath.lineTo(num10, num11);
							}
							num7 = num10;
							num8 = num11;
							num9 -= num6;
							num6 = 0.0;
						}
						if (num6 <= 0.0)
						{
							flag2 = !flag2;
							if (++num5 == this.state.lineDashLength)
							{
								num5 = 0;
							}
							num6 = this.state.lineDash[num5];
							flag3 = true;
						}
					}
				}
			}
			return ravenPath;
		}
		internal RavenError fill(RavenPath path, bool eo)
		{
			return this.fillWithPattern(path, eo, this.state.fillPattern, this.state.fillAlpha);
		}
		private void drawPixel(RavenPipe pipe, int x, int y, bool noClip)
		{
			if (noClip || this.state.clip.test(x, y))
			{
				this.pipeSetXY(pipe, x, y);
				pipe.run(pipe);
				this.updateModX(x);
				this.updateModY(y);
			}
		}
		private void strokeNarrow(RavenPath path)
		{
			RavenPipe pipe = new RavenPipe();
			int[] array = new int[3];
			array[0] = (array[1] = (array[2] = 0));
			RavenXPath ravenXPath = new RavenXPath(path, this.state.matrix, this.state.flatness, false);
			this.pipeInit(pipe, 0, 0, this.state.strokePattern, null, (byte)RavenMath.ravenRound(this.state.strokeAlpha * 255.0), false, false);
			for (int i = 0; i < ravenXPath.length; i++)
			{
				RavenXPathSeg ravenXPathSeg = ravenXPath.segs[i];
				int num;
				int num2;
				int num3;
				int num4;
				if (ravenXPathSeg.y0 <= ravenXPathSeg.y1)
				{
					num = RavenMath.ravenFloor(ravenXPathSeg.y0);
					num2 = RavenMath.ravenFloor(ravenXPathSeg.y1);
					num3 = RavenMath.ravenFloor(ravenXPathSeg.x0);
					num4 = RavenMath.ravenFloor(ravenXPathSeg.x1);
				}
				else
				{
					num = RavenMath.ravenFloor(ravenXPathSeg.y1);
					num2 = RavenMath.ravenFloor(ravenXPathSeg.y0);
					num3 = RavenMath.ravenFloor(ravenXPathSeg.x1);
					num4 = RavenMath.ravenFloor(ravenXPathSeg.x0);
				}
				RavenClipResult ravenClipResult;
				if ((ravenClipResult = this.state.clip.testRect((num3 <= num4) ? num3 : num4, num, (num3 <= num4) ? num4 : num3, num2)) != RavenClipResult.ravenClipAllOutside)
				{
					if (num == num2)
					{
						if (num3 <= num4)
						{
							this.drawSpan(pipe, num3, num4, num, ravenClipResult == RavenClipResult.ravenClipAllInside);
						}
						else
						{
							this.drawSpan(pipe, num4, num3, num, ravenClipResult == RavenClipResult.ravenClipAllInside);
						}
					}
					else
					{
						double dxdy = ravenXPathSeg.dxdy;
						if (num < this.state.clip.getYMinI())
						{
							num = this.state.clip.getYMinI();
							num3 = RavenMath.ravenFloor(ravenXPathSeg.x0 + ((double)num - ravenXPathSeg.y0) * dxdy);
						}
						if (num2 > this.state.clip.getYMaxI())
						{
							num2 = this.state.clip.getYMaxI();
							num4 = RavenMath.ravenFloor(ravenXPathSeg.x0 + ((double)num2 - ravenXPathSeg.y0) * dxdy);
						}
						if (num3 <= num4)
						{
							int num5 = num3;
							for (int j = num; j <= num2; j++)
							{
								int num6;
								if (j < num2)
								{
									num6 = RavenMath.ravenFloor(ravenXPathSeg.x0 + ((double)j + 1.0 - ravenXPathSeg.y0) * dxdy);
								}
								else
								{
									num6 = num4 + 1;
								}
								if (num5 == num6)
								{
									this.drawPixel(pipe, num5, j, ravenClipResult == RavenClipResult.ravenClipAllInside);
								}
								else
								{
									this.drawSpan(pipe, num5, num6 - 1, j, ravenClipResult == RavenClipResult.ravenClipAllInside);
								}
								num5 = num6;
							}
						}
						else
						{
							int num5 = num3;
							for (int j = num; j <= num2; j++)
							{
								int num6;
								if (j < num2)
								{
									num6 = RavenMath.ravenFloor(ravenXPathSeg.x0 + ((double)j + 1.0 - ravenXPathSeg.y0) * dxdy);
								}
								else
								{
									num6 = num4 - 1;
								}
								if (num5 == num6)
								{
									this.drawPixel(pipe, num5, j, ravenClipResult == RavenClipResult.ravenClipAllInside);
								}
								else
								{
									this.drawSpan(pipe, num6 + 1, num5, j, ravenClipResult == RavenClipResult.ravenClipAllInside);
								}
								num5 = num6;
							}
						}
					}
				}
				array[(int)ravenClipResult]++;
			}
			if (array[2] != 0 || (array[0] != 0 && array[1] != 0))
			{
				this.opClipRes = RavenClipResult.ravenClipPartial;
				return;
			}
			if (array[0] != 0)
			{
				this.opClipRes = RavenClipResult.ravenClipAllInside;
				return;
			}
			this.opClipRes = RavenClipResult.ravenClipAllOutside;
		}
		internal void strokeWide(RavenPath path, double w)
		{
			RavenPath path2 = this.makeStrokePath(path, w, false);
			this.fillWithPattern(path2, false, this.state.strokePattern, this.state.strokeAlpha);
		}
		internal RavenPath makeStrokePath(RavenPath path, double w)
		{
			return this.makeStrokePath(path, w, true);
		}
		internal RavenPath makeStrokePath(RavenPath path, double w, bool flatten)
		{
			RavenPath ravenPath = new RavenPath();
			if (path.length == 0)
			{
				return ravenPath;
			}
			RavenPath ravenPath2;
			if (flatten)
			{
				ravenPath2 = this.flattenPath(path, this.state.matrix, this.state.flatness);
				if (this.state.lineDashLength > 0)
				{
					RavenPath ravenPath3 = this.makeDashedPath(ravenPath2);
					ravenPath2 = ravenPath3;
					if (ravenPath2.length == 0)
					{
						return ravenPath;
					}
				}
			}
			else
			{
				ravenPath2 = path;
			}
			int num2;
			int num = num2 = 0;
			int num3 = 0;
			bool flag = false;
			int num7;
			int firstPt;
			int num6;
			int lastPt;
			int num5;
			int num4 = num5 = (lastPt = (num6 = (firstPt = (num7 = 0))));
			int num10;
			int num9;
			int num8 = num9 = (num10 = 0);
			int num11 = 0;
			int i = num11;
			while ((ravenPath2.flags[i] & 2) == 0 && i + 1 < ravenPath2.length && ravenPath2.pts[i + 1].x == ravenPath2.pts[i].x)
			{
				if (ravenPath2.pts[i + 1].y != ravenPath2.pts[i].y)
				{
					break;
				}
				i++;
			}
			while (i < ravenPath2.length)
			{
				bool flag2;
				if (flag2 = ((ravenPath2.flags[num11] & 1) != 0))
				{
					num2 = num11;
					num = i;
					num3 = 0;
					flag = ((ravenPath2.flags[num11] & 4) != 0);
				}
				int num12 = i + 1;
				int num13;
				if (num12 < ravenPath2.length)
				{
					num13 = num12;
					while ((ravenPath2.flags[num13] & 2) == 0 && num13 + 1 < ravenPath2.length && ravenPath2.pts[num13 + 1].x == ravenPath2.pts[num13].x)
					{
						if (ravenPath2.pts[num13 + 1].y != ravenPath2.pts[num13].y)
						{
							break;
						}
						num13++;
					}
				}
				else
				{
					num13 = num12;
				}
				if ((ravenPath2.flags[i] & 2) != 0)
				{
					if (flag2 && this.state.lineCap == 1)
					{
						ravenPath.moveTo(ravenPath2.pts[num11].x + 0.5 * w, ravenPath2.pts[num11].y);
						ravenPath.curveTo(ravenPath2.pts[num11].x + 0.5 * w, ravenPath2.pts[num11].y + 0.276142375 * w, ravenPath2.pts[num11].x + 0.276142375 * w, ravenPath2.pts[num11].y + 0.5 * w, ravenPath2.pts[num11].x, ravenPath2.pts[num11].y + 0.5 * w);
						ravenPath.curveTo(ravenPath2.pts[num11].x - 0.276142375 * w, ravenPath2.pts[num11].y + 0.5 * w, ravenPath2.pts[num11].x - 0.5 * w, ravenPath2.pts[num11].y + 0.276142375 * w, ravenPath2.pts[num11].x - 0.5 * w, ravenPath2.pts[num11].y);
						ravenPath.curveTo(ravenPath2.pts[num11].x - 0.5 * w, ravenPath2.pts[num11].y - 0.276142375 * w, ravenPath2.pts[num11].x - 0.276142375 * w, ravenPath2.pts[num11].y - 0.5 * w, ravenPath2.pts[num11].x, ravenPath2.pts[num11].y - 0.5 * w);
						ravenPath.curveTo(ravenPath2.pts[num11].x + 0.276142375 * w, ravenPath2.pts[num11].y - 0.5 * w, ravenPath2.pts[num11].x + 0.5 * w, ravenPath2.pts[num11].y - 0.276142375 * w, ravenPath2.pts[num11].x + 0.5 * w, ravenPath2.pts[num11].y);
						ravenPath.close();
					}
					num11 = num12;
					i = num13;
				}
				else
				{
					bool flag3 = (ravenPath2.flags[num13] & 2) != 0;
					int num14;
					if (flag3)
					{
						num14 = num + 1;
					}
					else
					{
						num14 = num13 + 1;
					}
					int num15 = num14;
					while ((ravenPath2.flags[num15] & 2) == 0 && num15 + 1 < ravenPath2.length && ravenPath2.pts[num15 + 1].x == ravenPath2.pts[num15].x && ravenPath2.pts[num15 + 1].y == ravenPath2.pts[num15].y)
					{
						num15++;
					}
					double num16 = 1.0 / RavenMath.ravenDist(ravenPath2.pts[i].x, ravenPath2.pts[i].y, ravenPath2.pts[num12].x, ravenPath2.pts[num12].y);
					double num17 = num16 * (ravenPath2.pts[num12].x - ravenPath2.pts[i].x);
					double num18 = num16 * (ravenPath2.pts[num12].y - ravenPath2.pts[i].y);
					double num19 = 0.5 * w * num17;
					double num20 = 0.5 * w * num18;
					ravenPath.moveTo(ravenPath2.pts[num11].x - num20, ravenPath2.pts[num11].y + num19);
					if (num11 == num2)
					{
						num10 = ravenPath.length - 1;
					}
					if (flag2 && !flag)
					{
						switch (this.state.lineCap)
						{
						case 0:
							ravenPath.lineTo(ravenPath2.pts[num11].x + num20, ravenPath2.pts[num11].y - num19);
							break;
						case 1:
							ravenPath.curveTo(ravenPath2.pts[num11].x - num20 - 0.55228475 * num19, ravenPath2.pts[num11].y + num19 - 0.55228475 * num20, ravenPath2.pts[num11].x - num19 - 0.55228475 * num20, ravenPath2.pts[num11].y - num20 + 0.55228475 * num19, ravenPath2.pts[num11].x - num19, ravenPath2.pts[num11].y - num20);
							ravenPath.curveTo(ravenPath2.pts[num11].x - num19 + 0.55228475 * num20, ravenPath2.pts[num11].y - num20 - 0.55228475 * num19, ravenPath2.pts[num11].x + num20 - 0.55228475 * num19, ravenPath2.pts[num11].y - num19 - 0.55228475 * num20, ravenPath2.pts[num11].x + num20, ravenPath2.pts[num11].y - num19);
							break;
						case 2:
							ravenPath.lineTo(ravenPath2.pts[num11].x - num19 - num20, ravenPath2.pts[num11].y + num19 - num20);
							ravenPath.lineTo(ravenPath2.pts[num11].x - num19 + num20, ravenPath2.pts[num11].y - num19 - num20);
							ravenPath.lineTo(ravenPath2.pts[num11].x + num20, ravenPath2.pts[num11].y - num19);
							break;
						}
					}
					else
					{
						ravenPath.lineTo(ravenPath2.pts[num11].x + num20, ravenPath2.pts[num11].y - num19);
					}
					int num21 = ravenPath.length - 1;
					ravenPath.lineTo(ravenPath2.pts[num12].x + num20, ravenPath2.pts[num12].y - num19);
					if (flag3 && !flag)
					{
						switch (this.state.lineCap)
						{
						case 0:
							ravenPath.lineTo(ravenPath2.pts[num12].x - num20, ravenPath2.pts[num12].y + num19);
							break;
						case 1:
							ravenPath.curveTo(ravenPath2.pts[num12].x + num20 + 0.55228475 * num19, ravenPath2.pts[num12].y - num19 + 0.55228475 * num20, ravenPath2.pts[num12].x + num19 + 0.55228475 * num20, ravenPath2.pts[num12].y + num20 - 0.55228475 * num19, ravenPath2.pts[num12].x + num19, ravenPath2.pts[num12].y + num20);
							ravenPath.curveTo(ravenPath2.pts[num12].x + num19 - 0.55228475 * num20, ravenPath2.pts[num12].y + num20 + 0.55228475 * num19, ravenPath2.pts[num12].x - num20 + 0.55228475 * num19, ravenPath2.pts[num12].y + num19 + 0.55228475 * num20, ravenPath2.pts[num12].x - num20, ravenPath2.pts[num12].y + num19);
							break;
						case 2:
							ravenPath.lineTo(ravenPath2.pts[num12].x + num20 + num19, ravenPath2.pts[num12].y - num19 + num20);
							ravenPath.lineTo(ravenPath2.pts[num12].x - num20 + num19, ravenPath2.pts[num12].y + num19 + num20);
							ravenPath.lineTo(ravenPath2.pts[num12].x - num20, ravenPath2.pts[num12].y + num19);
							break;
						}
					}
					else
					{
						ravenPath.lineTo(ravenPath2.pts[num12].x - num20, ravenPath2.pts[num12].y + num19);
					}
					int num22 = ravenPath.length - 1;
					ravenPath.close(this.state.strokeAdjust);
					int length = ravenPath.length;
					if (!flag3 || flag)
					{
						num16 = 1.0 / RavenMath.ravenDist(ravenPath2.pts[num13].x, ravenPath2.pts[num13].y, ravenPath2.pts[num14].x, ravenPath2.pts[num14].y);
						double num23 = num16 * (ravenPath2.pts[num14].x - ravenPath2.pts[num13].x);
						double num24 = num16 * (ravenPath2.pts[num14].y - ravenPath2.pts[num13].y);
						double num25 = 0.5 * w * num23;
						double num26 = 0.5 * w * num24;
						double num27 = num17 * num24 - num18 * num23;
						double num28 = -(num17 * num23 + num18 * num24);
						double num29;
						double num30;
						if (num28 > 0.9999)
						{
							num29 = (this.state.miterLimit + 1.0) * (this.state.miterLimit + 1.0);
							num30 = 0.0;
						}
						else
						{
							num29 = 2.0 / (1.0 - num28);
							if (num29 < 1.0)
							{
								num29 = 1.0;
							}
							num30 = RavenMath.ravenSqrt(num29 - 1.0);
						}
						if (this.state.lineJoin == 1)
						{
							ravenPath.moveTo(ravenPath2.pts[num12].x + 0.5 * w, ravenPath2.pts[num12].y);
							ravenPath.curveTo(ravenPath2.pts[num12].x + 0.5 * w, ravenPath2.pts[num12].y + 0.276142375 * w, ravenPath2.pts[num12].x + 0.276142375 * w, ravenPath2.pts[num12].y + 0.5 * w, ravenPath2.pts[num12].x, ravenPath2.pts[num12].y + 0.5 * w);
							ravenPath.curveTo(ravenPath2.pts[num12].x - 0.276142375 * w, ravenPath2.pts[num12].y + 0.5 * w, ravenPath2.pts[num12].x - 0.5 * w, ravenPath2.pts[num12].y + 0.276142375 * w, ravenPath2.pts[num12].x - 0.5 * w, ravenPath2.pts[num12].y);
							ravenPath.curveTo(ravenPath2.pts[num12].x - 0.5 * w, ravenPath2.pts[num12].y - 0.276142375 * w, ravenPath2.pts[num12].x - 0.276142375 * w, ravenPath2.pts[num12].y - 0.5 * w, ravenPath2.pts[num12].x, ravenPath2.pts[num12].y - 0.5 * w);
							ravenPath.curveTo(ravenPath2.pts[num12].x + 0.276142375 * w, ravenPath2.pts[num12].y - 0.5 * w, ravenPath2.pts[num12].x + 0.5 * w, ravenPath2.pts[num12].y - 0.276142375 * w, ravenPath2.pts[num12].x + 0.5 * w, ravenPath2.pts[num12].y);
						}
						else
						{
							ravenPath.moveTo(ravenPath2.pts[num12].x, ravenPath2.pts[num12].y);
							if (num27 < 0.0)
							{
								ravenPath.lineTo(ravenPath2.pts[num12].x - num26, ravenPath2.pts[num12].y + num25);
								if (this.state.lineJoin == 0 && RavenMath.ravenSqrt(num29) <= this.state.miterLimit)
								{
									ravenPath.lineTo(ravenPath2.pts[num12].x - num20 + num19 * num30, ravenPath2.pts[num12].y + num19 + num20 * num30);
									ravenPath.lineTo(ravenPath2.pts[num12].x - num20, ravenPath2.pts[num12].y + num19);
								}
								else
								{
									ravenPath.lineTo(ravenPath2.pts[num12].x - num20, ravenPath2.pts[num12].y + num19);
								}
							}
							else
							{
								ravenPath.lineTo(ravenPath2.pts[num12].x + num20, ravenPath2.pts[num12].y - num19);
								if (this.state.lineJoin == 0 && RavenMath.ravenSqrt(num29) <= this.state.miterLimit)
								{
									ravenPath.lineTo(ravenPath2.pts[num12].x + num20 + num19 * num30, ravenPath2.pts[num12].y - num19 + num20 * num30);
									ravenPath.lineTo(ravenPath2.pts[num12].x + num26, ravenPath2.pts[num12].y - num25);
								}
								else
								{
									ravenPath.lineTo(ravenPath2.pts[num12].x + num26, ravenPath2.pts[num12].y - num25);
								}
							}
						}
						ravenPath.close();
					}
					if (this.state.strokeAdjust)
					{
						if (num3 == 0 && !flag)
						{
							if (this.state.lineCap == 0)
							{
								ravenPath.addStrokeAdjustHint(num10, num21 + 1, num10, num10 + 1);
								if (flag3)
								{
									ravenPath.addStrokeAdjustHint(num10, num21 + 1, num21 + 1, num21 + 2);
								}
							}
							else
							{
								if (this.state.lineCap == 2)
								{
									if (flag3)
									{
										ravenPath.addStrokeAdjustHint(num10 + 1, num21 + 2, num10 + 1, num10 + 2);
										ravenPath.addStrokeAdjustHint(num10 + 1, num21 + 2, num21 + 2, num21 + 3);
									}
									else
									{
										ravenPath.addStrokeAdjustHint(num10 + 1, num21 + 1, num10 + 1, num10 + 2);
									}
								}
							}
						}
						if (num3 >= 1)
						{
							if (num3 >= 2)
							{
								ravenPath.addStrokeAdjustHint(num4, num6, num5 + 1, lastPt);
								ravenPath.addStrokeAdjustHint(num4, num6, firstPt, num21);
							}
							else
							{
								ravenPath.addStrokeAdjustHint(num4, num6, num10, num21);
							}
							ravenPath.addStrokeAdjustHint(num4, num6, num22 + 1, num22 + 1);
						}
						num5 = num4;
						num4 = num21;
						lastPt = num6;
						num6 = num22;
						firstPt = num7;
						num7 = length;
						if (num3 == 0)
						{
							num9 = num21;
							num8 = num22;
						}
						if (flag3)
						{
							if (num3 >= 2)
							{
								ravenPath.addStrokeAdjustHint(num4, num6, num5 + 1, lastPt);
								ravenPath.addStrokeAdjustHint(num4, num6, firstPt, ravenPath.length - 1);
							}
							else
							{
								ravenPath.addStrokeAdjustHint(num4, num6, num10, ravenPath.length - 1);
							}
							if (flag)
							{
								ravenPath.addStrokeAdjustHint(num4, num6, num10, num9);
								ravenPath.addStrokeAdjustHint(num4, num6, num8 + 1, num8 + 1);
								ravenPath.addStrokeAdjustHint(num9, num8, num4 + 1, num6);
								ravenPath.addStrokeAdjustHint(num9, num8, num7, ravenPath.length - 1);
							}
							if (!flag && num3 > 0)
							{
								if (this.state.lineCap == 0)
								{
									ravenPath.addStrokeAdjustHint(num4 - 1, num4 + 1, num4 + 1, num4 + 2);
								}
								else
								{
									if (this.state.lineCap == 2)
									{
										ravenPath.addStrokeAdjustHint(num4 - 1, num4 + 2, num4 + 2, num4 + 3);
									}
								}
							}
						}
					}
					num11 = num12;
					i = num13;
					num3++;
				}
			}
			return ravenPath;
		}
		internal void drawAALine(RavenPipe pipe, int x0, int x1, int y)
		{
			int[] array = new int[]
			{
				0,
				1,
				1,
				2,
				1,
				2,
				2,
				3,
				1,
				2,
				2,
				3,
				2,
				3,
				3,
				4
			};
			RavenColorPtr ravenColorPtr = new RavenColorPtr(this.aaBuf.getDataPtr(), x0 >> 1);
			RavenColorPtr ravenColorPtr2 = new RavenColorPtr(ravenColorPtr, this.aaBuf.getRowSize());
			RavenColorPtr ravenColorPtr3 = new RavenColorPtr(ravenColorPtr2, this.aaBuf.getRowSize());
			RavenColorPtr ravenColorPtr4 = new RavenColorPtr(ravenColorPtr3, this.aaBuf.getRowSize());
			this.pipeSetXY(pipe, x0, y);
			for (int i = x0; i <= x1; i++)
			{
				int num;
				if ((i & 1) != 0)
				{
					num = array[(int)(ravenColorPtr.ptr & 15)] + array[(int)(ravenColorPtr2.ptr & 15)] + array[(int)(ravenColorPtr3.ptr & 15)] + array[(int)(ravenColorPtr4.ptr & 15)];
					ravenColorPtr = ++ravenColorPtr;
					ravenColorPtr2 = ++ravenColorPtr2;
					ravenColorPtr3 = ++ravenColorPtr3;
					ravenColorPtr4 = ++ravenColorPtr4;
				}
				else
				{
					num = array[ravenColorPtr.ptr >> 4] + array[ravenColorPtr2.ptr >> 4] + array[ravenColorPtr3.ptr >> 4] + array[ravenColorPtr4.ptr >> 4];
				}
				if (num != 0)
				{
					pipe.shape = this.aaGamma[num];
					pipe.run(pipe);
					this.updateModX(i);
					this.updateModY(y);
				}
				else
				{
					this.pipeIncX(pipe);
				}
			}
		}
		internal void drawSpan(RavenPipe pipe, int x0, int x1, int y, bool noClip)
		{
			if (noClip)
			{
				this.pipeSetXY(pipe, x0, y);
				for (int i = x0; i <= x1; i++)
				{
					pipe.run(pipe);
				}
				this.updateModX(x0);
				this.updateModX(x1);
				this.updateModY(y);
				return;
			}
			if (x0 < this.state.clip.getXMinI())
			{
				x0 = this.state.clip.getXMinI();
			}
			if (x1 > this.state.clip.getXMaxI())
			{
				x1 = this.state.clip.getXMaxI();
			}
			this.pipeSetXY(pipe, x0, y);
			for (int i = x0; i <= x1; i++)
			{
				if (this.state.clip.test(i, y))
				{
					pipe.run(pipe);
					this.updateModX(i);
					this.updateModY(y);
				}
				else
				{
					this.pipeIncX(pipe);
				}
			}
		}
		internal void pipeIncX(RavenPipe pipe)
		{
			pipe.x++;
			if (this.state.softMask != null)
			{
				pipe.softMaskPtr = ++pipe.softMaskPtr;
			}
			switch (this.bitmap.mode)
			{
			case RavenColorMode.ravenModeMono1:
				if ((pipe.destColorMask >>= 1) == 0)
				{
					pipe.destColorMask = 128;
					pipe.destColorPtr = ++pipe.destColorPtr;
				}
				break;
			case RavenColorMode.ravenModeMono8:
				pipe.destColorPtr = ++pipe.destColorPtr;
				break;
			case RavenColorMode.ravenModeRGB8:
			case RavenColorMode.ravenModeBGR8:
				pipe.destColorPtr.Inc(3);
				break;
			}
			if (pipe.destAlphaPtr != null)
			{
				pipe.destAlphaPtr = ++pipe.destAlphaPtr;
			}
			if (pipe.alpha0Ptr != null)
			{
				pipe.alpha0Ptr = ++pipe.alpha0Ptr;
			}
		}
		internal RavenError clipToPath(RavenPath path, bool eo)
		{
			return this.state.clip.clipToPath(path, this.state.matrix, this.state.flatness, eo);
		}
		private RavenError fillWithPattern(RavenPath path, bool eo, RavenPattern pattern, double alpha)
		{
			RavenPipe pipe = new RavenPipe();
			int rectXMin = 0;
			int num = 0;
			int rectXMax = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			if (path.pts.Count == 0)
			{
				return RavenError.ravenErrEmptyPath;
			}
			if (this.pathAllOutside(path))
			{
				this.opClipRes = RavenClipResult.ravenClipAllOutside;
				return RavenError.ravenOk;
			}
			if (this.state.strokeAdjust && path.hints == null)
			{
				int length = path.length;
				if (length == 4 && (path.flags[0] & 4) == 0 && (path.flags[1] & 2) == 0 && (path.flags[2] & 2) == 0)
				{
					path.close(true);
					path.addStrokeAdjustHint(0, 2, 0, 4);
					path.addStrokeAdjustHint(1, 3, 0, 4);
				}
				else
				{
					if (length == 5 && (path.flags[0] & 4) != 0 && (path.flags[1] & 2) == 0 && (path.flags[2] & 2) == 0 && (path.flags[3] & 2) == 0)
					{
						path.addStrokeAdjustHint(0, 2, 0, 4);
						path.addStrokeAdjustHint(1, 3, 0, 4);
					}
				}
			}
			RavenXPath ravenXPath = new RavenXPath(path, this.state.matrix, this.state.flatness, true);
			if (this.vectorAntialias && !this.inShading)
			{
				ravenXPath.aaScale();
			}
			ravenXPath.sort();
			num = this.state.clip.getYMinI();
			num2 = this.state.clip.getYMaxI();
			if (this.vectorAntialias && !this.inShading)
			{
				num *= 4;
				num2 = (num2 + 1) * 4 - 1;
			}
			RavenXPathScanner ravenXPathScanner = new RavenXPathScanner(ravenXPath, eo, num, num2);
			if (this.vectorAntialias && !this.inShading)
			{
				ravenXPathScanner.getBBoxAA(ref rectXMin, ref num, ref rectXMax, ref num2);
			}
			else
			{
				ravenXPathScanner.getBBox(ref rectXMin, ref num, ref rectXMax, ref num2);
			}
			RavenClipResult ravenClipResult;
			if ((ravenClipResult = this.state.clip.testRect(rectXMin, num, rectXMax, num2)) != RavenClipResult.ravenClipAllOutside)
			{
				if (ravenXPathScanner.hasPartialClip())
				{
					ravenClipResult = RavenClipResult.ravenClipPartial;
				}
				this.pipeInit(pipe, 0, num, pattern, null, (byte)RavenMath.ravenRound(alpha * 255.0), this.vectorAntialias && !this.inShading, false);
				if (this.vectorAntialias && !this.inShading)
				{
					for (int i = num; i <= num2; i++)
					{
						ravenXPathScanner.renderAALine(this.aaBuf, ref num3, ref num4, i);
						if (ravenClipResult != RavenClipResult.ravenClipAllInside)
						{
							this.state.clip.clipAALine(this.aaBuf, ref num3, ref num4, i);
						}
						this.drawAALine(pipe, num3, num4, i);
					}
				}
				else
				{
					for (int i = num; i <= num2; i++)
					{
						while (ravenXPathScanner.getNextSpan(i, ref num3, ref num4))
						{
							if (ravenClipResult == RavenClipResult.ravenClipAllInside)
							{
								this.drawSpan(pipe, num3, num4, i, true);
							}
							else
							{
								if (num3 < this.state.clip.getXMinI())
								{
									num3 = this.state.clip.getXMinI();
								}
								if (num4 > this.state.clip.getXMaxI())
								{
									num4 = this.state.clip.getXMaxI();
								}
								RavenClipResult ravenClipResult2 = this.state.clip.testSpan(num3, num4, i);
								this.drawSpan(pipe, num3, num4, i, ravenClipResult2 == RavenClipResult.ravenClipAllInside);
							}
						}
					}
				}
			}
			this.opClipRes = ravenClipResult;
			return RavenError.ravenOk;
		}
		private bool pathAllOutside(RavenPath path)
		{
			double num = 0.0;
			double num2 = 0.0;
			double x;
			double num3 = x = path.pts[0].x;
			double y;
			double num4 = y = path.pts[0].y;
			for (int i = 1; i < path.pts.Count; i++)
			{
				if (path.pts[i].x < x)
				{
					x = path.pts[i].x;
				}
				else
				{
					if (path.pts[i].x > num3)
					{
						num3 = path.pts[i].x;
					}
				}
				if (path.pts[i].y < y)
				{
					y = path.pts[i].y;
				}
				else
				{
					if (path.pts[i].y > num4)
					{
						num4 = path.pts[i].y;
					}
				}
			}
			this.transform(this.state.matrix, x, y, ref num, ref num2);
			double num6;
			double num5 = num6 = num;
			double num8;
			double num7 = num8 = num2;
			this.transform(this.state.matrix, x, num4, ref num, ref num2);
			if (num < num6)
			{
				num6 = num;
			}
			else
			{
				if (num > num5)
				{
					num5 = num;
				}
			}
			if (num2 < num8)
			{
				num8 = num2;
			}
			else
			{
				if (num2 > num7)
				{
					num7 = num2;
				}
			}
			this.transform(this.state.matrix, num3, y, ref num, ref num2);
			if (num < num6)
			{
				num6 = num;
			}
			else
			{
				if (num > num5)
				{
					num5 = num;
				}
			}
			if (num2 < num8)
			{
				num8 = num2;
			}
			else
			{
				if (num2 > num7)
				{
					num7 = num2;
				}
			}
			this.transform(this.state.matrix, num3, num4, ref num, ref num2);
			if (num < num6)
			{
				num6 = num;
			}
			else
			{
				if (num > num5)
				{
					num5 = num;
				}
			}
			if (num2 < num8)
			{
				num8 = num2;
			}
			else
			{
				if (num2 > num7)
				{
					num7 = num2;
				}
			}
			int rectXMin = RavenMath.ravenFloor(num6);
			int rectYMin = RavenMath.ravenFloor(num8);
			int rectXMax = RavenMath.ravenFloor(num5);
			int rectYMax = RavenMath.ravenFloor(num7);
			return this.state.clip.testRect(rectXMin, rectYMin, rectXMax, rectYMax) == RavenClipResult.ravenClipAllOutside;
		}
		private void transform(double[] matrix, double xi, double yi, ref double xo, ref double yo)
		{
			xo = xi * matrix[0] + yi * matrix[2] + matrix[4];
			yo = xi * matrix[1] + yi * matrix[3] + matrix[5];
		}
		private void clearModRegion()
		{
			this.modXMin = this.bitmap.getWidth();
			this.modYMin = this.bitmap.getHeight();
			this.modXMax = -1;
			this.modYMax = -1;
		}
		private void updateModX(int x)
		{
			if (x < this.modXMin)
			{
				this.modXMin = x;
			}
			if (x > this.modXMax)
			{
				this.modXMax = x;
			}
		}
		private void updateModY(int y)
		{
			if (y < this.modYMin)
			{
				this.modYMin = y;
			}
			if (y > this.modYMax)
			{
				this.modYMax = y;
			}
		}
		private void pipeRun(RavenPipe pipe)
		{
			RavenColor ravenColor = new RavenColor();
			RavenColor ravenColor2 = new RavenColor();
			RavenColor ravenColor3 = new RavenColor();
			if (pipe.pattern != null)
			{
				pipe.pattern.getColor(pipe.x, pipe.y, ref pipe.cSrcVal);
			}
			if (pipe.noTransparency && this.state.blendFunc == null)
			{
				switch (this.bitmap.mode)
				{
				case RavenColorMode.ravenModeMono1:
				{
					byte b = this.state.grayTransfer[(int)pipe.cSrc[0]];
					if (this.state.screen.test(pipe.x, pipe.y, b) != 0)
					{
						RavenColorPtr expr_C0 = pipe.destColorPtr;
						expr_C0.ptr |= (byte)pipe.destColorMask;
					}
					else
					{
						RavenColorPtr expr_DC = pipe.destColorPtr;
                        expr_DC.ptr &= (byte)((uint)(byte)pipe.destColorMask ^ (uint)byte.MaxValue);
					}
					if ((pipe.destColorMask >>= 1) == 0)
					{
						pipe.destColorMask = 128;
						pipe.destColorPtr = ++pipe.destColorPtr;
					}
					break;
				}
				case RavenColorMode.ravenModeMono8:
					pipe.destColorPtr.ptr = this.state.grayTransfer[(int)pipe.cSrc[0]];
					pipe.destColorPtr = ++pipe.destColorPtr;
					break;
				case RavenColorMode.ravenModeRGB8:
					pipe.destColorPtr.ptr = this.state.rgbTransferR[(int)pipe.cSrc[0]];
					pipe.destColorPtr = ++pipe.destColorPtr;
					pipe.destColorPtr.ptr = this.state.rgbTransferG[(int)pipe.cSrc[1]];
					pipe.destColorPtr = ++pipe.destColorPtr;
					pipe.destColorPtr.ptr = this.state.rgbTransferB[(int)pipe.cSrc[2]];
					pipe.destColorPtr = ++pipe.destColorPtr;
					break;
				case RavenColorMode.ravenModeBGR8:
					pipe.destColorPtr.ptr = this.state.rgbTransferB[(int)pipe.cSrc[2]];
					pipe.destColorPtr = ++pipe.destColorPtr;
					pipe.destColorPtr.ptr = this.state.rgbTransferG[(int)pipe.cSrc[1]];
					pipe.destColorPtr = ++pipe.destColorPtr;
					pipe.destColorPtr.ptr = this.state.rgbTransferR[(int)pipe.cSrc[0]];
					pipe.destColorPtr = ++pipe.destColorPtr;
					break;
				}
				if (pipe.destAlphaPtr != null)
				{
					pipe.destAlphaPtr.ptr = 255;
					pipe.destAlphaPtr = ++pipe.destAlphaPtr;
				}
			}
			else
			{
				switch (this.bitmap.mode)
				{
				case RavenColorMode.ravenModeMono1:
					ravenColor2[0] = ((((int)pipe.destColorPtr.ptr & pipe.destColorMask) != 0) ? byte.MaxValue : (byte)0);
					break;
				case RavenColorMode.ravenModeMono8:
					ravenColor2[0] = pipe.destColorPtr.ptr;
					break;
				case RavenColorMode.ravenModeRGB8:
					ravenColor2[0] = pipe.destColorPtr[0];
					ravenColor2[1] = pipe.destColorPtr[1];
					ravenColor2[2] = pipe.destColorPtr[2];
					break;
				case RavenColorMode.ravenModeBGR8:
					ravenColor2[0] = pipe.destColorPtr[2];
					ravenColor2[1] = pipe.destColorPtr[1];
					ravenColor2[2] = pipe.destColorPtr[0];
					break;
				}
				byte b2;
				if (pipe.destAlphaPtr != null)
				{
					b2 = pipe.destAlphaPtr.ptr;
				}
				else
				{
					b2 = 255;
				}
				byte b3;
				if (this.state.softMask != null)
				{
					if (pipe.usesShape)
					{
						b3 = RavenMath.div255((int)(RavenMath.div255((int)(pipe.aInput * pipe.softMaskPtr.ptr)) * pipe.shape));
						pipe.softMaskPtr = ++pipe.softMaskPtr;
					}
					else
					{
						b3 = RavenMath.div255((int)(pipe.aInput * pipe.softMaskPtr.ptr));
						pipe.softMaskPtr = ++pipe.softMaskPtr;
					}
				}
				else
				{
					if (pipe.usesShape)
					{
						b3 = RavenMath.div255((int)(pipe.aInput * pipe.shape));
					}
					else
					{
						b3 = pipe.aInput;
					}
				}
				RavenColorPtr ravenColorPtr;
				if (pipe.nonIsolatedGroup)
				{
					if (pipe.shape == 0)
					{
						ravenColorPtr = pipe.cSrc;
					}
					else
					{
						int num = (int)(b2 * 255 / pipe.shape - b2);
						switch (this.bitmap.mode)
						{
						case RavenColorMode.ravenModeMono1:
						case RavenColorMode.ravenModeMono8:
							ravenColor[0] = RavenMath.clip255((int)pipe.cSrc[0] + (int)(pipe.cSrc[0] - ravenColor2[0]) * num / 255);
							break;
						case RavenColorMode.ravenModeRGB8:
						case RavenColorMode.ravenModeBGR8:
							ravenColor[2] = RavenMath.clip255((int)pipe.cSrc[2] + (int)(pipe.cSrc[2] - ravenColor2[2]) * num / 255);
							ravenColor[1] = RavenMath.clip255((int)pipe.cSrc[1] + (int)(pipe.cSrc[1] - ravenColor2[1]) * num / 255);
							ravenColor[0] = RavenMath.clip255((int)pipe.cSrc[0] + (int)(pipe.cSrc[0] - ravenColor2[0]) * num / 255);
							break;
						}
						ravenColorPtr = new RavenColorPtr(ravenColor.getPtr());
					}
				}
				else
				{
					ravenColorPtr = pipe.cSrc;
				}
				if (this.state.blendFunc != null)
				{
					this.state.blendFunc(ravenColorPtr, new RavenColorPtr(ravenColor2.getPtr()), new RavenColorPtr(ravenColor3.getPtr()), this.bitmap.mode);
				}
				byte b6;
				byte b5;
				byte b4;
				if (pipe.noTransparency)
				{
					b4 = (b5 = (b6 = 255));
				}
				else
				{
                    b6 = (byte)((uint)b3 + (uint)b2 - (uint)RavenMath.div255((int)b3 * (int)b2));
					if (pipe.alpha0Ptr != null)
					{
						byte ptr = pipe.alpha0Ptr.ptr;
						pipe.alpha0Ptr = ++pipe.alpha0Ptr;
                        b5 = (byte)((uint)b6 + (uint)ptr - (uint)RavenMath.div255((int)b6 * (int)ptr));
                        b4 = (byte)((uint)ptr + (uint)b2 - (uint)RavenMath.div255((int)ptr * (int)b2));
					}
					else
					{
						b5 = b6;
						b4 = b2;
					}
				}
				byte b;
				byte ptr3;
				byte ptr2 = b = (ptr3 = 0);
				switch (pipe.resultColorCtrl)
				{
				case RavenPipeResultColorCtrl.ravenPipeResultColorNoAlphaBlendMono:
					b = this.state.grayTransfer[(int)RavenMath.div255((int)((255 - b2) * ravenColorPtr[0] + b2 * ravenColor3[0]))];
					break;
				case RavenPipeResultColorCtrl.ravenPipeResultColorNoAlphaBlendRGB:
					b = this.state.rgbTransferR[(int)RavenMath.div255((int)((255 - b2) * ravenColorPtr[0] + b2 * ravenColor3[0]))];
					ptr2 = this.state.rgbTransferG[(int)RavenMath.div255((int)((255 - b2) * ravenColorPtr[1] + b2 * ravenColor3[1]))];
					ptr3 = this.state.rgbTransferB[(int)RavenMath.div255((int)((255 - b2) * ravenColorPtr[2] + b2 * ravenColor3[2]))];
					break;
				case RavenPipeResultColorCtrl.ravenPipeResultColorAlphaNoBlendMono:
					if (b5 == 0)
					{
						b = 0;
					}
					else
					{
						b = this.state.grayTransfer[(int)(((b5 - b3) * ravenColor2[0] + b3 * ravenColorPtr[0]) / b5)];
					}
					break;
				case RavenPipeResultColorCtrl.ravenPipeResultColorAlphaNoBlendRGB:
					if (b5 == 0)
					{
						b = 0;
						ptr2 = 0;
						ptr3 = 0;
					}
					else
					{
						b = this.state.rgbTransferR[(int)(((b5 - b3) * ravenColor2[0] + b3 * ravenColorPtr[0]) / b5)];
						ptr2 = this.state.rgbTransferG[(int)(((b5 - b3) * ravenColor2[1] + b3 * ravenColorPtr[1]) / b5)];
						ptr3 = this.state.rgbTransferB[(int)(((b5 - b3) * ravenColor2[2] + b3 * ravenColorPtr[2]) / b5)];
					}
					break;
				case RavenPipeResultColorCtrl.ravenPipeResultColorAlphaBlendMono:
					if (b5 == 0)
					{
						b = 0;
					}
					else
					{
						b = this.state.grayTransfer[(int)(((b5 - b3) * ravenColor2[0] + b3 * ((255 - b4) * ravenColorPtr[0] + b4 * ravenColor3[0]) / 255) / b5)];
					}
					break;
				case RavenPipeResultColorCtrl.ravenPipeResultColorAlphaBlendRGB:
					if (b5 == 0)
					{
						b = 0;
						ptr2 = 0;
						ptr3 = 0;
					}
					else
					{
						b = this.state.rgbTransferR[(int)(((b5 - b3) * ravenColor2[0] + b3 * ((255 - b4) * ravenColorPtr[0] + b4 * ravenColor3[0]) / 255) / b5)];
						ptr2 = this.state.rgbTransferG[(int)(((b5 - b3) * ravenColor2[1] + b3 * ((255 - b4) * ravenColorPtr[1] + b4 * ravenColor3[1]) / 255) / b5)];
						ptr3 = this.state.rgbTransferB[(int)(((b5 - b3) * ravenColor2[2] + b3 * ((255 - b4) * ravenColorPtr[2] + b4 * ravenColor3[2]) / 255) / b5)];
					}
					break;
				}
				switch (this.bitmap.mode)
				{
				case RavenColorMode.ravenModeMono1:
					if (this.state.screen.test(pipe.x, pipe.y, b) != 0)
					{
						RavenColorPtr expr_9A2 = pipe.destColorPtr;
						expr_9A2.ptr |= (byte)pipe.destColorMask;
					}
					else
					{
						RavenColorPtr expr_9BE = pipe.destColorPtr;
						expr_9BE.ptr &= (byte)(pipe.destColorMask ^ 255);
					}
					if ((pipe.destColorMask >>= 1) == 0)
					{
						pipe.destColorMask = 128;
						pipe.destColorPtr = ++pipe.destColorPtr;
					}
					break;
				case RavenColorMode.ravenModeMono8:
					pipe.destColorPtr.ptr = b;
					pipe.destColorPtr = ++pipe.destColorPtr;
					break;
				case RavenColorMode.ravenModeRGB8:
					pipe.destColorPtr.ptr = b;
					pipe.destColorPtr = ++pipe.destColorPtr;
					pipe.destColorPtr.ptr = ptr2;
					pipe.destColorPtr = ++pipe.destColorPtr;
					pipe.destColorPtr.ptr = ptr3;
					pipe.destColorPtr = ++pipe.destColorPtr;
					break;
				case RavenColorMode.ravenModeBGR8:
					pipe.destColorPtr.ptr = ptr3;
					pipe.destColorPtr = ++pipe.destColorPtr;
					pipe.destColorPtr.ptr = ptr2;
					pipe.destColorPtr = ++pipe.destColorPtr;
					pipe.destColorPtr.ptr = b;
					pipe.destColorPtr = ++pipe.destColorPtr;
					break;
				}
				if (pipe.destAlphaPtr != null)
				{
					pipe.destAlphaPtr.ptr = b6;
					pipe.destAlphaPtr = ++pipe.destAlphaPtr;
				}
			}
			pipe.x++;
		}
		internal void pipeRunSimpleMono1(RavenPipe pipe)
		{
			byte value = this.state.grayTransfer[(int)pipe.cSrc[0]];
			if (this.state.screen.test(pipe.x, pipe.y, value) != 0)
			{
				RavenColorPtr expr_3E = pipe.destColorPtr;
				expr_3E.ptr |= (byte)pipe.destColorMask;
			}
			else
			{
				RavenColorPtr expr_5A = pipe.destColorPtr;
				expr_5A.ptr &= (byte)(pipe.destColorMask ^ 255);
			}
			if ((pipe.destColorMask >>= 1) == 0)
			{
				pipe.destColorMask = 128;
				pipe.destColorPtr = ++pipe.destColorPtr;
			}
			pipe.x++;
		}
		internal void pipeRunSimpleMono8(RavenPipe pipe)
		{
			pipe.destColorPtr.ptr = this.state.grayTransfer[(int)pipe.cSrc[0]];
			pipe.destColorPtr = ++pipe.destColorPtr;
			pipe.destAlphaPtr.ptr = 255;
			pipe.destAlphaPtr = ++pipe.destAlphaPtr;
			pipe.x++;
		}
		internal void pipeRunSimpleRGB8(RavenPipe pipe)
		{
			pipe.destColorPtr.ptr = this.state.rgbTransferR[(int)pipe.cSrc[0]];
			pipe.destColorPtr = ++pipe.destColorPtr;
			pipe.destColorPtr.ptr = this.state.rgbTransferG[(int)pipe.cSrc[1]];
			pipe.destColorPtr = ++pipe.destColorPtr;
			pipe.destColorPtr.ptr = this.state.rgbTransferB[(int)pipe.cSrc[2]];
			pipe.destColorPtr = ++pipe.destColorPtr;
			pipe.destAlphaPtr.ptr = 255;
			pipe.destAlphaPtr = ++pipe.destAlphaPtr;
			pipe.x++;
		}
		internal void pipeRunSimpleBGR8(RavenPipe pipe)
		{
			pipe.destColorPtr.ptr = this.state.rgbTransferB[(int)pipe.cSrc[2]];
			pipe.destColorPtr = ++pipe.destColorPtr;
			pipe.destColorPtr.ptr = this.state.rgbTransferG[(int)pipe.cSrc[1]];
			pipe.destColorPtr = ++pipe.destColorPtr;
			pipe.destColorPtr.ptr = this.state.rgbTransferR[(int)pipe.cSrc[0]];
			pipe.destColorPtr = ++pipe.destColorPtr;
			pipe.destAlphaPtr.ptr = 255;
			pipe.destAlphaPtr = ++pipe.destAlphaPtr;
			pipe.x++;
		}
		internal void pipeRunAAMono1(RavenPipe pipe)
		{
			RavenColor ravenColor = new RavenColor();
            ravenColor[0] = ((int)pipe.destColorPtr.ptr & pipe.destColorMask) != 0 ? byte.MaxValue : (byte)0;
			byte b = RavenMath.div255((int)(pipe.aInput * pipe.shape));
			byte value = this.state.grayTransfer[(int)RavenMath.div255((int)((255 - b) * ravenColor[0] + b * pipe.cSrc[0]))];
			if (this.state.screen.test(pipe.x, pipe.y, value) != 0)
			{
				RavenColorPtr expr_91 = pipe.destColorPtr;
				expr_91.ptr |= (byte)pipe.destColorMask;
			}
			else
			{
				RavenColorPtr expr_AD = pipe.destColorPtr;
				expr_AD.ptr &= (byte)(pipe.destColorMask ^ 255);
			}
			if ((pipe.destColorMask >>= 1) == 0)
			{
				pipe.destColorMask = 128;
				pipe.destColorPtr = ++pipe.destColorPtr;
			}
			pipe.x++;
		}
		internal void pipeRunAAMono8(RavenPipe pipe)
		{
			RavenColor ravenColor = new RavenColor();
			ravenColor[0] = pipe.destColorPtr.ptr;
			byte ptr = pipe.destAlphaPtr.ptr;
			byte b = RavenMath.div255((int)(pipe.aInput * pipe.shape));
            byte b2 = (byte)((uint)b + (uint)ptr - (uint)RavenMath.div255((int)b * (int)ptr));
			byte b3 = b2;
			byte ptr2;
			if (b3 == 0)
			{
				ptr2 = 0;
			}
			else
			{
				ptr2 = this.state.grayTransfer[(int)(((b3 - b) * ravenColor[0] + b * pipe.cSrc[0]) / b3)];
			}
			pipe.destColorPtr.ptr = ptr2;
			pipe.destColorPtr = ++pipe.destColorPtr;
			pipe.destAlphaPtr.ptr = b2;
			pipe.destAlphaPtr = ++pipe.destAlphaPtr;
			pipe.x++;
		}
		internal void pipeRunAARGB8(RavenPipe pipe)
		{
			RavenColor ravenColor = new RavenColor();
			ravenColor[0] = pipe.destColorPtr[0];
			ravenColor[1] = pipe.destColorPtr[1];
			ravenColor[2] = pipe.destColorPtr[2];
			byte ptr = pipe.destAlphaPtr.ptr;
			byte b = RavenMath.div255((int)(pipe.aInput * pipe.shape));
            byte b2 = (byte)((uint)b + (uint)ptr - (uint)RavenMath.div255((int)b * (int)ptr));
			byte b3 = b2;
			byte ptr2;
			byte ptr3;
			byte ptr4;
			if (b3 == 0)
			{
				ptr2 = 0;
				ptr3 = 0;
				ptr4 = 0;
			}
			else
			{
				ptr2 = this.state.rgbTransferR[(int)(((b3 - b) * ravenColor[0] + b * pipe.cSrc[0]) / b3)];
				ptr3 = this.state.rgbTransferG[(int)(((b3 - b) * ravenColor[1] + b * pipe.cSrc[1]) / b3)];
				ptr4 = this.state.rgbTransferB[(int)(((b3 - b) * ravenColor[2] + b * pipe.cSrc[2]) / b3)];
			}
			pipe.destColorPtr.ptr = ptr2;
			pipe.destColorPtr = ++pipe.destColorPtr;
			pipe.destColorPtr.ptr = ptr3;
			pipe.destColorPtr = ++pipe.destColorPtr;
			pipe.destColorPtr.ptr = ptr4;
			pipe.destColorPtr = ++pipe.destColorPtr;
			pipe.destAlphaPtr.ptr = b2;
			pipe.destAlphaPtr = ++pipe.destAlphaPtr;
			pipe.x++;
		}
		internal void pipeRunAABGR8(RavenPipe pipe)
		{
			RavenColor ravenColor = new RavenColor();
			ravenColor[0] = pipe.destColorPtr[2];
			ravenColor[1] = pipe.destColorPtr[1];
			ravenColor[2] = pipe.destColorPtr[0];
			byte ptr = pipe.destAlphaPtr.ptr;
			byte b = RavenMath.div255((int)(pipe.aInput * pipe.shape));
            byte b2 = (byte)((uint)b + (uint)ptr - (uint)RavenMath.div255((int)b * (int)ptr));
			byte b3 = b2;
			byte ptr2;
			byte ptr3;
			byte ptr4;
			if (b3 == 0)
			{
				ptr2 = 0;
				ptr3 = 0;
				ptr4 = 0;
			}
			else
			{
				ptr2 = this.state.rgbTransferR[(int)(((b3 - b) * ravenColor[0] + b * pipe.cSrc[0]) / b3)];
				ptr3 = this.state.rgbTransferG[(int)(((b3 - b) * ravenColor[1] + b * pipe.cSrc[1]) / b3)];
				ptr4 = this.state.rgbTransferB[(int)(((b3 - b) * ravenColor[2] + b * pipe.cSrc[2]) / b3)];
			}
			pipe.destColorPtr.ptr = ptr4;
			pipe.destColorPtr = ++pipe.destColorPtr;
			pipe.destColorPtr.ptr = ptr3;
			pipe.destColorPtr = ++pipe.destColorPtr;
			pipe.destColorPtr.ptr = ptr2;
			pipe.destColorPtr = ++pipe.destColorPtr;
			pipe.destAlphaPtr.ptr = b2;
			pipe.destAlphaPtr = ++pipe.destAlphaPtr;
			pipe.x++;
		}
		private void pipeSetXY(RavenPipe pipe, int x, int y)
		{
			pipe.x = x;
			pipe.y = y;
			if (this.state.softMask != null)
			{
				pipe.softMaskPtr = new RavenColorPtr(this.state.softMask.data, y * this.state.softMask.rowSize + x);
			}
			switch (this.bitmap.mode)
			{
			case RavenColorMode.ravenModeMono1:
				pipe.destColorPtr = new RavenColorPtr(this.bitmap.data, y * this.bitmap.rowSize + (x >> 3));
				pipe.destColorMask = 128 >> (x & 7);
				break;
			case RavenColorMode.ravenModeMono8:
				pipe.destColorPtr = new RavenColorPtr(this.bitmap.data, y * this.bitmap.rowSize + x);
				break;
			case RavenColorMode.ravenModeRGB8:
			case RavenColorMode.ravenModeBGR8:
				pipe.destColorPtr = new RavenColorPtr(this.bitmap.data, y * this.bitmap.rowSize + 3 * x);
				break;
			}
			if (this.bitmap.alpha != null)
			{
				pipe.destAlphaPtr = new RavenColorPtr(this.bitmap.alpha, y * this.bitmap.width + x);
			}
			else
			{
				pipe.destAlphaPtr = null;
			}
			if (this.state.inNonIsolatedGroup && this.alpha0Bitmap.alpha != null)
			{
				pipe.alpha0Ptr = new RavenColorPtr(this.alpha0Bitmap.alpha, (this.alpha0Y + y) * this.alpha0Bitmap.width + (this.alpha0X + x));
				return;
			}
			pipe.alpha0Ptr = null;
		}
		private void pipeInit(RavenPipe pipe, int x, int y, RavenPattern pattern, RavenColorPtr cSrc, byte aInput, bool usesShape, bool nonIsolatedGroup)
		{
			this.pipeSetXY(pipe, x, y);
			pipe.pattern = null;
			if (pattern != null)
			{
				if (pattern.isStatic())
				{
					pattern.getColor(x, y, ref pipe.cSrcVal);
				}
				else
				{
					pipe.pattern = pattern;
				}
				pipe.cSrc = new RavenColorPtr(pipe.cSrcVal.getPtr(), 0);
			}
			else
			{
				pipe.cSrc = cSrc;
			}
			pipe.aInput = aInput;
			pipe.usesShape = usesShape;
			if (aInput == 255 && this.state.softMask == null && !usesShape && !this.state.inNonIsolatedGroup && !nonIsolatedGroup)
			{
				pipe.noTransparency = true;
			}
			else
			{
				pipe.noTransparency = false;
			}
			if (pipe.noTransparency)
			{
				pipe.resultColorCtrl = this.pipeResultColorNoAlphaBlend[(int)this.bitmap.mode];
			}
			else
			{
				if (this.state.blendFunc == null)
				{
					pipe.resultColorCtrl = this.pipeResultColorAlphaNoBlend[(int)this.bitmap.mode];
				}
				else
				{
					pipe.resultColorCtrl = this.pipeResultColorAlphaBlend[(int)this.bitmap.mode];
				}
			}
			pipe.nonIsolatedGroup = nonIsolatedGroup;
			pipe.run = new PipeFunc(this.pipeRun);
			if (pipe.pattern == null && pipe.noTransparency && this.state.blendFunc == null)
			{
				if (this.bitmap.mode == RavenColorMode.ravenModeMono1 && pipe.destAlphaPtr == null)
				{
					pipe.run = new PipeFunc(this.pipeRunSimpleMono1);
					return;
				}
				if (this.bitmap.mode == RavenColorMode.ravenModeMono8 && pipe.destAlphaPtr != null)
				{
					pipe.run = new PipeFunc(this.pipeRunSimpleMono8);
					return;
				}
				if (this.bitmap.mode == RavenColorMode.ravenModeRGB8 && pipe.destAlphaPtr != null)
				{
					pipe.run = new PipeFunc(this.pipeRunSimpleRGB8);
					return;
				}
				if (this.bitmap.mode == RavenColorMode.ravenModeBGR8 && pipe.destAlphaPtr != null)
				{
					pipe.run = new PipeFunc(this.pipeRunSimpleBGR8);
					return;
				}
			}
			else
			{
				if (pipe.pattern == null && !pipe.noTransparency && this.state.softMask == null && pipe.usesShape && (!this.state.inNonIsolatedGroup || this.alpha0Bitmap.alpha == null) && this.state.blendFunc == null && !pipe.nonIsolatedGroup)
				{
					if (this.bitmap.mode == RavenColorMode.ravenModeMono1 && pipe.destAlphaPtr == null)
					{
						pipe.run = new PipeFunc(this.pipeRunAAMono1);
						return;
					}
					if (this.bitmap.mode == RavenColorMode.ravenModeMono8 && pipe.destAlphaPtr != null)
					{
						pipe.run = new PipeFunc(this.pipeRunAAMono8);
						return;
					}
					if (this.bitmap.mode == RavenColorMode.ravenModeRGB8 && pipe.destAlphaPtr != null)
					{
						pipe.run = new PipeFunc(this.pipeRunAARGB8);
						return;
					}
					if (this.bitmap.mode == RavenColorMode.ravenModeBGR8 && pipe.destAlphaPtr != null)
					{
						pipe.run = new PipeFunc(this.pipeRunAABGR8);
					}
				}
			}
		}
		internal void setSoftMask(RavenBitmap softMask)
		{
			this.state.setSoftMask(softMask);
		}
		internal void saveState()
		{
			RavenState ravenState = this.state.copy();
			ravenState.next = this.state;
			this.state = ravenState;
		}
		internal RavenError restoreState()
		{
			if (this.state.next == null)
			{
				return RavenError.ravenErrNoSave;
			}
			this.state = this.state.next;
			return RavenError.ravenOk;
		}
		internal RavenScreen getScreen()
		{
			return this.state.screen;
		}
		internal RavenError fillImageMask(RavenImageMaskSource src, object srcData, int w, int h, double[] mat, bool glyphMode)
		{
			if (!RavenMath.ravenCheckDet(mat[0], mat[1], mat[2], mat[3], 1E-06))
			{
				return RavenError.ravenErrSingularMatrix;
			}
			bool flag = mat[1] == 0.0 && mat[2] == 0.0;
			if (mat[0] > 0.0 && flag && mat[3] > 0.0)
			{
				int num = Raven.imgCoordMungeLowerC(mat[4], glyphMode);
				int num2 = Raven.imgCoordMungeLowerC(mat[5], glyphMode);
				int num3 = Raven.imgCoordMungeUpperC(mat[0] + mat[4], glyphMode);
				int num4 = Raven.imgCoordMungeUpperC(mat[3] + mat[5], glyphMode);
				if (num == num3)
				{
					num3++;
				}
				if (num2 == num4)
				{
					num4++;
				}
				RavenClipResult ravenClipResult = this.state.clip.testRect(num, num2, num3 - 1, num4 - 1);
				this.opClipRes = ravenClipResult;
				if (ravenClipResult != RavenClipResult.ravenClipAllOutside)
				{
					int num5 = num3 - num;
					int num6 = num4 - num2;
					RavenBitmap ravenBitmap = this.scaleMask(src, srcData, w, h, num5, num6);
					this.blitMask(ravenBitmap, num, num2, ravenClipResult);
				}
			}
			else
			{
				if (mat[0] > 0.0 && flag && mat[3] < 0.0)
				{
					int num = Raven.imgCoordMungeLowerC(mat[4], glyphMode);
					int num2 = Raven.imgCoordMungeLowerC(mat[3] + mat[5], glyphMode);
					int num3 = Raven.imgCoordMungeUpperC(mat[0] + mat[4], glyphMode);
					int num4 = Raven.imgCoordMungeUpperC(mat[5], glyphMode);
					if (num == num3)
					{
						num3++;
					}
					if (num2 == num4)
					{
						num4++;
					}
					RavenClipResult ravenClipResult = this.state.clip.testRect(num, num2, num3 - 1, num4 - 1);
					this.opClipRes = ravenClipResult;
					if (ravenClipResult != RavenClipResult.ravenClipAllOutside)
					{
						int num5 = num3 - num;
						int num6 = num4 - num2;
						RavenBitmap ravenBitmap = this.scaleMask(src, srcData, w, h, num5, num6);
						this.vertFlipImage(ravenBitmap, num5, num6, 1);
						this.blitMask(ravenBitmap, num, num2, ravenClipResult);
					}
				}
				else
				{
					this.arbitraryTransformMask(src, srcData, w, h, mat, glyphMode);
				}
			}
			return RavenError.ravenOk;
		}
		private RavenBitmap scaleMask(RavenImageMaskSource src, object srcData, int srcWidth, int srcHeight, int scaledWidth, int scaledHeight)
		{
			RavenBitmap ravenBitmap = new RavenBitmap(scaledWidth, scaledHeight, 1, RavenColorMode.ravenModeMono8, false, true);
			if (scaledHeight < srcHeight)
			{
				if (scaledWidth < srcWidth)
				{
					this.scaleMaskYdXd(src, srcData, srcWidth, srcHeight, scaledWidth, scaledHeight, ravenBitmap);
				}
				else
				{
					this.scaleMaskYdXu(src, srcData, srcWidth, srcHeight, scaledWidth, scaledHeight, ravenBitmap);
				}
			}
			else
			{
				if (scaledWidth < srcWidth)
				{
					this.scaleMaskYuXd(src, srcData, srcWidth, srcHeight, scaledWidth, scaledHeight, ravenBitmap);
				}
				else
				{
					this.scaleMaskYuXu(src, srcData, srcWidth, srcHeight, scaledWidth, scaledHeight, ravenBitmap);
				}
			}
			return ravenBitmap;
		}
		private void scaleMaskYdXd(RavenImageMaskSource src, object srcData, int srcWidth, int srcHeight, int scaledWidth, int scaledHeight, RavenBitmap dest)
		{
			int num = srcHeight / scaledHeight;
			int num2 = srcHeight % scaledHeight;
			int num3 = srcWidth / scaledWidth;
			int num4 = srcWidth % scaledWidth;
			byte[] array = new byte[srcWidth];
			uint[] array2 = new uint[srcWidth];
			int num5 = 0;
			RavenColorPtr ravenColorPtr = new RavenColorPtr(dest.data);
			for (int i = 0; i < scaledHeight; i++)
			{
				int num6;
				if ((num5 += num2) >= scaledHeight)
				{
					num5 -= scaledHeight;
					num6 = num + 1;
				}
				else
				{
					num6 = num;
				}
				for (int j = 0; j < srcWidth; j++)
				{
					array2[j] = 0u;
				}
				for (int k = 0; k < num6; k++)
				{
					src(srcData, new RavenColorPtr(array));
					for (int l = 0; l < srcWidth; l++)
					{
						array2[l] += (uint)array[l];
					}
				}
				int num7 = 0;
				int num8 = 2139095040 / (num6 * num3);
				int num9 = 2139095040 / (num6 * (num3 + 1));
				int num10 = 0;
				for (int m = 0; m < scaledWidth; m++)
				{
					int num11;
					int num12;
					if ((num7 += num4) >= scaledWidth)
					{
						num7 -= scaledWidth;
						num11 = num3 + 1;
						num12 = num9;
					}
					else
					{
						num11 = num3;
						num12 = num8;
					}
					uint num13 = 0u;
					for (int k = 0; k < num11; k++)
					{
						num13 += array2[num10++];
					}
					num13 = (uint)((ulong)num13 * (ulong)((long)num12) >> 23);
					ravenColorPtr.ptr = (byte)num13;
					ravenColorPtr = ++ravenColorPtr;
				}
			}
		}
		private void scaleMaskYdXu(RavenImageMaskSource src, object srcData, int srcWidth, int srcHeight, int scaledWidth, int scaledHeight, RavenBitmap dest)
		{
			int num = srcHeight / scaledHeight;
			int num2 = srcHeight % scaledHeight;
			int num3 = scaledWidth / srcWidth;
			int num4 = scaledWidth % srcWidth;
			byte[] array = new byte[srcWidth];
			uint[] array2 = new uint[srcWidth];
			int num5 = 0;
			RavenColorPtr ravenColorPtr = new RavenColorPtr(dest.data);
			for (int i = 0; i < scaledHeight; i++)
			{
				int num6;
				if ((num5 += num2) >= scaledHeight)
				{
					num5 -= scaledHeight;
					num6 = num + 1;
				}
				else
				{
					num6 = num;
				}
				for (int j = 0; j < srcWidth; j++)
				{
					array2[j] = 0u;
				}
				for (int k = 0; k < num6; k++)
				{
					src(srcData, new RavenColorPtr(array));
					for (int l = 0; l < srcWidth; l++)
					{
						array2[l] += (uint)array[l];
					}
				}
				int num7 = 0;
				int num8 = 2139095040 / num6;
				for (int m = 0; m < srcWidth; m++)
				{
					int num9;
					if ((num7 += num4) >= srcWidth)
					{
						num7 -= srcWidth;
						num9 = num3 + 1;
					}
					else
					{
						num9 = num3;
					}
					uint num10 = array2[m];
					num10 = (uint)((ulong)num10 * (ulong)((long)num8) >> 23);
					for (int k = 0; k < num9; k++)
					{
						ravenColorPtr.ptr = (byte)num10;
						ravenColorPtr = ++ravenColorPtr;
					}
				}
			}
		}
		private void scaleMaskYuXd(RavenImageMaskSource src, object srcData, int srcWidth, int srcHeight, int scaledWidth, int scaledHeight, RavenBitmap dest)
		{
			int num = scaledHeight / srcHeight;
			int num2 = scaledHeight % srcHeight;
			int num3 = srcWidth / scaledWidth;
			int num4 = srcWidth % scaledWidth;
			byte[] array = new byte[srcWidth];
			int num5 = 0;
			RavenColorPtr ravenColorPtr = new RavenColorPtr(dest.data);
			for (int i = 0; i < srcHeight; i++)
			{
				int num6;
				if ((num5 += num2) >= srcHeight)
				{
					num5 -= srcHeight;
					num6 = num + 1;
				}
				else
				{
					num6 = num;
				}
				src(srcData, new RavenColorPtr(array));
				int num7 = 0;
				int num8 = 2139095040 / num3;
				int num9 = 2139095040 / (num3 + 1);
				int num10 = 0;
				for (int j = 0; j < scaledWidth; j++)
				{
					int num11;
					int num12;
					if ((num7 += num4) >= scaledWidth)
					{
						num7 -= scaledWidth;
						num11 = num3 + 1;
						num12 = num9;
					}
					else
					{
						num11 = num3;
						num12 = num8;
					}
					uint num13 = 0u;
					for (int k = 0; k < num11; k++)
					{
						num13 += (uint)array[num10++];
					}
					num13 = (uint)((ulong)num13 * (ulong)((long)num12) >> 23);
					for (int k = 0; k < num6; k++)
					{
						RavenColorPtr ravenColorPtr2 = new RavenColorPtr(ravenColorPtr, k * scaledWidth + j);
						ravenColorPtr2.ptr = (byte)num13;
					}
				}
				ravenColorPtr.Inc(num6 * scaledWidth);
			}
		}
		private void scaleMaskYuXu(RavenImageMaskSource src, object srcData, int srcWidth, int srcHeight, int scaledWidth, int scaledHeight, RavenBitmap dest)
		{
			int num = scaledHeight / srcHeight;
			int num2 = scaledHeight % srcHeight;
			int num3 = scaledWidth / srcWidth;
			int num4 = scaledWidth % srcWidth;
			byte[] array = new byte[srcWidth];
			int num5 = 0;
			RavenColorPtr ravenColorPtr = new RavenColorPtr(dest.data);
			for (int i = 0; i < srcHeight; i++)
			{
				int num6;
				if ((num5 += num2) >= srcHeight)
				{
					num5 -= srcHeight;
					num6 = num + 1;
				}
				else
				{
					num6 = num;
				}
				src(srcData, new RavenColorPtr(array));
				int num7 = 0;
				int num8 = 0;
				for (int j = 0; j < srcWidth; j++)
				{
					int num9;
					if ((num7 += num4) >= srcWidth)
					{
						num7 -= srcWidth;
						num9 = num3 + 1;
					}
					else
					{
						num9 = num3;
					}
					uint num10 = (array[j] != 0) ? 255u : 0u;
					for (int k = 0; k < num6; k++)
					{
						for (int l = 0; l < num9; l++)
						{

                            RavenColorPtr ravenColorPtr2 = new RavenColorPtr(ravenColorPtr, k * scaledWidth + num8 + l);
                            ravenColorPtr2.ptr = (byte)num10;
                            ravenColorPtr2 = ++ravenColorPtr2;
						}
					}
					num8 += num9;
				}
				ravenColorPtr.Inc(num6 * scaledWidth);
			}
		}
		private void blitMask(RavenBitmap src, int xDest, int yDest, RavenClipResult clipRes)
		{
			RavenPipe ravenPipe = new RavenPipe();
			int width = src.getWidth();
			int height = src.getHeight();
			RavenColorPtr ravenColorPtr;
			if (this.vectorAntialias && clipRes != RavenClipResult.ravenClipAllInside)
			{
				this.pipeInit(ravenPipe, xDest, yDest, this.state.fillPattern, null, (byte)RavenMath.ravenRound(this.state.fillAlpha * 255.0), true, false);
				this.drawAAPixelInit();
				ravenColorPtr = new RavenColorPtr(src.getDataPtr());
				for (int i = 0; i < height; i++)
				{
					for (int j = 0; j < width; j++)
					{
						ravenPipe.shape = ravenColorPtr.ptr;
						ravenColorPtr = ++ravenColorPtr;
						this.drawAAPixel(ravenPipe, xDest + j, yDest + i);
					}
				}
				return;
			}
			this.pipeInit(ravenPipe, xDest, yDest, this.state.fillPattern, null, (byte)RavenMath.ravenRound(this.state.fillAlpha * 255.0), true, false);
			ravenColorPtr = new RavenColorPtr(src.getDataPtr());
			if (clipRes == RavenClipResult.ravenClipAllInside)
			{
				for (int i = 0; i < height; i++)
				{
					this.pipeSetXY(ravenPipe, xDest, yDest + i);
					for (int j = 0; j < width; j++)
					{
						if (ravenColorPtr.ptr != 0)
						{
							ravenPipe.shape = ravenColorPtr.ptr;
							ravenPipe.run(ravenPipe);
						}
						else
						{
							this.pipeIncX(ravenPipe);
						}
						ravenColorPtr = ++ravenColorPtr;
					}
				}
				this.updateModX(xDest);
				this.updateModX(xDest + width - 1);
				this.updateModY(yDest);
				this.updateModY(yDest + height - 1);
				return;
			}
			for (int i = 0; i < height; i++)
			{
				this.pipeSetXY(ravenPipe, xDest, yDest + i);
				for (int j = 0; j < width; j++)
				{
					if (ravenColorPtr.ptr != 0 && this.state.clip.test(xDest + j, yDest + i))
					{
						ravenPipe.shape = ravenColorPtr.ptr;
						ravenPipe.run(ravenPipe);
						this.updateModX(xDest + j);
						this.updateModY(yDest + i);
					}
					else
					{
						this.pipeIncX(ravenPipe);
					}
					ravenColorPtr = ++ravenColorPtr;
				}
			}
		}
		private void vertFlipImage(RavenBitmap img, int width, int height, int nComps)
		{
			int num = width * nComps;
			byte[] array = new byte[num];
			RavenColorPtr ravenColorPtr = new RavenColorPtr(img.data);
			RavenColorPtr ravenColorPtr2 = new RavenColorPtr(img.data, (height - 1) * num);
			while (ravenColorPtr.offset < ravenColorPtr2.offset)
			{
				Array.Copy(ravenColorPtr.buffer, ravenColorPtr.offset, array, 0, num);
				Array.Copy(ravenColorPtr2.buffer, ravenColorPtr2.offset, ravenColorPtr.buffer, ravenColorPtr.offset, num);
				Array.Copy(array, 0, ravenColorPtr2.buffer, ravenColorPtr2.offset, num);
				ravenColorPtr.Inc(num);
				ravenColorPtr2.Inc(-num);
			}
			if (img.alpha != null)
			{
				ravenColorPtr = new RavenColorPtr(img.alpha);
				ravenColorPtr2 = new RavenColorPtr(img.alpha, (height - 1) * width);
				while (ravenColorPtr.offset < ravenColorPtr2.offset)
				{
					Array.Copy(ravenColorPtr.buffer, ravenColorPtr.offset, array, 0, width);
					Array.Copy(ravenColorPtr2.buffer, ravenColorPtr2.offset, ravenColorPtr.buffer, ravenColorPtr.offset, width);
					Array.Copy(array, 0, ravenColorPtr2.buffer, ravenColorPtr2.offset, width);
					ravenColorPtr.Inc(width);
					ravenColorPtr2.Inc(-width);
				}
			}
		}
		private void arbitraryTransformMask(RavenImageMaskSource src, object srcData, int srcWidth, int srcHeight, double[] mat, bool glyphMode)
		{
			RavenPipe ravenPipe = new RavenPipe();
			double[] array = new double[4];
			double[] array2 = new double[4];
			ImageSection[] array3 = new ImageSection[3];
			for (int i = 0; i < array3.Length; i++)
			{
				array3[i] = new ImageSection();
			}
			array[0] = mat[4];
			array2[0] = mat[5];
			array[1] = mat[2] + mat[4];
			array2[1] = mat[3] + mat[5];
			array[2] = mat[0] + mat[2] + mat[4];
			array2[2] = mat[1] + mat[3] + mat[5];
			array[3] = mat[0] + mat[4];
			array2[3] = mat[1] + mat[5];
			int num = Raven.imgCoordMungeLowerC(array[0], glyphMode);
			int num2 = Raven.imgCoordMungeUpperC(array[0], glyphMode);
			int num3 = Raven.imgCoordMungeLowerC(array2[0], glyphMode);
			int num4 = Raven.imgCoordMungeUpperC(array2[0], glyphMode);
			int j;
			int num5;
			int num6;
			for (j = 1; j < 4; j++)
			{
				num5 = Raven.imgCoordMungeLowerC(array[j], glyphMode);
				if (num5 < num)
				{
					num = num5;
				}
				num5 = Raven.imgCoordMungeUpperC(array[j], glyphMode);
				if (num5 > num2)
				{
					num2 = num5;
				}
				num6 = Raven.imgCoordMungeLowerC(array2[j], glyphMode);
				if (num6 < num3)
				{
					num3 = num6;
				}
				num6 = Raven.imgCoordMungeUpperC(array2[j], glyphMode);
				if (num6 > num4)
				{
					num4 = num6;
				}
			}
			RavenClipResult ravenClipResult = this.state.clip.testRect(num, num3, num2 - 1, num4 - 1);
			this.opClipRes = ravenClipResult;
			if (ravenClipResult == RavenClipResult.ravenClipAllOutside)
			{
				return;
			}
			if (mat[0] >= 0.0)
			{
				num5 = Raven.imgCoordMungeUpperC(mat[0] + mat[4], glyphMode) - Raven.imgCoordMungeLowerC(mat[4], glyphMode);
			}
			else
			{
				num5 = Raven.imgCoordMungeUpperC(mat[4], glyphMode) - Raven.imgCoordMungeLowerC(mat[0] + mat[4], glyphMode);
			}
			if (mat[1] >= 0.0)
			{
				num6 = Raven.imgCoordMungeUpperC(mat[1] + mat[5], glyphMode) - Raven.imgCoordMungeLowerC(mat[5], glyphMode);
			}
			else
			{
				num6 = Raven.imgCoordMungeUpperC(mat[5], glyphMode) - Raven.imgCoordMungeLowerC(mat[1] + mat[5], glyphMode);
			}
			int num7 = (num5 > num6) ? num5 : num6;
			if (mat[2] >= 0.0)
			{
				num5 = Raven.imgCoordMungeUpperC(mat[2] + mat[4], glyphMode) - Raven.imgCoordMungeLowerC(mat[4], glyphMode);
			}
			else
			{
				num5 = Raven.imgCoordMungeUpperC(mat[4], glyphMode) - Raven.imgCoordMungeLowerC(mat[2] + mat[4], glyphMode);
			}
			if (mat[3] >= 0.0)
			{
				num6 = Raven.imgCoordMungeUpperC(mat[3] + mat[5], glyphMode) - Raven.imgCoordMungeLowerC(mat[5], glyphMode);
			}
			else
			{
				num6 = Raven.imgCoordMungeUpperC(mat[5], glyphMode) - Raven.imgCoordMungeLowerC(mat[3] + mat[5], glyphMode);
			}
			int num8 = (num5 > num6) ? num5 : num6;
			if (num7 == 0)
			{
				num7 = 1;
			}
			if (num8 == 0)
			{
				num8 = 1;
			}
			double num9 = mat[0] / (double)num7;
			double num10 = mat[1] / (double)num7;
			double num11 = mat[2] / (double)num8;
			double num12 = mat[3] / (double)num8;
			double num13 = num9 * num12 - num10 * num11;
			if (RavenMath.ravenAbs(num13) < 1E-06)
			{
				return;
			}
			double num14 = num12 / num13;
			double num15 = -num10 / num13;
			double num16 = -num11 / num13;
			double num17 = num9 / num13;
			RavenBitmap ravenBitmap = this.scaleMask(src, srcData, srcWidth, srcHeight, num7, num8);
			j = ((array2[2] <= array2[3]) ? 2 : 3);
			if (array2[1] <= array2[j])
			{
				j = 1;
			}
			if (array2[0] < array2[j] || (j != 3 && array2[0] == array2[j]))
			{
				j = 0;
			}
			int num18;
			if (array2[j] == array2[j + 1 & 3])
			{
				array3[0].y0 = Raven.imgCoordMungeLowerC(array2[j], glyphMode);
				array3[0].y1 = Raven.imgCoordMungeUpperC(array2[j + 2 & 3], glyphMode) - 1;
				if (array[j] < array[j + 1 & 3])
				{
					array3[0].ia0 = j;
					array3[0].ia1 = (j + 3 & 3);
					array3[0].ib0 = (j + 1 & 3);
					array3[0].ib1 = (j + 2 & 3);
				}
				else
				{
					array3[0].ia0 = (j + 1 & 3);
					array3[0].ia1 = (j + 2 & 3);
					array3[0].ib0 = j;
					array3[0].ib1 = (j + 3 & 3);
				}
				num18 = 1;
			}
			else
			{
				array3[0].y0 = Raven.imgCoordMungeLowerC(array2[j], glyphMode);
				array3[2].y1 = Raven.imgCoordMungeUpperC(array2[j + 2 & 3], glyphMode) - 1;
				array3[0].ia0 = (array3[0].ib0 = j);
				array3[2].ia1 = (array3[2].ib1 = (j + 2 & 3));
				if (array[j + 1 & 3] < array[j + 3 & 3])
				{
					array3[0].ia1 = (array3[2].ia0 = (j + 1 & 3));
					array3[0].ib1 = (array3[2].ib0 = (j + 3 & 3));
				}
				else
				{
					array3[0].ia1 = (array3[2].ia0 = (j + 3 & 3));
					array3[0].ib1 = (array3[2].ib0 = (j + 1 & 3));
				}
				if (array2[j + 1 & 3] < array2[j + 3 & 3])
				{
					array3[1].y0 = Raven.imgCoordMungeLowerC(array2[j + 1 & 3], glyphMode);
					array3[2].y0 = Raven.imgCoordMungeUpperC(array2[j + 3 & 3], glyphMode);
					if (array[j + 1 & 3] < array[j + 3 & 3])
					{
						array3[1].ia0 = (j + 1 & 3);
						array3[1].ia1 = (j + 2 & 3);
						array3[1].ib0 = j;
						array3[1].ib1 = (j + 3 & 3);
					}
					else
					{
						array3[1].ia0 = j;
						array3[1].ia1 = (j + 3 & 3);
						array3[1].ib0 = (j + 1 & 3);
						array3[1].ib1 = (j + 2 & 3);
					}
				}
				else
				{
					array3[1].y0 = Raven.imgCoordMungeLowerC(array2[j + 3 & 3], glyphMode);
					array3[2].y0 = Raven.imgCoordMungeUpperC(array2[j + 1 & 3], glyphMode);
					if (array[j + 1 & 3] < array[j + 3 & 3])
					{
						array3[1].ia0 = j;
						array3[1].ia1 = (j + 1 & 3);
						array3[1].ib0 = (j + 3 & 3);
						array3[1].ib1 = (j + 2 & 3);
					}
					else
					{
						array3[1].ia0 = (j + 3 & 3);
						array3[1].ia1 = (j + 2 & 3);
						array3[1].ib0 = j;
						array3[1].ib1 = (j + 1 & 3);
					}
				}
				array3[0].y1 = array3[1].y0 - 1;
				array3[1].y1 = array3[2].y0 - 1;
				num18 = 3;
			}
			for (j = 0; j < num18; j++)
			{
				array3[j].xa0 = array[array3[j].ia0];
				array3[j].ya0 = array2[array3[j].ia0];
				array3[j].xa1 = array[array3[j].ia1];
				array3[j].ya1 = array2[array3[j].ia1];
				array3[j].xb0 = array[array3[j].ib0];
				array3[j].yb0 = array2[array3[j].ib0];
				array3[j].xb1 = array[array3[j].ib1];
				array3[j].yb1 = array2[array3[j].ib1];
				array3[j].dxdya = (array3[j].xa1 - array3[j].xa0) / (array3[j].ya1 - array3[j].ya0);
				array3[j].dxdyb = (array3[j].xb1 - array3[j].xb0) / (array3[j].yb1 - array3[j].yb0);
			}
			this.pipeInit(ravenPipe, 0, 0, this.state.fillPattern, null, (byte)RavenMath.ravenRound(this.state.fillAlpha * 255.0), true, false);
			if (this.vectorAntialias)
			{
				this.drawAAPixelInit();
			}
			if (num18 == 1)
			{
				if (array3[0].y0 == array3[0].y1)
				{
					array3[0].y1++;
					ravenClipResult = (this.opClipRes = RavenClipResult.ravenClipPartial);
				}
			}
			else
			{
				if (array3[0].y0 == array3[2].y1)
				{
					array3[1].y1++;
					ravenClipResult = (this.opClipRes = RavenClipResult.ravenClipPartial);
				}
			}
			for (j = 0; j < num18; j++)
			{
				for (int k = array3[j].y0; k <= array3[j].y1; k++)
				{
					int num19 = Raven.imgCoordMungeLowerC(array3[j].xa0 + ((double)k + 0.5 - array3[j].ya0) * array3[j].dxdya, glyphMode);
					int num20 = Raven.imgCoordMungeUpperC(array3[j].xb0 + ((double)k + 0.5 - array3[j].yb0) * array3[j].dxdyb, glyphMode);
					if (num19 == num20)
					{
						num20++;
					}
					RavenClipResult ravenClipResult2;
					if (ravenClipResult != RavenClipResult.ravenClipAllInside)
					{
						ravenClipResult2 = this.state.clip.testSpan(num19, num20 - 1, k);
					}
					else
					{
						ravenClipResult2 = ravenClipResult;
					}
					for (int l = num19; l < num20; l++)
					{
						int num21 = RavenMath.ravenFloor(((double)l + 0.5 - mat[4]) * num14 + ((double)k + 0.5 - mat[5]) * num16);
						int num22 = RavenMath.ravenFloor(((double)l + 0.5 - mat[4]) * num15 + ((double)k + 0.5 - mat[5]) * num17);
						if (num21 < 0)
						{
							num21 = 0;
						}
						else
						{
							if (num21 >= num7)
							{
								num21 = num7 - 1;
							}
						}
						if (num22 < 0)
						{
							num22 = 0;
						}
						else
						{
							if (num22 >= num8)
							{
								num22 = num8 - 1;
							}
						}
						ravenPipe.shape = ravenBitmap.data[num22 * num7 + num21];
						if (this.vectorAntialias && ravenClipResult2 != RavenClipResult.ravenClipAllInside)
						{
							this.drawAAPixel(ravenPipe, l, k);
						}
						else
						{
							this.drawPixel(ravenPipe, l, k, ravenClipResult2 == RavenClipResult.ravenClipAllInside);
						}
					}
				}
			}
		}
		private void drawAAPixelInit()
		{
			this.aaBufY = -1;
		}
		private void drawAAPixel(RavenPipe pipe, int x, int y)
		{
			int[] array = new int[]
			{
				0,
				1,
				1,
				2,
				1,
				2,
				2,
				3,
				1,
				2,
				2,
				3,
				2,
				3,
				3,
				4
			};
			if (x < 0 || x >= this.bitmap.width || y < this.state.clip.getYMinI() || y > this.state.clip.getYMaxI())
			{
				return;
			}
			if (y != this.aaBufY)
			{
				for (int i = 0; i < this.aaBuf.getRowSize() * this.aaBuf.getHeight(); i++)
				{
					this.aaBuf.getDataPtr()[i] = 255;
				}
				int num = 0;
				int num2 = this.bitmap.width - 1;
				this.state.clip.clipAALine(this.aaBuf, ref num, ref num2, y);
				this.aaBufY = y;
			}
			RavenColorPtr ravenColorPtr = new RavenColorPtr(this.aaBuf.getDataPtr(), x >> 1);
			int rowSize = this.aaBuf.getRowSize();
			int num3;
			if ((x & 1) != 0)
			{
				num3 = array[(int)(ravenColorPtr.ptr & 15)] + array[(int)(ravenColorPtr[rowSize] & 15)] + array[(int)(ravenColorPtr[2 * rowSize] & 15)] + array[(int)(ravenColorPtr[3 * rowSize] & 15)];
			}
			else
			{
				num3 = array[ravenColorPtr.ptr >> 4] + array[ravenColorPtr[rowSize] >> 4] + array[ravenColorPtr[2 * rowSize] >> 4] + array[ravenColorPtr[3 * rowSize] >> 4];
			}
			if (num3 != 0)
			{
				this.pipeSetXY(pipe, x, y);
				pipe.shape = RavenMath.div255((int)(this.aaGamma[num3] * pipe.shape));
				pipe.run(pipe);
				this.updateModX(x);
				this.updateModY(y);
			}
		}
		internal RavenError drawImage(RavenImageSource src, object srcData, RavenColorMode srcMode, bool srcAlpha, int w, int h, double[] mat)
		{
			int nComps = 0;
			bool flag;
			switch (this.bitmap.mode)
			{
			case RavenColorMode.ravenModeMono1:
			case RavenColorMode.ravenModeMono8:
				flag = (srcMode == RavenColorMode.ravenModeMono8);
				nComps = 1;
				break;
			case RavenColorMode.ravenModeRGB8:
				flag = (srcMode == RavenColorMode.ravenModeRGB8);
				nComps = 3;
				break;
			case RavenColorMode.ravenModeBGR8:
				flag = (srcMode == RavenColorMode.ravenModeBGR8);
				nComps = 3;
				break;
			default:
				flag = false;
				break;
			}
			if (!flag)
			{
				return RavenError.ravenErrModeMismatch;
			}
			if (!RavenMath.ravenCheckDet(mat[0], mat[1], mat[2], mat[3], 1E-06))
			{
				return RavenError.ravenErrSingularMatrix;
			}
			bool flag2 = mat[1] == 0.0 && mat[2] == 0.0;
			if (mat[0] > 0.0 && flag2 && mat[3] > 0.0)
			{
				int num = Raven.imgCoordMungeLower(mat[4]);
				int num2 = Raven.imgCoordMungeLower(mat[5]);
				int num3 = Raven.imgCoordMungeUpper(mat[0] + mat[4]);
				int num4 = Raven.imgCoordMungeUpper(mat[3] + mat[5]);
				if (num == num3)
				{
					num3++;
				}
				if (num2 == num4)
				{
					num4++;
				}
				RavenClipResult ravenClipResult = this.state.clip.testRect(num, num2, num3 - 1, num4 - 1);
				this.opClipRes = ravenClipResult;
				if (ravenClipResult != RavenClipResult.ravenClipAllOutside)
				{
					int num5 = num3 - num;
					int num6 = num4 - num2;
					RavenBitmap ravenBitmap = this.scaleImage(src, srcData, srcMode, nComps, srcAlpha, w, h, num5, num6);
					this.blitImage(ravenBitmap, srcAlpha, num, num2, ravenClipResult);
				}
			}
			else
			{
				if (mat[0] > 0.0 && flag2 && mat[3] < 0.0)
				{
					int num = Raven.imgCoordMungeLower(mat[4]);
					int num2 = Raven.imgCoordMungeLower(mat[3] + mat[5]);
					int num3 = Raven.imgCoordMungeUpper(mat[0] + mat[4]);
					int num4 = Raven.imgCoordMungeUpper(mat[5]);
					if (num == num3)
					{
						if (mat[4] + mat[0] * 0.5 < (double)num)
						{
							num--;
						}
						else
						{
							num3++;
						}
					}
					if (num2 == num4)
					{
						if (mat[5] + mat[1] * 0.5 < (double)num2)
						{
							num2--;
						}
						else
						{
							num4++;
						}
					}
					RavenClipResult ravenClipResult = this.state.clip.testRect(num, num2, num3 - 1, num4 - 1);
					this.opClipRes = ravenClipResult;
					if (ravenClipResult != RavenClipResult.ravenClipAllOutside)
					{
						int num5 = num3 - num;
						int num6 = num4 - num2;
						RavenBitmap ravenBitmap = this.scaleImage(src, srcData, srcMode, nComps, srcAlpha, w, h, num5, num6);
						this.vertFlipImage(ravenBitmap, num5, num6, nComps);
						this.blitImage(ravenBitmap, srcAlpha, num, num2, ravenClipResult);
					}
				}
				else
				{
					this.arbitraryTransformImage(src, srcData, srcMode, nComps, srcAlpha, w, h, mat);
				}
			}
			return RavenError.ravenOk;
		}
		private RavenBitmap scaleImage(RavenImageSource src, object srcData, RavenColorMode srcMode, int nComps, bool srcAlpha, int srcWidth, int srcHeight, int scaledWidth, int scaledHeight)
		{
			RavenBitmap ravenBitmap = new RavenBitmap(scaledWidth, scaledHeight, 1, srcMode, srcAlpha, true);
			if (scaledHeight < srcHeight)
			{
				if (scaledWidth < srcWidth)
				{
					this.scaleImageYdXd(src, srcData, srcMode, nComps, srcAlpha, srcWidth, srcHeight, scaledWidth, scaledHeight, ravenBitmap);
				}
				else
				{
					this.scaleImageYdXu(src, srcData, srcMode, nComps, srcAlpha, srcWidth, srcHeight, scaledWidth, scaledHeight, ravenBitmap);
				}
			}
			else
			{
				if (scaledWidth < srcWidth)
				{
					this.scaleImageYuXd(src, srcData, srcMode, nComps, srcAlpha, srcWidth, srcHeight, scaledWidth, scaledHeight, ravenBitmap);
				}
				else
				{
					this.scaleImageYuXu(src, srcData, srcMode, nComps, srcAlpha, srcWidth, srcHeight, scaledWidth, scaledHeight, ravenBitmap);
				}
			}
			return ravenBitmap;
		}
		private void scaleImageYdXd(RavenImageSource src, object srcData, RavenColorMode srcMode, int nComps, bool srcAlpha, int srcWidth, int srcHeight, int scaledWidth, int scaledHeight, RavenBitmap dest)
		{
			int num = srcHeight / scaledHeight;
			int num2 = srcHeight % scaledHeight;
			int num3 = srcWidth / scaledWidth;
			int num4 = srcWidth % scaledWidth;
			byte[] array = new byte[srcWidth * nComps];
			uint[] array2 = new uint[srcWidth * nComps];
			byte[] array3;
			uint[] array4;
			if (srcAlpha)
			{
				array3 = new byte[srcWidth];
				array4 = new uint[srcWidth];
			}
			else
			{
				array3 = null;
				array4 = null;
			}
			int num5 = 0;
			RavenColorPtr ravenColorPtr = new RavenColorPtr(dest.data);
			RavenColorPtr ravenColorPtr2 = new RavenColorPtr(dest.alpha);
			for (int i = 0; i < scaledHeight; i++)
			{
				int num6;
				if ((num5 += num2) >= scaledHeight)
				{
					num5 -= scaledHeight;
					num6 = num + 1;
				}
				else
				{
					num6 = num;
				}
				RavenTypes.MemSet(array2, 0, 0u, srcWidth * nComps);
				if (srcAlpha)
				{
					RavenTypes.MemSet(array4, 0, 0u, srcWidth);
				}
				for (int j = 0; j < num6; j++)
				{
					src(srcData, new RavenColorPtr(array), array3);
					for (int k = 0; k < srcWidth * nComps; k++)
					{
						array2[k] += (uint)array[k];
					}
					if (srcAlpha)
					{
						for (int k = 0; k < srcWidth; k++)
						{
							array4[k] += (uint)array3[k];
						}
					}
				}
				int num7 = 0;
				int num8 = 8388608 / (num6 * num3);
				int num9 = 8388608 / (num6 * (num3 + 1));
				int num11;
				int num10 = num11 = 0;
				for (int l = 0; l < scaledWidth; l++)
				{
					int num12;
					int num13;
					if ((num7 += num4) >= scaledWidth)
					{
						num7 -= scaledWidth;
						num12 = num3 + 1;
						num13 = num9;
					}
					else
					{
						num12 = num3;
						num13 = num8;
					}
					switch (srcMode)
					{
					case RavenColorMode.ravenModeMono8:
					{
						uint num14 = 0u;
						for (int j = 0; j < num12; j++)
						{
							num14 += array2[num11++];
						}
						num14 = (uint)((ulong)num14 * (ulong)((long)num13) >> 23);
						ravenColorPtr.ptr = (byte)num14;
						ravenColorPtr = ++ravenColorPtr;
						break;
					}
					case RavenColorMode.ravenModeRGB8:
					{
						uint num14;
						uint num16;
						uint num15 = num14 = (num16 = 0u);
						for (int j = 0; j < num12; j++)
						{
							num14 += array2[num11];
							num15 += array2[num11 + 1];
							num16 += array2[num11 + 2];
							num11 += 3;
						}
						num14 = (uint)((ulong)num14 * (ulong)((long)num13) >> 23);
						num15 = (uint)((ulong)num15 * (ulong)((long)num13) >> 23);
						num16 = (uint)((ulong)num16 * (ulong)((long)num13) >> 23);
						ravenColorPtr.ptr = (byte)num14;
						ravenColorPtr = ++ravenColorPtr;
						ravenColorPtr.ptr = (byte)num15;
						ravenColorPtr = ++ravenColorPtr;
						ravenColorPtr.ptr = (byte)num16;
						ravenColorPtr = ++ravenColorPtr;
						break;
					}
					case RavenColorMode.ravenModeBGR8:
					{
						uint num14;
						uint num16;
						uint num15 = num14 = (num16 = 0u);
						for (int j = 0; j < num12; j++)
						{
							num14 += array2[num11];
							num15 += array2[num11 + 1];
							num16 += array2[num11 + 2];
							num11 += 3;
						}
						num14 = (uint)((ulong)num14 * (ulong)((long)num13) >> 23);
						num15 = (uint)((ulong)num15 * (ulong)((long)num13) >> 23);
						num16 = (uint)((ulong)num16 * (ulong)((long)num13) >> 23);
						ravenColorPtr.ptr = (byte)num16;
						ravenColorPtr = ++ravenColorPtr;
						ravenColorPtr.ptr = (byte)num15;
						ravenColorPtr = ++ravenColorPtr;
						ravenColorPtr.ptr = (byte)num14;
						ravenColorPtr = ++ravenColorPtr;
						break;
					}
					}
					if (srcAlpha)
					{
						uint num17 = 0u;
						int j = 0;
						while (j < num12)
						{
							num17 += array4[num10];
							j++;
							num10++;
						}
						num17 = (uint)((ulong)num17 * (ulong)((long)num13) >> 23);
						ravenColorPtr2.ptr = (byte)num17;
						ravenColorPtr2 = ++ravenColorPtr2;
					}
				}
			}
		}
		private void scaleImageYdXu(RavenImageSource src, object srcData, RavenColorMode srcMode, int nComps, bool srcAlpha, int srcWidth, int srcHeight, int scaledWidth, int scaledHeight, RavenBitmap dest)
		{
			uint[] array = new uint[RavenColor.ravenMaxColorComps];
			int num = srcHeight / scaledHeight;
			int num2 = srcHeight % scaledHeight;
			int num3 = scaledWidth / srcWidth;
			int num4 = scaledWidth % srcWidth;
			byte[] array2 = new byte[srcWidth * nComps];
			uint[] array3 = new uint[srcWidth * nComps];
			byte[] array4;
			uint[] array5;
			if (srcAlpha)
			{
				array4 = new byte[srcWidth];
				array5 = new uint[srcWidth];
			}
			else
			{
				array4 = null;
				array5 = null;
			}
			int num5 = 0;
			RavenColorPtr ravenColorPtr = new RavenColorPtr(dest.data);
			RavenColorPtr ravenColorPtr2 = new RavenColorPtr(dest.alpha);
			for (int i = 0; i < scaledHeight; i++)
			{
				int num6;
				if ((num5 += num2) >= scaledHeight)
				{
					num5 -= scaledHeight;
					num6 = num + 1;
				}
				else
				{
					num6 = num;
				}
				RavenTypes.MemSet(array3, 0, 0u, srcWidth * nComps);
				if (srcAlpha)
				{
					RavenTypes.MemSet(array5, 0, 0u, srcWidth);
				}
				for (int j = 0; j < num6; j++)
				{
					src(srcData, new RavenColorPtr(array2), array4);
					for (int k = 0; k < srcWidth * nComps; k++)
					{
						array3[k] += (uint)array2[k];
					}
					if (srcAlpha)
					{
						for (int k = 0; k < srcWidth; k++)
						{
							array5[k] += (uint)array4[k];
						}
					}
				}
				int num7 = 0;
				int num8 = 8388608 / num6;
				for (int l = 0; l < srcWidth; l++)
				{
					int num9;
					if ((num7 += num4) >= srcWidth)
					{
						num7 -= srcWidth;
						num9 = num3 + 1;
					}
					else
					{
						num9 = num3;
					}
					for (int j = 0; j < nComps; j++)
					{
						array[j] = (uint)((ulong)array3[l * nComps + j] * (ulong)((long)num8) >> 23);
					}
					switch (srcMode)
					{
					case RavenColorMode.ravenModeMono8:
						for (int j = 0; j < num9; j++)
						{
							ravenColorPtr.ptr = (byte)array[0];
							ravenColorPtr = ++ravenColorPtr;
						}
						break;
					case RavenColorMode.ravenModeRGB8:
						for (int j = 0; j < num9; j++)
						{
							ravenColorPtr.ptr = (byte)array[0];
							ravenColorPtr = ++ravenColorPtr;
							ravenColorPtr.ptr = (byte)array[1];
							ravenColorPtr = ++ravenColorPtr;
							ravenColorPtr.ptr = (byte)array[2];
							ravenColorPtr = ++ravenColorPtr;
						}
						break;
					case RavenColorMode.ravenModeBGR8:
						for (int j = 0; j < num9; j++)
						{
							ravenColorPtr.ptr = (byte)array[2];
							ravenColorPtr = ++ravenColorPtr;
							ravenColorPtr.ptr = (byte)array[1];
							ravenColorPtr = ++ravenColorPtr;
							ravenColorPtr.ptr = (byte)array[0];
							ravenColorPtr = ++ravenColorPtr;
						}
						break;
					}
					if (srcAlpha)
					{
						uint num10 = (uint)((ulong)array5[l] * (ulong)((long)num8) >> 23);
						for (int j = 0; j < num9; j++)
						{
							ravenColorPtr2.ptr = (byte)num10;
							ravenColorPtr2 = ++ravenColorPtr2;
						}
					}
				}
			}
		}
		private void scaleImageYuXd(RavenImageSource src, object srcData, RavenColorMode srcMode, int nComps, bool srcAlpha, int srcWidth, int srcHeight, int scaledWidth, int scaledHeight, RavenBitmap dest)
		{
			uint[] array = new uint[RavenColor.ravenMaxColorComps];
			int num = scaledHeight / srcHeight;
			int num2 = scaledHeight % srcHeight;
			int num3 = srcWidth / scaledWidth;
			int num4 = srcWidth % scaledWidth;
			byte[] array2 = new byte[srcWidth * nComps];
			byte[] array3;
			if (srcAlpha)
			{
				array3 = new byte[srcWidth];
			}
			else
			{
				array3 = null;
			}
			int num5 = 0;
			RavenColorPtr ravenColorPtr = new RavenColorPtr(dest.data);
			RavenColorPtr ravenColorPtr2 = new RavenColorPtr(dest.alpha);
			for (int i = 0; i < srcHeight; i++)
			{
				int num6;
				if ((num5 += num2) >= srcHeight)
				{
					num5 -= srcHeight;
					num6 = num + 1;
				}
				else
				{
					num6 = num;
				}
				src(srcData, new RavenColorPtr(array2), array3);
				int num7 = 0;
				int num8 = 8388608 / num3;
				int num9 = 8388608 / (num3 + 1);
				int num11;
				int num10 = num11 = 0;
				for (int j = 0; j < scaledWidth; j++)
				{
					int num12;
					int num13;
					if ((num7 += num4) >= scaledWidth)
					{
						num7 -= scaledWidth;
						num12 = num3 + 1;
						num13 = num9;
					}
					else
					{
						num12 = num3;
						num13 = num8;
					}
					for (int k = 0; k < nComps; k++)
					{
						array[k] = 0u;
					}
					for (int k = 0; k < num12; k++)
					{
						int l = 0;
						while (l < nComps)
						{
							array[l] += (uint)array2[num11];
							l++;
							num11++;
						}
					}
					for (int k = 0; k < nComps; k++)
					{
						array[k] = (uint)((ulong)array[k] * (ulong)((long)num13) >> 23);
					}
					switch (srcMode)
					{
					case RavenColorMode.ravenModeMono8:
                        for (int k = 0; k < num6; ++k)
                        {
                            RavenColorPtr ravenColorPtr3 = new RavenColorPtr(ravenColorPtr, (k * scaledWidth + j) * nComps);
                            ravenColorPtr3.ptr = (byte)array[0];
                            ravenColorPtr = ++ravenColorPtr3;
                        }
						break;
					case RavenColorMode.ravenModeRGB8:
						for (int k = 0; k < num6; k++)
						{
                            RavenColorPtr ravenColorPtr3 = new RavenColorPtr(ravenColorPtr, (k * scaledWidth + j) * nComps);
                            ravenColorPtr3.ptr = (byte)array[0];
                            ravenColorPtr3 = ++ravenColorPtr3;
							ravenColorPtr3.ptr = (byte)array[1];
							ravenColorPtr3 = ++ravenColorPtr3;
							ravenColorPtr3.ptr = (byte)array[2];
							ravenColorPtr3 = ++ravenColorPtr3;
						}
						break;
					case RavenColorMode.ravenModeBGR8:
						for (int k = 0; k < num6; k++)
						{
                            RavenColorPtr ravenColorPtr3 = new RavenColorPtr(ravenColorPtr, (k * scaledWidth + j) * nComps);
                            ravenColorPtr3.ptr = (byte)array[2];
                            ravenColorPtr3 = ++ravenColorPtr3;

							ravenColorPtr3.ptr = (byte)array[1];
							ravenColorPtr3 = ++ravenColorPtr3;
							ravenColorPtr3.ptr = (byte)array[0];
							ravenColorPtr3 = ++ravenColorPtr3;
						}
						break;
					}
					if (srcAlpha)
					{
						uint num14 = 0u;
						int k = 0;
						while (k < num12)
						{
							num14 += (uint)array3[num10];
							k++;
							num10++;
						}
						num14 = (uint)((ulong)num14 * (ulong)((long)num13) >> 23);
						for (k = 0; k < num6; k++)
						{
							RavenColorPtr ravenColorPtr4 = new RavenColorPtr(ravenColorPtr2, k * scaledWidth + j);
							ravenColorPtr4.ptr = (byte)num14;
						}
					}
				}
				ravenColorPtr.Inc(num6 * scaledWidth * nComps);
				if (srcAlpha)
				{
					ravenColorPtr2.Inc(num6 * scaledWidth);
				}
			}
		}
		private void scaleImageYuXu(RavenImageSource src, object srcData, RavenColorMode srcMode, int nComps, bool srcAlpha, int srcWidth, int srcHeight, int scaledWidth, int scaledHeight, RavenBitmap dest)
		{
			uint[] array = new uint[RavenColor.ravenMaxColorComps];
			int num = scaledHeight / srcHeight;
			int num2 = scaledHeight % srcHeight;
			int num3 = scaledWidth / srcWidth;
			int num4 = scaledWidth % srcWidth;
			byte[] array2 = new byte[srcWidth * nComps];
			byte[] array3;
			if (srcAlpha)
			{
				array3 = new byte[srcWidth];
			}
			else
			{
				array3 = null;
			}
			int num5 = 0;
			RavenColorPtr ravenColorPtr = new RavenColorPtr(dest.data);
			RavenColorPtr ravenColorPtr2 = new RavenColorPtr(dest.alpha);
			for (int i = 0; i < srcHeight; i++)
			{
				int num6;
				if ((num5 += num2) >= srcHeight)
				{
					num5 -= srcHeight;
					num6 = num + 1;
				}
				else
				{
					num6 = num;
				}
				src(srcData, new RavenColorPtr(array2), array3);
				int num7 = 0;
				int num8 = 0;
				for (int j = 0; j < srcWidth; j++)
				{
					int num9;
					if ((num7 += num4) >= srcWidth)
					{
						num7 -= srcWidth;
						num9 = num3 + 1;
					}
					else
					{
						num9 = num3;
					}
					for (int k = 0; k < nComps; k++)
					{
						array[k] = (uint)array2[j * nComps + k];
					}
					switch (srcMode)
					{
					case RavenColorMode.ravenModeMono8:
						for (int k = 0; k < num6; k++)
						{
							for (int l = 0; l < num9; l++)
							{

                                RavenColorPtr ravenColorPtr3 = new RavenColorPtr(ravenColorPtr, (k * scaledWidth + num8 + l) * nComps);
                                ravenColorPtr3.ptr = (byte)array[0];
                                ravenColorPtr3 = ++ravenColorPtr3;
							}
						}
						break;
					case RavenColorMode.ravenModeRGB8:
						for (int k = 0; k < num6; k++)
						{
							for (int l = 0; l < num9; l++)
							{

                                RavenColorPtr ravenColorPtr3 = new RavenColorPtr(ravenColorPtr, (k * scaledWidth + num8 + l) * nComps);
                                ravenColorPtr3.ptr = (byte)array[0];
                                ravenColorPtr3 = ++ravenColorPtr3;

								ravenColorPtr3.ptr = (byte)array[1];
								ravenColorPtr3 = ++ravenColorPtr3;
								ravenColorPtr3.ptr = (byte)array[2];
								ravenColorPtr3 = ++ravenColorPtr3;
							}
						}
						break;
					case RavenColorMode.ravenModeBGR8:
						for (int k = 0; k < num6; k++)
						{
							for (int l = 0; l < num9; l++)
							{

                                RavenColorPtr ravenColorPtr3 = new RavenColorPtr(ravenColorPtr, (k * scaledWidth + num8 + l) * nComps);
                                ravenColorPtr3.ptr = (byte)array[2];
                                ravenColorPtr3 = ++ravenColorPtr3;

								ravenColorPtr3.ptr = (byte)array[1];
								ravenColorPtr3 = ++ravenColorPtr3;
								ravenColorPtr3.ptr = (byte)array[0];
								ravenColorPtr3 = ++ravenColorPtr3;
							}
						}
						break;
					}
					if (srcAlpha)
					{
						uint num10 = (uint)array3[j];
						for (int k = 0; k < num6; k++)
						{
							for (int l = 0; l < num9; l++)
							{
								RavenColorPtr ravenColorPtr4 = new RavenColorPtr(ravenColorPtr2, k * scaledWidth + num8 + l);
								ravenColorPtr4.ptr = (byte)num10;
							}
						}
					}
					num8 += num9;
				}
				ravenColorPtr.Inc(num6 * scaledWidth * nComps);
				if (srcAlpha)
				{
					ravenColorPtr2.Inc(num6 * scaledWidth);
				}
			}
		}
		private void blitImage(RavenBitmap src, bool srcAlpha, int xDest, int yDest, RavenClipResult clipRes)
		{
			RavenPipe ravenPipe = new RavenPipe();
			RavenColor ravenColor = new RavenColor();
			int width = src.getWidth();
			int height = src.getHeight();
			int num;
			int num2;
			int num3;
			int num4;
			if (clipRes == RavenClipResult.ravenClipAllInside)
			{
				num = 0;
				num2 = 0;
				num3 = width;
				num4 = height;
			}
			else
			{
				if (this.state.clip.getNumPaths() != 0)
				{
					num3 = (num = width);
					num4 = (num2 = height);
				}
				else
				{
					if ((num = RavenMath.ravenCeil(this.state.clip.getXMin()) - xDest) < 0)
					{
						num = 0;
					}
					if ((num2 = RavenMath.ravenCeil(this.state.clip.getYMin()) - yDest) < 0)
					{
						num2 = 0;
					}
					if ((num3 = RavenMath.ravenFloor(this.state.clip.getXMax()) - xDest) > width)
					{
						num3 = width;
					}
					if (num3 < num)
					{
						num3 = num;
					}
					if ((num4 = RavenMath.ravenFloor(this.state.clip.getYMax()) - yDest) > height)
					{
						num4 = height;
					}
					if (num4 < num2)
					{
						num4 = num2;
					}
				}
			}
			if (num < width && num2 < height && num < num3 && num2 < num4)
			{
				this.pipeInit(ravenPipe, xDest + num, yDest + num2, null, new RavenColorPtr(ravenColor.getPtr()), (byte)RavenMath.ravenRound(this.state.fillAlpha * 255.0), srcAlpha, false);
				if (srcAlpha)
				{
					for (int i = num2; i < num4; i++)
					{
						this.pipeSetXY(ravenPipe, xDest + num, yDest + i);
						RavenColorPtr ravenColorPtr = new RavenColorPtr(src.getAlphaPtr(), i * width + num);
						for (int j = num; j < num3; j++)
						{
							src.getPixel(j, i, ravenColor);
							ravenPipe.shape = ravenColorPtr.ptr;
							ravenColorPtr = ++ravenColorPtr;
							ravenPipe.run(ravenPipe);
						}
					}
				}
				else
				{
					for (int i = num2; i < num4; i++)
					{
						this.pipeSetXY(ravenPipe, xDest + num, yDest + i);
						for (int j = num; j < num3; j++)
						{
							src.getPixel(j, i, ravenColor);
							ravenPipe.run(ravenPipe);
						}
					}
				}
				this.updateModX(xDest + num);
				this.updateModX(xDest + num3 - 1);
				this.updateModY(yDest + num2);
				this.updateModY(yDest + num4 - 1);
			}
			if (num2 > 0)
			{
				this.blitImageClipped(src, srcAlpha, 0, 0, xDest, yDest, width, num2);
			}
			if (num4 < height)
			{
				this.blitImageClipped(src, srcAlpha, 0, num4, xDest, yDest + num4, width, height - num4);
			}
			if (num > 0 && num2 < num4)
			{
				this.blitImageClipped(src, srcAlpha, 0, num2, xDest, yDest + num2, num, num4 - num2);
			}
			if (num3 < width && num2 < num4)
			{
				this.blitImageClipped(src, srcAlpha, num3, num2, xDest + num3, yDest + num2, width - num3, num4 - num2);
			}
		}
		private void blitImageClipped(RavenBitmap src, bool srcAlpha, int xSrc, int ySrc, int xDest, int yDest, int w, int h)
		{
			RavenPipe ravenPipe = new RavenPipe();
			RavenColor ravenColor = new RavenColor();
			if (this.vectorAntialias)
			{
				this.pipeInit(ravenPipe, xDest, yDest, null, new RavenColorPtr(ravenColor.getPtr()), (byte)RavenMath.ravenRound(this.state.fillAlpha * 255.0), true, false);
				this.drawAAPixelInit();
				if (srcAlpha)
				{
					for (int i = 0; i < h; i++)
					{
						RavenColorPtr ravenColorPtr = new RavenColorPtr(src.getAlphaPtr(), (ySrc + i) * src.getWidth() + xSrc);
						for (int j = 0; j < w; j++)
						{
							src.getPixel(xSrc + j, ySrc + i, ravenColor);
							ravenPipe.shape = ravenColorPtr.ptr;
							ravenColorPtr = ++ravenColorPtr;
							this.drawAAPixel(ravenPipe, xDest + j, yDest + i);
						}
					}
					return;
				}
				for (int i = 0; i < h; i++)
				{
					for (int j = 0; j < w; j++)
					{
						src.getPixel(xSrc + j, ySrc + i, ravenColor);
						ravenPipe.shape = 255;
						this.drawAAPixel(ravenPipe, xDest + j, yDest + i);
					}
				}
				return;
			}
			else
			{
				this.pipeInit(ravenPipe, xDest, yDest, null, new RavenColorPtr(ravenColor.getPtr()), (byte)RavenMath.ravenRound(this.state.fillAlpha * 255.0), srcAlpha, false);
				if (srcAlpha)
				{
					for (int i = 0; i < h; i++)
					{
						RavenColorPtr ravenColorPtr = new RavenColorPtr(src.getAlphaPtr(), (ySrc + i) * src.getWidth() + xSrc);
						this.pipeSetXY(ravenPipe, xDest, yDest + i);
						for (int j = 0; j < w; j++)
						{
							if (this.state.clip.test(xDest + j, yDest + i))
							{
								src.getPixel(xSrc + j, ySrc + i, ravenColor);
								ravenPipe.shape = ravenColorPtr.ptr;
								ravenColorPtr = ++ravenColorPtr;
								ravenPipe.run(ravenPipe);
								this.updateModX(xDest + j);
								this.updateModY(yDest + i);
							}
							else
							{
								this.pipeIncX(ravenPipe);
								ravenColorPtr = ++ravenColorPtr;
							}
						}
					}
					return;
				}
				for (int i = 0; i < h; i++)
				{
					this.pipeSetXY(ravenPipe, xDest, yDest + i);
					for (int j = 0; j < w; j++)
					{
						if (this.state.clip.test(xDest + j, yDest + i))
						{
							src.getPixel(xSrc + j, ySrc + i, ravenColor);
							ravenPipe.run(ravenPipe);
							this.updateModX(xDest + j);
							this.updateModY(yDest + i);
						}
						else
						{
							this.pipeIncX(ravenPipe);
						}
					}
				}
				return;
			}
		}
		private void arbitraryTransformImage(RavenImageSource src, object srcData, RavenColorMode srcMode, int nComps, bool srcAlpha, int srcWidth, int srcHeight, double[] mat)
		{
			RavenPipe ravenPipe = new RavenPipe();
			RavenColor ravenColor = new RavenColor();
			double[] array = new double[4];
			double[] array2 = new double[4];
			ImageSection[] array3 = new ImageSection[3];
			for (int i = 0; i < array3.Length; i++)
			{
				array3[i] = new ImageSection();
			}
			array[0] = mat[4];
			array2[0] = mat[5];
			array[1] = mat[2] + mat[4];
			array2[1] = mat[3] + mat[5];
			array[2] = mat[0] + mat[2] + mat[4];
			array2[2] = mat[1] + mat[3] + mat[5];
			array[3] = mat[0] + mat[4];
			array2[3] = mat[1] + mat[5];
			int num = Raven.imgCoordMungeLower(array[0]);
			int num2 = Raven.imgCoordMungeUpper(array[0]);
			int num3 = Raven.imgCoordMungeLower(array2[0]);
			int num4 = Raven.imgCoordMungeUpper(array2[0]);
			int j;
			int num5;
			int num6;
			for (j = 1; j < 4; j++)
			{
				num5 = Raven.imgCoordMungeLower(array[j]);
				if (num5 < num)
				{
					num = num5;
				}
				num5 = Raven.imgCoordMungeUpper(array[j]);
				if (num5 > num2)
				{
					num2 = num5;
				}
				num6 = Raven.imgCoordMungeLower(array2[j]);
				if (num6 < num3)
				{
					num3 = num6;
				}
				num6 = Raven.imgCoordMungeUpper(array2[j]);
				if (num6 > num4)
				{
					num4 = num6;
				}
			}
			RavenClipResult ravenClipResult = this.state.clip.testRect(num, num3, num2 - 1, num4 - 1);
			this.opClipRes = ravenClipResult;
			if (ravenClipResult == RavenClipResult.ravenClipAllOutside)
			{
				return;
			}
			if (mat[0] >= 0.0)
			{
				num5 = Raven.imgCoordMungeUpper(mat[0] + mat[4]) - Raven.imgCoordMungeLower(mat[4]);
			}
			else
			{
				num5 = Raven.imgCoordMungeUpper(mat[4]) - Raven.imgCoordMungeLower(mat[0] + mat[4]);
			}
			if (mat[1] >= 0.0)
			{
				num6 = Raven.imgCoordMungeUpper(mat[1] + mat[5]) - Raven.imgCoordMungeLower(mat[5]);
			}
			else
			{
				num6 = Raven.imgCoordMungeUpper(mat[5]) - Raven.imgCoordMungeLower(mat[1] + mat[5]);
			}
			int num7 = (num5 > num6) ? num5 : num6;
			if (mat[2] >= 0.0)
			{
				num5 = Raven.imgCoordMungeUpper(mat[2] + mat[4]) - Raven.imgCoordMungeLower(mat[4]);
			}
			else
			{
				num5 = Raven.imgCoordMungeUpper(mat[4]) - Raven.imgCoordMungeLower(mat[2] + mat[4]);
			}
			if (mat[3] >= 0.0)
			{
				num6 = Raven.imgCoordMungeUpper(mat[3] + mat[5]) - Raven.imgCoordMungeLower(mat[5]);
			}
			else
			{
				num6 = Raven.imgCoordMungeUpper(mat[5]) - Raven.imgCoordMungeLower(mat[3] + mat[5]);
			}
			int num8 = (num5 > num6) ? num5 : num6;
			if (num7 == 0)
			{
				num7 = 1;
			}
			if (num8 == 0)
			{
				num8 = 1;
			}
			double num9 = mat[0] / (double)num7;
			double num10 = mat[1] / (double)num7;
			double num11 = mat[2] / (double)num8;
			double num12 = mat[3] / (double)num8;
			double num13 = num9 * num12 - num10 * num11;
			if (RavenMath.ravenAbs(num13) < 1E-06)
			{
				return;
			}
			double num14 = num12 / num13;
			double num15 = -num10 / num13;
			double num16 = -num11 / num13;
			double num17 = num9 / num13;
			RavenBitmap ravenBitmap = this.scaleImage(src, srcData, srcMode, nComps, srcAlpha, srcWidth, srcHeight, num7, num8);
			j = 0;
			if (array2[1] < array2[j])
			{
				j = 1;
			}
			if (array2[2] < array2[j])
			{
				j = 2;
			}
			if (array2[3] < array2[j])
			{
				j = 3;
			}
			if (RavenMath.ravenAbs(array2[j] - array2[j - 1 & 3]) <= 1E-06 && array2[j - 1 & 3] < array2[j + 1 & 3])
			{
				j = (j - 1 & 3);
			}
			int num18;
			if (RavenMath.ravenAbs(array2[j] - array2[j + 1 & 3]) <= 1E-06)
			{
				array3[0].y0 = Raven.imgCoordMungeLower(array2[j]);
				array3[0].y1 = Raven.imgCoordMungeUpper(array2[j + 2 & 3]) - 1;
				if (array[j] < array[j + 1 & 3])
				{
					array3[0].ia0 = j;
					array3[0].ia1 = (j + 3 & 3);
					array3[0].ib0 = (j + 1 & 3);
					array3[0].ib1 = (j + 2 & 3);
				}
				else
				{
					array3[0].ia0 = (j + 1 & 3);
					array3[0].ia1 = (j + 2 & 3);
					array3[0].ib0 = j;
					array3[0].ib1 = (j + 3 & 3);
				}
				num18 = 1;
			}
			else
			{
				array3[0].y0 = Raven.imgCoordMungeLower(array2[j]);
				array3[2].y1 = Raven.imgCoordMungeUpper(array2[j + 2 & 3]) - 1;
				array3[0].ia0 = (array3[0].ib0 = j);
				array3[2].ia1 = (array3[2].ib1 = (j + 2 & 3));
				if (array[j + 1 & 3] < array[j + 3 & 3])
				{
					array3[0].ia1 = (array3[2].ia0 = (j + 1 & 3));
					array3[0].ib1 = (array3[2].ib0 = (j + 3 & 3));
				}
				else
				{
					array3[0].ia1 = (array3[2].ia0 = (j + 3 & 3));
					array3[0].ib1 = (array3[2].ib0 = (j + 1 & 3));
				}
				if (array2[j + 1 & 3] < array2[j + 3 & 3])
				{
					array3[1].y0 = Raven.imgCoordMungeLower(array2[j + 1 & 3]);
					array3[2].y0 = Raven.imgCoordMungeUpper(array2[j + 3 & 3]);
					if (array[j + 1 & 3] < array[j + 3 & 3])
					{
						array3[1].ia0 = (j + 1 & 3);
						array3[1].ia1 = (j + 2 & 3);
						array3[1].ib0 = j;
						array3[1].ib1 = (j + 3 & 3);
					}
					else
					{
						array3[1].ia0 = j;
						array3[1].ia1 = (j + 3 & 3);
						array3[1].ib0 = (j + 1 & 3);
						array3[1].ib1 = (j + 2 & 3);
					}
				}
				else
				{
					array3[1].y0 = Raven.imgCoordMungeLower(array2[j + 3 & 3]);
					array3[2].y0 = Raven.imgCoordMungeUpper(array2[j + 1 & 3]);
					if (array[j + 1 & 3] < array[j + 3 & 3])
					{
						array3[1].ia0 = j;
						array3[1].ia1 = (j + 1 & 3);
						array3[1].ib0 = (j + 3 & 3);
						array3[1].ib1 = (j + 2 & 3);
					}
					else
					{
						array3[1].ia0 = (j + 3 & 3);
						array3[1].ia1 = (j + 2 & 3);
						array3[1].ib0 = j;
						array3[1].ib1 = (j + 1 & 3);
					}
				}
				array3[0].y1 = array3[1].y0 - 1;
				array3[1].y1 = array3[2].y0 - 1;
				num18 = 3;
			}
			for (j = 0; j < num18; j++)
			{
				array3[j].xa0 = array[array3[j].ia0];
				array3[j].ya0 = array2[array3[j].ia0];
				array3[j].xa1 = array[array3[j].ia1];
				array3[j].ya1 = array2[array3[j].ia1];
				array3[j].xb0 = array[array3[j].ib0];
				array3[j].yb0 = array2[array3[j].ib0];
				array3[j].xb1 = array[array3[j].ib1];
				array3[j].yb1 = array2[array3[j].ib1];
				array3[j].dxdya = (array3[j].xa1 - array3[j].xa0) / (array3[j].ya1 - array3[j].ya0);
				array3[j].dxdyb = (array3[j].xb1 - array3[j].xb0) / (array3[j].yb1 - array3[j].yb0);
			}
			this.pipeInit(ravenPipe, 0, 0, null, new RavenColorPtr(ravenColor.getPtr()), (byte)RavenMath.ravenRound(this.state.fillAlpha * 255.0), srcAlpha || (this.vectorAntialias && ravenClipResult != RavenClipResult.ravenClipAllInside), false);
			if (this.vectorAntialias)
			{
				this.drawAAPixelInit();
			}
			if (num18 == 1)
			{
				if (array3[0].y0 == array3[0].y1)
				{
					array3[0].y1++;
					ravenClipResult = (this.opClipRes = RavenClipResult.ravenClipPartial);
				}
			}
			else
			{
				if (array3[0].y0 == array3[2].y1)
				{
					array3[1].y1++;
					ravenClipResult = (this.opClipRes = RavenClipResult.ravenClipPartial);
				}
			}
			for (j = 0; j < num18; j++)
			{
				for (int k = array3[j].y0; k <= array3[j].y1; k++)
				{
					int num19 = Raven.imgCoordMungeLower(array3[j].xa0 + ((double)k + 0.5 - array3[j].ya0) * array3[j].dxdya);
					int num20 = Raven.imgCoordMungeUpper(array3[j].xb0 + ((double)k + 0.5 - array3[j].yb0) * array3[j].dxdyb);
					if (num19 == num20)
					{
						num20++;
					}
					RavenClipResult ravenClipResult2;
					if (ravenClipResult != RavenClipResult.ravenClipAllInside)
					{
						ravenClipResult2 = this.state.clip.testSpan(num19, num20 - 1, k);
					}
					else
					{
						ravenClipResult2 = ravenClipResult;
					}
					for (int l = num19; l < num20; l++)
					{
						int num21 = RavenMath.ravenFloor(((double)l + 0.5 - mat[4]) * num14 + ((double)k + 0.5 - mat[5]) * num16);
						int num22 = RavenMath.ravenFloor(((double)l + 0.5 - mat[4]) * num15 + ((double)k + 0.5 - mat[5]) * num17);
						if (num21 < 0)
						{
							num21 = 0;
						}
						else
						{
							if (num21 >= num7)
							{
								num21 = num7 - 1;
							}
						}
						if (num22 < 0)
						{
							num22 = 0;
						}
						else
						{
							if (num22 >= num8)
							{
								num22 = num8 - 1;
							}
						}
						ravenBitmap.getPixel(num21, num22, ravenColor);
						if (srcAlpha)
						{
							ravenPipe.shape = ravenBitmap.alpha[num22 * num7 + num21];
						}
						else
						{
							ravenPipe.shape = 255;
						}
						if (this.vectorAntialias && ravenClipResult2 != RavenClipResult.ravenClipAllInside)
						{
							this.drawAAPixel(ravenPipe, l, k);
						}
						else
						{
							this.drawPixel(ravenPipe, l, k, ravenClipResult2 == RavenClipResult.ravenClipAllInside);
						}
					}
				}
			}
		}
		internal RavenError blitTransparent(RavenBitmap src, int xSrc, int ySrc, int xDest, int yDest, int w, int h)
		{
			if (src.mode != this.bitmap.mode)
			{
				return RavenError.ravenErrModeMismatch;
			}
			switch (this.bitmap.mode)
			{
			case RavenColorMode.ravenModeMono1:
				for (int i = 0; i < h; i++)
				{
					RavenColorPtr ravenColorPtr = new RavenColorPtr(this.bitmap.data, (yDest + i) * this.bitmap.rowSize + (xDest >> 3));
					int num = 128 >> (xDest & 7);
					RavenColorPtr ravenColorPtr2 = new RavenColorPtr(src.data, (ySrc + i) * src.rowSize + (xSrc >> 3));
					int num2 = 128 >> (xSrc & 7);
					for (int j = 0; j < w; j++)
					{
						if (((int)ravenColorPtr2.ptr & num2) != 0)
						{
							RavenColorPtr expr_B2 = ravenColorPtr;
							expr_B2.ptr |= (byte)num;
						}
						else
						{
							RavenColorPtr expr_C5 = ravenColorPtr;
							expr_C5.ptr &= (byte)(num ^ 255);
						}
						if ((num >>= 1) == 0)
						{
							num = 128;
							ravenColorPtr = ++ravenColorPtr;
						}
						if ((num2 >>= 1) == 0)
						{
							num2 = 128;
							ravenColorPtr2 = ++ravenColorPtr2;
						}
					}
				}
				break;
			case RavenColorMode.ravenModeMono8:
				for (int i = 0; i < h; i++)
				{
					RavenColorPtr ravenColorPtr = new RavenColorPtr(this.bitmap.data, (yDest + i) * this.bitmap.rowSize + xDest);
					RavenColorPtr ravenColorPtr2 = new RavenColorPtr(src.data, (ySrc + i) * src.rowSize + xSrc);
					for (int j = 0; j < w; j++)
					{
						ravenColorPtr.ptr = ravenColorPtr2.ptr;
						ravenColorPtr = ++ravenColorPtr;
						ravenColorPtr2 = ++ravenColorPtr2;
					}
				}
				break;
			case RavenColorMode.ravenModeRGB8:
			case RavenColorMode.ravenModeBGR8:
				for (int i = 0; i < h; i++)
				{
					RavenColorPtr ravenColorPtr = new RavenColorPtr(this.bitmap.data, (yDest + i) * this.bitmap.rowSize + 3 * xDest);
					RavenColorPtr ravenColorPtr2 = new RavenColorPtr(src.data, (ySrc + i) * src.rowSize + 3 * xSrc);
					for (int j = 0; j < w; j++)
					{
						ravenColorPtr.ptr = ravenColorPtr2.ptr;
						ravenColorPtr = ++ravenColorPtr;
						ravenColorPtr2 = ++ravenColorPtr2;
						ravenColorPtr.ptr = ravenColorPtr2.ptr;
						ravenColorPtr = ++ravenColorPtr;
						ravenColorPtr2 = ++ravenColorPtr2;
						ravenColorPtr.ptr = ravenColorPtr2.ptr;
						ravenColorPtr = ++ravenColorPtr;
						ravenColorPtr2 = ++ravenColorPtr2;
					}
				}
				break;
			}
			if (this.bitmap.alpha != null)
			{
				for (int i = 0; i < h; i++)
				{
					RavenColorPtr ravenColorPtr2 = new RavenColorPtr(this.bitmap.alpha, (yDest + i) * this.bitmap.width + xDest);
					for (int j = 0; j < w; j++)
					{
						ravenColorPtr2.ptr = 0;
						ravenColorPtr2 = ++ravenColorPtr2;
					}
				}
			}
			return RavenError.ravenOk;
		}
		internal void setInNonIsolatedGroup(RavenBitmap alpha0BitmapA, int alpha0XA, int alpha0YA)
		{
			this.alpha0Bitmap = alpha0BitmapA;
			this.alpha0X = alpha0XA;
			this.alpha0Y = alpha0YA;
			this.state.inNonIsolatedGroup = true;
		}
		internal void compositeBackground(RavenColorPtr color)
		{
			switch (this.bitmap.mode)
			{
			case RavenColorMode.ravenModeMono1:
			{
				byte b = color[0];
				for (int i = 0; i < this.bitmap.height; i++)
				{
					RavenColorPtr ravenColorPtr = new RavenColorPtr(this.bitmap.data, i * this.bitmap.rowSize);
					RavenColorPtr ravenColorPtr2 = new RavenColorPtr(this.bitmap.alpha, i * this.bitmap.width);
					int num = 128;
					for (int j = 0; j < this.bitmap.width; j++)
					{
						byte ptr = ravenColorPtr2.ptr;
						ravenColorPtr2 = ++ravenColorPtr2;
						byte b2 = (byte)((uint)byte.MaxValue - (uint)ptr);
						byte b3 = (((int)ravenColorPtr.ptr & num) != 0) ? byte.MaxValue : (byte)0;
						b3 = RavenMath.div255((int)(b2 * b + ptr * b3));
						if ((b3 & 128) != 0)
						{
							RavenColorPtr expr_CE = ravenColorPtr;
							expr_CE.ptr |= (byte)num;
						}
						else
						{
							RavenColorPtr expr_E1 = ravenColorPtr;
							expr_E1.ptr &= (byte)(num ^ 255);
						}
						if ((num >>= 1) == 0)
						{
							num = 128;
							ravenColorPtr = ++ravenColorPtr;
						}
					}
				}
				break;
			}
			case RavenColorMode.ravenModeMono8:
			{
				byte b = color[0];
				for (int i = 0; i < this.bitmap.height; i++)
				{
					RavenColorPtr ravenColorPtr = new RavenColorPtr(this.bitmap.data, i * this.bitmap.rowSize);
					RavenColorPtr ravenColorPtr2 = new RavenColorPtr(this.bitmap.alpha, i * this.bitmap.width);
					for (int j = 0; j < this.bitmap.width; j++)
					{
						byte ptr = ravenColorPtr2.ptr;
						ravenColorPtr2 = ++ravenColorPtr2;
						byte b2 = (byte)((uint)byte.MaxValue - (uint)ptr);
						ravenColorPtr[0] = RavenMath.div255((int)(b2 * b + ptr * ravenColorPtr[0]));
						ravenColorPtr = ++ravenColorPtr;
					}
				}
				break;
			}
			case RavenColorMode.ravenModeRGB8:
			case RavenColorMode.ravenModeBGR8:
			{
				byte b = color[0];
				byte b4 = color[1];
				byte b5 = color[2];
				for (int i = 0; i < this.bitmap.height; i++)
				{
					RavenColorPtr ravenColorPtr = new RavenColorPtr(this.bitmap.data, i * this.bitmap.rowSize);
					RavenColorPtr ravenColorPtr2 = new RavenColorPtr(this.bitmap.alpha, i * this.bitmap.width);
					for (int j = 0; j < this.bitmap.width; j++)
					{
						byte ptr = ravenColorPtr2.ptr;
						ravenColorPtr2 = ++ravenColorPtr2;
						byte b2 = (byte)((uint)byte.MaxValue - (uint)ptr);
						ravenColorPtr[0] = RavenMath.div255((int)(b2 * b + ptr * ravenColorPtr[0]));
						ravenColorPtr[1] = RavenMath.div255((int)(b2 * b4 + ptr * ravenColorPtr[1]));
						ravenColorPtr[2] = RavenMath.div255((int)(b2 * b5 + ptr * ravenColorPtr[2]));
						ravenColorPtr.Inc(3);
					}
				}
				break;
			}
			}
			RavenTypes.MemSet(this.bitmap.alpha, 0, 255, this.bitmap.width * this.bitmap.height);
		}
		internal RavenError composite(RavenBitmap src, int xSrc, int ySrc, int xDest, int yDest, int w, int h, bool noClip, bool nonIsolated)
		{
			RavenPipe ravenPipe = new RavenPipe();
			RavenColor ravenColor = new RavenColor();
			if (src.mode != this.bitmap.mode)
			{
				return RavenError.ravenErrModeMismatch;
			}
			if (src.alpha != null)
			{
				this.pipeInit(ravenPipe, xDest, yDest, null, new RavenColorPtr(ravenColor.getPtr()), (byte)RavenMath.ravenRound(this.state.fillAlpha * 255.0), true, nonIsolated);
				if (noClip)
				{
					for (int i = 0; i < h; i++)
					{
						this.pipeSetXY(ravenPipe, xDest, yDest + i);
						RavenColorPtr ravenColorPtr = new RavenColorPtr(src.getAlphaPtr(), (ySrc + i) * src.getWidth() + xSrc);
						for (int j = 0; j < w; j++)
						{
							src.getPixel(xSrc + j, ySrc + i, ravenColor);
							byte ptr = ravenColorPtr.ptr;
							ravenColorPtr = ++ravenColorPtr;
							ravenPipe.shape = ptr;
							ravenPipe.run(ravenPipe);
						}
					}
					this.updateModX(xDest);
					this.updateModX(xDest + w - 1);
					this.updateModY(yDest);
					this.updateModY(yDest + h - 1);
				}
				else
				{
					for (int i = 0; i < h; i++)
					{
						this.pipeSetXY(ravenPipe, xDest, yDest + i);
						RavenColorPtr ravenColorPtr = new RavenColorPtr(src.getAlphaPtr(), (ySrc + i) * src.getWidth() + xSrc);
						for (int j = 0; j < w; j++)
						{
							src.getPixel(xSrc + j, ySrc + i, ravenColor);
							byte ptr = ravenColorPtr.ptr;
							ravenColorPtr = ++ravenColorPtr;
							if (this.state.clip.test(xDest + j, yDest + i))
							{
								ravenPipe.shape = ptr;
								ravenPipe.run(ravenPipe);
								this.updateModX(xDest + j);
								this.updateModY(yDest + i);
							}
							else
							{
								this.pipeIncX(ravenPipe);
							}
						}
					}
				}
			}
			else
			{
				this.pipeInit(ravenPipe, xDest, yDest, null, new RavenColorPtr(ravenColor.getPtr()), (byte)RavenMath.ravenRound(this.state.fillAlpha * 255.0), false, nonIsolated);
				if (noClip)
				{
					for (int i = 0; i < h; i++)
					{
						this.pipeSetXY(ravenPipe, xDest, yDest + i);
						for (int j = 0; j < w; j++)
						{
							src.getPixel(xSrc + j, ySrc + i, ravenColor);
							ravenPipe.run(ravenPipe);
						}
					}
					this.updateModX(xDest);
					this.updateModX(xDest + w - 1);
					this.updateModY(yDest);
					this.updateModY(yDest + h - 1);
				}
				else
				{
					for (int i = 0; i < h; i++)
					{
						this.pipeSetXY(ravenPipe, xDest, yDest + i);
						for (int j = 0; j < w; j++)
						{
							src.getPixel(xSrc + j, ySrc + i, ravenColor);
							if (this.state.clip.test(xDest + j, yDest + i))
							{
								ravenPipe.run(ravenPipe);
								this.updateModX(xDest + j);
								this.updateModY(yDest + i);
							}
							else
							{
								this.pipeIncX(ravenPipe);
							}
						}
					}
				}
			}
			return RavenError.ravenOk;
		}
		private static int imgCoordMungeLower(double x)
		{
			return RavenMath.ravenFloor(x);
		}
		private static int imgCoordMungeUpper(double x)
		{
			return RavenMath.ravenFloor(x) + 1;
		}
		private static int imgCoordMungeLowerC(double x, bool glyphMode)
		{
			if (!glyphMode)
			{
				return RavenMath.ravenFloor(x);
			}
			return RavenMath.ravenCeil(x + 0.5) - 1;
		}
		private static int imgCoordMungeUpperC(double x, bool glyphMode)
		{
			if (!glyphMode)
			{
				return RavenMath.ravenFloor(x) + 1;
			}
			return RavenMath.ravenCeil(x + 0.5) - 1;
		}
		internal RavenPattern getFillPattern()
		{
			return this.state.fillPattern;
		}
		internal RavenPattern getStrokePattern()
		{
			return this.state.strokePattern;
		}
		internal void setOverprintMask(uint overprintMask)
		{
			this.state.overprintMask = overprintMask;
		}
	}
}
