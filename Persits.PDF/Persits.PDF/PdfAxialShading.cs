using System;
namespace Persits.PDF
{
	internal class PdfAxialShading : PdfShading
	{
		private double x0;
		private double y0;
		private double x1;
		private double y1;
		private double t0;
		private double t1;
		private PdfFunctionTop[] funcs = new PdfFunctionTop[32];
		private int nFuncs;
		private bool extend0;
		private bool extend1;
		internal PdfAxialShading(double x0A, double y0A, double x1A, double y1A, double t0A, double t1A, PdfFunctionTop[] funcsA, int nFuncsA, bool extend0A, bool extend1A, PdfPreview pPreview) : base(2, pPreview)
		{
			this.x0 = x0A;
			this.y0 = y0A;
			this.x1 = x1A;
			this.y1 = y1A;
			this.t0 = t0A;
			this.t1 = t1A;
			this.nFuncs = nFuncsA;
			for (int i = 0; i < this.nFuncs; i++)
			{
				this.funcs[i] = funcsA[i];
			}
			this.extend0 = extend0A;
			this.extend1 = extend1A;
		}
		internal PdfAxialShading(PdfAxialShading shading) : base(shading)
		{
			this.x0 = shading.x0;
			this.y0 = shading.y0;
			this.x1 = shading.x1;
			this.y1 = shading.y1;
			this.t0 = shading.t0;
			this.t1 = shading.t1;
			this.nFuncs = shading.nFuncs;
			for (int i = 0; i < this.nFuncs; i++)
			{
				this.funcs[i] = shading.funcs[i].copy();
			}
			this.extend0 = shading.extend0;
			this.extend1 = shading.extend1;
		}
		internal static PdfAxialShading parse(PdfDict dict, PdfPreview pPreview)
		{
			PdfFunctionTop[] array = new PdfFunctionTop[32];
			PdfObject objectByName = dict.GetObjectByName("Coords");
			if (objectByName == null || objectByName.m_nType != enumType.pdfArray || ((PdfArray)objectByName).Size != 4)
			{
				AuxException.Throw("Missing or invalid Coords in shading dictionary.", PdfErrors._ERROR_PREVIEW_PARSE);
				return null;
			}
			PdfObject @object = ((PdfArray)objectByName).GetObject(0);
			double fValue = ((PdfNumber)@object).m_fValue;
			@object = ((PdfArray)objectByName).GetObject(1);
			double fValue2 = ((PdfNumber)@object).m_fValue;
			@object = ((PdfArray)objectByName).GetObject(2);
			double fValue3 = ((PdfNumber)@object).m_fValue;
			@object = ((PdfArray)objectByName).GetObject(3);
			double fValue4 = ((PdfNumber)@object).m_fValue;
			double t0A = 0.0;
			double t1A = 1.0;
			objectByName = dict.GetObjectByName("Domain");
			if (objectByName != null && objectByName.m_nType == enumType.pdfArray && ((PdfArray)objectByName).Size == 2)
			{
				@object = ((PdfArray)objectByName).GetObject(0);
				t0A = ((PdfNumber)@object).m_fValue;
				@object = ((PdfArray)objectByName).GetObject(1);
				t1A = ((PdfNumber)@object).m_fValue;
			}
			objectByName = dict.GetObjectByName("Function");
			int num;
			if (objectByName != null && objectByName.m_nType == enumType.pdfArray)
			{
				num = ((PdfArray)objectByName).Size;
				if (num > 32)
				{
					AuxException.Throw("Invalid Function array in shading dictionary.", PdfErrors._ERROR_PREVIEW_PARSE);
					return null;
				}
				for (int i = 0; i < num; i++)
				{
					@object = ((PdfArray)objectByName).GetObject(i);
					if ((array[i] = PdfFunctionTop.parse(@object)) == null)
					{
						return null;
					}
				}
			}
			else
			{
				num = 1;
				if ((array[0] = PdfFunctionTop.parse(objectByName)) == null)
				{
					return null;
				}
			}
			bool extend0A;
			bool extend1A = extend0A = false;
			objectByName = dict.GetObjectByName("Extend");
			if (objectByName != null && objectByName.m_nType == enumType.pdfArray && ((PdfArray)objectByName).Size == 2)
			{
				@object = ((PdfArray)objectByName).GetObject(0);
				if (@object.m_nType == enumType.pdfBool)
				{
					extend0A = ((PdfBool)@object).m_bValue;
				}
				@object = ((PdfArray)objectByName).GetObject(1);
				if (@object.m_nType == enumType.pdfBool)
				{
					extend1A = ((PdfBool)@object).m_bValue;
				}
			}
			PdfAxialShading pdfAxialShading = new PdfAxialShading(fValue, fValue2, fValue3, fValue4, t0A, t1A, array, num, extend0A, extend1A, pPreview);
			if (!pdfAxialShading.init(dict))
			{
				return null;
			}
			return pdfAxialShading;
		}
		internal void getCoords(ref double x0A, ref double y0A, ref double x1A, ref double y1A)
		{
			x0A = this.x0;
			y0A = this.y0;
			x1A = this.x1;
			y1A = this.y1;
		}
		internal double getDomain0()
		{
			return this.t0;
		}
		internal double getDomain1()
		{
			return this.t1;
		}
		internal void getColor(double t, PdfColor color)
		{
			double[] array = new double[32];
			for (int i = 0; i < 32; i++)
			{
				array[i] = 0.0;
			}
			double[] inValues = new double[]
			{
				t
			};
			double[] array2 = new double[32];
			for (int i = 0; i < this.nFuncs; i++)
			{
				this.funcs[i].transform(inValues, array2);
				Array.Copy(array2, 0, array, i, array.Length - i);
			}
			for (int i = 0; i < 32; i++)
			{
				color.c[i] = PdfState.dblToCol(array[i]);
			}
		}
		internal override PdfShading copy()
		{
			return new PdfAxialShading(this);
		}
		internal bool getExtend0()
		{
			return this.extend0;
		}
		internal bool getExtend1()
		{
			return this.extend1;
		}
	}
}
