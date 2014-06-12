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
using System.Xml;
using System.Xml.Serialization;

namespace SqlExpressTraceLib
{
	public class TraceDefinition
	{
		private const int DefaultDuration = 5;

		public TraceDefinition()
		{
			this.TraceEvents = new TraceEventList();
			this.TraceFilters = new TraceFilterList();
			this.DurationMinutes = DefaultDuration;
			this.DurationHours = 0;
			this.IntegratedConnection = true;
		}

		public int DurationHours { get; set; }

		public int DurationMinutes { get; set; }

		public bool IntegratedConnection { get; set; }

		public string Name { get; set; }

		[XmlIgnore]
		public string Password { get; set; }

		public string Server { get; set; }

		public TraceEventList TraceEvents { get; set; }

		public TraceFilterList TraceFilters { get; set; }

		public string UserId { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}