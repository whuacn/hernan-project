using System;
namespace Persits.PDF
{
	internal class HtmlBreak : HtmlWord
	{
		public HtmlAttributes m_nClear;
		public bool m_bParagraph;
		public override ObjectType Type
		{
			get
			{
				return ObjectType.htmlBreak;
			}
		}
		public HtmlBreak(HtmlManager pMgr) : base(pMgr)
		{
			this.m_fWidth = 0f;
			this.m_bParagraph = false;
			this.m_nClear = HtmlAttributes.htmlNone;
		}
	}
}
