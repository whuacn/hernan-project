using System;
using System.Collections;
namespace Persits.PDF
{
	public class PdfCells : IEnumerable
	{
		internal PdfRow m_pRow;
		internal PdfManager m_pManager;
		public int Count
		{
			get
			{
				return this.m_pRow.m_ptrCells.Count;
			}
		}
		public PdfCell this[int Index]
		{
			get
			{
				if (Index < 1 || Index > this.Count)
				{
					AuxException.Throw(string.Format("Index out of range, must be an integer between 1 and {0}.", this.Count), PdfErrors._ERROR_OUTOFRANGE);
				}
				return this.m_pRow.m_ptrCells[Index - 1];
			}
		}
		internal PdfCells()
		{
		}
		public IEnumerator GetEnumerator()
		{
			return this.m_pRow.m_ptrCells.GetEnumerator();
		}
	}
}
