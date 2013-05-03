using System;
namespace Persits.PDF
{
	internal class PdfExponentialFunction : PdfFunctionTop
	{
		private double[] c0 = new double[32];
		private double[] c1 = new double[32];
		private double e;
		private bool ok;
		internal PdfExponentialFunction(PdfObject funcObj, PdfDict dict)
		{
			this.ok = false;
			if (!base.init(dict))
			{
				return;
			}
			if (this.m != 1)
			{
				AuxException.Throw("Exponential function with more than one input.", PdfErrors._ERROR_PREVIEW_PARSE);
				return;
			}
			PdfObject objectByName = dict.GetObjectByName("C0");
			if (objectByName != null && objectByName.m_nType == enumType.pdfArray)
			{
				if (this.hasRange && ((PdfArray)objectByName).Size != this.n)
				{
					AuxException.Throw("Function's C0 array is wrong length.", PdfErrors._ERROR_PREVIEW_PARSE);
					return;
				}
				this.n = ((PdfArray)objectByName).Size;
				for (int i = 0; i < this.n; i++)
				{
					PdfObject @object = ((PdfArray)objectByName).GetObject(i);
					if (@object.m_nType != enumType.pdfNumber)
					{
						AuxException.Throw("Illegal value in function C0 array.", PdfErrors._ERROR_PREVIEW_PARSE);
						return;
					}
					this.c0[i] = ((PdfNumber)@object).m_fValue;
				}
			}
			else
			{
				if (this.hasRange && this.n != 1)
				{
					AuxException.Throw("Function's C0 array is wrong length.", PdfErrors._ERROR_PREVIEW_PARSE);
					return;
				}
				this.n = 1;
				this.c0[0] = 0.0;
			}
			objectByName = dict.GetObjectByName("C1");
			if (objectByName != null && objectByName.m_nType == enumType.pdfArray)
			{
				if (((PdfArray)objectByName).Size != this.n)
				{
					AuxException.Throw("Function's C1 array is wrong length.", PdfErrors._ERROR_PREVIEW_PARSE);
					return;
				}
				for (int i = 0; i < this.n; i++)
				{
					PdfObject @object = ((PdfArray)objectByName).GetObject(i);
					if (@object.m_nType != enumType.pdfNumber)
					{
						AuxException.Throw("Illegal value in function C1 array.", PdfErrors._ERROR_PREVIEW_PARSE);
						return;
					}
					this.c1[i] = ((PdfNumber)@object).m_fValue;
				}
			}
			else
			{
				if (this.n != 1)
				{
					AuxException.Throw("Function's C1 array is wrong length.", PdfErrors._ERROR_PREVIEW_PARSE);
					return;
				}
				this.c1[0] = 1.0;
			}
			objectByName = dict.GetObjectByName("N");
			if (objectByName == null || objectByName.m_nType != enumType.pdfNumber)
			{
				AuxException.Throw("Function has missing or invalid N.", PdfErrors._ERROR_PREVIEW_PARSE);
				return;
			}
			this.e = ((PdfNumber)objectByName).m_fValue;
			this.ok = true;
		}
		internal PdfExponentialFunction(PdfExponentialFunction func) : base(func)
		{
			this.e = func.e;
			Array.Copy(func.c0, this.c0, func.c0.Length);
			Array.Copy(func.c1, this.c1, func.c1.Length);
			this.ok = func.ok;
		}
		internal override void transform(double[] inv, double[] outv)
		{
			double x;
			if (inv[0] < this.domain[0, 0])
			{
				x = this.domain[0, 0];
			}
			else
			{
				if (inv[0] > this.domain[0, 1])
				{
					x = this.domain[0, 1];
				}
				else
				{
					x = inv[0];
				}
			}
			for (int i = 0; i < this.n; i++)
			{
				outv[i] = this.c0[i] + Math.Pow(x, this.e) * (this.c1[i] - this.c0[i]);
				if (this.hasRange)
				{
					if (outv[i] < this.range[i, 0])
					{
						outv[i] = this.range[i, 0];
					}
					else
					{
						if (outv[i] > this.range[i, 1])
						{
							outv[i] = this.range[i, 1];
						}
					}
				}
			}
		}
		internal override bool isOk()
		{
			return this.ok;
		}
		internal override PdfFunctionTop copy()
		{
			return new PdfExponentialFunction(this);
		}
	}
}
