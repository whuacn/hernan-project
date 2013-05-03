// Type: Persits.PDF.PdfAES
// Assembly: Persits.PDF, Version=2.5.0.19989, Culture=neutral, PublicKeyToken=b8d8c63f1ff142a8
// Assembly location: C:\Users\hernan\Desktop\asppdf_net\Samples\Bin\Persits.PDF.dll

using System;

namespace Persits.PDF
{
  internal class PdfAES
  {
    internal const int gf2_8_poly = 283;
    internal int Nb;
    internal int Nk;
    internal int Nr;
    internal int C1;
    internal int C2;
    internal int C3;
    private byte[] state;
    private byte[] W;
    internal int state_size;
    internal uint poly32;
    internal uint poly32_inv;
    internal int[] parameters;
    private byte[] gf2_8_inv;
    private byte[] byte_sub;
    private byte[] inv_byte_sub;
    private uint[] Rcon;

    public PdfAES()
    {
      this.state = new byte[64];
      this.W = new byte[480];
      this.poly32 = 50397442U;
      this.poly32_inv = 185403662U;
      this.parameters = new int[36]
      {
        10,
        1,
        2,
        3,
        12,
        1,
        2,
        3,
        14,
        1,
        2,
        3,
        12,
        1,
        2,
        3,
        12,
        1,
        2,
        3,
        14,
        1,
        2,
        3,
        14,
        1,
        3,
        4,
        14,
        1,
        3,
        4,
        14,
        1,
        3,
        4
      };
      this.gf2_8_inv = new byte[256]
      {
        (byte) 0,
        (byte) 1,
        (byte) 141,
        (byte) 246,
        (byte) 203,
        (byte) 82,
        (byte) 123,
        (byte) 209,
        (byte) 232,
        (byte) 79,
        (byte) 41,
        (byte) 192,
        (byte) 176,
        (byte) 225,
        (byte) 229,
        (byte) 199,
        (byte) 116,
        (byte) 180,
        (byte) 170,
        (byte) 75,
        (byte) 153,
        (byte) 43,
        (byte) 96,
        (byte) 95,
        (byte) 88,
        (byte) 63,
        (byte) 253,
        (byte) 204,
        byte.MaxValue,
        (byte) 64,
        (byte) 238,
        (byte) 178,
        (byte) 58,
        (byte) 110,
        (byte) 90,
        (byte) 241,
        (byte) 85,
        (byte) 77,
        (byte) 168,
        (byte) 201,
        (byte) 193,
        (byte) 10,
        (byte) 152,
        (byte) 21,
        (byte) 48,
        (byte) 68,
        (byte) 162,
        (byte) 194,
        (byte) 44,
        (byte) 69,
        (byte) 146,
        (byte) 108,
        (byte) 243,
        (byte) 57,
        (byte) 102,
        (byte) 66,
        (byte) 242,
        (byte) 53,
        (byte) 32,
        (byte) 111,
        (byte) 119,
        (byte) 187,
        (byte) 89,
        (byte) 25,
        (byte) 29,
        (byte) 254,
        (byte) 55,
        (byte) 103,
        (byte) 45,
        (byte) 49,
        (byte) 245,
        (byte) 105,
        (byte) 167,
        (byte) 100,
        (byte) 171,
        (byte) 19,
        (byte) 84,
        (byte) 37,
        (byte) 233,
        (byte) 9,
        (byte) 237,
        (byte) 92,
        (byte) 5,
        (byte) 202,
        (byte) 76,
        (byte) 36,
        (byte) 135,
        (byte) 191,
        (byte) 24,
        (byte) 62,
        (byte) 34,
        (byte) 240,
        (byte) 81,
        (byte) 236,
        (byte) 97,
        (byte) 23,
        (byte) 22,
        (byte) 94,
        (byte) 175,
        (byte) 211,
        (byte) 73,
        (byte) 166,
        (byte) 54,
        (byte) 67,
        (byte) 244,
        (byte) 71,
        (byte) 145,
        (byte) 223,
        (byte) 51,
        (byte) 147,
        (byte) 33,
        (byte) 59,
        (byte) 121,
        (byte) 183,
        (byte) 151,
        (byte) 133,
        (byte) 16,
        (byte) 181,
        (byte) 186,
        (byte) 60,
        (byte) 182,
        (byte) 112,
        (byte) 208,
        (byte) 6,
        (byte) 161,
        (byte) 250,
        (byte) 129,
        (byte) 130,
        (byte) 131,
        (byte) 126,
        (byte) 127,
        (byte) 128,
        (byte) 150,
        (byte) 115,
        (byte) 190,
        (byte) 86,
        (byte) 155,
        (byte) 158,
        (byte) 149,
        (byte) 217,
        (byte) 247,
        (byte) 2,
        (byte) 185,
        (byte) 164,
        (byte) 222,
        (byte) 106,
        (byte) 50,
        (byte) 109,
        (byte) 216,
        (byte) 138,
        (byte) 132,
        (byte) 114,
        (byte) 42,
        (byte) 20,
        (byte) 159,
        (byte) 136,
        (byte) 249,
        (byte) 220,
        (byte) 137,
        (byte) 154,
        (byte) 251,
        (byte) 124,
        (byte) 46,
        (byte) 195,
        (byte) 143,
        (byte) 184,
        (byte) 101,
        (byte) 72,
        (byte) 38,
        (byte) 200,
        (byte) 18,
        (byte) 74,
        (byte) 206,
        (byte) 231,
        (byte) 210,
        (byte) 98,
        (byte) 12,
        (byte) 224,
        (byte) 31,
        (byte) 239,
        (byte) 17,
        (byte) 117,
        (byte) 120,
        (byte) 113,
        (byte) 165,
        (byte) 142,
        (byte) 118,
        (byte) 61,
        (byte) 189,
        (byte) 188,
        (byte) 134,
        (byte) 87,
        (byte) 11,
        (byte) 40,
        (byte) 47,
        (byte) 163,
        (byte) 218,
        (byte) 212,
        (byte) 228,
        (byte) 15,
        (byte) 169,
        (byte) 39,
        (byte) 83,
        (byte) 4,
        (byte) 27,
        (byte) 252,
        (byte) 172,
        (byte) 230,
        (byte) 122,
        (byte) 7,
        (byte) 174,
        (byte) 99,
        (byte) 197,
        (byte) 219,
        (byte) 226,
        (byte) 234,
        (byte) 148,
        (byte) 139,
        (byte) 196,
        (byte) 213,
        (byte) 157,
        (byte) 248,
        (byte) 144,
        (byte) 107,
        (byte) 177,
        (byte) 13,
        (byte) 214,
        (byte) 235,
        (byte) 198,
        (byte) 14,
        (byte) 207,
        (byte) 173,
        (byte) 8,
        (byte) 78,
        (byte) 215,
        (byte) 227,
        (byte) 93,
        (byte) 80,
        (byte) 30,
        (byte) 179,
        (byte) 91,
        (byte) 35,
        (byte) 56,
        (byte) 52,
        (byte) 104,
        (byte) 70,
        (byte) 3,
        (byte) 140,
        (byte) 221,
        (byte) 156,
        (byte) 125,
        (byte) 160,
        (byte) 205,
        (byte) 26,
        (byte) 65,
        (byte) 28
      };
      this.byte_sub = new byte[256]
      {
        (byte) 99,
        (byte) 124,
        (byte) 119,
        (byte) 123,
        (byte) 242,
        (byte) 107,
        (byte) 111,
        (byte) 197,
        (byte) 48,
        (byte) 1,
        (byte) 103,
        (byte) 43,
        (byte) 254,
        (byte) 215,
        (byte) 171,
        (byte) 118,
        (byte) 202,
        (byte) 130,
        (byte) 201,
        (byte) 125,
        (byte) 250,
        (byte) 89,
        (byte) 71,
        (byte) 240,
        (byte) 173,
        (byte) 212,
        (byte) 162,
        (byte) 175,
        (byte) 156,
        (byte) 164,
        (byte) 114,
        (byte) 192,
        (byte) 183,
        (byte) 253,
        (byte) 147,
        (byte) 38,
        (byte) 54,
        (byte) 63,
        (byte) 247,
        (byte) 204,
        (byte) 52,
        (byte) 165,
        (byte) 229,
        (byte) 241,
        (byte) 113,
        (byte) 216,
        (byte) 49,
        (byte) 21,
        (byte) 4,
        (byte) 199,
        (byte) 35,
        (byte) 195,
        (byte) 24,
        (byte) 150,
        (byte) 5,
        (byte) 154,
        (byte) 7,
        (byte) 18,
        (byte) 128,
        (byte) 226,
        (byte) 235,
        (byte) 39,
        (byte) 178,
        (byte) 117,
        (byte) 9,
        (byte) 131,
        (byte) 44,
        (byte) 26,
        (byte) 27,
        (byte) 110,
        (byte) 90,
        (byte) 160,
        (byte) 82,
        (byte) 59,
        (byte) 214,
        (byte) 179,
        (byte) 41,
        (byte) 227,
        (byte) 47,
        (byte) 132,
        (byte) 83,
        (byte) 209,
        (byte) 0,
        (byte) 237,
        (byte) 32,
        (byte) 252,
        (byte) 177,
        (byte) 91,
        (byte) 106,
        (byte) 203,
        (byte) 190,
        (byte) 57,
        (byte) 74,
        (byte) 76,
        (byte) 88,
        (byte) 207,
        (byte) 208,
        (byte) 239,
        (byte) 170,
        (byte) 251,
        (byte) 67,
        (byte) 77,
        (byte) 51,
        (byte) 133,
        (byte) 69,
        (byte) 249,
        (byte) 2,
        (byte) 127,
        (byte) 80,
        (byte) 60,
        (byte) 159,
        (byte) 168,
        (byte) 81,
        (byte) 163,
        (byte) 64,
        (byte) 143,
        (byte) 146,
        (byte) 157,
        (byte) 56,
        (byte) 245,
        (byte) 188,
        (byte) 182,
        (byte) 218,
        (byte) 33,
        (byte) 16,
        byte.MaxValue,
        (byte) 243,
        (byte) 210,
        (byte) 205,
        (byte) 12,
        (byte) 19,
        (byte) 236,
        (byte) 95,
        (byte) 151,
        (byte) 68,
        (byte) 23,
        (byte) 196,
        (byte) 167,
        (byte) 126,
        (byte) 61,
        (byte) 100,
        (byte) 93,
        (byte) 25,
        (byte) 115,
        (byte) 96,
        (byte) 129,
        (byte) 79,
        (byte) 220,
        (byte) 34,
        (byte) 42,
        (byte) 144,
        (byte) 136,
        (byte) 70,
        (byte) 238,
        (byte) 184,
        (byte) 20,
        (byte) 222,
        (byte) 94,
        (byte) 11,
        (byte) 219,
        (byte) 224,
        (byte) 50,
        (byte) 58,
        (byte) 10,
        (byte) 73,
        (byte) 6,
        (byte) 36,
        (byte) 92,
        (byte) 194,
        (byte) 211,
        (byte) 172,
        (byte) 98,
        (byte) 145,
        (byte) 149,
        (byte) 228,
        (byte) 121,
        (byte) 231,
        (byte) 200,
        (byte) 55,
        (byte) 109,
        (byte) 141,
        (byte) 213,
        (byte) 78,
        (byte) 169,
        (byte) 108,
        (byte) 86,
        (byte) 244,
        (byte) 234,
        (byte) 101,
        (byte) 122,
        (byte) 174,
        (byte) 8,
        (byte) 186,
        (byte) 120,
        (byte) 37,
        (byte) 46,
        (byte) 28,
        (byte) 166,
        (byte) 180,
        (byte) 198,
        (byte) 232,
        (byte) 221,
        (byte) 116,
        (byte) 31,
        (byte) 75,
        (byte) 189,
        (byte) 139,
        (byte) 138,
        (byte) 112,
        (byte) 62,
        (byte) 181,
        (byte) 102,
        (byte) 72,
        (byte) 3,
        (byte) 246,
        (byte) 14,
        (byte) 97,
        (byte) 53,
        (byte) 87,
        (byte) 185,
        (byte) 134,
        (byte) 193,
        (byte) 29,
        (byte) 158,
        (byte) 225,
        (byte) 248,
        (byte) 152,
        (byte) 17,
        (byte) 105,
        (byte) 217,
        (byte) 142,
        (byte) 148,
        (byte) 155,
        (byte) 30,
        (byte) 135,
        (byte) 233,
        (byte) 206,
        (byte) 85,
        (byte) 40,
        (byte) 223,
        (byte) 140,
        (byte) 161,
        (byte) 137,
        (byte) 13,
        (byte) 191,
        (byte) 230,
        (byte) 66,
        (byte) 104,
        (byte) 65,
        (byte) 153,
        (byte) 45,
        (byte) 15,
        (byte) 176,
        (byte) 84,
        (byte) 187,
        (byte) 22
      };
      this.inv_byte_sub = new byte[256]
      {
        (byte) 82,
        (byte) 9,
        (byte) 106,
        (byte) 213,
        (byte) 48,
        (byte) 54,
        (byte) 165,
        (byte) 56,
        (byte) 191,
        (byte) 64,
        (byte) 163,
        (byte) 158,
        (byte) 129,
        (byte) 243,
        (byte) 215,
        (byte) 251,
        (byte) 124,
        (byte) 227,
        (byte) 57,
        (byte) 130,
        (byte) 155,
        (byte) 47,
        byte.MaxValue,
        (byte) 135,
        (byte) 52,
        (byte) 142,
        (byte) 67,
        (byte) 68,
        (byte) 196,
        (byte) 222,
        (byte) 233,
        (byte) 203,
        (byte) 84,
        (byte) 123,
        (byte) 148,
        (byte) 50,
        (byte) 166,
        (byte) 194,
        (byte) 35,
        (byte) 61,
        (byte) 238,
        (byte) 76,
        (byte) 149,
        (byte) 11,
        (byte) 66,
        (byte) 250,
        (byte) 195,
        (byte) 78,
        (byte) 8,
        (byte) 46,
        (byte) 161,
        (byte) 102,
        (byte) 40,
        (byte) 217,
        (byte) 36,
        (byte) 178,
        (byte) 118,
        (byte) 91,
        (byte) 162,
        (byte) 73,
        (byte) 109,
        (byte) 139,
        (byte) 209,
        (byte) 37,
        (byte) 114,
        (byte) 248,
        (byte) 246,
        (byte) 100,
        (byte) 134,
        (byte) 104,
        (byte) 152,
        (byte) 22,
        (byte) 212,
        (byte) 164,
        (byte) 92,
        (byte) 204,
        (byte) 93,
        (byte) 101,
        (byte) 182,
        (byte) 146,
        (byte) 108,
        (byte) 112,
        (byte) 72,
        (byte) 80,
        (byte) 253,
        (byte) 237,
        (byte) 185,
        (byte) 218,
        (byte) 94,
        (byte) 21,
        (byte) 70,
        (byte) 87,
        (byte) 167,
        (byte) 141,
        (byte) 157,
        (byte) 132,
        (byte) 144,
        (byte) 216,
        (byte) 171,
        (byte) 0,
        (byte) 140,
        (byte) 188,
        (byte) 211,
        (byte) 10,
        (byte) 247,
        (byte) 228,
        (byte) 88,
        (byte) 5,
        (byte) 184,
        (byte) 179,
        (byte) 69,
        (byte) 6,
        (byte) 208,
        (byte) 44,
        (byte) 30,
        (byte) 143,
        (byte) 202,
        (byte) 63,
        (byte) 15,
        (byte) 2,
        (byte) 193,
        (byte) 175,
        (byte) 189,
        (byte) 3,
        (byte) 1,
        (byte) 19,
        (byte) 138,
        (byte) 107,
        (byte) 58,
        (byte) 145,
        (byte) 17,
        (byte) 65,
        (byte) 79,
        (byte) 103,
        (byte) 220,
        (byte) 234,
        (byte) 151,
        (byte) 242,
        (byte) 207,
        (byte) 206,
        (byte) 240,
        (byte) 180,
        (byte) 230,
        (byte) 115,
        (byte) 150,
        (byte) 172,
        (byte) 116,
        (byte) 34,
        (byte) 231,
        (byte) 173,
        (byte) 53,
        (byte) 133,
        (byte) 226,
        (byte) 249,
        (byte) 55,
        (byte) 232,
        (byte) 28,
        (byte) 117,
        (byte) 223,
        (byte) 110,
        (byte) 71,
        (byte) 241,
        (byte) 26,
        (byte) 113,
        (byte) 29,
        (byte) 41,
        (byte) 197,
        (byte) 137,
        (byte) 111,
        (byte) 183,
        (byte) 98,
        (byte) 14,
        (byte) 170,
        (byte) 24,
        (byte) 190,
        (byte) 27,
        (byte) 252,
        (byte) 86,
        (byte) 62,
        (byte) 75,
        (byte) 198,
        (byte) 210,
        (byte) 121,
        (byte) 32,
        (byte) 154,
        (byte) 219,
        (byte) 192,
        (byte) 254,
        (byte) 120,
        (byte) 205,
        (byte) 90,
        (byte) 244,
        (byte) 31,
        (byte) 221,
        (byte) 168,
        (byte) 51,
        (byte) 136,
        (byte) 7,
        (byte) 199,
        (byte) 49,
        (byte) 177,
        (byte) 18,
        (byte) 16,
        (byte) 89,
        (byte) 39,
        (byte) 128,
        (byte) 236,
        (byte) 95,
        (byte) 96,
        (byte) 81,
        (byte) 127,
        (byte) 169,
        (byte) 25,
        (byte) 181,
        (byte) 74,
        (byte) 13,
        (byte) 45,
        (byte) 229,
        (byte) 122,
        (byte) 159,
        (byte) 147,
        (byte) 201,
        (byte) 156,
        (byte) 239,
        (byte) 160,
        (byte) 224,
        (byte) 59,
        (byte) 77,
        (byte) 174,
        (byte) 42,
        (byte) 245,
        (byte) 176,
        (byte) 200,
        (byte) 235,
        (byte) 187,
        (byte) 60,
        (byte) 131,
        (byte) 83,
        (byte) 153,
        (byte) 97,
        (byte) 23,
        (byte) 43,
        (byte) 4,
        (byte) 126,
        (byte) 186,
        (byte) 119,
        (byte) 214,
        (byte) 38,
        (byte) 225,
        (byte) 105,
        (byte) 20,
        (byte) 99,
        (byte) 85,
        (byte) 33,
        (byte) 12,
        (byte) 125
      };
      this.Rcon = new uint[60]
      {
        0U,
        1U,
        2U,
        4U,
        8U,
        16U,
        32U,
        64U,
        128U,
        27U,
        54U,
        108U,
        216U,
        171U,
        77U,
        154U,
        47U,
        94U,
        188U,
        99U,
        198U,
        151U,
        53U,
        106U,
        212U,
        179U,
        125U,
        250U,
        239U,
        197U,
        145U,
        57U,
        114U,
        228U,
        211U,
        189U,
        97U,
        194U,
        159U,
        37U,
        74U,
        148U,
        51U,
        102U,
        204U,
        131U,
        29U,
        58U,
        116U,
        232U,
        203U,
        141U,
        1U,
        2U,
        4U,
        8U,
        16U,
        32U,
        64U,
        27U
      };
      this.CreateRijndaelTables(true, false);
    }

