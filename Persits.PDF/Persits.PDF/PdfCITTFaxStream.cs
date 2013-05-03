// Type: Persits.PDF.PdfCITTFaxStream
// Assembly: Persits.PDF, Version=2.5.0.19989, Culture=neutral, PublicKeyToken=b8d8c63f1ff142a8
// Assembly location: C:\Users\hernan\Desktop\asppdf_net\Samples\Bin\Persits.PDF.dll

namespace Persits.PDF
{
  internal class PdfCITTFaxStream : PdfDecoderHelper
  {
    private const int twoDimPass = 0;
    private const int twoDimHoriz = 1;
    private const int twoDimVert0 = 2;
    private const int twoDimVertR1 = 3;
    private const int twoDimVertL1 = 4;
    private const int twoDimVertR2 = 5;
    private const int twoDimVertL2 = 6;
    private const int twoDimVertR3 = 7;
    private const int twoDimVertL3 = 8;
    private const int ccittEOL = -2;
    internal PdfStream str;
    private int encoding;
    private bool endOfLine;
    private bool byteAlign;
    private int columns;
    private int rows;
    private bool endOfBlock;
    private bool black;
    private bool eof;
    private bool nextLine2D;
    private int row;
    private int inputBuf;
    private int inputBits;
    private short[] refLine;
    private short[] codingLine;
    private int outputBits;
    private int buf;
    private bool err;
    private int a0i;
    internal static CITTCode[] twoDimTab1;
    internal static CITTCode[] whiteTab1;
    internal static CITTCode[] whiteTab2;
    internal static CITTCode[] blackTab1;
    internal static CITTCode[] blackTab2;
    internal static CITTCode[] blackTab3;

