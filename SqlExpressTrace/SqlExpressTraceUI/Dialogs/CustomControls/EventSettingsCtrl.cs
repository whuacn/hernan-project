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
using System.Windows.Forms;

using SqlExpressTraceLib;

namespace SqlExpressTraceUI.CustomControls
{
	public partial class EventSettingsCtrl : UserControl
	{
		private bool _preventDisplayingDescriptions = false;
		private TraceEventList _traceEvents = new TraceEventList();
		private Dictionary<int, List<int>> recentEventColumnSelections = new Dictionary<int, List<int>>();

		public EventSettingsCtrl()
		{
			InitializeComponent();
		}

		public TraceEventList TraceEvents
		{
			get { return this._traceEvents; }

			set
			{
				this._traceEvents = value;
				if (this.eventsListBox.Items.Count == 0) initializeLists();
				applyDefinedTraceEvents();
			}
		}

		private ColumnInfoItem selectedColumnInfoItem
		{
			get
			{
				ColumnInfoItem selected = null;
				if (this.columnListBox.SelectedIndex >= 0)
				{
					selected = this.columnListBox.SelectedItem as ColumnInfoItem;
				}

				return selected;
			}
		}

		private EventInfoItem selectedEventInfoItem
		{
			get
			{
				EventInfoItem current = null;
				if (this.eventsListBox.SelectedIndex >= 0)
				{
					current = this.eventsListBox.SelectedItem as EventInfoItem;
				}

				return current;
			}
		}

		private void applyDefinedTraceEvents()
		{
			this.eventsListBox.SelectedIndex = -1;
			for (int index = 0; index < this.eventsListBox.Items.Count; index++)
			{
				EventInfoItem eventInfo = this.eventsListBox.Items[index] as EventInfoItem;
				if (eventInfo == null) continue;

				int eventId = eventInfo.Id;
				this.eventsListBox.SetItemChecked(index, this.TraceEvents.ContainsEvent(eventId));
			}

			if (this.eventsListBox.Items.Count > 0)
			{
				this.eventsListBox.SelectedIndex = 0;
				if (this.nextEventLbl.Enabled) onNextEventLabelClick(this.nextEventLbl, null);
			}
		}

		private int getNextChecked(CheckedListBox listBox)
		{
			for (int i = listBox.SelectedIndex + 1; i < (listBox.Items.Count - 1); i++)
			{
				if (listBox.GetItemCheckState(i) == CheckState.Checked) return i;
			}
			return -1;
		}

		private int getPreviousChecked(CheckedListBox listBox)
		{
			for (int i = listBox.SelectedIndex - 1; i >= 0; i--)
			{
				if (listBox.GetItemCheckState(i) == CheckState.Checked) return i;
			}
			return -1;
		}

		private void initializeLists()
		{
			this.eventsListBox.Items.Clear();
			this.eventsListBox.SelectedIndex = -1;

			foreach (EventInfoItem enventInfo in TraceInfo.TraceEventsInfoList.Values)
			{
				int index = this.eventsListBox.Items.Add(enventInfo);
				this.eventsListBox.SetItemChecked(index, false);
			}

			this.columnListBox.Items.Clear();
			this.columnListBox.SelectedIndex = -1;

			foreach (ColumnInfoItem columnInfo in TraceInfo.TraceColumnsInfoList.Values)
			{
				int index = this.columnListBox.Items.Add(columnInfo);
				this.columnListBox.SetItemChecked(index, false);
			}
		}

		private void onColumnListBoxEnter(object sender, EventArgs e)
		{
			showEventDescription();
		}

		private void onColumnListBoxItemCheck(object sender, ItemCheckEventArgs e)
		{
			// Probably initializing
			if ((selectedEventInfoItem == null) || (selectedColumnInfoItem == null)) return;
			if (!this.TraceEvents.ContainsEvent(selectedEventInfoItem.Id)) return;

			TraceEvent traceEvent = this.TraceEvents.GetEvent(selectedEventInfoItem.Id);

			bool columnIsSelectedForTrace = (e.NewValue == CheckState.Checked);
			bool columnIsInTraceEvent = traceEvent.Columns.Contains(selectedColumnInfoItem.Id);

			if (columnIsSelectedForTrace && (!columnIsInTraceEvent))
			{
				traceEvent.Columns.Add(selectedColumnInfoItem.Id);
			}
			else if ((!columnIsSelectedForTrace) && columnIsInTraceEvent)
			{
				traceEvent.Columns.Remove(selectedColumnInfoItem.Id);
			}
		}

		private void onColumnListBoxSelectionChanged(object sender, EventArgs e)
		{
			showColumnDescription();
			setPreviousAndNextLabelStates();
		}

