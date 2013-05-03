using System;
using System.Collections;
using System.Collections.Generic;
namespace Persits.PDF
{
	public class PdfAnnots : IEnumerable
	{
		internal PdfManager m_pManager;
		internal PdfDocument m_pDoc;
		internal PdfPage m_pPage;
		internal List<PdfAnnot> m_parrAnnots;
		internal bool m_bFormFields;
		public PdfAnnot this[int index]
		{
			get
			{
				if (index < 1 && index > this.m_parrAnnots.Count)
				{
					AuxException.Throw("Index out of range.", PdfErrors._ERROR_OUTOFRANGE);
				}
				return this.m_parrAnnots[index - 1];
			}
		}
		public int Count
		{
			get
			{
				return this.m_parrAnnots.Count;
			}
		}
		internal PdfAnnots()
		{
		}
		public IEnumerator GetEnumerator()
		{
			return this.m_parrAnnots.GetEnumerator();
		}
		public PdfAnnot Add(string Contents, object Param)
		{
			return this.Add(Contents, Param, null, null);
		}
		public PdfAnnot Add(string Contents, object Param, string Name)
		{
			return this.Add(Contents, Param, Name, null);
		}
		public PdfAnnot Add(string Contents, object Param, string Name, string Path)
		{
			if (this.m_bFormFields)
			{
				AuxException.Throw("Operation not allowed on Document.Form.Fields.", PdfErrors._ERROR_INVALIDARG);
			}
			PdfParam pParam = this.m_pManager.VariantToParam(Param);
			PdfAnnot pdfAnnot = new PdfAnnot();
			pdfAnnot.m_pManager = this.m_pManager;
			pdfAnnot.m_pPage = this.m_pPage;
			pdfAnnot.m_pDoc = this.m_pPage.m_pDoc;
			pdfAnnot.Create(Contents, pParam, Name, Path);
			this.m_parrAnnots.Add(pdfAnnot);
			pdfAnnot.m_nIndex = this.m_parrAnnots.Count;
			if (this.m_pPage.m_pAnnotArrayObj != null)
			{
				((PdfArray)this.m_pPage.m_pAnnotArrayObj.m_pSimpleValue).Add(new PdfReference(null, pdfAnnot.m_pAnnotObj));
				this.m_pPage.m_pAnnotArrayObj.m_bModified = true;
			}
			else
			{
				PdfArray pdfArray = this.m_pPage.m_pPageObj.AddArray("Annots");
				pdfArray.Add(new PdfReference(null, pdfAnnot.m_pAnnotObj));
				this.m_pPage.m_pPageObj.m_bModified = true;
			}
			return pdfAnnot;
		}
		internal PdfAnnot AddExisting(PdfDict pAnnotDict, PdfArray pParentArray, int nObjNum, int nGenNum)
		{
			PdfAnnot pdfAnnot = new PdfAnnot();
			pdfAnnot.m_pManager = this.m_pManager;
			pdfAnnot.m_pPage = this.m_pPage;
			pdfAnnot.m_pDoc = this.m_pDoc;
			pdfAnnot.m_bExisting = true;
			this.m_parrAnnots.Add(pdfAnnot);
			pdfAnnot.m_nIndex = this.m_parrAnnots.Count;
			pdfAnnot.m_pAnnotObj = this.m_pDoc.AddExistingIndirectObject(enumIndirectType.pdfIndirectPage, nObjNum, nGenNum, pAnnotDict);
			pdfAnnot.m_pAnnotObj.m_pParentArrayOfKids = pParentArray;
			PdfObject objectByName = pAnnotDict.GetObjectByName("Contents");
			if (objectByName != null && objectByName.m_nType == enumType.pdfString)
			{
				pdfAnnot.m_bstrContents = ((PdfString)objectByName).ToString();
			}
			objectByName = pAnnotDict.GetObjectByName("Subtype");
			if (objectByName != null && objectByName.m_nType == enumType.pdfName)
			{
				pdfAnnot.m_bstrType = ((PdfName)objectByName).m_bstrName;
			}
			objectByName = pAnnotDict.GetObjectByName("Name");
			if (objectByName != null && objectByName.m_nType == enumType.pdfName)
			{
				pdfAnnot.m_bstrName = ((PdfName)objectByName).m_bstrName;
			}
			objectByName = pAnnotDict.GetObjectByName("M");
			if (objectByName != null && objectByName.m_nType == enumType.pdfString)
			{
				pdfAnnot.m_dtModified = ((PdfString)objectByName).ToDate();
			}
			objectByName = pAnnotDict.GetObjectByName("FT");
			if (objectByName != null && objectByName.m_nType == enumType.pdfName)
			{
				pdfAnnot.m_bstrFieldType = ((PdfName)objectByName).m_bstrName;
			}
			objectByName = pAnnotDict.GetObjectByName("Ff");
			if (objectByName != null && objectByName.m_nType == enumType.pdfNumber)
			{
				pdfAnnot.m_nFieldFlags = (int)((PdfNumber)objectByName).m_fValue;
			}
			objectByName = pAnnotDict.GetObjectByName("Q");
			if (objectByName != null && objectByName.m_nType == enumType.pdfNumber)
			{
				pdfAnnot.m_nAlignment = (int)((PdfNumber)objectByName).m_fValue;
			}
			pdfAnnot.m_bIsPushbutton = ((pdfAnnot.m_nFieldFlags & 65536) > 0);
			objectByName = pAnnotDict.GetObjectByName("F");
			if (objectByName != null && objectByName.m_nType == enumType.pdfNumber)
			{
				pdfAnnot.m_nFlags = (int)((PdfNumber)objectByName).m_fValue;
			}
			if (pdfAnnot.m_bstrFieldType != null && string.Compare(pdfAnnot.m_bstrFieldType, "Btn") == 0)
			{
				pdfAnnot.m_bIsRadiobutton = (!pdfAnnot.m_bIsPushbutton && (pdfAnnot.m_nFieldFlags & 32768) > 0);
				pdfAnnot.m_bIsCheckbox = (!pdfAnnot.m_bIsPushbutton && (pdfAnnot.m_nFieldFlags & 32768) == 0);
			}
			objectByName = pAnnotDict.GetObjectByName("AP");
			if (objectByName != null && objectByName.m_nType == enumType.pdfDictionary)
			{
				PdfDict pdfDict = (PdfDict)objectByName;
				pdfAnnot.m_ptrApperanceDict = (PdfDict)pdfDict.Copy();
				PdfObject objectByName2 = pdfDict.GetObjectByName("N");
				if (objectByName2 != null && objectByName2.m_nType == enumType.pdfDictionary)
				{
					PdfDict pdfDict2 = (PdfDict)objectByName2;
					for (int i = 0; i < pdfDict2.Size; i++)
					{
						PdfObject @object = pdfDict2.GetObject(i);
						if (@object != null && !(@object.m_bstrType == "Off"))
						{
							pdfAnnot.m_bstrFieldOnValue = @object.m_bstrType;
							break;
						}
					}
				}
			}
			objectByName = pAnnotDict.GetObjectByName("T");
			if (objectByName != null && objectByName.m_nType == enumType.pdfString)
			{
				pdfAnnot.m_bstrFieldName = ((PdfString)objectByName).ToString();
			}
			objectByName = pAnnotDict.GetObjectByName("TU");
			if (objectByName != null && objectByName.m_nType == enumType.pdfString)
			{
				pdfAnnot.m_bstrFieldAlternateName = ((PdfString)objectByName).ToString();
			}
			objectByName = pAnnotDict.GetObjectByName("AS");
			if (objectByName != null && objectByName.m_nType == enumType.pdfName)
			{
				pdfAnnot.m_bstrFieldActiveState = ((PdfName)objectByName).m_bstrName;
			}
			objectByName = pAnnotDict.GetObjectByName("DA");
			if (objectByName != null && objectByName.m_nType == enumType.pdfString)
			{
				pdfAnnot.m_bstrFieldDefaultAppearance = ((PdfString)objectByName).ToString();
			}
			objectByName = pAnnotDict.GetObjectByName("Opt");
			if (objectByName != null && objectByName.m_nType == enumType.pdfArray)
			{
				PdfArray pdfArray = (PdfArray)objectByName;
				for (int j = 0; j < pdfArray.Size; j++)
				{
					PdfObject object2 = pdfArray.GetObject(j);
					if (object2.m_nType == enumType.pdfString)
					{
						string text = ((PdfString)object2).ToString();
						pdfAnnot.m_mapOptions[text] = text;
					}
					else
					{
						if (object2.m_nType == enumType.pdfArray)
						{
							PdfArray pdfArray2 = (PdfArray)object2;
							if (pdfArray2.Size == 2)
							{
								PdfObject object3 = pdfArray2.GetObject(0);
								PdfObject object4 = pdfArray2.GetObject(1);
								if (object3.m_nType == enumType.pdfString && object4.m_nType == enumType.pdfString)
								{
									string key = ((PdfString)object3).ToString();
									string value = ((PdfString)object4).ToString();
									pdfAnnot.m_mapOptions[key] = value;
								}
							}
						}
					}
				}
			}
			string[] array = new string[]
			{
				pdfAnnot.m_bstrFieldValue,
				pdfAnnot.m_bstrFieldDefaultValue
			};
			string[] array2 = new string[]
			{
				"V",
				"DV"
			};
			for (int k = 0; k < array2.Length; k++)
			{
				objectByName = pAnnotDict.GetObjectByName(array2[k]);
				if (objectByName != null)
				{
					if (objectByName.m_nType == enumType.pdfString)
					{
						array[k] = ((PdfString)objectByName).ToString();
					}
					else
					{
						if (objectByName.m_nType == enumType.pdfArray)
						{
							PdfArray pdfArray3 = (PdfArray)objectByName;
							for (int l = 0; l < pdfArray3.Size; l++)
							{
								PdfObject object5 = pdfArray3.GetObject(l);
								if (object5.m_nType == enumType.pdfString)
								{
									if (array[k] != null)
									{
										string[] array3;
										IntPtr intPtr;
										(array3 = array)[(int)(intPtr = (IntPtr)k)] = array3[(int)intPtr] + "##";
									}
									string text2 = ((PdfString)object5).ToString();
									if (pdfAnnot.m_mapOptions.Count > 0)
									{
										text2 = pdfAnnot.m_mapOptions[text2];
									}
									string[] array4;
									IntPtr intPtr2;
									(array4 = array)[(int)(intPtr2 = (IntPtr)k)] = array4[(int)intPtr2] + text2;
								}
							}
						}
					}
				}
			}
			pdfAnnot.m_bstrFieldValue = array[0];
			pdfAnnot.m_bstrFieldDefaultValue = array[1];
			PdfObject objectByName3 = pAnnotDict.GetObjectByName("Rect");
			if (objectByName3 != null && objectByName3.m_nType == enumType.pdfArray)
			{
				PdfArray pdfArray4 = (PdfArray)objectByName3;
				float[] array5 = new float[]
				{
					pdfAnnot.m_fX1,
					pdfAnnot.m_fY1,
					pdfAnnot.m_fX2,
					pdfAnnot.m_fY2
				};
				for (int m = 0; m < array5.Length; m++)
				{
					PdfObject object6 = pdfArray4.GetObject(m);
					if (object6 != null && object6.m_nType == enumType.pdfNumber)
					{
						array5[m] = ((PdfNumber)object6).ToFloat();
					}
				}
				pdfAnnot.m_fX1 = array5[0];
				pdfAnnot.m_fY1 = array5[1];
				pdfAnnot.m_fX2 = array5[2];
				pdfAnnot.m_fY2 = array5[3];
			}
			PdfObject objectByName4 = pAnnotDict.GetObjectByName("MK");
			if (objectByName4 != null && objectByName4.m_nType == enumType.pdfDictionary)
			{
				PdfObject objectByName5 = ((PdfDict)objectByName4).GetObjectByName("R");
				if (objectByName5 != null && objectByName5.m_nType == enumType.pdfNumber)
				{
					int n;
					for (n = (int)((PdfNumber)objectByName5).m_fValue; n < 0; n += 360)
					{
					}
					pdfAnnot.m_nRotate = n % 360;
				}
			}
			PdfObject objectByName6 = pAnnotDict.GetObjectByName("BS");
			if (objectByName6 != null && objectByName6.m_nType == enumType.pdfDictionary)
			{
				PdfObject objectByName7 = ((PdfDict)objectByName6).GetObjectByName("W");
				if (objectByName7 != null && objectByName7.m_nType == enumType.pdfNumber)
				{
					pdfAnnot.m_fBorderWidth = (float)((PdfNumber)objectByName7).m_fValue;
				}
			}
			pdfAnnot.m_mapOptions.Clear();
			return pdfAnnot;
		}
		public void Remove(int Index)
		{
			if (this.m_bFormFields)
			{
				AuxException.Throw("Operation not allowed on Document.Form.Fields.", PdfErrors._ERROR_INVALIDARG);
			}
			if (Index < 1 || Index > this.m_parrAnnots.Count)
			{
				AuxException.Throw("Index out of range.", PdfErrors._ERROR_OUTOFRANGE);
			}
			PdfAnnot pdfAnnot = this.m_parrAnnots[Index - 1];
			if (pdfAnnot.m_pFilespecObj != null)
			{
				this.m_pPage.m_pDoc.RemoveIndirectObject(pdfAnnot.m_pFilespecObj);
			}
			if (pdfAnnot.m_pEmbeddedFileObj != null)
			{
				this.m_pPage.m_pDoc.RemoveIndirectObject(pdfAnnot.m_pEmbeddedFileObj);
			}
			this.m_pPage.m_pDoc.RemoveIndirectObject(pdfAnnot.m_pAnnotObj);
			this.m_parrAnnots.RemoveAt(Index - 1);
			for (int i = Index; i < this.m_parrAnnots.Count; i++)
			{
				this.m_parrAnnots[i].m_nIndex--;
			}
		}
	}
}
