using System;
namespace Persits.PDF
{
	internal class PdfDate : PdfObject
	{
		private DateTime m_dtValue = DateTime.Now;
		public PdfDate(PdfDate obj)
		{
			base.CopyType(obj);
			this.m_dtValue = obj.m_dtValue;
		}
		public PdfDate(string Type, DateTime dtDate)
		{
			this.m_bstrType = Type;
			this.m_dtValue = dtDate;
			this.m_nType = enumType.pdfDate;
		}
		public override int WriteOut(PdfOutput pOutput)
		{
			string text = string.Format("D:{0:d04}{1:d02}{2:d02}{3:d02}{4:d02}{5:d02}", new object[]
			{
				this.m_dtValue.Year,
				this.m_dtValue.Month,
				this.m_dtValue.Day,
				this.m_dtValue.Hour,
				this.m_dtValue.Minute,
				this.m_dtValue.Second
			});
			TimeZone currentTimeZone = TimeZone.CurrentTimeZone;
			int num = -(int)currentTimeZone.GetUtcOffset(DateTime.Now).TotalMinutes;
			char c = 'Z';
			if (num > 0)
			{
				c = '-';
			}
			if (num < 0)
			{
				c = '+';
				num = -num;
			}
			string str = string.Format("{0}{1:d02}'{2:d02}'", c, num / 60, num % 60);
			text += str;
			PdfString pdfString = new PdfString(this.m_bstrType, text);
			return pdfString.WriteOut(pOutput);
		}
		public override PdfObject Copy()
		{
			return new PdfDate(this.m_bstrType, this.m_dtValue);
		}
	}
}
