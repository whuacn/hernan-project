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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SqlExpressTraceUI
{
	public class SortableBindingList<T> : BindingList<T>
	{
		private readonly Dictionary<Type, PropertyComparer<T>> _comparers;
		private bool _isSorted;
		private ListSortDirection _listSortDirection;
		private PropertyDescriptor _propertyDescriptor;

		public SortableBindingList()
			: base(new List<T>())
		{
			this._comparers = new Dictionary<Type, PropertyComparer<T>>();
		}

		public SortableBindingList(IEnumerable<T> enumeration)
			: base(new List<T>(enumeration))
		{
			this._comparers = new Dictionary<Type, PropertyComparer<T>>();
		}

		protected override bool IsSortedCore
		{
			get { return this._isSorted; }
		}

		protected override ListSortDirection SortDirectionCore
		{
			get { return this._listSortDirection; }
		}

		protected override PropertyDescriptor SortPropertyCore
		{
			get { return this._propertyDescriptor; }
		}

		protected override bool SupportsSearchingCore
		{
			get { return true; } // <-- The BindingList base is also hard-coded this way to return false;
		}

		protected override bool SupportsSortingCore
		{
			get { return true; } // <-- The BindingList base is also hard-coded this way to return false;
		}

		protected override void ApplySortCore(PropertyDescriptor property, ListSortDirection direction)
		{
			List<T> itemsList = (List<T>)this.Items;

			Type propertyType = property.PropertyType;
			PropertyComparer<T> comparer;
			if (!this._comparers.TryGetValue(propertyType, out comparer))
			{
				comparer = new PropertyComparer<T>(property, direction);
				this._comparers.Add(propertyType, comparer);
			}

			comparer.SetPropertyAndDirection(property, direction);
			itemsList.Sort(comparer);

			this._propertyDescriptor = property;
			this._listSortDirection = direction;
			this._isSorted = true;

			this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
		}

		protected override int FindCore(PropertyDescriptor property, object key)
		{
			int count = this.Count;
			for (int i = 0; i < count; ++i)
			{
				T element = this[i];
				if (property.GetValue(element).Equals(key))
				{
					return i;
				}
			}

			return -1;
		}

		protected override void RemoveSortCore()
		{
			this._isSorted = false;
			this._propertyDescriptor = base.SortPropertyCore;
			this._listSortDirection = base.SortDirectionCore;

			this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
		}

		private class PropertyComparer<T> : IComparer<T>
		{
			private readonly IComparer _comparer;
			private PropertyDescriptor _propertyDescriptor;
			private int _reverse;

			public PropertyComparer(PropertyDescriptor property, ListSortDirection direction)
			{
				this._propertyDescriptor = property;
				Type comparerForPropertyType = typeof(Comparer<>).MakeGenericType(property.PropertyType);
				this._comparer = (IComparer)comparerForPropertyType.InvokeMember("Default",
					System.Reflection.BindingFlags.Static |
					System.Reflection.BindingFlags.GetProperty |
					System.Reflection.BindingFlags.Public, null, null, null);
				this.SetListSortDirection(direction);
			}

			public int Compare(T value1, T value2)
			{
				return this._reverse * this._comparer.Compare(
					this._propertyDescriptor.GetValue(value1),
					this._propertyDescriptor.GetValue(value2));
			}

			public void SetPropertyAndDirection(PropertyDescriptor descriptor, ListSortDirection direction)
			{
				this.SetPropertyDescriptor(descriptor);
				this.SetListSortDirection(direction);
			}

			private void SetListSortDirection(ListSortDirection sortDirection)
			{
				this._reverse = (sortDirection == ListSortDirection.Ascending) ? 1 : -1;
			}

			private void SetPropertyDescriptor(PropertyDescriptor descriptor)
			{
				this._propertyDescriptor = descriptor;
			}
		}
	}

}