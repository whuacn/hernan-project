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
	public class TraceEventList : List<TraceEvent>
	{
		public bool ContainsEvent(int eventID)
		{
			return (GetEvent(eventID) != null);
		}

		public TraceEventList Copy()
		{
			TraceEventList list = new TraceEventList();
			foreach (TraceEvent traceEvent in this)
			{
				TraceEvent newTraceEvent = new TraceEvent(traceEvent.Id);
				newTraceEvent.Columns.AddRange(traceEvent.Columns.ToArray());
				list.Add(newTraceEvent);
			}

			return list;
		}

		public TraceEvent GetEvent(int eventID)
		{
			foreach (TraceEvent traceEvent in this)
			{
				if (eventID == traceEvent.Id) return traceEvent;
			}
			return null;
		}

		public void RemoveEvent(int eventID)
		{
			for (int i = this.Count - 1; i >= 0; i--)
			{
				TraceEvent traceEvent = this[i];
				if (traceEvent.Id == eventID)
				{
					this.RemoveAt(i);
					break;
				}
			}
		}

		public void SortByEventId()
		{
			base.Sort(CompareEvents);
			foreach (TraceEvent traceEvent in this)
			{
				traceEvent.Columns.Sort();
			}
		}

		internal static int CompareEvents(TraceEvent x, TraceEvent y)
		{
			if (x.Id < y.Id) return -1;
			else if (x.Id > y.Id) return 1;
			else return 0;
		}

	}

}