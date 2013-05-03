using System;
using System.Collections.Generic;
namespace Persits.PDF
{
	internal class JBIG2Stream : PdfDecoderHelper
	{
		internal const uint jbig2HuffmanLOW = 4294967293u;
		internal const uint jbig2HuffmanOOB = 4294967294u;
		internal const uint jbig2HuffmanEOT = 4294967295u;
		private const int twoDimPass = 0;
		private const int twoDimHoriz = 1;
		private const int twoDimVert0 = 2;
		private const int twoDimVertR1 = 3;
		private const int twoDimVertL1 = 4;
		private const int twoDimVertR2 = 5;
		private const int twoDimVertL2 = 6;
		private const int twoDimVertR3 = 7;
		private const int twoDimVertL3 = 8;
		private PdfStream str;
		private PdfObject globalsStream;
		private uint pageW;
		private uint pageH;
		private uint curPageH;
		private uint pageDefPixel;
		private JBIG2Bitmap pageBitmap;
		private uint defCombOp;
		private List<JBIG2Segment> segments = new List<JBIG2Segment>();
		private List<JBIG2Segment> globalSegments = new List<JBIG2Segment>();
		private PdfStream curStr;
		private RavenColorPtr dataPtr;
		private RavenColorPtr dataEnd;
		private uint byteCounter;
		private JArithmeticDecoder arithDecoder;
		private JArithmeticDecoderStats genericRegionStats;
		private JArithmeticDecoderStats refinementRegionStats;
		private JArithmeticDecoderStats iadhStats;
		private JArithmeticDecoderStats iadwStats;
		private JArithmeticDecoderStats iaexStats;
		private JArithmeticDecoderStats iaaiStats;
		private JArithmeticDecoderStats iadtStats;
		private JArithmeticDecoderStats iaitStats;
		private JArithmeticDecoderStats iafsStats;
		private JArithmeticDecoderStats iadsStats;
		private JArithmeticDecoderStats iardxStats;
		private JArithmeticDecoderStats iardyStats;
		private JArithmeticDecoderStats iardwStats;
		private JArithmeticDecoderStats iardhStats;
		private JArithmeticDecoderStats iariStats;
		private JArithmeticDecoderStats iaidStats;
		private JBIG2HuffmanDecoder huffDecoder;
		private JBIG2MMRDecoder mmrDecoder;
		private JBIG2HuffmanTable[] huffTableA = new JBIG2HuffmanTable[]
		{
			new JBIG2HuffmanTable(0, 1u, 4u, 0u),
			new JBIG2HuffmanTable(16, 2u, 8u, 2u),
			new JBIG2HuffmanTable(272, 3u, 16u, 6u),
			new JBIG2HuffmanTable(65808, 3u, 32u, 7u),
			new JBIG2HuffmanTable(0, 0u, 4294967295u, 0u)
		};
		private JBIG2HuffmanTable[] huffTableB = new JBIG2HuffmanTable[]
		{
			new JBIG2HuffmanTable(0, 1u, 0u, 0u),
			new JBIG2HuffmanTable(1, 2u, 0u, 2u),
			new JBIG2HuffmanTable(2, 3u, 0u, 6u),
			new JBIG2HuffmanTable(3, 4u, 3u, 14u),
			new JBIG2HuffmanTable(11, 5u, 6u, 30u),
			new JBIG2HuffmanTable(75, 6u, 32u, 62u),
			new JBIG2HuffmanTable(0, 6u, 4294967294u, 63u),
			new JBIG2HuffmanTable(0, 0u, 4294967295u, 0u)
		};
		private JBIG2HuffmanTable[] huffTableC = new JBIG2HuffmanTable[]
		{
			new JBIG2HuffmanTable(0, 1u, 0u, 0u),
			new JBIG2HuffmanTable(1, 2u, 0u, 2u),
			new JBIG2HuffmanTable(2, 3u, 0u, 6u),
			new JBIG2HuffmanTable(3, 4u, 3u, 14u),
			new JBIG2HuffmanTable(11, 5u, 6u, 30u),
			new JBIG2HuffmanTable(0, 6u, 4294967294u, 62u),
			new JBIG2HuffmanTable(75, 7u, 32u, 254u),
			new JBIG2HuffmanTable(-256, 8u, 8u, 254u),
			new JBIG2HuffmanTable(-257, 8u, 4294967293u, 255u),
			new JBIG2HuffmanTable(0, 0u, 4294967295u, 0u)
		};
		private JBIG2HuffmanTable[] huffTableD = new JBIG2HuffmanTable[]
		{
			new JBIG2HuffmanTable(1, 1u, 0u, 0u),
			new JBIG2HuffmanTable(2, 2u, 0u, 2u),
			new JBIG2HuffmanTable(3, 3u, 0u, 6u),
			new JBIG2HuffmanTable(4, 4u, 3u, 14u),
			new JBIG2HuffmanTable(12, 5u, 6u, 30u),
			new JBIG2HuffmanTable(76, 5u, 32u, 31u),
			new JBIG2HuffmanTable(0, 0u, 4294967295u, 0u)
		};
		private JBIG2HuffmanTable[] huffTableE = new JBIG2HuffmanTable[]
		{
			new JBIG2HuffmanTable(1, 1u, 0u, 0u),
			new JBIG2HuffmanTable(2, 2u, 0u, 2u),
			new JBIG2HuffmanTable(3, 3u, 0u, 6u),
			new JBIG2HuffmanTable(4, 4u, 3u, 14u),
			new JBIG2HuffmanTable(12, 5u, 6u, 30u),
			new JBIG2HuffmanTable(76, 6u, 32u, 62u),
			new JBIG2HuffmanTable(-255, 7u, 8u, 126u),
			new JBIG2HuffmanTable(-256, 7u, 4294967293u, 127u),
			new JBIG2HuffmanTable(0, 0u, 4294967295u, 0u)
		};
		private JBIG2HuffmanTable[] huffTableF = new JBIG2HuffmanTable[]
		{
			new JBIG2HuffmanTable(0, 2u, 7u, 0u),
			new JBIG2HuffmanTable(128, 3u, 7u, 2u),
			new JBIG2HuffmanTable(256, 3u, 8u, 3u),
			new JBIG2HuffmanTable(-1024, 4u, 9u, 8u),
			new JBIG2HuffmanTable(-512, 4u, 8u, 9u),
			new JBIG2HuffmanTable(-256, 4u, 7u, 10u),
			new JBIG2HuffmanTable(-32, 4u, 5u, 11u),
			new JBIG2HuffmanTable(512, 4u, 9u, 12u),
			new JBIG2HuffmanTable(1024, 4u, 10u, 13u),
			new JBIG2HuffmanTable(-2048, 5u, 10u, 28u),
			new JBIG2HuffmanTable(-128, 5u, 6u, 29u),
			new JBIG2HuffmanTable(-64, 5u, 5u, 30u),
			new JBIG2HuffmanTable(-2049, 6u, 4294967293u, 62u),
			new JBIG2HuffmanTable(2048, 6u, 32u, 63u),
			new JBIG2HuffmanTable(0, 0u, 4294967295u, 0u)
		};
		private JBIG2HuffmanTable[] huffTableG = new JBIG2HuffmanTable[]
		{
			new JBIG2HuffmanTable(-512, 3u, 8u, 0u),
			new JBIG2HuffmanTable(256, 3u, 8u, 1u),
			new JBIG2HuffmanTable(512, 3u, 9u, 2u),
			new JBIG2HuffmanTable(1024, 3u, 10u, 3u),
			new JBIG2HuffmanTable(-1024, 4u, 9u, 8u),
			new JBIG2HuffmanTable(-256, 4u, 7u, 9u),
			new JBIG2HuffmanTable(-32, 4u, 5u, 10u),
			new JBIG2HuffmanTable(0, 4u, 5u, 11u),
			new JBIG2HuffmanTable(128, 4u, 7u, 12u),
			new JBIG2HuffmanTable(-128, 5u, 6u, 26u),
			new JBIG2HuffmanTable(-64, 5u, 5u, 27u),
			new JBIG2HuffmanTable(32, 5u, 5u, 28u),
			new JBIG2HuffmanTable(64, 5u, 6u, 29u),
			new JBIG2HuffmanTable(-1025, 5u, 4294967293u, 30u),
			new JBIG2HuffmanTable(2048, 5u, 32u, 31u),
			new JBIG2HuffmanTable(0, 0u, 4294967295u, 0u)
		};
		private JBIG2HuffmanTable[] huffTableH = new JBIG2HuffmanTable[]
		{
			new JBIG2HuffmanTable(0, 2u, 1u, 0u),
			new JBIG2HuffmanTable(0, 2u, 4294967294u, 1u),
			new JBIG2HuffmanTable(4, 3u, 4u, 4u),
			new JBIG2HuffmanTable(-1, 4u, 0u, 10u),
			new JBIG2HuffmanTable(22, 4u, 4u, 11u),
			new JBIG2HuffmanTable(38, 4u, 5u, 12u),
			new JBIG2HuffmanTable(2, 5u, 0u, 26u),
			new JBIG2HuffmanTable(70, 5u, 6u, 27u),
			new JBIG2HuffmanTable(134, 5u, 7u, 28u),
			new JBIG2HuffmanTable(3, 6u, 0u, 58u),
			new JBIG2HuffmanTable(20, 6u, 1u, 59u),
			new JBIG2HuffmanTable(262, 6u, 7u, 60u),
			new JBIG2HuffmanTable(646, 6u, 10u, 61u),
			new JBIG2HuffmanTable(-2, 7u, 0u, 124u),
			new JBIG2HuffmanTable(390, 7u, 8u, 125u),
			new JBIG2HuffmanTable(-15, 8u, 3u, 252u),
			new JBIG2HuffmanTable(-5, 8u, 1u, 253u),
			new JBIG2HuffmanTable(-7, 9u, 1u, 508u),
			new JBIG2HuffmanTable(-3, 9u, 0u, 509u),
			new JBIG2HuffmanTable(-16, 9u, 4294967293u, 510u),
			new JBIG2HuffmanTable(1670, 9u, 32u, 511u),
			new JBIG2HuffmanTable(0, 0u, 4294967295u, 0u)
		};
		private JBIG2HuffmanTable[] huffTableI = new JBIG2HuffmanTable[]
		{
			new JBIG2HuffmanTable(0, 2u, 4294967294u, 0u),
			new JBIG2HuffmanTable(-1, 3u, 1u, 2u),
			new JBIG2HuffmanTable(1, 3u, 1u, 3u),
			new JBIG2HuffmanTable(7, 3u, 5u, 4u),
			new JBIG2HuffmanTable(-3, 4u, 1u, 10u),
			new JBIG2HuffmanTable(43, 4u, 5u, 11u),
			new JBIG2HuffmanTable(75, 4u, 6u, 12u),
			new JBIG2HuffmanTable(3, 5u, 1u, 26u),
			new JBIG2HuffmanTable(139, 5u, 7u, 27u),
			new JBIG2HuffmanTable(267, 5u, 8u, 28u),
			new JBIG2HuffmanTable(5, 6u, 1u, 58u),
			new JBIG2HuffmanTable(39, 6u, 2u, 59u),
			new JBIG2HuffmanTable(523, 6u, 8u, 60u),
			new JBIG2HuffmanTable(1291, 6u, 11u, 61u),
			new JBIG2HuffmanTable(-5, 7u, 1u, 124u),
			new JBIG2HuffmanTable(779, 7u, 9u, 125u),
			new JBIG2HuffmanTable(-31, 8u, 4u, 252u),
			new JBIG2HuffmanTable(-11, 8u, 2u, 253u),
			new JBIG2HuffmanTable(-15, 9u, 2u, 508u),
			new JBIG2HuffmanTable(-7, 9u, 1u, 509u),
			new JBIG2HuffmanTable(-32, 9u, 4294967293u, 510u),
			new JBIG2HuffmanTable(3339, 9u, 32u, 511u),
			new JBIG2HuffmanTable(0, 0u, 4294967295u, 0u)
		};
		private JBIG2HuffmanTable[] huffTableJ = new JBIG2HuffmanTable[]
		{
			new JBIG2HuffmanTable(-2, 2u, 2u, 0u),
			new JBIG2HuffmanTable(6, 2u, 6u, 1u),
			new JBIG2HuffmanTable(0, 2u, 4294967294u, 2u),
			new JBIG2HuffmanTable(-3, 5u, 0u, 24u),
			new JBIG2HuffmanTable(2, 5u, 0u, 25u),
			new JBIG2HuffmanTable(70, 5u, 5u, 26u),
			new JBIG2HuffmanTable(3, 6u, 0u, 54u),
			new JBIG2HuffmanTable(102, 6u, 5u, 55u),
			new JBIG2HuffmanTable(134, 6u, 6u, 56u),
			new JBIG2HuffmanTable(198, 6u, 7u, 57u),
			new JBIG2HuffmanTable(326, 6u, 8u, 58u),
			new JBIG2HuffmanTable(582, 6u, 9u, 59u),
			new JBIG2HuffmanTable(1094, 6u, 10u, 60u),
			new JBIG2HuffmanTable(-21, 7u, 4u, 122u),
			new JBIG2HuffmanTable(-4, 7u, 0u, 123u),
			new JBIG2HuffmanTable(4, 7u, 0u, 124u),
			new JBIG2HuffmanTable(2118, 7u, 11u, 125u),
			new JBIG2HuffmanTable(-5, 8u, 0u, 252u),
			new JBIG2HuffmanTable(5, 8u, 0u, 253u),
			new JBIG2HuffmanTable(-22, 8u, 4294967293u, 254u),
			new JBIG2HuffmanTable(4166, 8u, 32u, 255u),
			new JBIG2HuffmanTable(0, 0u, 4294967295u, 0u)
		};
		private JBIG2HuffmanTable[] huffTableK = new JBIG2HuffmanTable[]
		{
			new JBIG2HuffmanTable(1, 1u, 0u, 0u),
			new JBIG2HuffmanTable(2, 2u, 1u, 2u),
			new JBIG2HuffmanTable(4, 4u, 0u, 12u),
			new JBIG2HuffmanTable(5, 4u, 1u, 13u),
			new JBIG2HuffmanTable(7, 5u, 1u, 28u),
			new JBIG2HuffmanTable(9, 5u, 2u, 29u),
			new JBIG2HuffmanTable(13, 6u, 2u, 60u),
			new JBIG2HuffmanTable(17, 7u, 2u, 122u),
			new JBIG2HuffmanTable(21, 7u, 3u, 123u),
			new JBIG2HuffmanTable(29, 7u, 4u, 124u),
			new JBIG2HuffmanTable(45, 7u, 5u, 125u),
			new JBIG2HuffmanTable(77, 7u, 6u, 126u),
			new JBIG2HuffmanTable(141, 7u, 32u, 127u),
			new JBIG2HuffmanTable(0, 0u, 4294967295u, 0u)
		};
		private JBIG2HuffmanTable[] huffTableL = new JBIG2HuffmanTable[]
		{
			new JBIG2HuffmanTable(1, 1u, 0u, 0u),
			new JBIG2HuffmanTable(2, 2u, 0u, 2u),
			new JBIG2HuffmanTable(3, 3u, 1u, 6u),
			new JBIG2HuffmanTable(5, 5u, 0u, 28u),
			new JBIG2HuffmanTable(6, 5u, 1u, 29u),
			new JBIG2HuffmanTable(8, 6u, 1u, 60u),
			new JBIG2HuffmanTable(10, 7u, 0u, 122u),
			new JBIG2HuffmanTable(11, 7u, 1u, 123u),
			new JBIG2HuffmanTable(13, 7u, 2u, 124u),
			new JBIG2HuffmanTable(17, 7u, 3u, 125u),
			new JBIG2HuffmanTable(25, 7u, 4u, 126u),
			new JBIG2HuffmanTable(41, 8u, 5u, 254u),
			new JBIG2HuffmanTable(73, 8u, 32u, 255u),
			new JBIG2HuffmanTable(0, 0u, 4294967295u, 0u)
		};
		private JBIG2HuffmanTable[] huffTableM = new JBIG2HuffmanTable[]
		{
			new JBIG2HuffmanTable(1, 1u, 0u, 0u),
			new JBIG2HuffmanTable(2, 3u, 0u, 4u),
			new JBIG2HuffmanTable(7, 3u, 3u, 5u),
			new JBIG2HuffmanTable(3, 4u, 0u, 12u),
			new JBIG2HuffmanTable(5, 4u, 1u, 13u),
			new JBIG2HuffmanTable(4, 5u, 0u, 28u),
			new JBIG2HuffmanTable(15, 6u, 1u, 58u),
			new JBIG2HuffmanTable(17, 6u, 2u, 59u),
			new JBIG2HuffmanTable(21, 6u, 3u, 60u),
			new JBIG2HuffmanTable(29, 6u, 4u, 61u),
			new JBIG2HuffmanTable(45, 6u, 5u, 62u),
			new JBIG2HuffmanTable(77, 7u, 6u, 126u),
			new JBIG2HuffmanTable(141, 7u, 32u, 127u),
			new JBIG2HuffmanTable(0, 0u, 4294967295u, 0u)
		};
		private JBIG2HuffmanTable[] huffTableN = new JBIG2HuffmanTable[]
		{
			new JBIG2HuffmanTable(0, 1u, 0u, 0u),
			new JBIG2HuffmanTable(-2, 3u, 0u, 4u),
			new JBIG2HuffmanTable(-1, 3u, 0u, 5u),
			new JBIG2HuffmanTable(1, 3u, 0u, 6u),
			new JBIG2HuffmanTable(2, 3u, 0u, 7u),
			new JBIG2HuffmanTable(0, 0u, 4294967295u, 0u)
		};
		private JBIG2HuffmanTable[] huffTableO = new JBIG2HuffmanTable[]
		{
			new JBIG2HuffmanTable(0, 1u, 0u, 0u),
			new JBIG2HuffmanTable(-1, 3u, 0u, 4u),
			new JBIG2HuffmanTable(1, 3u, 0u, 5u),
			new JBIG2HuffmanTable(-2, 4u, 0u, 12u),
			new JBIG2HuffmanTable(2, 4u, 0u, 13u),
			new JBIG2HuffmanTable(-4, 5u, 1u, 28u),
			new JBIG2HuffmanTable(3, 5u, 1u, 29u),
			new JBIG2HuffmanTable(-8, 6u, 2u, 60u),
			new JBIG2HuffmanTable(5, 6u, 2u, 61u),
			new JBIG2HuffmanTable(-24, 7u, 4u, 124u),
			new JBIG2HuffmanTable(9, 7u, 4u, 125u),
			new JBIG2HuffmanTable(-25, 7u, 4294967293u, 126u),
			new JBIG2HuffmanTable(25, 7u, 32u, 127u),
			new JBIG2HuffmanTable(0, 0u, 4294967295u, 0u)
		};
		internal static int[] contextSize = new int[]
		{
			16,
			13,
			10,
			10
		};
		internal static int[] refContextSize = new int[]
		{
			13,
			10
		};
		internal JBIG2Stream(PdfStream strA, PdfObject pParamObj, int nIndex)
		{
			this.str = strA;
			this.pageBitmap = null;
			this.arithDecoder = new JArithmeticDecoder();
			this.genericRegionStats = new JArithmeticDecoderStats(2);
			this.refinementRegionStats = new JArithmeticDecoderStats(2);
			this.iadhStats = new JArithmeticDecoderStats(512);
			this.iadwStats = new JArithmeticDecoderStats(512);
			this.iaexStats = new JArithmeticDecoderStats(512);
			this.iaaiStats = new JArithmeticDecoderStats(512);
			this.iadtStats = new JArithmeticDecoderStats(512);
			this.iaitStats = new JArithmeticDecoderStats(512);
			this.iafsStats = new JArithmeticDecoderStats(512);
			this.iadsStats = new JArithmeticDecoderStats(512);
			this.iardxStats = new JArithmeticDecoderStats(512);
			this.iardyStats = new JArithmeticDecoderStats(512);
			this.iardwStats = new JArithmeticDecoderStats(512);
			this.iardhStats = new JArithmeticDecoderStats(512);
			this.iariStats = new JArithmeticDecoderStats(512);
			this.iaidStats = new JArithmeticDecoderStats(2);
			this.huffDecoder = new JBIG2HuffmanDecoder();
			this.mmrDecoder = new JBIG2MMRDecoder();
			this.globalsStream = null;
			if (pParamObj != null)
			{
				PdfObject pdfObject = null;
				if (pParamObj.m_nType == enumType.pdfDictionary)
				{
					pdfObject = ((PdfDict)pParamObj).GetObjectByName("JBIG2Globals");
				}
				else
				{
					if (pParamObj.m_nType == enumType.pdfArray)
					{
						PdfObject @object = ((PdfArray)pParamObj).GetObject(nIndex);
						if (@object != null && @object.m_nType == enumType.pdfDictionary)
						{
							pdfObject = ((PdfDict)@object).GetObjectByName("JBIG2Globals");
						}
					}
				}
				if (pdfObject != null && pdfObject.m_nType == enumType.pdfDictWithStream)
				{
					this.curStr = ((PdfDictWithStream)pdfObject).m_objStream;
					this.curStr.reset();
					this.arithDecoder.setStream(this.curStr);
					this.huffDecoder.setStream(this.curStr);
					this.mmrDecoder.setStream(this.curStr);
					this.readSegments();
					foreach (JBIG2Segment current in this.segments)
					{
						this.globalSegments.Add(current);
					}
					this.segments.Clear();
				}
			}
			this.curStr = null;
			this.dataPtr = (this.dataEnd = null);
		}
		~JBIG2Stream()
		{
			this.close();
			this.arithDecoder = null;
			this.genericRegionStats = null;
			this.refinementRegionStats = null;
			this.iadhStats = null;
			this.iadwStats = null;
			this.iaexStats = null;
			this.iaaiStats = null;
			this.iadtStats = null;
			this.iaitStats = null;
			this.iafsStats = null;
			this.iadsStats = null;
			this.iardxStats = null;
			this.iardyStats = null;
			this.iardwStats = null;
			this.iardhStats = null;
			this.iariStats = null;
			this.iaidStats = null;
			this.huffDecoder = null;
			this.mmrDecoder = null;
		}
		private void close()
		{
			if (this.pageBitmap != null)
			{
				this.pageBitmap = null;
			}
			if (this.segments.Count > 0)
			{
				this.segments.Clear();
			}
			if (this.globalSegments.Count > 0)
			{
				foreach (JBIG2Segment arg_4B_0 in this.globalSegments)
				{
				}
				this.globalSegments.Clear();
			}
			this.dataPtr = (this.dataEnd = null);
		}
		internal override void reset()
		{
			this.segments.Clear();
			if (this.globalsStream != null && this.globalsStream.m_nType == enumType.pdfDictWithStream)
			{
				this.segments = this.globalSegments;
				this.curStr = ((PdfDictWithStream)this.globalsStream).m_objStream;
				this.curStr.reset();
				this.arithDecoder.setStream(this.curStr);
				this.huffDecoder.setStream(this.curStr);
				this.mmrDecoder.setStream(this.curStr);
				this.readSegments();
			}
			this.curStr = this.str;
			this.curStr.reset();
			this.arithDecoder.setStream(this.curStr);
			this.huffDecoder.setStream(this.curStr);
			this.mmrDecoder.setStream(this.curStr);
			this.readSegments();
			if (this.pageBitmap != null)
			{
				this.dataPtr = new RavenColorPtr(this.pageBitmap.getDataPtr());
				this.dataEnd = new RavenColorPtr(this.dataPtr, this.pageBitmap.getDataSize());
				return;
			}
			this.dataPtr = (this.dataEnd = null);
		}
		internal override int getChar()
		{
			if (this.dataPtr != null && this.dataPtr.offset < this.dataEnd.offset)
			{
				int result = (int)((this.dataPtr.ptr ^ 255) & 255);
				this.dataPtr = ++this.dataPtr;
				return result;
			}
			return -1;
		}
		internal int lookChar()
		{
			if (this.dataPtr != null && this.dataPtr.offset < this.dataEnd.offset)
			{
				return (int)((this.dataPtr.ptr ^ 255) & 255);
			}
			return -1;
		}
		private int getBlock(char[] blk, int size)
		{
			if (size <= 0)
			{
				return 0;
			}
			int num;
			if (this.dataEnd.offset - this.dataPtr.offset < size)
			{
				num = this.dataEnd.offset - this.dataPtr.offset;
			}
			else
			{
				num = size;
			}
			for (int i = 0; i < num; i++)
			{
				blk[i] = (char)(this.dataPtr.ptr ^ 255);
				RavenColorPtr expr_5B = this.dataPtr;
				expr_5B.ptr += 1;
			}
			return num;
		}
		private void readSegments()
		{
			uint num = 0u;
			uint num2 = 0u;
			uint num3 = 0u;
			uint num4 = 0u;
			uint num5 = 0u;
			while (this.readULong(ref num))
			{
				if (this.readUByte(ref num2))
				{
					uint num6 = num2 & 63u;
					if (this.readUByte(ref num5))
					{
						uint num7 = num5 >> 5;
						if (num7 == 7u)
						{
							int @char;
							int char2;
							int char3;
							if ((@char = this.curStr.getChar()) == -1 || (char2 = this.curStr.getChar()) == -1 || (char3 = this.curStr.getChar()) == -1)
							{
								goto IL_495;
							}
							num5 = (num5 << 24 | (uint)((uint)@char << 16) | (uint)((uint)char2 << 8) | (uint)char3);
							num7 = (num5 & 536870911u);
							for (uint num8 = 0u; num8 < num7 + 9u >> 3; num8 += 1u)
							{
								if (this.curStr.getChar() == -1)
								{
									goto IL_495;
								}
							}
						}
						uint[] array = new uint[num7];
						if (num <= 256u)
						{
							for (uint num8 = 0u; num8 < num7; num8 += 1u)
							{
								if (!this.readUByte(ref array[(int)((UIntPtr)num8)]))
								{
									goto IL_495;
								}
							}
						}
						else
						{
							if (num <= 65536u)
							{
								for (uint num8 = 0u; num8 < num7; num8 += 1u)
								{
									if (!this.readUWord(ref array[(int)((UIntPtr)num8)]))
									{
										goto IL_495;
									}
								}
							}
							else
							{
								for (uint num8 = 0u; num8 < num7; num8 += 1u)
								{
									if (!this.readULong(ref array[(int)((UIntPtr)num8)]))
									{
										goto IL_495;
									}
								}
							}
						}
						if ((num2 & 64u) != 0u)
						{
							if (!this.readULong(ref num3))
							{
								goto IL_495;
							}
						}
						else
						{
							if (!this.readUByte(ref num3))
							{
								goto IL_495;
							}
						}
						if (this.readULong(ref num4))
						{
							if (this.pageBitmap == null && ((num6 >= 4u && num6 <= 7u) || (num6 >= 20u && num6 <= 43u)))
							{
								AuxException.Throw("First JBIG2 segment associated with a page must be a page information segment.");
								return;
							}
							this.arithDecoder.resetByteCounter();
							this.huffDecoder.resetByteCounter();
							this.mmrDecoder.resetByteCounter();
							this.byteCounter = 0u;
							uint num9 = num6;
							if (num9 <= 16u)
							{
								if (num9 != 0u)
								{
									switch (num9)
									{
									case 4u:
										this.readTextRegionSeg(num, false, false, num4, array, num7);
										break;
									case 5u:
										goto IL_3B8;
									case 6u:
										this.readTextRegionSeg(num, true, false, num4, array, num7);
										break;
									case 7u:
										this.readTextRegionSeg(num, true, true, num4, array, num7);
										break;
									default:
										if (num9 != 16u)
										{
											goto IL_3B8;
										}
										this.readPatternDictSeg(num, num4);
										break;
									}
								}
								else
								{
									if (!this.readSymbolDictSeg(num, num4, array, num7))
									{
										return;
									}
								}
							}
							else
							{
								switch (num9)
								{
								case 20u:
									this.readHalftoneRegionSeg(num, false, false, num4, array, num7);
									break;
								case 21u:
									goto IL_3B8;
								case 22u:
									this.readHalftoneRegionSeg(num, true, false, num4, array, num7);
									break;
								case 23u:
									this.readHalftoneRegionSeg(num, true, true, num4, array, num7);
									break;
								default:
									switch (num9)
									{
									case 36u:
										this.readGenericRegionSeg(num, false, false, num4);
										break;
									case 37u:
									case 41u:
									case 44u:
									case 45u:
									case 46u:
									case 47u:
									case 49u:
									case 51u:
										goto IL_3B8;
									case 38u:
										this.readGenericRegionSeg(num, true, false, num4);
										break;
									case 39u:
										this.readGenericRegionSeg(num, true, true, num4);
										break;
									case 40u:
										this.readGenericRefinementRegionSeg(num, false, false, num4, array, num7);
										break;
									case 42u:
										this.readGenericRefinementRegionSeg(num, true, false, num4, array, num7);
										break;
									case 43u:
										this.readGenericRefinementRegionSeg(num, true, true, num4, array, num7);
										break;
									case 48u:
										this.readPageInfoSeg(num4);
										break;
									case 50u:
										this.readEndOfStripeSeg(num4);
										break;
									case 52u:
										this.readProfilesSeg(num4);
										break;
									case 53u:
										this.readCodeTableSeg(num, num4);
										break;
									default:
										if (num9 != 62u)
										{
											goto IL_3B8;
										}
										this.readExtensionSeg(num4);
										break;
									}
									break;
								}
							}
							IL_3E7:
							if (num6 == 38u && num4 == 4294967295u)
							{
								continue;
							}
							this.byteCounter += this.arithDecoder.getByteCounter();
							this.byteCounter += this.huffDecoder.getByteCounter();
							this.byteCounter += this.mmrDecoder.getByteCounter();
							if (this.byteCounter > num4 || num4 - this.byteCounter > 65536u)
							{
								AuxException.Throw("Invalid segment length in JBIG2 stream.");
								return;
							}
							while (this.byteCounter < num4 && this.curStr.getChar() != -1)
							{
								this.byteCounter += 1u;
							}
							continue;
							IL_3B8:
							AuxException.Throw("Unknown segment type in JBIG2 stream.");
							for (uint num8 = 0u; num8 < num4; num8 += 1u)
							{
								if (this.curStr.getChar() == -1)
								{
									goto IL_495;
								}
							}
							goto IL_3E7;
						}
					}
				}
				IL_495:
				AuxException.Throw("Unexpected EOF in JBIG2 stream.");
				return;
			}
		}
		private bool readUByte(ref uint x)
		{
			int @char;
			if ((@char = this.curStr.getChar()) == -1)
			{
				return false;
			}
			this.byteCounter += 1u;
			x = (uint)@char;
			return true;
		}
		private bool readByte(ref int x)
		{
			int @char;
			if ((@char = this.curStr.getChar()) == -1)
			{
				return false;
			}
			this.byteCounter += 1u;
			x = @char;
			if ((@char & 128) != 0)
			{
				x |= -256;
			}
			return true;
		}
		private bool readUWord(ref uint x)
		{
			int @char;
			int char2;
			if ((@char = this.curStr.getChar()) == -1 || (char2 = this.curStr.getChar()) == -1)
			{
				return false;
			}
			this.byteCounter += 2u;
			x = (uint)(@char << 8 | char2);
			return true;
		}
		private bool readULong(ref uint x)
		{
			int @char;
			int char2;
			int char3;
			int char4;
			if ((@char = this.curStr.getChar()) == -1 || (char2 = this.curStr.getChar()) == -1 || (char3 = this.curStr.getChar()) == -1 || (char4 = this.curStr.getChar()) == -1)
			{
				return false;
			}
			this.byteCounter += 4u;
			x = (uint)(@char << 24 | char2 << 16 | char3 << 8 | char4);
			return true;
		}
		private bool readLong(ref int x)
		{
			int @char;
			int char2;
			int char3;
			int char4;
			if ((@char = this.curStr.getChar()) == -1 || (char2 = this.curStr.getChar()) == -1 || (char3 = this.curStr.getChar()) == -1 || (char4 = this.curStr.getChar()) == -1)
			{
				return false;
			}
			this.byteCounter += 4u;
			x = (@char << 24 | char2 << 16 | char3 << 8 | char4);
			int arg_68_0 = @char & 128;
			return true;
		}
		private bool readSymbolDictSeg(uint segNum, uint length, uint[] refSegs, uint nRefSegs)
		{
			List<JBIG2Segment> list = new List<JBIG2Segment>();
			uint num = 0u;
			int[] array = new int[4];
			int[] array2 = new int[4];
			int[] array3 = new int[2];
			int[] array4 = new int[2];
			uint num2 = 0u;
			uint num3 = 0u;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int refDX = 0;
			int refDY = 0;
			int num7 = 0;
			int num8 = 0;
			uint[] array5 = null;
			if (this.readUWord(ref num))
			{
				uint num9 = num >> 10 & 3u;
				uint num10 = num >> 12 & 1u;
				uint num11 = num & 1u;
				uint num12 = num >> 1 & 1u;
				uint num13 = num >> 2 & 3u;
				uint num14 = num >> 4 & 3u;
				uint num15 = num >> 6 & 1u;
				uint num16 = num >> 7 & 1u;
				uint num17 = num >> 8 & 1u;
				uint num18 = num >> 9 & 1u;
				if (num11 == 0u)
				{
					if (num9 == 0u)
					{
						if (!this.readByte(ref array[0]) || !this.readByte(ref array2[0]) || !this.readByte(ref array[1]) || !this.readByte(ref array2[1]) || !this.readByte(ref array[2]) || !this.readByte(ref array2[2]) || !this.readByte(ref array[3]))
						{
							goto IL_98E;
						}
						if (!this.readByte(ref array2[3]))
						{
							goto IL_98E;
						}
					}
					else
					{
						if (!this.readByte(ref array[0]) || !this.readByte(ref array2[0]))
						{
							goto IL_98E;
						}
					}
				}
				if ((num12 == 0u || num10 != 0u || (this.readByte(ref array3[0]) && this.readByte(ref array4[0]) && this.readByte(ref array3[1]) && this.readByte(ref array4[1]))) && this.readULong(ref num2) && this.readULong(ref num3))
				{
					uint num19 = 0u;
					for (uint num20 = 0u; num20 < nRefSegs; num20 += 1u)
					{
						JBIG2Segment jBIG2Segment;
						if ((jBIG2Segment = this.findSegment(refSegs[(int)((UIntPtr)num20)])) != null)
						{
							if (jBIG2Segment.getType() == JBIG2SegmentType.jbig2SegSymbolDict)
							{
								uint num21 = ((JBIG2SymbolDict)jBIG2Segment).getSize();
								if (num19 > 4294967295u - num21)
								{
									AuxException.Throw("Too many input symbols in JBIG2 symbol dictionary.");
									list.Clear();
									goto IL_98E;
								}
								num19 += num21;
							}
							else
							{
								if (jBIG2Segment.getType() == JBIG2SegmentType.jbig2SegCodeTable)
								{
									list.Add(jBIG2Segment);
								}
							}
						}
					}
					if (num19 <= 4294967295u - num3)
					{
						uint num20 = num19 + num3;
						uint num22;
						if (num20 <= 1u)
						{
							num22 = ((num11 != 0u) ? 1u : 0u);
						}
						else
						{
							num20 -= 1u;
							num22 = 0u;
							while (num20 > 0u)
							{
								num22 += 1u;
								num20 >>= 1;
							}
						}
						JBIG2Bitmap[] array6 = new JBIG2Bitmap[num19 + num3];
						for (num20 = 0u; num20 < num19 + num3; num20 += 1u)
						{
							array6[(int)((UIntPtr)num20)] = null;
						}
						uint num23 = 0u;
						JBIG2SymbolDict jBIG2SymbolDict = null;
						uint num21;
						for (num20 = 0u; num20 < nRefSegs; num20 += 1u)
						{
							JBIG2Segment jBIG2Segment;
							if ((jBIG2Segment = this.findSegment(refSegs[(int)((UIntPtr)num20)])) != null && jBIG2Segment.getType() == JBIG2SegmentType.jbig2SegSymbolDict)
							{
								jBIG2SymbolDict = (JBIG2SymbolDict)jBIG2Segment;
								for (num21 = 0u; num21 < jBIG2SymbolDict.getSize(); num21 += 1u)
								{
									array6[(int)((UIntPtr)(num23++))] = jBIG2SymbolDict.getBitmap(num21);
								}
							}
						}
						JBIG2HuffmanTable[] table2;
						JBIG2HuffmanTable[] table = table2 = null;
						JBIG2HuffmanTable[] table4;
						JBIG2HuffmanTable[] table3 = table4 = null;
						num20 = 0u;
						if (num11 != 0u)
						{
							if (num13 == 0u)
							{
								table2 = this.huffTableD;
							}
							else
							{
								if (num13 == 1u)
								{
									table2 = this.huffTableE;
								}
								else
								{
									if (num20 >= (uint)list.Count)
									{
										goto IL_967;
									}
									table2 = ((JBIG2CodeTable)list[(int)num20++]).getHuffTable();
								}
							}
							if (num14 == 0u)
							{
								table = this.huffTableB;
							}
							else
							{
								if (num14 == 1u)
								{
									table = this.huffTableC;
								}
								else
								{
									if (num20 >= (uint)list.Count)
									{
										goto IL_967;
									}
									table = ((JBIG2CodeTable)list[(int)num20++]).getHuffTable();
								}
							}
							if (num15 == 0u)
							{
								table4 = this.huffTableA;
							}
							else
							{
								if (num20 >= (uint)list.Count)
								{
									goto IL_967;
								}
								table4 = ((JBIG2CodeTable)list[(int)num20++]).getHuffTable();
							}
							if (num16 == 0u)
							{
								table3 = this.huffTableA;
								goto IL_437;
							}
							if (num20 < (uint)list.Count)
							{
								table3 = ((JBIG2CodeTable)list[(int)num20++]).getHuffTable();
								goto IL_437;
							}
							IL_967:
							AuxException.Throw("Missing code table in JBIG2 symbol dictionary.");
							list.Clear();
							goto IL_978;
						}
						IL_437:
						list.Clear();
						if (num11 != 0u)
						{
							this.huffDecoder.reset();
						}
						else
						{
							if (num17 != 0u && jBIG2SymbolDict != null)
							{
								this.resetGenericStats(num9, jBIG2SymbolDict.getGenericRegionStats());
							}
							else
							{
								this.resetGenericStats(num9, null);
							}
							this.resetIntStats((int)num22);
							this.arithDecoder.start();
						}
						if (num12 != 0u)
						{
							if (num17 != 0u && jBIG2SymbolDict != null)
							{
								this.resetRefinementStats(num10, jBIG2SymbolDict.getRefinementRegionStats());
							}
							else
							{
								this.resetRefinementStats(num10, null);
							}
						}
						if (num11 != 0u && num12 == 0u)
						{
							array5 = new uint[num3];
						}
						uint num24 = 0u;
						num20 = 0u;
						while (num20 < num3)
						{
							if (num11 != 0u)
							{
								this.huffDecoder.decodeInt(ref num4, table2);
							}
							else
							{
								this.arithDecoder.decodeInt(ref num4, this.iadhStats);
							}
							if (num4 < 0 && -num4 >= (int)num24)
							{
								AuxException.Throw("Bad delta-height value in JBIG2 symbol dictionary.");
								goto IL_978;
							}
							num24 += (uint)num4;
							uint num25 = 0u;
							uint num26 = 0u;
							num21 = num20;
							while (true)
							{
								if (num11 != 0u)
								{
									if (!this.huffDecoder.decodeInt(ref num5, table))
									{
										break;
									}
								}
								else
								{
									if (!this.arithDecoder.decodeInt(ref num5, this.iadwStats))
									{
										break;
									}
								}
								if (num5 < 0 && -num5 >= (int)num25)
								{
									goto Block_57;
								}
								num25 += (uint)num5;
								if (num20 >= num3)
								{
									goto Block_58;
								}
								if (num11 != 0u && num12 == 0u)
								{
									array5[(int)((UIntPtr)num20)] = num25;
									num26 += num25;
								}
								else
								{
									if (num12 != 0u)
									{
										if (num11 != 0u)
										{
											if (!this.huffDecoder.decodeInt(ref num6, table3))
											{
												break;
											}
										}
										else
										{
											if (!this.arithDecoder.decodeInt(ref num6, this.iaaiStats))
											{
												break;
											}
										}
										if (num6 == 1)
										{
											uint num27;
											if (num11 != 0u)
											{
												num27 = this.huffDecoder.readBits(num22);
												this.huffDecoder.decodeInt(ref refDX, this.huffTableO);
												this.huffDecoder.decodeInt(ref refDY, this.huffTableO);
												this.huffDecoder.decodeInt(ref num7, this.huffTableA);
												this.huffDecoder.reset();
												this.arithDecoder.start();
											}
											else
											{
												num27 = this.arithDecoder.decodeIAID(num22, this.iaidStats);
												this.arithDecoder.decodeInt(ref refDX, this.iardxStats);
												this.arithDecoder.decodeInt(ref refDY, this.iardyStats);
											}
											if (num27 >= num19 + num20)
											{
												goto Block_66;
											}
											JBIG2Bitmap refBitmap = array6[(int)((UIntPtr)num27)];
											array6[(int)((UIntPtr)(num19 + num20))] = this.readGenericRefinementRegion((int)num25, (int)num24, (int)num10, false, refBitmap, refDX, refDY, array3, array4);
										}
										else
										{
											array6[(int)((UIntPtr)(num19 + num20))] = this.readTextRegion(num11 != 0u, true, (int)num25, (int)num24, (uint)num6, 0u, (int)(num19 + num20), null, num22, array6, 0u, 0u, 0u, 1u, 0, this.huffTableF, this.huffTableH, this.huffTableK, this.huffTableO, this.huffTableO, this.huffTableO, this.huffTableO, this.huffTableA, num10, array3, array4);
										}
									}
									else
									{
										array6[(int)((UIntPtr)(num19 + num20))] = this.readGenericBitmap(false, (int)num25, (int)num24, (int)num9, false, false, null, array, array2, 0);
									}
								}
								num20 += 1u;
							}
							IL_759:
							if (num11 != 0u && num12 == 0u)
							{
								this.huffDecoder.decodeInt(ref num7, table4);
								this.huffDecoder.reset();
								JBIG2Bitmap jBIG2Bitmap;
								if (num7 == 0)
								{
									jBIG2Bitmap = new JBIG2Bitmap(0u, (int)num26, (int)num24);
									num7 = (int)(num24 * (num26 + 7u >> 3));
									RavenColorPtr ravenColorPtr = new RavenColorPtr(jBIG2Bitmap.getDataPtr());
									for (num23 = 0u; num23 < (uint)num7; num23 += 1u)
									{
										int @char;
										if ((@char = this.curStr.getChar()) == -1)
										{
											break;
										}
										ravenColorPtr.ptr = (byte)@char;
										ravenColorPtr = ++ravenColorPtr;
										this.byteCounter += 1u;
									}
								}
								else
								{
									jBIG2Bitmap = this.readGenericBitmap(true, (int)num26, (int)num24, 0, false, false, null, null, null, num7);
								}
								uint num28 = 0u;
								while (num21 < num20)
								{
									array6[(int)((UIntPtr)(num19 + num21))] = jBIG2Bitmap.getSlice(num28, 0u, array5[(int)((UIntPtr)num21)], num24);
									num28 += array5[(int)((UIntPtr)num21)];
									num21 += 1u;
								}
								continue;
							}
							continue;
							goto IL_759;
							Block_57:
							AuxException.Throw("Bad delta-height value in JBIG2 symbol dictionary.");
							goto IL_978;
							Block_58:
							AuxException.Throw("Too many symbols in JBIG2 symbol dictionary.");
							goto IL_978;
							Block_66:
							AuxException.Throw("Invalid symbol ID in JBIG2 symbol dictionary.");
							goto IL_978;
						}
						JBIG2SymbolDict jBIG2SymbolDict2 = new JBIG2SymbolDict(segNum, num2);
						num21 = (num20 = 0u);
						bool flag = false;
						while (num20 < num19 + num3)
						{
							if (num11 != 0u)
							{
								this.huffDecoder.decodeInt(ref num8, this.huffTableA);
							}
							else
							{
								this.arithDecoder.decodeInt(ref num8, this.iaexStats);
							}
							if ((ulong)num20 + (ulong)((long)num8) > (ulong)(num19 + num3) || (flag && (ulong)num21 + (ulong)((long)num8) > (ulong)num2))
							{
								AuxException.Throw("Too many exported symbols in JBIG2 symbol dictionary.");
								goto IL_978;
							}
							if (flag)
							{
								for (int i = 0; i < num8; i++)
								{
									jBIG2SymbolDict2.setBitmap(num21++, array6[(int)((UIntPtr)(num20++))].copy());
								}
							}
							else
							{
								num20 += (uint)num8;
							}
							flag = !flag;
						}
						if (num21 == num2)
						{
							for (num20 = 0u; num20 < num3; num20 += 1u)
							{
							}
							if (num11 == 0u && num18 != 0u)
							{
								jBIG2SymbolDict2.setGenericRegionStats(this.genericRegionStats.copy());
								if (num12 != 0u)
								{
									jBIG2SymbolDict2.setRefinementRegionStats(this.refinementRegionStats.copy());
								}
							}
							this.segments.Add(jBIG2SymbolDict2);
							return true;
						}
						AuxException.Throw("Too few symbols in JBIG2 symbol dictionary.");
						IL_978:
						for (num20 = 0u; num20 < num3; num20 += 1u)
						{
						}
						return false;
					}
					AuxException.Throw("Too many input symbols in JBIG2 symbol dictionary.");
					list.Clear();
				}
			}
			IL_98E:
			AuxException.Throw("Unexpected EOF in JBIG2 stream.");
			return false;
		}
		private void readTextRegionSeg(uint segNum, bool imm, bool lossless, uint length, uint[] refSegs, uint nRefSegs)
		{
			JBIG2HuffmanTable[] array = new JBIG2HuffmanTable[36];
			List<JBIG2Segment> list = new List<JBIG2Segment>();
			uint w = 0u;
			uint num = 0u;
			uint x = 0u;
			uint num2 = 0u;
			uint num3 = 0u;
			uint num4 = 0u;
			uint num5 = 0u;
			uint numInstances = 0u;
			int[] array2 = new int[2];
			int[] array3 = new int[2];
			int num6 = 0;
			if (this.readULong(ref w) && this.readULong(ref num) && this.readULong(ref x) && this.readULong(ref num2) && this.readUByte(ref num3))
			{
				uint combOp = num3 & 7u;
				if (this.readUWord(ref num4))
				{
					uint num7 = num4 & 1u;
					uint num8 = num4 >> 1 & 1u;
					uint logStrips = num4 >> 2 & 3u;
					uint refCorner = num4 >> 4 & 3u;
					uint transposed = num4 >> 6 & 1u;
					uint combOp2 = num4 >> 7 & 3u;
					uint defPixel = num4 >> 9 & 1u;
					int num9 = (int)(num4 >> 10 & 31u);
					if ((num9 & 16) != 0)
					{
						num9 |= -16;
					}
					uint num10 = num4 >> 15 & 1u;
					uint num13;
					uint num12;
					uint num11 = num12 = (num13 = 0u);
					uint num18;
					uint num17;
					uint num16;
					uint num15;
					uint num14 = num15 = (num16 = (num17 = (num18 = 0u)));
					if (num7 != 0u)
					{
						if (!this.readUWord(ref num5))
						{
							goto IL_84C;
						}
						num12 = (num5 & 3u);
						num11 = (num5 >> 2 & 3u);
						num13 = (num5 >> 4 & 3u);
						num15 = (num5 >> 6 & 3u);
						num14 = (num5 >> 8 & 3u);
						num16 = (num5 >> 10 & 3u);
						num17 = (num5 >> 12 & 3u);
						num18 = (num5 >> 14 & 1u);
					}
					if ((num8 == 0u || num10 != 0u || (this.readByte(ref array2[0]) && this.readByte(ref array3[0]) && this.readByte(ref array2[1]) && this.readByte(ref array3[1]))) && this.readULong(ref numInstances))
					{
						uint num19 = 0u;
						uint num20;
						for (num20 = 0u; num20 < nRefSegs; num20 += 1u)
						{
							JBIG2Segment jBIG2Segment;
							if ((jBIG2Segment = this.findSegment(refSegs[(int)((UIntPtr)num20)])) == null)
							{
								AuxException.Throw("Invalid segment reference in JBIG2 text region.");
								foreach (JBIG2Segment arg_21B_0 in list)
								{
								}
								list.Clear();
								return;
							}
							if (jBIG2Segment.getType() == JBIG2SegmentType.jbig2SegSymbolDict)
							{
								num19 += ((JBIG2SymbolDict)jBIG2Segment).getSize();
							}
							else
							{
								if (jBIG2Segment.getType() == JBIG2SegmentType.jbig2SegCodeTable)
								{
									list.Add(jBIG2Segment);
								}
							}
						}
						num20 = num19;
						uint num21;
						if (num20 <= 1u)
						{
							num21 = ((num7 != 0u) ? 1u : 0u);
						}
						else
						{
							num20 -= 1u;
							num21 = 0u;
							while (num20 > 0u)
							{
								num21 += 1u;
								num20 >>= 1;
							}
						}
						JBIG2Bitmap[] array4 = new JBIG2Bitmap[num19];
						uint num22 = 0u;
						for (num20 = 0u; num20 < nRefSegs; num20 += 1u)
						{
							JBIG2Segment jBIG2Segment;
							if ((jBIG2Segment = this.findSegment(refSegs[(int)((UIntPtr)num20)])) != null && jBIG2Segment.getType() == JBIG2SegmentType.jbig2SegSymbolDict)
							{
								JBIG2SymbolDict jBIG2SymbolDict = (JBIG2SymbolDict)jBIG2Segment;
								for (uint num23 = 0u; num23 < jBIG2SymbolDict.getSize(); num23 += 1u)
								{
									array4[(int)((UIntPtr)(num22++))] = jBIG2SymbolDict.getBitmap(num23);
								}
							}
						}
						JBIG2HuffmanTable[] huffDTTable;
						JBIG2HuffmanTable[] huffFSTable;
						JBIG2HuffmanTable[] huffDSTable = huffFSTable = (huffDTTable = null);
						JBIG2HuffmanTable[] huffRDWTable;
						JBIG2HuffmanTable[] huffRDHTable = huffRDWTable = null;
						JBIG2HuffmanTable[] huffRSizeTable;
						JBIG2HuffmanTable[] huffRDXTable;
						JBIG2HuffmanTable[] huffRDYTable = huffRDXTable = (huffRSizeTable = null);
						num20 = 0u;
						if (num7 != 0u)
						{
							if (num12 == 0u)
							{
								huffFSTable = this.huffTableF;
							}
							else
							{
								if (num12 == 1u)
								{
									huffFSTable = this.huffTableG;
								}
								else
								{
									if (num20 >= (uint)list.Count)
									{
										goto IL_83A;
									}
									huffFSTable = ((JBIG2CodeTable)list[(int)num20++]).getHuffTable();
								}
							}
							if (num11 == 0u)
							{
								huffDSTable = this.huffTableH;
							}
							else
							{
								if (num11 == 1u)
								{
									huffDSTable = this.huffTableI;
								}
								else
								{
									if (num11 == 2u)
									{
										huffDSTable = this.huffTableJ;
									}
									else
									{
										if (num20 >= (uint)list.Count)
										{
											goto IL_83A;
										}
										huffDSTable = ((JBIG2CodeTable)list[(int)num20++]).getHuffTable();
									}
								}
							}
							if (num13 == 0u)
							{
								huffDTTable = this.huffTableK;
							}
							else
							{
								if (num13 == 1u)
								{
									huffDTTable = this.huffTableL;
								}
								else
								{
									if (num13 == 2u)
									{
										huffDTTable = this.huffTableM;
									}
									else
									{
										if (num20 >= (uint)list.Count)
										{
											goto IL_83A;
										}
										huffDTTable = ((JBIG2CodeTable)list[(int)num20++]).getHuffTable();
									}
								}
							}
							if (num15 == 0u)
							{
								huffRDWTable = this.huffTableN;
							}
							else
							{
								if (num15 == 1u)
								{
									huffRDWTable = this.huffTableO;
								}
								else
								{
									if (num20 >= (uint)list.Count)
									{
										goto IL_83A;
									}
									huffRDWTable = ((JBIG2CodeTable)list[(int)num20++]).getHuffTable();
								}
							}
							if (num14 == 0u)
							{
								huffRDHTable = this.huffTableN;
							}
							else
							{
								if (num14 == 1u)
								{
									huffRDHTable = this.huffTableO;
								}
								else
								{
									if (num20 >= (uint)list.Count)
									{
										goto IL_83A;
									}
									huffRDHTable = ((JBIG2CodeTable)list[(int)num20++]).getHuffTable();
								}
							}
							if (num16 == 0u)
							{
								huffRDXTable = this.huffTableN;
							}
							else
							{
								if (num16 == 1u)
								{
									huffRDXTable = this.huffTableO;
								}
								else
								{
									if (num20 >= (uint)list.Count)
									{
										goto IL_83A;
									}
									huffRDXTable = ((JBIG2CodeTable)list[(int)num20++]).getHuffTable();
								}
							}
							if (num17 == 0u)
							{
								huffRDYTable = this.huffTableN;
							}
							else
							{
								if (num17 == 1u)
								{
									huffRDYTable = this.huffTableO;
								}
								else
								{
									if (num20 >= (uint)list.Count)
									{
										goto IL_83A;
									}
									huffRDYTable = ((JBIG2CodeTable)list[(int)num20++]).getHuffTable();
								}
							}
							if (num18 == 0u)
							{
								huffRSizeTable = this.huffTableA;
								goto IL_53E;
							}
							if (num20 < (uint)list.Count)
							{
								huffRSizeTable = ((JBIG2CodeTable)list[(int)num20++]).getHuffTable();
								goto IL_53E;
							}
							IL_83A:
							AuxException.Throw("Missing code table in JBIG2 text region.");
							list.Clear();
							return;
						}
						IL_53E:
						foreach (JBIG2Segment arg_550_0 in list)
						{
						}
						list.Clear();
						JBIG2HuffmanTable[] array5;
						if (num7 != 0u)
						{
							this.huffDecoder.reset();
							for (num20 = 0u; num20 < 32u; num20 += 1u)
							{
								array[(int)((UIntPtr)num20)].val = (int)num20;
								array[(int)((UIntPtr)num20)].prefixLen = this.huffDecoder.readBits(4u);
								array[(int)((UIntPtr)num20)].rangeLen = 0u;
							}
							array[32].val = 259;
							array[32].prefixLen = this.huffDecoder.readBits(4u);
							array[32].rangeLen = 2u;
							array[33].val = 515;
							array[33].prefixLen = this.huffDecoder.readBits(4u);
							array[33].rangeLen = 3u;
							array[34].val = 523;
							array[34].prefixLen = this.huffDecoder.readBits(4u);
							array[34].rangeLen = 7u;
							array[35].prefixLen = 0u;
							array[35].rangeLen = 4294967295u;
							this.huffDecoder.buildTable(array, 35u);
							array5 = new JBIG2HuffmanTable[num19 + 1u];
							for (num20 = 0u; num20 < num19; num20 += 1u)
							{
								array5[(int)((UIntPtr)num20)].val = (int)num20;
								array5[(int)((UIntPtr)num20)].rangeLen = 0u;
							}
							num20 = 0u;
							while (num20 < num19)
							{
								this.huffDecoder.decodeInt(ref num6, array);
								if (num6 > 512)
								{
									for (num6 -= 512; num6 != 0; num6--)
									{
										if (num20 >= num19)
										{
											break;
										}
										array5[(int)((UIntPtr)(num20++))].prefixLen = 0u;
									}
								}
								else
								{
									if (num6 > 256)
									{
										for (num6 -= 256; num6 != 0; num6--)
										{
											if (num20 >= num19)
											{
												break;
											}
											array5[(int)((UIntPtr)num20)].prefixLen = array5[(int)((UIntPtr)(num20 - 1u))].prefixLen;
											num20 += 1u;
										}
									}
									else
									{
										array5[(int)((UIntPtr)(num20++))].prefixLen = (uint)num6;
									}
								}
							}
							array5[(int)((UIntPtr)num19)].prefixLen = 0u;
							array5[(int)((UIntPtr)num19)].rangeLen = 4294967295u;
							this.huffDecoder.buildTable(array5, num19);
							this.huffDecoder.reset();
						}
						else
						{
							array5 = null;
							this.resetIntStats((int)num21);
							this.arithDecoder.start();
						}
						if (num8 != 0u)
						{
							this.resetRefinementStats(num10, null);
						}
						JBIG2Bitmap jBIG2Bitmap = this.readTextRegion(num7 != 0u, num8 != 0u, (int)w, (int)num, numInstances, logStrips, (int)num19, array5, num21, array4, defPixel, combOp2, transposed, refCorner, num9, huffFSTable, huffDSTable, huffDTTable, huffRDWTable, huffRDHTable, huffRDXTable, huffRDYTable, huffRSizeTable, num10, array2, array3);
						if (imm)
						{
							if (this.pageH == 4294967295u && num2 + num > this.curPageH)
							{
								this.pageBitmap.expand((int)(num2 + num), this.pageDefPixel);
							}
							this.pageBitmap.combine(jBIG2Bitmap, (int)x, (int)num2, combOp);
						}
						else
						{
							jBIG2Bitmap.setSegNum(segNum);
							this.segments.Add(jBIG2Bitmap);
						}
						return;
					}
				}
			}
			IL_84C:
			AuxException.Throw("Unexpected EOF in JBIG2 stream.");
		}
		private JBIG2Bitmap readTextRegion(bool huff, bool refine, int w, int h, uint numInstances, uint logStrips, int numSyms, JBIG2HuffmanTable[] symCodeTab, uint symCodeLen, JBIG2Bitmap[] syms, uint defPixel, uint combOp, uint transposed, uint refCorner, int sOffset, JBIG2HuffmanTable[] huffFSTable, JBIG2HuffmanTable[] huffDSTable, JBIG2HuffmanTable[] huffDTTable, JBIG2HuffmanTable[] huffRDWTable, JBIG2HuffmanTable[] huffRDHTable, JBIG2HuffmanTable[] huffRDXTable, JBIG2HuffmanTable[] huffRDYTable, JBIG2HuffmanTable[] huffRSizeTable, uint templ, int[] atx, int[] aty)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			int num9 = 0;
			int num10 = 0;
			uint num11 = 1u << (int)logStrips;
			JBIG2Bitmap jBIG2Bitmap = new JBIG2Bitmap(0u, w, h);
			if (defPixel != 0u)
			{
				jBIG2Bitmap.clearToOne();
			}
			else
			{
				jBIG2Bitmap.clearToZero();
			}
			if (huff)
			{
				this.huffDecoder.decodeInt(ref num, huffDTTable);
			}
			else
			{
				this.arithDecoder.decodeInt(ref num, this.iadtStats);
			}
			num *= (int)(-(int)num11);
			uint num12 = 0u;
			int num13 = 0;
			while (num12 < numInstances)
			{
				if (huff)
				{
					this.huffDecoder.decodeInt(ref num2, huffDTTable);
				}
				else
				{
					this.arithDecoder.decodeInt(ref num2, this.iadtStats);
				}
				num += (int)((long)num2 * (long)((ulong)num11));
				if (huff)
				{
					this.huffDecoder.decodeInt(ref num3, huffFSTable);
				}
				else
				{
					this.arithDecoder.decodeInt(ref num3, this.iafsStats);
				}
				num13 += num3;
				int num14 = num13;
				while (num12 < numInstances)
				{
					if (num11 == 1u)
					{
						num2 = 0;
					}
					else
					{
						if (huff)
						{
							num2 = (int)this.huffDecoder.readBits(logStrips);
						}
						else
						{
							this.arithDecoder.decodeInt(ref num2, this.iaitStats);
						}
					}
					int num15 = num + num2;
					uint num16;
					if (huff)
					{
						if (symCodeTab != null)
						{
							this.huffDecoder.decodeInt(ref num4, symCodeTab);
							num16 = (uint)num4;
						}
						else
						{
							num16 = this.huffDecoder.readBits(symCodeLen);
						}
					}
					else
					{
						num16 = this.arithDecoder.decodeIAID(symCodeLen, this.iaidStats);
					}
					if (num16 >= (uint)numSyms)
					{
						AuxException.Throw("Invalid symbol number in JBIG2 text region.");
					}
					else
					{
						if (refine)
						{
							if (huff)
							{
								num9 = (int)this.huffDecoder.readBit();
							}
							else
							{
								this.arithDecoder.decodeInt(ref num9, this.iariStats);
							}
						}
						else
						{
							num9 = 0;
						}
						JBIG2Bitmap jBIG2Bitmap2;
						if (num9 != 0)
						{
							if (huff)
							{
								this.huffDecoder.decodeInt(ref num5, huffRDWTable);
								this.huffDecoder.decodeInt(ref num6, huffRDHTable);
								this.huffDecoder.decodeInt(ref num7, huffRDXTable);
								this.huffDecoder.decodeInt(ref num8, huffRDYTable);
								this.huffDecoder.decodeInt(ref num10, huffRSizeTable);
								this.huffDecoder.reset();
								this.arithDecoder.start();
							}
							else
							{
								this.arithDecoder.decodeInt(ref num5, this.iardwStats);
								this.arithDecoder.decodeInt(ref num6, this.iardhStats);
								this.arithDecoder.decodeInt(ref num7, this.iardxStats);
								this.arithDecoder.decodeInt(ref num8, this.iardyStats);
							}
							int refDX = ((num5 >= 0) ? num5 : (num5 - 1)) / 2 + num7;
							int refDY = ((num6 >= 0) ? num6 : (num6 - 1)) / 2 + num8;
							jBIG2Bitmap2 = this.readGenericRefinementRegion(num5 + syms[(int)((UIntPtr)num16)].getWidth(), num6 + syms[(int)((UIntPtr)num16)].getHeight(), (int)templ, false, syms[(int)((UIntPtr)num16)], refDX, refDY, atx, aty);
						}
						else
						{
							jBIG2Bitmap2 = syms[(int)((UIntPtr)num16)];
						}
						uint num17 = (uint)(jBIG2Bitmap2.getWidth() - 1);
						uint num18 = (uint)(jBIG2Bitmap2.getHeight() - 1);
						if (transposed != 0u)
						{
							switch (refCorner)
							{
							case 0u:
								jBIG2Bitmap.combine(jBIG2Bitmap2, num15, num14, combOp);
								break;
							case 1u:
								jBIG2Bitmap.combine(jBIG2Bitmap2, num15, num14, combOp);
								break;
							case 2u:
								jBIG2Bitmap.combine(jBIG2Bitmap2, (int)((long)num15 - (long)((ulong)num17)), num14, combOp);
								break;
							case 3u:
								jBIG2Bitmap.combine(jBIG2Bitmap2, (int)((long)num15 - (long)((ulong)num17)), num14, combOp);
								break;
							}
							num14 += (int)num18;
						}
						else
						{
							switch (refCorner)
							{
							case 0u:
								jBIG2Bitmap.combine(jBIG2Bitmap2, num14, (int)((long)num15 - (long)((ulong)num18)), combOp);
								break;
							case 1u:
								jBIG2Bitmap.combine(jBIG2Bitmap2, num14, num15, combOp);
								break;
							case 2u:
								jBIG2Bitmap.combine(jBIG2Bitmap2, num14, (int)((long)num15 - (long)((ulong)num18)), combOp);
								break;
							case 3u:
								jBIG2Bitmap.combine(jBIG2Bitmap2, num14, num15, combOp);
								break;
							}
							num14 += (int)num17;
						}
					}
					num12 += 1u;
					if (huff)
					{
						if (!this.huffDecoder.decodeInt(ref num3, huffDSTable))
						{
							break;
						}
					}
					else
					{
						if (!this.arithDecoder.decodeInt(ref num3, this.iadsStats))
						{
							break;
						}
					}
					num14 += sOffset + num3;
				}
			}
			return jBIG2Bitmap;
		}
		private void readPatternDictSeg(uint segNum, uint length)
		{
			uint num = 0u;
			uint num2 = 0u;
			uint num3 = 0u;
			uint num4 = 0u;
			int[] array = new int[4];
			int[] array2 = new int[4];
			if (this.readUByte(ref num) && this.readUByte(ref num2) && this.readUByte(ref num3) && this.readULong(ref num4))
			{
				uint templ = num >> 1 & 3u;
				uint num5 = num & 1u;
				if (num5 == 0u)
				{
					this.resetGenericStats(templ, null);
					this.arithDecoder.start();
				}
				array[0] = (int)(-(int)num2);
				array2[0] = 0;
				array[1] = -3;
				array2[1] = -1;
				array[2] = 2;
				array2[2] = -2;
				array[3] = -2;
				array2[3] = -2;
				JBIG2Bitmap jBIG2Bitmap = this.readGenericBitmap(num5 != 0u, (int)((num4 + 1u) * num2), (int)num3, (int)templ, false, false, null, array, array2, (int)(length - 7u));
				JBIG2PatternDict jBIG2PatternDict = new JBIG2PatternDict(segNum, num4 + 1u);
				uint num6 = 0u;
				for (uint num7 = 0u; num7 <= num4; num7 += 1u)
				{
					jBIG2PatternDict.setBitmap(num7, jBIG2Bitmap.getSlice(num6, 0u, num2, num3));
					num6 += num2;
				}
				this.segments.Add(jBIG2PatternDict);
				return;
			}
			AuxException.Throw("Unexpected EOF in JBIG2 stream.");
		}
		private void readHalftoneRegionSeg(uint segNum, bool imm, bool lossless, uint length, uint[] refSegs, uint nRefSegs)
		{
			uint num = 0u;
			uint num2 = 0u;
			uint x = 0u;
			uint num3 = 0u;
			uint num4 = 0u;
			uint num5 = 0u;
			uint num6 = 0u;
			uint num7 = 0u;
			uint num8 = 0u;
			uint num9 = 0u;
			int[] array = new int[4];
			int[] array2 = new int[4];
			int num10 = 0;
			int num11 = 0;
			if (this.readULong(ref num) && this.readULong(ref num2) && this.readULong(ref x) && this.readULong(ref num3) && this.readUByte(ref num4))
			{
				uint combOp = num4 & 7u;
				if (this.readUByte(ref num5))
				{
					uint num12 = num5 & 1u;
					uint num13 = num5 >> 1 & 3u;
					uint num14 = num5 >> 3 & 1u;
					uint combOp2 = num5 >> 4 & 7u;
					if (this.readULong(ref num6) && this.readULong(ref num7) && this.readLong(ref num10) && this.readLong(ref num11) && this.readUWord(ref num8) && this.readUWord(ref num9))
					{
						if (num == 0u || num2 == 0u || num >= 2147483647u / num2)
						{
							AuxException.Throw("Bad bitmap size in JBIG2 halftone segment.");
							return;
						}
						if (num7 == 0u || num6 >= 2147483647u / num7)
						{
							AuxException.Throw("Bad grid size in JBIG2 halftone segment.");
							return;
						}
						if (nRefSegs != 1u)
						{
							AuxException.Throw("Bad symbol dictionary reference in JBIG2 halftone segment.");
							return;
						}
						JBIG2Segment jBIG2Segment;
						if ((jBIG2Segment = this.findSegment(refSegs[0])) == null || jBIG2Segment.getType() != JBIG2SegmentType.jbig2SegPatternDict)
						{
							AuxException.Throw("Bad symbol dictionary reference in JBIG2 halftone segment.");
							return;
						}
						JBIG2PatternDict jBIG2PatternDict = (JBIG2PatternDict)jBIG2Segment;
						uint num15 = jBIG2PatternDict.getSize();
						uint num16;
						if (num15 <= 1u)
						{
							num16 = 0u;
						}
						else
						{
							num15 -= 1u;
							num16 = 0u;
							while (num15 > 0u)
							{
								num16 += 1u;
								num15 >>= 1;
							}
						}
						uint width = (uint)jBIG2PatternDict.getBitmap(0u).getWidth();
						uint height = (uint)jBIG2PatternDict.getBitmap(0u).getHeight();
						if (num12 == 0u)
						{
							this.resetGenericStats(num13, null);
							this.arithDecoder.start();
						}
						JBIG2Bitmap jBIG2Bitmap = new JBIG2Bitmap(segNum, (int)num, (int)num2);
						if ((num5 & 128u) != 0u)
						{
							jBIG2Bitmap.clearToOne();
						}
						else
						{
							jBIG2Bitmap.clearToZero();
						}
						JBIG2Bitmap jBIG2Bitmap2 = null;
						if (num14 != 0u)
						{
							jBIG2Bitmap2 = new JBIG2Bitmap(0u, (int)num6, (int)num7);
							jBIG2Bitmap2.clearToZero();
							for (uint num17 = 0u; num17 < num7; num17 += 1u)
							{
								for (uint num18 = 0u; num18 < num6; num18 += 1u)
								{
									int num19 = (int)((long)num10 + (long)((ulong)(num17 * num9)) + (long)((ulong)(num18 * num8)));
									int num20 = (int)((long)num11 + (long)((ulong)(num17 * num8)) - (long)((ulong)(num18 * num9)));
									if (num19 + (int)width >> 8 <= 0 || num19 >> 8 >= (int)num || num20 + (int)height >> 8 <= 0 || num20 >> 8 >= (int)num2)
									{
										jBIG2Bitmap2.setPixel((int)num18, (int)num17);
									}
								}
							}
						}
						uint[] array3 = new uint[num6 * num7];
						array[0] = ((num13 <= 1u) ? 3 : 2);
						array2[0] = -1;
						array[1] = -3;
						array2[1] = -1;
						array[2] = 2;
						array2[2] = -2;
						array[3] = -2;
						array2[3] = -2;
						for (int i = (int)(num16 - 1u); i >= 0; i--)
						{
							JBIG2Bitmap jBIG2Bitmap3 = this.readGenericBitmap(num12 != 0u, (int)num6, (int)num7, (int)num13, false, num14 != 0u, jBIG2Bitmap2, array, array2, -1);
							num15 = 0u;
							for (uint num17 = 0u; num17 < num7; num17 += 1u)
							{
								for (uint num18 = 0u; num18 < num6; num18 += 1u)
								{
									int num21 = (int)((long)jBIG2Bitmap3.getPixel((int)num18, (int)num17) ^ (long)((ulong)(array3[(int)((UIntPtr)num15)] & 1u)));
									array3[(int)((UIntPtr)num15)] = (array3[(int)((UIntPtr)num15)] << 1 | (uint)num21);
									num15 += 1u;
								}
							}
						}
						num15 = 0u;
						for (uint num17 = 0u; num17 < num7; num17 += 1u)
						{
							int num19 = (int)((long)num10 + (long)((ulong)(num17 * num9)));
							int num20 = (int)((long)num11 + (long)((ulong)(num17 * num8)));
							for (uint num18 = 0u; num18 < num6; num18 += 1u)
							{
								if (num14 == 0u || jBIG2Bitmap2.getPixel((int)num18, (int)num17) == 0)
								{
									JBIG2Bitmap bitmap = jBIG2PatternDict.getBitmap(array3[(int)((UIntPtr)num15)]);
									jBIG2Bitmap.combine(bitmap, num19 >> 8, num20 >> 8, combOp2);
								}
								num19 += (int)num8;
								num20 -= (int)num9;
								num15 += 1u;
							}
						}
						if (imm)
						{
							if (this.pageH == 4294967295u && num3 + num2 > this.curPageH)
							{
								this.pageBitmap.expand((int)(num3 + num2), this.pageDefPixel);
							}
							this.pageBitmap.combine(jBIG2Bitmap, (int)x, (int)num3, combOp);
							return;
						}
						this.segments.Add(jBIG2Bitmap);
						return;
					}
				}
			}
			AuxException.Throw("Unexpected EOF in JBIG2 stream.");
		}
		private void readGenericRegionSeg(uint segNum, bool imm, bool lossless, uint length)
		{
			uint w = 0u;
			uint num = 0u;
			uint x = 0u;
			uint num2 = 0u;
			uint num3 = 0u;
			uint num4 = 0u;
			uint num5 = 0u;
			int[] array = new int[4];
			int[] array2 = new int[4];
			if (this.readULong(ref w) && this.readULong(ref num) && this.readULong(ref x) && this.readULong(ref num2) && this.readUByte(ref num3))
			{
				uint combOp = num3 & 7u;
				if (this.readUByte(ref num5))
				{
					uint num6 = num5 & 1u;
					uint num7 = num5 >> 1 & 3u;
					uint num8 = num5 >> 3 & 1u;
					if (num6 == 0u)
					{
						if (num7 == 0u)
						{
							if (!this.readByte(ref array[0]) || !this.readByte(ref array2[0]) || !this.readByte(ref array[1]) || !this.readByte(ref array2[1]) || !this.readByte(ref array[2]) || !this.readByte(ref array2[2]) || !this.readByte(ref array[3]))
							{
								goto IL_204;
							}
							if (!this.readByte(ref array2[3]))
							{
								goto IL_204;
							}
						}
						else
						{
							if (!this.readByte(ref array[0]) || !this.readByte(ref array2[0]))
							{
								goto IL_204;
							}
						}
					}
					if (num6 == 0u)
					{
						this.resetGenericStats(num7, null);
						this.arithDecoder.start();
					}
					JBIG2Bitmap jBIG2Bitmap = this.readGenericBitmap(num6 != 0u, (int)w, (int)num, (int)num7, num8 != 0u, false, null, array, array2, (int)((num6 != 0u) ? (length - 18u) : 0u));
					if (imm)
					{
						if (this.pageH == 4294967295u && num2 + num > this.curPageH)
						{
							this.pageBitmap.expand((int)(num2 + num), this.pageDefPixel);
						}
						this.pageBitmap.combine(jBIG2Bitmap, (int)x, (int)num2, combOp);
					}
					else
					{
						jBIG2Bitmap.setSegNum(segNum);
						this.segments.Add(jBIG2Bitmap);
					}
					if (imm && length == 4294967295u)
					{
						this.readULong(ref num4);
					}
					return;
				}
			}
			IL_204:
			AuxException.Throw("Unexpected EOF in JBIG2 stream.");
		}
		private void mmrAddPixels(int a1, int blackPixels, int[] codingLine, ref int a0i, int w)
		{
			if (a1 > codingLine[a0i])
			{
				if (a1 > w)
				{
					AuxException.Throw("JBIG2 MMR row is wrong length.");
					a1 = w;
				}
				if (((a0i & 1) ^ blackPixels) != 0)
				{
					a0i++;
				}
				codingLine[a0i] = a1;
			}
		}
		private void mmrAddPixelsNeg(int a1, int blackPixels, int[] codingLine, ref int a0i, int w)
		{
			if (a1 > codingLine[a0i])
			{
				if (a1 > w)
				{
					AuxException.Throw("JBIG2 MMR row is wrong length.");
					a1 = w;
				}
				if (((a0i & 1) ^ blackPixels) != 0)
				{
					a0i++;
				}
				codingLine[a0i] = a1;
				return;
			}
			if (a1 < codingLine[a0i])
			{
				if (a1 < 0)
				{
					AuxException.Throw("Invalid JBIG2 MMR code.");
					a1 = 0;
				}
				while (a0i > 0 && a1 <= codingLine[a0i - 1])
				{
					a0i--;
				}
				codingLine[a0i] = a1;
			}
		}
		private JBIG2Bitmap readGenericBitmap(bool mmr, int w, int h, int templ, bool tpgdOn, bool useSkip, JBIG2Bitmap skip, int[] atx, int[] aty, int mmrDataLength)
		{
			JBIG2Bitmap jBIG2Bitmap = new JBIG2Bitmap(0u, w, h);
			jBIG2Bitmap.clearToZero();
			if (mmr)
			{
				this.mmrDecoder.reset();
				if (w > 2147483645)
				{
					AuxException.Throw("Bad width in JBIG2 generic bitmap.");
					w = -3;
				}
				int[] array = new int[w + 1];
				int[] array2 = new int[w + 2];
				array[0] = w;
				for (int i = 0; i < h; i++)
				{
					int num = 0;
					while (array[num] < w)
					{
						array2[num] = array[num];
						num++;
					}
					array2[num++] = w;
					array2[num] = w;
					array[0] = 0;
					int num2 = 0;
					int num3 = 0;
					int num4 = 0;
					while (array[num2] < w)
					{
						switch (this.mmrDecoder.get2DCode())
						{
						case -1:
							this.mmrAddPixels(w, 0, array, ref num2, w);
							break;
						case 0:
							this.mmrAddPixels(array2[num3 + 1], num4, array, ref num2, w);
							if (array2[num3 + 1] < w)
							{
								num3 += 2;
							}
							break;
						case 1:
						{
							int num6;
							int num5 = num6 = 0;
							if (num4 != 0)
							{
								int num7;
								do
								{
									num6 += (num7 = this.mmrDecoder.getBlackCode());
								}
								while (num7 >= 64);
								do
								{
									num5 += (num7 = this.mmrDecoder.getWhiteCode());
								}
								while (num7 >= 64);
							}
							else
							{
								int num7;
								do
								{
									num6 += (num7 = this.mmrDecoder.getWhiteCode());
								}
								while (num7 >= 64);
								do
								{
									num5 += (num7 = this.mmrDecoder.getBlackCode());
								}
								while (num7 >= 64);
							}
							this.mmrAddPixels(array[num2] + num6, num4, array, ref num2, w);
							if (array[num2] < w)
							{
								this.mmrAddPixels(array[num2] + num5, num4 ^ 1, array, ref num2, w);
							}
							while (array2[num3] <= array[num2])
							{
								if (array2[num3] >= w)
								{
									break;
								}
								num3 += 2;
							}
							break;
						}
						case 2:
							this.mmrAddPixels(array2[num3], num4, array, ref num2, w);
							num4 ^= 1;
							if (array[num2] < w)
							{
								num3++;
								while (array2[num3] <= array[num2])
								{
									if (array2[num3] >= w)
									{
										break;
									}
									num3 += 2;
								}
							}
							break;
						case 3:
							this.mmrAddPixels(array2[num3] + 1, num4, array, ref num2, w);
							num4 ^= 1;
							if (array[num2] < w)
							{
								num3++;
								while (array2[num3] <= array[num2])
								{
									if (array2[num3] >= w)
									{
										break;
									}
									num3 += 2;
								}
							}
							break;
						case 4:
							this.mmrAddPixelsNeg(array2[num3] - 1, num4, array, ref num2, w);
							num4 ^= 1;
							if (array[num2] < w)
							{
								if (num3 > 0)
								{
									num3--;
								}
								else
								{
									num3++;
								}
								while (array2[num3] <= array[num2])
								{
									if (array2[num3] >= w)
									{
										break;
									}
									num3 += 2;
								}
							}
							break;
						case 5:
							this.mmrAddPixels(array2[num3] + 2, num4, array, ref num2, w);
							num4 ^= 1;
							if (array[num2] < w)
							{
								num3++;
								while (array2[num3] <= array[num2])
								{
									if (array2[num3] >= w)
									{
										break;
									}
									num3 += 2;
								}
							}
							break;
						case 6:
							this.mmrAddPixelsNeg(array2[num3] - 2, num4, array, ref num2, w);
							num4 ^= 1;
							if (array[num2] < w)
							{
								if (num3 > 0)
								{
									num3--;
								}
								else
								{
									num3++;
								}
								while (array2[num3] <= array[num2])
								{
									if (array2[num3] >= w)
									{
										break;
									}
									num3 += 2;
								}
							}
							break;
						case 7:
							this.mmrAddPixels(array2[num3] + 3, num4, array, ref num2, w);
							num4 ^= 1;
							if (array[num2] < w)
							{
								num3++;
								while (array2[num3] <= array[num2])
								{
									if (array2[num3] >= w)
									{
										break;
									}
									num3 += 2;
								}
							}
							break;
						case 8:
							this.mmrAddPixelsNeg(array2[num3] - 3, num4, array, ref num2, w);
							num4 ^= 1;
							if (array[num2] < w)
							{
								if (num3 > 0)
								{
									num3--;
								}
								else
								{
									num3++;
								}
								while (array2[num3] <= array[num2])
								{
									if (array2[num3] >= w)
									{
										break;
									}
									num3 += 2;
								}
							}
							break;
						default:
							AuxException.Throw("Illegal code in JBIG2 MMR bitmap data.");
							this.mmrAddPixels(w, 0, array, ref num2, w);
							break;
						}
					}
					num = 0;
					while (true)
					{
						for (int j = array[num]; j < array[num + 1]; j++)
						{
							jBIG2Bitmap.setPixel(j, i);
						}
						if (array[num + 1] >= w || array[num + 2] >= w)
						{
							break;
						}
						num += 2;
					}
				}
				if (mmrDataLength >= 0)
				{
					this.mmrDecoder.skipTo((uint)mmrDataLength);
				}
				else
				{
					if (this.mmrDecoder.get24Bits() != 4097u)
					{
						AuxException.Throw("Missing EOFB in JBIG2 MMR bitmap data.");
					}
				}
			}
			else
			{
				uint context = 0u;
				if (tpgdOn)
				{
					switch (templ)
					{
					case 0:
						context = 14675u;
						break;
					case 1:
						context = 1946u;
						break;
					case 2:
						context = 227u;
						break;
					case 3:
						context = 394u;
						break;
					}
				}
				bool flag = false;
				int i = 0;
				while (i < h)
				{
					if (!tpgdOn)
					{
						goto IL_561;
					}
					if (this.arithDecoder.decodeBit(context, this.genericRegionStats) != 0)
					{
						flag = !flag;
					}
					if (!flag)
					{
						goto IL_561;
					}
					if (i > 0)
					{
						jBIG2Bitmap.duplicateRow(i, i - 1);
					}
					IL_15A6:
					i++;
					continue;
					IL_561:
					switch (templ)
					{
					case 0:
					{
						RavenColorPtr ravenColorPtr = new RavenColorPtr(jBIG2Bitmap.getDataPtr(), i * jBIG2Bitmap.getLineSize());
						RavenColorPtr ravenColorPtr2 = new RavenColorPtr(jBIG2Bitmap.getDataPtr(), i * jBIG2Bitmap.getLineSize());
						uint num8 = (uint)((uint)ravenColorPtr.ptr << 8);
						ravenColorPtr = ++ravenColorPtr;
						RavenColorPtr ravenColorPtr3;
						uint num9;
						RavenColorPtr ravenColorPtr4;
						uint num10;
						if (i >= 1)
						{
							ravenColorPtr3 = new RavenColorPtr(jBIG2Bitmap.getDataPtr(), (i - 1) * jBIG2Bitmap.getLineSize());
							num9 = (uint)((uint)ravenColorPtr3.ptr << 8);
							ravenColorPtr3 = ++ravenColorPtr3;
							if (i >= 2)
							{
								ravenColorPtr4 = new RavenColorPtr(jBIG2Bitmap.getDataPtr(), (i - 2) * jBIG2Bitmap.getLineSize());
								num10 = (uint)((uint)ravenColorPtr4.ptr << 8);
								ravenColorPtr4 = ++ravenColorPtr4;
							}
							else
							{
								ravenColorPtr4 = null;
								num10 = 0u;
							}
						}
						else
						{
							ravenColorPtr4 = (ravenColorPtr3 = null);
							num10 = (num9 = 0u);
						}
						int j;
						int k;
						if (atx[0] >= -8 && atx[0] <= 8 && atx[1] >= -8 && atx[1] <= 8 && atx[2] >= -8 && atx[2] <= 8 && atx[3] >= -8 && atx[3] <= 8)
						{
							RavenColorPtr ravenColorPtr5;
							uint num11;
							if (i + aty[0] >= 0)
							{
								ravenColorPtr5 = new RavenColorPtr(jBIG2Bitmap.getDataPtr(), (i + aty[0]) * jBIG2Bitmap.getLineSize());
								num11 = (uint)((uint)ravenColorPtr5.ptr << 8);
								ravenColorPtr5 = ++ravenColorPtr5;
							}
							else
							{
								ravenColorPtr5 = null;
								num11 = 0u;
							}
							int num12 = 15 - atx[0];
							RavenColorPtr ravenColorPtr6;
							uint num13;
							if (i + aty[1] >= 0)
							{
								ravenColorPtr6 = new RavenColorPtr(jBIG2Bitmap.getDataPtr(), (i + aty[1]) * jBIG2Bitmap.getLineSize());
								num13 = (uint)((uint)ravenColorPtr6.ptr << 8);
								ravenColorPtr6 = ++ravenColorPtr6;
							}
							else
							{
								ravenColorPtr6 = null;
								num13 = 0u;
							}
							int num14 = 15 - atx[1];
							RavenColorPtr ravenColorPtr7;
							uint num15;
							if (i + aty[2] >= 0)
							{
								ravenColorPtr7 = new RavenColorPtr(jBIG2Bitmap.getDataPtr(), (i + aty[2]) * jBIG2Bitmap.getLineSize());
								num15 = (uint)((uint)ravenColorPtr7.ptr << 8);
								ravenColorPtr7 = ++ravenColorPtr7;
							}
							else
							{
								ravenColorPtr7 = null;
								num15 = 0u;
							}
							int num16 = 15 - atx[2];
							RavenColorPtr ravenColorPtr8;
							uint num17;
							if (i + aty[3] >= 0)
							{
								ravenColorPtr8 = new RavenColorPtr(jBIG2Bitmap.getDataPtr(), (i + aty[3]) * jBIG2Bitmap.getLineSize());
								num17 = (uint)((uint)ravenColorPtr8.ptr << 8);
								ravenColorPtr8 = ++ravenColorPtr8;
							}
							else
							{
								ravenColorPtr8 = null;
								num17 = 0u;
							}
							int num18 = 15 - atx[3];
							k = 0;
							j = 0;
							while (k < w)
							{
								if (k + 8 < w)
								{
									if (ravenColorPtr4 != null)
									{
										num10 |= (uint)ravenColorPtr4.ptr;
										ravenColorPtr4 = ++ravenColorPtr4;
									}
									if (ravenColorPtr3 != null)
									{
										num9 |= (uint)ravenColorPtr3.ptr;
										ravenColorPtr3 = ++ravenColorPtr3;
									}
									num8 |= (uint)ravenColorPtr.ptr;
									ravenColorPtr = ++ravenColorPtr;
									if (ravenColorPtr5 != null)
									{
										num11 |= (uint)ravenColorPtr5.ptr;
										ravenColorPtr5 = ++ravenColorPtr5;
									}
									if (ravenColorPtr6 != null)
									{
										num13 |= (uint)ravenColorPtr6.ptr;
										ravenColorPtr6 = ++ravenColorPtr6;
									}
									if (ravenColorPtr7 != null)
									{
										num15 |= (uint)ravenColorPtr7.ptr;
										ravenColorPtr7 = ++ravenColorPtr7;
									}
									if (ravenColorPtr8 != null)
									{
										num17 |= (uint)ravenColorPtr8.ptr;
										ravenColorPtr8 = ++ravenColorPtr8;
									}
								}
								int num19 = 0;
								byte b = 128;
								while (num19 < 8 && j < w)
								{
									uint num20 = num10 >> 14 & 7u;
									uint num21 = num9 >> 13 & 31u;
									uint num22 = num8 >> 16 & 15u;
									uint context2 = num20 << 13 | num21 << 8 | num22 << 4 | (num11 >> num12 & 1u) << 3 | (num13 >> num14 & 1u) << 2 | (num15 >> num16 & 1u) << 1 | (num17 >> num18 & 1u);
									if ((!useSkip || skip.getPixel(j, i) == 0) && this.arithDecoder.decodeBit(context2, this.genericRegionStats) != 0)
									{
										RavenColorPtr expr_90A = ravenColorPtr2;
										expr_90A.ptr |= b;
										num8 |= 32768u;
										if (aty[0] == 0)
										{
											num11 |= 32768u;
										}
										if (aty[1] == 0)
										{
											num13 |= 32768u;
										}
										if (aty[2] == 0)
										{
											num15 |= 32768u;
										}
										if (aty[3] == 0)
										{
											num17 |= 32768u;
										}
									}
									num10 <<= 1;
									num9 <<= 1;
									num8 <<= 1;
									num11 <<= 1;
									num13 <<= 1;
									num15 <<= 1;
									num17 <<= 1;
									num19++;
									j++;
									b = (byte)(b >> 1);
								}
								k += 8;
								ravenColorPtr2 = ++ravenColorPtr2;
							}
							goto IL_15A6;
						}
						k = 0;
						j = 0;
						while (k < w)
						{
							if (k + 8 < w)
							{
								if (ravenColorPtr4 != null)
								{
									num10 |= (uint)ravenColorPtr4.ptr;
									ravenColorPtr4 = ++ravenColorPtr4;
								}
								if (ravenColorPtr3 != null)
								{
									num9 |= (uint)ravenColorPtr3.ptr;
									ravenColorPtr3 = ++ravenColorPtr3;
								}
								num8 |= (uint)ravenColorPtr.ptr;
								ravenColorPtr = ++ravenColorPtr;
							}
							int num19 = 0;
							byte b = 128;
							while (num19 < 8 && j < w)
							{
								uint num20 = num10 >> 14 & 7u;
								uint num21 = num9 >> 13 & 31u;
								uint num22 = num8 >> 16 & 15u;
								uint context2 = num20 << 13 | num21 << 8 | num22 << 4 | (uint)((uint)jBIG2Bitmap.getPixel(j + atx[0], i + aty[0]) << 3) | (uint)((uint)jBIG2Bitmap.getPixel(j + atx[1], i + aty[1]) << 2) | (uint)((uint)jBIG2Bitmap.getPixel(j + atx[2], i + aty[2]) << 1) | (uint)jBIG2Bitmap.getPixel(j + atx[3], i + aty[3]);
								if ((!useSkip || skip.getPixel(j, i) == 0) && this.arithDecoder.decodeBit(context2, this.genericRegionStats) != 0)
								{
									RavenColorPtr expr_ADF = ravenColorPtr2;
									expr_ADF.ptr |= b;
									num8 |= 32768u;
								}
								num10 <<= 1;
								num9 <<= 1;
								num8 <<= 1;
								num19++;
								j++;
								b = (byte)(b >> 1);
							}
							k += 8;
							ravenColorPtr2 = ++ravenColorPtr2;
						}
						goto IL_15A6;
					}
					case 1:
					{
						RavenColorPtr ravenColorPtr = new RavenColorPtr(jBIG2Bitmap.getDataPtr(), i * jBIG2Bitmap.getLineSize());
						RavenColorPtr ravenColorPtr2 = new RavenColorPtr(jBIG2Bitmap.getDataPtr(), i * jBIG2Bitmap.getLineSize());
						uint num8 = (uint)((uint)ravenColorPtr.ptr << 8);
						ravenColorPtr = ++ravenColorPtr;
						RavenColorPtr ravenColorPtr3;
						uint num9;
						RavenColorPtr ravenColorPtr4;
						uint num10;
						if (i >= 1)
						{
							ravenColorPtr3 = new RavenColorPtr(jBIG2Bitmap.getDataPtr(), (i - 1) * jBIG2Bitmap.getLineSize());
							num9 = (uint)((uint)ravenColorPtr3.ptr << 8);
							ravenColorPtr3 = ++ravenColorPtr3;
							if (i >= 2)
							{
								ravenColorPtr4 = new RavenColorPtr(jBIG2Bitmap.getDataPtr(), (i - 2) * jBIG2Bitmap.getLineSize());
								num10 = (uint)((uint)ravenColorPtr4.ptr << 8);
								ravenColorPtr4 = ++ravenColorPtr4;
							}
							else
							{
								ravenColorPtr4 = null;
								num10 = 0u;
							}
						}
						else
						{
							ravenColorPtr4 = (ravenColorPtr3 = null);
							num10 = (num9 = 0u);
						}
						int j;
						int k;
						if (atx[0] >= -8 && atx[0] <= 8)
						{
							RavenColorPtr ravenColorPtr5;
							uint num11;
							if (i + aty[0] >= 0)
							{
								ravenColorPtr5 = new RavenColorPtr(jBIG2Bitmap.getDataPtr(), (i + aty[0]) * jBIG2Bitmap.getLineSize());
								num11 = (uint)((uint)ravenColorPtr5.ptr << 8);
								ravenColorPtr5 = ++ravenColorPtr5;
							}
							else
							{
								ravenColorPtr5 = null;
								num11 = 0u;
							}
							int num12 = 15 - atx[0];
							k = 0;
							j = 0;
							while (k < w)
							{
								if (k + 8 < w)
								{
									if (ravenColorPtr4 != null)
									{
										num10 |= (uint)ravenColorPtr4.ptr;
										ravenColorPtr4 = ++ravenColorPtr4;
									}
									if (ravenColorPtr3 != null)
									{
										num9 |= (uint)ravenColorPtr3.ptr;
										ravenColorPtr3 = ++ravenColorPtr3;
									}
									num8 |= (uint)ravenColorPtr.ptr;
									ravenColorPtr = ++ravenColorPtr;
									if (ravenColorPtr5 != null)
									{
										num11 |= (uint)ravenColorPtr5.ptr;
										ravenColorPtr5 = ++ravenColorPtr5;
									}
								}
								int num19 = 0;
								byte b = 128;
								while (num19 < 8 && j < w)
								{
									uint num20 = num10 >> 13 & 15u;
									uint num21 = num9 >> 13 & 31u;
									uint num22 = num8 >> 16 & 7u;
									uint context2 = num20 << 9 | num21 << 4 | num22 << 1 | (num11 >> num12 & 1u);
									if ((!useSkip || skip.getPixel(j, i) == 0) && this.arithDecoder.decodeBit(context2, this.genericRegionStats) != 0)
									{
										RavenColorPtr expr_D3D = ravenColorPtr2;
										expr_D3D.ptr |= b;
										num8 |= 32768u;
										if (aty[0] == 0)
										{
											num11 |= 32768u;
										}
									}
									num10 <<= 1;
									num9 <<= 1;
									num8 <<= 1;
									num11 <<= 1;
									num19++;
									j++;
									b = (byte)(b >> 1);
								}
								k += 8;
								ravenColorPtr2 = ++ravenColorPtr2;
							}
							goto IL_15A6;
						}
						k = 0;
						j = 0;
						while (k < w)
						{
							if (k + 8 < w)
							{
								if (ravenColorPtr4 != null)
								{
									num10 |= (uint)ravenColorPtr4.ptr;
									ravenColorPtr4 = ++ravenColorPtr4;
								}
								if (ravenColorPtr3 != null)
								{
									num9 |= (uint)ravenColorPtr3.ptr;
									ravenColorPtr3 = ++ravenColorPtr3;
								}
								num8 |= (uint)ravenColorPtr.ptr;
								ravenColorPtr = ++ravenColorPtr;
							}
							int num19 = 0;
							byte b = 128;
							while (num19 < 8 && j < w)
							{
								uint num20 = num10 >> 13 & 15u;
								uint num21 = num9 >> 13 & 31u;
								uint num22 = num8 >> 16 & 7u;
								uint context2 = num20 << 9 | num21 << 4 | num22 << 1 | (uint)jBIG2Bitmap.getPixel(j + atx[0], i + aty[0]);
								if ((!useSkip || skip.getPixel(j, i) == 0) && this.arithDecoder.decodeBit(context2, this.genericRegionStats) != 0)
								{
									RavenColorPtr expr_E8B = ravenColorPtr2;
									expr_E8B.ptr |= b;
									num8 |= 32768u;
								}
								num10 <<= 1;
								num9 <<= 1;
								num8 <<= 1;
								num19++;
								j++;
								b = (byte)(b >> 1);
							}
							k += 8;
							ravenColorPtr2 = ++ravenColorPtr2;
						}
						goto IL_15A6;
					}
					case 2:
					{
						RavenColorPtr ravenColorPtr = new RavenColorPtr(jBIG2Bitmap.getDataPtr(), i * jBIG2Bitmap.getLineSize());
						RavenColorPtr ravenColorPtr2 = new RavenColorPtr(jBIG2Bitmap.getDataPtr(), i * jBIG2Bitmap.getLineSize());
						uint num8 = (uint)((uint)ravenColorPtr.ptr << 8);
						ravenColorPtr = ++ravenColorPtr;
						RavenColorPtr ravenColorPtr3;
						uint num9;
						RavenColorPtr ravenColorPtr4;
						uint num10;
						if (i >= 1)
						{
							ravenColorPtr3 = new RavenColorPtr(jBIG2Bitmap.getDataPtr(), (i - 1) * jBIG2Bitmap.getLineSize());
							num9 = (uint)((uint)ravenColorPtr3.ptr << 8);
							ravenColorPtr3 = ++ravenColorPtr3;
							if (i >= 2)
							{
								ravenColorPtr4 = new RavenColorPtr(jBIG2Bitmap.getDataPtr(), (i - 2) * jBIG2Bitmap.getLineSize());
								num10 = (uint)((uint)ravenColorPtr4.ptr << 8);
								ravenColorPtr4 = ++ravenColorPtr4;
							}
							else
							{
								ravenColorPtr4 = null;
								num10 = 0u;
							}
						}
						else
						{
							ravenColorPtr4 = (ravenColorPtr3 = null);
							num10 = (num9 = 0u);
						}
						int j;
						int k;
						if (atx[0] >= -8 && atx[0] <= 8)
						{
							RavenColorPtr ravenColorPtr5;
							uint num11;
							if (i + aty[0] >= 0)
							{
								ravenColorPtr5 = new RavenColorPtr(jBIG2Bitmap.getDataPtr(), (i + aty[0]) * jBIG2Bitmap.getLineSize());
								num11 = (uint)((uint)ravenColorPtr5.ptr << 8);
								ravenColorPtr5 = ++ravenColorPtr5;
							}
							else
							{
								ravenColorPtr5 = null;
								num11 = 0u;
							}
							int num12 = 15 - atx[0];
							k = 0;
							j = 0;
							while (k < w)
							{
								if (k + 8 < w)
								{
									if (ravenColorPtr4 != null)
									{
										num10 |= (uint)ravenColorPtr4.ptr;
										ravenColorPtr4 = ++ravenColorPtr4;
									}
									if (ravenColorPtr3 != null)
									{
										num9 |= (uint)ravenColorPtr3.ptr;
										ravenColorPtr3 = ++ravenColorPtr3;
									}
									num8 |= (uint)ravenColorPtr.ptr;
									ravenColorPtr = ++ravenColorPtr;
									if (ravenColorPtr5 != null)
									{
										num11 |= (uint)ravenColorPtr5.ptr;
										ravenColorPtr5 = ++ravenColorPtr5;
									}
								}
								int num19 = 0;
								byte b = 128;
								while (num19 < 8 && j < w)
								{
									uint num20 = num10 >> 14 & 7u;
									uint num21 = num9 >> 14 & 15u;
									uint num22 = num8 >> 16 & 3u;
									uint context2 = num20 << 7 | num21 << 3 | num22 << 1 | (num11 >> num12 & 1u);
									if ((!useSkip || skip.getPixel(j, i) == 0) && this.arithDecoder.decodeBit(context2, this.genericRegionStats) != 0)
									{
										RavenColorPtr expr_10E7 = ravenColorPtr2;
										expr_10E7.ptr |= b;
										num8 |= 32768u;
										if (aty[0] == 0)
										{
											num11 |= 32768u;
										}
									}
									num10 <<= 1;
									num9 <<= 1;
									num8 <<= 1;
									num11 <<= 1;
									num19++;
									j++;
									b = (byte)(b >> 1);
								}
								k += 8;
								ravenColorPtr2 = ++ravenColorPtr2;
							}
							goto IL_15A6;
						}
						k = 0;
						j = 0;
						while (k < w)
						{
							if (k + 8 < w)
							{
								if (ravenColorPtr4 != null)
								{
									num10 |= (uint)ravenColorPtr4.ptr;
									ravenColorPtr4 = ++ravenColorPtr4;
								}
								if (ravenColorPtr3 != null)
								{
									num9 |= (uint)ravenColorPtr3.ptr;
									ravenColorPtr3 = ++ravenColorPtr3;
								}
								num8 |= (uint)ravenColorPtr.ptr;
								ravenColorPtr = ++ravenColorPtr;
							}
							int num19 = 0;
							byte b = 128;
							while (num19 < 8 && j < w)
							{
								uint num20 = num10 >> 14 & 7u;
								uint num21 = num9 >> 14 & 15u;
								uint num22 = num8 >> 16 & 3u;
								uint context2 = num20 << 7 | num21 << 3 | num22 << 1 | (uint)jBIG2Bitmap.getPixel(j + atx[0], i + aty[0]);
								if ((!useSkip || skip.getPixel(j, i) == 0) && this.arithDecoder.decodeBit(context2, this.genericRegionStats) != 0)
								{
									RavenColorPtr expr_1233 = ravenColorPtr2;
									expr_1233.ptr |= b;
									num8 |= 32768u;
								}
								num10 <<= 1;
								num9 <<= 1;
								num8 <<= 1;
								num19++;
								j++;
								b = (byte)(b >> 1);
							}
							k += 8;
							ravenColorPtr2 = ++ravenColorPtr2;
						}
						goto IL_15A6;
					}
					case 3:
					{
						RavenColorPtr ravenColorPtr = new RavenColorPtr(jBIG2Bitmap.getDataPtr(), i * jBIG2Bitmap.getLineSize());
						RavenColorPtr ravenColorPtr2 = new RavenColorPtr(jBIG2Bitmap.getDataPtr(), i * jBIG2Bitmap.getLineSize());
						uint num8 = (uint)((uint)ravenColorPtr.ptr << 8);
						ravenColorPtr = ++ravenColorPtr;
						RavenColorPtr ravenColorPtr3;
						uint num9;
						if (i >= 1)
						{
							ravenColorPtr3 = new RavenColorPtr(jBIG2Bitmap.getDataPtr(), (i - 1) * jBIG2Bitmap.getLineSize());
							num9 = (uint)((uint)ravenColorPtr3.ptr << 8);
							ravenColorPtr3 = ++ravenColorPtr3;
						}
						else
						{
							ravenColorPtr3 = null;
							num9 = 0u;
						}
						int j;
						int k;
						if (atx[0] >= -8 && atx[0] <= 8)
						{
							RavenColorPtr ravenColorPtr5;
							uint num11;
							if (i + aty[0] >= 0)
							{
								ravenColorPtr5 = new RavenColorPtr(jBIG2Bitmap.getDataPtr(), (i + aty[0]) * jBIG2Bitmap.getLineSize());
								num11 = (uint)((uint)ravenColorPtr5.ptr << 8);
								ravenColorPtr5 = ++ravenColorPtr5;
							}
							else
							{
								ravenColorPtr5 = null;
								num11 = 0u;
							}
							int num12 = 15 - atx[0];
							k = 0;
							j = 0;
							while (k < w)
							{
								if (k + 8 < w)
								{
									if (ravenColorPtr3 != null)
									{
										num9 |= (uint)ravenColorPtr3.ptr;
										ravenColorPtr3 = ++ravenColorPtr3;
									}
									num8 |= (uint)ravenColorPtr.ptr;
									ravenColorPtr = ++ravenColorPtr;
									if (ravenColorPtr5 != null)
									{
										num11 |= (uint)ravenColorPtr5.ptr;
										ravenColorPtr5 = ++ravenColorPtr5;
									}
								}
								int num19 = 0;
								byte b = 128;
								while (num19 < 8 && j < w)
								{
									uint num21 = num9 >> 14 & 31u;
									uint num22 = num8 >> 16 & 15u;
									uint context2 = num21 << 5 | num22 << 1 | (num11 >> num12 & 1u);
									if ((!useSkip || skip.getPixel(j, i) == 0) && this.arithDecoder.decodeBit(context2, this.genericRegionStats) != 0)
									{
										RavenColorPtr expr_142A = ravenColorPtr2;
										expr_142A.ptr |= b;
										num8 |= 32768u;
										if (aty[0] == 0)
										{
											num11 |= 32768u;
										}
									}
									num9 <<= 1;
									num8 <<= 1;
									num11 <<= 1;
									num19++;
									j++;
									b = (byte)(b >> 1);
								}
								k += 8;
								ravenColorPtr2 = ++ravenColorPtr2;
							}
							goto IL_15A6;
						}
						k = 0;
						j = 0;
						while (k < w)
						{
							if (k + 8 < w)
							{
								if (ravenColorPtr3 != null)
								{
									num9 |= (uint)ravenColorPtr3.ptr;
									ravenColorPtr3 = ++ravenColorPtr3;
								}
								num8 |= (uint)ravenColorPtr.ptr;
								ravenColorPtr = ++ravenColorPtr;
							}
							int num19 = 0;
							byte b = 128;
							while (num19 < 8 && j < w)
							{
								uint num21 = num9 >> 14 & 31u;
								uint num22 = num8 >> 16 & 15u;
								uint context2 = num21 << 5 | num22 << 1 | (uint)jBIG2Bitmap.getPixel(j + atx[0], i + aty[0]);
								if ((!useSkip || skip.getPixel(j, i) == 0) && this.arithDecoder.decodeBit(context2, this.genericRegionStats) != 0)
								{
									RavenColorPtr expr_154A = ravenColorPtr2;
									expr_154A.ptr |= b;
									num8 |= 32768u;
								}
								num9 <<= 1;
								num8 <<= 1;
								num19++;
								j++;
								b = (byte)(b >> 1);
							}
							k += 8;
							ravenColorPtr2 = ++ravenColorPtr2;
						}
						goto IL_15A6;
					}
					default:
						goto IL_15A6;
					}
				}
			}
			return jBIG2Bitmap;
		}
		private void readGenericRefinementRegionSeg(uint segNum, bool imm, bool lossless, uint length, uint[] refSegs, uint nRefSegs)
		{
			uint num = 0u;
			uint num2 = 0u;
			uint x = 0u;
			uint num3 = 0u;
			uint num4 = 0u;
			uint num5 = 0u;
			int[] array = new int[2];
			int[] array2 = new int[2];
			if (this.readULong(ref num) && this.readULong(ref num2) && this.readULong(ref x) && this.readULong(ref num3) && this.readUByte(ref num4))
			{
				uint combOp = num4 & 7u;
				if (this.readUByte(ref num5))
				{
					uint num6 = num5 & 1u;
					uint num7 = num5 >> 1 & 1u;
					if (num6 == 0u || (this.readByte(ref array[0]) && this.readByte(ref array2[0]) && this.readByte(ref array[1]) && this.readByte(ref array2[1])))
					{
						if ((nRefSegs == 0u || imm) && this.pageH == 4294967295u && num3 + num2 > this.curPageH)
						{
							this.pageBitmap.expand((int)(num3 + num2), this.pageDefPixel);
						}
						if (nRefSegs > 1u)
						{
							AuxException.Throw("Bad reference in JBIG2 generic refinement segment.");
							return;
						}
						JBIG2Bitmap refBitmap;
						if (nRefSegs == 1u)
						{
							JBIG2Segment jBIG2Segment;
							if ((jBIG2Segment = this.findSegment(refSegs[0])) == null || jBIG2Segment.getType() != JBIG2SegmentType.jbig2SegBitmap)
							{
								AuxException.Throw("Bad bitmap reference in JBIG2 generic refinement segment.");
								return;
							}
							refBitmap = (JBIG2Bitmap)jBIG2Segment;
						}
						else
						{
							refBitmap = this.pageBitmap.getSlice(x, num3, num, num2);
						}
						this.resetRefinementStats(num6, null);
						this.arithDecoder.start();
						JBIG2Bitmap jBIG2Bitmap = this.readGenericRefinementRegion((int)num, (int)num2, (int)num6, num7 != 0u, refBitmap, 0, 0, array, array2);
						if (imm)
						{
							this.pageBitmap.combine(jBIG2Bitmap, (int)x, (int)num3, combOp);
						}
						else
						{
							jBIG2Bitmap.setSegNum(segNum);
							this.segments.Add(jBIG2Bitmap);
						}
						if (nRefSegs == 1u)
						{
							this.discardSegment(refSegs[0]);
						}
						return;
					}
				}
			}
			AuxException.Throw("Unexpected EOF in JBIG2 stream.");
		}
		private JBIG2Bitmap readGenericRefinementRegion(int w, int h, int templ, bool tpgrOn, JBIG2Bitmap refBitmap, int refDX, int refDY, int[] atx, int[] aty)
		{
			JBIG2BitmapPtr ptr = new JBIG2BitmapPtr();
			JBIG2BitmapPtr ptr2 = new JBIG2BitmapPtr();
			JBIG2BitmapPtr ptr3 = new JBIG2BitmapPtr();
			JBIG2BitmapPtr ptr4 = new JBIG2BitmapPtr();
			JBIG2BitmapPtr ptr5 = new JBIG2BitmapPtr();
			JBIG2BitmapPtr ptr6 = new JBIG2BitmapPtr();
			JBIG2BitmapPtr ptr7 = new JBIG2BitmapPtr();
			JBIG2BitmapPtr jBIG2BitmapPtr = new JBIG2BitmapPtr();
			JBIG2BitmapPtr jBIG2BitmapPtr2 = new JBIG2BitmapPtr();
			JBIG2BitmapPtr jBIG2BitmapPtr3 = new JBIG2BitmapPtr();
			JBIG2Bitmap jBIG2Bitmap = new JBIG2Bitmap(0u, w, h);
			jBIG2Bitmap.clearToZero();
			uint context;
			if (templ != 0)
			{
				context = 8u;
			}
			else
			{
				context = 16u;
			}
			bool flag = false;
			for (int i = 0; i < h; i++)
			{
				if (templ != 0)
				{
					jBIG2Bitmap.getPixelPtr(0, i - 1, ptr);
					uint num = (uint)jBIG2Bitmap.nextPixel(ptr);
					jBIG2Bitmap.getPixelPtr(-1, i, ptr2);
					refBitmap.getPixelPtr(-refDX, i - 1 - refDY, ptr3);
					refBitmap.getPixelPtr(-1 - refDX, i - refDY, ptr4);
					uint num2 = (uint)refBitmap.nextPixel(ptr4);
					num2 = (num2 << 1 | (uint)refBitmap.nextPixel(ptr4));
					refBitmap.getPixelPtr(-refDX, i + 1 - refDY, ptr5);
					uint num3 = (uint)refBitmap.nextPixel(ptr5);
					uint num6;
					uint num5;
					uint num4 = num5 = (num6 = 0u);
					if (tpgrOn)
					{
						refBitmap.getPixelPtr(-1 - refDX, i - 1 - refDY, jBIG2BitmapPtr);
						num5 = (uint)refBitmap.nextPixel(jBIG2BitmapPtr);
						num5 = (num5 << 1 | (uint)refBitmap.nextPixel(jBIG2BitmapPtr));
						num5 = (num5 << 1 | (uint)refBitmap.nextPixel(jBIG2BitmapPtr));
						refBitmap.getPixelPtr(-1 - refDX, i - refDY, jBIG2BitmapPtr2);
						num4 = (uint)refBitmap.nextPixel(jBIG2BitmapPtr2);
						num4 = (num4 << 1 | (uint)refBitmap.nextPixel(jBIG2BitmapPtr2));
						num4 = (num4 << 1 | (uint)refBitmap.nextPixel(jBIG2BitmapPtr2));
						refBitmap.getPixelPtr(-1 - refDX, i + 1 - refDY, jBIG2BitmapPtr3);
						num6 = (uint)refBitmap.nextPixel(jBIG2BitmapPtr3);
						num6 = (num6 << 1 | (uint)refBitmap.nextPixel(jBIG2BitmapPtr3));
						num6 = (num6 << 1 | (uint)refBitmap.nextPixel(jBIG2BitmapPtr3));
					}
					else
					{
						jBIG2BitmapPtr.p = (jBIG2BitmapPtr2.p = (jBIG2BitmapPtr3.p = null));
						jBIG2BitmapPtr.shift = (jBIG2BitmapPtr2.shift = (jBIG2BitmapPtr3.shift = 0));
						jBIG2BitmapPtr.x = (jBIG2BitmapPtr2.x = (jBIG2BitmapPtr3.x = 0));
					}
					int j = 0;
					while (j < w)
					{
						num = ((num << 1 | (uint)jBIG2Bitmap.nextPixel(ptr)) & 7u);
						num2 = ((num2 << 1 | (uint)refBitmap.nextPixel(ptr4)) & 7u);
						num3 = ((num3 << 1 | (uint)refBitmap.nextPixel(ptr5)) & 3u);
						if (!tpgrOn)
						{
							goto IL_2E2;
						}
						num5 = ((num5 << 1 | (uint)refBitmap.nextPixel(jBIG2BitmapPtr)) & 7u);
						num4 = ((num4 << 1 | (uint)refBitmap.nextPixel(jBIG2BitmapPtr2)) & 7u);
						num6 = ((num6 << 1 | (uint)refBitmap.nextPixel(jBIG2BitmapPtr3)) & 7u);
						if (this.arithDecoder.decodeBit(context, this.refinementRegionStats) != 0)
						{
							flag = !flag;
						}
						if (num5 == 0u && num4 == 0u && num6 == 0u)
						{
							jBIG2Bitmap.clearPixel(j, i);
						}
						else
						{
							if (num5 != 7u || num4 != 7u || num6 != 7u)
							{
								goto IL_2E2;
							}
							jBIG2Bitmap.setPixel(j, i);
						}
						IL_324:
						j++;
						continue;
						IL_2E2:
						uint context2 = num << 7 | (uint)((uint)jBIG2Bitmap.nextPixel(ptr2) << 6) | (uint)((uint)refBitmap.nextPixel(ptr3) << 5) | num2 << 2 | num3;
						if (this.arithDecoder.decodeBit(context2, this.refinementRegionStats) != 0)
						{
							jBIG2Bitmap.setPixel(j, i);
							goto IL_324;
						}
						goto IL_324;
					}
				}
				else
				{
					jBIG2Bitmap.getPixelPtr(0, i - 1, ptr);
					uint num = (uint)jBIG2Bitmap.nextPixel(ptr);
					jBIG2Bitmap.getPixelPtr(-1, i, ptr2);
					refBitmap.getPixelPtr(-refDX, i - 1 - refDY, ptr3);
					uint num7 = (uint)refBitmap.nextPixel(ptr3);
					refBitmap.getPixelPtr(-1 - refDX, i - refDY, ptr4);
					uint num2 = (uint)refBitmap.nextPixel(ptr4);
					num2 = (num2 << 1 | (uint)refBitmap.nextPixel(ptr4));
					refBitmap.getPixelPtr(-1 - refDX, i + 1 - refDY, ptr5);
					uint num3 = (uint)refBitmap.nextPixel(ptr5);
					num3 = (num3 << 1 | (uint)refBitmap.nextPixel(ptr5));
					jBIG2Bitmap.getPixelPtr(atx[0], i + aty[0], ptr6);
					refBitmap.getPixelPtr(atx[1] - refDX, i + aty[1] - refDY, ptr7);
					uint num6;
					uint num5;
					uint num4 = num5 = (num6 = 0u);
					if (tpgrOn)
					{
						refBitmap.getPixelPtr(-1 - refDX, i - 1 - refDY, jBIG2BitmapPtr);
						num5 = (uint)refBitmap.nextPixel(jBIG2BitmapPtr);
						num5 = (num5 << 1 | (uint)refBitmap.nextPixel(jBIG2BitmapPtr));
						num5 = (num5 << 1 | (uint)refBitmap.nextPixel(jBIG2BitmapPtr));
						refBitmap.getPixelPtr(-1 - refDX, i - refDY, jBIG2BitmapPtr2);
						num4 = (uint)refBitmap.nextPixel(jBIG2BitmapPtr2);
						num4 = (num4 << 1 | (uint)refBitmap.nextPixel(jBIG2BitmapPtr2));
						num4 = (num4 << 1 | (uint)refBitmap.nextPixel(jBIG2BitmapPtr2));
						refBitmap.getPixelPtr(-1 - refDX, i + 1 - refDY, jBIG2BitmapPtr3);
						num6 = (uint)refBitmap.nextPixel(jBIG2BitmapPtr3);
						num6 = (num6 << 1 | (uint)refBitmap.nextPixel(jBIG2BitmapPtr3));
						num6 = (num6 << 1 | (uint)refBitmap.nextPixel(jBIG2BitmapPtr3));
					}
					else
					{
						jBIG2BitmapPtr.p = (jBIG2BitmapPtr2.p = (jBIG2BitmapPtr3.p = null));
						jBIG2BitmapPtr.shift = (jBIG2BitmapPtr2.shift = (jBIG2BitmapPtr3.shift = 0));
						jBIG2BitmapPtr.x = (jBIG2BitmapPtr2.x = (jBIG2BitmapPtr3.x = 0));
					}
					int j = 0;
					while (j < w)
					{
						num = ((num << 1 | (uint)jBIG2Bitmap.nextPixel(ptr)) & 3u);
						num7 = ((num7 << 1 | (uint)refBitmap.nextPixel(ptr3)) & 3u);
						num2 = ((num2 << 1 | (uint)refBitmap.nextPixel(ptr4)) & 7u);
						num3 = ((num3 << 1 | (uint)refBitmap.nextPixel(ptr5)) & 7u);
						if (!tpgrOn)
						{
							goto IL_605;
						}
						num5 = ((num5 << 1 | (uint)refBitmap.nextPixel(jBIG2BitmapPtr)) & 7u);
						num4 = ((num4 << 1 | (uint)refBitmap.nextPixel(jBIG2BitmapPtr2)) & 7u);
						num6 = ((num6 << 1 | (uint)refBitmap.nextPixel(jBIG2BitmapPtr3)) & 7u);
						if (this.arithDecoder.decodeBit(context, this.refinementRegionStats) != 0)
						{
							flag = !flag;
						}
						if (num5 == 0u && num4 == 0u && num6 == 0u)
						{
							jBIG2Bitmap.clearPixel(j, i);
						}
						else
						{
							if (num5 != 7u || num4 != 7u || num6 != 7u)
							{
								goto IL_605;
							}
							jBIG2Bitmap.setPixel(j, i);
						}
						IL_659:
						j++;
						continue;
						IL_605:
						uint context2 = num << 11 | (uint)((uint)jBIG2Bitmap.nextPixel(ptr2) << 10) | num7 << 8 | num2 << 5 | num3 << 2 | (uint)((uint)jBIG2Bitmap.nextPixel(ptr6) << 1) | (uint)refBitmap.nextPixel(ptr7);
						if (this.arithDecoder.decodeBit(context2, this.refinementRegionStats) != 0)
						{
							jBIG2Bitmap.setPixel(j, i);
							goto IL_659;
						}
						goto IL_659;
					}
				}
			}
			return jBIG2Bitmap;
		}
		private void readPageInfoSeg(uint length)
		{
			uint num = 0u;
			uint num2 = 0u;
			uint num3 = 0u;
			uint num4 = 0u;
			if (!this.readULong(ref this.pageW) || !this.readULong(ref this.pageH) || !this.readULong(ref num) || !this.readULong(ref num2) || !this.readUByte(ref num3) || !this.readUWord(ref num4))
			{
				AuxException.Throw("Unexpected EOF in JBIG2 stream.");
				return;
			}
			this.pageDefPixel = (num3 >> 2 & 1u);
			this.defCombOp = (num3 >> 3 & 3u);
			if (this.pageH == 4294967295u)
			{
				this.curPageH = (num4 & 32767u);
			}
			else
			{
				this.curPageH = this.pageH;
			}
			this.pageBitmap = new JBIG2Bitmap(0u, (int)this.pageW, (int)this.curPageH);
			if (this.pageDefPixel != 0u)
			{
				this.pageBitmap.clearToOne();
				return;
			}
			this.pageBitmap.clearToZero();
		}
		private void readEndOfStripeSeg(uint length)
		{
			for (uint num = 0u; num < length; num += 1u)
			{
				if (this.curStr.getChar() == -1)
				{
					return;
				}
				this.byteCounter += 1u;
			}
		}
		private void readProfilesSeg(uint length)
		{
			for (uint num = 0u; num < length; num += 1u)
			{
				if (this.curStr.getChar() == -1)
				{
					return;
				}
				this.byteCounter += 1u;
			}
		}
		private void readCodeTableSeg(uint segNum, uint length)
		{
			uint num = 0u;
			int num2 = 0;
			int num3 = 0;
			if (this.readUByte(ref num) && this.readLong(ref num2) && this.readLong(ref num3))
			{
				uint num4 = num & 1u;
				uint n = (num >> 1 & 7u) + 1u;
				uint n2 = (num >> 4 & 7u) + 1u;
				this.huffDecoder.reset();
				uint num5 = 8u;
				JBIG2HuffmanTable[] array = new JBIG2HuffmanTable[num5];
				uint num6 = 0u;
				int i = num2;
				while (i < num3)
				{
					if (num6 == num5)
					{
						num5 *= 2u;
						JBIG2HuffmanTable[] array2 = new JBIG2HuffmanTable[num5];
						Array.Copy(array, array2, array.Length);
						array = array2;
					}
					array[(int)((UIntPtr)num6)].val = i;
					array[(int)((UIntPtr)num6)].prefixLen = this.huffDecoder.readBits(n);
					array[(int)((UIntPtr)num6)].rangeLen = this.huffDecoder.readBits(n2);
					i += 1 << (int)array[(int)((UIntPtr)num6)].rangeLen;
					num6 += 1u;
				}
				if (num6 + num4 + 3u > num5)
				{
					num5 = num6 + num4 + 3u;
					JBIG2HuffmanTable[] array3 = new JBIG2HuffmanTable[num5];
					Array.Copy(array, array3, array.Length);
					array = array3;
				}
				array[(int)((UIntPtr)num6)].val = num2 - 1;
				array[(int)((UIntPtr)num6)].prefixLen = this.huffDecoder.readBits(n);
				array[(int)((UIntPtr)num6)].rangeLen = 4294967293u;
				num6 += 1u;
				array[(int)((UIntPtr)num6)].val = num3;
				array[(int)((UIntPtr)num6)].prefixLen = this.huffDecoder.readBits(n);
				array[(int)((UIntPtr)num6)].rangeLen = 32u;
				num6 += 1u;
				if (num4 != 0u)
				{
					array[(int)((UIntPtr)num6)].val = 0;
					array[(int)((UIntPtr)num6)].prefixLen = this.huffDecoder.readBits(n);
					array[(int)((UIntPtr)num6)].rangeLen = 4294967294u;
					num6 += 1u;
				}
				array[(int)((UIntPtr)num6)].val = 0;
				array[(int)((UIntPtr)num6)].prefixLen = 0u;
				array[(int)((UIntPtr)num6)].rangeLen = 4294967295u;
				this.huffDecoder.buildTable(array, num6);
				this.segments.Add(new JBIG2CodeTable(segNum, array));
				return;
			}
			AuxException.Throw("Unexpected EOF in JBIG2 stream.");
		}
		private void readExtensionSeg(uint length)
		{
			for (uint num = 0u; num < length; num += 1u)
			{
				if (this.curStr.getChar() == -1)
				{
					return;
				}
				this.byteCounter += 1u;
			}
		}
		private JBIG2Segment findSegment(uint segNum)
		{
			for (int i = 0; i < this.globalSegments.Count; i++)
			{
				JBIG2Segment jBIG2Segment = this.globalSegments[i];
				if (jBIG2Segment.getSegNum() == segNum)
				{
					return jBIG2Segment;
				}
			}
			for (int i = 0; i < this.segments.Count; i++)
			{
				JBIG2Segment jBIG2Segment = this.segments[i];
				if (jBIG2Segment.getSegNum() == segNum)
				{
					return jBIG2Segment;
				}
			}
			return null;
		}
		private void discardSegment(uint segNum)
		{
			for (int i = 0; i < this.globalSegments.Count; i++)
			{
				JBIG2Segment jBIG2Segment = this.globalSegments[i];
				if (jBIG2Segment.getSegNum() == segNum)
				{
					this.globalSegments.RemoveAt(i);
					return;
				}
			}
			for (int i = 0; i < this.segments.Count; i++)
			{
				JBIG2Segment jBIG2Segment = this.segments[i];
				if (jBIG2Segment.getSegNum() == segNum)
				{
					this.segments.RemoveAt(i);
					return;
				}
			}
		}
		private void resetGenericStats(uint templ, JArithmeticDecoderStats prevStats)
		{
			int num = JBIG2Stream.contextSize[(int)((UIntPtr)templ)];
			if (prevStats != null && prevStats.getContextSize() == num)
			{
				if (this.genericRegionStats.getContextSize() == num)
				{
					this.genericRegionStats.copyFrom(prevStats);
					return;
				}
				this.genericRegionStats = prevStats.copy();
				return;
			}
			else
			{
				if (this.genericRegionStats.getContextSize() == num)
				{
					this.genericRegionStats.reset();
					return;
				}
				this.genericRegionStats = new JArithmeticDecoderStats(1 << num);
				return;
			}
		}
		private void resetRefinementStats(uint templ, JArithmeticDecoderStats prevStats)
		{
			int num = JBIG2Stream.refContextSize[(int)((UIntPtr)templ)];
			if (prevStats != null && prevStats.getContextSize() == num)
			{
				if (this.refinementRegionStats.getContextSize() == num)
				{
					this.refinementRegionStats.copyFrom(prevStats);
					return;
				}
				this.refinementRegionStats = prevStats.copy();
				return;
			}
			else
			{
				if (this.refinementRegionStats.getContextSize() == num)
				{
					this.refinementRegionStats.reset();
					return;
				}
				this.refinementRegionStats = new JArithmeticDecoderStats(1 << num);
				return;
			}
		}
		private void resetIntStats(int symCodeLen)
		{
			this.iadhStats.reset();
			this.iadwStats.reset();
			this.iaexStats.reset();
			this.iaaiStats.reset();
			this.iadtStats.reset();
			this.iaitStats.reset();
			this.iafsStats.reset();
			this.iadsStats.reset();
			this.iardxStats.reset();
			this.iardyStats.reset();
			this.iardwStats.reset();
			this.iardhStats.reset();
			this.iariStats.reset();
			if (this.iaidStats.getContextSize() == 1 << symCodeLen + 1)
			{
				this.iaidStats.reset();
				return;
			}
			this.iaidStats = new JArithmeticDecoderStats(1 << symCodeLen + 1);
		}
	}
}
