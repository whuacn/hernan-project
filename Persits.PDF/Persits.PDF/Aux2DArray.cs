using System;
namespace Persits.PDF
{
	internal class Aux2DArray
	{
		public int m_nWidth;
		public int m_nHeight;
		private float[][] m_pData;
		public float this[int nRow, int nCol]
		{
			get
			{
				return this.m_pData[nRow - 1][nCol - 1];
			}
			set
			{
				this.m_pData[nRow - 1][nCol - 1] = value;
			}
		}
		public Aux2DArray(int nWidth, int nHeight)
		{
			this.m_pData = new float[nHeight][];
			for (int i = 0; i < nHeight; i++)
			{
				this.m_pData[i] = new float[nWidth];
			}
			this.m_nWidth = nWidth;
			this.m_nHeight = nHeight;
			this.Reset();
		}
		public void Reset()
		{
			for (int i = 0; i < this.m_nHeight; i++)
			{
				for (int j = 0; j < this.m_nWidth; j++)
				{
					this.m_pData[i][j] = 0f;
				}
			}
		}
		public bool IsEmpty()
		{
			return this.m_nWidth == 0 || this.m_nHeight == 0;
		}
	}
}
