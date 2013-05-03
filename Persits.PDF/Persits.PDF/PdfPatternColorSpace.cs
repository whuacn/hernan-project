using System;
namespace Persits.PDF
{
	internal class PdfPatternColorSpace : PdfColorSpaceTop
	{
		private PdfColorSpaceTop under;
		internal PdfPatternColorSpace(PdfColorSpaceTop underA)
		{
			this.under = underA;
		}
		internal static PdfColorSpaceTop parse(PdfArray arr, PdfPreview pPreview)
		{
			if (arr.Size != 1 && arr.Size != 2)
			{
				return null;
			}
			PdfColorSpaceTop underA = null;
			if (arr.Size == 2)
			{
				PdfObject @object = arr.GetObject(1);
				if ((underA = PdfColorSpaceTop.parse(@object, pPreview)) == null)
				{
					return null;
				}
			}
			return new PdfPatternColorSpace(underA);
		}
		internal override void getGray(PdfColor color, PdfGray gray)
		{
			gray.g = 0;
		}
		internal override void getRGB(PdfColor color, PdfRGB rgb)
		{
			rgb.r = (rgb.g = (rgb.b = 0));
		}
		internal override void getCMYK(PdfColor color, PdfCMYK cmyk)
		{
			cmyk.c = (cmyk.m = (cmyk.y = 0));
			cmyk.k = 1;
		}
		internal override void getDefaultColor(PdfColor color)
		{
		}
		internal override PdfColorSpaceTop copy()
		{
			return new PdfPatternColorSpace((this.under != null) ? this.under.copy() : null);
		}
		internal override int getNComps()
		{
			return 0;
		}
		internal override enumColorSpaceMode getMode()
		{
			return enumColorSpaceMode.csPattern;
		}
		internal PdfColorSpaceTop getUnder()
		{
			return this.under;
		}
	}
}
