using System;
namespace Persits.PDF
{
	internal class PdfFunctionShading : PdfShading
	{
		private double x0;
		private double y0;
		private double x1;
		private double y1;
		private double[] matrix = new double[6];
		private PdfFunctionTop[] funcs = new PdfFunctionTop[32];
		private int nFuncs;
		internal PdfFunctionShading(double x0A, double y0A, double x1A, double y1A, double[] matrixA, PdfFunctionTop[] funcsA, int nFuncsA, PdfPreview pPreview) : base(1, pPreview)
		{
			this.x0 = x0A;
			this.y0 = y0A;
			this.x1 = x1A;
			this.y1 = y1A;
			for (int i = 0; i < 6; i++)
			{
				this.matrix[i] = matrixA[i];
			}
			this.nFuncs = nFuncsA;
			for (int i = 0; i < this.nFuncs; i++)
			{
				this.funcs[i] = funcsA[i];
			}
		}
		internal PdfFunctionShading(PdfFunctionShading shading) : base(shading)
		{
			this.x0 = shading.x0;
			this.y0 = shading.y0;
			this.x1 = shading.x1;
			this.y1 = shading.y1;
			for (int i = 0; i < 6; i++)
			{
				this.matrix[i] = shading.matrix[i];
			}
			this.nFuncs = shading.nFuncs;
			for (int i = 0; i < this.nFuncs; i++)
			{
				this.funcs[i] = shading.funcs[i].copy();
			}
		}
		internal override PdfShading copy()
		{
			return new PdfFunctionShading(this);
		}
		internal static PdfFunctionShading parse(PdfDict dict, PdfPreview pPreview)
		{
			double[] array = new double[6];
			PdfFunctionTop[] array2 = new PdfFunctionTop[32];
			double x0A;
			double y0A = x0A = 0.0;
			double x1A;
			double y1A = x1A = 1.0;
			PdfObject objectByName = dict.GetObjectByName("Domain");
			if (objectByName != null && objectByName.m_nType == enumType.pdfArray && ((PdfArray)objectByName).Size == 4)
			{
				PdfObject @object = ((PdfArray)objectByName).GetObject(0);
				x0A = ((PdfNumber)@object).m_fValue;
				@object = ((PdfArray)objectByName).GetObject(1);
				y0A = ((PdfNumber)@object).m_fValue;
				@object = ((PdfArray)objectByName).GetObject(2);
				x1A = ((PdfNumber)@object).m_fValue;
				@object = ((PdfArray)objectByName).GetObject(3);
				y1A = ((PdfNumber)@object).m_fValue;
			}
			array[0] = 1.0;
			array[1] = 0.0;
			array[2] = 0.0;
			array[3] = 1.0;
			array[4] = 0.0;
			array[5] = 0.0;
			objectByName = dict.GetObjectByName("Matrix");
			if (objectByName != null && objectByName.m_nType == enumType.pdfArray && ((PdfArray)objectByName).Size == 6)
			{
				PdfObject @object = ((PdfArray)objectByName).GetObject(0);
				array[0] = ((PdfNumber)@object).m_fValue;
				@object = ((PdfArray)objectByName).GetObject(1);
				array[1] = ((PdfNumber)@object).m_fValue;
				@object = ((PdfArray)objectByName).GetObject(2);
				array[2] = ((PdfNumber)@object).m_fValue;
				@object = ((PdfArray)objectByName).GetObject(3);
				array[3] = ((PdfNumber)@object).m_fValue;
				@object = ((PdfArray)objectByName).GetObject(4);
				array[4] = ((PdfNumber)@object).m_fValue;
				@object = ((PdfArray)objectByName).GetObject(5);
				array[5] = ((PdfNumber)@object).m_fValue;
			}
			objectByName = dict.GetObjectByName("Function");
			int num;
			if (objectByName != null && objectByName.m_nType == enumType.pdfArray)
			{
				num = ((PdfArray)objectByName).Size;
				if (num > 32)
				{
					AuxException.Throw("Invalid shading Function array.", PdfErrors._ERROR_PREVIEW_PARSE);
					return null;
				}
				for (int i = 0; i < num; i++)
				{
					PdfObject @object = ((PdfArray)objectByName).GetObject(i);
					if ((array2[i] = PdfFunctionTop.parse(@object)) == null)
					{
						return null;
					}
				}
			}
			else
			{
				num = 1;
				if ((array2[0] = PdfFunctionTop.parse(objectByName)) == null)
				{
					return null;
				}
			}
			PdfFunctionShading pdfFunctionShading = new PdfFunctionShading(x0A, y0A, x1A, y1A, array, array2, num, pPreview);
			if (!pdfFunctionShading.init(dict))
			{
				return null;
			}
			return pdfFunctionShading;
		}
		internal void getDomain(ref double x0A, ref double y0A, ref double x1A, ref double y1A)
		{
			x0A = this.x0;
			y0A = this.y0;
			x1A = this.x1;
			y1A = this.y1;
		}
		internal double[] getMatrix()
		{
			return this.matrix;
		}
		internal void getColor(double x, double y, PdfColor color)
		{
			double[] array = new double[2];
			double[] array2 = new double[32];
			for (int i = 0; i < 32; i++)
			{
				array2[i] = 0.0;
			}
			array[0] = x;
			array[1] = y;
			double[] array3 = new double[32];
			for (int i = 0; i < this.nFuncs; i++)
			{
				this.funcs[i].transform(array, array3);
				Array.Copy(array3, 0, array2, i, array2.Length - i);
			}
			for (int i = 0; i < 32; i++)
			{
				color.c[i] = PdfState.dblToCol(array2[i]);
			}
		}
	}
}
