using System;
using System.Collections.Generic;
using System.Text;
namespace Persits.PDF
{
	public class PdfForm
	{
		internal List<PdfAnnot> m_arrFields = new List<PdfAnnot>();
		internal PdfManager m_pManager;
		internal PdfDocument m_pDoc;
		internal PdfIndirectObj m_pFormObj;
		internal PdfIndirectObj m_pDRObj;
		internal PdfIndirectObj m_pFontObj;
		internal PdfFont m_ptrEncoding;
		internal int m_nSigFlags;
		internal bool m_bNeedAppearances;
		internal string m_bstrDefaultAppearance;
		public PdfAnnots Fields
		{
			get
			{
				return new PdfAnnots
				{
					m_bFormFields = true,
					m_pManager = this.m_pManager,
					m_pDoc = this.m_pDoc,
					m_pPage = null,
					m_parrAnnots = this.m_arrFields
				};
			}
		}
		public int SigFlags
		{
			get
			{
				return this.m_nSigFlags;
			}
			set
			{
				this.m_nSigFlags = (value & 3);
				this.m_pFormObj.AddInt("SigFlags", this.m_nSigFlags);
				this.m_pFormObj.m_bModified = true;
			}
		}
		public bool NeedAppearances
		{
			get
			{
				return this.m_bNeedAppearances;
			}
			set
			{
				this.m_bNeedAppearances = value;
				this.m_pFormObj.AddBool("NeedAppearances", value);
				this.m_pFormObj.m_bModified = true;
			}
		}
		internal PdfForm()
		{
		}
		internal void Create()
		{
			this.m_pFormObj = this.m_pDoc.AddNewIndirectObject(enumIndirectType.pdfIndirectForm);
			this.m_pFormObj.AddArray("Fields");
			if (this.m_pDoc.m_pCatalogObj.m_objAttributes.GetObjectByName("AcroForm") == null)
			{
				this.m_pDoc.m_pCatalogObj.m_bModified = true;
			}
			this.m_pDoc.m_pCatalogObj.AddReference("AcroForm", this.m_pFormObj);
		}
		internal void ValidateName(string Name)
		{
			if (Name == null)
			{
				return;
			}
			foreach (PdfAnnot current in this.m_arrFields)
			{
				if (current.m_bstrFieldName != null && string.Compare(current.m_bstrFieldName, Name, true) == 0)
				{
					AuxException.Throw("A field under this name already exists.", PdfErrors._ERROR_INVALIDARG);
				}
			}
		}
		internal void AddFontToResources(PdfFont Font, int nFontSize)
		{
			Font.CreateIndirectObject(null, null);
			PdfDict pdfDict;
			if (this.m_pFontObj != null)
			{
				pdfDict = this.m_pFontObj.m_objAttributes;
				this.m_pFontObj.m_bModified = true;
			}
			else
			{
				if (this.m_pDRObj != null)
				{
					pdfDict = (PdfDict)this.m_pDRObj.m_objAttributes.GetObjectByName("Font");
					if (pdfDict == null)
					{
						pdfDict = new PdfDict("Font");
						this.m_pDRObj.m_objAttributes.Add(pdfDict);
						this.m_pDRObj.m_bModified = true;
					}
				}
				else
				{
					PdfDict pdfDict2 = this.m_pFormObj.AddDict("DR");
					if (pdfDict2.m_nType != enumType.pdfDictionary)
					{
						return;
					}
					pdfDict = (PdfDict)pdfDict2.GetObjectByName("Font");
					if (pdfDict == null)
					{
						pdfDict = new PdfDict("Font");
						pdfDict2.Add(pdfDict);
					}
					this.m_pFormObj.m_bModified = true;
				}
			}
			if (pdfDict != null)
			{
				pdfDict.Add(new PdfReference(Font.m_szID, Font.m_pFontObj));
			}
		}
		public PdfAnnot FindField(string Name)
		{
			if (Name.Length == 0)
			{
				return null;
			}
			foreach (PdfAnnot current in this.m_arrFields)
			{
				if (current.m_bstrFieldName != null && string.Compare(current.m_bstrFieldName, Name, true) == 0)
				{
					PdfAnnot result = current;
					return result;
				}
			}
			string[] array = Name.Split(new char[]
			{
				'.'
			});
			if (array.Length <= 1)
			{
				return null;
			}
			PdfAnnot pdfAnnot = null;
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				PdfAnnots pdfAnnots;
				if (text == array[0])
				{
					pdfAnnots = this.Fields;
				}
				else
				{
					pdfAnnots = pdfAnnot.Children;
				}
				foreach (PdfAnnot current2 in pdfAnnots.m_parrAnnots)
				{
					if (current2.m_bstrFieldName != null && string.Compare(current2.m_bstrFieldName, text, true) == 0)
					{
						pdfAnnot = current2;
						break;
					}
				}
				if (pdfAnnot == null)
				{
					PdfAnnot result = null;
					return result;
				}
			}
			return pdfAnnot;
		}
		public void RemoveXFA()
		{
			this.m_pFormObj.m_objAttributes.RemoveByName("XFA");
			this.m_pFormObj.m_bModified = true;
		}
		public void Flatten()
		{
			PdfAnnots fields = this.Fields;
			this.Traverse(fields, 0);
			this.m_pDoc.m_pCatalogObj.m_objAttributes.RemoveByName("AcroForm");
			this.m_pDoc.m_pCatalogObj.m_bModified = true;
			this.m_pFormObj.m_bModified = false;
		}
		private void Traverse(PdfAnnots piAnnots, int nLevel)
		{
			PdfAnnots pdfAnnots = new PdfAnnots();
			pdfAnnots.m_parrAnnots = new List<PdfAnnot>();
			foreach (PdfAnnot item in piAnnots)
			{
				pdfAnnots.m_parrAnnots.Add(item);
			}
			foreach (PdfAnnot pdfAnnot in pdfAnnots)
			{
				this.FlattenField(pdfAnnot);
				this.Traverse(pdfAnnot.Children, nLevel + 1);
			}
		}
		private void FlattenField(PdfAnnot pField)
		{
			if (pField.m_ptrApperanceDict == null)
			{
				return;
			}
			if (pField.m_pPage == null)
			{
				return;
			}
			PdfObject objectByName = pField.m_ptrApperanceDict.GetObjectByName("N");
			if (objectByName == null)
			{
				return;
			}
			if (objectByName.m_nType == enumType.pdfDictionary)
			{
				string bstrFieldActiveState = pField.m_bstrFieldActiveState;
				if (bstrFieldActiveState.Length == 0)
				{
					return;
				}
				objectByName = ((PdfDict)objectByName).GetObjectByName(bstrFieldActiveState);
				if (objectByName == null)
				{
					return;
				}
			}
			if (objectByName.m_nType == enumType.pdfRef)
			{
				PdfIndirectObj pdfIndirectObj = null;
				try
				{
					PdfRef pdfRef = new PdfRef(null, ((PdfRef)objectByName).m_nObjNum, ((PdfRef)objectByName).m_nGenNum);
					PdfObject pdfObject = this.m_pDoc.m_pInput.ParseOriginalGenericObject(pdfRef, 0);
					if (pdfObject.m_nType != enumType.pdfDictWithStream)
					{
						return;
					}
					PdfDictWithStream pdfDictWithStream = (PdfDictWithStream)pdfObject;
					PdfObject pdfObject2 = pdfDictWithStream.GetObjectByName("Resources");
					if (pdfObject2 != null && pdfObject2.m_nType == enumType.pdfRef)
					{
						pdfObject2 = (PdfDict)this.m_pDoc.m_pInput.RemoveRef(pdfObject2, enumType.pdfDictionary);
						pdfDictWithStream.Add(pdfObject2);
					}
					if (pdfObject2 != null && pdfObject2.m_nType == enumType.pdfDictionary)
					{
						PdfObject pdfObject3 = ((PdfDict)pdfObject2).GetObjectByName("Font");
						if (pdfObject3 != null && pdfObject3.m_nType == enumType.pdfRef)
						{
							pdfObject3 = (PdfDict)this.m_pDoc.m_pInput.RemoveRef(pdfObject3, enumType.pdfDictionary);
							((PdfDict)pdfObject2).Add(pdfObject3);
						}
					}
					PdfDict pdfDict = pdfDictWithStream.CopyDict();
					pdfDict.RemoveByName("Filter");
					pdfIndirectObj = this.m_pDoc.AddExistingIndirectObject(enumIndirectType.pdfIndirectStream, pdfRef.m_nObjNum, pdfRef.m_nGenNum, pdfDict);
					pdfIndirectObj.m_objStream.Set(pdfDictWithStream.m_objStream);
				}
				catch (Exception)
				{
					return;
				}
				if (!this.IsEmpty(pdfIndirectObj.m_objStream))
				{
					if (pdfIndirectObj.m_objAttributes.GetObjectByName("Type") == null)
					{
						pdfIndirectObj.m_objAttributes.Add(new PdfName("Type", "XObject"));
						pdfIndirectObj.m_objAttributes.Add(new PdfName("Subtype", "Form"));
						pdfIndirectObj.m_bModified = true;
					}
					PdfObject objectByName2 = pdfIndirectObj.m_objAttributes.GetObjectByName("Resources");
					PdfObject objectByName3;
					if (pField.m_bstrFieldDefaultAppearance != null && objectByName2 != null && objectByName2 != null && objectByName2.m_nType == enumType.pdfDictionary && (objectByName3 = ((PdfDict)objectByName2).GetObjectByName("Font")) != null)
					{
						PdfDict pdfDict2;
						if (this.m_pFontObj != null)
						{
							pdfDict2 = this.m_pFontObj.m_objAttributes;
						}
						else
						{
							if (this.m_pDRObj != null)
							{
								pdfDict2 = (PdfDict)this.m_pDRObj.m_objAttributes.GetObjectByName("Font");
							}
							else
							{
								PdfDict pdfDict3 = (PdfDict)this.m_pFormObj.m_objAttributes.GetObjectByName("DR");
								pdfDict2 = ((pdfDict3 != null) ? ((PdfDict)pdfDict3.GetObjectByName("Font")) : null);
							}
						}
						if (pdfDict2 != null && objectByName2.m_nType == enumType.pdfDictionary)
						{
							PdfDict pdfDict4 = (PdfDict)objectByName3;
							for (int i = 0; i < pdfDict2.Size; i++)
							{
								PdfObject @object = pdfDict2.GetObject(i);
								if (@object != null && @object.m_nType == enumType.pdfRef && @object.m_bstrType.Length > 0 && pdfDict4.GetObjectByName(@object.m_bstrType) == null)
								{
									pdfDict4.Add(@object.Copy());
								}
							}
						}
						this.InsertDefaultAppearanceString(pdfIndirectObj, pField.m_bstrFieldDefaultAppearance);
					}
					PdfGraphics pdfGraphics = this.m_pDoc.CreateGraphics("left=0;right=0;top=0;bottom=0");
					PdfIndirectObj pXObj = pdfGraphics.m_pXObj;
					pXObj.m_bExisting = true;
					pXObj.m_bModified = false;
					pXObj.m_nObjNumber = ((PdfRef)objectByName).m_nObjNum;
					pXObj.m_nGenNumber = ((PdfRef)objectByName).m_nGenNum;
					PdfCanvas canvas = pField.m_pPage.Canvas;
					PdfParam pdfParam = new PdfParam();
					pdfParam["X"] = Math.Min(pField.m_fX1, pField.m_fX2);
					pdfParam["Y"] = Math.Min(pField.m_fY1, pField.m_fY2);
					canvas.DrawGraphics(pdfGraphics, pdfParam);
					pField.m_pPage.m_pPageObj.m_objAttributes.RemoveByName("Annots");
					pField.m_pPage.m_pPageObj.m_bModified = true;
					return;
				}
				return;
			}
		}
		private void InsertDefaultAppearanceString(PdfIndirectObj pObj, string bstrDA)
		{
			PdfString stream = new PdfString(null, bstrDA);
			for (int i = 0; i < pObj.m_objStream.Length - 1; i++)
			{
				if (pObj.m_objStream[i] == 66 && pObj.m_objStream[i + 1] == 84)
				{
					PdfStream pdfStream = new PdfStream();
					if (i > 0)
					{
						pdfStream.Append(pObj.m_objStream.ToBytes(), 0, i + 2);
					}
					pdfStream.AppendChar(13);
					pdfStream.AppendChar(10);
					pdfStream.Append(stream);
					pdfStream.Append(pObj.m_objStream.ToBytes(), i + 2, pObj.m_objStream.Length - i - 2);
					pObj.m_objStream.Set(pdfStream);
					pObj.m_bModified = true;
				}
			}
		}
		private bool IsEmpty(PdfStream pStream)
		{
			if (pStream.Length == 0)
			{
				return true;
			}
			int num = PdfStream.FindStringIgnoreCase(pStream.ToBytes(), 0, Encoding.UTF8.GetBytes("/Tx BMC"));
			if (num > -1)
			{
				for (int i = num; i < num + 7; i++)
				{
					pStream[i] = 32;
				}
			}
			int num2 = PdfStream.FindStringIgnoreCase(pStream.ToBytes(), 0, Encoding.UTF8.GetBytes("EMC"));
			if (num2 > -1)
			{
				for (int j = num2; j < num2 + 3; j++)
				{
					pStream[j] = 32;
				}
			}
			for (int k = 0; k < pStream.Length; k++)
			{
				if (!PdfCanvas.IsSpace((char)pStream[k]))
				{
					return false;
				}
			}
			return true;
		}
	}
}
