using System;
namespace Persits.PDF
{
	internal class PdfCalGrayColorSpace : PdfColorSpaceTop
	{
		private double whiteX;
		private double whiteY;
		private double whiteZ;
		private double blackX;
		private double blackY;
		private double blackZ;
		private double gamma;
		internal PdfCalGrayColorSpace()
		{
			this.whiteX = (this.whiteY = (this.whiteZ = 1.0));
			this.blackX = (this.blackY = (this.blackZ = 0.0));
			this.gamma = 1.0;
		}
		internal static PdfColorSpaceTop parse(PdfArray arr)
		{
			PdfObject @object = arr.GetObject(1);
			if (@object == null || @object.m_nType != enumType.pdfDictionary)
			{
				AuxException.Throw("Invalid CalGray color space.", PdfErrors._ERROR_PREVIEW_PARSE);
				return null;
			}
			PdfCalGrayColorSpace pdfCalGrayColorSpace = new PdfCalGrayColorSpace();
			PdfObject objectByName = ((PdfDict)@object).GetObjectByName("WhitePoint");
			if (objectByName != null && objectByName.m_nType == enumType.pdfArray && ((PdfArray)objectByName).Size == 3)
			{
				PdfObject object2 = ((PdfArray)objectByName).GetObject(0);
				if (object2.m_nType == enumType.pdfNumber)
				{
					pdfCalGrayColorSpace.whiteX = ((PdfNumber)object2).m_fValue;
				}
				object2 = ((PdfArray)objectByName).GetObject(1);
				if (object2.m_nType == enumType.pdfNumber)
				{
					pdfCalGrayColorSpace.whiteY = ((PdfNumber)object2).m_fValue;
				}
				object2 = ((PdfArray)objectByName).GetObject(2);
				if (object2.m_nType == enumType.pdfNumber)
				{
					pdfCalGrayColorSpace.whiteZ = ((PdfNumber)object2).m_fValue;
				}
			}
			objectByName = ((PdfDict)@object).GetObjectByName("BlackPoint");
			if (objectByName != null && objectByName.m_nType == enumType.pdfArray && ((PdfArray)objectByName).Size == 3)
			{
				PdfObject object2 = ((PdfArray)objectByName).GetObject(0);
				if (object2.m_nType == enumType.pdfNumber)
				{
					pdfCalGrayColorSpace.blackX = ((PdfNumber)object2).m_fValue;
				}
				object2 = ((PdfArray)objectByName).GetObject(1);
				if (object2.m_nType == enumType.pdfNumber)
				{
					pdfCalGrayColorSpace.blackY = ((PdfNumber)object2).m_fValue;
				}
				object2 = ((PdfArray)objectByName).GetObject(2);
				if (object2.m_nType == enumType.pdfNumber)
				{
					pdfCalGrayColorSpace.blackZ = ((PdfNumber)object2).m_fValue;
				}
			}
			objectByName = ((PdfDict)@object).GetObjectByName("Gamma");
			if (objectByName != null && objectByName.m_nType == enumType.pdfNumber)
			{
				pdfCalGrayColorSpace.gamma = ((PdfNumber)objectByName).m_fValue;
			}
			return pdfCalGrayColorSpace;
		}
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
			return new PdfCalGrayColorSpace
			{
				whiteX = this.whiteX,
				whiteY = this.whiteY,
				whiteZ = this.whiteZ,
				blackX = this.blackX,
				blackY = this.blackY,
				blackZ = this.blackZ,
				gamma = this.gamma
			};
		}
		internal override int getNComps()
		{
			return 1;
		}
		internal override enumColorSpaceMode getMode()
		{
			return enumColorSpaceMode.csCalGray;
		}
	}
}
