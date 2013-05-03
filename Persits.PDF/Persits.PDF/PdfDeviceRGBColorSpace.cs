using System;
namespace Persits.PDF
{
	internal class PdfDeviceRGBColorSpace : PdfColorSpaceTop
	{
		internal override void getGray(PdfColor color, PdfGray gray)
		{
			gray.g = PdfState.clip01((int)(0.3 * (double)color.c[0] + 0.59 * (double)color.c[1] + 0.11 * (double)color.c[2] + 0.5));
		}
		internal override void getRGB(PdfColor color, PdfRGB rgb)
		{
			rgb.r = PdfState.clip01(color.c[0]);
			rgb.g = PdfState.clip01(color.c[1]);
			rgb.b = PdfState.clip01(color.c[2]);
		}
		internal override void getCMYK(PdfColor color, PdfCMYK cmyk)
		{
			double num = (double)PdfState.clip01(1 - color.c[0]);
			double num2 = (double)PdfState.clip01(1 - color.c[1]);
			double num3 = (double)PdfState.clip01(1 - color.c[2]);
			double num4 = num;
			if (num2 < num4)
			{
				num4 = num2;
			}
			if (num3 < num4)
			{
				num4 = num3;
			}
			cmyk.c = (int)(num - num4);
			cmyk.m = (int)(num2 - num4);
			cmyk.y = (int)(num3 - num4);
			cmyk.k = (int)num4;
		}
		internal override void getDefaultColor(PdfColor color)
		{
			color.c[0] = 0;
			color.c[1] = 0;
			color.c[2] = 0;
		}
		internal override PdfColorSpaceTop copy()
		{
			return new PdfDeviceRGBColorSpace();
		}
		internal override enumColorSpaceMode getMode()
		{
			return enumColorSpaceMode.csDeviceRGB;
		}
		internal override int getNComps()
		{
			return 3;
		}
	}
}
