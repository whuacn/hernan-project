using System;
namespace Persits.PDF
{
	internal class PdfDeviceNColorSpace : PdfColorSpaceTop
	{
		internal int nComps;
		private string[] names = new string[32];
		private PdfColorSpaceTop alt;
		private PdfFunctionTop func;
		internal PdfDeviceNColorSpace(int nCompsA, PdfColorSpaceTop altA, PdfFunctionTop funcA)
		{
			this.nComps = nCompsA;
			this.alt = altA;
			this.func = funcA;
		}
		internal override PdfColorSpaceTop copy()
		{
			PdfDeviceNColorSpace pdfDeviceNColorSpace = new PdfDeviceNColorSpace(this.nComps, this.alt.copy(), this.func.copy());
			for (int i = 0; i < this.nComps; i++)
			{
				pdfDeviceNColorSpace.names[i] = this.names[i];
			}
			return pdfDeviceNColorSpace;
		}
		internal static PdfColorSpaceTop parse(PdfArray arr, PdfPreview pPreview)
		{
			string[] array = new string[32];
			if (arr.Size != 4 && arr.Size != 5)
			{
				AuxException.Throw("Invalid DeviceN color space.", PdfErrors._ERROR_PREVIEW_PARSE);
				return null;
			}
			PdfObject @object = arr.GetObject(1);
			if (@object.m_nType != enumType.pdfArray)
			{
				AuxException.Throw("Invalid DeviceN color space (names are missing or invalid.)", PdfErrors._ERROR_PREVIEW_PARSE);
				return null;
			}
			int num = ((PdfArray)@object).Size;
			if (num > 32)
			{
				num = 32;
			}
			for (int i = 0; i < num; i++)
			{
				PdfObject object2 = ((PdfArray)@object).GetObject(i);
				if (object2.m_nType != enumType.pdfName)
				{
					AuxException.Throw("Invalid DeviceN color space (names are missing or invalid.)", PdfErrors._ERROR_PREVIEW_PARSE);
					return null;
				}
				array[i] = ((PdfName)object2).m_bstrName;
			}
			@object = arr.GetObject(2);
			PdfColorSpaceTop pdfColorSpaceTop = PdfColorSpaceTop.parse(@object, pPreview);
			if (pdfColorSpaceTop == null)
			{
				AuxException.Throw("Invalid DeviceN color space (alternate color space is missing or invalid.)", PdfErrors._ERROR_PREVIEW_PARSE);
				return null;
			}
			@object = arr.GetObject(3);
			PdfFunctionTop pdfFunctionTop = PdfFunctionTop.parse(@object);
			if (pdfFunctionTop == null)
			{
				return null;
			}
			PdfDeviceNColorSpace pdfDeviceNColorSpace = new PdfDeviceNColorSpace(num, pdfColorSpaceTop, pdfFunctionTop);
			for (int i = 0; i < num; i++)
			{
				pdfDeviceNColorSpace.names[i] = array[i];
			}
			return pdfDeviceNColorSpace;
		}
		internal override void getGray(PdfColor color, PdfGray gray)
		{
			double[] array = new double[32];
			double[] array2 = new double[32];
			PdfColor pdfColor = new PdfColor();
			for (int i = 0; i < this.nComps; i++)
			{
				array[i] = PdfState.colToDbl(color.c[i]);
			}
			this.func.transform(array, array2);
			for (int i = 0; i < this.alt.getNComps(); i++)
			{
				pdfColor.c[i] = PdfState.dblToCol(array2[i]);
			}
			this.alt.getGray(pdfColor, gray);
		}
		internal override void getRGB(PdfColor color, PdfRGB rgb)
		{
			double[] array = new double[32];
			double[] array2 = new double[32];
			PdfColor pdfColor = new PdfColor();
			for (int i = 0; i < this.nComps; i++)
			{
				array[i] = PdfState.colToDbl(color.c[i]);
			}
			this.func.transform(array, array2);
			for (int i = 0; i < this.alt.getNComps(); i++)
			{
				pdfColor.c[i] = PdfState.dblToCol(array2[i]);
			}
			this.alt.getRGB(pdfColor, rgb);
		}
		internal override void getCMYK(PdfColor color, PdfCMYK cmyk)
		{
			double[] array = new double[32];
			double[] array2 = new double[32];
			PdfColor pdfColor = new PdfColor();
			for (int i = 0; i < this.nComps; i++)
			{
				array[i] = PdfState.colToDbl(color.c[i]);
			}
			this.func.transform(array, array2);
			for (int i = 0; i < this.alt.getNComps(); i++)
			{
				pdfColor.c[i] = PdfState.dblToCol(array2[i]);
			}
			this.alt.getCMYK(pdfColor, cmyk);
		}
		internal override void getDefaultColor(PdfColor color)
		{
			for (int i = 0; i < this.nComps; i++)
			{
				color.c[i] = 65536;
			}
		}
		internal override int getNComps()
		{
			return this.nComps;
		}
		internal override enumColorSpaceMode getMode()
		{
			return enumColorSpaceMode.csDeviceN;
		}
		internal static PdfColorSpaceTop parse(PdfArray arr)
		{
			return null;
		}
	}
}
