using System;
namespace Persits.PDF
{
	internal class JArithmeticDecoderStats
	{
		internal byte[] cxTab;
		internal int contextSize;
		internal JArithmeticDecoderStats(int contextSizeA)
		{
			this.contextSize = contextSizeA;
			this.cxTab = new byte[this.contextSize];
			this.reset();
		}
		~JArithmeticDecoderStats()
		{
		}
		internal JArithmeticDecoderStats copy()
		{
			JArithmeticDecoderStats jArithmeticDecoderStats = new JArithmeticDecoderStats(this.contextSize);
			Array.Copy(this.cxTab, jArithmeticDecoderStats.cxTab, this.contextSize);
			return jArithmeticDecoderStats;
		}
		internal void reset()
		{
			for (int i = 0; i < this.cxTab.Length; i++)
			{
				this.cxTab[i] = 0;
			}
		}
		internal int getContextSize()
		{
			return this.contextSize;
		}
		internal void copyFrom(JArithmeticDecoderStats stats)
		{
			Array.Copy(stats.cxTab, this.cxTab, this.contextSize);
		}
		internal void setEntry(uint cx, int i, int mps)
		{
			this.cxTab[(int)((UIntPtr)cx)] = (byte)((i << 1) + mps);
		}
	}
}
