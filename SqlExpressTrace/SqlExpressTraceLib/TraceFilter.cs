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

namespace SqlExpressTraceLib
{
	public class TraceFilter
	{

		public TraceFilter() { }

		public TraceFilter(int columnId, LogicalOperators logicalOperator,
			ComparisonOperators comparisonOperator, string value)
		{
			ColumnId = columnId;
			LogicalOperator = logicalOperator;
			ComparisonOperator = comparisonOperator;
			Value = value;
		}

		public int ColumnId { get; set; }

		public ComparisonOperators ComparisonOperator { get; set; }

		public LogicalOperators LogicalOperator { get; set; }

		public string Value { get; set; }

		public override string ToString()
		{
			if (TraceInfo.TraceColumnsInfoList.ContainsKey(this.ColumnId))
			{
				return LogicalOperator.ToString() + " " +
					TraceInfo.TraceColumnsInfoList[this.ColumnId].Name + " " +
					ComparisonOperator.ToString() + " " +
					Value;
			}
			else return string.Empty;
		}
	}
}