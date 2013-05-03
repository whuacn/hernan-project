using System;
namespace Persits.PDF
{
	internal abstract class HtmlObject
	{
		protected float m_fX;
		protected float m_fY;
		protected float m_fWidth;
		protected float m_fHeight;
		public HtmlTreeItem m_pTreeItem;
		public float m_fOldWidth;
		public float m_fOldHeight;
		public float m_fOldDescent;
		public bool m_bIsSpace;
		public bool m_bAlreadyRendered;
		public bool m_bPageBreakBefore;
		public bool m_bPageBreakAfter;
		public HtmlManager m_pHtmlManager;
		public bool m_bAbsolutePosition;
		public bool m_bAbsoluteLeft;
		public bool m_bAbsoluteTop;
		public bool m_bDisplayNone;
		public uint m_dwAlignFlags;
		private int m_nStatePtr;
		private char m_cStateChar;
		public char m_c
		{
			get
			{
				return this.m_pHtmlManager.m_c;
			}
			set
			{
				this.m_pHtmlManager.m_c = value;
			}
		}
		public int m_nPtr
		{
			get
			{
				return this.m_pHtmlManager.m_nPtr;
			}
			set
			{
				this.m_pHtmlManager.m_nPtr = value;
			}
		}
		public float X
		{
			get
			{
				return this.m_fX;
			}
			set
			{
				if (this.m_bAbsoluteLeft)
				{
					return;
				}
				this.m_fX = value;
			}
		}
		public float Y
		{
			get
			{
				return this.m_fY;
			}
			set
			{
				if (this.m_bAbsoluteTop)
				{
					return;
				}
				this.m_fY = value;
			}
		}
		public float Width
		{
			get
			{
				if (this.m_bAbsolutePosition || this.m_bDisplayNone)
				{
					return 0f;
				}
				return this.m_fWidth;
			}
			set
			{
				this.m_fWidth = value;
			}
		}
		public float Height
		{
			get
			{
				if (this.m_bAbsolutePosition || this.m_bDisplayNone)
				{
					return 0f;
				}
				return this.m_fHeight;
			}
			set
			{
				this.m_fHeight = value;
			}
		}
		public abstract ObjectType Type
		{
			get;
		}
		public virtual HtmlAttributes HorizAlignment
		{
			get
			{
				return HtmlAttributes.htmlNone;
			}
		}
		public HtmlObject(HtmlManager pMgr)
		{
			this.m_pHtmlManager = pMgr;
			this.m_bIsSpace = false;
			this.m_dwAlignFlags = 0u;
			this.m_fX = (this.m_fY = 0f);
			this.m_fOldWidth = 0f;
			this.m_bAlreadyRendered = false;
			this.m_bPageBreakBefore = (this.m_bPageBreakAfter = false);
			this.m_bAbsolutePosition = (this.m_bAbsoluteLeft = (this.m_bAbsoluteTop = false));
			this.m_bDisplayNone = false;
			if (this.m_pHtmlManager != null)
			{
				this.m_pTreeItem = this.m_pHtmlManager.m_objCSS.m_pCurrentLeaf;
				return;
			}
			this.m_pTreeItem = null;
		}
		public void chgX(float fDelta)
		{
			if (this.m_bAbsoluteLeft)
			{
				return;
			}
			this.m_fX += fDelta;
		}
		public void chgY(float fDelta)
		{
			if (this.m_bAbsoluteTop)
			{
				return;
			}
			this.m_fY += fDelta;
		}
		public void chgXY(float fDeltaX, float fDeltaY)
		{
			this.m_fX += fDeltaX;
			this.m_fY += fDeltaY;
		}
		public void chgWidth(float fDelta)
		{
			this.m_fWidth += fDelta;
		}
		public void chgHeight(float fDelta)
		{
			this.m_fHeight += fDelta;
		}
		public void ObtainPageBreakStatus()
		{
			if (this.m_pTreeItem != null)
			{
				CssProperties cssProperties = CssProperties.NOTFOUND;
				if (this.m_pTreeItem.GetEnumProperty(CssProperties.PAGE_BREAK_BEFORE, ref cssProperties, false))
				{
					this.m_bPageBreakBefore = (cssProperties == CssProperties.V_ALWAYS);
				}
				if (this.m_pTreeItem.GetEnumProperty(CssProperties.PAGE_BREAK_AFTER, ref cssProperties, false))
				{
					this.m_bPageBreakAfter = (cssProperties == CssProperties.V_ALWAYS);
				}
			}
		}
		public static bool IsSpace(char c)
		{
			return c == ' ' || c == '\r' || c == '\n' || c == '\t';
		}
		public static bool IsLetter(char c)
		{
			return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
		}
		public static void Trim(ref string s)
		{
			string text = " \r\n\t";
			string text2 = s.Trim(text.ToCharArray());
			s = text2;
		}
		public abstract void CalculateSize();
		public abstract void Parse();
		public abstract void AssignCoordinates();
		public PeekValues Peek()
		{
			PeekValues peekValues = PeekValues.PEEK_NOTATAG;
			this.SaveState();
			HtmlTag htmlTag = new HtmlTag(this.m_pHtmlManager);
			if (this.ParseTag(ref htmlTag))
			{
				if (htmlTag.m_bClosing)
				{
					peekValues |= PeekValues.PEEK_CLOSE;
				}
				if (htmlTag.m_nType >= TagType.TAG_B && htmlTag.m_nType <= TagType.TAG_BASEFONT)
				{
					peekValues |= PeekValues.PEEK_FONT;
				}
				else
				{
					peekValues |= PeekValues.PEEK_NONFONT;
				}
			}
			this.RestoreState();
			return peekValues;
		}
		public bool ParseTag(ref HtmlTag pTag)
		{
			this.SaveState();
			HtmlTag htmlTag = new HtmlTag(this.m_pHtmlManager);
			htmlTag.Parse();
			if (htmlTag.m_nType == TagType.TAG_NOTATAG)
			{
				return false;
			}
			htmlTag.CopyTo(ref pTag);
			return true;
		}
		public void SaveState()
		{
			this.m_nStatePtr = this.m_nPtr;
			this.m_cStateChar = this.m_c;
		}
		public void RestoreState()
		{
			this.m_nPtr = this.m_nStatePtr;
			this.m_c = this.m_cStateChar;
		}
		public void Next()
		{
			int nPtr = this.m_pHtmlManager.m_nPtr;
			if (nPtr >= this.m_pHtmlManager.m_nBufferSize)
			{
				throw new PdfException("Reached EOF.", 55);
			}
			this.m_c = this.m_pHtmlManager.m_pBuffer[this.m_pHtmlManager.m_nPtr++];
			if (this.m_c == '<' && this.m_pHtmlManager.m_pBuffer[this.m_pHtmlManager.m_nPtr] == '!' && this.m_pHtmlManager.m_nPtr < this.m_pHtmlManager.m_nBufferSize - 2 && this.m_pHtmlManager.m_pBuffer[this.m_pHtmlManager.m_nPtr + 1] == '-' && this.m_pHtmlManager.m_pBuffer[this.m_pHtmlManager.m_nPtr + 2] == '-')
			{
				int num = PdfStream.FindString(this.m_pHtmlManager.m_pBuffer, this.m_pHtmlManager.m_nPtr + 1, "-->".ToCharArray());
				if (num != -1)
				{
					this.m_pHtmlManager.m_nPtr = num + 3;
					this.Next();
					return;
				}
				while (this.m_pHtmlManager.m_nPtr < this.m_pHtmlManager.m_nBufferSize && this.m_pHtmlManager.m_pBuffer[this.m_pHtmlManager.m_nPtr] != '>')
				{
					this.m_pHtmlManager.m_nPtr++;
				}
				this.Next();
				this.Next();
			}
		}
		public virtual bool Render(PdfPage pPage, PdfCanvas pCanvas, float fShiftX, float fShiftY)
		{
			if (this.m_bAlreadyRendered)
			{
				return false;
			}
			this.HandlePageBreakBefore(fShiftY);
			if (this.m_bDisplayNone)
			{
				this.m_bAlreadyRendered = true;
				return false;
			}
			if (this.m_fY + fShiftY + this.m_fHeight < this.m_pHtmlManager.m_fFrameY + this.m_pHtmlManager.m_fFrameHeight)
			{
				this.m_bAlreadyRendered = true;
				return true;
			}
			this.m_pHtmlManager.m_bNonRenderedItemsExist = true;
			if (this.m_fHeight >= this.m_pHtmlManager.m_fFrameHeight)
			{
				return true;
			}
			if (this.m_pHtmlManager.m_bSplitImages && this.Type == ObjectType.htmlImage)
			{
				return true;
			}
			if (this.m_fY + fShiftY >= this.m_pHtmlManager.m_fFrameY && this.m_fY + fShiftY < this.m_pHtmlManager.m_fNextFrameY)
			{
				this.m_pHtmlManager.m_fNextFrameY = this.m_fY + fShiftY;
			}
			return false;
		}
		public void HandlePageBreakBefore(float fShiftY)
		{
			if (this.m_bPageBreakBefore && this.m_fY + fShiftY >= this.m_pHtmlManager.m_fFrameY && this.m_fY + fShiftY <= this.m_pHtmlManager.m_fFrameY + this.m_pHtmlManager.m_fFrameHeight)
			{
				this.m_bPageBreakBefore = false;
				this.m_pHtmlManager.m_fNextFrameY = this.m_fY + fShiftY;
				this.m_pHtmlManager.m_bNonRenderedItemsExist = true;
				throw new PdfException(PdfErrors._ERROR_PAGEBREAK, 0f);
			}
		}
		public void HandlePageBreakAfter(float fShiftY)
		{
			if (this.m_bAlreadyRendered && this.m_bPageBreakAfter)
			{
				this.m_bPageBreakAfter = false;
				this.m_pHtmlManager.m_fNextFrameY = this.m_fY + fShiftY + this.m_fHeight;
				this.m_pHtmlManager.m_bNonRenderedItemsExist = true;
				throw new PdfException(PdfErrors._ERROR_PAGEBREAK, 0f);
			}
		}
		public virtual void ExpandIfNecessary(float fAvailWidth)
		{
		}
	}
}
