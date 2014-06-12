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

namespace SqlExpressTraceLib
{
	public class CreateTraceResponseException : SETraceException
	{
		public CreateTraceResponseException(int responseCode)
			: base("An error response was recieved from sp_trace_create: " + ResponseCode.Values[responseCode]) { }
	}

	public class CurrentlyRunningTraceException : SETraceException
	{
		public CurrentlyRunningTraceException()
			: base("The TraceController cannot start a trace because it is already monitoring another trace.")
		{ }
	}

	public class InvalidTraceIdException : SETraceException
	{
		public InvalidTraceIdException()
			: base("the traceId returned from sp_trace_create was invalid (0).") { }
	}

	public class NotCurrentlyMonitoringException : SETraceException
	{
		public NotCurrentlyMonitoringException()
			: base("The TraceMonitor cannot stop, pause or resume a trace because there isn't one open.")
		{ }
	}

	public class SetEventResponseException : SETraceException
	{
		public SetEventResponseException(int responseCode)
			: base("An error response was recieved from sp_trace_setevent: " + ResponseCode.Values[responseCode]) { }
	}

	public class SetFilterResponseException : SETraceException
	{
		public SetFilterResponseException(int responseCode)
			: base("An error response was recieved from sp_trace_setfilter: " + ResponseCode.Values[responseCode]) { }
	}

	/// <summary>
	/// TraceException.  Just here to provide common inheritance to distinguish our own
	/// and give a chance to handle and avoid treating as fatal where possible, for the client's part anyway.
	/// </summary>
	///
	public abstract class SETraceException : Exception
	{
		public SETraceException(string message) : base(message) { }

		public SETraceException(string message, Exception ex) : base(message, ex) { }
	}

	public class SetStatusResponseException : SETraceException
	{
		public SetStatusResponseException(int responseCode)
			: base("An error response was recieved from sp_trace_setstatus: " + ResponseCode.Values[responseCode]) { }
	}

	public class TraceFailedToCloseAtServerException : SETraceException
	{
		public TraceFailedToCloseAtServerException()
			: base("The trace failed to close on the server in a reasonable amount of time.") { }
	}

	public class TraceFailedToStartAtServerException : SETraceException
	{
		public TraceFailedToStartAtServerException()
			: base("The trace failed to start on the server in a reasonable amount of time.") { }
	}

	public class TraceFailedToStopAtServerException : SETraceException
	{
		public TraceFailedToStopAtServerException()
			: base("The trace failed to stop on the server in a reasonable amount of time.") { }
	}

	public class TraceMonitorException : SETraceException
	{
		public TraceMonitorException(string monitor, Exception ex)
			: base("An exception was received in one of the TraceController monitor processes.  The " + monitor + " process has been stopped and the current trace will be closed.") { }
	}

	public class TraceMonitorFailedStartException : SETraceException
	{
		public TraceMonitorFailedStartException()
			: base("The trace monitor failed to start properly in a reasonable amount of time.") { }
	}

	internal static class ResponseCode
	{
		static ResponseCode()
		{
			Values = new Dictionary<int, string>();
			StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("SqlExpressTraceLib.ResourceFiles.ReturnCodes.tsv"));

			string line = reader.ReadLine();
			while (line != null)
			{
				string[] valueArray = line.Split('\t');
				if (valueArray.Length == 2)
				{
					int id = int.Parse(valueArray[0].Trim());
					string description = valueArray[1].Trim();
					Values.Add(id, description);
				}
				line = reader.ReadLine();
			}

		}

		public static Dictionary<int, string> Values { get; private set; }
	}
}