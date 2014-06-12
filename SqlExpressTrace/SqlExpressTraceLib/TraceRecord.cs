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
using System.Text;

namespace SqlExpressTraceLib
{
	public class TraceRecord
	{
		private Dictionary<int, string> _recordColumns = new Dictionary<int, string>();

		public TraceRecord()
		{
			initializeColumns();
		}

		public TraceRecord(int eventClassId)
			: this()
		{
			this._recordColumns[TraceInfo.TraceColumnsInfoList["EventClass"].Id] =
				TraceInfo.TraceEventsInfoList[eventClassId].Name;
		}

		// TODO: consider contructing properties for this record using the InfoItems, possibly using only the columns
		//  used for a particular trace definition. i.e. CodeDom.
		// Since TraceColumnsInfo list is also a dictionary with the column names indexed, these will come back quite fast with the ID
		// that is the key to our _recordcolumns dictionary.  But it is also the one place where there are string literals that
		// would require a recompile if columns were to change.

		public string ApplicationName { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["ApplicationName"].Id]; } }

		public string BigintData1 { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["BigintData1"].Id]; } }

		public string BigintData2 { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["BigintData2"].Id]; } }

		public string BinaryData { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["BinaryData"].Id]; } }

		public string ClientProcessID { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["ClientProcessID"].Id]; } }

		public string ColumnPermissions { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["ColumnPermissions"].Id]; } }

		public string CPU { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["CPU"].Id]; } }

		public string DatabaseID { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["DatabaseID"].Id]; } }

		public string DatabaseName { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["DatabaseName"].Id]; } }

		public string DBUserName { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["DBUserName"].Id]; } }

		public string Duration { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["Duration"].Id]; } }

		public string EndTime { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["EndTime"].Id]; } }

		public string Error { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["Error"].Id]; } }

		public string EventClass { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["EventClass"].Id]; } }

		public string EventSequence { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["EventSequence"].Id]; } }

		public string EventSubClass { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["EventSubClass"].Id]; } }

		public string FileName { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["FileName"].Id]; } }

		public string GUID { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["GUID"].Id]; } }

		public string Handle { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["Handle"].Id]; } }

		public string HostName { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["HostName"].Id]; } }

		public string IndexID { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["IndexID"].Id]; } }

		public string IntegerData { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["IntegerData"].Id]; } }

		public string IntegerData2 { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["IntegerData2"].Id]; } }

		public string IsSystem { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["IsSystem"].Id]; } }

		public string LineNumber { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["LineNumber"].Id]; } }

		public string LinkedServerName { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["LinkedServerName"].Id]; } }

		public string LoginName { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["LoginName"].Id]; } }

		public string LoginSid { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["LoginSid"].Id]; } }

		public string MethodName { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["MethodName"].Id]; } }

		public string Mode { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["Mode"].Id]; } }

		public string NestLevel { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["NestLevel"].Id]; } }

		public string NTDomainName { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["NTDomainName"].Id]; } }

		public string NTUserName { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["NTUserName"].Id]; } }

		public string ObjectID { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["ObjectID"].Id]; } }

		public string ObjectID2 { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["ObjectID2"].Id]; } }

		public string ObjectName { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["ObjectName"].Id]; } }

		public string ObjectType { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["ObjectType"].Id]; } }

		public string Offset { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["Offset"].Id]; } }

		public string OwnerID { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["OwnerID"].Id]; } }

		public string OwnerName { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["OwnerName"].Id]; } }

		public string ParentName { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["ParentName"].Id]; } }

		public string Permissions { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["Permissions"].Id]; } }

		public string ProviderName { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["ProviderName"].Id]; } }

		public string Reads { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["Reads"].Id]; } }

		public string RequestID { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["RequestID"].Id]; } }

		public string RoleName { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["RoleName"].Id]; } }

		public string RowCounts { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["RowCounts"].Id]; } }

		public string ServerName { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["ServerName"].Id]; } }

		public string SessionLoginName { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["SessionLoginName"].Id]; } }

		public string Severity { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["Severity"].Id]; } }

		public string SourceDatabaseID { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["SourceDatabaseID"].Id]; } }

		public string SPID { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["SPID"].Id]; } }

		public string SqlHandle { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["SqlHandle"].Id]; } }

		public string StartTime { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["StartTime"].Id]; } }

		public string State { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["State"].Id]; } }

		public string Success { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["Success"].Id]; } }

		public string TargetLoginName { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["TargetLoginName"].Id]; } }

		public string TargetLoginSid { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["TargetLoginSid"].Id]; } }

		public string TargetUserName { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["TargetUserName"].Id]; } }

		public string TextData { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["TextData"].Id]; } }

		public string TransactionID { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["TransactionID"].Id]; } }

		public string Type { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["Type"].Id]; } }

		public string Writes { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["Writes"].Id]; } }

		public string XactSequence { get { return this._recordColumns[TraceInfo.TraceColumnsInfoList["XactSequence"].Id]; } }

		public string this[int columnId]
		{
			get { return this._recordColumns[columnId]; }
		}

		public void SetColumnValueFromBytes(int columnId, byte[] bytes)
		{
			string stringVal = string.Empty;

			switch (TraceInfo.TraceColumnsInfoList[columnId].DataType)
			{
				case TraceColumnDataType.Int32:
					stringVal = BitConverter.ToInt32(bytes, 0).ToString();
					break;

				case TraceColumnDataType.String:
					stringVal = Encoding.Unicode.GetString(bytes);
					break;

				case TraceColumnDataType.Int64:
					long longVal = BitConverter.ToInt64(bytes, 0);

					if (columnId == TraceInfo.TraceColumnsInfoList["Duration"].Id) //Duration in microseconds.
					{
						double dVal = (double)longVal / 1000000;
						stringVal = dVal.ToString("0.0000");
					}
					else stringVal = longVal.ToString();
					break;

				case TraceColumnDataType.Byte:
					stringVal = BitConverter.ToString(bytes, 0);
					break;

				case TraceColumnDataType.DateTime:
					int year = BitConverter.ToInt16(bytes, 0);
					int month = BitConverter.ToInt16(bytes, 2);
					int day = BitConverter.ToInt16(bytes, 6);
					int hour = BitConverter.ToInt16(bytes, 8);
					int minute = BitConverter.ToInt16(bytes, 10);
					int second = BitConverter.ToInt16(bytes, 12);
					int millisecond = BitConverter.ToInt16(bytes, 14);

					stringVal = string.Format("{0}-{1}-{2} {3}:{4}:{5}.{6}", year.ToString("0000"),
					month.ToString("00"), day.ToString("00"), hour.ToString("00"), minute.ToString("00"),
					second.ToString("00"), millisecond.ToString("000"));
					break;

				case TraceColumnDataType.Guid:

					// Documented to return only the 16 bytes, already perfect fit for guid constructor.
					Guid guid = new Guid(bytes);
					stringVal = guid.ToString();
					break;
			}

			this._recordColumns[columnId] = stringVal.Trim();
		}

		private void initializeColumns()
		{
			foreach (ColumnInfoItem columnInfoItem in TraceInfo.TraceColumnsInfoList.Values)
			{
				this._recordColumns.Add(columnInfoItem.Id, string.Empty);
			}
		}
	}

}