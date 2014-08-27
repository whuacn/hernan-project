using Microsoft.Tools.TestClient.Variables;
using Microsoft.VisualStudio.VirtualTreeGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Threading;
using System.Windows.Forms;

namespace Microsoft.Tools.TestClient
{
	internal class ParameterTreeAdapter : IMultiColumnBranch, IBranch
	{
		internal const int NumColumns = 3;

		private ParameterTreeAdapter[] children;

		private ParameterTreeAdapter parent;

		private bool readOnly;

		private int relativeRow;

		private Variable[] variables;

		private ITree virtualTree;

		private VirtualTreeControl virtualTreeControl;

		private DataSetPropertyDescriptor dataSetPropertyDescriptor;

		public int ColumnCount
		{
			get
			{
				return 3;
			}
		}

		public BranchFeatures Features
		{
			get
			{
                return BranchFeatures.ImmediateSelectionLabelEdits;
				//return 83 | 16384;
			}
		}

		public int UpdateCounter
		{
			get
			{
				return 0;
			}
		}

		public int VisibleItemCount
		{
			get
			{
				return (int)this.variables.Length;
			}
		}

		internal ParameterTreeAdapter(ITree virtualTree, VirtualTreeControl virtualTreeControl, Variable[] variables, bool readOnly, ParameterTreeAdapter parent)
		{
			this.virtualTree = virtualTree;
			this.virtualTreeControl = virtualTreeControl;
			this.variables = variables;
			this.readOnly = readOnly;
			this.parent = parent;
			this.children = new ParameterTreeAdapter[(int)variables.Length];
			this.OnBranchModification += new BranchModificationEventHandler(this.ParameterTreeAdapter_OnBranchModification);
		}

		public VirtualTreeLabelEditData BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
		{
			if (column != 1)
			{
				return VirtualTreeLabelEditData.Invalid;
			}
			if (this.readOnly && !(this.variables[row] is DataSetVariable))
			{
				return VirtualTreeLabelEditData.Invalid;
			}
			VirtualTreeLabelEditData @default = VirtualTreeLabelEditData.Default;
			if (this.variables[row].EditorType != EditorType.TextBox)
			{
				ParameterTreeAdapter.ChoiceContainer choiceContainer = new ParameterTreeAdapter.ChoiceContainer(this, row, column);
				if (this.variables[row].EditorType != EditorType.EditableDropDownBox)
				{
					if (this.variables[row] is DataSetVariable && this.ShouldPopupDataSetEditor(row))
					{
						this.dataSetPropertyDescriptor = new DataSetPropertyDescriptor(this.variables[row], this.readOnly, this);
						TypeEditorHost typeEditorHost = TypeEditorHost.Create(this.dataSetPropertyDescriptor, null);
						typeEditorHost.KeyDown += new KeyEventHandler(this.dataSetHost_KeyDown);
						return new VirtualTreeLabelEditData(typeEditorHost);
					}
					if (this.variables[row].EditorType == EditorType.DropDownBox)
					{
						ParameterTreeAdapter.ChoiceConverter.StaticExclusive = true;
					}
				}
				else
				{
					ParameterTreeAdapter.ChoiceConverter.StaticExclusive = false;
				}
				ParameterTreeAdapter.ChoiceConverter.StaticChoices = this.variables[row].GetSelectionList();
				PropertyDescriptor item = TypeDescriptor.GetProperties(choiceContainer)["Choice"];
				@default.CustomInPlaceEdit = TypeEditorHost.Create(item, choiceContainer);
			}
			else
			{
				ParameterTreeAdapter.DummyContainer dummyContainer = new ParameterTreeAdapter.DummyContainer();
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(dummyContainer)["DummyProperty"];
				@default.CustomInPlaceEdit = TypeEditorHost.Create(propertyDescriptor, dummyContainer);
			}
			return @default;
		}

		public void Close()
		{
			if (this.dataSetPropertyDescriptor != null)
			{
				this.dataSetPropertyDescriptor.Close();
			}
			if (this.children != null)
			{
				ParameterTreeAdapter[] parameterTreeAdapterArray = this.children;
				for (int i = 0; i < (int)parameterTreeAdapterArray.Length; i++)
				{
					ParameterTreeAdapter parameterTreeAdapter = parameterTreeAdapterArray[i];
					if (parameterTreeAdapter != null)
					{
						parameterTreeAdapter.Close();
					}
				}
			}
		}

