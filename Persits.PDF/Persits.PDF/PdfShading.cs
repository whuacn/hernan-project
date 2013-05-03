using System;
namespace Persits.PDF
{
	internal abstract class PdfShading
	{
		protected int type;
		protected PdfColorSpaceTop colorSpace;
		protected PdfColor background = new PdfColor();
		protected bool hasBackground;
		protected double xMin;
		protected double yMin;
		protected double xMax;
		protected double yMax;
		protected bool hasBBox;
		protected PdfPreview m_pPreview;
		internal PdfShading(int typeA, PdfPreview pPreview)
		{
			this.type = typeA;
			this.colorSpace = null;
			this.m_pPreview = pPreview;
		}
		internal PdfShading(PdfShading shading)
		{
			this.type = shading.type;
			this.colorSpace = shading.colorSpace.copy();
			for (int i = 0; i < 32; i++)
			{
				this.background.c[i] = shading.background.c[i];
			}
			this.hasBackground = shading.hasBackground;
			this.xMin = shading.xMin;
			this.yMin = shading.yMin;
			this.xMax = shading.xMax;
			this.yMax = shading.yMax;
			this.hasBBox = shading.hasBBox;
			this.m_pPreview = shading.m_pPreview;
		}
		internal static PdfShading parse(PdfObject obj, PdfPreview pPreview)
		{
			PdfDict pdfDict;
			if (obj != null && obj.m_nType == enumType.pdfDictionary)
			{
				pdfDict = (PdfDict)obj;
			}
			else
			{
				if (obj == null || obj.m_nType != enumType.pdfDictWithStream)
				{
					return null;
				}
				pdfDict = (PdfDictWithStream)obj;
			}
			PdfObject objectByName = pdfDict.GetObjectByName("ShadingType");
			if (objectByName == null || objectByName.m_nType != enumType.pdfNumber)
			{
				AuxException.Throw("Invalid ShadingType in shading dictionary.", PdfErrors._ERROR_PREVIEW_PARSE);
				return null;
			}
			switch ((int)((PdfNumber)objectByName).m_fValue)
			{
			case 1:
			{
				PdfShading result = PdfFunctionShading.parse(pdfDict, pPreview);
				return result;
			}
			case 2:
			{
				PdfShading result = PdfAxialShading.parse(pdfDict, pPreview);
				return result;
			}
			case 3:
			{
				PdfShading result = PdfRadialShading.parse(pdfDict, pPreview);
				return result;
			}
			case 6:
				if (obj.m_nType == enumType.pdfDictWithStream)
				{
					PdfShading result = PdfPatchMeshShading.parse(6, pdfDict, (PdfDictWithStream)obj, pPreview);
					return result;
				}
				AuxException.Throw("Invalid Type 6 shading object.", PdfErrors._ERROR_PREVIEW_PARSE);
				return null;
			case 7:
				if (obj.m_nType == enumType.pdfDictWithStream)
				{
					PdfShading result = PdfPatchMeshShading.parse(7, pdfDict, (PdfDictWithStream)obj, pPreview);
					return result;
				}
				AuxException.Throw("Invalid Type 7 shading object.", PdfErrors._ERROR_PREVIEW_PARSE);
				return null;
			}
			AuxException.Throw("Shading type not imlpemented.", PdfErrors._ERROR_PREVIEW_PARSE);
			return null;
		}
		protected bool init(PdfDict dict)
		{
			PdfObject objectByName = dict.GetObjectByName("ColorSpace");
			if ((this.colorSpace = PdfColorSpaceTop.parse(objectByName, this.m_pPreview)) == null)
			{
				AuxException.Throw("Invalid shading color space.", PdfErrors._ERROR_PREVIEW_PARSE);
				return false;
			}
			for (int i = 0; i < 32; i++)
			{
				this.background.c[i] = 0;
			}
			this.hasBackground = false;
			objectByName = dict.GetObjectByName("Background");
			if (objectByName != null && objectByName.m_nType == enumType.pdfArray)
			{
				if (((PdfArray)objectByName).Size == this.colorSpace.getNComps())
				{
					this.hasBackground = true;
					for (int i = 0; i < this.colorSpace.getNComps(); i++)
					{
						PdfObject @object = ((PdfArray)objectByName).GetObject(i);
						this.background.c[i] = PdfState.dblToCol(((PdfNumber)@object).m_fValue);
					}
				}
				else
				{
					AuxException.Throw("Invalid shading background.", PdfErrors._ERROR_PREVIEW_PARSE);
				}
			}
			this.xMin = (this.yMin = (this.xMax = (this.yMax = 0.0)));
			this.hasBBox = false;
			objectByName = dict.GetObjectByName("BBox");
			if (objectByName != null && objectByName.m_nType == enumType.pdfArray)
			{
				if (((PdfArray)objectByName).Size == 4)
				{
					this.hasBBox = true;
					PdfObject @object = ((PdfArray)objectByName).GetObject(0);
					this.xMin = ((PdfNumber)@object).m_fValue;
					@object = ((PdfArray)objectByName).GetObject(1);
					this.yMin = ((PdfNumber)@object).m_fValue;
					@object = ((PdfArray)objectByName).GetObject(2);
					this.xMax = ((PdfNumber)@object).m_fValue;
					@object = ((PdfArray)objectByName).GetObject(3);
					this.yMax = ((PdfNumber)@object).m_fValue;
				}
				else
				{
					AuxException.Throw("Invalid shading BBox.", PdfErrors._ERROR_PREVIEW_PARSE);
				}
			}
			return true;
		}
		internal abstract PdfShading copy();
		internal int getType()
		{
			return this.type;
		}
		internal PdfColorSpaceTop getColorSpace()
		{
			return this.colorSpace;
		}
		internal PdfColor getBackground()
		{
			return this.background;
		}
		internal bool getHasBackground()
		{
			return this.hasBackground;
		}
		internal void getBBox(ref double xMinA, ref double yMinA, ref double xMaxA, ref double yMaxA)
		{
			xMinA = this.xMin;
			yMinA = this.yMin;
			xMaxA = this.xMax;
			yMaxA = this.yMax;
		}
		internal bool getHasBBox()
		{
			return this.hasBBox;
		}
	}
}