    private byte GF2_8_mult(byte a, byte b)
    {
      byte num1 = (byte) 0;
      int num2 = 8;
      while (num2-- != 0)
      {
        if (((int) b & 1) != 0)
          num1 ^= a;
        if (((int) a & 128) != 0)
        {
          a <<= 1;
          a ^= (byte) 27;
        }
        else
          a <<= 1;
        b >>= 1;
      }
      return num1;
    }

    private bool CheckInverses(bool create)
    {
      if (create)
        this.gf2_8_inv[0] = (byte) 0;
      else if ((int) this.gf2_8_inv[0] != 0)
        return false;
      for (uint index = 1U; index <= (uint) byte.MaxValue; ++index)
      {
        uint num = 1U;
        while ((int) this.GF2_8_mult((byte) index, (byte) num) != 1)
          ++num;
        if (create)
          this.gf2_8_inv[ index] = (byte) num;
        else if ((int) this.gf2_8_inv[ index] != (int) num)
          return false;
      }
      return true;
    }

    private byte BitSum(byte b)
    {
      b = (byte) ((int) b >> 4 ^ (int) b & 15);
      b = (byte) ((int) b >> 2 ^ (int) b & 3);
      return (byte) ((int) b >> 1 ^ (int) b & 1);
    }

    private bool CheckByteSub(bool create)
    {
      if (!this.CheckInverses(create))
        return false;
      for (uint index = 0U; index <= (uint) byte.MaxValue; ++index)
      {
        uint num1 = (uint) this.gf2_8_inv[index];
        uint num2 = (uint) ((int) this.BitSum((byte) (num1 & 241U)) | (int) this.BitSum((byte) (num1 & 227U)) << 1 | (int) this.BitSum((byte) (num1 & 199U)) << 2 | (int) this.BitSum((byte) (num1 & 143U)) << 3 | (int) this.BitSum((byte) (num1 & 31U)) << 4 | (int) this.BitSum((byte) (num1 & 62U)) << 5 | (int) this.BitSum((byte) (num1 & 124U)) << 6 | (int) this.BitSum((byte) (num1 & 248U)) << 7) ^ 99U;
        if (create)
          this.byte_sub[index] = (byte) num2;
        else if ((int) this.byte_sub[ index] != (int) num2)
          return false;
      }
      return true;
    }

