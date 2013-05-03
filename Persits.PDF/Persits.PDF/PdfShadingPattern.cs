using System;
namespace Persits.PDF
{
	internal class PdfShadingPattern : PdfPattern
	{
		private PdfShading shading;
		private double[] matrix = new double[6];
		internal new static PdfShadingPattern parse(PdfObject patObj, PdfPreview pPreview)
		{
			double[] array = new double[6];
			if (patObj.m_nType != enumType.pdfDictionary)
			{
				return null;
			}
			PdfDict pdfDict = (PdfDict)patObj;
			PdfObject objectByName = pdfDict.GetObjectByName("Shading");
			PdfShading pdfShading = PdfShading.parse(objectByName, pPreview);
			if (pdfShading == null)
			{
				return null;
			}
			array[0] = 1.0;
			array[1] = 0.0;
			array[2] = 0.0;
			array[3] = 1.0;
			array[4] = 0.0;
			array[5] = 0.0;
			objectByName = pdfDict.GetObjectByName("Matrix");
			if (objectByName != null && objectByName.m_nType == enumType.pdfArray && ((PdfArray)objectByName).Size == 6)
			{
				for (int i = 0; i < 6; i++)
				{
					PdfObject @object = ((PdfArray)objectByName).GetObject(i);
					if (@object != null && @object.m_nType == enumType.pdfNumber)
					{
						array[i] = ((PdfNumber)@object).m_fValue;
					}
				}
			}
			return new PdfShadingPattern(pdfShading, array);
		}
		internal override PdfPattern copy()
		{
			return new PdfShadingPattern(this.shading.copy(), this.matrix);
		}
		internal PdfShading getShading()
		{
			return this.shading;
		}
		internal double[] getMatrix()
		{
			return this.matrix;
		}
		internal PdfShadingPattern(PdfShading shadingA, double[] matrixA) : base(2)
		{
			this.shading = shadingA;
			for (int i = 0; i < 6; i++)
			{
				this.matrix[i] = matrixA[i];
			}
		}
	}
}
