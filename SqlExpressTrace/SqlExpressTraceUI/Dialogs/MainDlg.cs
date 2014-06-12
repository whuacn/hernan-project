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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SqlExpressTraceLib;

namespace SqlTraceExpressUI
{
	public partial class MainDlg : Form
	{
		#region private members

		private ClientSettings _clientSettings = ClientSettings.GetSavedSettings();
		private TraceStatus _currentStatus = TraceStatus.Closed;
		private TraceController _traceController = null;
		private BindingList<UiTraceDefinition> _traceDefinitionsBindingList = new BindingList<UiTraceDefinition>();

		#endregion private members

		public MainDlg()
		{
			InitializeComponent();
			setControlStatesForCurrentStatus(); // Status = 'Closed' at this stage;
			this.traceDefSelectCtrl.DataSource = this._traceDefinitionsBindingList;
		}

		private UiTraceDefinition currentTraceDefinition
		{
			get
			{
				if (this.traceDefSelectCtrl.SelectedItem != null)
				{
					return (UiTraceDefinition)this.traceDefSelectCtrl.SelectedItem;
				}
				else return null;
			}
		}

		private TraceStatus currentTraceStatus
		{
			get { return this._currentStatus; }

			set
			{
				bool changed = (this._currentStatus != value);
				this._currentStatus = value;

				if (changed) setControlStatesForCurrentStatus();
			}
		}

		private void applySavedClientSettingsToUi()
		{
			bool saveClientSettings = false;

			if (this._clientSettings.WindowSize != new Size(0, 0)) this.Size = this._clientSettings.WindowSize;
			else
			{
				this._clientSettings.WindowSize = this.Size;
				saveClientSettings = true;
			}

			if (this._clientSettings.WindowPosition != new Point(0, 0)) this.Location = this._clientSettings.WindowPosition;
			else
			{
				this._clientSettings.WindowPosition = this.Location;
				saveClientSettings = true;
			}

			if ((!string.IsNullOrEmpty(this._clientSettings.FontFamilyName)) && (this._clientSettings.FontSize > 0))
			{
				this.traceEventDisplay.DisplayGridFont = new Font(this._clientSettings.FontFamilyName, this._clientSettings.FontSize);
			}
			else
			{
				this._clientSettings.FontFamilyName = this.traceEventDisplay.DisplayGridFont.FontFamily.Name;
				this._clientSettings.FontSize = this.traceEventDisplay.DisplayGridFont.SizeInPoints;
				saveClientSettings = true;
			}

			if (this._clientSettings.TraceDefinitions.Count == 0)
			{
				this._clientSettings.TraceDefinitions.Add(UiTraceDefinition.GetDefaultTraceDefinition());
				saveClientSettings = true;
			}

			foreach (UiTraceDefinition traceDefinition in this._clientSettings.TraceDefinitions)
			{
				this._traceDefinitionsBindingList.Add(traceDefinition);
			}

			this.traceDefSelectCtrl.SelectedIndex = -1;
			if (this.traceDefSelectCtrl.Items.Count > 0) this.traceDefSelectCtrl.SelectedIndex = 0;

			if (saveClientSettings) this._clientSettings.Save();
		}

		private void applyUiToClientSettings()
		{
			this._clientSettings.FontFamilyName = this.traceEventDisplay.DisplayGridFont.FontFamily.Name;
			this._clientSettings.FontSize = this.traceEventDisplay.DisplayGridFont.Size;

			if (this.WindowState == FormWindowState.Normal)
			{
				this._clientSettings.WindowSize = this.Size;

				int x = ((this.Location.X < 0) || (this.Location.X > SystemInformation.VirtualScreen.Width)) ?
					0 : this.Location.X;
				int y = ((this.Location.Y < 0) || (this.Location.Y > SystemInformation.VirtualScreen.Height)) ?
					0 : this.Location.Y;

				this._clientSettings.WindowPosition = new Point(x, y);
			}
		}

