using System;
namespace Persits.PDF
{
	internal class PdfLZW
	{
		private const int TABLESIZE = 4097;
		private byte[] m_szStrings = new byte[4097];
		private short[] m_nLengths = new short[4097];
		private byte[] m_data;
		private int m_nTableIndex;
		private int m_nBitsToGet;
		private int m_nBytePointer;
		private int m_nNextData;
		private int m_nNextBits;
		private int[] andTable = new int[4];
		public bool m_bPredictor;
		private byte[] m_pCharTable;
		private int[] m_pPrevTable;
		private byte[] m_pStack;
		private byte m_chFirstChar;
		private PdfStream m_objOut = new PdfStream();
		public PdfLZW()
		{
			this.andTable[0] = 511;
			this.andTable[1] = 1023;
			this.andTable[2] = 2047;
			this.andTable[3] = 4095;
			this.m_bPredictor = false;
			this.Reset();
			this.m_pCharTable = new byte[4097];
			this.m_pPrevTable = new int[4097];
			this.m_pStack = new byte[4097];
		}
		~PdfLZW()
		{
			this.m_pStack = null;
			this.m_pPrevTable = null;
			this.m_pCharTable = null;
		}
		private void Reset()
		{
			this.m_nBitsToGet = 9;
			this.m_nNextData = 0;
			this.m_nNextBits = 0;
			this.m_objOut = new PdfStream();
		}
		private void InitializeStringTable()
		{
			for (int i = 0; i < 4097; i++)
			{
				if (i < 256)
				{
					this.m_pCharTable[i] = (byte)i;
					this.m_pPrevTable[i] = 0;
				}
				else
				{
					this.m_pCharTable[i] = 0;
				}
			}
			this.m_nTableIndex = 258;
			this.m_nBitsToGet = 9;
		}
		private int GetNextCode()
		{
			if (this.m_nBytePointer >= this.m_data.Length)
			{
				return 257;
			}
			this.m_nNextData = (this.m_nNextData << 8 | (int)(this.m_data[this.m_nBytePointer++] & 255));
			this.m_nNextBits += 8;
			if (this.m_nNextBits < this.m_nBitsToGet)
			{
				if (this.m_nBytePointer >= this.m_data.Length)
				{
					return 257;
				}
				this.m_nNextData = (this.m_nNextData << 8 | (int)(this.m_data[this.m_nBytePointer++] & 255));
				this.m_nNextBits += 8;
			}
			int result = this.m_nNextData >> this.m_nNextBits - this.m_nBitsToGet & this.andTable[this.m_nBitsToGet - 9];
			this.m_nNextBits -= this.m_nBitsToGet;
			return result;
		}
		public int Decode2(byte[] data, ref byte[] uncompData, int nWidth, int nHeight, int nSamplesPerPixel)
		{
			if (data.Length > 1 && data[0] == 0 && data[1] == 1)
			{
				return -1;
			}
			this.Reset();
			this.InitializeStringTable();
			this.m_data = data;
			this.m_nBytePointer = 0;
			this.m_nNextData = 0;
			this.m_nNextBits = 0;
			int nCode = 0;
			int nextCode;
			while ((nextCode = this.GetNextCode()) != 257)
			{
				if (nextCode == 256)
				{
					this.InitializeStringTable();
					nextCode = this.GetNextCode();
					if (nextCode == 257)
					{
						break;
					}
					this.WriteString2(nextCode);
					nCode = nextCode;
				}
				else
				{
					if (nextCode < this.m_nTableIndex)
					{
						this.WriteString2(nextCode);
						this.AddStringToTable2(nCode);
						nCode = nextCode;
					}
					else
					{
						this.AddStringToTable2(nCode);
						this.WriteString2(nextCode);
						nCode = nextCode;
					}
				}
			}
			byte[] array = this.m_objOut.ToBytes();
			if (this.m_bPredictor)
			{
				for (int i = 0; i < nHeight; i++)
				{
					int num = nSamplesPerPixel * (i * nWidth + 1);
					for (int j = nSamplesPerPixel; j < nWidth * nSamplesPerPixel; j++)
					{
						byte[] expr_D1_cp_0 = array;
						int expr_D1_cp_1 = num;
						expr_D1_cp_0[expr_D1_cp_1] += array[num - nSamplesPerPixel];
						num++;
					}
				}
			}
			uncompData = new byte[array.Length];
			Array.Copy(array, uncompData, array.Length);
			return 0;
		}
		private void WriteString2(int nCode)
		{
			int num = 4096;
			while (true)
			{
				this.m_pStack[num--] = this.m_pCharTable[nCode];
				if (nCode < 256)
				{
					break;
				}
				nCode = this.m_pPrevTable[nCode];
			}
			this.m_chFirstChar = this.m_pStack[num + 1];
			byte[] array = new byte[4097 - num - 1];
			Array.Copy(this.m_pStack, num + 1, array, 0, 4097 - num - 1);
			this.m_objOut.Append(array);
		}
		private void AddStringToTable2(int nCode)
		{
			this.m_pCharTable[this.m_nTableIndex] = this.m_chFirstChar;
			this.m_pPrevTable[this.m_nTableIndex] = nCode;
			this.m_nTableIndex++;
			if (this.m_nTableIndex == 511)
			{
				this.m_nBitsToGet = 10;
				return;
			}
			if (this.m_nTableIndex == 1023)
			{
				this.m_nBitsToGet = 11;
				return;
			}
			if (this.m_nTableIndex == 2047)
			{
				this.m_nBitsToGet = 12;
			}
		}
	}
}
