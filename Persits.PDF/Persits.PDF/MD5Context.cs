using System;
namespace Persits.PDF
{
	internal class MD5Context
	{
		private const int S11 = 7;
		private const int S12 = 12;
		private const int S13 = 17;
		private const int S14 = 22;
		private const int S21 = 5;
		private const int S22 = 9;
		private const int S23 = 14;
		private const int S24 = 20;
		private const int S31 = 4;
		private const int S32 = 11;
		private const int S33 = 16;
		private const int S34 = 23;
		private const int S41 = 6;
		private const int S42 = 10;
		private const int S43 = 15;
		private const int S44 = 21;
		public uint[] state = new uint[4];
		public uint[] count = new uint[2];
		public byte[] buffer = new byte[64];
		private static byte[] PADDING;
		public void MD5Init()
		{
			this.count[0] = (this.count[1] = 0u);
			this.state[0] = 1732584193u;
			this.state[1] = 4023233417u;
			this.state[2] = 2562383102u;
			this.state[3] = 271733878u;
		}
		public static void MD5_memcpy(byte[] output, byte[] input, uint len)
		{
			Array.Copy(input, output, (long)((ulong)len));
		}
		public static uint F(uint x, uint y, uint z)
		{
			return (x & y) | (~x & z);
		}
		public static uint G(uint x, uint y, uint z)
		{
			return (x & z) | (y & ~z);
		}
		public static uint H(uint x, uint y, uint z)
		{
			return x ^ y ^ z;
		}
		public static uint I(uint x, uint y, uint z)
		{
			return y ^ (x | ~z);
		}
		public static uint ROTATE_LEFT(uint x, int n)
		{
			return x << n | x >> 32 - n;
		}
		public static void FF(ref uint a, uint b, uint c, uint d, uint x, int s, uint ac)
		{
			a += MD5Context.F(b, c, d) + x + ac;
			a = MD5Context.ROTATE_LEFT(a, s);
			a += b;
		}
		public static void GG(ref uint a, uint b, uint c, uint d, uint x, int s, uint ac)
		{
			a += MD5Context.G(b, c, d) + x + ac;
			a = MD5Context.ROTATE_LEFT(a, s);
			a += b;
		}
		public static void HH(ref uint a, uint b, uint c, uint d, uint x, int s, uint ac)
		{
			a += MD5Context.H(b, c, d) + x + ac;
			a = MD5Context.ROTATE_LEFT(a, s);
			a += b;
		}
		public static void II(ref uint a, uint b, uint c, uint d, uint x, int s, uint ac)
		{
			a += MD5Context.I(b, c, d) + x + ac;
			a = MD5Context.ROTATE_LEFT(a, s);
			a += b;
		}
		public void MD5Update(byte[] input, uint inputLen)
		{
			uint num = this.count[0] >> 3 & 63u;
			if ((this.count[0] += inputLen << 3) < inputLen << 3)
			{
				this.count[1] += 1u;
			}
			this.count[1] += inputLen >> 29;
			uint num2 = 64u - num;
			uint num3;
			if (inputLen >= num2)
			{
				Array.Copy(input, 0L, this.buffer, (long)((ulong)num), (long)((ulong)num2));
				MD5Context.MD5Transform(this.state, this.buffer, 0u);
				num3 = num2;
				while (num3 + 63u < inputLen)
				{
					MD5Context.MD5Transform(this.state, input, num3);
					num3 += 64u;
				}
				num = 0u;
			}
			else
			{
				num3 = 0u;
			}
			Array.Copy(input, (long)((ulong)num3), this.buffer, (long)((ulong)num), (long)((ulong)(inputLen - num3)));
		}
		private static void Encode(byte[] output, uint[] input, uint len)
		{
			uint num = 0u;
			for (uint num2 = 0u; num2 < len; num2 += 4u)
			{
				output[(int)((UIntPtr)num2)] = (byte)(input[(int)((UIntPtr)num)] & 255u);
				output[(int)((UIntPtr)(num2 + 1u))] = (byte)(input[(int)((UIntPtr)num)] >> 8 & 255u);
				output[(int)((UIntPtr)(num2 + 2u))] = (byte)(input[(int)((UIntPtr)num)] >> 16 & 255u);
				output[(int)((UIntPtr)(num2 + 3u))] = (byte)(input[(int)((UIntPtr)num)] >> 24 & 255u);
				num += 1u;
			}
		}
		private static void Decode(uint[] output, byte[] input, uint nShift, uint len)
		{
			uint num = 0u;
			for (uint num2 = nShift; num2 < len + nShift; num2 += 4u)
			{
				output[(int)((UIntPtr)num)] = (uint)((int)input[(int)((UIntPtr)num2)] | (int)input[(int)((UIntPtr)(num2 + 1u))] << 8 | (int)input[(int)((UIntPtr)(num2 + 2u))] << 16 | (int)input[(int)((UIntPtr)(num2 + 3u))] << 24);
				num += 1u;
			}
		}
		private static void MD5Transform(uint[] state, byte[] block, uint nShift)
		{
			uint num = state[0];
			uint num2 = state[1];
			uint num3 = state[2];
			uint num4 = state[3];
			uint[] array = new uint[16];
			MD5Context.Decode(array, block, nShift, 64u);
			MD5Context.FF(ref num, num2, num3, num4, array[0], 7, 3614090360u);
			MD5Context.FF(ref num4, num, num2, num3, array[1], 12, 3905402710u);
			MD5Context.FF(ref num3, num4, num, num2, array[2], 17, 606105819u);
			MD5Context.FF(ref num2, num3, num4, num, array[3], 22, 3250441966u);
			MD5Context.FF(ref num, num2, num3, num4, array[4], 7, 4118548399u);
			MD5Context.FF(ref num4, num, num2, num3, array[5], 12, 1200080426u);
			MD5Context.FF(ref num3, num4, num, num2, array[6], 17, 2821735955u);
			MD5Context.FF(ref num2, num3, num4, num, array[7], 22, 4249261313u);
			MD5Context.FF(ref num, num2, num3, num4, array[8], 7, 1770035416u);
			MD5Context.FF(ref num4, num, num2, num3, array[9], 12, 2336552879u);
			MD5Context.FF(ref num3, num4, num, num2, array[10], 17, 4294925233u);
			MD5Context.FF(ref num2, num3, num4, num, array[11], 22, 2304563134u);
			MD5Context.FF(ref num, num2, num3, num4, array[12], 7, 1804603682u);
			MD5Context.FF(ref num4, num, num2, num3, array[13], 12, 4254626195u);
			MD5Context.FF(ref num3, num4, num, num2, array[14], 17, 2792965006u);
			MD5Context.FF(ref num2, num3, num4, num, array[15], 22, 1236535329u);
			MD5Context.GG(ref num, num2, num3, num4, array[1], 5, 4129170786u);
			MD5Context.GG(ref num4, num, num2, num3, array[6], 9, 3225465664u);
			MD5Context.GG(ref num3, num4, num, num2, array[11], 14, 643717713u);
			MD5Context.GG(ref num2, num3, num4, num, array[0], 20, 3921069994u);
			MD5Context.GG(ref num, num2, num3, num4, array[5], 5, 3593408605u);
			MD5Context.GG(ref num4, num, num2, num3, array[10], 9, 38016083u);
			MD5Context.GG(ref num3, num4, num, num2, array[15], 14, 3634488961u);
			MD5Context.GG(ref num2, num3, num4, num, array[4], 20, 3889429448u);
			MD5Context.GG(ref num, num2, num3, num4, array[9], 5, 568446438u);
			MD5Context.GG(ref num4, num, num2, num3, array[14], 9, 3275163606u);
			MD5Context.GG(ref num3, num4, num, num2, array[3], 14, 4107603335u);
			MD5Context.GG(ref num2, num3, num4, num, array[8], 20, 1163531501u);
			MD5Context.GG(ref num, num2, num3, num4, array[13], 5, 2850285829u);
			MD5Context.GG(ref num4, num, num2, num3, array[2], 9, 4243563512u);
			MD5Context.GG(ref num3, num4, num, num2, array[7], 14, 1735328473u);
			MD5Context.GG(ref num2, num3, num4, num, array[12], 20, 2368359562u);
			MD5Context.HH(ref num, num2, num3, num4, array[5], 4, 4294588738u);
			MD5Context.HH(ref num4, num, num2, num3, array[8], 11, 2272392833u);
			MD5Context.HH(ref num3, num4, num, num2, array[11], 16, 1839030562u);
			MD5Context.HH(ref num2, num3, num4, num, array[14], 23, 4259657740u);
			MD5Context.HH(ref num, num2, num3, num4, array[1], 4, 2763975236u);
			MD5Context.HH(ref num4, num, num2, num3, array[4], 11, 1272893353u);
			MD5Context.HH(ref num3, num4, num, num2, array[7], 16, 4139469664u);
			MD5Context.HH(ref num2, num3, num4, num, array[10], 23, 3200236656u);
			MD5Context.HH(ref num, num2, num3, num4, array[13], 4, 681279174u);
			MD5Context.HH(ref num4, num, num2, num3, array[0], 11, 3936430074u);
			MD5Context.HH(ref num3, num4, num, num2, array[3], 16, 3572445317u);
			MD5Context.HH(ref num2, num3, num4, num, array[6], 23, 76029189u);
			MD5Context.HH(ref num, num2, num3, num4, array[9], 4, 3654602809u);
			MD5Context.HH(ref num4, num, num2, num3, array[12], 11, 3873151461u);
			MD5Context.HH(ref num3, num4, num, num2, array[15], 16, 530742520u);
			MD5Context.HH(ref num2, num3, num4, num, array[2], 23, 3299628645u);
			MD5Context.II(ref num, num2, num3, num4, array[0], 6, 4096336452u);
			MD5Context.II(ref num4, num, num2, num3, array[7], 10, 1126891415u);
			MD5Context.II(ref num3, num4, num, num2, array[14], 15, 2878612391u);
			MD5Context.II(ref num2, num3, num4, num, array[5], 21, 4237533241u);
			MD5Context.II(ref num, num2, num3, num4, array[12], 6, 1700485571u);
			MD5Context.II(ref num4, num, num2, num3, array[3], 10, 2399980690u);
			MD5Context.II(ref num3, num4, num, num2, array[10], 15, 4293915773u);
			MD5Context.II(ref num2, num3, num4, num, array[1], 21, 2240044497u);
			MD5Context.II(ref num, num2, num3, num4, array[8], 6, 1873313359u);
			MD5Context.II(ref num4, num, num2, num3, array[15], 10, 4264355552u);
			MD5Context.II(ref num3, num4, num, num2, array[6], 15, 2734768916u);
			MD5Context.II(ref num2, num3, num4, num, array[13], 21, 1309151649u);
			MD5Context.II(ref num, num2, num3, num4, array[4], 6, 4149444226u);
			MD5Context.II(ref num4, num, num2, num3, array[11], 10, 3174756917u);
			MD5Context.II(ref num3, num4, num, num2, array[2], 15, 718787259u);
			MD5Context.II(ref num2, num3, num4, num, array[9], 21, 3951481745u);
			state[0] += num;
			state[1] += num2;
			state[2] += num3;
			state[3] += num4;
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 0u;
			}
		}
		public void MD5Final(byte[] digest)
		{
			byte[] array = new byte[8];
			MD5Context.Encode(array, this.count, 8u);
			uint num = this.count[0] >> 3 & 63u;
			uint inputLen = (num < 56u) ? (56u - num) : (120u - num);
			this.MD5Update(MD5Context.PADDING, inputLen);
			this.MD5Update(array, 8u);
			MD5Context.Encode(digest, this.state, 16u);
		}
		static MD5Context()
		{
			// Note: this type is marked as 'beforefieldinit'.
			byte[] array = new byte[64];
			array[0] = 128;
			MD5Context.PADDING = array;
		}
	}
}
