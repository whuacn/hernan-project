using System;
namespace Persits.PDF
{
	public class PdfFunction
	{
		internal PdfDocument m_pDoc;
		internal PdfManager m_pManager;
		internal PdfIndirectObj m_pFuncObj;
		internal int m_nTotalSampleSize;
		internal int m_nFuncNumber;
		internal int m_nBoundsNumber;
		internal int m_nEncodeNumber;
		internal string m_bstrPostScript;
		internal string m_bstrError;
		internal int m_nM;
		internal int m_nN;
		internal int m_nType;
		internal int m_nBitsPerSample;
		public long Inputs
		{
			get
			{
				return (long)this.m_nM;
			}
		}
		public long Outputs
		{
			get
			{
				return (long)this.m_nN;
			}
		}
		public bool IsValid
		{
			get
			{
				this.m_bstrError = "";
				if (this.m_nType == 2)
				{
					return true;
				}
				if (this.m_nType == 0 && this.m_pFuncObj.m_objStream.Length == 0)
				{
					this.m_bstrError = "The method SetSampleData must be called on this sampled function.";
					return false;
				}
				if (this.m_nType == 3)
				{
					if (this.m_nFuncNumber == 0)
					{
						this.m_bstrError = "The method AddFunction must be called at least once on this stitching function.";
						return false;
					}
					if (this.m_nBoundsNumber != this.m_nFuncNumber - 1)
					{
						this.m_bstrError = "Invalid number of entries in the Bounds array.";
						return false;
					}
					if (this.m_nEncodeNumber != this.m_nFuncNumber)
					{
						this.m_bstrError = "Invalid number of entries in the Encode array.";
						return false;
					}
				}
				if (this.m_nType == 4 && this.m_pFuncObj.m_objStream.Length == 0)
				{
					this.m_bstrError = "The property PostScript must be called on this PostScript function.";
					return false;
				}
				return true;
			}
		}
		public string ValidationError
		{
			get
			{
				return this.m_bstrError;
			}
		}
		public string PostScript
		{
			get
			{
				return this.m_bstrPostScript;
			}
			set
			{
				this.m_bstrPostScript = value;
				this.m_pFuncObj.m_objStream.Set(value);
			}
		}
		internal PdfFunction()
		{
			this.m_nM = (this.m_nN = 0);
			this.m_nType = 0;
			this.m_nTotalSampleSize = 1;
			this.m_nFuncNumber = 0;
		}
		internal void Create(PdfParam pParam)
		{
			if (!pParam.IsSet("Type"))
			{
				AuxException.Throw("Type parameter must be specified.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			this.m_nType = (int)pParam.Number("Type");
			if (this.m_nType < 0 || this.m_nType > 4 || this.m_nType == 1)
			{
				AuxException.Throw("Function type must be 0, 2, 3 or 4.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			double[][] array = new double[20][];
			double[][] array2 = new double[20][];
			for (int i = 0; i < 20; i++)
			{
				array[i] = new double[2];
				array2[i] = new double[2];
			}
			double num = 0.0;
			double num2 = 0.0;
			while (this.m_nM < 20 && pParam.GetNumberIfSet("dmin%d", this.m_nM + 1, ref num) && pParam.GetNumberIfSet("dmax%d", this.m_nM + 1, ref num2))
			{
				if (num2 < num)
				{
					AuxException.Throw("dminMMM cannot exceed dmaxMMM.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
				}
				array[this.m_nM][0] = num;
				array[this.m_nM][1] = num2;
				this.m_nM++;
			}
			if (this.m_nM < 1)
			{
				AuxException.Throw("At least one domain must be specified via parameters dmin1, dmax1, dmin2, dmax2, ..., dminM, dmaxM.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			while (this.m_nN < 20 && pParam.GetNumberIfSet("rmin%d", this.m_nN + 1, ref num) && pParam.GetNumberIfSet("rmax%d", this.m_nN + 1, ref num2))
			{
				if (num2 < num)
				{
					AuxException.Throw("rminNNN cannot exceed rmaxNNN.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
				}
				array2[this.m_nN][0] = num;
				array2[this.m_nN][1] = num2;
				this.m_nN++;
			}
			if ((this.m_nType == 0 || this.m_nType == 4) && this.m_nN < 1)
			{
				AuxException.Throw("For function types 0 and 4, at least one range must be specified via parameters rmin1, rmax1, rmin2, rmax2, ..., rminN, rmaxN.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			this.m_pFuncObj = this.m_pDoc.AddNewIndirectObject((this.m_nType == 0 || this.m_nType == 4) ? enumIndirectType.pdfIndirectContents : enumIndirectType.pdfIndirectFunction);
			this.m_pFuncObj.AddNumber("FunctionType", (float)this.m_nType);
			PdfArray pdfArray = this.m_pFuncObj.AddArray("Domain");
			for (int j = 0; j < this.m_nM; j++)
			{
				pdfArray.Add(new PdfNumber(null, array[j][0]));
				pdfArray.Add(new PdfNumber(null, array[j][1]));
			}
			if (this.m_nN > 0)
			{
				PdfArray pdfArray2 = this.m_pFuncObj.AddArray("Range");
				for (int k = 0; k < this.m_nN; k++)
				{
					pdfArray2.Add(new PdfNumber(null, array2[k][0]));
					pdfArray2.Add(new PdfNumber(null, array2[k][1]));
				}
			}
			switch (this.m_nType)
			{
			case 0:
				this.HandleSampledFunction(pParam);
				return;
			case 1:
				break;
			case 2:
				this.HandleExpFunction(pParam);
				return;
			case 3:
				this.HandleStitchingFunction(pParam);
				return;
			case 4:
				this.HandlePostScriptFunction(pParam);
				break;
			default:
				return;
			}
		}
		internal void HandleSampledFunction(PdfParam pParam)
		{
			double num = 0.0;
			this.m_nTotalSampleSize = this.m_nN;
			PdfArray pdfArray = this.m_pFuncObj.AddArray("Size");
			for (int i = 0; i < this.m_nM; i++)
			{
				if (!pParam.GetNumberIfSet("Size%d", i + 1, ref num) || num <= 0.0)
				{
					AuxException.Throw("Positive values Size1, Size2, ..., SizeM must be specified for function type 0.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
				}
				pdfArray.Add(new PdfNumber(null, (double)((int)num)));
				this.m_nTotalSampleSize *= (int)num;
			}
			if (!pParam.GetNumberIfSet("BitsPerSample", 0, ref num))
			{
				AuxException.Throw("BitsPerSample parameter must be specified.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			if (num != 8.0 && num != 16.0 && num != 32.0)
			{
				AuxException.Throw("Invalid BitsPerSample parameter, must be 8, 16 or 32.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			this.m_nBitsPerSample = (int)num;
			this.m_pFuncObj.AddNumber("BitsPerSample", (float)this.m_nBitsPerSample);
			if (pParam.GetNumberIfSet("Order", 0, ref num))
			{
				if (num != 1.0 && num != 3.0)
				{
					AuxException.Throw("Order parameter must be 1 or 3.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
				}
				this.m_pFuncObj.AddNumber("Order", (float)((int)num));
			}
			double[][] array = new double[50][];
			for (int j = 0; j < 50; j++)
			{
				array[j] = new double[2];
			}
			bool flag = true;
			for (int k = 0; k < this.m_nM; k++)
			{
				if (!pParam.GetNumberIfSet("EncodeMin%d", k + 1, ref array[k][0]) || !pParam.GetNumberIfSet("EncodeMax%d", k + 1, ref array[k][1]))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				PdfArray pdfArray2 = this.m_pFuncObj.AddArray("Encode");
				for (int l = 0; l < this.m_nM; l++)
				{
					pdfArray2.Add(new PdfNumber(null, array[l][0]));
					pdfArray2.Add(new PdfNumber(null, array[l][1]));
				}
			}
			flag = true;
			for (int m = 0; m < this.m_nN; m++)
			{
				if (!pParam.GetNumberIfSet("DecodeMin%d", m + 1, ref array[m][0]) || !pParam.GetNumberIfSet("DecodeMax%d", m + 1, ref array[m][1]))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				PdfArray pdfArray3 = this.m_pFuncObj.AddArray("Decode");
				for (int n = 0; n < this.m_nN; n++)
				{
					pdfArray3.Add(new PdfNumber(null, array[n][0]));
					pdfArray3.Add(new PdfNumber(null, array[n][1]));
				}
			}
		}
		public void SetSampleData(object[] Data)
		{
			if (this.m_nType != 0)
			{
				AuxException.Throw("This method can only be called on sampled functions (Type=0).", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			if (Data == null)
			{
				AuxException.Throw("An array of objects must be supplied.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			int num = Data.Length;
			if (num != this.m_nTotalSampleSize)
			{
				string err = string.Format("Invalid sample array length ({0}), should  be {1}.", num, this.m_nTotalSampleSize);
				AuxException.Throw(err, PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			this.m_pFuncObj.m_objStream.Alloc(num * this.m_nBitsPerSample / 8);
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				ulong value = (ulong)Convert.ToUInt32(Data[i]);
				byte[] bytes = BitConverter.GetBytes(value);
				byte[] array = new byte[4];
				for (int j = 0; j < 4; j++)
				{
					array[j] = bytes[3 - j];
				}
				int nBitsPerSample = this.m_nBitsPerSample;
				if (nBitsPerSample != 8)
				{
					if (nBitsPerSample != 16)
					{
						if (nBitsPerSample == 32)
						{
							this.m_pFuncObj.m_objStream.m_objMemStream.Write(array, 0, 4);
						}
					}
					else
					{
						this.m_pFuncObj.m_objStream.m_objMemStream.Write(array, 2, 2);
					}
				}
				else
				{
					this.m_pFuncObj.m_objStream.m_objMemStream.Write(array, 3, 1);
				}
				num2 += this.m_nBitsPerSample / 8;
			}
		}
		internal void HandleExpFunction(PdfParam pParam)
		{
			double num = 0.0;
			double[] array = new double[50];
			double[] array2 = new double[50];
			if (this.m_nN == 0)
			{
				this.m_nN = 1;
			}
			if (!pParam.GetNumberIfSet("N", 0, ref num))
			{
				AuxException.Throw("N (interpolation exponent) must be specified for function type 1.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			this.m_pFuncObj.AddNumber("N", (float)num);
			bool flag = true;
			bool flag2 = true;
			for (int i = 0; i < this.m_nN; i++)
			{
				if (!pParam.GetNumberIfSet("CZero%d", i + 1, ref array[i]))
				{
					flag = false;
				}
				if (!pParam.GetNumberIfSet("COne%d", i + 1, ref array2[i]))
				{
					flag2 = false;
				}
			}
			if (flag)
			{
				PdfArray pdfArray = this.m_pFuncObj.AddArray("C0");
				for (int j = 0; j < this.m_nN; j++)
				{
					pdfArray.Add(new PdfNumber(null, array[j]));
				}
			}
			if (flag2)
			{
				PdfArray pdfArray2 = this.m_pFuncObj.AddArray("C1");
				for (int k = 0; k < this.m_nN; k++)
				{
					pdfArray2.Add(new PdfNumber(null, array2[k]));
				}
			}
		}
		internal void HandleStitchingFunction(PdfParam pParam)
		{
			double value = 0.0;
			double value2 = 0.0;
			if (this.m_nM != 1)
			{
				AuxException.Throw("A stitching function must be a 1-input function.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			this.m_pFuncObj.AddArray("Functions");
			PdfArray pdfArray = this.m_pFuncObj.AddArray("Bounds");
			this.m_nBoundsNumber = 0;
			while (pParam.GetNumberIfSet("Bounds%d", this.m_nBoundsNumber + 1, ref value))
			{
				pdfArray.Add(new PdfNumber(null, value));
				this.m_nBoundsNumber++;
			}
			PdfArray pdfArray2 = this.m_pFuncObj.AddArray("Encode");
			this.m_nEncodeNumber = 0;
			while (pParam.GetNumberIfSet("EncodeMin%d", this.m_nEncodeNumber + 1, ref value) && pParam.GetNumberIfSet("EncodeMax%d", this.m_nEncodeNumber + 1, ref value2))
			{
				pdfArray2.Add(new PdfNumber(null, value));
				pdfArray2.Add(new PdfNumber(null, value2));
				this.m_nEncodeNumber++;
			}
		}
		internal void AddFunction(PdfFunction Function)
		{
			if (this.m_nType != 3)
			{
				AuxException.Throw("This method can only be called on stitching functions (Type=3).", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			if (Function == null)
			{
				AuxException.Throw("The Function object is empty.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			Function.Validate();
			if (Function.m_nM != 1)
			{
				AuxException.Throw("The Function object must represent a 1-input function.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			PdfObject objectByName = this.m_pFuncObj.m_objAttributes.GetObjectByName("Functions");
			if (objectByName != null && objectByName.m_nType == enumType.pdfArray)
			{
				((PdfArray)objectByName).Add(new PdfReference(null, Function.m_pFuncObj));
				this.m_nFuncNumber++;
			}
		}
		internal void HandlePostScriptFunction(PdfParam pParam)
		{
		}
		internal void Validate()
		{
			if (!this.IsValid)
			{
				AuxException.Throw(this.m_bstrError, PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
		}
	}
}
