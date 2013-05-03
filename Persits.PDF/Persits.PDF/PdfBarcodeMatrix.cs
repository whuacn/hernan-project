// Type: Persits.PDF.PdfBarcodeMatrix
// Assembly: Persits.PDF, Version=2.5.0.19989, Culture=neutral, PublicKeyToken=b8d8c63f1ff142a8
// Assembly location: C:\Users\hernan\Desktop\asppdf_net\Samples\Bin\Persits.PDF.dll

using System;

namespace Persits.PDF
{
  internal class PdfBarcodeMatrix
  {
    private const int MAXBARCODE = 3116;
    private static ecc200matrix_s[] ecc200matrix;
    private byte[,] switchcost;
    public PdfManager m_pManager;
    public PdfDocument m_pDoc;
    public byte[] m_pData;
    public int m_nDataLen;
    public float m_fBarWidth;
    public int m_nWidth;
    public int m_nHeight;
    private byte[] m_pEncoding;
    private int m_nEncodingLen;
    private EncodingType m_nEncodingType;
    private string m_wszErrorMessage;
    private char[] encchr;

    static PdfBarcodeMatrix()
    {
      PdfBarcodeMatrix.ecc200matrix = new ecc200matrix_s[30]
      {
        new ecc200matrix_s(10, 10, 10, 10, 3, 3, 5),
        new ecc200matrix_s(12, 12, 12, 12, 5, 5, 7),
        new ecc200matrix_s(8, 18, 8, 18, 5, 5, 7),
        new ecc200matrix_s(14, 14, 14, 14, 8, 8, 10),
        new ecc200matrix_s(8, 32, 8, 16, 10, 10, 11),
        new ecc200matrix_s(16, 16, 16, 16, 12, 12, 12),
        new ecc200matrix_s(12, 26, 12, 26, 16, 16, 14),
        new ecc200matrix_s(18, 18, 18, 18, 18, 18, 14),
        new ecc200matrix_s(20, 20, 20, 20, 22, 22, 18),
        new ecc200matrix_s(12, 36, 12, 18, 22, 22, 18),
        new ecc200matrix_s(22, 22, 22, 22, 30, 30, 20),
        new ecc200matrix_s(16, 36, 16, 18, 32, 32, 24),
        new ecc200matrix_s(24, 24, 24, 24, 36, 36, 24),
        new ecc200matrix_s(26, 26, 26, 26, 44, 44, 28),
        new ecc200matrix_s(16, 48, 16, 24, 49, 49, 28),
        new ecc200matrix_s(32, 32, 16, 16, 62, 62, 36),
        new ecc200matrix_s(36, 36, 18, 18, 86, 86, 42),
        new ecc200matrix_s(40, 40, 20, 20, 114, 114, 48),
        new ecc200matrix_s(44, 44, 22, 22, 144, 144, 56),
        new ecc200matrix_s(48, 48, 24, 24, 174, 174, 68),
        new ecc200matrix_s(52, 52, 26, 26, 204, 102, 42),
        new ecc200matrix_s(64, 64, 16, 16, 280, 140, 56),
        new ecc200matrix_s(72, 72, 18, 18, 368, 92, 36),
        new ecc200matrix_s(80, 80, 20, 20, 456, 114, 48),
        new ecc200matrix_s(88, 88, 22, 22, 576, 144, 56),
        new ecc200matrix_s(96, 96, 24, 24, 696, 174, 68),
        new ecc200matrix_s(104, 104, 26, 26, 816, 136, 56),
        new ecc200matrix_s(120, 120, 20, 20, 1050, 175, 68),
        new ecc200matrix_s(132, 132, 22, 22, 1304, 163, 62),
        new ecc200matrix_s(144, 144, 24, 24, 1558, 156, 62)
      };
    }

    public PdfBarcodeMatrix()
    {
      this.switchcost = new byte[6, 6]
      {
        {
          (byte) 0,
          (byte) 1,
          (byte) 1,
          (byte) 1,
          (byte) 1,
          (byte) 2
        },
        {
          (byte) 1,
          (byte) 0,
          (byte) 2,
          (byte) 2,
          (byte) 2,
          (byte) 3
        },
        {
          (byte) 1,
          (byte) 2,
          (byte) 0,
          (byte) 2,
          (byte) 2,
          (byte) 3
        },
        {
          (byte) 1,
          (byte) 2,
          (byte) 2,
          (byte) 0,
          (byte) 2,
          (byte) 3
        },
        {
          (byte) 1,
          (byte) 2,
          (byte) 2,
          (byte) 2,
          (byte) 0,
          (byte) 3
        },
        {
          (byte) 0,
          (byte) 1,
          (byte) 1,
          (byte) 1,
          (byte) 1,
          (byte) 0
        }
      };
      this.encchr = "ACTXEB".ToCharArray();
      //base.\u002Ector();
      this.m_pData = (byte[]) null;
      this.m_nDataLen = 0;
      this.m_pEncoding = (byte[]) null;
      this.m_nEncodingLen = 0;
      this.m_nWidth = this.m_nHeight = 0;
      this.m_nEncodingType = EncodingType.NotDef;
    }

    ~PdfBarcodeMatrix()
    {

        this.m_pEncoding = (byte[]) null;

    }

    private byte EncodingTypeToByte(EncodingType et)
    {
      switch (et)
      {
        case EncodingType.Ascii:
          return (byte) 65;
        case EncodingType.C40:
          return (byte) 67;
        case EncodingType.Text:
          return (byte) 84;
        case EncodingType.X12:
          return (byte) 88;
        case EncodingType.Edifact:
          return (byte) 69;
        case EncodingType.Binary:
          return (byte) 66;
        default:
          return (byte) 0;
      }
    }

