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
using System.Collections.Generic;
using System;

namespace SqlExpressTraceLib
{
	//Provides a duel-index dictionary that uses an int that is the Id property of the InfoItem,
	//or a string that is the Name property of the InfoItem.
	public class InfoItemCollection<TKey, TValue> : Dictionary<TKey, TValue>
		where TValue : InfoItem
	{
		protected Dictionary<string, TValue> stringIndexer = new Dictionary<string, TValue>();

		public TValue this[string itemName]
		{
			get
			{
				if (stringIndexer.ContainsKey(itemName)) return stringIndexer[itemName];
				else return null;
			}
		}

		public new void Add(TKey key, TValue value)
		{
			base.Add(key, value);
			stringIndexer.Add(value.Name, value);
		}
	}

}
