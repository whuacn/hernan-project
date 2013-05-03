using System;
namespace Persits.PDF
{
	internal class PdfPatchMeshShading : PdfShading
	{
		private PdfPatch[] patches;
		private int nPatches;
		private PdfFunctionTop[] funcs = new PdfFunctionTop[32];
		private int nFuncs;
		internal PdfPatchMeshShading(int typeA, PdfPatch[] patchesA, int nPatchesA, PdfFunctionTop[] funcsA, int nFuncsA, PdfPreview pPreview) : base(typeA, pPreview)
		{
			this.patches = patchesA;
			this.nPatches = nPatchesA;
			this.nFuncs = nFuncsA;
			for (int i = 0; i < this.nFuncs; i++)
			{
				this.funcs[i] = funcsA[i];
			}
		}
		internal PdfPatchMeshShading(PdfPatchMeshShading shading) : base(shading)
		{
			this.nPatches = shading.nPatches;
			this.patches = new PdfPatch[this.nPatches];
			for (int i = 0; i < this.nPatches; i++)
			{
				this.patches[i] = new PdfPatch(shading.patches[i]);
			}
			this.nFuncs = shading.nFuncs;
			for (int j = 0; j < this.nFuncs; j++)
			{
				this.funcs[j] = shading.funcs[j].copy();
			}
		}
		internal static PdfPatchMeshShading parse(int typeA, PdfDict dict, PdfDictWithStream str, PdfPreview pPreview)
		{
			PdfFunctionTop[] array = new PdfFunctionTop[32];
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			double[] array2 = new double[32];
			double[] array3 = new double[32];
			double[] array4 = new double[32];
			uint num5 = 0u;
			double[] array5 = new double[16];
			double[] array6 = new double[16];
			uint num6 = 0u;
			uint num7 = 0u;
			int[,] array7 = new int[4, 32];
			uint[] array8 = new uint[4];
			PdfObject objectByName = dict.GetObjectByName("BitsPerCoordinate");
			if (objectByName == null || objectByName.m_nType != enumType.pdfNumber)
			{
				AuxException.Throw("BitsPerCoordinate parameter is missing or invalid in shading dictionary.", PdfErrors._ERROR_PREVIEW_PARSE);
				return null;
			}
			int num8 = (int)((PdfNumber)objectByName).m_fValue;
			objectByName = dict.GetObjectByName("BitsPerComponent");
			if (objectByName == null || objectByName.m_nType != enumType.pdfNumber)
			{
				AuxException.Throw("BitsPerComponent parameter is missing or invalid in shading dictionary.", PdfErrors._ERROR_PREVIEW_PARSE);
				return null;
			}
			int num9 = (int)((PdfNumber)objectByName).m_fValue;
			objectByName = dict.GetObjectByName("BitsPerFlag");
			if (objectByName == null || objectByName.m_nType != enumType.pdfNumber)
			{
				AuxException.Throw("BitsPerFlag parameter is missing or invalid in shading dictionary.", PdfErrors._ERROR_PREVIEW_PARSE);
				return null;
			}
			int n = (int)((PdfNumber)objectByName).m_fValue;
			objectByName = dict.GetObjectByName("Decode");
			if (objectByName == null || objectByName.m_nType != enumType.pdfArray || ((PdfArray)objectByName).Size < 6)
			{
				AuxException.Throw("Decode array is missing or invalid in shading dictionary.", PdfErrors._ERROR_PREVIEW_PARSE);
				return null;
			}
			PdfObject @object = ((PdfArray)objectByName).GetObject(0);
			if (@object != null && @object.m_nType == enumType.pdfNumber)
			{
				num = ((PdfNumber)@object).m_fValue;
			}
			@object = ((PdfArray)objectByName).GetObject(1);
			if (@object != null && @object.m_nType == enumType.pdfNumber)
			{
				num2 = ((PdfNumber)@object).m_fValue;
			}
			double num10 = (num2 - num) / (Math.Pow(2.0, (double)num8) - 1.0);
			@object = ((PdfArray)objectByName).GetObject(2);
			if (@object != null && @object.m_nType == enumType.pdfNumber)
			{
				num3 = ((PdfNumber)@object).m_fValue;
			}
			@object = ((PdfArray)objectByName).GetObject(3);
			if (@object != null && @object.m_nType == enumType.pdfNumber)
			{
				num4 = ((PdfNumber)@object).m_fValue;
			}
			double num11 = (num4 - num3) / (Math.Pow(2.0, (double)num8) - 1.0);
			int i = 0;
			while (5 + 2 * i < ((PdfArray)objectByName).Size && i < 32)
			{
				@object = ((PdfArray)objectByName).GetObject(4 + 2 * i);
				if (@object != null && @object.m_nType == enumType.pdfNumber)
				{
					array2[i] = ((PdfNumber)@object).m_fValue;
				}
				@object = ((PdfArray)objectByName).GetObject(5 + 2 * i);
				if (@object != null && @object.m_nType == enumType.pdfNumber)
				{
					array3[i] = ((PdfNumber)@object).m_fValue;
				}
				array4[i] = (array3[i] - array2[i]) / (double)((1 << num9) - 1);
				i++;
			}
			int num12 = i;
			objectByName = dict.GetObjectByName("Function");
			int num13;
			if (objectByName != null)
			{
				if (objectByName.m_nType == enumType.pdfArray)
				{
					num13 = ((PdfArray)objectByName).Size;
					if (num13 > 32)
					{
						AuxException.Throw("Invalid Function array in shading dictionary.", PdfErrors._ERROR_PREVIEW_PARSE);
						return null;
					}
					for (i = 0; i < num13; i++)
					{
						@object = ((PdfArray)objectByName).GetObject(i);
						if ((array[i] = PdfFunctionTop.parse(@object)) == null)
						{
							return null;
						}
					}
				}
				else
				{
					num13 = 1;
					if ((array[0] = PdfFunctionTop.parse(objectByName)) == null)
					{
						return null;
					}
				}
			}
			else
			{
				num13 = 0;
			}
			int num14 = 0;
			PdfPatch[] array9 = null;
			int num15 = 0;
			PdfShadingBitBuf pdfShadingBitBuf = new PdfShadingBitBuf(str);
			while (pdfShadingBitBuf.getBits(n, ref num5))
			{
				int num16;
				int num17;
				if (typeA == 6)
				{
					switch (num5)
					{
					case 0u:
						num16 = 12;
						num17 = 4;
						goto IL_414;
					}
					num16 = 8;
					num17 = 2;
				}
				else
				{
					switch (num5)
					{
					case 0u:
						num16 = 16;
						num17 = 4;
						goto IL_414;
					}
					num16 = 12;
					num17 = 2;
				}
				IL_414:
				i = 0;
				while (i < num16 && pdfShadingBitBuf.getBits(num8, ref num6) && pdfShadingBitBuf.getBits(num8, ref num7))
				{
					array5[i] = num + num10 * num6;
					array6[i] = num3 + num11 * num7;
					i++;
				}
				if (i < num16)
				{
					break;
				}
				for (i = 0; i < num17; i++)
				{
					int j = 0;
					while (j < num12 && pdfShadingBitBuf.getBits(num9, ref array8[j]))
					{
						array7[i, j] = PdfState.dblToCol(array2[j] + array4[j] * array8[j]);
						j++;
					}
					if (j < num12)
					{
						break;
					}
				}
				if (i < num17)
				{
					break;
				}
				if (num14 == num15)
				{
					num15 = ((num15 == 0) ? 16 : (2 * num15));
					PdfPatch[] array10 = new PdfPatch[num15];
					for (int k = 0; k < array10.Length; k++)
					{
						array10[k] = ((array9 != null && k < array9.Length) ? new PdfPatch(array9[k]) : new PdfPatch());
					}
					array9 = array10;
				}
				PdfPatch pdfPatch = array9[num14];
				if (typeA == 6)
				{
					switch (num5)
					{
					case 0u:
						pdfPatch.x[0, 0] = array5[0];
						pdfPatch.y[0, 0] = array6[0];
						pdfPatch.x[0, 1] = array5[1];
						pdfPatch.y[0, 1] = array6[1];
						pdfPatch.x[0, 2] = array5[2];
						pdfPatch.y[0, 2] = array6[2];
						pdfPatch.x[0, 3] = array5[3];
						pdfPatch.y[0, 3] = array6[3];
						pdfPatch.x[1, 3] = array5[4];
						pdfPatch.y[1, 3] = array6[4];
						pdfPatch.x[2, 3] = array5[5];
						pdfPatch.y[2, 3] = array6[5];
						pdfPatch.x[3, 3] = array5[6];
						pdfPatch.y[3, 3] = array6[6];
						pdfPatch.x[3, 2] = array5[7];
						pdfPatch.y[3, 2] = array6[7];
						pdfPatch.x[3, 1] = array5[8];
						pdfPatch.y[3, 1] = array6[8];
						pdfPatch.x[3, 0] = array5[9];
						pdfPatch.y[3, 0] = array6[9];
						pdfPatch.x[2, 0] = array5[10];
						pdfPatch.y[2, 0] = array6[10];
						pdfPatch.x[1, 0] = array5[11];
						pdfPatch.y[1, 0] = array6[11];
						for (int j = 0; j < num12; j++)
						{
							pdfPatch.color[0, 0].c[j] = array7[0, j];
							pdfPatch.color[0, 1].c[j] = array7[1, j];
							pdfPatch.color[1, 1].c[j] = array7[2, j];
							pdfPatch.color[1, 0].c[j] = array7[3, j];
						}
						break;
					case 1u:
						pdfPatch.x[0, 0] = array9[num14 - 1].x[0, 3];
						pdfPatch.y[0, 0] = array9[num14 - 1].y[0, 3];
						pdfPatch.x[0, 1] = array9[num14 - 1].x[1, 3];
						pdfPatch.y[0, 1] = array9[num14 - 1].y[1, 3];
						pdfPatch.x[0, 2] = array9[num14 - 1].x[2, 3];
						pdfPatch.y[0, 2] = array9[num14 - 1].y[2, 3];
						pdfPatch.x[0, 3] = array9[num14 - 1].x[3, 3];
						pdfPatch.y[0, 3] = array9[num14 - 1].y[3, 3];
						pdfPatch.x[1, 3] = array5[0];
						pdfPatch.y[1, 3] = array6[0];
						pdfPatch.x[2, 3] = array5[1];
						pdfPatch.y[2, 3] = array6[1];
						pdfPatch.x[3, 3] = array5[2];
						pdfPatch.y[3, 3] = array6[2];
						pdfPatch.x[3, 2] = array5[3];
						pdfPatch.y[3, 2] = array6[3];
						pdfPatch.x[3, 1] = array5[4];
						pdfPatch.y[3, 1] = array6[4];
						pdfPatch.x[3, 0] = array5[5];
						pdfPatch.y[3, 0] = array6[5];
						pdfPatch.x[2, 0] = array5[6];
						pdfPatch.y[2, 0] = array6[6];
						pdfPatch.x[1, 0] = array5[7];
						pdfPatch.y[1, 0] = array6[7];
						for (int j = 0; j < num12; j++)
						{
							pdfPatch.color[0, 0].c[j] = array9[num14 - 1].color[0, 1].c[j];
							pdfPatch.color[0, 1].c[j] = array9[num14 - 1].color[1, 1].c[j];
							pdfPatch.color[1, 1].c[j] = array7[0, j];
							pdfPatch.color[1, 0].c[j] = array7[1, j];
						}
						break;
					case 2u:
						pdfPatch.x[0, 0] = array9[num14 - 1].x[3, 3];
						pdfPatch.y[0, 0] = array9[num14 - 1].y[3, 3];
						pdfPatch.x[0, 1] = array9[num14 - 1].x[3, 2];
						pdfPatch.y[0, 1] = array9[num14 - 1].y[3, 2];
						pdfPatch.x[0, 2] = array9[num14 - 1].x[3, 1];
						pdfPatch.y[0, 2] = array9[num14 - 1].y[3, 1];
						pdfPatch.x[0, 3] = array9[num14 - 1].x[3, 0];
						pdfPatch.y[0, 3] = array9[num14 - 1].y[3, 0];
						pdfPatch.x[1, 3] = array5[0];
						pdfPatch.y[1, 3] = array6[0];
						pdfPatch.x[2, 3] = array5[1];
						pdfPatch.y[2, 3] = array6[1];
						pdfPatch.x[3, 3] = array5[2];
						pdfPatch.y[3, 3] = array6[2];
						pdfPatch.x[3, 2] = array5[3];
						pdfPatch.y[3, 2] = array6[3];
						pdfPatch.x[3, 1] = array5[4];
						pdfPatch.y[3, 1] = array6[4];
						pdfPatch.x[3, 0] = array5[5];
						pdfPatch.y[3, 0] = array6[5];
						pdfPatch.x[2, 0] = array5[6];
						pdfPatch.y[2, 0] = array6[6];
						pdfPatch.x[1, 0] = array5[7];
						pdfPatch.y[1, 0] = array6[7];
						for (int j = 0; j < num12; j++)
						{
							pdfPatch.color[0, 0].c[j] = array9[num14 - 1].color[1, 1].c[j];
							pdfPatch.color[0, 1].c[j] = array9[num14 - 1].color[1, 0].c[j];
							pdfPatch.color[1, 1].c[j] = array7[0, j];
							pdfPatch.color[1, 0].c[j] = array7[1, j];
						}
						break;
					case 3u:
						pdfPatch.x[0, 0] = array9[num14 - 1].x[3, 0];
						pdfPatch.y[0, 0] = array9[num14 - 1].y[3, 0];
						pdfPatch.x[0, 1] = array9[num14 - 1].x[2, 0];
						pdfPatch.y[0, 1] = array9[num14 - 1].y[2, 0];
						pdfPatch.x[0, 2] = array9[num14 - 1].x[1, 0];
						pdfPatch.y[0, 2] = array9[num14 - 1].y[1, 0];
						pdfPatch.x[0, 3] = array9[num14 - 1].x[0, 0];
						pdfPatch.y[0, 3] = array9[num14 - 1].y[0, 0];
						pdfPatch.x[1, 3] = array5[0];
						pdfPatch.y[1, 3] = array6[0];
						pdfPatch.x[2, 3] = array5[1];
						pdfPatch.y[2, 3] = array6[1];
						pdfPatch.x[3, 3] = array5[2];
						pdfPatch.y[3, 3] = array6[2];
						pdfPatch.x[3, 2] = array5[3];
						pdfPatch.y[3, 2] = array6[3];
						pdfPatch.x[3, 1] = array5[4];
						pdfPatch.y[3, 1] = array6[4];
						pdfPatch.x[3, 0] = array5[5];
						pdfPatch.y[3, 0] = array6[5];
						pdfPatch.x[2, 0] = array5[6];
						pdfPatch.y[2, 0] = array6[6];
						pdfPatch.x[1, 0] = array5[7];
						pdfPatch.y[1, 0] = array6[7];
						for (int j = 0; j < num12; j++)
						{
							pdfPatch.color[0, 1].c[j] = array9[num14 - 1].color[1, 0].c[j];
							pdfPatch.color[0, 1].c[j] = array9[num14 - 1].color[0, 0].c[j];
							pdfPatch.color[1, 1].c[j] = array7[0, j];
							pdfPatch.color[1, 0].c[j] = array7[1, j];
						}
						break;
					}
				}
				else
				{
					switch (num5)
					{
					case 0u:
						pdfPatch.x[0, 0] = array5[0];
						pdfPatch.y[0, 0] = array6[0];
						pdfPatch.x[0, 1] = array5[1];
						pdfPatch.y[0, 1] = array6[1];
						pdfPatch.x[0, 2] = array5[2];
						pdfPatch.y[0, 2] = array6[2];
						pdfPatch.x[0, 3] = array5[3];
						pdfPatch.y[0, 3] = array6[3];
						pdfPatch.x[1, 3] = array5[4];
						pdfPatch.y[1, 3] = array6[4];
						pdfPatch.x[2, 3] = array5[5];
						pdfPatch.y[2, 3] = array6[5];
						pdfPatch.x[3, 3] = array5[6];
						pdfPatch.y[3, 3] = array6[6];
						pdfPatch.x[3, 2] = array5[7];
						pdfPatch.y[3, 2] = array6[7];
						pdfPatch.x[3, 1] = array5[8];
						pdfPatch.y[3, 1] = array6[8];
						pdfPatch.x[3, 0] = array5[9];
						pdfPatch.y[3, 0] = array6[9];
						pdfPatch.x[2, 0] = array5[10];
						pdfPatch.y[2, 0] = array6[10];
						pdfPatch.x[1, 0] = array5[11];
						pdfPatch.y[1, 0] = array6[11];
						pdfPatch.x[1, 1] = array5[12];
						pdfPatch.y[1, 1] = array6[12];
						pdfPatch.x[1, 2] = array5[13];
						pdfPatch.y[1, 2] = array6[13];
						pdfPatch.x[2, 2] = array5[14];
						pdfPatch.y[2, 2] = array6[14];
						pdfPatch.x[2, 1] = array5[15];
						pdfPatch.y[2, 1] = array6[15];
						for (int j = 0; j < num12; j++)
						{
							pdfPatch.color[0, 0].c[j] = array7[0, j];
							pdfPatch.color[0, 1].c[j] = array7[1, j];
							pdfPatch.color[1, 1].c[j] = array7[2, j];
							pdfPatch.color[1, 0].c[j] = array7[3, j];
						}
						break;
					case 1u:
						pdfPatch.x[0, 0] = array9[num14 - 1].x[0, 3];
						pdfPatch.y[0, 0] = array9[num14 - 1].y[0, 3];
						pdfPatch.x[0, 1] = array9[num14 - 1].x[1, 3];
						pdfPatch.y[0, 1] = array9[num14 - 1].y[1, 3];
						pdfPatch.x[0, 2] = array9[num14 - 1].x[2, 3];
						pdfPatch.y[0, 2] = array9[num14 - 1].y[2, 3];
						pdfPatch.x[0, 3] = array9[num14 - 1].x[3, 3];
						pdfPatch.y[0, 3] = array9[num14 - 1].y[3, 3];
						pdfPatch.x[1, 3] = array5[0];
						pdfPatch.y[1, 3] = array6[0];
						pdfPatch.x[2, 3] = array5[1];
						pdfPatch.y[2, 3] = array6[1];
						pdfPatch.x[3, 3] = array5[2];
						pdfPatch.y[3, 3] = array6[2];
						pdfPatch.x[3, 2] = array5[3];
						pdfPatch.y[3, 2] = array6[3];
						pdfPatch.x[3, 1] = array5[4];
						pdfPatch.y[3, 1] = array6[4];
						pdfPatch.x[3, 0] = array5[5];
						pdfPatch.y[3, 0] = array6[5];
						pdfPatch.x[2, 0] = array5[6];
						pdfPatch.y[2, 0] = array6[6];
						pdfPatch.x[1, 0] = array5[7];
						pdfPatch.y[1, 0] = array6[7];
						pdfPatch.x[1, 1] = array5[8];
						pdfPatch.y[1, 1] = array6[8];
						pdfPatch.x[1, 2] = array5[9];
						pdfPatch.y[1, 2] = array6[9];
						pdfPatch.x[2, 2] = array5[10];
						pdfPatch.y[2, 2] = array6[10];
						pdfPatch.x[2, 1] = array5[11];
						pdfPatch.y[2, 1] = array6[11];
						for (int j = 0; j < num12; j++)
						{
							pdfPatch.color[0, 0].c[j] = array9[num14 - 1].color[0, 1].c[j];
							pdfPatch.color[0, 1].c[j] = array9[num14 - 1].color[1, 1].c[j];
							pdfPatch.color[1, 1].c[j] = array7[0, j];
							pdfPatch.color[1, 0].c[j] = array7[1, j];
						}
						break;
					case 2u:
						pdfPatch.x[0, 0] = array9[num14 - 1].x[3, 3];
						pdfPatch.y[0, 0] = array9[num14 - 1].y[3, 3];
						pdfPatch.x[0, 1] = array9[num14 - 1].x[3, 2];
						pdfPatch.y[0, 1] = array9[num14 - 1].y[3, 2];
						pdfPatch.x[0, 2] = array9[num14 - 1].x[3, 1];
						pdfPatch.y[0, 2] = array9[num14 - 1].y[3, 1];
						pdfPatch.x[0, 3] = array9[num14 - 1].x[3, 0];
						pdfPatch.y[0, 3] = array9[num14 - 1].y[3, 0];
						pdfPatch.x[1, 3] = array5[0];
						pdfPatch.y[1, 3] = array6[0];
						pdfPatch.x[2, 3] = array5[1];
						pdfPatch.y[2, 3] = array6[1];
						pdfPatch.x[3, 3] = array5[2];
						pdfPatch.y[3, 3] = array6[2];
						pdfPatch.x[3, 2] = array5[3];
						pdfPatch.y[3, 2] = array6[3];
						pdfPatch.x[3, 1] = array5[4];
						pdfPatch.y[3, 1] = array6[4];
						pdfPatch.x[3, 0] = array5[5];
						pdfPatch.y[3, 0] = array6[5];
						pdfPatch.x[2, 0] = array5[6];
						pdfPatch.y[2, 0] = array6[6];
						pdfPatch.x[1, 0] = array5[7];
						pdfPatch.y[1, 0] = array6[7];
						pdfPatch.x[1, 1] = array5[8];
						pdfPatch.y[1, 1] = array6[8];
						pdfPatch.x[1, 2] = array5[9];
						pdfPatch.y[1, 2] = array6[9];
						pdfPatch.x[2, 2] = array5[10];
						pdfPatch.y[2, 2] = array6[10];
						pdfPatch.x[2, 1] = array5[11];
						pdfPatch.y[2, 1] = array6[11];
						for (int j = 0; j < num12; j++)
						{
							pdfPatch.color[0, 0].c[j] = array9[num14 - 1].color[1, 1].c[j];
							pdfPatch.color[0, 1].c[j] = array9[num14 - 1].color[1, 0].c[j];
							pdfPatch.color[1, 1].c[j] = array7[0, j];
							pdfPatch.color[1, 0].c[j] = array7[1, j];
						}
						break;
					case 3u:
						pdfPatch.x[0, 0] = array9[num14 - 1].x[3, 0];
						pdfPatch.y[0, 0] = array9[num14 - 1].y[3, 0];
						pdfPatch.x[0, 1] = array9[num14 - 1].x[2, 0];
						pdfPatch.y[0, 1] = array9[num14 - 1].y[2, 0];
						pdfPatch.x[0, 2] = array9[num14 - 1].x[1, 0];
						pdfPatch.y[0, 2] = array9[num14 - 1].y[1, 0];
						pdfPatch.x[0, 3] = array9[num14 - 1].x[0, 0];
						pdfPatch.y[0, 3] = array9[num14 - 1].y[0, 0];
						pdfPatch.x[1, 3] = array5[0];
						pdfPatch.y[1, 3] = array6[0];
						pdfPatch.x[2, 3] = array5[1];
						pdfPatch.y[2, 3] = array6[1];
						pdfPatch.x[3, 3] = array5[2];
						pdfPatch.y[3, 3] = array6[2];
						pdfPatch.x[3, 2] = array5[3];
						pdfPatch.y[3, 2] = array6[3];
						pdfPatch.x[3, 1] = array5[4];
						pdfPatch.y[3, 1] = array6[4];
						pdfPatch.x[3, 0] = array5[5];
						pdfPatch.y[3, 0] = array6[5];
						pdfPatch.x[2, 0] = array5[6];
						pdfPatch.y[2, 0] = array6[6];
						pdfPatch.x[1, 0] = array5[7];
						pdfPatch.y[1, 0] = array6[7];
						pdfPatch.x[1, 1] = array5[8];
						pdfPatch.y[1, 1] = array6[8];
						pdfPatch.x[1, 2] = array5[9];
						pdfPatch.y[1, 2] = array6[9];
						pdfPatch.x[2, 2] = array5[10];
						pdfPatch.y[2, 2] = array6[10];
						pdfPatch.x[2, 1] = array5[11];
						pdfPatch.y[2, 1] = array6[11];
						for (int j = 0; j < num12; j++)
						{
							pdfPatch.color[0, 0].c[j] = array9[num14 - 1].color[1, 0].c[j];
							pdfPatch.color[0, 1].c[j] = array9[num14 - 1].color[0, 0].c[j];
							pdfPatch.color[1, 1].c[j] = array7[0, j];
							pdfPatch.color[1, 0].c[j] = array7[1, j];
						}
						break;
					}
				}
				num14++;
				pdfShadingBitBuf.flushBits();
			}
			if (typeA == 6)
			{
				for (i = 0; i < num14; i++)
				{
					PdfPatch pdfPatch = array9[i];
					pdfPatch.x[1, 1] = (-4.0 * pdfPatch.x[0, 0] + 6.0 * (pdfPatch.x[0, 1] + pdfPatch.x[1, 0]) - 2.0 * (pdfPatch.x[0, 3] + pdfPatch.x[3, 0]) + 3.0 * (pdfPatch.x[3, 1] + pdfPatch.x[1, 3]) - pdfPatch.x[3, 3]) / 9.0;
					pdfPatch.y[1, 1] = (-4.0 * pdfPatch.y[0, 0] + 6.0 * (pdfPatch.y[0, 1] + pdfPatch.y[1, 0]) - 2.0 * (pdfPatch.y[0, 3] + pdfPatch.y[3, 0]) + 3.0 * (pdfPatch.y[3, 1] + pdfPatch.y[1, 3]) - pdfPatch.y[3, 3]) / 9.0;
					pdfPatch.x[1, 2] = (-4.0 * pdfPatch.x[0, 3] + 6.0 * (pdfPatch.x[0, 2] + pdfPatch.x[1, 3]) - 2.0 * (pdfPatch.x[0, 0] + pdfPatch.x[3, 3]) + 3.0 * (pdfPatch.x[3, 2] + pdfPatch.x[1, 0]) - pdfPatch.x[3, 0]) / 9.0;
					pdfPatch.y[1, 2] = (-4.0 * pdfPatch.y[0, 3] + 6.0 * (pdfPatch.y[0, 2] + pdfPatch.y[1, 3]) - 2.0 * (pdfPatch.y[0, 0] + pdfPatch.y[3, 3]) + 3.0 * (pdfPatch.y[3, 2] + pdfPatch.y[1, 0]) - pdfPatch.y[3, 0]) / 9.0;
					pdfPatch.x[2, 1] = (-4.0 * pdfPatch.x[3, 0] + 6.0 * (pdfPatch.x[3, 1] + pdfPatch.x[2, 0]) - 2.0 * (pdfPatch.x[3, 3] + pdfPatch.x[0, 0]) + 3.0 * (pdfPatch.x[0, 1] + pdfPatch.x[2, 3]) - pdfPatch.x[0, 3]) / 9.0;
					pdfPatch.y[2, 1] = (-4.0 * pdfPatch.y[3, 0] + 6.0 * (pdfPatch.y[3, 1] + pdfPatch.y[2, 0]) - 2.0 * (pdfPatch.y[3, 3] + pdfPatch.y[0, 0]) + 3.0 * (pdfPatch.y[0, 1] + pdfPatch.y[2, 3]) - pdfPatch.y[0, 3]) / 9.0;
					pdfPatch.x[2, 2] = (-4.0 * pdfPatch.x[3, 3] + 6.0 * (pdfPatch.x[3, 2] + pdfPatch.x[2, 3]) - 2.0 * (pdfPatch.x[3, 0] + pdfPatch.x[0, 3]) + 3.0 * (pdfPatch.x[0, 2] + pdfPatch.x[2, 0]) - pdfPatch.x[0, 0]) / 9.0;
					pdfPatch.y[2, 2] = (-4.0 * pdfPatch.y[3, 3] + 6.0 * (pdfPatch.y[3, 2] + pdfPatch.y[2, 3]) - 2.0 * (pdfPatch.y[3, 0] + pdfPatch.y[0, 3]) + 3.0 * (pdfPatch.y[0, 2] + pdfPatch.y[2, 0]) - pdfPatch.y[0, 0]) / 9.0;
				}
			}
			PdfPatchMeshShading pdfPatchMeshShading = new PdfPatchMeshShading(typeA, array9, num14, array, num13, pPreview);
			if (!pdfPatchMeshShading.init(dict))
			{
				return null;
			}
			return pdfPatchMeshShading;
		}
		internal override PdfShading copy()
		{
			return new PdfPatchMeshShading(this);
		}
	}
}
