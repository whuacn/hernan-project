using System;
namespace Persits.PDF
{
	internal class ReedSol
	{
		private int gfpoly;
		private int symsize;
		private int logmod;
		private int rlen;
		private int[] log;
		private int[] alog;
		private int[] rspoly;
		public void rs_init_gf(int poly)
		{
			int i = 1;
			int num = 0;
			while (i <= poly)
			{
				num++;
				i <<= 1;
			}
			i >>= 1;
			num--;
			this.gfpoly = poly;
			this.symsize = num;
			this.logmod = (1 << num) - 1;
			this.log = new int[this.logmod + 1];
			this.alog = new int[this.logmod];
			int num2 = 1;
			for (int j = 0; j < this.logmod; j++)
			{
				this.alog[j] = num2;
				this.log[num2] = j;
				num2 <<= 1;
				if ((num2 & i) != 0)
				{
					num2 ^= poly;
				}
			}
		}
		public void rs_init_code(int nsym, int index)
		{
			this.rspoly = new int[nsym + 1];
			this.rlen = nsym;
			this.rspoly[0] = 1;
			for (int i = 1; i <= nsym; i++)
			{
				this.rspoly[i] = 1;
				for (int j = i - 1; j > 0; j--)
				{
					if (this.rspoly[j] != 0)
					{
						this.rspoly[j] = this.alog[(this.log[this.rspoly[j]] + index) % this.logmod];
					}
					this.rspoly[j] ^= this.rspoly[j - 1];
				}
				this.rspoly[0] = this.alog[(this.log[this.rspoly[0]] + index) % this.logmod];
				index++;
			}
		}
		public void rs_encode(int len, byte[] data, out byte[] res)
		{
			res = new byte[this.rlen];
			for (int i = 0; i < this.rlen; i++)
			{
				res[i] = 0;
			}
			for (int i = 0; i < len; i++)
			{
				int num = (int)(res[this.rlen - 1] ^ data[i]);
				for (int j = this.rlen - 1; j > 0; j--)
				{
					if (num != 0 && this.rspoly[j] != 0)
					{
						res[j] = (byte)((int)res[j - 1] ^ this.alog[(this.log[num] + this.log[this.rspoly[j]]) % this.logmod]);
					}
					else
					{
						res[j] = res[j - 1];
					}
				}
				if (num != 0 && this.rspoly[0] != 0)
				{
					res[0] = (byte)this.alog[(this.log[num] + this.log[this.rspoly[0]]) % this.logmod];
				}
				else
				{
					res[0] = 0;
				}
			}
		}
	}
}
