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
using System.Collections.Generic;

namespace SqlExpressTraceLib
{

	/// <summary>
	/// This collection provides a simple serializeable list interface allowing storage of user filter settings
	/// in trace definitions.  It also provides extra members that allow referencing multiple filters by column
	/// as would be typically offered in user interfaces when configuring traces.
	/// </summary>
	///
	public class TraceFilterList : List<TraceFilter>
	{
		public TraceFilterList() { }

		public bool ContainsFiltersForColumn(int columnId)
		{
			bool contains = false;
			foreach (TraceFilter filter in this)
			{
				if (filter.ColumnId == columnId)
				{
					contains = true;
					break;
				}
			}
			return contains;
		}

		public TraceFilterList Copy()
		{
			TraceFilterList list = new TraceFilterList();
			foreach (TraceFilter filter in this)
			{
				TraceFilter newfilter = new TraceFilter(
					filter.ColumnId,
					filter.LogicalOperator,
					filter.ComparisonOperator,
					filter.Value);

				list.Add(newfilter);
			}
			return list;
		}

		public List<TraceFilter> FiltersForColumn(int columnId)
		{
			List<TraceFilter> filters = new List<TraceFilter>();

			foreach (TraceFilter filter in this)
			{
				if (filter.ColumnId == columnId) filters.Add(filter);
			}

			return filters;
		}

		public void RemoveFiltersForColumn(int columnId)
		{
			for (int i = this.Count - 1; i >= 0; i--)
			{
				if (this[i].ColumnId == columnId) this.RemoveAt(i);
			}
		}

		public void SortFiltersByColumnId()
		{
			base.Sort(CompareFilters);
		}

		internal static int CompareFilters(TraceFilter x, TraceFilter y)
		{
			if (x.ColumnId < y.ColumnId) return -1;
			else if (x.ColumnId > y.ColumnId) return 1;
			else return 0;
		}

	}
}