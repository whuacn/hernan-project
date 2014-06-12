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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using SqlExpressTraceLib;

namespace SqlExpressTraceUI.CustomControls
{
	public delegate void ColumnOrderChangedEventHandler(object sender, int[] orderedColumns);

	public partial class TraceEventDisplay : UserControl
	{
		private bool _initializingColumns = false;
		private int[] _orderedColumns = null;
		private SortableBindingList<TraceRecord> _traceRecords = new SortableBindingList<TraceRecord>();
		private TraceStatus _traceStatus = TraceStatus.Closed;

		public TraceEventDisplay()
		{
			InitializeComponent();
			this.traceDisplayGrid.AutoGenerateColumns = false;
			this.traceDisplayGrid.DataSource = _traceRecords;
			setControlStatesForTraceStatus();
		}

		public event ColumnOrderChangedEventHandler ColumnOrderChanged;

		public Font DisplayGridFont
		{
			get { return this.traceDisplayGrid.Font; }
			set { this.traceDisplayGrid.Font = value; }
		}

		public TraceStatus Status
		{
			get { return _traceStatus; }

			set
			{
				bool changed = (this._traceStatus != value);
				this._traceStatus = value;
				if (changed) setControlStatesForTraceStatus();
			}
		}

		public int AddTraceRecord(TraceRecord record)
		{
			this._traceRecords.Add(record);
			int lastIdx = this.traceDisplayGrid.RowCount - 1;
			this.traceDisplayGrid.ClearSelection();
			if (this.autoScrollOption.Checked) this.traceDisplayGrid.FirstDisplayedScrollingRowIndex = lastIdx;

			return lastIdx;
		}

		public void InitializeColumns(int[] orderedColumnIds)
		{
			this._initializingColumns = true;

			this._orderedColumns = orderedColumnIds;
			this.traceDisplayGrid.Columns.Clear();
			this.traceDisplayGrid.AutoGenerateColumns = false;
			this.traceDisplayGrid.AllowUserToResizeColumns = true;

			foreach (int columnId in orderedColumnIds)
			{
				DataGridViewTextBoxColumn gridColumn = new DataGridViewTextBoxColumn();
				ColumnInfoItem columnInfo = TraceInfo.TraceColumnsInfoList[columnId];

				gridColumn.Name = columnInfo.Name;
				gridColumn.DataPropertyName = columnInfo.Name;
				gridColumn.HeaderText = columnInfo.Name;
				gridColumn.ReadOnly = true;

				gridColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
				gridColumn.Resizable = DataGridViewTriState.True;
				gridColumn.SortMode = DataGridViewColumnSortMode.Automatic;

				if (columnInfo.Name == "EventClass")
				{
					gridColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
					gridColumn.Width = getApproxTextWidthForType(TraceColumnDataType.String, sampleText: "SP:StmtCompleted");
				}
				else if ((columnInfo.DataType == TraceColumnDataType.Int32) ||
						(columnInfo.DataType == TraceColumnDataType.Int64))
				{
					gridColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
					gridColumn.Width = getApproxTextWidthForType(TraceColumnDataType.Int32);
				}
				else
				{
					gridColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
					gridColumn.Width = getApproxTextWidthForType(columnInfo.DataType);
				}

				this.traceDisplayGrid.Columns.Add(gridColumn);
			}

			this._initializingColumns = false;
		}

		private int getApproxTextWidthForType(TraceColumnDataType dataType, string sampleText = null)
		{
			int padding = 8;
			int points = 0;

			if (sampleText == null)
			{
				switch (dataType)
				{
					case TraceColumnDataType.Int32:
					case TraceColumnDataType.Int64:
						sampleText = int.MaxValue.ToString();
						break;

					case TraceColumnDataType.Guid:
						sampleText = Guid.NewGuid().ToString();
						break;

					case TraceColumnDataType.DateTime:
						sampleText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
						break;

					default:

						// string or bytes.  just setting for a width of 30 char at current font.
						sampleText = "012345678901234567890123456789";
						break;
				}
			}

			Font font = this.traceDisplayGrid.Font;
			using (Graphics graphics = this.CreateGraphics())
			{
				int width = Convert.ToInt32(graphics.MeasureString(sampleText, font).Width);
				points = width + padding; //Rounded
			}

			return points;
		}

		private void onCellEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
			{
				this.textDataTextBox.Text = this.traceDisplayGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
				this.textDataTextBox.Refresh();
			}
		}

