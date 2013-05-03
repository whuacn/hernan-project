using System;
namespace Persits.PDF
{
	internal class PdfRavenOutputDev
	{
		internal class RavenTransparencyGroup
		{
			internal int tx;
			internal int ty;
			internal RavenBitmap tBitmap;
			internal PdfColorSpaceTop blendingColorSpace;
			internal bool isolated;
			internal RavenBitmap origBitmap;
			internal Raven origRaven;
			internal PdfRavenOutputDev.RavenTransparencyGroup next;
		}
		internal class RavenOutImageMaskData
		{
			internal PdfImageStream imgStr;
			internal int invert;
			internal int width;
			internal int height;
			internal int y;
		}
		internal class RavenOutMaskedImageData
		{
			internal PdfImageStream imgStr;
			internal PdfImageColorMap colorMap;
			internal RavenBitmap mask;
			internal RavenColorPtr lookup;
			internal RavenColorMode colorMode;
			internal int width;
			internal int height;
			internal int y;
		}
		internal class RavenOutImageData
		{
			internal PdfImageStream imgStr;
			internal PdfImageColorMap colorMap;
			internal RavenColorPtr lookup;
			internal int[] maskColors;
			internal RavenColorMode colorMode;
			internal int width;
			internal int height;
			internal int y;
		}
		internal class T3GlyphStack
		{
			private ushort code;
			private double x;
			private double y;
			private byte[] cacheData;
			private RavenBitmap origBitmap;
			private Raven origRaven;
			private double origCTM4;
			private double origCTM5;
			private PdfRavenOutputDev.T3GlyphStack next;
		}
		private static RavenBlendFunc[] ravenOutBlendFuncs = new RavenBlendFunc[]
		{
			null,
			new RavenBlendFunc(PdfRavenOutputDev.ravenOutBlendMultiply),
			new RavenBlendFunc(PdfRavenOutputDev.ravenOutBlendScreen),
			new RavenBlendFunc(PdfRavenOutputDev.ravenOutBlendOverlay),
			new RavenBlendFunc(PdfRavenOutputDev.ravenOutBlendDarken),
			new RavenBlendFunc(PdfRavenOutputDev.ravenOutBlendLighten),
			new RavenBlendFunc(PdfRavenOutputDev.ravenOutBlendColorDodge),
			new RavenBlendFunc(PdfRavenOutputDev.ravenOutBlendColorBurn),
			new RavenBlendFunc(PdfRavenOutputDev.ravenOutBlendHardLight),
			new RavenBlendFunc(PdfRavenOutputDev.ravenOutBlendSoftLight),
			new RavenBlendFunc(PdfRavenOutputDev.ravenOutBlendDifference),
			new RavenBlendFunc(PdfRavenOutputDev.ravenOutBlendExclusion),
			new RavenBlendFunc(PdfRavenOutputDev.ravenOutBlendHue),
			new RavenBlendFunc(PdfRavenOutputDev.ravenOutBlendSaturation),
			new RavenBlendFunc(PdfRavenOutputDev.ravenOutBlendColor),
			new RavenBlendFunc(PdfRavenOutputDev.ravenOutBlendLuminosity)
		};
		internal PdfDocument m_pDoc;
		internal RavenFontEngine m_pFontEngine;
		internal RavenBitmap m_pBitmap;
		internal Raven m_pRaven;
		internal int m_bitmapRowPad;
		internal RavenColorMode m_colorMode;
		internal bool m_bitmapTopDown;
		internal bool m_vectorAntialias;
		internal RavenScreenParams m_screenParams;
		internal RavenColor m_paperColor;
		internal bool m_bitmapUpsideDown;
		internal bool m_allowAntialias;
		internal bool m_reverseVideo;
		internal bool m_skipHorizText;
		internal bool m_skipRotatedText;
		internal bool m_bNeedFontUpdate;
		internal PdfRavenOutputDev.T3GlyphStack t3GlyphStack;
		internal PdfRavenOutputDev.RavenTransparencyGroup m_pTranspGroupStack;
		internal int m_nestCount;
		private static void ravenOutBlendMultiply(RavenColorPtr src, RavenColorPtr dest, RavenColorPtr blend, RavenColorMode cm)
		{
			for (int i = 0; i < RavenState.ravenColorModeNComps[(int)cm]; i++)
			{
                blend[i] = (byte)((int)dest[i] * (int)src[i] / (int)byte.MaxValue);
			}
		}
		private static void ravenOutBlendScreen(RavenColorPtr src, RavenColorPtr dest, RavenColorPtr blend, RavenColorMode cm)
		{
			for (int i = 0; i < RavenState.ravenColorModeNComps[(int)cm]; i++)
			{
                blend[i] = (byte)((int)dest[i] + (int)src[i] - (int)dest[i] * (int)src[i] / (int)byte.MaxValue);
			}
		}
		private static void ravenOutBlendOverlay(RavenColorPtr src, RavenColorPtr dest, RavenColorPtr blend, RavenColorMode cm)
		{
			for (int i = 0; i < RavenState.ravenColorModeNComps[(int)cm]; i++)
			{
                blend[i] = (int)dest[i] < 128 ? (byte)((int)src[i] * 2 * (int)dest[i] / (int)byte.MaxValue) : (byte)((int)byte.MaxValue - 2 * (((int)byte.MaxValue - (int)src[i]) * ((int)byte.MaxValue - (int)dest[i])) / (int)byte.MaxValue);
			}
		}
		private static void ravenOutBlendDarken(RavenColorPtr src, RavenColorPtr dest, RavenColorPtr blend, RavenColorMode cm)
		{
			for (int i = 0; i < RavenState.ravenColorModeNComps[(int)cm]; i++)
			{
				blend[i] = ((dest[i] < src[i]) ? dest[i] : src[i]);
			}
		}
		private static void ravenOutBlendLighten(RavenColorPtr src, RavenColorPtr dest, RavenColorPtr blend, RavenColorMode cm)
		{
			for (int i = 0; i < RavenState.ravenColorModeNComps[(int)cm]; i++)
			{
				blend[i] = ((dest[i] > src[i]) ? dest[i] : src[i]);
			}
		}
		private static void ravenOutBlendColorDodge(RavenColorPtr src, RavenColorPtr dest, RavenColorPtr blend, RavenColorMode cm)
		{
			for (int i = 0; i < RavenState.ravenColorModeNComps[(int)cm]; i++)
			{
				if (src[i] == 255)
				{
					blend[i] = 255;
				}
				else
				{
					int num = (int)(dest[i] * 255 / (255 - src[i]));
					blend[i] = num <= (int)byte.MaxValue ? (byte)num : byte.MaxValue;
				}
			}
		}
		private static void ravenOutBlendColorBurn(RavenColorPtr src, RavenColorPtr dest, RavenColorPtr blend, RavenColorMode cm)
		{
			for (int i = 0; i < RavenState.ravenColorModeNComps[(int)cm]; i++)
			{
				if (src[i] == 0)
				{
					blend[i] = 0;
				}
				else
				{
					int num = (int)((255 - dest[i]) * 255 / src[i]);
                    blend[i] = num <= (int)byte.MaxValue ? (byte)((int)byte.MaxValue - num) : (byte)0;
				}
			}
		}
		private static void ravenOutBlendHardLight(RavenColorPtr src, RavenColorPtr dest, RavenColorPtr blend, RavenColorMode cm)
		{
			for (int i = 0; i < RavenState.ravenColorModeNComps[(int)cm]; i++)
			{
                blend[i] = (int)src[i] < 128 ? (byte)((int)dest[i] * 2 * (int)src[i] / (int)byte.MaxValue) : (byte)((int)byte.MaxValue - 2 * (((int)byte.MaxValue - (int)dest[i]) * ((int)byte.MaxValue - (int)src[i])) / (int)byte.MaxValue);
			}
		}
		private static void ravenOutBlendSoftLight(RavenColorPtr src, RavenColorPtr dest, RavenColorPtr blend, RavenColorMode cm)
		{
			for (int i = 0; i < RavenState.ravenColorModeNComps[(int)cm]; i++)
			{
				if (src[i] < 128)
				{
					blend[i] = (byte)((int)dest[i] - (int)((255 - 2 * src[i]) * dest[i] * (255 - dest[i])) / 65025);
				}
				else
				{
					int num;
					if (dest[i] < 64)
					{
						num = (((int)(16 * dest[i]) - 3060) * (int)dest[i] / 255 + 1020) * (int)dest[i] / 255;
					}
					else
					{
						num = (int)Math.Sqrt(255.0 * (double)dest[i]);
					}
					blend[i] = (byte)((int)dest[i] + (int)(2 * src[i] - 255) * (num - (int)dest[i]) / 255);
				}
			}
		}
		private static void ravenOutBlendDifference(RavenColorPtr src, RavenColorPtr dest, RavenColorPtr blend, RavenColorMode cm)
		{
			for (int i = 0; i < RavenState.ravenColorModeNComps[(int)cm]; i++)
			{
                blend[i] = (int)dest[i] < (int)src[i] ? (byte)((int)src[i] - (int)dest[i]) : (byte)((int)dest[i] - (int)src[i]);
			}
		}
		private static void ravenOutBlendExclusion(RavenColorPtr src, RavenColorPtr dest, RavenColorPtr blend, RavenColorMode cm)
		{
			for (int i = 0; i < RavenState.ravenColorModeNComps[(int)cm]; i++)
			{
                blend[i] = (byte)((int)dest[i] + (int)src[i] - 2 * (int)dest[i] * (int)src[i] / (int)byte.MaxValue);
			}
		}
		private static int getLum(int r, int g, int b)
		{
			return (int)(0.3 * (double)r + 0.59 * (double)g + 0.11 * (double)b);
		}
		private static void setLum(byte rIn, byte gIn, byte bIn, int lum, ref byte rOut, ref byte gOut, ref byte bOut)
		{
			int num = lum - PdfRavenOutputDev.getLum((int)rIn, (int)gIn, (int)bIn);
			PdfRavenOutputDev.clipColor((int)rIn + num, (int)gIn + num, (int)bIn + num, ref rOut, ref gOut, ref bOut);
		}
		private static void clipColor(int rIn, int gIn, int bIn, ref byte rOut, ref byte gOut, ref byte bOut)
		{
			int lum = PdfRavenOutputDev.getLum(rIn, gIn, bIn);
			int num = rIn;
			int num2 = rIn;
			if (gIn < num2)
			{
				num2 = gIn;
			}
			else
			{
				if (gIn > num)
				{
					num = gIn;
				}
			}
			if (bIn < num2)
			{
				num2 = bIn;
			}
			else
			{
				if (bIn > num)
				{
					num = bIn;
				}
			}
			if (num2 < 0)
			{
				rOut = (byte)(lum + (rIn - lum) * lum / (lum - num2));
				gOut = (byte)(lum + (gIn - lum) * lum / (lum - num2));
				bOut = (byte)(lum + (bIn - lum) * lum / (lum - num2));
				return;
			}
			if (num > 255)
			{
				rOut = (byte)(lum + (rIn - lum) * (255 - lum) / (num - lum));
				gOut = (byte)(lum + (gIn - lum) * (255 - lum) / (num - lum));
				bOut = (byte)(lum + (bIn - lum) * (255 - lum) / (num - lum));
				return;
			}
			rOut = (byte)rIn;
			gOut = (byte)gIn;
			bOut = (byte)bIn;
		}
		private static void setSat(byte rIn, byte gIn, byte bIn, int sat, ref byte rOut, ref byte gOut, ref byte bOut)
		{
			byte[] array = new byte[3];
			byte[] array2 = new byte[]
			{
				0,
				1,
				2
			};
			int num;
			int num2;
			if (rIn < gIn)
			{
				num = (int)rIn;
				array2[0] = 0;
				num2 = (int)gIn;
				array2[1] = 1;
			}
			else
			{
				num = (int)gIn;
				array2[0] = 1;
				num2 = (int)rIn;
				array2[1] = 0;
			}
			int num3;
			if ((int)bIn > num2)
			{
				num3 = (int)bIn;
				array2[2] = 2;
			}
			else
			{
				if ((int)bIn > num)
				{
					num3 = num2;
					array2[2] = array2[1];
					num2 = (int)bIn;
					array2[1] = 2;
				}
				else
				{
					num3 = num2;
					array2[2] = array2[1];
					num2 = num;
					array2[1] = array2[0];
					num = (int)bIn;
					array2[0] = 2;
				}
			}
			if (num3 > num)
			{
				array[(int)array2[1]] = (byte)((num2 - num) * sat / (num3 - num));
				array[(int)array2[2]] = (byte)sat;
			}
			else
			{
				array[(int)array2[1]] = (array[(int)array2[2]] = 0);
			}
			array[(int)array2[0]] = 0;
			rOut = array[0];
			gOut = array[1];
			bOut = array[2];
		}
		private static int getSat(int r, int g, int b)
		{
			int num = r;
			int num2 = r;
			if (g < num2)
			{
				num2 = g;
			}
			else
			{
				if (g > num)
				{
					num = g;
				}
			}
			if (b < num2)
			{
				num2 = b;
			}
			else
			{
				if (b > num)
				{
					num = b;
				}
			}
			return num - num2;
		}
		private static void ravenOutBlendHue(RavenColorPtr src, RavenColorPtr dest, RavenColorPtr blend, RavenColorMode cm)
		{
			byte rIn = 0;
			byte gIn = 0;
			byte bIn = 0;
			byte value = 0;
			byte value2 = 0;
			byte value3 = 0;
			switch (cm)
			{
			case RavenColorMode.ravenModeMono1:
			case RavenColorMode.ravenModeMono8:
				blend[0] = dest[0];
				return;
			case RavenColorMode.ravenModeRGB8:
			case RavenColorMode.ravenModeBGR8:
				PdfRavenOutputDev.setSat(src[0], src[1], src[2], PdfRavenOutputDev.getSat((int)dest[0], (int)dest[1], (int)dest[2]), ref rIn, ref gIn, ref bIn);
				PdfRavenOutputDev.setLum(rIn, gIn, bIn, PdfRavenOutputDev.getLum((int)dest[0], (int)dest[1], (int)dest[2]), ref value, ref value2, ref value3);
				blend[0] = value;
				blend[1] = value2;
				blend[2] = value3;
				return;
			default:
				return;
			}
		}
		private static void ravenOutBlendSaturation(RavenColorPtr src, RavenColorPtr dest, RavenColorPtr blend, RavenColorMode cm)
		{
			byte rIn = 0;
			byte gIn = 0;
			byte bIn = 0;
			byte value = 0;
			byte value2 = 0;
			byte value3 = 0;
			switch (cm)
			{
			case RavenColorMode.ravenModeMono1:
			case RavenColorMode.ravenModeMono8:
				blend[0] = dest[0];
				return;
			case RavenColorMode.ravenModeRGB8:
			case RavenColorMode.ravenModeBGR8:
				PdfRavenOutputDev.setSat(dest[0], dest[1], dest[2], PdfRavenOutputDev.getSat((int)src[0], (int)src[1], (int)src[2]), ref rIn, ref gIn, ref bIn);
				PdfRavenOutputDev.setLum(rIn, gIn, bIn, PdfRavenOutputDev.getLum((int)dest[0], (int)dest[1], (int)dest[2]), ref value, ref value2, ref value3);
				blend[0] = value;
				blend[1] = value2;
				blend[2] = value3;
				return;
			default:
				return;
			}
		}
		private static void ravenOutBlendColor(RavenColorPtr src, RavenColorPtr dest, RavenColorPtr blend, RavenColorMode cm)
		{
			byte value = 0;
			byte value2 = 0;
			byte value3 = 0;
			switch (cm)
			{
			case RavenColorMode.ravenModeMono1:
			case RavenColorMode.ravenModeMono8:
				blend[0] = dest[0];
				return;
			case RavenColorMode.ravenModeRGB8:
			case RavenColorMode.ravenModeBGR8:
				PdfRavenOutputDev.setLum(src[0], src[1], src[2], PdfRavenOutputDev.getLum((int)dest[0], (int)dest[1], (int)dest[2]), ref value, ref value2, ref value3);
				blend[0] = value;
				blend[1] = value2;
				blend[2] = value3;
				return;
			default:
				return;
			}
		}
		private static void ravenOutBlendLuminosity(RavenColorPtr src, RavenColorPtr dest, RavenColorPtr blend, RavenColorMode cm)
		{
			byte value = 0;
			byte value2 = 0;
			byte value3 = 0;
			switch (cm)
			{
			case RavenColorMode.ravenModeMono1:
			case RavenColorMode.ravenModeMono8:
				blend[0] = dest[0];
				return;
			case RavenColorMode.ravenModeRGB8:
			case RavenColorMode.ravenModeBGR8:
				PdfRavenOutputDev.setLum(dest[0], dest[1], dest[2], PdfRavenOutputDev.getLum((int)src[0], (int)src[1], (int)src[2]), ref value, ref value2, ref value3);
				blend[0] = value;
				blend[1] = value2;
				blend[2] = value3;
				return;
			default:
				return;
			}
		}
		internal PdfRavenOutputDev(RavenColorMode colorModeA, int bitmapRowPadA, bool reverseVideoA, RavenColor paperColorA, bool bitmapTopDownA, bool allowAntialiasA)
		{
			this.m_colorMode = colorModeA;
			this.m_bitmapRowPad = bitmapRowPadA;
			this.m_bitmapTopDown = bitmapTopDownA;
			this.m_bitmapUpsideDown = false;
			this.m_allowAntialias = allowAntialiasA;
			this.m_vectorAntialias = (this.m_allowAntialias && this.m_colorMode != RavenColorMode.ravenModeMono1);
			this.setupScreenParams(72.0, 72.0);
			this.m_reverseVideo = reverseVideoA;
			this.m_paperColor = new RavenColor();
			RavenTypes.ravenColorCopy(this.m_paperColor, paperColorA);
			this.m_skipHorizText = false;
			this.m_skipRotatedText = false;
			this.m_pBitmap = new RavenBitmap(1, 1, this.m_bitmapRowPad, this.m_colorMode, this.m_colorMode != RavenColorMode.ravenModeMono1, this.m_bitmapTopDown);
			this.m_pRaven = new Raven(this.m_pBitmap, this.m_vectorAntialias, this.m_screenParams);
			this.m_pRaven.setMinLineWidth(0.0);
			this.m_pRaven.clear(this.m_paperColor, 0);
			this.m_nestCount = 0;
			this.t3GlyphStack = null;
			this.m_pTranspGroupStack = null;
		}
		private void setupScreenParams(double hDPI, double vDPI)
		{
			this.m_screenParams = new RavenScreenParams();
			this.m_screenParams.size = -1;
			this.m_screenParams.gamma = 1.0;
			this.m_screenParams.blackThreshold = 0.0;
			this.m_screenParams.whiteThreshold = 1.0;
			if (hDPI > 299.9 && vDPI > 299.9)
			{
				this.m_screenParams.type = RavenScreenType.ravenScreenStochasticClustered;
				if (this.m_screenParams.size < 0)
				{
					this.m_screenParams.size = 64;
					return;
				}
			}
			else
			{
				this.m_screenParams.type = RavenScreenType.ravenScreenDispersed;
				if (this.m_screenParams.size < 0)
				{
					this.m_screenParams.size = 4;
				}
			}
		}
		internal void startPage(int pageNum, PdfState state)
		{
			RavenColor ravenColor = new RavenColor();
			int num = (state != null) ? ((int)(state.GetPageWidth() + 0.5)) : 1;
			int num2 = (state != null) ? ((int)(state.GetPageHeight() + 0.5)) : 1;
			if (this.m_pBitmap == null || num != this.m_pBitmap.getWidth() || num2 != this.m_pBitmap.getHeight())
			{
				this.m_pBitmap = new RavenBitmap(num, num2, this.m_bitmapRowPad, this.m_colorMode, this.m_colorMode != RavenColorMode.ravenModeMono1, this.m_bitmapTopDown);
			}
            this.m_pRaven = new Raven(this.m_pBitmap, this.m_vectorAntialias, (RavenScreenParams)null);
			if (this.m_pBitmap.getDataPtr() == null)
			{
				return;
			}
			if (state != null)
			{
				double[] array = new double[6];
				double[] cTM = state.getCTM();
				array[0] = cTM[0];
				array[1] = cTM[1];
				array[2] = cTM[2];
				array[3] = cTM[3];
				array[4] = cTM[4];
				array[5] = cTM[5];
				this.m_pRaven.setMatrix(array);
			}
			switch (this.m_colorMode)
			{
			case RavenColorMode.ravenModeMono1:
			case RavenColorMode.ravenModeMono8:
				ravenColor[0] = 0;
				break;
			case RavenColorMode.ravenModeRGB8:
			case RavenColorMode.ravenModeBGR8:
				ravenColor[0] = (ravenColor[1] = (ravenColor[2] = 0));
				break;
			}
			this.m_pRaven.setStrokePattern(new RavenSolidColor(ravenColor));
			this.m_pRaven.setFillPattern(new RavenSolidColor(ravenColor));
			this.m_pRaven.setLineCap(0);
			this.m_pRaven.setLineJoin(0);
			this.m_pRaven.setLineDash(null, 0, 0.0);
			this.m_pRaven.setMiterLimit(10.0);
			this.m_pRaven.setFlatness(1.0);
			this.m_pRaven.setStrokeAdjust(true);
			this.m_pRaven.clear(this.m_paperColor, 255);
		}
		internal void clip(PdfState state)
		{
			RavenPath path = this.convertPath(state, state.getPath(), true);
			this.m_pRaven.clipToPath(path, false);
		}
		internal void stroke(PdfState state)
		{
			RavenPath path = this.convertPath(state, state.getPath(), false);
			this.m_pRaven.stroke(path);
		}
		internal void fill(PdfState state)
		{
			RavenPath path = this.convertPath(state, state.getPath(), true);
			this.m_pRaven.fill(path, false);
		}
		internal void eoFill(PdfState state)
		{
			RavenPath path = this.convertPath(state, state.getPath(), true);
			this.m_pRaven.fill(path, true);
		}
		internal void updateStrokeColor(PdfState state)
		{
			PdfGray gray = new PdfGray();
			PdfRGB rgb = new PdfRGB();
			switch (this.m_colorMode)
			{
			case RavenColorMode.ravenModeMono1:
			case RavenColorMode.ravenModeMono8:
				state.getStrokeGray(gray);
				this.m_pRaven.setStrokePattern(this.getColor(gray));
				return;
			case RavenColorMode.ravenModeRGB8:
			case RavenColorMode.ravenModeBGR8:
				state.getStrokeRGB(rgb);
				this.m_pRaven.setStrokePattern(this.getColor(rgb));
				return;
			default:
				return;
			}
		}
		internal void updateFillColor(PdfState state)
		{
			PdfGray gray = new PdfGray();
			PdfRGB rgb = new PdfRGB();
			switch (this.m_colorMode)
			{
			case RavenColorMode.ravenModeMono1:
			case RavenColorMode.ravenModeMono8:
				state.getFillGray(gray);
				this.m_pRaven.setFillPattern(this.getColor(gray));
				return;
			case RavenColorMode.ravenModeRGB8:
			case RavenColorMode.ravenModeBGR8:
				state.getFillRGB(rgb);
				this.m_pRaven.setFillPattern(this.getColor(rgb));
				return;
			default:
				return;
			}
		}
		internal void updateLineWidth(PdfState state)
		{
			this.m_pRaven.setLineWidth(state.getLineWidth());
		}
		internal void updateLineCap(PdfState state)
		{
			this.m_pRaven.setLineCap(state.getLineCap());
		}
		internal void updateMiterLimit(PdfState state)
		{
			this.m_pRaven.setMiterLimit(state.getMiterLimit());
		}
		internal void updateCTM(PdfState state, double m11, double m12, double m21, double m22, double m31, double m32)
		{
			double[] array = new double[6];
			double[] cTM = state.getCTM();
			array[0] = cTM[0];
			array[1] = cTM[1];
			array[2] = cTM[2];
			array[3] = cTM[3];
			array[4] = cTM[4];
			array[5] = cTM[5];
			this.m_pRaven.setMatrix(array);
		}
		internal void updateLineDash(PdfState state)
		{
			double[] array = null;
			int num = 0;
			double w = 0.0;
			double[] array2 = new double[20];
			state.getLineDash(ref array, ref num, ref w);
			if (num > 20)
			{
				num = 20;
			}
			for (int i = 0; i < num; i++)
			{
				array2[i] = state.transformWidth(array[i]);
				if (array2[i] < 1.0)
				{
					array2[i] = 1.0;
				}
			}
			double lineDashPhase = state.transformWidth(w);
			this.m_pRaven.setLineDash(array2, num, lineDashPhase);
		}
		private RavenPattern getColor(PdfGray gray)
		{
			RavenColor ravenColor = new RavenColor();
			if (this.m_reverseVideo)
			{
				gray.g = 65536 - gray.g;
			}
			ravenColor[0] = PdfState.colToByte(gray.g);
			return new RavenSolidColor(ravenColor);
		}
		private RavenPattern getColor(PdfRGB rgb)
		{
			RavenColor ravenColor = new RavenColor();
			int x;
			int x2;
			int x3;
			if (this.m_reverseVideo)
			{
				x = 65536 - rgb.r;
				x2 = 65536 - rgb.g;
				x3 = 65536 - rgb.b;
			}
			else
			{
				x = rgb.r;
				x2 = rgb.g;
				x3 = rgb.b;
			}
			ravenColor[0] = PdfState.colToByte(x);
			ravenColor[1] = PdfState.colToByte(x2);
			ravenColor[2] = PdfState.colToByte(x3);
			return new RavenSolidColor(ravenColor);
		}
		private RavenPath convertPath(PdfState state, PdfPath path, bool dropEmptySubpaths)
		{
			int num = dropEmptySubpaths ? 1 : 0;
			RavenPath ravenPath = new RavenPath();
			for (int i = 0; i < path.getNumSubpaths(); i++)
			{
				PdfSubpath subpath = path.getSubpath(i);
				if (subpath.getNumPoints() > num)
				{
					ravenPath.moveTo(subpath.getX(0), subpath.getY(0));
					int j = 1;
					while (j < subpath.getNumPoints())
					{
						if (subpath.getCurve(j))
						{
							ravenPath.curveTo(subpath.getX(j), subpath.getY(j), subpath.getX(j + 1), subpath.getY(j + 1), subpath.getX(j + 2), subpath.getY(j + 2));
							j += 3;
						}
						else
						{
							ravenPath.lineTo(subpath.getX(j), subpath.getY(j));
							j++;
						}
					}
					if (subpath.isClosed())
					{
						ravenPath.close();
					}
				}
			}
			return ravenPath;
		}
		internal void eoClip(PdfState state)
		{
			RavenPath path = this.convertPath(state, state.getPath(), true);
			this.m_pRaven.clipToPath(path, true);
		}
		internal void updateBlendMode(PdfState state)
		{
			this.m_pRaven.setBlendFunc(PdfRavenOutputDev.ravenOutBlendFuncs[(int)state.getBlendMode()]);
		}
		internal void updateFillOpacity(PdfState state)
		{
			this.m_pRaven.setFillAlpha(state.getFillOpacity());
		}
		internal void updateStrokeOpacity(PdfState state)
		{
			this.m_pRaven.setStrokeAlpha(state.getStrokeOpacity());
		}
		internal static bool imageSrc(object data, RavenColorPtr colorLine, byte[] alphaLine)
		{
			PdfRavenOutputDev.RavenOutImageData ravenOutImageData = (PdfRavenOutputDev.RavenOutImageData)data;
			PdfRGB pdfRGB = new PdfRGB();
			PdfGray pdfGray = new PdfGray();
			if (ravenOutImageData.y == ravenOutImageData.height)
			{
				return false;
			}
			RavenColorPtr ravenColorPtr = new RavenColorPtr(ravenOutImageData.imgStr.getLine());
			if (ravenColorPtr.buffer == null)
			{
				return false;
			}
			int numPixelComps = ravenOutImageData.colorMap.getNumPixelComps();
			if (ravenOutImageData.lookup != null)
			{
				switch (ravenOutImageData.colorMode)
				{
				case RavenColorMode.ravenModeMono1:
				case RavenColorMode.ravenModeMono8:
				{
					int i = 0;
					RavenColorPtr ravenColorPtr2 = new RavenColorPtr(colorLine, 0);
					while (i < ravenOutImageData.width)
					{
						ravenColorPtr2.ptr = ravenOutImageData.lookup[(int)ravenColorPtr.ptr];
						ravenColorPtr2 = ++ravenColorPtr2;
						i++;
						ravenColorPtr = ++ravenColorPtr;
					}
					break;
				}
				case RavenColorMode.ravenModeRGB8:
				case RavenColorMode.ravenModeBGR8:
				{
					int i = 0;
					RavenColorPtr ravenColorPtr2 = colorLine;
					while (i < ravenOutImageData.width)
					{
						RavenColorPtr ravenColorPtr3 = new RavenColorPtr(ravenOutImageData.lookup, (int)(3 * ravenColorPtr.ptr));
						ravenColorPtr2.ptr = ravenColorPtr3[0];
						ravenColorPtr2 = ++ravenColorPtr2;
						ravenColorPtr2.ptr = ravenColorPtr3[1];
						ravenColorPtr2 = ++ravenColorPtr2;
						ravenColorPtr2.ptr = ravenColorPtr3[2];
						ravenColorPtr2 = ++ravenColorPtr2;
						i++;
						ravenColorPtr = ++ravenColorPtr;
					}
					break;
				}
				}
			}
			else
			{
				switch (ravenOutImageData.colorMode)
				{
				case RavenColorMode.ravenModeMono1:
				case RavenColorMode.ravenModeMono8:
				{
					int i = 0;
					RavenColorPtr ravenColorPtr2 = colorLine;
					while (i < ravenOutImageData.width)
					{
						ravenOutImageData.colorMap.getGray(ravenColorPtr, pdfGray);
						ravenColorPtr2.ptr = PdfState.colToByte(pdfGray.g);
						ravenColorPtr2 = ++ravenColorPtr2;
						i++;
						ravenColorPtr.Inc(numPixelComps);
					}
					break;
				}
				case RavenColorMode.ravenModeRGB8:
				case RavenColorMode.ravenModeBGR8:
				{
					int i = 0;
					RavenColorPtr ravenColorPtr2 = colorLine;
					while (i < ravenOutImageData.width)
					{
						ravenOutImageData.colorMap.getRGB(ravenColorPtr, pdfRGB);
						ravenColorPtr2.ptr = PdfState.colToByte(pdfRGB.r);
						ravenColorPtr2 = ++ravenColorPtr2;
						ravenColorPtr2.ptr = PdfState.colToByte(pdfRGB.g);
						ravenColorPtr2 = ++ravenColorPtr2;
						ravenColorPtr2.ptr = PdfState.colToByte(pdfRGB.b);
						ravenColorPtr2 = ++ravenColorPtr2;
						i++;
						ravenColorPtr.Inc(numPixelComps);
					}
					break;
				}
				}
			}
			ravenOutImageData.y++;
			return true;
		}
		internal static bool imageMaskSrc(object data, RavenColorPtr line)
		{
			PdfRavenOutputDev.RavenOutImageMaskData ravenOutImageMaskData = (PdfRavenOutputDev.RavenOutImageMaskData)data;
			if (ravenOutImageMaskData.y == ravenOutImageMaskData.height)
			{
				return false;
			}
			byte[] line2;
			if ((line2 = ravenOutImageMaskData.imgStr.getLine()) == null)
			{
				return false;
			}
			int num = 0;
			int i = 0;
			RavenColorPtr ravenColorPtr = line;
			while (i < ravenOutImageMaskData.width)
			{
                ravenColorPtr.ptr = (byte)((uint)line2[num++] ^ (uint)(byte)ravenOutImageMaskData.invert);
				ravenColorPtr = ++ravenColorPtr;
				i++;
			}
			ravenOutImageMaskData.y++;
			return true;
		}
		private bool maskedImageSrc(object data, RavenColorPtr colorLine, byte[] alphaLine)
		{
			PdfRavenOutputDev.RavenOutMaskedImageData ravenOutMaskedImageData = (PdfRavenOutputDev.RavenOutMaskedImageData)data;
			PdfRGB pdfRGB = new PdfRGB();
			PdfGray pdfGray = new PdfGray();
			if (ravenOutMaskedImageData.y == ravenOutMaskedImageData.height)
			{
				return false;
			}
			RavenColorPtr ravenColorPtr = new RavenColorPtr(ravenOutMaskedImageData.imgStr.getLine());
			if (ravenColorPtr.buffer == null)
			{
				return false;
			}
			int numPixelComps = ravenOutMaskedImageData.colorMap.getNumPixelComps();
			RavenColorPtr ravenColorPtr2 = new RavenColorPtr(ravenOutMaskedImageData.mask.getDataPtr(), ravenOutMaskedImageData.y * ravenOutMaskedImageData.mask.getRowSize());
			int num = 128;
			int i = 0;
			RavenColorPtr ravenColorPtr3 = colorLine;
			RavenColorPtr ravenColorPtr4 = new RavenColorPtr(alphaLine);
			while (i < ravenOutMaskedImageData.width)
			{
                byte ptr = ((int)ravenColorPtr2.ptr & num) != 0 ? byte.MaxValue : (byte)0;
				if ((num >>= 1) == 0)
				{
					ravenColorPtr2 = ++ravenColorPtr2;
					num = 128;
				}
				if (ravenOutMaskedImageData.lookup != null)
				{
					switch (ravenOutMaskedImageData.colorMode)
					{
					case RavenColorMode.ravenModeMono1:
					case RavenColorMode.ravenModeMono8:
						ravenColorPtr3.ptr = ravenOutMaskedImageData.lookup[(int)ravenColorPtr.ptr];
						ravenColorPtr3 = ++ravenColorPtr3;
						break;
					case RavenColorMode.ravenModeRGB8:
					case RavenColorMode.ravenModeBGR8:
					{
						RavenColorPtr ravenColorPtr5 = new RavenColorPtr(ravenOutMaskedImageData.lookup, (int)(3 * ravenColorPtr.ptr));
						ravenColorPtr3.ptr = ravenColorPtr5[0];
						ravenColorPtr3 = ++ravenColorPtr3;
						ravenColorPtr3.ptr = ravenColorPtr5[1];
						ravenColorPtr3 = ++ravenColorPtr3;
						ravenColorPtr3.ptr = ravenColorPtr5[2];
						ravenColorPtr3 = ++ravenColorPtr3;
						break;
					}
					}
					ravenColorPtr4.ptr = ptr;
					ravenColorPtr4 = ++ravenColorPtr4;
				}
				else
				{
					switch (ravenOutMaskedImageData.colorMode)
					{
					case RavenColorMode.ravenModeMono1:
					case RavenColorMode.ravenModeMono8:
						ravenOutMaskedImageData.colorMap.getGray(ravenColorPtr, pdfGray);
						ravenColorPtr3.ptr = PdfState.colToByte(pdfGray.g);
						ravenColorPtr3 = ++ravenColorPtr3;
						break;
					case RavenColorMode.ravenModeRGB8:
					case RavenColorMode.ravenModeBGR8:
						ravenOutMaskedImageData.colorMap.getRGB(ravenColorPtr, pdfRGB);
						ravenColorPtr3.ptr = PdfState.colToByte(pdfRGB.r);
						ravenColorPtr3 = ++ravenColorPtr3;
						ravenColorPtr3.ptr = PdfState.colToByte(pdfRGB.g);
						ravenColorPtr3 = ++ravenColorPtr3;
						ravenColorPtr3.ptr = PdfState.colToByte(pdfRGB.b);
						ravenColorPtr3 = ++ravenColorPtr3;
						break;
					}
					ravenColorPtr4.ptr = ptr;
					ravenColorPtr4 = ++ravenColorPtr4;
				}
				i++;
				ravenColorPtr.Inc(numPixelComps);
			}
			ravenOutMaskedImageData.y++;
			return true;
		}
		internal static bool alphaImageSrc(object data, RavenColorPtr colorLine, byte[] alphaLine)
		{
			PdfRavenOutputDev.RavenOutImageData ravenOutImageData = (PdfRavenOutputDev.RavenOutImageData)data;
			PdfRGB pdfRGB = new PdfRGB();
			PdfGray pdfGray = new PdfGray();
			if (ravenOutImageData.y == ravenOutImageData.height)
			{
				return false;
			}
			RavenColorPtr ravenColorPtr = new RavenColorPtr(ravenOutImageData.imgStr.getLine());
			if (ravenColorPtr.buffer == null)
			{
				return false;
			}
			int numPixelComps = ravenOutImageData.colorMap.getNumPixelComps();
			int i = 0;
			RavenColorPtr ravenColorPtr2 = colorLine;
			RavenColorPtr ravenColorPtr3 = new RavenColorPtr(alphaLine);
			while (i < ravenOutImageData.width)
			{
				byte ptr = 0;
				for (int j = 0; j < numPixelComps; j++)
				{
					if ((int)ravenColorPtr[j] < ravenOutImageData.maskColors[2 * j] || (int)ravenColorPtr[j] > ravenOutImageData.maskColors[2 * j + 1])
					{
						ptr = 255;
						break;
					}
				}
				if (ravenOutImageData.lookup != null)
				{
					switch (ravenOutImageData.colorMode)
					{
					case RavenColorMode.ravenModeMono1:
					case RavenColorMode.ravenModeMono8:
						ravenColorPtr2.ptr = ravenOutImageData.lookup[(int)ravenColorPtr.ptr];
						ravenColorPtr2 = ++ravenColorPtr2;
						break;
					case RavenColorMode.ravenModeRGB8:
					case RavenColorMode.ravenModeBGR8:
					{
						RavenColorPtr ravenColorPtr4 = new RavenColorPtr(ravenOutImageData.lookup, (int)(3 * ravenColorPtr.ptr));
						ravenColorPtr2.ptr = ravenColorPtr4[0];
						ravenColorPtr2 = ++ravenColorPtr2;
						ravenColorPtr2.ptr = ravenColorPtr4[1];
						ravenColorPtr2 = ++ravenColorPtr2;
						ravenColorPtr2.ptr = ravenColorPtr4[2];
						ravenColorPtr2 = ++ravenColorPtr2;
						break;
					}
					}
					ravenColorPtr3.ptr = ptr;
					ravenColorPtr3 = ++ravenColorPtr3;
				}
				else
				{
					switch (ravenOutImageData.colorMode)
					{
					case RavenColorMode.ravenModeMono1:
					case RavenColorMode.ravenModeMono8:
						ravenOutImageData.colorMap.getGray(ravenColorPtr, pdfGray);
						ravenColorPtr2.ptr = PdfState.colToByte(pdfGray.g);
						ravenColorPtr2 = ++ravenColorPtr2;
						break;
					case RavenColorMode.ravenModeRGB8:
					case RavenColorMode.ravenModeBGR8:
						ravenOutImageData.colorMap.getRGB(ravenColorPtr, pdfRGB);
						ravenColorPtr2.ptr = PdfState.colToByte(pdfRGB.r);
						ravenColorPtr2 = ++ravenColorPtr2;
						ravenColorPtr2.ptr = PdfState.colToByte(pdfRGB.g);
						ravenColorPtr2 = ++ravenColorPtr2;
						ravenColorPtr2.ptr = PdfState.colToByte(pdfRGB.b);
						ravenColorPtr2 = ++ravenColorPtr2;
						break;
					}
					ravenColorPtr3.ptr = ptr;
					ravenColorPtr3 = ++ravenColorPtr3;
				}
				i++;
				ravenColorPtr.Inc(numPixelComps);
			}
			ravenOutImageData.y++;
			return true;
		}
		internal void drawImageMask(PdfState state, PdfObject reff, PdfDictWithStream str, int width, int height, bool invert, bool inlineImg)
		{
			double[] array = new double[6];
			PdfRavenOutputDev.RavenOutImageMaskData ravenOutImageMaskData = new PdfRavenOutputDev.RavenOutImageMaskData();
			if (state.getFillColorSpace().isNonMarking())
			{
				return;
			}
			this.setOverprintMask(state.getFillColorSpace(), state.getFillOverprint(), state.getOverprintMode(), state.getFillColor());
			double[] cTM = state.getCTM();
			array[0] = cTM[0];
			array[1] = cTM[1];
			array[2] = -cTM[2];
			array[3] = -cTM[3];
			array[4] = cTM[2] + cTM[4];
			array[5] = cTM[3] + cTM[5];
			ravenOutImageMaskData.imgStr = new PdfImageStream(str, width, 1, 1);
			ravenOutImageMaskData.imgStr.reset();
			ravenOutImageMaskData.invert = (invert ? 0 : 1);
			ravenOutImageMaskData.width = width;
			ravenOutImageMaskData.height = height;
			ravenOutImageMaskData.y = 0;
			this.m_pRaven.fillImageMask(new RavenImageMaskSource(PdfRavenOutputDev.imageMaskSrc), ravenOutImageMaskData, width, height, array, this.t3GlyphStack != null);
			if (inlineImg)
			{
				while (ravenOutImageMaskData.y < height)
				{
					ravenOutImageMaskData.imgStr.getLine();
					ravenOutImageMaskData.y++;
				}
			}
		}
		private void setOverprintMask(PdfColorSpaceTop colorSpace, bool overprintFlag, int overprintMode, PdfColor singleColor)
		{
		}
		internal void drawSoftMaskedImage(PdfState state, PdfObject reff, PdfDictWithStream str, int width, int height, PdfImageColorMap colorMap, PdfDictWithStream maskStr, int maskWidth, int maskHeight, PdfImageColorMap maskColorMap)
		{
			double[] array = new double[6];
			PdfRavenOutputDev.RavenOutImageData ravenOutImageData = new PdfRavenOutputDev.RavenOutImageData();
			PdfRavenOutputDev.RavenOutImageData ravenOutImageData2 = new PdfRavenOutputDev.RavenOutImageData();
			RavenColor ravenColor = new RavenColor();
			PdfGray pdfGray = new PdfGray();
			PdfRGB pdfRGB = new PdfRGB();
			RavenColorPtr ravenColorPtr = new RavenColorPtr(new byte[1]);
			this.setOverprintMask(colorMap.getColorSpace(), state.getFillOverprint(), state.getOverprintMode(), null);
			double[] cTM = state.getCTM();
			array[0] = cTM[0];
			array[1] = cTM[1];
			array[2] = -cTM[2];
			array[3] = -cTM[3];
			array[4] = cTM[2] + cTM[4];
			array[5] = cTM[3] + cTM[5];
			ravenOutImageData2.imgStr = new PdfImageStream(maskStr, maskWidth, maskColorMap.getNumPixelComps(), maskColorMap.getBits());
			ravenOutImageData2.imgStr.reset();
			ravenOutImageData2.colorMap = maskColorMap;
			ravenOutImageData2.maskColors = null;
			ravenOutImageData2.colorMode = RavenColorMode.ravenModeMono8;
			ravenOutImageData2.width = maskWidth;
			ravenOutImageData2.height = maskHeight;
			ravenOutImageData2.y = 0;
			int num = 1 << maskColorMap.getBits();
			ravenOutImageData2.lookup = new RavenColorPtr(new byte[num]);
			for (int i = 0; i < num; i++)
			{
				ravenColorPtr[0] = (byte)i;
				maskColorMap.getGray(ravenColorPtr, pdfGray);
				ravenOutImageData2.lookup[i] = PdfState.colToByte(pdfGray.g);
			}
			RavenBitmap ravenBitmap = new RavenBitmap(this.m_pBitmap.getWidth(), this.m_pBitmap.getHeight(), 1, RavenColorMode.ravenModeMono8, false, true);
            Raven raven = new Raven(ravenBitmap, this.m_vectorAntialias, (RavenScreenParams)null);
			ravenColor[0] = 0;
			raven.clear(ravenColor);
			raven.drawImage(new RavenImageSource(PdfRavenOutputDev.imageSrc), ravenOutImageData2, RavenColorMode.ravenModeMono8, false, maskWidth, maskHeight, array);
			this.m_pRaven.setSoftMask(ravenBitmap);
			ravenOutImageData.imgStr = new PdfImageStream(str, width, colorMap.getNumPixelComps(), colorMap.getBits());
			ravenOutImageData.imgStr.reset();
			ravenOutImageData.colorMap = colorMap;
			ravenOutImageData.maskColors = null;
			ravenOutImageData.colorMode = this.m_colorMode;
			ravenOutImageData.width = width;
			ravenOutImageData.height = height;
			ravenOutImageData.y = 0;
			ravenOutImageData.lookup = null;
			if (colorMap.getNumPixelComps() == 1)
			{
				num = 1 << colorMap.getBits();
				switch (this.m_colorMode)
				{
				case RavenColorMode.ravenModeMono1:
				case RavenColorMode.ravenModeMono8:
					ravenOutImageData.lookup = new RavenColorPtr(new byte[num]);
					for (int i = 0; i < num; i++)
					{
						ravenColorPtr[0] = (byte)i;
						colorMap.getGray(ravenColorPtr, pdfGray);
						ravenOutImageData.lookup[i] = PdfState.colToByte(pdfGray.g);
					}
					break;
				case RavenColorMode.ravenModeRGB8:
				case RavenColorMode.ravenModeBGR8:
					ravenOutImageData.lookup = new RavenColorPtr(new byte[num * 3]);
					for (int i = 0; i < num; i++)
					{
						ravenColorPtr[0] = (byte)i;
						colorMap.getRGB(ravenColorPtr, pdfRGB);
						ravenOutImageData.lookup[3 * i] = PdfState.colToByte(pdfRGB.r);
						ravenOutImageData.lookup[3 * i + 1] = PdfState.colToByte(pdfRGB.g);
						ravenOutImageData.lookup[3 * i + 2] = PdfState.colToByte(pdfRGB.b);
					}
					break;
				}
			}
			RavenColorMode srcMode;
			if (this.m_colorMode == RavenColorMode.ravenModeMono1)
			{
				srcMode = RavenColorMode.ravenModeMono8;
			}
			else
			{
				srcMode = this.m_colorMode;
			}
			this.m_pRaven.drawImage(new RavenImageSource(PdfRavenOutputDev.imageSrc), ravenOutImageData, srcMode, false, width, height, array);
			this.m_pRaven.setSoftMask(null);
		}
		internal void drawMaskedImage(PdfState state, PdfObject reff, PdfDictWithStream str, int width, int height, PdfImageColorMap colorMap, PdfDictWithStream maskStr, int maskWidth, int maskHeight, bool maskInvert)
		{
			double[] array = new double[6];
			PdfRavenOutputDev.RavenOutMaskedImageData ravenOutMaskedImageData = new PdfRavenOutputDev.RavenOutMaskedImageData();
			PdfRavenOutputDev.RavenOutImageMaskData ravenOutImageMaskData = new PdfRavenOutputDev.RavenOutImageMaskData();
			RavenColor ravenColor = new RavenColor();
			PdfGray pdfGray = new PdfGray();
			PdfRGB pdfRGB = new PdfRGB();
			RavenColorPtr ravenColorPtr = new RavenColorPtr(new byte[1]);
			this.setOverprintMask(colorMap.getColorSpace(), state.getFillOverprint(), state.getOverprintMode(), null);
			if (maskWidth > width || maskHeight > height)
			{
				PdfArray pdfArray = new PdfArray(null);
				if (maskInvert)
				{
					pdfArray.Add(new PdfNumber(null, 0.0));
					pdfArray.Add(new PdfNumber(null, 1.0));
				}
				else
				{
					pdfArray.Add(new PdfNumber(null, 1.0));
					pdfArray.Add(new PdfNumber(null, 0.0));
				}
				PdfImageColorMap maskColorMap = new PdfImageColorMap(1, pdfArray, new PdfDeviceGrayColorSpace());
				this.drawSoftMaskedImage(state, reff, str, width, height, colorMap, maskStr, maskWidth, maskHeight, maskColorMap);
				return;
			}
			array[0] = (double)width;
			array[1] = 0.0;
			array[2] = 0.0;
			array[3] = (double)height;
			array[4] = 0.0;
			array[5] = 0.0;
			ravenOutImageMaskData.imgStr = new PdfImageStream(maskStr, maskWidth, 1, 1);
			ravenOutImageMaskData.imgStr.reset();
			ravenOutImageMaskData.invert = (maskInvert ? 0 : 1);
			ravenOutImageMaskData.width = maskWidth;
			ravenOutImageMaskData.height = maskHeight;
			ravenOutImageMaskData.y = 0;
			RavenBitmap ravenBitmap = new RavenBitmap(width, height, 1, RavenColorMode.ravenModeMono1, false, true);
            Raven raven = new Raven(ravenBitmap, false, (RavenScreenParams)null);
			ravenColor[0] = 0;
			raven.clear(ravenColor);
			ravenColor[0] = 255;
			raven.setFillPattern(new RavenSolidColor(ravenColor));
			raven.fillImageMask(new RavenImageMaskSource(PdfRavenOutputDev.imageMaskSrc), ravenOutImageMaskData, maskWidth, maskHeight, array, false);
			double[] cTM = state.getCTM();
			array[0] = cTM[0];
			array[1] = cTM[1];
			array[2] = -cTM[2];
			array[3] = -cTM[3];
			array[4] = cTM[2] + cTM[4];
			array[5] = cTM[3] + cTM[5];
			ravenOutMaskedImageData.imgStr = new PdfImageStream(str, width, colorMap.getNumPixelComps(), colorMap.getBits());
			ravenOutMaskedImageData.imgStr.reset();
			ravenOutMaskedImageData.colorMap = colorMap;
			ravenOutMaskedImageData.mask = ravenBitmap;
			ravenOutMaskedImageData.colorMode = this.m_colorMode;
			ravenOutMaskedImageData.width = width;
			ravenOutMaskedImageData.height = height;
			ravenOutMaskedImageData.y = 0;
			ravenOutMaskedImageData.lookup = null;
			if (colorMap.getNumPixelComps() == 1)
			{
				int num = 1 << colorMap.getBits();
				switch (this.m_colorMode)
				{
				case RavenColorMode.ravenModeMono1:
				case RavenColorMode.ravenModeMono8:
					ravenOutMaskedImageData.lookup = new RavenColorPtr(new byte[num]);
					for (int i = 0; i < num; i++)
					{
						ravenColorPtr[0] = (byte)i;
						colorMap.getGray(ravenColorPtr, pdfGray);
						ravenOutMaskedImageData.lookup[i] = PdfState.colToByte(pdfGray.g);
					}
					break;
				case RavenColorMode.ravenModeRGB8:
				case RavenColorMode.ravenModeBGR8:
					ravenOutMaskedImageData.lookup = new RavenColorPtr(new byte[3 * num]);
					for (int i = 0; i < num; i++)
					{
						ravenColorPtr[0] = (byte)i;
						colorMap.getRGB(ravenColorPtr, pdfRGB);
						ravenOutMaskedImageData.lookup[3 * i] = PdfState.colToByte(pdfRGB.r);
						ravenOutMaskedImageData.lookup[3 * i + 1] = PdfState.colToByte(pdfRGB.g);
						ravenOutMaskedImageData.lookup[3 * i + 2] = PdfState.colToByte(pdfRGB.b);
					}
					break;
				}
			}
			RavenColorMode srcMode;
			if (this.m_colorMode == RavenColorMode.ravenModeMono1)
			{
				srcMode = RavenColorMode.ravenModeMono8;
			}
			else
			{
				srcMode = this.m_colorMode;
			}
			this.m_pRaven.drawImage(new RavenImageSource(this.maskedImageSrc), ravenOutMaskedImageData, srcMode, true, width, height, array);
		}
		internal void drawImage(PdfState state, PdfObject reff, PdfDictWithStream str, int width, int height, PdfImageColorMap colorMap, int[] maskColors, bool inlineImg)
		{
			double[] array = new double[6];
			PdfRavenOutputDev.RavenOutImageData ravenOutImageData = new PdfRavenOutputDev.RavenOutImageData();
			PdfGray pdfGray = new PdfGray();
			PdfRGB pdfRGB = new PdfRGB();
			RavenColorPtr ravenColorPtr = new RavenColorPtr(new byte[1]);
			this.setOverprintMask(colorMap.getColorSpace(), state.getFillOverprint(), state.getOverprintMode(), null);
			double[] cTM = state.getCTM();
			array[0] = cTM[0];
			array[1] = cTM[1];
			array[2] = -cTM[2];
			array[3] = -cTM[3];
			array[4] = cTM[2] + cTM[4];
			array[5] = cTM[3] + cTM[5];
			ravenOutImageData.imgStr = new PdfImageStream(str, width, colorMap.getNumPixelComps(), colorMap.getBits());
			ravenOutImageData.imgStr.reset();
			ravenOutImageData.colorMap = colorMap;
			ravenOutImageData.maskColors = maskColors;
			ravenOutImageData.colorMode = this.m_colorMode;
			ravenOutImageData.width = width;
			ravenOutImageData.height = height;
			ravenOutImageData.y = 0;
			PdfFunctionTop pdfFunctionTop = state.getTransfer()[0];
			double[] array2 = new double[1];
			double[] array3 = new double[1];
			ravenOutImageData.lookup = null;
			if (colorMap.getNumPixelComps() == 1)
			{
				int num = 1 << colorMap.getBits();
				switch (this.m_colorMode)
				{
				case RavenColorMode.ravenModeMono1:
				case RavenColorMode.ravenModeMono8:
					ravenOutImageData.lookup = new RavenColorPtr(new byte[num]);
					for (int i = 0; i < num; i++)
					{
						ravenColorPtr[0] = (byte)i;
						colorMap.getGray(ravenColorPtr, pdfGray);
						ravenOutImageData.lookup[i] = PdfState.colToByte(pdfGray.g);
					}
					break;
				case RavenColorMode.ravenModeRGB8:
				case RavenColorMode.ravenModeBGR8:
					ravenOutImageData.lookup = new RavenColorPtr(new byte[num * 3]);
					for (int i = 0; i < num; i++)
					{
						ravenColorPtr[0] = (byte)i;
						colorMap.getRGB(ravenColorPtr, pdfRGB);
						if (pdfFunctionTop != null && pdfFunctionTop.getInputSize() == 1 && pdfFunctionTop.getOutputSize() == 1)
						{
							array2[0] = PdfState.colToDbl(pdfRGB.r);
							pdfFunctionTop.transform(array2, array3);
							pdfRGB.r = PdfState.dblToCol(array3[0]);
							array2[0] = PdfState.colToDbl(pdfRGB.g);
							pdfFunctionTop.transform(array2, array3);
							pdfRGB.g = PdfState.dblToCol(array3[0]);
							array2[0] = PdfState.colToDbl(pdfRGB.b);
							pdfFunctionTop.transform(array2, array3);
							pdfRGB.b = PdfState.dblToCol(array3[0]);
						}
						ravenOutImageData.lookup[3 * i] = PdfState.colToByte(pdfRGB.r);
						ravenOutImageData.lookup[3 * i + 1] = PdfState.colToByte(pdfRGB.g);
						ravenOutImageData.lookup[3 * i + 2] = PdfState.colToByte(pdfRGB.b);
					}
					break;
				}
			}
			RavenColorMode srcMode;
			if (this.m_colorMode == RavenColorMode.ravenModeMono1)
			{
				srcMode = RavenColorMode.ravenModeMono8;
			}
			else
			{
				srcMode = this.m_colorMode;
			}
			RavenImageSource src;
			if (maskColors != null)
			{
				src = new RavenImageSource(PdfRavenOutputDev.alphaImageSrc);
			}
			else
			{
				src = new RavenImageSource(PdfRavenOutputDev.imageSrc);
			}
			this.m_pRaven.drawImage(src, ravenOutImageData, srcMode, maskColors != null, width, height, array);
			if (inlineImg)
			{
				while (ravenOutImageData.y < height)
				{
					ravenOutImageData.imgStr.getLine();
					ravenOutImageData.y++;
				}
			}
		}
		internal void beginTransparencyGroup(PdfState state, double[] bbox, PdfColorSpaceTop blendingColorSpace, bool isolated, bool knockout, bool forSoftMask)
		{
			RavenColor ravenColor = new RavenColor();
			double num = 0.0;
			double num2 = 0.0;
			state.transform(bbox[0], bbox[1], ref num, ref num2);
			double num4;
			double num3 = num4 = num;
			double num6;
			double num5 = num6 = num2;
			state.transform(bbox[0], bbox[3], ref num, ref num2);
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
			if (num2 < num6)
			{
				num6 = num2;
			}
			else
			{
				if (num2 > num5)
				{
					num5 = num2;
				}
			}
			state.transform(bbox[2], bbox[1], ref num, ref num2);
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
			if (num2 < num6)
			{
				num6 = num2;
			}
			else
			{
				if (num2 > num5)
				{
					num5 = num2;
				}
			}
			state.transform(bbox[2], bbox[3], ref num, ref num2);
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
			if (num2 < num6)
			{
				num6 = num2;
			}
			else
			{
				if (num2 > num5)
				{
					num5 = num2;
				}
			}
			int num7 = (int)Math.Floor(num4);
			if (num7 < 0)
			{
				num7 = 0;
			}
			else
			{
				if (num7 >= this.m_pBitmap.getWidth())
				{
					num7 = this.m_pBitmap.getWidth() - 1;
				}
			}
			int num8 = (int)Math.Floor(num6);
			if (num8 < 0)
			{
				num8 = 0;
			}
			else
			{
				if (num8 >= this.m_pBitmap.getHeight())
				{
					num8 = this.m_pBitmap.getHeight() - 1;
				}
			}
			int num9 = (int)Math.Ceiling(num3) - num7 + 1;
			if (num7 + num9 > this.m_pBitmap.getWidth())
			{
				num9 = this.m_pBitmap.getWidth() - num7;
			}
			if (num9 < 1)
			{
				num9 = 1;
			}
			int num10 = (int)Math.Ceiling(num5) - num8 + 1;
			if (num8 + num10 > this.m_pBitmap.getHeight())
			{
				num10 = this.m_pBitmap.getHeight() - num8;
			}
			if (num10 < 1)
			{
				num10 = 1;
			}
			PdfRavenOutputDev.RavenTransparencyGroup ravenTransparencyGroup = new PdfRavenOutputDev.RavenTransparencyGroup();
			ravenTransparencyGroup.tx = num7;
			ravenTransparencyGroup.ty = num8;
			ravenTransparencyGroup.blendingColorSpace = blendingColorSpace;
			ravenTransparencyGroup.isolated = isolated;
			ravenTransparencyGroup.next = this.m_pTranspGroupStack;
			this.m_pTranspGroupStack = ravenTransparencyGroup;
			ravenTransparencyGroup.origBitmap = this.m_pBitmap;
			ravenTransparencyGroup.origRaven = this.m_pRaven;
			if (forSoftMask && isolated && blendingColorSpace != null)
			{
				if (blendingColorSpace.getMode() == enumColorSpaceMode.csDeviceGray || blendingColorSpace.getMode() == enumColorSpaceMode.csCalGray || (blendingColorSpace.getMode() == enumColorSpaceMode.csICCBased && blendingColorSpace.getNComps() == 1))
				{
					this.m_colorMode = RavenColorMode.ravenModeMono8;
				}
				else
				{
					if (blendingColorSpace.getMode() == enumColorSpaceMode.csDeviceRGB || blendingColorSpace.getMode() == enumColorSpaceMode.csCalRGB || (blendingColorSpace.getMode() == enumColorSpaceMode.csICCBased && blendingColorSpace.getNComps() == 3))
					{
						this.m_colorMode = RavenColorMode.ravenModeRGB8;
					}
				}
			}
			this.m_pBitmap = new RavenBitmap(num9, num10, this.m_bitmapRowPad, this.m_colorMode, true, this.m_bitmapTopDown);
			this.m_pRaven = new Raven(this.m_pBitmap, this.m_vectorAntialias, ravenTransparencyGroup.origRaven.getScreen());
			this.m_pRaven.setMinLineWidth(0.0);
			this.m_pRaven.setFillPattern(ravenTransparencyGroup.origRaven.getFillPattern().copy());
			this.m_pRaven.setStrokePattern(ravenTransparencyGroup.origRaven.getStrokePattern().copy());
			if (isolated)
			{
				for (int i = 0; i < RavenColor.ravenMaxColorComps; i++)
				{
					ravenColor[i] = 0;
				}
				this.m_pRaven.clear(ravenColor, 0);
			}
			else
			{
				this.m_pRaven.blitTransparent(ravenTransparencyGroup.origBitmap, num7, num8, 0, 0, num9, num10);
				this.m_pRaven.setInNonIsolatedGroup(ravenTransparencyGroup.origBitmap, num7, num8);
			}
			ravenTransparencyGroup.tBitmap = this.m_pBitmap;
			state.shiftCTM((double)(-(double)num7), (double)(-(double)num8));
			this.updateCTM(state, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
			this.m_nestCount++;
		}
		internal void endTransparencyGroup(PdfState state)
		{
			this.m_nestCount--;
			this.m_pBitmap = this.m_pTranspGroupStack.origBitmap;
			this.m_colorMode = this.m_pBitmap.getMode();
			this.m_pRaven = this.m_pTranspGroupStack.origRaven;
			state.shiftCTM((double)this.m_pTranspGroupStack.tx, (double)this.m_pTranspGroupStack.ty);
			this.updateCTM(state, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
		}
		internal void setSoftMask(PdfState state, double[] bbox, bool alpha, PdfFunctionTop transferFunc, PdfColor backdropColor)
		{
			RavenColor ravenColor = new RavenColor();
			PdfGray pdfGray = new PdfGray();
			PdfRGB pdfRGB = new PdfRGB();
			double num = 0.0;
			int tx = this.m_pTranspGroupStack.tx;
			int ty = this.m_pTranspGroupStack.ty;
			RavenBitmap tBitmap = this.m_pTranspGroupStack.tBitmap;
			if (!alpha && tBitmap.getMode() != RavenColorMode.ravenModeMono1)
			{
				Raven raven = new Raven(tBitmap, this.m_vectorAntialias, this.m_pTranspGroupStack.origRaven.getScreen());
				if (this.m_pTranspGroupStack.blendingColorSpace != null)
				{
					switch (tBitmap.getMode())
					{
					case RavenColorMode.ravenModeMono8:
						this.m_pTranspGroupStack.blendingColorSpace.getGray(backdropColor, pdfGray);
						ravenColor[0] = PdfState.colToByte(pdfGray.g);
						raven.compositeBackground(new RavenColorPtr(ravenColor.getPtr()));
						break;
					case RavenColorMode.ravenModeRGB8:
					case RavenColorMode.ravenModeBGR8:
						this.m_pTranspGroupStack.blendingColorSpace.getRGB(backdropColor, pdfRGB);
						ravenColor[0] = PdfState.colToByte(pdfRGB.r);
						ravenColor[1] = PdfState.colToByte(pdfRGB.g);
						ravenColor[2] = PdfState.colToByte(pdfRGB.b);
						raven.compositeBackground(new RavenColorPtr(ravenColor.getPtr()));
						break;
					}
				}
			}
			RavenBitmap ravenBitmap = new RavenBitmap(this.m_pBitmap.getWidth(), this.m_pBitmap.getHeight(), 1, RavenColorMode.ravenModeMono8, false, true);
			if (tx < ravenBitmap.getWidth() && ty < ravenBitmap.getHeight())
			{
				RavenColorPtr ravenColorPtr = new RavenColorPtr(ravenBitmap.getDataPtr(), ty * ravenBitmap.getRowSize() + tx);
				for (int i = 0; i < tBitmap.getHeight(); i++)
				{
					for (int j = 0; j < tBitmap.getWidth(); j++)
					{
						if (alpha)
						{
							num = (double)tBitmap.getAlpha(j, i) / 255.0;
						}
						else
						{
							tBitmap.getPixel(j, i, ravenColor);
							switch (tBitmap.getMode())
							{
							case RavenColorMode.ravenModeMono1:
							case RavenColorMode.ravenModeMono8:
								num = (double)ravenColor[0] / 255.0;
								break;
							case RavenColorMode.ravenModeRGB8:
							case RavenColorMode.ravenModeBGR8:
								num = 0.001176470588235294 * (double)ravenColor[0] + 0.0023137254901960782 * (double)ravenColor[1] + 0.00043137254901960784 * (double)ravenColor[2];
								break;
							}
						}
						double num2;
						if (transferFunc != null)
						{
							double[] inValues = new double[]
							{
								num
							};
							double[] array = new double[1];
							transferFunc.transform(inValues, array);
							num2 = array[0];
						}
						else
						{
							num2 = num;
						}
						ravenColorPtr[j] = (byte)(num2 * 255.0 + 0.5);
					}
					ravenColorPtr.Inc(ravenBitmap.getRowSize());
				}
			}
			this.m_pRaven.setSoftMask(ravenBitmap);
			PdfRavenOutputDev.RavenTransparencyGroup pTranspGroupStack = this.m_pTranspGroupStack;
			this.m_pTranspGroupStack = pTranspGroupStack.next;
		}
		internal void paintTransparencyGroup(PdfState state, double[] bbox)
		{
			int tx = this.m_pTranspGroupStack.tx;
			int ty = this.m_pTranspGroupStack.ty;
			RavenBitmap tBitmap = this.m_pTranspGroupStack.tBitmap;
			bool isolated = this.m_pTranspGroupStack.isolated;
			if (tx < this.m_pBitmap.getWidth() && ty < this.m_pBitmap.getHeight())
			{
				this.m_pRaven.setOverprintMask(4294967295u);
				this.m_pRaven.composite(tBitmap, 0, 0, tx, ty, tBitmap.getWidth(), tBitmap.getHeight(), false, !isolated);
			}
			PdfRavenOutputDev.RavenTransparencyGroup pTranspGroupStack = this.m_pTranspGroupStack;
			this.m_pTranspGroupStack = pTranspGroupStack.next;
		}
		internal void updateCharSpace(PdfState state)
		{
		}
		internal void updateWordSpace(PdfState state)
		{
		}
		internal void updateTextShift(PdfState state, double shift)
		{
		}
		internal void updateRender(PdfState state)
		{
		}
		internal void updateRise(PdfState state)
		{
		}
		internal void updateHorizScaling(PdfState state)
		{
		}
		internal void updateTransfer(PdfState state)
		{
		}
		internal void clearSoftMask(PdfState state)
		{
			this.m_pRaven.setSoftMask(null);
		}
		internal void updateFlatness(PdfState state)
		{
			this.m_pRaven.setFlatness((double)state.getFlatness());
		}
		internal void updateLineJoin(PdfState state)
		{
			this.m_pRaven.setLineJoin(state.getLineJoin());
		}
		internal void saveState(PdfState state)
		{
			this.m_pRaven.saveState();
		}
		internal void restoreState(PdfState state)
		{
			this.m_pRaven.restoreState();
			this.m_bNeedFontUpdate = true;
		}
		internal bool useTilingPatternFill()
		{
			return true;
		}
		internal void clipToStrokePath(PdfState state)
		{
			RavenPath path = this.convertPath(state, state.getPath(), false);
			RavenPath path2 = this.m_pRaven.makeStrokePath(path, state.getLineWidth());
			this.m_pRaven.clipToPath(path2, false);
		}
		internal void tilingPatternFill(PdfState state, PdfPreview gfx, PdfObject str, int paintType, PdfDict resDict, double[] mat, double[] bbox, int x0, int y0, int x1, int y1, double xStep, double yStep)
		{
			double num = 0.0;
			double num2 = 0.0;
			RavenColor ravenColor = new RavenColor();
			double[] array = new double[6];
			double num3 = 0.0;
			double num4 = 0.0;
			state.transform(bbox[0] * mat[0] + bbox[1] * mat[2] + mat[4], bbox[0] * mat[1] + bbox[1] * mat[3] + mat[5], ref num, ref num2);
			double num6;
			double num5 = num6 = num;
			double num8;
			double num7 = num8 = num2;
			state.transform(bbox[2] * mat[0] + bbox[1] * mat[2] + mat[4], bbox[2] * mat[1] + bbox[1] * mat[3] + mat[5], ref num, ref num2);
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
			state.transform(bbox[2] * mat[0] + bbox[3] * mat[2] + mat[4], bbox[2] * mat[1] + bbox[3] * mat[3] + mat[5], ref num, ref num2);
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
			state.transform(bbox[0] * mat[0] + bbox[3] * mat[2] + mat[4], bbox[0] * mat[1] + bbox[3] * mat[3] + mat[5], ref num, ref num2);
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
			if (num6 == num5 || num8 == num7)
			{
				return;
			}
			int num9 = (int)Math.Floor(num6);
			int num10 = (int)Math.Floor(num8);
			int num11 = (int)Math.Ceiling(num5) - num9;
			int num12 = (int)Math.Ceiling(num7) - num10;
			int num13 = num11 * num12;
			if (num13 > 1000000 || num13 < 0)
			{
				array[0] = mat[0];
				array[1] = mat[1];
				array[2] = mat[2];
				array[3] = mat[3];
				for (int i = y0; i < y1; i++)
				{
					for (int j = x0; j < x1; j++)
					{
						double num14 = (double)j * xStep;
						double num15 = (double)i * yStep;
						array[4] = num14 * mat[0] + num15 * mat[2] + mat[4];
						array[5] = num14 * mat[1] + num15 * mat[3] + mat[5];
						gfx.drawForm(str, resDict, array, bbox);
					}
				}
				return;
			}
			RavenBitmap pBitmap = this.m_pBitmap;
			Raven pRaven = this.m_pRaven;
			RavenBitmap src = this.m_pBitmap = new RavenBitmap(num11, num12, this.m_bitmapRowPad, this.m_colorMode, true, this.m_bitmapTopDown);
			this.m_pRaven = new Raven(this.m_pBitmap, this.m_vectorAntialias, pRaven.getScreen());
			this.m_pRaven.setMinLineWidth(0.0);
			for (int k = 0; k < RavenColor.ravenMaxColorComps; k++)
			{
				ravenColor[k] = 0;
			}
			this.m_pRaven.clear(ravenColor);
			this.m_nestCount++;
			this.m_pRaven.setFillPattern(pRaven.getFillPattern().copy());
			this.m_pRaven.setStrokePattern(pRaven.getStrokePattern().copy());
			state.shiftCTM((double)(-(double)num9), (double)(-(double)num10));
			this.updateCTM(state, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
			gfx.drawForm(str, resDict, mat, bbox);
			state.shiftCTM((double)num9, (double)num10);
			this.updateCTM(state, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
			this.m_nestCount--;
			this.m_pRaven = null;
			this.m_pBitmap = pBitmap;
			this.m_pRaven = pRaven;
			this.m_pRaven.setOverprintMask(4294967295u);
			for (int i = y0; i < y1; i++)
			{
				for (int j = x0; j < x1; j++)
				{
					double num14 = (double)j * xStep;
					double num15 = (double)i * yStep;
					double x2 = num14 * mat[0] + num15 * mat[2];
					double y2 = num14 * mat[1] + num15 * mat[3];
					state.transformDelta(x2, y2, ref num3, ref num4);
					int xDest = (int)(num3 + (double)num9 + 0.5);
					int yDest = (int)(num4 + (double)num10 + 0.5);
					this.m_pRaven.composite(src, 0, 0, xDest, yDest, num11, num12, false, false);
				}
			}
		}
		internal bool upsideDown()
		{
			return true;
		}
		internal RavenBitmap getBitmap()
		{
			return this.m_pBitmap;
		}
	}
}
