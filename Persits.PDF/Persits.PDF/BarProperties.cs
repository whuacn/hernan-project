using System;
namespace Persits.PDF
{
	internal class BarProperties
	{
		public float fWidth;
		public float fTop;
		public float fBottom;
		public BarProperties()
		{
			this.fWidth = (this.fTop = (this.fBottom = PdfBarcode.BAR_UNSPEC));
		}
	}
}
