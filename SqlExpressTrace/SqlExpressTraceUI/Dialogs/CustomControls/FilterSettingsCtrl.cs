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
	public partial class FilterSettingsCtrl : UserControl
	{
		private TraceFilterList _traceFilters = new TraceFilterList();

		public FilterSettingsCtrl()
		{
			InitializeComponent();
		}

		public TraceFilterList TraceFilters
		{
			get { return this._traceFilters; }

			set
			{
				this._traceFilters = value;
				if (this.filterColumnListBox.Items.Count == 0) initializeLists();
				applyDefinedTraceFilters();
			}
		}

		private ColumnInfoItem selectedColumnInfoItem
		{
			get
			{
				ColumnInfoItem selected = null;
				if (this.filterColumnListBox.SelectedIndex >= 0)
				{
					selected = this.filterColumnListBox.SelectedItem as ColumnInfoItem;
				}

				return selected;
			}
		}

		private void applyDefinedTraceFilters()
		{
			this.TraceFilters.SortFiltersByColumnId();
			this.currentFiltersListBox.Items.Clear();
			foreach (TraceFilter filter in TraceFilters) this.currentFiltersListBox.Items.Add(filter);

			if (this.currentFiltersListBox.Items.Count > 0)
			{
				this.currentFiltersListBox.Focus();
				this.currentFiltersListBox.SelectedIndex = 0;
			}
		}

		private bool currentColumnHasFilter(TraceFilter filter)
		{
			List<TraceFilter> filters = TraceFilters.FiltersForColumn(filter.ColumnId);
			foreach (TraceFilter currentFilter in filters)
			{
				if (string.Compare(filter.ToString(), currentFilter.ToString(), ignoreCase: true) == 0) return true;
			}
			return false;
		}

		private void initializeLists()
		{
			foreach (ColumnInfoItem columnInfo in TraceInfo.TraceColumnsInfoList.Values)
			{
				this.filterColumnListBox.Items.Add(columnInfo);
			}

			foreach (string operatorName in Enum.GetNames(typeof(ComparisonOperators)))
			{
				this.operatorList.Items.Add(operatorName);
			}

			this.operatorList.SelectedIndex = 0;
		}

		private void onAddFilterClick(object sender, EventArgs e)
		{
			LogicalOperators lOperator = this.optionAdd.Checked ? LogicalOperators.And : LogicalOperators.Or;
			ComparisonOperators cOperator = (ComparisonOperators)Enum.Parse(typeof(ComparisonOperators),
				this.operatorList.Text);

			TraceFilter newFilter = new TraceFilter(selectedColumnInfoItem.Id, lOperator, cOperator, this.filterValueTextBox.Text);

			if (!currentColumnHasFilter(newFilter))
			{
				TraceFilters.Add(newFilter);
				int idx = this.currentFiltersListBox.Items.Add(newFilter);
				this.currentFiltersListBox.Focus();
				this.currentFiltersListBox.SelectedIndex = idx;
			}
			else
			{
				string msg = "A filter with these values has already been defined for column " + selectedColumnInfoItem.Name;
				MessageBox.Show(this, msg, "Filter Already Defined");
			}

		}

		private void onCurrentFiltersSelectChanged(object sender, EventArgs e)
		{
			this.removeButton.Enabled = (this.currentFiltersListBox.SelectedIndex >= 0);
		}

		private void onRemoveButtonClick(object sender, EventArgs e)
		{
			if ((selectedColumnInfoItem != null) && (this.currentFiltersListBox.SelectedIndex >= 0))
			{
				TraceFilter filter = (TraceFilter)this.currentFiltersListBox.SelectedItem;
				TraceFilters.Remove(filter);
				applyDefinedTraceFilters();
			}
		}

		private void onValueChanged(object sender, EventArgs e)
		{
			this.addButton.Enabled = (this.filterValueTextBox.Text.Trim().Length > 0);
		}
	}
}