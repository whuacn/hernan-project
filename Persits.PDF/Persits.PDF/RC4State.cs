using System;
namespace Persits.PDF
{
	internal class RC4State
	{
		private int m_x;
		private int m_y;
		private int[] m_m = new int[256];
		public void Setup(byte[] key)
		{
			this.m_x = 0;
			this.m_y = 0;
			int[] m = this.m_m;
			for (int i = 0; i < 256; i++)
			{
				m[i] = i;
			}
			int num2;
			int num = num2 = 0;
			for (int i = 0; i < 256; i++)
			{
				int num3 = m[i];
				num2 = (num2 + num3 + (int)key[num] & 255);
				m[i] = m[num2];
				m[num2] = num3;
				if (++num >= key.Length)
				{
					num = 0;
				}
			}
		}
		public void Crypt(byte[] data)
		{
			int num = this.m_x;
			int num2 = this.m_y;
			int[] m = this.m_m;
			for (int i = 0; i < data.Length; i++)
			{
				num = (num + 1 & 255);
				int num3 = m[num];
				num2 = (num2 + num3 & 255);
				int num4 = m[num] = m[num2];
				m[num2] = num3;
				int expr_4A_cp_1 = i;
				data[expr_4A_cp_1] ^= (byte)m[num3 + num4 & 255];
			}
			this.m_x = num;
			this.m_y = num2;
		}
	}
}
