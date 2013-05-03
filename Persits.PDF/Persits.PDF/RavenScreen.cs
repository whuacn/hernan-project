using System;
namespace Persits.PDF
{
	internal class RavenScreen
	{
		private static RavenScreenParams defaultParams = new RavenScreenParams(RavenScreenType.ravenScreenDispersed, 2, 1.0, 0.0, 1.0);
		internal byte[] mat;
		internal int size;
		internal int sizeM1;
		internal int log2Size;
		internal byte minVal;
		internal byte maxVal;
		internal RavenScreen(RavenScreenParams parms)
		{
			if (parms == null)
			{
				parms = RavenScreen.defaultParams;
			}
			this.size = 2;
			this.log2Size = 1;
			while (this.size < parms.size)
			{
				this.size <<= 1;
				this.log2Size++;
			}
			switch (parms.type)
			{
			case RavenScreenType.ravenScreenDispersed:
				this.mat = new byte[this.size * this.size];
				this.buildDispersedMatrix(this.size / 2, this.size / 2, 1, this.size / 2, 1);
				break;
			case RavenScreenType.ravenScreenClustered:
				this.mat = new byte[this.size * this.size];
				this.buildClusteredMatrix();
				break;
			}
			this.sizeM1 = this.size - 1;
			this.minVal = 255;
			this.maxVal = 0;
			int num = RavenMath.ravenRound(255.0 * parms.blackThreshold);
			if (num < 1)
			{
				num = 1;
			}
			int num2 = RavenMath.ravenRound(255.0 * parms.whiteThreshold);
			if (num2 > 255)
			{
				num2 = 255;
			}
			for (int i = 0; i < this.size * this.size; i++)
			{
				byte b = (byte)RavenMath.ravenRound(255.0 * RavenMath.ravenPow((double)this.mat[i] / 255.0, parms.gamma));
				if ((int)b < num)
				{
					b = (byte)num;
				}
				else
				{
					if ((int)b >= num2)
					{
						b = (byte)num2;
					}
				}
				this.mat[i] = b;
				if (b < this.minVal)
				{
					this.minVal = b;
				}
				else
				{
					if (b > this.maxVal)
					{
						this.maxVal = b;
					}
				}
			}
		}
		internal RavenScreen(RavenScreen screen)
		{
			this.size = screen.size;
			this.sizeM1 = screen.sizeM1;
			this.log2Size = screen.log2Size;
			this.mat = new byte[this.size];
			Array.Copy(screen.mat, this.mat, this.size);
			this.minVal = screen.minVal;
			this.maxVal = screen.maxVal;
		}
		internal RavenScreen copy()
		{
			return new RavenScreen(this);
		}
		private void buildDispersedMatrix(int i, int j, int val, int delta, int offset)
		{
			if (delta == 0)
			{
				this.mat[(i << this.log2Size) + j] = (byte)(1 + 254 * (val - 1) / (this.size * this.size - 1));
				return;
			}
			this.buildDispersedMatrix(i, j, val, delta / 2, 4 * offset);
			this.buildDispersedMatrix((i + delta) % this.size, (j + delta) % this.size, val + offset, delta / 2, 4 * offset);
			this.buildDispersedMatrix((i + delta) % this.size, j, val + 2 * offset, delta / 2, 4 * offset);
			this.buildDispersedMatrix((i + 2 * delta) % this.size, (j + delta) % this.size, val + 3 * offset, delta / 2, 4 * offset);
		}
		private void buildClusteredMatrix()
		{
			int num = this.size >> 1;
			for (int i = 0; i < this.size; i++)
			{
				for (int j = 0; j < this.size; j++)
				{
					this.mat[(i << this.log2Size) + j] = 0;
				}
			}
			double[] array = new double[this.size * num];
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num; j++)
				{
					double num2;
					double num3;
					if (j + i < num - 1)
					{
						num2 = (double)j + 0.5 - 0.0;
						num3 = (double)i + 0.5 - 0.0;
					}
					else
					{
						num2 = (double)j + 0.5 - (double)num;
						num3 = (double)i + 0.5 - (double)num;
					}
					array[i * num + j] = num2 * num2 + num3 * num3;
				}
			}
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num; j++)
				{
					double num2;
					double num3;
					if (j < i)
					{
						num2 = (double)j + 0.5 - 0.0;
						num3 = (double)i + 0.5 - (double)num;
					}
					else
					{
						num2 = (double)j + 0.5 - (double)num;
						num3 = (double)i + 0.5 - 0.0;
					}
					array[(num + i) * num + j] = num2 * num2 + num3 * num3;
				}
			}
			int num5;
			int num4 = num5 = 0;
			for (int k = 0; k < this.size * num; k++)
			{
				double num6 = -1.0;
				for (int i = 0; i < this.size; i++)
				{
					for (int j = 0; j < num; j++)
					{
						if (this.mat[(i << this.log2Size) + j] == 0 && array[i * num + j] > num6)
						{
							num5 = j;
							num4 = i;
							num6 = array[num4 * num + num5];
						}
					}
				}
				byte b = (byte)(1 + 254 * (2 * k) / (2 * this.size * num - 1));
				this.mat[(num4 << this.log2Size) + num5] = b;
				b = (byte)(1 + 254 * (2 * k + 1) / (2 * this.size * num - 1));
				if (num4 < num)
				{
					this.mat[(num4 + num << this.log2Size) + num5 + num] = b;
				}
				else
				{
					this.mat[(num4 - num << this.log2Size) + num5 + num] = b;
				}
			}
		}
		private int distance(int x0, int y0, int x1, int y1)
		{
			int num = Math.Abs(x0 - x1);
			int num2 = this.size - num;
			int num3 = (num < num2) ? num : num2;
			int num4 = Math.Abs(y0 - y1);
			int num5 = this.size - num4;
			int num6 = (num4 < num5) ? num4 : num5;
			return num3 * num3 + num6 * num6;
		}
		internal int test(int x, int y, byte value)
		{
			int num = x & this.sizeM1;
			int num2 = y & this.sizeM1;
			if (value >= this.mat[(num2 << this.log2Size) + num])
			{
				return 1;
			}
			return 0;
		}
	}
}
