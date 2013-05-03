using System;
using System.Collections;
namespace Persits.PDF
{
	public class PdfPages : IEnumerable
	{
		public PdfDocument m_pDoc;
		public PdfManager m_pManager;
		public PdfPage this[int index]
		{
			get
			{
				if (index < 1 && index > this.m_pDoc.m_ptrPages.Count)
				{
					AuxException.Throw("Index out of range.", PdfErrors._ERROR_OUTOFRANGE);
				}
				return this.m_pDoc.m_ptrPages[index - 1];
			}
		}
		public int Count
		{
			get
			{
				return this.m_pDoc.m_ptrPages.Count;
			}
		}
		internal PdfPages()
		{
		}
		public IEnumerator GetEnumerator()
		{
			return this.m_pDoc.m_ptrPages.GetEnumerator();
		}
		internal PdfPage AddExisting(PdfDict pPageDict, PdfObjectList pStack, int nObjNum, int nGenNum)
		{
			PdfPage pdfPage = new PdfPage();
			pdfPage.m_pManager = this.m_pManager;
			pdfPage.m_pDoc = this.m_pDoc;
			pdfPage.m_bExisting = true;
			this.m_pDoc.m_ptrPages.Add(pdfPage);
			pdfPage.m_nIndex = this.m_pDoc.m_ptrPages.Count;
			pdfPage.m_pPageObj = this.m_pDoc.AddExistingIndirectObject(enumIndirectType.pdfIndirectPage, nObjNum, nGenNum, pPageDict);
			PdfObject objectByName = pPageDict.GetObjectByName("Parent");
			if (objectByName != null && objectByName.m_nType == enumType.pdfRef && pStack.Size > 0)
			{
				PdfDict pObj = (PdfDict)pStack.GetObject(pStack.Size - 1);
				pdfPage.m_pParentObj = this.m_pDoc.AddExistingIndirectObject(enumIndirectType.pdfIndirectPages, ((PdfRef)objectByName).m_nObjNum, ((PdfRef)objectByName).m_nGenNum, pObj);
				PdfObject objectByName2 = pdfPage.m_pParentObj.m_objAttributes.GetObjectByName("Kids");
				if (objectByName2.m_nType == enumType.pdfArray)
				{
					pdfPage.m_pPageObj.m_pParentArrayOfKids = (PdfArray)objectByName2;
				}
			}
			PdfObject objectByName3 = pPageDict.GetObjectByName("MediaBox");
			if (objectByName3 == null)
			{
				for (int i = pStack.Size - 1; i >= 0; i--)
				{
					objectByName3 = ((PdfDict)pStack.GetObject(i)).GetObjectByName("MediaBox");
					if (objectByName3 != null)
					{
						break;
					}
				}
			}
			if (objectByName3 != null && objectByName3.m_nType == enumType.pdfArray)
			{
				PdfArray pdfArray = (PdfArray)objectByName3;
				if (pdfArray.Size == 4)
				{
					pdfPage.m_ptrMediaBox.Set(((PdfNumber)pdfArray.GetObject(0)).ToFloat(), ((PdfNumber)pdfArray.GetObject(1)).ToFloat(), ((PdfNumber)pdfArray.GetObject(2)).ToFloat(), ((PdfNumber)pdfArray.GetObject(3)).ToFloat());
					pdfPage.m_ptrCropBox.SetRect(pdfPage.m_ptrMediaBox);
				}
			}
			objectByName3 = pPageDict.GetObjectByName("CropBox");
			if (objectByName3 == null)
			{
				for (int j = pStack.Size - 1; j >= 0; j--)
				{
					objectByName3 = ((PdfDict)pStack.GetObject(j)).GetObjectByName("CropBox");
					if (objectByName3 != null)
					{
						break;
					}
				}
			}
			if (objectByName3 != null && objectByName3.m_nType == enumType.pdfArray)
			{
				PdfArray pdfArray2 = (PdfArray)objectByName3;
				if (pdfArray2.Size == 4)
				{
					pdfPage.m_ptrCropBox.Set(((PdfNumber)pdfArray2.GetObject(0)).ToFloat(), ((PdfNumber)pdfArray2.GetObject(1)).ToFloat(), ((PdfNumber)pdfArray2.GetObject(2)).ToFloat(), ((PdfNumber)pdfArray2.GetObject(3)).ToFloat());
				}
			}
			objectByName3 = pPageDict.GetObjectByName("Rotate");
			if (objectByName3 == null)
			{
				for (int k = pStack.Size - 1; k >= 0; k--)
				{
					objectByName3 = ((PdfDict)pStack.GetObject(k)).GetObjectByName("Rotate");
					if (objectByName3 != null)
					{
						break;
					}
				}
			}
			if (objectByName3 != null && objectByName3.m_nType == enumType.pdfNumber)
			{
				pdfPage.m_nRotate = (int)((PdfNumber)objectByName3).m_fValue;
			}
			return pdfPage;
		}
		public PdfPage Add()
		{
			return this.Add(-1f, -1f, -1);
		}
		public PdfPage Add(int InsertBefore)
		{
			return this.Add(-1f, -1f, InsertBefore);
		}
		public PdfPage Add(float Width, float Height)
		{
			return this.Add(Width, Height, -1);
		}
		public PdfPage Add(float Width, float Height, int InsertBefore)
		{
			PdfPage pdfPage = null;
			if (InsertBefore != -1)
			{
				if (InsertBefore < 1 || InsertBefore > this.m_pDoc.m_ptrPages.Count)
				{
					AuxException.Throw("InsertBefore argument out of range.", PdfErrors._ERROR_OUTOFRANGE);
				}
				pdfPage = this[InsertBefore];
			}
			PdfPage pdfPage2 = new PdfPage();
			pdfPage2.m_pManager = this.m_pManager;
			pdfPage2.m_pDoc = this.m_pDoc;
			pdfPage2.m_pParentObj = ((pdfPage == null) ? this.m_pDoc.m_pPageRootObj : pdfPage.m_pParentObj);
			if (Height != -1f)
			{
				pdfPage2.m_ptrMediaBox.Top = Height;
			}
			if (Width != -1f)
			{
				pdfPage2.m_ptrMediaBox.Right = Width;
			}
			pdfPage2.m_pPageObj = this.m_pDoc.AddNewIndirectObject(enumIndirectType.pdfIndirectPage);
			pdfPage2.m_pPageObj.AddName("Type", "Page");
			pdfPage2.m_pPageObj.AddReference("Parent", pdfPage2.m_pParentObj);
			pdfPage2.m_ptrMediaBox.AddArray("MediaBox", pdfPage2.m_pPageObj);
			if (InsertBefore == -1)
			{
				this.m_pDoc.m_ptrPages.Add(pdfPage2);
			}
			else
			{
				this.m_pDoc.m_ptrPages.Insert(InsertBefore - 1, pdfPage2);
			}
			PdfObject objectByName = pdfPage2.m_pParentObj.m_objAttributes.GetObjectByName("Count");
			if (objectByName == null)
			{
				pdfPage2.m_pParentObj.AddInt("Count", 1);
				PdfObject[] ppPtr = new PdfObject[]
				{
					pdfPage2.m_pPageObj
				};
				pdfPage2.m_pParentObj.AddArrayOfRef("Kids", ppPtr);
			}
			else
			{
				if (objectByName.m_nType == enumType.pdfNumber)
				{
					((PdfNumber)objectByName).m_fValue += 1.0;
				}
				if (pdfPage2.m_pParentObj != this.m_pDoc.m_pPageRootObj)
				{
					this.m_pDoc.m_pPageRootObj.ChangeInt("Count", 1);
					this.m_pDoc.m_pPageRootObj.m_bModified = true;
				}
				PdfObject objectByName2 = pdfPage2.m_pParentObj.m_objAttributes.GetObjectByName("Kids");
				if (objectByName2 != null && objectByName2.m_nType == enumType.pdfArray)
				{
					PdfArray pdfArray = (PdfArray)objectByName2;
					if (InsertBefore == -1)
					{
						pdfArray.Add(new PdfReference(null, pdfPage2.m_pPageObj));
					}
					else
					{
						for (int i = 0; i < pdfArray.Size; i++)
						{
							PdfObject @object = pdfArray.GetObject(i);
							if (pdfArray != null && @object.m_nType == enumType.pdfReference && ((PdfReference)@object).m_pPtr == pdfPage.m_pPageObj)
							{
								pdfArray.Insert(new PdfReference(null, pdfPage2.m_pPageObj), i);
								break;
							}
							if (pdfArray != null && @object.m_nType == enumType.pdfRef && ((PdfRef)@object).m_nObjNum == pdfPage.m_pPageObj.m_nObjNumber && ((PdfRef)@object).m_nGenNum == pdfPage.m_pPageObj.m_nGenNumber)
							{
								pdfArray.Insert(new PdfReference(null, pdfPage2.m_pPageObj), i);
								break;
							}
						}
					}
					pdfArray.m_nItemsPerLine = 10;
				}
			}
			pdfPage2.m_pParentObj.m_bModified = true;
			pdfPage2.m_pContentsObj = this.m_pDoc.AddNewIndirectObject(enumIndirectType.pdfIndirectContents);
			pdfPage2.m_pPageObj.AddReference("Contents", pdfPage2.m_pContentsObj);
			pdfPage2.m_ptrCanvas = pdfPage2.CreateCanvas(pdfPage2.m_pContentsObj);
			return pdfPage2;
		}
		public void Remove(int Index)
		{
			if (Index < 1 || Index > this.m_pDoc.m_ptrPages.Count)
			{
				AuxException.Throw("Index out of range.", PdfErrors._ERROR_OUTOFRANGE);
				return;
			}
			PdfPage pdfPage = this.m_pDoc.m_ptrPages[Index - 1];
			if (pdfPage.m_pParentObj != null)
			{
				pdfPage.m_pParentObj.ChangeInt("Count", -1);
				pdfPage.m_pParentObj.m_bModified = true;
				if (pdfPage.m_pParentObj != this.m_pDoc.m_pPageRootObj)
				{
					this.m_pDoc.m_pPageRootObj.ChangeInt("Count", -1);
					this.m_pDoc.m_pPageRootObj.m_bModified = true;
				}
			}
			this.m_pDoc.RemoveIndirectObject(pdfPage.m_pContentsObj);
			this.m_pDoc.RemoveIndirectObject(pdfPage.m_pPageObj);
			this.m_pDoc.m_ptrPages.RemoveAt(Index - 1);
			int i = Index;
			while (i < this.m_pDoc.m_ptrPages.Count)
			{
				this.m_pDoc.m_ptrPages[i++].m_nIndex--;
			}
		}
	}
}