		private void createAndStartTrace(UiTraceDefinition traceDefinition)
		{
			if ((!traceDefinition.IntegratedConnection) &&
				(string.IsNullOrEmpty(traceDefinition.Password)))
			{
				string prompt = string.Format("Enter password to connect to {0} as {1}.",
					this.currentTraceDefinition.Server, traceDefinition.UserId);
				string password = string.Empty;

				if (InputBox.ShowDialog(this, "Enter password.", prompt, ref password,
				usePasswordChars: true) == DialogResult.Cancel) return;

				traceDefinition.Password = password;
			}

			this._traceController.StartTrace(traceDefinition);
		}

		private void onAboutLabelClick(object sender, LinkLabelLinkClickedEventArgs e)
		{
			SqlExpressTraceUI.AboutDlg aboutDlg = new SqlExpressTraceUI.AboutDlg();
			aboutDlg.ShowDialog(this);
		}

		private void onAddButtonClick(object sender, EventArgs e)
		{
			try
			{
				UiTraceDefinition newTraceDefinition = UiTraceDefinition.GetNewTraceDefinitionWithDefaults();

				TraceDefinitionDlg traceDlg = new TraceDefinitionDlg();
				if (traceDlg.EditTraceDefinition(this, ref newTraceDefinition) != DialogResult.Cancel)
				{
					this._clientSettings.TraceDefinitions.Add(newTraceDefinition);
					this._clientSettings.Save();
					this._traceDefinitionsBindingList.Add(newTraceDefinition);
					this.traceDefSelectCtrl.SelectedItem = newTraceDefinition;
				}

			}
			catch (Exception ex) { Program.DisplayException(this, ex); }
		}

		private void onButtonEnableChanged(object sender, EventArgs e)
		{
			Button button = sender as Button;

			if (button != null)
			{

			}
		}

		private void onCloseClick(object sender, EventArgs e)
		{
			try { this.Close(); }
			catch (Exception ex) { Program.DisplayException(this, ex); }
		}

		private void onColumnOrderChanged(object sender, int[] orderedColumns)
		{
			this.currentTraceDefinition.OrderedColumns = orderedColumns;
			this._clientSettings.Save();
		}

		private void onDefinitionSelectionChanged(object sender, EventArgs e)
		{
			if (this.currentTraceStatus != TraceStatus.Closed) return;
			setControlStatesForCurrentStatus();
			if (currentTraceDefinition != null)
			{
				this.traceEventDisplay.InitializeColumns(currentTraceDefinition.OrderedColumns);
			}
		}

