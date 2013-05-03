using System;
using System.Collections.Generic;
namespace Persits.PDF
{
	public class PdfRow
	{
		internal PdfManager m_pManager;
		internal PdfTable m_pTable;
		internal float m_fHeight;
		internal int m_nIndex;
		internal float m_fX;
		internal float m_fY;
		internal float m_fCellPadding;
		internal float m_fCellBorder;
		internal bool m_bFixed;
		internal float m_fHeightAdjustment;
		internal AuxRGB m_rgbBorderColor;
		internal AuxRGB m_rgbBgColor;
		internal List<PdfCell> m_ptrCells = new List<PdfCell>();
		public float Height
		{
			get
			{
				return this.m_fHeight;
			}
			set
			{
				if (value <= 0f)
				{
					AuxException.Throw("Invalid Height parameter, must be a positive number.", PdfErrors._ERROR_OUTOFRANGE);
				}
				float num = value - this.m_fHeight;
				PdfCell pdfCell2;
				for (PdfCell pdfCell = this.FirstCell(); pdfCell != null; pdfCell = this.NextCell(pdfCell))
				{
					pdfCell.m_fHeight += num;
					pdfCell2 = pdfCell;
					while ((pdfCell2 = this.CellBelow(pdfCell2)) != null)
					{
						pdfCell2.m_fY -= num;
					}
				}
				pdfCell2 = this.FirstCell();
				while ((pdfCell2 = this.CellBelow(pdfCell2)) != null)
				{
					pdfCell2.m_pRow.m_fY -= num;
				}
				this.m_pTable.m_fHeight += num;
				this.m_fHeight += num;
				this.m_pTable.m_bInvalidated = true;
			}
		}
		public PdfCells Cells
		{
			get
			{
				return new PdfCells
				{
					m_pManager = this.m_pManager,
					m_pRow = this
				};
			}
		}
		public PdfCell this[int Index]
		{
			get
			{
				return this.Cells[Index];
			}
		}
		public int Index
		{
			get
			{
				return this.m_nIndex;
			}
		}
		public object BgColor
		{
			set
			{
				foreach (PdfCell current in this.m_ptrCells)
				{
					current.BgColor = value;
				}
			}
		}
		public object BorderColor
		{
			set
			{
				foreach (PdfCell current in this.m_ptrCells)
				{
					current.BorderColor = value;
				}
			}
		}
		public float X
		{
			get
			{
				return this.m_fX;
			}
		}
		public float Y
		{
			get
			{
				return this.m_fY;
			}
		}
		public bool Fixed
		{
			get
			{
				return this.m_bFixed;
			}
			set
			{
				this.m_bFixed = value;
			}
		}
		internal PdfRow()
		{
			this.m_fHeight = 0f;
			this.m_nIndex = 0;
			this.m_fX = 0f;
			this.m_fY = 0f;
			this.m_fCellPadding = 0f;
			this.m_fCellBorder = 1f;
			this.m_bFixed = false;
			this.m_fHeightAdjustment = 0f;
		}
		internal void CreateCells(int CellNum, float Width, float Height, float X)
		{
			for (int i = 0; i < CellNum; i++)
			{
				PdfCell pdfCell = new PdfCell();
				pdfCell.m_pManager = this.m_pManager;
				pdfCell.m_pTable = this.m_pTable;
				pdfCell.m_pRow = this;
				pdfCell.m_nIndex = i + 1;
				PdfCell arg_40_0 = pdfCell;
				pdfCell.m_fSpanWidth = Width;
				arg_40_0.m_fWidth = Width;
				PdfCell arg_52_0 = pdfCell;
				pdfCell.m_fSpanHeight = Height;
				arg_52_0.m_fHeight = Height;
				pdfCell.m_fCellPadding = this.m_fCellPadding;
				pdfCell.m_fBorder = this.m_fCellBorder;
				int num = (this.m_pTable.m_fBorder == 0f) ? 0 : 1;
				pdfCell.m_fX = this.m_fX + (float)i * Width + (float)(i + num) * this.m_pTable.m_fCellSpacing;
				pdfCell.m_fY = this.m_fY;
				if (X != -1f)
				{
					pdfCell.m_fX = X;
				}
				pdfCell.m_rgbBorderColor = this.m_rgbBorderColor;
				pdfCell.m_rgbBgColor = this.m_rgbBgColor;
				this.m_ptrCells.Add(pdfCell);
			}
		}
		internal void Draw(PdfCanvas pCanvas, float X, float Y)
		{
			foreach (PdfCell current in this.m_ptrCells)
			{
				current.Draw(pCanvas, X, Y);
			}
		}
		internal PdfCell FirstCell()
		{
			if (this.m_ptrCells.Count == 0)
			{
				return null;
			}
			return this.m_ptrCells[0];
		}
		internal PdfCell NextCell(PdfCell pCurrentCell)
		{
			int i = 0;
			while (i < this.m_ptrCells.Count)
			{
				if (this.m_ptrCells[i] == pCurrentCell)
				{
					if (i + 1 < this.m_ptrCells.Count)
					{
						return this.m_ptrCells[i + 1];
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
		internal PdfCell CellAbove(PdfCell pCurrentCell)
		{
			if (pCurrentCell == null)
			{
				return null;
			}
			PdfRow pdfRow = this.m_pTable.PrevRow(pCurrentCell.m_pRow);
			if (pdfRow == null)
			{
				return null;
			}
			foreach (PdfCell current in pdfRow.m_ptrCells)
			{
				if (current.m_nIndex == pCurrentCell.m_nIndex)
				{
					return current;
				}
			}
			return null;
		}
		internal PdfCell CellBelow(PdfCell pCurrentCell)
		{
			if (pCurrentCell == null)
			{
				return null;
			}
			PdfRow pdfRow = this.m_pTable.NextRow(pCurrentCell.m_pRow);
			if (pdfRow == null)
			{
				return null;
			}
			foreach (PdfCell current in pdfRow.m_ptrCells)
			{
				if (current.m_nIndex == pCurrentCell.m_nIndex)
				{
					return current;
				}
			}
			return null;
		}
		internal PdfCell GetCellByIndex(int nIndex)
		{
			foreach (PdfCell current in this.m_ptrCells)
			{
				if (current.m_nIndex == nIndex)
				{
					return current;
				}
			}
			return null;
		}
		public void MoveUp(int Rows)
		{
			int num = Rows;
			PdfRow pdfRow = this;
			while (num > 0 && (pdfRow = this.m_pTable.PrevRow(this)) != null)
			{
				int num2 = 0;
				while (num2 < this.m_pTable.m_ptrRows.Count && this.m_pTable.m_ptrRows[num2 + 1] != this)
				{
					num2++;
				}
				PdfRow value = this.m_pTable.m_ptrRows[num2 + 1];
				this.m_pTable.m_ptrRows[num2 + 1] = this.m_pTable.m_ptrRows[num2];
				this.m_pTable.m_ptrRows[num2] = value;
				int nIndex = pdfRow.m_nIndex;
				pdfRow.m_nIndex = this.m_nIndex;
				this.m_nIndex = nIndex;
				this.m_fY = pdfRow.m_fY;
				pdfRow.m_fY -= this.m_fHeight + this.m_pTable.m_fCellSpacing;
				foreach (PdfCell current in this.m_ptrCells)
				{
					current.m_fY = this.m_fY;
				}
				foreach (PdfCell current2 in pdfRow.m_ptrCells)
				{
					current2.m_fY = pdfRow.m_fY;
				}
				num--;
			}
			this.m_pTable.m_bInvalidated = true;
		}
		public void MoveDown(int Rows)
		{
			int num = Rows;
			PdfRow pdfRow = this;
			while (num > 0 && (pdfRow = this.m_pTable.NextRow(this)) != null)
			{
				int num2 = 0;
				while (num2 < this.m_pTable.m_ptrRows.Count && this.m_pTable.m_ptrRows[num2] != this)
				{
					num2++;
				}
				PdfRow value = this.m_pTable.m_ptrRows[num2 + 1];
				this.m_pTable.m_ptrRows[num2 + 1] = this.m_pTable.m_ptrRows[num2];
				this.m_pTable.m_ptrRows[num2] = value;
				int nIndex = pdfRow.m_nIndex;
				pdfRow.m_nIndex = this.m_nIndex;
				this.m_nIndex = nIndex;
				this.m_fY -= pdfRow.m_fHeight + this.m_pTable.m_fCellSpacing;
				pdfRow.m_fY += this.m_fHeight + this.m_pTable.m_fCellSpacing;
				foreach (PdfCell current in this.m_ptrCells)
				{
					current.m_fY = this.m_fY;
				}
				foreach (PdfCell current2 in pdfRow.m_ptrCells)
				{
					current2.m_fY = pdfRow.m_fY;
				}
				num--;
			}
			this.m_pTable.m_bInvalidated = true;
		}
		internal float SpanHeight()
		{
			float num = this.m_fHeight;
			foreach (PdfCell current in this.m_ptrCells)
			{
				if (num < current.m_fSpanHeight)
				{
					num = current.m_fSpanHeight;
				}
			}
			return num;
		}
	}
}
