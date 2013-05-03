using System;
using System.Collections.Generic;
namespace Persits.PDF
{
	internal class AuxSparce2DArray
	{
		private Dictionary<AuxPair, float> m_mat = new Dictionary<AuxPair, float>();
		public float this[int nRow, int nCol]
		{
			get
			{
				float result = 0f;
				this.m_mat.TryGetValue(new AuxPair(nRow, nCol), out result);
				return result;
			}
			set
			{
				this.m_mat[new AuxPair(nRow, nCol)] = value;
			}
		}
		public AuxSparce2DArray(int a, int b)
		{
		}
		public void Reset()
		{
			this.m_mat.Clear();
		}
	}
}
