using System;
using System.Collections.Generic;
namespace Persits.PDF
{
	public class PdfTable
	{
		internal PdfManager m_pManager;
		internal PdfDocument m_pDoc;
		internal float m_fWidth;
		internal float m_fHeight;
		internal float m_fBorder;
		internal float m_fCellBorder;
		internal PdfFont m_ptrFont;
		internal int m_nRows;
		internal int m_nCols;
		internal float m_fCellSpacing;
		internal float m_fCellPadding;
		internal bool m_bInvalidated;
		internal AuxRGB m_rgbBgColor = new AuxRGB();
		internal AuxRGB m_rgbBorderColor = new AuxRGB();
		internal AuxRGB m_rgbCellBgColor = new AuxRGB();
		internal AuxRGB m_rgbCellBorderColor = new AuxRGB();
		internal List<PdfRow> m_ptrRows = new List<PdfRow>();
		internal List<PdfPoint> m_ptrFromTo = new List<PdfPoint>();
		public float Width
		{
			get
			{
				return this.m_fWidth;
			}
		}
		public float Height
		{
			get
			{
				return this.m_fHeight;
			}
		}
		public float Border
		{
			get
			{
				return this.m_fBorder;
			}
			set
			{
				this.m_fBorder = value;
			}
		}
		public int RowNum
		{
			get
			{
				return this.m_nRows;
			}
		}
		public int ColNum
		{
			get
			{
				return this.m_nCols;
			}
		}
		public PdfRows Rows
		{
			get
			{
				return new PdfRows
				{
					m_pTable = this,
					m_pManager = this.m_pManager
				};
			}
		}
		public PdfFont Font
		{
			get
			{
				if (this.m_ptrFont == null)
				{
					AuxException.Throw("Font property not set.", PdfErrors._ERROR_PROPNOTSET);
				}
				return this.m_ptrFont;
			}
			set
			{
				this.m_ptrFont = value;
			}
		}
		public PdfCell this[int Row, int Col]
		{
			get
			{
				return this.At(Row, Col);
			}
		}
		internal PdfTable()
		{
			this.m_fWidth = (this.m_fHeight = (this.m_fBorder = 0f));
			this.m_nRows = 1;
			this.m_nCols = 1;
			this.m_fCellSpacing = (this.m_fCellPadding = 0f);
			this.m_bInvalidated = false;
			this.m_fCellBorder = 1f;
		}
		internal void Create(PdfParam pParam)
		{
			if (!pParam.IsSet("Width") || !pParam.IsSet("Height"))
			{
				AuxException.Throw("Width and Height parameters expected.", PdfErrors._ERROR_INVALIDARGUMENT);
			}
			this.m_fWidth = pParam.Number("Width");
			this.m_fHeight = pParam.Number("Height");
			if (pParam.IsSet("Cols"))
			{
				this.m_nCols = pParam.Long("Cols");
			}
			if (pParam.IsSet("Rows"))
			{
				this.m_nRows = pParam.Long("Rows");
			}
			if (this.m_nRows * this.m_nCols == 0 || this.m_nRows * this.m_nCols > 100000 || this.m_nRows * this.m_nCols < 0)
			{
				AuxException.Throw("Invalid Rows and/or Cols values.", PdfErrors._ERROR_INVALIDARGUMENT);
			}
			if (pParam.IsSet("CellSpacing"))
			{
				this.m_fCellSpacing = pParam.Number("CellSpacing");
			}
			if (pParam.IsSet("CellPadding"))
			{
				this.m_fCellPadding = pParam.Number("CellPadding");
			}
			if (pParam.IsSet("Border"))
			{
				this.m_fBorder = pParam.Number("Border");
			}
			if (pParam.IsSet("CellBorder"))
			{
				this.m_fCellBorder = pParam.Number("CellBorder");
			}
			if (pParam.IsSet("BorderColor"))
			{
				pParam.Color("BorderColor", ref this.m_rgbBorderColor);
			}
			if (pParam.IsSet("CellBorderColor"))
			{
				pParam.Color("CellBorderColor", ref this.m_rgbCellBorderColor);
			}
			if (pParam.IsSet("BGColor"))
			{
				pParam.Color("BGColor", ref this.m_rgbBgColor);
			}
			if (pParam.IsSet("CellBGColor"))
			{
				pParam.Color("CellBGColor", ref this.m_rgbCellBgColor);
			}
			for (int i = 0; i < this.m_nRows; i++)
			{
				PdfRow pdfRow = new PdfRow();
				pdfRow.m_pManager = this.m_pManager;
				pdfRow.m_pTable = this;
				pdfRow.m_nIndex = i + 1;
				int num = (this.m_fBorder == 0f) ? -1 : 1;
				int num2 = (this.m_fBorder == 0f) ? 0 : 1;
				pdfRow.m_fHeight = (this.m_fHeight - 2f * this.m_fBorder - (float)(this.m_nRows + num) * this.m_fCellSpacing) / (float)this.m_nRows;
				pdfRow.m_fX = this.m_fBorder;
				pdfRow.m_fY = -this.m_fBorder - (float)(i + num2) * this.m_fCellSpacing - (float)i * pdfRow.m_fHeight;
				pdfRow.m_rgbBorderColor = this.m_rgbCellBorderColor;
				pdfRow.m_rgbBgColor = this.m_rgbCellBgColor;
				pdfRow.m_fCellBorder = this.m_fCellBorder;
				pdfRow.m_fCellPadding = this.m_fCellPadding;
				pdfRow.CreateCells(this.m_nCols, (this.m_fWidth - 2f * this.m_fBorder - (float)(this.m_nCols + num) * this.m_fCellSpacing) / (float)this.m_nCols, pdfRow.m_fHeight, -1f);
				this.m_ptrRows.Add(pdfRow);
			}
		}
		internal int Draw(PdfCanvas pCanvas, object Param)
		{
			PdfParam pdfParam = this.m_pManager.VariantToParam(Param);
			if (!pdfParam.IsSet("X") || !pdfParam.IsSet("Y"))
			{
				AuxException.Throw("X, Y parameters must be set.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			float num = pdfParam.Number("X");
			float num2 = pdfParam.Number("Y");
			float num3 = num2;
			if (pdfParam.IsSet("MaxHeight"))
			{
				num3 = pdfParam.Number("MaxHeight");
			}
			int num4 = 0;
			this.m_ptrFromTo.Clear();
			while (true)
			{
				string name;
				string name2;
				if (num4 == 0)
				{
					name = "RowFrom";
					name2 = "RowTo";
				}
				else
				{
					name = string.Format("RowFrom{0}", num4);
					name2 = string.Format("RowTo{0}", num4);
				}
				if (!pdfParam.IsSet(name))
				{
					break;
				}
				int num5 = pdfParam.Long(name);
				int num6 = this.m_ptrRows.Count;
				if (pdfParam.IsSet(name2))
				{
					num6 = pdfParam.Long(name2);
				}
				if (num5 < 1 || num5 > this.m_ptrRows.Count || num6 < 1 || num6 > this.m_ptrRows.Count || num6 < num5)
				{
					AuxException.Throw("Invalid RowFrom and/or RowTo values.", PdfErrors._ERROR_OUTOFRANGE);
				}
				PdfPoint pdfPoint = new PdfPoint();
				pdfPoint.x = num5;
				pdfPoint.y = num6;
				this.m_ptrFromTo.Add(pdfPoint);
				num4++;
			}
			this.AdjustCellSizes();
			this.HandleVerticalAlignment();
			float num7 = (this.m_fBorder == 0f) ? 0f : (2f * this.m_fBorder + this.m_fCellSpacing);
			float num8 = 0f;
			int num9 = -1;
			foreach (PdfRow current in this.m_ptrRows)
			{
				current.m_fHeightAdjustment = 0f;
				if (!this.ShouldBeDisplayed(current.m_nIndex))
				{
					num8 += current.m_fHeight + this.m_fCellSpacing;
				}
				else
				{
					current.m_fHeightAdjustment = num8;
					if (num7 + current.SpanHeight() + this.m_fCellSpacing > num3)
					{
						num9 = current.m_nIndex - 1;
						break;
					}
					num7 += current.m_fHeight + this.m_fCellSpacing;
				}
			}
			if (num9 == 0)
			{
				return 0;
			}
			if (this.m_rgbBgColor.m_bIsSet)
			{
				pCanvas.SetFillColor(this.m_rgbBgColor.r, this.m_rgbBgColor.g, this.m_rgbBgColor.b);
				pCanvas.FillRect(num + this.m_fBorder / 2f, num2 - num7 + this.m_fBorder / 2f, this.m_fWidth - this.m_fBorder, num7 - this.m_fBorder);
			}
			if (this.m_fBorder > 0f)
			{
				if (this.m_rgbBorderColor.m_bIsSet)
				{
					pCanvas.SetColor(this.m_rgbBorderColor.r, this.m_rgbBorderColor.g, this.m_rgbBorderColor.b);
				}
				pCanvas.LineWidth = this.m_fBorder;
				pCanvas.DrawRect(num + this.m_fBorder / 2f, num2 - num7 + this.m_fBorder / 2f, this.m_fWidth - this.m_fBorder, num7 - this.m_fBorder);
			}
			int result = 0;
			foreach (PdfRow current2 in this.m_ptrRows)
			{
				if (this.ShouldBeDisplayed(current2.m_nIndex))
				{
					current2.Draw(pCanvas, num, num2 + current2.m_fHeightAdjustment);
					result = current2.m_nIndex;
					if (current2.m_nIndex == num9)
					{
						break;
					}
				}
			}
			return result;
		}
		internal bool ShouldBeDisplayed(int nIndex)
		{
			if (this.m_ptrFromTo.Count == 0)
			{
				return true;
			}
			foreach (PdfPoint current in this.m_ptrFromTo)
			{
				if (nIndex >= current.x && nIndex <= current.y)
				{
					return true;
				}
			}
			return false;
		}
		internal PdfRow FirstRow()
		{
			if (this.m_ptrRows.Count == 0)
			{
				return null;
			}
			return this.m_ptrRows[0];
		}
		internal PdfRow PrevRow(PdfRow pCurrentRow)
		{
			for (int i = 0; i < this.m_ptrRows.Count - 1; i++)
			{
				if (this.m_ptrRows[i + 1] == pCurrentRow)
				{
					return this.m_ptrRows[i];
				}
			}
			return null;
		}
		internal PdfRow NextRow(PdfRow pCurrentRow)
		{
			int i = 0;
			while (i < this.m_ptrRows.Count)
			{
				if (this.m_ptrRows[i] == pCurrentRow)
				{
					if (i + 1 < this.m_ptrRows.Count)
					{
						return this.m_ptrRows[i + 1];
					}
					return null;
				}
				else
				{
					i++;
				}
			}
			return null;
		}
		internal void AdjustCellSizes()
		{
			if (!this.m_bInvalidated)
			{
				return;
			}
			foreach (PdfRow current in this.m_ptrRows)
			{
				foreach (PdfCell current2 in current.m_ptrCells)
				{
					current2.m_bSwallowed = false;
				}
			}
			foreach (PdfRow current3 in this.m_ptrRows)
			{
				foreach (PdfCell current4 in current3.m_ptrCells)
				{
					current4.m_fSpanHeight = current4.m_fHeight;
					current4.m_fSpanWidth = current4.m_fWidth;
					if (current4.m_nColSpan != 1 || current4.m_nRowSpan != 1)
					{
						if (current4.m_nColSpan > 1)
						{
							int num = current4.m_nColSpan - 1;
							PdfCell pdfCell = current4;
							while (num > 0 && (pdfCell = current3.NextCell(pdfCell)) != null && !pdfCell.CannotBeSwallowed())
							{
								current4.m_fSpanWidth += pdfCell.m_fWidth + this.m_fCellSpacing;
								pdfCell.MarkAsSwallowed(current4.m_nRowSpan - 1, false);
								pdfCell.m_bSwallowed = true;
								num--;
							}
						}
						if (current4.m_nRowSpan > 1)
						{
							int num2 = current4.m_nRowSpan - 1;
							PdfCell pdfCell2 = current4;
							while (num2 > 0 && (pdfCell2 = current3.CellBelow(pdfCell2)) != null && !pdfCell2.CannotBeSwallowed())
							{
								current4.m_fSpanHeight += pdfCell2.m_fHeight + this.m_fCellSpacing;
								pdfCell2.MarkAsSwallowed(current4.m_nColSpan - 1, true);
								pdfCell2.m_bSwallowed = true;
								num2--;
							}
						}
					}
				}
			}
			this.m_bInvalidated = false;
		}
		internal void HandleVerticalAlignment()
		{
			foreach (PdfRow current in this.m_ptrRows)
			{
				foreach (PdfCell current2 in current.m_ptrCells)
				{
					if (!current2.m_bSwallowed)
					{
						foreach (AuxTextChunk current3 in current2.m_ptrTextChunks)
						{
							PdfParam pParam = current3.m_pParam;
							if (current3.m_nVAlignment != 0)
							{
								float fNumber = (current2.m_fSpanHeight - current2.m_fCellPadding * 2f - current2.m_fBorder * 2f - current3.m_fVerticalExtent) / (float)((current3.m_nVAlignment == 2) ? 2 : 1);
								pParam.AssignValue("IndentY", fNumber);
							}
						}
					}
				}
			}
		}
		public PdfCell At(int Row, int Col)
		{
			if (Row < 1 || Row > this.m_ptrRows.Count)
			{
				AuxException.Throw("The Row parameter is out of range.", PdfErrors._ERROR_OUTOFRANGE);
			}
			PdfRow pdfRow = this.m_ptrRows[Row - 1];
			if (Col < 1 || Col > pdfRow.m_ptrCells.Count)
			{
				AuxException.Throw("The Col parameter is out of range.", PdfErrors._ERROR_OUTOFRANGE);
			}
			return pdfRow.m_ptrCells[Col - 1];
		}
	}
}
