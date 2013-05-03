using System;
using System.Collections.Generic;
namespace Persits.PDF
{
	internal class PdfSubpath
	{
		private List<double> m_pX = new List<double>();
		private List<double> m_pY = new List<double>();
		private List<bool> m_pCurve = new List<bool>();
		private int m_nN;
		private bool m_bClosed;
		internal PdfSubpath(double x1, double y1)
		{
			this.m_nN = 1;
			this.m_pX.Add(x1);
			this.m_pY.Add(y1);
			this.m_pCurve.Add(false);
			this.m_bClosed = false;
		}
		internal PdfSubpath(PdfSubpath subpath)
		{
			this.m_nN = subpath.m_nN;
			this.m_pX.AddRange(subpath.m_pX);
			this.m_pY.AddRange(subpath.m_pY);
			this.m_pCurve.AddRange(subpath.m_pCurve);
			this.m_bClosed = subpath.m_bClosed;
		}
		internal void lineTo(double x1, double y1)
		{
			this.m_pX.Add(x1);
			this.m_pY.Add(y1);
			this.m_pCurve.Add(false);
			this.m_nN++;
		}
		internal void curveTo(double x1, double y1, double x2, double y2, double x3, double y3)
		{
			this.m_pX.Add(x1);
			this.m_pX.Add(x2);
			this.m_pX.Add(x3);
			this.m_pY.Add(y1);
			this.m_pY.Add(y2);
			this.m_pY.Add(y3);
			this.m_pCurve.Add(true);
			this.m_pCurve.Add(true);
			this.m_pCurve.Add(false);
			this.m_nN += 3;
		}
		internal void close()
		{
			if (this.m_pX[this.m_nN - 1] != this.m_pX[0] || this.m_pY[this.m_nN - 1] != this.m_pY[0] || this.m_nN == 1)
			{
				this.lineTo(this.m_pX[0], this.m_pY[0]);
			}
			this.m_bClosed = true;
		}
		internal void offset(double dx, double dy)
		{
			for (int i = 0; i < this.m_nN; i++)
			{
				List<double> pX;
				int index;
				(pX = this.m_pX)[index = i] = pX[index] + dx;
				List<double> pY;
				int index2;
				(pY = this.m_pY)[index2 = i] = pY[index2] + dy;
			}
		}
		internal PdfSubpath copy()
		{
			return new PdfSubpath(this);
		}
		internal int getNumPoints()
		{
			return this.m_nN;
		}
		internal double getX(int i)
		{
			return this.m_pX[i];
		}
		internal double getY(int i)
		{
			return this.m_pY[i];
		}
		internal bool getCurve(int i)
		{
			return this.m_pCurve[i];
		}
		internal double getLastX()
		{
			return this.m_pX[this.m_nN - 1];
		}
		internal double getLastY()
		{
			return this.m_pY[this.m_nN - 1];
		}
		internal bool isClosed()
		{
			return this.m_bClosed;
		}
	}
}
