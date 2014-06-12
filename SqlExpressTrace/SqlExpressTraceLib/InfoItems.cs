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
namespace SqlExpressTraceLib
{

	/// <summary>
	/// Base InfoItem class that is used for looking up basic reference info: IDs names and descriptions.
	/// </summary>
	public class InfoItem
	{
		public InfoItem(int id, string name, string description)
		{
			Id = id;
			Name = name;
			Description = description;
		}

		public string Description { get; protected set; }
		public int Id { get; protected set; }
		public string Name { get; protected set; }

		public override string ToString() { return Name; }
	}

	/// <summary>
	/// EventInfoItem contains an Id, Name and Description pertaining to a Sql Server Event
	/// </summary>
	public class EventInfoItem : InfoItem
	{
		public EventInfoItem(int eventId, string name, string description)
			: base(eventId, name, description) { }
	}

	/// <summary>
	/// ColumnInfoItem contains an Id, Name and Description pertaining to a Sql Server Event Column
	/// </summary>
	public class ColumnInfoItem : InfoItem
	{
		public ColumnInfoItem(int columnId, string name, string description, TraceColumnDataType dataType)
			: base(columnId, name, description)
		{
			DataType = dataType;
		}

		public TraceColumnDataType DataType { get; private set; }
	}



}