		private void onDeleteButtonClick(object sender, EventArgs e)
		{
			if ((currentTraceDefinition != null) && (!currentTraceDefinition.IsDefault))
			{
				UiTraceDefinition traceDefinition = currentTraceDefinition;

				if (MessageBox.Show(this, "Are you sure you want to delete the " + traceDefinition.Name + " trace definition?",
					"Delete Trace Definition?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					this.traceDefSelectCtrl.SelectedIndex = 0; //default;
					this._clientSettings.TraceDefinitions.Remove(traceDefinition);
					this._clientSettings.Save();
					this._traceDefinitionsBindingList.Remove(traceDefinition);

				}
			}
		}

		private void onEditButtonClick(object sender, EventArgs e)
		{
			try
			{
				UiTraceDefinition traceDefinition = currentTraceDefinition;
				TraceDefinitionDlg traceDefinitionDlg = new TraceDefinitionDlg();
				if (traceDefinitionDlg.EditTraceDefinition(this, ref traceDefinition) != DialogResult.Cancel)
				{
					this._clientSettings.Save();
					this._traceDefinitionsBindingList.ResetItem(this.traceDefSelectCtrl.SelectedIndex);
					this.traceEventDisplay.InitializeColumns(traceDefinition.OrderedColumns);
				}
			}
			catch (Exception ex) { Program.DisplayException(this, ex); }

		}

		private void onFormClosing(object sender, FormClosingEventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			try
			{
				saveSettings();
				if (this._traceController != null) this._traceController.Dispose();
			}
			catch { }
			finally { this.Cursor = Cursors.Default; }
		}

		private void onFormLoad(object sender, EventArgs e)
		{
			try
			{
				this._traceController = new TraceController();
				this._traceController.TraceRecordedEvent += new TraceEventRecordedHandler(onTraceEventRecorded);
				this._traceController.TraceStatusChangedEvent += new TraceStatusChangedEventHandler(onTraceStatusChanged);
				this._traceController.TraceMonitorExceptionEvent += onTraceMonitorExceptionEvent;

				currentTraceStatus = TraceStatus.Closed;
				applySavedClientSettingsToUi();
				setControlStatesForCurrentStatus();

			}
			catch (Exception ex) { Program.DisplayException(this, ex); }

		}

		private void onPauseButtonClick(object sender, EventArgs e)
		{
			try
			{
				if ((this._traceController != null) && (this._traceController.Status == TraceStatus.Started))
				{
					this._traceController.PauseTrace();
				}
			}
			catch (Exception ex) { Program.DisplayException(this, ex); }
			finally { this.Cursor = Cursors.Default; }

		}

		private void onStartButtonClick(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;

			try
			{
				if ((this._traceController != null) && (this._traceController.Status == TraceStatus.Stopped))
				{
					this._traceController.ResumeTrace();
				}
				else if (this.currentTraceDefinition != null) createAndStartTrace(this.currentTraceDefinition);
			}
			catch (Exception ex) { Program.DisplayException(this, ex); }
			finally { this.Cursor = Cursors.Default; }

		}

		private void onStopButtonClick(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			try
			{
				if ((this._traceController != null) && (this._traceController.Status != TraceStatus.Closed))
				{
					_traceController.CloseTrace();
				}
			}
			catch (Exception ex) { Program.DisplayException(this, ex); }
			finally { this.Cursor = Cursors.Default; }
		}

		private void onTraceEventRecorded(object sender, TraceRecord record)
		{
			try
			{
				this.traceEventDisplay.AddTraceRecord(record);
			}
			catch (Exception ex) { Program.DisplayException(this, ex); }
		}

		private void onTraceMonitorExceptionEvent(object sender, TraceMonitorException exception)
		{
			Program.DisplayException(this, exception, "An error occurred in the Trace Monitor");
		}

		private void onTraceStatusChanged(object sender, TraceStatus status)
		{
			this.currentTraceStatus = status;
		}

		private void saveSettings()
		{
			applyUiToClientSettings();
			this._clientSettings.Save();
		}

		private void setControlStatesForClosed()
		{
			this.traceDefEditPanel.Enabled = true;
			this.addButton.Enabled = true;
			this.editButton.Enabled = (currentTraceDefinition != null);
			this.deleteButton.Enabled = ((currentTraceDefinition != null) && (!currentTraceDefinition.IsDefault));
			this.startButton.Enabled = (currentTraceDefinition != null);
			this.stopButton.Enabled = false;
			this.pauseButton.Enabled = false;
			this.statusLabel.Text = "No trace is open and running.";
		}

		private void setControlStatesForCurrentStatus()
		{
			if (currentTraceStatus == TraceStatus.Closed) setControlStatesForClosed();
			else if (currentTraceStatus == TraceStatus.Stopped) setControlStatesForStopped();
			else setControlStatesForStarted();

			this.traceEventDisplay.Status = currentTraceStatus;
		}

		private void setControlStatesForStarted()
		{
			this.traceDefEditPanel.Enabled = false;
			this.startButton.Enabled = false;
			this.stopButton.Enabled = true;
			this.pauseButton.Enabled = true;

			this.statusLabel.Text = "Trace has started and is running.";
		}

		private void setControlStatesForStopped()
		{
			this.traceDefEditPanel.Enabled = false;
			this.startButton.Enabled = true;
			this.stopButton.Enabled = true;
			this.pauseButton.Enabled = false;

			this.statusLabel.Text = "Trace is paused.";
		}

	}
}