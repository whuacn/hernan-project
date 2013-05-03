using System;
namespace Persits.PDF
{
	internal class PdfCalRGBColorSpace : PdfColorSpaceTop
	{
		private double whiteX;
		private double whiteY;
		private double whiteZ;
		private double blackX;
		private double blackY;
		private double blackZ;
		private double gammaR;
		private double gammaG;
		private double gammaB;
		private double[] mat = new double[9];
		internal PdfCalRGBColorSpace()
		{
			this.whiteX = (this.whiteY = (this.whiteZ = 1.0));
			this.blackX = (this.blackY = (this.blackZ = 0.0));
			this.gammaR = (this.gammaG = (this.gammaB = 1.0));
			this.mat[0] = 1.0;
			this.mat[1] = 0.0;
			this.mat[2] = 0.0;
			this.mat[3] = 0.0;
			this.mat[4] = 1.0;
			this.mat[5] = 0.0;
			this.mat[6] = 0.0;
			this.mat[7] = 0.0;
			this.mat[8] = 1.0;
		}
		internal override PdfColorSpaceTop copy()
		{
			PdfCalRGBColorSpace pdfCalRGBColorSpace = new PdfCalRGBColorSpace();
			pdfCalRGBColorSpace.whiteX = this.whiteX;
			pdfCalRGBColorSpace.whiteY = this.whiteY;
			pdfCalRGBColorSpace.whiteZ = this.whiteZ;
			pdfCalRGBColorSpace.blackX = this.blackX;
			pdfCalRGBColorSpace.blackY = this.blackY;
			pdfCalRGBColorSpace.blackZ = this.blackZ;
			pdfCalRGBColorSpace.gammaR = this.gammaR;
			pdfCalRGBColorSpace.gammaG = this.gammaG;
			pdfCalRGBColorSpace.gammaB = this.gammaB;
			for (int i = 0; i < 9; i++)
			{
				pdfCalRGBColorSpace.mat[i] = this.mat[i];
			}
			return pdfCalRGBColorSpace;
		}
		internal static PdfColorSpaceTop parse(PdfArray arr)
		{
			PdfObject @object = arr.GetObject(1);
			if (@object == null || @object.m_nType != enumType.pdfDictionary)
			{
				AuxException.Throw("Invalid CalRGB color space.", PdfErrors._ERROR_PREVIEW_PARSE);
				return null;
			}
			PdfCalRGBColorSpace pdfCalRGBColorSpace = new PdfCalRGBColorSpace();
			PdfObject objectByName = ((PdfDict)@object).GetObjectByName("WhitePoint");
			if (objectByName != null && objectByName.m_nType == enumType.pdfArray && ((PdfArray)objectByName).Size == 3)
			{
				PdfObject object2 = ((PdfArray)objectByName).GetObject(0);
				if (object2.m_nType == enumType.pdfNumber)
				{
					pdfCalRGBColorSpace.whiteX = ((PdfNumber)object2).m_fValue;
				}
				object2 = ((PdfArray)objectByName).GetObject(1);
				if (object2.m_nType == enumType.pdfNumber)
				{
					pdfCalRGBColorSpace.whiteY = ((PdfNumber)object2).m_fValue;
				}
				object2 = ((PdfArray)objectByName).GetObject(2);
				if (object2.m_nType == enumType.pdfNumber)
				{
					pdfCalRGBColorSpace.whiteZ = ((PdfNumber)object2).m_fValue;
				}
			}
			objectByName = ((PdfDict)@object).GetObjectByName("BlackPoint");
			if (objectByName != null && objectByName.m_nType == enumType.pdfArray && ((PdfArray)objectByName).Size == 3)
			{
				PdfObject object2 = ((PdfArray)objectByName).GetObject(0);
				if (object2.m_nType == enumType.pdfNumber)
				{
					pdfCalRGBColorSpace.blackX = ((PdfNumber)object2).m_fValue;
				}
				object2 = ((PdfArray)objectByName).GetObject(1);
				if (object2.m_nType == enumType.pdfNumber)
				{
					pdfCalRGBColorSpace.blackY = ((PdfNumber)object2).m_fValue;
				}
				object2 = ((PdfArray)objectByName).GetObject(2);
				if (object2.m_nType == enumType.pdfNumber)
				{
					pdfCalRGBColorSpace.blackZ = ((PdfNumber)object2).m_fValue;
				}
			}
			objectByName = ((PdfDict)@object).GetObjectByName("Gamma");
			if (objectByName != null && objectByName.m_nType == enumType.pdfArray && ((PdfArray)objectByName).Size == 3)
			{
				PdfObject object2 = ((PdfArray)objectByName).GetObject(0);
				if (object2.m_nType == enumType.pdfNumber)
				{
					pdfCalRGBColorSpace.gammaR = ((PdfNumber)object2).m_fValue;
				}
				object2 = ((PdfArray)objectByName).GetObject(1);
				if (object2.m_nType == enumType.pdfNumber)
				{
					pdfCalRGBColorSpace.gammaG = ((PdfNumber)object2).m_fValue;
				}
				object2 = ((PdfArray)objectByName).GetObject(2);
				if (object2.m_nType == enumType.pdfNumber)
				{
					pdfCalRGBColorSpace.gammaB = ((PdfNumber)object2).m_fValue;
				}
			}
			objectByName = ((PdfDict)@object).GetObjectByName("Matrix");
			if (objectByName != null && objectByName.m_nType == enumType.pdfArray && ((PdfArray)objectByName).Size == 9)
			{
				for (int i = 0; i < 9; i++)
				{
					PdfObject object2 = ((PdfArray)objectByName).GetObject(i);
					pdfCalRGBColorSpace.mat[i] = ((PdfNumber)object2).m_fValue;
				}
			}
			return pdfCalRGBColorSpace;
		}
		internal override void getGray(PdfColor color, PdfGray gray)
		{
			gray.g = PdfState.clip01((int)(0.299 * (double)color.c[0] + 0.587 * (double)color.c[1] + 0.114 * (double)color.c[2] + 0.5));
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
		internal override int getNComps()
		{
			return 3;
		}
		internal override enumColorSpaceMode getMode()
		{
			return enumColorSpaceMode.csCalRGB;
		}
	}
}
