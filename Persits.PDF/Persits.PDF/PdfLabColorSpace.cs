using System;
namespace Persits.PDF
{
	internal class PdfLabColorSpace : PdfColorSpaceTop
	{
		private double whiteX;
		private double whiteY;
		private double whiteZ;
		private double blackX;
		private double blackY;
		private double blackZ;
		private double aMin;
		private double aMax;
		private double bMin;
		private double bMax;
		private double kr;
		private double kg;
		private double kb;
		private static double[,] xyzrgb = new double[,]
		{

			{
				3.240449,
				-1.537136,
				-0.498531
			},

			{
				-0.969265,
				1.876011,
				0.041556
			},

			{
				0.055643,
				-0.204026,
				1.057229
			}
		};
		internal PdfLabColorSpace()
		{
			this.whiteX = (this.whiteY = (this.whiteZ = 1.0));
			this.blackX = (this.blackY = (this.blackZ = 0.0));
			this.aMin = (this.bMin = -100.0);
			this.aMax = (this.bMax = 100.0);
		}
		internal override PdfColorSpaceTop copy()
		{
			return new PdfLabColorSpace
			{
				whiteX = this.whiteX,
				whiteY = this.whiteY,
				whiteZ = this.whiteZ,
				blackX = this.blackX,
				blackY = this.blackY,
				blackZ = this.blackZ,
				aMin = this.aMin,
				aMax = this.aMax,
				bMin = this.bMin,
				bMax = this.bMax,
				kr = this.kr,
				kg = this.kg,
				kb = this.kb
			};
		}
		internal static PdfColorSpaceTop parse(PdfArray arr)
		{
			PdfObject @object = arr.GetObject(1);
			if (@object.m_nType != enumType.pdfDictionary)
			{
				AuxException.Throw("Invalid Lab color space.", PdfErrors._ERROR_PREVIEW_PARSE);
				return null;
			}
			PdfLabColorSpace pdfLabColorSpace = new PdfLabColorSpace();
			PdfObject objectByName = ((PdfDict)@object).GetObjectByName("WhitePoint");
			if (objectByName != null && objectByName.m_nType == enumType.pdfArray && ((PdfArray)objectByName).Size == 3)
			{
				PdfObject object2 = ((PdfArray)objectByName).GetObject(0);
				if (object2.m_nType == enumType.pdfNumber)
				{
					pdfLabColorSpace.whiteX = ((PdfNumber)object2).m_fValue;
				}
				object2 = ((PdfArray)objectByName).GetObject(1);
				if (object2.m_nType == enumType.pdfNumber)
				{
					pdfLabColorSpace.whiteY = ((PdfNumber)object2).m_fValue;
				}
				object2 = ((PdfArray)objectByName).GetObject(2);
				if (object2.m_nType == enumType.pdfNumber)
				{
					pdfLabColorSpace.whiteZ = ((PdfNumber)object2).m_fValue;
				}
			}
			objectByName = ((PdfDict)@object).GetObjectByName("BlackPoint");
			if (objectByName != null && objectByName.m_nType == enumType.pdfArray && ((PdfArray)objectByName).Size == 3)
			{
				PdfObject object2 = ((PdfArray)objectByName).GetObject(0);
				if (object2.m_nType == enumType.pdfNumber)
				{
					pdfLabColorSpace.blackX = ((PdfNumber)object2).m_fValue;
				}
				object2 = ((PdfArray)objectByName).GetObject(1);
				if (object2.m_nType == enumType.pdfNumber)
				{
					pdfLabColorSpace.blackY = ((PdfNumber)object2).m_fValue;
				}
				object2 = ((PdfArray)objectByName).GetObject(2);
				if (object2.m_nType == enumType.pdfNumber)
				{
					pdfLabColorSpace.blackZ = ((PdfNumber)object2).m_fValue;
				}
			}
			objectByName = ((PdfDict)@object).GetObjectByName("Range");
			if (objectByName != null && objectByName.m_nType == enumType.pdfArray && ((PdfArray)objectByName).Size == 4)
			{
				PdfObject object2 = ((PdfArray)objectByName).GetObject(0);
				if (object2.m_nType == enumType.pdfNumber)
				{
					pdfLabColorSpace.aMin = ((PdfNumber)object2).m_fValue;
				}
				object2 = ((PdfArray)objectByName).GetObject(1);
				if (object2.m_nType == enumType.pdfNumber)
				{
					pdfLabColorSpace.aMax = ((PdfNumber)object2).m_fValue;
				}
				object2 = ((PdfArray)objectByName).GetObject(2);
				if (object2.m_nType == enumType.pdfNumber)
				{
					pdfLabColorSpace.bMin = ((PdfNumber)object2).m_fValue;
				}
				object2 = ((PdfArray)objectByName).GetObject(3);
				if (object2.m_nType == enumType.pdfNumber)
				{
					pdfLabColorSpace.bMax = ((PdfNumber)object2).m_fValue;
				}
			}
			pdfLabColorSpace.kr = 1.0 / (PdfLabColorSpace.xyzrgb[0, 0] * pdfLabColorSpace.whiteX + PdfLabColorSpace.xyzrgb[0, 1] * pdfLabColorSpace.whiteY + PdfLabColorSpace.xyzrgb[0, 2] * pdfLabColorSpace.whiteZ);
			pdfLabColorSpace.kg = 1.0 / (PdfLabColorSpace.xyzrgb[1, 0] * pdfLabColorSpace.whiteX + PdfLabColorSpace.xyzrgb[1, 1] * pdfLabColorSpace.whiteY + PdfLabColorSpace.xyzrgb[1, 2] * pdfLabColorSpace.whiteZ);
			pdfLabColorSpace.kb = 1.0 / (PdfLabColorSpace.xyzrgb[2, 0] * pdfLabColorSpace.whiteX + PdfLabColorSpace.xyzrgb[2, 1] * pdfLabColorSpace.whiteY + PdfLabColorSpace.xyzrgb[2, 2] * pdfLabColorSpace.whiteZ);
			return pdfLabColorSpace;
		}
		internal override void getGray(PdfColor color, PdfGray gray)
		{
			PdfRGB pdfRGB = new PdfRGB();
			this.getRGB(color, pdfRGB);
			gray.g = PdfState.clip01((int)(0.299 * (double)pdfRGB.r + 0.587 * (double)pdfRGB.g + 0.114 * (double)pdfRGB.b + 0.5));
		}
		internal override void getRGB(PdfColor color, PdfRGB rgb)
		{
			double num = (PdfState.colToDbl(color.c[0]) + 16.0) / 116.0;
			double num2 = num + PdfState.colToDbl(color.c[1]) / 500.0;
			double num3;
			if (num2 >= 0.20689655172413793)
			{
				num3 = num2 * num2 * num2;
			}
			else
			{
				num3 = 0.12841854934601665 * (num2 - 0.13793103448275862);
			}
			num3 *= this.whiteX;
			double num4;
			if (num >= 0.20689655172413793)
			{
				num4 = num * num * num;
			}
			else
			{
				num4 = 0.12841854934601665 * (num - 0.13793103448275862);
			}
			num4 *= this.whiteY;
			num2 = num - PdfState.colToDbl(color.c[2]) / 200.0;
			double num5;
			if (num2 >= 0.20689655172413793)
			{
				num5 = num2 * num2 * num2;
			}
			else
			{
				num5 = 0.12841854934601665 * (num2 - 0.13793103448275862);
			}
			num5 *= this.whiteZ;
			double num6 = PdfLabColorSpace.xyzrgb[0, 0] * num3 + PdfLabColorSpace.xyzrgb[0, 1] * num4 + PdfLabColorSpace.xyzrgb[0, 2] * num5;
			double num7 = PdfLabColorSpace.xyzrgb[1, 0] * num3 + PdfLabColorSpace.xyzrgb[1, 1] * num4 + PdfLabColorSpace.xyzrgb[1, 2] * num5;
			double num8 = PdfLabColorSpace.xyzrgb[2, 0] * num3 + PdfLabColorSpace.xyzrgb[2, 1] * num4 + PdfLabColorSpace.xyzrgb[2, 2] * num5;
			rgb.r = PdfState.dblToCol(Math.Pow(PdfState.clip01(num6 * this.kr), 0.5));
			rgb.g = PdfState.dblToCol(Math.Pow(PdfState.clip01(num7 * this.kg), 0.5));
			rgb.b = PdfState.dblToCol(Math.Pow(PdfState.clip01(num8 * this.kb), 0.5));
		}
		internal override void getCMYK(PdfColor color, PdfCMYK cmyk)
		{
			PdfRGB pdfRGB = new PdfRGB();
			this.getRGB(color, pdfRGB);
			double num = (double)PdfState.clip01(1 - pdfRGB.r);
			double num2 = (double)PdfState.clip01(1 - pdfRGB.g);
			double num3 = (double)PdfState.clip01(1 - pdfRGB.b);
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
			if (this.aMin > 0.0)
			{
				color.c[1] = PdfState.dblToCol(this.aMin);
			}
			else
			{
				if (this.aMax < 0.0)
				{
					color.c[1] = PdfState.dblToCol(this.aMax);
				}
				else
				{
					color.c[1] = 0;
				}
			}
			if (this.bMin > 0.0)
			{
				color.c[2] = PdfState.dblToCol(this.bMin);
				return;
			}
			if (this.bMax < 0.0)
			{
				color.c[2] = PdfState.dblToCol(this.bMax);
				return;
			}
			color.c[2] = 0;
		}
		internal override void getDefaultRanges(double[] decodeLow, double[] decodeRange, int maxImgPixel)
		{
			decodeLow[0] = 0.0;
			decodeRange[0] = 100.0;
			decodeLow[1] = this.aMin;
			decodeRange[1] = this.aMax - this.aMin;
			decodeLow[2] = this.bMin;
			decodeRange[2] = this.bMax - this.bMin;
		}
		internal override int getNComps()
		{
			return 3;
		}
		internal override enumColorSpaceMode getMode()
		{
			return enumColorSpaceMode.csLab;
		}
	}
}
