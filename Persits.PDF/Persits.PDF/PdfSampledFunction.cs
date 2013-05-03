using System;
namespace Persits.PDF
{
	internal class PdfSampledFunction : PdfFunctionTop
	{
		private bool ok;
		private double[] samples;
		private int[] sampleSize = new int[8];
		private double[,] encode = new double[8, 2];
		private double[,] decode = new double[32, 2];
		internal PdfSampledFunction(PdfObject funcObj, PdfDict dict)
		{
			this.samples = null;
			this.ok = false;
			if (!base.init(dict))
			{
				return;
			}
			if (!this.hasRange)
			{
				AuxException.Throw("Type 0 function is missing range.", PdfErrors._ERROR_PREVIEW_PARSE);
				return;
			}
			if (funcObj.m_nType != enumType.pdfDictWithStream)
			{
				AuxException.Throw("Type 0 function isn't a stream.", PdfErrors._ERROR_PREVIEW_PARSE);
				return;
			}
			PdfDictWithStream pdfDictWithStream = (PdfDictWithStream)funcObj;
			PdfObject objectByName = dict.GetObjectByName("Size");
			if (objectByName == null || objectByName.m_nType != enumType.pdfArray || ((PdfArray)objectByName).Size != this.m)
			{
				AuxException.Throw("Function has missing or invalid size array.", PdfErrors._ERROR_PREVIEW_PARSE);
				return;
			}
			for (int i = 0; i < this.m; i++)
			{
				PdfObject @object = ((PdfArray)objectByName).GetObject(i);
				if (@object.m_nType != enumType.pdfNumber)
				{
					AuxException.Throw("Illegal value in function size array.", PdfErrors._ERROR_PREVIEW_PARSE);
					return;
				}
				this.sampleSize[i] = (int)((PdfNumber)@object).m_fValue;
			}
			objectByName = dict.GetObjectByName("BitsPerSample");
			if (objectByName == null || objectByName.m_nType != enumType.pdfNumber)
			{
				AuxException.Throw("Function has missing or invalid BitsPerSample.", PdfErrors._ERROR_PREVIEW_PARSE);
				return;
			}
			int num = (int)((PdfNumber)objectByName).m_fValue;
			double num2 = 1.0 / (Math.Pow(2.0, (double)num) - 1.0);
			objectByName = dict.GetObjectByName("Encode");
			if (objectByName != null && objectByName.m_nType == enumType.pdfArray && ((PdfArray)objectByName).Size == 2 * this.m)
			{
				for (int i = 0; i < this.m; i++)
				{
					PdfObject @object = ((PdfArray)objectByName).GetObject(2 * i);
					if (@object.m_nType != enumType.pdfNumber)
					{
						AuxException.Throw("Illegal value in function encode array.", PdfErrors._ERROR_PREVIEW_PARSE);
						return;
					}
					this.encode[i, 0] = ((PdfNumber)@object).m_fValue;
					@object = ((PdfArray)objectByName).GetObject(2 * i + 1);
					if (@object.m_nType != enumType.pdfNumber)
					{
						AuxException.Throw("Illegal value in function encode array.", PdfErrors._ERROR_PREVIEW_PARSE);
						return;
					}
					this.encode[i, 1] = ((PdfNumber)@object).m_fValue;
				}
			}
			else
			{
				for (int i = 0; i < this.m; i++)
				{
					this.encode[i, 0] = 0.0;
					this.encode[i, 1] = (double)(this.sampleSize[i] - 1);
				}
			}
			objectByName = dict.GetObjectByName("Decode");
			if (objectByName != null && objectByName.m_nType == enumType.pdfArray && ((PdfArray)objectByName).Size == 2 * this.n)
			{
				for (int i = 0; i < this.n; i++)
				{
					PdfObject @object = ((PdfArray)objectByName).GetObject(2 * i);
					if (@object.m_nType != enumType.pdfNumber)
					{
						AuxException.Throw("Illegal value in function encode array.", PdfErrors._ERROR_PREVIEW_PARSE);
						return;
					}
					this.decode[i, 0] = ((PdfNumber)@object).m_fValue;
					@object = ((PdfArray)objectByName).GetObject(2 * i + 1);
					if (@object.m_nType != enumType.pdfNumber)
					{
						AuxException.Throw("Illegal value in function decode array.", PdfErrors._ERROR_PREVIEW_PARSE);
						return;
					}
					this.decode[i, 1] = ((PdfNumber)@object).m_fValue;
				}
			}
			else
			{
				for (int i = 0; i < this.n; i++)
				{
					this.decode[i, 0] = this.range[i, 0];
					this.decode[i, 1] = this.range[i, 1];
				}
			}
			int num3 = this.n;
			for (int i = 0; i < this.m; i++)
			{
				num3 *= this.sampleSize[i];
			}
			this.samples = new double[num3];
			uint num4 = 0u;
			int j = 0;
			uint num5 = (1u << num) - 1u;
			pdfDictWithStream.reset();
			for (int i = 0; i < num3; i++)
			{
				ulong num6;
				if (num == 8)
				{
					num6 = (ulong)((long)pdfDictWithStream.getChar());
				}
				else
				{
					if (num == 16)
					{
						num6 = (ulong)((long)pdfDictWithStream.getChar());
						num6 = (num6 << 8) + (ulong)((long)pdfDictWithStream.getChar());
					}
					else
					{
						if (num == 32)
						{
							num6 = (ulong)((long)pdfDictWithStream.getChar());
							num6 = (num6 << 8) + (ulong)((long)pdfDictWithStream.getChar());
							num6 = (num6 << 8) + (ulong)((long)pdfDictWithStream.getChar());
							num6 = (num6 << 8) + (ulong)((long)pdfDictWithStream.getChar());
						}
						else
						{
							while (j < num)
							{
								num4 = (num4 << 8 | (uint)(pdfDictWithStream.getChar() & 255));
								j += 8;
							}
							num6 = (ulong)(num4 >> j - num & num5);
							j -= num;
						}
					}
				}
				this.samples[i] = num6 * num2;
			}
			this.ok = true;
		}
		internal PdfSampledFunction(PdfSampledFunction func) : base(func)
		{
			this.samples = new double[func.samples.Length];
			Array.Copy(func.samples, this.samples, this.samples.Length);
			this.ok = func.ok;
			this.sampleSize = new int[func.sampleSize.Length];
			Array.Copy(func.sampleSize, this.sampleSize, func.sampleSize.Length);
			Array.Copy(func.encode, this.encode, 16);
			Array.Copy(func.decode, this.decode, 64);
		}
		internal override void transform(double[] inv, double[] outv)
		{
			int[,] array = new int[2, 8];
			double[] array2 = new double[8];
			double[] array3 = new double[256];
			double[] array4 = new double[256];
			for (int i = 0; i < this.m; i++)
			{
				double num = (inv[i] - this.domain[i, 0]) / (this.domain[i, 1] - this.domain[i, 0]) * (this.encode[i, 1] - this.encode[i, 0]) + this.encode[i, 0];
				if (num < 0.0)
				{
					num = 0.0;
				}
				else
				{
					if (num > (double)(this.sampleSize[i] - 1))
					{
						num = (double)(this.sampleSize[i] - 1);
					}
				}
				array[0, i] = (int)Math.Floor(num);
				array[1, i] = (int)Math.Ceiling(num);
				array2[i] = num - (double)array[0, i];
			}
			for (int i = 0; i < this.n; i++)
			{
				for (int j = 0; j < 1 << this.m; j++)
				{
					int num2 = 0;
					for (int k = this.m - 1; k >= 0; k--)
					{
						num2 = num2 * this.sampleSize[k] + array[j >> k & 1, k];
					}
					num2 = num2 * this.n + i;
					array3[j] = this.samples[num2];
				}
				for (int j = 0; j < this.m; j++)
				{
					for (int k = 0; k < 1 << this.m - j; k += 2)
					{
						array4[k >> 1] = (1.0 - array2[j]) * array3[k] + array2[j] * array3[k + 1];
					}
					Array.Copy(array4, array3, 1 << this.m - j - 1);
				}
				outv[i] = array3[0] * (this.decode[i, 1] - this.decode[i, 0]) + this.decode[i, 0];
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
		internal override PdfFunctionTop copy()
		{
			return new PdfSampledFunction(this);
		}
		internal override bool isOk()
		{
			return this.ok;
		}
	}
}
