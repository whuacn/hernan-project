/************************************************ 2014 Pete_H *******************************************************
 * 
 * This software released under the Code Project Open License. Refer to: http://www.codeproject.com/info/cpol10.aspx
 * or refer to the copy of the Code Project Open License (CPOL.htm) included with this solution. 
 * 
 * This code and the compiled components including libraries and the demonstration application have been made 
 * available only for the purpose of learning, sharing and demonstrating ideas and NOT to imply, recommend or 
 * suggest usage of any part of the code or components.
 * 
 * No claim of suitability, guarantee, or any warranty whatsoever is provided. The software is provided "as-is"
 * Usage of any of this code or components is entirely at your own risk.
 * 
 ********************************************************************************************************************/
using System.Drawing;
using System.Windows.Forms;

namespace SqlExpressTraceUI.CustomControls
{
	/// <summary>
	/// Extendeds DataGridView to add column grouping and row header line numbers.
	/// </summary>
	public class CustomDataGridView : DataGridView
	{
		private bool _groupSortedColumnValues = false;

		public CustomDataGridView()
		{
			preventFlicker();
		}

		public bool DisplayRowNumbers { get; set; }

		public bool GroupSortedColumnValues
		{
			get { return ((_groupSortedColumnValues) && (this.SortedColumn != null)); }

			set
			{
				_groupSortedColumnValues = value;
				this.Refresh();
			}
		}

		protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs formattingEventArgs)
		{

			base.OnCellFormatting(formattingEventArgs);

			if ((!GroupSortedColumnValues) || (formattingEventArgs.ColumnIndex != this.SortedColumn.Index) ||
				(formattingEventArgs.RowIndex == 0)) return;

			if (IsRepeatedCellValue(formattingEventArgs.RowIndex, formattingEventArgs.ColumnIndex))
			{
				formattingEventArgs.Value = string.Empty;
				formattingEventArgs.FormattingApplied = true;
			}
		}

		protected override void OnCellPainting(DataGridViewCellPaintingEventArgs paintingEventArgs)
		{
			base.OnCellPainting(paintingEventArgs);

			if ((!GroupSortedColumnValues) || (paintingEventArgs.ColumnIndex != this.SortedColumn.Index)) return;

			paintingEventArgs.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;

			if ((paintingEventArgs.RowIndex < 1) || (paintingEventArgs.ColumnIndex < 0)) return;

			if (IsRepeatedCellValue(paintingEventArgs.RowIndex, paintingEventArgs.ColumnIndex))
				paintingEventArgs.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
			else
				paintingEventArgs.AdvancedBorderStyle.Top = AdvancedCellBorderStyle.Top;
		}

		protected override void OnRowPostPaint(DataGridViewRowPostPaintEventArgs e)
		{
			if (this.DisplayRowNumbers && this.RowHeadersVisible)
			{
				string rowNumber = (e.RowIndex + 1).ToString();
				System.Drawing.SizeF size = e.Graphics.MeasureString(rowNumber, this.Font);

				//if necessary
				if (this.RowHeadersWidth < (int)(size.Width + 20)) this.RowHeadersWidth = (int)(size.Width + 20);

				//this brush will be used to draw the row number string on the
				//row header cell using the system's current ControlText color
				Brush b = SystemBrushes.ControlText;

				//draw the row number string on the current row header cell using
				//the brush defined above and the DataGridView's default font
				e.Graphics.DrawString(rowNumber, this.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2));

				base.OnRowPostPaint(e);

			}

		}

		private bool IsRepeatedCellValue(int rowIndex, int colIndex)
		{
			if (colIndex != this.SortedColumn.Index) return false;

			DataGridViewCell currentCell = Rows[rowIndex].Cells[colIndex];
			DataGridViewCell previousCell = Rows[rowIndex - 1].Cells[colIndex];

			if ((currentCell.Value == previousCell.Value) || (currentCell.Value != null && previousCell.Value != null &&
				currentCell.Value.ToString() == previousCell.Value.ToString()))
			{
				return true;
			}
			else return false;
		}

		private void preventFlicker()
		{
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.Opaque, true);

			// These two settings are for forcing the grid to use a double buffer and opaque background to control flickering
			// when updating the underlying datasource.
			//object[] arguments = new object[] { };
			//this.GetType().InvokeMember("SetStyle", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy,
			//                                        null, this, arguments);
			//arguments = new object[] { ControlStyles.Opaque, true };
			//this.GetType().InvokeMember("SetStyle", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy,
			//                                       null, this, arguments);
		}
	}
}