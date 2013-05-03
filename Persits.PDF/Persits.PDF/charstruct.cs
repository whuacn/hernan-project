using System;
namespace Persits.PDF
{
	internal class charstruct
	{
		public char basechar;
		public char mark1;
		public char vowel;
		public byte lignum;
		public byte numshapes = 1;
		public void Set(charstruct old)
		{
			this.basechar = old.basechar;
			this.mark1 = old.mark1;
			this.vowel = old.vowel;
			this.lignum = old.lignum;
			this.numshapes = old.numshapes;
		}
	}
}
