using System;
namespace Persits.PDF
{
	internal class HtmlCSSProperty
	{
		public CssPropertyType m_nType;
		public CssProperties m_nValue;
		public CssProperties m_nValue2;
		public CssProperties m_nName;
		public float m_fValue;
		public float m_fValue2;
		public bool m_bPercent;
		public AuxRGB m_rgbColor = new AuxRGB();
		public bool m_bImportant;
		public string m_bstrValue;
		public int m_nSpecificity;
		public int m_nOrder;
		public bool m_bAuthorProperty;
		public bool m_bInheritable;
		public bool m_bNoMeasurementUnit;
		public HtmlCSSProperty(CssProperties Name, CssPropertyType Type)
		{
			this.m_nType = Type;
			this.m_nName = Name;
			this.m_fValue = 0f;
			this.m_bPercent = false;
			this.m_bImportant = false;
			this.m_nValue = CssProperties.NOTFOUND;
			this.m_nSpecificity = 1;
			this.m_bAuthorProperty = true;
			this.m_nOrder = 0;
			this.m_bNoMeasurementUnit = false;
		}
		public HtmlCSSProperty(HtmlCSSProperty obj)
		{
			this.m_nType = obj.m_nType;
			this.m_nName = obj.m_nName;
			this.m_nSpecificity = obj.m_nSpecificity;
			this.m_nOrder = obj.m_nOrder;
			this.m_bAuthorProperty = obj.m_bAuthorProperty;
			this.CopyFrom(obj);
		}
		public void CopyFrom(HtmlCSSProperty pProp)
		{
			this.m_nValue = pProp.m_nValue;
			this.m_nValue2 = pProp.m_nValue2;
			this.m_fValue = pProp.m_fValue;
			this.m_fValue2 = pProp.m_fValue2;
			this.m_bPercent = pProp.m_bPercent;
			this.m_rgbColor.Set(ref pProp.m_rgbColor);
			this.m_bImportant = pProp.m_bImportant;
			this.m_bInheritable = pProp.m_bInheritable;
			this.m_bNoMeasurementUnit = pProp.m_bNoMeasurementUnit;
			if (pProp.m_bstrValue != null)
			{
				this.m_bstrValue = pProp.m_bstrValue;
			}
		}
	}
}
