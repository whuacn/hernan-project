using System;
namespace Persits.PDF
{
	internal class IccProfile
	{
		private const double D50_X = 0.9642;
		private const double D50_Y = 1.0;
		private const double D50_Z = 0.8249;
		private int m_nInputChannels;
		private int m_nOutputChannels;
		private int m_nGridPoints;
		private double[] m_nEMatrix = new double[9];
		private int m_nInputEntries;
		private double[] m_fCacheCMYK = new double[4];
		private double[] m_fCacheRGB = new double[3];
		private ushort[] m_arrInputTable;
		private int m_nCLUTSize;
		private float[] m_arrLab;
		private int[] m_arrIncrements = new int[10];
		private int[] m_arrCubeOffsets = new int[1024];
		private double[] m_arrRGB = new double[3];
		internal void LoadProfileFromResource()
		{
			byte[] uSWebUncoated_icc = AspPDF.USWebUncoated_icc;
			this.m_nInputChannels = BitConverter.ToInt32(uSWebUncoated_icc, 0);
			this.m_nOutputChannels = BitConverter.ToInt32(uSWebUncoated_icc, 4);
			this.m_nGridPoints = BitConverter.ToInt32(uSWebUncoated_icc, 8);
			this.m_nInputEntries = BitConverter.ToInt32(uSWebUncoated_icc, 12);
			this.m_arrInputTable = new ushort[this.m_nInputEntries * this.m_nInputChannels];
			for (int i = 0; i < this.m_arrInputTable.Length; i++)
			{
				this.m_arrInputTable[i] = BitConverter.ToUInt16(uSWebUncoated_icc, 16 + i * 2);
			}
			this.m_nCLUTSize = (int)((double)this.m_nOutputChannels * Math.Pow((double)this.m_nGridPoints, (double)this.m_nInputChannels));
			this.m_arrLab = new float[this.m_nCLUTSize];
			int num = 16 + this.m_nInputEntries * this.m_nInputChannels * 2;
			for (int j = 0; j < this.m_arrLab.Length; j++)
			{
				this.m_arrLab[j] = BitConverter.ToSingle(uSWebUncoated_icc, num + j * 4);
			}
			int k = this.m_nInputChannels - 1;
			this.m_arrIncrements[k--] = this.m_nOutputChannels;
			while (k >= 0)
			{
				this.m_arrIncrements[k] = this.m_arrIncrements[k + 1] * this.m_nGridPoints;
				k--;
			}
			this.m_arrCubeOffsets[0] = 0;
			int num2 = 1;
			for (int l = 0; l < this.m_nInputChannels; l++)
			{
				for (k = 0; k < num2; k++)
				{
					this.m_arrCubeOffsets[num2 + k] = this.m_arrCubeOffsets[k] + this.m_arrIncrements[l];
				}
				num2 *= 2;
			}
			this.m_fCacheCMYK[0] = (this.m_fCacheCMYK[1] = (this.m_fCacheCMYK[2] = (this.m_fCacheCMYK[3] = 0.0)));
			this.m_fCacheRGB[0] = (double)this.m_arrLab[0];
			this.m_fCacheRGB[1] = (double)this.m_arrLab[1];
			this.m_fCacheRGB[2] = (double)this.m_arrLab[2];
		}
		private static void Lab2XYZ(double[] outv, double[] inv)
		{
			double num = inv[0];
			double num2 = inv[1];
			double num3 = inv[2];
			double num4;
			double num5;
			if (num > 8.0)
			{
				num4 = (num + 16.0) / 116.0;
				num5 = Math.Pow(num4, 3.0);
			}
			else
			{
				num5 = num / 903.2963058;
				num4 = 7.787036979 * num5 + 0.13793103448275862;
			}
			double num6 = num2 / 500.0 + num4;
			double num7;
			if (num6 > 0.20689655172413793)
			{
				num7 = Math.Pow(num6, 3.0);
			}
			else
			{
				num7 = (num6 - 0.13793103448275862) / 7.787036979;
			}
			double num8 = num4 - num3 / 200.0;
			double num9;
			if (num8 > 0.20689655172413793)
			{
				num9 = Math.Pow(num8, 3.0);
			}
			else
			{
				num9 = (num8 - 0.13793103448275862) / 7.787036979;
			}
			outv[0] = num7 * 0.9642;
			outv[1] = num5 * 1.0;
			outv[2] = num9 * 0.8249;
		}
		private static void RGBp_RGB(double[] outv, double[] inv)
		{
			outv[0] = 1.055 * IccProfile.ppow(inv[0], 0.41670000553131104) - 0.055;
			outv[1] = 1.055 * IccProfile.ppow(inv[1], 0.41670000553131104) - 0.055;
			outv[2] = 1.055 * IccProfile.ppow(inv[2], 0.41670000553131104) - 0.055;
		}
		private static void XYZp_RGBp(double[] outv, double[] inv)
		{
			double num = 3.13360257102309 * inv[0] + -1.6168214013565443 * inv[1] + -0.490742404412824 * inv[2];
			double num2 = -0.9786503158825 * inv[0] + 1.9160610041253281 * inv[1] + 0.033512902048440089 * inv[2];
			double num3 = 0.072076557813989556 * inv[0] + -0.2290655454722216 * inv[1] + 1.4053594967545651 * inv[2];
			outv[0] = num;
			outv[1] = num2;
			outv[2] = num3;
		}
		private static void Labp_RGBp(double[] outv, double[] inv)
		{
			IccProfile.Lab2XYZ(outv, inv);
			IccProfile.XYZp_RGBp(outv, outv);
		}
		private static void Lab_RGB(double[] outv, double[] inv)
		{
			IccProfile.Labp_RGBp(outv, inv);
			IccProfile.RGBp_RGB(outv, outv);
		}
		internal void CMYK2RGB(double[] pCMYK, double[] pRGB)
		{
			if (pCMYK[0] == this.m_fCacheCMYK[0] && pCMYK[1] == this.m_fCacheCMYK[1] && pCMYK[2] == this.m_fCacheCMYK[2] && pCMYK[3] == this.m_fCacheCMYK[3])
			{
				Array.Copy(this.m_fCacheRGB, pRGB, 3);
				return;
			}
			Array.Copy(pCMYK, this.m_fCacheCMYK, 4);
			this.ConvertViaClut(pCMYK, this.m_arrRGB);
			for (int i = 0; i < 3; i++)
			{
				if (this.m_arrRGB[i] < 0.0)
				{
					this.m_arrRGB[i] = 0.0;
				}
				if (this.m_arrRGB[i] > 1.0)
				{
					this.m_arrRGB[i] = 1.0;
				}
				pRGB[i] = this.m_arrRGB[i];
			}
			Array.Copy(pRGB, this.m_fCacheRGB, 3);
		}
		private void ConvertViaClut(double[] pIn, double[] pOut)
		{
			double[] array = new double[15];
			double[] array2 = new double[256];
			double[] array3 = array2;
			int num = 0;
			double num2 = (double)(this.m_nGridPoints - 1);
			int num3 = this.m_nGridPoints - 2;
			for (int i = 0; i < this.m_nInputChannels; i++)
			{
				double num4 = pIn[i] * num2;
				if (num4 < 0.0)
				{
					num4 = 0.0;
				}
				else
				{
					if (num4 > num2)
					{
						num4 = num2;
					}
				}
				int num5 = (int)Math.Floor(num4);
				if (num5 > num3)
				{
					num5 = num3;
				}
				array[i] = num4 - (double)((float)num5);
				num += num5 * this.m_arrIncrements[i];
			}
			int num6 = 1;
			array3[0] = 1.0;
			for (int i = 0; i < this.m_nInputChannels; i++)
			{
				for (int j = 0; j < num6; j++)
				{
					array3[num6 + j] = array3[j] * array[i];
					array3[j] *= 1.0 - array[i];
				}
				num6 *= 2;
			}
			double num7 = array3[0];
			int num8 = num + this.m_arrCubeOffsets[0];
			for (int k = 0; k < this.m_nOutputChannels; k++)
			{
				pOut[k] = num7 * (double)this.m_arrLab[num8 + k];
			}
			for (int j = 1; j < 1 << this.m_nInputChannels; j++)
			{
				num7 = array3[j];
				num8 = num + this.m_arrCubeOffsets[j];
				for (int k = 0; k < this.m_nOutputChannels; k++)
				{
					pOut[k] += num7 * (double)this.m_arrLab[num8 + k];
				}
			}
		}
		private static double ppow(double num, double p)
		{
			if (num < 0.0)
			{
				return -Math.Pow(-num, p);
			}
			return Math.Pow(num, p);
		}
	}
}
