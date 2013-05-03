using System;
namespace Persits.PDF
{
	public class PdfGraphics
	{
		internal PdfManager m_pManager;
		internal PdfDocument m_pDoc;
		internal string m_bstrID;
		internal float m_fLeft;
		internal float m_fRight;
		internal float m_fTop;
		internal float m_fBottom;
		internal PdfCanvas m_ptrCanvas;
		internal PdfIndirectObj m_pXObj;
		internal bool m_bPattern;
		public PdfCanvas Canvas
		{
			get
			{
				if (this.m_ptrCanvas != null)
				{
					return this.m_ptrCanvas;
				}
				PdfCanvas pdfCanvas = new PdfCanvas();
				pdfCanvas.m_pManager = this.m_pManager;
				pdfCanvas.m_pDoc = this.m_pDoc;
				this.m_ptrCanvas = pdfCanvas;
				pdfCanvas.m_pContentsObj = this.m_pXObj;
				pdfCanvas.m_pResourceObj = this.m_pXObj;
				return pdfCanvas;
			}
		}
		internal PdfGraphics()
		{
		}
		internal void Create(PdfParam pParam)
		{
			if (pParam.IsSet("Left") && pParam.IsSet("Right") && pParam.IsSet("Top") && pParam.IsSet("Bottom"))
			{
				this.m_fLeft = pParam.Number("Left");
				this.m_fRight = pParam.Number("Right");
				this.m_fTop = pParam.Number("Top");
				this.m_fBottom = pParam.Number("Bottom");
			}
			else
			{
				AuxException.Throw("Left, Bottom, Right and Top arguments required.", PdfErrors._ERROR_INVALIDARG);
			}
			this.m_pXObj = this.m_pDoc.AddNewIndirectObject(enumIndirectType.pdfIndirectXObject);
			this.m_pXObj.AddName("Type", "XObject");
			this.m_pXObj.AddName("Subtype", "Form");
			this.m_pXObj.AddInt("FormType", 1);
			PdfArray pdfArray = this.m_pXObj.AddArray("BBox");
			pdfArray.Add(new PdfNumber(null, (double)this.m_fLeft));
			pdfArray.Add(new PdfNumber(null, (double)this.m_fBottom));
			pdfArray.Add(new PdfNumber(null, (double)this.m_fRight));
			pdfArray.Add(new PdfNumber(null, (double)this.m_fTop));
			float[] array = new float[6];
			array[0] = 1f;
			array[3] = 1f;
			float[] array2 = array;
			PdfArray pdfArray2 = this.m_pXObj.AddArray("Matrix");
			for (char c = 'a'; c <= 'f'; c += '\u0001')
			{
				string name = c.ToString();
				if (pParam.IsSet(name))
				{
					array2[(int)(c - 'a')] = pParam.Number(name);
				}
				pdfArray2.Add(new PdfNumber(null, (double)array2[(int)(c - 'a')]));
			}
			PdfImage pdfImage = new PdfImage();
			pdfImage.m_pDoc = this.m_pDoc;
			pdfImage.m_pManager = this.m_pManager;
			pdfImage.SetIndex();
			this.m_bstrID = pdfImage.m_bstrID;
			if (pParam.IsTrue("Pattern"))
			{
				this.m_pXObj.AddName("Type", "Pattern");
				this.m_pXObj.AddInt("PatternType", 1);
				this.m_pXObj.AddInt("PaintType", 1);
				this.m_pXObj.AddInt("TilingType", 1);
				this.m_pXObj.AddNumber("XStep", this.m_fRight - this.m_fLeft);
				this.m_pXObj.AddNumber("YStep", this.m_fTop - this.m_fBottom);
				this.m_bPattern = true;
			}
		}
	}
}
