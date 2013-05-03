// Type: Persits.PDF.RavenClip
// Assembly: Persits.PDF, Version=2.5.0.19989, Culture=neutral, PublicKeyToken=b8d8c63f1ff142a8
// Assembly location: C:\Users\hernan\Desktop\asppdf_net\Samples\Bin\Persits.PDF.dll

using System.Collections.Generic;

namespace Persits.PDF
{
  internal class RavenClip
  {
    private bool antialias;
    private double xMin;
    private double yMin;
    private double xMax;
    private double yMax;
    private int xMinI;
    private int yMinI;
    private int xMaxI;
    private int yMaxI;
    private List<RavenXPath> paths;
    private List<byte> flags;
    private List<RavenXPathScanner> scanners;
    private int length;
    private int size;

    internal RavenClip(double x0, double y0, double x1, double y1, bool antialiasA)
    {
      this.antialias = antialiasA;
      if (x0 < x1)
      {
        this.xMin = x0;
        this.xMax = x1;
      }
      else
      {
        this.xMin = x1;
        this.xMax = x0;
      }
      if (y0 < y1)
      {
        this.yMin = y0;
        this.yMax = y1;
      }
      else
      {
        this.yMin = y1;
        this.yMax = y0;
      }
      this.xMinI = RavenMath.ravenFloor(this.xMin);
      this.yMinI = RavenMath.ravenFloor(this.yMin);
      this.xMaxI = RavenMath.ravenCeil(this.xMax) - 1;
      this.yMaxI = RavenMath.ravenCeil(this.yMax) - 1;
      this.paths = (List<RavenXPath>) null;
      this.flags = (List<byte>) null;
      this.scanners = (List<RavenXPathScanner>) null;
      this.length = this.size = 0;
    }

    internal RavenClip(RavenClip clip)
    {
      this.antialias = clip.antialias;
      this.xMin = clip.xMin;
      this.yMin = clip.yMin;
      this.xMax = clip.xMax;
      this.yMax = clip.yMax;
      this.xMinI = clip.xMinI;
      this.yMinI = clip.yMinI;
      this.xMaxI = clip.xMaxI;
      this.yMaxI = clip.yMaxI;
      this.length = clip.length;
      this.size = clip.size;
      this.paths = new List<RavenXPath>();
      this.flags = new List<byte>();
      this.scanners = new List<RavenXPathScanner>();
      for (int index = 0; index < this.length; ++index)
      {
        this.paths.Add(clip.paths[index].copy());
        this.flags.Add(clip.flags[index]);
        int clipYMin;
        int clipYMax;
        if (this.antialias)
        {
          clipYMin = this.yMinI * RavenConstants.ravenAASize;
          clipYMax = (this.yMaxI + 1) * RavenConstants.ravenAASize - 1;
        }
        else
        {
          clipYMin = this.yMinI;
          clipYMax = this.yMaxI;
        }
        this.scanners.Add(new RavenXPathScanner(this.paths[index], ((int) this.flags[index] & (int) RavenConstants.ravenClipEO) != 0, clipYMin, clipYMax));
      }
    }

    internal RavenClip copy()
    {
      return new RavenClip(this);
    }

    internal void resetToRect(double x0, double y0, double x1, double y1)
    {
      this.paths.Clear();
      this.flags.Clear();
      this.scanners.Clear();
      this.paths = (List<RavenXPath>) null;
      this.flags = (List<byte>) null;
      this.scanners = (List<RavenXPathScanner>) null;
      this.length = this.size = 0;
      if (x0 < x1)
      {
        this.xMin = x0;
        this.xMax = x1;
      }
      else
      {
        this.xMin = x1;
        this.xMax = x0;
      }
      if (y0 < y1)
      {
        this.yMin = y0;
        this.yMax = y1;
      }
      else
      {
        this.yMin = y1;
        this.yMax = y0;
      }
      this.xMinI = RavenMath.ravenFloor(this.xMin);
      this.yMinI = RavenMath.ravenFloor(this.yMin);
      this.xMaxI = RavenMath.ravenCeil(this.xMax) - 1;
      this.yMaxI = RavenMath.ravenCeil(this.yMax) - 1;
    }

