using System;
namespace Persits.PDF
{
	internal class PdfSeparationColorSpace : PdfColorSpaceTop
	{
		private PdfColorSpaceTop alt;
		private PdfFunctionTop func;
		internal PdfSeparationColorSpace(PdfColorSpaceTop altA, PdfFunctionTop funcA)
		{
			this.alt = altA;
			this.func = funcA;
		}
		internal static PdfColorSpaceTop parse(PdfArray arr, PdfPreview pPreview)
		{
			if (arr.Size != 4)
			{
				AuxException.Throw("Invalid Separation color space.", PdfErrors._ERROR_PREVIEW_PARSE);
				return null;
			}
			PdfObject @object = arr.GetObject(1);
			if (@object.m_nType != enumType.pdfName)
			{
				AuxException.Throw("Invalid Separation color space (Name is missing or invalid.)", PdfErrors._ERROR_PREVIEW_PARSE);
				return null;
			}
			@object = arr.GetObject(2);
			PdfColorSpaceTop pdfColorSpaceTop = PdfColorSpaceTop.parse(@object, pPreview);
			if (pdfColorSpaceTop == null)
			{
				AuxException.Throw("Invalid Separation color space (alternate color space is missing or invalid.)", PdfErrors._ERROR_PREVIEW_PARSE);
				return null;
			}
			@object = arr.GetObject(3);
			PdfFunctionTop pdfFunctionTop = PdfFunctionTop.parse(@object);
			if (pdfFunctionTop == null)
			{
				return null;
			}
			return new PdfSeparationColorSpace(pdfColorSpaceTop, pdfFunctionTop);
		}
		internal override void getGray(PdfColor color, PdfGray gray)
		{
			double[] array = new double[32];
			PdfColor pdfColor = new PdfColor();
			double num = PdfState.colToDbl(color.c[0]);
			this.func.transform(new double[]
			{
				num
			}, array);
			for (int i = 0; i < this.alt.getNComps(); i++)
			{
				pdfColor.c[i] = PdfState.dblToCol(array[i]);
			}
			this.alt.getGray(pdfColor, gray);
		}
		internal override void getRGB(PdfColor color, PdfRGB rgb)
		{
			double[] array = new double[32];
			PdfColor pdfColor = new PdfColor();
			double num = PdfState.colToDbl(color.c[0]);
			this.func.transform(new double[]
			{
				num
			}, array);
			for (int i = 0; i < this.alt.getNComps(); i++)
			{
				pdfColor.c[i] = PdfState.dblToCol(array[i]);
			}
			this.alt.getRGB(pdfColor, rgb);
		}
		internal override void getCMYK(PdfColor color, PdfCMYK cmyk)
		{
			double[] array = new double[32];
			PdfColor pdfColor = new PdfColor();
			double num = PdfState.colToDbl(color.c[0]);
			this.func.transform(new double[]
			{
				num
			}, array);
			for (int i = 0; i < this.alt.getNComps(); i++)
			{
				pdfColor.c[i] = PdfState.dblToCol(array[i]);
			}
			this.alt.getCMYK(pdfColor, cmyk);
		}
		internal override void getDefaultColor(PdfColor color)
		{
			color.c[0] = 65536;
		}
		internal override PdfColorSpaceTop copy()
		{
			return new PdfSeparationColorSpace(this.alt.copy(), this.func.copy());
		}
		internal override int getNComps()
		{
			return 1;
		}
		internal override enumColorSpaceMode getMode()
		{
			return enumColorSpaceMode.csSeparation;
		}
		internal PdfColorSpaceTop getAlt()
		{
			return this.alt;
		}
		internal PdfFunctionTop getFunc()
		{
			return this.func;
		}
	}
}