    public void Draw(PdfCanvas pCanvas, PdfParam pParam)
    {
      float num1 = 1f;
      if (pParam.IsSet("BarWidth"))
        num1 = pParam.Number("BarWidth");
      if ((double) num1 < 0.0)
        AuxException.Throw("Invalid BarWidth parameter.", PdfErrors._ERROR_INVALIDARG);
      this.m_fBarWidth = num1;
      if (!pParam.IsSet("X") || !pParam.IsSet("Y"))
        AuxException.Throw("X and Y parameters are required.", PdfErrors._ERROR_INVALIDARG);
      float e = pParam.Number("X");
      float f = pParam.Number("Y");
      if (pParam.IsSet("Angle"))
      {
        float num2 = pParam.Number("Angle");
        float num3 = 0.01745329f;
        pCanvas.SetCTM((float) Math.Cos((double) num2 * (double) num3), (float) Math.Sin((double) num2 * (double) num3), -(float) Math.Sin((double) num2 * (double) num3), (float) Math.Cos((double) num2 * (double) num3), e, f);
        e = f = 0.0f;
      }
      AuxRGB rgb1 = new AuxRGB();
      if (pParam.IsSet("Color"))
      {
        pParam.Color("Color", ref rgb1);
        pCanvas.SetFillColor(rgb1.r, rgb1.g, rgb1.b);
      }
      bool flag = false;
      float pOut = num1 * 2f;
      AuxRGB rgb2 = new AuxRGB();
      if (pParam.IsSet("BgColor"))
      {
        pParam.Color("BgColor", ref rgb2);
        flag = true;
        pParam.GetNumberIfSet("BgMargin", 0, ref pOut);
      }
      if (pParam.IsSet("Encoding"))
        this.m_nEncodingType = (EncodingType) pParam.Long("Encoding");
      if (this.m_nEncodingType < EncodingType.NotDef || this.m_nEncodingType > EncodingType.Binary)
        AuxException.Throw("Valid Encoding parameter values are: 0 (Automatic), 1 (ASCII), 2 (C40), 3 (Text), 4 (X12), 5 (Edifact) and 6 (Binary).", PdfErrors._ERROR_INVALIDARG);
      int lenp = 0;
      int maxp = 0;
      int eccp = 0;
      if (this.m_nEncodingType != EncodingType.NotDef)
      {
        this.m_pEncoding = new byte[this.m_nDataLen + 1];
        this.m_nEncodingLen = this.m_nDataLen + 1;
        byte num2 = this.EncodingTypeToByte(this.m_nEncodingType);
        for (int index = 0; index < this.m_nEncodingLen; ++index)
          this.m_pEncoding[index] = num2;
        this.m_pEncoding[this.m_nDataLen] = (byte) 0;
      }
      if (pParam.IsSet("Columns"))
        this.m_nWidth = pParam.Long("Columns");
      if (pParam.IsSet("Rows"))
        this.m_nHeight = pParam.Long("Rows");
      byte[] numArray = this.iec16022ecc200(ref this.m_nWidth, ref this.m_nHeight, ref this.m_pEncoding, this.m_nDataLen, this.m_pData, ref lenp, ref maxp, ref eccp);
      if (this.m_wszErrorMessage != null)
        AuxException.Throw(this.m_wszErrorMessage, PdfErrors._ERROR_BARCODE_MATRIX);
      if (flag)
      {
        pCanvas.SetFillColor(rgb2.r, rgb2.g, rgb2.b);
        pCanvas.FillRect(e - pOut, f - pOut, (float) ((double) num1 * (double) this.m_nWidth + 2.0 * (double) pOut), (float) ((double) num1 * (double) this.m_nHeight + 2.0 * (double) pOut));
        pCanvas.SetFillColor(rgb1.r, rgb1.g, rgb1.b);
      }
      float X = e;
      float Y = f;
      for (int index1 = 0; index1 < this.m_nHeight; ++index1)
      {
        for (int index2 = 0; index2 < this.m_nWidth; ++index2)
        {
          if ((int) numArray[this.m_nWidth * index1 + index2] > 0)
            pCanvas.FillRect(X, Y, num1, num1);
          X += num1;
        }
        Y += num1;
        X = e;
      }
    }

    private void ecc200placementbit(int[] array, int NR, int NC, int r, int c, int p, char b)
    {
      if (r < 0)
      {
        r += NR;
        c += 4 - (NR + 4) % 8;
      }
      if (c < 0)
      {
        c += NC;
        r += 4 - (NC + 4) % 8;
      }
      array[r * NC + c] = (p << 3) + (int) b;
    }

    private void ecc200placementblock(int[] array, int NR, int NC, int r, int c, int p)
    {
      this.ecc200placementbit(array, NR, NC, r - 2, c - 2, p, '\a');
      this.ecc200placementbit(array, NR, NC, r - 2, c - 1, p, '\x0006');
      this.ecc200placementbit(array, NR, NC, r - 1, c - 2, p, '\x0005');
      this.ecc200placementbit(array, NR, NC, r - 1, c - 1, p, '\x0004');
      this.ecc200placementbit(array, NR, NC, r - 1, c, p, '\x0003');
      this.ecc200placementbit(array, NR, NC, r, c - 2, p, '\x0002');
      this.ecc200placementbit(array, NR, NC, r, c - 1, p, '\x0001');
      this.ecc200placementbit(array, NR, NC, r, c, p, char.MinValue);
    }

    private void ecc200placementcornerA(int[] array, int NR, int NC, int p)
    {
      this.ecc200placementbit(array, NR, NC, NR - 1, 0, p, '\a');
      this.ecc200placementbit(array, NR, NC, NR - 1, 1, p, '\x0006');
      this.ecc200placementbit(array, NR, NC, NR - 1, 2, p, '\x0005');
      this.ecc200placementbit(array, NR, NC, 0, NC - 2, p, '\x0004');
      this.ecc200placementbit(array, NR, NC, 0, NC - 1, p, '\x0003');
      this.ecc200placementbit(array, NR, NC, 1, NC - 1, p, '\x0002');
      this.ecc200placementbit(array, NR, NC, 2, NC - 1, p, '\x0001');
      this.ecc200placementbit(array, NR, NC, 3, NC - 1, p, char.MinValue);
    }

