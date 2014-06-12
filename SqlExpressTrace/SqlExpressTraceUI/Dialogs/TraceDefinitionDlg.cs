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
using System.Windows.Forms;
using SqlExpressTraceLib;

namespace SqlTraceExpressUI
{
	public partial class TraceDefinitionDlg : Form
	{
		private readonly static string _missingValueMsgFormat = "A value must be provided for {0}";
		private readonly static string _nameUsedMsgFormat = "The trace definition name {0} is currently being used.  Please enter another trace definition name.";
		private readonly static string _noColumnsMsg = "One or more selected trace event has no columns defined.";
		private readonly static string _noEventsMsg = "There are no trace events selected for the trace definition.";
		private UiTraceDefinition _traceDefinition = new UiTraceDefinition();

		public TraceDefinitionDlg()
		{
			InitializeComponent();
		}

		public DialogResult EditTraceDefinition(IWin32Window owner, ref UiTraceDefinition traceDefinition)
		{
			this._traceDefinition = traceDefinition;
			this.generalSettingsCtrl.AllowEditingName = (!traceDefinition.IsDefault);
			this.Text = "Trace Definition for " + this._traceDefinition.Name;
			applyTraceDefinitionToUi();

			return this.ShowDialog(owner);
		}

		private void applyEventSettings()
		{
			this.eventSettingsCtrl.TraceEvents = this._traceDefinition.TraceEvents.Copy();
		}

		private void applyEventSettingsControl()
		{
			this._traceDefinition.TraceEvents = this.eventSettingsCtrl.TraceEvents;
			this._traceDefinition.UpdateOrderedColumns();
		}

		private void applyFilterSettings()
		{
			this.filterSettingsCtrl.TraceFilters = this._traceDefinition.TraceFilters.Copy();
		}

		private void applyFilterSettingsControl()
		{
			this._traceDefinition.TraceFilters = this.filterSettingsCtrl.TraceFilters;
		}

		private void applyGeneralSettings()
		{
			this.generalSettingsCtrl.DefinitionName = this._traceDefinition.Name;
			this.generalSettingsCtrl.ServerName = this._traceDefinition.Server;
			this.generalSettingsCtrl.UserId = this._traceDefinition.UserId;
			this.generalSettingsCtrl.DurationMinutes = this._traceDefinition.DurationMinutes;
			this.generalSettingsCtrl.DurationHours = this._traceDefinition.DurationHours;
			this.generalSettingsCtrl.RecordLimit = this._traceDefinition.RecordLimit;
			this.generalSettingsCtrl.IntegratedConnect = this._traceDefinition.IntegratedConnection;
		}

		private void applyGeneralSettingsControl()
		{
			this._traceDefinition.Name = this.generalSettingsCtrl.DefinitionName;
			this._traceDefinition.Server = this.generalSettingsCtrl.ServerName;
			this._traceDefinition.UserId = this.generalSettingsCtrl.UserId;
			this._traceDefinition.DurationMinutes = this.generalSettingsCtrl.DurationMinutes;
			this._traceDefinition.DurationHours = this.generalSettingsCtrl.DurationHours;
			this._traceDefinition.RecordLimit = this.generalSettingsCtrl.RecordLimit;
			this._traceDefinition.IntegratedConnection = this.generalSettingsCtrl.IntegratedConnect;
		}

		private void applyTraceDefinitionToUi()
		{
			applyGeneralSettings();
			applyEventSettings();
			applyFilterSettings();
		}

		private void applyUiValuesToTraceDefinition()
		{
			applyGeneralSettingsControl();
			applyEventSettingsControl();
			applyFilterSettingsControl();
		}

		private void displayProblemMessage(string caption, string msg, params object[] msgParameters)
		{
			MessageBoxButtons buttons = MessageBoxButtons.OK;
			MessageBoxIcon icon = MessageBoxIcon.Stop;
			string message = (msgParameters.Length > 0) ? string.Format(msg, msgParameters) : msg;
			MessageBox.Show(this, message, caption, buttons, icon);
		}

		private bool eachEventHasColumns()
		{
			foreach (TraceEvent traceEvent in eventSettingsCtrl.TraceEvents)
			{
				if (traceEvent.Columns.Count == 0) return false;
			}
			return true;
		}

		private bool nameIsBeingUsed(string name)
		{
			ClientSettings settings = ClientSettings.GetSavedSettings();
			foreach (TraceDefinition traceDefinition in settings.TraceDefinitions)
			{
				if ((string.Compare(traceDefinition.Name, name, ignoreCase: true) == 0) &&
					(traceDefinition != this._traceDefinition))
				{
					return true;
				}
			}

			return false;
		}

		private void onCancelButtonClick(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void onOkButtonClick(object sender, EventArgs e)
		{
			if (!validateSettings()) return;

			applyUiValuesToTraceDefinition();
			this.DialogResult = DialogResult.OK;
		}

		private bool validateSettings()
		{
			if (this.generalSettingsCtrl.DefinitionName.Length == 0)
			{
				this.tabControl1.SelectedTab = generalSettingsTab;
				displayProblemMessage("Required Value Missing", _missingValueMsgFormat, "Definition Name");
				this.generalSettingsCtrl.FocusTraceDefinitionName();
				return false;
			}
			else if ((this.generalSettingsCtrl.AllowEditingName) &&
				(nameIsBeingUsed(this.generalSettingsCtrl.DefinitionName)))
			{
				displayProblemMessage("Definition Name is Used", _nameUsedMsgFormat,
					this.generalSettingsCtrl.DefinitionName);
				this.tabControl1.SelectedTab = generalSettingsTab;
				this.generalSettingsCtrl.FocusTraceDefinitionName();
				return false;
			}
			else if (this.generalSettingsCtrl.ServerName.Length == 0)
			{
				displayProblemMessage("Required Value Missing", _missingValueMsgFormat, "Server Name");
				this.tabControl1.SelectedTab = generalSettingsTab;
				this.generalSettingsCtrl.FocusServerName();
				return false;
			}
			else if (!this.generalSettingsCtrl.IntegratedConnect && (this.generalSettingsCtrl.UserId.Length == 0))
			{
				displayProblemMessage("Required Value Missing", _missingValueMsgFormat, "User Id");
				this.tabControl1.SelectedTab = generalSettingsTab;
				this.generalSettingsCtrl.FocusUserId();
				return false;
			}
			else if (eventSettingsCtrl.TraceEvents.Count == 0)
			{
				displayProblemMessage("No Events", _noEventsMsg);
				this.tabControl1.SelectedTab = eventsTab;
				return false;
			}
			else if (!eachEventHasColumns())
			{
				displayProblemMessage("No Columns in Event(s)", _noColumnsMsg);
				this.tabControl1.SelectedTab = eventsTab;
				return false;
			}
			return true;
		}
	}
}