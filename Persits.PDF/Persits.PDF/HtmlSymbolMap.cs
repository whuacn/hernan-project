using System;
namespace Persits.PDF
{
	internal class HtmlSymbolMap
	{
		public string symbol;
		public char code;
		public HtmlSymbolMap(string s, int c)
		{
			this.symbol = s;
			this.code = (char)c;
		}
	}
}