		private void onClearButtonClick(object sender, EventArgs e)
		{
			lock (this._traceRecords)
			{
				this._traceRecords.Clear();
			}
		}

		private void onColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
		{
			// When one column is dragged and dropped, this gets called for each column that has an updated
			// DisplayIndex.  I'd much rather handle such a change only once and raise our ColumnOrderChanged event.
			// If I apply the entire new order when the first one changes, then subsequent calls will find
			// the remaining settings will already be correct allowing no further actions.
			if (this._initializingColumns) return;

			int columnId = TraceInfo.TraceColumnsInfoList[e.Column.DataPropertyName].Id;
			if (this._orderedColumns[e.Column.DisplayIndex] == columnId) return; // After the first column change, this will exit for the rest.

			SortedDictionary<int, int> columIdsByDisplayIndex = new SortedDictionary<int, int>();

			// The purpose is to keep the column order as it is sorted by the client by saving it in
			//the current trace definition .ColumnOrders property.
			for (int i = 0; i < this.traceDisplayGrid.Columns.Count; i++)
			{
				DataGridViewColumn gridColumn = this.traceDisplayGrid.Columns[i];
				int displayIndex = gridColumn.DisplayIndex;
				columnId = TraceInfo.TraceColumnsInfoList[gridColumn.DataPropertyName].Id;
				columIdsByDisplayIndex.Add(displayIndex, columnId);
			}

			this._orderedColumns = new int[this.traceDisplayGrid.Columns.Count];
			columIdsByDisplayIndex.Values.CopyTo(this._orderedColumns, 0);
			if (this.ColumnOrderChanged != null) this.ColumnOrderChanged(this, this._orderedColumns);
		}

		private void onFontButtonClick(object sender, EventArgs e)
		{
			FontDialog dialog = new FontDialog();

			dialog.FontMustExist = true;
			dialog.ShowEffects = true;
			dialog.Font = this.traceDisplayGrid.Font;

			if (dialog.ShowDialog(this) != DialogResult.Cancel)
			{
				this.traceDisplayGrid.Font = dialog.Font;
			}
		}

		private void onFreezeColumnOptionChange(object sender, EventArgs e)
		{
			DataGridViewColumn column = this.traceDisplayGrid.Columns.GetFirstColumn(DataGridViewElementStates.Visible);
			if (column != null) column.Frozen = this.freezeColumnOption.Checked;
		}

		private void onGroupColumnsOptionChanged(object sender, EventArgs e)
		{
			this.traceDisplayGrid.GroupSortedColumnValues = this.groupColumnsOption.Checked;
		}

		private void onSaveButtonClick(object sender, EventArgs e)
		{
			SaveFileDialog fileDialog = new SaveFileDialog();
			fileDialog.CheckPathExists = true;
			fileDialog.Filter = "comma separated value file (*.csv)|*.csv";
			fileDialog.DefaultExt = "csv";
			fileDialog.OverwritePrompt = true;

			if (fileDialog.ShowDialog(this.ParentForm) != DialogResult.Cancel) saveRecordsToFile(fileDialog.FileName);
		}

		private void saveRecordsToFile(string filePath)
		{
			using (FileStream stream = File.Open(filePath, FileMode.Create, FileAccess.Write))
			{
				foreach (TraceRecord record in this._traceRecords)
				{
					string line = string.Empty;
					foreach (int columnId in this._orderedColumns)
					{
						string columnValue = record[columnId];
						if ((TraceInfo.TraceColumnsInfoList[columnId].DataType == TraceColumnDataType.String) ||
							(columnValue.Contains(",")))
						{
							columnValue = "\"" + columnValue + "\"";
						}

						if (line.Length > 0) columnValue = "," + columnValue;
						line = line + columnValue;
					}
					line += Environment.NewLine;
					stream.Write(ASCIIEncoding.ASCII.GetBytes(line), 0, line.Length);
				}
			}
		}

		private void setControlStatesForTraceStatus()
		{
			this.displayOptionsPanel.Enabled = (this.Status != TraceStatus.Started);
			for (int i = 0; i < this.traceDisplayGrid.Columns.Count; i++)
			{
				DataGridViewColumn column = this.traceDisplayGrid.Columns[i];
				column.SortMode = (this.Status == TraceStatus.Started) ?
					DataGridViewColumnSortMode.NotSortable : DataGridViewColumnSortMode.Automatic;
			}
		}
	}

}