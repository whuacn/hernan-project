using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
namespace Persits.PDF
{
	internal class PdfStream : PdfObject
	{
		private static byte[] fromB64 = new byte[]
		{
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			62,
			65,
			65,
			65,
			63,
			52,
			53,
			54,
			55,
			56,
			57,
			58,
			59,
			60,
			61,
			65,
			65,
			65,
			64,
			65,
			65,
			65,
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
			65,
			65,
			65,
			65,
			65,
			65,
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
			36,
			37,
			38,
			39,
			40,
			41,
			42,
			43,
			44,
			45,
			46,
			47,
			48,
			49,
			50,
			51,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65,
			65
		};
		private static uint[] pow85 = new uint[]
		{
			52200625u,
			614125u,
			7225u,
			85u,
			1u
		};
		private byte[] Padding = new byte[]
		{
			40,
			191,
			78,
			94,
			78,
			117,
			138,
			65,
			100,
			0,
			78,
			86,
			255,
			250,
			1,
			8,
			46,
			46,
			0,
			182,
			208,
			104,
			62,
			128,
			47,
			12,
			169,
			254,
			100,
			83,
			105,
			122
		};
		public MemoryStream m_objMemStream;
		public int m_nPtr;
		public int Length
		{
			get
			{
				return (int)this.m_objMemStream.Length;
			}
		}
		public byte this[int i]
		{
			get
			{
				long position = this.m_objMemStream.Position;
				this.m_objMemStream.Position = (long)i;
				byte result = (byte)this.m_objMemStream.ReadByte();
				this.m_objMemStream.Position = position;
				return result;
			}
			set
			{
				long position = this.m_objMemStream.Position;
				this.m_objMemStream.Position = (long)i;
				this.m_objMemStream.WriteByte(value);
				this.m_objMemStream.Position = position;
			}
		}
		public long DecodeBase64()
		{
			byte[] array = new byte[this.Length];
			int num = PdfStream.DecodeBase64Helper(array, (uint)this.Length, this.m_objMemStream.ToArray());
			if (num > 0 && num < this.Length)
			{
				this.Set(array, num - 1);
				return 0L;
			}
			return -1L;
		}
		public static int DecodeBase64Helper(byte[] outbuf, uint outsize, byte[] inbuf)
		{
			long num = 0L;
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < inbuf.Length; i++)
			{
				byte b = inbuf[i];
				int num4;
				if ((num4 = (int)PdfStream.fromB64[(int)b]) < 64)
				{
					num2++;
					num <<= 6;
					num += (long)num4;
					if (num2 >= 4)
					{
						if (outsize < 3u)
						{
							return -1;
						}
						outsize -= 3u;
						outbuf[num3 + 2] = (byte)num;
						num >>= 8;
						outbuf[num3 + 1] = (byte)num;
						num >>= 8;
						outbuf[num3] = (byte)num;
						num3 += 3;
						num = 0L;
						num2 = 0;
					}
				}
			}
			switch (num2)
			{
			case 2:
				if (outsize < 1u)
				{
					return 0;
				}
				outsize -= 1u;
				num >>= 4;
				outbuf[num3] = (byte)num;
				num3++;
				break;
			case 3:
				if (outsize < 2u)
				{
					return 0;
				}
				outsize -= 2u;
				num >>= 2;
				outbuf[num3 + 1] = (byte)num;
				num >>= 8;
				outbuf[num3] = (byte)num;
				num3 += 2;
				break;
			}
			if (outsize < 1u)
			{
				return 0;
			}
			outbuf[num3++] = 0;
			return num3;
		}
		public int EncodeFlate()
		{
			MemoryStream memoryStream = new MemoryStream();
			memoryStream.WriteByte(88);
			memoryStream.WriteByte(133);
			using (DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Compress, true))
			{
				deflateStream.Write(this.m_objMemStream.ToArray(), 0, (int)this.m_objMemStream.Length);
			}
			memoryStream.Write(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(this.Adler32(this.m_objMemStream.ToArray()))), 0, 4);
			byte[] array = memoryStream.ToArray();
			this.Set(array, array.Length);
			return 0;
		}
		private int Adler32(byte[] bytes)
		{
			uint num = 1u;
			uint num2 = 0u;
			for (int i = 0; i < bytes.Length; i++)
			{
				byte b = bytes[i];
				num = (num + (uint)b) % 65521u;
				num2 = (num2 + num) % 65521u;
			}
			return (int)((num2 << 16) + num);
		}
		public int DecodeFlate()
		{
			int result = 0;
			try
			{
				MemoryStream memoryStream = new MemoryStream(this.m_objMemStream.ToArray());
				MemoryStream memoryStream2 = new MemoryStream();
				memoryStream.ReadByte();
				memoryStream.ReadByte();
				DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Decompress, true);
				byte[] array = new byte[4096];
				int num;
				do
				{
					num = deflateStream.Read(array, 0, array.Length);
					if (num > 0)
					{
						memoryStream2.Write(array, 0, num);
					}
				}
				while (num > 0);
				deflateStream.Close();
				memoryStream2.Flush();
				byte[] array2 = memoryStream2.ToArray();
				this.Set(array2, array2.Length);
			}
			catch
			{
				result = -1;
			}
			return result;
		}
		public int DecodeFlate(PdfDict pDict, int nIndex)
		{
			int result = 0;
			try
			{
				MemoryStream memoryStream = new MemoryStream(this.m_objMemStream.ToArray());
				MemoryStream memoryStream2 = new MemoryStream();
				memoryStream.ReadByte();
				memoryStream.ReadByte();
				DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Decompress, true);
				byte[] array = new byte[4096];
				int num;
				do
				{
					num = deflateStream.Read(array, 0, array.Length);
					if (num > 0)
					{
						memoryStream2.Write(array, 0, num);
					}
				}
				while (num > 0);
				deflateStream.Close();
				memoryStream2.Flush();
				byte[] array2 = memoryStream2.ToArray();
				this.Set(array2, array2.Length);
				this.Unpredict(nIndex, pDict);
			}
			catch
			{
				result = -1;
			}
			return result;
		}
		private void Unpredict(int nIndex, PdfDict pDict)
		{
			if (pDict == null)
			{
				return;
			}
			PdfObject pdfObject = pDict.GetObjectByName("DecodeParms");
			if (pdfObject == null)
			{
				pdfObject = pDict.GetObjectByName("DP");
			}
			if (pdfObject != null && pdfObject.m_nType == enumType.pdfArray)
			{
				pdfObject = ((PdfArray)pdfObject).GetObject(nIndex);
			}
			if (pdfObject == null)
			{
				return;
			}
			if (pdfObject.m_nType != enumType.pdfDictionary)
			{
				return;
			}
			PdfDict pdfDict = (PdfDict)pdfObject;
			int predictorA = 1;
			int widthA = 1;
			int nCompsA = 1;
			int nBitsA = 8;
			pdfObject = pdfDict.GetObjectByName("Predictor");
			if (pdfObject != null && pdfObject.m_nType == enumType.pdfNumber)
			{
				predictorA = (int)((PdfNumber)pdfObject).m_fValue;
			}
			pdfObject = pdfDict.GetObjectByName("Columns");
			if (pdfObject != null && pdfObject.m_nType == enumType.pdfNumber)
			{
				widthA = (int)((PdfNumber)pdfObject).m_fValue;
			}
			pdfObject = pdfDict.GetObjectByName("Colors");
			if (pdfObject != null && pdfObject.m_nType == enumType.pdfNumber)
			{
				nCompsA = (int)((PdfNumber)pdfObject).m_fValue;
			}
			pdfObject = pdfDict.GetObjectByName("BitsPerComponent");
			if (pdfObject != null && pdfObject.m_nType == enumType.pdfNumber)
			{
				nBitsA = (int)((PdfNumber)pdfObject).m_fValue;
			}
			PdfStreamPredictor pdfStreamPredictor = new PdfStreamPredictor(this, predictorA, widthA, nCompsA, nBitsA);
			this.reset();
			PdfStream pdfStream = new PdfStream();
			int @char;
			while ((@char = pdfStreamPredictor.getChar()) != -1)
			{
				pdfStream.AppendChar((byte)@char);
			}
			this.Set(pdfStream);
		}
		public void reset()
		{
			this.m_nPtr = 0;
		}
		public int EncodeHex()
		{
			if (this.m_objMemStream == null || this.Length == 0)
			{
				return 0;
			}
			int size = this.Length * 2 + (this.Length - 1) / 80;
			PdfStream pdfStream = new PdfStream();
			pdfStream.Alloc(size);
			this.m_objMemStream.Position = 0L;
			int num = 0;
			int num2;
			while ((num2 = this.m_objMemStream.ReadByte()) >= 0)
			{
				byte[] bytes = Encoding.UTF8.GetBytes(string.Format("{0:X02}", num2));
				pdfStream.m_objMemStream.Write(bytes, 0, bytes.Length);
				num++;
				if (num > 0 && num % 80 == 0)
				{
					pdfStream.m_objMemStream.WriteByte(10);
				}
			}
			this.Set(pdfStream);
			return 0;
		}
		public int DecodeHex()
		{
			if (this.m_objMemStream == null || this.Length == 0)
			{
				return 0;
			}
			PdfStream pdfStream = new PdfStream();
			pdfStream.Alloc(this.Length);
			int num = 0;
			byte b = 0;
			this.m_objMemStream.Position = 0L;
			int num2;
			while ((num2 = this.m_objMemStream.ReadByte()) >= 0)
			{
				if (num2 != 0 && num2 != 9 && num2 != 10 && num2 != 12 && num2 != 13 && num2 != 32)
				{
					if (num == 0)
					{
						b = (byte)PdfInput.FromHex((byte)num2);
					}
					else
					{
						b = (byte)(((int)b << 4) + PdfInput.FromHex((byte)num2));
						pdfStream.m_objMemStream.WriteByte(b);
					}
					num ^= 1;
				}
			}
			if (num == 1)
			{
				pdfStream.m_objMemStream.WriteByte((byte)(b << 4));
			}
			this.Set(pdfStream);
			return 0;
		}
		public int DecodeLZW()
		{
			LZWDecoder lZWDecoder = new LZWDecoder();
			PdfStream pStream = new PdfStream();
			lZWDecoder.Decode(this, ref pStream);
			this.Set(pStream);
			return 0;
		}
		private void wput(ref PdfStream stream, uint tuple, int bytes)
		{
			switch (bytes)
			{
			case 1:
				stream.AppendChar((byte)(tuple >> 24));
				return;
			case 2:
				stream.AppendChar((byte)(tuple >> 24));
				stream.AppendChar((byte)(tuple >> 16));
				return;
			case 3:
				stream.AppendChar((byte)(tuple >> 24));
				stream.AppendChar((byte)(tuple >> 16));
				stream.AppendChar((byte)(tuple >> 8));
				return;
			case 4:
				stream.AppendChar((byte)(tuple >> 24));
				stream.AppendChar((byte)(tuple >> 16));
				stream.AppendChar((byte)(tuple >> 8));
				stream.AppendChar((byte)tuple);
				return;
			default:
				return;
			}
		}
		public int DecodeASCII85()
		{
			PdfStream pdfStream = new PdfStream();
			uint num = 0u;
			int num2 = 0;
			int i = 0;
			while (i < this.Length)
			{
				int num3 = (int)(this[i] & 255);
				int num4 = num3;
				if (num4 <= 32)
				{
					if (num4 != 0)
					{
						switch (num4)
						{
						case 8:
						case 9:
						case 10:
						case 12:
						case 13:
							break;
						case 11:
							goto IL_76;
						default:
							if (num4 != 32)
							{
								goto IL_76;
							}
							break;
						}
					}
				}
				else
				{
					if (num4 != 122)
					{
						if (num4 != 126)
						{
							if (num4 != 177)
							{
								goto IL_76;
							}
						}
						else
						{
							if (i < this.Length - 1 && this[i + 1] == 62)
							{
								if (num2 > 0)
								{
									num2--;
									num += PdfStream.pow85[num2];
									this.wput(ref pdfStream, num, num2);
								}
								this.Set(pdfStream);
								return 0;
							}
							return -3;
						}
					}
					else
					{
						if (num2 != 0)
						{
							return -2;
						}
						pdfStream.AppendChar(0);
						pdfStream.AppendChar(0);
						pdfStream.AppendChar(0);
						pdfStream.AppendChar(0);
					}
				}
				IL_113:
				i++;
				continue;
				IL_76:
				if (num3 < 33 || num3 > 117)
				{
					return -1;
				}
				num += (uint)((num3 - 33) * (int)PdfStream.pow85[num2++]);
				if (num2 == 5)
				{
					this.wput(ref pdfStream, num, 4);
					num2 = 0;
					num = 0u;
					goto IL_113;
				}
				goto IL_113;
			}
			return -4;
		}
		public int Encode(enumEncoding enc)
		{
			if (enc == enumEncoding.PdfEncAsciiHex)
			{
				return this.EncodeHex();
			}
			if (enc == enumEncoding.PdfEncFlate)
			{
				return this.EncodeFlate();
			}
			return 0;
		}
		public int Encrypt(byte[] Key)
		{
			RC4State rC4State = new RC4State();
			rC4State.Setup(Key);
			byte[] array = this.m_objMemStream.ToArray();
			rC4State.Crypt(array);
			this.Set(array);
			return 0;
		}
		public int Encrypt(PdfStream Key, int nLen)
		{
			byte[] sourceArray = Key.m_objMemStream.ToArray();
			byte[] array = new byte[nLen];
			Array.Copy(sourceArray, array, nLen);
			return this.Encrypt(array);
		}
		public void PadString(string Password)
		{
			byte[] array = new byte[32];
			int num = (Password == null) ? 0 : Password.Length;
			if (num > 32)
			{
				num = 32;
			}
			if (num > 0)
			{
				Array.Copy(Encoding.UTF8.GetBytes(Password), array, num);
			}
			for (int i = num; i < 32; i++)
			{
				array[i] = this.Padding[i - num];
			}
			this.Set(array);
		}
		public void MD5FromPaddedString(string Password, int nIterations)
		{
			byte[] array = new byte[16];
			PdfStream pdfStream = new PdfStream();
			pdfStream.PadString(Password);
			PdfHash pdfHash = new PdfHash();
			pdfHash.Compute(pdfStream);
			Array.Copy(pdfHash.m_objMemStream.ToArray(), array, pdfHash.Length);
			if (nIterations > 0)
			{
				PdfHash pdfHash2 = new PdfHash();
				pdfHash2.Set(pdfHash);
				for (int i = 0; i < nIterations; i++)
				{
					pdfHash2.HashItself();
				}
				Array.Copy(pdfHash2.m_objMemStream.ToArray(), array, pdfHash2.Length);
			}
			this.Set(array);
		}
		public void EscapeSymbols()
		{
			int num = 0;
			this.m_objMemStream.Position = 0L;
			int num2;
			while ((num2 = this.m_objMemStream.ReadByte()) >= 0)
			{
				if (num2 == 40 || num2 == 41 || num2 == 92 || num2 == 13)
				{
					num++;
				}
			}
			byte[] array = new byte[this.Length + num];
			int num3 = 0;
			this.m_objMemStream.Position = 0L;
			while ((num2 = this.m_objMemStream.ReadByte()) >= 0)
			{
				if (num2 == 40 || num2 == 41 || num2 == 92 || num2 == 13)
				{
					array[num3++] = 92;
				}
                array[num3++] = num2 == 13 ? (byte)114 : (byte)num2;
			}
			this.Set(array);
		}
		public PdfStream()
		{
			this.m_objMemStream = new MemoryStream(0);
			this.m_nPtr = 0;
		}
		public PdfStream(int nSize)
		{
			this.m_objMemStream = new MemoryStream(nSize);
			this.m_nPtr = 0;
		}
		~PdfStream()
		{
			this.m_objMemStream = null;
		}
		public PdfStream(PdfStream stream)
		{
			MemoryStream memoryStream = new MemoryStream(stream.Length);
			memoryStream.Write(stream.m_objMemStream.ToArray(), 0, stream.Length);
			this.m_objMemStream = memoryStream;
			this.m_nPtr = stream.m_nPtr;
		}
		public void Alloc(int Size)
		{
			this.m_objMemStream = new MemoryStream(Size);
		}
		public byte[] ToBytes()
		{
			return this.m_objMemStream.ToArray();
		}
		public void Set(PdfStream pStream)
		{
			this.Set(pStream.ToBytes(), pStream.Length);
		}
		public void Set(byte[] Buffer)
		{
			this.Set(Buffer, Buffer.Length);
		}
		public void Set(byte[] Buffer, int nLen)
		{
			this.Alloc(Buffer.Length);
			this.m_objMemStream.Seek(0L, SeekOrigin.Begin);
			this.m_objMemStream.Write(Buffer, 0, nLen);
		}
		public int Append(byte[] Buffer)
		{
			return this.Append(Buffer, 0, (Buffer != null) ? Buffer.Length : 0);
		}
		public int Append(byte[] Buffer, int nStart, int nCount)
		{
			if (nCount <= 0)
			{
				return 0;
			}
			if (this.m_objMemStream == null)
			{
				this.Alloc(nCount);
			}
			while (this.m_objMemStream.Length + (long)nCount > (long)this.m_objMemStream.Capacity)
			{
				this.m_objMemStream.Capacity = 2 * this.m_objMemStream.Capacity + nCount;
			}
			this.m_objMemStream.Seek(this.m_objMemStream.Length, SeekOrigin.Begin);
			this.m_objMemStream.Write(Buffer, nStart, nCount);
			return (int)this.m_objMemStream.Length;
		}
		public int Append(PdfStream stream)
		{
			return this.Append(stream.ToBytes());
		}
		public int Append(string str)
		{
			return this.Append(Encoding.UTF8.GetBytes(str));
		}
		public int AppendSpace()
		{
			return this.Append(new byte[]
			{
				32
			});
		}
		public bool Compare(int nIndex, PdfStream s, int n2ndIndex, int nBytes)
		{
			byte[] array = this.ToBytes();
			byte[] array2 = s.ToBytes();
			int num = nIndex;
			int num2 = n2ndIndex;
			int num3 = 0;
			while (num < array.Length && num2 < array2.Length && num3 < nBytes)
			{
				if (array[num] != array2[num2])
				{
					return false;
				}
				num++;
				num2++;
				num3++;
			}
			return true;
		}
		public int Checksum()
		{
			uint num = 0u;
			int i = 0;
			this.m_objMemStream.Seek(0L, SeekOrigin.Begin);
			int num2 = (this.Length + 3 & -4) / 4;
			byte[] array = new byte[4];
			while (i < num2)
			{
				array[0] = (array[1] = (array[2] = (array[3] = 0)));
				this.m_objMemStream.Read(array, 0, 4);
				num += BitConverter.ToUInt32(array, 0);
				i++;
			}
			return (int)num;
		}
		public void Set(string HexStr)
		{
            int length = HexStr.Length;
            this.Alloc(length / 2);
            int index = 0;
            while (index < length)
            {
                byte num1 = (byte)HexStr[index];
                byte num2 = (byte)HexStr[index + 1];
                byte num3 = (int)num1 < 97 || (int)num1 > 102 ? ((int)num1 < 65 || (int)num1 > 70 ? (byte)((uint)num1 - 48U) : (byte)(10 + (int)num1 - 65)) : (byte)(10 + (int)num1 - 97);
                byte num4 = (int)num2 < 97 || (int)num2 > 102 ? ((int)num2 < 65 || (int)num2 > 70 ? (byte)((uint)num2 - 48U) : (byte)(10 + (int)num2 - 65)) : (byte)(10 + (int)num2 - 97);
                this.m_objMemStream.Seek((long)(index / 2), SeekOrigin.Begin);
                this.m_objMemStream.WriteByte((byte)((uint)num3 * 16U + (uint)num4));
                index += 2;
            }
		}
		public bool ContainsEOL()
		{
			if (this.m_objMemStream == null)
			{
				return false;
			}
			this.m_objMemStream.Seek(0L, SeekOrigin.Begin);
			int num = 0;
			while (num != -1)
			{
				num = this.m_objMemStream.ReadByte();
				if (num == 13 || num == 10)
				{
					return true;
				}
			}
			return false;
		}
		public void LoadFromFile(string Path)
		{
			FileStream fileStream = null;
			try
			{
				fileStream = File.OpenRead(Path);
				int num = (int)fileStream.Length;
				this.Alloc(num);
				byte[] buffer = new byte[num];
				fileStream.Read(buffer, 0, num);
				this.m_objMemStream.Seek(0L, SeekOrigin.Begin);
				this.m_objMemStream.Write(buffer, 0, num);
			}
			catch (Exception ex)
			{
				AuxException.Throw("Loading text from a file failed: " + ex.Message, PdfErrors._ERROR_OPENFILE);
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
			}
		}
		public bool Equals(ref PdfStream AnotherStream)
		{
			return this.Equals(ref AnotherStream, -1);
		}
		public bool Equals(ref PdfStream AnotherStream, int nLength)
		{
			if (nLength == -1 && this.Length != AnotherStream.Length)
			{
				return false;
			}
			byte[] array = this.m_objMemStream.ToArray();
			byte[] array2 = AnotherStream.m_objMemStream.ToArray();
			int num;
			if (nLength == -1)
			{
				num = this.Length;
			}
			else
			{
				num = Math.Min(array.Length, array2.Length);
				num = Math.Min(num, nLength);
			}
			for (int i = 0; i < num; i++)
			{
				if (array[i] != array2[i])
				{
					return false;
				}
			}
			return true;
		}
		public long AppendChar(byte ch)
		{
			return (long)this.Append(new byte[]
			{
				ch
			});
		}
		public void AppendToString(ref string bstrAppendTo)
		{
			this.AppendToString(ref bstrAppendTo, null);
		}
		public void AppendToString(ref string bstrAppendTo, string bstrDelimiter)
		{
			if (bstrDelimiter != null && bstrAppendTo.Length > 0)
			{
				bstrAppendTo += bstrDelimiter;
			}
			string @string = Encoding.UTF8.GetString(this.m_objMemStream.ToArray());
			bstrAppendTo += @string;
		}
		public override string ToString()
		{
			byte[] array = this.m_objMemStream.ToArray();
			StringBuilder stringBuilder = new StringBuilder();
			byte[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				byte b = array2[i];
				stringBuilder.Append(b.ToString("X2"));
			}
			return stringBuilder.ToString();
		}
		public override PdfObject Copy()
		{
			PdfStream pdfStream = new PdfStream();
			pdfStream.Set(this);
			pdfStream.m_bstrType = this.m_bstrType;
			pdfStream.m_nType = this.m_nType;
			return pdfStream;
		}
		public virtual void TestForAnsi()
		{
		}
		public void Decrypt(PdfStream pKey, int nObjNum, int nGenNum)
		{
			if (pKey == null || nObjNum + nGenNum == 0)
			{
				this.TestForAnsi();
				return;
			}
			byte[] pBuffer = new byte[]
			{
				115,
				65,
				108,
				84
			};
			PdfHash pdfHash = new PdfHash();
			pdfHash.Init();
			pdfHash.Update(pKey);
			byte[] bytes = BitConverter.GetBytes(nObjNum);
			byte[] bytes2 = BitConverter.GetBytes(nGenNum);
			pdfHash.Update(bytes, 3u);
			pdfHash.Update(bytes2, 2u);
			if (pKey.m_nPtr == 1)
			{
				pdfHash.Update(pBuffer, 4u);
			}
			pdfHash.Final();
			int num = pKey.Length + 5;
			if (num > 16)
			{
				num = 16;
			}
			byte[] array = new byte[num];
			Array.Copy(pdfHash.m_objMemStream.ToArray(), array, num);
			PdfStream pdfStream = new PdfStream();
			pdfStream.Set(array);
			if (pKey.m_nPtr == 1)
			{
				PdfAES.Decrypt(pdfStream, num, this);
			}
			else
			{
				this.Encrypt(array);
			}
			this.TestForAnsi();
		}
		public override int WriteOut(PdfOutput pOutput)
		{
			int result = 0;
			pOutput.Write("stream\n", ref result);
			if (this.Length > 0)
			{
				if (pOutput.m_bEncrypt)
				{
					if (pOutput.m_pEncKey.m_nPtr == 1)
					{
						PdfAES.Encrypt(pOutput.m_pEncKey, pOutput.m_pEncKey.Length, this);
					}
					else
					{
						this.Encrypt(pOutput.m_pEncKey.m_objMemStream.ToArray());
					}
				}
				pOutput.Write(this, ref result);
				if (this[this.Length - 1] != 10 || pOutput.m_bEncrypt)
				{
					pOutput.Write("\n", ref result);
				}
			}
			pOutput.Write("endstream\n", ref result);
			return result;
		}
		public static int FindString(byte[] szBuffer, int nFrom, byte[] szLittleStr)
		{
			int num = szLittleStr.Length;
			if (num == 0 || nFrom + num > szBuffer.Length)
			{
				return -1;
			}
			for (int i = nFrom; i <= szBuffer.Length - num; i++)
			{
				while (i < szBuffer.Length - num && szBuffer[i] != szLittleStr[0])
				{
					i++;
				}
				if (Convert.ToBase64String(szBuffer, i, num) == Convert.ToBase64String(szLittleStr))
				{
					return i;
				}
				if (i >= szBuffer.Length - num)
				{
					return -1;
				}
			}
			return -1;
		}
		public static int FindStringIgnoreCase(byte[] szBuffer, int nFrom, byte[] szLittleStr)
		{
			int num = szLittleStr.Length;
			if (num == 0 || nFrom + num > szBuffer.Length)
			{
				return -1;
			}
			int i = nFrom;
			string @string = Encoding.UTF8.GetString(szLittleStr);
			while (i <= szBuffer.Length - num)
			{
				while (i < szBuffer.Length - num && szBuffer[i] != szLittleStr[0])
				{
					i++;
				}
				string strA = new string(Encoding.UTF8.GetChars(szBuffer, i, num));
				if (string.Compare(strA, @string, true) == 0)
				{
					return i;
				}
				if (i >= szBuffer.Length - num)
				{
					return -1;
				}
				i++;
			}
			return -1;
		}
		public static int FindString(char[] szBuffer, int nFrom, char[] szLittleStr)
		{
			int num = szLittleStr.Length;
			if (num == 0 || nFrom + num > szBuffer.Length)
			{
				return -1;
			}
			int i = nFrom;
			string strB = new string(szLittleStr).ToUpper();
			while (i <= szBuffer.Length - num)
			{
				while (szBuffer[i] != szLittleStr[0] && i < szBuffer.Length - num)
				{
					i++;
				}
				string strA = new string(szBuffer, i, num).ToUpper();
				if (string.Compare(strA, strB) == 0)
				{
					return i;
				}
				if (i >= szBuffer.Length - num)
				{
					return -1;
				}
				i++;
			}
			return -1;
		}
		public int getChar()
		{
			if (this.m_nPtr >= this.Length)
			{
				return -1;
			}
			return (int)this[this.m_nPtr++];
		}
		public int lookChar()
		{
			if (this.m_nPtr >= this.Length)
			{
				return -1;
			}
			return (int)this[this.m_nPtr];
		}
		internal int lookChar(int idx)
		{
			if (this.m_nPtr + idx >= this.Length)
			{
				return -1;
			}
			return (int)this[this.m_nPtr + idx];
		}
	}
}
