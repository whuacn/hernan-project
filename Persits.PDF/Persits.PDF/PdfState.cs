using System;
namespace Persits.PDF
{
	internal class PdfState
	{
		internal const int ColorMaxComps = 32;
		internal const int pdfColorComp1 = 65536;
		internal const int ravenLineCapButt = 0;
		internal const int ravenLineCapRound = 1;
		internal const int ravenLineCapProjecting = 2;
		internal const int ravenLineJoinMiter = 0;
		internal const int ravenLineJoinRound = 1;
		internal const int ravenLineJoinBevel = 2;
		private static PdfBlendModeInfo[] BlendModeNames = new PdfBlendModeInfo[]
		{
			new PdfBlendModeInfo("Normal", enumBlendMode.bmBlendNormal),
			new PdfBlendModeInfo("Compatible", enumBlendMode.bmBlendNormal),
			new PdfBlendModeInfo("Multiply", enumBlendMode.bmBlendMultiply),
			new PdfBlendModeInfo("Screen", enumBlendMode.bmBlendScreen),
			new PdfBlendModeInfo("Overlay", enumBlendMode.bmBlendOverlay),
			new PdfBlendModeInfo("Darken", enumBlendMode.bmBlendDarken),
			new PdfBlendModeInfo("Lighten", enumBlendMode.bmBlendLighten),
			new PdfBlendModeInfo("ColorDodge", enumBlendMode.bmBlendColorDodge),
			new PdfBlendModeInfo("ColorBurn", enumBlendMode.bmBlendColorBurn),
			new PdfBlendModeInfo("HardLight", enumBlendMode.bmBlendHardLight),
			new PdfBlendModeInfo("SoftLight", enumBlendMode.bmBlendSoftLight),
			new PdfBlendModeInfo("Difference", enumBlendMode.bmBlendDifference),
			new PdfBlendModeInfo("Exclusion", enumBlendMode.bmBlendExclusion),
			new PdfBlendModeInfo("Hue", enumBlendMode.bmBlendHue),
			new PdfBlendModeInfo("Saturation", enumBlendMode.bmBlendSaturation),
			new PdfBlendModeInfo("Color", enumBlendMode.bmBlendColor),
			new PdfBlendModeInfo("Luminosity", enumBlendMode.bmBlendLuminosity)
		};
		private PdfPath m_pPath;
		private double m_fCurX;
		private double m_fCurY;
		private double m_fLineX;
		private double m_fLineY;
		internal double m_hDPI;
		internal double m_vDPI;
		internal double[] m_ctm = new double[6];
		internal double[] m_textMat = new double[6];
		internal double px1;
		internal double py1;
		internal double px2;
		internal double py2;
		internal double m_fPageWidth;
		internal double m_fPageHeight;
		internal double m_fLineWidth;
		internal double[] m_pLineDash;
		internal int m_nLineDashLength;
		internal double m_fLineDashStart;
		internal int m_nFlatness;
		internal int m_nLineJoin;
		internal int m_nLineCap;
		internal double m_fMiterLimit;
		internal bool m_strokeAdjust;
		internal double m_fCharSpace;
		internal double m_fWordSpace;
		internal double m_fHorizScaling;
		internal double m_fLeading;
		internal double m_fRise;
		internal PdfColorSpaceTop m_fillColorSpace;
		internal PdfColorSpaceTop m_strokeColorSpace;
		internal PdfColor m_fillColor;
		internal PdfColor m_strokeColor;
		internal PdfPattern m_fillPattern;
		internal PdfPattern m_strokePattern;
		internal enumBlendMode m_blendMode;
		internal bool m_fillOverprint;
		internal bool m_strokeOverprint;
		internal int m_overprintMode;
		private PdfFunctionTop[] m_pTransfer = new PdfFunctionTop[20];
		internal double m_fFillOpacity;
		internal double m_fStrokeOpacity;
		internal double m_fClipXMin;
		internal double m_fClipYMin;
		internal double m_fClipXMax;
		internal double m_fClipYMax;
		private PdfPreviewFont m_pFont;
		internal double m_fFontSize;
		internal int m_nRender;
		internal PdfState m_pSaved;
		private PdfPreview m_pPreview;
		public PdfState(double hDPI, double vDPI, double fLeft, double fBottom, double fRight, double fTop, int rotate, bool upsideDown, PdfPreview pPreview)
		{
			this.m_hDPI = hDPI;
			this.m_vDPI = vDPI;
			this.px1 = fLeft;
			this.py1 = fBottom;
			this.px2 = fRight;
			this.py2 = fTop;
			double num = hDPI / 72.0;
			double num2 = vDPI / 72.0;
			if (rotate == 90)
			{
				this.m_ctm[0] = 0.0;
				this.m_ctm[1] = (upsideDown ? num2 : (-num2));
				this.m_ctm[2] = num;
				this.m_ctm[3] = 0.0;
				this.m_ctm[4] = -num * this.py1;
				this.m_ctm[5] = num2 * (upsideDown ? (-this.px1) : this.px2);
				this.m_fPageWidth = num * (this.py2 - this.py1);
				this.m_fPageHeight = num2 * (this.px2 - this.px1);
			}
			else
			{
				if (rotate == 180)
				{
					this.m_ctm[0] = -num;
					this.m_ctm[1] = 0.0;
					this.m_ctm[2] = 0.0;
					this.m_ctm[3] = (upsideDown ? num2 : (-num2));
					this.m_ctm[4] = num * this.px2;
					this.m_ctm[5] = num2 * (upsideDown ? (-this.py1) : this.py2);
					this.m_fPageWidth = num * (this.px2 - this.px1);
					this.m_fPageHeight = num2 * (this.py2 - this.py1);
				}
				else
				{
					if (rotate == 270)
					{
						this.m_ctm[0] = 0.0;
						this.m_ctm[1] = (upsideDown ? (-num2) : num2);
						this.m_ctm[2] = -num;
						this.m_ctm[3] = 0.0;
						this.m_ctm[4] = num * this.py2;
						this.m_ctm[5] = num2 * (upsideDown ? this.px2 : (-this.px1));
						this.m_fPageWidth = num * (this.py2 - this.py1);
						this.m_fPageHeight = num2 * (this.px2 - this.px1);
					}
					else
					{
						this.m_ctm[0] = num;
						this.m_ctm[1] = 0.0;
						this.m_ctm[2] = 0.0;
						this.m_ctm[3] = (upsideDown ? (-num2) : num2);
						this.m_ctm[4] = -num * this.px1;
						this.m_ctm[5] = num2 * (upsideDown ? this.py2 : (-this.py1));
						this.m_fPageWidth = num * (this.px2 - this.px1);
						this.m_fPageHeight = num2 * (this.py2 - this.py1);
					}
				}
			}
			this.m_fillColorSpace = new PdfDeviceGrayColorSpace();
			this.m_strokeColorSpace = new PdfDeviceGrayColorSpace();
			this.m_fillColor = new PdfColor();
			this.m_fillColor.c[0] = 0;
			this.m_strokeColor = new PdfColor();
			this.m_strokeColor.c[0] = 0;
			this.m_fillPattern = null;
			this.m_strokePattern = null;
			this.m_blendMode = enumBlendMode.bmBlendNormal;
			this.m_fFillOpacity = 1.0;
			this.m_fStrokeOpacity = 1.0;
			this.m_fillOverprint = false;
			this.m_strokeOverprint = false;
			this.m_overprintMode = 0;
			this.m_pTransfer[0] = (this.m_pTransfer[1] = (this.m_pTransfer[2] = (this.m_pTransfer[3] = null)));
			this.m_fLineWidth = 1.0;
			this.m_pLineDash = null;
			this.m_nLineDashLength = 0;
			this.m_fLineDashStart = 0.0;
			this.m_nFlatness = 1;
			this.m_nLineJoin = 0;
			this.m_nLineCap = 0;
			this.m_fMiterLimit = 10.0;
			this.m_strokeAdjust = false;
			this.m_pFont = null;
			this.m_fFontSize = 0.0;
			this.m_textMat[0] = 1.0;
			this.m_textMat[1] = 0.0;
			this.m_textMat[2] = 0.0;
			this.m_textMat[3] = 1.0;
			this.m_textMat[4] = 0.0;
			this.m_textMat[5] = 0.0;
			this.m_fCharSpace = 0.0;
			this.m_fWordSpace = 0.0;
			this.m_fHorizScaling = 1.0;
			this.m_fLeading = 0.0;
			this.m_fRise = 0.0;
			this.m_nRender = 0;
			this.m_pPath = new PdfPath();
			this.m_fCurX = (this.m_fCurY = 0.0);
			this.m_fLineX = (this.m_fLineY = 0.0);
			this.m_fClipXMin = 0.0;
			this.m_fClipYMin = 0.0;
			this.m_fClipXMax = this.m_fPageWidth;
			this.m_fClipYMax = this.m_fPageHeight;
			this.m_pSaved = null;
			this.m_pPreview = pPreview;
		}
		internal PdfState(PdfState state, bool copyPath)
		{
			Array.Copy(state.m_ctm, this.m_ctm, state.m_ctm.Length);
			Array.Copy(state.m_textMat, this.m_textMat, state.m_textMat.Length);
			this.m_fFillOpacity = state.m_fFillOpacity;
			this.m_fStrokeOpacity = state.m_fStrokeOpacity;
			this.m_fClipXMin = state.m_fClipXMin;
			this.m_fClipYMin = state.m_fClipYMin;
			this.m_fClipXMax = state.m_fClipXMax;
			this.m_fClipYMax = state.m_fClipYMax;
			this.m_fFontSize = state.m_fFontSize;
			this.m_nRender = state.m_nRender;
			this.m_blendMode = state.m_blendMode;
			this.m_fillOverprint = state.m_fillOverprint;
			this.m_strokeOverprint = state.m_strokeOverprint;
			this.m_overprintMode = state.m_overprintMode;
			this.m_fLineWidth = state.m_fLineWidth;
			this.m_nLineDashLength = state.m_nLineDashLength;
			this.m_fLineDashStart = state.m_fLineDashStart;
			this.m_nFlatness = state.m_nFlatness;
			this.m_nLineJoin = state.m_nLineJoin;
			this.m_nLineCap = state.m_nLineCap;
			this.m_fMiterLimit = state.m_fMiterLimit;
			this.m_strokeAdjust = state.m_strokeAdjust;
			this.m_fCharSpace = state.m_fCharSpace;
			this.m_fWordSpace = state.m_fWordSpace;
			this.m_fHorizScaling = state.m_fHorizScaling;
			this.m_fLeading = state.m_fLeading;
			this.m_fRise = state.m_fRise;
			this.m_fCurX = state.m_fCurX;
			this.m_fCurY = state.m_fCurY;
			this.m_fLineX = state.m_fLineX;
			this.m_fLineY = state.m_fLineY;
			this.m_hDPI = state.m_hDPI;
			this.m_vDPI = state.m_vDPI;
			this.px1 = state.px1;
			this.py1 = state.py1;
			this.px2 = state.px2;
			this.py2 = state.py2;
			this.m_fPageWidth = state.m_fPageWidth;
			this.m_fPageHeight = state.m_fPageHeight;
			this.m_fillColor = new PdfColor();
			Array.Copy(state.m_fillColor.c, this.m_fillColor.c, state.m_fillColor.c.Length);
			this.m_strokeColor = new PdfColor();
			Array.Copy(state.m_strokeColor.c, this.m_strokeColor.c, state.m_strokeColor.c.Length);
			if (state.m_fillColorSpace != null)
			{
				this.m_fillColorSpace = state.m_fillColorSpace.copy();
			}
			if (state.m_strokeColorSpace != null)
			{
				this.m_strokeColorSpace = state.m_strokeColorSpace.copy();
			}
			if (state.m_fillPattern != null)
			{
				this.m_fillPattern = state.m_fillPattern.copy();
			}
			if (state.m_strokePattern != null)
			{
				this.m_strokePattern = state.m_strokePattern.copy();
			}
			if (this.m_nLineDashLength > 0)
			{
				this.m_pLineDash = new double[this.m_nLineDashLength];
				Array.Copy(state.m_pLineDash, this.m_pLineDash, this.m_nLineDashLength);
			}
			for (int i = 0; i < 4; i++)
			{
				if (state.m_pTransfer[i] != null)
				{
					this.m_pTransfer[i] = state.m_pTransfer[i].copy();
				}
			}
			if (copyPath)
			{
				this.m_pPath = state.m_pPath.copy();
			}
			else
			{
				this.m_pPath = state.m_pPath;
			}
			this.m_pSaved = null;
			this.m_pPreview = state.m_pPreview;
		}
		internal PdfState copy()
		{
			return new PdfState(this, false);
		}
		internal PdfState copy(bool copyPath)
		{
			return new PdfState(this, copyPath);
		}
		internal double[] getCTM()
		{
			return this.m_ctm;
		}
		internal double GetPageWidth()
		{
			return this.m_fPageWidth;
		}
		internal double GetPageHeight()
		{
			return this.m_fPageHeight;
		}
		internal void moveTo(double x, double y)
		{
			PdfPath arg_1A_0 = this.m_pPath;
			this.m_fCurX = x;
			this.m_fCurY = y;
			arg_1A_0.moveTo(x, y);
		}
		internal void lineTo(double x, double y)
		{
			PdfPath arg_1A_0 = this.m_pPath;
			this.m_fCurX = x;
			this.m_fCurY = y;
			arg_1A_0.lineTo(x, y);
		}
		internal void closePath()
		{
			this.m_pPath.close();
			this.m_fCurX = this.m_pPath.getLastX();
			this.m_fCurY = this.m_pPath.getLastY();
		}
		internal void clip()
		{
			double num = 0.0;
			double num2 = 0.0;
			double num6;
			double num5;
			double num4;
			double num3 = num4 = (num5 = (num6 = 0.0));
			for (int i = 0; i < this.m_pPath.getNumSubpaths(); i++)
			{
				PdfSubpath subpath = this.m_pPath.getSubpath(i);
				for (int j = 0; j < subpath.getNumPoints(); j++)
				{
					this.transform(subpath.getX(j), subpath.getY(j), ref num, ref num2);
					if (i == 0 && j == 0)
					{
						num3 = (num4 = num);
						num6 = (num5 = num2);
					}
					else
					{
						if (num < num4)
						{
							num4 = num;
						}
						else
						{
							if (num > num3)
							{
								num3 = num;
							}
						}
						if (num2 < num5)
						{
							num5 = num2;
						}
						else
						{
							if (num2 > num6)
							{
								num6 = num2;
							}
						}
					}
				}
			}
			if (num4 > this.m_fClipXMin)
			{
				this.m_fClipXMin = num4;
			}
			if (num5 > this.m_fClipYMin)
			{
				this.m_fClipYMin = num5;
			}
			if (num3 < this.m_fClipXMax)
			{
				this.m_fClipXMax = num3;
			}
			if (num6 < this.m_fClipYMax)
			{
				this.m_fClipYMax = num6;
			}
		}
		internal void clearPath()
		{
			this.m_pPath = new PdfPath();
		}
		internal bool isCurPt()
		{
			return this.m_pPath.isCurPt();
		}
		internal bool isPath()
		{
			return this.m_pPath.isPath();
		}
		internal PdfPath getPath()
		{
			return this.m_pPath;
		}
		internal void setStrokeColorSpace(PdfColorSpaceTop colorSpace)
		{
			this.m_strokeColorSpace = colorSpace;
		}
		internal void setStrokePattern(PdfPattern pattern)
		{
			this.m_strokePattern = pattern;
		}
		internal void setFillColor(PdfColor color)
		{
			this.m_fillColor = color;
		}
		internal void setStrokeColor(PdfColor color)
		{
			this.m_strokeColor = color;
		}
		internal PdfColorSpaceTop getFillColorSpace()
		{
			return this.m_fillColorSpace;
		}
		internal PdfColorSpaceTop getStrokeColorSpace()
		{
			return this.m_strokeColorSpace;
		}
		internal bool getFillOverprint()
		{
			return this.m_fillOverprint;
		}
		internal bool getStrokeOverprint()
		{
			return this.m_strokeOverprint;
		}
		internal int getOverprintMode()
		{
			return this.m_overprintMode;
		}
		internal PdfColor getFillColor()
		{
			return this.m_fillColor;
		}
		internal void getFillGray(PdfGray gray)
		{
			this.m_fillColorSpace.getGray(this.m_fillColor, gray);
		}
		internal void getStrokeGray(PdfGray gray)
		{
			this.m_strokeColorSpace.getGray(this.m_strokeColor, gray);
		}
		internal void getFillRGB(PdfRGB rgb)
		{
			this.m_fillColorSpace.getRGB(this.m_fillColor, rgb);
		}
		internal void getStrokeRGB(PdfRGB rgb)
		{
			this.m_strokeColorSpace.getRGB(this.m_strokeColor, rgb);
		}
		internal void setFillPattern(PdfPattern pattern)
		{
			this.m_fillPattern = pattern;
		}
		internal void shiftCTM(double tx, double ty)
		{
			this.m_ctm[4] += tx;
			this.m_ctm[5] += ty;
			this.m_fClipXMin += tx;
			this.m_fClipYMin += ty;
			this.m_fClipXMax += tx;
			this.m_fClipYMax += ty;
		}
		internal void setFillColorSpace(PdfColorSpaceTop colorSpace)
		{
			this.m_fillColorSpace = colorSpace;
		}
		internal void setLineWidth(double width)
		{
			this.m_fLineWidth = width;
		}
		internal double getLineWidth()
		{
			return this.m_fLineWidth;
		}
		internal void setLineCap(int lineCap1)
		{
			this.m_nLineCap = lineCap1;
		}
		internal int getLineCap()
		{
			return this.m_nLineCap;
		}
		internal void setMiterLimit(double limit)
		{
			this.m_fMiterLimit = limit;
		}
		internal double getMiterLimit()
		{
			return this.m_fMiterLimit;
		}
		internal void curveTo(double x1, double y1, double x2, double y2, double x3, double y3)
		{
			PdfPath arg_21_0 = this.m_pPath;
			this.m_fCurX = x3;
			this.m_fCurY = y3;
			arg_21_0.curveTo(x1, y1, x2, y2, x3, y3);
		}
		internal void concatCTM(double a, double b, double c, double d, double e, double f)
		{
			double num = this.m_ctm[0];
			double num2 = this.m_ctm[1];
			double num3 = this.m_ctm[2];
			double num4 = this.m_ctm[3];
			this.m_ctm[0] = a * num + b * num3;
			this.m_ctm[1] = a * num2 + b * num4;
			this.m_ctm[2] = c * num + d * num3;
			this.m_ctm[3] = c * num2 + d * num4;
			this.m_ctm[4] = e * num + f * num3 + this.m_ctm[4];
			this.m_ctm[5] = e * num2 + f * num4 + this.m_ctm[5];
			for (int i = 0; i < 6; i++)
			{
				if (this.m_ctm[i] > 10000000000.0)
				{
					this.m_ctm[i] = 10000000000.0;
				}
				else
				{
					if (this.m_ctm[i] < -10000000000.0)
					{
						this.m_ctm[i] = -10000000000.0;
					}
				}
			}
		}
		internal void setLineDash(double[] dash, int length, double start)
		{
			this.m_pLineDash = dash;
			this.m_nLineDashLength = length;
			this.m_fLineDashStart = start;
		}
		internal void getLineDash(ref double[] dash, ref int length, ref double start)
		{
			dash = this.m_pLineDash;
			length = this.m_nLineDashLength;
			start = this.m_fLineDashStart;
		}
		internal double transformWidth(double w)
		{
			double num = this.m_ctm[0] + this.m_ctm[2];
			double num2 = this.m_ctm[1] + this.m_ctm[3];
			return w * Math.Sqrt(0.5 * (num * num + num2 * num2));
		}
		internal void setBlendMode(enumBlendMode mode)
		{
			this.m_blendMode = mode;
		}
		internal enumBlendMode getBlendMode()
		{
			return this.m_blendMode;
		}
		internal bool parseBlendMode(PdfObject obj, ref enumBlendMode mode)
		{
			if (obj != null && obj.m_nType == enumType.pdfName)
			{
				for (int i = 0; i < PdfState.BlendModeNames.Length; i++)
				{
					if (string.Compare(((PdfName)obj).m_bstrName, PdfState.BlendModeNames[i].name) == 0)
					{
						mode = PdfState.BlendModeNames[i].mode;
						return true;
					}
				}
				return false;
			}
			if (obj != null && obj.m_nType == enumType.pdfArray)
			{
				for (int i = 0; i < ((PdfArray)obj).Size; i++)
				{
					PdfObject @object = ((PdfArray)obj).GetObject(i);
					if (@object == null || @object.m_nType != enumType.pdfName)
					{
						return false;
					}
					for (int j = 0; j < PdfState.BlendModeNames.Length; j++)
					{
						if (string.Compare(((PdfName)@object).m_bstrName, PdfState.BlendModeNames[j].name) == 0)
						{
							mode = PdfState.BlendModeNames[j].mode;
							return true;
						}
					}
				}
				mode = enumBlendMode.bmBlendNormal;
				return true;
			}
			return false;
		}
		internal void setFillOpacity(double opac)
		{
			this.m_fFillOpacity = opac;
		}
		internal double getFillOpacity()
		{
			return this.m_fFillOpacity;
		}
		internal void setStrokeOpacity(double opac)
		{
			this.m_fStrokeOpacity = opac;
		}
		internal double getStrokeOpacity()
		{
			return this.m_fStrokeOpacity;
		}
		internal void setTransfer(PdfFunctionTop[] funcs)
		{
			for (int i = 0; i < 4; i++)
			{
				this.m_pTransfer[i] = funcs[i];
			}
		}
		internal PdfFunctionTop[] getTransfer()
		{
			return this.m_pTransfer;
		}
		internal void setFlatness(int flatness1)
		{
			this.m_nFlatness = flatness1;
		}
		internal int getFlatness()
		{
			return this.m_nFlatness;
		}
		internal void setLineJoin(int lineJoin)
		{
			this.m_nLineJoin = lineJoin;
		}
		internal int getLineJoin()
		{
			return this.m_nLineJoin;
		}
		internal PdfState save()
		{
			PdfState pdfState = this.copy();
			pdfState.m_pSaved = this;
			return pdfState;
		}
		internal PdfState restore()
		{
			PdfState pdfState;
			if (this.m_pSaved != null)
			{
				pdfState = this.m_pSaved;
				pdfState.m_pPath = this.m_pPath;
				pdfState.m_fCurX = this.m_fCurX;
				pdfState.m_fCurY = this.m_fCurY;
				pdfState.m_fLineX = this.m_fLineX;
				pdfState.m_fLineY = this.m_fLineY;
				this.m_pPath = null;
				this.m_pSaved = null;
			}
			else
			{
				pdfState = this;
			}
			return pdfState;
		}
		internal void transform(double x1, double y1, ref double x2, ref double y2)
		{
			x2 = this.m_ctm[0] * x1 + this.m_ctm[2] * y1 + this.m_ctm[4];
			y2 = this.m_ctm[1] * x1 + this.m_ctm[3] * y1 + this.m_ctm[5];
		}
		internal void getUserClipBBox(ref double xMin, ref double yMin, ref double xMax, ref double yMax)
		{
			double[] array = new double[6];
			double num = 1.0 / (this.m_ctm[0] * this.m_ctm[3] - this.m_ctm[1] * this.m_ctm[2]);
			array[0] = this.m_ctm[3] * num;
			array[1] = -this.m_ctm[1] * num;
			array[2] = -this.m_ctm[2] * num;
			array[3] = this.m_ctm[0] * num;
			array[4] = (this.m_ctm[2] * this.m_ctm[5] - this.m_ctm[3] * this.m_ctm[4]) * num;
			array[5] = (this.m_ctm[1] * this.m_ctm[4] - this.m_ctm[0] * this.m_ctm[5]) * num;
			double num3;
			double num2 = num3 = this.m_fClipXMin * array[0] + this.m_fClipYMin * array[2] + array[4];
			double num5;
			double num4 = num5 = this.m_fClipXMin * array[1] + this.m_fClipYMin * array[3] + array[5];
			double num6 = this.m_fClipXMin * array[0] + this.m_fClipYMax * array[2] + array[4];
			double num7 = this.m_fClipXMin * array[1] + this.m_fClipYMax * array[3] + array[5];
			if (num6 < num3)
			{
				num3 = num6;
			}
			else
			{
				if (num6 > num2)
				{
					num2 = num6;
				}
			}
			if (num7 < num5)
			{
				num5 = num7;
			}
			else
			{
				if (num7 > num4)
				{
					num4 = num7;
				}
			}
			num6 = this.m_fClipXMax * array[0] + this.m_fClipYMin * array[2] + array[4];
			num7 = this.m_fClipXMax * array[1] + this.m_fClipYMin * array[3] + array[5];
			if (num6 < num3)
			{
				num3 = num6;
			}
			else
			{
				if (num6 > num2)
				{
					num2 = num6;
				}
			}
			if (num7 < num5)
			{
				num5 = num7;
			}
			else
			{
				if (num7 > num4)
				{
					num4 = num7;
				}
			}
			num6 = this.m_fClipXMax * array[0] + this.m_fClipYMax * array[2] + array[4];
			num7 = this.m_fClipXMax * array[1] + this.m_fClipYMax * array[3] + array[5];
			if (num6 < num3)
			{
				num3 = num6;
			}
			else
			{
				if (num6 > num2)
				{
					num2 = num6;
				}
			}
			if (num7 < num5)
			{
				num5 = num7;
			}
			else
			{
				if (num7 > num4)
				{
					num4 = num7;
				}
			}
			xMin = num3;
			yMin = num5;
			xMax = num2;
			yMax = num4;
		}
		internal bool hasSaves()
		{
			return this.m_pSaved != null;
		}
		internal PdfPattern getFillPattern()
		{
			return this.m_fillPattern;
		}
		internal void getClipBBox(ref double xMin, ref double yMin, ref double xMax, ref double yMax)
		{
			xMin = this.m_fClipXMin;
			yMin = this.m_fClipYMin;
			xMax = this.m_fClipXMax;
			yMax = this.m_fClipYMax;
		}
		internal void transformDelta(double x1, double y1, ref double x2, ref double y2)
		{
			x2 = this.m_ctm[0] * x1 + this.m_ctm[2] * y1;
			y2 = this.m_ctm[1] * x1 + this.m_ctm[3] * y1;
		}
		internal void clipToStrokePath()
		{
			double num = 0.0;
			double num2 = 0.0;
			double num6;
			double num5;
			double num4;
			double num3 = num4 = (num5 = (num6 = 0.0));
			for (int i = 0; i < this.m_pPath.getNumSubpaths(); i++)
			{
				PdfSubpath subpath = this.m_pPath.getSubpath(i);
				for (int j = 0; j < subpath.getNumPoints(); j++)
				{
					this.transform(subpath.getX(j), subpath.getY(j), ref num, ref num2);
					if (i == 0 && j == 0)
					{
						num3 = (num4 = num);
						num6 = (num5 = num2);
					}
					else
					{
						if (num < num4)
						{
							num4 = num;
						}
						else
						{
							if (num > num3)
							{
								num3 = num;
							}
						}
						if (num2 < num5)
						{
							num5 = num2;
						}
						else
						{
							if (num2 > num6)
							{
								num6 = num2;
							}
						}
					}
				}
			}
			double num7 = Math.Abs(this.m_ctm[0]);
			double num8 = Math.Abs(this.m_ctm[2]);
			if (num7 > num8)
			{
				num4 -= 0.5 * this.m_fLineWidth * num7;
				num3 += 0.5 * this.m_fLineWidth * num7;
			}
			else
			{
				num4 -= 0.5 * this.m_fLineWidth * num8;
				num3 += 0.5 * this.m_fLineWidth * num8;
			}
			num7 = Math.Abs(this.m_ctm[0]);
			num8 = Math.Abs(this.m_ctm[3]);
			if (num7 > num8)
			{
				num5 -= 0.5 * this.m_fLineWidth * num7;
				num6 += 0.5 * this.m_fLineWidth * num7;
			}
			else
			{
				num5 -= 0.5 * this.m_fLineWidth * num8;
				num6 += 0.5 * this.m_fLineWidth * num8;
			}
			if (num4 > this.m_fClipXMin)
			{
				this.m_fClipXMin = num4;
			}
			if (num5 > this.m_fClipYMin)
			{
				this.m_fClipYMin = num5;
			}
			if (num3 < this.m_fClipXMax)
			{
				this.m_fClipXMax = num3;
			}
			if (num6 < this.m_fClipYMax)
			{
				this.m_fClipYMax = num6;
			}
		}
		internal double getCurX()
		{
			return this.m_fCurX;
		}
		internal double getCurY()
		{
			return this.m_fCurY;
		}
		internal static int clip01(int x)
		{
			if (x < 0)
			{
				return 0;
			}
			if (x <= 65536)
			{
				return x;
			}
			return 65536;
		}
		internal static double clip01(double x)
		{
			if (x < 0.0)
			{
				return 0.0;
			}
			if (x <= 1.0)
			{
				return x;
			}
			return 1.0;
		}
		internal static int dblToCol(double x)
		{
			return (int)(x * 65536.0);
		}
		internal static byte colToByte(int x)
		{
			return (byte)((x << 8) - x + 32768 >> 16);
		}
		internal static double colToDbl(int x)
		{
			return (double)x / 65536.0;
		}
	}
}