		private void onEventsListBoxEnter(object sender, EventArgs e)
		{
			showEventDescription();
		}

		private void onEventsListBoxItemCheck(object sender, ItemCheckEventArgs e)
		{
			// Probably initializing lists.
			if ((e.Index < 0) ||
				(e.Index != this.eventsListBox.SelectedIndex) ||
				selectedEventInfoItem == null) return;

			int eventId = selectedEventInfoItem.Id;

			bool eventIsSelectedForTrace = (e.NewValue == CheckState.Checked);
			bool eventIsInTraceDefinition = this.TraceEvents.ContainsEvent(eventId);

			if ((eventIsSelectedForTrace) && (!eventIsInTraceDefinition))
			{
				TraceEvent newEvent = new TraceEvent(eventId);
				this.TraceEvents.Add(newEvent);
				if (this.recentEventColumnSelections.ContainsKey(eventId))
				{
					newEvent.Columns = this.recentEventColumnSelections[eventId];
					this.recentEventColumnSelections.Remove(eventId);
					setColumnListForEvent(newEvent);
				}
			}
			else if ((!eventIsSelectedForTrace) && eventIsInTraceDefinition)
			{
				// mini-backup in case client changes mind.  Cancelling out of dialog prevents losing all prev selections,
				// but losing all column ids int this dialog when accidentally unchecking an event still sucks.
				this.recentEventColumnSelections.Add(eventId, this.TraceEvents.GetEvent(eventId).Columns);
				this.TraceEvents.RemoveEvent(eventId);
			}

		}

		private void onEventsListBoxSelectionChanged(object sender, EventArgs e)
		{
			// Probably initializing lists.
			if (selectedEventInfoItem == null) return;

			setColumnListForEvent(this.TraceEvents.GetEvent(selectedEventInfoItem.Id));

			showEventDescription();
			setPreviousAndNextLabelStates();
		}

		private void onNextColumnLabelClick(object sender, LinkLabelLinkClickedEventArgs e)
		{
			int i = getNextChecked(this.columnListBox);
			if (i > -1) this.columnListBox.SelectedIndex = i;
		}

		private void onNextEventLabelClick(object sender, LinkLabelLinkClickedEventArgs e)
		{
			int i = getNextChecked(this.eventsListBox);
			if (i > -1) this.eventsListBox.SelectedIndex = i;
		}

		private void onPrevColumnLabelClick(object sender, LinkLabelLinkClickedEventArgs e)
		{
			int i = getPreviousChecked(this.columnListBox);
			if (i > -1) this.columnListBox.SelectedIndex = i;
		}

		private void onPrevEventLabelClick(object sender, LinkLabelLinkClickedEventArgs e)
		{
			int i = getPreviousChecked(this.eventsListBox);
			if (i > -1) this.eventsListBox.SelectedIndex = i;
		}

		private void setColumnListForEvent(TraceEvent traceEvent)
		{
			this.columnListBox.SelectedIndex = -1;

			for (int i = 0; i < this.columnListBox.Items.Count; i++)
			{
				ColumnInfoItem column = this.columnListBox.Items[i] as ColumnInfoItem;

				this.columnListBox.SetItemChecked(i, (traceEvent != null) ?
					traceEvent.Columns.Contains(column.Id) : false);
			}

			if (traceEvent != null)
			{
				this.columnListBox.Enabled = true;
				if (this.columnListBox.Items.Count > 0)
				{
					this._preventDisplayingDescriptions = true;
					this.columnListBox.SelectedIndex = 0;
					if (nextColumnLbl.Enabled) onNextColumnLabelClick(nextColumnLbl, null);
					this._preventDisplayingDescriptions = false;
				}
			}
			else this.columnListBox.Enabled = false;

		}

		private void setPreviousAndNextLabelStates()
		{
			this.nextEventLbl.Enabled = (getNextChecked(this.eventsListBox) != -1);
			this.previousEventLbl.Enabled = (getPreviousChecked(this.eventsListBox) != -1);

			this.nextColumnLbl.Enabled = (getNextChecked(this.columnListBox) != -1);
			this.previousColumnLbl.Enabled = (getPreviousChecked(this.columnListBox) != -1);
		}

		private void showColumnDescription()
		{
			if ((!this._preventDisplayingDescriptions) && (selectedColumnInfoItem != null))
			{
				this.descriptionLabel.Text = selectedColumnInfoItem.Description;
			}
		}

		private void showEventDescription()
		{
			if ((!this._preventDisplayingDescriptions) && (selectedEventInfoItem != null))
			{
				this.descriptionLabel.Text = selectedEventInfoItem.Description;
			}
		}
	}
}