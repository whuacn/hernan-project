using System;
namespace Persits.PDF
{
	internal class PdfImageColorMap
	{
		private PdfColorSpaceTop colorSpace;
		private int bits;
		private int nComps;
		private PdfColorSpaceTop colorSpace2;
		private int nComps2;
		private double[] decodeLow = new double[32];
		private double[] decodeRange = new double[32];
		private bool ok;
		private int[][] lookup = new int[32][];
		private int[][] lookup2 = new int[32][];
		internal PdfImageColorMap(int bitsA, PdfObject decode, PdfColorSpaceTop colorSpaceA)
		{
			double[] array = new double[32];
			double[] array2 = new double[32];
			this.ok = true;
			this.bits = bitsA;
			int num = (1 << this.bits) - 1;
			this.colorSpace = colorSpaceA;
			for (int i = 0; i < 32; i++)
			{
				this.lookup[i] = null;
				this.lookup2[i] = null;
			}
			if (decode == null)
			{
				this.nComps = this.colorSpace.getNComps();
				this.colorSpace.getDefaultRanges(this.decodeLow, this.decodeRange, num);
			}
			else
			{
				if (decode.m_nType != enumType.pdfArray)
				{
					this.ok = false;
					return;
				}
				this.nComps = ((PdfArray)decode).Size / 2;
				if (this.nComps != this.colorSpace.getNComps())
				{
					this.ok = false;
					return;
				}
				for (int j = 0; j < this.nComps; j++)
				{
					PdfObject @object = ((PdfArray)decode).GetObject(2 * j);
					if (@object.m_nType != enumType.pdfNumber)
					{
						this.ok = false;
						return;
					}
					this.decodeLow[j] = ((PdfNumber)@object).m_fValue;
					@object = ((PdfArray)decode).GetObject(2 * j + 1);
					if (@object.m_nType != enumType.pdfNumber)
					{
						this.ok = false;
						return;
					}
					this.decodeRange[j] = ((PdfNumber)@object).m_fValue - this.decodeLow[j];
				}
			}
			for (int i = 0; i < this.nComps; i++)
			{
				this.lookup[i] = new int[num + 1];
				for (int j = 0; j <= num; j++)
				{
					this.lookup[i][j] = PdfState.dblToCol(this.decodeLow[i] + (double)j * this.decodeRange[i] / (double)num);
				}
			}
			this.colorSpace2 = null;
			this.nComps2 = 0;
			if (this.colorSpace.getMode() == enumColorSpaceMode.csIndexed)
			{
				PdfIndexedColorSpace pdfIndexedColorSpace = (PdfIndexedColorSpace)this.colorSpace;
				this.colorSpace2 = pdfIndexedColorSpace.getBase();
				int indexHigh = pdfIndexedColorSpace.getIndexHigh();
				this.nComps2 = this.colorSpace2.getNComps();
				byte[] array3 = pdfIndexedColorSpace.getLookup();
				this.colorSpace2.getDefaultRanges(array, array2, indexHigh);
				for (int i = 0; i < this.nComps2; i++)
				{
					this.lookup2[i] = new int[num + 1];
				}
				for (int j = 0; j <= num; j++)
				{
					int num2 = (int)(this.decodeLow[0] + (double)j * this.decodeRange[0] / (double)num + 0.5);
					if (num2 < 0)
					{
						num2 = 0;
					}
					else
					{
						if (num2 > indexHigh)
						{
							num2 = indexHigh;
						}
					}
					for (int i = 0; i < this.nComps2; i++)
					{
						this.lookup2[i][j] = PdfState.dblToCol(array[i] + (double)array3[num2 * this.nComps2 + i] / 255.0 * array2[i]);
					}
				}
				return;
			}
			if (this.colorSpace.getMode() == enumColorSpaceMode.csSeparation)
			{
				PdfSeparationColorSpace pdfSeparationColorSpace = (PdfSeparationColorSpace)this.colorSpace;
				this.colorSpace2 = pdfSeparationColorSpace.getAlt();
				this.nComps2 = this.colorSpace2.getNComps();
				PdfFunctionTop func = pdfSeparationColorSpace.getFunc();
				for (int i = 0; i < this.nComps2; i++)
				{
					this.lookup2[i] = new int[num + 1];
				}
				for (int j = 0; j <= num; j++)
				{
					array[0] = this.decodeLow[0] + (double)j * this.decodeRange[0] / (double)num;
					func.transform(array, array2);
					for (int i = 0; i < this.nComps2; i++)
					{
						this.lookup2[i][j] = PdfState.dblToCol(array2[i]);
					}
				}
			}
		}
		private PdfImageColorMap(PdfImageColorMap colorMap)
		{
			this.colorSpace = colorMap.colorSpace.copy();
			this.bits = colorMap.bits;
			this.nComps = colorMap.nComps;
			this.nComps2 = colorMap.nComps2;
			this.colorSpace2 = null;
			for (int i = 0; i < 32; i++)
			{
				this.lookup[i] = null;
				this.lookup2[i] = null;
			}
			int num = 1 << this.bits;
			for (int i = 0; i < this.nComps; i++)
			{
				this.lookup[i] = new int[num];
				Array.Copy(colorMap.lookup[i], this.lookup[i], num);
			}
			if (this.colorSpace.getMode() == enumColorSpaceMode.csIndexed)
			{
				this.colorSpace2 = ((PdfIndexedColorSpace)this.colorSpace).getBase();
				for (int i = 0; i < this.nComps2; i++)
				{
					this.lookup2[i] = new int[num];
					Array.Copy(colorMap.lookup2[i], this.lookup2[i], num);
				}
			}
			else
			{
				if (this.colorSpace.getMode() == enumColorSpaceMode.csSeparation)
				{
					this.colorSpace2 = ((PdfSeparationColorSpace)this.colorSpace).getAlt();
					for (int i = 0; i < this.nComps2; i++)
					{
						this.lookup2[i] = new int[num];
						Array.Copy(colorMap.lookup2[i], this.lookup2[i], num);
					}
				}
			}
			for (int j = 0; j < this.nComps; j++)
			{
				this.decodeLow[j] = colorMap.decodeLow[j];
				this.decodeRange[j] = colorMap.decodeRange[j];
			}
			this.ok = true;
		}
		internal void getGray(RavenColorPtr x, PdfGray gray)
		{
			PdfColor pdfColor = new PdfColor();
			if (this.colorSpace2 != null)
			{
				for (int i = 0; i < this.nComps2; i++)
				{
					pdfColor.c[i] = this.lookup2[i][(int)x[0]];
				}
				this.colorSpace2.getGray(pdfColor, gray);
				return;
			}
			for (int i = 0; i < this.nComps; i++)
			{
				pdfColor.c[i] = this.lookup[i][(int)x[i]];
			}
			this.colorSpace.getGray(pdfColor, gray);
		}
		internal void getRGB(RavenColorPtr x, PdfRGB rgb)
		{
			PdfColor pdfColor = new PdfColor();
			if (this.colorSpace2 != null)
			{
				for (int i = 0; i < this.nComps2; i++)
				{
					pdfColor.c[i] = this.lookup2[i][(int)x[0]];
				}
				this.colorSpace2.getRGB(pdfColor, rgb);
				return;
			}
			for (int i = 0; i < this.nComps; i++)
			{
				pdfColor.c[i] = this.lookup[i][(int)x[i]];
			}
			this.colorSpace.getRGB(pdfColor, rgb);
		}
		private void getCMYK(byte[] x, PdfCMYK cmyk)
		{
			PdfColor pdfColor = new PdfColor();
			if (this.colorSpace2 != null)
			{
				for (int i = 0; i < this.nComps2; i++)
				{
					pdfColor.c[i] = this.lookup2[i][(int)x[0]];
				}
				this.colorSpace2.getCMYK(pdfColor, cmyk);
				return;
			}
			for (int i = 0; i < this.nComps; i++)
			{
				pdfColor.c[i] = this.lookup[i][(int)x[i]];
			}
			this.colorSpace.getCMYK(pdfColor, cmyk);
		}
		internal void getColor(byte[] x, PdfColor color)
		{
			int num = (1 << this.bits) - 1;
			for (int i = 0; i < this.nComps; i++)
			{
				color.c[i] = PdfState.dblToCol(this.decodeLow[i] + (double)x[i] * this.decodeRange[i] / (double)num);
			}
		}
		internal PdfColorSpaceTop getColorSpace()
		{
			return this.colorSpace;
		}
		internal int getBits()
		{
			return this.bits;
		}
		internal int getNumPixelComps()
		{
			return this.nComps;
		}
		internal bool isOk()
		{
			return this.ok;
		}
	}
}