    private bool CheckInvByteSub(bool create)
    {
      if (!this.CheckInverses(create) || !this.CheckByteSub(create))
        return false;
      for (uint index = 0U; index <= (uint) byte.MaxValue; ++index)
      {
        uint num = 0U;
        while ((int) this.byte_sub[num] != (int) index)
          ++num;
        if (create)
          this.inv_byte_sub[index] = (byte) num;
        else if ((int) this.inv_byte_sub[index] != (int) num)
          return false;
      }
      return true;
    }

    private bool CheckRcon(bool create)
    {
      byte a = (byte) 1;
      if (create)
        this.Rcon[0] = 0U;
      else if ((int) this.Rcon[0] != 0)
        return false;
      for (int index = 1; index < this.Rcon.Length - 1; ++index)
      {
        if (create)
          this.Rcon[index] = (uint) a;
        else if ((int) this.Rcon[index] != (int) a)
          return false;
        a = this.GF2_8_mult(a, (byte) 2);
      }
      return true;
    }

    private void ByteSub()
    {
      for (int index = 0; index < this.state_size; ++index)
        this.state[index] = this.byte_sub[(int) this.state[index]];
    }

    private void InvByteSub()
    {
      int index1 = 0;
      for (int index2 = 0; index2 < this.state_size; ++index2)
        this.state[index1] = this.inv_byte_sub[(int) this.state[index1++]];
    }

