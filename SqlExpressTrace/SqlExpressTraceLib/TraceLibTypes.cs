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
using System.IO;
using System.Reflection;
using System.Text;

namespace SqlExpressTraceLib
{
	public enum ComparisonOperators
	{
		Equal = 0,
		Not_Equal = 1,
		Greater_Than = 2,
		Less_Than = 3,
		Greater_Than_Or_Equal = 4,
		Less_Than_Or_Equal = 5,
		Like = 6,
		Not_Like = 7
	}

	public enum LogicalOperators
	{
		And = 0,
		Or = 1
	}

	public enum TraceColumnDataType
	{
		Int32 = 0,
		Int64 = 1,
		String = 2,
		Byte = 3,
		DateTime = 4,
		Guid = 5
	}

	public enum TraceStatus
	{
		Stopped = 0,
		Started = 1,
		Closed = 2
	}

	public class TraceInfo
	{
		public static readonly InfoItemCollection<int, ColumnInfoItem> TraceColumnsInfoList = loadTraceColumnInfo();
		public static readonly InfoItemCollection<int, EventInfoItem> TraceEventsInfoList = loadTraceEventInfo();

		private static InfoItemCollection<int, ColumnInfoItem> loadTraceColumnInfo()
		{
			/************************************************************************************************************************
			* integer columns = 3, 5, 9, 12, 18, 20, 21, 22, 23, 24, 25, 27, 28, 29, 30, 31, 32, 33, 44, 49, 55, 57, 58, 60, 61, 62
			* string columns = 1, 6, 7, 8, 10, 11, 26, 34, 35, 36, 37, 38, 39, 40, 42, 45, 46, 47, 59, 64
			* long columns = 4, 13, 16, 17, 19, 48, 50, 51, 52, 53, 56
			* byte columns = 2, 41, 43, 54, 63, 65
			* DateTime columns = 14, 15
			*************************************************************************************************************************/

			InfoItemCollection<int, ColumnInfoItem> infoList = new InfoItemCollection<int, ColumnInfoItem>();
			StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("SqlExpressTraceLib.ResourceFiles.ColumnInfo.tsv"));

			string line = reader.ReadLine();
			while (line != null)
			{
				string[] vals = line.Split('\t');
				if (vals.Length == 4)
				{
					int id = int.Parse(vals[0].Trim());
					string name = vals[1].Trim();
					TraceColumnDataType dataType = (TraceColumnDataType)Enum.Parse(typeof(TraceColumnDataType), vals[2].Trim());
					string description = vals[3].Trim();
					int dataTypeInt = (int)dataType;
					infoList.Add(id, new ColumnInfoItem(id, name, description, dataType));
				}
				line = reader.ReadLine();
			}

			return infoList;

		}

		private static InfoItemCollection<int, EventInfoItem> loadTraceEventInfo()
		{
			string[] names = Assembly.GetExecutingAssembly().GetManifestResourceNames();
			InfoItemCollection<int, EventInfoItem> infoList = new InfoItemCollection<int, EventInfoItem>();
			StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("SqlExpressTraceLib.ResourceFiles.EventInfo.tsv"));

			string line = reader.ReadLine();
			while (line != null)
			{
				string[] vals = line.Split('\t');
				if (vals.Length == 3)
				{
					int id = int.Parse(vals[0].Trim());
					string name = vals[1].Trim();
					string description = vals[2].Trim();
					infoList.Add(id, new EventInfoItem(id, name, description));
				}
				line = reader.ReadLine();
			}

			return infoList;
		}
	}

}