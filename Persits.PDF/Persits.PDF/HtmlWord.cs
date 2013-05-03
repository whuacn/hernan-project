using System;
using System.Collections.Generic;
namespace Persits.PDF
{
	internal class HtmlWord : HtmlObject
	{
		public bool m_bInvisible;
		public float m_fMaxDescent;
		public List<HtmlChunk> m_arrChunks = new List<HtmlChunk>();
		public override ObjectType Type
		{
			get
			{
				return ObjectType.htmlWord;
			}
		}
		public HtmlWord(HtmlManager pMgr) : base(pMgr)
		{
			this.m_fX = (this.m_fY = (this.m_fHeight = (this.m_fWidth = (this.m_fMaxDescent = 0f))));
			this.m_bInvisible = false;
		}
		public override void CalculateSize()
		{
			float num = 0f;
			float num2 = 0f;
			this.m_fMaxDescent = 0f;
			foreach (HtmlChunk current in this.m_arrChunks)
			{
				if (current.Height > num)
				{
					num = current.Height;
				}
				if (current.m_fDescent < this.m_fMaxDescent)
				{
					this.m_fMaxDescent = current.m_fDescent;
				}
				num2 += current.Width;
			}
			this.m_fHeight = num;
			this.m_fWidth = num2;
		}
		public override void Parse()
		{
			HtmlTag pTag = new HtmlTag(this.m_pHtmlManager);
			while (!HtmlObject.IsSpace(base.m_c))
			{
				if (base.m_c == '<')
				{
					PeekValues peekValues = base.Peek();
					if (peekValues != PeekValues.PEEK_NOTATAG && (peekValues & PeekValues.PEEK_FONT) == PeekValues.PEEK_NOTATAG)
					{
						break;
					}
				}
				if (base.m_c == '<' && base.ParseTag(ref pTag))
				{
					this.m_pHtmlManager.m_objCSS.AddToTree(pTag);
				}
				HtmlChunk htmlChunk = this.AddChunk();
				htmlChunk.Parse();
				if (htmlChunk.m_bstrText == null || htmlChunk.m_bstrText.ToString().Length == 0)
				{
					this.RemoveLastChunk();
				}
				else
				{
					this.m_dwAlignFlags = htmlChunk.m_dwFlags;
				}
			}
			this.CalculateSize();
		}
		public HtmlChunk AddChunk()
		{
			HtmlChunk htmlChunk = new HtmlChunk(this.m_pHtmlManager);
			htmlChunk.ObtainStyles();
			this.m_arrChunks.Add(htmlChunk);
			return htmlChunk;
		}
		public override bool Render(PdfPage pPage, PdfCanvas pCanvas, float fShiftX, float fShiftY)
		{
			if (!base.Render(pPage, pCanvas, fShiftX, fShiftY))
			{
				return false;
			}
			if (this.m_bInvisible)
			{
				return true;
			}
			foreach (HtmlChunk current in this.m_arrChunks)
			{
				current.Render(pPage, pCanvas, fShiftX, fShiftY);
			}
			return true;
		}
		private void RemoveLastChunk()
		{
			if (this.m_arrChunks.Count == 0)
			{
				return;
			}
			this.m_arrChunks.RemoveAt(this.m_arrChunks.Count - 1);
		}
		public void MakeInvisible()
		{
			if (this.m_bInvisible)
			{
				return;
			}
			this.m_bInvisible = true;
			this.m_fOldWidth = this.m_fWidth;
			this.m_fOldHeight = this.m_fHeight;
			this.m_fOldDescent = this.m_fMaxDescent;
			this.m_fWidth = (this.m_fHeight = (this.m_fMaxDescent = 0f));
		}
		public override void AssignCoordinates()
		{
			float num = this.m_fX;
			foreach (HtmlChunk current in this.m_arrChunks)
			{
				current.X = num;
				num += current.Width;
				current.Y = this.m_fY + this.m_fHeight - current.Height;
			}
		}
	}
}
