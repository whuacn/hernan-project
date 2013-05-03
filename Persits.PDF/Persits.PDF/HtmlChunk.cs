using System;
using System.Text;
namespace Persits.PDF
{
	internal class HtmlChunk : HtmlObject
	{
		public CssProperties m_nCapitalization;
		public StringBuilder m_bstrText = new StringBuilder();
		public uint m_dwFlags;
		public float m_fSize;
		public float m_fAscent;
		public float m_fDescent;
		public string m_bstrFont;
		public AuxRGB m_rgbColor = new AuxRGB();
		public AuxRGB m_rgbBackgroundColor = new AuxRGB();
		public float m_fSubSupAdjustment;
		public float m_fLineHeight;
		public bool m_bLineHeightPercent;
		public string m_bstrHref;
		public string m_bstrHrefName;
		public PdfFont m_ptrFont;
		public override ObjectType Type
		{
			get
			{
				return ObjectType.htmlChunk;
			}
		}
		public HtmlChunk(HtmlManager pMgr) : base(pMgr)
		{
			this.m_fX = (this.m_fY = (this.m_fWidth = (this.m_fHeight = 0f)));
			this.m_bstrFont = "Times New Roman";
			this.m_fSize = 12f;
			this.m_dwFlags = 0u;
			this.m_fSubSupAdjustment = 0f;
			this.m_nCapitalization = CssProperties.V_NONE;
			this.m_fLineHeight = 100f;
			this.m_bLineHeightPercent = true;
		}
		public override void CalculateSize()
		{
			this.ChangeCase();
			if (!this.m_pHtmlManager.ObtainFont(this.m_bstrFont, this.m_dwFlags, ref this.m_ptrFont))
			{
				this.m_fAscent = (this.m_fDescent = (this.m_fWidth = (this.m_fHeight = 0f)));
				return;
			}
			try
			{
				PdfRect textExtent = this.m_ptrFont.GetTextExtent(this.m_bstrText.ToString(), this.m_fSize);
				this.m_fWidth = textExtent.Width;
			}
			catch (Exception)
			{
				this.m_bstrText = null;
				this.m_fWidth = 0f;
			}
			this.m_fDescent = this.m_fSize * (float)this.m_ptrFont.m_nDescent / 1000f;
			this.m_fAscent = this.m_fSize * (float)this.m_ptrFont.m_nAscent / 1000f;
			this.m_fHeight = this.m_fAscent;
			if ((this.m_dwFlags & 16u) != 0u)
			{
				this.m_fDescent -= this.m_fHeight / 2f;
				this.m_fSubSupAdjustment = this.m_fHeight / 2f;
			}
			if ((this.m_dwFlags & 32u) != 0u)
			{
				this.m_fHeight *= 1.4f;
				this.m_fSubSupAdjustment = -this.m_fHeight * 0.4f;
			}
			if (this.m_bLineHeightPercent)
			{
				this.m_fHeight *= this.m_fLineHeight / 100f;
				return;
			}
			this.m_fHeight = this.m_fLineHeight + this.m_fDescent;
		}
		public override void Parse()
		{
			new HtmlTag(this.m_pHtmlManager);
			while (!HtmlObject.IsSpace(base.m_c) && (base.m_c != '<' || base.Peek() == PeekValues.PEEK_NOTATAG))
			{
				if (base.m_c == '&')
				{
					bool flag = false;
					if (this.ParseAmpersandSymbol(ref flag))
					{
						continue;
					}
				}
				this.m_bstrText.Append(base.m_c);
				base.Next();
			}
			this.CalculateSize();
		}
		private void ChangeCase()
		{
			if (this.m_nCapitalization == CssProperties.V_NONE || this.m_bstrText == null)
			{
				return;
			}
			if (this.m_nCapitalization == CssProperties.V_UPPERCASE)
			{
				this.m_bstrText = new StringBuilder(this.m_bstrText.ToString().ToUpper());
				return;
			}
			if (this.m_nCapitalization == CssProperties.V_LOWERCASE)
			{
				this.m_bstrText = new StringBuilder(this.m_bstrText.ToString().ToLower());
				return;
			}
			if (this.m_nCapitalization == CssProperties.V_CAPITALIZE)
			{
				this.m_bstrText = new StringBuilder(this.m_bstrText.ToString().Substring(0, 1).ToUpper() + this.m_bstrText.ToString().Substring(1));
			}
		}
		private char toupper(char c)
		{
			string text = new string(c, 1);
			return text.ToUpper()[0];
		}
		private bool ParseAmpersandSymbol(ref bool pIsSpace)
		{
			Encoding.GetEncoding(this.m_pHtmlManager.m_nCodePage);
			pIsSpace = false;
			base.SaveState();
			base.Next();
			if (base.m_c == '#')
			{
				base.Next();
				int num = 0;
				int num2 = 0;
				int num3 = 10;
				char c = this.toupper(base.m_c);
				if (c == 'X')
				{
					num3 = 16;
					base.Next();
				}
				while ((base.m_c >= '0' && base.m_c <= '9') || (c >= 'A' && c <= 'F'))
				{
					if (base.m_c >= '0' && base.m_c <= '9')
					{
						num = num3 * num + (int)(base.m_c - '0');
					}
					else
					{
						if (num3 == 10)
						{
							base.RestoreState();
							return false;
						}
						num = num3 * num;
					}
					num2++;
					base.Next();
				}
				if (base.m_c == ';' && num2 > 0 && num2 <= 5 && num <= 65535)
				{
					base.Next();
					char c2 = (char)num;
					this.m_bstrText.Append(c2);
					if (c2 == ' ')
					{
						pIsSpace = true;
					}
					return true;
				}
				base.RestoreState();
				return false;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				int num4 = 0;
				if (HtmlObject.IsLetter(base.m_c))
				{
					while (HtmlObject.IsLetter(base.m_c) || (base.m_c >= '0' && base.m_c <= '9'))
					{
						stringBuilder.Append(base.m_c);
						num4++;
						base.Next();
						if (num4 >= 20)
						{
							base.RestoreState();
							return false;
						}
					}
				}
				if (num4 == 0 || base.m_c != ';')
				{
					base.RestoreState();
					return false;
				}
				base.Next();
				string strA = stringBuilder.ToString();
				for (int i = 0; i < HtmlCSS.SymbolMap.Length; i++)
				{
					if (string.Compare(strA, 0, HtmlCSS.SymbolMap[i].symbol, 1, num4) == 0)
					{
						this.m_bstrText.Append(HtmlCSS.SymbolMap[i].code, 1);
						return true;
					}
				}
				base.RestoreState();
				return false;
			}
		}
		public override bool Render(PdfPage pPage, PdfCanvas pCanvas, float fShiftX, float fShiftY)
		{
			if (!base.Render(pPage, pCanvas, fShiftX, fShiftY))
			{
				return false;
			}
			if (this.m_rgbBackgroundColor.m_bIsSet)
			{
				pCanvas.SaveState();
				pCanvas.SetFillColor(this.m_rgbBackgroundColor.r, this.m_rgbBackgroundColor.g, this.m_rgbBackgroundColor.b);
				pCanvas.FillRect(this.m_fX + fShiftX, this.m_pHtmlManager.m_fShiftY - (this.m_fY + this.m_fHeight + fShiftY - this.m_fDescent), this.m_fWidth, this.m_fHeight - this.m_fDescent);
				pCanvas.RestoreState();
			}
			PdfParam pdfParam = new PdfParam();
			pdfParam.AddItem("X", this.m_fX + fShiftX);
			pdfParam.AddItem("Y", this.m_pHtmlManager.m_fShiftY - this.m_fY - this.m_fHeight - fShiftY + this.m_fSize - this.m_fSubSupAdjustment);
			pdfParam.AddItem("Size", this.m_fSize);
			pdfParam.AddItem("Color", PdfGif.RGB((byte)(this.m_rgbColor.b * 255f), (byte)(this.m_rgbColor.g * 255f), (byte)(this.m_rgbColor.r * 255f)));
			pCanvas.DrawText(this.m_bstrText.ToString(), pdfParam, this.m_ptrFont);
			if (this.m_pHtmlManager.m_fShiftY - this.m_fY - this.m_fHeight - fShiftY - this.m_fSubSupAdjustment < this.m_pHtmlManager.m_fLowestYCoordinate)
			{
				this.m_pHtmlManager.m_fLowestYCoordinate = this.m_pHtmlManager.m_fShiftY - this.m_fY - this.m_fHeight - fShiftY - this.m_fSubSupAdjustment;
			}
			if ((this.m_dwFlags & 1036u) != 0u)
			{
				pCanvas.LineWidth = this.m_fSize / 20f;
				if (this.m_rgbColor.m_bIsSet)
				{
					pCanvas.SetColor(this.m_rgbColor.r, this.m_rgbColor.g, this.m_rgbColor.b);
				}
				else
				{
					pCanvas.SetColor(0f, 0f, 0f);
				}
				float num = this.m_pHtmlManager.m_fShiftY - this.m_fY - fShiftY - this.m_fHeight + 2f * this.m_fDescent / 3f - this.m_fSubSupAdjustment;
				if ((this.m_dwFlags & 4u) != 0u)
				{
					pCanvas.DrawLine(this.m_fX + fShiftX, num, this.m_fX + fShiftX + this.m_fWidth, num);
				}
				if ((this.m_dwFlags & 8u) != 0u)
				{
					num += this.m_fAscent / 2f;
					pCanvas.DrawLine(this.m_fX + fShiftX, num, this.m_fX + fShiftX + this.m_fWidth, num);
					num -= this.m_fAscent / 2f;
				}
				if ((this.m_dwFlags & 1024u) != 0u)
				{
					num += this.m_fAscent - 2f * this.m_fDescent / 3f;
					pCanvas.DrawLine(this.m_fX + fShiftX, num, this.m_fX + fShiftX + this.m_fWidth, num);
				}
			}
			float fScale = this.m_pHtmlManager.m_fScale;
			if (this.m_bstrHref != null)
			{
				if (this.m_bstrHref.Length > 1 && this.m_bstrHref[0] == '#')
				{
					PdfAnnots annots = pPage.Annots;
					PdfParam pdfParam2 = new PdfParam();
					pdfParam2["Type"] = 1f;
					pdfParam2["Border"] = 0f;
					pdfParam2["x"] = (this.m_fX + fShiftX) * fScale;
					pdfParam2["y"] = (this.m_pHtmlManager.m_fShiftY - (this.m_fY + this.m_fHeight - this.m_fDescent) - fShiftY) * fScale;
					pdfParam2["width"] = this.m_fWidth * fScale;
					pdfParam2["height"] = (this.m_fHeight - this.m_fDescent) * fScale;
					PdfAnnot pdfAnnot = annots.Add(null, pdfParam2);
					pdfAnnot.Name = this.m_bstrHref;
					this.m_pHtmlManager.m_arrBookmarkAnnots.Add(pdfAnnot);
				}
				else
				{
					HtmlXmlUrl htmlXmlUrl = new HtmlXmlUrl(this.m_pHtmlManager);
					if (htmlXmlUrl.MakeUrl(this.m_pHtmlManager.m_pBaseUrl, this.m_bstrHref))
					{
						PdfAction action = this.m_pHtmlManager.m_pDocument.CreateAction("type=URI", htmlXmlUrl.m_bstrUrl);
						PdfParam pdfParam3 = new PdfParam();
						pdfParam3["Type"] = 1f;
						pdfParam3["Border"] = 0f;
						pdfParam3["x"] = (this.m_fX + fShiftX) * fScale;
						pdfParam3["y"] = (this.m_pHtmlManager.m_fShiftY - (this.m_fY + this.m_fHeight - this.m_fDescent) - fShiftY) * fScale;
						pdfParam3["width"] = this.m_fWidth * fScale;
						pdfParam3["height"] = (this.m_fHeight - this.m_fDescent) * fScale;
						PdfAnnot pdfAnnot2 = pPage.Annots.Add(null, pdfParam3);
						pdfAnnot2.SetAction(action);
					}
				}
			}
			if (this.m_bstrHrefName != null)
			{
				PdfParam pdfParam4 = new PdfParam();
				pdfParam4["XYZ"] = 1f;
				pdfParam4["Left"] = (this.m_fX + fShiftX) * fScale;
				pdfParam4["Top"] = (this.m_pHtmlManager.m_fShiftY - this.m_fY - fShiftY) * fScale;
				PdfDest pdfDest = pPage.CreateDest(pdfParam4);
				pdfDest.m_bstrName = "#" + this.m_bstrHrefName;
				this.m_pHtmlManager.m_arrBookmarkDests.Add(pdfDest);
			}
			return true;
		}
		public void ObtainStyles()
		{
			if (!this.m_pHtmlManager.m_objCSS.m_bStateChanged)
			{
				this.m_dwFlags = this.m_pHtmlManager.m_objCSS.m_dwFlags;
				this.m_fSize = this.m_pHtmlManager.m_objCSS.m_fSize;
				this.m_bstrFont = this.m_pHtmlManager.m_objCSS.m_bstrFont;
				this.m_bstrHref = this.m_pHtmlManager.m_objCSS.m_bstrHref;
				this.m_rgbColor.Set(ref this.m_pHtmlManager.m_objCSS.m_rgbColor);
				this.m_rgbBackgroundColor.Set(ref this.m_pHtmlManager.m_objCSS.m_rgbBackgroundColor);
				this.m_nCapitalization = this.m_pHtmlManager.m_objCSS.m_nCapitalization;
				this.m_fLineHeight = this.m_pHtmlManager.m_objCSS.m_fLineHeight;
				this.m_bLineHeightPercent = this.m_pHtmlManager.m_objCSS.m_bLineHeightPercent;
				this.m_bDisplayNone = this.m_pHtmlManager.m_objCSS.m_bDisplayNone;
				return;
			}
			this.m_pHtmlManager.m_objCSS.m_bStateChanged = false;
			this.m_pTreeItem.GetStringProperty(CssProperties.FONT_FAMILY, ref this.m_bstrFont);
			this.m_pTreeItem.GetNumericProperty(CssProperties.FONT_SIZE, ref this.m_fSize);
			CssProperties cssProperties = CssProperties.NOTFOUND;
			if (this.m_pTreeItem.GetEnumProperty(CssProperties.FONT_STYLE, ref cssProperties) && (cssProperties == CssProperties.V_ITALIC || cssProperties == CssProperties.V_OBLIQUE))
			{
				this.m_dwFlags |= 2u;
			}
			float num = 0f;
			if (this.m_pTreeItem.GetNumericProperty(CssProperties.FONT_WEIGHT, ref num) && num >= 600f)
			{
				this.m_dwFlags |= 1u;
			}
			if (this.m_pTreeItem.GetEnumProperty(CssProperties.FONT_VARIANT, ref cssProperties) && cssProperties == CssProperties.V_SMALL_CAPS)
			{
				this.m_nCapitalization = CssProperties.V_UPPERCASE;
			}
			if (this.m_pTreeItem.GetEnumProperty(CssProperties.TEXT_TRANSFORM, ref cssProperties))
			{
				this.m_nCapitalization = cssProperties;
			}
			if (this.m_pTreeItem.GetEnumProperty(CssProperties.TEXT_DECORATION, ref cssProperties))
			{
				if (cssProperties == CssProperties.V_UNDERLINE)
				{
					this.m_dwFlags |= 4u;
				}
				if (cssProperties == CssProperties.V_LINE_THROUGH)
				{
					this.m_dwFlags |= 8u;
				}
				if (cssProperties == CssProperties.V_OVERLINE)
				{
					this.m_dwFlags |= 1024u;
				}
			}
			if (this.m_pTreeItem.GetEnumProperty(CssProperties.TEXT_ALIGN, ref cssProperties))
			{
				if (cssProperties == CssProperties.V_RIGHT)
				{
					this.m_dwFlags |= 128u;
				}
				else
				{
					if (cssProperties == CssProperties.V_CENTER)
					{
						this.m_dwFlags |= 256u;
					}
					else
					{
						if (cssProperties == CssProperties.V_JUSTIFY)
						{
							this.m_dwFlags |= 512u;
						}
					}
				}
			}
			if (this.m_pTreeItem.GetEnumProperty(CssProperties.VERTICAL_ALIGN, ref cssProperties))
			{
				if (cssProperties == CssProperties.V_SUB)
				{
					this.m_dwFlags |= 16u;
				}
				else
				{
					if (cssProperties == CssProperties.V_SUPER)
					{
						this.m_dwFlags |= 32u;
					}
				}
			}
			this.m_pTreeItem.GetColorProperty(CssProperties.COLOR, ref this.m_rgbColor);
			this.m_pTreeItem.GetColorProperty(CssProperties.BACKGROUND_COLOR, ref this.m_rgbBackgroundColor, false);
			this.m_pTreeItem.GetNumericProperty(CssProperties.LINE_HEIGHT, ref this.m_fLineHeight, ref this.m_bLineHeightPercent);
			if (this.m_pHtmlManager.m_bEnableHyperlinks)
			{
				this.m_pTreeItem.GetStringProperty(CssProperties.HREF, ref this.m_bstrHref);
			}
			if (this.m_pTreeItem.GetEnumProperty(CssProperties.DISPLAY, ref cssProperties, false))
			{
				this.m_bDisplayNone = (cssProperties == CssProperties.V_NONE);
			}
			this.m_pHtmlManager.m_objCSS.m_dwFlags = this.m_dwFlags;
			this.m_pHtmlManager.m_objCSS.m_fSize = this.m_fSize;
			this.m_pHtmlManager.m_objCSS.m_bstrFont = this.m_bstrFont;
			this.m_pHtmlManager.m_objCSS.m_bstrHref = this.m_bstrHref;
			this.m_pHtmlManager.m_objCSS.m_rgbColor.Set(ref this.m_rgbColor);
			this.m_pHtmlManager.m_objCSS.m_rgbBackgroundColor.Set(ref this.m_rgbBackgroundColor);
			this.m_pHtmlManager.m_objCSS.m_nCapitalization = this.m_nCapitalization;
			this.m_pHtmlManager.m_objCSS.m_fLineHeight = this.m_fLineHeight;
			this.m_pHtmlManager.m_objCSS.m_bLineHeightPercent = this.m_bLineHeightPercent;
			this.m_pHtmlManager.m_objCSS.m_bDisplayNone = this.m_bDisplayNone;
		}
		public override void AssignCoordinates()
		{
		}
	}
}
