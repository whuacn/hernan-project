using System;
namespace Persits.PDF
{
	internal class AuxRect
	{
		public float m_x;
		public float m_y;
		public float m_width;
		public float m_height;
		public float m_r;
		public float m_g;
		public float m_b;
		public string m_anchor;
		public AuxRect(float x, float y, float width, float height, float r, float g, float b)
		{
			this.m_x = x;
			this.m_y = y;
			this.m_width = width;
			this.m_height = height;
			this.m_r = r;
			this.m_g = g;
			this.m_b = b;
		}
	}
}