    private void ShiftRow()
    {
      byte[] numArray = new byte[10];
      int index1 = 0;
      int index2 = 1;
      for (; index1 < this.Nb; ++index1)
      {
        numArray[index1] = this.state[index2];
        index2 += 4;
      }
      int index3 = this.C1;
      int index4 = 1;
      for (; index3 < this.Nb; ++index3)
      {
        this.state[index4] = numArray[index3];
        index4 += 4;
      }
      int index5 = 0;
      int index6 = 1 + 4 * (this.Nb - this.C1);
      for (; index5 < this.C1; ++index5)
      {
        this.state[index6] = numArray[index5];
        index6 += 4;
      }
      int index7 = 0;
      int index8 = 2;
      for (; index7 < this.Nb; ++index7)
      {
        numArray[index7] = this.state[index8];
        index8 += 4;
      }
      int index9 = this.C2;
      int index10 = 2;
      for (; index9 < this.Nb; ++index9)
      {
        this.state[index10] = numArray[index9];
        index10 += 4;
      }
      int index11 = 0;
      int index12 = 2 + 4 * (this.Nb - this.C2);
      for (; index11 < this.C2; ++index11)
      {
        this.state[index12] = numArray[index11];
        index12 += 4;
      }
      int index13 = 0;
      int index14 = 3;
      for (; index13 < this.Nb; ++index13)
      {
        numArray[index13] = this.state[index14];
        index14 += 4;
      }
      int index15 = this.C3;
      int index16 = 3;
      for (; index15 < this.Nb; ++index15)
      {
        this.state[index16] = numArray[index15];
        index16 += 4;
      }
      int index17 = 0;
      int index18 = 3 + 4 * (this.Nb - this.C3);
      for (; index17 < this.C3; ++index17)
      {
        this.state[index18] = numArray[index17];
        index18 += 4;
      }
    }

