using System;
namespace Persits.PDF
{
	public class PdfRect
	{
		internal float m_fLeft;
		internal float m_fBottom;
		internal float m_fRight;
		internal float m_fTop;
		internal bool m_bIsSet;
		internal PdfIndirectObj m_pHostObj;
		internal string m_bstrName;
		public float Left
		{
			get
			{
				return this.m_fLeft;
			}
			set
			{
				this.m_fLeft = value;
				this.Update();
			}
		}
		public float Right
		{
			get
			{
				return this.m_fRight;
			}
			set
			{
				this.m_fRight = value;
				this.Update();
			}
		}
		public float Top
		{
			get
			{
				return this.m_fTop;
			}
			set
			{
				this.m_fTop = value;
				this.Update();
			}
		}
		public float Bottom
		{
			get
			{
				return this.m_fBottom;
			}
			set
			{
				this.m_fBottom = value;
				this.Update();
			}
		}
		public float Height
		{
			get
			{
				return Math.Abs(this.m_fTop - this.m_fBottom);
			}
			set
			{
				this.Top = this.m_fBottom + value;
			}
		}
		public float Width
		{
			get
			{
				return Math.Abs(this.m_fRight - this.m_fLeft);
			}
			set
			{
				this.Right = this.m_fLeft + value;
			}
		}
		internal PdfRect()
		{
			this.m_fLeft = (this.m_fBottom = (this.m_fRight = (this.m_fTop = 0f)));
			this.m_bIsSet = false;
		}
		public void Set(float fLeft, float fBottom, float fRight, float fTop)
		{
			this.Left = fLeft;
			this.Right = fRight;
			this.Bottom = fBottom;
			this.Top = fTop;
		}
		internal void AddArray(string Name, PdfIndirectObj pHostObj)
		{
			pHostObj.m_objAttributes.RemoveByName(Name);
			PdfArray pdfArray = pHostObj.AddArray(Name);
			pdfArray.Add(new PdfNumber(null, (double)this.m_fLeft));
			pdfArray.Add(new PdfNumber(null, (double)this.m_fBottom));
			pdfArray.Add(new PdfNumber(null, (double)this.m_fRight));
			pdfArray.Add(new PdfNumber(null, (double)this.m_fTop));
			pHostObj.m_bModified = true;
		}
		public void SetRect(PdfRect Rect)
		{
			if (Rect == null)
			{
				return;
			}
			this.Left = Rect.Left;
			this.Bottom = Rect.Bottom;
			this.Right = Rect.Right;
			this.Top = Rect.Top;
		}
		private void Update()
		{
			if (this.m_pHostObj == null)
			{
				return;
			}
			this.AddArray(this.m_bstrName, this.m_pHostObj);
		}
	}
}
