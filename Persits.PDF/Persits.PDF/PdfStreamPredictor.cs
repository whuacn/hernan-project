// Type: Persits.PDF.PdfStreamPredictor
// Assembly: Persits.PDF, Version=2.5.0.19989, Culture=neutral, PublicKeyToken=b8d8c63f1ff142a8
// Assembly location: C:\Users\hernan\Desktop\asppdf_net\Samples\Bin\Persits.PDF.dll

namespace Persits.PDF
{
  internal class PdfStreamPredictor
  {
    private PdfStream str;
    private int predictor;
    private int width;
    private int nComps;
    private int nBits;
    private int nVals;
    private int pixBytes;
    private int rowBytes;
    private byte[] predLine;
    private int predIdx;

    public PdfStreamPredictor(PdfStream strA, int predictorA, int widthA, int nCompsA, int nBitsA)
    {
      //base.\u002Ector();
      this.str = strA;
      this.predictor = predictorA;
      this.width = widthA;
      this.nComps = nCompsA;
      this.nBits = nBitsA;
      this.nVals = this.width * this.nComps;
      this.pixBytes = this.nComps * this.nBits + 7 >> 3;
      this.rowBytes = (this.nVals * this.nBits + 7 >> 3) + this.pixBytes;
      this.predLine = new byte[this.rowBytes];
      this.predIdx = this.rowBytes;
    }

    ~PdfStreamPredictor()
    {

    }

    public int getChar()
    {
      if (this.predIdx >= this.rowBytes && !this.getNextLine())
        return -1;
      else
        return (int) this.predLine[this.predIdx++];
    }

    public bool getNextLine()
    {
      byte[] numArray1 = new byte[4];
      int num1;
      if (this.predictor >= 10)
      {
        int @char;
        if ((@char = this.str.getChar()) == -1)
          return false;
        num1 = @char + 10;
      }
      else
        num1 = this.predictor;
      numArray1[0] = numArray1[1] = numArray1[2] = numArray1[3] = (byte) 0;
      for (int index = this.pixBytes; index < this.rowBytes; ++index)
      {
        numArray1[3] = numArray1[2];
        numArray1[2] = numArray1[1];
        numArray1[1] = numArray1[0];
        numArray1[0] = this.predLine[index];
        int @char;
        if ((@char = this.str.getChar()) == -1)
          return false;
        switch (num1)
        {
          case 11:
            this.predLine[index] = (byte) ((uint) this.predLine[index - this.pixBytes] + (uint) (byte) @char);
            break;
          case 12:
            this.predLine[index] = (byte) ((uint) this.predLine[index] + (uint) (byte) @char);
            break;
          case 13:
            this.predLine[index] = (byte) ((uint) ((int) this.predLine[index - this.pixBytes] + (int) this.predLine[index] >> 1) + (uint) (byte) @char);
            break;
          case 14:
            int num2 = (int) this.predLine[index - this.pixBytes];
            int num3 = (int) this.predLine[index];
            int num4 = (int) numArray1[this.pixBytes];
            int num5 = num2 + num3 - num4;
            int num6;
            if ((num6 = num5 - num2) < 0)
              num6 = -num6;
            int num7;
            if ((num7 = num5 - num3) < 0)
              num7 = -num7;
            int num8;
            if ((num8 = num5 - num4) < 0)
              num8 = -num8;
            this.predLine[index] = num6 > num7 || num6 > num8 ? (num7 > num8 ? (byte) ((uint) num4 + (uint) (byte) @char) : (byte) ((uint) num3 + (uint) (byte) @char)) : (byte) ((uint) num2 + (uint) (byte) @char);
            break;
          default:
            this.predLine[index] = (byte) @char;
            break;
        }
      }
      if (this.predictor == 2)
      {
        if (this.nBits == 1)
        {
          ulong num2 = (ulong) this.predLine[this.pixBytes - 1];
          int index = this.pixBytes;
          while (index < this.rowBytes)
          {
            num2 = num2 << 8 | (ulong) this.predLine[index];
            this.predLine[index] ^= (byte) (num2 >> this.nComps);
            index += 8;
          }
        }
        else if (this.nBits == 8)
        {
          for (int index = this.pixBytes; index < this.rowBytes; ++index)
            this.predLine[index] += this.predLine[index - this.nComps];
        }
        else
        {
          numArray1[0] = numArray1[1] = numArray1[2] = numArray1[3] = (byte) 0;
          ulong num2 = (ulong) ((1 << this.nBits) - 1);
          ulong num3;
          ulong num4 = num3 = 0UL;
          int num5;
          int num6 = num5 = 0;
          int num7;
          int num8 = num7 = this.pixBytes;
          for (int index = 0; index < this.nVals; ++index)
          {
            if (num6 < this.nBits)
            {
              num4 = num4 << 8 | (ulong) (byte) ((uint) this.predLine[num8++] & (uint) byte.MaxValue);
              num6 += 8;
            }
            numArray1[3] = numArray1[2];
            numArray1[2] = numArray1[1];
            numArray1[1] = numArray1[0];
            numArray1[0] = (byte) ((ulong) numArray1[this.nComps] + (num4 >> num6 - this.nBits) & num2);
            num3 = num3 << this.nBits | (ulong) numArray1[0];
            num6 -= this.nBits;
            num5 += this.nBits;
            if (num5 > 8)
              this.predLine[num7++] = (byte) (num3 >> num5 - 8);
          }
          if (num5 > 0)
          {
            byte[] numArray2 = this.predLine;
            int index = num7;
            int num9 = 1;
            int num10 = index + num9;
            int num11 = (int) (byte) (num3 << 8 - num5);
            numArray2[index] = (byte) num11;
          }
        }
      }
      this.predIdx = this.pixBytes;
      return true;
    }
  }
}
