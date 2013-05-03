using System;
using System.Collections.Generic;
namespace Persits.PDF
{
	internal class HtmlCSSRule
	{
		public List<HtmlCSSSelector> m_arrSelectors = new List<HtmlCSSSelector>();
		public List<HtmlCSSProperty> m_arrProperties = new List<HtmlCSSProperty>();
		public bool m_bValid;
		public bool m_bNeedsCopying;
		public HtmlCSSRule()
		{
			this.m_bValid = false;
			this.m_bNeedsCopying = true;
		}
		public HtmlCSSRule(TagType nSelector, CssProperties nProperty, CssPropertyType nType, CssProperties nValue, float fValue)
		{
			this.m_bValid = true;
			this.m_bNeedsCopying = false;
			HtmlCSSSelector htmlCSSSelector = new HtmlCSSSelector();
			htmlCSSSelector.m_nTag = nSelector;
			this.m_arrSelectors.Add(htmlCSSSelector);
			HtmlCSSProperty htmlCSSProperty = new HtmlCSSProperty(nProperty, nType);
			if (nType == CssPropertyType.PROP_ENUM)
			{
				htmlCSSProperty.m_nValue = nValue;
			}
			else
			{
				htmlCSSProperty.m_fValue = fValue;
			}
			htmlCSSProperty.m_nOrder = 0;
			htmlCSSProperty.m_nSpecificity = 1;
			htmlCSSProperty.m_bAuthorProperty = false;
			this.m_arrProperties.Add(htmlCSSProperty);
		}
		public void CopyFrom(HtmlCSSRule pRule, bool bCopySelectors)
		{
			this.m_bValid = pRule.m_bValid;
			this.m_bNeedsCopying = pRule.m_bNeedsCopying;
			foreach (HtmlCSSProperty current in pRule.m_arrProperties)
			{
				HtmlCSSProperty item = new HtmlCSSProperty(current);
				this.m_arrProperties.Add(item);
			}
			if (bCopySelectors)
			{
				foreach (HtmlCSSSelector current2 in pRule.m_arrSelectors)
				{
					HtmlCSSSelector item2 = new HtmlCSSSelector(current2);
					this.m_arrSelectors.Add(item2);
				}
			}
		}
	}
}
