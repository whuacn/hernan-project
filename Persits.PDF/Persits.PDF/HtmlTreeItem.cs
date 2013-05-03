using System;
using System.Collections.Generic;
namespace Persits.PDF
{
	internal class HtmlTreeItem
	{
		public TagType m_nType;
		public HtmlTreeItem m_pParent;
		public List<HtmlTreeItem> m_arrChildren = new List<HtmlTreeItem>();
		public string m_bstrClass;
		public string m_bstrID;
		public List<HtmlCSSProperty> m_arrProperties = new List<HtmlCSSProperty>();
		public uint m_dwTagFlag;
		public HtmlTreeItem()
		{
			this.m_pParent = null;
		}
		public HtmlTreeItem(HtmlTreeItem item) : this(item, false)
		{
		}
		public HtmlTreeItem(HtmlTreeItem item, bool bCopyProperties)
		{
			this.m_nType = item.m_nType;
			this.m_dwTagFlag = item.m_dwTagFlag;
			this.m_pParent = null;
			if (bCopyProperties)
			{
				foreach (HtmlCSSProperty current in item.m_arrProperties)
				{
					HtmlCSSProperty item2 = new HtmlCSSProperty(current);
					this.m_arrProperties.Add(item2);
				}
			}
		}
		public void AddApplicableProperties(HtmlCSS pCSS, HtmlTag pTag)
		{
			List<HtmlCSSProperty> list = new List<HtmlCSSProperty>();
			int nSpecificity = 0;
			int num = 1;
			string text = null;
			pTag.GetStringAttribute("Style", ref text);
			int num2 = 0;
			foreach (HtmlCSSRule current in pCSS.m_arrRules)
			{
				if (this.MatchesSelectors(current, ref nSpecificity))
				{
					foreach (HtmlCSSProperty current2 in current.m_arrProperties)
					{
						HtmlCSSProperty htmlCSSProperty = new HtmlCSSProperty(current2);
						htmlCSSProperty.m_nSpecificity = nSpecificity;
						htmlCSSProperty.m_nOrder = num++;
						list.Add(htmlCSSProperty);
					}
				}
				num2++;
			}
			string text2 = "";
			if (pTag.m_nType == TagType.TAG_A && !pTag.GetStringAttribute("Href", ref text2) && (pTag.GetStringAttribute("Name", ref text2) || pTag.GetStringAttribute("ID", ref text2)))
			{
				list.Clear();
			}
			if (text != null && text.Length > 0)
			{
				HtmlCSSRule pCurrentRule = new HtmlCSSRule();
				pCSS.m_pCurrentRule = pCurrentRule;
				pCSS.ParseProperties(text.ToCharArray(), 0, text.Length);
				foreach (HtmlCSSProperty current3 in pCSS.m_pCurrentRule.m_arrProperties)
				{
					HtmlCSSProperty htmlCSSProperty2 = new HtmlCSSProperty(current3);
					htmlCSSProperty2.m_nSpecificity = 1000;
					htmlCSSProperty2.m_nOrder = num++;
					list.Add(htmlCSSProperty2);
				}
			}
			this.HandleSpecialTagsAndAttributes(pCSS, pTag, ref list);
			this.SortProperties(ref list);
			CssProperties cssProperties = CssProperties.NOTFOUND;
			foreach (HtmlCSSProperty current4 in list)
			{
				if (current4.m_nName != cssProperties)
				{
					this.AddOrReplaceProperty(current4);
				}
				cssProperties = current4.m_nName;
			}
		}
		private void HandleSpecialTagsAndAttributes(HtmlCSS pCSS, HtmlTag pTag, ref List<HtmlCSSProperty> arrProperties)
		{
			if (pTag.m_nType == TagType.TAG_FONT || pTag.m_nType == TagType.TAG_BASEFONT)
			{
				AuxRGB auxRGB = new AuxRGB();
				if (pTag.GetColorAttribute("Color", ref auxRGB))
				{
					HtmlCSSProperty htmlCSSProperty = new HtmlCSSProperty(CssProperties.COLOR, CssPropertyType.PROP_COLOR);
					htmlCSSProperty.m_rgbColor.Set(ref auxRGB);
					arrProperties.Add(htmlCSSProperty);
				}
				string bstrValue = null;
				if (pTag.GetStringAttribute("FACE", ref bstrValue))
				{
					HtmlCSSProperty htmlCSSProperty = new HtmlCSSProperty(CssProperties.FONT_FAMILY, CssPropertyType.PROP_STRING);
					htmlCSSProperty.m_bstrValue = bstrValue;
					arrProperties.Add(htmlCSSProperty);
				}
				int num = 0;
				bool flag = false;
				char c = '\0';
				if (pTag.GetIntAttribute("SIZE", ref num, ref c, ref flag) && (c == '\0' || num != 0))
				{
					HtmlCSSProperty htmlCSSProperty = new HtmlCSSProperty(CssProperties.FONT_SIZE, CssPropertyType.PROP_NUMBER);
					float[] array = new float[]
					{
						0f,
						7.5f,
						10f,
						12f,
						13.5f,
						18f,
						24f,
						36f
					};
					if (c == '\0')
					{
						if (num < 1)
						{
							num = 1;
						}
						if (num > 7)
						{
							num = 7;
						}
						htmlCSSProperty.m_fValue = array[num];
					}
					else
					{
						htmlCSSProperty.m_fValue = this.CalculateFontSize((c == '+') ? CssProperties.V_LARGER : CssProperties.V_SMALLER);
						htmlCSSProperty.m_nValue = CssProperties.NOTFOUND;
					}
					arrProperties.Add(htmlCSSProperty);
				}
			}
			if (pTag.IsOneOf(106384u))
			{
				HtmlAttributes htmlAttributes = HtmlAttributes.htmlNone;
				pTag.GetStandardAttribute("align", ref htmlAttributes);
				if (htmlAttributes != HtmlAttributes.htmlNone)
				{
					CssProperties nValue = CssProperties.V_LEFT;
					if (htmlAttributes == HtmlAttributes.htmlCenter)
					{
						nValue = CssProperties.V_CENTER;
					}
					if (htmlAttributes == HtmlAttributes.htmlMiddle)
					{
						nValue = CssProperties.V_CENTER;
					}
					if (htmlAttributes == HtmlAttributes.htmlRight)
					{
						nValue = CssProperties.V_RIGHT;
					}
					if (htmlAttributes == HtmlAttributes.htmlJustify)
					{
						nValue = CssProperties.V_JUSTIFY;
					}
					if (htmlAttributes == HtmlAttributes.htmlLeft)
					{
						nValue = CssProperties.V_LEFT;
					}
					HtmlCSSProperty htmlCSSProperty = new HtmlCSSProperty(CssProperties.TEXT_ALIGN, CssPropertyType.PROP_ENUM);
					htmlCSSProperty.m_nValue = nValue;
					arrProperties.Add(htmlCSSProperty);
				}
			}
			if (pTag.IsOneOf(72704u))
			{
				HtmlAttributes htmlAttributes2 = HtmlAttributes.htmlNone;
				pTag.GetStandardAttribute("valign", ref htmlAttributes2);
				if (htmlAttributes2 != HtmlAttributes.htmlNone)
				{
					CssProperties nValue2 = CssProperties.V_MIDDLE;
					if (htmlAttributes2 == HtmlAttributes.htmlTop)
					{
						nValue2 = CssProperties.V_TOP;
					}
					if (htmlAttributes2 == HtmlAttributes.htmlBottom)
					{
						nValue2 = CssProperties.V_BOTTOM;
					}
					HtmlCSSProperty htmlCSSProperty = new HtmlCSSProperty(CssProperties.VERTICAL_ALIGN, CssPropertyType.PROP_ENUM);
					htmlCSSProperty.m_nValue = nValue2;
					arrProperties.Add(htmlCSSProperty);
				}
			}
			if (pTag.m_nType == TagType.TAG_BODY)
			{
				AuxRGB auxRGB2 = new AuxRGB();
				if (pTag.GetColorAttribute("text", ref auxRGB2))
				{
					HtmlCSSProperty htmlCSSProperty = new HtmlCSSProperty(CssProperties.COLOR, CssPropertyType.PROP_COLOR);
					htmlCSSProperty.m_rgbColor.Set(ref auxRGB2);
					arrProperties.Add(htmlCSSProperty);
				}
				auxRGB2.Reset();
				if (pTag.GetColorAttribute("link", ref auxRGB2))
				{
					pCSS.m_arrRules[pCSS.m_nHyperLinkColorRuleNumber].m_arrProperties[0].m_rgbColor.Set(ref auxRGB2);
				}
			}
			if (pTag.IsOneOf(7u))
			{
				string text = null;
				if (pTag.GetStringAttribute("type", ref text))
				{
					HtmlCSSProperty htmlCSSProperty = new HtmlCSSProperty(CssProperties.LIST_STYLE_TYPE, CssPropertyType.PROP_ENUM);
					arrProperties.Add(htmlCSSProperty);
					htmlCSSProperty.m_nValue = CssProperties.NOTFOUND;
					string text2 = text;
					text2.ToLower();
					if (text == "i")
					{
						htmlCSSProperty.m_nValue = CssProperties.V_LOWER_ROMAN;
					}
					else
					{
						if (text == "I")
						{
							htmlCSSProperty.m_nValue = CssProperties.V_UPPER_ROMAN;
						}
						else
						{
							if (text == "1")
							{
								htmlCSSProperty.m_nValue = CssProperties.V_DECIMAL;
							}
							else
							{
								if (text == "A")
								{
									htmlCSSProperty.m_nValue = CssProperties.V_UPPER_ALPHA;
								}
								else
								{
									if (text == "a")
									{
										htmlCSSProperty.m_nValue = CssProperties.V_LOWER_ALPHA;
									}
									else
									{
										if (text2 == "circle")
										{
											htmlCSSProperty.m_nValue = CssProperties.V_CIRCLE;
										}
										else
										{
											if (text2 == "square")
											{
												htmlCSSProperty.m_nValue = CssProperties.V_SQUARE;
											}
											else
											{
												if (text2 == "disc")
												{
													htmlCSSProperty.m_nValue = CssProperties.V_DISC;
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			char c2 = ' ';
			bool flag2 = true;
			if (pTag.IsOneOf(171040u))
			{
				int num2 = 0;
				bool flag3 = false;
				if (pTag.GetIntAttribute("width", ref num2, ref c2, ref flag3))
				{
					HtmlCSSProperty htmlCSSProperty = new HtmlCSSProperty(CssProperties.WIDTH, CssPropertyType.PROP_NUMBER);
					htmlCSSProperty.m_fValue = (flag3 ? ((float)num2) : ((float)num2 * HtmlManager.fEmpiricalScaleFactor));
					htmlCSSProperty.m_bPercent = flag3;
					arrProperties.Add(htmlCSSProperty);
				}
				num2 = 0;
				if (pTag.GetIntAttribute("height", ref num2, ref c2, ref flag3))
				{
					HtmlCSSProperty htmlCSSProperty = new HtmlCSSProperty(CssProperties.HEIGHT, CssPropertyType.PROP_NUMBER);
					htmlCSSProperty.m_fValue = (flag3 ? ((float)num2) : ((float)num2 * HtmlManager.fEmpiricalScaleFactor));
					htmlCSSProperty.m_bPercent = flag3;
					arrProperties.Add(htmlCSSProperty);
				}
			}
			if (pTag.m_nType == TagType.TAG_TABLE)
			{
				int num3 = 0;
				if (pTag.GetIntAttribute("cellspacing", ref num3, ref c2, ref flag2))
				{
					HtmlCSSProperty htmlCSSProperty = new HtmlCSSProperty(CssProperties.BORDER_SPACING, CssPropertyType.PROP_NUMBER);
					htmlCSSProperty.m_fValue = (float)num3;
					arrProperties.Add(htmlCSSProperty);
				}
			}
			CssProperties[] array2 = new CssProperties[]
			{
				CssProperties.BORDER_TOP_STYLE,
				CssProperties.BORDER_RIGHT_STYLE,
				CssProperties.BORDER_BOTTOM_STYLE,
				CssProperties.BORDER_LEFT_STYLE
			};
			CssProperties[] array3 = new CssProperties[]
			{
				CssProperties.BORDER_TOP_WIDTH,
				CssProperties.BORDER_RIGHT_WIDTH,
				CssProperties.BORDER_BOTTOM_WIDTH,
				CssProperties.BORDER_LEFT_WIDTH
			};
			if (pTag.m_nType == TagType.TAG_IMG || pTag.m_nType == TagType.TAG_TABLE)
			{
				int num4 = 9999;
				if (pTag.GetIntAttribute("Border", ref num4, ref c2, ref flag2))
				{
					if (num4 == 9999)
					{
						num4 = 1;
					}
					for (int i = 0; i < 4; i++)
					{
						HtmlCSSProperty htmlCSSProperty = new HtmlCSSProperty(array2[i], CssPropertyType.PROP_ENUM);
						htmlCSSProperty.m_nValue = CssProperties.V_SOLID;
						arrProperties.Add(htmlCSSProperty);
						htmlCSSProperty = new HtmlCSSProperty(array3[i], CssPropertyType.PROP_NUMBER);
						htmlCSSProperty.m_fValue = (float)num4 * HtmlManager.fEmpiricalScaleFactor;
						arrProperties.Add(htmlCSSProperty);
					}
				}
				if (pTag.GetIntAttribute("VSpace", ref num4, ref c2, ref flag2))
				{
					HtmlCSSProperty htmlCSSProperty = new HtmlCSSProperty(CssProperties.MARGIN_TOP, CssPropertyType.PROP_NUMBER);
					htmlCSSProperty.m_fValue = (float)num4 * HtmlManager.fEmpiricalScaleFactor;
					arrProperties.Add(htmlCSSProperty);
					htmlCSSProperty = new HtmlCSSProperty(CssProperties.MARGIN_BOTTOM, CssPropertyType.PROP_NUMBER);
					htmlCSSProperty.m_fValue = (float)num4 * HtmlManager.fEmpiricalScaleFactor;
					arrProperties.Add(htmlCSSProperty);
				}
				if (pTag.GetIntAttribute("HSpace", ref num4, ref c2, ref flag2))
				{
					HtmlCSSProperty htmlCSSProperty = new HtmlCSSProperty(CssProperties.MARGIN_LEFT, CssPropertyType.PROP_NUMBER);
					htmlCSSProperty.m_fValue = (float)num4 * HtmlManager.fEmpiricalScaleFactor;
					arrProperties.Add(htmlCSSProperty);
					htmlCSSProperty = new HtmlCSSProperty(CssProperties.MARGIN_RIGHT, CssPropertyType.PROP_NUMBER);
					htmlCSSProperty.m_fValue = (float)num4 * HtmlManager.fEmpiricalScaleFactor;
					arrProperties.Add(htmlCSSProperty);
				}
			}
			if (pTag.m_nType == TagType.TAG_HR)
			{
				int num5 = 0;
				if (pTag.GetIntAttribute("Size", ref num5, ref c2, ref flag2))
				{
					HtmlCSSProperty htmlCSSProperty = new HtmlCSSProperty(CssProperties.HEIGHT, CssPropertyType.PROP_NUMBER);
					htmlCSSProperty.m_fValue = (float)num5 * HtmlManager.fEmpiricalScaleFactor;
					arrProperties.Add(htmlCSSProperty);
				}
			}
			if (pTag.IsOneOf(15392u))
			{
				AuxRGB auxRGB3 = new AuxRGB();
				if (pTag.GetColorAttribute("BgColor", ref auxRGB3))
				{
					HtmlCSSProperty htmlCSSProperty = new HtmlCSSProperty(CssProperties.BACKGROUND_COLOR, CssPropertyType.PROP_COLOR);
					htmlCSSProperty.m_rgbColor.Set(ref auxRGB3);
					arrProperties.Add(htmlCSSProperty);
				}
				string bstrValue2 = null;
				if (pTag.GetStringAttribute("Background", ref bstrValue2))
				{
					HtmlCSSProperty htmlCSSProperty = new HtmlCSSProperty(CssProperties.BACKGROUND_IMAGE, CssPropertyType.PROP_STRING);
					htmlCSSProperty.m_bstrValue = bstrValue2;
					arrProperties.Add(htmlCSSProperty);
				}
			}
			if (pTag.m_nType == TagType.TAG_A)
			{
				string bstrValue3 = null;
				if (pTag.GetStringAttribute("Href", ref bstrValue3))
				{
					HtmlCSSProperty htmlCSSProperty = new HtmlCSSProperty(CssProperties.HREF, CssPropertyType.PROP_STRING);
					htmlCSSProperty.m_bstrValue = bstrValue3;
					arrProperties.Add(htmlCSSProperty);
				}
			}
		}
		private void AddOrReplaceProperty(HtmlCSSProperty pProp)
		{
			HtmlCSSProperty htmlCSSProperty = new HtmlCSSProperty(pProp);
			for (int i = 0; i < this.m_arrProperties.Count; i++)
			{
				if (this.m_arrProperties[i].m_nName == pProp.m_nName)
				{
					HtmlCSSProperty htmlCSSProperty2 = this.m_arrProperties[i];
					this.m_arrProperties[i] = htmlCSSProperty;
					if (pProp.m_bPercent)
					{
						htmlCSSProperty.m_fValue = htmlCSSProperty2.m_fValue * pProp.m_fValue / 100f;
						htmlCSSProperty.m_bPercent = false;
					}
					if (pProp.m_nName == CssProperties.FONT_SIZE && (pProp.m_nValue == CssProperties.V_SMALLER || pProp.m_nValue == CssProperties.V_LARGER))
					{
						htmlCSSProperty.m_nValue = CssProperties.NOTFOUND;
						htmlCSSProperty.m_fValue = this.CalculateFontSize(pProp.m_nValue);
					}
					return;
				}
			}
			float num = 0f;
			if (pProp.m_bPercent && pProp.m_nName == CssProperties.FONT_SIZE)
			{
				if (this.m_pParent == null || !this.m_pParent.GetNumericProperty(pProp.m_nName, ref num, true))
				{
					num = this.GetDefaultValue(pProp.m_nName);
				}
				htmlCSSProperty.m_fValue = num * pProp.m_fValue / 100f;
				htmlCSSProperty.m_nValue = CssProperties.NOTFOUND;
				htmlCSSProperty.m_bPercent = false;
			}
			if (pProp.m_nName == CssProperties.FONT_SIZE && (pProp.m_nValue == CssProperties.V_SMALLER || pProp.m_nValue == CssProperties.V_LARGER))
			{
				htmlCSSProperty.m_fValue = this.CalculateFontSize(pProp.m_nValue);
				htmlCSSProperty.m_nValue = CssProperties.NOTFOUND;
			}
			this.m_arrProperties.Add(htmlCSSProperty);
		}
		private float CalculateFontSize(CssProperties nSmallerLarger)
		{
			float num = 12f;
			if (this.m_pParent != null)
			{
				this.m_pParent.GetNumericProperty(CssProperties.FONT_SIZE, ref num);
			}
			if ((nSmallerLarger == CssProperties.V_LARGER && (num < 8f || num >= 36f)) || (nSmallerLarger == CssProperties.V_SMALLER && (num <= 8f || num > 36f)))
			{
				return num * ((nSmallerLarger == CssProperties.V_LARGER) ? 1.1f : 0.9f);
			}
			int num2 = 1;
			float num3 = 9999f;
			float[] array = new float[]
			{
				0f,
				7.5f,
				10f,
				12f,
				13.5f,
				18f,
				24f,
				36f
			};
			for (int i = 1; i <= 7; i++)
			{
				float num4 = Math.Abs(array[i] - num);
				if (num4 < num3)
				{
					num3 = num4;
					num2 = i;
				}
			}
			if (nSmallerLarger == CssProperties.V_LARGER && num2 < 7)
			{
				num2++;
			}
			if (nSmallerLarger == CssProperties.V_SMALLER && num2 > 1)
			{
				num2--;
			}
			return array[num2];
		}
		private float GetDefaultValue(CssProperties nProp)
		{
			if (nProp == CssProperties.FONT_SIZE)
			{
				return 12f;
			}
			if (nProp == CssProperties.LINE_HEIGHT)
			{
				return 12f;
			}
			return 0f;
		}
		private bool LessThan(HtmlCSSProperty pProp1, HtmlCSSProperty pProp2)
		{
			if (pProp1.m_nName != pProp2.m_nName)
			{
				return pProp1.m_nName < pProp2.m_nName;
			}
			if (pProp1.m_bImportant != pProp2.m_bImportant)
			{
				return !pProp1.m_bImportant;
			}
			if (pProp1.m_bAuthorProperty != pProp2.m_bAuthorProperty)
			{
				return !pProp1.m_bAuthorProperty;
			}
			if (pProp1.m_nSpecificity != pProp2.m_nSpecificity)
			{
				return pProp1.m_nSpecificity < pProp2.m_nSpecificity;
			}
			return pProp1.m_nOrder < pProp2.m_nOrder;
		}
		private void SortProperties(ref List<HtmlCSSProperty> arrProperties)
		{
			int count = arrProperties.Count;
			if (count == 0)
			{
				return;
			}
			int num = 0;
			for (int i = 0; i < count - 1; i++)
			{
				for (int j = count - 1; j > i; j--)
				{
					if (!this.LessThan(arrProperties[num + j], arrProperties[num + j - 1]))
					{
						HtmlCSSProperty value = arrProperties[num + j - 1];
						arrProperties[num + j - 1] = arrProperties[num + j];
						arrProperties[num + j] = value;
					}
				}
			}
		}
		private bool MatchesThisSelector(HtmlCSSSelector pSelector)
		{
			if (pSelector.m_nTag != TagType.TAG_NOTATAG && pSelector.m_nTag != this.m_nType)
			{
				return false;
			}
			if (pSelector.m_nTag == TagType.TAG_UNKNOWN)
			{
				return false;
			}
			if (pSelector.m_bstrID != null && this.m_bstrID != null && pSelector.m_bstrID.Length > 0 && this.m_bstrID.Length > 0 && string.Compare(pSelector.m_bstrID, this.m_bstrID, true) != 0)
			{
				return false;
			}
			if (this.m_bstrID == null && pSelector.m_bstrID != null && pSelector.m_bstrID.Length > 0)
			{
				return false;
			}
			if (this.m_bstrClass == null && pSelector.m_arrClasses.Count > 0)
			{
				return false;
			}
			if (this.m_bstrClass != null && this.m_bstrClass.Length > 0 && pSelector.m_arrClasses.Count > 0)
			{
				foreach (string current in pSelector.m_arrClasses)
				{
					string[] array = this.m_bstrClass.Split(new char[]
					{
						' '
					});
					bool flag = false;
					for (int i = 0; i < array.Length; i++)
					{
						if (string.Compare(array[i], current, true) == 0)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return false;
					}
				}
				return true;
			}
			return true;
		}
		private bool MatchesSelectors(HtmlCSSRule pRule, ref int pSpecificity)
		{
			if (!pRule.m_bValid)
			{
				return false;
			}
			if (this.m_nType == TagType.TAG_UNKNOWN)
			{
				return false;
			}
			if (pRule.m_arrSelectors.Count == 0)
			{
				return false;
			}
			HtmlTreeItem htmlTreeItem = this;
			bool flag = true;
			for (int i = pRule.m_arrSelectors.Count - 1; i >= 0; i--)
			{
				HtmlCSSSelector pSelector = pRule.m_arrSelectors[i];
				if (flag)
				{
					if (!htmlTreeItem.MatchesThisSelector(pSelector))
					{
						return false;
					}
				}
				else
				{
					while ((htmlTreeItem = htmlTreeItem.m_pParent) != null && !htmlTreeItem.MatchesThisSelector(pSelector))
					{
					}
					if (htmlTreeItem == null)
					{
						return false;
					}
				}
				flag = false;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			foreach (HtmlCSSSelector current in pRule.m_arrSelectors)
			{
				if (current.m_nTag != TagType.TAG_NOTATAG)
				{
					num3++;
				}
				if (current.m_bstrID != null && current.m_bstrID.Length > 0)
				{
					num++;
				}
				num2 += current.m_arrClasses.Count;
				if (current.m_bstrPseudo != null && current.m_bstrPseudo.Length > 0)
				{
					num2++;
				}
			}
			pSpecificity = 100 * num + 10 * num2 + num3;
			return true;
		}
		public bool GetStringProperty(CssProperties nProp, ref string pValue)
		{
			return this.GetStringProperty(nProp, ref pValue, true);
		}
		public bool GetStringProperty(CssProperties nProp, ref string pValue, bool bInherited)
		{
			HtmlCSSProperty htmlCSSProperty = null;
			if (!this.GetPropertyHelper(nProp, ref htmlCSSProperty, bInherited))
			{
				return false;
			}
			pValue = htmlCSSProperty.m_bstrValue;
			return true;
		}
		public bool GetEnumProperty(CssProperties nProp, ref CssProperties pValue)
		{
			return this.GetEnumProperty(nProp, ref pValue, true);
		}
		public bool GetEnumProperty(CssProperties nProp, ref CssProperties pValue, bool bInherited)
		{
			HtmlCSSProperty htmlCSSProperty = null;
			if (!this.GetPropertyHelper(nProp, ref htmlCSSProperty, bInherited))
			{
				return false;
			}
			if (htmlCSSProperty.m_nValue == CssProperties.NOTFOUND)
			{
				return false;
			}
			pValue = htmlCSSProperty.m_nValue;
			return true;
		}
		public bool GetBackgroundPositionProperties(CssProperties nProp, ref CssProperties pValue1, ref CssProperties pValue2, bool bInherited)
		{
			HtmlCSSProperty htmlCSSProperty = null;
			if (!this.GetPropertyHelper(nProp, ref htmlCSSProperty, bInherited))
			{
				return false;
			}
			if (htmlCSSProperty.m_nValue == CssProperties.NOTFOUND)
			{
				return false;
			}
			if (htmlCSSProperty.m_nValue2 == CssProperties.NOTFOUND)
			{
				if (htmlCSSProperty.m_nValue == CssProperties.V_LEFT || htmlCSSProperty.m_nValue == CssProperties.V_RIGHT)
				{
					pValue1 = htmlCSSProperty.m_nValue;
					pValue2 = CssProperties.V_CENTER;
				}
				else
				{
					if (htmlCSSProperty.m_nValue == CssProperties.V_TOP || htmlCSSProperty.m_nValue == CssProperties.V_BOTTOM)
					{
						pValue1 = CssProperties.V_CENTER;
						pValue2 = htmlCSSProperty.m_nValue;
					}
					else
					{
						pValue1 = CssProperties.V_CENTER;
						pValue2 = CssProperties.V_CENTER;
					}
				}
			}
			else
			{
				pValue1 = htmlCSSProperty.m_nValue;
				pValue2 = htmlCSSProperty.m_nValue2;
			}
			return true;
		}
		public bool GetNumericProperty(CssProperties nProp, ref float pValue, ref bool pPercent)
		{
			return this.GetNumericProperty(nProp, ref pValue, ref pPercent, true);
		}
		public bool GetNumericProperty(CssProperties nProp, ref float pValue)
		{
			bool flag = false;
			return this.GetNumericProperty(nProp, ref pValue, ref flag, true);
		}
		public bool GetNumericProperty(CssProperties nProp, ref float pValue, bool bInherited)
		{
			bool flag = false;
			return this.GetNumericProperty(nProp, ref pValue, ref flag, bInherited);
		}
		public bool GetNumericProperty(CssProperties nProp, ref float pValue, ref bool pPercent, bool bInherited)
		{
			HtmlCSSProperty htmlCSSProperty = null;
			if (!this.GetPropertyHelper(nProp, ref htmlCSSProperty, bInherited))
			{
				return false;
			}
			pValue = htmlCSSProperty.m_fValue;
			if (htmlCSSProperty.m_bPercent)
			{
				pPercent = true;
			}
			if (nProp == CssProperties.FONT_SIZE && htmlCSSProperty.m_nValue != CssProperties.NOTFOUND)
			{
				CssProperties nValue = htmlCSSProperty.m_nValue;
				if (nValue <= CssProperties.V_MEDIUM)
				{
					if (nValue != CssProperties.V_LARGE)
					{
						if (nValue == CssProperties.V_MEDIUM)
						{
							pValue = 13.5f;
						}
					}
					else
					{
						pValue = 18f;
					}
				}
				else
				{
					if (nValue != CssProperties.V_SMALL)
					{
						switch (nValue)
						{
						case CssProperties.V_X_LARGE:
							pValue = 24f;
							break;
						case CssProperties.V_X_SMALL:
							pValue = 10f;
							break;
						case CssProperties.V_XX_LARGE:
							pValue = 36f;
							break;
						case CssProperties.V_XX_SMALL:
							pValue = 8f;
							break;
						}
					}
					else
					{
						pValue = 12f;
					}
				}
			}
			if (nProp == CssProperties.FONT_WEIGHT && htmlCSSProperty.m_nValue != CssProperties.NOTFOUND)
			{
				CssProperties nValue2 = htmlCSSProperty.m_nValue;
				if (nValue2 <= CssProperties.V_EXTRA_LIGHT)
				{
					switch (nValue2)
					{
					case CssProperties.V_BOLD:
					case CssProperties.V_BOLDER:
						break;
					default:
						switch (nValue2)
						{
						case CssProperties.V_DEMI_BOLD:
						case CssProperties.V_DEMI_LIGHT:
						case CssProperties.V_EXTRA_LIGHT:
							goto IL_11B;
						case CssProperties.V_DISC:
						case CssProperties.V_DOTTED:
						case CssProperties.V_DOUBLE:
							goto IL_12B;
						case CssProperties.V_EXTRA_BOLD:
							break;
						default:
							goto IL_12B;
						}
						break;
					}
					pValue = 900f;
					goto IL_12B;
				}
				switch (nValue2)
				{
				case CssProperties.V_LIGHT:
				case CssProperties.V_LIGHTER:
					break;
				default:
					if (nValue2 != CssProperties.V_MEDIUM && nValue2 != CssProperties.V_NORMAL)
					{
						goto IL_12B;
					}
					break;
				}
				IL_11B:
				pValue = 100f;
			}
			IL_12B:
			if (nProp == CssProperties.LINE_HEIGHT)
			{
				if (htmlCSSProperty.m_bNoMeasurementUnit && !htmlCSSProperty.m_bPercent)
				{
					pValue = htmlCSSProperty.m_fValue * 100f * 4f / 3f;
					pPercent = true;
				}
				else
				{
					pPercent = htmlCSSProperty.m_bPercent;
				}
			}
			if (nProp == CssProperties.BORDER_TOP_WIDTH || nProp == CssProperties.BORDER_RIGHT_WIDTH || nProp == CssProperties.BORDER_BOTTOM_WIDTH || nProp == CssProperties.BORDER_LEFT_WIDTH)
			{
				CssProperties nValue3 = htmlCSSProperty.m_nValue;
				if (nValue3 != CssProperties.V_MEDIUM)
				{
					switch (nValue3)
					{
					case CssProperties.V_THICK:
						pValue = 4.5f;
						break;
					case CssProperties.V_THIN:
						pValue = 1.5f;
						break;
					}
				}
				else
				{
					pValue = 3f;
				}
			}
			return true;
		}
		public bool GetColorProperty(CssProperties nProp, ref AuxRGB pColor)
		{
			return this.GetColorProperty(nProp, ref pColor, true);
		}
		public bool GetColorProperty(CssProperties nProp, ref AuxRGB pColor, bool bInherited)
		{
			HtmlCSSProperty htmlCSSProperty = null;
			if (!this.GetPropertyHelper(nProp, ref htmlCSSProperty, bInherited))
			{
				return false;
			}
			pColor.Set(ref htmlCSSProperty.m_rgbColor);
			return true;
		}
		public bool GetBorderProperties(CssProperties[] pStyles, ref float[] pWidths, ref AuxRGB[] pColors)
		{
			CssProperties[] array = new CssProperties[]
			{
				CssProperties.BORDER_TOP_STYLE,
				CssProperties.BORDER_RIGHT_STYLE,
				CssProperties.BORDER_BOTTOM_STYLE,
				CssProperties.BORDER_LEFT_STYLE
			};
			CssProperties[] array2 = new CssProperties[]
			{
				CssProperties.BORDER_TOP_WIDTH,
				CssProperties.BORDER_RIGHT_WIDTH,
				CssProperties.BORDER_BOTTOM_WIDTH,
				CssProperties.BORDER_LEFT_WIDTH
			};
			CssProperties[] array3 = new CssProperties[]
			{
				CssProperties.BORDER_TOP_COLOR,
				CssProperties.BORDER_RIGHT_COLOR,
				CssProperties.BORDER_BOTTOM_COLOR,
				CssProperties.BORDER_LEFT_COLOR
			};
			for (int i = 0; i < 4; i++)
			{
				this.GetEnumProperty(array[i], ref pStyles[i], false);
				this.GetNumericProperty(array2[i], ref pWidths[i], false);
				if (pStyles[i] == CssProperties.V_NONE)
				{
					pWidths[i] = 0f;
				}
				this.GetColorProperty(array3[i], ref pColors[i], false);
			}
			return true;
		}
		public bool GetMarginProperties(ref float[] pWidths)
		{
			return this.GetMarginProperties(ref pWidths, false);
		}
		public bool GetMarginProperties(ref float[] pWidths, bool bPadding)
		{
			CssProperties[] array = new CssProperties[]
			{
				CssProperties.MARGIN_TOP,
				CssProperties.MARGIN_RIGHT,
				CssProperties.MARGIN_BOTTOM,
				CssProperties.MARGIN_LEFT
			};
			if (bPadding)
			{
				array[0] = CssProperties.PADDING_TOP;
				array[1] = CssProperties.PADDING_RIGHT;
				array[2] = CssProperties.PADDING_BOTTOM;
				array[3] = CssProperties.PADDING_LEFT;
			}
			for (int i = 0; i < 4; i++)
			{
				this.GetNumericProperty(array[i], ref pWidths[i], false);
			}
			return true;
		}
		public bool GetPropertyHelper(CssProperties nProp, ref HtmlCSSProperty ppProp)
		{
			return this.GetPropertyHelper(nProp, ref ppProp, true);
		}
		public bool GetPropertyHelper(CssProperties nProp, ref HtmlCSSProperty ppProp, bool bInherited)
		{
			foreach (HtmlCSSProperty current in this.m_arrProperties)
			{
				if (current.m_nName == nProp)
				{
					ppProp = current;
					bool result = true;
					return result;
				}
			}
			if (this.m_pParent == null || !bInherited)
			{
				return false;
			}
			if (this.m_nType != TagType.TAG_TABLE)
			{
				return this.m_pParent.GetPropertyHelper(nProp, ref ppProp);
			}
			if (nProp == CssProperties.FONT_SIZE || nProp == CssProperties.TEXT_ALIGN)
			{
				return false;
			}
			HtmlTreeItem htmlTreeItem = this;
			while (htmlTreeItem.m_pParent != null)
			{
				htmlTreeItem = htmlTreeItem.m_pParent;
			}
			return htmlTreeItem.GetPropertyHelper(nProp, ref ppProp);
		}
	}
}
