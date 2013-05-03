using System;
using System.Collections.Generic;
namespace Persits.PDF
{
	internal class PdfObjectList : PdfObject
	{
		public PdfObject m_pParent;
		public bool m_bTerminate;
		public List<PdfObject> m_arrItems = new List<PdfObject>();
		public int Size
		{
			get
			{
				return this.m_arrItems.Count;
			}
		}
		public PdfObjectList()
		{
			this.m_pParent = null;
			this.m_bTerminate = false;
		}
		~PdfObjectList()
		{
			this.m_arrItems.Clear();
		}
		public void Add(PdfObject NewObj)
		{
			if (NewObj.m_bstrType != null)
			{
				foreach (PdfObject current in this.m_arrItems)
				{
					if (string.Compare(current.m_bstrType, NewObj.m_bstrType) == 0)
					{
						this.m_arrItems.Remove(current);
						break;
					}
				}
			}
			this.m_arrItems.Add(NewObj);
		}
		public void Add(PdfReference NewObj)
		{
            ((PdfIndirectObj)NewObj.m_pPtr).m_arrBackPointers.Add(this);
            this.Add((PdfObject)NewObj);
		}
		public void Insert(PdfObject NewObj, int Before)
		{
			if (Before < 0 || Before >= this.Size)
			{
				this.Add(NewObj);
			}
			this.m_arrItems.Insert(Before, NewObj);
		}
		public void Insert(PdfReference NewObj, int Before)
		{
			PdfIndirectObj pdfIndirectObj = (PdfIndirectObj)NewObj.m_pPtr;
			pdfIndirectObj.m_arrBackPointers.Add(this);
			this.Insert(NewObj, Before);
		}
		public void RemoveReference(PdfObject ObjToRemove)
		{
			foreach (PdfObject current in this.m_arrItems)
			{
				if (current.m_nType == enumType.pdfReference && ((PdfReference)current).m_pPtr == ObjToRemove)
				{
					this.m_arrItems.Remove(current);
					break;
				}
			}
		}
		public void RemoveRef(int nObjNum, int nGenNum)
		{
			foreach (PdfObject current in this.m_arrItems)
			{
				if (current.m_nType == enumType.pdfRef)
				{
					PdfRef pdfRef = (PdfRef)current;
					if (pdfRef.m_nObjNum == nObjNum && pdfRef.m_nGenNum == nGenNum)
					{
						this.m_arrItems.Remove(current);
						break;
					}
				}
			}
		}
		public void RemoveLast()
		{
			if (this.m_arrItems.Count == 0)
			{
				return;
			}
			this.m_arrItems.RemoveAt(this.m_arrItems.Count - 1);
		}
		public void RemoveByName(string Name)
		{
			if (Name == null)
			{
				return;
			}
			foreach (PdfObject current in this.m_arrItems)
			{
				if (string.Compare(current.m_bstrType, Name) == 0)
				{
					this.m_arrItems.Remove(current);
					break;
				}
			}
		}
		public void RemoveByIndex(int Index)
		{
			this.m_arrItems.RemoveAt(Index);
		}
		public PdfObject GetObjectByName(string Name)
		{
			foreach (PdfObject current in this.m_arrItems)
			{
				if (string.Compare(current.m_bstrType, Name) == 0)
				{
					return current;
				}
			}
			return null;
		}
		public override int WriteOut(PdfOutput pOutput)
		{
			int num = 0;
			foreach (PdfObject current in this.m_arrItems)
			{
				num += current.WriteOut(pOutput);
			}
			return num;
		}
		public PdfObject GetObject(int nIndex)
		{
			if (nIndex < 0 || nIndex > this.m_arrItems.Count)
			{
				return null;
			}
			return this.m_arrItems[nIndex];
		}
		public override PdfObject Copy()
		{
			return null;
		}
		public void CopyItems(PdfObjectList obj)
		{
			this.m_bstrType = obj.m_bstrType;
			this.m_nType = obj.m_nType;
			foreach (PdfObject current in obj.m_arrItems)
			{
				PdfObject pdfObject = current.Copy();
				if (pdfObject.m_nType == enumType.pdfArray)
				{
					((PdfArray)pdfObject).m_pParent = this.m_pParent;
				}
				if (pdfObject.m_nType == enumType.pdfDictionary)
				{
					((PdfDict)pdfObject).m_pParent = this.m_pParent;
				}
				this.m_arrItems.Add(pdfObject);
			}
		}
	}
}
