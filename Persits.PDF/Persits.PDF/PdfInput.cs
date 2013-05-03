using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
namespace Persits.PDF
{
	internal class PdfInput
	{
		internal const int BUFFER_SIZE = 1024;
		public Stream m_objIOStream;
		public int m_nSize;
		public bool m_bMacBinary;
		public int m_nMacBinarySize;
		public bool m_bParseAllPagesCalled;
		public bool m_bParseAcroFormCalled;
		public bool m_bParseMetaDataCalled;
		public bool m_bParseOutlinesCalled;
		public string m_bstrPassword;
		private bool m_bBinary;
		private PdfDocument m_pDoc;
		internal bool m_bPasswordAuthenticated;
		internal StringBuilder m_bstrValue = new StringBuilder();
		internal int m_nReference;
		internal int m_nGeneration;
		internal byte m_c;
		internal bool m_bIgnoreDef;
		internal byte[] m_szBuffer;
		internal int m_dwStartByte;
		internal int m_dwBytesRead;
		internal int m_dwPtr;
		internal PdfStream m_objObjStm = new PdfStream();
		internal List<int> m_arrObjStmList = new List<int>();
		internal bool m_bCompressedObjectsExist;
		internal bool m_bInsideCompressedObjectExtension;
		private bool[] delims = new bool[]
		{
			true,
			true,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			true,
			true,
			false,
			true,
			true,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			true,
			false,
			false,
			false,
			false,
			true,
			false,
			false,
			true,
			true,
			false,
			false,
			false,
			false,
			false,
			true,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			true,
			false,
			true,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			true,
			false,
			true,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			false
		};
		private PdfInput(PdfDocument pDoc, string Password)
		{
			this.m_pDoc = pDoc;
			this.m_bstrPassword = Password;
			this.m_bParseAllPagesCalled = false;
			this.m_bParseAcroFormCalled = false;
			this.m_bParseMetaDataCalled = false;
			this.m_bParseOutlinesCalled = false;
			this.m_bMacBinary = false;
		}
		public PdfInput(string Path, PdfDocument pDoc, string Password) : this(pDoc, Password)
		{
			try
			{
				this.m_objIOStream = new FileStream(Path, FileMode.Open, FileAccess.Read);
			}
			catch (Exception ex)
			{
				AuxException.Throw(ex.Message, PdfErrors._ERROR_INPUT_CONSTRUCTOR);
			}
			this.m_nSize = (int)this.m_objIOStream.Length;
			this.m_bBinary = false;
			this.m_szBuffer = new byte[1024];
		}
		public PdfInput(byte[] pBlob, PdfDocument pDoc, string Password) : this(pDoc, Password)
		{
			this.m_objIOStream = new MemoryStream(pBlob);
			this.m_nSize = pBlob.Length;
			this.m_bBinary = true;
			this.m_dwBytesRead = this.m_nSize;
			this.m_szBuffer = pBlob;
		}
		public PdfInput(ref PdfStream stream, PdfDocument pDoc, string Password) : this(pDoc, Password)
		{
			this.m_szBuffer = new byte[stream.Length + 1];
			Array.Copy(stream.ToBytes(), this.m_szBuffer, stream.Length);
			this.m_nSize = stream.Length + 1;
			this.m_szBuffer[this.m_nSize - 1] = 32;
			this.m_objIOStream = new MemoryStream(this.m_szBuffer);
			this.m_bBinary = true;
			this.m_dwBytesRead = this.m_nSize;
		}
		~PdfInput()
		{
			this.Close();
		}
		public void Close()
		{
			if (this.m_objIOStream != null)
			{
				this.m_objIOStream.Dispose();
			}
			this.m_objIOStream = null;
		}
		internal void Parse()
		{
			this.ParsePDFVersion();
			this.ParseXRef();
			this.ParseCatalog();
		}
		private void ParsePDFVersion()
		{
			this.SetPointer(0);
			string @string;
			if (this.m_szBuffer[0] == 0)
			{
				int num = this.StrStrLast(this.m_szBuffer, this.m_szBuffer.Length, Encoding.UTF8.GetBytes("%PDF-"));
				if (num == -1)
				{
					this.Throw("File starts with 0: PDF file signature not found.");
				}
				this.m_bMacBinary = true;
				this.m_nMacBinarySize = num;
				@string = Encoding.UTF8.GetString(this.m_szBuffer, num, 20);
				this.m_pDoc.m_bIsMac = true;
				this.m_pDoc.m_nMacBinarySize = this.m_nMacBinarySize;
				if (this.m_bBinary)
				{
					Array.Copy(this.m_szBuffer, this.m_nMacBinarySize, this.m_szBuffer, 0, this.m_dwBytesRead - this.m_nMacBinarySize);
					this.m_dwBytesRead -= this.m_nMacBinarySize;
					this.m_nSize -= this.m_nMacBinarySize;
					this.m_bMacBinary = false;
					this.m_pDoc.m_nMacBinarySize = 0;
				}
			}
			else
			{
				@string = Encoding.UTF8.GetString(this.m_szBuffer, 0, 20);
			}
			if (string.Compare(@string, 0, "%PDF-", 0, 5, true) != 0)
			{
				this.Throw("PDF file signature not found.");
			}
			this.m_pDoc.m_bstrOriginalVersion = @string.Substring(5, 3);
		}
		private void ParseXRef()
		{
			if (!this.m_bBinary)
			{
				this.m_dwBytesRead = 0;
				this.m_dwStartByte = 0;
			}
			int pointer = (this.m_nSize - 1024 > 0) ? (this.m_nSize - 1024) : 0;
			this.SetPointer(pointer);
			int num = this.StrStrLast(this.m_szBuffer, this.m_bBinary ? this.m_nSize : 1024, "startxref");
			if (num == -1)
			{
				this.Throw("PDF startxref not found.");
			}
			this.SetPointer(this.m_dwStartByte + num);
			this.ParseNextWord("startxref");
			int num2 = this.ParseNextInt("startxref is not followed by a number.");
			this.m_pDoc.m_dwLastXrefPos = num2;
			this.SetPointer(num2);
			string a = "";
			int num3 = 0;
			while (true)
			{
				string a2 = this.ParseNextWord("xref", false);
				if (!(a2 == "xref"))
				{
					break;
				}
				while (true)
				{
					int num4 = this.ParseNextInt(null, true, ref a);
					if (num4 == -999)
					{
						if (a == "trailer")
						{
							break;
						}
						this.Throw("First object number not found after xref.");
					}
					int num5 = this.ParseNextInt("Number of entries not found after xref.");
					for (int i = 0; i < num5; i++)
					{
						int num6 = this.ParseNextInt("Offset value in an xref entry not found. ");
						int num7 = this.ParseNextInt("Generation value in an xref entry not found. ");
						string text = this.ParseNextWord(null);
						if (i == 0 && text == "f" && num4 == 1 && num6 == 0 && num7 == 65535)
						{
							num4 = 0;
						}
						if (text == "n" || text == "f")
						{
							this.m_pDoc.m_objExistingXRef.AddEntry(num6, i + num4, num7, (byte)text[0]);
						}
						else
						{
							this.Throw("n or f value expected in an xref entry. ");
						}
					}
				}
				PdfObject pdfObject = this.ParseObject(null, 0, 0);
				if (pdfObject == null || pdfObject.m_nType != enumType.pdfDictionary)
				{
					this.Throw("A trailer dictionary could not be read.");
				}
				PdfObject objectByName = ((PdfDict)pdfObject).GetObjectByName("ID");
				if (objectByName != null && objectByName.m_nType == enumType.pdfArray)
				{
					PdfArray pdfArray = (PdfArray)objectByName;
					if (pdfArray.Size > 0)
					{
						PdfObject @object = pdfArray.GetObject(0);
						if (@object.m_nType == enumType.pdfString)
						{
							this.m_pDoc.m_objID.Set((PdfStream)@object);
							this.m_pDoc.m_objID2.SetRandomID();
						}
					}
				}
				PdfObject objectByName2 = ((PdfDict)pdfObject).GetObjectByName("Prev");
				PdfObject objectByName3 = ((PdfDict)pdfObject).GetObjectByName("Encrypt");
				if (num3 == 0)
				{
					this.m_pDoc.m_pTrailer = (PdfDict)pdfObject;
				}
				if (objectByName3 != null)
				{
					this.ParseEncrypt(objectByName3);
				}
				num3++;
				if (objectByName2 == null)
				{
					return;
				}
				if (objectByName2.m_nType != enumType.pdfNumber)
				{
					this.Throw("The /Prev entry of a trailer dictionary is not a number.");
				}
				int num8 = (int)((PdfNumber)objectByName2).m_fValue;
				this.SetPointer(num8);
				num2 = num8;
			}
			this.ParseXRefStream(num2);
		}
		private void ParseXRefStream(int nOffset)
		{
			int num = 0;
			List<int> list = new List<int>();
			while (true)
			{
				this.SetPointer(nOffset);
				int nObjNum;
				int nGenNum;
				this.ParseObjectHeader(out nObjNum, out nGenNum);
				PdfObject pdfObject = this.ParseObject(null, nObjNum, nGenNum);
				if (pdfObject.m_nType != enumType.pdfDictionary)
				{
					this.Throw("XRef Stream dictionary not found. ", PdfErrors._ERROR_INPUT_STREAM);
				}
				int item = 0;
				PdfObject objectByName = ((PdfDict)pdfObject).GetObjectByName("Size");
				if (objectByName != null && objectByName.m_nType == enumType.pdfNumber)
				{
					item = (int)((PdfNumber)objectByName).m_fValue;
				}
				else
				{
					this.Throw("/Size entry not found in XRef Stream dictionary. ", PdfErrors._ERROR_INPUT_STREAM);
				}
				if (this.m_pDoc.m_objID.Length == 0)
				{
					PdfObject objectByName2 = ((PdfDict)pdfObject).GetObjectByName("ID");
					if (objectByName2 != null && objectByName2.m_nType == enumType.pdfArray)
					{
						PdfArray pdfArray = (PdfArray)objectByName2;
						if (pdfArray.Size > 0)
						{
							PdfObject @object = pdfArray.GetObject(0);
							if (@object.m_nType == enumType.pdfString)
							{
								this.m_pDoc.m_objID.Set((PdfStream)@object);
								this.m_pDoc.m_objID2.SetRandomID();
							}
						}
					}
				}
				List<int> list2 = new List<int>();
				List<int> list3 = new List<int>();
				PdfObject objectByName3 = ((PdfDict)pdfObject).GetObjectByName("Index");
				if (objectByName3 != null && objectByName3.m_nType == enumType.pdfArray)
				{
					PdfArray pdfArray2 = (PdfArray)objectByName3;
					for (int i = 0; i < pdfArray2.Size / 2; i++)
					{
						list2.Add((int)((PdfNumber)pdfArray2.GetObject(i * 2)).m_fValue);
						list3.Add((int)((PdfNumber)pdfArray2.GetObject(i * 2 + 1)).m_fValue);
					}
				}
				else
				{
					list2.Add(0);
					list3.Add(item);
				}
				int[] array = new int[3];
				int[] array2 = array;
				int num2 = 0;
				PdfObject objectByName4 = ((PdfDict)pdfObject).GetObjectByName("W");
				if (objectByName4 != null && objectByName4.m_nType == enumType.pdfArray)
				{
					for (int j = 0; j < 3; j++)
					{
						PdfObject object2 = ((PdfArray)objectByName4).GetObject(j);
						if (object2 != null && object2.m_nType == enumType.pdfNumber)
						{
							array2[j] = (int)((PdfNumber)object2).m_fValue;
						}
						if (array2[j] > 0)
						{
							num2++;
						}
					}
				}
				else
				{
					this.Throw("Error decoding compressed XRef stream. /W array not found.", PdfErrors._ERROR_INPUT_STREAM);
				}
				PdfStream pdfStream = new PdfStream();
				this.ParseStream(null, ref pdfStream, nOffset, false);
				byte[] array3 = pdfStream.ToBytes();
				int num3 = 0;
				int[] array4 = new int[3];
				int[] array5 = new int[3];
				int num4 = 0;
				int num5 = list2[num4];
				for (int k = 0; k < pdfStream.Length; k++)
				{
					if (k % (array2[0] + array2[1] + array2[2]) == 0)
					{
						num3 = 0;
						array4[0] = array2[0];
						array4[1] = array2[1];
						array4[2] = array2[2];
						array5[0] = (array5[1] = (array5[2] = 0));
					}
					array5[num3] = 256 * array5[num3] + (int)array3[k];
					array4[num3]--;
					if (array4[num3] == 0)
					{
						num3++;
					}
					if (num3 >= num2)
					{
						if (array5[0] == 2)
						{
							list.Add(array5[1]);
						}
						else
						{
                            this.m_pDoc.m_objExistingXRef.AddEntry(array5[1], num5, array5[2], array5[0] == 1 ? (byte)110 : (byte)102);
						}
						List<int> list4;
						int index;
						(list4 = list3)[index = num4] = list4[index] - 1;
						if (list3[num4] <= 0)
						{
							num4++;
							if (num4 < list2.Count)
							{
								num5 = list2[num4];
							}
						}
						else
						{
							num5++;
						}
					}
				}
				PdfObject objectByName5 = ((PdfDict)pdfObject).GetObjectByName("Encrypt");
				if (objectByName5 != null)
				{
					this.ParseEncrypt(objectByName5);
				}
				PdfObject objectByName6 = ((PdfDict)pdfObject).GetObjectByName("Prev");
				if (num == 0)
				{
					this.m_pDoc.m_pTrailer = (PdfDict)pdfObject;
				}
				if (objectByName6 == null || objectByName6.m_nType != enumType.pdfNumber)
				{
					break;
				}
				nOffset = (int)((PdfNumber)objectByName6).m_fValue;
				num++;
			}
			foreach (int current in list)
			{
				this.ParseCompressedObject(current);
			}
			this.m_pDoc.m_bCompressedXRef = true;
		}
		private void ParseCompressedObject(int nObjNumber)
		{
			foreach (int current in this.m_arrObjStmList)
			{
				if (nObjNumber == current)
				{
					return;
				}
			}
			this.m_arrObjStmList.Add(nObjNumber);
			this.m_bCompressedObjectsExist = true;
			int offset = this.m_pDoc.m_objExistingXRef.GetOffset(nObjNumber, 0);
			this.SetPointer(offset);
			int nObjNum;
			int nGenNum;
			this.ParseObjectHeader(out nObjNum, out nGenNum);
			PdfObject pdfObject = this.ParseObject(null, nObjNum, nGenNum);
			if (pdfObject.m_nType != enumType.pdfDictionary)
			{
				this.Throw("ObjStm dictionary not found. ", PdfErrors._ERROR_INPUT_STREAM);
			}
			int num = 0;
			PdfObject objectByName = ((PdfDict)pdfObject).GetObjectByName("N");
			if (objectByName != null && objectByName.m_nType == enumType.pdfNumber)
			{
				num = (int)((PdfNumber)objectByName).m_fValue;
			}
			else
			{
				this.Throw("/N entry not found in ObjStm dictionary. ", PdfErrors._ERROR_INPUT_STREAM);
			}
			int num2 = 0;
			PdfObject objectByName2 = ((PdfDict)pdfObject).GetObjectByName("First");
			if (objectByName2 != null && objectByName2.m_nType == enumType.pdfNumber)
			{
				num2 = (int)((PdfNumber)objectByName2).m_fValue;
			}
			else
			{
				this.Throw("/First entry not found in ObjStm dictionary. ", PdfErrors._ERROR_INPUT_STREAM);
			}
			PdfStream pdfStream = new PdfStream();
			this.ParseStream(null, ref pdfStream, offset);
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			PdfInput pdfInput = new PdfInput(ref pdfStream, this.m_pDoc, null);
			pdfInput.Next();
			for (int i = 0; i < 2 * num; i++)
			{
				PdfObject pdfObject2 = pdfInput.ParseObject(null, 0, 0);
				if (pdfObject2.m_nType == enumType.pdfNumber)
				{
					int item = (int)((PdfNumber)pdfObject2).m_fValue;
					if (i % 2 != 0)
					{
						list2.Add(item);
					}
					else
					{
						list.Add(item);
					}
				}
				else
				{
					this.Throw("Invalid ObjStm stream.", PdfErrors._ERROR_INPUT_STREAM);
				}
			}
			list2.Add(pdfStream.Length - num2);
			for (int j = 0; j < list.Count; j++)
			{
				int num3 = list[j];
				int num4 = list2[j];
				this.m_pDoc.m_objExistingXRef.AddEntry(this.m_nSize + this.m_objObjStm.Length, num3, 0);
				this.m_objObjStm.Append(string.Format("{0} 0 obj\n", num3));
				byte[] array = new byte[list2[j + 1] - list2[j]];
				Array.Copy(pdfStream.ToBytes(), num2 + num4, array, 0, array.Length);
				this.m_objObjStm.Append(array);
				this.m_objObjStm.Append("\nendobj\n\n");
			}
		}
		private void ParseCatalog()
		{
			PdfObject objectByName = this.m_pDoc.m_pTrailer.GetObjectByName("Root");
			if (objectByName != null && objectByName.m_nType == enumType.pdfRef)
			{
				int offset = this.m_pDoc.m_objExistingXRef.GetOffset((PdfRef)objectByName);
				if (offset == 0)
				{
					this.Throw("/Root object cannot be found.");
				}
				this.SetPointer(offset);
				int num;
				int num2;
				this.ParseObjectHeader(out num, out num2);
				PdfObject pdfObject = this.ParseObject(null, num, num2);
				if (pdfObject == null || pdfObject.m_nType != enumType.pdfDictionary)
				{
					this.Throw("A dictionary object expected.");
				}
				this.m_pDoc.m_pCatalogObj = this.m_pDoc.AddExistingIndirectObject(enumIndirectType.pdfIndirectCatalog, num, num2, pdfObject);
			}
			else
			{
				this.Throw("/Root entry not found in the trailer.");
			}
			PdfObject objectByName2 = this.m_pDoc.m_pTrailer.GetObjectByName("Info");
			if (objectByName2 != null && objectByName2.m_nType == enumType.pdfRef)
			{
				int offset2 = this.m_pDoc.m_objExistingXRef.GetOffset((PdfRef)objectByName2);
				if (offset2 == 0)
				{
					this.Throw("/Info object cannot be found.");
				}
				this.SetPointer(offset2);
				int num3;
				int num4;
				this.ParseObjectHeader(out num3, out num4);
				PdfObject pdfObject2 = this.ParseObject(null, num3, num4);
				if (pdfObject2 == null || pdfObject2.m_nType != enumType.pdfDictionary)
				{
					this.Throw("A dictionary object expected.");
				}
				this.m_pDoc.m_pInfoObj = this.m_pDoc.AddExistingIndirectObject(enumIndirectType.pdfIndirectInfo, num3, num4, pdfObject2);
				this.ParseInfoDict((PdfDict)pdfObject2);
				return;
			}
			this.m_pDoc.m_pInfoObj = this.m_pDoc.AddNewIndirectObject(enumIndirectType.pdfIndirectInfo);
			this.m_pDoc.m_pInfoObj.m_bModified = false;
		}
		private void ParseInfoDict(PdfDict pObj)
		{
			string[] array = new string[]
			{
				"Title",
				"Author",
				"Subject",
				"Keywords",
				"Creator",
				"Producer",
				"CreationDate",
				"ModDate"
			};
			string[] array2 = new string[array.Length];
			DateTime[] array3 = new DateTime[2];
			for (int i = 0; i < array.Length; i++)
			{
				PdfObject objectByName = pObj.GetObjectByName(array[i]);
				if (objectByName != null && objectByName.m_nType == enumType.pdfString)
				{
					PdfString pdfString = (PdfString)objectByName;
					if (i < 6)
					{
						array2[i] = pdfString.ToString();
					}
					else
					{
						array3[i - 6] = pdfString.ToDate();
					}
				}
			}
			this.m_pDoc.m_bstrTitle = array2[0];
			this.m_pDoc.m_bstrAuthor = array2[1];
			this.m_pDoc.m_bstrSubject = array2[2];
			this.m_pDoc.m_bstrKeywords = array2[3];
			this.m_pDoc.m_bstrCreator = array2[4];
			this.m_pDoc.m_bstrProducer = array2[5];
			this.m_pDoc.m_dtCreationDate = array3[0];
			this.m_pDoc.m_dtModDate = array3[1];
		}
		internal void ParseAllPages()
		{
			if (this.m_bParseAllPagesCalled)
			{
				return;
			}
			PdfObject objectByName = this.m_pDoc.m_pCatalogObj.m_objAttributes.GetObjectByName("Pages");
			if (objectByName != null && objectByName.m_nType == enumType.pdfRef)
			{
				PdfObjectList pStack = new PdfObjectList();
				this.ParsePages((PdfRef)objectByName, pStack, 0);
			}
			else
			{
				this.Throw("No /Pages entry in the document catalog.");
			}
			this.m_bParseAllPagesCalled = true;
		}
		internal void ParsePages(PdfRef pRoot, PdfObjectList pStack, int nLevel)
		{
			if (pRoot == null)
			{
				return;
			}
			int offset = this.m_pDoc.m_objExistingXRef.GetOffset(pRoot);
			if (offset == 0)
			{
				this.Throw("Object cannot be found.");
			}
			this.SetPointer(offset);
			int nObjNum;
			int nGenNum;
			this.ParseObjectHeader(out nObjNum, out nGenNum);
			if (nObjNum != pRoot.m_nObjNum || nGenNum != pRoot.m_nGenNum)
			{
				nObjNum = pRoot.m_nObjNum;
				nGenNum = pRoot.m_nGenNum;
			}
			PdfObject pdfObject = this.ParseObject(null, nObjNum, nGenNum);
			if (pdfObject == null || pdfObject.m_nType != enumType.pdfDictionary)
			{
				this.Throw("A dictionary object expected.");
			}
			if (nLevel == 0)
			{
				this.m_pDoc.m_pPageRootObj = this.m_pDoc.AddExistingIndirectObject(enumIndirectType.pdfIndirectPageRoot, nObjNum, nGenNum, pdfObject);
			}
			PdfDict pdfDict = (PdfDict)pdfObject;
			PdfObject objectByName = pdfDict.GetObjectByName("Type");
			bool flag = false;
			if (objectByName == null && pdfDict.GetObjectByName("Kids") != null)
			{
				flag = true;
			}
			if (flag || (objectByName != null && objectByName.m_nType == enumType.pdfName && ((PdfName)objectByName).m_bstrName == "Pages"))
			{
				PdfObject pdfObject2 = this.RemoveRef(pdfDict.GetObjectByName("Kids"), enumType.pdfArray);
				if (pdfObject2 != null && pdfObject2.m_nType == enumType.pdfArray)
				{
					PdfArray pdfArray = (PdfArray)pdfObject2;
					for (int i = 0; i < pdfArray.Size; i++)
					{
						PdfObject @object = pdfArray.GetObject(i);
						if (@object != null && @object.m_nType == enumType.pdfRef)
						{
							PdfDict pdfDict2 = (PdfDict)pdfDict.Copy();
							pdfDict2.m_ref.x = nObjNum;
							pdfDict2.m_ref.y = nGenNum;
							pStack.Add(pdfDict2);
							this.ParsePages((PdfRef)@object, pStack, nLevel + 1);
							pStack.RemoveLast();
						}
					}
					return;
				}
				this.Throw("/Kids entry not found or not valid.");
				return;
			}
			else
			{
				if (objectByName != null && objectByName.m_nType == enumType.pdfName && ((PdfName)objectByName).m_bstrName == "Page")
				{
					PdfPages pages = this.m_pDoc.GetPages();
					if (pages == null)
					{
						this.Throw("Obtaining the Pages collection failed.");
					}
					PdfPage pPage = pages.AddExisting(pdfDict, pStack, nObjNum, nGenNum);
					this.ParseResources(pPage, pStack, pdfDict);
					this.ParseAnnots(pPage);
					this.ParseContents(pPage);
					return;
				}
				this.Throw("This object is neither a /Page nor /Pages.");
				return;
			}
		}
		private void ParseXObjects(PdfPage pPage, bool bSpace, int nCodePage)
		{
			PdfDict pdfDict = null;
			if (pPage.m_pXObjectDictObj != null)
			{
				pdfDict = pPage.m_pXObjectDictObj.m_objAttributes;
			}
			else
			{
				if (pPage.m_pResourceObj != null)
				{
					pdfDict = (PdfDict)pPage.m_pResourceObj.m_objAttributes.GetObjectByName("XObject");
				}
				else
				{
					PdfDict pdfDict2 = (PdfDict)pPage.m_pPageObj.m_objAttributes.GetObjectByName("Resources");
					if (pdfDict2 != null)
					{
						pdfDict = (PdfDict)pdfDict2.GetObjectByName("XObject");
					}
				}
			}
			if (pdfDict == null)
			{
				return;
			}
			for (int i = 0; i < pdfDict.Size; i++)
			{
				PdfObject @object = pdfDict.GetObject(i);
				if (@object != null && @object.m_nType == enumType.pdfRef)
				{
					this.ParseXObject(pPage, bSpace, nCodePage, (PdfRef)@object);
				}
			}
		}
		private void ParseXObject(PdfPage pPage, bool bSpace, int nCodePage, PdfRef pRef)
		{
			this.SetPointer(this.m_pDoc.m_objExistingXRef.GetOffset(pRef));
			int num;
			int num2;
			this.ParseObjectHeader(out num, out num2);
			PdfObject pdfObject = this.ParseObject(null, num, num2);
			if (pdfObject.m_nType != enumType.pdfDictionary)
			{
				this.Throw("XObject does not have a dictionary.");
			}
			PdfDict pdfDict = (PdfDict)pdfObject;
			PdfObject objectByName = pdfDict.GetObjectByName("Subtype");
			if (objectByName == null || objectByName.m_nType != enumType.pdfName || !(((PdfName)objectByName).m_bstrName == "Form"))
			{
				return;
			}
			PdfObjectList pStack = new PdfObjectList();
			if (this.m_pDoc.GetPages() == null)
			{
				this.Throw("Obtaining the Pages collection failed (XObject handling).");
			}
			PdfPage pdfPage = new PdfPage();
			pdfPage.m_pDoc = this.m_pDoc;
			pdfPage.m_pPageObj = this.m_pDoc.AddExistingIndirectObject(enumIndirectType.pdfIndirectPage, num, num2, pdfDict);
			this.ParseResources(pdfPage, pStack, pdfDict);
			PdfArray pdfArray = pdfPage.m_pPageObj.AddArray("Contents");
			pdfArray.Add(new PdfRef(pRef));
			this.ParseContents(pdfPage);
			this.ParseContentStreams(pdfPage, bSpace, nCodePage);
			pPage.m_bstrContentText.Append(" ");
			pPage.m_bstrContentText.Append(pdfPage.m_bstrContentText);
		}
		private void ParseResources(PdfPage pPage, PdfObjectList pStack, PdfDict pPageDict)
		{
			PdfDict pdfDict = null;
			PdfObject objectByName = pPageDict.GetObjectByName("Resources");
			bool flag = false;
			if (objectByName == null)
			{
				for (int i = pStack.Size - 1; i >= 0; i--)
				{
					pdfDict = (PdfDict)pStack.GetObject(i);
					objectByName = pdfDict.GetObjectByName("Resources");
					if (objectByName != null)
					{
						flag = true;
						break;
					}
				}
			}
			if (objectByName == null)
			{
				return;
			}
			PdfDict pdfDict2;
			if (objectByName.m_nType == enumType.pdfRef)
			{
				this.SetPointer(this.m_pDoc.m_objExistingXRef.GetOffset((PdfRef)objectByName));
				int num;
				int num2;
				this.ParseObjectHeader(out num, out num2);
				PdfObject pdfObject = this.ParseObject(null, num, num2);
				if (pdfObject.m_nType != enumType.pdfDictionary)
				{
					return;
				}
				pdfDict2 = (PdfDict)pdfObject;
				pPage.m_pResourceObj = this.m_pDoc.AddExistingIndirectObject(enumIndirectType.pdfIndirectResource, num, num2, pdfDict2);
			}
			else
			{
				if (objectByName.m_nType != enumType.pdfDictionary)
				{
					return;
				}
				pdfDict2 = (PdfDict)objectByName;
				if (flag)
				{
					pPage.m_pResourceObj = this.m_pDoc.AddExistingIndirectObject(enumIndirectType.pdfIndirectDict, pdfDict.m_ref.x, pdfDict.m_ref.y, pdfDict);
				}
			}
			PdfObject pdfObject2 = pdfDict2.GetObjectByName("Font");
			if (pdfObject2 != null && pdfObject2.m_nType == enumType.pdfRef)
			{
				this.SetPointer(this.m_pDoc.m_objExistingXRef.GetOffset((PdfRef)pdfObject2));
				int num;
				int num2;
				this.ParseObjectHeader(out num, out num2);
				PdfObject pdfObject3 = this.ParseObject(null, num, num2);
				if (pdfObject3.m_nType == enumType.pdfDictionary)
				{
					pPage.m_pFontDictObj = this.m_pDoc.AddExistingIndirectObject(enumIndirectType.pdfIndirectDict, num, num2, pdfObject3);
					pdfObject2 = pdfObject3;
				}
			}
			if (pdfObject2 != null && pdfObject2.m_nType == enumType.pdfDictionary)
			{
				PdfDict pdfDict3 = (PdfDict)pdfObject2;
				for (int j = 0; j < pdfDict3.Size; j++)
				{
					PdfObject @object = pdfDict3.GetObject(j);
					this.m_pDoc.m_listFontNames.Add(new PdfName(@object.m_bstrType, null));
				}
			}
			PdfObject pdfObject4 = pdfDict2.GetObjectByName("XObject");
			if (pdfObject4 != null && pdfObject4.m_nType == enumType.pdfRef)
			{
				this.SetPointer(this.m_pDoc.m_objExistingXRef.GetOffset((PdfRef)pdfObject4));
				int num;
				int num2;
				this.ParseObjectHeader(out num, out num2);
				PdfObject pdfObject5 = this.ParseObject(null, num, num2);
				if (pdfObject5.m_nType == enumType.pdfDictionary)
				{
					pPage.m_pXObjectDictObj = this.m_pDoc.AddExistingIndirectObject(enumIndirectType.pdfIndirectDict, num, num2, pdfObject5);
					pdfObject4 = pdfObject5;
				}
			}
			if (pdfObject4 != null && pdfObject4.m_nType == enumType.pdfDictionary)
			{
				PdfDict pdfDict4 = (PdfDict)pdfObject4;
				for (int k = 0; k < pdfDict4.Size; k++)
				{
					PdfObject object2 = pdfDict4.GetObject(k);
					this.m_pDoc.m_listXObjectNames.Add(new PdfName(object2.m_bstrType, null));
				}
			}
			PdfObject pdfObject6 = pdfDict2.GetObjectByName("ColorSpace");
			if (pdfObject6 != null && pdfObject6.m_nType == enumType.pdfRef)
			{
				this.SetPointer(this.m_pDoc.m_objExistingXRef.GetOffset((PdfRef)pdfObject6));
				int num;
				int num2;
				this.ParseObjectHeader(out num, out num2);
				PdfObject pdfObject7 = this.ParseObject(null, num, num2);
				if (pdfObject7.m_nType == enumType.pdfDictionary)
				{
					pPage.m_pCSDictObj = this.m_pDoc.AddExistingIndirectObject(enumIndirectType.pdfIndirectDict, num, num2, pdfObject7);
					pdfObject6 = pdfObject7;
				}
			}
			if (pdfObject6 != null && pdfObject6.m_nType == enumType.pdfDictionary)
			{
				PdfDict pdfDict5 = (PdfDict)pdfObject6;
				for (int l = 0; l < pdfDict5.Size; l++)
				{
					PdfObject object3 = pdfDict5.GetObject(l);
					this.m_pDoc.m_listCSNames.Add(new PdfName(object3.m_bstrType, null));
				}
			}
		}
		private void ParseContents(PdfPage pPage)
		{
			PdfObject objectByName = pPage.m_pPageObj.m_objAttributes.GetObjectByName("Contents");
			if (objectByName == null)
			{
				return;
			}
			if (objectByName.m_nType != enumType.pdfRef)
			{
				return;
			}
			this.SetPointer(this.m_pDoc.m_objExistingXRef.GetOffset((PdfRef)objectByName));
			int nObjNum;
			int nGenNum;
			this.ParseObjectHeader(out nObjNum, out nGenNum);
			PdfObject pdfObject = this.ParseObject(null, nObjNum, nGenNum);
			if (pdfObject.m_nType != enumType.pdfArray)
			{
				return;
			}
			PdfArray pdfArray = (PdfArray)((PdfArray)pdfObject).Copy();
			pdfArray.m_bstrType = "Contents";
			pdfArray.m_nItemsPerLine = 5;
			pPage.m_pPageObj.m_objAttributes.Add(pdfArray);
		}
		public void ParseContentStreams(PdfPage pPage, bool bSpace, int nCodePage)
		{
			PdfObject objectByName = pPage.m_pPageObj.m_objAttributes.GetObjectByName("Contents");
			if (objectByName == null)
			{
				return;
			}
			this.ParseFonts(pPage);
			PdfStream pdfStream = new PdfStream();
			if (objectByName.m_nType == enumType.pdfRef)
			{
				this.ParseStream((PdfRef)objectByName, ref pdfStream);
				pdfStream.AppendToString(ref pPage.m_bstrContents);
			}
			else
			{
				if (objectByName.m_nType == enumType.pdfArray)
				{
					PdfArray pdfArray = (PdfArray)objectByName;
					for (int i = 0; i < pdfArray.Size; i++)
					{
						PdfObject @object = pdfArray.GetObject(i);
						if (@object.m_nType == enumType.pdfRef)
						{
							PdfStream pdfStream2 = new PdfStream();
							this.ParseStream((PdfRef)@object, ref pdfStream2);
							pdfStream2.AppendToString(ref pPage.m_bstrContents, " ");
							pdfStream.Append(pdfStream2);
						}
					}
				}
			}
			pdfStream.AppendSpace();
			PdfInput pdfInput = new PdfInput(pdfStream.ToBytes(), this.m_pDoc, null);
			pdfInput.Next();
			pdfInput.ParseTextInContents(pPage, bSpace, nCodePage);
			this.ParseXObjects(pPage, bSpace, nCodePage);
		}
		private void ParseTextInContents(PdfPage pPage, bool bSpace, int nCodePage)
		{
			int nFontObjNum = 0;
			int nFontGenNum = 0;
			bool flag = false;
			while (this.m_dwPtr < this.m_dwBytesRead)
			{
				PdfObject pdfObject = this.ParseObject(null, 0, 0);
				if (pdfObject.m_nType == enumType.pdfOther)
				{
					string text = ((PdfOther)pdfObject).m_bstrValue;
					if (text == "bi")
					{
						this.SkipTo(new byte[]
						{
							10,
							69,
							73
						});
						flag = true;
					}
					if (text == "td" || text == "tm" || text == "t*" || text == "et" || text == "bt")
					{
						flag = true;
					}
					if (text == "'" || text == "\"")
					{
						flag = true;
					}
				}
				else
				{
					if (pdfObject.m_nType == enumType.pdfName)
					{
						string text = ((PdfName)pdfObject).m_bstrName;
						if (this.m_pDoc.IsFontName(text))
						{
							string name = text;
							pPage.FromFontNameToObjID(name, ref nFontObjNum, ref nFontGenNum);
						}
					}
					else
					{
						if (pdfObject.m_nType == enumType.pdfString)
						{
							this.m_pDoc.ConvertAndAppend(nFontObjNum, nFontGenNum, (PdfString)pdfObject, flag ? " " : "", nCodePage, ref pPage.m_bstrContentText);
							flag = false;
						}
						else
						{
							if (pdfObject.m_nType == enumType.pdfArray)
							{
								PdfArray pdfArray = (PdfArray)pdfObject;
								bSpace = true;
								for (int i = 0; i < pdfArray.Size; i++)
								{
									PdfObject @object = pdfArray.GetObject(i);
									if (@object.m_nType == enumType.pdfString)
									{
										this.m_pDoc.ConvertAndAppend(nFontObjNum, nFontGenNum, (PdfString)@object, bSpace ? " " : null, nCodePage, ref pPage.m_bstrContentText);
									}
									if (@object.m_nType == enumType.pdfNumber)
									{
										float num = (float)((PdfNumber)@object).m_fValue;
										bSpace = ((num >= 0f || -num >= 100f) && num < 0f);
									}
								}
							}
						}
					}
				}
			}
		}
		public void ParseContentStreamsForPreview(PdfPage pPage, ref PdfStream pOutStream)
		{
			PdfObject objectByName = pPage.m_pPageObj.m_objAttributes.GetObjectByName("Contents");
			if (objectByName == null)
			{
				return;
			}
			if (objectByName.m_nType == enumType.pdfRef)
			{
				this.ParseStream((PdfRef)objectByName, ref pOutStream);
				return;
			}
			if (objectByName.m_nType == enumType.pdfArray)
			{
				PdfArray pdfArray = (PdfArray)objectByName;
				for (int i = 0; i < pdfArray.Size; i++)
				{
					PdfObject @object = pdfArray.GetObject(i);
					if (@object.m_nType == enumType.pdfRef)
					{
						PdfStream stream = new PdfStream();
						this.ParseStream((PdfRef)@object, ref stream);
						pOutStream.Append(stream);
						pOutStream.AppendChar(32);
					}
				}
			}
		}
		internal void ParseStream(PdfRef pRef, ref PdfStream OutStream)
		{
			this.ParseStream(pRef, ref OutStream, 0);
		}
		internal void ParseStream(PdfRef pRef, ref PdfStream OutStream, int nOffset)
		{
			this.ParseStream(pRef, ref OutStream, nOffset, true);
		}
		internal void ParseStream(PdfRef pRef, ref PdfStream OutStream, int nOffset, bool bDecryptNormally)
		{
			if (nOffset == 0)
			{
				this.SetPointer(this.m_pDoc.m_objExistingXRef.GetOffset(pRef));
			}
			else
			{
				this.SetPointer(nOffset);
			}
			int nObjNum;
			int nGenNum;
			this.ParseObjectHeader(out nObjNum, out nGenNum);
			PdfObject pdfObject = this.ParseObject(null, nObjNum, nGenNum);
			if (pdfObject.m_nType != enumType.pdfDictionary)
			{
				this.Throw("Stream dictionary not found. ", PdfErrors._ERROR_INPUT_STREAM);
			}
			int num = 0;
			PdfObject objectByName = ((PdfDict)pdfObject).GetObjectByName("Length");
			if (objectByName.m_nType == enumType.pdfNumber)
			{
				num = (int)((PdfNumber)objectByName).m_fValue;
			}
			else
			{
				if (objectByName.m_nType == enumType.pdfRef)
				{
					int pointer = this.m_dwStartByte + this.m_dwPtr - 1;
					this.SetPointer(this.m_pDoc.m_objExistingXRef.GetOffset((PdfRef)objectByName));
					int num2;
					int num3;
					this.ParseObjectHeader(out num2, out num3);
					PdfObject pdfObject2 = this.ParseObject(null, nObjNum, nGenNum);
					if (pdfObject2.m_nType == enumType.pdfNumber)
					{
						num = (int)((PdfNumber)pdfObject2).m_fValue;
					}
					else
					{
						this.Throw("/Length entry could not be obtained from a stream.");
					}
					this.SetPointer(pointer);
				}
				else
				{
					this.Throw("Invalid /Length entry in a stream.");
				}
			}
			string[] array = new string[15];
			int num4 = 0;
			PdfObject objectByName2 = ((PdfDict)pdfObject).GetObjectByName("Filter");
			if (objectByName2 != null)
			{
				if (objectByName2.m_nType == enumType.pdfName)
				{
					array[num4++] = ((PdfName)objectByName2).m_bstrName;
				}
				else
				{
					if (objectByName2.m_nType == enumType.pdfArray)
					{
						PdfArray pdfArray = (PdfArray)objectByName2;
						for (int i = 0; i < pdfArray.Size; i++)
						{
							PdfObject @object = pdfArray.GetObject(i);
							if (@object.m_nType == enumType.pdfName)
							{
								array[num4++] = ((PdfName)@object).m_bstrName;
							}
						}
					}
					else
					{
						if (objectByName2.m_nType == enumType.pdfRef)
						{
							int pointer2 = this.m_dwStartByte + this.m_dwPtr - 1;
							this.SetPointer(this.m_pDoc.m_objExistingXRef.GetOffset((PdfRef)objectByName2));
							int nObjNum2;
							int nGenNum2;
							this.ParseObjectHeader(out nObjNum2, out nGenNum2);
							PdfObject pdfObject3 = this.ParseObject(null, nObjNum2, nGenNum2);
							if (pdfObject3.m_nType == enumType.pdfName)
							{
								array[num4++] = ((PdfName)pdfObject3).m_bstrName;
							}
							else
							{
								if (pdfObject3.m_nType == enumType.pdfArray)
								{
									PdfArray pdfArray2 = (PdfArray)pdfObject3;
									for (int j = 0; j < pdfArray2.Size; j++)
									{
										PdfObject object2 = pdfArray2.GetObject(j);
										if (object2.m_nType == enumType.pdfName)
										{
											array[num4++] = ((PdfName)object2).m_bstrName;
										}
									}
								}
							}
							this.SetPointer(pointer2);
						}
					}
				}
			}
			this.ParseNextWordNoWhiteSpace("stream");
			if (this.m_c == 13)
			{
				this.Next();
				if (this.m_c == 10)
				{
					this.Next();
				}
			}
			else
			{
				if (this.m_c == 10)
				{
					this.Next();
				}
			}
			byte[] array2 = new byte[num];
			for (int k = 0; k < num; k++)
			{
				array2[k] = this.m_c;
				this.Next();
			}
			this.ParseNextWord("endstream");
			int num5 = array2.Length;
			if (num4 > 0)
			{
				if (array2.Length >= 1 && (array2[array2.Length - 1] == 10 || array2[array2.Length - 1] == 13))
				{
					num5--;
				}
				if (array2.Length >= 2 && (array2[array2.Length - 2] == 10 || array2[array2.Length - 2] == 13))
				{
					num5--;
				}
			}
			OutStream.Set(array2, num5);
			if (bDecryptNormally)
			{
				OutStream.Decrypt(this.m_pDoc.m_pKey, nObjNum, nGenNum);
			}
			this.ApplyFilters(OutStream, array, num4, (PdfDict)pdfObject);
		}
		internal void ParseAnnots(PdfPage pPage)
		{
			PdfObject objectByName = pPage.m_pPageObj.m_objAttributes.GetObjectByName("Annots");
			if (objectByName == null)
			{
				return;
			}
			PdfArray pdfArray = null;
			if (objectByName.m_nType == enumType.pdfArray)
			{
				pdfArray = (PdfArray)objectByName;
			}
			if (objectByName.m_nType == enumType.pdfRef)
			{
				this.SetPointer(this.m_pDoc.m_objExistingXRef.GetOffset((PdfRef)objectByName));
				int num;
				int num2;
				this.ParseObjectHeader(out num, out num2);
				PdfObject pdfObject = this.ParseObject(null, num, num2);
				if (pdfObject.m_nType == enumType.pdfArray)
				{
					PdfIndirectObj pdfIndirectObj = this.m_pDoc.AddExistingIndirectObject(enumIndirectType.pdfIndirectArray, num, num2, pdfObject);
					pdfArray = (PdfArray)pdfIndirectObj.m_pSimpleValue;
					pdfArray.m_pParent = pdfIndirectObj;
					pdfArray.m_bTerminate = true;
					pPage.m_pAnnotArrayObj = pdfIndirectObj;
				}
			}
			if (pdfArray == null)
			{
				return;
			}
			for (int i = 0; i < pdfArray.Size; i++)
			{
				PdfObject @object = pdfArray.GetObject(i);
				if (@object.m_nType == enumType.pdfRef)
				{
					this.SetPointer(this.m_pDoc.m_objExistingXRef.GetOffset((PdfRef)@object));
					int num;
					int num2;
					this.ParseObjectHeader(out num, out num2);
					PdfObject pdfObject2 = this.ParseObject(null, num, num2);
					if (pdfObject2.m_nType == enumType.pdfDictionary)
					{
						PdfAnnots annots = pPage.Annots;
						if (objectByName == null)
						{
							this.Throw("Obtaining the Annots collection failed.");
						}
						annots.AddExisting((PdfDict)pdfObject2, pdfArray, num, num2);
					}
				}
			}
		}
		internal void ParseOutlines()
		{
			if (this.m_bParseOutlinesCalled)
			{
				return;
			}
			PdfObject objectByName = this.m_pDoc.m_pCatalogObj.m_objAttributes.GetObjectByName("Outlines");
			if (objectByName != null && objectByName.m_nType == enumType.pdfRef)
			{
				PdfObject pdfObject = this.RemoveRef(objectByName, enumType.pdfDictionary);
				PdfDict pdfDict = (PdfDict)pdfObject;
				PdfOutline outline = this.m_pDoc.GetOutline();
				if (pdfObject == null)
				{
					this.Throw("Obtaining the Outline collection failed.");
				}
				outline.m_pOutlineObj = this.m_pDoc.AddExistingIndirectObject(enumIndirectType.pdfIndirectOutline, ((PdfRef)objectByName).m_nObjNum, ((PdfRef)objectByName).m_nGenNum, pdfObject);
				outline.m_pOutlineObj.m_objAttributes.m_bstrType = null;
				PdfObject objectByName2 = pdfDict.GetObjectByName("First");
				if (objectByName2 != null && objectByName2.m_nType == enumType.pdfRef)
				{
					this.ParseOutlineItem((PdfRef)objectByName2, 0);
				}
				outline.ArrangeExistingItems();
			}
			this.m_bParseOutlinesCalled = true;
		}
		private void ParseOutlineItem(PdfRef pItem, int nLevel)
		{
			int nObjNum = pItem.m_nObjNum;
			int nGenNum = pItem.m_nGenNum;
			PdfObject pdfObject = this.RemoveRef(pItem, enumType.pdfDictionary);
			if (pdfObject == null)
			{
				return;
			}
			PdfDict pdfDict = (PdfDict)pdfObject;
			PdfOutline outline = this.m_pDoc.GetOutline();
			outline.AddExisting(pdfDict, nObjNum, nGenNum, nLevel);
			PdfObject objectByName = pdfDict.GetObjectByName("First");
			if (objectByName != null && objectByName.m_nType == enumType.pdfRef)
			{
				this.ParseOutlineItem((PdfRef)objectByName, nLevel + 1);
			}
			objectByName = pdfDict.GetObjectByName("Next");
			if (objectByName != null && objectByName.m_nType == enumType.pdfRef)
			{
				this.ParseOutlineItem((PdfRef)objectByName, nLevel);
			}
		}
		public PdfIndirectObj ParseGenericIndirectObj(int nOffset, PdfDocument pMasterDoc, int nAdjustment)
		{
			this.SetPointer(nOffset);
			int num;
			int num2;
			this.ParseObjectHeader(out num, out num2);
			int num3 = 0;
			bool flag = false;
			PdfObject pdfObject = this.ParseObject(null, num, num2);
			PdfIndirectObj pdfIndirectObj = pMasterDoc.AddExistingIndirectObject(enumIndirectType.pdfIndirectDict, num + nAdjustment, num2, pdfObject);
			pdfIndirectObj.m_bModified = true;
			if (pdfObject.m_nType == enumType.pdfDictionary)
			{
				PdfObject objectByName = ((PdfDict)pdfObject).GetObjectByName("Length");
				if (objectByName != null && objectByName.m_nType == enumType.pdfNumber)
				{
					num3 = (int)((PdfNumber)objectByName).m_fValue;
				}
				else
				{
					if (objectByName != null && objectByName.m_nType == enumType.pdfRef)
					{
						int pointer = this.m_dwStartByte + this.m_dwPtr - 1;
						this.SetPointer(this.m_pDoc.m_objExistingXRef.GetOffset((PdfRef)objectByName));
						int num4;
						int num5;
						this.ParseObjectHeader(out num4, out num5);
						PdfObject pdfObject2 = this.ParseObject(null, num, num2);
						if (pdfObject2.m_nType == enumType.pdfNumber)
						{
							num3 = (int)((PdfNumber)pdfObject2).m_fValue;
						}
						else
						{
							this.Throw("/Length entry could not be obtained from a stream.");
						}
						this.SetPointer(pointer);
						flag = true;
					}
				}
				if (this.m_c == 115)
				{
					this.ParseNextWordNoWhiteSpace("stream");
					if (this.m_c == 32)
					{
						this.Next();
					}
					if (this.m_c == 13)
					{
						this.Next();
						if (this.m_c == 10)
						{
							this.Next();
						}
					}
					else
					{
						if (this.m_c == 10)
						{
							this.Next();
						}
					}
					byte[] array = new byte[num3];
					for (int i = 0; i < num3; i++)
					{
						array[i] = this.m_c;
						this.Next();
					}
					this.ParseNextWord("endstream");
					pdfIndirectObj.m_objStream.Set(array);
					pdfIndirectObj.m_objStream.Decrypt(this.m_pDoc.m_pKey, num, num2);
					if (pMasterDoc.m_pKey != null && pMasterDoc.m_pKey.m_nPtr == 1 && flag)
					{
						pdfIndirectObj.m_objAttributes.Add(new PdfNumber("Length", (double)pdfIndirectObj.m_objStream.Length));
					}
					pdfIndirectObj.m_nType = enumIndirectType.pdfIndirectStream;
				}
			}
			return pdfIndirectObj;
		}
		internal void ParseAcroForm()
		{
			if (this.m_bParseAcroFormCalled)
			{
				return;
			}
			this.m_bParseAcroFormCalled = true;
			PdfObject objectByName = this.m_pDoc.m_pCatalogObj.m_objAttributes.GetObjectByName("AcroForm");
			if (objectByName == null)
			{
				return;
			}
			PdfForm form = this.m_pDoc.GetForm();
			int num = 0;
			int num2 = 0;
			if (objectByName.m_nType == enumType.pdfRef)
			{
				int offset = this.m_pDoc.m_objExistingXRef.GetOffset((PdfRef)objectByName);
				this.SetPointer(offset);
				this.ParseObjectHeader(out num, out num2);
				PdfObject pdfObject = this.ParseObject(null, num, num2);
				if (pdfObject.m_nType == enumType.pdfDictionary)
				{
					this.m_pDoc.RemoveIndirectObject(form.m_pFormObj);
					form.m_pFormObj = this.m_pDoc.AddExistingIndirectObject(enumIndirectType.pdfIndirectForm, num, num2, (PdfDict)pdfObject);
				}
			}
			else
			{
				if (objectByName.m_nType != enumType.pdfDictionary)
				{
					return;
				}
				this.m_pDoc.RemoveIndirectObject(form.m_pFormObj);
				form.m_pFormObj = this.m_pDoc.AddNewIndirectObject(enumIndirectType.pdfIndirectForm);
				form.m_pFormObj.m_objAttributes.CopyItems((PdfDict)objectByName);
				form.m_pFormObj.m_objAttributes.m_bstrType = null;
				this.m_pDoc.m_pCatalogObj.m_objAttributes.RemoveByName("AcroForm");
				this.m_pDoc.m_pCatalogObj.AddReference("AcroForm", form.m_pFormObj);
				this.m_pDoc.m_pCatalogObj.m_bModified = true;
			}
			PdfObject objectByName2 = form.m_pFormObj.m_objAttributes.GetObjectByName("NeedAppearances");
			if (objectByName2 != null && objectByName2.m_nType == enumType.pdfBool)
			{
				form.m_bNeedAppearances = ((PdfBool)objectByName2).m_bValue;
			}
			PdfObject objectByName3 = form.m_pFormObj.m_objAttributes.GetObjectByName("SigFlags");
			if (objectByName3 != null && objectByName3.m_nType == enumType.pdfNumber)
			{
				form.m_nSigFlags = (int)((PdfNumber)objectByName3).m_fValue;
			}
			PdfObject objectByName4 = form.m_pFormObj.m_objAttributes.GetObjectByName("DA");
			if (objectByName4 != null && objectByName4.m_nType == enumType.pdfString)
			{
				form.m_bstrDefaultAppearance = ((PdfString)objectByName4).ToString();
			}
			PdfObject pdfObject2 = this.RemoveRef(form.m_pFormObj.m_objAttributes.GetObjectByName("DR"), enumType.pdfDictionary);
			if (pdfObject2 != null)
			{
				PdfObject objectByName5 = ((PdfDict)pdfObject2).GetObjectByName("Font");
				if (objectByName5 != null && objectByName5.m_nType == enumType.pdfRef)
				{
					int offset2 = this.m_pDoc.m_objExistingXRef.GetOffset((PdfRef)objectByName5);
					this.SetPointer(offset2);
					this.ParseObjectHeader(out num, out num2);
					PdfObject pObj = this.ParseObject(null, num, num2);
					form.m_pFontObj = this.m_pDoc.AddExistingIndirectObject(enumIndirectType.pdfIndirectResource, num, num2, pObj);
				}
				else
				{
					if (objectByName5 != null && objectByName5.m_nType == enumType.pdfDictionary && form.m_pFormObj.m_objAttributes.GetObjectByName("DR").m_nType == enumType.pdfRef)
					{
						PdfRef pdfRef = (PdfRef)form.m_pFormObj.m_objAttributes.GetObjectByName("DR");
						form.m_pDRObj = this.m_pDoc.AddExistingIndirectObject(enumIndirectType.pdfIndirectResource, pdfRef.m_nObjNum, pdfRef.m_nGenNum, pdfObject2);
						form.m_pDRObj.m_objAttributes.m_bstrType = null;
					}
				}
				PdfObject pdfObject3 = this.RemoveRef(((PdfDict)pdfObject2).GetObjectByName("Font"), enumType.pdfDictionary);
				PdfDict pdfDict = (PdfDict)pdfObject3;
				if (pdfDict != null)
				{
					for (int i = 0; i < pdfDict.Size; i++)
					{
						PdfObject @object = pdfDict.GetObject(i);
						if (@object != null && @object.m_nType == enumType.pdfRef)
						{
							this.ParseFont((PdfRef)@object);
						}
					}
				}
				PdfObject pdfObject4 = this.RemoveRef(((PdfDict)pdfObject2).GetObjectByName("Encoding"), enumType.pdfNull);
				if (pdfObject4 != null && pdfObject4.m_nType == enumType.pdfDictionary)
				{
					PdfDict pdfDict2 = (PdfDict)pdfObject4;
					if (pdfDict2.Size > 0)
					{
						PdfObject object2 = pdfDict2.GetObject(0);
						if (object2 != null && object2.m_nType == enumType.pdfRef)
						{
							PdfObject pdfObject5 = this.RemoveRef(object2, enumType.pdfDictionary);
							if (pdfObject5 != null)
							{
								PdfObject pdfObject6 = this.RemoveRef(((PdfDict)pdfObject5).GetObjectByName("Differences"), enumType.pdfArray);
								PdfFont pdfFont = new PdfFont();
								form.m_ptrEncoding = pdfFont;
								pdfFont.HandleDifferences((PdfArray)pdfObject6);
							}
						}
					}
				}
			}
			PdfObject objectByName6 = form.m_pFormObj.m_objAttributes.GetObjectByName("Fields");
			if (objectByName6 == null)
			{
				return;
			}
			if (objectByName6.m_nType == enumType.pdfRef)
			{
				int offset3 = this.m_pDoc.m_objExistingXRef.GetOffset((PdfRef)objectByName6);
				this.SetPointer(offset3);
				this.ParseObjectHeader(out num, out num2);
				PdfObject pdfObject7 = this.ParseObject(null, num, num2);
				if (pdfObject7.m_nType == enumType.pdfArray)
				{
					PdfArray pdfArray = (PdfArray)((PdfArray)pdfObject7).Copy();
					pdfArray.m_bstrType = "Fields";
					pdfArray.m_nItemsPerLine = 5;
					form.m_pFormObj.m_objAttributes.Add(pdfArray);
					form.m_pFormObj.m_bModified = true;
					objectByName6 = form.m_pFormObj.m_objAttributes.GetObjectByName("Fields");
				}
			}
			if (objectByName6.m_nType != enumType.pdfArray)
			{
				return;
			}
			PdfArray pdfArray2 = (PdfArray)objectByName6;
			for (int j = 0; j < pdfArray2.Size; j++)
			{
				PdfObject object3 = pdfArray2.GetObject(j);
				if (object3.m_nType == enumType.pdfRef)
				{
					PdfAnnot pdfAnnot = this.m_pDoc.FindAnnot((PdfRef)object3);
					if (pdfAnnot != null)
					{
						form.m_arrFields.Add(pdfAnnot);
					}
					else
					{
						this.ParseFormField((PdfRef)object3, pdfArray2);
					}
				}
			}
		}
		internal void ParseFormField(PdfRef pRef, PdfArray pParentArray)
		{
			PdfForm form = this.m_pDoc.GetForm();
			this.SetPointer(this.m_pDoc.m_objExistingXRef.GetOffset(pRef));
			int nObjNum;
			int nGenNum;
			this.ParseObjectHeader(out nObjNum, out nGenNum);
			PdfObject pdfObject = this.ParseObject(null, nObjNum, nGenNum);
			if (pdfObject.m_nType != enumType.pdfDictionary)
			{
				return;
			}
			PdfAnnots fields = form.Fields;
			if (fields == null)
			{
				this.Throw("Obtaining the Form.Fields collection failed.");
			}
			fields.AddExisting((PdfDict)pdfObject, pParentArray, nObjNum, nGenNum);
		}
		public void ParseSignature(PdfRef pRef, PdfSignature pSig)
		{
			int offset = this.m_pDoc.m_objExistingXRef.GetOffset(pRef);
			this.SetPointer(offset);
			int nObjNum;
			int nGenNum;
			this.ParseObjectHeader(out nObjNum, out nGenNum);
			PdfStream pKey = this.m_pDoc.m_pKey;
			this.m_pDoc.m_pKey = null;
			PdfObject pdfObject = this.ParseObject(null, nObjNum, nGenNum);
			this.m_pDoc.m_pKey = pKey;
			if (pdfObject.m_nType == enumType.pdfDictionary)
			{
				PdfDict pdfDict = (PdfDict)pdfObject;
				for (int i = 0; i < pdfDict.Size; i++)
				{
					PdfObject @object = pdfDict.GetObject(i);
					if (@object != null && @object.m_nType == enumType.pdfString && !(((PdfString)@object).m_bstrType == "Contents"))
					{
						((PdfString)@object).Decrypt(this.m_pDoc.m_pKey, nObjNum, nGenNum);
					}
				}
				pSig.Populate((PdfDict)pdfObject, nObjNum, nGenNum);
			}
		}
		public void ParseMetaData()
		{
			if (this.m_bParseMetaDataCalled)
			{
				return;
			}
			this.m_bParseMetaDataCalled = true;
			PdfObject objectByName = this.m_pDoc.m_pCatalogObj.m_objAttributes.GetObjectByName("Metadata");
			if (objectByName != null && objectByName.m_nType == enumType.pdfRef)
			{
				PdfString pdfString = new PdfString();
				PdfStream pStream = new PdfStream();
				this.ParseStream((PdfRef)objectByName, ref pStream);
				pdfString.Set(pStream);
				pdfString.TestForAnsi();
				this.m_pDoc.m_bstrMetaData = pdfString.ToString();
			}
		}
		public PdfObject ParseOriginalGenericObject(PdfRef Ref, int nLevel)
		{
			int offset = this.m_pDoc.m_objExistingXRef.GetOffset(Ref);
			if (offset == 0)
			{
				this.Throw("Object cannot be found.");
			}
			this.SetPointer(offset);
			int nObjNum;
			int nGenNum;
			this.ParseObjectHeader(out nObjNum, out nGenNum);
			int num = 0;
			PdfObject pdfObject = this.ParseObject(Ref.m_bstrType, nObjNum, nGenNum);
			if (pdfObject.m_nType == enumType.pdfDictionary)
			{
				PdfObject objectByName = ((PdfDict)pdfObject).GetObjectByName("Length");
				if (objectByName != null && objectByName.m_nType == enumType.pdfNumber)
				{
					num = (int)((PdfNumber)objectByName).m_fValue;
				}
				else
				{
					if (objectByName != null && objectByName.m_nType == enumType.pdfRef)
					{
						int pointer = this.m_dwStartByte + this.m_dwPtr - 1;
						PdfNumber pdfNumber = (PdfNumber)this.RemoveRef(objectByName, enumType.pdfNumber);
						num = (int)pdfNumber.m_fValue;
						this.SetPointer(pointer);
					}
				}
				string[] array = new string[15];
				int nFilterCount = 0;
				PdfObject objectByName2 = ((PdfDict)pdfObject).GetObjectByName("Filter");
				if (objectByName2 != null)
				{
					if (objectByName2.m_nType == enumType.pdfName)
					{
						array[nFilterCount++] = ((PdfName)objectByName2).m_bstrName;
					}
					else
					{
						if (objectByName2.m_nType == enumType.pdfArray)
						{
							PdfArray pdfArray = (PdfArray)objectByName2;
							for (int i = 0; i < pdfArray.Size; i++)
							{
								PdfObject @object = pdfArray.GetObject(i);
								if (@object.m_nType == enumType.pdfName)
								{
									array[nFilterCount++] = ((PdfName)@object).m_bstrName;
								}
							}
						}
						else
						{
							if (objectByName2.m_nType == enumType.pdfRef)
							{
								int pointer2 = this.m_dwStartByte + this.m_dwPtr - 1;
								offset = this.m_pDoc.m_objExistingXRef.GetOffset((PdfRef)objectByName2);
								this.SetPointer(offset);
								int nObjNum2;
								int nGenNum2;
								this.ParseObjectHeader(out nObjNum2, out nGenNum2);
								PdfObject pdfObject2 = this.ParseObject(null, nObjNum2, nGenNum2);
								if (pdfObject2.m_nType == enumType.pdfName)
								{
									array[nFilterCount++] = ((PdfName)pdfObject2).m_bstrName;
								}
								else
								{
									if (pdfObject2.m_nType == enumType.pdfArray)
									{
										PdfArray pdfArray2 = (PdfArray)pdfObject2;
										for (int j = 0; j < pdfArray2.Size; j++)
										{
											PdfObject object2 = pdfArray2.GetObject(j);
											if (object2.m_nType == enumType.pdfName)
											{
												array[nFilterCount++] = ((PdfName)object2).m_bstrName;
											}
										}
									}
								}
								this.SetPointer(pointer2);
							}
						}
					}
				}
				if (this.m_c == 115)
				{
					PdfStream pdfStream = new PdfStream();
					this.ParseNextWordNoWhiteSpace("stream");
					if (this.m_c == 13)
					{
						this.Next();
						if (this.m_c == 10)
						{
							this.Next();
						}
					}
					else
					{
						if (this.m_c == 10)
						{
							this.Next();
						}
					}
					for (int k = 0; k < num; k++)
					{
						pdfStream.Append(new byte[]
						{
							this.m_c
						});
						this.Next();
					}
					this.ParseNextWord("endstream");
					pdfStream.Decrypt(this.m_pDoc.m_pKey, nObjNum, nGenNum);
					this.ApplyFilters(pdfStream, array, nFilterCount, (PdfDict)pdfObject);
					PdfDictWithStream pdfDictWithStream = new PdfDictWithStream(pdfObject.m_bstrType);
					pdfDictWithStream.CopyItems((PdfDict)pdfObject);
					pdfDictWithStream.m_nType = enumType.pdfDictWithStream;
					pdfDictWithStream.m_objStream.Set(pdfStream);
					pdfObject = pdfDictWithStream;
				}
			}
			return pdfObject;
		}
		internal PdfObject ParseGenericObject(PdfRef reff, int nLevel)
		{
			PdfObject pdfObject = this.ParseOriginalGenericObject(reff, nLevel);
			this.RemoveAllRefsFromObject(pdfObject, nLevel);
			pdfObject.m_ref.x = reff.m_nObjNum;
			pdfObject.m_ref.y = reff.m_nGenNum;
			return pdfObject;
		}
		internal void ApplyFilters(PdfStream pStream, string[] pFilterList, int nFilterCount, PdfDict pDict)
		{
			if (pStream.Length == 0)
			{
				return;
			}
			long num = 0L;
			for (int i = 0; i < nFilterCount; i++)
			{
				if (pFilterList[i] == "FlateDecode" || pFilterList[i] == "Fl")
				{
					num = (long)pStream.DecodeFlate(pDict, i);
				}
				else
				{
					if (pFilterList[i] == "ASCIIHexDecode" || pFilterList[i] == "AHx")
					{
						num = (long)pStream.DecodeHex();
					}
					else
					{
						if (pFilterList[i] == "LZWDecode" || pFilterList[i] == "LZW")
						{
							num = (long)pStream.DecodeLZW();
						}
						else
						{
							if (pFilterList[i] == "ASCII85Decode" || pFilterList[i] == "A85")
							{
								num = (long)pStream.DecodeASCII85();
							}
							else
							{
								if (pFilterList[i] == "DCTDecode" || pFilterList[i] == "DCT")
								{
									PdfDCTStream pdfDCTStream = new PdfDCTStream(pStream);
									pdfDCTStream.Decode(pStream);
								}
								else
								{
									if (pFilterList[i] == "CCITTFaxDecode" || pFilterList[i] == "CCF")
									{
										if (pDict != null)
										{
											this.RemoveAllRefsFromObject(pDict, 0);
										}
										PdfCITTFaxStream pdfCITTFaxStream = new PdfCITTFaxStream(pStream, pDict, i);
										pdfCITTFaxStream.Decode(pStream);
									}
									else
									{
										if (pFilterList[i] == "RunLengthDecode" || pFilterList[i] == "RL")
										{
											PdfRunLengthStream pdfRunLengthStream = new PdfRunLengthStream(pStream);
											pdfRunLengthStream.Decode(pStream);
										}
										else
										{
											if (pFilterList[i] == "JBIG2Decode")
											{
												PdfObject objectByName = pDict.GetObjectByName("DecodeParms");
												if (objectByName != null)
												{
													this.RemoveAllRefsFromObject(objectByName, 0);
												}
												JBIG2Stream jBIG2Stream = new JBIG2Stream(pStream, objectByName, i);
												jBIG2Stream.Decode(pStream);
											}
											else
											{
												if (pFilterList[i] == "JPXDecode")
												{
													JPXStream jPXStream = new JPXStream(pStream);
													jPXStream.Decode(pStream);
												}
												else
												{
													string szError = string.Format("Filter {0} not supported.", pFilterList[i]);
													this.Throw(szError, PdfErrors._ERROR_INPUT_FILTER);
												}
											}
										}
									}
								}
							}
						}
					}
				}
				if (num < 0L)
				{
					this.Throw("Decoder failed.");
				}
			}
		}
		internal void RemoveAllRefsFromObject(PdfObject pObj, int nLevel)
		{
			if (nLevel > 25)
			{
				return;
			}
			if (pObj.m_nType != enumType.pdfArray && pObj.m_nType != enumType.pdfDictionary && pObj.m_nType != enumType.pdfDictWithStream)
			{
				return;
			}
			PdfObjectList pdfObjectList = (PdfObjectList)pObj;
			int num = -1;
			PdfObject newObj = null;
			for (int i = 0; i < pdfObjectList.Size; i++)
			{
				PdfObject @object = pdfObjectList.GetObject(i);
				if (@object.m_nType == enumType.pdfRef)
				{
					newObj = this.ParseGenericObject((PdfRef)@object, nLevel + 1);
					num = i;
					break;
				}
				if (@object.m_nType == enumType.pdfArray || @object.m_nType == enumType.pdfDictionary)
				{
					this.RemoveAllRefsFromObject(@object, nLevel + 1);
				}
			}
			if (num != -1)
			{
				pdfObjectList.Insert(newObj, num);
				pdfObjectList.RemoveByIndex(num + 1);
				this.RemoveAllRefsFromObject(pObj, nLevel);
			}
		}
		private void ParseFonts(PdfPage pPage)
		{
			PdfDict pdfDict = null;
			if (pPage.m_pFontDictObj != null)
			{
				pdfDict = pPage.m_pFontDictObj.m_objAttributes;
			}
			else
			{
				if (pPage.m_pResourceObj != null)
				{
					pdfDict = (PdfDict)pPage.m_pResourceObj.m_objAttributes.GetObjectByName("Font");
				}
				else
				{
					PdfDict pdfDict2 = (PdfDict)pPage.m_pPageObj.m_objAttributes.GetObjectByName("Resources");
					if (pdfDict2 != null)
					{
						pdfDict = (PdfDict)pdfDict2.GetObjectByName("Font");
					}
				}
			}
			if (pdfDict == null)
			{
				return;
			}
			pPage.m_ptrFontDict = (PdfDict)pdfDict.Copy();
			for (int i = 0; i < pdfDict.Size; i++)
			{
				PdfObject @object = pdfDict.GetObject(i);
				if (@object != null && @object.m_nType == enumType.pdfRef)
				{
					this.ParseFont((PdfRef)@object);
				}
			}
		}
		private void ParseFont(PdfRef pRef)
		{
			if (this.m_pDoc.IsFontParsed(pRef))
			{
				return;
			}
			this.SetPointer(this.m_pDoc.m_objExistingXRef.GetOffset(pRef));
			int nObjNum;
			int nGenNum;
			this.ParseObjectHeader(out nObjNum, out nGenNum);
			PdfObject pdfObject = this.ParseObject(null, nObjNum, nGenNum);
			if (pdfObject.m_nType != enumType.pdfDictionary)
			{
				this.Throw("Font object is not a dictionary.");
			}
			PdfDict pdfDict = (PdfDict)pdfObject;
			if (pdfDict.GetObjectByName("Widths") != null)
			{
				pdfDict.Add(this.RemoveRef(pdfDict.GetObjectByName("Widths"), enumType.pdfArray));
			}
			if (pdfDict.GetObjectByName("FontDescriptor") != null)
			{
				pdfDict.Add(this.RemoveRef(pdfDict.GetObjectByName("FontDescriptor"), enumType.pdfDictionary));
			}
			PdfFont pdfFont = this.m_pDoc.AddExistingFont(pdfDict, pRef.m_bstrType, nObjNum, nGenNum);
			PdfObject objectByName = pdfDict.GetObjectByName("ToUnicode");
			if (objectByName != null && objectByName.m_nType == enumType.pdfRef)
			{
				PdfRef pdfRef = (PdfRef)objectByName;
				if (!this.m_pDoc.IsToUnicodeParsed(pdfRef))
				{
					PdfStream pdfStream = new PdfStream();
					this.ParseStream(pdfRef, ref pdfStream);
					pdfStream.AppendSpace();
					PdfInput pdfInput = new PdfInput(pdfStream.ToBytes(), this.m_pDoc, null);
					pdfInput.m_bIgnoreDef = true;
					pdfInput.Next();
					pdfInput.ParseToUnicode(pdfFont);
					pdfFont.m_nToUnicodeObjNum = pdfRef.m_nObjNum;
					pdfFont.m_nToUnicodeGenNum = pdfRef.m_nGenNum;
				}
			}
			this.ParseFontEncoding("Encoding", pdfFont, pdfDict);
		}
		private void ParseToUnicode(PdfFont pFont)
		{
			int num = 0;
			while (this.m_dwPtr < this.m_dwBytesRead)
			{
				PdfObject pdfObject = this.ParseObject(null, 0, 0);
				if (pdfObject.m_nType == enumType.pdfNumber)
				{
					num = (int)((PdfNumber)pdfObject).m_fValue;
				}
				else
				{
					if (pdfObject.m_nType == enumType.pdfOther)
					{
						string bstrValue = ((PdfOther)pdfObject).m_bstrValue;
						if (bstrValue == "beginbfrange")
						{
							for (int i = num; i > 0; i--)
							{
								PdfObject pBegin = this.ParseObject(null, 0, 0);
								PdfObject pEnd = this.ParseObject(null, 0, 0);
								PdfObject pCode = this.ParseObject(null, 0, 0);
								pFont.AddCmapRangeEntry(pBegin, pEnd, pCode);
							}
						}
						if (bstrValue == "beginbfchar")
						{
							for (int j = num; j > 0; j--)
							{
								PdfObject pBegin = this.ParseObject(null, 0, 0);
								PdfObject pCode = this.ParseObject(null, 0, 0);
								pFont.AddCmapCharEntry(pBegin, pCode);
							}
						}
					}
				}
			}
		}
		private void ParseFontEncoding(string EntryName, PdfFont pFont, PdfDict pFontDict)
		{
			PdfObject objectByName = pFontDict.GetObjectByName(EntryName);
			if (objectByName == null)
			{
				return;
			}
			if (objectByName.m_nType == enumType.pdfName)
			{
				pFont.m_bstrEncoding = ((PdfName)objectByName).m_bstrName;
				if (((PdfName)objectByName).m_bstrName == "MacRomanEncoding")
				{
					pFont.m_bMacRomanEncoding = true;
				}
				return;
			}
			PdfDict pdfDict = null;
			if (objectByName.m_nType == enumType.pdfDictionary)
			{
				pdfDict = (PdfDict)objectByName;
			}
			else
			{
				if (objectByName.m_nType == enumType.pdfRef)
				{
					PdfFont pdfFont = this.m_pDoc.IsEncodingParsed((PdfRef)objectByName);
					if (pdfFont != null)
					{
						pFont.CopyEncodingInfo(pdfFont);
						return;
					}
					this.SetPointer(this.m_pDoc.m_objExistingXRef.GetOffset((PdfRef)objectByName));
					int num;
					int num2;
					this.ParseObjectHeader(out num, out num2);
					PdfObject pdfObject = this.ParseObject(null, num, num2);
					if (pdfObject.m_nType == enumType.pdfDictionary)
					{
						pdfDict = (PdfDict)pdfObject;
					}
					pFont.m_nEncodingObjNum = num;
					pFont.m_nEncodingGenNum = num2;
				}
			}
			if (pdfDict == null)
			{
				return;
			}
			this.ParseFontEncoding("BaseEncoding", pFont, pdfDict);
			PdfObject objectByName2 = pdfDict.GetObjectByName("Differences");
			if (objectByName2 == null)
			{
				return;
			}
			PdfArray pdfArray = null;
			if (objectByName2.m_nType == enumType.pdfArray)
			{
				pdfArray = (PdfArray)objectByName2;
			}
			else
			{
				if (objectByName2.m_nType == enumType.pdfRef)
				{
					this.SetPointer(this.m_pDoc.m_objExistingXRef.GetOffset((PdfRef)objectByName2));
					int num;
					int num2;
					this.ParseObjectHeader(out num, out num2);
					PdfObject pdfObject2 = this.ParseObject(null, num, num2);
					if (pdfObject2.m_nType == enumType.pdfArray)
					{
						pdfArray = (PdfArray)pdfObject2;
					}
				}
			}
			if (pdfArray != null)
			{
				pFont.HandleDifferences(pdfArray);
			}
		}
		public void ParseEncrypt(PdfObject pObj)
		{
			if (this.m_bPasswordAuthenticated)
			{
				return;
			}
			PdfDict pdfDict = null;
			int num = 0;
			int num2 = 0;
			if (pObj.m_nType == enumType.pdfNull)
			{
				return;
			}
			if (pObj.m_nType == enumType.pdfRef)
			{
				int offset = this.m_pDoc.m_objExistingXRef.GetOffset((PdfRef)pObj);
				if (offset <= 0)
				{
					return;
				}
				this.SetPointer(offset);
				this.ParseObjectHeader(out num, out num2);
				PdfObject pdfObject = this.ParseObject(null, num, num2);
				if (pdfObject.m_nType == enumType.pdfDictionary)
				{
					pdfDict = (PdfDict)pdfObject;
				}
				else
				{
					this.Throw("/Encrypt entry does not point to a dictionary.");
				}
			}
			else
			{
				if (pObj.m_nType == enumType.pdfDictionary)
				{
					pdfDict = (PdfDict)pObj;
				}
				else
				{
					this.Throw("/Encrypt is neither a reference nor dictionary.");
				}
			}
			this.m_pDoc.m_pEncDict = this.m_pDoc.AddExistingIndirectObject(enumIndirectType.pdfIndirectEncDict, num, num2, pdfDict);
			int num3 = 40;
			PdfObject objectByName = pdfDict.GetObjectByName("Length");
			if (objectByName != null && objectByName.m_nType == enumType.pdfNumber)
			{
				num3 = (int)((PdfNumber)objectByName).m_fValue;
			}
			objectByName = pdfDict.GetObjectByName("R");
			if (objectByName == null || objectByName.m_nType != enumType.pdfNumber)
			{
				this.Throw("/R entry not found or invalid in the encryption dictionary.");
			}
			int num4 = (int)((PdfNumber)objectByName).m_fValue;
			bool bEncryptMetadata = true;
			objectByName = pdfDict.GetObjectByName("EncryptMetadata");
			if (objectByName != null && objectByName.m_nType == enumType.pdfBool)
			{
				bEncryptMetadata = ((PdfBool)objectByName).m_bValue;
			}
			objectByName = pdfDict.GetObjectByName("P");
			if (objectByName == null || objectByName.m_nType != enumType.pdfNumber)
			{
				this.Throw("/P entry not found or invalid in the encryption dictionary.");
			}
			int nPermissions = (int)((PdfNumber)objectByName).m_fValue;
			this.m_pDoc.m_nPermissions = (pdfPermissions)nPermissions;
			objectByName = pdfDict.GetObjectByName("O");
			if (objectByName == null || objectByName.m_nType != enumType.pdfString)
			{
				this.Throw("/O entry not found or invalid in the encryption dictionary.");
			}
			PdfString pdfString = new PdfString(null, (PdfString)objectByName);
			objectByName = pdfDict.GetObjectByName("U");
			if (objectByName == null || objectByName.m_nType != enumType.pdfString)
			{
				this.Throw("/U entry not found or invalid in the encryption dictionary.");
			}
			PdfString u = new PdfString(null, (PdfString)objectByName);
			PdfStream pdfStream = new PdfStream(pdfString);
			PdfStream pdfStream2 = new PdfStream();
			pdfStream2.MD5FromPaddedString(this.m_bstrPassword, (num4 >= 3) ? 50 : 0);
			pdfStream.Encrypt(pdfStream2, num3 / 8);
			if (num4 >= 3)
			{
				for (int i = 1; i <= 19; i++)
				{
					byte[] array = new byte[num3 / 8];
					Array.Copy(pdfStream2.ToBytes(), array, num3 / 8);
					for (int j = 0; j < num3 / 8; j++)
					{
						byte[] expr_25A_cp_0 = array;
						int expr_25A_cp_1 = j;
						expr_25A_cp_0[expr_25A_cp_1] ^= (byte)i;
					}
					pdfStream.Encrypt(array);
				}
			}
			if (!this.AuthenticateUserPassword(pdfStream, pdfString, u, num4, bEncryptMetadata, nPermissions, num3))
			{
				PdfStream pdfStream3 = new PdfStream();
				pdfStream3.PadString(this.m_bstrPassword);
				if (!this.AuthenticateUserPassword(pdfStream3, pdfString, u, num4, bEncryptMetadata, nPermissions, num3))
				{
					this.Throw("Invalid password specified.", PdfErrors._ERROR_INVALIDPASSWORD);
				}
				else
				{
					this.m_pDoc.m_bUserPassword = (this.m_bstrPassword != null && this.m_bstrPassword.Length > 0);
					this.m_pDoc.m_bOwnerPassword = true;
				}
			}
			else
			{
				this.m_pDoc.m_bOwnerPassword = (this.m_bstrPassword != null);
				PdfStream pdfStream4 = new PdfStream();
				pdfStream4.PadString("");
				this.m_pDoc.m_bUserPassword = !pdfStream.Equals(ref pdfStream4);
				PdfStream pdfStream5 = new PdfStream();
				pdfStream5.PadString(this.m_bstrPassword);
				this.m_pDoc.m_bOwnerPassword = !pdfStream.Equals(ref pdfStream5);
				this.m_pDoc.m_bOwnerPasswordSpecified = true;
			}
			this.m_bPasswordAuthenticated = true;
			this.ParseCryptoFilters(pdfDict);
		}
		private bool AuthenticateUserPassword(PdfStream PaddedPwd, PdfStream O, PdfStream U, int nRev, bool bEncryptMetadata, int nPermissions, int nKeyLength)
		{
			PdfHash pdfHash = new PdfHash();
			pdfHash.Init();
			pdfHash.Update(PaddedPwd);
			pdfHash.Update(O);
			byte[] bytes = BitConverter.GetBytes(nPermissions);
			pdfHash.Update(bytes);
			pdfHash.Update(this.m_pDoc.m_objID);
			if (nRev >= 4 && !bEncryptMetadata)
			{
				byte[] bytes2 = BitConverter.GetBytes(4294967295u);
				pdfHash.Update(bytes2);
			}
			pdfHash.Final();
			if (nRev >= 3)
			{
				for (int i = 0; i < 50; i++)
				{
					pdfHash.HashItself();
				}
			}
			PdfStream pdfStream = new PdfStream();
			pdfStream.PadString("");
			new PdfStream();
			if (nRev >= 3)
			{
				PdfHash pdfHash2 = new PdfHash();
				pdfHash2.Init();
				pdfHash2.Update(pdfStream);
				pdfHash2.Update(this.m_pDoc.m_objID);
				pdfHash2.Final();
				pdfHash2.Encrypt(pdfHash, nKeyLength / 8);
				byte[] array = new byte[nKeyLength / 8];
				for (int j = 1; j <= 19; j++)
				{
					Array.Copy(pdfHash.ToBytes(), array, nKeyLength / 8);
					for (int k = 0; k < nKeyLength / 8; k++)
					{
						byte[] expr_FC_cp_0 = array;
						int expr_FC_cp_1 = k;
						expr_FC_cp_0[expr_FC_cp_1] ^= (byte)j;
					}
					pdfHash2.Encrypt(array);
				}
				PdfStream pdfStream2 = new PdfStream(pdfHash2);
				if (!U.Equals(ref pdfStream2, 16))
				{
					return false;
				}
			}
			else
			{
				pdfStream.Encrypt(pdfHash, nKeyLength / 8);
				if (!U.Equals(ref pdfStream))
				{
					return false;
				}
			}
			byte[] array2 = new byte[nKeyLength / 8];
			Array.Copy(pdfHash.ToBytes(), array2, nKeyLength / 8);
			this.m_pDoc.m_pKey = new PdfStream();
			this.m_pDoc.m_pKey.Set(array2);
			this.m_pDoc.m_bEncrypted = true;
			return true;
		}
		private void ParseCryptoFilters(PdfDict pDict)
		{
			PdfObject objectByName = pDict.GetObjectByName("V");
			if (objectByName != null && objectByName.m_nType == enumType.pdfNumber && ((PdfNumber)objectByName).m_fValue != 4.0)
			{
				return;
			}
			PdfDict pdfDict = (PdfDict)this.RemoveRef(pDict.GetObjectByName("CF"), enumType.pdfDictionary);
			if (pdfDict == null)
			{
				return;
			}
			PdfDict pdfDict2 = pdfDict;
			PdfObject objectByName2 = pDict.GetObjectByName("StmF");
			PdfObject objectByName3 = pDict.GetObjectByName("StrF");
			if (objectByName2 == null || objectByName3 == null || objectByName2.m_nType != enumType.pdfName || objectByName3.m_nType != enumType.pdfName || string.Compare(((PdfName)objectByName2).m_bstrName, ((PdfName)objectByName3).m_bstrName) != 0)
			{
				return;
			}
			string bstrName = ((PdfName)objectByName2).m_bstrName;
			PdfDict pdfDict3 = (PdfDict)this.RemoveRef(pdfDict2.GetObjectByName(bstrName), enumType.pdfDictionary);
			if (pdfDict3 == null)
			{
				return;
			}
			PdfDict pdfDict4 = pdfDict3;
			PdfObject objectByName4 = pdfDict4.GetObjectByName("CFM");
			if (objectByName4 != null && objectByName4.m_nType == enumType.pdfName && ((PdfName)objectByName4).m_bstrName == "AESV2")
			{
				this.m_pDoc.m_pKey.m_nPtr = 1;
			}
		}
		public PdfObject RemoveRef(PdfObject pObj, enumType objType)
		{
			int nObjNum = 0;
			int nGenNum = 0;
			if (pObj == null)
			{
				return null;
			}
			PdfObject pdfObject;
			if (pObj.m_nType == enumType.pdfRef)
			{
				int offset = this.m_pDoc.m_objExistingXRef.GetOffset((PdfRef)pObj);
				this.SetPointer(offset);
				this.ParseObjectHeader(out nObjNum, out nGenNum);
				pdfObject = this.ParseObject(pObj.m_bstrType, nObjNum, nGenNum);
			}
			else
			{
				pdfObject = pObj.Copy();
			}
			if (objType != enumType.pdfNull && pdfObject != null && pdfObject.m_nType != objType)
			{
				this.Throw("Unexpected object type.");
			}
			return pdfObject;
		}
		public int ReadAndSwapInt(int nPtr, out int value)
		{
			uint num;
			int result = this.ReadAndSwapUint(nPtr, out num);
			value = (int)num;
			return result;
		}
		public int ReadAndSwapUint(int nPtr, out uint value)
		{
			byte[] array = new byte[4];
			int result = this.ReadBytes(nPtr, array, 4);
			value = BitConverter.ToUInt32(new byte[]
			{
				array[3],
				array[2],
				array[1],
				array[0]
			}, 0);
			return result;
		}
		public int ReadUint(int nPtr, out uint value)
		{
			byte[] array = new byte[4];
			int result = this.ReadBytes(nPtr, array, 4);
			value = BitConverter.ToUInt32(array, 0);
			return result;
		}
		public int ReadInt(int nPtr, out int value)
		{
			uint num;
			int result = this.ReadUint(nPtr, out num);
			value = (int)num;
			return result;
		}
		public int ReadAndSwapShort(int nPtr, out short value)
		{
			ushort num;
			int result = this.ReadAndSwapUshort(nPtr, out num);
			value = (short)num;
			return result;
		}
		public int ReadAndSwapUshort(int nPtr, out ushort value)
		{
			byte[] array = new byte[2];
			int result = this.ReadBytes(nPtr, array, 2);
			value = BitConverter.ToUInt16(new byte[]
			{
				array[1],
				array[0]
			}, 0);
			return result;
		}
		public int ReadUshort(int nPtr, out ushort value)
		{
			byte[] array = new byte[2];
			int result = this.ReadBytes(nPtr, array, 2);
			value = BitConverter.ToUInt16(array, 0);
			return result;
		}
		public int ReadShort(int nPtr, out short value)
		{
			ushort num;
			int result = this.ReadUshort(nPtr, out num);
			value = (short)num;
			return result;
		}
		public int ReadBytes(int dwFrom, byte[] pBuffer, int dwSize)
		{
			int num;
			if (!this.m_bBinary)
			{
				this.m_objIOStream.Position = (long)dwFrom;
				num = this.m_objIOStream.Read(pBuffer, 0, dwSize);
				if (num == -1)
				{
					this.Throw("File read failed. The error message is: ", PdfErrors._ERROR_INPUT_READ);
				}
				if (num != -1 && dwSize != num)
				{
					this.Throw("Unexpected end of file.");
				}
			}
			else
			{
				this.m_objIOStream.Position = (long)dwFrom;
				if (dwFrom < 0 || dwFrom + dwSize > this.m_nSize)
				{
					this.Throw("Pointer out of range, reading binary info failed.", PdfErrors._ERROR_INPUT_READ);
				}
				num = this.m_objIOStream.Read(pBuffer, 0, dwSize);
			}
			return num;
		}
		private void ReadFromFile()
		{
			this.m_dwBytesRead = this.m_objIOStream.Read(this.m_szBuffer, 0, 1024);
			this.m_dwPtr = 0;
			if (this.m_dwBytesRead > 0)
			{
				this.Next();
			}
		}
		private void SetPointer(int Distance)
		{
			if (this.m_bCompressedObjectsExist && Distance >= this.m_nSize && Distance < this.m_nSize + this.m_objObjStm.Length)
			{
				this.m_dwPtr = Distance;
				this.Next();
				return;
			}
			if (Distance < 0 || Distance >= this.m_nSize)
			{
				this.Throw("Invalid Distance value passed to SetPointer.", PdfErrors._ERROR_INPUT_SETFILEPTR);
			}
			if (Distance >= this.m_dwStartByte && Distance < this.m_dwStartByte + this.m_dwBytesRead)
			{
				this.m_dwPtr = Distance - this.m_dwStartByte;
				this.Next();
				return;
			}
			if (this.m_bMacBinary)
			{
				Distance += this.m_nMacBinarySize;
			}
			this.m_objIOStream.Position = (long)Distance;
			this.m_dwStartByte = Distance;
			if (this.m_bMacBinary)
			{
				this.m_dwStartByte -= this.m_nMacBinarySize;
			}
			this.ReadFromFile();
		}
		public void Throw(string szError, PdfErrors nCode)
		{
			AuxException.Throw(szError, nCode, (int)this.m_objIOStream.Position);
		}
		public void Throw(string szError)
		{
			AuxException.Throw(szError, PdfErrors._ERROR_INPUT, -1);
		}
		internal void SkipWhite()
		{
			while (this.IsWhitespace(this.m_c))
			{
				if (!this.m_bCompressedObjectsExist && this.m_bBinary && this.m_dwPtr >= this.m_dwBytesRead)
				{
					return;
				}
				this.Next();
			}
			if (this.m_c == 37)
			{
				byte[] array = new byte[5];
				byte[] array2 = array;
				int num = 0;
				do
				{
					this.Next();
					array2[num++] = this.m_c;
					if (num == 4 && string.Compare(Encoding.UTF8.GetString(array2, 0, 5), "%EOF") == 0)
					{
						return;
					}
					if (num > 4)
					{
						num = 0;
					}
					if (this.m_c == 13)
					{
						break;
					}
				}
				while (this.m_c != 10);
				IL_B6:
				while (this.IsWhitespace(this.m_c) && (!this.m_bBinary || this.m_dwPtr < this.m_dwBytesRead))
				{
					this.Next();
				}
				if (this.m_c == 37)
				{
					this.SkipWhite();
					return;
				}
				return;
				goto IL_B6;
			}
		}
		public PdfObject ParseObject(string Type, int nObjNum, int nGenNum)
		{
			char c = (char)this.m_c;
			if (c <= '/')
			{
				if (c == '(')
				{
					this.Next();
					return this.ParseString(Type, nObjNum, nGenNum);
				}
				if (c == '/')
				{
					this.Next();
					return this.ParseName(Type);
				}
			}
			else
			{
				if (c != '<')
				{
					if (c == '[')
					{
						this.Next();
						return this.ParseArray(Type, nObjNum, nGenNum);
					}
				}
				else
				{
					this.Next();
					if (this.m_c == 60)
					{
						this.Next();
						return this.ParseDict(Type, nObjNum, nGenNum);
					}
					return this.ParseBinaryString(Type, nObjNum, nGenNum);
				}
			}
			return this.ParseNumberEtc(Type);
		}
		private PdfArray ParseArray(string Type, int nObjNum, int nGenNum)
		{
			this.SkipWhite();
			PdfArray pdfArray = new PdfArray(Type);
			while (this.m_c != 93)
			{
				PdfObject newObj = this.ParseObject(null, nObjNum, nGenNum);
				pdfArray.Add(newObj);
				this.SkipWhite();
			}
			this.Next();
			this.SkipWhite();
			return pdfArray;
		}
		private PdfDict ParseDict(string Type, int nObjNum, int nGenNum)
		{
			this.SkipWhite();
			PdfDict pdfDict = new PdfDict(Type);
			while (true)
			{
				if (this.m_c == 62)
				{
					this.Next();
					if (this.m_c == 62)
					{
						break;
					}
					this.Throw("Unexpected '>' character.");
				}
				PdfObject pdfObject = this.ParseObject(Type, nObjNum, nGenNum);
				if (!this.m_bIgnoreDef || pdfObject.m_nType != enumType.pdfOther || !(((PdfOther)pdfObject).m_bstrValue == "def"))
				{
					if (pdfObject.m_nType != enumType.pdfName)
					{
						this.Throw("A dictionary entry must start with a name.");
					}
					string bstrName = ((PdfName)pdfObject).m_bstrName;
					this.SkipWhite();
					PdfObject newObj = this.ParseObject(bstrName, nObjNum, nGenNum);
					pdfDict.Add(newObj);
					this.SkipWhite();
				}
			}
			this.Next();
			this.SkipWhite();
			return pdfDict;
		}
		private PdfString ParseString(string Type, int nObjNum, int nGenNum)
		{
			int num = 0;
            PdfString pdfString = new PdfString(Type, (string)null);
			while (true)
			{
				if (this.m_c == 40)
				{
					num++;
				}
				else
				{
					if (this.m_c == 41)
					{
						num--;
					}
					else
					{
						if (this.m_c == 92)
						{
							bool flag = false;
							this.Next();
							char c = (char)this.m_c;
							if (c <= '\\')
							{
								if (c <= '\r')
								{
									if (c == '\n')
									{
										flag = true;
										this.Next();
										goto IL_1EC;
									}
									if (c == '\r')
									{
										flag = true;
										this.Next();
										if (this.m_c == 10)
										{
											this.Next();
											goto IL_1EC;
										}
										goto IL_1EC;
									}
								}
								else
								{
									switch (c)
									{
									case '(':
									case ')':
										goto IL_1EC;
									default:
										if (c == '\\')
										{
											goto IL_1EC;
										}
										break;
									}
								}
							}
							else
							{
								if (c <= 'f')
								{
									if (c == 'b')
									{
										this.m_c = 8;
										goto IL_1EC;
									}
									if (c == 'f')
									{
										this.m_c = 12;
										goto IL_1EC;
									}
								}
								else
								{
									if (c == 'n')
									{
										this.m_c = 10;
										goto IL_1EC;
									}
									switch (c)
									{
									case 'r':
										this.m_c = 13;
										goto IL_1EC;
									case 't':
										this.m_c = 9;
										goto IL_1EC;
									}
								}
							}
							if (this.m_c >= 48 && this.m_c <= 55)
							{
								int num2 = (int)(this.m_c - 48);
								this.Next();
								if (this.m_c < 48 || this.m_c > 55)
								{
									pdfString.Append(new byte[]
									{
										(byte)num2
									});
									continue;
								}
								num2 = (num2 << 3) + (int)this.m_c - 48;
								this.Next();
								if (this.m_c < 48 || this.m_c > 55)
								{
									pdfString.Append(new byte[]
									{
										(byte)num2
									});
									continue;
								}
								num2 = (num2 << 3) + (int)this.m_c - 48;
								this.m_c = (byte)(num2 & 255);
							}
							IL_1EC:
							if (flag)
							{
								continue;
							}
						}
						else
						{
							if (this.m_c == 13)
							{
								this.Next();
								if (this.m_c == 10)
								{
									this.Next();
								}
								char c2 = '\n';
								pdfString.Append(new byte[]
								{
									(byte)c2
								});
								continue;
							}
						}
					}
				}
				if (num == -1)
				{
					break;
				}
				pdfString.Append(new byte[]
				{
					this.m_c
				});
				this.Next();
			}
			this.Next();
			pdfString.Decrypt(this.m_bInsideCompressedObjectExtension ? null : this.m_pDoc.m_pKey, nObjNum, nGenNum);
			return pdfString;
		}
		private PdfString ParseBinaryString(string Type, int nObjNum, int nGenNum)
		{
            PdfString pdfString = new PdfString(Type, (string)null);
			int num;
			int num3;
			while (true)
			{
				this.SkipWhite();
				if (this.m_c == 62)
				{
					goto IL_88;
				}
				num = PdfInput.FromHex(this.m_c);
				this.Next();
				this.SkipWhite();
				if (this.m_c == 62)
				{
					break;
				}
				int num2 = PdfInput.FromHex(this.m_c);
				num3 = (num << 4) + num2;
				pdfString.Append(new byte[]
				{
					(byte)num3
				});
				this.Next();
			}
			num3 = num << 4;
			pdfString.Append(new byte[]
			{
				(byte)num3
			});
			IL_88:
			this.Next();
			this.SkipWhite();
			pdfString.Decrypt(this.m_bInsideCompressedObjectExtension ? null : this.m_pDoc.m_pKey, nObjNum, nGenNum);
			return pdfString;
		}
		public void Next()
		{
			if (this.m_bCompressedObjectsExist && this.m_dwPtr >= this.m_nSize && this.m_dwPtr < this.m_nSize + this.m_objObjStm.Length)
			{
				this.m_c = this.m_objObjStm[this.m_dwPtr - this.m_nSize];
				this.m_dwPtr++;
				this.m_bInsideCompressedObjectExtension = true;
				return;
			}
			this.m_bInsideCompressedObjectExtension = false;
			if (this.m_dwPtr >= this.m_dwBytesRead)
			{
				this.m_dwStartByte += this.m_dwBytesRead;
				this.ReadFromFile();
				if (this.m_dwBytesRead == 0)
				{
					this.Throw("End of file reached.", PdfErrors._ERROR_INPUT_EOF);
				}
				return;
			}
			this.m_c = this.m_szBuffer[this.m_dwPtr++];
		}
		private PdfObject ParseNumberEtc(string Type)
		{
			return this.ParseNumberEtc(Type, 0);
		}
		private PdfObject ParseNumberEtc(string Type, int nLevel)
		{
			this.m_bstrValue = new StringBuilder();
			this.SkipWhite();
			if (this.m_c == 45 || this.m_c == 43 || this.m_c == 46 || (this.m_c >= 48 && this.m_c <= 57))
			{
				do
				{
					this.Append(this.m_c);
					this.Next();
				}
				while ((this.m_c >= 48 && this.m_c <= 57) || this.m_c == 46);
				this.SkipWhite();
				if (nLevel != 0)
				{
					return new PdfNumber(Type, this.m_bstrValue.ToString());
				}
				int pointer = this.m_dwStartByte + this.m_dwPtr - 1;
				if (this.m_bCompressedObjectsExist && this.m_dwPtr >= this.m_nSize)
				{
					pointer = this.m_dwPtr - 1;
				}
				string value = this.m_bstrValue.ToString();
				this.m_nReference = (int)float.Parse(this.m_bstrValue.ToString(), NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
				PdfObject pdfObject = this.ParseNumberEtc(Type, nLevel + 1);
				if (pdfObject.m_nType != enumType.pdfNumber)
				{
					this.SetPointer(pointer);
					return new PdfNumber(Type, value);
				}
				this.m_nGeneration = (int)float.Parse(this.m_bstrValue.ToString(), NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
				if (this.m_bBinary && !this.m_bCompressedObjectsExist && this.m_dwPtr >= this.m_dwBytesRead)
				{
					this.SetPointer(pointer);
					return new PdfNumber(Type, value);
				}
				PdfObject pdfObject2 = this.ParseNumberEtc(Type, nLevel + 1);
				if (pdfObject2.m_nType == enumType.pdfOther && ((PdfOther)pdfObject2).m_bstrValue == "R")
				{
					return new PdfRef(Type, this.m_nReference, this.m_nGeneration);
				}
				this.SetPointer(pointer);
				return new PdfNumber(Type, value);
			}
			else
			{
				if (this.m_c == 40)
				{
					return new PdfOther(Type, null, null);
				}
				do
				{
					this.Append(this.m_c);
					this.Next();
				}
				while (!this.delims[(int)(this.m_c + 1)]);
				this.SkipWhite();
				string text = this.m_bstrValue.ToString();
				string text2 = text.ToLower();
				if (text2 == "true")
				{
					return new PdfBool(Type, true);
				}
				if (text2 == "false")
				{
					return new PdfBool(Type, false);
				}
				if (text2 == "null")
				{
					return new PdfNull(Type);
				}
				if (text2 == "r")
				{
					return new PdfOther(Type, "R", "R");
				}
				return new PdfOther(Type, text2, text);
			}
		}
		private PdfName ParseName(string Type)
		{
			this.m_bstrValue = new StringBuilder();
			while (!this.delims[(int)(this.m_c + 1)])
			{
				if (this.m_c == 35)
				{
					this.Next();
					byte b = (byte)(PdfInput.FromHex(this.m_c) << 4);
					this.Next();
					b += (byte)PdfInput.FromHex(this.m_c);
					this.Append(b);
				}
				else
				{
					this.Append(this.m_c);
				}
				this.Next();
			}
			this.SkipWhite();
			return new PdfName(Type, this.m_bstrValue.ToString());
		}
		private void ParseObjectHeader(out int ObjNum, out int GenNum)
		{
			int num = this.ParseNextInt("Object number not found in object header.");
			int num2 = this.ParseNextInt("Generation number not found in object header.");
			this.ParseNextWord("obj");
			ObjNum = num;
			GenNum = num2;
		}
		private void Append(byte c)
		{
			if (c != 0)
			{
				this.m_bstrValue.Append((char)c);
			}
		}
		public static int FromHex(byte c)
		{
			if (c >= 48 && c <= 57)
			{
				return (int)(c - 48);
			}
			if (c >= 65 && c <= 70)
			{
				return (int)(c - 65 + 10);
			}
			if (c >= 97 && c <= 102)
			{
				return (int)(c - 97 + 10);
			}
			return 0;
		}
		private int StrStrLast(byte[] szBuffer, int nSize, string szLittleStr)
		{
			return this.StrStrLast(szBuffer, nSize, Encoding.UTF8.GetBytes(szLittleStr));
		}
		private int StrStrLast(byte[] szBuffer, int nSize, byte[] szLittleStr)
		{
			int num = szLittleStr.Length;
			if (num == 0)
			{
				return 0;
			}
			int result = -1;
			for (int i = nSize - num; i >= 0; i--)
			{
				if (Convert.ToBase64String(szBuffer, i, num) == Convert.ToBase64String(szLittleStr))
				{
					result = i;
					break;
				}
			}
			return result;
		}
		private void ParseNextWordNoWhiteSpace(string wszExpectedWord)
		{
			if (wszExpectedWord == null)
			{
				return;
			}
			int length = wszExpectedWord.Length;
			int num = 0;
			while (num < length && wszExpectedWord[num] == (char)this.m_c)
			{
				num++;
				this.Next();
			}
			if (num == length)
			{
				return;
			}
			this.Throw(string.Format("The word '{0}' not found.", wszExpectedWord));
		}
		private string ParseNextWord(string wszExpectedWord)
		{
			return this.ParseNextWord(wszExpectedWord, true);
		}
		private string ParseNextWord(string wszExpectedWord, bool bThrowException)
		{
			string szError = string.Format("'{0}' word not found.", wszExpectedWord);
			PdfObject pdfObject = this.ParseNumberEtc(null, 1);
			if (pdfObject.m_nType != enumType.pdfOther)
			{
				if (!bThrowException)
				{
					return "";
				}
				this.Throw(szError, PdfErrors._ERROR_INPUT_1);
			}
			string bstrValue = ((PdfOther)pdfObject).m_bstrValue;
			if (wszExpectedWord != null && string.Compare(bstrValue, wszExpectedWord) != 0)
			{
				this.Throw(szError, PdfErrors._ERROR_INPUT_2);
			}
			return bstrValue;
		}
		private int ParseNextInt(string wszDefaultError)
		{
			string text = "";
			return this.ParseNextInt(wszDefaultError, false, ref text);
		}
		private int ParseNextInt(string wszDefaultError, bool bReturnActualWord, ref string pActualWord)
		{
			PdfObject pdfObject = this.ParseNumberEtc(null, 1);
			if (pdfObject.m_nType == enumType.pdfNumber)
			{
				return (int)((PdfNumber)pdfObject).m_fValue;
			}
			if (bReturnActualWord)
			{
				pActualWord = this.m_bstrValue.ToString();
			}
			else
			{
				this.Throw((wszDefaultError != null) ? wszDefaultError : "An integer expected.");
			}
			return -999;
		}
		public void SkipTo(byte[] szSubstr)
		{
			if (szSubstr == null)
			{
				return;
			}
			int num = szSubstr.Length;
			while (this.m_dwPtr + num < this.m_dwBytesRead)
			{
				if (Convert.ToBase64String(this.m_szBuffer, this.m_dwPtr, num) == Convert.ToBase64String(szSubstr) && this.IsWhitespace(this.m_szBuffer[this.m_dwPtr + num]))
				{
					this.m_dwPtr += num + 1;
					this.Next();
					return;
				}
				this.Next();
			}
		}
		internal bool IsWhitespace(byte ch)
		{
			return ch == 0 || ch == 9 || ch == 10 || ch == 12 || ch == 13 || ch == 32;
		}
		internal bool IsDelimiter(byte ch)
		{
			return ch == 40 || ch == 41 || ch == 60 || ch == 62 || ch == 91 || ch == 93 || ch == 47 || ch == 37;
		}
		internal bool IsDelimiterWhitespace(byte ch)
		{
			return this.delims[(int)(ch + 1)];
		}
	}
}
