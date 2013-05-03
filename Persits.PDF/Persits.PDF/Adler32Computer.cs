using System;
namespace Persits.PDF
{
	internal class Adler32Computer
	{
		private int a = 1;
		private int b;
		private static readonly int Modulus = 65521;
		public int Checksum
		{
			get
			{
				return this.b * 65536 + this.a;
			}
		}
		public void Update(byte[] data, int offset, int length)
		{
			for (int i = 0; i < length; i++)
			{
				this.a = (this.a + (int)data[offset + i]) % Adler32Computer.Modulus;
				this.b = (this.b + this.a) % Adler32Computer.Modulus;
			}
		}
	}
}
