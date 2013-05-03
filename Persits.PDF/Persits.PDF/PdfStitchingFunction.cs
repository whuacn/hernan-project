using System;
namespace Persits.PDF
{
	internal class PdfStitchingFunction : PdfFunctionTop
	{
		private int k;
		private PdfFunctionTop[] funcs;
		private double[] bounds;
		private double[] encode;
		private double[] scale;
		private bool ok;
		internal PdfStitchingFunction(PdfObject funcObj, PdfDict dict)
		{
			this.ok = false;
			this.funcs = null;
			this.bounds = null;
			this.encode = null;
			this.scale = null;
			if (!base.init(dict))
			{
				return;
			}
			if (this.m != 1)
			{
				AuxException.Throw("Stitching function with more than one input.", PdfErrors._ERROR_PREVIEW_PARSE);
				return;
			}
			PdfObject objectByName = dict.GetObjectByName("Functions");
			if (objectByName == null || objectByName.m_nType != enumType.pdfArray)
			{
				AuxException.Throw("Missing 'Functions' entry in stitching function.", PdfErrors._ERROR_PREVIEW_PARSE);
				return;
			}
			this.k = ((PdfArray)objectByName).Size;
			this.funcs = new PdfFunctionTop[this.k];
			this.bounds = new double[this.k + 1];
			this.encode = new double[2 * this.k];
			this.scale = new double[this.k];
			for (int i = 0; i < this.k; i++)
			{
				this.funcs[i] = null;
			}
			for (int i = 0; i < this.k; i++)
			{
				if ((this.funcs[i] = PdfFunctionTop.parse(((PdfArray)objectByName).GetObject(i))) == null)
				{
					return;
				}
				if (i > 0 && (this.funcs[i].getInputSize() != 1 || this.funcs[i].getOutputSize() != this.funcs[0].getOutputSize()))
				{
					AuxException.Throw("Incompatible subfunctions in stitching function.", PdfErrors._ERROR_PREVIEW_PARSE);
					return;
				}
			}
			objectByName = dict.GetObjectByName("Bounds");
			if (objectByName == null || objectByName.m_nType != enumType.pdfArray || ((PdfArray)objectByName).Size != this.k - 1)
			{
				AuxException.Throw("Missing or invalid 'Bounds' entry in stitching function.", PdfErrors._ERROR_PREVIEW_PARSE);
				return;
			}
			this.bounds[0] = this.domain[0, 0];
			for (int i = 1; i < this.k; i++)
			{
				PdfObject @object = ((PdfArray)objectByName).GetObject(i - 1);
				if (@object.m_nType != enumType.pdfNumber)
				{
					AuxException.Throw("Invalid type in 'Bounds' array in stitching function.", PdfErrors._ERROR_PREVIEW_PARSE);
					return;
				}
				this.bounds[i] = ((PdfNumber)@object).m_fValue;
			}
			this.bounds[this.k] = this.domain[0, 1];
			objectByName = dict.GetObjectByName("Encode");
			if (objectByName == null || objectByName.m_nType != enumType.pdfArray || ((PdfArray)objectByName).Size != 2 * this.k)
			{
				AuxException.Throw("Missing or invalid 'Encode' entry in stitching function.", PdfErrors._ERROR_PREVIEW_PARSE);
				return;
			}
			for (int i = 0; i < 2 * this.k; i++)
			{
				PdfObject @object = ((PdfArray)objectByName).GetObject(i);
				if (@object.m_nType != enumType.pdfNumber)
				{
					AuxException.Throw("Invalid type in 'Encode' array in stitching function.", PdfErrors._ERROR_PREVIEW_PARSE);
					return;
				}
				this.encode[i] = ((PdfNumber)@object).m_fValue;
			}
			for (int i = 0; i < this.k; i++)
			{
				if (this.bounds[i] == this.bounds[i + 1])
				{
					this.scale[i] = 0.0;
				}
				else
				{
					this.scale[i] = (this.encode[2 * i + 1] - this.encode[2 * i]) / (this.bounds[i + 1] - this.bounds[i]);
				}
			}
			this.ok = true;
		}
		internal PdfStitchingFunction(PdfStitchingFunction func) : base(func)
		{
			this.k = func.k;
			this.funcs = new PdfFunctionTop[this.k];
			for (int i = 0; i < this.k; i++)
			{
				this.funcs[i] = func.funcs[i].copy();
			}
			this.bounds = new double[this.k + 1];
			Array.Copy(func.bounds, this.bounds, this.bounds.Length);
			this.encode = new double[2 * this.k];
			Array.Copy(func.encode, this.encode, this.encode.Length);
			this.scale = new double[this.k];
			Array.Copy(func.scale, this.scale, this.scale.Length);
			this.ok = true;
		}
		internal override void transform(double[] inv, double[] outv)
		{
			double num;
			if (inv[0] < this.domain[0, 0])
			{
				num = this.domain[0, 0];
			}
			else
			{
				if (inv[0] > this.domain[0, 1])
				{
					num = this.domain[0, 1];
				}
				else
				{
					num = inv[0];
				}
			}
			int num2 = 0;
			while (num2 < this.k - 1 && num >= this.bounds[num2 + 1])
			{
				num2++;
			}
			num = this.encode[2 * num2] + (num - this.bounds[num2]) * this.scale[num2];
			this.funcs[num2].transform(new double[]
			{
				num
			}, outv);
		}
		internal override bool isOk()
		{
			return this.ok;
		}
		internal override PdfFunctionTop copy()
		{
			return new PdfStitchingFunction(this);
		}
	}
}
