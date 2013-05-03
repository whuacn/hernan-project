using System;
using System.Collections.Generic;
namespace Persits.PDF
{
	internal class HtmlCSSSelector
	{
		public TagType m_nTag;
		public string m_bstrID;
		public string m_bstrPseudo;
		public List<string> m_arrClasses = new List<string>();
		public CssSymbol m_nDelimiter;
		public HtmlCSSSelector()
		{
			this.m_nTag = TagType.TAG_UNKNOWN;
			this.m_nDelimiter = CssSymbol.cssSpace;
		}
		public HtmlCSSSelector(HtmlCSSSelector Selector)
		{
			this.m_nTag = Selector.m_nTag;
			this.m_nDelimiter = Selector.m_nDelimiter;
			this.m_bstrID = Selector.m_bstrID;
			this.m_bstrPseudo = Selector.m_bstrPseudo;
			foreach (string current in Selector.m_arrClasses)
			{
				this.m_arrClasses.Add(current);
			}
		}
	}
}
