using System;
namespace Persits.PDF
{
	internal class PdfICCBasedColorSpace : PdfColorSpaceTop
	{
		private int nComps;
		private PdfColorSpaceTop alt;
		private double[] rangeMin = new double[4];
		private double[] rangeMax = new double[4];
		private PdfRef iccProfileStream;
		internal PdfICCBasedColorSpace(int nCompsA, PdfColorSpaceTop altA, PdfRef iccProfileStreamA)
		{
			this.nComps = nCompsA;
			this.alt = altA;
			this.iccProfileStream = iccProfileStreamA;
			this.rangeMin[0] = (this.rangeMin[1] = (this.rangeMin[2] = (this.rangeMin[3] = 0.0)));
			this.rangeMax[0] = (this.rangeMax[1] = (this.rangeMax[2] = (this.rangeMax[3] = 1.0)));
		}
		internal override PdfColorSpaceTop copy()
		{
			PdfICCBasedColorSpace pdfICCBasedColorSpace = new PdfICCBasedColorSpace(this.nComps, this.alt.copy(), this.iccProfileStream);
			for (int i = 0; i < 4; i++)
			{
				pdfICCBasedColorSpace.rangeMin[i] = this.rangeMin[i];
				pdfICCBasedColorSpace.rangeMax[i] = this.rangeMax[i];
			}
			return pdfICCBasedColorSpace;
		}
		internal static PdfColorSpaceTop parse(PdfArray arr, PdfPreview pPreview)
		{
			PdfRef iccProfileStreamA = null;
			PdfObject @object = arr.GetObject(1);
			if (@object.m_nType != enumType.pdfDictWithStream)
			{
				AuxException.Throw("Invalid ICCBased color space (should be a stream.)", PdfErrors._ERROR_PREVIEW_PARSE);
				return null;
			}
			PdfDictWithStream pdfDictWithStream = (PdfDictWithStream)@object;
			PdfObject objectByName = pdfDictWithStream.GetObjectByName("N");
			if (objectByName == null || objectByName.m_nType != enumType.pdfNumber)
			{
				AuxException.Throw("Invalid ICCBased color space (N is missing or invalid.)", PdfErrors._ERROR_PREVIEW_PARSE);
				return null;
			}
			int num = (int)((PdfNumber)objectByName).m_fValue;
			objectByName = pdfDictWithStream.GetObjectByName("Alternate");
			PdfColorSpaceTop altA;
			if (objectByName == null || (altA = PdfColorSpaceTop.parse(objectByName, pPreview)) == null)
			{
				switch (num)
				{
				case 1:
					altA = new PdfDeviceGrayColorSpace();
					goto IL_C8;
				case 3:
					altA = new PdfDeviceRGBColorSpace();
					goto IL_C8;
				case 4:
					altA = new PdfDeviceCMYKColorSpace(pPreview);
					goto IL_C8;
				}
				AuxException.Throw("Invalid ICCBased color space - N is missing or invalid.", PdfErrors._ERROR_PREVIEW_PARSE);
				return null;
			}
			IL_C8:
			PdfICCBasedColorSpace pdfICCBasedColorSpace = new PdfICCBasedColorSpace(num, altA, iccProfileStreamA);
			objectByName = pdfDictWithStream.GetObjectByName("Range");
			if (objectByName != null && objectByName.m_nType == enumType.pdfArray && ((PdfArray)objectByName).Size == 2 * num)
			{
				for (int i = 0; i < num; i++)
				{
					PdfObject object2 = ((PdfArray)objectByName).GetObject(2 * i);
					if (object2.m_nType == enumType.pdfNumber)
					{
						pdfICCBasedColorSpace.rangeMin[i] = ((PdfNumber)object2).m_fValue;
					}
					object2 = ((PdfArray)objectByName).GetObject(2 * i + 1);
					if (object2.m_nType == enumType.pdfNumber)
					{
						pdfICCBasedColorSpace.rangeMax[i] = ((PdfNumber)object2).m_fValue;
					}
				}
			}
			return pdfICCBasedColorSpace;
		}
		internal override void getGray(PdfColor color, PdfGray gray)
		{
			this.alt.getGray(color, gray);
		}
		internal override void getRGB(PdfColor color, PdfRGB rgb)
		{
			this.alt.getRGB(color, rgb);
		}
		internal override void getCMYK(PdfColor color, PdfCMYK cmyk)
		{
			this.alt.getCMYK(color, cmyk);
		}
		internal override void getDefaultColor(PdfColor color)
		{
			for (int i = 0; i < this.nComps; i++)
			{
				if (this.rangeMin[i] > 0.0)
				{
					color.c[i] = PdfState.dblToCol(this.rangeMin[i]);
				}
				else
				{
					if (this.rangeMax[i] < 0.0)
					{
						color.c[i] = PdfState.dblToCol(this.rangeMax[i]);
					}
					else
					{
						color.c[i] = 0;
					}
				}
			}
		}
		internal override void getDefaultRanges(double[] decodeLow, double[] decodeRange, int maxImgPixel)
		{
			this.alt.getDefaultRanges(decodeLow, decodeRange, maxImgPixel);
		}
		internal override int getNComps()
		{
			return this.nComps;
		}
		internal override enumColorSpaceMode getMode()
		{
			return enumColorSpaceMode.csICCBased;
		}
		internal static PdfColorSpaceTop parse(PdfArray arr)
		{
			return null;
		}
	}
}