    private void ecc200placementcornerB(int[] array, int NR, int NC, int p)
    {
      this.ecc200placementbit(array, NR, NC, NR - 3, 0, p, '\a');
      this.ecc200placementbit(array, NR, NC, NR - 2, 0, p, '\x0006');
      this.ecc200placementbit(array, NR, NC, NR - 1, 0, p, '\x0005');
      this.ecc200placementbit(array, NR, NC, 0, NC - 4, p, '\x0004');
      this.ecc200placementbit(array, NR, NC, 0, NC - 3, p, '\x0003');
      this.ecc200placementbit(array, NR, NC, 0, NC - 2, p, '\x0002');
      this.ecc200placementbit(array, NR, NC, 0, NC - 1, p, '\x0001');
      this.ecc200placementbit(array, NR, NC, 1, NC - 1, p, char.MinValue);
    }

    private void ecc200placementcornerC(int[] array, int NR, int NC, int p)
    {
      this.ecc200placementbit(array, NR, NC, NR - 3, 0, p, '\a');
      this.ecc200placementbit(array, NR, NC, NR - 2, 0, p, '\x0006');
      this.ecc200placementbit(array, NR, NC, NR - 1, 0, p, '\x0005');
      this.ecc200placementbit(array, NR, NC, 0, NC - 2, p, '\x0004');
      this.ecc200placementbit(array, NR, NC, 0, NC - 1, p, '\x0003');
      this.ecc200placementbit(array, NR, NC, 1, NC - 1, p, '\x0002');
      this.ecc200placementbit(array, NR, NC, 2, NC - 1, p, '\x0001');
      this.ecc200placementbit(array, NR, NC, 3, NC - 1, p, char.MinValue);
    }

    private void ecc200placementcornerD(int[] array, int NR, int NC, int p)
    {
      this.ecc200placementbit(array, NR, NC, NR - 1, 0, p, '\a');
      this.ecc200placementbit(array, NR, NC, NR - 1, NC - 1, p, '\x0006');
      this.ecc200placementbit(array, NR, NC, 0, NC - 3, p, '\x0005');
      this.ecc200placementbit(array, NR, NC, 0, NC - 2, p, '\x0004');
      this.ecc200placementbit(array, NR, NC, 0, NC - 1, p, '\x0003');
      this.ecc200placementbit(array, NR, NC, 1, NC - 3, p, '\x0002');
      this.ecc200placementbit(array, NR, NC, 1, NC - 2, p, '\x0001');
      this.ecc200placementbit(array, NR, NC, 1, NC - 1, p, char.MinValue);
    }

    private void ecc200placement(int[] array, int NR, int NC)
    {
      for (int index1 = 0; index1 < NR; ++index1)
      {
        for (int index2 = 0; index2 < NC; ++index2)
          array[index1 * NC + index2] = 0;
      }
      int num = 1;
      int r1 = 4;
      int c1 = 0;
      do
      {
        if (r1 == NR && c1 == 0)
          this.ecc200placementcornerA(array, NR, NC, num++);
        if (r1 == NR - 2 && c1 == 0 && NC % 4 != 0)
          this.ecc200placementcornerB(array, NR, NC, num++);
        if (r1 == NR - 2 && c1 == 0 && NC % 8 == 4)
          this.ecc200placementcornerC(array, NR, NC, num++);
        if (r1 == NR + 4 && c1 == 2 && NC % 8 == 0)
          this.ecc200placementcornerD(array, NR, NC, num++);
        do
        {
          if (r1 < NR && c1 >= 0 && array[r1 * NC + c1] == 0)
            this.ecc200placementblock(array, NR, NC, r1, c1, num++);
          r1 -= 2;
          c1 += 2;
        }
        while (r1 >= 0 && c1 < NC);
        int r2 = r1 + 1;
        int c2 = c1 + 3;
        do
        {
          if (r2 >= 0 && c2 < NC && array[r2 * NC + c2] == 0)
            this.ecc200placementblock(array, NR, NC, r2, c2, num++);
          r2 += 2;
          c2 -= 2;
        }
        while (r2 < NR && c2 >= 0);
        r1 = r2 + 3;
        c1 = c2 + 1;
      }
      while (r1 < NR || c1 < NC);
      if (array[NR * NC - 1] != 0)
        return;
      array[NR * NC - 1] = array[NR * NC - NC - 2] = 1;
    }

    private void ecc200(byte[] binary, int bytes, int datablock, int rsblock)
    {
      ReedSol reedSol = new ReedSol();
      int num1 = (bytes + 2) / datablock;
      reedSol.rs_init_gf(301);
      reedSol.rs_init_code(rsblock, 1);
      for (int index1 = 0; index1 < num1; ++index1)
      {
        byte[] data = new byte[256];
        byte[] res = new byte[256];
        int len = 0;
        int index2 = index1;
        while (index2 < bytes)
        {
          data[len++] = binary[index2];
          index2 += num1;
        }
        reedSol.rs_encode(len, data, out res);
        int num2 = rsblock - 1;
        int num3 = index1;
        while (num3 < rsblock * num1)
        {
          binary[bytes + num3] = res[num2--];
          num3 += num1;
        }
      }
    }

