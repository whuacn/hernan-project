using System;
using System.Collections.Generic;
namespace Persits.PDF
{
	internal class PdfXRef : PdfObjectList
	{
		public int m_nMaxNum;
		public int m_nLastDeletedObjNum;
		public PdfXRef()
		{
			this.m_nMaxNum = 0;
			this.m_nLastDeletedObjNum = 0;
		}
		private PdfXRefEntry BinarySearch(long nObjNumber, byte cType, ref bool pFound)
		{
			pFound = false;
			int count = this.m_arrItems.Count;
			if (count == 0)
			{
				return null;
			}
			int num = 0;
			int num2 = count - 1;
			PdfXRefEntry pdfXRefEntry;
			while (true)
			{
				int num3 = (num2 + num) / 2;
				pdfXRefEntry = (PdfXRefEntry)this.m_arrItems[num3];
				if ((long)pdfXRefEntry.m_nObjNumber == nObjNumber)
				{
					break;
				}
				if (num2 <= num)
				{
					goto Block_4;
				}
				if (nObjNumber < (long)pdfXRefEntry.m_nObjNumber)
				{
					num2 = num3 - 1;
				}
				else
				{
					num = num3 + 1;
				}
			}
			if (pdfXRefEntry.m_cType == cType)
			{
				pFound = true;
				return pdfXRefEntry;
			}
			return null;
			Block_4:
			return null;
		}
		public void AddEntry(int nOffset, int nObjNumber, int nGenNumber)
		{
			this.AddEntry(nOffset, nObjNumber, nGenNumber, 110);
		}
		public void AddEntry(int nOffset, int nObjNumber, int nGenNumber, byte cType)
		{
			PdfXRefEntry pdfXRefEntry = new PdfXRefEntry();
			pdfXRefEntry.m_nGenNumber = nGenNumber;
			pdfXRefEntry.m_nObjNumber = nObjNumber;
			pdfXRefEntry.m_nOffset = nOffset;
			pdfXRefEntry.m_cType = cType;
			if (cType == 102)
			{
				pdfXRefEntry.m_nOffset = 0;
				bool flag = false;
				pdfXRefEntry.m_nGenNumber++;
				PdfXRefEntry pdfXRefEntry2;
				if ((pdfXRefEntry2 = this.BinarySearch((long)this.m_nLastDeletedObjNum, cType, ref flag)) != null)
				{
					pdfXRefEntry2.m_nOffset = nObjNumber;
				}
				if (!flag && this.m_nLastDeletedObjNum == 0)
				{
					PdfXRefEntry pdfXRefEntry3 = new PdfXRefEntry();
					pdfXRefEntry3.m_nGenNumber = 65535;
					pdfXRefEntry3.m_nObjNumber = 0;
					pdfXRefEntry3.m_nOffset = nObjNumber;
					pdfXRefEntry3.m_cType = 102;
					this.m_arrItems.Insert(0, pdfXRefEntry3);
				}
				this.m_nLastDeletedObjNum = nObjNumber;
			}
			bool flag2 = false;
			int num = 0;
			foreach (PdfObject current in this.m_arrItems)
			{
				PdfXRefEntry pdfXRefEntry2 = (PdfXRefEntry)current;
				if (pdfXRefEntry2.m_nObjNumber == nObjNumber)
				{
					if (pdfXRefEntry2.m_cType == 102 && cType == 110)
					{
						pdfXRefEntry2.m_nGenNumber = nGenNumber;
						pdfXRefEntry2.m_nOffset = nOffset;
					}
					return;
				}
				if (pdfXRefEntry2.m_nObjNumber > nObjNumber)
				{
					this.m_arrItems.Insert(num, pdfXRefEntry);
					flag2 = true;
					break;
				}
				num++;
			}
			if (!flag2)
			{
				this.m_arrItems.Add(pdfXRefEntry);
			}
			if (nObjNumber > this.m_nMaxNum)
			{
				this.m_nMaxNum = nObjNumber;
			}
		}
		public void AddEntryNewStyle(int Value1, int Value2, int Value3)
		{
			if (Value1 == 0)
			{
				this.AddEntry(Value1, Value2, Value3, 102);
			}
		}
		public int GetOffset(int nObjNumber, int nGenNumber)
		{
			foreach (PdfObject current in this.m_arrItems)
			{
				PdfXRefEntry pdfXRefEntry = (PdfXRefEntry)current;
				if (pdfXRefEntry.m_nObjNumber == nObjNumber && pdfXRefEntry.m_nGenNumber == nGenNumber)
				{
					return pdfXRefEntry.m_nOffset;
				}
			}
			return 0;
		}
		public int GetOffset(PdfRef pRef)
		{
			if (pRef == null)
			{
				return 0;
			}
			return this.GetOffset(pRef.m_nObjNum, pRef.m_nGenNum);
		}
		public void CheckForHoles()
		{
			List<int> list = new List<int>();
			int i = 0;
			int nObjNumber = ((PdfXRefEntry)this.m_arrItems[0]).m_nObjNumber;
			while (i < this.m_arrItems.Count)
			{
				PdfXRefEntry pdfXRefEntry = (PdfXRefEntry)this.m_arrItems[i];
				if (pdfXRefEntry.m_nObjNumber > nObjNumber + 1)
				{
					for (int j = nObjNumber + 1; j <= pdfXRefEntry.m_nObjNumber - 1; j++)
					{
						list.Add(j);
					}
				}
				nObjNumber = pdfXRefEntry.m_nObjNumber;
				i++;
			}
			foreach (int current in list)
			{
				this.AddEntry(0, current, 0, 102);
			}
		}
		public override int WriteOut(PdfOutput pOutput)
		{
			int result = 0;
			if (this.m_arrItems.Count <= 0)
			{
				return result;
			}
			pOutput.Write("xref\n", ref result);
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			int i = 0;
			int num = -1;
			int num2 = 0;
			int nObjNumber = ((PdfXRefEntry)this.m_arrItems[i]).m_nObjNumber;
			while (i < this.m_arrItems.Count)
			{
				PdfXRefEntry pdfXRefEntry = (PdfXRefEntry)this.m_arrItems[i];
				if (num2 > 0 && pdfXRefEntry.m_nObjNumber > num + 1)
				{
					list.Add(nObjNumber);
					list2.Add(num2);
					nObjNumber = pdfXRefEntry.m_nObjNumber;
					num2 = 1;
				}
				else
				{
					num2++;
				}
				num = pdfXRefEntry.m_nObjNumber;
				i++;
			}
			list.Add(nObjNumber);
			list2.Add(num2);
			i = 0;
			for (int j = 0; j < list.Count; j++)
			{
				string objString = string.Format("{0} {1}\n", list[j], list2[j]);
				pOutput.Write(objString, ref result);
				int k = list2[j];
				while (k > 0)
				{
					this.m_arrItems[i].WriteOut(pOutput);
					k--;
					i++;
				}
			}
			return result;
		}
		public override PdfObject Copy()
		{
			return null;
		}
		public int NextObjNumber()
		{
			if (this.m_arrItems.Count == 0)
			{
				return 1;
			}
			PdfObject pdfObject = this.m_arrItems[this.m_arrItems.Count - 1];
			return ((PdfXRefEntry)pdfObject).m_nObjNumber + 1;
		}
		public void GenerateCompressedXRef(PdfDict pXRefDict, PdfStream pStream)
		{
			PdfArray pdfArray = new PdfArray("Index");
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			int i = 0;
			int num = -1;
			int num2 = 0;
			int nObjNumber = ((PdfXRefEntry)this.m_arrItems[i]).m_nObjNumber;
			while (i < this.m_arrItems.Count)
			{
				PdfXRefEntry pdfXRefEntry = (PdfXRefEntry)this.m_arrItems[i];
				if (num2 > 0 && pdfXRefEntry.m_nObjNumber > num + 1)
				{
					list.Add(nObjNumber);
					list2.Add(num2);
					nObjNumber = pdfXRefEntry.m_nObjNumber;
					num2 = 1;
				}
				else
				{
					num2++;
				}
				num = pdfXRefEntry.m_nObjNumber;
				i++;
			}
			list.Add(nObjNumber);
			list2.Add(num2);
			i = 0;
			for (int j = 0; j < list2.Count; j++)
			{
				pdfArray.Add(new PdfNumber(null, (double)list[j]));
				pdfArray.Add(new PdfNumber(null, (double)list2[j]));
				int k = list2[j];
				while (k > 0)
				{
					PdfXRefEntry pdfXRefEntry = (PdfXRefEntry)this.m_arrItems[i];
					pStream.AppendChar(2);
                    pStream.AppendChar((int)pdfXRefEntry.m_cType == 110 ? (byte)1 : (byte)0);
					int nOffset = pdfXRefEntry.m_nOffset;
					int nGenNumber = pdfXRefEntry.m_nGenNumber;
					byte[] bytes = BitConverter.GetBytes(nOffset);
					byte[] bytes2 = BitConverter.GetBytes(nGenNumber);
					pStream.Append(new byte[]
					{
						bytes[3],
						bytes[2],
						bytes[1],
						bytes[0]
					});
					pStream.Append(new byte[]
					{
						bytes2[3],
						bytes2[2],
						bytes2[1],
						bytes2[0]
					});
					k--;
					i++;
				}
			}
			int num3 = 9;
			byte[] array = pStream.ToBytes();
			for (int l = pStream.Length - 1; l >= 0; l--)
			{
                byte b = l - num3 - 1 < 0 ? (byte)0 : array[l - num3 - 1];
				if (l % (num3 + 1) == 0)
				{
					b = 0;
				}
				int num4 = (int)(array[l] - b);
				if (num4 < 0)
				{
					num4 += 256;
				}
				array[l] = (byte)num4;
			}
			pStream.Set(array);
			pStream.EncodeFlate();
			pXRefDict.Add(new PdfName("Filter", "FlateDecode"));
			PdfArray pdfArray2 = new PdfArray("W");
			pdfArray2.Add(new PdfNumber(null, 1.0));
			pdfArray2.Add(new PdfNumber(null, 4.0));
			pdfArray2.Add(new PdfNumber(null, 4.0));
			pXRefDict.Add(pdfArray2);
			PdfDict pdfDict = new PdfDict("DecodeParms");
			pdfDict.Add(new PdfNumber("Columns", (double)num3));
			pdfDict.Add(new PdfNumber("Predictor", 12.0));
			pXRefDict.Add(pdfDict);
			pXRefDict.Add(pdfArray);
		}
	}
}
