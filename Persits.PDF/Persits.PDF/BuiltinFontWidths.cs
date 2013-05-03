using System;
namespace Persits.PDF
{
	internal class BuiltinFontWidths
	{
		private BuiltinFontWidth[] tab;
		private int size;
		public BuiltinFontWidths(BuiltinFontWidth[] widths, int sizeA)
		{
			this.size = sizeA;
			this.tab = new BuiltinFontWidth[this.size];
			for (int i = 0; i < this.size; i++)
			{
				this.tab[i] = null;
			}
			for (int i = 0; i < sizeA; i++)
			{
				int num = this.hash(widths[i].name);
				widths[i].next = this.tab[num];
				this.tab[num] = widths[i];
			}
		}
		~BuiltinFontWidths()
		{
		}
		private bool getWidth(string name, ref short width)
		{
			int num = this.hash(name);
			for (BuiltinFontWidth builtinFontWidth = this.tab[num]; builtinFontWidth != null; builtinFontWidth = builtinFontWidth.next)
			{
				if (string.Compare(builtinFontWidth.name, name) == 0)
				{
					width = builtinFontWidth.width;
					return true;
				}
			}
			return false;
		}
		private int hash(string name)
		{
			uint num = 0u;
			for (int i = 0; i < name.Length; i++)
			{
				char c = name[i];
				num = 17u * num + (uint)((byte)c & 255);
			}
			return (int)((ulong)num % (ulong)((long)this.size));
		}
	}
}
