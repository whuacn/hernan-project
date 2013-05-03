// Type: Persits.PDF.zlib.InfBlocks
// Assembly: Persits.PDF, Version=2.5.0.19989, Culture=neutral, PublicKeyToken=b8d8c63f1ff142a8
// Assembly location: C:\Users\hernan\Desktop\asppdf_net\Samples\Bin\Persits.PDF.dll

using System;

namespace Persits.PDF.zlib
{
  internal sealed class InfBlocks
  {
    private const int MANY = 1440;
    private const int Z_OK = 0;
    private const int Z_STREAM_END = 1;
    private const int Z_NEED_DICT = 2;
    private const int Z_ERRNO = -1;
    private const int Z_STREAM_ERROR = -2;
    private const int Z_DATA_ERROR = -3;
    private const int Z_MEM_ERROR = -4;
    private const int Z_BUF_ERROR = -5;
    private const int Z_VERSION_ERROR = -6;
    private const int TYPE = 0;
    private const int LENS = 1;
    private const int STORED = 2;
    private const int TABLE = 3;
    private const int BTREE = 4;
    private const int DTREE = 5;
    private const int CODES = 6;
    private const int DRY = 7;
    private const int DONE = 8;
    private const int BAD = 9;
    private static readonly int[] inflate_mask;
    internal static readonly int[] border;
    internal int mode;
    internal int left;
    internal int table;
    internal int index;
    internal int[] blens;
    internal int[] bb;
    internal int[] tb;
    internal InfCodes codes;
    internal int last;
    internal int bitk;
    internal int bitb;
    internal int[] hufts;
    internal byte[] window;
    internal int end;
    internal int read;
    internal int write;
    internal object checkfn;
    internal long check;

    static InfBlocks()
    {
      InfBlocks.inflate_mask = new int[17]
      {
        0,
        1,
        3,
        7,
        15,
        31,
        63,
        (int) sbyte.MaxValue,
        (int) byte.MaxValue,
        511,
        1023,
        2047,
        4095,
        8191,
        16383,
        (int) short.MaxValue,
        (int) ushort.MaxValue
      };
      InfBlocks.border = new int[19]
      {
        16,
        17,
        18,
        0,
        8,
        7,
        9,
        6,
        10,
        5,
        11,
        4,
        12,
        3,
        13,
        2,
        14,
        1,
        15
      };
    }

    internal InfBlocks(ZStream z, object checkfn, int w)
    {
      this.bb = new int[1];
      this.tb = new int[1];
      this.hufts = new int[4320];
      this.window = new byte[w];
      this.end = w;
      this.checkfn = checkfn;
      this.mode = 0;
      this.reset(z, (long[]) null);
    }

    internal void reset(ZStream z, long[] c)
    {
      if (c != null)
        c[0] = this.check;
      if (this.mode == 4 || this.mode == 5)
        this.blens = (int[]) null;
      if (this.mode == 6)
        this.codes.free(z);
      this.mode = 0;
      this.bitk = 0;
      this.bitb = 0;
      this.read = this.write = 0;
      if (this.checkfn == null)
        return;
      z.adler = this.check = z._adler.adler32(0L, (byte[]) null, 0, 0);
    }