    private void InvShiftRow()
    {
      byte[] numArray = new byte[10];
      int index1 = 0;
      int index2 = 1;
      for (; index1 < this.Nb; ++index1)
      {
        numArray[index1] = this.state[index2];
        index2 += 4;
      }
      int index3 = this.Nb - this.C1;
      int index4 = 1;
      for (; index3 < this.Nb; ++index3)
      {
        this.state[index4] = numArray[index3];
        index4 += 4;
      }
      int index5 = 0;
      int index6 = 1 + 4 * this.C1;
      for (; index5 < this.Nb - this.C1; ++index5)
      {
        this.state[index6] = numArray[index5];
        index6 += 4;
      }
      int index7 = 0;
      int index8 = 2;
      for (; index7 < this.Nb; ++index7)
      {
        numArray[index7] = this.state[index8];
        index8 += 4;
      }
      int index9 = this.Nb - this.C2;
      int index10 = 2;
      for (; index9 < this.Nb; ++index9)
      {
        this.state[index10] = numArray[index9];
        index10 += 4;
      }
      int index11 = 0;
      int index12 = 2 + 4 * this.C2;
      for (; index11 < this.Nb - this.C2; ++index11)
      {
        this.state[index12] = numArray[index11];
        index12 += 4;
      }
      int index13 = 0;
      int index14 = 3;
      for (; index13 < this.Nb; ++index13)
      {
        numArray[index13] = this.state[index14];
        index14 += 4;
      }
      int index15 = this.Nb - this.C3;
      int index16 = 3;
      for (; index15 < this.Nb; ++index15)
      {
        this.state[index16] = numArray[index15];
        index16 += 4;
      }
      int index17 = 0;
      int index18 = 3 + 4 * this.C3;
      for (; index17 < this.Nb - this.C3; ++index17)
      {
        this.state[index18] = numArray[index17];
        index18 += 4;
      }
    }

