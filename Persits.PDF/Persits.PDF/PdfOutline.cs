using System;
using System.Collections;
using System.Collections.Generic;
namespace Persits.PDF
{
	public class PdfOutline : IEnumerable
	{
		internal PdfManager m_pManager;
		internal PdfDocument m_pDoc;
		internal PdfIndirectObj m_pOutlineObj;
		internal PdfIndirectObj m_pFirstObj;
		internal PdfIndirectObj m_pLastObj;
		internal List<PdfOutlineItem> m_arrItems = new List<PdfOutlineItem>();
		internal int m_nCount;
		public PdfOutlineItem this[int index]
		{
			get
			{
				if (index < 1 && index > this.m_arrItems.Count)
				{
					AuxException.Throw("Index out of range.", PdfErrors._ERROR_OUTOFRANGE);
				}
				return this.m_arrItems[index - 1];
			}
		}
		public int Count
		{
			get
			{
				return this.m_arrItems.Count;
			}
		}
		internal PdfOutline()
		{
		}
		public IEnumerator GetEnumerator()
		{
			return this.m_arrItems.GetEnumerator();
		}
		internal void Create()
		{
			this.m_pOutlineObj = this.m_pDoc.AddNewIndirectObject(enumIndirectType.pdfIndirectOutline);
			this.m_pOutlineObj.AddName("Type", "Outlines");
			this.m_pDoc.m_pCatalogObj.AddReference("Outlines", this.m_pOutlineObj);
		}
		internal void AdjustEntries(PdfIndirectObj pFirst, PdfIndirectObj pLast, int nCount)
		{
			if (pFirst != null)
			{
				this.m_pOutlineObj.m_objAttributes.RemoveByName("First");
				this.m_pOutlineObj.AddReference("First", pFirst);
				this.m_pFirstObj = pFirst;
			}
			if (pLast != null)
			{
				this.m_pOutlineObj.m_objAttributes.RemoveByName("Last");
				this.m_pOutlineObj.AddReference("Last", pLast);
				this.m_pLastObj = pLast;
			}
			if (nCount >= 0)
			{
				this.m_pOutlineObj.AddInt("Count", nCount);
			}
			this.m_pOutlineObj.m_bModified = true;
		}
		private PdfOutlineItem FindPrevious(PdfIndirectObj pParentObj, PdfOutlineItem pItem)
		{
			foreach (PdfOutlineItem current in this.m_arrItems)
			{
				if (current.m_pParentObj == pParentObj && current.m_pNextObj == pItem.m_pItemObj)
				{
					return current;
				}
			}
			return null;
		}
		private PdfOutlineItem FindLast(PdfIndirectObj pParentObj)
		{
			PdfOutlineItem result = null;
			foreach (PdfOutlineItem current in this.m_arrItems)
			{
				if (current.m_pParentObj == pParentObj)
				{
					result = current;
				}
			}
			return result;
		}
		internal PdfOutlineItem AddExisting(PdfDict pItemDict, int nObjNum, int nGenNum, int nLevel)
		{
			PdfOutlineItem pdfOutlineItem = new PdfOutlineItem();
			pdfOutlineItem.m_pManager = this.m_pManager;
			pdfOutlineItem.m_pDoc = this.m_pDoc;
			pdfOutlineItem.m_pOutline = this;
			pdfOutlineItem.m_nLevel = nLevel;
			this.m_arrItems.Add(pdfOutlineItem);
			pdfOutlineItem.m_pItemObj = this.m_pDoc.AddExistingIndirectObject(enumIndirectType.pdfIndirectOutlineItem, nObjNum, nGenNum, pItemDict);
			pdfOutlineItem.m_pItemObj.m_objAttributes.m_bstrType = null;
			PdfObject objectByName = pItemDict.GetObjectByName("Title");
			if (objectByName != null && objectByName.m_nType == enumType.pdfString)
			{
				pdfOutlineItem.m_bstrTitle = ((PdfString)objectByName).ToString();
			}
			objectByName = pItemDict.GetObjectByName("Count");
			if (objectByName != null && objectByName.m_nType == enumType.pdfNumber)
			{
				pdfOutlineItem.m_nCount = (int)((PdfNumber)objectByName).m_fValue;
			}
			objectByName = pItemDict.GetObjectByName("F");
			if (objectByName != null && objectByName.m_nType == enumType.pdfNumber)
			{
				pdfOutlineItem.m_nFlags = (int)((PdfNumber)objectByName).m_fValue;
			}
			return pdfOutlineItem;
		}
		public PdfOutlineItem Add(string Title, PdfDest Dest, PdfOutlineItem Parent, PdfOutlineItem InsertBefore)
		{
			return this.Add(Title, Dest, Parent, InsertBefore, null);
		}
		public PdfOutlineItem Add(string Title, PdfDest Dest, PdfOutlineItem Parent, PdfOutlineItem InsertBefore, object Param)
		{
			PdfOutlineItem pdfOutlineItem = new PdfOutlineItem();
			pdfOutlineItem.m_pManager = this.m_pManager;
			pdfOutlineItem.m_pDoc = this.m_pDoc;
			pdfOutlineItem.m_pOutline = this;
			pdfOutlineItem.m_bstrTitle = Title;
			pdfOutlineItem.m_pItemObj = this.m_pDoc.AddNewIndirectObject(enumIndirectType.pdfIndirectOutlineItem);
			pdfOutlineItem.SetParams(Param);
			pdfOutlineItem.m_nIndex = this.m_arrItems.Count + 1;
			PdfOutlineItem pdfOutlineItem2 = null;
			PdfOutlineItem pdfOutlineItem3 = null;
			foreach (PdfOutlineItem current in this.m_arrItems)
			{
				if (Parent != null && current == Parent)
				{
					pdfOutlineItem2 = current;
				}
				if (InsertBefore != null && current == InsertBefore)
				{
					pdfOutlineItem3 = current;
				}
			}
			if (this.m_arrItems.Count == 0)
			{
				this.AdjustEntries(pdfOutlineItem.m_pItemObj, null, -1);
			}
			if (pdfOutlineItem2 == null)
			{
				pdfOutlineItem.m_pParentObj = this.m_pOutlineObj;
			}
			else
			{
				pdfOutlineItem.m_pParentObj = pdfOutlineItem2.m_pItemObj;
				pdfOutlineItem.m_nLevel = pdfOutlineItem2.m_nLevel + 1;
			}
			if (pdfOutlineItem3 == null)
			{
				PdfOutlineItem pdfOutlineItem4 = this.FindLast(pdfOutlineItem.m_pParentObj);
				pdfOutlineItem.m_pPrevObj = ((pdfOutlineItem4 == null) ? null : pdfOutlineItem4.m_pItemObj);
				this.AdjustEntries(null, pdfOutlineItem.m_pItemObj, -1);
				if (pdfOutlineItem4 != null)
				{
					pdfOutlineItem4.AdjustEntries(null, null, pdfOutlineItem.m_pItemObj, null, null, -1);
				}
				if (pdfOutlineItem2 != null)
				{
					pdfOutlineItem2.AdjustEntries(null, null, null, (pdfOutlineItem2.m_nCount == 0) ? pdfOutlineItem.m_pItemObj : null, pdfOutlineItem.m_pItemObj, pdfOutlineItem2.m_nCount + 1);
				}
			}
			else
			{
				pdfOutlineItem.m_pNextObj = pdfOutlineItem3.m_pItemObj;
				PdfOutlineItem pdfOutlineItem5 = this.FindPrevious(pdfOutlineItem.m_pParentObj, pdfOutlineItem3);
				pdfOutlineItem.m_pPrevObj = ((pdfOutlineItem5 == null) ? null : pdfOutlineItem5.m_pItemObj);
				if (this.m_pFirstObj == pdfOutlineItem3.m_pItemObj)
				{
					this.AdjustEntries(pdfOutlineItem.m_pItemObj, null, -1);
				}
				if (pdfOutlineItem5 != null)
				{
					pdfOutlineItem5.AdjustEntries(null, null, pdfOutlineItem.m_pItemObj, null, null, -1);
				}
				pdfOutlineItem3.AdjustEntries(null, pdfOutlineItem.m_pItemObj, null, null, null, -1);
				if (pdfOutlineItem2 != null)
				{
					pdfOutlineItem2.AdjustEntries(null, null, null, (pdfOutlineItem5 == null) ? pdfOutlineItem.m_pItemObj : null, (pdfOutlineItem2.m_nCount == 0) ? pdfOutlineItem.m_pItemObj : null, pdfOutlineItem2.m_nCount + 1);
				}
			}
			pdfOutlineItem.Create();
			this.AdjustEntries(null, null, this.m_arrItems.Count + 1);
			this.m_arrItems.Add(pdfOutlineItem);
			if (Dest != null)
			{
				pdfOutlineItem.SetDest(Dest);
			}
			return pdfOutlineItem;
		}
		internal void ArrangeExistingItems()
		{
			PdfObject objectByName = this.m_pOutlineObj.m_objAttributes.GetObjectByName("Count");
			if (objectByName != null && objectByName.m_nType == enumType.pdfNumber)
			{
				this.m_nCount = (int)((PdfNumber)objectByName).m_fValue;
			}
			this.m_pFirstObj = this.FindExisting(this.m_pOutlineObj.m_objAttributes.GetObjectByName("First"));
			this.m_pLastObj = this.FindExisting(this.m_pOutlineObj.m_objAttributes.GetObjectByName("Last"));
			foreach (PdfOutlineItem current in this.m_arrItems)
			{
				current.m_pNextObj = this.FindExisting(current.m_pItemObj.m_objAttributes.GetObjectByName("Next"));
				current.m_pPrevObj = this.FindExisting(current.m_pItemObj.m_objAttributes.GetObjectByName("Prev"));
				current.m_pParentObj = this.FindExisting(current.m_pItemObj.m_objAttributes.GetObjectByName("Parent"));
				current.m_pFirstChildObj = this.FindExisting(current.m_pItemObj.m_objAttributes.GetObjectByName("First"));
				current.m_pLastChildObj = this.FindExisting(current.m_pItemObj.m_objAttributes.GetObjectByName("Last"));
			}
		}
		private PdfIndirectObj FindExisting(PdfObject pObj)
		{
			if (pObj == null)
			{
				return null;
			}
			if (pObj.m_nType != enumType.pdfRef)
			{
				return null;
			}
			PdfRef pdfRef = (PdfRef)pObj;
			if (pdfRef.m_nObjNum == this.m_pOutlineObj.m_nObjNumber && pdfRef.m_nGenNum == this.m_pOutlineObj.m_nGenNumber)
			{
				return this.m_pOutlineObj;
			}
			foreach (PdfOutlineItem current in this.m_arrItems)
			{
				if (current.m_pItemObj.m_nObjNumber == pdfRef.m_nObjNum && current.m_pItemObj.m_nGenNumber == pdfRef.m_nGenNum)
				{
					return current.m_pItemObj;
				}
			}
			return null;
		}
		public void Clear()
		{
			this.m_arrItems.Clear();
			this.m_pOutlineObj.m_objAttributes.RemoveByName("First");
			this.m_pOutlineObj.m_objAttributes.RemoveByName("Last");
			this.m_pOutlineObj.m_objAttributes.RemoveByName("Count");
			this.m_pOutlineObj.m_bModified = true;
		}
	}
}
