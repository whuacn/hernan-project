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
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace SqlExpressTraceLib
{
	public delegate void TraceEventRecordedHandler(object sender, TraceRecord traceRecord);
	public delegate void TraceMonitorExceptionHandler(object sender, TraceMonitorException exception);
	public delegate void TraceStatusChangedEventHandler(object sender, TraceStatus status);

	/// <summary>
	/// Used for creating, maintaining and monitoring traces on Sql Servers.
	/// </summary>
	public class TraceController : IDisposable
	{
		/// <summary>
		/// TraceController creates, maintains, monitors and reports event traces on SqlServers.
		/// </summary>
		public TraceController()
		{
			this._traceStatus = TraceStatus.Closed;

			string processName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
			this._processName = processName + "." + DateTime.Now.Ticks.ToString().Substring(3);
		}

		public event TraceMonitorExceptionHandler TraceMonitorExceptionEvent;
		public event TraceEventRecordedHandler TraceRecordedEvent;
		public event TraceStatusChangedEventHandler TraceStatusChangedEvent;

		#region private members

		private volatile TraceStatus _traceStatus = TraceStatus.Closed;
		private volatile bool _continueMonitoringEvents = false;
		private volatile bool _continueMonitoringStatus = false;
		private bool _disposed = false;
		private Thread _monitorTraceEventThread = null;
		private Thread _monitorTraceStatusThread = null;
		private string _processName;
		private TraceDefinition _traceDefinition = null;
		private int _traceId = 0;

		#endregion private members

		public TraceStatus Status
		{
			get { return this._traceStatus; }

			private set
			{
				bool changed = (this._traceStatus != value);
				this._traceStatus = value;

				if (this._traceStatus == TraceStatus.Closed)
				{
					this._continueMonitoringStatus = false;
					this._continueMonitoringEvents = false;
				}

				if (changed) raiseStatusChangedEvent(this._traceStatus);
			}
		}

		private string connectionString
		{
			get
			{
				if (this._traceDefinition.IntegratedConnection)
				{
					return string.Format("Server={0}; Database=master; Trusted_Connection=True; Application Name={1}",
						this._traceDefinition.Server, _processName);
				}
				else
				{
					return string.Format("Server={0}; Database=master; User Id={1}; Password={2}; Application Name={3}",
						this._traceDefinition.Server, this._traceDefinition.UserId, this._traceDefinition.Password, _processName);
				}

			}
		}

		private TraceStatus currentServerTraceStatus
		{
			get
			{
				TraceStatus returnStatus = TraceStatus.Closed;
				if (this._traceId == 0) return returnStatus;

				// This sql function returns one row for each trace property, identified by the number assigned to
				// the property field that is returned with each row.  '5' is the status property.
				string commandText = string.Format("select value from fn_trace_getinfo ({0}) where property = 5",
					this._traceId);

				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					using (SqlCommand command = new SqlCommand(commandText, connection))
					{
						object responseObj = command.ExecuteScalar();
						if ((responseObj != null) && (responseObj != DBNull.Value))
						{
							int response = (int)responseObj;
							returnStatus = (TraceStatus)response;
						}
					}
				}
				return returnStatus;
			}
		}

		private TraceFilter thisProcessFilter
		{
			get
			{
				int appNameColumn = TraceInfo.TraceColumnsInfoList["ApplicationName"].Id;
				TraceFilter filter = new TraceFilter(appNameColumn, LogicalOperators.And,
					ComparisonOperators.Not_Equal, this._processName);

				return filter;
			}
		}

		private bool traceEventMonitorIsRunning
		{
			get { return ((this._monitorTraceEventThread != null) && (this._monitorTraceEventThread.IsAlive)); }
		}

		private bool traceStatusMonitorIsRunning
		{
			get { return ((this._monitorTraceStatusThread != null) && (this._monitorTraceStatusThread.IsAlive)); }
		}

		public void CloseTrace()
		{
			if (Status == TraceStatus.Closed) throw new NotCurrentlyMonitoringException();
			else
			{
				closeTraceAtServer();
				int counter = 0;
				while (Status != TraceStatus.Closed)
				{
					if (counter == 10) throw new TraceFailedToCloseAtServerException();
					Thread.Sleep(500);
					counter++;
				}
				stopBothMonitorThreads();
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void PauseTrace()
		{
			if (Status == TraceStatus.Closed) throw new NotCurrentlyMonitoringException();
			else if (Status == TraceStatus.Started)
			{
				int response = setTraceStatusOnServer(TraceStatus.Stopped);
				if (response > 0) throw new SetStatusResponseException(response);

				int counter = 0;
				while (Status != TraceStatus.Stopped)
				{
					if (counter == 10) throw new TraceFailedToStopAtServerException();
					Thread.Sleep(500);
					counter++;
				}
			}
		}

		public void ResumeTrace()
		{
			if (Status == TraceStatus.Closed) throw new NotCurrentlyMonitoringException();
			if (Status == TraceStatus.Stopped)
			{
				try
				{
					int response = setTraceStatusOnServer(TraceStatus.Started);
					if (response > 0) throw new SetStatusResponseException(response);

					int counter = 0;
					while (Status != TraceStatus.Started)
					{
						if (counter == 10) throw new TraceFailedToStartAtServerException();
						Thread.Sleep(500);
						counter++;
					}

					startTraceEventMonitor();
				}
				catch
				{
					try { setTraceStatusOnServer(TraceStatus.Closed); }
					catch { }
					this.Status = TraceStatus.Closed;
					throw;
				}
			}
		}

		public void StartTrace(TraceDefinition traceDefinition)
		{
			if (Status != TraceStatus.Closed) throw new CurrentlyRunningTraceException();

			this._traceDefinition = traceDefinition;

			int response = createTrace();
			if (response != 0) throw new CreateTraceResponseException(response);
			if (this._traceId == 0) throw new InvalidTraceIdException();

			try
			{
				response = applyEvents();
				if (response > 0) throw new SetEventResponseException(response);

				response = applyFilters();
				if (response > 0) throw new SetFilterResponseException(response);

				response = setTraceStatusOnServer(TraceStatus.Started);
				if (response > 0) throw new SetStatusResponseException(response);
			}
			catch
			{
				try { setTraceStatusOnServer(TraceStatus.Closed); }
				catch { }
				throw;
			}

			try
			{
				startTraceStatusMonitor();
				int counter = 0;
				while (Status != TraceStatus.Started)
				{
					if (counter == 10) throw new TraceFailedToStartAtServerException();
					Thread.Sleep(500);
					counter++;
				}
				startTraceEventMonitor();
			}
			catch
			{
				try { setTraceStatusOnServer(TraceStatus.Closed); }
				catch { }
				this.Status = TraceStatus.Closed;
				throw;
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!this._disposed)
			{
				if (disposing)
				{
					try
					{
						closeTraceAtServer();
					}
					catch { };
				}

				this._disposed = true;
				Debug.WriteLine(DateTime.Now.ToShortTimeString() + "TraceMonitor disposed.");
			}
		}

		private int applyEvents()
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				foreach (TraceEvent sqlEvent in this._traceDefinition.TraceEvents)
				{
					foreach (int columnId in sqlEvent.Columns)
					{
						string commandText = string.Format("declare	@response int; " +
							"exec @response = sp_trace_setevent " +
							"@traceid = {0}, " +
							"@eventid = {1}," +
							"@columnid = {2}, " +
							"@on = {3}; " +
							"select @response;",
							this._traceId,
							sqlEvent.Id,
							columnId,
							"1");

						using (SqlCommand command = new SqlCommand(commandText, connection))
						{
							command.CommandType = CommandType.Text;
							int response = (int)command.ExecuteScalar();
							if (response != 0) return response;
						}
					}
				}
			}

			return 0;
		}

		private int applyFilters()
		{
			// Add std filter to the client's specified filters to keep this process from being traced.
			List<TraceFilter> filters = new List<TraceFilter>(this._traceDefinition.TraceFilters);
			filters.Add(thisProcessFilter);

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();

				foreach (TraceFilter filter in filters)
				{
					string val = filter.Value;
					if (TraceInfo.TraceColumnsInfoList[filter.ColumnId].DataType == TraceColumnDataType.String)
					{
						if (!val.StartsWith("N'"))
						{  // Forcing it to be nvarchar.
							if (!val.StartsWith("'")) val = "N'" + val;
							else val = "N" + val;
						}

						if (!val.EndsWith("'")) val = val + "'";
					}

					string commandText = string.Format("declare	@response int; " +
						"exec @response = sp_trace_setfilter " +
						"@traceid = {0}, " +
						"@columnid = {1}, " +
						"@logical_operator = {2}, " +
						"@comparison_operator = {3}, " +
						"@value = {4}; " +
						"select @response;",
						this._traceId,
						filter.ColumnId,
						(int)filter.LogicalOperator,
						(int)filter.ComparisonOperator,
						val);

					using (SqlCommand command = new SqlCommand(commandText, connection))
					{
						command.CommandType = CommandType.Text;
						int response = (int)command.ExecuteScalar();
						if (response != 0) return response;
					}
				}
			}

			return 0;
		}

		private void closeTraceAtServer()
		{
			try { setTraceStatusOnServer(TraceStatus.Stopped); }
			catch { }

			try { setTraceStatusOnServer(TraceStatus.Closed); }
			catch { }

			this._traceId = 0;
		}

		private int createTrace()
		{
			int response = 0;
			int traceId = 0;

			DateTime stopTime = DateTime.Now.AddMinutes((double)this._traceDefinition.DurationMinutes);
			stopTime = stopTime.AddHours((double)this._traceDefinition.DurationHours);

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();

				using (SqlCommand command = new SqlCommand("sp_trace_create", connection))
				{
					command.CommandType = CommandType.StoredProcedure;

					command.Parameters.Add("@traceid", SqlDbType.Int).Direction = ParameterDirection.Output;
					command.Parameters.Add("@options", SqlDbType.Int).Value = 1;
					command.Parameters.Add("@trace_file", SqlDbType.NVarChar, 1).Value = DBNull.Value;
					command.Parameters.Add("@maxfilesize", SqlDbType.BigInt).Value = DBNull.Value;
					command.Parameters.Add("@stoptime", SqlDbType.DateTime).Value = stopTime;
					command.Parameters.Add("@filecount", SqlDbType.Int).Value = DBNull.Value;
					command.Parameters.Add("@result", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

					command.ExecuteNonQuery();
					response = (int)command.Parameters["@result"].Value;
					traceId = (int)command.Parameters["@traceid"].Value;
				}
			}

			if (response == 0)
			{
				this._traceId = traceId;
			}
			return response;
		}

		private DbDataReader getMonitorDataReader(SqlConnection connection)
		{
			SqlCommand command = new SqlCommand("sp_trace_getdata", connection);
			command.CommandType = CommandType.StoredProcedure;
			command.CommandTimeout = 0;

			command.Parameters.Add("@traceid", SqlDbType.Int).Value = this._traceId;
			command.Parameters.Add("@records", SqlDbType.Int).Value = 0;

			return command.ExecuteReader(CommandBehavior.SingleResult);
		}

		private TraceRecord getSqlTraceRecordFromReader(DbDataReader dataReader)
		{
			TraceRecord record = null;

			int eventId = BitConverter.ToUInt16((byte[])dataReader[2], 0);
			if ((eventId >= 0) && (eventId < 255))
			{
				record = new TraceRecord(eventId);
				while (dataReader.Read())
				{
					int columnId = (int)dataReader[0];
					if (columnId > 65) { break; }
					record.SetColumnValueFromBytes(columnId, (byte[])dataReader[2]);
				}

			}

			return record;
		}

		private void monitorServerTraceEvents()
		{
			int StartReceiveEventVal = 65526;
			SqlConnection connection = null;
			DbDataReader dataReader = null;

			try
			{
				connection = new SqlConnection(this.connectionString);
				connection.Open();

				dataReader = getMonitorDataReader(connection);
				Thread.Sleep(500);

				while (this._continueMonitoringEvents && dataReader.Read())
				{
					if ((int)dataReader[0] == StartReceiveEventVal)
					{
						TraceRecord record = getSqlTraceRecordFromReader(dataReader);
						if (record != null) this.raiseTraceRecordedEvent(record);
					}
				}
			}
			catch (ThreadAbortException) { Debug.WriteLine("trace event monitor thread aborted"); }
			catch (Exception ex) { raiseTraceMonitorExceptionEvent(new TraceMonitorException("Trace Event", ex)); }
			finally
			{
				Debug.WriteLine("TraceEventMonitor Exited");
				this._continueMonitoringEvents = false;

				if ((dataReader != null) && (!dataReader.IsClosed))
				{
					try { dataReader.Dispose(); }
					catch { }
				}

				if ((connection != null) && (connection.State != ConnectionState.Closed))
				{
					try { connection.Dispose(); }
					catch { }
				}
			}
		}

		private void monitorServerTraceStatus()
		{
			try
			{
				while (this._continueMonitoringStatus)
				{
					Thread.Sleep(500);
					this.Status = currentServerTraceStatus;
				}
			}
			catch (ThreadAbortException) { Debug.WriteLine("trace status monitor thread aborted"); }
			catch (Exception ex) { raiseTraceMonitorExceptionEvent(new TraceMonitorException("Trace Status", ex)); }
			finally
			{
				this._continueMonitoringStatus = false;
			}
		}

		private void raiseStatusChangedEvent(TraceStatus status)
		{
			raiseSynchronizedEvent(TraceStatusChangedEvent, new object[] { this, status });
		}

		private void raiseSynchronizedEvent(Delegate targetEvent, object[] args)
		{
			// If a UI application (System.Windows.Form) assigned a delegate to the event, then that delegate will
			// be in the event invocation list and it's target(method/handler) is a descendent of ISynchronizeInvoke.
			// We can call its inherited BeginInvoke method assuring it recieves the event on its own thread.
			if (targetEvent != null)
			{
				Delegate[] invocationList = targetEvent.GetInvocationList();

				foreach (Delegate targetDelegate in invocationList)
				{
					ISynchronizeInvoke syncComponent = targetDelegate.Target as ISynchronizeInvoke;
					if ((syncComponent != null) && (syncComponent.InvokeRequired))
					{
						syncComponent.BeginInvoke(targetDelegate, args);
					}
					else
					{
						targetDelegate.DynamicInvoke(args);
					}
				}
			}
		}

		private void raiseTraceMonitorExceptionEvent(TraceMonitorException ex)
		{
			raiseSynchronizedEvent(TraceMonitorExceptionEvent, new object[] { this, ex });
		}

		private void raiseTraceRecordedEvent(TraceRecord record)
		{
			raiseSynchronizedEvent(TraceRecordedEvent, new object[] { this, record });
		}

		private int setTraceStatusOnServer(TraceStatus status)
		{
			int response = 0;
			string commandText = string.Format("declare	@response int; " +
				"exec @response = sp_trace_setstatus " +
				"@traceid = {0}, " +
				"@status = {1}; " +
				"select @response;",
				this._traceId,
				(int)status);

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				using (SqlCommand command = new SqlCommand(commandText, connection))
				{
					response = (int)command.ExecuteScalar();
				}
			}

			return response;
		}

		private void startTraceEventMonitor()
		{
			if (traceEventMonitorIsRunning) return;

			this._continueMonitoringEvents = true;
			this._monitorTraceEventThread = new Thread(new ThreadStart(monitorServerTraceEvents));
			this._monitorTraceEventThread.Name = "monitorTraceEventThread";
			this._monitorTraceEventThread.Start();
		}

		private void startTraceStatusMonitor()
		{
			if (traceStatusMonitorIsRunning) return;

			this._continueMonitoringStatus = true;
			this._monitorTraceStatusThread = new Thread(new ThreadStart(monitorServerTraceStatus));
			this._monitorTraceStatusThread.Name = "monitorTraceThread";
			this._monitorTraceStatusThread.Start();
		}

		private void stopBothMonitorThreads()
		{
			stopTraceEventMonitor();
			stopTraceStatusMonitor();
		}

		private void stopTraceEventMonitor()
		{
			if (!traceEventMonitorIsRunning) return;

			this._continueMonitoringEvents = false;

			this._monitorTraceEventThread.Join(1000);
			if (traceEventMonitorIsRunning) this._monitorTraceEventThread.Abort();

		}

		private void stopTraceStatusMonitor()
		{
			if (!traceStatusMonitorIsRunning) return;

			this._continueMonitoringStatus = false;
			if ((this._monitorTraceStatusThread != null) && (this._monitorTraceStatusThread.IsAlive))
			{
				this._monitorTraceStatusThread.Join(1000);
				if (traceStatusMonitorIsRunning) this._monitorTraceStatusThread.Abort();
			}
		}
	}

}