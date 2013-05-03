using System;
namespace Persits.PDF
{
	internal abstract class PdfFunctionTop
	{
		protected const int funcMaxInputs = 8;
		protected const int funcMaxOutputs = 32;
		protected int m;
		protected int n;
		protected double[,] domain;
		protected double[,] range;
		protected bool hasRange;
		internal PdfFunctionTop()
		{
			this.domain = new double[8, 2];
			this.range = new double[32, 2];
		}
		internal PdfFunctionTop(PdfFunctionTop func)
		{
			this.m = func.m;
			this.n = func.n;
			this.domain = new double[8, 2];
			this.range = new double[32, 2];
			Array.Copy(func.domain, this.domain, 16);
			Array.Copy(func.range, this.range, 64);
			this.hasRange = func.hasRange;
		}
		internal static PdfFunctionTop parse(PdfObject funcObj)
		{
			PdfDict pdfDict;
			if (funcObj.m_nType == enumType.pdfDictWithStream)
			{
				pdfDict = (PdfDict)funcObj;
			}
			else
			{
				if (funcObj.m_nType == enumType.pdfDictionary)
				{
					pdfDict = (PdfDict)funcObj;
				}
				else
				{
					if (funcObj.m_nType == enumType.pdfName && string.Compare(((PdfName)funcObj).m_bstrName, "Identity") == 0)
					{
						return new PdfIdentityFunction();
					}
					AuxException.Throw("Expected function dictionary or stream.", PdfErrors._ERROR_PREVIEW_PARSE);
					return null;
				}
			}
			PdfObject objectByName = pdfDict.GetObjectByName("FunctionType");
			if (objectByName == null || objectByName.m_nType != enumType.pdfNumber)
			{
				AuxException.Throw("Function type is missing or wrong type.", PdfErrors._ERROR_PREVIEW_PARSE);
				return null;
			}
			int num = (int)((PdfNumber)objectByName).m_fValue;
			PdfFunctionTop pdfFunctionTop;
			if (num == 0)
			{
				pdfFunctionTop = new PdfSampledFunction(funcObj, pdfDict);
			}
			else
			{
				if (num == 2)
				{
					pdfFunctionTop = new PdfExponentialFunction(funcObj, pdfDict);
				}
				else
				{
					if (num == 3)
					{
						pdfFunctionTop = new PdfStitchingFunction(funcObj, pdfDict);
					}
					else
					{
						if (num != 4)
						{
							AuxException.Throw("Unimplemented function type.", PdfErrors._ERROR_PREVIEW_PARSE);
							return null;
						}
						pdfFunctionTop = new PdfPostScriptFunction(funcObj, pdfDict);
					}
				}
			}
			if (!pdfFunctionTop.isOk())
			{
				return null;
			}
			return pdfFunctionTop;
		}
		internal bool init(PdfDict dict)
		{
			PdfObject objectByName = dict.GetObjectByName("Domain");
			if (objectByName == null || objectByName.m_nType != enumType.pdfArray)
			{
				AuxException.Throw("Function is missing domain.", PdfErrors._ERROR_PREVIEW_PARSE);
				return false;
			}
			this.m = ((PdfArray)objectByName).Size / 2;
			if (this.m > 8)
			{
				string err = string.Format("Functions with more than {0} inputs are unsupported.", 8);
				AuxException.Throw(err, PdfErrors._ERROR_PREVIEW_PARSE);
				return false;
			}
			for (int i = 0; i < this.m; i++)
			{
				PdfObject @object = ((PdfArray)objectByName).GetObject(2 * i);
				if (@object.m_nType != enumType.pdfNumber)
				{
					AuxException.Throw("Illegal value in function domain array.", PdfErrors._ERROR_PREVIEW_PARSE);
					return false;
				}
				this.domain[i, 0] = ((PdfNumber)@object).m_fValue;
				@object = ((PdfArray)objectByName).GetObject(2 * i + 1);
				if (@object.m_nType != enumType.pdfNumber)
				{
					AuxException.Throw("Illegal value in function domain array.", PdfErrors._ERROR_PREVIEW_PARSE);
					return false;
				}
				this.domain[i, 1] = ((PdfNumber)@object).m_fValue;
			}
			this.hasRange = false;
			this.n = 0;
			objectByName = dict.GetObjectByName("Range");
			if (objectByName != null && objectByName.m_nType == enumType.pdfArray)
			{
				this.hasRange = true;
				this.n = ((PdfArray)objectByName).Size / 2;
				if (this.n > 32)
				{
					string err = string.Format("Functions with more than {0} outputs are unsupported.", 32);
					AuxException.Throw(err, PdfErrors._ERROR_PREVIEW_PARSE);
					return false;
				}
				for (int i = 0; i < this.n; i++)
				{
					PdfObject @object = ((PdfArray)objectByName).GetObject(2 * i);
					if (@object.m_nType != enumType.pdfNumber)
					{
						AuxException.Throw("Illegal value in function range array.", PdfErrors._ERROR_PREVIEW_PARSE);
						return false;
					}
					this.range[i, 0] = ((PdfNumber)@object).m_fValue;
					@object = ((PdfArray)objectByName).GetObject(2 * i + 1);
					if (@object.m_nType != enumType.pdfNumber)
					{
						AuxException.Throw("Illegal value in function range array.", PdfErrors._ERROR_PREVIEW_PARSE);
						return false;
					}
					this.range[i, 1] = ((PdfNumber)@object).m_fValue;
				}
			}
			return true;
		}
		internal abstract PdfFunctionTop copy();
		internal int getInputSize()
		{
			return this.m;
		}
		internal int getOutputSize()
		{
			return this.n;
		}
		internal abstract void transform(double[] inValues, double[] outValues);
		internal abstract bool isOk();
	}
}
