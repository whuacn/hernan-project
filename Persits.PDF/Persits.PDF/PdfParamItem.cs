using System;
namespace Persits.PDF
{
	internal class PdfParamItem
	{
		private string m_bstrName;
		private float m_fValue;
		public string Name
		{
			get
			{
				return this.m_bstrName;
			}
			set
			{
				this.m_bstrName = value;
			}
		}
		public float Value
		{
			get
			{
				return this.m_fValue;
			}
			set
			{
				this.m_fValue = value;
			}
		}
		internal PdfParamItem()
		{
			this.m_fValue = 0f;
			this.m_bstrName = "";
		}
	}
}
