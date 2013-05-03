using System;
using System.Collections;
namespace Persits.PDF
{
	public class PdfRows : IEnumerable
	{
		internal PdfTable m_pTable;
		internal PdfManager m_pManager;
		public int Count
		{
			get
			{
				return this.m_pTable.m_ptrRows.Count;
			}
		}
		public PdfRow this[int Index]
		{
			get
			{
				if (Index < 1 || Index > this.Count)
				{
					AuxException.Throw(string.Format("Index out of range, must be an integer between 1 and {0}.", this.Count), PdfErrors._ERROR_OUTOFRANGE);
				}
				return this.m_pTable.m_ptrRows[Index - 1];
			}
		}
		internal PdfRows()
		{
		}
		public IEnumerator GetEnumerator()
		{
			return this.m_pTable.m_ptrRows.GetEnumerator();
		}
		public PdfRow Add(float Height)
		{
			return this.Add(Height, -1);
		}
		public PdfRow Add(float Height, int AddAfter)
		{
			PdfRow pdfRow = new PdfRow();
			pdfRow.m_pManager = this.m_pManager;
			pdfRow.m_pTable = this.m_pTable;
			pdfRow.m_fHeight = Height;
			pdfRow.m_nIndex = this.m_pTable.m_ptrRows.Count + 1;
			pdfRow.m_rgbBorderColor = this.m_pTable.m_rgbCellBorderColor;
			pdfRow.m_rgbBgColor = this.m_pTable.m_rgbCellBgColor;
			pdfRow.m_fCellPadding = this.m_pTable.m_fCellPadding;
			pdfRow.m_fCellBorder = this.m_pTable.m_fCellBorder;
			if (this.m_pTable.m_ptrRows.Count == 0)
			{
				int num = (this.m_pTable.m_fBorder == 0f) ? -1 : 1;
				int num2 = (this.m_pTable.m_fBorder == 0f) ? 0 : 1;
				pdfRow.m_fX = this.m_pTable.m_fBorder;
				pdfRow.m_fY = -this.m_pTable.m_fBorder - (float)num2 * this.m_pTable.m_fCellSpacing;
				pdfRow.CreateCells(this.m_pTable.m_nCols, (this.m_pTable.m_fWidth - 2f * this.m_pTable.m_fBorder - (float)(this.m_pTable.m_nCols + num) * this.m_pTable.m_fCellSpacing) / (float)this.m_pTable.m_nCols, pdfRow.m_fHeight, -1f);
			}
			else
			{
				PdfRow pdfRow2 = this.m_pTable.m_ptrRows[this.m_pTable.m_ptrRows.Count - 1];
				pdfRow.m_fY = pdfRow2.m_fY - pdfRow2.m_fHeight - this.m_pTable.m_fCellSpacing;
				foreach (PdfCell current in pdfRow2.m_ptrCells)
				{
					pdfRow.CreateCells(1, current.m_fWidth, Height, current.m_fX);
				}
				int num3 = 1;
				foreach (PdfCell current2 in pdfRow.m_ptrCells)
				{
					current2.m_nIndex = num3++;
				}
			}
			this.m_pTable.m_nRows++;
			this.m_pTable.m_fHeight += pdfRow.m_fHeight + this.m_pTable.m_fCellSpacing;
			this.m_pTable.m_ptrRows.Add(pdfRow);
			if (AddAfter >= 0 && AddAfter < pdfRow.m_nIndex - 1)
			{
				pdfRow.MoveUp(pdfRow.m_nIndex - AddAfter - 1);
			}
			this.m_pTable.m_bInvalidated = true;
			return pdfRow;
		}
		public void Remove(int Index)
		{
			foreach (PdfRow current in this.m_pTable.m_ptrRows)
			{
				if (current.m_nIndex == Index)
				{
					PdfRow pdfRow = current;
					while ((pdfRow = this.m_pTable.NextRow(pdfRow)) != null)
					{
						pdfRow.m_nIndex--;
						pdfRow.m_fY += current.m_fHeight + this.m_pTable.m_fCellSpacing;
						foreach (PdfCell current2 in pdfRow.m_ptrCells)
						{
							current2.m_fY = pdfRow.m_fY;
						}
					}
					this.m_pTable.m_fHeight -= current.m_fHeight + this.m_pTable.m_fCellSpacing;
					this.m_pTable.m_ptrRows.Remove(current);
					break;
				}
			}
			this.m_pTable.m_bInvalidated = true;
		}
	}
}
