using System;
namespace Persits.PDF
{
	internal class PdfDeviceCMYKColorSpace : PdfColorSpaceTop
	{
		private PdfPreview m_pPreview;
		internal PdfDeviceCMYKColorSpace(PdfPreview pPreview)
		{
			this.m_pPreview = pPreview;
		}
		internal override void getGray(PdfColor color, PdfGray gray)
		{
			gray.g = PdfState.clip01((int)((double)(65536 - color.c[3]) - 0.3 * (double)color.c[0] - 0.59 * (double)color.c[1] - 0.11 * (double)color.c[2] + 0.5));
		}
		internal override void getRGB(PdfColor color, PdfRGB rgb)
		{
			if (this.m_pPreview.m_pIccProfile != null)
			{
				this.m_pPreview.getRGB(color, rgb);
				return;
			}
			double num = PdfState.colToDbl(color.c[0]);
			double num2 = PdfState.colToDbl(color.c[1]);
			double num3 = PdfState.colToDbl(color.c[2]);
			double num4 = PdfState.colToDbl(color.c[3]);
			double num5 = 1.0 - num;
			double num6 = 1.0 - num2;
			double num7 = 1.0 - num3;
			double num8 = 1.0 - num4;
			double num9 = num5 * num6 * num7 * num8;
			double num12;
			double num11;
			double num10 = num11 = (num12 = num9);
			num9 = num5 * num6 * num7 * num4;
			num11 += 0.1373 * num9;
			num10 += 0.1216 * num9;
			num12 += 0.1255 * num9;
			num9 = num5 * num6 * num3 * num8;
			num11 += num9;
			num10 += 0.949 * num9;
			num9 = num5 * num6 * num3 * num4;
			num11 += 0.1098 * num9;
			num10 += 0.102 * num9;
			num9 = num5 * num2 * num7 * num8;
			num11 += 0.9255 * num9;
			num12 += 0.549 * num9;
			num9 = num5 * num2 * num7 * num4;
			num11 += 0.1412 * num9;
			num9 = num5 * num2 * num3 * num8;
			num11 += 0.9294 * num9;
			num10 += 0.1098 * num9;
			num12 += 0.1412 * num9;
			num9 = num5 * num2 * num3 * num4;
			num11 += 0.1333 * num9;
			num9 = num * num6 * num7 * num8;
			num10 += 0.6784 * num9;
			num12 += 0.9373 * num9;
			num9 = num * num6 * num7 * num4;
			num10 += 0.0588 * num9;
			num12 += 0.1412 * num9;
			num9 = num * num6 * num3 * num8;
			num10 += 0.651 * num9;
			num12 += 0.3137 * num9;
			num9 = num * num6 * num3 * num4;
			num10 += 0.0745 * num9;
			num9 = num * num2 * num7 * num8;
			num11 += 0.1804 * num9;
			num10 += 0.1922 * num9;
			num12 += 0.5725 * num9;
			num9 = num * num2 * num7 * num4;
			num12 += 0.0078 * num9;
			num9 = num * num2 * num3 * num8;
			num11 += 0.2118 * num9;
			num10 += 0.2119 * num9;
			num12 += 0.2235 * num9;
			rgb.r = PdfState.clip01(PdfState.dblToCol(num11));
			rgb.g = PdfState.clip01(PdfState.dblToCol(num10));
			rgb.b = PdfState.clip01(PdfState.dblToCol(num12));
		}
		internal override void getCMYK(PdfColor color, PdfCMYK cmyk)
		{
			cmyk.c = PdfState.clip01(color.c[0]);
			cmyk.m = PdfState.clip01(color.c[1]);
			cmyk.y = PdfState.clip01(color.c[2]);
			cmyk.k = PdfState.clip01(color.c[3]);
		}
		internal override void getDefaultColor(PdfColor color)
		{
			color.c[0] = 0;
			color.c[1] = 0;
			color.c[2] = 0;
			color.c[3] = 65536;
		}
		internal override PdfColorSpaceTop copy()
		{
			return new PdfDeviceCMYKColorSpace(this.m_pPreview);
		}
		internal override enumColorSpaceMode getMode()
		{
			return enumColorSpaceMode.csDeviceCMYK;
		}
		internal override int getNComps()
		{
			return 4;
		}
	}
}
