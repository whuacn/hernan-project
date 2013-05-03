using System;
namespace Persits.PDF
{
	internal class BuiltinFont
	{
		public string name;
		public string[] defaultBaseEnc;
		public short ascent;
		public short descent;
		public short[] bbox = new short[4];
		public BuiltinFontWidths widths;
	}
}
