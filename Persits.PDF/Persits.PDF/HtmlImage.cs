using System;
using System.IO;
using System.Text;
namespace Persits.PDF
{
	internal class HtmlImage : HtmlBox
	{
		private const int IMAGE_ALIGNMENT_MARGIN = 3;
		private InputStruct[] arrInputTypes = new InputStruct[]
		{
			new InputStruct(InputTypeAttribute.INPUT_TEXT, "text"),
			new InputStruct(InputTypeAttribute.INPUT_IMAGE, "image"),
			new InputStruct(InputTypeAttribute.INPUT_RADIO, "radio"),
			new InputStruct(InputTypeAttribute.INPUT_CHECKBOX, "checkbox"),
			new InputStruct(InputTypeAttribute.INPUT_BUTTON, "button"),
			new InputStruct(InputTypeAttribute.INPUT_SUBMIT, "submit"),
			new InputStruct(InputTypeAttribute.INPUT_RESET, "reset"),
			new InputStruct(InputTypeAttribute.INPUT_HIDDEN, "hidden"),
			new InputStruct(InputTypeAttribute.INPUT_SELECT, "_select")
		};
		public bool m_bNotFound;
		public TagType m_nParentContainerType;
		public string m_bstrAlt;
		public string m_bstrHref;
		public float m_fSpecifiedWidth;
		public float m_fSpecifiedHeight;
		public float m_fContainerWidth;
		public float m_fScaleX;
		public float m_fScaleY;
		public InputTypeAttribute m_nInputType;
		public string m_bstrValue;
		private HtmlChunk m_pInputChunk;
		private AuxRGB m_rgbBgColor = new AuxRGB();
		private bool m_bSelected;
		private bool m_bInput;
		private string m_bstrFileName;
		public PdfImage m_ptrImage;
		public override HtmlAttributes HorizAlignment
		{
			get
			{
				if (this.m_nAlignment != HtmlAttributes.htmlLeft && this.m_nAlignment != HtmlAttributes.htmlRight)
				{
					return HtmlAttributes.htmlNone;
				}
				return this.m_nAlignment;
			}
		}
		public override ObjectType Type
		{
			get
			{
				return ObjectType.htmlImage;
			}
		}
		public HtmlImage(HtmlManager pMgr, float fContainerWidth) : base(pMgr)
		{
			this.m_bNotFound = false;
			this.m_nAlignment = HtmlAttributes.htmlBottom;
			this.m_bInput = false;
			this.m_fSpecifiedWidth = (this.m_fSpecifiedHeight = 0f);
			this.m_fContainerWidth = fContainerWidth;
			this.m_fScaleX = (this.m_fScaleY = 1f);
			base.ObtainPageBreakStatus();
			this.m_pInputChunk = null;
			this.m_bSelected = false;
		}
		public override void CalculateSize()
		{
		}
		public override bool Render(PdfPage pPage, PdfCanvas pCanvas, float fShiftX, float fShiftY)
		{
			if (!base.Render(pPage, pCanvas, fShiftX, fShiftY))
			{
				return false;
			}
			if (this.m_bNotFound && (double)this.m_fHeight < 7.5)
			{
				return true;
			}
			base.DrawBorder(pCanvas, fShiftX, fShiftY);
			if (this.m_bInput)
			{
				return this.RenderInput(pPage, pCanvas, fShiftX, fShiftY);
			}
			if (this.m_bNotFound)
			{
				pCanvas.SaveState();
				pCanvas.SetFillColor(1f, 1f, 1f);
				pCanvas.FillRect(this.m_fX + fShiftX + this.m_arrBorders[3] + this.m_arrMargins[3], this.m_pHtmlManager.m_fShiftY - (this.m_fY + this.m_fHeight + fShiftY) + this.m_arrBorders[2] + this.m_arrMargins[2], this.m_fWidth - this.m_arrMargins[3] - this.m_arrMargins[1] - this.m_arrBorders[1] - this.m_arrBorders[3], this.m_fHeight - this.m_arrMargins[0] - this.m_arrMargins[2] - this.m_arrBorders[0] - this.m_arrBorders[2]);
				pCanvas.SetParams("LineWidth=1; Color=gray");
				pCanvas.DrawRect(this.m_fX + fShiftX + this.m_arrBorders[3] + this.m_arrMargins[3], this.m_pHtmlManager.m_fShiftY - (this.m_fY + this.m_fHeight + fShiftY) + this.m_arrBorders[2] + this.m_arrMargins[2], this.m_fWidth - this.m_arrMargins[3] - this.m_arrMargins[1] - this.m_arrBorders[1] - this.m_arrBorders[3], this.m_fHeight - this.m_arrMargins[0] - this.m_arrMargins[2] - this.m_arrBorders[0] - this.m_arrBorders[2]);
				if (this.m_bstrAlt != null && this.m_bstrAlt.Length == 0)
				{
					this.m_bstrAlt = this.m_bstrFileName;
				}
				HtmlChunk htmlChunk = new HtmlChunk(this.m_pHtmlManager);
				htmlChunk.m_bstrFont = "Times New Roman";
				htmlChunk.m_fSize = 10f;
				htmlChunk.m_bstrText = new StringBuilder("Image: ");
				htmlChunk.m_bstrText.Append(this.m_bstrAlt);
				htmlChunk.CalculateSize();
				htmlChunk.X = this.m_fX + this.m_arrBorders[3] + this.m_arrMargins[3] + 5f;
				htmlChunk.Y = this.m_fY + this.m_arrBorders[0] + this.m_arrMargins[0] + 6f;
				pCanvas.AddRect(this.m_fX + fShiftX + this.m_arrBorders[3] + this.m_arrMargins[3], this.m_pHtmlManager.m_fShiftY - (this.m_fY + this.m_fHeight + fShiftY) + this.m_arrBorders[2] + this.m_arrMargins[2], this.m_fWidth - this.m_arrMargins[3] - this.m_arrMargins[1] - this.m_arrBorders[1] - this.m_arrBorders[3], this.m_fHeight - this.m_arrMargins[2] - this.m_arrMargins[0] - this.m_arrBorders[0] - this.m_arrBorders[2]);
				pCanvas.Clip(false);
				pCanvas.SetFillColor(0f, 0f, 0f);
				htmlChunk.Render(pPage, pCanvas, fShiftX, fShiftY);
				pCanvas.RestoreState();
				return true;
			}
			PdfParam pdfParam = new PdfParam();
			pdfParam.AddItem("X", this.m_fX + fShiftX + this.m_arrBorders[3] + this.m_arrMargins[3]);
			pdfParam.AddItem("Y", this.m_pHtmlManager.m_fShiftY - (this.m_fY + fShiftY + this.m_fHeight) + this.m_arrBorders[2] + this.m_arrMargins[2]);
			pdfParam.AddItem("ScaleX", this.m_fScaleX);
			pdfParam.AddItem("ScaleY", this.m_fScaleY);
			pCanvas.DrawImage(this.m_ptrImage, pdfParam);
			if (this.m_pHtmlManager.m_fShiftY - (this.m_fY + fShiftY + this.m_fHeight) < this.m_pHtmlManager.m_fLowestYCoordinate)
			{
				this.m_pHtmlManager.m_fLowestYCoordinate = this.m_pHtmlManager.m_fShiftY - (this.m_fY + fShiftY + this.m_fHeight);
			}
			if (this.m_bstrHref != null)
			{
				HtmlXmlUrl htmlXmlUrl = new HtmlXmlUrl(this.m_pHtmlManager);
				if (htmlXmlUrl.MakeUrl(this.m_pHtmlManager.m_pBaseUrl, this.m_bstrHref))
				{
					PdfAction action = this.m_pHtmlManager.m_pDocument.CreateAction("type=URI", htmlXmlUrl.m_bstrUrl);
					float fScale = this.m_pHtmlManager.m_fScale;
					pdfParam.Clear();
					pdfParam.AddItem("Type", 1f);
					pdfParam.AddItem("Border", 0f);
					pdfParam.AddItem("X", (this.m_fX + fShiftX) * fScale);
					pdfParam.AddItem("Y", (this.m_pHtmlManager.m_fShiftY - (this.m_fY + this.m_fHeight) - fShiftY) * fScale);
					pdfParam.AddItem("Width", this.m_fWidth * fScale);
					pdfParam.AddItem("Height", this.m_fHeight * fScale);
					PdfAnnot pdfAnnot = pPage.Annots.Add(null, pdfParam);
					pdfAnnot.SetAction(action);
				}
			}
			return true;
		}
		public void Load(HtmlTag pTag)
		{
			this.Load(pTag, false);
		}
		public void Load(HtmlTag pTag, bool bBackgroundImage)
		{
			CssProperties cssProperties = CssProperties.NOTFOUND;
			this.m_bNotFound = true;
			pTag.GetStringAttribute("ALT", ref this.m_bstrAlt);
			pTag.GetStandardAttribute("ALIGN", ref this.m_nAlignment);
			if (this.m_nAlignment == HtmlAttributes.htmlCenter)
			{
				this.m_nAlignment = HtmlAttributes.htmlMiddle;
			}
			if (this.m_pTreeItem.GetEnumProperty(CssProperties.FLOAT, ref cssProperties, false))
			{
				if (cssProperties == CssProperties.V_RIGHT)
				{
					this.m_nAlignment = HtmlAttributes.htmlRight;
				}
				else
				{
					if (cssProperties == CssProperties.V_LEFT)
					{
						this.m_nAlignment = HtmlAttributes.htmlLeft;
					}
				}
			}
			if (this.m_pTreeItem.GetEnumProperty(CssProperties.TEXT_ALIGN, ref cssProperties))
			{
				if (cssProperties == CssProperties.V_RIGHT)
				{
					this.m_dwAlignFlags |= 128u;
				}
				else
				{
					if (cssProperties == CssProperties.V_CENTER)
					{
						this.m_dwAlignFlags = 256u;
					}
					else
					{
						if (cssProperties == CssProperties.V_JUSTIFY)
						{
							this.m_dwAlignFlags |= 512u;
						}
					}
				}
			}
			if (this.m_nAlignment != HtmlAttributes.htmlTop && this.m_nAlignment != HtmlAttributes.htmlBottom && this.m_nAlignment != HtmlAttributes.htmlLeft && this.m_nAlignment != HtmlAttributes.htmlRight && this.m_nAlignment != HtmlAttributes.htmlMiddle)
			{
				this.m_nAlignment = HtmlAttributes.htmlBottom;
			}
			if (!bBackgroundImage)
			{
				this.m_pTreeItem.GetBorderProperties(this.m_arrBorderStyles, ref this.m_arrBorders, ref this.m_arrBorderColors);
				this.m_pTreeItem.GetMarginProperties(ref this.m_arrMargins);
			}
			bool flag = false;
			if (this.m_pTreeItem.GetNumericProperty(CssProperties.WIDTH, ref this.m_fSpecifiedWidth, ref flag, false) && flag)
			{
				this.m_fSpecifiedWidth = (float)((int)((double)(this.m_fContainerWidth * this.m_fSpecifiedWidth) / 100.0));
			}
			flag = false;
			if (this.m_pTreeItem.GetNumericProperty(CssProperties.HEIGHT, ref this.m_fSpecifiedHeight, ref flag, false) && flag)
			{
				float num;
				if (this.m_nParentContainerType != TagType.TAG_UNKNOWN)
				{
					num = 0f;
				}
				else
				{
					num = this.m_pHtmlManager.m_fDefaultPageHeight;
				}
				this.m_fSpecifiedHeight = (float)((int)((double)(num * this.m_fSpecifiedHeight) / 100.0));
			}
			if (this.m_pHtmlManager.m_bEnableHyperlinks)
			{
				this.m_pTreeItem.GetStringProperty(CssProperties.HREF, ref this.m_bstrHref);
			}
			if (bBackgroundImage)
			{
				this.m_fSpecifiedWidth = (this.m_fSpecifiedHeight = 0f);
				this.m_bAbsolutePosition = (this.m_bAbsoluteLeft = (this.m_bAbsoluteTop = false));
			}
			if ((pTag.m_nType == TagType.TAG_INPUT || pTag.m_nType == TagType.TAG_TEXTAREA) && this.HandleInput(pTag))
			{
				return;
			}
			float num2 = 28f;
			float num3 = 30f;
			if (this.m_bstrAlt != null && this.m_bstrAlt.Length > 0)
			{
				HtmlChunk htmlChunk = new HtmlChunk(this.m_pHtmlManager);
				htmlChunk.m_bstrFont = "Times New Roman";
				htmlChunk.m_fSize = 12f;
				htmlChunk.m_bstrText = new StringBuilder("Image: ");
				htmlChunk.m_bstrText.Append(this.m_bstrAlt);
				htmlChunk.CalculateSize();
				num2 += htmlChunk.Width;
			}
			this.m_fWidth = ((this.m_fSpecifiedWidth > 0f) ? this.m_fSpecifiedWidth : num2) + this.m_arrBorders[1] + this.m_arrBorders[3] + this.m_arrMargins[1] + this.m_arrMargins[3];
			this.m_fHeight = ((this.m_fSpecifiedHeight > 0f) ? this.m_fSpecifiedHeight : num3) + this.m_arrBorders[0] + this.m_arrBorders[2] + this.m_arrMargins[0] + this.m_arrMargins[2];
			string text = null;
			if (!pTag.GetStringAttribute("SRC", ref text))
			{
				this.m_bstrFileName = "Unknown";
				return;
			}
			if (string.Compare(text, 0, "data:", 0, 5) == 0)
			{
				this.m_bstrFileName = "data";
			}
			else
			{
				try
				{
					this.m_bstrFileName = Path.GetFileName(text);
				}
				catch (Exception)
				{
					this.m_bstrFileName = "Unknown";
				}
			}
			HtmlXmlUrl htmlXmlUrl = new HtmlXmlUrl(this.m_pHtmlManager);
			if (!htmlXmlUrl.MakeUrl(this.m_pHtmlManager.m_pBaseUrl, text))
			{
				return;
			}
			if (!this.m_pHtmlManager.LookUpImageCache(htmlXmlUrl.m_bstrUrl, ref this.m_ptrImage))
			{
				byte[] blob = null;
				try
				{
					htmlXmlUrl.ReadUrl(ref blob);
				}
				catch (Exception ex)
				{
					this.m_pHtmlManager.LogError("Image", htmlXmlUrl.m_bstrUrl, ex.Message);
					return;
				}
				try
				{
					this.m_ptrImage = this.m_pHtmlManager.m_pDocument.OpenImage(blob);
				}
				catch
				{
					this.m_pHtmlManager.LogError("Image", htmlXmlUrl.m_bstrUrl, "Invalid image format.");
					return;
				}
				this.m_pHtmlManager.AddToImageCache(htmlXmlUrl.m_bstrUrl, this.m_ptrImage);
				if (this.m_pHtmlManager.m_bInvertCMYK && this.m_ptrImage.m_nComponentsPerSample == 4)
				{
					PdfArray pdfArray = this.m_ptrImage.m_pImageObj.AddArray("Decode");
					for (int i = 0; i < this.m_ptrImage.m_nComponentsPerSample; i++)
					{
						pdfArray.Add(new PdfNumber(null, 1.0));
						pdfArray.Add(new PdfNumber(null, 0.0));
					}
				}
			}
			float num4 = this.m_ptrImage.m_fWidth * HtmlManager.fEmpiricalScaleFactor;
			float num5 = this.m_ptrImage.m_fHeight * HtmlManager.fEmpiricalScaleFactor;
			this.m_fScaleX = HtmlManager.fEmpiricalScaleFactor / this.m_ptrImage.m_fDefaultScaleX;
			this.m_fScaleY = HtmlManager.fEmpiricalScaleFactor / this.m_ptrImage.m_fDefaultScaleY;
			if (this.m_fSpecifiedWidth > 0f)
			{
				this.m_fScaleX *= this.m_fSpecifiedWidth / num4;
				num4 = this.m_fSpecifiedWidth;
				if (this.m_fSpecifiedHeight == 0f)
				{
					num5 = num4 * this.m_ptrImage.m_fHeight / this.m_ptrImage.m_fWidth;
					this.m_fScaleY = this.m_fScaleX * (this.m_ptrImage.m_fDefaultScaleX / this.m_ptrImage.m_fDefaultScaleY);
				}
			}
			if (this.m_fSpecifiedHeight > 0f)
			{
				this.m_fScaleY *= this.m_fSpecifiedHeight / num5;
				num5 = this.m_fSpecifiedHeight;
				if (this.m_fSpecifiedWidth == 0f)
				{
					num4 = num5 * this.m_ptrImage.m_fWidth / this.m_ptrImage.m_fHeight;
					this.m_fScaleX = this.m_fScaleY * (this.m_ptrImage.m_fDefaultScaleY / this.m_ptrImage.m_fDefaultScaleX);
				}
			}
			if (this.m_nAlignment == HtmlAttributes.htmlLeft)
			{
				this.m_arrMargins[1] += 3f;
			}
			if (this.m_nAlignment == HtmlAttributes.htmlRight)
			{
				this.m_arrMargins[3] += 3f;
			}
			this.m_fWidth = num4 + this.m_arrBorders[1] + this.m_arrBorders[3] + this.m_arrMargins[1] + this.m_arrMargins[3];
			this.m_fHeight = num5 + this.m_arrBorders[0] + this.m_arrBorders[2] + this.m_arrMargins[0] + this.m_arrMargins[2];
			this.m_bNotFound = false;
		}
		public override void AssignCoordinates()
		{
		}
		private bool HandleInput(HtmlTag pTag)
		{
			this.m_nInputType = InputTypeAttribute.INPUT_TEXT;
			this.m_bNotFound = false;
			bool flag = false;
			string text = null;
			pTag.GetStringAttribute("type", ref text);
			if (text != null && text.Length > 0)
			{
				for (int i = 0; i < 9; i++)
				{
					if (string.Compare(text, this.arrInputTypes[i].m_szName, true) == 0)
					{
						this.m_nInputType = this.arrInputTypes[i].m_nType;
						break;
					}
				}
			}
			if (this.m_nInputType == InputTypeAttribute.INPUT_IMAGE)
			{
				return false;
			}
			if (this.m_nInputType == InputTypeAttribute.INPUT_HIDDEN)
			{
				this.m_fHeight = (this.m_fWidth = 0f);
				this.m_bNotFound = true;
				return true;
			}
			if (pTag.m_nType == TagType.TAG_TEXTAREA)
			{
				this.m_nInputType = InputTypeAttribute.INPUT_TEXTAREA;
			}
			if (this.m_nInputType == InputTypeAttribute.INPUT_SELECT)
			{
				if (this.m_pTreeItem.m_arrChildren.Count > 0)
				{
					for (int j = this.m_pTreeItem.m_arrChildren.Count - 1; j >= 0; j--)
					{
						if (this.m_pTreeItem.m_arrChildren[j].m_nType == TagType.TAG_SELECT)
						{
							this.m_pTreeItem = this.m_pTreeItem.m_arrChildren[j];
							this.m_pTreeItem.GetNumericProperty(CssProperties.WIDTH, ref this.m_fSpecifiedWidth, false);
							this.m_pTreeItem.GetNumericProperty(CssProperties.HEIGHT, ref this.m_fSpecifiedHeight, false);
							break;
						}
					}
				}
				flag = pTag.GetStringAttribute("MULTIPLE");
			}
			if (!pTag.GetStringAttribute("value", ref this.m_bstrValue) && pTag.m_nType != TagType.TAG_TEXTAREA && !flag)
			{
				if (this.m_nInputType == InputTypeAttribute.INPUT_SUBMIT)
				{
					this.m_bstrValue = "Submit";
				}
				if (this.m_nInputType == InputTypeAttribute.INPUT_RESET)
				{
					this.m_bstrValue = "Reset";
				}
			}
			this.m_pInputChunk = new HtmlChunk(this.m_pHtmlManager);
			this.m_pInputChunk.ObtainStyles();
			this.m_pInputChunk.m_bstrText = new StringBuilder(this.m_bstrValue);
			this.m_pInputChunk.m_fLineHeight = 100f;
			this.m_pInputChunk.m_bLineHeightPercent = true;
			if (this.m_arrBorders[0] + this.m_arrBorders[1] + this.m_arrBorders[2] + this.m_arrBorders[3] <= 0f)
			{
				uint rgb = 15790320u;
				uint rgb2 = 15790320u;
				uint rgb3 = 6579300u;
				uint rgb4 = 6579300u;
				if (this.m_nInputType == InputTypeAttribute.INPUT_TEXT || this.m_nInputType == InputTypeAttribute.INPUT_TEXTAREA || flag)
				{
					rgb2 = (rgb = (rgb3 = (rgb4 = 8363449u)));
				}
				this.m_arrBorders[0] = (this.m_arrBorders[1] = (this.m_arrBorders[2] = (this.m_arrBorders[3] = 0.75f)));
				this.m_arrBorderColors[0].Set(rgb);
				this.m_arrBorderColors[1].Set(rgb2);
				this.m_arrBorderColors[2].Set(rgb3);
				this.m_arrBorderColors[3].Set(rgb4);
			}
			if (this.m_nInputType == InputTypeAttribute.INPUT_TEXT)
			{
				int num = 20;
				pTag.GetIntAttribute("size", ref num);
				if (this.m_fSpecifiedWidth == 0f)
				{
					this.m_fSpecifiedWidth = (float)num * this.m_pInputChunk.m_fSize / 2f;
				}
			}
			if (pTag.m_nType == TagType.TAG_TEXTAREA)
			{
				int num2 = 2;
				int num3 = 20;
				pTag.GetIntAttribute("rows", ref num2);
				pTag.GetIntAttribute("cols", ref num3);
				if (this.m_fSpecifiedWidth == 0f)
				{
					this.m_fSpecifiedWidth = (float)num3 * this.m_pInputChunk.m_fSize / 2f;
				}
				if (this.m_fSpecifiedHeight == 0f)
				{
					this.m_fSpecifiedHeight = (float)num2 * this.m_pInputChunk.m_fSize;
				}
			}
			if (this.m_nInputType == InputTypeAttribute.INPUT_CHECKBOX || this.m_nInputType == InputTypeAttribute.INPUT_RADIO)
			{
				float num4 = 12f;
				this.m_pHtmlManager.m_pParam.GetNumberIfSet("CheckboxSize", 0, ref num4);
				this.m_fSpecifiedWidth = (this.m_fSpecifiedHeight = num4);
				this.m_arrBorders[0] = (this.m_arrBorders[1] = (this.m_arrBorders[2] = (this.m_arrBorders[3] = 0.75f)));
				this.m_arrBorderColors[0].Set(1855872u);
				this.m_arrBorderColors[1].Set(1855872u);
				this.m_arrBorderColors[2].Set(1855872u);
				this.m_arrBorderColors[3].Set(1855872u);
				int num5 = 0;
				this.m_bSelected = pTag.GetIntAttribute("checked", ref num5);
				if (this.m_nInputType == InputTypeAttribute.INPUT_CHECKBOX)
				{
					this.m_pInputChunk.m_bstrFont = "ZapfDingbats";
					this.m_pInputChunk.m_bstrText = new StringBuilder(this.m_bSelected ? "4" : "");
					this.m_pInputChunk.m_fSize = num4;
				}
				if (this.m_nInputType == InputTypeAttribute.INPUT_RADIO)
				{
					this.m_arrBorders[0] = (this.m_arrBorders[1] = (this.m_arrBorders[2] = (this.m_arrBorders[3] = 0f)));
				}
			}
			if (flag)
			{
				this.m_pInputChunk.m_bstrText = null;
				this.ComputSelectMultipleSizes(this.m_bstrValue, pTag, ref this.m_fSpecifiedWidth, ref this.m_fSpecifiedHeight);
				this.m_nInputType = InputTypeAttribute.INPUT_TEXTAREA;
			}
			this.HandleAmpersandChars(ref this.m_pInputChunk.m_bstrText);
			this.m_pInputChunk.CalculateSize();
			this.m_fWidth = ((this.m_fSpecifiedWidth > 0f) ? this.m_fSpecifiedWidth : (this.m_pInputChunk.Width + 10f)) + this.m_arrBorders[1] + this.m_arrBorders[3] + this.m_arrMargins[1] + this.m_arrMargins[3];
			this.m_fHeight = ((this.m_fSpecifiedHeight > 0f) ? this.m_fSpecifiedHeight : (this.m_pInputChunk.Height + 5f)) + this.m_arrBorders[0] + this.m_arrBorders[2] + this.m_arrMargins[0] + this.m_arrMargins[2];
			this.m_rgbBgColor.Set((this.m_nInputType == InputTypeAttribute.INPUT_TEXT || this.m_nInputType == InputTypeAttribute.INPUT_SELECT || this.m_nInputType == InputTypeAttribute.INPUT_TEXTAREA || this.m_nInputType == InputTypeAttribute.INPUT_RADIO || this.m_nInputType == InputTypeAttribute.INPUT_CHECKBOX) ? 16777215u : 14737632u);
			this.m_pTreeItem.GetColorProperty(CssProperties.BACKGROUND_COLOR, ref this.m_rgbBgColor, false);
			if (this.m_nInputType == InputTypeAttribute.INPUT_SELECT)
			{
				this.m_pTreeItem.GetColorProperty(CssProperties.COLOR, ref this.m_pInputChunk.m_rgbColor, false);
			}
			this.m_bInput = true;
			return true;
		}
		private void ComputSelectMultipleSizes(string bstr, HtmlTag pTag, ref float fWidth, ref float fHeight)
		{
			if (fWidth > 0f && fHeight > 0f)
			{
				return;
			}
			if (fHeight == 0f)
			{
				int num = 4;
				pTag.GetIntAttribute("size", ref num);
				fHeight = (float)num * this.m_pInputChunk.m_fSize;
			}
			if (fWidth > 0f)
			{
				return;
			}
			fWidth = 200f;
			float num2 = 0f;
			string text = null;
			for (int i = 0; i < bstr.Length; i++)
			{
				if (bstr[i] == '\r')
				{
					i++;
					HtmlChunk htmlChunk = new HtmlChunk(this.m_pHtmlManager);
					htmlChunk.ObtainStyles();
					htmlChunk.m_bstrText = new StringBuilder(text);
					htmlChunk.CalculateSize();
					if (htmlChunk.Width > num2)
					{
						num2 = htmlChunk.Width;
					}
					text = null;
				}
				else
				{
					text += bstr[i];
				}
			}
			fWidth = num2;
		}
		private bool RenderInput(PdfPage pPage, PdfCanvas pCanvas, float fShiftX, float fShiftY)
		{
			this.m_pInputChunk.X = this.m_fX;
			this.m_pInputChunk.Y = this.m_fY;
			pCanvas.SaveState();
			if (this.m_nInputType == InputTypeAttribute.INPUT_BUTTON || this.m_nInputType == InputTypeAttribute.INPUT_SUBMIT || this.m_nInputType == InputTypeAttribute.INPUT_RESET || this.m_nInputType == InputTypeAttribute.INPUT_TEXT || this.m_nInputType == InputTypeAttribute.INPUT_SELECT || this.m_nInputType == InputTypeAttribute.INPUT_TEXTAREA || this.m_nInputType == InputTypeAttribute.INPUT_CHECKBOX)
			{
				pCanvas.SetFillColor(this.m_rgbBgColor.r, this.m_rgbBgColor.g, this.m_rgbBgColor.b);
				pCanvas.FillRect(this.m_fX + fShiftX + this.m_arrBorders[3] + this.m_arrMargins[3], this.m_pHtmlManager.m_fShiftY - (this.m_fY + fShiftY) - this.m_arrBorders[0] - this.m_arrMargins[0], this.m_fWidth - this.m_arrBorders[1] - this.m_arrBorders[3] - this.m_arrMargins[1] - this.m_arrMargins[3], -this.m_fHeight + this.m_arrBorders[0] + this.m_arrBorders[2] + this.m_arrMargins[0] + this.m_arrMargins[2]);
			}
			float num = this.m_arrBorders[3] + this.m_arrMargins[3];
			float num2 = this.m_arrBorders[0] + this.m_arrMargins[0];
			float num3 = this.m_arrBorders[1] + this.m_arrBorders[3] + this.m_arrMargins[1] + this.m_arrMargins[3];
			float num4 = this.m_arrBorders[0] + this.m_arrBorders[2] + this.m_arrMargins[0] + this.m_arrMargins[2];
			if (this.m_pInputChunk.Width < this.m_fWidth - num3)
			{
				CssProperties cssProperties = CssProperties.V_LEFT;
				if (this.m_nInputType == InputTypeAttribute.INPUT_BUTTON || this.m_nInputType == InputTypeAttribute.INPUT_SUBMIT || this.m_nInputType == InputTypeAttribute.INPUT_RESET)
				{
					cssProperties = CssProperties.V_CENTER;
				}
				this.m_pTreeItem.GetEnumProperty(CssProperties.TEXT_ALIGN, ref cssProperties, false);
				if (cssProperties == CssProperties.V_RIGHT)
				{
					num += this.m_fWidth - num3 - this.m_pInputChunk.Width;
				}
				else
				{
					if (cssProperties == CssProperties.V_CENTER)
					{
						num += (this.m_fWidth - num3 - this.m_pInputChunk.Width) / 2f;
					}
				}
			}
			num2 += (this.m_fHeight - num4 - this.m_pInputChunk.Height + this.m_pInputChunk.m_fDescent) / 2f;
			if (this.m_nInputType != InputTypeAttribute.INPUT_RADIO)
			{
				pCanvas.AddRect(this.m_fX + fShiftX + this.m_arrBorders[3] + this.m_arrMargins[3], this.m_pHtmlManager.m_fShiftY - (this.m_fY + fShiftY) - this.m_arrBorders[0] - this.m_arrMargins[0], this.m_fWidth - this.m_arrBorders[1] - this.m_arrBorders[3], -this.m_fHeight + this.m_arrBorders[0] + this.m_arrBorders[2]);
				pCanvas.Clip(false);
			}
			if (this.m_nInputType == InputTypeAttribute.INPUT_CHECKBOX)
			{
				num += 1f;
			}
			if (this.m_nInputType == InputTypeAttribute.INPUT_RADIO)
			{
				float num5 = (this.m_fWidth - num3 - 1f) / 2f;
				pCanvas.SetFillColor(this.m_rgbBgColor.r, this.m_rgbBgColor.g, this.m_rgbBgColor.b);
				pCanvas.FillEllipse(this.m_fX + fShiftX + this.m_arrMargins[3] + num5 + 0.5f, this.m_pHtmlManager.m_fShiftY - (this.m_fY + fShiftY) - this.m_arrMargins[0] - num5 - 0.5f, num5, num5);
				pCanvas.SetColor(this.m_arrBorderColors[0].r, this.m_arrBorderColors[0].g, this.m_arrBorderColors[0].b);
				pCanvas.DrawEllipse(this.m_fX + fShiftX + this.m_arrMargins[3] + num5 + 0.5f, this.m_pHtmlManager.m_fShiftY - (this.m_fY + fShiftY) - this.m_arrMargins[0] - num5 - 0.5f, num5, num5);
				if (this.m_bSelected)
				{
					pCanvas.SetFillColor(this.m_pInputChunk.m_rgbColor.r, this.m_pInputChunk.m_rgbColor.g, this.m_pInputChunk.m_rgbColor.b);
					pCanvas.FillEllipse(this.m_fX + fShiftX + this.m_arrMargins[3] + num5 + 0.5f, this.m_pHtmlManager.m_fShiftY - (this.m_fY + fShiftY) - this.m_arrMargins[0] - num5 - 0.5f, num5 / 2f, num5 / 2f);
				}
			}
			else
			{
				this.m_pInputChunk.Render(pPage, pCanvas, fShiftX + num, fShiftY + num2);
			}
			if (this.m_nInputType == InputTypeAttribute.INPUT_TEXTAREA)
			{
				PdfParam pdfParam = new PdfParam();
				pdfParam.AddItem("X", this.m_fX + fShiftX + this.m_arrBorders[3] + this.m_arrMargins[3]);
				pdfParam.AddItem("Y", this.m_pHtmlManager.m_fShiftY - (this.m_fY + fShiftY) - this.m_arrBorders[0] - this.m_arrMargins[0]);
				pdfParam.AddItem("Width", this.m_fWidth - this.m_arrBorders[1] - this.m_arrBorders[3]);
				pdfParam.AddItem("Height", this.m_fHeight - this.m_arrBorders[0] - this.m_arrBorders[2]);
				pdfParam.AddItem("Size", this.m_pInputChunk.m_fSize);
				pCanvas.SetFillColor(this.m_pInputChunk.m_rgbColor.r, this.m_pInputChunk.m_rgbColor.g, this.m_pInputChunk.m_rgbColor.b);
				pCanvas.DrawText(this.m_bstrValue, pdfParam, this.m_pInputChunk.m_ptrFont);
			}
			pCanvas.RestoreState();
			return true;
		}
		private void HandleAmpersandChars(ref StringBuilder bstrValue)
		{
			int length = bstrValue.Length;
			if (length == 0)
			{
				return;
			}
			HtmlManager htmlManager = new HtmlManager();
			htmlManager.m_pBuffer = new char[length + 10];
			htmlManager.m_nBufferSize = length + 1;
			htmlManager.m_pBuffer[length] = ' ';
			htmlManager.m_pBuffer[length + 1] = '\0';
			Array.Copy(bstrValue.ToString().ToCharArray(), htmlManager.m_pBuffer, length);
			htmlManager.m_pDocument = this.m_pHtmlManager.m_pDocument;
			for (int i = 0; i < length; i++)
			{
				if (htmlManager.m_pBuffer[i] == ' ')
				{
					htmlManager.m_pBuffer[i] = '\u00a0';
				}
			}
			htmlManager.m_nPtr = 0;
			HtmlWord htmlWord = new HtmlWord(htmlManager);
			try
			{
				htmlWord.Next();
				htmlWord.Parse();
			}
			catch (PdfException)
			{
			}
			if (htmlWord.m_arrChunks.Count > 0)
			{
				bstrValue = new StringBuilder(htmlWord.m_arrChunks[0].m_bstrText.ToString());
			}
		}
		public override void Parse()
		{
		}
	}
}
