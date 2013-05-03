// Type: Persits.PDF.RavenBitmap
// Assembly: Persits.PDF, Version=2.5.0.19989, Culture=neutral, PublicKeyToken=b8d8c63f1ff142a8
// Assembly location: C:\Users\hernan\Desktop\asppdf_net\Samples\Bin\Persits.PDF.dll

namespace Persits.PDF
{
  internal class RavenBitmap
  {
    internal int rowSize;
    internal int width;
    internal int height;
    internal RavenColorMode mode;
    internal byte[] data;
    internal byte[] alpha;

    internal int Width
    {
      get
      {
        return this.width;
      }
    }

    internal int Height
    {
      get
      {
        return this.height;
      }
    }

    internal RavenBitmap(int widthA, int heightA, int rowPad, RavenColorMode modeA, bool alphaA, bool topDown)
    {
      this.width = widthA;
      this.height = heightA;
      this.mode = modeA;
      switch (this.mode)
      {
        case RavenColorMode.ravenModeMono1:
          this.rowSize = this.width <= 0 ? -1 : this.width + 7 >> 3;
          break;
        case RavenColorMode.ravenModeMono8:
          this.rowSize = this.width <= 0 ? -1 : this.width;
          break;
        case RavenColorMode.ravenModeRGB8:
        case RavenColorMode.ravenModeBGR8:
          this.rowSize = this.width <= 0 || this.width > 715827882 ? -1 : this.width * 3;
          break;
      }
      if (this.rowSize > 0)
      {
        this.rowSize += rowPad - 1;
        this.rowSize -= this.rowSize % rowPad;
      }
      this.data = new byte[this.height * this.rowSize];
      int num = topDown ? 1 : 0;
      if (alphaA)
        this.alpha = new byte[this.width * this.height];
      else
        this.alpha = (byte[]) null;
    }

    internal void getPixel(int x, int y, RavenColor pixel)
    {
      if (y < 0 || y >= this.height || (x < 0 || x >= this.width))
        return;
      switch (this.mode)
      {
        case RavenColorMode.ravenModeMono1:
          RavenColorPtr ravenColorPtr1 = new RavenColorPtr(this.data, y * this.rowSize + (x >> 3));
          pixel[0] = ((int) ravenColorPtr1[0] & 128 >> (x & 7)) != 0 ? byte.MaxValue : (byte) 0;
          break;
        case RavenColorMode.ravenModeMono8:
          RavenColorPtr ravenColorPtr2 = new RavenColorPtr(this.data, y * this.rowSize + x);
          pixel[0] = ravenColorPtr2[0];
          break;
        case RavenColorMode.ravenModeRGB8:
          RavenColorPtr ravenColorPtr3 = new RavenColorPtr(this.data, y * this.rowSize + 3 * x);
          pixel[0] = ravenColorPtr3[0];
          pixel[1] = ravenColorPtr3[1];
          pixel[2] = ravenColorPtr3[2];
          break;
        case RavenColorMode.ravenModeBGR8:
          int index = y * this.rowSize + 3 * x;
          pixel[0] = this.data[index + 2];
          pixel[1] = this.data[index + 1];
          pixel[2] = this.data[index];
          break;
      }
    }

    internal byte getAlpha(int x, int y)
    {
      return this.alpha[y * this.width + x];
    }

    internal byte[] getDataPtr()
    {
      return this.data;
    }

    internal byte[] getAlphaPtr()
    {
      return this.alpha;
    }

    internal int getWidth()
    {
      return this.width;
    }

    internal int getHeight()
    {
      return this.height;
    }

    internal int getRowSize()
    {
      return this.rowSize;
    }

    internal RavenColorMode getMode()
    {
      return this.mode;
    }
  }
}
