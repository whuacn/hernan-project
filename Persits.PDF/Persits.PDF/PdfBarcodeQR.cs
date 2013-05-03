using System;
using System.Text;
namespace Persits.PDF
{
	internal class PdfBarcodeQR
	{
		internal enum QRencodeMode
		{
			QR_MODE_NUL = -1,
			QR_MODE_NUM,
			QR_MODE_AN,
			QR_MODE_8,
			QR_MODE_KANJI,
			QR_MODE_STRUCTURE
		}
		internal enum QRecLevel
		{
			QR_ECLEVEL_L,
			QR_ECLEVEL_M,
			QR_ECLEVEL_Q,
			QR_ECLEVEL_H
		}
		internal class QRspec_Capacity
		{
			public int width;
			public int words;
			public int remainder;
			public int[] ec = new int[4];
			public QRspec_Capacity(int w, int wo, int r, int[] arr)
			{
				this.width = w;
				this.words = wo;
				this.remainder = r;
				for (int i = 0; i < 4; i++)
				{
					this.ec[i] = arr[i];
				}
			}
		}
		internal class QRcode
		{
			public int version;
			public int width;
			public byte[] data;
		}
		internal class QRcode_List
		{
			public PdfBarcodeQR.QRcode code;
			public PdfBarcodeQR.QRcode_List next;
		}
		internal class BitStream
		{
			public int length;
			public byte[] data;
		}
		internal class QRinput_List
		{
			public PdfBarcodeQR.QRencodeMode mode;
			public int size;
			public byte[] data;
			public PdfBarcodeQR.BitStream bstream;
			public PdfBarcodeQR.QRinput_List next;
		}
		internal class QRinput
		{
			public int version;
			public PdfBarcodeQR.QRecLevel level;
			public PdfBarcodeQR.QRinput_List head;
			public PdfBarcodeQR.QRinput_List tail;
		}
		internal class FrameFiller
		{
			public int width;
			public byte[] frame;
			public int x;
			public int y;
			public int dir;
			public int bit;
		}
		internal class RSblock
		{
			public int dataLength;
			public byte[] data;
			public int eccLength;
			public byte[] ecc;
		}
		internal class QRinput_InputList
		{
			public PdfBarcodeQR.QRinput input;
			public PdfBarcodeQR.QRinput_InputList next;
		}
		internal class QRinput_Struct
		{
			public int size;
			public int parity;
			public PdfBarcodeQR.QRinput_InputList head;
			public PdfBarcodeQR.QRinput_InputList tail;
		}
		internal class QRRawCode
		{
			public int version;
			public byte[] datacode;
			public byte[] ecccode;
			public int blocks;
			public PdfBarcodeQR.RSblock[] rsblock;
			public int count;
			public int dataLength;
			public int eccLength;
			public int b1;
		}
		internal class QR_RS
		{
			public int mm;
			public int nn;
			public byte[] alpha_to;
			public byte[] index_of;
			public byte[] genpoly;
			public int nroots;
			public int fcr;
			public int prim;
			public int iprim;
			public int pad;
			public int gfpoly;
			public PdfBarcodeQR.QR_RS next;
		}
		private delegate int MyDelegate(int x, int y);
		private const int N1 = 3;
		private const int N2 = 3;
		private const int N3 = 40;
		private const int N4 = 10;
		private const int QRSPEC_VERSION_MAX = 40;
		private const int QRSPEC_WIDTH_MAX = 177;
		private const int STRUCTURE_HEADER_BITS = 20;
		private const int MAX_STRUCTURED_SYMBOLS = 16;
		private static PdfBarcodeQR.QRspec_Capacity[] qrspecCapacity;
		private static int[][][] eccTable;
		private static uint[] versionPattern;
		private static int[][] alignmentPattern;
		private static int[][] lengthTableBits;
		private static sbyte[] QRinput_anTable;
		private static PdfBarcodeQR.MyDelegate[] Masks;
		private static uint[][] formatInfo;
		private int[] runLength = new int[178];
		public PdfManager m_pManager;
		public PdfDocument m_pDoc;
		public byte[] m_pData;
		public int m_nDataLen;
		public float m_fBarWidth;
		public int m_nWidth;
		public int m_nHeight;
		public int m_nCaseSensitive;
		public int m_nEightBit;
		public int m_nVersion;
		public int m_nStructured;
		public PdfBarcodeQR.QRecLevel m_nLevel;
		public PdfBarcodeQR.QRencodeMode m_nHint;
		private PdfBarcodeQR.QR_RS rslist;
		private byte[] m_pMatrix;
		private byte[][] frames = new byte[41][];
		public PdfBarcodeQR()
		{
			this.m_nCaseSensitive = 1;
			this.m_nEightBit = 0;
			this.m_nVersion = 0;
			this.m_nStructured = 0;
			this.m_nLevel = PdfBarcodeQR.QRecLevel.QR_ECLEVEL_L;
			this.m_nHint = PdfBarcodeQR.QRencodeMode.QR_MODE_8;
			this.rslist = null;
			this.m_pMatrix = null;
			for (int i = 0; i < 41; i++)
			{
				this.frames[i] = null;
			}
		}
		~PdfBarcodeQR()
		{
			this.free_rs_cache();
		}
		public void Draw(PdfCanvas pCanvas, PdfParam pParam)
		{
			if (pParam.IsSet("ErrorLevel"))
			{
				this.m_nLevel = (PdfBarcodeQR.QRecLevel)pParam.Long("ErrorLevel");
				if (this.m_nLevel < PdfBarcodeQR.QRecLevel.QR_ECLEVEL_L || this.m_nLevel > PdfBarcodeQR.QRecLevel.QR_ECLEVEL_H)
				{
					AuxException.Throw("The ErrorLevel parameter must be between 0 and 3.", PdfErrors._ERROR_INVALIDARG);
				}
			}
			if (pParam.IsSet("Hint"))
			{
				this.m_nHint = (PdfBarcodeQR.QRencodeMode)pParam.Long("Hint");
				if (this.m_nHint < PdfBarcodeQR.QRencodeMode.QR_MODE_NUM || this.m_nHint > PdfBarcodeQR.QRencodeMode.QR_MODE_KANJI)
				{
					AuxException.Throw("Valid values for the Hint parameter are 0 (numeric), 1 (alpha-numeric), 2 (binary) and 3 (Kanji).", PdfErrors._ERROR_INVALIDARG);
				}
			}
			if (pParam.IsSet("Version"))
			{
				this.m_nVersion = pParam.Long("Version");
				if (this.m_nVersion < 0 || this.m_nVersion > 40)
				{
					AuxException.Throw("Valid values for the Version parameter are 0 to 40.", PdfErrors._ERROR_INVALIDARG);
				}
			}
			if (this.m_nStructured == 0)
			{
				this.qrencode(this.m_pData);
			}
			this.m_fBarWidth = 1f;
			if (pParam.IsSet("BarWidth"))
			{
				this.m_fBarWidth = pParam.Number("BarWidth");
			}
			if (this.m_fBarWidth <= 0f)
			{
				AuxException.Throw("Invalid BarWidth parameter.", PdfErrors._ERROR_INVALIDARG);
			}
			if (!pParam.IsSet("X") || !pParam.IsSet("Y"))
			{
				AuxException.Throw("X and Y parameters are required.", PdfErrors._ERROR_INVALIDARG);
			}
			float num = pParam.Number("X");
			float num2 = pParam.Number("Y");
			if (pParam.IsSet("Angle"))
			{
				float num3 = pParam.Number("Angle");
				float num4 = 0.01745329f;
				pCanvas.SetCTM((float)Math.Cos((double)(num3 * num4)), (float)Math.Sin((double)(num3 * num4)), -(float)Math.Sin((double)(num3 * num4)), (float)Math.Cos((double)(num3 * num4)), num, num2);
				num2 = (num = 0f);
			}
			AuxRGB auxRGB = new AuxRGB();
			if (pParam.IsSet("Color"))
			{
				pParam.Color("Color", ref auxRGB);
				pCanvas.SetFillColor(auxRGB.r, auxRGB.g, auxRGB.b);
			}
			float num5 = this.m_fBarWidth * 2f;
			AuxRGB auxRGB2 = new AuxRGB();
			if (pParam.IsSet("BgColor"))
			{
				pParam.Color("BgColor", ref auxRGB2);
				pParam.GetNumberIfSet("BgMargin", 0, ref num5);
				pCanvas.SetFillColor(auxRGB2.r, auxRGB2.g, auxRGB2.b);
				pCanvas.FillRect(num - num5, num2 - num5, this.m_fBarWidth * (float)this.m_nWidth + 2f * num5, this.m_fBarWidth * (float)this.m_nHeight + 2f * num5);
				pCanvas.SetFillColor(auxRGB.r, auxRGB.g, auxRGB.b);
			}
			float num6 = num;
			float num7 = num2 + this.m_fBarWidth * (float)(this.m_nHeight - 1);
			for (int i = 0; i < this.m_nWidth * this.m_nHeight; i++)
			{
				if (this.m_pMatrix[i] != 0)
				{
					pCanvas.FillRect(num6, num7, this.m_fBarWidth, this.m_fBarWidth);
				}
				num6 += this.m_fBarWidth;
				if ((i + 1) % this.m_nWidth == 0)
				{
					num6 = num;
					num7 -= this.m_fBarWidth;
				}
			}
		}
		private PdfBarcodeQR.QRcode encode(byte[] intext)
		{
			PdfBarcodeQR.QRcode result;
			if (this.m_nEightBit != 0)
			{
				result = this.QRcode_encodeString8bit(intext, this.m_nVersion, this.m_nLevel);
			}
			else
			{
				result = this.QRcode_encodeString(intext, this.m_nVersion, this.m_nLevel, this.m_nHint, this.m_nCaseSensitive);
			}
			return result;
		}
		private void qrencode(byte[] intext)
		{
			PdfBarcodeQR.QRcode qRcode = this.encode(intext);
			if (qRcode == null)
			{
				AuxException.Throw("Failed to encode the input data.", PdfErrors._ERROR_INVALIDARG);
			}
			this.m_nWidth = (this.m_nHeight = qRcode.width);
			this.m_pMatrix = new byte[qRcode.width * qRcode.width];
			for (int i = 0; i < qRcode.width * qRcode.width; i++)
			{
                this.m_pMatrix[i] = ((int)qRcode.data[i] & 1) != 0 ? (byte)1 : (byte)0;
			}
			this.QRcode_free(qRcode);
		}
		private void QRcode_List_free(PdfBarcodeQR.QRcode_List qrlist)
		{
			PdfBarcodeQR.QRcode_List next;
			for (PdfBarcodeQR.QRcode_List qRcode_List = qrlist; qRcode_List != null; qRcode_List = next)
			{
				next = qRcode_List.next;
				this.QRcode_List_freeEntry(qRcode_List);
			}
		}
		private void QRcode_List_freeEntry(PdfBarcodeQR.QRcode_List entry)
		{
			if (entry != null)
			{
				this.QRcode_free(entry.code);
			}
		}
		private void QRcode_free(PdfBarcodeQR.QRcode qrcode)
		{
		}
		private PdfBarcodeQR.QRcode QRcode_encodeString8bit(byte[] str, int version, PdfBarcodeQR.QRecLevel level)
		{
			if (str == null)
			{
				return null;
			}
			PdfBarcodeQR.QRinput qRinput = this.QRinput_new2(version, level);
			if (qRinput == null)
			{
				return null;
			}
			int num = this.QRinput_append(qRinput, PdfBarcodeQR.QRencodeMode.QR_MODE_8, str.Length, str);
			if (num < 0)
			{
				this.QRinput_free(qRinput);
				return null;
			}
			PdfBarcodeQR.QRcode result = this.QRcode_encodeInput(qRinput);
			this.QRinput_free(qRinput);
			return result;
		}
		private PdfBarcodeQR.QRcode QRcode_encodeString(byte[] str, int version, PdfBarcodeQR.QRecLevel level, PdfBarcodeQR.QRencodeMode hint, int casesensitive)
		{
			if (hint != PdfBarcodeQR.QRencodeMode.QR_MODE_8 && hint != PdfBarcodeQR.QRencodeMode.QR_MODE_KANJI)
			{
				return null;
			}
			PdfBarcodeQR.QRinput qRinput = this.QRinput_new2(version, level);
			if (qRinput == null)
			{
				return null;
			}
			int num = this.Split_splitStringToQRinput(str, qRinput, hint, casesensitive);
			if (num < 0)
			{
				this.QRinput_free(qRinput);
				return null;
			}
			PdfBarcodeQR.QRcode result = this.QRcode_encodeInput(qRinput);
			this.QRinput_free(qRinput);
			return result;
		}
		private PdfBarcodeQR.QRinput QRinput_new2(int version, PdfBarcodeQR.QRecLevel level)
		{
			if (version < 0 || version > 40 || level > PdfBarcodeQR.QRecLevel.QR_ECLEVEL_H)
			{
				return null;
			}
			return new PdfBarcodeQR.QRinput
			{
				head = null,
				tail = null,
				version = version,
				level = level
			};
		}
		private int QRinput_append(PdfBarcodeQR.QRinput input, PdfBarcodeQR.QRencodeMode mode, int size, byte[] data)
		{
			PdfBarcodeQR.QRinput_List qRinput_List = this.QRinput_List_newEntry(mode, size, data);
			if (qRinput_List == null)
			{
				return -1;
			}
			this.QRinput_appendEntry(input, qRinput_List);
			return 0;
		}
		private void QRinput_free(PdfBarcodeQR.QRinput input)
		{
			if (input != null)
			{
				PdfBarcodeQR.QRinput_List next;
				for (PdfBarcodeQR.QRinput_List qRinput_List = input.head; qRinput_List != null; qRinput_List = next)
				{
					next = qRinput_List.next;
					this.QRinput_List_freeEntry(qRinput_List);
				}
			}
		}
		private PdfBarcodeQR.QRcode QRcode_encodeMask(PdfBarcodeQR.QRinput input, int mask)
		{
			if (input.version < 0 || input.version > 40)
			{
				return null;
			}
			if (input.level > PdfBarcodeQR.QRecLevel.QR_ECLEVEL_H)
			{
				return null;
			}
			PdfBarcodeQR.QRRawCode qRRawCode = this.QRraw_new(input);
			if (qRRawCode == null)
			{
				return null;
			}
			int version = qRRawCode.version;
			int width = this.QRspec_getWidth(version);
			byte[] array = this.QRspec_newFrame(version);
			if (array == null)
			{
				this.QRraw_free(qRRawCode);
				return null;
			}
			PdfBarcodeQR.FrameFiller frameFiller = this.FrameFiller_new(width, array);
			if (frameFiller == null)
			{
				this.QRraw_free(qRRawCode);
				return null;
			}
			int j;
			for (int i = 0; i < qRRawCode.dataLength + qRRawCode.eccLength; i++)
			{
				byte b = this.QRraw_getCode(qRRawCode);
				byte b2 = 128;
				for (j = 0; j < 8; j++)
				{
					int num = this.FrameFiller_next(frameFiller);
                    frameFiller.frame[num] = ((int)b2 & (int)b) != 0 ? (byte)3 : (byte)2;
					b2 = (byte)(b2 >> 1);
				}
			}
			this.QRraw_free(qRRawCode);
			j = this.QRspec_getRemainder(version);
			for (int i = 0; i < j; i++)
			{
				int num = this.FrameFiller_next(frameFiller);
				frameFiller.frame[num] = 2;
			}
			byte[] array2;
			if (mask < 0)
			{
				array2 = this.Mask_mask(width, array, input.level);
			}
			else
			{
				array2 = this.Mask_makeMask(width, array, mask, input.level);
			}
			if (array2 == null)
			{
				return null;
			}
			return this.QRcode_new(version, width, array2);
		}
		private int FrameFiller_next(PdfBarcodeQR.FrameFiller filler)
		{
			if (filler.bit == -1)
			{
				filler.bit = 0;
				return filler.y * filler.width + filler.x;
			}
			int num = filler.x;
			int num2 = filler.y;
			int width = filler.width;
			if (filler.bit == 0)
			{
				num--;
				filler.bit++;
			}
			else
			{
				num++;
				num2 += filler.dir;
				filler.bit--;
			}
			if (filler.dir < 0)
			{
				if (num2 < 0)
				{
					num2 = 0;
					num -= 2;
					filler.dir = 1;
					if (num == 6)
					{
						num--;
						num2 = 9;
					}
				}
			}
			else
			{
				if (num2 == width)
				{
					num2 = width - 1;
					num -= 2;
					filler.dir = -1;
					if (num == 6)
					{
						num--;
						num2 -= 8;
					}
				}
			}
			filler.x = num;
			filler.y = num2;
			if ((filler.frame[num2 * width + num] & 128) != 0)
			{
				return this.FrameFiller_next(filler);
			}
			return num2 * width + num;
		}
		private PdfBarcodeQR.QRcode QRcode_encodeInput(PdfBarcodeQR.QRinput input)
		{
			return this.QRcode_encodeMask(input, -1);
		}
		private PdfBarcodeQR.QRcode_List QRcode_encodeInputToStructured(PdfBarcodeQR.QRinput input)
		{
			PdfBarcodeQR.QRinput_Struct qRinput_Struct = this.QRinput_splitQRinputToStruct(input);
			if (qRinput_Struct == null)
			{
				return null;
			}
			PdfBarcodeQR.QRcode_List result = this.QRcode_encodeInputStructured(qRinput_Struct);
			this.QRinput_Struct_free(qRinput_Struct);
			return result;
		}
		private PdfBarcodeQR.QRinput_List QRinput_List_newEntry(PdfBarcodeQR.QRencodeMode mode, int size, byte[] data)
		{
			if (this.QRinput_check(mode, size, data) != 0)
			{
				return null;
			}
			PdfBarcodeQR.QRinput_List qRinput_List = new PdfBarcodeQR.QRinput_List();
			qRinput_List.mode = mode;
			qRinput_List.size = size;
			qRinput_List.data = new byte[size];
			Array.Copy(data, qRinput_List.data, size);
			qRinput_List.bstream = null;
			qRinput_List.next = null;
			return qRinput_List;
		}
		private void QRinput_appendEntry(PdfBarcodeQR.QRinput input, PdfBarcodeQR.QRinput_List entry)
		{
			if (input.tail == null)
			{
				input.head = entry;
				input.tail = entry;
			}
			else
			{
				input.tail.next = entry;
				input.tail = entry;
			}
			entry.next = null;
		}
		private void QRinput_InputList_freeEntry(PdfBarcodeQR.QRinput_InputList entry)
		{
			if (entry != null)
			{
				this.QRinput_free(entry.input);
				entry = null;
			}
		}
		private void QRinput_List_freeEntry(PdfBarcodeQR.QRinput_List entry)
		{
			if (entry != null)
			{
				entry.data = null;
				this.BitStream_free(entry.bstream);
				entry = null;
			}
		}
		private int Split_splitStringToQRinput(byte[] str, PdfBarcodeQR.QRinput input, PdfBarcodeQR.QRencodeMode hint, int casesensitive)
		{
			if (str == null || str[0] == 0)
			{
				return -1;
			}
			int result;
			if (casesensitive == 0)
			{
				byte[] array = this.dupAndToUpper(str, hint);
				if (array == null)
				{
					return -1;
				}
				result = this.Split_splitString(array, input, hint);
			}
			else
			{
				result = this.Split_splitString(str, input, hint);
			}
			return result;
		}
		private PdfBarcodeQR.QRRawCode QRraw_new(PdfBarcodeQR.QRinput input)
		{
			int[] array = new int[5];
			PdfBarcodeQR.QRRawCode qRRawCode = new PdfBarcodeQR.QRRawCode();
			qRRawCode.datacode = this.QRinput_getByteStream(input);
			if (qRRawCode.datacode == null)
			{
				return null;
			}
			this.QRspec_getEccSpec(input.version, input.level, array);
			qRRawCode.version = input.version;
			qRRawCode.b1 = array[0];
			qRRawCode.dataLength = array[0] * array[1] + array[3] * array[4];
			qRRawCode.eccLength = (array[0] + array[3]) * array[2];
			qRRawCode.ecccode = new byte[qRRawCode.eccLength];
			qRRawCode.blocks = array[0] + array[3];
			qRRawCode.rsblock = new PdfBarcodeQR.RSblock[qRRawCode.blocks];
			for (int i = 0; i < qRRawCode.blocks; i++)
			{
				qRRawCode.rsblock[i] = new PdfBarcodeQR.RSblock();
			}
			int num = this.RSblock_init(qRRawCode.rsblock, array, qRRawCode.datacode, qRRawCode.ecccode);
			if (num < 0)
			{
				this.QRraw_free(qRRawCode);
				return null;
			}
			qRRawCode.count = 0;
			return qRRawCode;
		}
		private int QRspec_getWidth(int version)
		{
			return PdfBarcodeQR.qrspecCapacity[version].width;
		}
		private byte[] QRspec_newFrame(int version)
		{
			if (version < 1 || version > 40)
			{
				return null;
			}
			if (this.frames[version] == null)
			{
				this.frames[version] = this.QRspec_createFrame(version);
			}
			if (this.frames[version] == null)
			{
				return null;
			}
			int width = PdfBarcodeQR.qrspecCapacity[version].width;
			byte[] array = new byte[width * width];
			Array.Copy(this.frames[version], array, width * width);
			return array;
		}
		private void QRraw_free(PdfBarcodeQR.QRRawCode raw)
		{
			if (raw != null)
			{
				raw.datacode = null;
				raw.ecccode = null;
				if (raw.rsblock != null)
				{
					raw.rsblock = null;
				}
				raw = null;
			}
		}
		private PdfBarcodeQR.FrameFiller FrameFiller_new(int width, byte[] frame)
		{
			return new PdfBarcodeQR.FrameFiller
			{
				width = width,
				frame = frame,
				x = width - 1,
				y = width - 1,
				dir = -1,
				bit = -1
			};
		}
		private byte QRraw_getCode(PdfBarcodeQR.QRRawCode raw)
		{
			byte result;
			if (raw.count < raw.dataLength)
			{
				int num = raw.count % raw.blocks;
				int num2 = raw.count / raw.blocks;
				if (num2 >= raw.rsblock[0].dataLength)
				{
					num += raw.b1;
				}
				result = raw.rsblock[num].data[num2];
			}
			else
			{
				if (raw.count >= raw.dataLength + raw.eccLength)
				{
					return 0;
				}
				int num = (raw.count - raw.dataLength) % raw.blocks;
				int num2 = (raw.count - raw.dataLength) / raw.blocks;
				result = raw.rsblock[num].ecc[num2];
			}
			raw.count++;
			return result;
		}
		private int QRspec_getRemainder(int version)
		{
			return PdfBarcodeQR.qrspecCapacity[version].remainder;
		}
		private byte[] QRspec_createFrame(int version)
		{
			int width = PdfBarcodeQR.qrspecCapacity[version].width;
			byte[] array = new byte[width * width];
			this.putFinderPattern(ref array, width, 0, 0);
			this.putFinderPattern(ref array, width, width - 7, 0);
			this.putFinderPattern(ref array, width, 0, width - 7);
			int num = 0;
			int num2 = width * (width - 7);
			for (int i = 0; i < 7; i++)
			{
				array[num + 7] = 192;
				array[num + width - 8] = 192;
				array[num2 + 7] = 192;
				num += width;
				num2 += width;
			}
			for (int j = 0; j < 8; j++)
			{
				array[width * 7 + j] = 192;
			}
			for (int j = 0; j < 8; j++)
			{
				array[width * 8 - 8 + j] = 192;
			}
			for (int j = 0; j < 8; j++)
			{
				array[width * (width - 8) + j] = 192;
			}
			for (int j = 0; j < 9; j++)
			{
				array[width * 8 + j] = 132;
			}
			for (int j = 0; j < 8; j++)
			{
				array[width * 9 - 8 + j] = 132;
			}
			num = 8;
			for (int i = 0; i < 8; i++)
			{
				array[num] = 132;
				num += width;
			}
			num = width * (width - 7) + 8;
			for (int i = 0; i < 7; i++)
			{
				array[num] = 132;
				num += width;
			}
			num = width * 6 + 8;
			num2 = width * 8 + 6;
			for (int k = 1; k < width - 15; k++)
			{
				array[num] = (byte)(144 | (k & 1));
				array[num2] = (byte)(144 | (k & 1));
				num++;
				num2 += width;
			}
			this.QRspec_putAlignmentPattern(version, array, width);
			if (version >= 7)
			{
				uint num3 = this.QRspec_getVersionPattern(version);
				num = width * (width - 11);
				uint num4 = num3;
				for (int k = 0; k < 6; k++)
				{
					for (int i = 0; i < 3; i++)
					{
						array[num + width * i + k] = (byte)(136u | (num4 & 1u));
						num4 >>= 1;
					}
				}
				num = width - 11;
				num4 = num3;
				for (int i = 0; i < 6; i++)
				{
					for (int k = 0; k < 3; k++)
					{
						array[num + k] = (byte)(136u | (num4 & 1u));
						num4 >>= 1;
					}
					num += width;
				}
			}
			array[width * (width - 8) + 8] = 129;
			return array;
		}
		private void putFinderPattern(ref byte[] frame, int width, int ox, int oy)
		{
			byte[] array = new byte[]
			{
				193,
				193,
				193,
				193,
				193,
				193,
				193,
				193,
				192,
				192,
				192,
				192,
				192,
				193,
				193,
				192,
				193,
				193,
				193,
				192,
				193,
				193,
				192,
				193,
				193,
				193,
				192,
				193,
				193,
				192,
				193,
				193,
				193,
				192,
				193,
				193,
				192,
				192,
				192,
				192,
				192,
				193,
				193,
				193,
				193,
				193,
				193,
				193,
				193
			};
			int num = 0;
			num += oy * width + ox;
			int num2 = 0;
			for (int i = 0; i < 7; i++)
			{
				for (int j = 0; j < 7; j++)
				{
					frame[j + num] = array[num2 + j];
				}
				num += width;
				num2 += 7;
			}
		}
		private uint QRspec_getVersionPattern(int version)
		{
			if (version < 7 || version > 40)
			{
				return 0u;
			}
			return PdfBarcodeQR.versionPattern[version - 7];
		}
		private void QRspec_putAlignmentPattern(int version, byte[] frame, int width)
		{
			if (version < 2)
			{
				return;
			}
			int num = PdfBarcodeQR.alignmentPattern[version][1] - PdfBarcodeQR.alignmentPattern[version][0];
			int num2;
			if (num < 0)
			{
				num2 = 2;
			}
			else
			{
				num2 = (width - PdfBarcodeQR.alignmentPattern[version][0]) / num + 2;
			}
			if (num2 * num2 - 3 == 1)
			{
				int i = PdfBarcodeQR.alignmentPattern[version][0];
				int j = PdfBarcodeQR.alignmentPattern[version][0];
				this.QRspec_putAlignmentMarker(ref frame, width, i, j);
				return;
			}
			int num3 = PdfBarcodeQR.alignmentPattern[version][0];
			for (int i = 1; i < num2 - 1; i++)
			{
				this.QRspec_putAlignmentMarker(ref frame, width, 6, num3);
				this.QRspec_putAlignmentMarker(ref frame, width, num3, 6);
				num3 += num;
			}
			int num4 = PdfBarcodeQR.alignmentPattern[version][0];
			for (int j = 0; j < num2 - 1; j++)
			{
				num3 = PdfBarcodeQR.alignmentPattern[version][0];
				for (int i = 0; i < num2 - 1; i++)
				{
					this.QRspec_putAlignmentMarker(ref frame, width, num3, num4);
					num3 += num;
				}
				num4 += num;
			}
		}
		private void QRspec_putAlignmentMarker(ref byte[] frame, int width, int ox, int oy)
		{
			byte[] array = new byte[]
			{
				161,
				161,
				161,
				161,
				161,
				161,
				160,
				160,
				160,
				161,
				161,
				160,
				161,
				160,
				161,
				161,
				160,
				160,
				160,
				161,
				161,
				161,
				161,
				161,
				161
			};
			int num = 0;
			int num2 = 0;
			num += (oy - 2) * width + ox - 2;
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					frame[num + j] = array[num2 + j];
				}
				num += width;
				num2 += 5;
			}
		}
		private int RSblock_init(PdfBarcodeQR.RSblock[] blocks, int[] spec, byte[] data, byte[] ecc)
		{
			int num = spec[1];
			int num2 = spec[2];
			PdfBarcodeQR.QR_RS qR_RS = this.init_rs(8, 285, 0, 1, num2, 255 - num - num2);
			if (qR_RS == null)
			{
				return -1;
			}
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			for (int i = 0; i < spec[0]; i++)
			{
				this.RSblock_initBlock(ref blocks[num3], num, num4, data, num2, num5, ecc, qR_RS);
				num4 += num;
				num5 += num2;
				num3++;
			}
			if (spec[3] == 0)
			{
				return 0;
			}
			num = spec[4];
			num2 = spec[2];
			qR_RS = this.init_rs(8, 285, 0, 1, num2, 255 - num - num2);
			if (qR_RS == null)
			{
				return -1;
			}
			for (int i = 0; i < spec[3]; i++)
			{
				this.RSblock_initBlock(ref blocks[num3], num, num4, data, num2, num5, ecc, qR_RS);
				num4 += num;
				num5 += num2;
				num3++;
			}
			return 0;
		}
		private void RSblock_initBlock(ref PdfBarcodeQR.RSblock block, int dl, int doffset, byte[] data, int el, int eoffset, byte[] ecc, PdfBarcodeQR.QR_RS rs)
		{
			block.dataLength = dl;
			block.data = new byte[dl];
			Array.Copy(data, doffset, block.data, 0, dl);
			block.eccLength = el;
			block.ecc = new byte[el];
			Array.Copy(ecc, eoffset, block.ecc, 0, el);
			byte[] sourceArray = new byte[el];
			this.encode_rs_char(rs, block.data, ref sourceArray);
			Array.Copy(sourceArray, 0, block.ecc, 0, el);
		}
		private int modnn(PdfBarcodeQR.QR_RS rs, int x)
		{
			while (x >= rs.nn)
			{
				x -= rs.nn;
				x = (x >> rs.mm) + (x & rs.nn);
			}
			return x;
		}
		private PdfBarcodeQR.QR_RS init_rs_char(int symsize, int gfpoly, int fcr, int prim, int nroots, int pad)
		{
            PdfBarcodeQR.QR_RS rs = (PdfBarcodeQR.QR_RS)null;
            if (symsize >= 0 && symsize <= 8 && (fcr >= 0 && fcr < 1 << symsize) && (prim > 0 && prim < 1 << symsize && (nroots >= 0 && nroots < 1 << symsize)) && (pad >= 0 && pad < (1 << symsize) - 1 - nroots))
            {
                rs = new PdfBarcodeQR.QR_RS();
                rs.mm = symsize;
                rs.nn = (1 << symsize) - 1;
                rs.pad = pad;
                rs.alpha_to = new byte[rs.nn + 1];
                rs.index_of = new byte[rs.nn + 1];
                rs.index_of[0] = (byte)rs.nn;
                rs.alpha_to[rs.nn] = (byte)0;
                int index1 = 1;
                for (int index2 = 0; index2 < rs.nn; ++index2)
                {
                    rs.index_of[index1] = (byte)index2;
                    rs.alpha_to[index2] = (byte)index1;
                    int num = index1 << 1;
                    if ((num & 1 << symsize) != 0)
                        num ^= gfpoly;
                    index1 = num & rs.nn;
                }
                if (index1 != 1)
                {
                    rs = (PdfBarcodeQR.QR_RS)null;
                }
                else
                {
                    rs.genpoly = new byte[nroots + 1];
                    rs.fcr = fcr;
                    rs.prim = prim;
                    rs.nroots = nroots;
                    rs.gfpoly = gfpoly;
                    int num1 = 1;
                    while (num1 % prim != 0)
                        num1 += rs.nn;
                    rs.iprim = num1 / prim;
                    rs.genpoly[0] = (byte)1;
                    int num2 = 0;
                    int num3 = fcr * prim;
                    while (num2 < nroots)
                    {
                        rs.genpoly[num2 + 1] = (byte)1;
                        for (int index2 = num2; index2 > 0; --index2)
                            rs.genpoly[index2] = (int)rs.genpoly[index2] == 0 ? rs.genpoly[index2 - 1] : (byte)((uint)rs.genpoly[index2 - 1] ^ (uint)rs.alpha_to[this.modnn(rs, (int)rs.index_of[(int)rs.genpoly[index2]] + num3)]);
                        rs.genpoly[0] = rs.alpha_to[this.modnn(rs, (int)rs.index_of[(int)rs.genpoly[0]] + num3)];
                        ++num2;
                        num3 += prim;
                    }
                    for (int index2 = 0; index2 <= nroots; ++index2)
                        rs.genpoly[index2] = rs.index_of[(int)rs.genpoly[index2]];
                }
            }
            return rs;
		}
		private PdfBarcodeQR.QR_RS init_rs(int symsize, int gfpoly, int fcr, int prim, int nroots, int pad)
		{
			PdfBarcodeQR.QR_RS qR_RS;
			for (qR_RS = this.rslist; qR_RS != null; qR_RS = qR_RS.next)
			{
				if (qR_RS.pad == pad && qR_RS.nroots == nroots && qR_RS.mm == symsize && qR_RS.gfpoly == gfpoly && qR_RS.fcr == fcr && qR_RS.prim == prim)
				{
					return qR_RS;
				}
			}
			qR_RS = this.init_rs_char(symsize, gfpoly, fcr, prim, nroots, pad);
			if (qR_RS != null)
			{
				qR_RS.next = this.rslist;
				this.rslist = qR_RS;
				return qR_RS;
			}
			return qR_RS;
		}
		private void free_rs_char(PdfBarcodeQR.QR_RS rs)
		{
		}
		private void free_rs_cache()
		{
			PdfBarcodeQR.QR_RS next;
			for (PdfBarcodeQR.QR_RS qR_RS = this.rslist; qR_RS != null; qR_RS = next)
			{
				next = qR_RS.next;
				this.free_rs_char(qR_RS);
			}
		}
		private void encode_rs_char(PdfBarcodeQR.QR_RS rs, byte[] data, ref byte[] parity)
		{
			for (int i = 0; i < rs.nroots; i++)
			{
				parity[i] = 0;
			}
			for (int j = 0; j < rs.nn - rs.nroots - rs.pad; j++)
			{
				byte b = rs.index_of[(int)(data[j] ^ parity[0])];
				if ((int)b != rs.nn)
				{
					for (int k = 1; k < rs.nroots; k++)
					{
						byte[] expr_42_cp_0 = parity;
						int expr_42_cp_1 = k;
						expr_42_cp_0[expr_42_cp_1] ^= rs.alpha_to[this.modnn(rs, (int)(b + rs.genpoly[rs.nroots - k]))];
					}
				}
				for (int l = 0; l < rs.nroots - 1; l++)
				{
					parity[l] = parity[l + 1];
				}
				if ((int)b != rs.nn)
				{
					parity[rs.nroots - 1] = rs.alpha_to[this.modnn(rs, (int)(b + rs.genpoly[0]))];
				}
				else
				{
					parity[rs.nroots - 1] = 0;
				}
			}
		}
		private void QRspec_getEccSpec(int version, PdfBarcodeQR.QRecLevel level, int[] spec)
		{
			int num = PdfBarcodeQR.eccTable[version][(int)level][0];
			int num2 = PdfBarcodeQR.eccTable[version][(int)level][1];
			int num3 = this.QRspec_getDataLength(version, level);
			int num4 = this.QRspec_getECCLength(version, level);
			if (num2 == 0)
			{
				spec[0] = num;
				spec[1] = num3 / num;
				spec[2] = num4 / num;
				spec[3] = (spec[4] = 0);
				return;
			}
			spec[0] = num;
			spec[1] = num3 / (num + num2);
			spec[2] = num4 / (num + num2);
			spec[3] = num2;
			spec[4] = spec[1] + 1;
		}
		private int QRspec_getECCLength(int version, PdfBarcodeQR.QRecLevel level)
		{
			return PdfBarcodeQR.qrspecCapacity[version].ec[(int)level];
		}
		private int QRspec_getDataLength(int version, PdfBarcodeQR.QRecLevel level)
		{
			return PdfBarcodeQR.qrspecCapacity[version].words - PdfBarcodeQR.qrspecCapacity[version].ec[(int)level];
		}
		private byte[] QRinput_getByteStream(PdfBarcodeQR.QRinput input)
		{
			PdfBarcodeQR.BitStream bitStream = this.QRinput_getBitStream(input);
			if (bitStream == null)
			{
				return null;
			}
			byte[] result = this.BitStream_toByte(bitStream);
			this.BitStream_free(bitStream);
			return result;
		}
		private byte[] BitStream_toByte(PdfBarcodeQR.BitStream bstream)
		{
			int length = bstream.length;
			if (length == 0)
			{
				return null;
			}
			byte[] array = new byte[(length + 7) / 8];
			int num = length / 8;
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				byte b = 0;
				for (int j = 0; j < 8; j++)
				{
					b = (byte)(b << 1);
					b |= bstream.data[num2];
					num2++;
				}
				array[i] = b;
			}
			if ((length & 7) != 0)
			{
				byte b = 0;
				for (int j = 0; j < (length & 7); j++)
				{
					b = (byte)(b << 1);
					b |= bstream.data[num2];
					num2++;
				}
				array[num] = b;
			}
			return array;
		}
		private PdfBarcodeQR.BitStream QRinput_getBitStream(PdfBarcodeQR.QRinput input)
		{
			PdfBarcodeQR.BitStream bitStream = this.QRinput_mergeBitStream(input);
			if (bitStream == null)
			{
				return null;
			}
			int num = this.QRinput_appendPaddingBit(bitStream, input);
			if (num < 0)
			{
				this.BitStream_free(bitStream);
				return null;
			}
			return bitStream;
		}
		private int QRinput_appendPaddingBit(PdfBarcodeQR.BitStream bstream, PdfBarcodeQR.QRinput input)
		{
			PdfBarcodeQR.BitStream bitStream = null;
			int num = bstream.length;
			int num2 = this.QRspec_getDataLength(input.version, input.level);
			int num3 = num2 * 8;
			if (num3 == num)
			{
				return 0;
			}
			int num4;
			if (num3 - num < 5)
			{
				num4 = this.BitStream_appendNum(bstream, num3 - num, 0u);
			}
			else
			{
				num += 4;
				int num5 = (num + 7) / 8;
				bitStream = this.BitStream_new();
				if (bitStream == null)
				{
					return -1;
				}
				num4 = this.BitStream_appendNum(bitStream, num5 * 8 - num + 4, 0u);
				if (num4 >= 0)
				{
					int num6 = num2 - num5;
					if (num6 > 0)
					{
						byte[] array = new byte[num6];
						for (int i = 0; i < num6; i++)
						{
                            array[i] = (i & 1) != 0 ? (byte)17 : (byte)236;
						}
						num4 = this.BitStream_appendBytes(bitStream, num6, array);
						if (num4 < 0)
						{
							goto IL_C5;
						}
					}
					num4 = this.BitStream_append(bstream, bitStream);
				}
			}
			IL_C5:
			this.BitStream_free(bitStream);
			return num4;
		}
		private int BitStream_append(PdfBarcodeQR.BitStream bstream, PdfBarcodeQR.BitStream arg)
		{
			if (arg == null)
			{
				return -1;
			}
			if (arg.length == 0)
			{
				return 0;
			}
			if (bstream.length != 0)
			{
				byte[] array = new byte[bstream.length + arg.length];
				Array.Copy(bstream.data, array, bstream.length);
				Array.Copy(arg.data, 0, array, bstream.length, arg.length);
				bstream.length += arg.length;
				bstream.data = array;
				return 0;
			}
			if (this.BitStream_allocate(bstream, arg.length) != 0)
			{
				return -1;
			}
			Array.Copy(arg.data, bstream.data, arg.length);
			return 0;
		}
		private int BitStream_allocate(PdfBarcodeQR.BitStream bstream, int length)
		{
			if (bstream == null)
			{
				return -1;
			}
			byte[] data = new byte[length];
			byte[] arg_12_0 = bstream.data;
			bstream.length = length;
			bstream.data = data;
			return 0;
		}
		private int BitStream_appendBytes(PdfBarcodeQR.BitStream bstream, int size, byte[] data)
		{
			if (size == 0)
			{
				return 0;
			}
			PdfBarcodeQR.BitStream bitStream = this.BitStream_newFromBytes(size, data);
			if (bitStream == null)
			{
				return -1;
			}
			int result = this.BitStream_append(bstream, bitStream);
			this.BitStream_free(bitStream);
			return result;
		}
		private PdfBarcodeQR.BitStream BitStream_newFromBytes(int size, byte[] data)
		{
			PdfBarcodeQR.BitStream bitStream = this.BitStream_new();
			if (bitStream == null)
			{
				return null;
			}
			if (this.BitStream_allocate(bitStream, size * 8) != 0)
			{
				this.BitStream_free(bitStream);
				return null;
			}
			int num = 0;
			for (int i = 0; i < size; i++)
			{
				byte b = 128;
				for (int j = 0; j < 8; j++)
				{
					if ((data[i] & b) != 0)
					{
						bitStream.data[num] = 1;
					}
					else
					{
						bitStream.data[num] = 0;
					}
					num++;
					b = (byte)(b >> 1);
				}
			}
			return bitStream;
		}
		private PdfBarcodeQR.BitStream BitStream_new()
		{
			return new PdfBarcodeQR.BitStream
			{
				length = 0,
				data = null
			};
		}
		private int BitStream_appendNum(PdfBarcodeQR.BitStream bstream, int bits, uint num)
		{
			if (bits == 0)
			{
				return 0;
			}
			PdfBarcodeQR.BitStream bitStream = this.BitStream_newFromNum(bits, num);
			if (bitStream == null)
			{
				return -1;
			}
			int result = this.BitStream_append(bstream, bitStream);
			this.BitStream_free(bitStream);
			return result;
		}
		private PdfBarcodeQR.BitStream BitStream_newFromNum(int bits, uint num)
		{
			PdfBarcodeQR.BitStream bitStream = this.BitStream_new();
			if (bitStream == null)
			{
				return null;
			}
			if (this.BitStream_allocate(bitStream, bits) != 0)
			{
				this.BitStream_free(bitStream);
				return null;
			}
			int num2 = 0;
			uint num3 = 1u << bits - 1;
			for (int i = 0; i < bits; i++)
			{
				if ((num & num3) != 0u)
				{
					bitStream.data[num2] = 1;
				}
				else
				{
					bitStream.data[num2] = 0;
				}
				num2++;
				num3 >>= 1;
			}
			return bitStream;
		}
		private PdfBarcodeQR.BitStream QRinput_mergeBitStream(PdfBarcodeQR.QRinput input)
		{
			if (this.QRinput_convertData(input) < 0)
			{
				return null;
			}
			PdfBarcodeQR.BitStream bitStream = this.BitStream_new();
			for (PdfBarcodeQR.QRinput_List qRinput_List = input.head; qRinput_List != null; qRinput_List = qRinput_List.next)
			{
				int num = this.BitStream_append(bitStream, qRinput_List.bstream);
				if (num < 0)
				{
					this.BitStream_free(bitStream);
					return null;
				}
			}
			return bitStream;
		}
		private int QRinput_convertData(PdfBarcodeQR.QRinput input)
		{
			int num = this.QRinput_estimateVersion(input);
			if (num > this.QRinput_getVersion(input))
			{
				this.QRinput_setVersion(input, num);
			}
			while (true)
			{
				int num2 = this.QRinput_createBitStream(input);
				if (num2 < 0)
				{
					break;
				}
				num = this.QRspec_getMinimumVersion((num2 + 7) / 8, input.level);
				if (num < 0)
				{
					return -1;
				}
				if (num <= this.QRinput_getVersion(input))
				{
					return 0;
				}
				this.QRinput_setVersion(input, num);
			}
			return -1;
		}
		private int QRspec_getMinimumVersion(int size, PdfBarcodeQR.QRecLevel level)
		{
			for (int i = 1; i <= 40; i++)
			{
				int num = PdfBarcodeQR.qrspecCapacity[i].words - PdfBarcodeQR.qrspecCapacity[i].ec[(int)level];
				if (num >= size)
				{
					return i;
				}
			}
			return -1;
		}
		private int QRinput_createBitStream(PdfBarcodeQR.QRinput input)
		{
			int num = 0;
			for (PdfBarcodeQR.QRinput_List qRinput_List = input.head; qRinput_List != null; qRinput_List = qRinput_List.next)
			{
				int num2 = this.QRinput_encodeBitStream(qRinput_List, input.version);
				if (num2 < 0)
				{
					return -1;
				}
				num += num2;
			}
			return num;
		}
		private int QRinput_encodeBitStream(PdfBarcodeQR.QRinput_List entry, int version)
		{
			PdfBarcodeQR.QRinput_List qRinput_List = null;
			if (entry.bstream != null)
			{
				this.BitStream_free(entry.bstream);
				entry.bstream = null;
			}
			int num = this.QRspec_maximumWords(entry.mode, version);
			int num2;
			if (entry.size > num)
			{
				PdfBarcodeQR.QRinput_List qRinput_List2 = this.QRinput_List_newEntry(entry.mode, num, entry.data);
				if (qRinput_List2 != null)
				{
					byte[] array = new byte[entry.data.Length - num];
					Array.Copy(entry.data, num, array, 0, entry.data.Length - num);
					qRinput_List = this.QRinput_List_newEntry(entry.mode, entry.size - num, array);
					if (qRinput_List != null)
					{
						num2 = this.QRinput_encodeBitStream(qRinput_List2, version);
						if (num2 >= 0)
						{
							num2 = this.QRinput_encodeBitStream(qRinput_List, version);
							if (num2 >= 0)
							{
								entry.bstream = this.BitStream_new();
								if (entry.bstream != null)
								{
									num2 = this.BitStream_append(entry.bstream, qRinput_List2.bstream);
									if (num2 >= 0)
									{
										num2 = this.BitStream_append(entry.bstream, qRinput_List.bstream);
										if (num2 >= 0)
										{
											this.QRinput_List_freeEntry(qRinput_List2);
											this.QRinput_List_freeEntry(qRinput_List);
											goto IL_173;
										}
									}
								}
							}
						}
					}
				}
				this.QRinput_List_freeEntry(qRinput_List2);
				this.QRinput_List_freeEntry(qRinput_List);
				return -1;
			}
			num2 = 0;
			switch (entry.mode)
			{
			case PdfBarcodeQR.QRencodeMode.QR_MODE_NUM:
				num2 = this.QRinput_encodeModeNum(entry, version);
				break;
			case PdfBarcodeQR.QRencodeMode.QR_MODE_AN:
				num2 = this.QRinput_encodeModeAn(entry, version);
				break;
			case PdfBarcodeQR.QRencodeMode.QR_MODE_8:
				num2 = this.QRinput_encodeMode8(entry, version);
				break;
			case PdfBarcodeQR.QRencodeMode.QR_MODE_KANJI:
				num2 = this.QRinput_encodeModeKanji(entry, version);
				break;
			case PdfBarcodeQR.QRencodeMode.QR_MODE_STRUCTURE:
				num2 = this.QRinput_encodeModeStructure(entry);
				break;
			}
			if (num2 < 0)
			{
				return -1;
			}
			IL_173:
			return entry.bstream.length;
		}
		private int QRinput_estimateVersion(PdfBarcodeQR.QRinput input)
		{
			int num = 0;
			while (true)
			{
				int num2 = num;
				int num3 = this.QRinput_estimateBitStreamSize(input, num2);
				num = this.QRspec_getMinimumVersion((num3 + 7) / 8, input.level);
				if (num < 0)
				{
					break;
				}
				if (num <= num2)
				{
					return num;
				}
			}
			return -1;
		}
		private int QRinput_estimateBitStreamSizeOfEntry(PdfBarcodeQR.QRinput_List entry, int version)
		{
			if (version == 0)
			{
				version = 1;
			}
			int num;
			switch (entry.mode)
			{
			case PdfBarcodeQR.QRencodeMode.QR_MODE_NUM:
				num = this.QRinput_estimateBitsModeNum(entry.size);
				break;
			case PdfBarcodeQR.QRencodeMode.QR_MODE_AN:
				num = this.QRinput_estimateBitsModeAn(entry.size);
				break;
			case PdfBarcodeQR.QRencodeMode.QR_MODE_8:
				num = this.QRinput_estimateBitsMode8(entry.size);
				break;
			case PdfBarcodeQR.QRencodeMode.QR_MODE_KANJI:
				num = this.QRinput_estimateBitsModeKanji(entry.size);
				break;
			case PdfBarcodeQR.QRencodeMode.QR_MODE_STRUCTURE:
				return 20;
			default:
				return 0;
			}
			int num2 = this.QRspec_lengthIndicator(entry.mode, version);
			int num3 = 1 << num2;
			int num4 = (entry.size + num3 - 1) / num3;
			return num + num4 * (4 + num2);
		}
		private int QRspec_lengthIndicator(PdfBarcodeQR.QRencodeMode mode, int version)
		{
			if (mode == PdfBarcodeQR.QRencodeMode.QR_MODE_STRUCTURE)
			{
				return 0;
			}
			int num;
			if (version <= 9)
			{
				num = 0;
			}
			else
			{
				if (version <= 26)
				{
					num = 1;
				}
				else
				{
					num = 2;
				}
			}
			return PdfBarcodeQR.lengthTableBits[(int)mode][num];
		}
		private int QRinput_estimateBitsModeKanji(int size)
		{
			return size / 2 * 13;
		}
		private int QRinput_estimateBitsModeNum(int size)
		{
			int num = size / 3;
			int num2 = num * 10;
			switch (size - num * 3)
			{
			case 1:
				num2 += 4;
				break;
			case 2:
				num2 += 7;
				break;
			}
			return num2;
		}
		private int QRinput_estimateBitsModeAn(int size)
		{
			int num = size / 2;
			int num2 = num * 11;
			if ((size & 1) != 0)
			{
				num2 += 6;
			}
			return num2;
		}
		private int QRinput_estimateBitsMode8(int size)
		{
			return size * 8;
		}
		private int QRinput_estimateBitStreamSize(PdfBarcodeQR.QRinput input, int version)
		{
			int num = 0;
			for (PdfBarcodeQR.QRinput_List qRinput_List = input.head; qRinput_List != null; qRinput_List = qRinput_List.next)
			{
				num += this.QRinput_estimateBitStreamSizeOfEntry(qRinput_List, version);
			}
			return num;
		}
		private int QRinput_encodeModeStructure(PdfBarcodeQR.QRinput_List entry)
		{
			entry.bstream = this.BitStream_new();
			if (entry.bstream == null)
			{
				return -1;
			}
			int num = this.BitStream_appendNum(entry.bstream, 4, 3u);
			if (num >= 0)
			{
				num = this.BitStream_appendNum(entry.bstream, 4, (uint)(entry.data[1] - 1));
				if (num >= 0)
				{
					num = this.BitStream_appendNum(entry.bstream, 4, (uint)(entry.data[0] - 1));
					if (num >= 0)
					{
						num = this.BitStream_appendNum(entry.bstream, 8, (uint)entry.data[2]);
						if (num >= 0)
						{
							return 0;
						}
					}
				}
			}
			this.BitStream_free(entry.bstream);
			entry.bstream = null;
			return -1;
		}
		private int QRinput_encodeModeKanji(PdfBarcodeQR.QRinput_List entry, int version)
		{
			entry.bstream = this.BitStream_new();
			if (entry.bstream == null)
			{
				return -1;
			}
			uint num = 8u;
			int num2 = this.BitStream_appendNum(entry.bstream, 4, num);
			if (num2 >= 0)
			{
				num = (uint)(entry.size / 2);
				num2 = this.BitStream_appendNum(entry.bstream, this.QRspec_lengthIndicator(PdfBarcodeQR.QRencodeMode.QR_MODE_KANJI, version), num);
				if (num2 >= 0)
				{
					for (int i = 0; i < entry.size; i += 2)
					{
						num = (uint)((int)entry.data[i] << 8 | (int)entry.data[i + 1]);
						if (num <= 40956u)
						{
							num -= 33088u;
						}
						else
						{
							num -= 49472u;
						}
						uint num3 = (num >> 8) * 192u;
						num = (num & 255u) + num3;
						num2 = this.BitStream_appendNum(entry.bstream, 13, num);
						if (num2 < 0)
						{
							goto IL_BC;
						}
					}
					return 0;
				}
			}
			IL_BC:
			this.BitStream_free(entry.bstream);
			entry.bstream = null;
			return -1;
		}
		private int QRinput_encodeMode8(PdfBarcodeQR.QRinput_List entry, int version)
		{
			entry.bstream = this.BitStream_new();
			if (entry.bstream == null)
			{
				return -1;
			}
			uint num = 4u;
			int num2 = this.BitStream_appendNum(entry.bstream, 4, num);
			if (num2 >= 0)
			{
				num = (uint)entry.size;
				num2 = this.BitStream_appendNum(entry.bstream, this.QRspec_lengthIndicator(PdfBarcodeQR.QRencodeMode.QR_MODE_8, version), num);
				if (num2 >= 0)
				{
					for (int i = 0; i < entry.size; i++)
					{
						num2 = this.BitStream_appendNum(entry.bstream, 8, (uint)entry.data[i]);
						if (num2 < 0)
						{
							goto IL_79;
						}
					}
					return 0;
				}
			}
			IL_79:
			this.BitStream_free(entry.bstream);
			entry.bstream = null;
			return -1;
		}
		private int QRinput_encodeModeAn(PdfBarcodeQR.QRinput_List entry, int version)
		{
			int num = entry.size / 2;
			entry.bstream = this.BitStream_new();
			if (entry.bstream == null)
			{
				return -1;
			}
			uint num2 = 2u;
			int num3 = this.BitStream_appendNum(entry.bstream, 4, num2);
			if (num3 >= 0)
			{
				num2 = (uint)entry.size;
				num3 = this.BitStream_appendNum(entry.bstream, this.QRspec_lengthIndicator(PdfBarcodeQR.QRencodeMode.QR_MODE_AN, version), num2);
				if (num3 >= 0)
				{
					for (int i = 0; i < num; i++)
					{
						num2 = (uint)(PdfBarcodeQR.QRinput_lookAnTable((int)entry.data[i * 2]) * 45);
						num2 += (uint)PdfBarcodeQR.QRinput_lookAnTable((int)entry.data[i * 2 + 1]);
						num3 = this.BitStream_appendNum(entry.bstream, 11, num2);
						if (num3 < 0)
						{
							goto IL_CE;
						}
					}
					if ((entry.size & 1) != 0)
					{
						num2 = (uint)PdfBarcodeQR.QRinput_lookAnTable((int)entry.data[num * 2]);
						num3 = this.BitStream_appendNum(entry.bstream, 6, num2);
						if (num3 < 0)
						{
							goto IL_CE;
						}
					}
					return 0;
				}
			}
			IL_CE:
			this.BitStream_free(entry.bstream);
			entry.bstream = null;
			return -1;
		}
		private int QRinput_encodeModeNum(PdfBarcodeQR.QRinput_List entry, int version)
		{
			int num = entry.size / 3;
			entry.bstream = this.BitStream_new();
			if (entry.bstream == null)
			{
				return -1;
			}
			uint num2 = 1u;
			int num3 = this.BitStream_appendNum(entry.bstream, 4, num2);
			if (num3 >= 0)
			{
				num2 = (uint)entry.size;
				num3 = this.BitStream_appendNum(entry.bstream, this.QRspec_lengthIndicator(PdfBarcodeQR.QRencodeMode.QR_MODE_NUM, version), num2);
				if (num3 >= 0)
				{
					for (int i = 0; i < num; i++)
					{
						num2 = (uint)((entry.data[i * 3] - 48) * 100);
						num2 += (uint)((entry.data[i * 3 + 1] - 48) * 10);
						num2 += (uint)(entry.data[i * 3 + 2] - 48);
						num3 = this.BitStream_appendNum(entry.bstream, 10, num2);
						if (num3 < 0)
						{
							goto IL_128;
						}
					}
					if (entry.size - num * 3 == 1)
					{
						num2 = (uint)(entry.data[num * 3] - 48);
						num3 = this.BitStream_appendNum(entry.bstream, 4, num2);
						if (num3 < 0)
						{
							goto IL_128;
						}
					}
					else
					{
						if (entry.size - num * 3 == 2)
						{
							num2 = (uint)((entry.data[num * 3] - 48) * 10);
							num2 += (uint)(entry.data[num * 3 + 1] - 48);
							this.BitStream_appendNum(entry.bstream, 7, num2);
							if (num3 < 0)
							{
								goto IL_128;
							}
						}
					}
					return 0;
				}
			}
			IL_128:
			this.BitStream_free(entry.bstream);
			entry.bstream = null;
			return -1;
		}
		private int QRspec_maximumWords(PdfBarcodeQR.QRencodeMode mode, int version)
		{
			if (mode == PdfBarcodeQR.QRencodeMode.QR_MODE_STRUCTURE)
			{
				return 3;
			}
			int num;
			if (version <= 9)
			{
				num = 0;
			}
			else
			{
				if (version <= 26)
				{
					num = 1;
				}
				else
				{
					num = 2;
				}
			}
			int num2 = PdfBarcodeQR.lengthTableBits[(int)mode][num];
			int num3 = (1 << num2) - 1;
			if (mode == PdfBarcodeQR.QRencodeMode.QR_MODE_KANJI)
			{
				num3 *= 2;
			}
			return num3;
		}
		private int QRinput_getVersion(PdfBarcodeQR.QRinput input)
		{
			return input.version;
		}
		private int QRinput_setVersion(PdfBarcodeQR.QRinput input, int version)
		{
			if (version < 0 || version > 40)
			{
				return -1;
			}
			input.version = version;
			return 0;
		}
		private int QRinput_check(PdfBarcodeQR.QRencodeMode mode, int size, byte[] data)
		{
			if (size <= 0)
			{
				return -1;
			}
			switch (mode)
			{
			case PdfBarcodeQR.QRencodeMode.QR_MODE_NUM:
				return this.QRinput_checkModeNum(size, data);
			case PdfBarcodeQR.QRencodeMode.QR_MODE_AN:
				return this.QRinput_checkModeAn(size, data);
			case PdfBarcodeQR.QRencodeMode.QR_MODE_8:
				return 0;
			case PdfBarcodeQR.QRencodeMode.QR_MODE_KANJI:
				return this.QRinput_checkModeKanji(size, data);
			case PdfBarcodeQR.QRencodeMode.QR_MODE_STRUCTURE:
				return 0;
			default:
				return -1;
			}
		}
		private int QRinput_checkModeKanji(int size, byte[] data)
		{
			if ((size & 1) != 0)
			{
				return -1;
			}
			for (int i = 0; i < size; i += 2)
			{
				uint num = (uint)((int)data[i] << 8 | (int)data[i + 1]);
				if (num < 33088u || (num > 40956u && num < 57408u) || num > 60351u)
				{
					return -1;
				}
			}
			return 0;
		}
		private int QRinput_checkModeAn(int size, byte[] data)
		{
			for (int i = 0; i < size; i++)
			{
				if (PdfBarcodeQR.QRinput_lookAnTable((int)data[i]) < 0)
				{
					return -1;
				}
			}
			return 0;
		}
		private int QRinput_checkModeNum(int size, byte[] data)
		{
			for (int i = 0; i < size; i++)
			{
				if (data[i] < 48 || data[i] > 57)
				{
					return -1;
				}
			}
			return 0;
		}
		private void BitStream_free(PdfBarcodeQR.BitStream bstream)
		{
			if (bstream != null)
			{
				bstream.data = null;
				bstream = null;
			}
		}
		private void QRinput_Struct_free(PdfBarcodeQR.QRinput_Struct s)
		{
			if (s != null)
			{
				PdfBarcodeQR.QRinput_InputList next;
				for (PdfBarcodeQR.QRinput_InputList qRinput_InputList = s.head; qRinput_InputList != null; qRinput_InputList = next)
				{
					next = qRinput_InputList.next;
					this.QRinput_InputList_freeEntry(qRinput_InputList);
				}
			}
		}
		private PdfBarcodeQR.QRinput_Struct QRinput_splitQRinputToStruct(PdfBarcodeQR.QRinput input)
		{
			PdfBarcodeQR.QRinput_Struct qRinput_Struct = this.QRinput_Struct_new();
			if (qRinput_Struct == null)
			{
				return null;
			}
			input = this.QRinput_dup(input);
			if (input == null)
			{
				this.QRinput_Struct_free(qRinput_Struct);
				return null;
			}
			this.QRinput_Struct_setParity(qRinput_Struct, this.QRinput_calcParity(input));
			int num = this.QRspec_getDataLength(input.version, input.level) * 8 - 20;
			if (num <= 0)
			{
				this.QRinput_Struct_free(qRinput_Struct);
				this.QRinput_free(input);
				return null;
			}
			int num2 = 0;
			PdfBarcodeQR.QRinput_List qRinput_List = input.head;
			PdfBarcodeQR.QRinput_List qRinput_List2 = null;
			int num4;
			while (qRinput_List != null)
			{
				int num3 = this.QRinput_estimateBitStreamSizeOfEntry(qRinput_List, input.version);
				if (num2 + num3 <= num)
				{
					num4 = this.QRinput_encodeBitStream(qRinput_List, input.version);
					if (num4 >= 0)
					{
						num2 += num4;
						qRinput_List2 = qRinput_List;
						qRinput_List = qRinput_List.next;
						continue;
					}
				}
				else
				{
					int num5 = this.QRinput_lengthOfCode(qRinput_List.mode, input.version, num - num2);
					PdfBarcodeQR.QRinput qRinput;
					if (num5 > 0)
					{
						num4 = this.QRinput_splitEntry(qRinput_List, num5);
						if (num4 < 0)
						{
							goto IL_1BE;
						}
						PdfBarcodeQR.QRinput_List next = qRinput_List.next;
						qRinput_List.next = null;
						qRinput = this.QRinput_new2(input.version, input.level);
						if (qRinput == null)
						{
							goto IL_1BE;
						}
						qRinput.head = next;
						qRinput.tail = input.tail;
						input.tail = qRinput_List;
						qRinput_List2 = qRinput_List;
						qRinput_List = next;
					}
					else
					{
						qRinput_List2.next = null;
						qRinput = this.QRinput_new2(input.version, input.level);
						if (qRinput == null)
						{
							goto IL_1BE;
						}
						qRinput.head = qRinput_List;
						qRinput.tail = input.tail;
						input.tail = qRinput_List2;
					}
					num4 = this.QRinput_Struct_appendInput(qRinput_Struct, input);
					if (num4 >= 0)
					{
						input = qRinput;
						num2 = 0;
						continue;
					}
				}
				IL_1BE:
				this.QRinput_free(input);
				this.QRinput_Struct_free(qRinput_Struct);
				return null;
			}
			this.QRinput_Struct_appendInput(qRinput_Struct, input);
			if (qRinput_Struct.size > 16)
			{
				this.QRinput_Struct_free(qRinput_Struct);
				return null;
			}
			num4 = this.QRinput_Struct_insertStructuredAppendHeaders(qRinput_Struct);
			if (num4 < 0)
			{
				this.QRinput_Struct_free(qRinput_Struct);
				return null;
			}
			return qRinput_Struct;
		}
		private int QRinput_Struct_insertStructuredAppendHeaders(PdfBarcodeQR.QRinput_Struct s)
		{
			if (s.parity < 0)
			{
				this.QRinput_Struct_calcParity(s);
			}
			int num = 0;
			for (PdfBarcodeQR.QRinput_InputList qRinput_InputList = s.head; qRinput_InputList != null; qRinput_InputList = qRinput_InputList.next)
			{
				num++;
			}
			int num2 = 1;
			for (PdfBarcodeQR.QRinput_InputList qRinput_InputList = s.head; qRinput_InputList != null; qRinput_InputList = qRinput_InputList.next)
			{
				if (this.QRinput_insertStructuredAppendHeader(qRinput_InputList.input, num, num2, (byte)s.parity) != 0)
				{
					return -1;
				}
				num2++;
			}
			return 0;
		}
		private int QRinput_insertStructuredAppendHeader(PdfBarcodeQR.QRinput input, int size, int index, byte parity)
		{
			byte[] array = new byte[3];
			if (size > 16)
			{
				return -1;
			}
			if (index <= 0 || index > 16)
			{
				return -1;
			}
			array[0] = (byte)size;
			array[1] = (byte)index;
			array[2] = parity;
			PdfBarcodeQR.QRinput_List qRinput_List = this.QRinput_List_newEntry(PdfBarcodeQR.QRencodeMode.QR_MODE_STRUCTURE, 3, array);
			if (qRinput_List == null)
			{
				return -1;
			}
			qRinput_List.next = input.head;
			input.head = qRinput_List;
			return 0;
		}
		private byte QRinput_Struct_calcParity(PdfBarcodeQR.QRinput_Struct s)
		{
			byte b = 0;
			for (PdfBarcodeQR.QRinput_InputList qRinput_InputList = s.head; qRinput_InputList != null; qRinput_InputList = qRinput_InputList.next)
			{
				b ^= this.QRinput_calcParity(qRinput_InputList.input);
			}
			this.QRinput_Struct_setParity(s, b);
			return b;
		}
		private void QRinput_Struct_setParity(PdfBarcodeQR.QRinput_Struct s, byte parity)
		{
			s.parity = (int)parity;
		}
		private int QRinput_Struct_appendInput(PdfBarcodeQR.QRinput_Struct s, PdfBarcodeQR.QRinput input)
		{
			PdfBarcodeQR.QRinput_InputList qRinput_InputList = this.QRinput_InputList_newEntry(input);
			if (qRinput_InputList == null)
			{
				return -1;
			}
			s.size++;
			if (s.tail == null)
			{
				s.head = qRinput_InputList;
				s.tail = qRinput_InputList;
			}
			else
			{
				s.tail.next = qRinput_InputList;
				s.tail = qRinput_InputList;
			}
			return s.size;
		}
		private PdfBarcodeQR.QRinput_InputList QRinput_InputList_newEntry(PdfBarcodeQR.QRinput input)
		{
			return new PdfBarcodeQR.QRinput_InputList
			{
				input = input,
				next = null
			};
		}
		private int QRinput_splitEntry(PdfBarcodeQR.QRinput_List entry, int bytes)
		{
			byte[] array = new byte[entry.size - bytes];
			Array.Copy(entry.data, bytes, array, 0, entry.size - bytes);
			PdfBarcodeQR.QRinput_List qRinput_List = this.QRinput_List_newEntry(entry.mode, entry.size - bytes, array);
			if (qRinput_List == null)
			{
				return -1;
			}
			int num = this.QRinput_List_shrinkEntry(entry, bytes);
			if (num < 0)
			{
				this.QRinput_List_freeEntry(qRinput_List);
				return -1;
			}
			qRinput_List.next = entry.next;
			entry.next = qRinput_List;
			return 0;
		}
		private int QRinput_List_shrinkEntry(PdfBarcodeQR.QRinput_List entry, int bytes)
		{
			byte[] array = new byte[bytes];
			Array.Copy(entry.data, array, bytes);
			entry.data = array;
			entry.size = bytes;
			return 0;
		}
		private int QRinput_lengthOfCode(PdfBarcodeQR.QRencodeMode mode, int version, int bits)
		{
			int num = bits - 4 - this.QRspec_lengthIndicator(mode, version);
			int num4;
			switch (mode)
			{
			case PdfBarcodeQR.QRencodeMode.QR_MODE_NUM:
			{
				int num2 = num / 10;
				int num3 = num - num2 * 10;
				num4 = num2 * 3;
				if (num3 >= 7)
				{
					num4 += 2;
				}
				else
				{
					if (num3 >= 4)
					{
						num4++;
					}
				}
				break;
			}
			case PdfBarcodeQR.QRencodeMode.QR_MODE_AN:
			{
				int num2 = num / 11;
				int num3 = num - num2 * 11;
				num4 = num2 * 2;
				if (num3 >= 6)
				{
					num4++;
				}
				break;
			}
			case PdfBarcodeQR.QRencodeMode.QR_MODE_8:
				num4 = num / 8;
				break;
			case PdfBarcodeQR.QRencodeMode.QR_MODE_KANJI:
				num4 = num / 13 * 2;
				break;
			case PdfBarcodeQR.QRencodeMode.QR_MODE_STRUCTURE:
				num4 = num / 8;
				break;
			default:
				num4 = 0;
				break;
			}
			int num5 = this.QRspec_maximumWords(mode, version);
			if (num4 < 0)
			{
				num4 = 0;
			}
			if (num4 > num5)
			{
				num4 = num5;
			}
			return num4;
		}
		private byte QRinput_calcParity(PdfBarcodeQR.QRinput input)
		{
			byte b = 0;
			for (PdfBarcodeQR.QRinput_List qRinput_List = input.head; qRinput_List != null; qRinput_List = qRinput_List.next)
			{
				if (qRinput_List.mode != PdfBarcodeQR.QRencodeMode.QR_MODE_STRUCTURE)
				{
					for (int i = qRinput_List.size - 1; i >= 0; i--)
					{
						b ^= qRinput_List.data[i];
					}
				}
			}
			return b;
		}
		private PdfBarcodeQR.QRinput QRinput_dup(PdfBarcodeQR.QRinput input)
		{
			PdfBarcodeQR.QRinput qRinput = this.QRinput_new2(input.version, input.level);
			if (qRinput == null)
			{
				return null;
			}
			for (PdfBarcodeQR.QRinput_List qRinput_List = input.head; qRinput_List != null; qRinput_List = qRinput_List.next)
			{
				PdfBarcodeQR.QRinput_List qRinput_List2 = this.QRinput_List_dup(qRinput_List);
				if (qRinput_List2 == null)
				{
					this.QRinput_free(qRinput);
					return null;
				}
				this.QRinput_appendEntry(qRinput, qRinput_List2);
			}
			return qRinput;
		}
		private PdfBarcodeQR.QRinput_List QRinput_List_dup(PdfBarcodeQR.QRinput_List entry)
		{
			PdfBarcodeQR.QRinput_List qRinput_List = new PdfBarcodeQR.QRinput_List();
			qRinput_List.mode = entry.mode;
			qRinput_List.size = entry.size;
			qRinput_List.data = new byte[qRinput_List.size];
			Array.Copy(entry.data, qRinput_List.data, entry.size);
			qRinput_List.bstream = null;
			qRinput_List.next = null;
			return qRinput_List;
		}
		private PdfBarcodeQR.QRinput_Struct QRinput_Struct_new()
		{
			return new PdfBarcodeQR.QRinput_Struct
			{
				size = 0,
				parity = -1,
				head = null,
				tail = null
			};
		}
		private static bool isdigit(byte c)
		{
			return (byte)((sbyte)c - 48) < 10;
		}
		private static bool isalnum(byte c)
		{
			return PdfBarcodeQR.QRinput_lookAnTable((int)c) >= 0;
		}
		private PdfBarcodeQR.QRencodeMode Split_identifyMode(byte[] str, PdfBarcodeQR.QRencodeMode hint)
		{
			if (str.Length == 0)
			{
				return PdfBarcodeQR.QRencodeMode.QR_MODE_NUL;
			}
			byte b = str[0];
			if (b == 0)
			{
				return PdfBarcodeQR.QRencodeMode.QR_MODE_NUL;
			}
			if (PdfBarcodeQR.isdigit(b))
			{
				return PdfBarcodeQR.QRencodeMode.QR_MODE_NUM;
			}
			if (PdfBarcodeQR.isalnum(b))
			{
				return PdfBarcodeQR.QRencodeMode.QR_MODE_AN;
			}
			if (hint == PdfBarcodeQR.QRencodeMode.QR_MODE_KANJI)
			{
				byte b2 = str[1];
				if (b2 != 0)
				{
					uint num = (uint)((int)b << 8 | (int)b2);
					if ((num >= 33088u && num <= 40956u) || (num >= 57408u && num <= 60351u))
					{
						return PdfBarcodeQR.QRencodeMode.QR_MODE_KANJI;
					}
				}
			}
			return PdfBarcodeQR.QRencodeMode.QR_MODE_8;
		}
		private int Split_eatNum(byte[] str, PdfBarcodeQR.QRinput input, PdfBarcodeQR.QRencodeMode hint)
		{
			int num = this.QRspec_lengthIndicator(PdfBarcodeQR.QRencodeMode.QR_MODE_NUM, input.version);
			int num2 = 0;
			while (num2 < str.Length && PdfBarcodeQR.isdigit(str[num2]))
			{
				num2++;
			}
			int num3 = num2;
			byte[] array = new byte[str.Length - num2];
			Array.Copy(str, num2, array, 0, str.Length - num2);
			PdfBarcodeQR.QRencodeMode qRencodeMode = this.Split_identifyMode(array, hint);
			if (qRencodeMode == PdfBarcodeQR.QRencodeMode.QR_MODE_8)
			{
				int num4 = this.QRinput_estimateBitsModeNum(num3) + 4 + num + this.QRinput_estimateBitsMode8(1) - this.QRinput_estimateBitsMode8(num3 + 1);
				if (num4 > 0)
				{
					return this.Split_eat8(str, input, hint);
				}
			}
			if (qRencodeMode == PdfBarcodeQR.QRencodeMode.QR_MODE_AN)
			{
				int num4 = this.QRinput_estimateBitsModeNum(num3) + 4 + num + this.QRinput_estimateBitsModeAn(1) - this.QRinput_estimateBitsModeAn(num3 + 1);
				if (num4 > 0)
				{
					return this.Split_eatAn(str, input, hint);
				}
			}
			int num5 = this.QRinput_append(input, PdfBarcodeQR.QRencodeMode.QR_MODE_NUM, num3, str);
			if (num5 < 0)
			{
				return -1;
			}
			return num3;
		}
		private int Split_eatAn(byte[] str, PdfBarcodeQR.QRinput input, PdfBarcodeQR.QRencodeMode hint)
		{
			int num = this.QRspec_lengthIndicator(PdfBarcodeQR.QRencodeMode.QR_MODE_AN, input.version);
			int num2 = this.QRspec_lengthIndicator(PdfBarcodeQR.QRencodeMode.QR_MODE_NUM, input.version);
			int num3 = 0;
			while (num3 < str.Length && PdfBarcodeQR.isalnum(str[num3]))
			{
				if (PdfBarcodeQR.isdigit(str[num3]))
				{
					int num4 = num3;
					while (num4 < str.Length && PdfBarcodeQR.isdigit(str[num4]))
					{
						num4++;
					}
					int num5 = this.QRinput_estimateBitsModeAn(num3) + this.QRinput_estimateBitsModeNum(num4 - num3) + 4 + num2 - this.QRinput_estimateBitsModeAn(num4);
					if (num5 < 0)
					{
						break;
					}
					num3 = num4;
				}
				else
				{
					num3++;
				}
			}
			int num6 = num3;
			if (num3 < str.Length && !PdfBarcodeQR.isalnum(str[num3]))
			{
				int num5 = this.QRinput_estimateBitsModeAn(num6) + 4 + num + this.QRinput_estimateBitsMode8(1) - this.QRinput_estimateBitsMode8(num6 + 1);
				if (num5 > 0)
				{
					return this.Split_eat8(str, input, hint);
				}
			}
			int num7 = this.QRinput_append(input, PdfBarcodeQR.QRencodeMode.QR_MODE_AN, num6, str);
			if (num7 < 0)
			{
				return -1;
			}
			return num6;
		}
		private int Split_eatKanji(byte[] str, PdfBarcodeQR.QRinput input, PdfBarcodeQR.QRencodeMode hint)
		{
			int num = 0;
			byte[] array = new byte[str.Length - num];
			Array.Copy(str, num, array, 0, str.Length - num);
			while (this.Split_identifyMode(array, hint) == PdfBarcodeQR.QRencodeMode.QR_MODE_KANJI)
			{
				num += 2;
				array = new byte[str.Length - num];
				Array.Copy(str, num, array, 0, str.Length - num);
			}
			int num2 = num;
			int num3 = this.QRinput_append(input, PdfBarcodeQR.QRencodeMode.QR_MODE_KANJI, num2, str);
			if (num3 < 0)
			{
				return -1;
			}
			return num2;
		}
		private int Split_eat8(byte[] str, PdfBarcodeQR.QRinput input, PdfBarcodeQR.QRencodeMode hint)
		{
			int num = this.QRspec_lengthIndicator(PdfBarcodeQR.QRencodeMode.QR_MODE_AN, input.version);
			int num2 = this.QRspec_lengthIndicator(PdfBarcodeQR.QRencodeMode.QR_MODE_NUM, input.version);
			int i = 1;
			while (i < str.Length)
			{
				byte[] array = new byte[str.Length - i];
				Array.Copy(str, i, array, 0, str.Length - i);
				PdfBarcodeQR.QRencodeMode qRencodeMode = this.Split_identifyMode(array, hint);
				if (qRencodeMode == PdfBarcodeQR.QRencodeMode.QR_MODE_KANJI)
				{
					break;
				}
				if (qRencodeMode == PdfBarcodeQR.QRencodeMode.QR_MODE_NUM)
				{
					int num3 = i;
					while (num3 < str.Length && PdfBarcodeQR.isdigit(str[num3]))
					{
						num3++;
					}
					int num4 = this.QRinput_estimateBitsMode8(i) + this.QRinput_estimateBitsModeNum(num3 - i) + 4 + num2 - this.QRinput_estimateBitsMode8(num3);
					if (num4 < 0)
					{
						break;
					}
					i = num3;
				}
				else
				{
					if (qRencodeMode == PdfBarcodeQR.QRencodeMode.QR_MODE_AN)
					{
						int num3 = i;
						while (num3 < str.Length && PdfBarcodeQR.isalnum(str[num3]))
						{
							num3++;
						}
						int num4 = this.QRinput_estimateBitsMode8(i) + this.QRinput_estimateBitsModeAn(num3 - i) + 4 + num - this.QRinput_estimateBitsMode8(num3);
						if (num4 < 0)
						{
							break;
						}
						i = num3;
					}
					else
					{
						i++;
					}
				}
			}
			int num5 = i;
			int num6 = this.QRinput_append(input, PdfBarcodeQR.QRencodeMode.QR_MODE_8, num5, str);
			if (num6 < 0)
			{
				return -1;
			}
			return num5;
		}
		private int Split_splitString(byte[] str, PdfBarcodeQR.QRinput input, PdfBarcodeQR.QRencodeMode hint)
		{
			if (str.Length == 0)
			{
				return 0;
			}
			PdfBarcodeQR.QRencodeMode qRencodeMode = this.Split_identifyMode(str, hint);
			int num;
			if (qRencodeMode == PdfBarcodeQR.QRencodeMode.QR_MODE_NUM)
			{
				num = this.Split_eatNum(str, input, hint);
			}
			else
			{
				if (qRencodeMode == PdfBarcodeQR.QRencodeMode.QR_MODE_AN)
				{
					num = this.Split_eatAn(str, input, hint);
				}
				else
				{
					if (qRencodeMode == PdfBarcodeQR.QRencodeMode.QR_MODE_KANJI && hint == PdfBarcodeQR.QRencodeMode.QR_MODE_KANJI)
					{
						num = this.Split_eatKanji(str, input, hint);
					}
					else
					{
						num = this.Split_eat8(str, input, hint);
					}
				}
			}
			if (num == 0)
			{
				return 0;
			}
			if (num < 0)
			{
				return -1;
			}
			byte[] array = new byte[str.Length - num];
			Array.Copy(str, num, array, 0, str.Length - num);
			return this.Split_splitString(array, input, hint);
		}
		private byte[] dupAndToUpper(byte[] str, PdfBarcodeQR.QRencodeMode hint)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(str).ToUpper());
			int num = 0;
			while (bytes[num] != 0)
			{
				byte[] destinationArray = new byte[bytes.Length - num];
				Array.Copy(bytes, num, destinationArray, 0, bytes.Length - num);
				PdfBarcodeQR.QRencodeMode qRencodeMode = this.Split_identifyMode(bytes, hint);
				if (qRencodeMode == PdfBarcodeQR.QRencodeMode.QR_MODE_KANJI)
				{
					num += 2;
				}
				else
				{
					if (bytes[num] >= 97 && bytes[num] <= 122)
					{
						bytes[num] -= 32;
					}
					num++;
				}
			}
			return bytes;
		}
		private PdfBarcodeQR.QRcode_List QRcode_encodeInputStructured(PdfBarcodeQR.QRinput_Struct s)
		{
			PdfBarcodeQR.QRcode_List qRcode_List = null;
			PdfBarcodeQR.QRcode_List qRcode_List2 = null;
			PdfBarcodeQR.QRinput_InputList qRinput_InputList = s.head;
			while (qRinput_InputList != null)
			{
				if (qRcode_List == null)
				{
					PdfBarcodeQR.QRcode_List qRcode_List3 = this.QRcode_List_newEntry();
					if (qRcode_List3 == null)
					{
						goto IL_5E;
					}
					qRcode_List = qRcode_List3;
					qRcode_List2 = qRcode_List;
				}
				else
				{
					PdfBarcodeQR.QRcode_List qRcode_List3 = this.QRcode_List_newEntry();
					if (qRcode_List3 == null)
					{
						goto IL_5E;
					}
					qRcode_List2.next = qRcode_List3;
					qRcode_List2 = qRcode_List2.next;
				}
				qRcode_List2.code = this.QRcode_encodeInput(qRinput_InputList.input);
				if (qRcode_List2.code != null)
				{
					qRinput_InputList = qRinput_InputList.next;
					continue;
				}
				IL_5E:
				this.QRcode_List_free(qRcode_List);
				return null;
			}
			return qRcode_List;
		}
		private PdfBarcodeQR.QRcode_List QRcode_List_newEntry()
		{
			return new PdfBarcodeQR.QRcode_List
			{
				next = null,
				code = null
			};
		}
		private PdfBarcodeQR.QRcode QRcode_new(int version, int width, byte[] data)
		{
			return new PdfBarcodeQR.QRcode
			{
				version = version,
				width = width,
				data = data
			};
		}
		private int Mask_writeFormatInformation(int width, byte[] frame, int mask, PdfBarcodeQR.QRecLevel level)
		{
			int num = 0;
			uint num2 = this.QRspec_getFormatInfo(mask, level);
			for (int i = 0; i < 8; i++)
			{
				byte b;
				if ((num2 & 1u) != 0u)
				{
					num += 2;
					b = 133;
				}
				else
				{
					b = 132;
				}
				frame[width * 8 + width - 1 - i] = b;
				if (i < 6)
				{
					frame[width * i + 8] = b;
				}
				else
				{
					frame[width * (i + 1) + 8] = b;
				}
				num2 >>= 1;
			}
			for (int i = 0; i < 7; i++)
			{
				byte b;
				if ((num2 & 1u) != 0u)
				{
					num += 2;
					b = 133;
				}
				else
				{
					b = 132;
				}
				frame[width * (width - 7 + i) + 8] = b;
				if (i == 0)
				{
					frame[width * 8 + 7] = b;
				}
				else
				{
					frame[width * 8 + 6 - i] = b;
				}
				num2 >>= 1;
			}
			return num;
		}
		private static int MaskMaker(int width, byte[] ss, ref byte[] dd, PdfBarcodeQR.MyDelegate foo)
		{
            int num = 0;
            int index1 = 0;
            int index2 = 0;
            for (int y = 0; y < width; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    dd[index2] = ((int)ss[index1] & 128) == 0 ? (byte)((int)ss[index1] ^ (foo(x, y) == 0 ? 1 : 0)) : ss[index1];
                    num += (int)dd[index2] & 1;
                    ++index1;
                    ++index2;
                }
            }
            return num;
		}
		private static int Mask0Exp(int x, int y)
		{
			return x + y & 1;
		}
		private static int Mask1Exp(int x, int y)
		{
			return y & 1;
		}
		private static int Mask2Exp(int x, int y)
		{
			return x % 3;
		}
		private static int Mask3Exp(int x, int y)
		{
			return (x + y) % 3;
		}
		private static int Mask4Exp(int x, int y)
		{
			return y / 2 + x / 3 & 1;
		}
		private static int Mask5Exp(int x, int y)
		{
			return (x * y & 1) + x * y % 3;
		}
		private static int Mask6Exp(int x, int y)
		{
			return (x * y & 1) + x * y % 3 & 1;
		}
		private static int Mask7Exp(int x, int y)
		{
			return x * y % 3 + (x + y & 1) & 1;
		}
		private static int Mask_maskAUX(int mask, int width, byte[] s, ref byte[] d)
		{
			PdfBarcodeQR.MyDelegate foo = new PdfBarcodeQR.MyDelegate(PdfBarcodeQR.Masks[mask].Invoke);
			return PdfBarcodeQR.MaskMaker(width, s, ref d, foo);
		}
		private byte[] Mask_makeMask(int width, byte[] frame, int mask, PdfBarcodeQR.QRecLevel level)
		{
			byte[] array = new byte[width * width];
			PdfBarcodeQR.Mask_maskAUX(mask, width, frame, ref array);
			this.Mask_writeFormatInformation(width, array, mask, level);
			return array;
		}
		private int Mask_calcN1N3(int length, int[] runLength)
		{
			int num = 0;
			for (int i = 0; i < length; i++)
			{
				if (runLength[i] >= 5)
				{
					num += 3 + (runLength[i] - 5);
				}
				if ((i & 1) != 0 && i >= 3 && i < length - 2 && runLength[i] % 3 == 0)
				{
					int num2 = runLength[i] / 3;
					if (runLength[i - 2] == num2 && runLength[i - 1] == num2 && runLength[i + 1] == num2 && runLength[i + 2] == num2)
					{
						if (runLength[i - 3] < 0 || runLength[i - 3] >= 4 * num2)
						{
							num += 40;
						}
						else
						{
							if (i + 3 >= length || runLength[i + 3] >= 4 * num2)
							{
								num += 40;
							}
						}
					}
				}
			}
			return num;
		}
		private int Mask_evaluateSymbol(int width, byte[] frame)
		{
            int num = 0;
            int index1 = 0;
            for (int index2 = 0; index2 < width; ++index2)
            {
                int index3 = 0;
                this.runLength[0] = 1;
                for (int index4 = 0; index4 < width; ++index4)
                {
                    if (index4 > 0 && index2 > 0 && (((int)(byte)((uint)frame[index1] & (uint)frame[index1 - 1] & (uint)frame[index1 - width] & (uint)frame[index1 - width - 1]) | (int)(byte)((uint)frame[index1] | (uint)frame[index1 - 1] | (uint)frame[index1 - width] | (uint)frame[index1 - width - 1]) ^ 1) & 1) != 0)
                        num += 3;
                    if (index4 == 0 && ((int)frame[index1] & 1) != 0)
                    {
                        this.runLength[0] = -1;
                        index3 = 1;
                        this.runLength[index3] = 1;
                    }
                    else if (index4 > 0)
                    {
                        if ((((int)frame[index1] ^ (int)frame[index1 - 1]) & 1) != 0)
                        {
                            ++index3;
                            this.runLength[index3] = 1;
                        }
                        else
                            ++this.runLength[index3];
                    }
                    ++index1;
                }
                num += this.Mask_calcN1N3(index3 + 1, this.runLength);
            }
            for (int index2 = 0; index2 < width; ++index2)
            {
                int index3 = 0;
                this.runLength[0] = 1;
                int index4 = index2;
                for (int index5 = 0; index5 < width; ++index5)
                {
                    if (index5 == 0 && ((int)frame[index4] & 1) != 0)
                    {
                        this.runLength[0] = -1;
                        index3 = 1;
                        this.runLength[index3] = 1;
                    }
                    else if (index5 > 0)
                    {
                        if ((((int)frame[index4] ^ (int)frame[index4 - width]) & 1) != 0)
                        {
                            ++index3;
                            this.runLength[index3] = 1;
                        }
                        else
                            ++this.runLength[index3];
                    }
                    index4 += width;
                }
                num += this.Mask_calcN1N3(index3 + 1, this.runLength);
            }
            return num;
		}
		private byte[] Mask_mask(int width, byte[] frame, PdfBarcodeQR.QRecLevel level)
		{
			byte[] array = null;
			int num = 2147483647;
			byte[] array2 = new byte[width * width];
			for (int i = 0; i < 8; i++)
			{
				int num2 = PdfBarcodeQR.Mask_maskAUX(i, width, frame, ref array2);
				num2 += this.Mask_writeFormatInformation(width, array2, i, level);
				num2 = 100 * num2 / (width * width);
				int num3 = Math.Abs(num2 - 50) / 5 * 10;
				num3 += this.Mask_evaluateSymbol(width, array2);
				if (num3 < num)
				{
					num = num3;
					array = new byte[width * width];
					Array.Copy(array2, array, width * width);
				}
			}
			return array;
		}
		private uint QRspec_getFormatInfo(int mask, PdfBarcodeQR.QRecLevel level)
		{
			if (mask < 0 || mask > 7)
			{
				return 0u;
			}
			return PdfBarcodeQR.formatInfo[(int)level][mask];
		}
		private static sbyte QRinput_lookAnTable(int c)
		{
			if ((c & 128) == 0)
			{
				return PdfBarcodeQR.QRinput_anTable[c];
			}
			return -1;
		}
		static PdfBarcodeQR()
		{
			// Note: this type is marked as 'beforefieldinit'.
			PdfBarcodeQR.QRspec_Capacity[] array = new PdfBarcodeQR.QRspec_Capacity[41];
			PdfBarcodeQR.QRspec_Capacity[] arg_1A_0 = array;
			int arg_1A_1 = 0;
			int arg_15_0 = 0;
			int arg_15_1 = 0;
			int arg_15_2 = 0;
			int[] array2 = new int[4];
			arg_1A_0[arg_1A_1] = new PdfBarcodeQR.QRspec_Capacity(arg_15_0, arg_15_1, arg_15_2, array2);
			array[1] = new PdfBarcodeQR.QRspec_Capacity(21, 26, 0, new int[]
			{
				7,
				10,
				13,
				17
			});
			array[2] = new PdfBarcodeQR.QRspec_Capacity(25, 44, 7, new int[]
			{
				10,
				16,
				22,
				28
			});
			array[3] = new PdfBarcodeQR.QRspec_Capacity(29, 70, 7, new int[]
			{
				15,
				26,
				36,
				44
			});
			array[4] = new PdfBarcodeQR.QRspec_Capacity(33, 100, 7, new int[]
			{
				20,
				36,
				52,
				64
			});
			array[5] = new PdfBarcodeQR.QRspec_Capacity(37, 134, 7, new int[]
			{
				26,
				48,
				72,
				88
			});
			array[6] = new PdfBarcodeQR.QRspec_Capacity(41, 172, 7, new int[]
			{
				36,
				64,
				96,
				112
			});
			array[7] = new PdfBarcodeQR.QRspec_Capacity(45, 196, 0, new int[]
			{
				40,
				72,
				108,
				130
			});
			array[8] = new PdfBarcodeQR.QRspec_Capacity(49, 242, 0, new int[]
			{
				48,
				88,
				132,
				156
			});
			array[9] = new PdfBarcodeQR.QRspec_Capacity(53, 292, 0, new int[]
			{
				60,
				110,
				160,
				192
			});
			array[10] = new PdfBarcodeQR.QRspec_Capacity(57, 346, 0, new int[]
			{
				72,
				130,
				192,
				224
			});
			array[11] = new PdfBarcodeQR.QRspec_Capacity(61, 404, 0, new int[]
			{
				80,
				150,
				224,
				264
			});
			array[12] = new PdfBarcodeQR.QRspec_Capacity(65, 466, 0, new int[]
			{
				96,
				176,
				260,
				308
			});
			array[13] = new PdfBarcodeQR.QRspec_Capacity(69, 532, 0, new int[]
			{
				104,
				198,
				288,
				352
			});
			array[14] = new PdfBarcodeQR.QRspec_Capacity(73, 581, 3, new int[]
			{
				120,
				216,
				320,
				384
			});
			array[15] = new PdfBarcodeQR.QRspec_Capacity(77, 655, 3, new int[]
			{
				132,
				240,
				360,
				432
			});
			array[16] = new PdfBarcodeQR.QRspec_Capacity(81, 733, 3, new int[]
			{
				144,
				280,
				408,
				480
			});
			array[17] = new PdfBarcodeQR.QRspec_Capacity(85, 815, 3, new int[]
			{
				168,
				308,
				448,
				532
			});
			array[18] = new PdfBarcodeQR.QRspec_Capacity(89, 901, 3, new int[]
			{
				180,
				338,
				504,
				588
			});
			array[19] = new PdfBarcodeQR.QRspec_Capacity(93, 991, 3, new int[]
			{
				196,
				364,
				546,
				650
			});
			array[20] = new PdfBarcodeQR.QRspec_Capacity(97, 1085, 3, new int[]
			{
				224,
				416,
				600,
				700
			});
			array[21] = new PdfBarcodeQR.QRspec_Capacity(101, 1156, 4, new int[]
			{
				224,
				442,
				644,
				750
			});
			array[22] = new PdfBarcodeQR.QRspec_Capacity(105, 1258, 4, new int[]
			{
				252,
				476,
				690,
				816
			});
			array[23] = new PdfBarcodeQR.QRspec_Capacity(109, 1364, 4, new int[]
			{
				270,
				504,
				750,
				900
			});
			array[24] = new PdfBarcodeQR.QRspec_Capacity(113, 1474, 4, new int[]
			{
				300,
				560,
				810,
				960
			});
			array[25] = new PdfBarcodeQR.QRspec_Capacity(117, 1588, 4, new int[]
			{
				312,
				588,
				870,
				1050
			});
			array[26] = new PdfBarcodeQR.QRspec_Capacity(121, 1706, 4, new int[]
			{
				336,
				644,
				952,
				1110
			});
			array[27] = new PdfBarcodeQR.QRspec_Capacity(125, 1828, 4, new int[]
			{
				360,
				700,
				1020,
				1200
			});
			array[28] = new PdfBarcodeQR.QRspec_Capacity(129, 1921, 3, new int[]
			{
				390,
				728,
				1050,
				1260
			});
			array[29] = new PdfBarcodeQR.QRspec_Capacity(133, 2051, 3, new int[]
			{
				420,
				784,
				1140,
				1350
			});
			array[30] = new PdfBarcodeQR.QRspec_Capacity(137, 2185, 3, new int[]
			{
				450,
				812,
				1200,
				1440
			});
			array[31] = new PdfBarcodeQR.QRspec_Capacity(141, 2323, 3, new int[]
			{
				480,
				868,
				1290,
				1530
			});
			array[32] = new PdfBarcodeQR.QRspec_Capacity(145, 2465, 3, new int[]
			{
				510,
				924,
				1350,
				1620
			});
			array[33] = new PdfBarcodeQR.QRspec_Capacity(149, 2611, 3, new int[]
			{
				540,
				980,
				1440,
				1710
			});
			array[34] = new PdfBarcodeQR.QRspec_Capacity(153, 2761, 3, new int[]
			{
				570,
				1036,
				1530,
				1800
			});
			array[35] = new PdfBarcodeQR.QRspec_Capacity(157, 2876, 0, new int[]
			{
				570,
				1064,
				1590,
				1890
			});
			array[36] = new PdfBarcodeQR.QRspec_Capacity(161, 3034, 0, new int[]
			{
				600,
				1120,
				1680,
				1980
			});
			array[37] = new PdfBarcodeQR.QRspec_Capacity(165, 3196, 0, new int[]
			{
				630,
				1204,
				1770,
				2100
			});
			array[38] = new PdfBarcodeQR.QRspec_Capacity(169, 3362, 0, new int[]
			{
				660,
				1260,
				1860,
				2220
			});
			array[39] = new PdfBarcodeQR.QRspec_Capacity(173, 3532, 0, new int[]
			{
				720,
				1316,
				1950,
				2310
			});
			array[40] = new PdfBarcodeQR.QRspec_Capacity(177, 3706, 0, new int[]
			{
				750,
				1372,
				2040,
				2430
			});
			PdfBarcodeQR.qrspecCapacity = array;
			int[][][] array3 = new int[41][][];
			int[][][] arg_5CA_0 = array3;
			int arg_5CA_1 = 0;
			int[][] array4 = new int[4][];
			int[][] arg_5A1_0 = array4;
			int arg_5A1_1 = 0;
			int[] array5 = new int[2];
			arg_5A1_0[arg_5A1_1] = array5;
			int[][] arg_5AE_0 = array4;
			int arg_5AE_1 = 1;
			int[] array6 = new int[2];
			arg_5AE_0[arg_5AE_1] = array6;
			int[][] arg_5BB_0 = array4;
			int arg_5BB_1 = 2;
			int[] array7 = new int[2];
			arg_5BB_0[arg_5BB_1] = array7;
			int[][] arg_5C8_0 = array4;
			int arg_5C8_1 = 3;
			int[] array8 = new int[2];
			arg_5C8_0[arg_5C8_1] = array8;
			arg_5CA_0[arg_5CA_1] = array4;
			int[][][] arg_623_0 = array3;
			int arg_623_1 = 1;
			int[][] array9 = new int[4][];
			int[][] arg_5E7_0 = array9;
			int arg_5E7_1 = 0;
			int[] array10 = new int[2];
			array10[0] = 1;
			arg_5E7_0[arg_5E7_1] = array10;
			int[][] arg_5FA_0 = array9;
			int arg_5FA_1 = 1;
			int[] array11 = new int[2];
			array11[0] = 1;
			arg_5FA_0[arg_5FA_1] = array11;
			int[][] arg_60D_0 = array9;
			int arg_60D_1 = 2;
			int[] array12 = new int[2];
			array12[0] = 1;
			arg_60D_0[arg_60D_1] = array12;
			int[][] arg_620_0 = array9;
			int arg_620_1 = 3;
			int[] array13 = new int[2];
			array13[0] = 1;
			arg_620_0[arg_620_1] = array13;
			arg_623_0[arg_623_1] = array9;
			int[][][] arg_67C_0 = array3;
			int arg_67C_1 = 2;
			int[][] array14 = new int[4][];
			int[][] arg_640_0 = array14;
			int arg_640_1 = 0;
			int[] array15 = new int[2];
			array15[0] = 1;
			arg_640_0[arg_640_1] = array15;
			int[][] arg_653_0 = array14;
			int arg_653_1 = 1;
			int[] array16 = new int[2];
			array16[0] = 1;
			arg_653_0[arg_653_1] = array16;
			int[][] arg_666_0 = array14;
			int arg_666_1 = 2;
			int[] array17 = new int[2];
			array17[0] = 1;
			arg_666_0[arg_666_1] = array17;
			int[][] arg_679_0 = array14;
			int arg_679_1 = 3;
			int[] array18 = new int[2];
			array18[0] = 1;
			arg_679_0[arg_679_1] = array18;
			arg_67C_0[arg_67C_1] = array14;
			int[][][] arg_6D5_0 = array3;
			int arg_6D5_1 = 3;
			int[][] array19 = new int[4][];
			int[][] arg_699_0 = array19;
			int arg_699_1 = 0;
			int[] array20 = new int[2];
			array20[0] = 1;
			arg_699_0[arg_699_1] = array20;
			int[][] arg_6AC_0 = array19;
			int arg_6AC_1 = 1;
			int[] array21 = new int[2];
			array21[0] = 1;
			arg_6AC_0[arg_6AC_1] = array21;
			int[][] arg_6BF_0 = array19;
			int arg_6BF_1 = 2;
			int[] array22 = new int[2];
			array22[0] = 2;
			arg_6BF_0[arg_6BF_1] = array22;
			int[][] arg_6D2_0 = array19;
			int arg_6D2_1 = 3;
			int[] array23 = new int[2];
			array23[0] = 2;
			arg_6D2_0[arg_6D2_1] = array23;
			arg_6D5_0[arg_6D5_1] = array19;
			int[][][] arg_72E_0 = array3;
			int arg_72E_1 = 4;
			int[][] array24 = new int[4][];
			int[][] arg_6F2_0 = array24;
			int arg_6F2_1 = 0;
			int[] array25 = new int[2];
			array25[0] = 1;
			arg_6F2_0[arg_6F2_1] = array25;
			int[][] arg_705_0 = array24;
			int arg_705_1 = 1;
			int[] array26 = new int[2];
			array26[0] = 2;
			arg_705_0[arg_705_1] = array26;
			int[][] arg_718_0 = array24;
			int arg_718_1 = 2;
			int[] array27 = new int[2];
			array27[0] = 2;
			arg_718_0[arg_718_1] = array27;
			int[][] arg_72B_0 = array24;
			int arg_72B_1 = 3;
			int[] array28 = new int[2];
			array28[0] = 4;
			arg_72B_0[arg_72B_1] = array28;
			arg_72E_0[arg_72E_1] = array24;
			int[][][] arg_791_0 = array3;
			int arg_791_1 = 5;
			int[][] array29 = new int[4][];
			int[][] arg_74B_0 = array29;
			int arg_74B_1 = 0;
			int[] array30 = new int[2];
			array30[0] = 1;
			arg_74B_0[arg_74B_1] = array30;
			int[][] arg_75E_0 = array29;
			int arg_75E_1 = 1;
			int[] array31 = new int[2];
			array31[0] = 2;
			arg_75E_0[arg_75E_1] = array31;
			array29[2] = new int[]
			{
				2,
				2
			};
			array29[3] = new int[]
			{
				2,
				2
			};
			arg_791_0[arg_791_1] = array29;
			int[][][] arg_7EA_0 = array3;
			int arg_7EA_1 = 6;
			int[][] array32 = new int[4][];
			int[][] arg_7AE_0 = array32;
			int arg_7AE_1 = 0;
			int[] array33 = new int[2];
			array33[0] = 2;
			arg_7AE_0[arg_7AE_1] = array33;
			int[][] arg_7C1_0 = array32;
			int arg_7C1_1 = 1;
			int[] array34 = new int[2];
			array34[0] = 4;
			arg_7C1_0[arg_7C1_1] = array34;
			int[][] arg_7D4_0 = array32;
			int arg_7D4_1 = 2;
			int[] array35 = new int[2];
			array35[0] = 4;
			arg_7D4_0[arg_7D4_1] = array35;
			int[][] arg_7E7_0 = array32;
			int arg_7E7_1 = 3;
			int[] array36 = new int[2];
			array36[0] = 4;
			arg_7E7_0[arg_7E7_1] = array36;
			arg_7EA_0[arg_7EA_1] = array32;
			int[][][] arg_84D_0 = array3;
			int arg_84D_1 = 7;
			int[][] array37 = new int[4][];
			int[][] arg_807_0 = array37;
			int arg_807_1 = 0;
			int[] array38 = new int[2];
			array38[0] = 2;
			arg_807_0[arg_807_1] = array38;
			int[][] arg_81A_0 = array37;
			int arg_81A_1 = 1;
			int[] array39 = new int[2];
			array39[0] = 4;
			arg_81A_0[arg_81A_1] = array39;
			array37[2] = new int[]
			{
				2,
				4
			};
			array37[3] = new int[]
			{
				4,
				1
			};
			arg_84D_0[arg_84D_1] = array37;
			int[][][] arg_8B5_0 = array3;
			int arg_8B5_1 = 8;
			int[][] array40 = new int[4][];
			int[][] arg_86A_0 = array40;
			int arg_86A_1 = 0;
			int[] array41 = new int[2];
			array41[0] = 2;
			arg_86A_0[arg_86A_1] = array41;
			array40[1] = new int[]
			{
				2,
				2
			};
			array40[2] = new int[]
			{
				4,
				2
			};
			array40[3] = new int[]
			{
				4,
				2
			};
			arg_8B5_0[arg_8B5_1] = array40;
			int[][][] arg_91E_0 = array3;
			int arg_91E_1 = 9;
			int[][] array42 = new int[4][];
			int[][] arg_8D3_0 = array42;
			int arg_8D3_1 = 0;
			int[] array43 = new int[2];
			array43[0] = 2;
			arg_8D3_0[arg_8D3_1] = array43;
			array42[1] = new int[]
			{
				3,
				2
			};
			array42[2] = new int[]
			{
				4,
				4
			};
			array42[3] = new int[]
			{
				4,
				4
			};
			arg_91E_0[arg_91E_1] = array42;
			array3[10] = new int[][]
			{
				new int[]
				{
					2,
					2
				},
				new int[]
				{
					4,
					1
				},
				new int[]
				{
					6,
					2
				},
				new int[]
				{
					6,
					2
				}
			};
			int[][][] arg_9ED_0 = array3;
			int arg_9ED_1 = 11;
			int[][] array44 = new int[4][];
			int[][] arg_9AA_0 = array44;
			int arg_9AA_1 = 0;
			int[] array45 = new int[2];
			array45[0] = 4;
			arg_9AA_0[arg_9AA_1] = array45;
			array44[1] = new int[]
			{
				1,
				4
			};
			array44[2] = new int[]
			{
				4,
				4
			};
			array44[3] = new int[]
			{
				3,
				8
			};
			arg_9ED_0[arg_9ED_1] = array44;
			array3[12] = new int[][]
			{
				new int[]
				{
					2,
					2
				},
				new int[]
				{
					6,
					2
				},
				new int[]
				{
					4,
					6
				},
				new int[]
				{
					7,
					4
				}
			};
			int[][][] arg_A9A_0 = array3;
			int arg_A9A_1 = 13;
			array4 = new int[4][];
			int[][] arg_A5E_0 = array4;
			int arg_A5E_1 = 0;
			array2 = new int[2];
			array2[0] = 4;
			arg_A5E_0[arg_A5E_1] = array2;
			array4[1] = new int[]
			{
				8,
				1
			};
			array4[2] = new int[]
			{
				8,
				4
			};
			array4[3] = new int[]
			{
				12,
				4
			};
			arg_A9A_0[arg_A9A_1] = array4;
			array3[14] = new int[][]
			{
				new int[]
				{
					3,
					1
				},
				new int[]
				{
					4,
					5
				},
				new int[]
				{
					11,
					5
				},
				new int[]
				{
					11,
					5
				}
			};
			array3[15] = new int[][]
			{
				new int[]
				{
					5,
					1
				},
				new int[]
				{
					5,
					5
				},
				new int[]
				{
					5,
					7
				},
				new int[]
				{
					11,
					7
				}
			};
			array3[16] = new int[][]
			{
				new int[]
				{
					5,
					1
				},
				new int[]
				{
					7,
					3
				},
				new int[]
				{
					15,
					2
				},
				new int[]
				{
					3,
					13
				}
			};
			array3[17] = new int[][]
			{
				new int[]
				{
					1,
					5
				},
				new int[]
				{
					10,
					1
				},
				new int[]
				{
					1,
					15
				},
				new int[]
				{
					2,
					17
				}
			};
			array3[18] = new int[][]
			{
				new int[]
				{
					5,
					1
				},
				new int[]
				{
					9,
					4
				},
				new int[]
				{
					17,
					1
				},
				new int[]
				{
					2,
					19
				}
			};
			array3[19] = new int[][]
			{
				new int[]
				{
					3,
					4
				},
				new int[]
				{
					3,
					11
				},
				new int[]
				{
					17,
					4
				},
				new int[]
				{
					9,
					16
				}
			};
			array3[20] = new int[][]
			{
				new int[]
				{
					3,
					5
				},
				new int[]
				{
					3,
					13
				},
				new int[]
				{
					15,
					5
				},
				new int[]
				{
					15,
					10
				}
			};
			int[][][] arg_D6C_0 = array3;
			int arg_D6C_1 = 21;
			array4 = new int[4][];
			array4[0] = new int[]
			{
				4,
				4
			};
			int[][] arg_D42_0 = array4;
			int arg_D42_1 = 1;
			array2 = new int[2];
			array2[0] = 17;
			arg_D42_0[arg_D42_1] = array2;
			array4[2] = new int[]
			{
				17,
				6
			};
			array4[3] = new int[]
			{
				19,
				6
			};
			arg_D6C_0[arg_D6C_1] = array4;
			int[][][] arg_DBF_0 = array3;
			int arg_DBF_1 = 22;
			array4 = new int[4][];
			array4[0] = new int[]
			{
				2,
				7
			};
			int[][] arg_D99_0 = array4;
			int arg_D99_1 = 1;
			array2 = new int[2];
			array2[0] = 17;
			arg_D99_0[arg_D99_1] = array2;
			array4[2] = new int[]
			{
				7,
				16
			};
			int[][] arg_DBD_0 = array4;
			int arg_DBD_1 = 3;
			array2 = new int[2];
			array2[0] = 34;
			arg_DBD_0[arg_DBD_1] = array2;
			arg_DBF_0[arg_DBF_1] = array4;
			array3[23] = new int[][]
			{
				new int[]
				{
					4,
					5
				},
				new int[]
				{
					4,
					14
				},
				new int[]
				{
					11,
					14
				},
				new int[]
				{
					16,
					14
				}
			};
			array3[24] = new int[][]
			{
				new int[]
				{
					6,
					4
				},
				new int[]
				{
					6,
					14
				},
				new int[]
				{
					11,
					16
				},
				new int[]
				{
					30,
					2
				}
			};
			array3[25] = new int[][]
			{
				new int[]
				{
					8,
					4
				},
				new int[]
				{
					8,
					13
				},
				new int[]
				{
					7,
					22
				},
				new int[]
				{
					22,
					13
				}
			};
			array3[26] = new int[][]
			{
				new int[]
				{
					10,
					2
				},
				new int[]
				{
					19,
					4
				},
				new int[]
				{
					28,
					6
				},
				new int[]
				{
					33,
					4
				}
			};
			array3[27] = new int[][]
			{
				new int[]
				{
					8,
					4
				},
				new int[]
				{
					22,
					3
				},
				new int[]
				{
					8,
					26
				},
				new int[]
				{
					12,
					28
				}
			};
			array3[28] = new int[][]
			{
				new int[]
				{
					3,
					10
				},
				new int[]
				{
					3,
					23
				},
				new int[]
				{
					4,
					31
				},
				new int[]
				{
					11,
					31
				}
			};
			array3[29] = new int[][]
			{
				new int[]
				{
					7,
					7
				},
				new int[]
				{
					21,
					7
				},
				new int[]
				{
					1,
					37
				},
				new int[]
				{
					19,
					26
				}
			};
			array3[30] = new int[][]
			{
				new int[]
				{
					5,
					10
				},
				new int[]
				{
					19,
					10
				},
				new int[]
				{
					15,
					25
				},
				new int[]
				{
					23,
					25
				}
			};
			array3[31] = new int[][]
			{
				new int[]
				{
					13,
					3
				},
				new int[]
				{
					2,
					29
				},
				new int[]
				{
					42,
					1
				},
				new int[]
				{
					23,
					28
				}
			};
			int[][][] arg_115C_0 = array3;
			int arg_115C_1 = 32;
			array4 = new int[4][];
			int[][] arg_111B_0 = array4;
			int arg_111B_1 = 0;
			array2 = new int[2];
			array2[0] = 17;
			arg_111B_0[arg_111B_1] = array2;
			array4[1] = new int[]
			{
				10,
				23
			};
			array4[2] = new int[]
			{
				10,
				35
			};
			array4[3] = new int[]
			{
				19,
				35
			};
			arg_115C_0[arg_115C_1] = array4;
			array3[33] = new int[][]
			{
				new int[]
				{
					17,
					1
				},
				new int[]
				{
					14,
					21
				},
				new int[]
				{
					29,
					19
				},
				new int[]
				{
					11,
					46
				}
			};
			array3[34] = new int[][]
			{
				new int[]
				{
					13,
					6
				},
				new int[]
				{
					14,
					23
				},
				new int[]
				{
					44,
					7
				},
				new int[]
				{
					59,
					1
				}
			};
			array3[35] = new int[][]
			{
				new int[]
				{
					12,
					7
				},
				new int[]
				{
					12,
					26
				},
				new int[]
				{
					39,
					14
				},
				new int[]
				{
					22,
					41
				}
			};
			array3[36] = new int[][]
			{
				new int[]
				{
					6,
					14
				},
				new int[]
				{
					6,
					34
				},
				new int[]
				{
					46,
					10
				},
				new int[]
				{
					2,
					64
				}
			};
			array3[37] = new int[][]
			{
				new int[]
				{
					17,
					4
				},
				new int[]
				{
					29,
					14
				},
				new int[]
				{
					49,
					10
				},
				new int[]
				{
					24,
					46
				}
			};
			array3[38] = new int[][]
			{
				new int[]
				{
					4,
					18
				},
				new int[]
				{
					13,
					32
				},
				new int[]
				{
					48,
					14
				},
				new int[]
				{
					42,
					32
				}
			};
			array3[39] = new int[][]
			{
				new int[]
				{
					20,
					4
				},
				new int[]
				{
					40,
					7
				},
				new int[]
				{
					43,
					22
				},
				new int[]
				{
					10,
					67
				}
			};
			array3[40] = new int[][]
			{
				new int[]
				{
					19,
					6
				},
				new int[]
				{
					18,
					31
				},
				new int[]
				{
					34,
					34
				},
				new int[]
				{
					20,
					61
				}
			};
			PdfBarcodeQR.eccTable = array3;
			PdfBarcodeQR.versionPattern = new uint[]
			{
				31892u,
				34236u,
				39577u,
				42195u,
				48118u,
				51042u,
				55367u,
				58893u,
				63784u,
				68472u,
				70749u,
				76311u,
				79154u,
				84390u,
				87683u,
				92361u,
				96236u,
				102084u,
				102881u,
				110507u,
				110734u,
				117786u,
				119615u,
				126325u,
				127568u,
				133589u,
				136944u,
				141498u,
				145311u,
				150283u,
				152622u,
				158308u,
				161089u,
				167017u
			};
			array4 = new int[41][];
			int[][] arg_147F_0 = array4;
			int arg_147F_1 = 0;
			array2 = new int[2];
			arg_147F_0[arg_147F_1] = array2;
			int[][] arg_148A_0 = array4;
			int arg_148A_1 = 1;
			array2 = new int[2];
			arg_148A_0[arg_148A_1] = array2;
			int[][] arg_149A_0 = array4;
			int arg_149A_1 = 2;
			array2 = new int[2];
			array2[0] = 18;
			arg_149A_0[arg_149A_1] = array2;
			int[][] arg_14AA_0 = array4;
			int arg_14AA_1 = 3;
			array2 = new int[2];
			array2[0] = 22;
			arg_14AA_0[arg_14AA_1] = array2;
			int[][] arg_14BA_0 = array4;
			int arg_14BA_1 = 4;
			array2 = new int[2];
			array2[0] = 26;
			arg_14BA_0[arg_14BA_1] = array2;
			int[][] arg_14CA_0 = array4;
			int arg_14CA_1 = 5;
			array2 = new int[2];
			array2[0] = 30;
			arg_14CA_0[arg_14CA_1] = array2;
			int[][] arg_14DA_0 = array4;
			int arg_14DA_1 = 6;
			array2 = new int[2];
			array2[0] = 34;
			arg_14DA_0[arg_14DA_1] = array2;
			array4[7] = new int[]
			{
				22,
				38
			};
			array4[8] = new int[]
			{
				24,
				42
			};
			array4[9] = new int[]
			{
				26,
				46
			};
			array4[10] = new int[]
			{
				28,
				50
			};
			array4[11] = new int[]
			{
				30,
				54
			};
			array4[12] = new int[]
			{
				32,
				58
			};
			array4[13] = new int[]
			{
				34,
				62
			};
			array4[14] = new int[]
			{
				26,
				46
			};
			array4[15] = new int[]
			{
				26,
				48
			};
			array4[16] = new int[]
			{
				26,
				50
			};
			array4[17] = new int[]
			{
				30,
				54
			};
			array4[18] = new int[]
			{
				30,
				56
			};
			array4[19] = new int[]
			{
				30,
				58
			};
			array4[20] = new int[]
			{
				34,
				62
			};
			array4[21] = new int[]
			{
				28,
				50
			};
			array4[22] = new int[]
			{
				26,
				50
			};
			array4[23] = new int[]
			{
				30,
				54
			};
			array4[24] = new int[]
			{
				28,
				54
			};
			array4[25] = new int[]
			{
				32,
				58
			};
			array4[26] = new int[]
			{
				30,
				58
			};
			array4[27] = new int[]
			{
				34,
				62
			};
			array4[28] = new int[]
			{
				26,
				50
			};
			array4[29] = new int[]
			{
				30,
				54
			};
			array4[30] = new int[]
			{
				26,
				52
			};
			array4[31] = new int[]
			{
				30,
				56
			};
			array4[32] = new int[]
			{
				34,
				60
			};
			array4[33] = new int[]
			{
				30,
				58
			};
			array4[34] = new int[]
			{
				34,
				62
			};
			array4[35] = new int[]
			{
				30,
				54
			};
			array4[36] = new int[]
			{
				24,
				50
			};
			array4[37] = new int[]
			{
				28,
				54
			};
			array4[38] = new int[]
			{
				32,
				58
			};
			array4[39] = new int[]
			{
				26,
				54
			};
			array4[40] = new int[]
			{
				30,
				58
			};
			PdfBarcodeQR.alignmentPattern = array4;
			PdfBarcodeQR.lengthTableBits = new int[][]
			{
				new int[]
				{
					10,
					12,
					14
				},
				new int[]
				{
					9,
					11,
					13
				},
				new int[]
				{
					8,
					16,
					16
				},
				new int[]
				{
					8,
					10,
					12
				}
			};
			PdfBarcodeQR.QRinput_anTable = new sbyte[]
			{
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				36,
				-1,
				-1,
				-1,
				37,
				38,
				-1,
				-1,
				-1,
				-1,
				39,
				40,
				-1,
				41,
				42,
				43,
				0,
				1,
				2,
				3,
				4,
				5,
				6,
				7,
				8,
				9,
				44,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				10,
				11,
				12,
				13,
				14,
				15,
				16,
				17,
				18,
				19,
				20,
				21,
				22,
				23,
				24,
				25,
				26,
				27,
				28,
				29,
				30,
				31,
				32,
				33,
				34,
				35,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1
			};
			PdfBarcodeQR.Masks = new PdfBarcodeQR.MyDelegate[]
			{
				new PdfBarcodeQR.MyDelegate(PdfBarcodeQR.Mask0Exp),
				new PdfBarcodeQR.MyDelegate(PdfBarcodeQR.Mask1Exp),
				new PdfBarcodeQR.MyDelegate(PdfBarcodeQR.Mask2Exp),
				new PdfBarcodeQR.MyDelegate(PdfBarcodeQR.Mask3Exp),
				new PdfBarcodeQR.MyDelegate(PdfBarcodeQR.Mask4Exp),
				new PdfBarcodeQR.MyDelegate(PdfBarcodeQR.Mask5Exp),
				new PdfBarcodeQR.MyDelegate(PdfBarcodeQR.Mask6Exp),
				new PdfBarcodeQR.MyDelegate(PdfBarcodeQR.Mask7Exp)
			};
			PdfBarcodeQR.formatInfo = new uint[][]
			{
				new uint[]
				{
					30660u,
					29427u,
					32170u,
					30877u,
					26159u,
					25368u,
					27713u,
					26998u
				},
				new uint[]
				{
					21522u,
					20773u,
					24188u,
					23371u,
					17913u,
					16590u,
					20375u,
					19104u
				},
				new uint[]
				{
					13663u,
					12392u,
					16177u,
					14854u,
					9396u,
					8579u,
					11994u,
					11245u
				},
				new uint[]
				{
					5769u,
					5054u,
					7399u,
					6608u,
					1890u,
					597u,
					3340u,
					2107u
				}
			};
		}
	}
}
