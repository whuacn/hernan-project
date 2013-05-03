using System;
namespace Persits.PDF
{
	internal abstract class HtmlBox : HtmlObject
	{
		public float[] m_arrBorders = new float[4];
		public float[] m_arrMargins = new float[4];
		public CssProperties[] m_arrBorderStyles = new CssProperties[4];
		public AuxRGB[] m_arrBorderColors = new AuxRGB[4];
		public HtmlAttributes m_nAlignment;
		public HtmlBox(HtmlManager pMgr) : base(pMgr)
		{
			this.m_arrBorders[0] = (this.m_arrBorders[1] = (this.m_arrBorders[2] = (this.m_arrBorders[3] = 0f)));
			this.m_arrMargins[0] = (this.m_arrMargins[1] = (this.m_arrMargins[2] = (this.m_arrMargins[3] = 0f)));
			CssProperties cssProperties = CssProperties.NOTFOUND;
			if (this.m_pTreeItem.GetEnumProperty(CssProperties.POSITION, ref cssProperties, false) && cssProperties == CssProperties.V_ABSOLUTE)
			{
				if (this.m_pTreeItem.GetNumericProperty(CssProperties.LEFT, ref this.m_fX, false))
				{
					this.m_bAbsoluteLeft = true;
				}
				if (this.m_pTreeItem.GetNumericProperty(CssProperties.TOP, ref this.m_fY, false))
				{
					this.m_bAbsoluteTop = true;
				}
				this.m_bAbsolutePosition = (this.m_bAbsoluteLeft || this.m_bAbsoluteTop);
			}
			if (this.m_pTreeItem.GetEnumProperty(CssProperties.DISPLAY, ref cssProperties, false))
			{
				this.m_bDisplayNone = (cssProperties == CssProperties.V_NONE);
			}
			for (int i = 0; i < this.m_arrBorderColors.Length; i++)
			{
				this.m_arrBorderColors[i] = new AuxRGB();
			}
		}
		public void DrawBorder(PdfCanvas pCanvas, float fShiftX, float fShiftY)
		{
			this.DrawBorder(pCanvas, fShiftX, fShiftY, 0f, 0f, 0f, 0f);
		}
		public void DrawBorder(PdfCanvas pCanvas, float fShiftX, float fShiftY, float fAdjX1, float fAdjY1, float fAdjX2, float fAdjY2)
		{
			float num = this.m_fX + fShiftX + this.m_arrMargins[3] + fAdjX1;
			float num2 = this.m_pHtmlManager.m_fShiftY - (this.m_fY + this.m_fHeight + fShiftY) + this.m_arrMargins[2] + fAdjY1;
			float num3 = num + this.m_fWidth - this.m_arrMargins[3] - this.m_arrMargins[1] + fAdjX2;
			float num4 = num2 + this.m_fHeight - this.m_arrMargins[0] - this.m_arrMargins[2] + fAdjY2;
			if (num4 < this.m_pHtmlManager.m_fLowerMargin || num2 > this.m_pHtmlManager.m_fLowerMargin + this.m_pHtmlManager.m_fFrameHeight)
			{
				return;
			}
			if (this.m_arrBorders[0] > 0f)
			{
				pCanvas.SetFillColor(this.m_arrBorderColors[0].r, this.m_arrBorderColors[0].g, this.m_arrBorderColors[0].b);
				pCanvas.MoveTo(num, num4);
				pCanvas.LineTo(num3, num4);
				pCanvas.LineTo(num3 - this.m_arrBorders[1], num4 - this.m_arrBorders[0]);
				pCanvas.LineTo(num + this.m_arrBorders[3], num4 - this.m_arrBorders[0]);
				pCanvas.LineTo(num, num4);
				pCanvas.Fill(false);
			}
			if (this.m_arrBorders[1] > 0f)
			{
				pCanvas.SetFillColor(this.m_arrBorderColors[1].r, this.m_arrBorderColors[1].g, this.m_arrBorderColors[1].b);
				pCanvas.MoveTo(num3, num4);
				pCanvas.LineTo(num3, num2);
				pCanvas.LineTo(num3 - this.m_arrBorders[1], num2 + this.m_arrBorders[2]);
				pCanvas.LineTo(num3 - this.m_arrBorders[1], num4 - this.m_arrBorders[0]);
				pCanvas.LineTo(num3, num4);
				pCanvas.Fill(false);
			}
			if (this.m_arrBorders[2] > 0f)
			{
				pCanvas.SetFillColor(this.m_arrBorderColors[2].r, this.m_arrBorderColors[2].g, this.m_arrBorderColors[2].b);
				pCanvas.MoveTo(num, num2);
				pCanvas.LineTo(num3, num2);
				pCanvas.LineTo(num3 - this.m_arrBorders[1], num2 + this.m_arrBorders[2]);
				pCanvas.LineTo(num + this.m_arrBorders[3], num2 + this.m_arrBorders[2]);
				pCanvas.LineTo(num, num2);
				pCanvas.Fill(false);
			}
			if (this.m_arrBorders[3] > 0f)
			{
				pCanvas.SetFillColor(this.m_arrBorderColors[3].r, this.m_arrBorderColors[3].g, this.m_arrBorderColors[3].b);
				pCanvas.MoveTo(num, num2);
				pCanvas.LineTo(num, num4);
				pCanvas.LineTo(num + this.m_arrBorders[3], num4 - this.m_arrBorders[0]);
				pCanvas.LineTo(num + this.m_arrBorders[3], num2 + this.m_arrBorders[2]);
				pCanvas.LineTo(num, num2);
				pCanvas.Fill(false);
			}
		}
	}
}
