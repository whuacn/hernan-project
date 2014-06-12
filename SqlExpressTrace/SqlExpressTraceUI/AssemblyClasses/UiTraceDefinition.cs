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
using System.Text;
using SqlExpressTraceLib;

namespace SqlTraceExpressUI
{
	public class UiTraceDefinition : TraceDefinition
	{
		public readonly static string DefaultTraceDefinitionName = "<< Default >>";
		public readonly static string NewTraceDefinitionName = "New Trace Definition";

		private int[] _orderedColumns = new int[0];

		public UiTraceDefinition()
			: base()
		{
			this.OutputFile = string.Empty;
			this.RecordLimit = 0;
		}

		public UiTraceDefinition(string definitionName)
			: this()
		{
			base.Name = definitionName;
		}

		public bool IsDefault
		{
			get { return (this.Name == DefaultTraceDefinitionName); }
		}

		public int[] OrderedColumns
		{
			get
			{
				if (this._orderedColumns.Length == 0) UpdateOrderedColumns();
				return this._orderedColumns;
			}

			set { this._orderedColumns = value; }
		}

		public string OutputFile { get; set; }

		public int RecordLimit { get; set; }

		public static UiTraceDefinition GetDefaultTraceDefinition()
		{
			UiTraceDefinition defaultDefinition = new UiTraceDefinition();
			defaultDefinition.Name = DefaultTraceDefinitionName;
			defaultDefinition.Server = "localhost\\sqlexpress";

			List<int> columns = new List<int>();
			columns.Add(TraceInfo.TraceColumnsInfoList["EventClass"].Id);
			columns.Add(TraceInfo.TraceColumnsInfoList["LoginName"].Id);
			columns.Add(TraceInfo.TraceColumnsInfoList["TextData"].Id);
			columns.Add(TraceInfo.TraceColumnsInfoList["StartTime"].Id);
			columns.Add(TraceInfo.TraceColumnsInfoList["EndTime"].Id);
			columns.Add(TraceInfo.TraceColumnsInfoList["Duration"].Id);

			TraceEvent traceEvent = new TraceEvent(TraceInfo.TraceEventsInfoList["RPC:Completed"]);
			traceEvent.Columns = columns;
			defaultDefinition.TraceEvents.Add(traceEvent);

			defaultDefinition.RecordLimit = 5000;
			defaultDefinition.DurationHours = 1;
			defaultDefinition.DurationMinutes = 0;

			defaultDefinition.TraceFilters.Add(new TraceFilter(TraceInfo.TraceColumnsInfoList["TextData"].Id,
				LogicalOperators.And, ComparisonOperators.Not_Equal, "exec sp_reset_connection"));
			return defaultDefinition;
		}

		public static UiTraceDefinition GetNewTraceDefinitionWithDefaults()
		{
			UiTraceDefinition newDefinition = GetDefaultTraceDefinition();
			newDefinition.Name = NewTraceDefinitionName;
			return newDefinition;
		}

		public void UpdateOrderedColumns()
		{
			// Don't clear.  This effectively appends newly selected columns to the end, and removes de-selected.
			// Will leave previous columns as-is as they may have been sorted.
			// Sorting takes place only when the client drags and drops columns in the display grid.
			// if this was a trace definition that was just created, this ends up adding all the columns in the order they are found.
			// if this trace definition had  been saved before, then a subsequent edit on the definition may call this to pick
			// up new columns, and again the client may re-sort

			HashSet<int> allSelectedEventIds = new HashSet<int>();
			HashSet<int> currentColumns = new HashSet<int>(this._orderedColumns);

			foreach (TraceEvent tEvent in this.TraceEvents)
			{
				foreach (int columnId in tEvent.Columns)
				{
					if (!allSelectedEventIds.Contains(columnId)) allSelectedEventIds.Add(columnId);
					if (!currentColumns.Contains(columnId)) currentColumns.Add(columnId); // add new columns to the end.
				}
			}
			currentColumns.IntersectWith(allSelectedEventIds);  //remove de-selected columns.

			this._orderedColumns = new int[currentColumns.Count];
			currentColumns.CopyTo(this._orderedColumns, 0);

		}
	}
}