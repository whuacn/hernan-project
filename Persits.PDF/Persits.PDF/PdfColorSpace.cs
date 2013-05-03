using System;
namespace Persits.PDF
{
	public class PdfColorSpace
	{
		internal PdfDocument m_pDoc;
		internal PdfManager m_pManager;
		internal string m_bstrName;
		internal string m_bstrError;
		internal string m_bstrID;
		internal PdfIndirectObj m_pCSObj;
		internal PdfIndirectObj m_pICCObj;
		internal PdfIndirectObj m_pAttrObj;
		internal long m_nComponents;
		internal bool m_bStandard;
		internal bool m_bSeparationParamsSet;
		internal bool m_bIndexedParamsSet;
		internal int m_nHiVal;
		internal int m_nColorantNumber;
		public string Name
		{
			get
			{
				return this.m_bstrName;
			}
		}
		public long Components
		{
			get
			{
				return this.m_nComponents;
			}
		}
		public bool IsValid
		{
			get
			{
				this.m_bstrError = "";
				if (this.m_bstrName == "ICCBased" && this.m_pICCObj.m_objStream.Length == 0)
				{
					this.m_bstrError = "LoadDataFromFile needs to be called on the ICCBased color space object.";
					return false;
				}
				if (this.m_bstrName == "Separation" && !this.m_bSeparationParamsSet)
				{
					this.m_bstrError = "SetSeparationParams method must be called for the Separation color space object.";
					return false;
				}
				if (this.m_bstrName == "Indexed" && !this.m_bIndexedParamsSet)
				{
					this.m_bstrError = "SetIndexedParams needs to be called on the Indexed color space object.";
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
		internal PdfColorSpace()
		{
			this.m_bStandard = false;
			this.m_pCSObj = null;
			this.m_pICCObj = null;
			this.m_pAttrObj = null;
			this.m_nComponents = 0L;
			this.m_bSeparationParamsSet = false;
			this.m_bIndexedParamsSet = false;
			this.m_nHiVal = 0;
			this.m_nColorantNumber = 0;
		}
		internal void Create(PdfParam pParam)
		{
			if (this.m_bStandard)
			{
				this.m_bstrID = this.m_bstrName;
				return;
			}
			this.SetIndex();
			this.m_pCSObj = this.m_pDoc.AddNewIndirectObject(enumIndirectType.pdfIndirectColorSpace);
			PdfArray pdfArray = new PdfArray(null);
			pdfArray.Add(new PdfName(null, this.m_bstrName));
			if (this.m_bstrName == "CalGray" || this.m_bstrName == "CalRGB" || this.m_bstrName == "Lab")
			{
				if (!pParam.IsSet("Xw") || !pParam.IsSet("Yw") || !pParam.IsSet("Zw"))
				{
					AuxException.Throw("White point parameters Xw, Yw, Zw are required.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
				}
				float num = pParam.Number("Xw");
				float num2 = pParam.Number("Yw");
				float num3 = pParam.Number("Zw");
				if (num <= 0f || num3 <= 0f || num2 != 1f)
				{
					AuxException.Throw("Xw and Zx must be positive and Yw must be equal to 1.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
				}
				PdfDict pdfDict = new PdfDict(null);
				PdfArray pdfArray2 = new PdfArray("WhitePoint");
				pdfArray2.Add(new PdfNumber(null, (double)num));
				pdfArray2.Add(new PdfNumber(null, (double)num2));
				pdfArray2.Add(new PdfNumber(null, (double)num3));
				pdfDict.Add(pdfArray2);
				if (pParam.IsSet("Xb") && pParam.IsSet("Yb") && pParam.IsSet("Zb"))
				{
					float num4 = pParam.Number("Xb");
					float num5 = pParam.Number("Yb");
					float num6 = pParam.Number("Zb");
					if (num4 < 0f || num5 < 0f || num6 < 0f)
					{
						AuxException.Throw("Black point parameters Xb, Yb, Zb must be non-negative.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
					}
					PdfArray pdfArray3 = new PdfArray("BlackPoint");
					pdfArray3.Add(new PdfNumber(null, (double)num4));
					pdfArray3.Add(new PdfNumber(null, (double)num5));
					pdfArray3.Add(new PdfNumber(null, (double)num6));
					pdfDict.Add(pdfArray3);
				}
				if (this.m_bstrName == "CalGray")
				{
					if (pParam.IsSet("Gamma"))
					{
						float num7 = pParam.Number("Gamma");
						if (num7 <= 0f)
						{
							AuxException.Throw("Gamma parameter must be positive.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
						}
						pdfDict.Add(new PdfNumber("Gamma", (double)num7));
					}
				}
				else
				{
					if (this.m_bstrName == "CalRGB" && pParam.IsSet("GammaR") && pParam.IsSet("GammaG") && pParam.IsSet("GammaB"))
					{
						float num8 = pParam.Number("GammaR");
						float num9 = pParam.Number("GammaG");
						float num10 = pParam.Number("GammaB");
						if (num8 <= 0f || num9 <= 0f || num10 <= 0f)
						{
							AuxException.Throw("Gamma parameters GammaR, GammaG, GammaB must be positive.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
						}
						PdfArray pdfArray4 = new PdfArray("Gamma");
						pdfArray4.Add(new PdfNumber(null, (double)num8));
						pdfArray4.Add(new PdfNumber(null, (double)num9));
						pdfArray4.Add(new PdfNumber(null, (double)num10));
						pdfDict.Add(pdfArray4);
					}
				}
				if (this.m_bstrName == "CalRGB")
				{
					float[] array = new float[9];
					string text = "m";
					int num11 = 0;
					int num12 = 1;
					while (num12 <= 9 && pParam.IsSet(text + num12.ToString()))
					{
						array[num12 - 1] = pParam.Number(text);
						num11 = num12;
						num12++;
					}
					if (num11 > 9)
					{
						PdfArray pdfArray5 = new PdfArray("Matrix");
						for (int i = 0; i < 9; i++)
						{
							pdfArray5.Add(new PdfNumber(null, (double)array[i]));
						}
						pdfDict.Add(pdfArray5);
					}
				}
				if (this.m_bstrName == "Lab" && pParam.IsSet("amin") && pParam.IsSet("amax") && pParam.IsSet("bmin") && pParam.IsSet("bmax"))
				{
					float num13 = pParam.Number("amin");
					float num14 = pParam.Number("amax");
					float num15 = pParam.Number("bmin");
					float num16 = pParam.Number("bmax");
					PdfArray pdfArray6 = new PdfArray("Range");
					pdfArray6.Add(new PdfNumber(null, (double)num13));
					pdfArray6.Add(new PdfNumber(null, (double)num14));
					pdfArray6.Add(new PdfNumber(null, (double)num15));
					pdfArray6.Add(new PdfNumber(null, (double)num16));
					pdfDict.Add(pdfArray6);
				}
				pdfArray.Add(pdfDict);
			}
			if (this.m_bstrName == "ICCBased")
			{
				this.HandleICCBased(pParam, pdfArray);
			}
			if (this.m_bstrName == "Indexed")
			{
				this.HandleIndexed(pParam, pdfArray);
			}
			this.m_pCSObj.m_pSimpleValue = pdfArray;
		}
		internal void HandleIndexed(PdfParam pParam, PdfArray pMainArray)
		{
			double num = 0.0;
			if (!pParam.GetNumberIfSet("Hival", 0, ref num))
			{
				AuxException.Throw("Indexed color space requires the Hival parameter.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			if (num <= 0.0)
			{
				AuxException.Throw("Hival parameter must be a positive number.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			this.m_nHiVal = (int)num;
		}
		internal void HandleICCBased(PdfParam pParam, PdfArray pMainArray)
		{
			if (!pParam.IsSet("N"))
			{
				AuxException.Throw("ICCBased color space requires the N parameter (number of color components.)", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			int num = (int)pParam.Number("N");
			if (num != 1 && num != 3 && num != 4)
			{
				AuxException.Throw("The N parameter's legal values are 1, 3 and 4.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			this.m_nComponents = (long)num;
			this.m_pICCObj = this.m_pDoc.AddNewIndirectObject(enumIndirectType.pdfIndirectContents);
			this.m_pICCObj.AddNumber("N", (float)num);
			string[] array = new string[]
			{
				"min1",
				"max1",
				"min2",
				"max2",
				"min3",
				"max3",
				"min4",
				"max4"
			};
			bool flag = true;
			for (int i = 0; i < 2 * num; i++)
			{
				if (!pParam.IsSet(array[i]))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				PdfArray pdfArray = this.m_pICCObj.AddArray("Range");
				for (int j = 0; j < 2 * num; j++)
				{
					pdfArray.Add(new PdfNumber(null, (double)pParam.Number(array[j])));
				}
			}
			pMainArray.Add(new PdfReference(null, this.m_pICCObj));
		}
		public void LoadDataFromFile(string Path)
		{
			if (this.m_pICCObj == null)
			{
				AuxException.Throw("This method can only be called on ICCBased or Indexed colorspaces.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			if (this.m_pICCObj.m_objStream.Length > 0)
			{
				AuxException.Throw("This method can only be called once per object instance.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			this.m_pICCObj.m_objStream.LoadFromFile(Path);
		}
		public void SetIndexedParams(PdfColorSpace BaseSpace, object[] Data)
		{
			if (!(this.m_bstrName == "Indexed"))
			{
				AuxException.Throw("This method can only be called on Indexed color space objects.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			if (this.m_pICCObj != null)
			{
				AuxException.Throw("This method can only be called once per object instance.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			if (BaseSpace == null)
			{
				AuxException.Throw("The Base color space argument cannot be empty.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			BaseSpace.Validate();
			PdfArray pdfArray = (PdfArray)this.m_pCSObj.m_pSimpleValue;
			if (BaseSpace.m_pCSObj == null)
			{
				pdfArray.Add(new PdfName(null, BaseSpace.m_bstrName));
			}
			else
			{
				pdfArray.Add(new PdfReference(null, BaseSpace.m_pCSObj));
			}
			if (Data == null)
			{
				AuxException.Throw("The Data argument cannot be empty.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			int num = Data.Length;
			if ((long)num != BaseSpace.m_nComponents * (long)(this.m_nHiVal + 1))
			{
				string err = string.Format("Invalid data array length ({0}), should  be {1}.", num, BaseSpace.m_nComponents * (long)(this.m_nHiVal + 1));
				AuxException.Throw(err, PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			this.m_pICCObj = this.m_pDoc.AddNewIndirectObject(enumIndirectType.pdfIndirectContents);
			this.m_pICCObj.m_objStream.Alloc(num);
			for (int i = 0; i < num; i++)
			{
				ulong num2 = (ulong)Convert.ToUInt32(Data[i]);
				byte value = (byte)(num2 & 255uL);
				this.m_pICCObj.m_objStream.m_objMemStream.WriteByte(value);
			}
			pdfArray.Add(new PdfNumber(null, (double)this.m_nHiVal));
			pdfArray.Add(new PdfReference(null, this.m_pICCObj));
			this.m_bIndexedParamsSet = true;
		}
		internal void SetIndex()
		{
			int num = 0;
			string text;
			do
			{
				num++;
				text = string.Format("Cs{0}", num);
			}
			while (this.m_pDoc.m_listCSNames.GetObjectByName(text) != null);
			this.m_bstrID = text;
			this.m_pDoc.m_listCSNames.Add(new PdfName(text, null));
		}
		internal void Validate()
		{
			if (!this.IsValid)
			{
				AuxException.Throw(this.m_bstrError, PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
		}
		public void SetSeparationParams(string Name, PdfColorSpace AltSpace, PdfFunction Function)
		{
			if (!(this.m_bstrName == "Separation") && !(this.m_bstrName == "DeviceN"))
			{
				AuxException.Throw("This method can only be called on a Separation or DeviceN color space.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			if (Name == null || Name.Length == 0)
			{
				AuxException.Throw("The Name argument cannot be empty.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			if (AltSpace == null)
			{
				AuxException.Throw("The Alternative color space argument cannot be empty.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			if (Function == null)
			{
				AuxException.Throw("The Function argument cannot be empty.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			AltSpace.Validate();
			Function.Validate();
			PdfArray pdfArray = (PdfArray)this.m_pCSObj.m_pSimpleValue;
			if (this.m_bstrName == "Separation")
			{
				pdfArray.Add(new PdfName(null, Name));
			}
			else
			{
				PdfArray pdfArray2 = new PdfArray(null);
				this.SplitString(Name, pdfArray2);
				pdfArray.Add(pdfArray2);
			}
			if (AltSpace.m_pCSObj == null)
			{
				pdfArray.Add(new PdfName(null, AltSpace.m_bstrName));
			}
			else
			{
				pdfArray.Add(new PdfReference(null, AltSpace.m_pCSObj));
			}
			pdfArray.Add(new PdfReference(null, Function.m_pFuncObj));
			this.m_bSeparationParamsSet = true;
		}
		internal void SplitString(string Str, PdfArray pArray)
		{
			string[] array = Str.Split(new string[]
			{
				"##"
			}, StringSplitOptions.None);
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string name = array2[i];
				pArray.Add(new PdfName(null, name));
				this.m_nComponents += 1L;
			}
		}
		public void AddColorant(string Name, PdfColorSpace ColorSpace, PdfFunction Function)
		{
			if (!(this.m_bstrName == "DeviceN"))
			{
				AuxException.Throw("This method can only be called on a DeviceN color space.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			if (Name == null || Name.Length == 0)
			{
				AuxException.Throw("The Name argument cannot be empty.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			if (ColorSpace == null)
			{
				AuxException.Throw("The color space argument cannot be empty.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			if (Function == null)
			{
				AuxException.Throw("The Function argument cannot be empty.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			if (this.m_nComponents == 0L)
			{
				AuxException.Throw("The method SetSeparationParams must be called first.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			ColorSpace.Validate();
			Function.Validate();
			PdfArray pdfArray = (PdfArray)this.m_pCSObj.m_pSimpleValue;
			PdfDict pdfDict;
			if (pdfArray.Size < 5)
			{
				this.m_pAttrObj = this.m_pDoc.AddNewIndirectObject(enumIndirectType.pdfIndirectColorSpace);
				pdfArray.Add(new PdfReference(null, this.m_pAttrObj));
				pdfDict = this.m_pAttrObj.AddDict("Colorants");
			}
			else
			{
				pdfDict = (PdfDict)this.m_pAttrObj.m_objAttributes.GetObjectByName("Colorants");
			}
			PdfArray pdfArray2 = new PdfArray(Name);
			pdfArray2.Add(new PdfName(null, "Separation"));
			pdfArray2.Add(new PdfName(null, Name));
			if (ColorSpace.m_pCSObj == null)
			{
				pdfArray2.Add(new PdfName(null, ColorSpace.m_bstrName));
			}
			else
			{
				pdfArray2.Add(new PdfReference(null, ColorSpace.m_pCSObj));
			}
			pdfArray2.Add(new PdfReference(null, Function.m_pFuncObj));
			pdfDict.Add(pdfArray2);
			this.m_nColorantNumber++;
		}
		public void SetAltColorSpace(PdfColorSpace AltSpace)
		{
			if (!(this.m_bstrName == "ICCBased"))
			{
				AuxException.Throw("This method can only be called on an ICCBased color space.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			if (AltSpace == null)
			{
				AuxException.Throw("The Alternative color space argument cannot be empty.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			AltSpace.Validate();
			if (this.m_pICCObj == null)
			{
				return;
			}
			if (AltSpace.m_pCSObj == null)
			{
				this.m_pICCObj.AddName("Alternate", AltSpace.m_bstrName);
				return;
			}
			this.m_pICCObj.AddReference("Alternate", AltSpace.m_pCSObj);
		}
	}
}
