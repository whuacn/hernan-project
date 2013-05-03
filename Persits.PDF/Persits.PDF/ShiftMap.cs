using System;
namespace Persits.PDF
{
	internal class ShiftMap
	{
		public int m_nWidth;
		public int m_nHeight;
		public PngPoint[] m_arrXY;
		public ShiftMap(int Width, int Height, int[] Points)
		{
			this.m_nWidth = Width;
			this.m_nHeight = Height;
			this.m_arrXY = new PngPoint[32];
			for (int i = 0; i < Points.Length / 2; i++)
			{
				this.m_arrXY[i].x = Points[2 * i];
				this.m_arrXY[i].y = Points[2 * i + 1];
			}
		}
	}
}
