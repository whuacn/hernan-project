using System;
using System.Runtime.CompilerServices;
namespace My_Watermark
{
	public class Class_AddDataItem
	{
		public object my_Value;
		public string my_Description;
		public Class_AddDataItem(object my_NewValue, string my_NewDescription)
		{
			this.my_Value = RuntimeHelpers.GetObjectValue(my_NewValue);
			this.my_Description = my_NewDescription;
		}
		public override string ToString()
		{
			return this.my_Description;
		}
	}
}
