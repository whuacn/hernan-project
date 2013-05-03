using System;
namespace Persits.PDF
{
	internal class PdfDeviceGrayColorSpace : PdfColorSpaceTop
	{
		internal override void getGray(PdfColor color, PdfGray gray)
		{
			gray.g = PdfState.clip01(color.c[0]);
		}
		internal override void getRGB(PdfColor color, PdfRGB rgb)
		{
			rgb.r = (rgb.g = (rgb.b = PdfState.clip01(color.c[0])));
		}
		internal override void getCMYK(PdfColor color, PdfCMYK cmyk)
		{
			cmyk.c = (cmyk.m = (cmyk.y = 0));
			cmyk.k = PdfState.clip01(1 - color.c[0]);
		}
		internal override void getDefaultColor(PdfColor color)
		{
			color.c[0] = 0;
		}
		internal override PdfColorSpaceTop copy()
		{
			return new PdfDeviceGrayColorSpace();
		}
		internal override enumColorSpaceMode getMode()
		{
			return enumColorSpaceMode.csDeviceGray;
		}
		internal override int getNComps()
		{
			return 1;
		}
	}
}
