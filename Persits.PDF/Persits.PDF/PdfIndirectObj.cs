using System;
using System.Collections.Generic;
namespace Persits.PDF
{
	internal class PdfIndirectObj : PdfObject
	{
		public bool m_bEncryptable;
		public bool m_bExisting;
		public bool m_bModified;
		public bool m_bDeleted;
		public new enumIndirectType m_nType;
		public int m_nObjNumber;
		public int m_nGenNumber;
		public PdfDict m_objAttributes = new PdfDict();
		public PdfStream m_objStream = new PdfStream();
		public PdfObject m_pSimpleValue;
		public List<PdfObjectList> m_arrBackPointers = new List<PdfObjectList>();
		public PdfArray m_pParentArrayOfKids;
		public PdfManager m_pManager;
		internal PdfIndirectObj()
		{
			this.m_nObjNumber = 0;
			this.m_nGenNumber = 0;
			this.m_bEncryptable = true;
			this.m_objAttributes.m_bTerminate = true;
			this.m_bExisting = false;
			this.m_bModified = true;
			this.m_bDeleted = false;
			this.m_pParentArrayOfKids = null;
			this.m_objAttributes.m_pParent = this;
			this.m_objStream.Alloc(10);
		}
		public override PdfObject Copy()
		{
			return null;
		}
		public override int WriteOut(PdfOutput pOutput)
		{
			int result = 0;
			string objString = string.Format("{0} {1} obj\n", this.m_nObjNumber, this.m_nGenNumber);
			pOutput.Write(objString, ref result);
			bool flag = this.m_nType == enumIndirectType.pdfIndirectContents || this.m_nType == enumIndirectType.pdfIndirectImage || this.m_nType == enumIndirectType.pdfIndirectFontFile || this.m_nType == enumIndirectType.pdfIndirectToUnicode || this.m_nType == enumIndirectType.pdfIndirectXObject || this.m_nType == enumIndirectType.pdfIndirectStream || this.m_nType == enumIndirectType.pdfIndirectEmbeddedFile;
			if ((this.m_nType == enumIndirectType.pdfIndirectContents || this.m_nType == enumIndirectType.pdfIndirectXObject) && this.m_objStream.Length > 50 && this.m_objAttributes.GetObjectByName("Filter") == null)
			{
				this.m_objStream.EncodeFlate();
				this.m_objAttributes.Add(new PdfName("Filter", "FlateDecode"));
			}
			if (this.m_objStream.Length > 0 || flag)
			{
				int num = this.m_objStream.Length;
				if (pOutput.m_bEncrypt && pOutput.m_pEncKey.m_nPtr == 1)
				{
					num += 16 + (16 - num % 16);
				}
				PdfObject objectByName = this.m_objAttributes.GetObjectByName("Length");
				if (objectByName != null)
				{
					if (objectByName.m_nType == enumType.pdfNumber)
					{
						((PdfNumber)objectByName).m_fValue = (double)num;
					}
				}
				else
				{
					this.m_objAttributes.Add(new PdfNumber("Length", (double)num));
				}
			}
			this.m_objAttributes.WriteOut(pOutput);
			if (this.m_pSimpleValue != null)
			{
				this.m_pSimpleValue.WriteOut(pOutput);
				if (this.m_pSimpleValue.m_nType == enumType.pdfName || this.m_pSimpleValue.m_nType == enumType.pdfNumber || this.m_pSimpleValue.m_nType == enumType.pdfArray || this.m_pSimpleValue.m_nType == enumType.pdfNull)
				{
					pOutput.Write("\n", ref result);
				}
			}
			else
			{
				if (this.m_objAttributes.Size == 0 && !flag)
				{
					pOutput.Write("<<>>\n", ref result);
				}
			}
			if (flag)
			{
				this.m_objStream.WriteOut(pOutput);
			}
			pOutput.Write("endobj\n\n", ref result);
			if (this.m_nType == enumIndirectType.pdfIndirectSignature)
			{
				pOutput.m_nSignatureLocation = pOutput.CurrentLocation();
			}
			return result;
		}
		public void AddName(string Name, string Value)
		{
			PdfObject objectByName = this.m_objAttributes.GetObjectByName(Name);
			if (objectByName != null)
			{
				if (objectByName.m_nType == enumType.pdfName)
				{
					((PdfName)objectByName).m_bstrName = Value;
					return;
				}
			}
			else
			{
				this.m_objAttributes.Add(new PdfName(Name, Value));
			}
		}
		public void AddString(string Name, string Value)
		{
			this.m_objAttributes.Add(new PdfString(Name, Value));
		}
		public void AddString(string Name, PdfStream Value)
		{
			this.m_objAttributes.Add(new PdfString(Name, Value));
		}
		public void AddReference(string Name, PdfObject pPtr)
		{
			PdfObject objectByName = this.m_objAttributes.GetObjectByName(Name);
			if (objectByName != null)
			{
				if (objectByName.m_nType == enumType.pdfReference)
				{
					((PdfReference)objectByName).m_pPtr = pPtr;
					return;
				}
			}
			else
			{
				this.m_objAttributes.Add(new PdfReference(Name, pPtr));
			}
		}
		public void AddArrayOfRef(string Name, PdfObject[] ppPtr)
		{
			PdfArray pdfArray = new PdfArray(Name, ppPtr);
			pdfArray.m_pParent = this;
			this.m_objAttributes.Add(pdfArray);
		}
		public PdfArray AddArray(string Name)
		{
			PdfObject objectByName = this.m_objAttributes.GetObjectByName(Name);
			if (objectByName == null)
			{
				PdfArray pdfArray = new PdfArray(Name);
				pdfArray.m_pParent = this;
				this.m_objAttributes.Add(pdfArray);
				return pdfArray;
			}
			return (PdfArray)objectByName;
		}
		public PdfDict AddDict(string Name)
		{
			PdfObject objectByName = this.m_objAttributes.GetObjectByName(Name);
			if (objectByName == null)
			{
				PdfDict pdfDict = new PdfDict(Name);
				pdfDict.m_pParent = this;
				this.m_objAttributes.Add(pdfDict);
				return pdfDict;
			}
			return (PdfDict)objectByName;
		}
		public void AddDate(string Name, DateTime Value)
		{
			if (Value.ToOADate() == 0.0)
			{
				return;
			}
			this.m_objAttributes.Add(new PdfDate(Name, Value));
		}
		public void AddBool(string Name, bool Value)
		{
			this.m_objAttributes.Add(new PdfBool(Name, Value));
		}
		public void AddInt(string Name, int Value)
		{
			PdfObject objectByName = this.m_objAttributes.GetObjectByName(Name);
			if (objectByName != null)
			{
				if (objectByName.m_nType == enumType.pdfNumber)
				{
					((PdfNumber)objectByName).m_fValue = (double)Value;
					return;
				}
			}
			else
			{
				this.m_objAttributes.Add(new PdfNumber(Name, (double)Value));
			}
		}
		public void AddNumber(string Name, float Value)
		{
			PdfObject objectByName = this.m_objAttributes.GetObjectByName(Name);
			if (objectByName != null)
			{
				if (objectByName.m_nType == enumType.pdfNumber)
				{
					((PdfNumber)objectByName).m_fValue = (double)Value;
					return;
				}
			}
			else
			{
				this.m_objAttributes.Add(new PdfNumber(Name, (double)Value));
			}
		}
		public void ChangeInt(string Name, int ChangeBy)
		{
			PdfObject pdfObject = this.m_objAttributes.GetObjectByName(Name);
			if (pdfObject == null)
			{
				pdfObject = new PdfNumber(Name, 0.0);
				this.m_objAttributes.Add(pdfObject);
			}
			if (pdfObject.m_nType == enumType.pdfNumber)
			{
				((PdfNumber)pdfObject).m_fValue += (double)ChangeBy;
			}
		}
	}
}
