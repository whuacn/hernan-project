using System;
using System.Text;
namespace Persits.PDF
{
	internal class PdfOutput : PdfStream
	{
		internal int m_nSignatureLocation = -1;
		public bool m_bEncrypt;
		public PdfStream m_pMasterKey;
		public PdfStream m_pEncKey;
		public PdfOutput(int InitialSize)
		{
			base.Alloc(InitialSize);
			this.m_bEncrypt = false;
			this.m_pMasterKey = null;
			this.m_pEncKey = null;
			this.m_nSignatureLocation = -1;
			this.m_nSignatureLocation = this.m_nSignatureLocation;
		}
		~PdfOutput()
		{
			this.m_pEncKey = null;
			this.m_pMasterKey = null;
		}
		public void Write(string objString, ref int NewSize)
		{
			this.Write(Encoding.UTF8.GetBytes(objString), ref NewSize);
		}
		public void Write(int Number, ref int NewSize)
		{
			this.Write(string.Format("{0}", Number), ref NewSize);
		}
		public void Write(double Number, ref int NewSize)
		{
			PdfNumber pdfNumber = new PdfNumber(null, Number);
			this.Write(pdfNumber.ToString(), ref NewSize);
		}
		public void Write(PdfStream stream, ref int NewSize)
		{
			this.Write(stream.ToBytes(), ref NewSize);
		}
		public virtual void Write(byte[] objString, ref int NewSize)
		{
			base.Append(objString);
			NewSize += base.Length;
		}
		public virtual void Write(byte[] objString, int nStart, int nCount, ref int NewSize)
		{
			base.Append(objString, nStart, nCount);
			NewSize += base.Length;
		}
		public virtual void Flush()
		{
		}
		public virtual int CurrentLocation()
		{
			return base.Length;
		}
		public virtual void Open()
		{
		}
		public virtual void Close()
		{
		}
		public void SetObjectID(long ObjNumber, long GenNumber)
		{
			byte[] pBuffer = new byte[]
			{
				115,
				65,
				108,
				84
			};
			if (!this.m_bEncrypt)
			{
				return;
			}
			PdfHash pdfHash = new PdfHash();
			pdfHash.Init();
			pdfHash.Update(this.m_pMasterKey);
			byte[] bytes = BitConverter.GetBytes(ObjNumber);
			byte[] bytes2 = BitConverter.GetBytes(GenNumber);
			pdfHash.Update(bytes, 3u);
			pdfHash.Update(bytes2, 2u);
			if (this.m_pMasterKey.m_nPtr == 1)
			{
				pdfHash.Update(pBuffer, 4u);
			}
			pdfHash.Final();
			int num = this.m_pMasterKey.Length + 5;
			if (num > 16)
			{
				num = 16;
			}
			byte[] array = new byte[num];
			Array.Copy(pdfHash.m_objMemStream.ToArray(), array, num);
			this.m_pEncKey.Set(array);
		}
	}
}
