using System;
namespace Persits.PDF
{
	internal class LZWDecoder
	{
		private PdfString[] stringTable;
		private PdfString m_data = new PdfString();
		private PdfString m_uncompData = new PdfString();
		private int tableIndex;
		private int bitsToGet;
		private int nextData;
		private int nextBits;
		private int[] andTable = new int[4];
		public LZWDecoder()
		{
			this.bitsToGet = 9;
			this.nextData = 0;
			this.nextBits = 0;
			this.andTable[0] = 511;
			this.andTable[1] = 1023;
			this.andTable[2] = 2047;
			this.andTable[3] = 4095;
			this.stringTable = null;
		}
		~LZWDecoder()
		{
			this.stringTable = null;
		}
		public int Decode(PdfStream data, ref PdfStream uncompData)
		{
			data.m_objMemStream.Position = 0L;
			int num = data.m_objMemStream.ReadByte();
			int num2 = data.m_objMemStream.ReadByte();
			if (data.Length > 1 && num == 0 && num2 == 1)
			{
				return -1;
			}
			this.initializeStringTable();
			this.m_data.Set(data);
			this.m_data.m_objMemStream.Position = 0L;
			this.nextData = 0;
			this.nextBits = 0;
			int num3 = 0;
			PdfString pdfString = new PdfString();
			int nextCode;
			while ((nextCode = this.getNextCode()) != 257)
			{
				if (nextCode == 256)
				{
					this.initializeStringTable();
					nextCode = this.getNextCode();
					if (nextCode == 257)
					{
						break;
					}
					this.writeString(this.stringTable[nextCode]);
					num3 = nextCode;
				}
				else
				{
					if (nextCode < this.tableIndex)
					{
						pdfString.Set(this.stringTable[nextCode]);
						this.writeString(pdfString);
						this.addStringToTable(this.stringTable[num3], pdfString[0]);
						num3 = nextCode;
					}
					else
					{
						pdfString.Set(this.stringTable[num3]);
						this.composeString(ref pdfString, pdfString[0]);
						this.writeString(pdfString);
						this.addStringToTable(pdfString);
						num3 = nextCode;
					}
				}
			}
			uncompData.Set(this.m_uncompData);
			return 0;
		}
		private void initializeStringTable()
		{
			this.stringTable = new PdfString[8192];
			for (int i = 0; i < this.stringTable.Length; i++)
			{
				this.stringTable[i] = new PdfString();
			}
			for (int j = 0; j < 256; j++)
			{
				this.stringTable[j].AppendChar((byte)j);
			}
			this.tableIndex = 258;
			this.bitsToGet = 9;
		}
		private void writeString(PdfString objString)
		{
			this.m_uncompData.Append(objString);
		}
		private void addStringToTable(PdfString oldString, byte newString)
		{
			PdfString pdfString = new PdfString();
			pdfString.Set(oldString);
			pdfString.AppendChar(newString);
			this.stringTable[this.tableIndex++].Set(pdfString);
			if (this.tableIndex == 511)
			{
				this.bitsToGet = 10;
				return;
			}
			if (this.tableIndex == 1023)
			{
				this.bitsToGet = 11;
				return;
			}
			if (this.tableIndex == 2047)
			{
				this.bitsToGet = 12;
			}
		}
		private void addStringToTable(PdfString objString)
		{
			this.stringTable[this.tableIndex++].Set(objString);
			if (this.tableIndex == 511)
			{
				this.bitsToGet = 10;
				return;
			}
			if (this.tableIndex == 1023)
			{
				this.bitsToGet = 11;
				return;
			}
			if (this.tableIndex == 2047)
			{
				this.bitsToGet = 12;
			}
		}
		private void composeString(ref PdfString oldString, byte newString)
		{
			oldString.AppendChar(newString);
		}
		private int getNextCode()
		{
			if (this.m_data.m_objMemStream.Position >= (long)this.m_data.Length)
			{
				return 257;
			}
			this.nextData = (this.nextData << 8 | (this.m_data.m_objMemStream.ReadByte() & 255));
			this.nextBits += 8;
			if (this.nextBits < this.bitsToGet)
			{
				if (this.m_data.m_objMemStream.Position >= (long)this.m_data.Length)
				{
					return 257;
				}
				this.nextData = (this.nextData << 8 | (this.m_data.m_objMemStream.ReadByte() & 255));
				this.nextBits += 8;
			}
			int result = this.nextData >> this.nextBits - this.bitsToGet & this.andTable[this.bitsToGet - 9];
			this.nextBits -= this.bitsToGet;
			return result;
		}
	}
}
