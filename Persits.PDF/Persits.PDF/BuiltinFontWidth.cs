using System;
namespace Persits.PDF
{
	internal class BuiltinFontWidth
	{
		public string name;
		public short width;
		public BuiltinFontWidth next;
		internal BuiltinFontWidth(string n, short w, BuiltinFontWidth nxt)
		{
			this.name = n;
			this.width = w;
			this.next = nxt;
		}
	}
}
