using System;
namespace Persits.PDF
{
	public class PdfDest
	{
		internal bool m_bXYZ;
		internal bool m_bFit;
		internal bool m_bFitB;
		internal bool m_bFitV;
		internal bool m_bFitH;
		internal bool m_bFitR;
		internal float m_fZoom;
		internal float m_fLeft;
		internal float m_fTop;
		internal float m_fRight;
		internal float m_fBottom;
		internal PdfManager m_pManager;
		internal PdfDocument m_pDoc;
		internal PdfPage m_ptrPage;
		internal string m_bstrName = "";
		internal PdfDest()
		{
			this.m_bXYZ = (this.m_bFit = (this.m_bFitB = (this.m_bFitV = (this.m_bFitH = (this.m_bFitR = false)))));
			this.m_fZoom = (this.m_fLeft = (this.m_fTop = (this.m_fRight = (this.m_fBottom = -1f))));
		}
		internal void Populate(PdfArray pArray, bool bUsePageIndex)
		{
			if (pArray == null)
			{
				return;
			}
			PdfPage ptrPage = this.m_ptrPage;
			if (bUsePageIndex)
			{
				pArray.Add(new PdfNumber(null, (double)(ptrPage.m_nIndex - 1)));
			}
			else
			{
				pArray.Add(new PdfReference(null, ptrPage.m_pPageObj));
			}
			if (this.m_bXYZ)
			{
				pArray.Add(new PdfName(null, "XYZ"));
				if (this.m_fLeft > -1f)
				{
					pArray.Add(new PdfNumber(null, (double)this.m_fLeft));
				}
				else
				{
					pArray.Add(new PdfNull(null));
				}
				if (this.m_fTop > -1f)
				{
					pArray.Add(new PdfNumber(null, (double)this.m_fTop));
				}
				else
				{
					pArray.Add(new PdfNull(null));
				}
				if (this.m_fZoom > -1f)
				{
					pArray.Add(new PdfNumber(null, (double)this.m_fZoom));
					return;
				}
				pArray.Add(new PdfNull(null));
				return;
			}
			else
			{
				if (this.m_bFitH)
				{
					pArray.Add(new PdfName(null, this.m_bFitB ? "FitBH" : "FitH"));
					if (this.m_fTop > -1f)
					{
						pArray.Add(new PdfNumber(null, (double)this.m_fTop));
						return;
					}
					pArray.Add(new PdfNull(null));
					return;
				}
				else
				{
					if (this.m_bFitV)
					{
						pArray.Add(new PdfName(null, this.m_bFitB ? "FitBV" : "FitV"));
						if (this.m_fLeft > -1f)
						{
							pArray.Add(new PdfNumber(null, (double)this.m_fLeft));
							return;
						}
						pArray.Add(new PdfNull(null));
						return;
					}
					else
					{
						if (this.m_bFitR)
						{
							pArray.Add(new PdfName(null, "FitR"));
							pArray.Add(new PdfNumber(null, (double)this.m_fLeft));
							pArray.Add(new PdfNumber(null, (double)this.m_fBottom));
							pArray.Add(new PdfNumber(null, (double)this.m_fRight));
							pArray.Add(new PdfNumber(null, (double)this.m_fTop));
							return;
						}
						pArray.Add(new PdfName(null, this.m_bFitB ? "FitB" : "Fit"));
						return;
					}
				}
			}
		}
		internal void Create(PdfPage Page, PdfParam pParam)
		{
			if (Page == null)
			{
				AuxException.Throw("Page argument is empty.", PdfErrors._ERROR_INVALIDARG);
			}
			this.m_ptrPage = Page;
			if (pParam == null)
			{
				return;
			}
			if (pParam.IsSet("XYZ"))
			{
				this.m_bXYZ = pParam.Bool("XYZ");
			}
			if (pParam.IsSet("Zoom"))
			{
				this.m_fZoom = pParam.Number("Zoom");
			}
			if (pParam.IsSet("Left"))
			{
				this.m_fLeft = pParam.Number("Left");
			}
			if (pParam.IsSet("Right"))
			{
				this.m_fRight = pParam.Number("Right");
			}
			if (pParam.IsSet("Top"))
			{
				this.m_fTop = pParam.Number("Top");
			}
			if (pParam.IsSet("Bottom"))
			{
				this.m_fBottom = pParam.Number("Bottom");
			}
			if (pParam.IsSet("Fit"))
			{
				this.m_bFit = pParam.Bool("Fit");
			}
			if (pParam.IsSet("FitB"))
			{
				this.m_bFitB = pParam.Bool("FitB");
			}
			if (pParam.IsSet("FitV"))
			{
				this.m_bFitV = pParam.Bool("FitV");
			}
			if (pParam.IsSet("FitH"))
			{
				this.m_bFitH = pParam.Bool("FitH");
			}
			if (pParam.IsSet("FitR"))
			{
				this.m_bFitR = pParam.Bool("FitR");
			}
		}
	}
}
