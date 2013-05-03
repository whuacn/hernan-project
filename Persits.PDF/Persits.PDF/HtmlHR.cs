using System;
namespace Persits.PDF
{
	internal class HtmlHR : HtmlBox
	{
		public AuxRGB m_rgbBgColor = new AuxRGB();
		public bool m_bNoShade;
		public bool m_bFlexibleWidth;
		private float m_fWidthPercent;
		public override ObjectType Type
		{
			get
			{
				return ObjectType.htmlHR;
			}
		}
		public HtmlHR(HtmlManager pMgr, HtmlCell pParentCell, HtmlTag pTag) : base(pMgr)
		{
			this.m_bFlexibleWidth = false;
			this.m_fWidthPercent = 100f;
			this.m_bNoShade = false;
			int num = 1;
			if (pTag.GetIntAttribute("noshade", ref num))
			{
				this.m_bNoShade = true;
			}
			this.m_fHeight = 2f * HtmlManager.fEmpiricalScaleFactor;
			this.m_pTreeItem.GetNumericProperty(CssProperties.HEIGHT, ref this.m_fHeight, false);
			float num2 = 0f;
			bool flag = false;
			if (this.m_pTreeItem.GetNumericProperty(CssProperties.WIDTH, ref num2, ref flag, false))
			{
				if (flag && num2 <= 100f && num2 > 0f)
				{
					this.m_fWidth = num2 * pParentCell.Width / 100f;
					this.m_bFlexibleWidth = true;
					this.m_fWidthPercent = num2;
				}
				else
				{
					this.m_fWidth = num2;
				}
			}
			else
			{
				this.m_fWidth = 0f;
				this.m_bFlexibleWidth = true;
				this.m_fWidthPercent = 100f;
			}
			this.m_dwAlignFlags = 256u;
			CssProperties cssProperties = CssProperties.NOTFOUND;
			if (this.m_pTreeItem.GetEnumProperty(CssProperties.TEXT_ALIGN, ref cssProperties))
			{
				if (cssProperties == CssProperties.V_RIGHT)
				{
					this.m_dwAlignFlags = 128u;
				}
				else
				{
					if (cssProperties == CssProperties.V_LEFT)
					{
						this.m_dwAlignFlags = 0u;
					}
				}
			}
			this.m_arrBorderColors[1].Set(15000804u);
			this.m_arrBorderColors[2].Set(15000804u);
			this.m_arrBorderColors[0].Set(6579300u);
			this.m_arrBorderColors[3].Set(6579300u);
			this.m_arrBorders[0] = (this.m_arrBorders[1] = (this.m_arrBorders[2] = (this.m_arrBorders[3] = 1f * HtmlManager.fEmpiricalScaleFactor)));
			this.m_pTreeItem.GetBorderProperties(this.m_arrBorderStyles, ref this.m_arrBorders, ref this.m_arrBorderColors);
			this.m_pTreeItem.GetMarginProperties(ref this.m_arrMargins);
			this.m_fWidth += this.m_arrMargins[1] + this.m_arrMargins[3];
			this.m_fHeight += this.m_arrMargins[0] + this.m_arrMargins[2];
			this.m_pTreeItem.GetColorProperty(CssProperties.BACKGROUND_COLOR, ref this.m_rgbBgColor, false);
			base.ObtainPageBreakStatus();
		}
		public override void ExpandIfNecessary(float fAvailWidth)
		{
			if (!this.m_bFlexibleWidth)
			{
				return;
			}
			this.m_fWidth = fAvailWidth * this.m_fWidthPercent / 100f;
		}
		public override void Parse()
		{
		}
		public override void AssignCoordinates()
		{
		}
		public override bool Render(PdfPage pPage, PdfCanvas pCanvas, float fShiftX, float fShiftY)
		{
			if (!base.Render(pPage, pCanvas, fShiftX, fShiftY))
			{
				return false;
			}
			if (this.m_bNoShade || this.m_rgbBgColor.m_bIsSet)
			{
				pCanvas.SaveState();
				AuxRGB auxRGB = new AuxRGB();
				auxRGB.Set(6579300u);
				if (this.m_rgbBgColor.m_bIsSet)
				{
					auxRGB.Set(ref this.m_rgbBgColor);
				}
				pCanvas.SetFillColor(auxRGB.r, auxRGB.g, auxRGB.b);
				pCanvas.FillRect(this.m_fX + fShiftX + this.m_arrMargins[3], this.m_pHtmlManager.m_fShiftY - (this.m_fY + this.m_fHeight + fShiftY) + this.m_arrMargins[0], this.m_fWidth - this.m_arrMargins[1] - this.m_arrMargins[3], this.m_fHeight - this.m_arrMargins[0] - this.m_arrMargins[2]);
				pCanvas.RestoreState();
			}
			else
			{
				base.DrawBorder(pCanvas, fShiftX, fShiftY);
			}
			return true;
		}
		public override void CalculateSize()
		{
		}
	}
}
