using System;
namespace Persits.PDF
{
	internal class NameToCharCode
	{
		private NameToCharCodeEntry[] tab;
		private int size;
		private int len;
		public NameToCharCode()
		{
			this.size = 31;
			this.len = 0;
			this.tab = new NameToCharCodeEntry[this.size];
			for (int i = 0; i < this.size; i++)
			{
				this.tab[i] = new NameToCharCodeEntry();
				this.tab[i].name = null;
			}
		}
		public void add(string name, uint c)
		{
			int i;
			if (this.len >= this.size / 2)
			{
				int num = this.size;
				NameToCharCodeEntry[] array = this.tab;
				this.size = 2 * this.size + 1;
				this.tab = new NameToCharCodeEntry[this.size];
				for (i = 0; i < this.size; i++)
				{
					this.tab[i] = new NameToCharCodeEntry();
					this.tab[i].name = null;
				}
				for (int j = 0; j < num; j++)
				{
					if (array[j].name != null)
					{
						i = this.hash(array[j].name);
						while (this.tab[i].name != null)
						{
							if (++i == this.size)
							{
								i = 0;
							}
						}
						this.tab[i] = array[j];
					}
				}
			}
			i = this.hash(name);
			while (this.tab[i].name != null && string.Compare(this.tab[i].name, name) != 0)
			{
				if (++i == this.size)
				{
					i = 0;
				}
			}
			if (this.tab[i].name == null)
			{
				this.tab[i].name = new string(name.ToCharArray());
			}
			this.tab[i].c = c;
			this.len++;
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
		private uint lookup(string name)
		{
			int num = this.hash(name);
			while (this.tab[num].name != null)
			{
				if (string.Compare(this.tab[num].name, name) == 0)
				{
					return this.tab[num].c;
				}
				if (++num == this.size)
				{
					num = 0;
				}
			}
			return 0u;
		}
	}
}