    private RavenError clipToRect(double x0, double y0, double x1, double y1)
    {
      if (x0 < x1)
      {
        if (x0 > this.xMin)
        {
          this.xMin = x0;
          this.xMinI = RavenMath.ravenFloor(this.xMin);
        }
        if (x1 < this.xMax)
        {
          this.xMax = x1;
          this.xMaxI = RavenMath.ravenCeil(this.xMax) - 1;
        }
      }
      else
      {
        if (x1 > this.xMin)
        {
          this.xMin = x1;
          this.xMinI = RavenMath.ravenFloor(this.xMin);
        }
        if (x0 < this.xMax)
        {
          this.xMax = x0;
          this.xMaxI = RavenMath.ravenCeil(this.xMax) - 1;
        }
      }
      if (y0 < y1)
      {
        if (y0 > this.yMin)
        {
          this.yMin = y0;
          this.yMinI = RavenMath.ravenFloor(this.yMin);
        }
        if (y1 < this.yMax)
        {
          this.yMax = y1;
          this.yMaxI = RavenMath.ravenCeil(this.yMax) - 1;
        }
      }
      else
      {
        if (y1 > this.yMin)
        {
          this.yMin = y1;
          this.yMinI = RavenMath.ravenFloor(this.yMin);
        }
        if (y0 < this.yMax)
        {
          this.yMax = y0;
          this.yMaxI = RavenMath.ravenCeil(this.yMax) - 1;
        }
      }
      return RavenError.ravenOk;
    }

    internal RavenError clipToPath(RavenPath path, double[] matrix, double flatness, bool eo)
    {
      RavenXPath xPathA = new RavenXPath(path, matrix, flatness, true);
      if (xPathA.length == 0)
      {
        this.xMax = this.xMin - 1.0;
        this.yMax = this.yMin - 1.0;
        this.xMaxI = RavenMath.ravenCeil(this.xMax) - 1;
        this.yMaxI = RavenMath.ravenCeil(this.yMax) - 1;
      }
      else if (xPathA.length == 4 && (xPathA.segs[0].x0 == xPathA.segs[0].x1 && xPathA.segs[0].x0 == xPathA.segs[1].x0 && (xPathA.segs[0].x0 == xPathA.segs[3].x1 && xPathA.segs[2].x0 == xPathA.segs[2].x1) && (xPathA.segs[2].x0 == xPathA.segs[1].x1 && xPathA.segs[2].x0 == xPathA.segs[3].x0 && (xPathA.segs[1].y0 == xPathA.segs[1].y1 && xPathA.segs[1].y0 == xPathA.segs[0].y1)) && (xPathA.segs[1].y0 == xPathA.segs[2].y0 && xPathA.segs[3].y0 == xPathA.segs[3].y1 && (xPathA.segs[3].y0 == xPathA.segs[0].y0 && xPathA.segs[3].y0 == xPathA.segs[2].y1)) || xPathA.segs[0].y0 == xPathA.segs[0].y1 && xPathA.segs[0].y0 == xPathA.segs[1].y0 && (xPathA.segs[0].y0 == xPathA.segs[3].y1 && xPathA.segs[2].y0 == xPathA.segs[2].y1) && (xPathA.segs[2].y0 == xPathA.segs[1].y1 && xPathA.segs[2].y0 == xPathA.segs[3].y0 && (xPathA.segs[1].x0 == xPathA.segs[1].x1 && xPathA.segs[1].x0 == xPathA.segs[0].x1)) && (xPathA.segs[1].x0 == xPathA.segs[2].x0 && xPathA.segs[3].x0 == xPathA.segs[3].x1 && (xPathA.segs[3].x0 == xPathA.segs[0].x0 && xPathA.segs[3].x0 == xPathA.segs[2].x1))))
      {
        int num = (int) this.clipToRect(xPathA.segs[0].x0, xPathA.segs[0].y0, xPathA.segs[2].x0, xPathA.segs[2].y0);
      }
      else
      {
        if (this.antialias)
          xPathA.aaScale();
        xPathA.sort();
        this.paths.Add(xPathA);
        this.flags.Add(eo ? RavenConstants.ravenClipEO : (byte) 0);
        int clipYMin;
        int clipYMax;
        if (this.antialias)
        {
          clipYMin = this.yMinI * RavenConstants.ravenAASize;
          clipYMax = (this.yMaxI + 1) * RavenConstants.ravenAASize - 1;
        }
        else
        {
          clipYMin = this.yMinI;
          clipYMax = this.yMaxI;
        }
        this.scanners.Add(new RavenXPathScanner(xPathA, eo, clipYMin, clipYMax));
        ++this.length;
      }
      return RavenError.ravenOk;
    }

    internal bool test(int x, int y)
    {
      if (x < this.xMinI || x > this.xMaxI || (y < this.yMinI || y > this.yMaxI))
        return false;
      if (this.antialias)
      {
        for (int index = 0; index < this.length; ++index)
        {
          if (!this.scanners[index].test(x * RavenConstants.ravenAASize, y * RavenConstants.ravenAASize))
            return false;
        }
      }
      else
      {
        for (int index = 0; index < this.length; ++index)
        {
          if (!this.scanners[index].test(x, y))
            return false;
        }
      }
      return true;
    }