    internal int proc(ZStream z, int r)
    {
      int sourceIndex = z.next_in_index;
      int num1 = z.avail_in;
      int number1 = this.bitb;
      int num2 = this.bitk;
      int destinationIndex = this.write;
      int num3 = destinationIndex < this.read ? this.read - destinationIndex - 1 : this.end - destinationIndex;
      int num4;
      int num5;
      while (true)
      {
        int length = 0;
        do
        {
          switch (this.mode)
          {
            case 0:
              while (num2 < 3)
              {
                if (num1 != 0)
                {
                  r = 0;
                  --num1;
                  number1 |= ((int) z.next_in[sourceIndex++] & (int) byte.MaxValue) << num2;
                  num2 += 8;
                }
                else
                {
                  this.bitb = number1;
                  this.bitk = num2;
                  z.avail_in = num1;
                  z.total_in += (long) (sourceIndex - z.next_in_index);
                  z.next_in_index = sourceIndex;
                  this.write = destinationIndex;
                  return this.inflate_flush(z, r);
                }
              }
              int number2 = number1 & 7;
              this.last = number2 & 1;
              switch (SupportClass.URShift(number2, 1))
              {
                case 0:
                  int number3 = SupportClass.URShift(number1, 3);
                  int num6 = num2 - 3;
                  int bits1 = num6 & 7;
                  number1 = SupportClass.URShift(number3, bits1);
                  num2 = num6 - bits1;
                  this.mode = 1;
                  continue;
                case 1:
                  int[] bl1 = new int[1];
                  int[] bd1 = new int[1];
                  int[][] tl1 = new int[1][];
                  int[][] td1 = new int[1][];
                  InfTree.inflate_trees_fixed(bl1, bd1, tl1, td1, z);
                  this.codes = new InfCodes(bl1[0], bd1[0], tl1[0], td1[0], z);
                  number1 = SupportClass.URShift(number1, 3);
                  num2 -= 3;
                  this.mode = 6;
                  continue;
                case 2:
                  number1 = SupportClass.URShift(number1, 3);
                  num2 -= 3;
                  this.mode = 3;
                  continue;
                case 3:
                  int num7 = SupportClass.URShift(number1, 3);
                  int num8 = num2 - 3;
                  this.mode = 9;
                  z.msg = "invalid block type";
                  r = -3;
                  this.bitb = num7;
                  this.bitk = num8;
                  z.avail_in = num1;
                  z.total_in += (long) (sourceIndex - z.next_in_index);
                  z.next_in_index = sourceIndex;
                  this.write = destinationIndex;
                  return this.inflate_flush(z, r);
                default:
                  continue;
              }
            case 1:
              while (num2 < 32)
              {
                if (num1 != 0)
                {
                  r = 0;
                  --num1;
                  number1 |= ((int) z.next_in[sourceIndex++] & (int) byte.MaxValue) << num2;
                  num2 += 8;
                }
                else
                {
                  this.bitb = number1;
                  this.bitk = num2;
                  z.avail_in = num1;
                  z.total_in += (long) (sourceIndex - z.next_in_index);
                  z.next_in_index = sourceIndex;
                  this.write = destinationIndex;
                  return this.inflate_flush(z, r);
                }
              }
              if ((SupportClass.URShift(~number1, 16) & (int) ushort.MaxValue) != (number1 & (int) ushort.MaxValue))
              {
                this.mode = 9;
                z.msg = "invalid stored block lengths";
                r = -3;
                this.bitb = number1;
                this.bitk = num2;
                z.avail_in = num1;
                z.total_in += (long) (sourceIndex - z.next_in_index);
                z.next_in_index = sourceIndex;
                this.write = destinationIndex;
                return this.inflate_flush(z, r);
              }
              else
              {
                this.left = number1 & (int) ushort.MaxValue;
                number1 = num2 = 0;
                this.mode = this.left != 0 ? 2 : (this.last != 0 ? 7 : 0);
                continue;
              }
            case 2:
              if (num1 == 0)
              {
                this.bitb = number1;
                this.bitk = num2;
                z.avail_in = num1;
                z.total_in += (long) (sourceIndex - z.next_in_index);
                z.next_in_index = sourceIndex;
                this.write = destinationIndex;
                return this.inflate_flush(z, r);
              }
              else
              {
                if (num3 == 0)
                {
                  if (destinationIndex == this.end && this.read != 0)
                  {
                    destinationIndex = 0;
                    num3 = destinationIndex < this.read ? this.read - destinationIndex - 1 : this.end - destinationIndex;
                  }
                  if (num3 == 0)
                  {
                    this.write = destinationIndex;
                    r = this.inflate_flush(z, r);
                    destinationIndex = this.write;
                    num3 = destinationIndex < this.read ? this.read - destinationIndex - 1 : this.end - destinationIndex;
                    if (destinationIndex == this.end && this.read != 0)
                    {
                      destinationIndex = 0;
                      num3 = destinationIndex < this.read ? this.read - destinationIndex - 1 : this.end - destinationIndex;
                    }
                    if (num3 == 0)
                    {
                      this.bitb = number1;
                      this.bitk = num2;
                      z.avail_in = num1;
                      z.total_in += (long) (sourceIndex - z.next_in_index);
                      z.next_in_index = sourceIndex;
                      this.write = destinationIndex;
                      return this.inflate_flush(z, r);
                    }
                  }
                }
                r = 0;
                length = this.left;
                if (length > num1)
                  length = num1;
                if (length > num3)
                  length = num3;
                Array.Copy((Array) z.next_in, sourceIndex, (Array) this.window, destinationIndex, length);
                sourceIndex += length;
                num1 -= length;
                destinationIndex += length;
                num3 -= length;
                continue;
              }
            case 3:
              goto label_37;
            case 4:
              goto label_46;
            case 5:
              goto label_54;
            case 6:
              goto label_76;
            case 7:
              goto label_81;
            case 8:
              goto label_84;
            case 9:
              goto label_85;
            default:
              goto label_86;
          }
        }
        while ((this.left -= length) != 0);
        this.mode = this.last != 0 ? 7 : 0;
        continue;
label_37:
        while (num2 < 14)
        {
          if (num1 != 0)
          {
            r = 0;
            --num1;
            number1 |= ((int) z.next_in[sourceIndex++] & (int) byte.MaxValue) << num2;
            num2 += 8;
          }
          else
          {
            this.bitb = number1;
            this.bitk = num2;
            z.avail_in = num1;
            z.total_in += (long) (sourceIndex - z.next_in_index);
            z.next_in_index = sourceIndex;
            this.write = destinationIndex;
            return this.inflate_flush(z, r);
          }
        }
        int num9;
        this.table = num9 = number1 & 16383;
        if ((num9 & 31) <= 29 && (num9 >> 5 & 31) <= 29)
        {
          this.blens = new int[258 + (num9 & 31) + (num9 >> 5 & 31)];
          number1 = SupportClass.URShift(number1, 14);
          num2 -= 14;
          this.index = 0;
          this.mode = 4;
        }
        else
          break;
label_46:
        while (this.index < 4 + SupportClass.URShift(this.table, 10))
        {
          while (num2 < 3)
          {
            if (num1 != 0)
            {
              r = 0;
              --num1;
              number1 |= ((int) z.next_in[sourceIndex++] & (int) byte.MaxValue) << num2;
              num2 += 8;
            }
            else
            {
              this.bitb = number1;
              this.bitk = num2;
              z.avail_in = num1;
              z.total_in += (long) (sourceIndex - z.next_in_index);
              z.next_in_index = sourceIndex;
              this.write = destinationIndex;
              return this.inflate_flush(z, r);
            }
          }
          this.blens[InfBlocks.border[this.index++]] = number1 & 7;
          number1 = SupportClass.URShift(number1, 3);
          num2 -= 3;
        }
        while (this.index < 19)
          this.blens[InfBlocks.border[this.index++]] = 0;
        this.bb[0] = 7;
        num4 = InfTree.inflate_trees_bits(this.blens, this.bb, this.tb, this.hufts, z);
        if (num4 == 0)
        {
          this.index = 0;
          this.mode = 5;
        }
        else
          goto label_50;
label_54:
        while (true)
        {
          int num10 = this.table;
          if (this.index < 258 + (num10 & 31) + (num10 >> 5 & 31))
          {
            int index = this.bb[0];
            while (num2 < index)
            {
              if (num1 != 0)
              {
                r = 0;
                --num1;
                number1 |= ((int) z.next_in[sourceIndex++] & (int) byte.MaxValue) << num2;
                num2 += 8;
              }
              else
              {
                this.bitb = number1;
                this.bitk = num2;
                z.avail_in = num1;
                z.total_in += (long) (sourceIndex - z.next_in_index);
                z.next_in_index = sourceIndex;
                this.write = destinationIndex;
                return this.inflate_flush(z, r);
              }
            }
            int num11 = this.tb[0];
            int bits2 = this.hufts[(this.tb[0] + (number1 & InfBlocks.inflate_mask[index])) * 3 + 1];
            int num12 = this.hufts[(this.tb[0] + (number1 & InfBlocks.inflate_mask[bits2])) * 3 + 2];
            if (num12 < 16)
            {
              number1 = SupportClass.URShift(number1, bits2);
              num2 -= bits2;
              this.blens[this.index++] = num12;
            }
            else
            {
              int bits3 = num12 == 18 ? 7 : num12 - 14;
              int num13 = num12 == 18 ? 11 : 3;
              while (num2 < bits2 + bits3)
              {
                if (num1 != 0)
                {
                  r = 0;
                  --num1;
                  number1 |= ((int) z.next_in[sourceIndex++] & (int) byte.MaxValue) << num2;
                  num2 += 8;
                }
                else
                {
                  this.bitb = number1;
                  this.bitk = num2;
                  z.avail_in = num1;
                  z.total_in += (long) (sourceIndex - z.next_in_index);
                  z.next_in_index = sourceIndex;
                  this.write = destinationIndex;
                  return this.inflate_flush(z, r);
                }
              }
              int number4 = SupportClass.URShift(number1, bits2);
              int num14 = num2 - bits2;
              int num15 = num13 + (number4 & InfBlocks.inflate_mask[bits3]);
              number1 = SupportClass.URShift(number4, bits3);
              num2 = num14 - bits3;
              int num16 = this.index;
              int num17 = this.table;
              if (num16 + num15 <= 258 + (num17 & 31) + (num17 >> 5 & 31) && (num12 != 16 || num16 >= 1))
              {
                int num18 = num12 == 16 ? this.blens[num16 - 1] : 0;
                do
                {
                  this.blens[num16++] = num18;
                }
                while (--num15 != 0);
                this.index = num16;
              }
              else
                goto label_68;
            }
          }
          else
            break;
        }
        this.tb[0] = -1;
        int[] bl2 = new int[1];
        int[] bd2 = new int[1];
        int[] tl2 = new int[1];
        int[] td2 = new int[1];
        bl2[0] = 9;
        bd2[0] = 6;
        int num19 = this.table;
        num5 = InfTree.inflate_trees_dynamic(257 + (num19 & 31), 1 + (num19 >> 5 & 31), this.blens, bl2, bd2, tl2, td2, this.hufts, z);
        switch (num5)
        {
          case 0:
            this.codes = new InfCodes(bl2[0], bd2[0], this.hufts, tl2[0], this.hufts, td2[0], z);
            this.blens = (int[]) null;
            this.mode = 6;
            break;
          case -3:
            goto label_73;
          default:
            goto label_74;
        }
label_76:
        this.bitb = number1;
        this.bitk = num2;
        z.avail_in = num1;
        z.total_in += (long) (sourceIndex - z.next_in_index);
        z.next_in_index = sourceIndex;
        this.write = destinationIndex;
        if ((r = this.codes.proc(this, z, r)) == 1)
        {
          r = 0;
          this.codes.free(z);
          sourceIndex = z.next_in_index;
          num1 = z.avail_in;
          number1 = this.bitb;
          num2 = this.bitk;
          destinationIndex = this.write;
          num3 = destinationIndex < this.read ? this.read - destinationIndex - 1 : this.end - destinationIndex;
          if (this.last == 0)
            this.mode = 0;
          else
            goto label_80;
        }
        else
          goto label_77;
      }
      this.mode = 9;
      z.msg = "too many length or distance symbols";
      r = -3;
      this.bitb = number1;
      this.bitk = num2;
      z.avail_in = num1;
      z.total_in += (long) (sourceIndex - z.next_in_index);
      z.next_in_index = sourceIndex;
      this.write = destinationIndex;
      return this.inflate_flush(z, r);
label_50:
      r = num4;
      if (r == -3)
      {
        this.blens = (int[]) null;
        this.mode = 9;
      }
      this.bitb = number1;
      this.bitk = num2;
      z.avail_in = num1;
      z.total_in += (long) (sourceIndex - z.next_in_index);
      z.next_in_index = sourceIndex;
      this.write = destinationIndex;
      return this.inflate_flush(z, r);
label_68:
      this.blens = (int[]) null;
      this.mode = 9;
      z.msg = "invalid bit length repeat";
      r = -3;
      this.bitb = number1;
      this.bitk = num2;
      z.avail_in = num1;
      z.total_in += (long) (sourceIndex - z.next_in_index);
      z.next_in_index = sourceIndex;
      this.write = destinationIndex;
      return this.inflate_flush(z, r);
label_73:
      this.blens = (int[]) null;
      this.mode = 9;
label_74:
      r = num5;
      this.bitb = number1;
      this.bitk = num2;
      z.avail_in = num1;
      z.total_in += (long) (sourceIndex - z.next_in_index);
      z.next_in_index = sourceIndex;
      this.write = destinationIndex;
      return this.inflate_flush(z, r);
label_77:
      return this.inflate_flush(z, r);
label_80:
      this.mode = 7;
label_81:
      this.write = destinationIndex;
      r = this.inflate_flush(z, r);
      destinationIndex = this.write;
      int num20 = destinationIndex < this.read ? this.read - destinationIndex - 1 : this.end - destinationIndex;
      if (this.read != this.write)
      {
        this.bitb = number1;
        this.bitk = num2;
        z.avail_in = num1;
        z.total_in += (long) (sourceIndex - z.next_in_index);
        z.next_in_index = sourceIndex;
        this.write = destinationIndex;
        return this.inflate_flush(z, r);
      }
      else
        this.mode = 8;
label_84:
      r = 1;
      this.bitb = number1;
      this.bitk = num2;
      z.avail_in = num1;
      z.total_in += (long) (sourceIndex - z.next_in_index);
      z.next_in_index = sourceIndex;
      this.write = destinationIndex;
      return this.inflate_flush(z, r);
label_85:
      r = -3;
      this.bitb = number1;
      this.bitk = num2;
      z.avail_in = num1;
      z.total_in += (long) (sourceIndex - z.next_in_index);
      z.next_in_index = sourceIndex;
      this.write = destinationIndex;
      return this.inflate_flush(z, r);
label_86:
      r = -2;
      this.bitb = number1;
      this.bitk = num2;
      z.avail_in = num1;
      z.total_in += (long) (sourceIndex - z.next_in_index);
      z.next_in_index = sourceIndex;
      this.write = destinationIndex;
      return this.inflate_flush(z, r);
    }

