using System;
namespace Persits.PDF
{
	internal class PdfIndexedColorSpace : PdfColorSpaceTop
	{
		private PdfColorSpaceTop baseSpace;
		private int indexHigh;
		private byte[] lookup;
		internal PdfIndexedColorSpace(PdfColorSpaceTop baseA, int indexHighA)
		{
			this.baseSpace = baseA;
			this.indexHigh = indexHighA;
			this.lookup = new byte[(this.indexHigh + 1) * this.baseSpace.getNComps()];
		}
		internal static PdfColorSpaceTop parse(PdfArray arr, PdfPreview pPreview)
		{
			if (arr.Size != 4)
			{
				AuxException.Throw("Invalid Indexed color space.", PdfErrors._ERROR_PREVIEW_PARSE);
				return null;
			}
			PdfObject @object = arr.GetObject(1);
			PdfColorSpaceTop pdfColorSpaceTop = PdfColorSpaceTop.parse(@object, pPreview);
			if (pdfColorSpaceTop == null)
			{
				AuxException.Throw("Invalid Indexed color space (base color space is missing or invalid.)", PdfErrors._ERROR_PREVIEW_PARSE);
				return null;
			}
			@object = arr.GetObject(2);
			if (@object.m_nType != enumType.pdfNumber)
			{
				AuxException.Throw("Invalid Indexed color space (hival).", PdfErrors._ERROR_PREVIEW_PARSE);
				return null;
			}
			int num = (int)((PdfNumber)@object).m_fValue;
			if (num < 0 || num > 255)
			{
				AuxException.Throw("Invalid Indexed color space (invalid indexHigh value.)", PdfErrors._ERROR_PREVIEW_PARSE);
				return null;
			}
			PdfIndexedColorSpace pdfIndexedColorSpace = new PdfIndexedColorSpace(pdfColorSpaceTop, num);
			@object = arr.GetObject(3);
			int nComps = pdfColorSpaceTop.getNComps();
			if (@object.m_nType == enumType.pdfDictWithStream)
			{
				int num2 = 0;
				for (int i = 0; i <= num; i++)
				{
					for (int j = 0; j < nComps; j++)
					{
						int num3 = (int)((PdfDictWithStream)@object).m_objStream[num2++];
						if (num2 > ((PdfDictWithStream)@object).m_objStream.Length)
						{
							AuxException.Throw("Invalid Indexed color space (lookup table stream is too short.)", PdfErrors._ERROR_PREVIEW_PARSE);
							return null;
						}
						pdfIndexedColorSpace.lookup[i * nComps + j] = (byte)num3;
					}
				}
			}
			else
			{
				if (@object.m_nType != enumType.pdfString)
				{
					AuxException.Throw("Invalid Indexed color space (lookup table is invalid or missing.)", PdfErrors._ERROR_PREVIEW_PARSE);
					return null;
				}
				string text = ((PdfString)@object).ToString();
				if (text.Length < (num + 1) * nComps)
				{
					AuxException.Throw("Invalid Indexed color space (lookup table string is too short.)", PdfErrors._ERROR_PREVIEW_PARSE);
					return null;
				}
				int num4 = 0;
				for (int i = 0; i <= num; i++)
				{
					for (int j = 0; j < nComps; j++)
					{
						pdfIndexedColorSpace.lookup[i * nComps + j] = (byte)text[num4++];
					}
				}
			}
			return pdfIndexedColorSpace;
		}
		private PdfColor mapColorToBase(PdfColor color, PdfColor baseColor)
		{
			double[] array = new double[32];
			double[] array2 = new double[32];
			int nComps = this.baseSpace.getNComps();
			this.baseSpace.getDefaultRanges(array, array2, this.indexHigh);
			int num = (int)(PdfState.colToDbl(color.c[0]) + 0.5) * nComps;
			for (int i = 0; i < nComps; i++)
			{
				baseColor.c[i] = PdfState.dblToCol(array[i] + (double)this.lookup[num + i] / 255.0 * array2[i]);
			}
			return baseColor;
		}
		internal override void getGray(PdfColor color, PdfGray gray)
		{
			PdfColor baseColor = new PdfColor();
			this.baseSpace.getGray(this.mapColorToBase(color, baseColor), gray);
		}
		internal override void getRGB(PdfColor color, PdfRGB rgb)
		{
			PdfColor baseColor = new PdfColor();
			this.baseSpace.getRGB(this.mapColorToBase(color, baseColor), rgb);
		}
		internal override void getCMYK(PdfColor color, PdfCMYK cmyk)
		{
			PdfColor baseColor = new PdfColor();
			this.baseSpace.getCMYK(this.mapColorToBase(color, baseColor), cmyk);
		}
		internal override void getDefaultColor(PdfColor color)
		{
			color.c[0] = 0;
		}
		internal override PdfColorSpaceTop copy()
		{
			PdfIndexedColorSpace pdfIndexedColorSpace = new PdfIndexedColorSpace(this.baseSpace.copy(), this.indexHigh);
			Array.Copy(this.lookup, pdfIndexedColorSpace.lookup, (this.indexHigh + 1) * this.baseSpace.getNComps());
			return pdfIndexedColorSpace;
		}
		internal override void getDefaultRanges(double[] decodeLow, double[] decodeRange, int maxImgPixel)
		{
			decodeLow[0] = 0.0;
			decodeRange[0] = (double)maxImgPixel;
		}
		internal PdfColorSpaceTop getBase()
		{
			return this.baseSpace;
		}
		internal int getIndexHigh()
		{
			return this.indexHigh;
		}
		internal byte[] getLookup()
		{
			return this.lookup;
		}
		internal override int getNComps()
		{
			return 1;
		}
		internal override enumColorSpaceMode getMode()
		{
			return enumColorSpaceMode.csIndexed;
		}
	}
}
