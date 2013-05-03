using System;
namespace Persits.PDF
{
	internal class AuxRGB
	{
		public float r;
		public float g;
		public float b;
		public float c;
		public float m;
		public float y;
		public float k;
		public bool m_bIsSet;
		public AuxRGB()
		{
			this.Reset();
		}
		public AuxRGB(ref AuxRGB rgb)
		{
			this.r = rgb.r;
			this.g = rgb.g;
			this.b = rgb.b;
			this.m_bIsSet = rgb.m_bIsSet;
		}
		public AuxRGB(float rr, float gg, float bb)
		{
			this.r = rr;
			this.g = gg;
			this.b = bb;
			this.m_bIsSet = true;
		}
		public void Reset()
		{
			this.r = (this.g = (this.b = 0f));
			this.c = (this.m = (this.y = (this.k = 0f)));
			this.m_bIsSet = false;
		}
		public void Set(ref AuxRGB rgb)
		{
			this.r = rgb.r;
			this.g = rgb.g;
			this.b = rgb.b;
			this.m_bIsSet = rgb.m_bIsSet;
		}
		public void Set(uint rgb)
		{
			this.b = (float)AuxRGB.GetRValue((ulong)rgb) / 255f;
			this.g = (float)AuxRGB.GetGValue((ulong)rgb) / 255f;
			this.r = (float)AuxRGB.GetBValue((ulong)rgb) / 255f;
			this.m_bIsSet = true;
		}
		public void Set(ushort rgb)
		{
			this.b = (float)AuxRGB.GetRValue((ulong)rgb) / 255f;
			this.g = (float)AuxRGB.GetGValue((ulong)rgb) / 255f;
			this.r = (float)AuxRGB.GetBValue((ulong)rgb) / 255f;
			this.m_bIsSet = true;
		}
		public static byte GetRValue(ulong rgb)
		{
			return (byte)rgb;
		}
		public static byte GetGValue(ulong rgb)
		{
			return (byte)(rgb >> 8);
		}
		public static byte GetBValue(ulong rgb)
		{
			return (byte)(rgb >> 16);
		}
	}
}
