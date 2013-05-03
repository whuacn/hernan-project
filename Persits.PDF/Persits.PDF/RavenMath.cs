using System;
namespace Persits.PDF
{
	internal class RavenMath
	{
		internal static double ravenPow(double x, double y)
		{
			return Math.Pow(x, y);
		}
		internal static int ravenRound(double x)
		{
			return (int)Math.Floor(x + 0.5);
		}
		internal static int ravenFloor(double x)
		{
			return (int)Math.Floor(x);
		}
		internal static int ravenCeil(double x)
		{
			return (int)Math.Ceiling(x);
		}
		internal static byte div255(int x)
		{
			return (byte)(x + (x >> 8) + 128 >> 8);
		}
		internal static byte clip255(int x)
		{
			if (x < 0)
			{
				return 0;
			}
			if (x <= 255)
			{
				return (byte)x;
			}
			return 255;
		}
		internal static double ravenSqrt(double x)
		{
			return Math.Sqrt(x);
		}
		internal static double ravenDist(double x0, double y0, double x1, double y1)
		{
			double num = x1 - x0;
			double num2 = y1 - y0;
			return Math.Sqrt(num * num + num2 * num2);
		}
		internal static double ravenAvg(double x, double y)
		{
			return 0.5 * (x + y);
		}
		internal static bool ravenCheckDet(double m11, double m12, double m21, double m22, double epsilon)
		{
			return Math.Abs(m11 * m22 - m12 * m21) >= epsilon;
		}
		internal static double ravenAbs(double x)
		{
			return Math.Abs(x);
		}
	}
}