    static PdfCITTFaxStream()
    {
      PdfCITTFaxStream.twoDimTab1 = new CITTCode[128]
      {
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) 7, (short) 8),
        new CITTCode((short) 7, (short) 7),
        new CITTCode((short) 6, (short) 6),
        new CITTCode((short) 6, (short) 6),
        new CITTCode((short) 6, (short) 5),
        new CITTCode((short) 6, (short) 5),
        new CITTCode((short) 4, (short) 0),
        new CITTCode((short) 4, (short) 0),
        new CITTCode((short) 4, (short) 0),
        new CITTCode((short) 4, (short) 0),
        new CITTCode((short) 4, (short) 0),
        new CITTCode((short) 4, (short) 0),
        new CITTCode((short) 4, (short) 0),
        new CITTCode((short) 4, (short) 0),
        new CITTCode((short) 3, (short) 1),
        new CITTCode((short) 3, (short) 1),
        new CITTCode((short) 3, (short) 1),
        new CITTCode((short) 3, (short) 1),
        new CITTCode((short) 3, (short) 1),
        new CITTCode((short) 3, (short) 1),
        new CITTCode((short) 3, (short) 1),
        new CITTCode((short) 3, (short) 1),
        new CITTCode((short) 3, (short) 1),
        new CITTCode((short) 3, (short) 1),
        new CITTCode((short) 3, (short) 1),
        new CITTCode((short) 3, (short) 1),
        new CITTCode((short) 3, (short) 1),
        new CITTCode((short) 3, (short) 1),
        new CITTCode((short) 3, (short) 1),
        new CITTCode((short) 3, (short) 1),
        new CITTCode((short) 3, (short) 4),
        new CITTCode((short) 3, (short) 4),
        new CITTCode((short) 3, (short) 4),
        new CITTCode((short) 3, (short) 4),
        new CITTCode((short) 3, (short) 4),
        new CITTCode((short) 3, (short) 4),
        new CITTCode((short) 3, (short) 4),
        new CITTCode((short) 3, (short) 4),
        new CITTCode((short) 3, (short) 4),
        new CITTCode((short) 3, (short) 4),
        new CITTCode((short) 3, (short) 4),
        new CITTCode((short) 3, (short) 4),
        new CITTCode((short) 3, (short) 4),
        new CITTCode((short) 3, (short) 4),
        new CITTCode((short) 3, (short) 4),
        new CITTCode((short) 3, (short) 4),
        new CITTCode((short) 3, (short) 3),
        new CITTCode((short) 3, (short) 3),
        new CITTCode((short) 3, (short) 3),
        new CITTCode((short) 3, (short) 3),
        new CITTCode((short) 3, (short) 3),
        new CITTCode((short) 3, (short) 3),
        new CITTCode((short) 3, (short) 3),
        new CITTCode((short) 3, (short) 3),
        new CITTCode((short) 3, (short) 3),
        new CITTCode((short) 3, (short) 3),
        new CITTCode((short) 3, (short) 3),
        new CITTCode((short) 3, (short) 3),
        new CITTCode((short) 3, (short) 3),
        new CITTCode((short) 3, (short) 3),
        new CITTCode((short) 3, (short) 3),
        new CITTCode((short) 3, (short) 3),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2),
        new CITTCode((short) 1, (short) 2)
      };
      PdfCITTFaxStream.whiteTab1 = new CITTCode[32]
      {
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) 12, (short) -2),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) 11, (short) 1792),
        new CITTCode((short) 11, (short) 1792),
        new CITTCode((short) 12, (short) 1984),
        new CITTCode((short) 12, (short) 2048),
        new CITTCode((short) 12, (short) 2112),
        new CITTCode((short) 12, (short) 2176),
        new CITTCode((short) 12, (short) 2240),
        new CITTCode((short) 12, (short) 2304),
        new CITTCode((short) 11, (short) 1856),
        new CITTCode((short) 11, (short) 1856),
        new CITTCode((short) 11, (short) 1920),
        new CITTCode((short) 11, (short) 1920),
        new CITTCode((short) 12, (short) 2368),
        new CITTCode((short) 12, (short) 2432),
        new CITTCode((short) 12, (short) 2496),
        new CITTCode((short) 12, (short) 2560)
      };
      PdfCITTFaxStream.whiteTab2 = new CITTCode[512]
      {
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) 8, (short) 29),
        new CITTCode((short) 8, (short) 29),
        new CITTCode((short) 8, (short) 30),
        new CITTCode((short) 8, (short) 30),
        new CITTCode((short) 8, (short) 45),
        new CITTCode((short) 8, (short) 45),
        new CITTCode((short) 8, (short) 46),
        new CITTCode((short) 8, (short) 46),
        new CITTCode((short) 7, (short) 22),
        new CITTCode((short) 7, (short) 22),
        new CITTCode((short) 7, (short) 22),
        new CITTCode((short) 7, (short) 22),
        new CITTCode((short) 7, (short) 23),
        new CITTCode((short) 7, (short) 23),
        new CITTCode((short) 7, (short) 23),
        new CITTCode((short) 7, (short) 23),
        new CITTCode((short) 8, (short) 47),
        new CITTCode((short) 8, (short) 47),
        new CITTCode((short) 8, (short) 48),
        new CITTCode((short) 8, (short) 48),
        new CITTCode((short) 6, (short) 13),
        new CITTCode((short) 6, (short) 13),
        new CITTCode((short) 6, (short) 13),
        new CITTCode((short) 6, (short) 13),
        new CITTCode((short) 6, (short) 13),
        new CITTCode((short) 6, (short) 13),
        new CITTCode((short) 6, (short) 13),
        new CITTCode((short) 6, (short) 13),
        new CITTCode((short) 7, (short) 20),
        new CITTCode((short) 7, (short) 20),
        new CITTCode((short) 7, (short) 20),
        new CITTCode((short) 7, (short) 20),
        new CITTCode((short) 8, (short) 33),
        new CITTCode((short) 8, (short) 33),
        new CITTCode((short) 8, (short) 34),
        new CITTCode((short) 8, (short) 34),
        new CITTCode((short) 8, (short) 35),
        new CITTCode((short) 8, (short) 35),
        new CITTCode((short) 8, (short) 36),
        new CITTCode((short) 8, (short) 36),
        new CITTCode((short) 8, (short) 37),
        new CITTCode((short) 8, (short) 37),
        new CITTCode((short) 8, (short) 38),
        new CITTCode((short) 8, (short) 38),
        new CITTCode((short) 7, (short) 19),
        new CITTCode((short) 7, (short) 19),
        new CITTCode((short) 7, (short) 19),
        new CITTCode((short) 7, (short) 19),
        new CITTCode((short) 8, (short) 31),
        new CITTCode((short) 8, (short) 31),
        new CITTCode((short) 8, (short) 32),
        new CITTCode((short) 8, (short) 32),
        new CITTCode((short) 6, (short) 1),
        new CITTCode((short) 6, (short) 1),
        new CITTCode((short) 6, (short) 1),
        new CITTCode((short) 6, (short) 1),
        new CITTCode((short) 6, (short) 1),
        new CITTCode((short) 6, (short) 1),
        new CITTCode((short) 6, (short) 1),
        new CITTCode((short) 6, (short) 1),
        new CITTCode((short) 6, (short) 12),
        new CITTCode((short) 6, (short) 12),
        new CITTCode((short) 6, (short) 12),
        new CITTCode((short) 6, (short) 12),
        new CITTCode((short) 6, (short) 12),
        new CITTCode((short) 6, (short) 12),
        new CITTCode((short) 6, (short) 12),
        new CITTCode((short) 6, (short) 12),
        new CITTCode((short) 8, (short) 53),
        new CITTCode((short) 8, (short) 53),
        new CITTCode((short) 8, (short) 54),
        new CITTCode((short) 8, (short) 54),
        new CITTCode((short) 7, (short) 26),
        new CITTCode((short) 7, (short) 26),
        new CITTCode((short) 7, (short) 26),
        new CITTCode((short) 7, (short) 26),
        new CITTCode((short) 8, (short) 39),
        new CITTCode((short) 8, (short) 39),
        new CITTCode((short) 8, (short) 40),
        new CITTCode((short) 8, (short) 40),
        new CITTCode((short) 8, (short) 41),
        new CITTCode((short) 8, (short) 41),
        new CITTCode((short) 8, (short) 42),
        new CITTCode((short) 8, (short) 42),
        new CITTCode((short) 8, (short) 43),
        new CITTCode((short) 8, (short) 43),
        new CITTCode((short) 8, (short) 44),
        new CITTCode((short) 8, (short) 44),
        new CITTCode((short) 7, (short) 21),
        new CITTCode((short) 7, (short) 21),
        new CITTCode((short) 7, (short) 21),
        new CITTCode((short) 7, (short) 21),
        new CITTCode((short) 7, (short) 28),
        new CITTCode((short) 7, (short) 28),
        new CITTCode((short) 7, (short) 28),
        new CITTCode((short) 7, (short) 28),
        new CITTCode((short) 8, (short) 61),
        new CITTCode((short) 8, (short) 61),
        new CITTCode((short) 8, (short) 62),
        new CITTCode((short) 8, (short) 62),
        new CITTCode((short) 8, (short) 63),
        new CITTCode((short) 8, (short) 63),
        new CITTCode((short) 8, (short) 0),
        new CITTCode((short) 8, (short) 0),
        new CITTCode((short) 8, (short) 320),
        new CITTCode((short) 8, (short) 320),
        new CITTCode((short) 8, (short) 384),
        new CITTCode((short) 8, (short) 384),
        new CITTCode((short) 5, (short) 10),
        new CITTCode((short) 5, (short) 10),
        new CITTCode((short) 5, (short) 10),
        new CITTCode((short) 5, (short) 10),
        new CITTCode((short) 5, (short) 10),
        new CITTCode((short) 5, (short) 10),
        new CITTCode((short) 5, (short) 10),
        new CITTCode((short) 5, (short) 10),
        new CITTCode((short) 5, (short) 10),
        new CITTCode((short) 5, (short) 10),
        new CITTCode((short) 5, (short) 10),
        new CITTCode((short) 5, (short) 10),
        new CITTCode((short) 5, (short) 10),
        new CITTCode((short) 5, (short) 10),
        new CITTCode((short) 5, (short) 10),
        new CITTCode((short) 5, (short) 10),
        new CITTCode((short) 5, (short) 11),
        new CITTCode((short) 5, (short) 11),
        new CITTCode((short) 5, (short) 11),
        new CITTCode((short) 5, (short) 11),
        new CITTCode((short) 5, (short) 11),
        new CITTCode((short) 5, (short) 11),
        new CITTCode((short) 5, (short) 11),
        new CITTCode((short) 5, (short) 11),
        new CITTCode((short) 5, (short) 11),
        new CITTCode((short) 5, (short) 11),
        new CITTCode((short) 5, (short) 11),
        new CITTCode((short) 5, (short) 11),
        new CITTCode((short) 5, (short) 11),
        new CITTCode((short) 5, (short) 11),
        new CITTCode((short) 5, (short) 11),
        new CITTCode((short) 5, (short) 11),
        new CITTCode((short) 7, (short) 27),
        new CITTCode((short) 7, (short) 27),
        new CITTCode((short) 7, (short) 27),
        new CITTCode((short) 7, (short) 27),
        new CITTCode((short) 8, (short) 59),
        new CITTCode((short) 8, (short) 59),
        new CITTCode((short) 8, (short) 60),
        new CITTCode((short) 8, (short) 60),
        new CITTCode((short) 9, (short) 1472),
        new CITTCode((short) 9, (short) 1536),
        new CITTCode((short) 9, (short) 1600),
        new CITTCode((short) 9, (short) 1728),
        new CITTCode((short) 7, (short) 18),
        new CITTCode((short) 7, (short) 18),
        new CITTCode((short) 7, (short) 18),
        new CITTCode((short) 7, (short) 18),
        new CITTCode((short) 7, (short) 24),
        new CITTCode((short) 7, (short) 24),
        new CITTCode((short) 7, (short) 24),
        new CITTCode((short) 7, (short) 24),
        new CITTCode((short) 8, (short) 49),
        new CITTCode((short) 8, (short) 49),
        new CITTCode((short) 8, (short) 50),
        new CITTCode((short) 8, (short) 50),
        new CITTCode((short) 8, (short) 51),
        new CITTCode((short) 8, (short) 51),
        new CITTCode((short) 8, (short) 52),
        new CITTCode((short) 8, (short) 52),
        new CITTCode((short) 7, (short) 25),
        new CITTCode((short) 7, (short) 25),
        new CITTCode((short) 7, (short) 25),
        new CITTCode((short) 7, (short) 25),
        new CITTCode((short) 8, (short) 55),
        new CITTCode((short) 8, (short) 55),
        new CITTCode((short) 8, (short) 56),
        new CITTCode((short) 8, (short) 56),
        new CITTCode((short) 8, (short) 57),
        new CITTCode((short) 8, (short) 57),
        new CITTCode((short) 8, (short) 58),
        new CITTCode((short) 8, (short) 58),
        new CITTCode((short) 6, (short) 192),
        new CITTCode((short) 6, (short) 192),
        new CITTCode((short) 6, (short) 192),
        new CITTCode((short) 6, (short) 192),
        new CITTCode((short) 6, (short) 192),
        new CITTCode((short) 6, (short) 192),
        new CITTCode((short) 6, (short) 192),
        new CITTCode((short) 6, (short) 192),
        new CITTCode((short) 6, (short) 1664),
        new CITTCode((short) 6, (short) 1664),
        new CITTCode((short) 6, (short) 1664),
        new CITTCode((short) 6, (short) 1664),
        new CITTCode((short) 6, (short) 1664),
        new CITTCode((short) 6, (short) 1664),
        new CITTCode((short) 6, (short) 1664),
        new CITTCode((short) 6, (short) 1664),
        new CITTCode((short) 8, (short) 448),
        new CITTCode((short) 8, (short) 448),
        new CITTCode((short) 8, (short) 512),
        new CITTCode((short) 8, (short) 512),
        new CITTCode((short) 9, (short) 704),
        new CITTCode((short) 9, (short) 768),
        new CITTCode((short) 8, (short) 640),
        new CITTCode((short) 8, (short) 640),
        new CITTCode((short) 8, (short) 576),
        new CITTCode((short) 8, (short) 576),
        new CITTCode((short) 9, (short) 832),
        new CITTCode((short) 9, (short) 896),
        new CITTCode((short) 9, (short) 960),
        new CITTCode((short) 9, (short) 1024),
        new CITTCode((short) 9, (short) 1088),
        new CITTCode((short) 9, (short) 1152),
        new CITTCode((short) 9, (short) 1216),
        new CITTCode((short) 9, (short) 1280),
        new CITTCode((short) 9, (short) 1344),
        new CITTCode((short) 9, (short) 1408),
        new CITTCode((short) 7, (short) 256),
        new CITTCode((short) 7, (short) 256),
        new CITTCode((short) 7, (short) 256),
        new CITTCode((short) 7, (short) 256),
        new CITTCode((short) 4, (short) 2),
        new CITTCode((short) 4, (short) 2),
        new CITTCode((short) 4, (short) 2),
        new CITTCode((short) 4, (short) 2),
        new CITTCode((short) 4, (short) 2),
        new CITTCode((short) 4, (short) 2),
        new CITTCode((short) 4, (short) 2),
        new CITTCode((short) 4, (short) 2),
        new CITTCode((short) 4, (short) 2),
        new CITTCode((short) 4, (short) 2),
        new CITTCode((short) 4, (short) 2),
        new CITTCode((short) 4, (short) 2),
        new CITTCode((short) 4, (short) 2),
        new CITTCode((short) 4, (short) 2),
        new CITTCode((short) 4, (short) 2),
        new CITTCode((short) 4, (short) 2),
        new CITTCode((short) 4, (short) 2),
        new CITTCode((short) 4, (short) 2),
        new CITTCode((short) 4, (short) 2),
        new CITTCode((short) 4, (short) 2),
        new CITTCode((short) 4, (short) 2),
        new CITTCode((short) 4, (short) 2),
        new CITTCode((short) 4, (short) 2),
        new CITTCode((short) 4, (short) 2),
        new CITTCode((short) 4, (short) 2),
        new CITTCode((short) 4, (short) 2),
        new CITTCode((short) 4, (short) 2),
        new CITTCode((short) 4, (short) 2),
        new CITTCode((short) 4, (short) 2),
        new CITTCode((short) 4, (short) 2),
        new CITTCode((short) 4, (short) 2),
        new CITTCode((short) 4, (short) 2),
        new CITTCode((short) 4, (short) 3),
        new CITTCode((short) 4, (short) 3),
        new CITTCode((short) 4, (short) 3),
        new CITTCode((short) 4, (short) 3),
        new CITTCode((short) 4, (short) 3),
        new CITTCode((short) 4, (short) 3),
        new CITTCode((short) 4, (short) 3),
        new CITTCode((short) 4, (short) 3),
        new CITTCode((short) 4, (short) 3),
        new CITTCode((short) 4, (short) 3),
        new CITTCode((short) 4, (short) 3),
        new CITTCode((short) 4, (short) 3),
        new CITTCode((short) 4, (short) 3),
        new CITTCode((short) 4, (short) 3),
        new CITTCode((short) 4, (short) 3),
        new CITTCode((short) 4, (short) 3),
        new CITTCode((short) 4, (short) 3),
        new CITTCode((short) 4, (short) 3),
        new CITTCode((short) 4, (short) 3),
        new CITTCode((short) 4, (short) 3),
        new CITTCode((short) 4, (short) 3),
        new CITTCode((short) 4, (short) 3),
        new CITTCode((short) 4, (short) 3),
        new CITTCode((short) 4, (short) 3),
        new CITTCode((short) 4, (short) 3),
        new CITTCode((short) 4, (short) 3),
        new CITTCode((short) 4, (short) 3),
        new CITTCode((short) 4, (short) 3),
        new CITTCode((short) 4, (short) 3),
        new CITTCode((short) 4, (short) 3),
        new CITTCode((short) 4, (short) 3),
        new CITTCode((short) 4, (short) 3),
        new CITTCode((short) 5, (short) 128),
        new CITTCode((short) 5, (short) 128),
        new CITTCode((short) 5, (short) 128),
        new CITTCode((short) 5, (short) 128),
        new CITTCode((short) 5, (short) 128),
        new CITTCode((short) 5, (short) 128),
        new CITTCode((short) 5, (short) 128),
        new CITTCode((short) 5, (short) 128),
        new CITTCode((short) 5, (short) 128),
        new CITTCode((short) 5, (short) 128),
        new CITTCode((short) 5, (short) 128),
        new CITTCode((short) 5, (short) 128),
        new CITTCode((short) 5, (short) 128),
        new CITTCode((short) 5, (short) 128),
        new CITTCode((short) 5, (short) 128),
        new CITTCode((short) 5, (short) 128),
        new CITTCode((short) 5, (short) 8),
        new CITTCode((short) 5, (short) 8),
        new CITTCode((short) 5, (short) 8),
        new CITTCode((short) 5, (short) 8),
        new CITTCode((short) 5, (short) 8),
        new CITTCode((short) 5, (short) 8),
        new CITTCode((short) 5, (short) 8),
        new CITTCode((short) 5, (short) 8),
        new CITTCode((short) 5, (short) 8),
        new CITTCode((short) 5, (short) 8),
        new CITTCode((short) 5, (short) 8),
        new CITTCode((short) 5, (short) 8),
        new CITTCode((short) 5, (short) 8),
        new CITTCode((short) 5, (short) 8),
        new CITTCode((short) 5, (short) 8),
        new CITTCode((short) 5, (short) 8),
        new CITTCode((short) 5, (short) 9),
        new CITTCode((short) 5, (short) 9),
        new CITTCode((short) 5, (short) 9),
        new CITTCode((short) 5, (short) 9),
        new CITTCode((short) 5, (short) 9),
        new CITTCode((short) 5, (short) 9),
        new CITTCode((short) 5, (short) 9),
        new CITTCode((short) 5, (short) 9),
        new CITTCode((short) 5, (short) 9),
        new CITTCode((short) 5, (short) 9),
        new CITTCode((short) 5, (short) 9),
        new CITTCode((short) 5, (short) 9),
        new CITTCode((short) 5, (short) 9),
        new CITTCode((short) 5, (short) 9),
        new CITTCode((short) 5, (short) 9),
        new CITTCode((short) 5, (short) 9),
        new CITTCode((short) 6, (short) 16),
        new CITTCode((short) 6, (short) 16),
        new CITTCode((short) 6, (short) 16),
        new CITTCode((short) 6, (short) 16),
        new CITTCode((short) 6, (short) 16),
        new CITTCode((short) 6, (short) 16),
        new CITTCode((short) 6, (short) 16),
        new CITTCode((short) 6, (short) 16),
        new CITTCode((short) 6, (short) 17),
        new CITTCode((short) 6, (short) 17),
        new CITTCode((short) 6, (short) 17),
        new CITTCode((short) 6, (short) 17),
        new CITTCode((short) 6, (short) 17),
        new CITTCode((short) 6, (short) 17),
        new CITTCode((short) 6, (short) 17),
        new CITTCode((short) 6, (short) 17),
        new CITTCode((short) 4, (short) 4),
        new CITTCode((short) 4, (short) 4),
        new CITTCode((short) 4, (short) 4),
        new CITTCode((short) 4, (short) 4),
        new CITTCode((short) 4, (short) 4),
        new CITTCode((short) 4, (short) 4),
        new CITTCode((short) 4, (short) 4),
        new CITTCode((short) 4, (short) 4),
        new CITTCode((short) 4, (short) 4),
        new CITTCode((short) 4, (short) 4),
        new CITTCode((short) 4, (short) 4),
        new CITTCode((short) 4, (short) 4),
        new CITTCode((short) 4, (short) 4),
        new CITTCode((short) 4, (short) 4),
        new CITTCode((short) 4, (short) 4),
        new CITTCode((short) 4, (short) 4),
        new CITTCode((short) 4, (short) 4),
        new CITTCode((short) 4, (short) 4),
        new CITTCode((short) 4, (short) 4),
        new CITTCode((short) 4, (short) 4),
        new CITTCode((short) 4, (short) 4),
        new CITTCode((short) 4, (short) 4),
        new CITTCode((short) 4, (short) 4),
        new CITTCode((short) 4, (short) 4),
        new CITTCode((short) 4, (short) 4),
        new CITTCode((short) 4, (short) 4),
        new CITTCode((short) 4, (short) 4),
        new CITTCode((short) 4, (short) 4),
        new CITTCode((short) 4, (short) 4),
        new CITTCode((short) 4, (short) 4),
        new CITTCode((short) 4, (short) 4),
        new CITTCode((short) 4, (short) 4),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 6, (short) 14),
        new CITTCode((short) 6, (short) 14),
        new CITTCode((short) 6, (short) 14),
        new CITTCode((short) 6, (short) 14),
        new CITTCode((short) 6, (short) 14),
        new CITTCode((short) 6, (short) 14),
        new CITTCode((short) 6, (short) 14),
        new CITTCode((short) 6, (short) 14),
        new CITTCode((short) 6, (short) 15),
        new CITTCode((short) 6, (short) 15),
        new CITTCode((short) 6, (short) 15),
        new CITTCode((short) 6, (short) 15),
        new CITTCode((short) 6, (short) 15),
        new CITTCode((short) 6, (short) 15),
        new CITTCode((short) 6, (short) 15),
        new CITTCode((short) 6, (short) 15),
        new CITTCode((short) 5, (short) 64),
        new CITTCode((short) 5, (short) 64),
        new CITTCode((short) 5, (short) 64),
        new CITTCode((short) 5, (short) 64),
        new CITTCode((short) 5, (short) 64),
        new CITTCode((short) 5, (short) 64),
        new CITTCode((short) 5, (short) 64),
        new CITTCode((short) 5, (short) 64),
        new CITTCode((short) 5, (short) 64),
        new CITTCode((short) 5, (short) 64),
        new CITTCode((short) 5, (short) 64),
        new CITTCode((short) 5, (short) 64),
        new CITTCode((short) 5, (short) 64),
        new CITTCode((short) 5, (short) 64),
        new CITTCode((short) 5, (short) 64),
        new CITTCode((short) 5, (short) 64),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 7),
        new CITTCode((short) 4, (short) 7),
        new CITTCode((short) 4, (short) 7),
        new CITTCode((short) 4, (short) 7),
        new CITTCode((short) 4, (short) 7),
        new CITTCode((short) 4, (short) 7),
        new CITTCode((short) 4, (short) 7),
        new CITTCode((short) 4, (short) 7),
        new CITTCode((short) 4, (short) 7),
        new CITTCode((short) 4, (short) 7),
        new CITTCode((short) 4, (short) 7),
        new CITTCode((short) 4, (short) 7),
        new CITTCode((short) 4, (short) 7),
        new CITTCode((short) 4, (short) 7),
        new CITTCode((short) 4, (short) 7),
        new CITTCode((short) 4, (short) 7),
        new CITTCode((short) 4, (short) 7),
        new CITTCode((short) 4, (short) 7),
        new CITTCode((short) 4, (short) 7),
        new CITTCode((short) 4, (short) 7),
        new CITTCode((short) 4, (short) 7),
        new CITTCode((short) 4, (short) 7),
        new CITTCode((short) 4, (short) 7),
        new CITTCode((short) 4, (short) 7),
        new CITTCode((short) 4, (short) 7),
        new CITTCode((short) 4, (short) 7),
        new CITTCode((short) 4, (short) 7),
        new CITTCode((short) 4, (short) 7),
        new CITTCode((short) 4, (short) 7),
        new CITTCode((short) 4, (short) 7),
        new CITTCode((short) 4, (short) 7),
        new CITTCode((short) 4, (short) 7)
      };
      PdfCITTFaxStream.blackTab1 = new CITTCode[128]
      {
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) 12, (short) -2),
        new CITTCode((short) 12, (short) -2),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) 11, (short) 1792),
        new CITTCode((short) 11, (short) 1792),
        new CITTCode((short) 11, (short) 1792),
        new CITTCode((short) 11, (short) 1792),
        new CITTCode((short) 12, (short) 1984),
        new CITTCode((short) 12, (short) 1984),
        new CITTCode((short) 12, (short) 2048),
        new CITTCode((short) 12, (short) 2048),
        new CITTCode((short) 12, (short) 2112),
        new CITTCode((short) 12, (short) 2112),
        new CITTCode((short) 12, (short) 2176),
        new CITTCode((short) 12, (short) 2176),
        new CITTCode((short) 12, (short) 2240),
        new CITTCode((short) 12, (short) 2240),
        new CITTCode((short) 12, (short) 2304),
        new CITTCode((short) 12, (short) 2304),
        new CITTCode((short) 11, (short) 1856),
        new CITTCode((short) 11, (short) 1856),
        new CITTCode((short) 11, (short) 1856),
        new CITTCode((short) 11, (short) 1856),
        new CITTCode((short) 11, (short) 1920),
        new CITTCode((short) 11, (short) 1920),
        new CITTCode((short) 11, (short) 1920),
        new CITTCode((short) 11, (short) 1920),
        new CITTCode((short) 12, (short) 2368),
        new CITTCode((short) 12, (short) 2368),
        new CITTCode((short) 12, (short) 2432),
        new CITTCode((short) 12, (short) 2432),
        new CITTCode((short) 12, (short) 2496),
        new CITTCode((short) 12, (short) 2496),
        new CITTCode((short) 12, (short) 2560),
        new CITTCode((short) 12, (short) 2560),
        new CITTCode((short) 10, (short) 18),
        new CITTCode((short) 10, (short) 18),
        new CITTCode((short) 10, (short) 18),
        new CITTCode((short) 10, (short) 18),
        new CITTCode((short) 10, (short) 18),
        new CITTCode((short) 10, (short) 18),
        new CITTCode((short) 10, (short) 18),
        new CITTCode((short) 10, (short) 18),
        new CITTCode((short) 12, (short) 52),
        new CITTCode((short) 12, (short) 52),
        new CITTCode((short) 13, (short) 640),
        new CITTCode((short) 13, (short) 704),
        new CITTCode((short) 13, (short) 768),
        new CITTCode((short) 13, (short) 832),
        new CITTCode((short) 12, (short) 55),
        new CITTCode((short) 12, (short) 55),
        new CITTCode((short) 12, (short) 56),
        new CITTCode((short) 12, (short) 56),
        new CITTCode((short) 13, (short) 1280),
        new CITTCode((short) 13, (short) 1344),
        new CITTCode((short) 13, (short) 1408),
        new CITTCode((short) 13, (short) 1472),
        new CITTCode((short) 12, (short) 59),
        new CITTCode((short) 12, (short) 59),
        new CITTCode((short) 12, (short) 60),
        new CITTCode((short) 12, (short) 60),
        new CITTCode((short) 13, (short) 1536),
        new CITTCode((short) 13, (short) 1600),
        new CITTCode((short) 11, (short) 24),
        new CITTCode((short) 11, (short) 24),
        new CITTCode((short) 11, (short) 24),
        new CITTCode((short) 11, (short) 24),
        new CITTCode((short) 11, (short) 25),
        new CITTCode((short) 11, (short) 25),
        new CITTCode((short) 11, (short) 25),
        new CITTCode((short) 11, (short) 25),
        new CITTCode((short) 13, (short) 1664),
        new CITTCode((short) 13, (short) 1728),
        new CITTCode((short) 12, (short) 320),
        new CITTCode((short) 12, (short) 320),
        new CITTCode((short) 12, (short) 384),
        new CITTCode((short) 12, (short) 384),
        new CITTCode((short) 12, (short) 448),
        new CITTCode((short) 12, (short) 448),
        new CITTCode((short) 13, (short) 512),
        new CITTCode((short) 13, (short) 576),
        new CITTCode((short) 12, (short) 53),
        new CITTCode((short) 12, (short) 53),
        new CITTCode((short) 12, (short) 54),
        new CITTCode((short) 12, (short) 54),
        new CITTCode((short) 13, (short) 896),
        new CITTCode((short) 13, (short) 960),
        new CITTCode((short) 13, (short) 1024),
        new CITTCode((short) 13, (short) 1088),
        new CITTCode((short) 13, (short) 1152),
        new CITTCode((short) 13, (short) 1216),
        new CITTCode((short) 10, (short) 64),
        new CITTCode((short) 10, (short) 64),
        new CITTCode((short) 10, (short) 64),
        new CITTCode((short) 10, (short) 64),
        new CITTCode((short) 10, (short) 64),
        new CITTCode((short) 10, (short) 64),
        new CITTCode((short) 10, (short) 64),
        new CITTCode((short) 10, (short) 64)
      };
      PdfCITTFaxStream.blackTab2 = new CITTCode[192]
      {
        new CITTCode((short) 8, (short) 13),
        new CITTCode((short) 8, (short) 13),
        new CITTCode((short) 8, (short) 13),
        new CITTCode((short) 8, (short) 13),
        new CITTCode((short) 8, (short) 13),
        new CITTCode((short) 8, (short) 13),
        new CITTCode((short) 8, (short) 13),
        new CITTCode((short) 8, (short) 13),
        new CITTCode((short) 8, (short) 13),
        new CITTCode((short) 8, (short) 13),
        new CITTCode((short) 8, (short) 13),
        new CITTCode((short) 8, (short) 13),
        new CITTCode((short) 8, (short) 13),
        new CITTCode((short) 8, (short) 13),
        new CITTCode((short) 8, (short) 13),
        new CITTCode((short) 8, (short) 13),
        new CITTCode((short) 11, (short) 23),
        new CITTCode((short) 11, (short) 23),
        new CITTCode((short) 12, (short) 50),
        new CITTCode((short) 12, (short) 51),
        new CITTCode((short) 12, (short) 44),
        new CITTCode((short) 12, (short) 45),
        new CITTCode((short) 12, (short) 46),
        new CITTCode((short) 12, (short) 47),
        new CITTCode((short) 12, (short) 57),
        new CITTCode((short) 12, (short) 58),
        new CITTCode((short) 12, (short) 61),
        new CITTCode((short) 12, (short) 256),
        new CITTCode((short) 10, (short) 16),
        new CITTCode((short) 10, (short) 16),
        new CITTCode((short) 10, (short) 16),
        new CITTCode((short) 10, (short) 16),
        new CITTCode((short) 10, (short) 17),
        new CITTCode((short) 10, (short) 17),
        new CITTCode((short) 10, (short) 17),
        new CITTCode((short) 10, (short) 17),
        new CITTCode((short) 12, (short) 48),
        new CITTCode((short) 12, (short) 49),
        new CITTCode((short) 12, (short) 62),
        new CITTCode((short) 12, (short) 63),
        new CITTCode((short) 12, (short) 30),
        new CITTCode((short) 12, (short) 31),
        new CITTCode((short) 12, (short) 32),
        new CITTCode((short) 12, (short) 33),
        new CITTCode((short) 12, (short) 40),
        new CITTCode((short) 12, (short) 41),
        new CITTCode((short) 11, (short) 22),
        new CITTCode((short) 11, (short) 22),
        new CITTCode((short) 8, (short) 14),
        new CITTCode((short) 8, (short) 14),
        new CITTCode((short) 8, (short) 14),
        new CITTCode((short) 8, (short) 14),
        new CITTCode((short) 8, (short) 14),
        new CITTCode((short) 8, (short) 14),
        new CITTCode((short) 8, (short) 14),
        new CITTCode((short) 8, (short) 14),
        new CITTCode((short) 8, (short) 14),
        new CITTCode((short) 8, (short) 14),
        new CITTCode((short) 8, (short) 14),
        new CITTCode((short) 8, (short) 14),
        new CITTCode((short) 8, (short) 14),
        new CITTCode((short) 8, (short) 14),
        new CITTCode((short) 8, (short) 14),
        new CITTCode((short) 8, (short) 14),
        new CITTCode((short) 7, (short) 10),
        new CITTCode((short) 7, (short) 10),
        new CITTCode((short) 7, (short) 10),
        new CITTCode((short) 7, (short) 10),
        new CITTCode((short) 7, (short) 10),
        new CITTCode((short) 7, (short) 10),
        new CITTCode((short) 7, (short) 10),
        new CITTCode((short) 7, (short) 10),
        new CITTCode((short) 7, (short) 10),
        new CITTCode((short) 7, (short) 10),
        new CITTCode((short) 7, (short) 10),
        new CITTCode((short) 7, (short) 10),
        new CITTCode((short) 7, (short) 10),
        new CITTCode((short) 7, (short) 10),
        new CITTCode((short) 7, (short) 10),
        new CITTCode((short) 7, (short) 10),
        new CITTCode((short) 7, (short) 10),
        new CITTCode((short) 7, (short) 10),
        new CITTCode((short) 7, (short) 10),
        new CITTCode((short) 7, (short) 10),
        new CITTCode((short) 7, (short) 10),
        new CITTCode((short) 7, (short) 10),
        new CITTCode((short) 7, (short) 10),
        new CITTCode((short) 7, (short) 10),
        new CITTCode((short) 7, (short) 10),
        new CITTCode((short) 7, (short) 10),
        new CITTCode((short) 7, (short) 10),
        new CITTCode((short) 7, (short) 10),
        new CITTCode((short) 7, (short) 10),
        new CITTCode((short) 7, (short) 10),
        new CITTCode((short) 7, (short) 10),
        new CITTCode((short) 7, (short) 10),
        new CITTCode((short) 7, (short) 11),
        new CITTCode((short) 7, (short) 11),
        new CITTCode((short) 7, (short) 11),
        new CITTCode((short) 7, (short) 11),
        new CITTCode((short) 7, (short) 11),
        new CITTCode((short) 7, (short) 11),
        new CITTCode((short) 7, (short) 11),
        new CITTCode((short) 7, (short) 11),
        new CITTCode((short) 7, (short) 11),
        new CITTCode((short) 7, (short) 11),
        new CITTCode((short) 7, (short) 11),
        new CITTCode((short) 7, (short) 11),
        new CITTCode((short) 7, (short) 11),
        new CITTCode((short) 7, (short) 11),
        new CITTCode((short) 7, (short) 11),
        new CITTCode((short) 7, (short) 11),
        new CITTCode((short) 7, (short) 11),
        new CITTCode((short) 7, (short) 11),
        new CITTCode((short) 7, (short) 11),
        new CITTCode((short) 7, (short) 11),
        new CITTCode((short) 7, (short) 11),
        new CITTCode((short) 7, (short) 11),
        new CITTCode((short) 7, (short) 11),
        new CITTCode((short) 7, (short) 11),
        new CITTCode((short) 7, (short) 11),
        new CITTCode((short) 7, (short) 11),
        new CITTCode((short) 7, (short) 11),
        new CITTCode((short) 7, (short) 11),
        new CITTCode((short) 7, (short) 11),
        new CITTCode((short) 7, (short) 11),
        new CITTCode((short) 7, (short) 11),
        new CITTCode((short) 7, (short) 11),
        new CITTCode((short) 9, (short) 15),
        new CITTCode((short) 9, (short) 15),
        new CITTCode((short) 9, (short) 15),
        new CITTCode((short) 9, (short) 15),
        new CITTCode((short) 9, (short) 15),
        new CITTCode((short) 9, (short) 15),
        new CITTCode((short) 9, (short) 15),
        new CITTCode((short) 9, (short) 15),
        new CITTCode((short) 12, (short) 128),
        new CITTCode((short) 12, (short) 192),
        new CITTCode((short) 12, (short) 26),
        new CITTCode((short) 12, (short) 27),
        new CITTCode((short) 12, (short) 28),
        new CITTCode((short) 12, (short) 29),
        new CITTCode((short) 11, (short) 19),
        new CITTCode((short) 11, (short) 19),
        new CITTCode((short) 11, (short) 20),
        new CITTCode((short) 11, (short) 20),
        new CITTCode((short) 12, (short) 34),
        new CITTCode((short) 12, (short) 35),
        new CITTCode((short) 12, (short) 36),
        new CITTCode((short) 12, (short) 37),
        new CITTCode((short) 12, (short) 38),
        new CITTCode((short) 12, (short) 39),
        new CITTCode((short) 11, (short) 21),
        new CITTCode((short) 11, (short) 21),
        new CITTCode((short) 12, (short) 42),
        new CITTCode((short) 12, (short) 43),
        new CITTCode((short) 10, (short) 0),
        new CITTCode((short) 10, (short) 0),
        new CITTCode((short) 10, (short) 0),
        new CITTCode((short) 10, (short) 0),
        new CITTCode((short) 7, (short) 12),
        new CITTCode((short) 7, (short) 12),
        new CITTCode((short) 7, (short) 12),
        new CITTCode((short) 7, (short) 12),
        new CITTCode((short) 7, (short) 12),
        new CITTCode((short) 7, (short) 12),
        new CITTCode((short) 7, (short) 12),
        new CITTCode((short) 7, (short) 12),
        new CITTCode((short) 7, (short) 12),
        new CITTCode((short) 7, (short) 12),
        new CITTCode((short) 7, (short) 12),
        new CITTCode((short) 7, (short) 12),
        new CITTCode((short) 7, (short) 12),
        new CITTCode((short) 7, (short) 12),
        new CITTCode((short) 7, (short) 12),
        new CITTCode((short) 7, (short) 12),
        new CITTCode((short) 7, (short) 12),
        new CITTCode((short) 7, (short) 12),
        new CITTCode((short) 7, (short) 12),
        new CITTCode((short) 7, (short) 12),
        new CITTCode((short) 7, (short) 12),
        new CITTCode((short) 7, (short) 12),
        new CITTCode((short) 7, (short) 12),
        new CITTCode((short) 7, (short) 12),
        new CITTCode((short) 7, (short) 12),
        new CITTCode((short) 7, (short) 12),
        new CITTCode((short) 7, (short) 12),
        new CITTCode((short) 7, (short) 12),
        new CITTCode((short) 7, (short) 12),
        new CITTCode((short) 7, (short) 12),
        new CITTCode((short) 7, (short) 12),
        new CITTCode((short) 7, (short) 12)
      };
      PdfCITTFaxStream.blackTab3 = new CITTCode[64]
      {
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) -1, (short) -1),
        new CITTCode((short) 6, (short) 9),
        new CITTCode((short) 6, (short) 8),
        new CITTCode((short) 5, (short) 7),
        new CITTCode((short) 5, (short) 7),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 6),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 4, (short) 5),
        new CITTCode((short) 3, (short) 1),
        new CITTCode((short) 3, (short) 1),
        new CITTCode((short) 3, (short) 1),
        new CITTCode((short) 3, (short) 1),
        new CITTCode((short) 3, (short) 1),
        new CITTCode((short) 3, (short) 1),
        new CITTCode((short) 3, (short) 1),
        new CITTCode((short) 3, (short) 1),
        new CITTCode((short) 3, (short) 4),
        new CITTCode((short) 3, (short) 4),
        new CITTCode((short) 3, (short) 4),
        new CITTCode((short) 3, (short) 4),
        new CITTCode((short) 3, (short) 4),
        new CITTCode((short) 3, (short) 4),
        new CITTCode((short) 3, (short) 4),
        new CITTCode((short) 3, (short) 4),
        new CITTCode((short) 2, (short) 3),
        new CITTCode((short) 2, (short) 3),
        new CITTCode((short) 2, (short) 3),
        new CITTCode((short) 2, (short) 3),
        new CITTCode((short) 2, (short) 3),
        new CITTCode((short) 2, (short) 3),
        new CITTCode((short) 2, (short) 3),
        new CITTCode((short) 2, (short) 3),
        new CITTCode((short) 2, (short) 3),
        new CITTCode((short) 2, (short) 3),
        new CITTCode((short) 2, (short) 3),
        new CITTCode((short) 2, (short) 3),
        new CITTCode((short) 2, (short) 3),
        new CITTCode((short) 2, (short) 3),
        new CITTCode((short) 2, (short) 3),
        new CITTCode((short) 2, (short) 3),
        new CITTCode((short) 2, (short) 2),
        new CITTCode((short) 2, (short) 2),
        new CITTCode((short) 2, (short) 2),
        new CITTCode((short) 2, (short) 2),
        new CITTCode((short) 2, (short) 2),
        new CITTCode((short) 2, (short) 2),
        new CITTCode((short) 2, (short) 2),
        new CITTCode((short) 2, (short) 2),
        new CITTCode((short) 2, (short) 2),
        new CITTCode((short) 2, (short) 2),
        new CITTCode((short) 2, (short) 2),
        new CITTCode((short) 2, (short) 2),
        new CITTCode((short) 2, (short) 2),
        new CITTCode((short) 2, (short) 2),
        new CITTCode((short) 2, (short) 2),
        new CITTCode((short) 2, (short) 2)
      };
    }

    internal PdfCITTFaxStream(PdfStream strA, PdfDict pDict, int nIndex)
    {
      this.encoding = 0;
      this.endOfLine = false;
      this.byteAlign = false;
      this.columns = 1728;
      this.rows = 0;
      this.endOfBlock = true;
      this.black = false;
      PdfObject pdfObject = pDict.GetObjectByName("DecodeParms") ?? pDict.GetObjectByName("DP");
      if (pdfObject != null && pdfObject.m_nType == enumType.pdfArray)
        pdfObject = ((PdfObjectList) pdfObject).GetObject(nIndex);
      if (pdfObject != null && pdfObject.m_nType == enumType.pdfDictionary)
      {
        PdfDict pdfDict = (PdfDict) pdfObject;
        PdfObject objectByName1 = pdfDict.GetObjectByName("K");
        if (objectByName1 != null && objectByName1.m_nType == enumType.pdfNumber)
          this.encoding = (int) ((PdfNumber) objectByName1).m_fValue;
        PdfObject objectByName2 = pdfDict.GetObjectByName("EndOfLine");
        if (objectByName2 != null && objectByName2.m_nType == enumType.pdfBool)
          this.endOfLine = ((PdfBool) objectByName2).m_bValue;
        PdfObject objectByName3 = pdfDict.GetObjectByName("EncodedByteAlign");
        if (objectByName3 != null && objectByName3.m_nType == enumType.pdfBool)
          this.byteAlign = ((PdfBool) objectByName3).m_bValue;
        PdfObject objectByName4 = pdfDict.GetObjectByName("Columns");
        if (objectByName4 != null && objectByName4.m_nType == enumType.pdfNumber)
          this.columns = (int) ((PdfNumber) objectByName4).m_fValue;
        PdfObject objectByName5 = pdfDict.GetObjectByName("Rows");
        if (objectByName5 != null && objectByName5.m_nType == enumType.pdfNumber)
          this.rows = (int) ((PdfNumber) objectByName5).m_fValue;
        PdfObject objectByName6 = pdfDict.GetObjectByName("EndOfBlock");
        if (objectByName6 != null && objectByName6.m_nType == enumType.pdfBool)
          this.endOfBlock = ((PdfBool) objectByName6).m_bValue;
        PdfObject objectByName7 = pdfDict.GetObjectByName("BlackIs1");
        if (objectByName7 != null && objectByName7.m_nType == enumType.pdfBool)
          this.black = ((PdfBool) objectByName7).m_bValue;
      }
      if (this.columns < 1)
        this.columns = 1;
      else if (this.columns > 2147483645)
        this.columns = 2147483645;
      this.refLine = new short[this.columns + 3];
      this.codingLine = new short[this.columns + 2];
      this.eof = false;
      this.row = 0;
      this.nextLine2D = this.encoding < 0;
      this.inputBits = 0;
      this.codingLine[0] = (short) 0;
      this.codingLine[1] = this.refLine[2] = (short) this.columns;
      this.buf = -1;
      this.a0i = 0;
      this.str = strA;
    }

    internal override int getChar()
    {
      int num = this.lookChar();
      this.buf = -1;
      return num;
    }

    private short getTwoDimCode()
    {
      if (this.endOfBlock)
      {
        int index;
        if ((index = (int) this.lookBits(7)) != -1)
        {
          CITTCode cittCode = PdfCITTFaxStream.twoDimTab1[index];
          if ((int) cittCode.bits > 0)
          {
            this.eatBits((int) cittCode.bits);
            return cittCode.n;
          }
        }
      }
      else
      {
        int index;
        for (int n = 1; n <= 7 && (index = (int) this.lookBits(n)) != -1; ++n)
        {
          if (n < 7)
            index <<= 7 - n;
          CITTCode cittCode = PdfCITTFaxStream.twoDimTab1[index];
          if ((int) cittCode.bits == n)
          {
            this.eatBits(n);
            return cittCode.n;
          }
        }
      }
      return (short) -1;
    }

    private short getWhiteCode()
    {
      if (this.endOfBlock)
      {
        short num = this.lookBits(12);
        if ((int) num == -1)
          return (short) 1;
        CITTCode cittCode = (int) num >> 5 != 0 ? PdfCITTFaxStream.whiteTab2[(int) num >> 3] : PdfCITTFaxStream.whiteTab1[(int) num];
        if ((int) cittCode.bits > 0)
        {
          this.eatBits((int) cittCode.bits);
          return cittCode.n;
        }
      }
      else
      {
        for (int n = 1; n <= 9; ++n)
        {
          short num = this.lookBits(n);
          if ((int) num == -1)
            return (short) 1;
          if (n < 9)
            num <<= 9 - n;
          CITTCode cittCode = PdfCITTFaxStream.whiteTab2[(int) num];
          if ((int) cittCode.bits == n)
          {
            this.eatBits(n);
            return cittCode.n;
          }
        }
        for (int n = 11; n <= 12; ++n)
        {
          short num = this.lookBits(n);
          if ((int) num == -1)
            return (short) 1;
          if (n < 12)
            num <<= 12 - n;
          CITTCode cittCode = PdfCITTFaxStream.whiteTab1[(int) num];
          if ((int) cittCode.bits == n)
          {
            this.eatBits(n);
            return cittCode.n;
          }
        }
      }
      this.eatBits(1);
      return (short) 1;
    }

    private short getBlackCode()
    {
      if (this.endOfBlock)
      {
        short num = this.lookBits(13);
        if ((int) num == -1)
          return (short) 1;
        CITTCode cittCode = (int) num >> 7 != 0 ? ((int) num >> 9 != 0 || (int) num >> 7 == 0 ? PdfCITTFaxStream.blackTab3[(int) num >> 7] : PdfCITTFaxStream.blackTab2[((int) num >> 1) - 64]) : PdfCITTFaxStream.blackTab1[(int) num];
        if ((int) cittCode.bits > 0)
        {
          this.eatBits((int) cittCode.bits);
          return cittCode.n;
        }
      }
      else
      {
        for (int n = 2; n <= 6; ++n)
        {
          short num = this.lookBits(n);
          if ((int) num == -1)
            return (short) 1;
          if (n < 6)
            num <<= 6 - n;
          CITTCode cittCode = PdfCITTFaxStream.blackTab3[(int) num];
          if ((int) cittCode.bits == n)
          {
            this.eatBits(n);
            return cittCode.n;
          }
        }
        for (int n = 7; n <= 12; ++n)
        {
          short num = this.lookBits(n);
          if ((int) num == -1)
            return (short) 1;
          if (n < 12)
            num <<= 12 - n;
          if ((int) num >= 64)
          {
            CITTCode cittCode = PdfCITTFaxStream.blackTab2[(int) num - 64];
            if ((int) cittCode.bits == n)
            {
              this.eatBits(n);
              return cittCode.n;
            }
          }
        }
        for (int n = 10; n <= 13; ++n)
        {
          short num = this.lookBits(n);
          if ((int) num == -1)
            return (short) 1;
          if (n < 13)
            num <<= 13 - n;
          CITTCode cittCode = PdfCITTFaxStream.blackTab1[(int) num];
          if ((int) cittCode.bits == n)
          {
            this.eatBits(n);
            return cittCode.n;
          }
        }
      }
      this.eatBits(1);
      return (short) 1;
    }

    private short lookBits(int n)
    {
      while (this.inputBits < n)
      {
        int @char;
        if ((@char = this.str.getChar()) == -1)
        {
          if (this.inputBits == 0)
            return (short) -1;
          else
            return (short) ((long) (this.inputBuf << n - this.inputBits) & (long) (uint.MaxValue >> 32 - n));
        }
        else
        {
          this.inputBuf = (this.inputBuf << 8) + @char;
          this.inputBits += 8;
        }
      }
      return (short) ((long) (this.inputBuf >> this.inputBits - n) & (long) (uint.MaxValue >> 32 - n));
    }

    private void eatBits(int n)
    {
      if ((this.inputBits -= n) >= 0)
        return;
      this.inputBits = 0;
    }

    internal void CopyTo(PdfStream pTo)
    {
      int @char;
      while ((@char = this.getChar()) != -1)
        pTo.AppendChar((byte) @char);
    }

    private void addPixels(int a1, int blackPixels)
    {
      if (a1 <= (int) this.codingLine[this.a0i])
        return;
      if (a1 > this.columns)
      {
        AuxException.Throw("CCITTFax row is wrong length.");
        this.err = true;
        a1 = this.columns;
      }
      if ((this.a0i & 1 ^ blackPixels) != 0)
        ++this.a0i;
      this.codingLine[this.a0i] = (short) a1;
    }

    private void addPixelsNeg(int a1, int blackPixels)
    {
      if (a1 > (int) this.codingLine[this.a0i])
      {
        if (a1 > this.columns)
        {
          AuxException.Throw("CCITTFax row is wrong length.");
          this.err = true;
          a1 = this.columns;
        }
        if ((this.a0i & 1 ^ blackPixels) != 0)
          ++this.a0i;
        this.codingLine[this.a0i] = (short) a1;
      }
      else
      {
        if (a1 >= (int) this.codingLine[this.a0i])
          return;
        if (a1 < 0)
        {
          AuxException.Throw("Invalid CCITTFax code.");
          this.err = true;
          a1 = 0;
        }
        while (this.a0i > 0 && a1 <= (int) this.codingLine[this.a0i - 1])
          --this.a0i;
        this.codingLine[this.a0i] = (short) a1;
      }
    }

    internal override void reset()
    {
      this.str.reset();
      this.eof = false;
      this.row = 0;
      this.nextLine2D = this.encoding < 0;
      this.inputBits = 0;
      this.codingLine[0] = (short) this.columns;
      this.a0i = 0;
      this.outputBits = 0;
      this.buf = -1;
      int num;
      while ((num = (int) this.lookBits(12)) == 0)
        this.eatBits(1);
      if (num == 1)
      {
        this.eatBits(12);
        this.endOfLine = true;
      }
      if (this.encoding <= 0)
        return;
      this.nextLine2D = (int) this.lookBits(1) == 0;
      this.eatBits(1);
    }

    private int lookChar()
    {
      if (this.buf != -1)
        return this.buf;
      if (this.outputBits == 0)
      {
        if (this.eof)
          return -1;
        this.err = false;
        if (this.nextLine2D)
        {
          int index1;
          for (index1 = 0; (int) this.codingLine[index1] < this.columns; ++index1)
            this.refLine[index1] = this.codingLine[index1];
          short[] numArray = this.refLine;
          int index2 = index1;
          int num1 = 1;
          int index3 = index2 + num1;
          int num2 = (int) (short) this.columns;
          numArray[index2] = (short) num2;
          this.refLine[index3] = (short) this.columns;
          this.codingLine[0] = (short) 0;
          this.a0i = 0;
          int index4 = 0;
          int blackPixels = 0;
label_58:
          while ((int) this.codingLine[this.a0i] < this.columns)
          {
            switch (this.getTwoDimCode())
            {
              case (short) -1:
                this.addPixels(this.columns, 0);
                this.eof = true;
                continue;
              case (short) 0:
                this.addPixels((int) this.refLine[index4 + 1], blackPixels);
                if ((int) this.refLine[index4 + 1] < this.columns)
                {
                  index4 += 2;
                  continue;
                }
                else
                  continue;
              case (short) 1:
                int num3;
                int num4 = num3 = 0;
                if (blackPixels != 0)
                {
                  int num5;
                  do
                  {
                    num4 += num5 = (int) this.getBlackCode();
                  }
                  while (num5 >= 64);
                  int num6;
                  do
                  {
                    num3 += num6 = (int) this.getWhiteCode();
                  }
                  while (num6 >= 64);
                }
                else
                {
                  int num5;
                  do
                  {
                    num4 += num5 = (int) this.getWhiteCode();
                  }
                  while (num5 >= 64);
                  int num6;
                  do
                  {
                    num3 += num6 = (int) this.getBlackCode();
                  }
                  while (num6 >= 64);
                }
                this.addPixels((int) this.codingLine[this.a0i] + num4, blackPixels);
                if ((int) this.codingLine[this.a0i] < this.columns)
                  this.addPixels((int) this.codingLine[this.a0i] + num3, blackPixels ^ 1);
                while (true)
                {
                  if ((int) this.refLine[index4] <= (int) this.codingLine[this.a0i] && (int) this.refLine[index4] < this.columns)
                    index4 += 2;
                  else
                    goto label_58;
                }
              case (short) 2:
                this.addPixels((int) this.refLine[index4], blackPixels);
                blackPixels ^= 1;
                if ((int) this.codingLine[this.a0i] < this.columns)
                {
                  ++index4;
                  while (true)
                  {
                    if ((int) this.refLine[index4] <= (int) this.codingLine[this.a0i] && (int) this.refLine[index4] < this.columns)
                      index4 += 2;
                    else
                      goto label_58;
                  }
                }
                else
                  continue;
              case (short) 3:
                this.addPixels((int) this.refLine[index4] + 1, blackPixels);
                blackPixels ^= 1;
                if ((int) this.codingLine[this.a0i] < this.columns)
                {
                  ++index4;
                  while (true)
                  {
                    if ((int) this.refLine[index4] <= (int) this.codingLine[this.a0i] && (int) this.refLine[index4] < this.columns)
                      index4 += 2;
                    else
                      goto label_58;
                  }
                }
                else
                  continue;
              case (short) 4:
                this.addPixelsNeg((int) this.refLine[index4] - 1, blackPixels);
                blackPixels ^= 1;
                if ((int) this.codingLine[this.a0i] < this.columns)
                {
                  if (index4 > 0)
                    --index4;
                  else
                    ++index4;
                  while (true)
                  {
                    if ((int) this.refLine[index4] <= (int) this.codingLine[this.a0i] && (int) this.refLine[index4] < this.columns)
                      index4 += 2;
                    else
                      goto label_58;
                  }
                }
                else
                  continue;
              case (short) 5:
                this.addPixels((int) this.refLine[index4] + 2, blackPixels);
                blackPixels ^= 1;
                if ((int) this.codingLine[this.a0i] < this.columns)
                {
                  ++index4;
                  while (true)
                  {
                    if ((int) this.refLine[index4] <= (int) this.codingLine[this.a0i] && (int) this.refLine[index4] < this.columns)
                      index4 += 2;
                    else
                      goto label_58;
                  }
                }
                else
                  continue;
              case (short) 6:
                this.addPixelsNeg((int) this.refLine[index4] - 2, blackPixels);
                blackPixels ^= 1;
                if ((int) this.codingLine[this.a0i] < this.columns)
                {
                  if (index4 > 0)
                    --index4;
                  else
                    ++index4;
                  while (true)
                  {
                    if ((int) this.refLine[index4] <= (int) this.codingLine[this.a0i] && (int) this.refLine[index4] < this.columns)
                      index4 += 2;
                    else
                      goto label_58;
                  }
                }
                else
                  continue;
              case (short) 7:
                this.addPixels((int) this.refLine[index4] + 3, blackPixels);
                blackPixels ^= 1;
                if ((int) this.codingLine[this.a0i] < this.columns)
                {
                  ++index4;
                  while (true)
                  {
                    if ((int) this.refLine[index4] <= (int) this.codingLine[this.a0i] && (int) this.refLine[index4] < this.columns)
                      index4 += 2;
                    else
                      goto label_58;
                  }
                }
                else
                  continue;
              case (short) 8:
                this.addPixelsNeg((int) this.refLine[index4] - 3, blackPixels);
                blackPixels ^= 1;
                if ((int) this.codingLine[this.a0i] < this.columns)
                {
                  if (index4 > 0)
                    --index4;
                  else
                    ++index4;
                  while (true)
                  {
                    if ((int) this.refLine[index4] <= (int) this.codingLine[this.a0i] && (int) this.refLine[index4] < this.columns)
                      index4 += 2;
                    else
                      goto label_58;
                  }
                }
                else
                  continue;
              default:
                AuxException.Throw("Invalid 2D code in CCITTFax stream");
                this.addPixels(this.columns, 0);
                this.err = true;
                continue;
            }
          }
        }
        else
        {
          this.codingLine[0] = (short) 0;
          this.a0i = 0;
          int blackPixels = 0;
          while ((int) this.codingLine[this.a0i] < this.columns)
          {
            int num1 = 0;
            if (blackPixels != 0)
            {
              int num2;
              do
              {
                num1 += num2 = (int) this.getBlackCode();
              }
              while (num2 >= 64);
            }
            else
            {
              int num2;
              do
              {
                num1 += num2 = (int) this.getWhiteCode();
              }
              while (num2 >= 64);
            }
            this.addPixels((int) this.codingLine[this.a0i] + num1, blackPixels);
            blackPixels ^= 1;
          }
        }
        bool flag = false;
        if (!this.endOfBlock && this.row == this.rows - 1)
          this.eof = true;
        else if (this.endOfLine || !this.byteAlign)
        {
          int num = (int) this.lookBits(12);
          if (this.endOfLine)
          {
            for (; num != -1 && num != 1; num = (int) this.lookBits(12))
              this.eatBits(1);
          }
          else
          {
            for (; num == 0; num = (int) this.lookBits(12))
              this.eatBits(1);
          }
          if (num == 1)
          {
            this.eatBits(12);
            flag = true;
          }
        }
        if (this.byteAlign && !flag)
          this.inputBits &= -8;
        if ((int) this.lookBits(1) == -1)
          this.eof = true;
        if (!this.eof && this.encoding > 0)
        {
          this.nextLine2D = (int) this.lookBits(1) == 0;
          this.eatBits(1);
        }
        if (this.endOfBlock && !this.endOfLine && (this.byteAlign && (int) this.lookBits(24) == 4097))
        {
          this.eatBits(12);
          flag = true;
        }
        if (this.endOfBlock && flag)
        {
          if ((int) this.lookBits(12) == 1)
          {
            this.eatBits(12);
            if (this.encoding > 0)
            {
              int num = (int) this.lookBits(1);
              this.eatBits(1);
            }
            if (this.encoding >= 0)
            {
              for (int index = 0; index < 4; ++index)
              {
                if ((int) this.lookBits(12) != 1)
                  AuxException.Throw("Invalid RTC code in CCITTFax stream");
                this.eatBits(12);
                if (this.encoding > 0)
                {
                  int num = (int) this.lookBits(1);
                  this.eatBits(1);
                }
              }
            }
            this.eof = true;
          }
        }
        else if (this.err && this.endOfLine)
        {
          int num;
          while (true)
          {
            num = (int) this.lookBits(13);
            if (num != -1)
            {
              if (num >> 1 != 1)
                this.eatBits(1);
              else
                goto label_101;
            }
            else
              break;
          }
          this.eof = true;
          return -1;
label_101:
          this.eatBits(12);
          if (this.encoding > 0)
          {
            this.eatBits(1);
            this.nextLine2D = (num & 1) == 0;
          }
        }
        this.outputBits = (int) this.codingLine[0] <= 0 ? (int) this.codingLine[this.a0i = 1] : (int) this.codingLine[this.a0i = 0];
        ++this.row;
      }
      if (this.outputBits >= 8)
      {
        this.buf = (this.a0i & 1) != 0 ? 0 : (int) byte.MaxValue;
        this.outputBits -= 8;
        if (this.outputBits == 0 && (int) this.codingLine[this.a0i] < this.columns)
        {
          ++this.a0i;
          this.outputBits = (int) this.codingLine[this.a0i] - (int) this.codingLine[this.a0i - 1];
        }
      }
      else
      {
        int num = 8;
        this.buf = 0;
        do
        {
          if (this.outputBits > num)
          {
            this.buf <<= num;
            if ((this.a0i & 1) == 0)
              this.buf |= (int) byte.MaxValue >> 8 - num;
            this.outputBits -= num;
            num = 0;
          }
          else
          {
            this.buf <<= this.outputBits;
            if ((this.a0i & 1) == 0)
              this.buf |= (int) byte.MaxValue >> 8 - this.outputBits;
            num -= this.outputBits;
            this.outputBits = 0;
            if ((int) this.codingLine[this.a0i] < this.columns)
            {
              ++this.a0i;
              this.outputBits = (int) this.codingLine[this.a0i] - (int) this.codingLine[this.a0i - 1];
            }
            else if (num > 0)
            {
              this.buf <<= num;
              num = 0;
            }
          }
        }
        while (num != 0);
      }
      if (this.black)
        this.buf ^= (int) byte.MaxValue;
      return this.buf;
    }
  }
}
