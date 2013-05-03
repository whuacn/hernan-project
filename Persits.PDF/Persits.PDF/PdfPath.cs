using System;
namespace Persits.PDF
{
	internal class PdfPath
	{
		private bool m_bJustMoved;
		private double m_fFirstX;
		private double m_fFirstY;
		private PdfSubpath[] m_subpaths;
		private int m_nN;
		private int m_nSize;
		internal PdfPath()
		{
			this.m_bJustMoved = false;
			this.m_nSize = 16;
			this.m_nN = 0;
			this.m_fFirstX = (this.m_fFirstY = 0.0);
			this.m_subpaths = new PdfSubpath[this.m_nSize];
		}
		private PdfPath(bool justMoved1, double firstX1, double firstY1, PdfSubpath[] subpaths1, int n1, int size1)
		{
			this.m_bJustMoved = justMoved1;
			this.m_fFirstX = firstX1;
			this.m_fFirstY = firstY1;
			this.m_nSize = size1;
			this.m_nN = n1;
			this.m_subpaths = new PdfSubpath[this.m_nSize];
			for (int i = 0; i < this.m_nN; i++)
			{
				this.m_subpaths[i] = subpaths1[i].copy();
			}
		}
		internal PdfPath copy()
		{
			return new PdfPath(this.m_bJustMoved, this.m_fFirstX, this.m_fFirstY, this.m_subpaths, this.m_nN, this.m_nSize);
		}
		internal bool isCurPt()
		{
			return this.m_nN > 0 || this.m_bJustMoved;
		}
		internal bool isPath()
		{
			return this.m_nN > 0;
		}
		internal int getNumSubpaths()
		{
			return this.m_nN;
		}
		internal PdfSubpath getSubpath(int i)
		{
			return this.m_subpaths[i];
		}
		internal double getLastX()
		{
			return this.m_subpaths[this.m_nN - 1].getLastX();
		}
		internal double getLastY()
		{
			return this.m_subpaths[this.m_nN - 1].getLastY();
		}
		internal void moveTo(double x, double y)
		{
			this.m_bJustMoved = true;
			this.m_fFirstX = x;
			this.m_fFirstY = y;
		}
		internal void lineTo(double x, double y)
		{
			if (this.m_bJustMoved)
			{
				if (this.m_nN >= this.m_nSize)
				{
					this.m_nSize += 16;
					PdfSubpath[] array = new PdfSubpath[this.m_nSize];
					Array.Copy(this.m_subpaths, array, this.m_subpaths.Length);
					this.m_subpaths = array;
				}
				this.m_subpaths[this.m_nN] = new PdfSubpath(this.m_fFirstX, this.m_fFirstY);
				this.m_nN++;
				this.m_bJustMoved = false;
			}
			this.m_subpaths[this.m_nN - 1].lineTo(x, y);
		}
		internal void curveTo(double x1, double y1, double x2, double y2, double x3, double y3)
		{
			if (this.m_bJustMoved)
			{
				if (this.m_nN >= this.m_nSize)
				{
					this.m_nSize += 16;
					PdfSubpath[] array = new PdfSubpath[this.m_nSize];
					Array.Copy(this.m_subpaths, array, this.m_subpaths.Length);
					this.m_subpaths = array;
				}
				this.m_subpaths[this.m_nN] = new PdfSubpath(this.m_fFirstX, this.m_fFirstY);
				this.m_nN++;
				this.m_bJustMoved = false;
			}
			this.m_subpaths[this.m_nN - 1].curveTo(x1, y1, x2, y2, x3, y3);
		}
		internal void close()
		{
			if (this.m_bJustMoved)
			{
				if (this.m_nN >= this.m_nSize)
				{
					this.m_nSize += 16;
					PdfSubpath[] array = new PdfSubpath[this.m_nSize];
					Array.Copy(this.m_subpaths, array, this.m_subpaths.Length);
					this.m_subpaths = array;
				}
				this.m_subpaths[this.m_nN] = new PdfSubpath(this.m_fFirstX, this.m_fFirstY);
				this.m_nN++;
				this.m_bJustMoved = false;
			}
			this.m_subpaths[this.m_nN - 1].close();
		}
		internal void append(PdfPath path)
		{
			if (this.m_nN + path.m_nN > this.m_nSize)
			{
				this.m_nSize = this.m_nN + path.m_nN;
				PdfSubpath[] array = new PdfSubpath[this.m_nSize];
				Array.Copy(this.m_subpaths, array, this.m_subpaths.Length);
				this.m_subpaths = array;
			}
			for (int i = 0; i < path.m_nN; i++)
			{
				this.m_subpaths[this.m_nN++] = path.m_subpaths[i].copy();
			}
			this.m_bJustMoved = false;
		}
		internal void offset(double dx, double dy)
		{
			for (int i = 0; i < this.m_nN; i++)
			{
				this.m_subpaths[i].offset(dx, dy);
			}
		}
	}
}
