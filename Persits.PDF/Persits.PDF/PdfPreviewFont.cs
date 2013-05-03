using System;
namespace Persits.PDF
{
	internal class PdfPreviewFont
	{
		internal const int nBuiltinFonts = 14;
		internal const int nBuiltinFontSubsts = 12;
		private bool ok;
		private string tag;
		private string name;
		private string origName;
		private _Point id;
		public bool isOk
		{
			get
			{
				return this.ok;
			}
		}
		private PdfPreviewFont(string tagA, _Point idA, string nameA)
		{
			this.ok = false;
			this.tag = tagA;
			this.id = idA;
			this.name = nameA;
			this.origName = nameA;
		}
		public static PdfPreviewFont makeFont(string tagA, _Point idA, PdfDict fontDict, PdfPreview pPreview)
		{
			return null;
		}
		public bool matches(string tagA)
		{
			return string.Compare(this.tag, tagA) == 0;
		}
	}
}
