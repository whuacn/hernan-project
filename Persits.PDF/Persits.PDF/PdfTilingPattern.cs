using System;
namespace Persits.PDF
{
	internal class PdfTilingPattern : PdfPattern
	{
		private int paintType;
		private int tilingType;
		private double[] bbox = new double[4];
		private double xStep;
		private double yStep;
		private PdfObject resDict;
		private double[] matrix = new double[6];
		private PdfObject contentStream;
		internal static PdfTilingPattern parse(PdfObject patObj)
		{
			double[] array = new double[4];
			double[] array2 = new double[6];
			if (patObj.m_nType != enumType.pdfDictWithStream)
			{
				return null;
			}
			PdfDict pdfDict = (PdfDict)patObj;
			PdfObject objectByName = pdfDict.GetObjectByName("PaintType");
			int paintTypeA;
			if (objectByName != null && objectByName.m_nType == enumType.pdfNumber)
			{
				paintTypeA = (int)((PdfNumber)objectByName).m_fValue;
			}
			else
			{
				paintTypeA = 1;
				AuxException.Throw("Invalid or missing pattern PaintType.", PdfErrors._ERROR_PREVIEW_PARSE);
			}
			objectByName = pdfDict.GetObjectByName("TilingType");
			int tilingTypeA;
			if (objectByName != null && objectByName.m_nType == enumType.pdfNumber)
			{
				tilingTypeA = (int)((PdfNumber)objectByName).m_fValue;
			}
			else
			{
				tilingTypeA = 1;
				AuxException.Throw("Invalid or missing pattern TilingType.", PdfErrors._ERROR_PREVIEW_PARSE);
			}
			array[0] = (array[1] = 0.0);
			array[2] = (array[3] = 1.0);
			objectByName = pdfDict.GetObjectByName("BBox");
			if (objectByName != null && objectByName.m_nType == enumType.pdfArray && ((PdfArray)objectByName).Size == 4)
			{
				for (int i = 0; i < 4; i++)
				{
					PdfObject @object = ((PdfArray)objectByName).GetObject(i);
					if (@object != null && @object.m_nType == enumType.pdfNumber)
					{
						array[i] = ((PdfNumber)@object).m_fValue;
					}
				}
			}
			else
			{
				AuxException.Throw("Invalid or missing pattern BBox.", PdfErrors._ERROR_PREVIEW_PARSE);
			}
			objectByName = pdfDict.GetObjectByName("XStep");
			double xStepA;
			if (objectByName != null && objectByName.m_nType == enumType.pdfNumber)
			{
				xStepA = ((PdfNumber)objectByName).m_fValue;
			}
			else
			{
				xStepA = 1.0;
				AuxException.Throw("Invalid or missing pattern XStep.", PdfErrors._ERROR_PREVIEW_PARSE);
			}
			objectByName = pdfDict.GetObjectByName("YStep");
			double yStepA;
			if (objectByName != null && objectByName.m_nType == enumType.pdfNumber)
			{
				yStepA = ((PdfNumber)objectByName).m_fValue;
			}
			else
			{
				yStepA = 1.0;
				AuxException.Throw("Invalid or missing pattern YStep.", PdfErrors._ERROR_PREVIEW_PARSE);
			}
			PdfObject objectByName2 = pdfDict.GetObjectByName("Resources");
			if (objectByName2 == null || objectByName2.m_nType != enumType.pdfDictionary)
			{
				AuxException.Throw("Invalid or missing pattern Resources.", PdfErrors._ERROR_PREVIEW_PARSE);
			}
			array2[0] = 1.0;
			array2[1] = 0.0;
			array2[2] = 0.0;
			array2[3] = 1.0;
			array2[4] = 0.0;
			array2[5] = 0.0;
			objectByName = pdfDict.GetObjectByName("Matrix");
			if (objectByName != null && objectByName.m_nType == enumType.pdfArray && ((PdfArray)objectByName).Size == 6)
			{
				for (int i = 0; i < 6; i++)
				{
					PdfObject @object = ((PdfArray)objectByName).GetObject(i);
					if (@object != null && @object.m_nType == enumType.pdfNumber)
					{
						array2[i] = ((PdfNumber)@object).m_fValue;
					}
				}
			}
			return new PdfTilingPattern(paintTypeA, tilingTypeA, array, xStepA, yStepA, objectByName2, array2, patObj);
		}
		internal PdfTilingPattern(int paintTypeA, int tilingTypeA, double[] bboxA, double xStepA, double yStepA, PdfObject resDictA, double[] matrixA, PdfObject contentStreamA) : base(1)
		{
			this.paintType = paintTypeA;
			this.tilingType = tilingTypeA;
			for (int i = 0; i < 4; i++)
			{
				this.bbox[i] = bboxA[i];
			}
			this.xStep = xStepA;
			this.yStep = yStepA;
			this.resDict = resDictA.Copy();
			for (int i = 0; i < 6; i++)
			{
				this.matrix[i] = matrixA[i];
			}
			this.contentStream = contentStreamA.Copy();
		}
		internal override PdfPattern copy()
		{
			return new PdfTilingPattern(this.paintType, this.tilingType, this.bbox, this.xStep, this.yStep, this.resDict, this.matrix, this.contentStream);
		}
		internal int getPaintType()
		{
			return this.paintType;
		}
		internal int getTilingType()
		{
			return this.tilingType;
		}
		internal double[] getBBox()
		{
			return this.bbox;
		}
		internal double getXStep()
		{
			return this.xStep;
		}
		internal double getYStep()
		{
			return this.yStep;
		}
		internal PdfDict getResDict()
		{
			if (this.resDict.m_nType != enumType.pdfDictionary)
			{
				return null;
			}
			return (PdfDict)this.resDict;
		}
		internal double[] getMatrix()
		{
			return this.matrix;
		}
		internal PdfObject getContentStream()
		{
			return this.contentStream;
		}
	}
}
