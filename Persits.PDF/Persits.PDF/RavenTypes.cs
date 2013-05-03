using System;
namespace Persits.PDF
{
	internal class RavenTypes
	{
		internal static void ravenColorCopy(RavenColor dest, RavenColor src)
		{
			dest[0] = src[0];
			dest[1] = src[1];
			dest[2] = src[2];
		}
		internal static void MemSet(byte[] arr, int offset, byte val, int nLen)
		{
			for (int i = offset; i < offset + nLen; i++)
			{
				arr[i] = val;
			}
		}
		internal static void MemSet(uint[] arr, int offset, uint val, int nLen)
		{
			for (int i = offset; i < offset + nLen; i++)
			{
				arr[i] = val;
			}
		}
		internal static void MemSet(int[] arr, int offset, int val, int nLen)
		{
			for (int i = offset; i < offset + nLen; i++)
			{
				arr[i] = val;
			}
		}
	}
}