    private char ecc200encode(byte[] t, int tl, byte[] s, int sl, byte[] encoding, ref int lenp)
    {
      char ch1 = 'a';
      int index1 = 0;
      int index2 = 0;
      if (encoding.Length < sl)
      {
        this.m_wszErrorMessage = "The encoding string is too short.";
        return char.MinValue;
      }
      else
      {
        while (index2 < sl && index1 < tl)
        {
          if (tl - index1 <= 1 && ((int) ch1 == 99 || (int) ch1 == 116) || tl - index1 <= 2 && (int) ch1 == 120)
            ch1 = 'a';
          char ch2 = this.tolower(encoding[index2]);
          switch (ch2)
          {
            case 'a':
              if ((int) ch1 != (int) ch2)
                t[index1++] = (int) ch1 == 99 || (int) ch1 == 116 || (int) ch1 == 120 ? (byte) 254 : (byte) 124;
              ch1 = 'a';
              if (sl - index2 >= 2 && this.isdigit(s[index2]) && this.isdigit(s[index2 + 1]))
              {
                t[index1++] = (byte) (((int) s[index2] - 48) * 10 + (int) s[index2 + 1] - 48 + 130);
                index2 += 2;
                continue;
              }
              else if ((int) s[index2] > (int) sbyte.MaxValue)
              {
                byte[] numArray1 = t;
                int index3 = index1;
                int num1 = 1;
                int num2 = index3 + num1;
                int num3 = 235;
                numArray1[index3] = (byte) num3;
                byte[] numArray2 = t;
                int index4 = num2;
                int num4 = 1;
                index1 = index4 + num4;
                int num5 = (int) (byte) ((uint) s[index2++] - (uint) sbyte.MaxValue);
                numArray2[index4] = (byte) num5;
                continue;
              }
              else
              {
                t[index1++] = (byte) ((uint) s[index2++] + 1U);
                continue;
              }
            case 'b':
              int num6 = 0;
              if (encoding != null)
              {
                for (int index3 = index2; index3 < sl && (int) this.tolower(encoding[index3]) == 98; ++index3)
                  ++num6;
              }
              byte[] numArray3 = t;
              int index5 = index1;
              int num7 = 1;
              int num8 = index5 + num7;
              int num9 = 231;
              numArray3[index5] = (byte) num9;
              if (num6 < 250)
              {
                byte[] numArray1 = t;
                int index3 = num8;
                int num1 = 1;
                index1 = index3 + num1;
                int num2 = (int) (byte) num6;
                numArray1[index3] = (byte) num2;
              }
              else
              {
                byte[] numArray1 = t;
                int index3 = num8;
                int num1 = 1;
                int num2 = index3 + num1;
                int num3 = (int) (byte) (249 + num6 / 250);
                numArray1[index3] = (byte) num3;
                byte[] numArray2 = t;
                int index4 = num2;
                int num4 = 1;
                index1 = index4 + num4;
                int num5 = (int) (byte) (num6 % 250);
                numArray2[index4] = (byte) num5;
              }
              for (; num6-- != 0 && index1 < tl; ++index1)
                t[index1] = (byte) ((int) s[index2++] + (index1 + 1) * 149 % (int) byte.MaxValue + 1);
              ch1 = 'a';
              continue;
            case 'c':
            case 't':
            case 'x':
              char[] chArray1 = new char[6];
              char ch3 = char.MinValue;
              string str1 = "!\"#$%&'()*+,-./:;<=>?@[\\]_";
              string str2 = "";
              string str3 = "";
              if ((int) ch2 == 99)
              {
                str3 = " 0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                str2 = "`abcdefghijklmnopqrstuvwxyz{|}~\0177";
              }
              if ((int) ch2 == 116)
              {
                str3 = " 0123456789abcdefghijklmnopqrstuvwxyz";
                str2 = "`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~\0177";
              }
              if ((int) ch2 == 120)
                str3 = " 0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ\r*>";
              do
              {
                char ch4 = (char) s[index2++];
                if (((int) ch4 & 128) != 0)
                {
                  if ((int) ch2 == 120)
                  {
                    this.m_wszErrorMessage = string.Format("Cannot encode the character {0:x} in X12.", (object) ch4);
                    return char.MinValue;
                  }
                  else
                  {
                    ch4 &= '\x007F';
                    char[] chArray2 = chArray1;
                    int index3 = (int) ch3;
                    int num1 = 1;
                    char ch5 = (char) (index3 + num1);
                    int num2 = 1;
                    chArray2[index3] = (char) num2;
                    char[] chArray3 = chArray1;
                    int index4 = (int) ch5;
                    int num3 = 1;
                    ch3 = (char) (index4 + num3);
                    int num4 = 30;
                    chArray3[index4] = (char) num4;
                  }
                }
                int num5 = str3.IndexOf(ch4);
                if (num5 >= 0)
                  chArray1[(int) ch3++] = (char) ((num5 + 3) % 40);
                else if ((int) ch2 == 120)
                {
                  this.m_wszErrorMessage = string.Format("Cannot encode the character {0:x} in X12.", (object) ch4);
                  return char.MinValue;
                }
                else if ((int) ch4 < 32)
                {
                  char[] chArray2 = chArray1;
                  int index3 = (int) ch3;
                  int num1 = 1;
                  char ch5 = (char) (index3 + num1);
                  int num2 = 0;
                  chArray2[index3] = (char) num2;
                  char[] chArray3 = chArray1;
                  int index4 = (int) ch5;
                  int num3 = 1;
                  ch3 = (char) (index4 + num3);
                  int num4 = (int) ch4;
                  chArray3[index4] = (char) num4;
                }
                else
                {
                  int num1 = str1.IndexOf(ch4);
                  if (num1 >= 0)
                  {
                    char[] chArray2 = chArray1;
                    int index3 = (int) ch3;
                    int num2 = 1;
                    char ch5 = (char) (index3 + num2);
                    int num3 = 1;
                    chArray2[index3] = (char) num3;
                    char[] chArray3 = chArray1;
                    int index4 = (int) ch5;
                    int num4 = 1;
                    ch3 = (char) (index4 + num4);
                    int num10 = (int) (ushort) num1;
                    chArray3[index4] = (char) num10;
                  }
                  else
                  {
                    int num2 = str2.IndexOf(ch4);
                    if (num2 >= 0)
                    {
                      char[] chArray2 = chArray1;
                      int index3 = (int) ch3;
                      int num3 = 1;
                      char ch5 = (char) (index3 + num3);
                      int num4 = 2;
                      chArray2[index3] = (char) num4;
                      char[] chArray3 = chArray1;
                      int index4 = (int) ch5;
                      int num10 = 1;
                      ch3 = (char) (index4 + num10);
                      int num11 = (int) (ushort) num2;
                      chArray3[index4] = (char) num11;
                    }
                    else
                    {
                      this.m_wszErrorMessage = string.Format("Could not encode {0:x}.", (object) ch4);
                      return char.MinValue;
                    }
                  }
                }
                if ((int) ch3 == 2 && index1 + 2 == tl && index2 == sl)
                  chArray1[(int) ch3++] = char.MinValue;
                while ((int) ch3 >= 3)
                {
                  int num1 = (int) chArray1[0] * 1600 + (int) chArray1[1] * 40 + (int) chArray1[2] + 1;
                  if ((int) ch1 != (int) ch2)
                  {
                    if ((int) ch1 == 99 || (int) ch1 == 116 || (int) ch1 == 120)
                      t[index1++] = (byte) 254;
                    else if ((int) ch1 == 120)
                      t[index1++] = (byte) 124;
                    if ((int) ch2 == 99)
                      t[index1++] = (byte) 230;
                    if ((int) ch2 == 116)
                      t[index1++] = (byte) 239;
                    if ((int) ch2 == 120)
                      t[index1++] = (byte) 238;
                    ch1 = ch2;
                  }
                  byte[] numArray1 = t;
                  int index3 = index1;
                  int num2 = 1;
                  int num3 = index3 + num2;
                  int num4 = (int) (byte) (num1 >> 8);
                  numArray1[index3] = (byte) num4;
                  byte[] numArray2 = t;
                  int index4 = num3;
                  int num10 = 1;
                  index1 = index4 + num10;
                  int num11 = (int) (byte) (num1 & (int) byte.MaxValue);
                  numArray2[index4] = (byte) num11;
                  ch3 -= '\x0003';
                  chArray1[0] = chArray1[3];
                  chArray1[1] = chArray1[4];
                  chArray1[2] = chArray1[5];
                }
              }
              while ((int) ch3 != 0 && index2 < sl);
              continue;
            case 'e':
              byte[] numArray4 = new byte[4];
              byte num12 = (byte) 0;
              if ((int) ch1 != (int) ch2)
              {
                t[index1++] = (byte) 254;
                ch1 = 'a';
              }
              while (index2 < sl && (int) this.tolower(encoding[index2]) == 101 && (int) num12 < 4)
                numArray4[(int) num12++] = s[index2++];
              if ((int) num12 < 4)
              {
                numArray4[(int) num12++] = (byte) 31;
                ch1 = 'a';
              }
              t[index1] = (byte) (((int) s[0] & 63) << 2);
              byte[] numArray5 = t;
              int index6 = index1;
              int num13 = 1;
              int index7 = index6 + num13;
              numArray5[index6] |= (byte) (((int) s[1] & 48) >> 4);
              t[index7] = (byte) (((int) s[1] & 15) << 4);
              if ((int) num12 == 2)
              {
                index1 = index7 + 1;
                continue;
              }
              else
              {
                byte[] numArray1 = t;
                int index3 = index7;
                int num1 = 1;
                int index4 = index3 + num1;
                numArray1[index3] |= (byte) (((int) s[2] & 60) >> 2);
                t[index4] = (byte) (((int) s[2] & 3) << 6);
                byte[] numArray2 = t;
                int index8 = index4;
                int num2 = 1;
                index1 = index8 + num2;
                numArray2[index8] |= (byte) ((uint) s[3] & 63U);
                continue;
              }
            default:
              this.m_wszErrorMessage = string.Format("Unknown encoding: {0}. ", (object) ch2);
              return char.MinValue;
          }
        }
        if (lenp != 0)
          lenp = index1;
        if (index1 < tl && (int) ch1 != 97)
          t[index1++] = (int) ch1 == 99 || (int) ch1 == 120 || (int) ch1 == 116 ? (byte) 254 : (byte) 124;
        if (index1 < tl)
          t[index1++] = (byte) 129;
        while (index1 < tl) {
          int num;          
          num = 129 + (index1 + 1) * 149 % 253 + 1;
          if (num > 254)
            num -= 254;
          t[index1++] = (byte)num;
        }
        return index1 > tl || index2 < sl ? char.MinValue : '\x0001';
      }
    }

