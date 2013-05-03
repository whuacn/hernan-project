using System;
namespace Persits.PDF
{
	public class PdfSignature
	{
		internal PdfManager m_pManager;
		internal PdfDocument m_pDoc;
		internal bool m_bPopulated;
		internal int m_dwFrom1;
		internal int m_dwLength1;
		internal int m_dwFrom2;
		internal int m_dwLength2;
		internal string m_bstrName;
		internal string m_bstrReason;
		internal string m_bstrLocation;
		internal string m_bstrFilter;
		internal string m_bstrSubFilter;
		internal DateTime m_dtModified;
		internal bool m_bStatus;
		internal PdfString m_objContents = new PdfString();
		public string Name
		{
			get
			{
				return this.m_bstrName;
			}
		}
		public string Reason
		{
			get
			{
				return this.m_bstrReason;
			}
		}
		public string Location
		{
			get
			{
				return this.m_bstrLocation;
			}
		}
		public string Contents
		{
			get
			{
				PdfStream pdfStream = new PdfStream();
				pdfStream.Append(this.m_objContents.ToBytes());
				return pdfStream.ToString();
			}
		}
		public string Filter
		{
			get
			{
				return this.m_bstrFilter;
			}
		}
		public string SubFilter
		{
			get
			{
				return this.m_bstrSubFilter;
			}
		}
		public bool Status
		{
			get
			{
				return this.m_bStatus;
			}
		}
		public int From1
		{
			get
			{
				return this.m_dwFrom1;
			}
		}
		public int From2
		{
			get
			{
				return this.m_dwFrom2;
			}
		}
		public int Length1
		{
			get
			{
				return this.m_dwLength1;
			}
		}
		public int Length2
		{
			get
			{
				return this.m_dwLength2;
			}
		}
		public DateTime Modified
		{
			get
			{
				return this.m_dtModified;
			}
		}
		internal PdfSignature()
		{
			this.m_pDoc = null;
			this.m_pManager = null;
			this.m_bPopulated = false;
			this.m_dwFrom1 = (this.m_dwLength1 = (this.m_dwFrom2 = (this.m_dwLength2 = 0)));
			this.m_bStatus = false;
		}
		internal void Populate(PdfDict pDict, int nObjNum, int nGenNum)
		{
			PdfObject objectByName = pDict.GetObjectByName("Filter");
			if (objectByName != null && objectByName.m_nType == enumType.pdfName)
			{
				this.m_bstrFilter = ((PdfName)objectByName).m_bstrName;
				if (!(this.m_bstrFilter == "VeriSign.PPKVS") && !(this.m_bstrFilter == "Adobe.PPKMS") && !(this.m_bstrFilter == "Adobe.PPKLite"))
				{
					return;
				}
			}
			PdfObject objectByName2 = pDict.GetObjectByName("ByteRange");
			if (objectByName2 == null || objectByName2.m_nType != enumType.pdfArray)
			{
				return;
			}
			PdfArray pdfArray = (PdfArray)objectByName2;
			if (pdfArray.Size != 4)
			{
				return;
			}
			int[] array = new int[4];
			for (int i = 0; i < 4; i++)
			{
				PdfObject @object = pdfArray.GetObject(i);
				if (@object != null && @object.m_nType == enumType.pdfNumber)
				{
					array[i] = (int)((PdfNumber)@object).m_fValue;
				}
			}
			if (this.m_bPopulated && this.m_dwLength1 + this.m_dwLength2 > array[1] + array[3])
			{
				return;
			}
			this.m_dwFrom1 = array[0];
			this.m_dwLength1 = array[1];
			this.m_dwFrom2 = array[2];
			this.m_dwLength2 = array[3];
			PdfObject objectByName3 = pDict.GetObjectByName("SubFilter");
			if (objectByName3 != null && objectByName3.m_nType == enumType.pdfName)
			{
				this.m_bstrSubFilter = ((PdfName)objectByName3).m_bstrName;
			}
			PdfObject objectByName4 = pDict.GetObjectByName("Name");
			if (objectByName4 != null && objectByName4.m_nType == enumType.pdfString)
			{
				this.m_bstrName = ((PdfString)objectByName4).ToString();
			}
			PdfObject objectByName5 = pDict.GetObjectByName("Reason");
			if (objectByName5 != null && objectByName5.m_nType == enumType.pdfString)
			{
				this.m_bstrReason = ((PdfString)objectByName5).ToString();
			}
			PdfObject objectByName6 = pDict.GetObjectByName("Location");
			if (objectByName6 != null && objectByName6.m_nType == enumType.pdfString)
			{
				this.m_bstrLocation = ((PdfString)objectByName6).ToString();
			}
			PdfObject objectByName7 = pDict.GetObjectByName("M");
			if (objectByName7 != null && objectByName7.m_nType == enumType.pdfString)
			{
				this.m_dtModified = ((PdfString)objectByName7).ToDate();
			}
			PdfObject objectByName8 = pDict.GetObjectByName("Contents");
			if (objectByName8 != null && objectByName8.m_nType == enumType.pdfString)
			{
				this.m_objContents.Set((PdfString)objectByName8);
			}
			this.m_bPopulated = true;
		}
	}
}