		public SubItemCellStyles ColumnStyles(int column)
		{
			if (column != 0)
			{
				return SubItemCellStyles.Simple;
			}
			return SubItemCellStyles.Expandable;
		}

		public LabelEditResult CommitLabelEdit(int row, int column, string newText)
		{
			if (newText == null || this.readOnly || this.ShouldPopupDataSetEditor(row) && !SerializableType.IsNullRepresentation(newText))
			{
				this.virtualTreeControl.EndLabelEdit(true);
				return LabelEditResult.CancelEdit;
			}
			ValidationResult validationResult = this.variables[row].SetValue(newText);
			if (!validationResult.Valid)
			{
				this.virtualTreeControl.EndLabelEdit(true);
				return LabelEditResult.CancelEdit;
			}
			if (validationResult.RefreshRequired)
			{
				this.virtualTreeControl.BeginUpdate();
				this.relativeRow = row;
				this.virtualTree.ListShuffle = true;
				this.virtualTree.Realign(this);
				this.virtualTreeControl.EndUpdate();
				this.virtualTree.ListShuffle = false;
			}
			this.PropagateValueUpdateEvent();
			return LabelEditResult.AcceptEdit;
		}

		private void dataSetHost_KeyDown(object sender, KeyEventArgs e)
		{
			if (!e.Alt && !e.Control && !e.Shift && e.KeyData == Keys.Space)
			{
				this.dataSetPropertyDescriptor = (sender as TypeEditorHost).CurrentPropertyDescriptor as DataSetPropertyDescriptor;
				((DataSetUITypeEditor)this.dataSetPropertyDescriptor.GetEditor(null)).EditValue(null, this.dataSetPropertyDescriptor.GetValue(this));
			}
		}

		public VirtualTreeAccessibilityData GetAccessibilityData(int row, int column)
		{
			return new VirtualTreeAccessibilityData();
		}

		public VirtualTreeDisplayData GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
		{
			return VirtualTreeDisplayData.Empty;
		}

		public int GetJaggedColumnCount(int row)
		{
			return 0;
		}

		public object GetObject(int row, int column, ObjectStyle style, ref int options)
		{
			if (style == ObjectStyle.TrackingObject)
			{
				return new RowCol(row, column);
			}
			if (!this.IsExpandable(row, column))
			{
				return null;
			}
			ParameterTreeAdapter[] parameterTreeAdapterArray = this.children;
			ParameterTreeAdapter parameterTreeAdapter = new ParameterTreeAdapter(this.virtualTree, this.virtualTreeControl, this.variables[row].GetChildVariables(), this.readOnly, this);
			ParameterTreeAdapter parameterTreeAdapter1 = parameterTreeAdapter;
			parameterTreeAdapterArray[row] = parameterTreeAdapter;
			return parameterTreeAdapter1;
		}

		public string GetText(int row, int column)
		{
			if (column == 0)
			{
				return this.variables[row].Name;
			}
			if (column != 2)
			{
				if (column != 1)
				{
					return "";
				}
				return this.variables[row].GetValue();
			}
			if (this.variables[row].TypeName.Equals(typeof(NullObject).FullName))
			{
				return "NullObject";
			}
			return this.variables[row].FriendlyTypeName;
		}

		public string GetTipText(int row, int column, ToolTipType tipType)
		{
			return this.GetText(row, column);
		}

		internal Variable[] GetVariables()
		{
			return this.variables;
		}

		public bool IsExpandable(int row, int column)
		{
			if (column != 0)
			{
				return false;
			}
			return this.variables[row].IsExpandable();
		}

