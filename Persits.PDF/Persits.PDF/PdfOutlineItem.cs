using System;
namespace Persits.PDF
{
	public class PdfOutlineItem
	{
		internal PdfManager m_pManager;
		internal PdfDocument m_pDoc;
		internal PdfOutline m_pOutline;
		internal PdfIndirectObj m_pItemObj;
		internal PdfIndirectObj m_pParentObj;
		internal PdfIndirectObj m_pPrevObj;
		internal PdfIndirectObj m_pNextObj;
		internal PdfIndirectObj m_pFirstChildObj;
		internal PdfIndirectObj m_pLastChildObj;
		internal int m_nCount;
		internal string m_bstrTitle;
		internal int m_nLevel;
		internal int m_nIndex;
		internal bool m_bExpanded;
		internal int m_nFlags;
		private bool m_bActionSet;
		private bool m_bDestSet;
		public string Title
		{
			get
			{
				return this.m_bstrTitle;
			}
			set
			{
				this.m_bstrTitle = value;
				this.m_pItemObj.AddString("Title", this.m_bstrTitle);
				this.m_pItemObj.m_bModified = true;
			}
		}
		public int Level
		{
			get
			{
				return this.m_nLevel;
			}
		}
		public int Index
		{
			get
			{
				return this.m_nIndex;
			}
		}
		public PdfOutline Children
		{
			get
			{
				PdfOutline pdfOutline = new PdfOutline();
				pdfOutline.m_pManager = this.m_pManager;
				pdfOutline.m_pDoc = this.m_pDoc;
				foreach (PdfOutlineItem current in this.m_pOutline.m_arrItems)
				{
					if (current.m_pParentObj == this.m_pItemObj)
					{
						pdfOutline.m_arrItems.Add(current);
					}
				}
				return pdfOutline;
			}
		}
		internal PdfOutlineItem()
		{
			this.m_nLevel = 0;
			this.m_nIndex = 0;
			this.m_pItemObj = null;
			this.m_pParentObj = (this.m_pPrevObj = (this.m_pNextObj = (this.m_pFirstChildObj = (this.m_pLastChildObj = null))));
			this.m_nCount = 0;
			this.m_bExpanded = true;
			this.m_bActionSet = false;
			this.m_bDestSet = false;
			this.m_nFlags = 0;
		}
		internal void Create()
		{
			this.m_pItemObj.AddString("Title", this.m_bstrTitle);
			this.AdjustEntries(this.m_pParentObj, this.m_pPrevObj, this.m_pNextObj, this.m_pFirstChildObj, this.m_pLastChildObj, this.m_nCount);
		}
		internal void AdjustEntries(PdfIndirectObj pParentObj, PdfIndirectObj pPrevObj, PdfIndirectObj pNextObj, PdfIndirectObj pFirstChildObj, PdfIndirectObj pLastChildObj, int nCount)
		{
			if (pParentObj != null)
			{
				this.m_pItemObj.m_objAttributes.Add(new PdfReference("Parent", pParentObj));
			}
			if (pPrevObj != null)
			{
				this.m_pItemObj.m_objAttributes.Add(new PdfReference("Prev", pPrevObj));
			}
			if (pNextObj != null)
			{
				this.m_pItemObj.m_objAttributes.Add(new PdfReference("Next", pNextObj));
			}
			if (pFirstChildObj != null)
			{
				this.m_pItemObj.m_objAttributes.Add(new PdfReference("First", pFirstChildObj));
			}
			if (pLastChildObj != null)
			{
				this.m_pItemObj.m_objAttributes.Add(new PdfReference("Last", pLastChildObj));
			}
			if (nCount != 0)
			{
				this.m_pItemObj.AddInt("Count", this.m_bExpanded ? Math.Abs(nCount) : (-Math.Abs(nCount)));
				this.m_nCount = nCount;
			}
			this.m_pItemObj.m_bModified = true;
		}
		public void SetParams(object Param)
		{
			PdfParam pdfParam = this.m_pManager.VariantToParam(Param);
			if (pdfParam.IsSet("Expanded"))
			{
				this.m_bExpanded = pdfParam.Bool("Expanded");
				this.AdjustEntries(null, null, null, null, null, this.m_nCount);
			}
			int num = this.m_nFlags;
			if (pdfParam.IsSet("Italic"))
			{
				if (pdfParam.IsTrue("Italic"))
				{
					num |= 1;
				}
				else
				{
					num &= -2;
				}
			}
			if (pdfParam.IsSet("Bold"))
			{
				if (pdfParam.IsTrue("Bold"))
				{
					num |= 2;
				}
				else
				{
					num &= -3;
				}
			}
			if (this.m_nFlags != num)
			{
				this.m_pItemObj.AddInt("F", num);
				this.m_nFlags = num;
			}
			if (pdfParam.IsSet("Color") || (pdfParam.IsSet("R") && pdfParam.IsSet("G") && pdfParam.IsSet("B")))
			{
				this.m_pItemObj.m_objAttributes.RemoveByName("C");
				PdfArray pdfArray = this.m_pItemObj.AddArray("C");
				if (pdfParam.IsSet("Color"))
				{
					AuxRGB auxRGB = new AuxRGB();
					auxRGB.Set((uint)pdfParam.Long("Color"));
					pdfArray.Add(new PdfNumber(null, (double)auxRGB.r));
					pdfArray.Add(new PdfNumber(null, (double)auxRGB.g));
					pdfArray.Add(new PdfNumber(null, (double)auxRGB.b));
				}
				else
				{
					pdfArray.Add(new PdfNumber(null, (double)pdfParam.Number("R")));
					pdfArray.Add(new PdfNumber(null, (double)pdfParam.Number("G")));
					pdfArray.Add(new PdfNumber(null, (double)pdfParam.Number("B")));
				}
			}
			this.m_pItemObj.m_bModified = true;
		}
		public void SetAction(PdfAction Action)
		{
			if (this.m_bDestSet)
			{
				AuxException.Throw("Action cannot be set on this item because a Destination is already set on it.", PdfErrors._ERROR_INVALIDARG);
			}
			if (Action == null)
			{
				AuxException.Throw("Action argument is empty.", PdfErrors._ERROR_INVALIDARG);
			}
			if (!Action.IsValid)
			{
				AuxException.Throw("This type of Action requires a destination.", PdfErrors._ERROR_INVALIDARG);
			}
			this.m_pItemObj.m_objAttributes.RemoveByName("A");
			this.m_pItemObj.m_objAttributes.RemoveByName("Dest");
			this.m_pItemObj.AddReference("A", Action.m_pActionObj);
			this.m_bActionSet = true;
			this.m_pItemObj.m_bModified = true;
		}
		public void SetDest(PdfDest Dest)
		{
			if (this.m_bActionSet)
			{
				AuxException.Throw("Destination cannot be set on this item because an Action is already set on it.", PdfErrors._ERROR_INVALIDARG);
			}
			if (Dest == null)
			{
				AuxException.Throw("Destination argument is empty.", PdfErrors._ERROR_INVALIDARG);
			}
			this.m_pItemObj.m_objAttributes.RemoveByName("A");
			this.m_pItemObj.m_objAttributes.RemoveByName("Dest");
			PdfArray pArray = this.m_pItemObj.AddArray("Dest");
			Dest.Populate(pArray, false);
			this.m_bDestSet = true;
			this.m_pItemObj.m_bModified = true;
		}
	}
}
