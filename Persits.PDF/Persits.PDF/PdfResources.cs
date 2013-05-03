using System;
namespace Persits.PDF
{
	internal class PdfResources
	{
		internal PdfResources m_pNext;
		private PdfDocument m_pDoc;
		private PdfPreview m_pPreview;
		private PdfDict m_pResDict;
		private PdfFontDict m_pFonts;
		private PdfDict m_pColorSpaceDict;
		private PdfDict m_pFontDict;
		private PdfDict m_pXObjectDict;
		private PdfDict m_pPatternDict;
		private PdfDict m_pShadingDict;
		private PdfDict m_pExtGState;
		internal PdfResources(PdfObject pResObj, PdfDict pPagesDict, PdfDocument pDoc, PdfPage pPage, PdfPreview pPreview)
		{
			this.m_pDoc = pDoc;
			this.m_pPreview = pPreview;
			this.m_pNext = null;
			this.m_pFonts = null;
			this.m_pColorSpaceDict = (this.m_pFontDict = (this.m_pXObjectDict = (this.m_pPatternDict = (this.m_pShadingDict = (this.m_pExtGState = null)))));
			if (pResObj != null)
			{
				if (pResObj.m_nType == enumType.pdfRef)
				{
					PdfObject pdfObject = this.m_pDoc.m_pInput.ParseGenericObject((PdfRef)pResObj, 0);
					if (pdfObject.m_nType == enumType.pdfDictionary)
					{
						this.m_pResDict = (PdfDict)pdfObject;
					}
				}
				else
				{
					if (pResObj.m_nType == enumType.pdfDictionary)
					{
						this.m_pResDict = (PdfDict)pResObj.Copy();
						this.m_pDoc.m_pInput.RemoveAllRefsFromObject(this.m_pResDict, 0);
					}
				}
			}
			if (this.m_pResDict != null)
			{
				PdfObject objectByName = this.m_pResDict.GetObjectByName("Font");
				if (objectByName != null && objectByName.m_nType == enumType.pdfDictionary)
				{
					this.m_pFonts = new PdfFontDict((PdfDict)objectByName, pPreview);
				}
				string[] array = new string[]
				{
					"ColorSpace",
					"ExtGState",
					"Pattern",
					"Shading",
					"XObject"
				};
				for (int i = 0; i < array.Length; i++)
				{
					objectByName = this.m_pResDict.GetObjectByName(array[i]);
					if (objectByName != null && objectByName.m_nType == enumType.pdfDictionary)
					{
						switch (i)
						{
						case 0:
							this.m_pColorSpaceDict = (PdfDict)objectByName;
							goto IL_1C7;
						case 1:
							this.m_pExtGState = (PdfDict)objectByName;
							goto IL_1C7;
						case 2:
							this.m_pPatternDict = (PdfDict)objectByName;
							goto IL_1C7;
						case 3:
							this.m_pShadingDict = (PdfDict)objectByName;
							goto IL_1C7;
						}
						this.m_pXObjectDict = (PdfDict)objectByName;
					}
					IL_1C7:;
				}
				this.Handle16BitImages();
			}
			if (pPagesDict == null)
			{
				return;
			}
			PdfObject objectByName2 = pPagesDict.GetObjectByName("Parent");
			if (objectByName2 == null || objectByName2.m_nType != enumType.pdfRef)
			{
				return;
			}
			PdfDict pdfDict = (PdfDict)this.m_pDoc.m_pInput.RemoveRef(objectByName2, enumType.pdfDictionary);
			PdfObject objectByName3 = pdfDict.GetObjectByName("Resources");
			if (objectByName3 == null)
			{
				return;
			}
			PdfResources pNext = new PdfResources(objectByName3, pdfDict, pDoc, pPage, pPreview);
			this.m_pNext = pNext;
		}
		private void Handle16BitImages()
		{
			if (this.m_pXObjectDict == null)
			{
				return;
			}
			for (int i = 0; i < this.m_pXObjectDict.Size; i++)
			{
				PdfObject @object = this.m_pXObjectDict.GetObject(i);
				if (@object != null && @object.m_nType == enumType.pdfDictWithStream)
				{
					PdfDictWithStream pdfDictWithStream = (PdfDictWithStream)@object;
					PdfObject objectByName = pdfDictWithStream.GetObjectByName("BitsPerComponent");
					if (objectByName != null && objectByName.m_nType == enumType.pdfNumber && ((PdfNumber)objectByName).m_fValue == 16.0)
					{
						PdfStream pdfStream = new PdfStream();
						for (int j = 0; j < pdfDictWithStream.m_objStream.Length; j += 2)
						{
							pdfStream.Append(new byte[]
							{
								pdfDictWithStream.m_objStream[j]
							});
						}
						pdfDictWithStream.m_objStream.Set(pdfStream);
						pdfDictWithStream.Add(new PdfNumber("BitsPerComponent", 8.0));
					}
				}
			}
		}
		internal PdfObject LookupColorSpace(string Name)
		{
			for (PdfResources pdfResources = this; pdfResources != null; pdfResources = pdfResources.m_pNext)
			{
				if (pdfResources.m_pColorSpaceDict != null)
				{
					return pdfResources.m_pColorSpaceDict.GetObjectByName(Name);
				}
			}
			return null;
		}
		internal PdfObject LookupExtGState(string Name)
		{
			for (PdfResources pdfResources = this; pdfResources != null; pdfResources = pdfResources.m_pNext)
			{
				if (pdfResources.m_pExtGState != null)
				{
					return pdfResources.m_pExtGState.GetObjectByName(Name);
				}
			}
			return null;
		}
		internal PdfPattern LookupPattern(string Name)
		{
			for (PdfResources pdfResources = this; pdfResources != null; pdfResources = pdfResources.m_pNext)
			{
				if (pdfResources.m_pPatternDict != null)
				{
					PdfObject objectByName = pdfResources.m_pPatternDict.GetObjectByName(Name);
					return PdfPattern.parse(objectByName, this.m_pPreview);
				}
			}
			return null;
		}
		internal PdfShading LookupShading(string Name)
		{
			for (PdfResources pdfResources = this; pdfResources != null; pdfResources = pdfResources.m_pNext)
			{
				if (pdfResources.m_pShadingDict != null)
				{
					PdfObject objectByName = pdfResources.m_pShadingDict.GetObjectByName(Name);
					return PdfShading.parse(objectByName, this.m_pPreview);
				}
			}
			return null;
		}
		internal PdfObject LookupXObject(string Name)
		{
			for (PdfResources pdfResources = this; pdfResources != null; pdfResources = pdfResources.m_pNext)
			{
				if (pdfResources.m_pXObjectDict != null)
				{
					return pdfResources.m_pXObjectDict.GetObjectByName(Name);
				}
			}
			return null;
		}
		internal PdfPreviewFont LookupFont(string Name)
		{
			for (PdfResources pdfResources = this; pdfResources != null; pdfResources = pdfResources.m_pNext)
			{
				if (pdfResources.m_pFonts != null)
				{
					return pdfResources.m_pFonts.lookup(Name);
				}
			}
			return null;
		}
	}
}