		public LocateObjectData LocateObject(object obj, ObjectStyle style, int locateOptions)
		{
			LocateObjectData row = new LocateObjectData();
			if (style == ObjectStyle.TrackingObject)
			{
				RowCol rowCol = (RowCol)obj;
				row.Row = rowCol.Row;
				row.Column = rowCol.Col;
				row.Options = 1;
			}
			else if (style == ObjectStyle.ExpandedBranch)
			{
				ParameterTreeAdapter parameterTreeAdapter = (ParameterTreeAdapter)obj;
				ParameterTreeAdapter parameterTreeAdapter1 = parameterTreeAdapter.parent;
				row.Row = -1;
				for (int i = 0; i < (int)parameterTreeAdapter1.children.Length; i++)
				{
					if (parameterTreeAdapter1.children[i] == parameterTreeAdapter)
					{
						row.Row = i;
					}
				}
				row.Column = 0;
				if (row.Row != this.relativeRow)
				{
					row.Options = 1;
				}
				else
				{
					row.Options = 0;
				}
			}
			return row;
		}

		public void OnDragEvent(object sender, int row, int column, DragEventType eventType, DragEventArgs args)
		{
		}

		public void OnGiveFeedback(GiveFeedbackEventArgs args, int row, int column)
		{
		}

		public void OnQueryContinueDrag(QueryContinueDragEventArgs args, int row, int column)
		{
		}

		public VirtualTreeStartDragData OnStartDrag(object sender, int row, int column, DragReason reason)
		{
			return VirtualTreeStartDragData.Empty;
		}

		public void ParameterTreeAdapter_OnBranchModification(object sender, BranchModificationEventArgs e)
		{
		}

		public void PropagateValueUpdateEvent()
		{
			ParameterTreeAdapter parameterTreeAdapter = this;
			while (parameterTreeAdapter.parent != null)
			{
				parameterTreeAdapter = parameterTreeAdapter.parent;
			}
			if (parameterTreeAdapter.OnValueUpdated != null)
			{
				parameterTreeAdapter.OnValueUpdated();
			}
		}

		private bool ShouldPopupDataSetEditor(int row)
		{
			if (!(this.variables[row] is DataSetVariable))
			{
				return false;
			}
			if (this.readOnly)
			{
				return true;
			}
			return !SerializableType.IsNullRepresentation(this.variables[row].GetValue());
		}

		public StateRefreshChanges SynchronizeState(int row, int column, IBranch matchBranch, int matchRow, int matchColumn)
		{
			return StateRefreshChanges.None;
		}

		public StateRefreshChanges ToggleState(int row, int column)
		{
			return StateRefreshChanges.None;
		}

		public event BranchModificationEventHandler OnBranchModification;

		internal event ParameterTreeAdapter.ValueUpdatedEventHandler OnValueUpdated;

		private sealed class ChoiceContainer
		{
			private ParameterTreeAdapter branch;

			private int column;

			private int row;

			[TypeConverter(typeof(ParameterTreeAdapter.ChoiceConverter))]
			public string Choice
			{
				get
				{
					return this.branch.GetText(this.row, this.column);
				}
				set
				{
					if ((new List<string>(ParameterTreeAdapter.ChoiceConverter.StaticChoices)).Contains(value))
					{
						this.branch.CommitLabelEdit(this.row, this.column, value);
						return;
					}
					this.branch.CommitLabelEdit(this.row, this.column, null);
				}
			}

			internal ChoiceContainer(ParameterTreeAdapter branch, int row, int column)
			{
				this.branch = branch;
				this.row = row;
				this.column = column;
			}
		}

		private sealed class ChoiceConverter : StringConverter
		{
			internal static string[] StaticChoices;

			internal static bool StaticExclusive;

			public ChoiceConverter()
			{
			}

			public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
			{
				return new TypeConverter.StandardValuesCollection(ParameterTreeAdapter.ChoiceConverter.StaticChoices);
			}

			public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
			{
				return ParameterTreeAdapter.ChoiceConverter.StaticExclusive;
			}

			public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
			{
				return true;
			}
		}

		private enum ColumnIndex
		{
			VariableName,
			VariableValue,
			VariableType
		}

		private sealed class DummyContainer
		{
			[Editor(typeof(UITypeEditor), typeof(UITypeEditor))]
			public string DummyProperty
			{
				get
				{
					return null;
				}
			}

			public DummyContainer()
			{
			}
		}

		internal delegate void ValueUpdatedEventHandler();
	}
}