    private byte xmult(byte a)
    {
      return (byte) ((int) a << 1 ^ (((int) a & 128) != 0 ? 27 : 0));
    }

    private void MixColumn()
    {
      for (int index = 0; index < this.Nb; ++index)
      {
        byte a1 = this.state[index * 4];
        byte a2 = this.state[index * 4 + 1];
        byte a3 = this.state[index * 4 + 2];
        byte a4 = this.state[index * 4 + 3];
        byte num1 = (byte) ((uint) this.xmult(a1) ^ (uint) a2 ^ (uint) this.xmult(a2) ^ (uint) a3 ^ (uint) a4);
        byte num2 = (byte) ((uint) a1 ^ (uint) this.xmult(a2) ^ (uint) a3 ^ (uint) this.xmult(a3) ^ (uint) a4);
        byte num3 = (byte) ((uint) a1 ^ (uint) a2 ^ (uint) this.xmult(a3) ^ (uint) a4 ^ (uint) this.xmult(a4));
        byte num4 = (byte) ((uint) a1 ^ (uint) this.xmult(a1) ^ (uint) a2 ^ (uint) a3 ^ (uint) this.xmult(a4));
        this.state[index * 4] = num1;
        this.state[index * 4 + 1] = num2;
        this.state[index * 4 + 2] = num3;
        this.state[index * 4 + 3] = num4;
      }
    }

