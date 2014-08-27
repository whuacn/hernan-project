using System;
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.Tools.TestClient.UI
{
	internal class TabPageCloseOrderManager
	{
		private List<int> leastRecentlyAccessedTabIndexes = new List<int>();

		public TabPageCloseOrderManager()
		{
		}

		private static bool AlwaysTruePredicate(int anyNumber)
		{
			return true;
		}

		internal void HandleTabAdded(int addedTabIndex)
		{
			this.leastRecentlyAccessedTabIndexes.Add(addedTabIndex);
		}

		internal void HandleTabChanged(int selectedTabIndex)
		{
			this.leastRecentlyAccessedTabIndexes.Remove(selectedTabIndex);
			this.leastRecentlyAccessedTabIndexes.Add(selectedTabIndex);
		}

		internal void HandleTabClosed(int removingIndex)
		{
			this.leastRecentlyAccessedTabIndexes.Remove(removingIndex);
			List<int> nums = new List<int>();
			foreach (int leastRecentlyAccessedTabIndex in this.leastRecentlyAccessedTabIndexes)
			{
				int num = leastRecentlyAccessedTabIndex;
				if (num > removingIndex)
				{
					num--;
				}
				nums.Add(num);
			}
			this.leastRecentlyAccessedTabIndexes = nums;
		}

		internal int LastTab()
		{
			return this.leastRecentlyAccessedTabIndexes.FindLast(new Predicate<int>(TabPageCloseOrderManager.AlwaysTruePredicate));
		}

		public override string ToString()
		{
			string empty = string.Empty;
			int num = 0;
			foreach (int leastRecentlyAccessedTabIndex in this.leastRecentlyAccessedTabIndexes)
			{
				string str = string.Concat(empty, "{0}");
				int num1 = num + 1;
				num = num1;
				if (num1 != this.leastRecentlyAccessedTabIndexes.Count)
				{
					str = string.Concat(str, ", ");
				}
				CultureInfo currentUICulture = CultureInfo.CurrentUICulture;
				object[] objArray = new object[] { leastRecentlyAccessedTabIndex };
				empty = string.Format(currentUICulture, str, objArray);
			}
			return empty;
		}
	}
}