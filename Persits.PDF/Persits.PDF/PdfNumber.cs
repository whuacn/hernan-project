using System;
using System.Globalization;
using System.Text;
namespace Persits.PDF
{
	internal class PdfNumber : PdfObject
	{
		public double m_fValue;
		public PdfNumber()
		{
			this.m_nType = enumType.pdfNumber;
			this.m_fValue = 0.0;
		}
		public PdfNumber(PdfNumber obj)
		{
			base.CopyType(obj);
			this.m_fValue = obj.m_fValue;
		}
		public PdfNumber(string Name, double Value)
		{
			this.m_nType = enumType.pdfNumber;
			this.m_bstrType = Name;
			this.m_fValue = Value;
		}
		public PdfNumber(string Name, string Value)
		{
			this.m_nType = enumType.pdfNumber;
			this.m_bstrType = Name;
			if (Value != null)
			{
				this.m_fValue = Convert.ToDouble(Value, CultureInfo.InvariantCulture);
			}
		}
		public override int WriteOut(PdfOutput pOutput)
		{
			int result = 0;
			base.WriteOutName(pOutput, ref result);
			pOutput.Write(this.m_fValue, ref result);
			return result;
		}
		public override PdfObject Copy()
		{
			return new PdfNumber(this.m_bstrType, this.m_fValue);
		}
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(string.Format(new NumberFormatInfo
			{
				PercentDecimalSeparator = ".",
				PercentGroupSeparator = ""
			}, "{0:f6}", new object[]
			{
				this.m_fValue
			}));
			while (stringBuilder[stringBuilder.Length - 1] == '0')
			{
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			if (stringBuilder[stringBuilder.Length - 1] == '.')
			{
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			return stringBuilder.ToString();
		}
		public float ToFloat()
		{
			return (float)this.m_fValue;
		}
	}
}