    private void InvMixColumn()
    {
      for (int index = 0; index < this.Nb; ++index)
      {
        byte b1 = this.state[4 * index];
        byte b2 = this.state[4 * index + 1];
        byte b3 = this.state[4 * index + 2];
        byte b4 = this.state[4 * index + 3];
        byte num1 = (byte) ((uint) this.GF2_8_mult((byte) 14, b1) ^ (uint) this.GF2_8_mult((byte) 11, b2) ^ (uint) this.GF2_8_mult((byte) 13, b3) ^ (uint) this.GF2_8_mult((byte) 9, b4));
        byte num2 = (byte) ((uint) this.GF2_8_mult((byte) 9, b1) ^ (uint) this.GF2_8_mult((byte) 14, b2) ^ (uint) this.GF2_8_mult((byte) 11, b3) ^ (uint) this.GF2_8_mult((byte) 13, b4));
        byte num3 = (byte) ((uint) this.GF2_8_mult((byte) 13, b1) ^ (uint) this.GF2_8_mult((byte) 9, b2) ^ (uint) this.GF2_8_mult((byte) 14, b3) ^ (uint) this.GF2_8_mult((byte) 11, b4));
        byte num4 = (byte) ((uint) this.GF2_8_mult((byte) 11, b1) ^ (uint) this.GF2_8_mult((byte) 13, b2) ^ (uint) this.GF2_8_mult((byte) 9, b3) ^ (uint) this.GF2_8_mult((byte) 14, b4));
        this.state[4 * index] = num1;
        this.state[4 * index + 1] = num2;
        this.state[4 * index + 2] = num3;
        this.state[4 * index + 3] = num4;
      }
    }

    private void AddRoundKey(int round)
    {
      int num1 = round * this.state_size;
      int num2 = 0;
      for (int index = 0; index < this.state_size; ++index)
        this.state[num2++] ^= this.W[num1++];
    }

    private void Round(int round)
    {
      this.ByteSub();
      this.ShiftRow();
      this.MixColumn();
      this.AddRoundKey(round);
    }

    private void InvRound(int round)
    {
      this.AddRoundKey(round);
      this.InvMixColumn();
      this.InvShiftRow();
      this.InvByteSub();
    }

    private void FinalRound(int round)
    {
      this.ByteSub();
      this.ShiftRow();
      this.AddRoundKey(round);
    }

    private void InvFinalRound(int round)
    {
      this.AddRoundKey(round);
      this.InvShiftRow();
      this.InvByteSub();
    }

    private uint RotByte(uint data)
    {
      return data << 24 | data >> 8;
    }

    private uint SubByte(uint data)
    {
      return (((uint) this.byte_sub[(data >> 24)] << 8 | (uint) this.byte_sub[(data >> 16 & (uint) byte.MaxValue)]) << 8 | (uint) this.byte_sub[(data >> 8 & (uint) byte.MaxValue)]) << 8 | (uint) this.byte_sub[(data & (uint) byte.MaxValue)];
    }

    private void KeyExpansion(byte[] key)
    {
      if (this.Nk > 6)
        return;
      for (int index = 0; index < 4 * this.Nk; ++index)
        this.W[index] = key[index];
      uint[] numArray = new uint[this.W.Length / 4];
      for (int index = 0; index < numArray.Length; ++index)
        numArray[index] = BitConverter.ToUInt32(this.W, 4 * index);
      for (int index = this.Nk; index < this.Nb * (this.Nr + 1); ++index)
      {
        uint data = numArray[index - 1];
        if (index % this.Nk == 0)
          data = this.SubByte(this.RotByte(data)) ^ this.Rcon[index / this.Nk];
        numArray[index] = numArray[index - this.Nk] ^ data;
      }
      for (int index1 = 0; index1 < numArray.Length; ++index1)
      {
        byte[] bytes = BitConverter.GetBytes(numArray[index1]);
        for (int index2 = 0; index2 < bytes.Length; ++index2)
          this.W[index1 * 4 + index2] = bytes[index2];
      }
    }

    public void SetParameters(int keylength, int blocklength)
    {
      this.Nk = this.Nr = this.Nb = 0;
      if (keylength != 128 && keylength != 192 && keylength != 256 || blocklength != 128 && blocklength != 192 && blocklength != 256)
        return;
      this.Nk = keylength / 32;
      this.Nb = blocklength / 32;
      this.state_size = 4 * this.Nb;
      this.Nr = this.parameters[((this.Nk - 4) / 2 + 3 * (this.Nb - 4) / 2) * 4];
      this.C1 = this.parameters[((this.Nk - 4) / 2 + 3 * (this.Nb - 4) / 2) * 4 + 1];
      this.C2 = this.parameters[((this.Nk - 4) / 2 + 3 * (this.Nb - 4) / 2) * 4 + 2];
      this.C3 = this.parameters[((this.Nk - 4) / 2 + 3 * (this.Nb - 4) / 2) * 4 + 3];
    }

    private bool CreateRijndaelTables(bool create, bool create_file)
    {
      bool flag = true;
      if (!this.CheckInverses(create))
        flag = false;
      if (!this.CheckByteSub(create))
        flag = false;
      if (!this.CheckInvByteSub(create))
        flag = false;
      if (!this.CheckRcon(create))
        return false;
      else
        return flag;
    }

    public void StartEncryption(byte[] key)
    {
      this.KeyExpansion(key);
    }

    private void EncryptBlock(byte[] datain1, int nOffsetIn, byte[] dataout1, int nOffsetOut)
    {
      Array.Copy((Array) datain1, nOffsetIn, (Array) this.state, 0, this.state_size);
      this.AddRoundKey(0);
      for (int round = 1; round < this.Nr; ++round)
        this.Round(round);
      this.FinalRound(this.Nr);
      Array.Copy((Array) this.state, 0, (Array) dataout1, nOffsetOut, this.state_size);
    }

