// Type: Persits.PDF.RavenPath
// Assembly: Persits.PDF, Version=2.5.0.19989, Culture=neutral, PublicKeyToken=b8d8c63f1ff142a8
// Assembly location: C:\Users\hernan\Desktop\asppdf_net\Samples\Bin\Persits.PDF.dll

using System.Collections.Generic;

namespace Persits.PDF
{
  internal class RavenPath
  {
    internal const byte ravenPathFirst = (byte) 1;
    internal const byte ravenPathLast = (byte) 2;
    internal const byte ravenPathClosed = (byte) 4;
    internal const byte ravenPathCurve = (byte) 8;
    internal List<RavenPathPoint> pts;
    internal List<byte> flags;
    private int curSubpath;
    internal List<RavenPathHint> hints;

    internal int length
    {
      get
      {
        return this.pts.Count;
      }
    }

    internal RavenPath()
    {
      this.pts = new List<RavenPathPoint>();
      this.flags = new List<byte>();
      this.curSubpath = 0;
      this.hints = (List<RavenPathHint>) null;
    }

    internal RavenPath(RavenPath path)
    {
      this.pts.AddRange((IEnumerable<RavenPathPoint>) path.pts);
      this.flags.AddRange((IEnumerable<byte>) path.flags);
      this.curSubpath = path.curSubpath;
      if (path.hints != null)
      {
        this.hints = new List<RavenPathHint>();
        this.hints.AddRange((IEnumerable<RavenPathHint>) path.hints);
      }
      else
        this.hints = (List<RavenPathHint>) null;
    }

    ~RavenPath()
    {
        this.pts.Clear();
        this.flags.Clear();
        if (this.hints == null)
          return;
        this.hints.Clear();
    }

    private void grow(int nPts)
    {
    }

    internal void append(RavenPath path)
    {
      this.curSubpath = this.pts.Count + path.curSubpath;
      for (int index = 0; index < path.pts.Count; ++index)
      {
        this.pts.Add(path.pts[index]);
        this.flags.Add(path.flags[index]);
      }
    }

    internal RavenError moveTo(double x, double y)
    {
      if (this.onePointSubpath())
        return RavenError.ravenErrBogusPath;
      this.grow(1);
      this.pts.Add(new RavenPathPoint(x, y));
      this.flags.Add((byte) 3);
      this.curSubpath = this.pts.Count - 1;
      return RavenError.ravenOk;
    }

    internal RavenError lineTo(double x, double y)
    {
      if (this.noCurrentPoint())
        return RavenError.ravenErrNoCurPt;
      List<byte> list;
      int index;
      (list = this.flags)[index = this.flags.Count - 1] = (byte) ((uint) list[index] & 253U);
      this.grow(1);
      this.pts.Add(new RavenPathPoint(x, y));
      this.flags.Add((byte) 2);
      return RavenError.ravenOk;
    }

    internal RavenError curveTo(double x1, double y1, double x2, double y2, double x3, double y3)
    {
      if (this.noCurrentPoint())
        return RavenError.ravenErrNoCurPt;
      List<byte> list;
      int index;
      (list = this.flags)[index = this.flags.Count - 1] = (byte) ((uint) list[index] & 253U);
      this.grow(3);
      this.pts.Add(new RavenPathPoint(x1, y1));
      this.flags.Add((byte) 8);
      this.pts.Add(new RavenPathPoint(x2, y2));
      this.flags.Add((byte) 8);
      this.pts.Add(new RavenPathPoint(x3, y3));
      this.flags.Add((byte) 2);
      return RavenError.ravenOk;
    }

    internal RavenError close()
    {
      return this.close(false);
    }

    internal RavenError close(bool force)
    {
      if (this.noCurrentPoint())
        return RavenError.ravenErrNoCurPt;
      if (force || this.curSubpath == this.pts.Count - 1 || (this.pts[this.pts.Count - 1].x != this.pts[this.curSubpath].x || this.pts[this.pts.Count - 1].y != this.pts[this.curSubpath].y))
      {
        int num = (int) this.lineTo(this.pts[this.curSubpath].x, this.pts[this.curSubpath].y);
      }
      List<byte> list1;
      int index1;
      (list1 = this.flags)[index1 = this.curSubpath] = (byte) ((uint) list1[index1] | 4U);
      List<byte> list2;
      int index2;
      (list2 = this.flags)[index2 = this.pts.Count - 1] = (byte) ((uint) list2[index2] | 4U);
      this.curSubpath = this.pts.Count;
      return RavenError.ravenOk;
    }

    internal void addStrokeAdjustHint(int ctrl0, int ctrl1, int firstPt, int lastPt)
    {
      if (this.hints == null)
        this.hints = new List<RavenPathHint>();
      this.hints.Add(new RavenPathHint(ctrl0, ctrl1, firstPt, lastPt));
    }

    private void offset(double dx, double dy)
    {
      for (int index = 0; index < this.pts.Count; ++index)
        this.pts[index].Add(dx, dy);
    }

    private bool getCurPt(ref double x, ref double y)
    {
      if (this.noCurrentPoint())
        return false;
      x = this.pts[this.pts.Count - 1].x;
      y = this.pts[this.pts.Count - 1].y;
      return true;
    }

    private bool noCurrentPoint()
    {
      return this.curSubpath == this.pts.Count;
    }

    private bool onePointSubpath()
    {
      return this.curSubpath == this.pts.Count - 1;
    }

    private bool openSubpath()
    {
      return this.curSubpath < this.pts.Count - 1;
    }
  }
}