    private byte[] encmake(int l, byte[] s, ref int lenp, char exact)
    {
      int index1 = l;
      TempStruct1[,] tempStruct1Array = new TempStruct1[3116, 6];
      if (l == 0)
        return (byte[]) null;
      if (l > 3116)
        return (byte[]) null;
      while (index1-- > 0)
      {
        char ch1 = char.MinValue;
        int num1;
        int num2 = num1 = 1;
        if (this.isdigit(s[index1]) && index1 + 1 < l && this.isdigit(s[index1 + 1]))
          num2 = 2;
        else if (((int) s[index1] & 128) != 0)
          num1 = 2;
        int num3 = 0;
        if (index1 + num2 < l)
        {
          for (char ch2 = char.MinValue; (int) ch2 < 6; ++ch2)
          {
            int num4;
            if ((int) tempStruct1Array[index1 + num2, (int) ch2].t != 0 && ((num4 = (int) tempStruct1Array[index1 + num2, (int) ch2].t + (int) this.switchcost[0, (int) ch2]) < num3 || num3 == 0))
            {
              num3 = num4;
              ch1 = ch2;
            }
          }
        }
        tempStruct1Array[index1, 0].t = (short) (num1 + num3);
        tempStruct1Array[index1, 0].s = (short) num2;
        if (num3 != 0 && (int) ch1 == 0)
          tempStruct1Array[index1, (int) ch1].s += tempStruct1Array[index1 + num2, (int) ch1].s;
        char ch3 = char.MinValue;
        int num5 = 0;
        int num6 = 0;
        do
        {
          byte b = s[index1 + num6++];
          if (((int) b & 128) != 0)
          {
            ch3 += '\x0002';
            b &= (byte) 127;
          }
          if ((int) b != 32 && !this.isdigit(b) && !this.isupper(b))
            ++ch3;
          ++ch3;
          while ((int) ch3 >= 3)
          {
            ch3 -= '\x0003';
            num5 += 2;
          }
        }
        while ((int) ch3 != 0 && index1 + num6 < l);
        if ((int) exact != 0 && (int) ch3 == 2 && index1 + num6 == l)
        {
          ch3 = char.MinValue;
          num5 += 2;
        }
        if ((int) ch3 == 0)
        {
          int num4 = 0;
          if (index1 + num6 < l)
          {
            for (char ch2 = char.MinValue; (int) ch2 < 6; ++ch2)
            {
              int num7;
              if ((int) tempStruct1Array[index1 + num6, (int) ch2].t != 0 && ((num7 = (int) tempStruct1Array[index1 + num6, (int) ch2].t + (int) this.switchcost[1, (int) ch2]) < num4 || num4 == 0))
              {
                num4 = num7;
                ch1 = ch2;
              }
            }
          }
          if ((int) exact != 0 && (int) tempStruct1Array[index1 + num6, 0].t == 1 && 1 < num4)
          {
            num4 = 1;
            ch1 = char.MinValue;
          }
          tempStruct1Array[index1, 1].t = (short) (num5 + num4);
          tempStruct1Array[index1, 1].s = (short) num6;
          if (num4 != 0 && (int) ch1 == 1)
            tempStruct1Array[index1, (int) ch1].s += tempStruct1Array[index1 + num6, (int) ch1].s;
        }
        char ch4 = char.MinValue;
        int num8 = 0;
        int num9 = 0;
        do
        {
          byte b = s[index1 + num9++];
          if (((int) b & 128) != 0)
          {
            ch4 += '\x0002';
            b &= (byte) 127;
          }
          if ((int) b != 32 && !this.isdigit(b) && !this.islower(b))
            ++ch4;
          ++ch4;
          while ((int) ch4 >= 3)
          {
            ch4 -= '\x0003';
            num8 += 2;
          }
        }
        while ((int) ch4 != 0 && index1 + num9 < l);
        if ((int) exact != 0 && (int) ch4 == 2 && index1 + num9 == l)
        {
          ch4 = char.MinValue;
          num8 += 2;
        }
        if ((int) ch4 == 0 && num9 != 0)
        {
          int num4 = 0;
          if (index1 + num9 < l)
          {
            for (char ch2 = char.MinValue; (int) ch2 < 6; ++ch2)
            {
              int num7;
              if ((int) tempStruct1Array[index1 + num9, (int) ch2].t != 0 && ((num7 = (int) tempStruct1Array[index1 + num9, (int) ch2].t + (int) this.switchcost[2, (int) ch2]) < num4 || num4 == 0))
              {
                num4 = num7;
                ch1 = ch2;
              }
            }
          }
          if ((int) exact != 0 && (int) tempStruct1Array[index1 + num9, 0].t == 1 && 1 < num4)
          {
            num4 = 1;
            ch1 = char.MinValue;
          }
          tempStruct1Array[index1, 2].t = (short) (num8 + num4);
          tempStruct1Array[index1, 2].s = (short) num9;
          if (num4 != 0 && (int) ch1 == 2)
            tempStruct1Array[index1, (int) ch1].s += tempStruct1Array[index1 + num9, (int) ch1].s;
        }
        char ch5 = char.MinValue;
        int num10 = 0;
        int num11 = 0;
        do
        {
          byte b = s[index1 + num11++];
          switch (b)
          {
            case (byte) 13:
            case (byte) 42:
            case (byte) 62:
            case (byte) 32:
              ++ch5;
              while ((int) ch5 >= 3)
              {
                ch5 -= '\x0003';
                num10 += 2;
              }
              continue;
            default:
              if (!this.isdigit(b) && !this.isupper(b))
              {
                num11 = 0;
                goto label_68;
              }
              else
                goto case (byte) 13;
          }
        }
        while ((int) ch5 != 0 && index1 + num11 < l);
label_68:
        if ((int) ch5 == 0 && num11 != 0)
        {
          int num4 = 0;
          if (index1 + num11 < l)
          {
            for (char ch2 = char.MinValue; (int) ch2 < 6; ++ch2)
            {
              int num7;
              if ((int) tempStruct1Array[index1 + num11, (int) ch2].t != 0 && ((num7 = (int) tempStruct1Array[index1 + num11, (int) ch2].t + (int) this.switchcost[3, (int) ch2]) < num4 || num4 == 0))
              {
                num4 = num7;
                ch1 = ch2;
              }
            }
          }
          if ((int) exact != 0 && (int) tempStruct1Array[index1 + num11, 0].t == 1 && 1 < num4)
          {
            num4 = 1;
            ch1 = char.MinValue;
          }
          tempStruct1Array[index1, 3].t = (short) (num10 + num4);
          tempStruct1Array[index1, 3].s = (short) num11;
          if (num4 != 0 && (int) ch1 == 3)
            tempStruct1Array[index1, (int) ch1].s += tempStruct1Array[index1 + num11, (int) ch1].s;
        }
        int num12;
        int num13 = num12 = 0;
        if ((int) s[index1] >= 32 && (int) s[index1] <= 94)
        {
          char ch2 = char.MinValue;
          if (index1 + 1 == l && (num12 == 0 || num12 < 2))
          {
            num12 = 2;
            ch2 = '\x0001';
          }
          else
          {
            for (char ch6 = char.MinValue; (int) ch6 < 6; ++ch6)
            {
              int num4;
              if ((int) ch6 != 4 && (int) tempStruct1Array[index1 + 1, (int) ch6].t != 0 && ((num4 = 2 + (int) tempStruct1Array[index1 + 1, (int) ch6].t + (int) this.switchcost[0, (int) ch6]) < num12 || num12 == 0))
              {
                ch2 = '\x0001';
                num12 = num4;
                ch1 = ch6;
              }
            }
          }
          if (index1 + 1 < l && (int) s[index1 + 1] >= 32 && (int) s[index1 + 1] <= 94)
          {
            if (index1 + 2 == l && (num12 == 0 || num12 < 2))
            {
              num12 = 3;
              ch2 = '\x0002';
            }
            else
            {
              for (char ch6 = char.MinValue; (int) ch6 < 6; ++ch6)
              {
                int num4;
                if ((int) ch6 != 4 && (int) tempStruct1Array[index1 + 2, (int) ch6].t != 0 && ((num4 = 3 + (int) tempStruct1Array[index1 + 2, (int) ch6].t + (int) this.switchcost[0, (int) ch6]) < num12 || num12 == 0))
                {
                  ch2 = '\x0002';
                  num12 = num4;
                  ch1 = ch6;
                }
              }
            }
            if (index1 + 2 < l && (int) s[index1 + 2] >= 32 && (int) s[index1 + 2] <= 94)
            {
              if (index1 + 3 == l && (num12 == 0 || num12 < 3))
              {
                num12 = 3;
                ch2 = '\x0003';
              }
              else
              {
                for (char ch6 = char.MinValue; (int) ch6 < 6; ++ch6)
                {
                  int num4;
                  if ((int) ch6 != 4 && (int) tempStruct1Array[index1 + 3, (int) ch6].t != 0 && ((num4 = 3 + (int) tempStruct1Array[index1 + 3, (int) ch6].t + (int) this.switchcost[0, (int) ch6]) < num12 || num12 == 0))
                  {
                    ch2 = '\x0003';
                    num12 = num4;
                    ch1 = ch6;
                  }
                }
              }
              if (index1 + 4 < l && (int) s[index1 + 3] >= 32 && (int) s[index1 + 3] <= 94)
              {
                if (index1 + 4 == l && (num12 == 0 || num12 < 3))
                {
                  num12 = 3;
                  ch2 = '\x0004';
                }
                else
                {
                  for (char ch6 = char.MinValue; (int) ch6 < 6; ++ch6)
                  {
                    int num4;
                    if ((int) tempStruct1Array[index1 + 4, (int) ch6].t != 0 && ((num4 = 3 + (int) tempStruct1Array[index1 + 4, (int) ch6].t + (int) this.switchcost[4, (int) ch6]) < num12 || num12 == 0))
                    {
                      ch2 = '\x0004';
                      num12 = num4;
                      ch1 = ch6;
                    }
                  }
                  int num7;
                  if ((int) exact != 0 && (int) tempStruct1Array[index1 + 4, 0].t != 0 && ((int) tempStruct1Array[index1 + 4, 0].t <= 2 && (num7 = 3 + (int) tempStruct1Array[index1 + 4, 0].t) < num12))
                  {
                    ch2 = '\x0004';
                    num12 = num7;
                    ch1 = char.MinValue;
                  }
                }
              }
            }
          }
          tempStruct1Array[index1, 4].t = (short) num12;
          tempStruct1Array[index1, 4].s = (short) ch2;
          if (num12 != 0 && (int) ch1 == 4)
            tempStruct1Array[index1, (int) ch1].s += tempStruct1Array[index1 + (int) ch2, (int) ch1].s;
        }
        int num14 = 0;
        for (char ch2 = char.MinValue; (int) ch2 < 6; ++ch2)
        {
          if ((int) tempStruct1Array[index1 + 1, (int) ch2].t != 0)
          {
            int num4 = (int) tempStruct1Array[index1 + 1, (int) ch2].t + (int) this.switchcost[5, (int) ch2];
            int num7 = (int) ch2 != 5 || (int) tempStruct1Array[index1 + 1, (int) ch2].t != 249 ? 0 : 1;
            int num15;
            if ((num15 = num4 + num7) < num14 || num14 == 0)
            {
              num14 = num15;
              ch1 = ch2;
            }
          }
        }
        tempStruct1Array[index1, 5].t = (short) (1 + num14);
        tempStruct1Array[index1, 5].s = (short) 1;
        if (num14 != 0 && (int) ch1 == 5)
          tempStruct1Array[index1, (int) ch1].s += tempStruct1Array[index1 + 1, (int) ch1].s;
      }
      byte[] numArray = new byte[l + 1];
      int index2 = 0;
      char ch7 = char.MinValue;
label_134:
      while (index2 < l)
      {
        int num1 = 0;
        char ch1 = char.MinValue;
        for (char ch2 = char.MinValue; (int) ch2 < 6; ++ch2)
        {
          int num2;
          if ((int) tempStruct1Array[index2, (int) ch2].t != 0 && ((num2 = (int) tempStruct1Array[index2, (int) ch2].t + (int) this.switchcost[(int) ch7, (int) ch2]) < num1 || num2 == num1 && (int) ch2 == (int) ch7 || num1 == 0))
          {
            ch1 = ch2;
            num1 = num2;
          }
        }
        ch7 = ch1;
        int num3 = (int) tempStruct1Array[index2, (int) ch1].s;
        if (index2 == 0 && lenp != 0)
          lenp = (int) tempStruct1Array[index2, (int) ch1].t;
        while (true)
        {
          if (index2 < l && num3-- != 0)
            numArray[index2++] = (byte) this.encchr[(int) ch1];
          else
            goto label_134;
        }
      }
      numArray[index2] = (byte) 0;
      return numArray;
    }