    internal RavenClipResult testRect(int rectXMin, int rectYMin, int rectXMax, int rectYMax)
    {
      if ((double) (rectXMax + 1) <= this.xMin || (double) rectXMin >= this.xMax || ((double) (rectYMax + 1) <= this.yMin || (double) rectYMin >= this.yMax))
        return RavenClipResult.ravenClipAllOutside;
      return (double) rectXMin >= this.xMin && (double) (rectXMax + 1) <= this.xMax && ((double) rectYMin >= this.yMin && (double) (rectYMax + 1) <= this.yMax) && this.length == 0 ? RavenClipResult.ravenClipAllInside : RavenClipResult.ravenClipPartial;
    }

    internal RavenClipResult testSpan(int spanXMin, int spanXMax, int spanY)
    {
      if ((double) (spanXMax + 1) <= this.xMin || (double) spanXMin >= this.xMax || ((double) (spanY + 1) <= this.yMin || (double) spanY >= this.yMax))
        return RavenClipResult.ravenClipAllOutside;
      if ((double) spanXMin < this.xMin || (double) (spanXMax + 1) > this.xMax || ((double) spanY < this.yMin || (double) (spanY + 1) > this.yMax))
        return RavenClipResult.ravenClipPartial;
      if (this.antialias)
      {
        for (int index = 0; index < this.length; ++index)
        {
          if (!this.scanners[index].testSpan(spanXMin * RavenConstants.ravenAASize, spanXMax * RavenConstants.ravenAASize + (RavenConstants.ravenAASize - 1), spanY * RavenConstants.ravenAASize))
            return RavenClipResult.ravenClipPartial;
        }
      }
      else
      {
        for (int index = 0; index < this.length; ++index)
        {
          if (!this.scanners[index].testSpan(spanXMin, spanXMax, spanY))
            return RavenClipResult.ravenClipPartial;
        }
      }
      return RavenClipResult.ravenClipAllInside;
    }

    internal void clipAALine(RavenBitmap aaBuf, ref int x0, ref int x1, int y)
    {
      byte[] dataPtr = aaBuf.getDataPtr();
      int num1 = x0 * RavenConstants.ravenAASize;
      int num2 = RavenMath.ravenFloor(this.xMin * (double) RavenConstants.ravenAASize);
      if (num2 > aaBuf.getWidth())
        num2 = aaBuf.getWidth();
      if (num1 < num2)
      {
        int num3 = num1 & -8;
        for (int index1 = 0; index1 < RavenConstants.ravenAASize; ++index1)
        {
          int index2 = index1 * aaBuf.getRowSize() + (num3 >> 3);
          int num4 = num3;
          while (num4 + 7 < num2)
          {
            dataPtr[index2++] = (byte) 0;
            num4 += 8;
          }
          if (num4 < num2)
            dataPtr[index2] &= (byte) ((int) byte.MaxValue >> (num2 & 7));
        }
        x0 = RavenMath.ravenFloor(this.xMin);
      }
      int num5 = RavenMath.ravenFloor(this.xMax * (double) RavenConstants.ravenAASize) + 1;
      if (num5 < 0)
        num5 = 0;
      int num6 = (x1 + 1) * RavenConstants.ravenAASize;
      if (num5 < num6)
      {
        for (int index1 = 0; index1 < RavenConstants.ravenAASize; ++index1)
        {
          int index2 = index1 * aaBuf.getRowSize() + (num5 >> 3);
          int num3 = num5;
          if ((num3 & 7) != 0)
          {
            dataPtr[index2] &= (byte) (65280 >> (num3 & 7));
            num3 = (num3 & -8) + 8;
            ++index2;
          }
          while (num3 < num6)
          {
            dataPtr[index2++] = (byte) 0;
            num3 += 8;
          }
        }
        x1 = RavenMath.ravenFloor(this.xMax);
      }
      for (int index = 0; index < this.length; ++index)
        this.scanners[index].clipAALine(aaBuf, ref x0, ref x1, y);
    }

    internal int getNumPaths()
    {
      return this.length;
    }

    internal int getXMinI()
    {
      return this.xMinI;
    }

    internal int getXMaxI()
    {
      return this.xMaxI;
    }

    internal int getYMinI()
    {
      return this.yMinI;
    }

    internal int getYMaxI()
    {
      return this.yMaxI;
    }

    internal double getXMin()
    {
      return this.xMin;
    }

    internal double getXMax()
    {
      return this.xMax;
    }

    internal double getYMin()
    {
      return this.yMin;
    }

    internal double getYMax()
    {
      return this.yMax;
    }
  }
}