    internal void free(ZStream z)
    {
      this.reset(z, (long[]) null);
      this.window = (byte[]) null;
      this.hufts = (int[]) null;
    }

    internal void set_dictionary(byte[] d, int start, int n)
    {
      Array.Copy((Array) d, start, (Array) this.window, 0, n);
      this.read = this.write = n;
    }

    internal int sync_point()
    {
      return this.mode != 1 ? 0 : 1;
    }

    internal int inflate_flush(ZStream z, int r)
    {
      int destinationIndex1 = z.next_out_index;
      int num1 = this.read;
      int num2 = (num1 <= this.write ? this.write : this.end) - num1;
      if (num2 > z.avail_out)
        num2 = z.avail_out;
      if (num2 != 0 && r == -5)
        r = 0;
      z.avail_out -= num2;
      z.total_out += (long) num2;
      if (this.checkfn != null)
        z.adler = this.check = z._adler.adler32(this.check, this.window, num1, num2);
      Array.Copy((Array) this.window, num1, (Array) z.next_out, destinationIndex1, num2);
      int destinationIndex2 = destinationIndex1 + num2;
      int num3 = num1 + num2;
      if (num3 == this.end)
      {
        int num4 = 0;
        if (this.write == this.end)
          this.write = 0;
        int num5 = this.write - num4;
        if (num5 > z.avail_out)
          num5 = z.avail_out;
        if (num5 != 0 && r == -5)
          r = 0;
        z.avail_out -= num5;
        z.total_out += (long) num5;
        if (this.checkfn != null)
          z.adler = this.check = z._adler.adler32(this.check, this.window, num4, num5);
        Array.Copy((Array) this.window, num4, (Array) z.next_out, destinationIndex2, num5);
        destinationIndex2 += num5;
        num3 = num4 + num5;
      }
      z.next_out_index = destinationIndex2;
      this.read = num3;
      return r;
    }
  }
}