    public byte[] iec16022ecc200(ref int Wptr, ref int Hptr, ref byte[] encodingptr, int barcodelen, byte[] barcode, ref int lenp, ref int maxp, ref int eccp)
    {
      byte[] numArray1 = new byte[3000];
      int num1 = 0;
      int num2 = 0;
      byte[] encoding = (byte[]) null;
      if (encodingptr != null)
        encoding = encodingptr;
      if (Wptr != 0)
        num1 = Wptr;
      if (Hptr != 0)
        num2 = Hptr;
      int index1;
      if (num1 != 0)
      {
        index1 = 0;
        while (index1 < PdfBarcodeMatrix.ecc200matrix.Length && (PdfBarcodeMatrix.ecc200matrix[index1].W != num1 || PdfBarcodeMatrix.ecc200matrix[index1].H != num2))
          ++index1;
        if (index1 == PdfBarcodeMatrix.ecc200matrix.Length)
        {
          this.m_wszErrorMessage = string.Format("Invalid matrix size [{0} x {1}].", (object) num1, (object) num2);
          return (byte[]) null;
        }
        else if (encoding == null)
        {
          int lenp1 = 0;
          byte[] numArray2 = this.encmake(barcodelen, barcode, ref lenp1, '\x0001');
          if (numArray2 != null && lenp1 != PdfBarcodeMatrix.ecc200matrix[index1].Bytes)
          {
            numArray2 = this.encmake(barcodelen, barcode, ref lenp1, char.MinValue);
            if (lenp1 > PdfBarcodeMatrix.ecc200matrix[index1].Bytes)
            {
              this.m_wszErrorMessage = string.Format("Cannot make the barcode fit the matrix [{0} x {1}].", (object) num1, (object) num2);
              return (byte[]) null;
            }
          }
          encoding = numArray2;
        }
      }
      else
      {
        int lenp1 = 0;
        if (encoding == null)
          encoding = this.encmake(barcodelen, barcode, ref lenp1, '\x0001');
        if (encoding != null)
        {
          for (index1 = 0; PdfBarcodeMatrix.ecc200matrix[index1].W != 0; ++index1)
          {
            int lenp2 = 0;
            if ((int) this.ecc200encode(numArray1, PdfBarcodeMatrix.ecc200matrix[index1].Bytes, barcode, barcodelen, encoding, ref lenp2) != 0)
              break;
          }
        }
        else
        {
          int lenp2 = 0;
          byte[] numArray2 = this.encmake(barcodelen, barcode, ref lenp2, '\x0001');
          index1 = 0;
          while (PdfBarcodeMatrix.ecc200matrix[index1].W != 0 && PdfBarcodeMatrix.ecc200matrix[index1].Bytes != lenp2)
            ++index1;
          if (numArray2 != null && PdfBarcodeMatrix.ecc200matrix[index1].W == 0)
          {
            numArray2 = this.encmake(barcodelen, barcode, ref lenp2, char.MinValue);
            index1 = 0;
            while (PdfBarcodeMatrix.ecc200matrix[index1].W != 0 && PdfBarcodeMatrix.ecc200matrix[index1].Bytes < lenp2)
              ++index1;
          }
          encoding = numArray2;
        }
        if (PdfBarcodeMatrix.ecc200matrix[index1].W == 0)
        {
          this.m_wszErrorMessage = "Cannot find suitable matrix size, the barcode is too long.";
          return (byte[]) null;
        }
        else
        {
          num1 = PdfBarcodeMatrix.ecc200matrix[index1].W;
          num2 = PdfBarcodeMatrix.ecc200matrix[index1].H;
        }
      }
      if ((int) this.ecc200encode(numArray1, PdfBarcodeMatrix.ecc200matrix[index1].Bytes, barcode, barcodelen, encoding, ref lenp) == 0)
      {
        this.m_wszErrorMessage = string.Format("The barcode is too long to fit in the matrix [{0} x {1}].", (object) num1, (object) num2);
        return (byte[]) null;
      }
      else
      {
        this.ecc200(numArray1, PdfBarcodeMatrix.ecc200matrix[index1].Bytes, PdfBarcodeMatrix.ecc200matrix[index1].Datablock, PdfBarcodeMatrix.ecc200matrix[index1].RSblock);
        int NC = num1 - 2 * (num1 / PdfBarcodeMatrix.ecc200matrix[index1].FW);
        int NR = num2 - 2 * (num2 / PdfBarcodeMatrix.ecc200matrix[index1].FH);
        int[] array = new int[NC * NR];
        this.ecc200placement(array, NR, NC);
        byte[] numArray2 = new byte[num1 * num2];
        int num3 = 0;
        while (num3 < num2)
        {
          for (int index2 = 0; index2 < num1; ++index2)
            numArray2[num3 * num1 + index2] = (byte) 1;
          int num4 = 0;
          while (num4 < num1)
          {
            numArray2[(num3 + PdfBarcodeMatrix.ecc200matrix[index1].FH - 1) * num1 + num4] = (byte) 1;
            num4 += 2;
          }
          num3 += PdfBarcodeMatrix.ecc200matrix[index1].FH;
        }
        int num5 = 0;
        while (num5 < num1)
        {
          for (int index2 = 0; index2 < num2; ++index2)
            numArray2[index2 * num1 + num5] = (byte) 1;
          int num4 = 0;
          while (num4 < num2)
          {
            numArray2[num4 * num1 + num5 + PdfBarcodeMatrix.ecc200matrix[index1].FW - 1] = (byte) 1;
            num4 += 2;
          }
          num5 += PdfBarcodeMatrix.ecc200matrix[index1].FW;
        }
        for (int index2 = 0; index2 < NR; ++index2)
        {
          for (int index3 = 0; index3 < NC; ++index3)
          {
            int num4 = array[(NR - index2 - 1) * NC + index3];
            if (num4 == 1 || num4 > 7 && ((int) numArray1[(num4 >> 3) - 1] & 1 << (num4 & 7)) != 0)
              numArray2[(1 + index2 + 2 * (index2 / (PdfBarcodeMatrix.ecc200matrix[index1].FH - 2))) * num1 + 1 + index3 + 2 * (index3 / (PdfBarcodeMatrix.ecc200matrix[index1].FW - 2))] = (byte) 1;
          }
        }
        Wptr = num1;
        Hptr = num2;
        encodingptr = encoding;
        maxp = PdfBarcodeMatrix.ecc200matrix[index1].Bytes;
        eccp = (PdfBarcodeMatrix.ecc200matrix[index1].Bytes + 2) / PdfBarcodeMatrix.ecc200matrix[index1].Datablock * PdfBarcodeMatrix.ecc200matrix[index1].RSblock;
        return numArray2;
      }
    }

    private bool isdigit(byte b)
    {
      if ((int) b >= 48)
        return (int) b <= 57;
      else
        return false;
    }

    private bool isupper(byte b)
    {
      if ((int) b >= 65)
        return (int) b <= 90;
      else
        return false;
    }

    private bool islower(byte b)
    {
      if ((int) b >= 97)
        return (int) b <= 122;
      else
        return false;
    }

    private char tolower(byte b)
    {
      return ((char) b).ToString().ToLower()[0];
    }
  }
}
