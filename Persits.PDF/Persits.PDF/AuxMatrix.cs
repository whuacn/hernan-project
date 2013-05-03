using System;
namespace Persits.PDF
{
	internal class AuxMatrix
	{
		public float[][] m_mat;
		public AuxMatrix()
		{
			this.m_mat = new float[3][];
			this.m_mat[0] = new float[3];
			this.m_mat[1] = new float[3];
			this.m_mat[2] = new float[3];
			this.Init();
		}
		public AuxMatrix(AuxMatrix Mat) : this()
		{
			this.Init(Mat);
		}
		public void Init()
		{
			this.m_mat[0][0] = 1f;
			this.m_mat[0][1] = 0f;
			this.m_mat[0][2] = 0f;
			this.m_mat[1][0] = 0f;
			this.m_mat[1][1] = 1f;
			this.m_mat[1][2] = 0f;
			this.m_mat[2][0] = 0f;
			this.m_mat[2][1] = 0f;
			this.m_mat[2][2] = 1f;
		}
		public void Init(float a, float b, float c, float d, float e, float f)
		{
			this.m_mat[0][0] = a;
			this.m_mat[0][1] = b;
			this.m_mat[0][2] = 0f;
			this.m_mat[1][0] = c;
			this.m_mat[1][1] = d;
			this.m_mat[1][2] = 0f;
			this.m_mat[2][0] = e;
			this.m_mat[2][1] = f;
			this.m_mat[2][2] = 1f;
		}
		public void Init(AuxMatrix Mat)
		{
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					this.m_mat[i][j] = Mat.m_mat[i][j];
				}
			}
		}
		public void Multiply(AuxMatrix Mat)
		{
			AuxMatrix auxMatrix = new AuxMatrix(this);
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					float num = 0f;
					for (int k = 0; k < 3; k++)
					{
						num += auxMatrix.m_mat[i][k] * Mat.m_mat[k][j];
					}
					this.m_mat[i][j] = num;
				}
			}
		}
		public bool Inverse()
		{
			float num = this.m_mat[0][0];
			float num2 = this.m_mat[0][1];
			float num3 = this.m_mat[0][2];
			float num4 = this.m_mat[1][0];
			float num5 = this.m_mat[1][1];
			float num6 = this.m_mat[1][2];
			float num7 = this.m_mat[2][0];
			float num8 = this.m_mat[2][1];
			float num9 = this.m_mat[2][2];
			float num10 = num * (num5 * num9 - num8 * num6) - num2 * (num4 * num9 - num7 * num6) + num3 * (num4 * num8 - num7 * num5);
			if (num10 == 0f)
			{
				return false;
			}
			float num11 = (num5 * num9 - num8 * num6) / num10;
			float num12 = -(num4 * num9 - num7 * num6) / num10;
			float num13 = (num4 * num8 - num7 * num5) / num10;
			float num14 = -(num2 * num9 - num3 * num8) / num10;
			float num15 = (num * num9 - num3 * num7) / num10;
			float num16 = -(num * num8 - num2 * num7) / num10;
			float num17 = (num2 * num6 - num5 * num3) / num10;
			float num18 = -(num * num6 - num3 * num4) / num10;
			float num19 = (num * num5 - num2 * num4) / num10;
			this.m_mat[0][0] = num11;
			this.m_mat[0][1] = num14;
			this.m_mat[0][2] = num17;
			this.m_mat[1][0] = num12;
			this.m_mat[1][1] = num15;
			this.m_mat[1][2] = num18;
			this.m_mat[2][0] = num13;
			this.m_mat[2][1] = num16;
			this.m_mat[2][2] = num19;
			return true;
		}
		public bool IsUnit()
		{
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					if ((j == i && this.m_mat[i][j] != 1f) || (j != i && this.m_mat[i][j] != 0f))
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
