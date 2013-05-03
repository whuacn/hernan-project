using System;
using System.Collections.Generic;
namespace Persits.PDF
{
	internal class PdfTiff
	{
		private static string[] m_arrModeNames = new string[]
		{
			"modePass",
			"modeH",
			"modeV",
			"modeVR1",
			"modeVR2",
			"modeVR3",
			"modeVL1",
			"modeVL2",
			"modeVL3",
			"modeExt",
			"modeError",
			"modeEOL"
		};
		private static TreeNode[] m_arrModes = new TreeNode[]
		{
			new TreeNode(-1, 1, 6),
			new TreeNode(-1, 2, 7),
			new TreeNode(-1, 3, 5),
			new TreeNode(-1, 9, 4),
			new TreeNode(0, -1, -1),
			new TreeNode(1, -1, -1),
			new TreeNode(2, -1, -1),
			new TreeNode(-1, 15, 8),
			new TreeNode(3, -1, -1),
			new TreeNode(-1, 12, 10),
			new TreeNode(-1, 16, 11),
			new TreeNode(4, -1, -1),
			new TreeNode(-1, 18, 13),
			new TreeNode(-1, 17, 14),
			new TreeNode(5, -1, -1),
			new TreeNode(6, -1, -1),
			new TreeNode(7, -1, -1),
			new TreeNode(8, -1, -1),
			new TreeNode(-1, 20, 19),
			new TreeNode(9, -1, -1),
			new TreeNode(-1, 21, -1),
			new TreeNode(-1, 22, -1),
			new TreeNode(-1, 23, -1),
			new TreeNode(-1, 24, -1),
			new TreeNode(-1, 26, 25),
			new TreeNode(11, -1, -1),
			new TreeNode(-1, 28, 27),
			new TreeNode(11, -1, -1),
			new TreeNode(-1, 30, 29),
			new TreeNode(11, -1, -1),
			new TreeNode(-1, 32, 31),
			new TreeNode(11, -1, -1),
			new TreeNode(-1, -1, 33),
			new TreeNode(11, -1, -1)
		};
		private static TreeNode[] m_arrWhite = new TreeNode[]
		{
			new TreeNode(-1, 1, 16),
			new TreeNode(-1, 2, 13),
			new TreeNode(-1, 9, 3),
			new TreeNode(-1, 36, 4),
			new TreeNode(-1, 5, 32),
			new TreeNode(-1, 75, 6),
			new TreeNode(-1, 7, 141),
			new TreeNode(-1, 133, 8),
			new TreeNode(0, -1, -1),
			new TreeNode(-1, 39, 10),
			new TreeNode(-1, 54, 11),
			new TreeNode(-1, 52, 12),
			new TreeNode(1, -1, -1),
			new TreeNode(-1, 33, 14),
			new TreeNode(-1, 137, 15),
			new TreeNode(2, -1, -1),
			new TreeNode(-1, 17, 22),
			new TreeNode(-1, 18, 20),
			new TreeNode(-1, 19, 28),
			new TreeNode(3, -1, -1),
			new TreeNode(-1, 30, 21),
			new TreeNode(4, -1, -1),
			new TreeNode(-1, 23, 25),
			new TreeNode(-1, 24, 42),
			new TreeNode(5, -1, -1),
			new TreeNode(-1, 26, 27),
			new TreeNode(6, -1, -1),
			new TreeNode(7, -1, -1),
			new TreeNode(-1, 135, 29),
			new TreeNode(8, -1, -1),
			new TreeNode(-1, 31, 46),
			new TreeNode(9, -1, -1),
			new TreeNode(10, -1, -1),
			new TreeNode(-1, 34, 65),
			new TreeNode(-1, 35, 49),
			new TreeNode(11, -1, -1),
			new TreeNode(-1, 37, 57),
			new TreeNode(-1, 38, 71),
			new TreeNode(12, -1, -1),
			new TreeNode(-1, 60, 40),
			new TreeNode(-1, 63, 41),
			new TreeNode(13, -1, -1),
			new TreeNode(-1, 43, 134),
			new TreeNode(-1, 44, 45),
			new TreeNode(14, -1, -1),
			new TreeNode(15, -1, -1),
			new TreeNode(-1, 47, 48),
			new TreeNode(16, -1, -1),
			new TreeNode(17, -1, -1),
			new TreeNode(-1, 73, 50),
			new TreeNode(-1, 174, 51),
			new TreeNode(18, -1, -1),
			new TreeNode(-1, 53, 81),
			new TreeNode(19, -1, -1),
			new TreeNode(-1, 55, 87),
			new TreeNode(-1, 56, 84),
			new TreeNode(20, -1, -1),
			new TreeNode(-1, 94, 58),
			new TreeNode(-1, 101, 59),
			new TreeNode(21, -1, -1),
			new TreeNode(-1, 77, 61),
			new TreeNode(-1, 104, 62),
			new TreeNode(22, -1, -1),
			new TreeNode(-1, 64, 107),
			new TreeNode(23, -1, -1),
			new TreeNode(-1, 66, 119),
			new TreeNode(-1, 67, 69),
			new TreeNode(-1, 68, 110),
			new TreeNode(24, -1, -1),
			new TreeNode(-1, 113, 70),
			new TreeNode(25, -1, -1),
			new TreeNode(-1, 116, 72),
			new TreeNode(26, -1, -1),
			new TreeNode(-1, 74, 127),
			new TreeNode(27, -1, -1),
			new TreeNode(-1, 76, 130),
			new TreeNode(28, -1, -1),
			new TreeNode(-1, 182, 78),
			new TreeNode(-1, 79, 80),
			new TreeNode(29, -1, -1),
			new TreeNode(30, -1, -1),
			new TreeNode(-1, 82, 83),
			new TreeNode(31, -1, -1),
			new TreeNode(32, -1, -1),
			new TreeNode(-1, 85, 86),
			new TreeNode(33, -1, -1),
			new TreeNode(34, -1, -1),
			new TreeNode(-1, 88, 91),
			new TreeNode(-1, 89, 90),
			new TreeNode(35, -1, -1),
			new TreeNode(36, -1, -1),
			new TreeNode(-1, 92, 93),
			new TreeNode(37, -1, -1),
			new TreeNode(38, -1, -1),
			new TreeNode(-1, 95, 98),
			new TreeNode(-1, 96, 97),
			new TreeNode(39, -1, -1),
			new TreeNode(40, -1, -1),
			new TreeNode(-1, 99, 100),
			new TreeNode(41, -1, -1),
			new TreeNode(42, -1, -1),
			new TreeNode(-1, 102, 103),
			new TreeNode(43, -1, -1),
			new TreeNode(44, -1, -1),
			new TreeNode(-1, 105, 106),
			new TreeNode(45, -1, -1),
			new TreeNode(46, -1, -1),
			new TreeNode(-1, 108, 109),
			new TreeNode(47, -1, -1),
			new TreeNode(48, -1, -1),
			new TreeNode(-1, 111, 112),
			new TreeNode(49, -1, -1),
			new TreeNode(50, -1, -1),
			new TreeNode(-1, 114, 115),
			new TreeNode(51, -1, -1),
			new TreeNode(52, -1, -1),
			new TreeNode(-1, 117, 118),
			new TreeNode(53, -1, -1),
			new TreeNode(54, -1, -1),
			new TreeNode(-1, 120, 136),
			new TreeNode(-1, 121, 124),
			new TreeNode(-1, 122, 123),
			new TreeNode(55, -1, -1),
			new TreeNode(56, -1, -1),
			new TreeNode(-1, 125, 126),
			new TreeNode(57, -1, -1),
			new TreeNode(58, -1, -1),
			new TreeNode(-1, 128, 129),
			new TreeNode(59, -1, -1),
			new TreeNode(60, -1, -1),
			new TreeNode(-1, 131, 132),
			new TreeNode(61, -1, -1),
			new TreeNode(62, -1, -1),
			new TreeNode(63, -1, -1),
			new TreeNode(64, -1, -1),
			new TreeNode(128, -1, -1),
			new TreeNode(192, -1, -1),
			new TreeNode(-1, 144, 138),
			new TreeNode(-1, 149, 139),
			new TreeNode(-1, 167, 140),
			new TreeNode(256, -1, -1),
			new TreeNode(-1, 142, 143),
			new TreeNode(320, -1, -1),
			new TreeNode(384, -1, -1),
			new TreeNode(-1, 180, 145),
			new TreeNode(-1, 146, 152),
			new TreeNode(-1, 147, 148),
			new TreeNode(448, -1, -1),
			new TreeNode(512, -1, -1),
			new TreeNode(-1, 150, 160),
			new TreeNode(-1, 151, 157),
			new TreeNode(576, -1, -1),
			new TreeNode(-1, 154, 153),
			new TreeNode(640, -1, -1),
			new TreeNode(-1, 155, 156),
			new TreeNode(704, -1, -1),
			new TreeNode(768, -1, -1),
			new TreeNode(-1, 158, 159),
			new TreeNode(832, -1, -1),
			new TreeNode(896, -1, -1),
			new TreeNode(-1, 161, 164),
			new TreeNode(-1, 162, 163),
			new TreeNode(960, -1, -1),
			new TreeNode(1024, -1, -1),
			new TreeNode(-1, 165, 166),
			new TreeNode(1088, -1, -1),
			new TreeNode(1152, -1, -1),
			new TreeNode(-1, 168, 171),
			new TreeNode(-1, 169, 170),
			new TreeNode(1216, -1, -1),
			new TreeNode(1280, -1, -1),
			new TreeNode(-1, 172, 173),
			new TreeNode(1344, -1, -1),
			new TreeNode(1408, -1, -1),
			new TreeNode(-1, 175, 178),
			new TreeNode(-1, 176, 177),
			new TreeNode(1472, -1, -1),
			new TreeNode(1536, -1, -1),
			new TreeNode(-1, 179, 181),
			new TreeNode(1600, -1, -1),
			new TreeNode(1664, -1, -1),
			new TreeNode(1728, -1, -1),
			new TreeNode(-1, 183, 188),
			new TreeNode(-1, 184, -1),
			new TreeNode(-1, 185, -1),
			new TreeNode(-1, 186, -1),
			new TreeNode(-1, -1, 187),
			new TreeNode(9999, -1, -1),
			new TreeNode(-1, 189, 192),
			new TreeNode(-1, 190, 199),
			new TreeNode(-1, 191, 196),
			new TreeNode(1792, -1, -1),
			new TreeNode(-1, 193, 206),
			new TreeNode(-1, 194, 195),
			new TreeNode(1856, -1, -1),
			new TreeNode(1920, -1, -1),
			new TreeNode(-1, 197, 198),
			new TreeNode(1984, -1, -1),
			new TreeNode(2048, -1, -1),
			new TreeNode(-1, 200, 203),
			new TreeNode(-1, 201, 202),
			new TreeNode(2112, -1, -1),
			new TreeNode(2176, -1, -1),
			new TreeNode(-1, 204, 205),
			new TreeNode(2240, -1, -1),
			new TreeNode(2304, -1, -1),
			new TreeNode(-1, 207, 210),
			new TreeNode(-1, 208, 209),
			new TreeNode(2368, -1, -1),
			new TreeNode(2432, -1, -1),
			new TreeNode(-1, 211, 212),
			new TreeNode(2496, -1, -1),
			new TreeNode(2560, -1, -1)
		};
		private static TreeNode[] m_arrBlack = new TreeNode[]
		{
			new TreeNode(-1, 1, 13),
			new TreeNode(-1, 2, 11),
			new TreeNode(-1, 3, 17),
			new TreeNode(-1, 4, 20),
			new TreeNode(-1, 29, 5),
			new TreeNode(-1, 25, 6),
			new TreeNode(-1, 7, 28),
			new TreeNode(-1, 35, 8),
			new TreeNode(-1, 51, 9),
			new TreeNode(-1, 54, 10),
			new TreeNode(0, -1, -1),
			new TreeNode(-1, 12, 16),
			new TreeNode(1, -1, -1),
			new TreeNode(-1, 15, 14),
			new TreeNode(2, -1, -1),
			new TreeNode(3, -1, -1),
			new TreeNode(4, -1, -1),
			new TreeNode(-1, 19, 18),
			new TreeNode(5, -1, -1),
			new TreeNode(6, -1, -1),
			new TreeNode(-1, 22, 21),
			new TreeNode(7, -1, -1),
			new TreeNode(-1, 24, 23),
			new TreeNode(8, -1, -1),
			new TreeNode(9, -1, -1),
			new TreeNode(-1, 26, 27),
			new TreeNode(10, -1, -1),
			new TreeNode(11, -1, -1),
			new TreeNode(12, -1, -1),
			new TreeNode(-1, 43, 30),
			new TreeNode(-1, 31, 33),
			new TreeNode(-1, 32, 37),
			new TreeNode(13, -1, -1),
			new TreeNode(-1, 40, 34),
			new TreeNode(14, -1, -1),
			new TreeNode(-1, 36, 48),
			new TreeNode(15, -1, -1),
			new TreeNode(-1, 59, 38),
			new TreeNode(-1, 128, 39),
			new TreeNode(16, -1, -1),
			new TreeNode(-1, 41, 56),
			new TreeNode(-1, 42, 106),
			new TreeNode(17, -1, -1),
			new TreeNode(-1, 182, 44),
			new TreeNode(-1, 45, 65),
			new TreeNode(-1, 46, 62),
			new TreeNode(-1, 47, 113),
			new TreeNode(18, -1, -1),
			new TreeNode(-1, 69, 49),
			new TreeNode(-1, 73, 50),
			new TreeNode(19, -1, -1),
			new TreeNode(-1, 52, 86),
			new TreeNode(-1, 53, 83),
			new TreeNode(20, -1, -1),
			new TreeNode(-1, 55, 96),
			new TreeNode(21, -1, -1),
			new TreeNode(-1, 76, 57),
			new TreeNode(-1, 93, 58),
			new TreeNode(22, -1, -1),
			new TreeNode(-1, 60, 99),
			new TreeNode(-1, 61, 110),
			new TreeNode(23, -1, -1),
			new TreeNode(-1, 125, 63),
			new TreeNode(-1, 134, 64),
			new TreeNode(24, -1, -1),
			new TreeNode(-1, 66, 119),
			new TreeNode(-1, 67, 116),
			new TreeNode(-1, 68, 146),
			new TreeNode(25, -1, -1),
			new TreeNode(-1, 142, 70),
			new TreeNode(-1, 71, 72),
			new TreeNode(26, -1, -1),
			new TreeNode(27, -1, -1),
			new TreeNode(-1, 74, 75),
			new TreeNode(28, -1, -1),
			new TreeNode(29, -1, -1),
			new TreeNode(-1, 77, 80),
			new TreeNode(-1, 78, 79),
			new TreeNode(30, -1, -1),
			new TreeNode(31, -1, -1),
			new TreeNode(-1, 81, 82),
			new TreeNode(32, -1, -1),
			new TreeNode(33, -1, -1),
			new TreeNode(-1, 84, 85),
			new TreeNode(34, -1, -1),
			new TreeNode(35, -1, -1),
			new TreeNode(-1, 87, 90),
			new TreeNode(-1, 88, 89),
			new TreeNode(36, -1, -1),
			new TreeNode(37, -1, -1),
			new TreeNode(-1, 91, 92),
			new TreeNode(38, -1, -1),
			new TreeNode(39, -1, -1),
			new TreeNode(-1, 94, 95),
			new TreeNode(40, -1, -1),
			new TreeNode(41, -1, -1),
			new TreeNode(-1, 97, 98),
			new TreeNode(42, -1, -1),
			new TreeNode(43, -1, -1),
			new TreeNode(-1, 100, 103),
			new TreeNode(-1, 101, 102),
			new TreeNode(44, -1, -1),
			new TreeNode(45, -1, -1),
			new TreeNode(-1, 104, 105),
			new TreeNode(46, -1, -1),
			new TreeNode(47, -1, -1),
			new TreeNode(-1, 107, 138),
			new TreeNode(-1, 108, 109),
			new TreeNode(48, -1, -1),
			new TreeNode(49, -1, -1),
			new TreeNode(-1, 111, 112),
			new TreeNode(50, -1, -1),
			new TreeNode(51, -1, -1),
			new TreeNode(-1, 114, 123),
			new TreeNode(-1, 115, 154),
			new TreeNode(52, -1, -1),
			new TreeNode(-1, 148, 117),
			new TreeNode(-1, 151, 118),
			new TreeNode(53, -1, -1),
			new TreeNode(-1, 120, 141),
			new TreeNode(-1, 121, 163),
			new TreeNode(-1, 122, 160),
			new TreeNode(54, -1, -1),
			new TreeNode(-1, 157, 124),
			new TreeNode(55, -1, -1),
			new TreeNode(-1, 126, 132),
			new TreeNode(-1, 127, 170),
			new TreeNode(56, -1, -1),
			new TreeNode(-1, 129, 136),
			new TreeNode(-1, 130, 131),
			new TreeNode(57, -1, -1),
			new TreeNode(58, -1, -1),
			new TreeNode(-1, 173, 133),
			new TreeNode(59, -1, -1),
			new TreeNode(-1, 135, 176),
			new TreeNode(60, -1, -1),
			new TreeNode(-1, 137, 145),
			new TreeNode(61, -1, -1),
			new TreeNode(-1, 139, 140),
			new TreeNode(62, -1, -1),
			new TreeNode(63, -1, -1),
			new TreeNode(64, -1, -1),
			new TreeNode(-1, 143, 144),
			new TreeNode(128, -1, -1),
			new TreeNode(192, -1, -1),
			new TreeNode(256, -1, -1),
			new TreeNode(-1, 179, 147),
			new TreeNode(320, -1, -1),
			new TreeNode(-1, 149, 150),
			new TreeNode(384, -1, -1),
			new TreeNode(448, -1, -1),
			new TreeNode(-1, 152, 153),
			new TreeNode(512, -1, -1),
			new TreeNode(576, -1, -1),
			new TreeNode(-1, 155, 156),
			new TreeNode(640, -1, -1),
			new TreeNode(704, -1, -1),
			new TreeNode(-1, 158, 159),
			new TreeNode(768, -1, -1),
			new TreeNode(832, -1, -1),
			new TreeNode(-1, 161, 162),
			new TreeNode(896, -1, -1),
			new TreeNode(960, -1, -1),
			new TreeNode(-1, 164, 167),
			new TreeNode(-1, 165, 166),
			new TreeNode(1024, -1, -1),
			new TreeNode(1088, -1, -1),
			new TreeNode(-1, 168, 169),
			new TreeNode(1152, -1, -1),
			new TreeNode(1216, -1, -1),
			new TreeNode(-1, 171, 172),
			new TreeNode(1280, -1, -1),
			new TreeNode(1344, -1, -1),
			new TreeNode(-1, 174, 175),
			new TreeNode(1408, -1, -1),
			new TreeNode(1472, -1, -1),
			new TreeNode(-1, 177, 178),
			new TreeNode(1536, -1, -1),
			new TreeNode(1600, -1, -1),
			new TreeNode(-1, 180, 181),
			new TreeNode(1664, -1, -1),
			new TreeNode(1728, -1, -1),
			new TreeNode(-1, 183, 187),
			new TreeNode(-1, 184, -1),
			new TreeNode(-1, 185, -1),
			new TreeNode(-1, 186, -1),
			new TreeNode(9999, -1, -1),
			new TreeNode(-1, 188, 191),
			new TreeNode(-1, 189, 198),
			new TreeNode(-1, 190, 195),
			new TreeNode(1792, -1, -1),
			new TreeNode(-1, 192, 205),
			new TreeNode(-1, 193, 194),
			new TreeNode(1856, -1, -1),
			new TreeNode(1920, -1, -1),
			new TreeNode(-1, 196, 197),
			new TreeNode(1984, -1, -1),
			new TreeNode(2048, -1, -1),
			new TreeNode(-1, 199, 202),
			new TreeNode(-1, 200, 201),
			new TreeNode(2112, -1, -1),
			new TreeNode(2176, -1, -1),
			new TreeNode(-1, 203, 204),
			new TreeNode(2240, -1, -1),
			new TreeNode(2304, -1, -1),
			new TreeNode(-1, 206, 209),
			new TreeNode(-1, 207, 208),
			new TreeNode(2368, -1, -1),
			new TreeNode(2432, -1, -1),
			new TreeNode(-1, 210, 211),
			new TreeNode(2496, -1, -1),
			new TreeNode(2560, -1, -1)
		};
		private bool m_bWhiteRun;
		private int m_nBitCount;
		private int m_nByteCount;
		private int m_nOutBitCount;
		private int m_nOutByteCount;
		private int[] sizeOfType = new int[]
		{
			0,
			1,
			1,
			2,
			4,
			8,
			1,
			1,
			2,
			4,
			8,
			4,
			8
		};
		public bool m_bBigEndian;
		public PdfManager m_pManager;
		public PdfDocument m_pDoc;
		public PdfInput m_pInput;
		public int m_nWidth;
		public int m_nHeight;
		public int m_nBitsPerComponent;
		public int m_nComponentsPerSample;
		public string m_bstrColorSpace;
		public float m_fDefaultScaleX;
		public float m_fDefaultScaleY;
		public float m_fResolutionX;
		public float m_fResolutionY;
		public int m_nExtraSamples;
		public TiffCompressions m_nCompression;
		public int m_nPlanarConfig;
		private bool m_bPalette;
		private byte[] m_pPaletteArray;
		public PdfStream m_objData = new PdfStream();
		public int m_dwPtr;
		public int m_nFillOrder;
		public int m_nImageIndex;
		private List<PdfTiffField> m_arrFields = new List<PdfTiffField>();
		private PdfLZW m_lzw = new PdfLZW();
		public bool m_bPageIndexOutOfRange;
		public void DecodeHuffman(byte[] Strip, ref byte[] Out, TiffCompressions nCompression)
		{
			PdfStream pdfStream = new PdfStream(10);
			this.m_nBitCount = 0;
			this.m_nByteCount = 0;
			this.m_nOutBitCount = 0;
			this.m_nOutByteCount = 0;
			short num = 0;
			int num2 = 0;
			TreeNode[] array = PdfTiff.m_arrWhite;
			this.m_bWhiteRun = true;
			int num3 = 0;
			int num4 = 0;
			while (this.NextBit(Strip, ref num))
			{
				int num5 = array[num2].m_nChild[(int)num];
				if (num5 != -1)
				{
					num2 = num5;
					int nValue = array[num2].m_nValue;
					if (nValue != -1)
					{
						num2 = 0;
						if (nValue != 9999)
						{
							num3 += nValue;
							num4 += nValue;
							if (nValue <= 63)
							{
                                this.PutBits(ref pdfStream, this.m_bWhiteRun ? (short)0 : (short)1, num3);
								this.m_bWhiteRun = !this.m_bWhiteRun;
								array = (this.m_bWhiteRun ? PdfTiff.m_arrWhite : PdfTiff.m_arrBlack);
								num3 = 0;
								if (num4 >= this.m_nWidth)
								{
									num4 = 0;
									this.m_bWhiteRun = true;
									array = PdfTiff.m_arrWhite;
									if (this.m_nBitCount != 0 && nCompression == TiffCompressions.TIFF_COMPRESSION_HUFFMAN)
									{
										this.m_nBitCount = 0;
										this.m_nByteCount++;
									}
									this.PutBits(ref pdfStream, 0, -1);
								}
							}
						}
					}
				}
			}
			Out = pdfStream.ToBytes();
		}
		private void DecodeGroup4(byte[] Strip, ref byte[] Out)
		{
			PdfStream pdfStream = new PdfStream(10);
			this.m_nBitCount = 0;
			this.m_nByteCount = 0;
			this.m_nOutBitCount = 0;
			this.m_nOutByteCount = 0;
			PdfTiffLine pdfTiffLine = new PdfTiffLine(this.m_nWidth);
			PdfTiffLine pdfTiffLine2 = new PdfTiffLine(this.m_nWidth);
			int num = -1;
			bool flag = true;
			int num2 = 0;
			while (flag)
			{
				int num3 = pdfTiffLine2.FindBit(num + 1, 1 - num2);
				int num4 = pdfTiffLine2.FindBit(num3 + 1, num2);
				Group4Modes mode = this.GetMode(Strip);
				switch (mode)
				{
				case Group4Modes.modePass:
					pdfTiffLine.PutBitsFromCurrentTo(num4, num2, false);
					num = num4;
					break;
				case Group4Modes.modeH:
				{
					int num5 = this.GetHuffmanSequence(Strip, num2 == 0);
					int huffmanSequence = this.GetHuffmanSequence(Strip, num2 != 0);
					pdfTiffLine.SetPointer((num == -1) ? 0 : num);
					pdfTiffLine.PutBits(num5, num2);
					pdfTiffLine.PutBits(huffmanSequence, 1 - num2);
					if (num == -1)
					{
						num = 0;
					}
					num += num5 + huffmanSequence;
					break;
				}
				case Group4Modes.modeV:
				case Group4Modes.modeVR1:
				case Group4Modes.modeVR2:
				case Group4Modes.modeVR3:
				case Group4Modes.modeVL1:
				case Group4Modes.modeVL2:
				case Group4Modes.modeVL3:
				{
					int num5;
					if (mode >= Group4Modes.modeV && mode <= Group4Modes.modeVR3)
					{
						num5 = num3 + (mode - Group4Modes.modeV);
					}
					else
					{
						num5 = num3 - (mode - Group4Modes.modeV - 3);
					}
					pdfTiffLine.PutBitsFromCurrentTo(num5, num2);
					num = num5;
					num2 = 1 - num2;
					break;
				}
				case Group4Modes.modeError:
					flag = false;
					break;
				case Group4Modes.modeEOL:
					flag = false;
					break;
				}
				if (num >= this.m_nWidth)
				{
					num = -1;
					pdfTiffLine2.Copy(pdfTiffLine);
					pdfStream.Append(pdfTiffLine.m_pBuffer);
					pdfTiffLine.Clear();
					num2 = 0;
				}
			}
			Out = pdfStream.ToBytes();
		}
		private int GetHuffmanSequence(byte[] Strip, bool bWhite)
		{
			int num = 0;
			TreeNode[] array = bWhite ? PdfTiff.m_arrWhite : PdfTiff.m_arrBlack;
			this.m_bWhiteRun = true;
			int num2 = 0;
			int num3 = 0;
			short num4 = 0;
			while (this.NextBit(Strip, ref num4))
			{
				int num5 = array[num].m_nChild[(int)num4];
				if (num5 != -1)
				{
					num = num5;
					int nValue = array[num].m_nValue;
					if (nValue != -1)
					{
						num = 0;
						if (nValue != 9999)
						{
							num2 += nValue;
							num3 += nValue;
							if (nValue <= 63)
							{
								return num2;
							}
						}
					}
				}
			}
			return -1;
		}
		private Group4Modes GetMode(byte[] Strip)
		{
			int num = 0;
			short num2 = 0;
			while (this.NextBit(Strip, ref num2))
			{
				int num3 = PdfTiff.m_arrModes[num].m_nChild[(int)num2];
				if (num3 == -1)
				{
					return Group4Modes.modeError;
				}
				num = num3;
				int nValue = PdfTiff.m_arrModes[num].m_nValue;
				if (nValue != -1)
				{
					return (Group4Modes)nValue;
				}
			}
			return Group4Modes.modeError;
		}
		private bool NextBit(byte[] data, ref short nBit)
		{
            if (this.m_nByteCount >= data.Length)
                return false;
            byte num = data[this.m_nByteCount];
            bool flag = this.m_nFillOrder != 1 ? ((int)num >> this.m_nBitCount & 1) == 1 : ((int)num << this.m_nBitCount & 128) == 128;
            nBit = flag ? (short)1 : (short)0;
            ++this.m_nBitCount;
            if (this.m_nBitCount >= 8)
            {
                this.m_nBitCount = 0;
                ++this.m_nByteCount;
            }
            return true;
		}
		private void PutBits(ref PdfStream objOut, short nBit, int nHowMany)
		{
			if (nHowMany == -1)
			{
				if (this.m_nOutBitCount != 0)
				{
					this.m_nOutBitCount = 0;
					this.m_nOutByteCount++;
				}
				return;
			}
			if (nHowMany == 0)
			{
				return;
			}
			byte b = (byte)(1 << 7 - this.m_nOutBitCount);
			int num = 8 - this.m_nOutBitCount;
			if (this.m_nOutByteCount >= objOut.Length)
			{
				PdfStream arg_59_0 = objOut;
				byte[] buffer = new byte[1];
				arg_59_0.Append(buffer);
			}
			for (int i = 0; i < num; i++)
			{
				if (nBit != 0)
				{
                    objOut[this.m_nOutByteCount] = (byte)((uint)objOut[this.m_nOutByteCount] | (uint)b);
				}
				b = (byte)(b >> 1);
				nHowMany--;
				this.m_nOutBitCount++;
				if (nHowMany <= 0)
				{
					break;
				}
			}
			if (this.m_nOutBitCount >= 8)
			{
				this.m_nOutBitCount = 0;
				this.m_nOutByteCount++;
			}
			if (nHowMany / 8 > 0)
			{
				byte[] array = new byte[nHowMany / 8];
				for (int j = 0; j < nHowMany / 8; j++)
				{
                    array[j] = (int)nBit != 0 ? byte.MaxValue : (byte)0;
				}
				objOut.Append(array);
				this.m_nOutByteCount += nHowMany / 8;
				nHowMany -= 8 * (nHowMany / 8);
			}
			if (nHowMany > 0)
			{
				byte b2 = 0;
				b = 128;
				for (int k = 0; k < nHowMany; k++)
				{
					if (nBit != 0)
					{
						b2 |= b;
					}
					b = (byte)(b >> 1);
					this.m_nOutBitCount++;
				}
				objOut.Append(new byte[]
				{
					b2
				});
			}
		}
		public PdfTiff(PdfManager pManager, PdfDocument pDoc)
		{
			this.m_pManager = pManager;
			this.m_pDoc = pDoc;
			this.m_pInput = null;
			this.m_dwPtr = 0;
			this.m_nExtraSamples = 0;
			this.m_bBigEndian = false;
			this.m_bPalette = false;
			this.m_pPaletteArray = null;
			this.m_fDefaultScaleX = (this.m_fDefaultScaleY = 1f);
			this.m_fResolutionX = (this.m_fResolutionY = 72f);
			this.m_nFillOrder = 1;
			this.m_nImageIndex = 1;
		}
		public void Close()
		{
			if (this.m_pInput != null)
			{
				this.m_pInput.Close();
			}
		}
		public virtual void ParseImage()
		{
			ushort num = this.ReadShort();
			if (num == 19789)
			{
				this.m_bBigEndian = true;
			}
			else
			{
				if (num != 18761)
				{
					AuxException.Throw(string.Format("Invalid TIFF header {0}, 'II' or 'MM' expected.", num), PdfErrors._ERROR_TIFF_FIELD);
				}
			}
			ushort num2 = this.ReadShort();
			if (num2 != 42)
			{
				AuxException.Throw(string.Format("Invalid TIFF magic number {0}, 42 expected.", num2), PdfErrors._ERROR_TIFF_FIELD);
			}
			if (this.m_nImageIndex <= 0)
			{
				AuxException.Throw(string.Format("Invalid TIFF image index {0}: must be 1 or greater.", this.m_nImageIndex), PdfErrors._ERROR_TIFF_FIELD);
			}
			int nOffset = (int)this.ReadLong();
			this.ParseDirectory(nOffset, this.m_nImageIndex - 1);
			this.ObtainImageData();
		}
		private void ParseDirectory(int nOffset, int nImagesToSkip)
		{
			if (nOffset == 0)
			{
				this.m_bPageIndexOutOfRange = true;
				return;
			}
			ushort num = this.ReadShort(nOffset);
			if (nImagesToSkip > 0)
			{
				int nOffset2 = (int)this.ReadLong(nOffset + 2 + (int)(num * 12));
				this.ParseDirectory(nOffset2, nImagesToSkip - 1);
				return;
			}
			int i = 0;
			while (i < (int)num)
			{
				PdfTiffField pdfTiffField = new PdfTiffField();
				pdfTiffField.m_nTag = (TiffTags)this.ReadShort();
				pdfTiffField.m_nType = (TiffFieldType)this.ReadShort();
				pdfTiffField.m_nCount = (int)this.ReadLong();
				if (pdfTiffField.m_nType < TiffFieldType.TIFF_BYTE || pdfTiffField.m_nType > TiffFieldType.TIFF_DOUBLE)
				{
					AuxException.Throw(string.Format("Invalid TIFF field type {0}.", pdfTiffField.m_nType), PdfErrors._ERROR_TIFF_FIELD);
				}
				int dwPtr;
				if (pdfTiffField.m_nCount * this.sizeOfType[(int)pdfTiffField.m_nType] <= 4)
				{
					pdfTiffField.m_nValueOffset = this.m_dwPtr;
					dwPtr = this.m_dwPtr + 4;
					goto IL_104;
				}
				pdfTiffField.m_nValueOffset = (int)this.ReadLong();
				if ((ulong)pdfTiffField.m_nValueOffset <= (ulong)((long)this.m_pInput.m_nSize))
				{
					dwPtr = this.m_dwPtr;
					this.m_dwPtr = pdfTiffField.m_nValueOffset;
					goto IL_104;
				}
				IL_265:
				i++;
				continue;
				IL_104:
				switch (pdfTiffField.m_nType)
				{
				case TiffFieldType.TIFF_BYTE:
				case TiffFieldType.TIFF_ASCII:
				case TiffFieldType.TIFF_SBYTE:
				case TiffFieldType.TIFF_UNDEFINED:
				{
					byte[] array = new byte[pdfTiffField.m_nCount];
					this.m_pInput.ReadBytes(pdfTiffField.m_nValueOffset, array, pdfTiffField.m_nCount);
					pdfTiffField.m_objStream.Set(array);
					break;
				}
				case TiffFieldType.TIFF_SHORT:
					for (int j = 0; j < pdfTiffField.m_nCount; j++)
					{
						pdfTiffField.m_arrLongs.Add((uint)this.ReadShort());
					}
					break;
				case TiffFieldType.TIFF_LONG:
					for (int j = 0; j < pdfTiffField.m_nCount; j++)
					{
						pdfTiffField.m_arrLongs.Add(this.ReadLong());
					}
					break;
				case TiffFieldType.TIFF_RATIONAL:
					for (int j = 0; j < pdfTiffField.m_nCount; j++)
					{
						pdfTiffField.m_arrLongs.Add(this.ReadLong());
						pdfTiffField.m_arrLongs2.Add(this.ReadLong());
					}
					break;
				case TiffFieldType.TIFF_SSHORT:
					for (int j = 0; j < pdfTiffField.m_nCount; j++)
					{
						pdfTiffField.m_arrSShorts.Add((short)this.ReadShort());
					}
					break;
				case TiffFieldType.TIFF_SLONG:
					for (int j = 0; j < pdfTiffField.m_nCount; j++)
					{
						pdfTiffField.m_arrSLongs.Add((int)this.ReadLong());
					}
					break;
				}
				this.m_arrFields.Add(pdfTiffField);
				this.m_dwPtr = dwPtr;
				goto IL_265;
			}
		}
		private void ObtainImageData()
		{
			if (this.m_bPageIndexOutOfRange)
			{
				return;
			}
			int num = this.GetIntValue(TiffTags.TIFF_TAG_PHOTOMETRIC);
			TiffCompressions tiffCompressions = TiffCompressions.TIFF_COMPRESSION_NONE;
			int num2 = 1;
			if (this.GetOptionalIntValue(TiffTags.TIFF_TAG_COMPRESSION, ref num2))
			{
				tiffCompressions = (TiffCompressions)num2;
			}
			this.m_nCompression = tiffCompressions;
			if (tiffCompressions == TiffCompressions.TIFF_COMPRESSION_LZW)
			{
				int num3 = 0;
				this.GetOptionalIntValue(TiffTags.TIFF_TAG_PREDICTOR, ref num3);
				if (num3 == 2)
				{
					this.m_lzw.m_bPredictor = true;
				}
			}
			if (tiffCompressions == TiffCompressions.TIFF_COMPRESSION_HUFFMAN || tiffCompressions == TiffCompressions.TIFF_COMPRESSION_GROUP3 || tiffCompressions == TiffCompressions.TIFF_COMPRESSION_GROUP4)
			{
				this.GetOptionalIntValue(TiffTags.TIFF_TAG_FILLORDER, ref this.m_nFillOrder);
				num = 0;
			}
			this.m_nBitsPerComponent = 1;
			this.GetOptionalIntValue(TiffTags.TIFF_TAG_BITSPERSAMPLE, ref this.m_nBitsPerComponent);
			this.m_nComponentsPerSample = 1;
			this.GetOptionalIntValue(TiffTags.TIFF_TAG_SAMPLESPERPIXEL, ref this.m_nComponentsPerSample);
			this.m_nPlanarConfig = 1;
			this.GetOptionalIntValue(TiffTags.TIFF_TAG_PLANARCONFIG, ref this.m_nPlanarConfig);
			switch (num)
			{
			case 0:
			case 1:
				this.m_bstrColorSpace = "DeviceGray";
				goto IL_258;
			case 2:
				if (this.m_nComponentsPerSample > 3)
				{
					this.m_nExtraSamples = this.m_nComponentsPerSample - 3;
				}
				this.m_bstrColorSpace = "DeviceRGB";
				goto IL_258;
			case 3:
			{
				if (this.m_nBitsPerComponent != 8)
				{
					AuxException.Throw(string.Format("Color Palette TIFF images with the BitsPerSample value of {0} are not supported.", this.m_nBitsPerComponent), PdfErrors._ERROR_TIFF_FIELD);
				}
				PdfTiffField field = this.GetField(TiffTags.TIFF_TAG_COLORMAP);
				if (field.m_arrLongs.Count != 3 * (2 << this.m_nBitsPerComponent - 1))
				{
					AuxException.Throw(string.Format("The number of components in the TIFF color map ({0}) is invalid.", field.m_arrLongs.Count), PdfErrors._ERROR_TIFF_FIELD);
				}
				this.m_pPaletteArray = new byte[field.m_arrLongs.Count];
				int i = 0;
				for (int j = 0; j < field.m_arrLongs.Count; j++)
				{
					this.m_pPaletteArray[i++] = (byte)(field.m_arrLongs[j] >> 8);
				}
				this.m_bPalette = true;
				this.m_bstrColorSpace = "DeviceRGB";
				goto IL_258;
			}
			case 5:
				if (this.m_nComponentsPerSample > 4)
				{
					this.m_nExtraSamples = this.m_nComponentsPerSample - 4;
				}
				this.m_bstrColorSpace = "DeviceCMYK";
				goto IL_258;
			case 6:
				this.m_bstrColorSpace = "DeviceRGB";
				goto IL_258;
			case 8:
				this.m_bstrColorSpace = "Lab";
				goto IL_258;
			}
			AuxException.Throw(string.Format("TIFF PhotometricInterpretation value of {0} is not supported.", num), PdfErrors._ERROR_TIFF_FIELD);
			IL_258:
			this.m_nWidth = this.GetIntValue(TiffTags.TIFF_TAG_WIDTH);
			this.m_nHeight = this.GetIntValue(TiffTags.TIFF_TAG_LENGTH);
			int num4 = 0;
			if (!this.GetOptionalIntValue(TiffTags.TIFF_TAG_ROWSPERSTRIP, ref num4) || num4 <= 0)
			{
				num4 = this.m_nHeight;
			}
			this.ComputeResolutions();
			PdfTiffField field2 = this.GetField(TiffTags.TIFF_TAG_STRIPOFFSETS);
			PdfTiffField pdfTiffField = this.GetOptionalField(TiffTags.TIFF_TAG_STRIPBYTECOUNTS);
			if (pdfTiffField == null)
			{
				pdfTiffField = this.CreateField(TiffTags.TIFF_TAG_STRIPOFFSETS);
				uint num5 = (uint)((this.m_nWidth * this.m_nBitsPerComponent * this.m_nComponentsPerSample + 7) / 8 * this.m_nHeight);
				if ((ulong)(num5 + field2.m_arrLongs[0]) > (ulong)((long)this.m_pInput.m_nSize))
				{
					num5 = (uint)((long)this.m_pInput.m_nSize - (long)((ulong)field2.m_arrLongs[0]));
				}
				pdfTiffField.m_arrLongs.Add(num5);
			}
			if (field2.m_arrLongs.Count != pdfTiffField.m_arrLongs.Count)
			{
				AuxException.Throw("TIFF strip offset and byte count array sizes do not match.", PdfErrors._ERROR_TIFF_FIELD);
			}
			int num6 = this.m_nHeight;
			for (int k = 0; k < field2.m_arrLongs.Count; k++)
			{
				int num7 = (int)field2.m_arrLongs[k];
				int num8 = (int)pdfTiffField.m_arrLongs[k];
				byte[] array = new byte[num8];
				if (num7 + num8 > this.m_pInput.m_nSize && this.m_pInput.m_nSize - num7 > 0)
				{
					num8 = this.m_pInput.m_nSize - num7;
				}
				this.m_pInput.ReadBytes(num7, array, num8);
				int nRowsInStrip = Math.Min(num4, num6);
				byte[] array2 = this.Decompress(array, nRowsInStrip, tiffCompressions);
				if (this.m_nBitsPerComponent == 16 || this.m_nBitsPerComponent == 32)
				{
					array2 = this.ReduceBitsPerComponent(array2);
				}
				if (num == 0)
				{
					for (int i = 0; i < array2.Length; i++)
					{
						byte[] expr_421_cp_0 = array2;
						int expr_421_cp_1 = i;
						expr_421_cp_0[expr_421_cp_1] ^= 255;
					}
				}
				if (num == 8)
				{
					for (int i = 0; i < array2.Length / 3; i++)
					{
						byte[] expr_457_cp_0 = array2;
						int expr_457_cp_1 = i * 3 + 1;
						expr_457_cp_0[expr_457_cp_1] ^= 128;
						byte[] expr_476_cp_0 = array2;
						int expr_476_cp_1 = i * 3 + 2;
						expr_476_cp_0[expr_476_cp_1] ^= 128;
					}
				}
				this.m_objData.Append(array2);
				num6 -= num4;
			}
			this.m_nComponentsPerSample -= this.m_nExtraSamples;
			if (this.m_bPalette)
			{
				this.m_nBitsPerComponent = 8;
				this.m_nComponentsPerSample = 3;
			}
			if (this.m_nBitsPerComponent == 16 || this.m_nBitsPerComponent == 32)
			{
				this.m_nBitsPerComponent = 8;
			}
			if (this.m_nPlanarConfig == 2)
			{
				byte[] array3 = new byte[this.m_objData.Length];
				Array.Copy(this.m_objData.ToBytes(), array3, this.m_objData.Length);
				int num9 = 0;
				for (int l = 0; l < this.m_nComponentsPerSample; l++)
				{
					for (int m = 0; m < this.m_objData.Length / this.m_nComponentsPerSample; m++)
					{
						array3[m * this.m_nComponentsPerSample + l] = this.m_objData[num9++];
					}
				}
				this.m_objData.Set(array3);
			}
			long num10 = (long)((this.m_nWidth * this.m_nHeight * this.m_nComponentsPerSample * this.m_nBitsPerComponent + 7) / 8 - this.m_objData.Length);
			if (num10 > 0L && this.m_nCompression != TiffCompressions.TIFF_COMPRESSION_JPEG)
			{
				byte[] buffer = new byte[num10];
				this.m_objData.Append(buffer);
			}
		}
		private byte[] ReduceBitsPerComponent(byte[] Data)
		{
			int num = 0;
			int num2 = this.m_nBitsPerComponent / 8;
			byte[] array = new byte[Data.Length / num2];
			int num3 = this.m_bBigEndian ? 0 : ((this.m_nBitsPerComponent == 16) ? 1 : 3);
			for (int i = 0; i < Data.Length; i += num2)
			{
				array[num++] = Data[i + num3];
			}
			return array;
		}
		private byte[] Decompress(byte[] Strip, int nRowsInStrip, TiffCompressions nCompression)
		{
			byte[] array = null;
			switch (nCompression)
			{
			case TiffCompressions.TIFF_COMPRESSION_NONE:
			{
				array = new byte[Strip.Length];
				Array.Copy(Strip, array, Strip.Length);
				int num = array.Length;
				break;
			}
			case TiffCompressions.TIFF_COMPRESSION_HUFFMAN:
			case TiffCompressions.TIFF_COMPRESSION_GROUP3:
				this.DecodeHuffman(Strip, ref array, nCompression);
				break;
			case TiffCompressions.TIFF_COMPRESSION_GROUP4:
				this.DecodeGroup4(Strip, ref array);
				break;
			case TiffCompressions.TIFF_COMPRESSION_LZW:
				this.m_lzw.Decode2(Strip, ref array, this.m_nWidth, nRowsInStrip, this.m_nComponentsPerSample);
				break;
			case TiffCompressions.TIFF_COMPRESSION_JPEG:
				this.DecompressJpeg(Strip, ref array);
				break;
			default:
				if (nCompression != TiffCompressions.TIFF_COMPRESSION_PACKBITS)
				{
					AuxException.Throw(string.Format("TIFF compression type {0} is not supported.", nCompression), PdfErrors._ERROR_TIFF_FIELD);
				}
				else
				{
					int num = (this.m_nWidth * this.m_nBitsPerComponent * this.m_nComponentsPerSample + 7) / 8 * nRowsInStrip;
					array = new byte[num];
					this.DecodePackBits(Strip, array);
				}
				break;
			}
			if (this.m_bPalette)
			{
				PdfTiffField field = this.GetField(TiffTags.TIFF_TAG_COLORMAP);
				int num = this.m_nWidth * nRowsInStrip * 3;
				byte[] array2 = new byte[num];
				int num2 = 0;
				int num3 = field.m_arrLongs.Count / 3;
				for (int i = 0; i < this.m_nWidth * nRowsInStrip; i++)
				{
					int num4 = (int)array[i];
					array2[num2++] = this.m_pPaletteArray[num4];
					array2[num2++] = this.m_pPaletteArray[num4 + num3];
					array2[num2++] = this.m_pPaletteArray[num4 + 2 * num3];
				}
				array = new byte[array2.Length];
				Array.Copy(array2, array, array2.Length);
			}
			if (this.m_nExtraSamples == 0)
			{
				return array;
			}
			byte[] array3 = new byte[array.Length * (this.m_nComponentsPerSample - this.m_nExtraSamples) / this.m_nComponentsPerSample];
			Array.Copy(array, array3, array3.Length);
			int num5 = 0;
			for (int j = 0; j < array.Length; j++)
			{
				if (j % this.m_nComponentsPerSample < this.m_nComponentsPerSample - this.m_nExtraSamples)
				{
					array3[num5++] = array[j];
				}
			}
			return array3;
		}
		private void DecodePackBits(byte[] Strip, byte[] Out)
		{
			int num = 0;
			int i = 0;
			while (i < Out.Length)
			{
				sbyte b = (sbyte)Strip[num++];
				if (b >= 0 && b <= 127)
				{
					for (int j = 0; j < (int)(b + 1); j++)
					{
						Out[i++] = Strip[num++];
					}
				}
				else
				{
					if (b <= -1 && b >= -127)
					{
						byte b2 = Strip[num++];
						for (int k = 0; k < (int)(-b + 1); k++)
						{
							Out[i++] = b2;
						}
					}
					else
					{
						num++;
					}
				}
			}
		}
		private void DecompressJpeg(byte[] Strip, ref byte[] Out)
		{
			PdfJpeg pdfJpeg = new PdfJpeg(this.m_pManager, this.m_pDoc);
			pdfJpeg.LoadFromMemory(Strip);
			Out = new byte[pdfJpeg.m_objData.Length];
			Array.Copy(pdfJpeg.m_objData.ToBytes(), Out, pdfJpeg.m_objData.Length);
			this.m_bstrColorSpace = pdfJpeg.m_bstrColorSpace;
		}
		private void ComputeResolutions()
		{
			PdfTiffField optionalField = this.GetOptionalField(TiffTags.TIFF_TAG_XRESOLUTION);
			PdfTiffField optionalField2 = this.GetOptionalField(TiffTags.TIFF_TAG_YRESOLUTION);
			if (optionalField == null || optionalField2 == null)
			{
				return;
			}
			if (optionalField.m_arrLongs.Count == 0 || optionalField.m_arrLongs2.Count == 0)
			{
				return;
			}
			if (optionalField2.m_arrLongs.Count == 0 || optionalField2.m_arrLongs2.Count == 0)
			{
				return;
			}
			float num = optionalField.m_arrLongs[0] / optionalField.m_arrLongs2[0];
			float num2 = optionalField2.m_arrLongs[0] / optionalField2.m_arrLongs2[0];
			if (num == 0f || num2 == 0f)
			{
				num2 = (num = 72f);
			}
			PdfTiffField optionalField3 = this.GetOptionalField(TiffTags.TIFF_TAG_RESOLUTIONUNIT);
			if (optionalField3 != null && optionalField3.m_arrLongs.Count > 0)
			{
				uint num3 = optionalField3.m_arrLongs[0];
				if (num3 == 3u)
				{
					num *= 2.54f;
					num2 *= 2.54f;
				}
			}
			this.m_fResolutionX = num;
			this.m_fResolutionY = num2;
			this.m_fDefaultScaleX = 72f / num;
			this.m_fDefaultScaleY = 72f / num2;
		}
		private PdfTiffField CreateField(TiffTags tag)
		{
			PdfTiffField pdfTiffField = new PdfTiffField();
			pdfTiffField.m_nTag = tag;
			pdfTiffField.m_nType = TiffFieldType.TIFF_LONG;
			pdfTiffField.m_nCount = 1;
			this.m_arrFields.Add(pdfTiffField);
			return pdfTiffField;
		}
		private PdfTiffField GetField(TiffTags tag)
		{
			PdfTiffField optionalField = this.GetOptionalField(tag);
			if (optionalField == null)
			{
				AuxException.Throw(string.Format("TIFF field tag {0} is not found.", tag), PdfErrors._ERROR_TIFF_FIELD);
			}
			return optionalField;
		}
		private PdfTiffField GetOptionalField(TiffTags tag)
		{
			foreach (PdfTiffField current in this.m_arrFields)
			{
				if (tag == current.m_nTag)
				{
					return current;
				}
			}
			return null;
		}
		private int GetIntValue(TiffTags tag)
		{
			PdfTiffField field = this.GetField(tag);
			if (field.m_arrLongs.Count > 0)
			{
				return (int)field.m_arrLongs[0];
			}
			AuxException.Throw("This TIFF field does not have an integer value.", PdfErrors._ERROR_TIFF_FIELD);
			return 0;
		}
		private bool GetOptionalIntValue(TiffTags tag, ref int pValue)
		{
			PdfTiffField optionalField = this.GetOptionalField(tag);
			if (optionalField == null)
			{
				return false;
			}
			if (optionalField.m_arrLongs.Count == 0)
			{
				return false;
			}
			pValue = (int)optionalField.m_arrLongs[0];
			return true;
		}
		public void LoadFromFile(string Path)
		{
			this.m_pInput = new PdfInput(Path, null, null);
			try
			{
				this.ParseImage();
			}
			catch (Exception ex)
			{
				this.m_pInput.Close();
				throw ex;
			}
		}
		public void LoadFromMemory(byte[] pData)
		{
			this.m_pInput = new PdfInput(pData, null, null);
			this.ParseImage();
		}
		public byte ReadByte()
		{
			byte[] array = new byte[1];
			this.m_pInput.ReadBytes(this.m_dwPtr, array, 1);
			this.m_dwPtr++;
			return array[0];
		}
		public ushort ReadShort()
		{
			byte[] array = new byte[2];
			this.m_pInput.ReadBytes(this.m_dwPtr, array, 2);
			this.m_dwPtr += 2;
			if (this.m_bBigEndian)
			{
				return (ushort)(((int)array[0] << 8) + (int)array[1]);
			}
			return (ushort)(((int)array[1] << 8) + (int)array[0]);
		}
		public ushort ReadShort(int nOffset)
		{
			byte[] array = new byte[2];
			this.m_pInput.ReadBytes(nOffset, array, 2);
			this.m_dwPtr = nOffset + 2;
			if (this.m_bBigEndian)
			{
				return (ushort)(((int)array[0] << 8) + (int)array[1]);
			}
			return (ushort)(((int)array[1] << 8) + (int)array[0]);
		}
		public uint ReadLong()
		{
			byte[] array = new byte[4];
			this.m_pInput.ReadBytes(this.m_dwPtr, array, 4);
			this.m_dwPtr += 4;
			if (this.m_bBigEndian)
			{
				return (uint)(((int)array[0] << 24) + ((int)array[1] << 16) + ((int)array[2] << 8) + (int)array[3]);
			}
			return (uint)(((int)array[3] << 24) + ((int)array[2] << 16) + ((int)array[1] << 8) + (int)array[0]);
		}
		public uint ReadLong(int nOffset)
		{
			byte[] array = new byte[4];
			this.m_pInput.ReadBytes(nOffset, array, 4);
			this.m_dwPtr = nOffset + 4;
			if (this.m_bBigEndian)
			{
				return (uint)(((int)array[0] << 24) + ((int)array[1] << 16) + ((int)array[2] << 8) + (int)array[3]);
			}
			return (uint)(((int)array[3] << 24) + ((int)array[2] << 16) + ((int)array[1] << 8) + (int)array[0]);
		}
	}
}
