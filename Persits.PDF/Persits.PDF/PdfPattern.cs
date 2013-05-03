using System;
namespace Persits.PDF
{
	internal abstract class PdfPattern
	{
		private int type;
		internal PdfPattern(int typeA)
		{
			this.type = typeA;
		}
		internal static PdfPattern parse(PdfObject obj, PdfPreview pPreview)
		{
			PdfObject objectByName;
			if (obj.m_nType == enumType.pdfDictionary)
			{
				objectByName = ((PdfDict)obj).GetObjectByName("PatternType");
			}
			else
			{
				if (obj.m_nType != enumType.pdfDictWithStream)
				{
					return null;
				}
				objectByName = ((PdfDictWithStream)obj).GetObjectByName("PatternType");
			}
			PdfPattern result;
			if (objectByName != null && objectByName.m_nType == enumType.pdfNumber && ((PdfNumber)objectByName).m_fValue == 1.0)
			{
				result = PdfTilingPattern.parse(obj);
			}
			else
			{
				if (objectByName == null || objectByName.m_nType != enumType.pdfNumber || ((PdfNumber)objectByName).m_fValue != 2.0)
				{
					return null;
				}
				result = PdfShadingPattern.parse(obj, pPreview);
			}
			return result;
		}
		internal abstract PdfPattern copy();
		internal int getType()
		{
			return this.type;
		}
	}
}