    public void EncryptHelper(byte[] iv, byte[] datain, byte[] dataout, int numBlocks)
    {
      if (numBlocks == 0)
        return;
      int length = this.Nb * 4;
      byte[] datain1 = new byte[16];
      if (iv != null)
      {
        Array.Copy((Array) iv, (Array) datain1, iv.Length);
      }
      else
      {
        for (int index = 0; index < datain1.Length; ++index)
          datain1[index] = (byte) 0;
      }
      int num1 = 0;
      int num2 = 0;
      for (; numBlocks != 0; --numBlocks)
      {
        for (uint index = 0U; (long) index < (long) length; ++index)
          datain1[index] ^= datain[num1++];
        this.EncryptBlock(datain1, 0, dataout, num2);
        Array.Copy((Array) dataout, num2, (Array) datain1, 0, length);
        num2 += length;
      }
    }

    private void StartDecryption(byte[] key)
    {
      this.KeyExpansion(key);
    }

    private void DecryptHelper(byte[] iv, int nIVLen, byte[] datain, uint nOffsetIn, byte[] dataout, uint numBlocks)
    {
      if ((int) numBlocks == 0)
        return;
      uint num = (uint) (this.Nb * 4);
      byte[] numArray = new byte[16];
      if (iv != null)
      {
        Array.Copy((Array) iv, (Array) numArray, nIVLen);
      }
      else
      {
        for (int index = 0; index < numArray.Length; ++index)
          numArray[index] = (byte) 0;
      }
      uint nOffsetOut1 = 0U;
      this.DecryptBlock(datain, nOffsetIn, dataout, nOffsetOut1);
      for (uint index = 0U; index < num; ++index)
        dataout[(nOffsetOut1 + index)] ^= numArray[index];
      nOffsetIn += num;
      uint nOffsetOut2 = nOffsetOut1 + num;
      for (--numBlocks; (int) numBlocks != 0; --numBlocks)
      {
        this.DecryptBlock(datain, nOffsetIn, dataout, nOffsetOut2);
        for (uint index = 0U; index < num; ++index)
          dataout[(nOffsetOut2 + index)] ^= datain[(nOffsetIn - num + index)];
        nOffsetIn += num;
        nOffsetOut2 += num;
      }
    }

    private void DecryptBlock(byte[] datain1, uint nOffsetIn, byte[] dataout1, uint nOffsetOut)
    {
      Array.Copy((Array) datain1, (long) nOffsetIn, (Array) this.state, 0L, (long) this.state_size);
      this.InvFinalRound(this.Nr);
      for (int round = this.Nr - 1; round > 0; --round)
        this.InvRound(round);
      this.AddRoundKey(0);
      Array.Copy((Array) this.state, 0L, (Array) dataout1, (long) nOffsetOut, (long) this.state_size);
    }

    internal static void Encrypt(PdfStream pKey, int nKeySize, PdfStream pData)
    {
      PdfAES pdfAes = new PdfAES();
      pdfAes.SetParameters(nKeySize * 8, 128);
      pdfAes.StartEncryption(pKey.ToBytes());
      int num = 16 - pData.Length % 16;
      int Size = pData.Length + num;
      PdfStream pdfStream = new PdfStream();
      pdfStream.Alloc(Size);
      pdfStream.m_objMemStream.Write(pData.ToBytes(), 0, pData.Length);
      for (int index = 0; index < num; ++index)
        pdfStream[index + pData.Length] = (byte) num;
      PdfString pdfString = new PdfString();
      pdfString.SetRandomID();
      byte[] numArray = new byte[Size];
      pdfAes.EncryptHelper(pdfString.ToBytes(), pdfStream.ToBytes(), numArray, Size / 16);
      pData.Set((PdfStream) pdfString);
      pData.Append(numArray);
    }

    internal static bool Decrypt(PdfStream pKey, int nKeySize, PdfStream pData)
    {
      PdfAES pdfAes = new PdfAES();
      pdfAes.SetParameters(nKeySize * 8, 128);
      pdfAes.StartDecryption(pKey.ToBytes());
      if (pData.Length < 32)
        return false;
      byte[] numArray = new byte[pData.Length - 16];
      pdfAes.DecryptHelper(pData.ToBytes(), 16, pData.ToBytes(), 16U, numArray, (uint) (pData.Length / 16 - 1));
      byte num = numArray[numArray.Length - 1];
      if ((int) num > 16 || pData.Length < (int) num)
        return false;
      for (int index = numArray.Length - (int) num; index < numArray.Length; ++index)
      {
        if ((int) numArray[index] != (int) num)
          return false;
      }
      pData.Set(numArray, numArray.Length - (int) num);
      return true;
    }
  }
}
