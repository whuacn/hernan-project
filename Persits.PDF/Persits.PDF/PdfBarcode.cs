using System;
using System.Text;
namespace Persits.PDF
{
	public class PdfBarcode
	{
		internal PdfManager m_pManager;
		internal PdfDocument m_pDoc;
		internal static float BAR_UNSPEC = -998f;
		internal static float BAR_DEFAULTFONTSIZE = 12f;
		internal PdfCanvas m_pCanvas;
		internal PdfFont m_ptrFont;
		internal PdfParam m_pParam;
		internal BarProperties[] m_Bars;
		internal TextProperties[] m_Text;
		internal bool bDrawText;
		internal int iNumText;
		internal static byte[][] UPCBARS = new byte[][]
		{
			new byte[]
			{
				3,
				2,
				1,
				1
			},
			new byte[]
			{
				2,
				2,
				2,
				1
			},
			new byte[]
			{
				2,
				1,
				2,
				2
			},
			new byte[]
			{
				1,
				4,
				1,
				1
			},
			new byte[]
			{
				1,
				1,
				3,
				2
			},
			new byte[]
			{
				1,
				2,
				3,
				1
			},
			new byte[]
			{
				1,
				1,
				1,
				4
			},
			new byte[]
			{
				1,
				3,
				1,
				2
			},
			new byte[]
			{
				1,
				2,
				1,
				3
			},
			new byte[]
			{
				3,
				1,
				1,
				2
			}
		};
		internal static string CODE39CHARS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%*";
		internal static byte[][] CODE39BARS = new byte[][]
		{
			new byte[]
			{
				0,
				0,
				0,
				1,
				1,
				0,
				1,
				0,
				0
			},
			new byte[]
			{
				1,
				0,
				0,
				1,
				0,
				0,
				0,
				0,
				1
			},
			new byte[]
			{
				0,
				0,
				1,
				1,
				0,
				0,
				0,
				0,
				1
			},
			new byte[]
			{
				1,
				0,
				1,
				1,
				0,
				0,
				0,
				0,
				0
			},
			new byte[]
			{
				0,
				0,
				0,
				1,
				1,
				0,
				0,
				0,
				1
			},
			new byte[]
			{
				1,
				0,
				0,
				1,
				1,
				0,
				0,
				0,
				0
			},
			new byte[]
			{
				0,
				0,
				1,
				1,
				1,
				0,
				0,
				0,
				0
			},
			new byte[]
			{
				0,
				0,
				0,
				1,
				0,
				0,
				1,
				0,
				1
			},
			new byte[]
			{
				1,
				0,
				0,
				1,
				0,
				0,
				1,
				0,
				0
			},
			new byte[]
			{
				0,
				0,
				1,
				1,
				0,
				0,
				1,
				0,
				0
			},
			new byte[]
			{
				1,
				0,
				0,
				0,
				0,
				1,
				0,
				0,
				1
			},
			new byte[]
			{
				0,
				0,
				1,
				0,
				0,
				1,
				0,
				0,
				1
			},
			new byte[]
			{
				1,
				0,
				1,
				0,
				0,
				1,
				0,
				0,
				0
			},
			new byte[]
			{
				0,
				0,
				0,
				0,
				1,
				1,
				0,
				0,
				1
			},
			new byte[]
			{
				1,
				0,
				0,
				0,
				1,
				1,
				0,
				0,
				0
			},
			new byte[]
			{
				0,
				0,
				1,
				0,
				1,
				1,
				0,
				0,
				0
			},
			new byte[]
			{
				0,
				0,
				0,
				0,
				0,
				1,
				1,
				0,
				1
			},
			new byte[]
			{
				1,
				0,
				0,
				0,
				0,
				1,
				1,
				0,
				0
			},
			new byte[]
			{
				0,
				0,
				1,
				0,
				0,
				1,
				1,
				0,
				0
			},
			new byte[]
			{
				0,
				0,
				0,
				0,
				1,
				1,
				1,
				0,
				0
			},
			new byte[]
			{
				1,
				0,
				0,
				0,
				0,
				0,
				0,
				1,
				1
			},
			new byte[]
			{
				0,
				0,
				1,
				0,
				0,
				0,
				0,
				1,
				1
			},
			new byte[]
			{
				1,
				0,
				1,
				0,
				0,
				0,
				0,
				1,
				0
			},
			new byte[]
			{
				0,
				0,
				0,
				0,
				1,
				0,
				0,
				1,
				1
			},
			new byte[]
			{
				1,
				0,
				0,
				0,
				1,
				0,
				0,
				1,
				0
			},
			new byte[]
			{
				0,
				0,
				1,
				0,
				1,
				0,
				0,
				1,
				0
			},
			new byte[]
			{
				0,
				0,
				0,
				0,
				0,
				0,
				1,
				1,
				1
			},
			new byte[]
			{
				1,
				0,
				0,
				0,
				0,
				0,
				1,
				1,
				0
			},
			new byte[]
			{
				0,
				0,
				1,
				0,
				0,
				0,
				1,
				1,
				0
			},
			new byte[]
			{
				0,
				0,
				0,
				0,
				1,
				0,
				1,
				1,
				0
			},
			new byte[]
			{
				1,
				1,
				0,
				0,
				0,
				0,
				0,
				0,
				1
			},
			new byte[]
			{
				0,
				1,
				1,
				0,
				0,
				0,
				0,
				0,
				1
			},
			new byte[]
			{
				1,
				1,
				1,
				0,
				0,
				0,
				0,
				0,
				0
			},
			new byte[]
			{
				0,
				1,
				0,
				0,
				1,
				0,
				0,
				0,
				1
			},
			new byte[]
			{
				1,
				1,
				0,
				0,
				1,
				0,
				0,
				0,
				0
			},
			new byte[]
			{
				0,
				1,
				1,
				0,
				1,
				0,
				0,
				0,
				0
			},
			new byte[]
			{
				0,
				1,
				0,
				0,
				0,
				0,
				1,
				0,
				1
			},
			new byte[]
			{
				1,
				1,
				0,
				0,
				0,
				0,
				1,
				0,
				0
			},
			new byte[]
			{
				0,
				1,
				1,
				0,
				0,
				0,
				1,
				0,
				0
			},
			new byte[]
			{
				0,
				1,
				0,
				1,
				0,
				1,
				0,
				0,
				0
			},
			new byte[]
			{
				0,
				1,
				0,
				1,
				0,
				0,
				0,
				1,
				0
			},
			new byte[]
			{
				0,
				1,
				0,
				0,
				0,
				1,
				0,
				1,
				0
			},
			new byte[]
			{
				0,
				0,
				0,
				1,
				0,
				1,
				0,
				1,
				0
			},
			new byte[]
			{
				0,
				1,
				0,
				0,
				1,
				0,
				1,
				0,
				0
			}
		};
		internal static byte[][] INDBARS;
		internal static string CODABARCHARS;
		internal static byte[][] CODABARS;
		internal static byte[][] UPCEPARITY;
		internal static byte[][] EANPARITY;
		internal static byte[][] EXT5PARITY;
		internal static string CODE11CHARS;
		internal static byte[][] CODE11BARS;
		internal static string CODE93CHARS;
		internal static byte[][] CODE93BARS;
		internal static byte[][] CODE128BARS;
		internal static byte[][] POSTALBARS;
		internal static byte[][] UKPOSTALBARS;
		internal int[][] nCheckChar = new int[][]
		{
			new int[]
			{
				35,
				30,
				31,
				32,
				33,
				34
			},
			new int[]
			{
				5,
				0,
				1,
				2,
				3,
				4
			},
			new int[]
			{
				11,
				6,
				7,
				8,
				9,
				10
			},
			new int[]
			{
				17,
				12,
				13,
				14,
				15,
				16
			},
			new int[]
			{
				23,
				18,
				19,
				20,
				21,
				22
			},
			new int[]
			{
				29,
				24,
				25,
				26,
				27,
				28
			}
		};
		internal PdfBarcode()
		{
		}
		internal void BAR_SETUPVARS(int numbars, int numtext, bool drawtext)
		{
			this.iNumText = numtext;
			this.bDrawText = drawtext;
			this.m_Bars = new BarProperties[numbars];
			for (int i = 0; i < numbars; i++)
			{
				this.m_Bars[i] = new BarProperties();
			}
			this.m_Text = new TextProperties[numtext];
			for (int j = 0; j < numtext; j++)
			{
				this.m_Text[j] = new TextProperties();
			}
		}
		internal void Draw(string Data, PdfCanvas Canvas, PdfParam pParam)
		{
			if (!pParam.IsSet("X") || !pParam.IsSet("Y"))
			{
				AuxException.Throw("X and Y parameters are required.", PdfErrors._ERROR_INVALIDARG);
			}
			float num = pParam.Number("X");
			float num2 = pParam.Number("Y");
			if (pParam.IsSet("Angle"))
			{
				float num3 = pParam.Number("Angle");
				float num4 = 0.01745329f;
				Canvas.SetCTM((float)Math.Cos((double)(num3 * num4)), (float)Math.Sin((double)(num3 * num4)), -(float)Math.Sin((double)(num3 * num4)), (float)Math.Cos((double)(num3 * num4)), num, num2);
				num2 = (num = 0f);
			}
			if (!pParam.IsSet("Width") || !pParam.IsSet("Height"))
			{
				AuxException.Throw("Width and Height parameters are required.", PdfErrors._ERROR_INVALIDARG);
			}
			float fWidth = pParam.Number("Width");
			float fHeight = pParam.Number("Height");
			if (!pParam.IsSet("Type"))
			{
				AuxException.Throw("Type barcode parameter is required.", PdfErrors._ERROR_INVALIDARG);
			}
			pdfBarcodeType pdfBarcodeType = (pdfBarcodeType)pParam.Number("Type");
			string text = Data;
			if (text.Length < 1)
			{
				AuxException.Throw("No barcode data specified", PdfErrors._ERROR_INVALIDARG);
			}
			float fWideMult;
			if (pParam.IsSet("BarWeight"))
			{
				fWideMult = pParam.Number("BarWeight");
			}
			else
			{
				fWideMult = 2f;
			}
			bool flag = false;
			if (pParam.IsSet("AddCheck"))
			{
				flag = pParam.Bool("AddCheck");
			}
			this.m_ptrFont = this.m_pDoc.Fonts.LoadFromResource();
			float num5 = PdfBarcode.BAR_DEFAULTFONTSIZE;
			this.m_pCanvas = Canvas;
			this.m_pParam = pParam;
			int num6 = 0;
			bool flag2 = true;
			switch (pdfBarcodeType)
			{
			case pdfBarcodeType.barUPCA:
				num6 = 59;
				this.BAR_SETUPVARS(num6, 4, true);
				flag2 = false;
				this.CalcUPCA(text, num2, fHeight, fWidth);
				goto IL_610;
			case pdfBarcodeType.barUPCE:
				num6 = 33;
				this.BAR_SETUPVARS(num6, 3, true);
				flag2 = false;
				this.CalcUPCE(text, num2, fHeight, fWidth);
				goto IL_610;
			case pdfBarcodeType.barEAN13:
				num6 = 59;
				this.BAR_SETUPVARS(num6, 3, true);
				flag2 = false;
				this.CalcEAN13(text, num2, fHeight, fWidth);
				goto IL_610;
			case pdfBarcodeType.barEAN8:
				num6 = 43;
				this.BAR_SETUPVARS(num6, 2, true);
				flag2 = false;
				this.CalcEAN8(text, num2, fHeight, fWidth);
				goto IL_610;
			case pdfBarcodeType.barUPCWithSupplemental:
				num6 = 59;
				this.BAR_SETUPVARS(num6, 4, true);
				flag2 = false;
				this.CalcUPCWithSupplemental(text, num2, fHeight, fWidth);
				goto IL_610;
			case pdfBarcodeType.barUPCEWithSupplemental:
				num6 = 33;
				this.BAR_SETUPVARS(num6, 3, true);
				flag2 = false;
				this.CalcUPCEWithSupplemental(text, num2, fHeight, fWidth);
				goto IL_610;
			case pdfBarcodeType.barEAN13WithSupplemental:
				num6 = 59;
				this.BAR_SETUPVARS(num6, 3, true);
				flag2 = false;
				this.CalcEAN13WithSupplemental(text, num2, fHeight, fWidth);
				goto IL_610;
			case pdfBarcodeType.barEAN8WithSupplemental:
				num6 = 43;
				this.BAR_SETUPVARS(num6, 2, true);
				flag2 = false;
				this.CalcEAN8WithSupplemental(text, num2, fHeight, fWidth);
				goto IL_610;
			case pdfBarcodeType.barSupplemental2:
				num6 = 13;
				this.BAR_SETUPVARS(num6, 1, true);
				flag2 = false;
				this.CalcSupplemental2(text, num2, fHeight, fWidth);
				goto IL_610;
			case pdfBarcodeType.barSupplemental5:
				num6 = 31;
				this.BAR_SETUPVARS(num6, 1, true);
				flag2 = false;
				this.CalcSupplemental5(text, num2, fHeight, fWidth);
				goto IL_610;
			case pdfBarcodeType.barIndustrial25:
				num6 = 10 * text.Length + 11;
				this.BAR_SETUPVARS(num6, 1, false);
				this.CalcIndustrial25(text, num2, fHeight, fWidth, fWideMult);
				goto IL_610;
			case pdfBarcodeType.barInterleave25:
			case pdfBarcodeType.barInterleave25Special:
				num6 = 5 * text.Length + 7;
				if (text.Length % 2 != 0)
				{
					num6 += 5;
				}
				this.BAR_SETUPVARS(num6, 1, false);
				this.CalcInterleave25(text, num2, fHeight, fWidth, fWideMult, pdfBarcodeType == pdfBarcodeType.barInterleave25Special);
				goto IL_610;
			case pdfBarcodeType.barDataLogic25:
				num6 = 6 * text.Length + 11;
				this.BAR_SETUPVARS(num6, 1, false);
				this.CalcDataLogic25(text, num2, fHeight, fWidth, fWideMult);
				goto IL_610;
			case pdfBarcodeType.barPlessey:
				num6 = 8 * text.Length + 5;
				this.BAR_SETUPVARS(num6, 1, false);
				this.CalcPlessey(text, num2, fHeight, fWidth, fWideMult);
				goto IL_610;
			case pdfBarcodeType.barCodabar:
				num6 = 8 * text.Length;
				this.BAR_SETUPVARS(num6, 1, false);
				this.CalcCodabar(text, num2, fHeight, fWidth);
				goto IL_610;
			case pdfBarcodeType.barCode39:
				num6 = 10 * (text.Length + 2);
				this.BAR_SETUPVARS(num6, 1, true);
				this.CalcCode39(text, num2, fHeight, fWidth, fWideMult);
				goto IL_610;
			case pdfBarcodeType.barCode11:
				num6 = 6 * (text.Length + 2);
				this.BAR_SETUPVARS(num6, 1, false);
				this.CalcCode11(text, num2, fHeight, fWidth, fWideMult);
				goto IL_610;
			case pdfBarcodeType.barCode39Extended:
			{
				string text2 = text;
				this.CalcCode39Extended(ref text2);
				num6 = 10 * (text2.Length + 2);
				this.BAR_SETUPVARS(num6, 1, true);
				this.CalcCode39(text2, num2, fHeight, fWidth, fWideMult);
				goto IL_610;
			}
			case pdfBarcodeType.barCode93:
			{
				string text3 = text;
				this.EncodeCode93(ref text, flag);
				num6 = 6 * (text.Length + 2) + 1;
				this.BAR_SETUPVARS(num6, 1, false);
				this.CalcCode93(text, num2, fHeight, fWidth);
				text = text3;
				goto IL_610;
			}
			case pdfBarcodeType.barCode128:
			case pdfBarcodeType.barCode128Raw:
			{
				string text2 = text;
				if (pdfBarcodeType == pdfBarcodeType.barCode128)
				{
					bool bCompress = false;
					if (pParam.IsSet("Compress"))
					{
						bCompress = pParam.Bool("Compress");
					}
					this.EncodeCode128(ref text2, bCompress);
				}
				if (flag)
				{
					this.AddCheckCode128(ref text2);
				}
				char c = '\u008a';
				text2 += c;
				num6 = 6 * text2.Length + 1;
				this.BAR_SETUPVARS(num6, 1, false);
				this.CalcCode128(text2, num2, fHeight, fWidth);
				goto IL_610;
			}
			case pdfBarcodeType.barPostal:
				num6 = 5 * (text.Length + 1) * 2 + 3;
				this.BAR_SETUPVARS(num6, 1, false);
				this.CalcPostal(text, num2, fHeight, fWidth);
				goto IL_610;
			case pdfBarcodeType.barUKPostal:
				num6 = 4 * text.Length * 2 + 4 + (flag ? 8 : 0);
				this.BAR_SETUPVARS(num6, 1, false);
				this.CalcUKPostal(text, num2, fHeight, fWidth, flag);
				goto IL_610;
			case pdfBarcodeType.barMailIntelligent:
				num6 = 129;
				this.BAR_SETUPVARS(num6, 1, false);
				this.CalcMailIntelligent(text, num2, fHeight, fWidth);
				goto IL_610;
			}
			AuxException.Throw("Invalid barcode type.", PdfErrors._ERROR_INVALIDARG);
			IL_610:
			if (num6 > 0)
			{
				this.DrawBars(num6, num);
			}
			if (this.m_pParam.IsSet("DrawText"))
			{
				this.bDrawText = this.m_pParam.Bool("DrawText");
			}
			if (this.iNumText > 0 && this.bDrawText)
			{
				if (flag2)
				{
					num5 = (this.m_pParam.IsSet("FontSize") ? this.m_pParam.Number("FontSize") : num5);
					this.m_Text[0].fY = num2;
					this.CenterText(text, 0f, fWidth, ref num5, 0);
				}
				this.DrawText(this.iNumText, num);
			}
		}
		private void AddUPCCheck(ref string Data, int iFirstIsOdd)
		{
			int num = 0;
			for (int i = 0; i < Data.Length; i++)
			{
				num += (int)((Data[i] - '0') * ((i % 2 == iFirstIsOdd) ? '\u0001' : '\u0003'));
			}
			num %= 10;
			num = (10 - num) % 10;
			char c = (char)(num + 48);
			Data += c;
		}
		private void CalcUPCA(string Data, float fY, float fHeight, float fWidth)
		{
			int length = Data.Length;
			if (length < 11 || length > 12)
			{
				AuxException.Throw("A UPC-A code must consist of 11 or 12 numeric digits.", PdfErrors._ERROR_INVALIDARG);
			}
			for (int i = 0; i < length; i++)
			{
				if (Data[i] < '0' || Data[i] > '9')
				{
					AuxException.Throw(string.Format("Character '{0:c}' is invalid for UPC-A.", Data[i]), PdfErrors._ERROR_INVALIDARG);
				}
			}
			if (length == 11)
			{
				this.AddUPCCheck(ref Data, 1);
			}
			float num = fWidth / 95f;
			float num2 = 0.2f * fHeight;
			string data = Data.Substring(1, 5);
			this.CenterText(data, num * 10f, num * 36f, ref num2, 1);
			data = Data.Substring(6, 5);
			num2 = 0.2f * fHeight;
			this.CenterText(data, num * 49f, num * 36f, ref num2, 2);
			float num3 = fY + num2 / 2f;
			float num4 = num3 + num2 * 0.2f;
			this.m_Text[1].fY = (this.m_Text[2].fY = num4);
			float num5 = num2 * 0.75f;
			float fY2 = num4 - num2 + num5;
			this.m_Text[0].fSize = (this.m_Text[3].fSize = num5);
			this.m_Text[0].fY = (this.m_Text[3].fY = fY2);
			this.m_Text[0].fX = -num * 3f - num5 * 0.75f;
			this.m_Text[0].Text = Data.Substring(0, 1);
			this.m_Text[3].fX = fWidth + num * 3f;
			this.m_Text[3].Text = Data.Substring(11, 1);
			this.m_Bars[0].fTop = fY + fHeight;
			this.m_Bars[0].fBottom = fY;
			int j = 0;
			while (j < 3)
			{
				this.m_Bars[j++].fWidth = num;
			}
			for (int k = 0; k < 6; k++)
			{
				for (int l = 0; l <= 3; l++)
				{
					this.m_Bars[j++].fWidth = (float)PdfBarcode.UPCBARS[(int)(Data[k] - '0')][l] * num;
				}
			}
			this.m_Bars[7].fBottom = num3;
			this.m_Bars[j].fBottom = fY;
			for (int m = 1; m <= 5; m++)
			{
				this.m_Bars[j++].fWidth = num;
			}
			this.m_Bars[j].fBottom = num3;
			for (int n = 6; n < 12; n++)
			{
				for (int num6 = 0; num6 <= 3; num6++)
				{
					this.m_Bars[j++].fWidth = (float)PdfBarcode.UPCBARS[(int)(Data[n] - '0')][num6] * num;
				}
			}
			this.m_Bars[52].fBottom = fY;
			for (int num7 = 1; num7 <= 3; num7++)
			{
				this.m_Bars[j++].fWidth = num;
			}
		}
		private void CalcEAN13(string Data, float fY, float fHeight, float fWidth)
		{
			int length = Data.Length;
			if (length < 12 || length > 13)
			{
				AuxException.Throw("An EAN-13 code must consist of 12 or 13 numeric digits.", PdfErrors._ERROR_INVALIDARG);
			}
			for (int i = 0; i < length; i++)
			{
				if (Data[i] < '0' || Data[i] > '9')
				{
					AuxException.Throw(string.Format("Character '(0:c}' is invalid for EAN-13.", Data[i]), PdfErrors._ERROR_INVALIDARG);
				}
			}
			if (length == 12)
			{
				this.AddUPCCheck(ref Data, 0);
			}
			float num = fWidth / 95f;
			float num2 = 0.2f * fHeight;
			string data = Data.Substring(1, 6);
			this.CenterText(data, num * 3f, num * 43f, ref num2, 1);
			data = Data.Substring(7, 6);
			num2 = 0.2f * fHeight;
			this.CenterText(data, num * 49f, num * 43f, ref num2, 2);
			float num3 = fY + num2 / 2f;
			float num4 = num3 + num2 * 0.2f;
			this.m_Text[1].fY = (this.m_Text[2].fY = num4);
			float num5 = num2 * 0.75f;
			float fY2 = num4 - num2 + num5;
			this.m_Text[0].fSize = num5;
			this.m_Text[0].fY = fY2;
			this.m_Text[0].fX = -num * 3f - num5 * 0.75f;
			this.m_Text[0].Text = Data.Substring(0, 1);
			this.m_Bars[0].fTop = fY + fHeight;
			this.m_Bars[0].fBottom = fY;
			int j = 0;
			while (j < 3)
			{
				this.m_Bars[j++].fWidth = num;
			}
			this.m_Bars[j].fBottom = num3;
			for (int k = 1; k <= 12; k++)
			{
				if (k <= 6 && PdfBarcode.EANPARITY[(int)(Data[0] - '0')][k - 1] != 0)
				{
					for (int l = 3; l >= 0; l--)
					{
						this.m_Bars[j++].fWidth = (float)PdfBarcode.UPCBARS[(int)(Data[k] - '0')][l] * num;
					}
				}
				else
				{
					for (int m = 0; m <= 3; m++)
					{
						this.m_Bars[j++].fWidth = (float)PdfBarcode.UPCBARS[(int)(Data[k] - '0')][m] * num;
					}
				}
				if (k == 6)
				{
					this.m_Bars[j].fBottom = fY;
					for (int n = 1; n <= 5; n++)
					{
						this.m_Bars[j++].fWidth = num;
					}
					this.m_Bars[j].fBottom = num3;
				}
			}
			this.m_Bars[j].fBottom = fY;
			for (int num6 = 1; num6 <= 3; num6++)
			{
				this.m_Bars[j++].fWidth = num;
			}
		}
		private void CalcEAN8(string Data, float fY, float fHeight, float fWidth)
		{
			int length = Data.Length;
			if (length < 7 || length > 8)
			{
				AuxException.Throw("An EAN-8 code must consist of 7 or 8 numeric digits.", PdfErrors._ERROR_INVALIDARG);
			}
			for (int i = 0; i < length; i++)
			{
				if (Data[i] < '0' || Data[i] > '9')
				{
					AuxException.Throw(string.Format("Character '{0:c}' in invalid for EAN-8.", Data[i]), PdfErrors._ERROR_INVALIDARG);
				}
			}
			if (length == 7)
			{
				this.AddUPCCheck(ref Data, 1);
			}
			float num = fWidth / 67f;
			float num2 = 0.2f * fHeight;
			string data = Data.Substring(0, 4);
			this.CenterText(data, num * 3f, num * 29f, ref num2, 0);
			data = Data.Substring(4, 4);
			num2 = 0.2f * fHeight;
			this.CenterText(data, num * 35f, num * 29f, ref num2, 1);
			float num3 = fY + num2 / 2f;
			float fY2 = num3 + num2 * 0.2f;
			this.m_Text[0].fY = (this.m_Text[1].fY = fY2);
			this.m_Bars[0].fTop = fY + fHeight;
			this.m_Bars[0].fBottom = fY;
			int j = 0;
			while (j < 3)
			{
				this.m_Bars[j++].fWidth = num;
			}
			this.m_Bars[j].fBottom = num3;
			for (int k = 0; k <= 7; k++)
			{
				for (int l = 0; l <= 3; l++)
				{
					this.m_Bars[j++].fWidth = (float)PdfBarcode.UPCBARS[(int)(Data[k] - '0')][l] * num;
				}
				if (k == 3)
				{
					this.m_Bars[j].fBottom = fY;
					for (int m = 1; m <= 5; m++)
					{
						this.m_Bars[j++].fWidth = num;
					}
					this.m_Bars[j].fBottom = num3;
				}
			}
			this.m_Bars[j].fBottom = fY;
			for (int n = 1; n <= 3; n++)
			{
				this.m_Bars[j++].fWidth = num;
			}
		}
		private void CalcUPCE(string Data, float fY, float fHeight, float fWidth)
		{
			int length = Data.Length;
			if (length != 8)
			{
				AuxException.Throw("A UPC-E code must consist of 8 numeric digits, including a check digit.", PdfErrors._ERROR_INVALIDARG);
			}
			if (Data[0] < '0' || Data[0] > '1')
			{
				AuxException.Throw("A UPC-E code must start with a number-system digit, either 0 or 1.", PdfErrors._ERROR_INVALIDARG);
			}
			for (int i = 0; i < length; i++)
			{
				if (Data[i] < '0' || Data[i] > '9')
				{
					AuxException.Throw(string.Format("Character '{0:c}' is invalid for UPC-E.", Data[i]), PdfErrors._ERROR_INVALIDARG);
				}
			}
			float num = fWidth / 51f;
			float num2 = 0.2f * fHeight;
			string data = Data.Substring(1, 6);
			this.CenterText(data, num * 3f, num * 43f, ref num2, 1);
			float num3 = fY + num2 / 2f;
			float num4 = num3 + num2 * 0.2f;
			this.m_Text[1].fY = num4;
			float num5 = num2 * 0.75f;
			float fY2 = num4 - num2 + num5;
			this.m_Text[0].fSize = (this.m_Text[2].fSize = num5);
			this.m_Text[0].fY = (this.m_Text[2].fY = fY2);
			this.m_Text[0].fX = -num * 3f - num5 * 0.75f;
			this.m_Text[0].Text = Data.Substring(0, 1);
			this.m_Text[2].fX = fWidth + num * 3f;
			this.m_Text[2].Text = Data.Substring(7, 1);
			this.m_Bars[0].fTop = fY + fHeight;
			this.m_Bars[0].fBottom = fY;
			int j = 0;
			while (j < 3)
			{
				this.m_Bars[j++].fWidth = num;
			}
			this.m_Bars[3].fBottom = num3;
			for (int k = 1; k < 7; k++)
			{
				if (((char)PdfBarcode.UPCEPARITY[(int)(Data[7] - '0')][k - 1] ^ Data[0] - '0') != '\0')
				{
					for (int l = 3; l >= 0; l--)
					{
						this.m_Bars[j++].fWidth = (float)PdfBarcode.UPCBARS[(int)(Data[k] - '0')][l] * num;
					}
				}
				else
				{
					for (int m = 0; m <= 3; m++)
					{
						this.m_Bars[j++].fWidth = (float)PdfBarcode.UPCBARS[(int)(Data[k] - '0')][m] * num;
					}
				}
			}
			this.m_Bars[27].fBottom = fY;
			for (int n = 1; n <= 6; n++)
			{
				this.m_Bars[j++].fWidth = num;
			}
		}
		private void CalcUPCWithSupplemental(string Data, float fY, float fHeight, float fWidth)
		{
			int length = Data.Length;
			int num = length;
			if (num == 14 || num == 17)
			{
				string data = Data.Substring(0, (Data[11] < '0' || Data[11] > '9') ? 11 : 12);
				float num2 = (length == 14) ? 20f : 47f;
				float num3 = 10f;
				float num4 = 95f + num3 + num2;
				float num5 = fWidth * 95f / num4;
				float num6 = fWidth * num3 / num4;
				float fWidth2 = fWidth * num2 / num4;
				this.CalcUPCA(data, fY, fHeight, num5);
				data = Data.Substring(12, length - 12);
				this.RecurseSupplemental(data, num5 + num6, fWidth2);
				return;
			}
			AuxException.Throw("A UPC-A barcode with Supplemental must consist of 14 or 17 numeric digits.", PdfErrors._ERROR_INVALIDARG);
		}
		private void CalcUPCEWithSupplemental(string Data, float fY, float fHeight, float fWidth)
		{
			int length = Data.Length;
			int num = length;
			if (num == 10 || num == 13)
			{
				string data = Data.Substring(0, 8);
				float num2 = (length == 10) ? 20f : 47f;
				float num3 = 10f;
				float num4 = 51f + num3 + num2;
				float num5 = fWidth * 51f / num4;
				float num6 = fWidth * num3 / num4;
				float fWidth2 = fWidth * num2 / num4;
				this.CalcUPCE(data, fY, fHeight, num5);
				data = Data.Substring(8, length - 8);
				this.RecurseSupplemental(data, num5 + num6, fWidth2);
				return;
			}
			AuxException.Throw("A UPC-E barcode with Supplemental must consist of 10 or 13 numeric digits.", PdfErrors._ERROR_INVALIDARG);
		}
		private void CalcEAN13WithSupplemental(string Data, float fY, float fHeight, float fWidth)
		{
			int length = Data.Length;
			int num = length;
			if (num == 15 || num == 18)
			{
				string data = Data.Substring(0, (Data[12] < '0' || Data[12] > '9') ? 12 : 13);
				float num2 = (length == 15) ? 20f : 47f;
				float num3 = 10f;
				float num4 = 95f + num3 + num2;
				float num5 = fWidth * 95f / num4;
				float num6 = fWidth * num3 / num4;
				float fWidth2 = fWidth * num2 / num4;
				this.CalcEAN13(data, fY, fHeight, num5);
				data = Data.Substring(13, length - 13);
				this.RecurseSupplemental(data, num5 + num6, fWidth2);
				return;
			}
			AuxException.Throw("An EAN-13 barcode with Supplemental must consist of 15 or 18 numeric digits.", PdfErrors._ERROR_INVALIDARG);
		}
		private void CalcEAN8WithSupplemental(string Data, float fY, float fHeight, float fWidth)
		{
			int length = Data.Length;
			int num = length;
			if (num == 10 || num == 13)
			{
				string data = Data.Substring(0, (Data[7] < '0' || Data[7] > '9') ? 7 : 8);
				float num2 = (length == 10) ? 20f : 47f;
				float num3 = 10f;
				float num4 = 67f + num3 + num2;
				float num5 = fWidth * 67f / num4;
				float num6 = fWidth * num3 / num4;
				float fWidth2 = fWidth * num2 / num4;
				this.CalcEAN8(data, fY, fHeight, num5);
				data = Data.Substring(8, length - 8);
				this.RecurseSupplemental(data, num5 + num6, fWidth2);
				return;
			}
			AuxException.Throw("An EAN-8 barcode with Supplemental must consist of 10 or 13 numeric digits.", PdfErrors._ERROR_INVALIDARG);
		}
		private void RecurseSupplemental(string Data, float fXOffset, float fWidth)
		{
			PdfParam pdfParam = new PdfParam();
			pdfParam.Copy(this.m_pParam);
			PdfBarcode pdfBarcode = new PdfBarcode();
			pdfBarcode.m_pManager = this.m_pManager;
			pdfBarcode.m_pDoc = this.m_pDoc;
			pdfParam["Type"] = ((Data.Length == 2) ? 9f : 10f);
			pdfParam["X"] = this.m_pParam.Number("X") + fXOffset;
			pdfParam["Width"] = fWidth;
			pdfBarcode.Draw(Data, this.m_pCanvas, pdfParam);
		}
		private void CalcSupplemental2(string Data, float fY, float fHeight, float fWidth)
		{
			int length = Data.Length;
			if (length != 2)
			{
				AuxException.Throw("A UPC/EAN Supplemental code must consist of 2 numeric digits.", PdfErrors._ERROR_INVALIDARG);
			}
			for (int i = 0; i < length; i++)
			{
				if (Data[i] < '0' || Data[i] > '9')
				{
					AuxException.Throw(string.Format("Character '{0:c}' is invalid for UPC/EAN Supplemental.", Data[i]), PdfErrors._ERROR_INVALIDARG);
				}
			}
			float num = fWidth / 20f;
			float num2 = 0.2f * fHeight;
			this.CenterText(Data, num * 4f, num * 14f, ref num2, 0);
			this.m_Text[0].fY = fY + fHeight + num2 * 0.05f;
			int num3 = 0;
			int num4 = (int)(((Data[0] - '0') * '\u0002' + Data[1] - '0') % '\u0004');
			this.m_Bars[num3].fBottom = fY;
			this.m_Bars[num3].fTop = fY + fHeight - num2;
			this.m_Bars[num3++].fWidth = num;
			this.m_Bars[num3++].fWidth = num;
			this.m_Bars[num3++].fWidth = num * 2f;
			if (num4 < 2)
			{
				for (int j = 0; j < 4; j++)
				{
					this.m_Bars[num3++].fWidth = num * (float)PdfBarcode.UPCBARS[(int)(Data[0] - '0')][j];
				}
			}
			else
			{
				for (int j = 3; j >= 0; j--)
				{
					this.m_Bars[num3++].fWidth = num * (float)PdfBarcode.UPCBARS[(int)(Data[0] - '0')][j];
				}
			}
			this.m_Bars[num3++].fWidth = num;
			this.m_Bars[num3++].fWidth = num;
			if (num4 % 2 == 0)
			{
				for (int j = 0; j < 4; j++)
				{
					this.m_Bars[num3++].fWidth = num * (float)PdfBarcode.UPCBARS[(int)(Data[1] - '0')][j];
				}
				return;
			}
			for (int j = 3; j >= 0; j--)
			{
				this.m_Bars[num3++].fWidth = num * (float)PdfBarcode.UPCBARS[(int)(Data[1] - '0')][j];
			}
		}
		private void CalcSupplemental5(string Data, float fY, float fHeight, float fWidth)
		{
			int length = Data.Length;
			if (length != 5)
			{
				AuxException.Throw("A UPC/EAN Supplemental code must consist of 5 numeric digits.", PdfErrors._ERROR_INVALIDARG);
			}
			for (int i = 0; i < length; i++)
			{
				if (Data[i] < '0' || Data[i] > '9')
				{
					AuxException.Throw(string.Format("Character '{0:c}' is invalid for UPC/EAN Supplemental.", Data[i]), PdfErrors._ERROR_INVALIDARG);
				}
			}
			float num = fWidth / 47f;
			float num2 = 0.2f * fHeight;
			this.CenterText(Data, num * 6f, num * 35f, ref num2, 0);
			this.m_Text[0].fY = fY + fHeight + num2 * 0.05f;
			int num3 = 0;
			int num4 = (int)('\u0003' * (Data[1] - '0' + Data[3] - '0'));
			num4 += (int)(Data[0] + Data[2] + Data[4] - '\u0090');
			num4 %= 10;
			this.m_Bars[num3].fBottom = fY;
			this.m_Bars[num3].fTop = fY + fHeight - num2;
			this.m_Bars[num3++].fWidth = num;
			this.m_Bars[num3++].fWidth = num;
			this.m_Bars[num3++].fWidth = num * 2f;
			for (int j = 0; j < 5; j++)
			{
				if (PdfBarcode.EXT5PARITY[num4][j] == 0)
				{
					for (int k = 0; k < 4; k++)
					{
						this.m_Bars[num3++].fWidth = num * (float)PdfBarcode.UPCBARS[(int)(Data[j] - '0')][k];
					}
				}
				else
				{
					for (int k = 3; k >= 0; k--)
					{
						this.m_Bars[num3++].fWidth = num * (float)PdfBarcode.UPCBARS[(int)(Data[j] - '0')][k];
					}
				}
				if (j < 4)
				{
					this.m_Bars[num3++].fWidth = num;
					this.m_Bars[num3++].fWidth = num;
				}
			}
		}
		private void CalcCode39(string Data, float fY, float fHeight, float fWidth, float fWideMult)
		{
			string text = string.Format("*{0}*", Data).ToUpper();
			float num = fWidth / (float)text.Length / (7f + 3f * fWideMult);
			int num2 = 0;
			this.m_Bars[0].fBottom = fY;
			this.m_Bars[0].fTop = fY + fHeight;
			for (int i = 0; i < text.Length; i++)
			{
				int num3 = PdfBarcode.CODE39CHARS.IndexOf(text[i]);
				if (num3 == -1)
				{
					AuxException.Throw(string.Format("Character '{0:c}' is invalid for Code-39.", text[i]), PdfErrors._ERROR_INVALIDARG);
				}
				int num4 = num3;
				for (int j = 0; j < 9; j++)
				{
					this.m_Bars[num2++].fWidth = num * ((PdfBarcode.CODE39BARS[num4][j] != 0) ? fWideMult : 1f);
				}
				this.m_Bars[num2++].fWidth = num;
			}
		}
		private void CalcCode39Extended(ref string Data)
		{
			string text = Data;
			string text2 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. ";
			string text3 = "\u001b\u001c\u001d\u001e\u001f;<=>?[\\]^_{|}~\u007f\u0080@`";
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < text.Length; i++)
			{
				int num = (int)text[i];
				int num2 = text2.IndexOf((char)num);
				if (num2 == -1)
				{
					if (1 <= num && num <= 26)
					{
						stringBuilder.Append("$");
						num += 64;
					}
					else
					{
						if (97 <= num && num <= 122)
						{
							stringBuilder.Append("+");
							num -= 32;
						}
						else
						{
							if (33 <= num && num <= 58)
							{
								stringBuilder.Append("/");
								num += 32;
							}
							else
							{
								if (num == 0)
								{
									stringBuilder.Append("%");
									num = 85;
								}
								else
								{
									num2 = text3.IndexOf((char)num);
									if (num2 == -1)
									{
										AuxException.Throw(string.Format("Character '{0:c}' is invalid for Code-39 Extended.", (char)num), PdfErrors._ERROR_INVALIDARG);
									}
									int num3 = num2;
									stringBuilder.Append("%");
									num = num3 + 65;
								}
							}
						}
					}
				}
				stringBuilder.Append((char)num);
			}
			Data = stringBuilder.ToString();
		}
		private void CalcIndustrial25(string Data, float fY, float fHeight, float fWidth, float fWideMult)
		{
			int length = Data.Length;
			for (int i = 0; i < length; i++)
			{
				if (Data[i] < '0' || Data[i] > '9')
				{
					AuxException.Throw(string.Format("Character '{0:c}' is invalid for Industrial 2 of 5.", Data[i]), PdfErrors._ERROR_INVALIDARG);
				}
			}
			float num = fWidth / (4f * fWideMult + 7f + (2f * fWideMult + 3f + 5f) * (float)length);
			this.m_Bars[0].fBottom = fY;
			this.m_Bars[0].fTop = fY + fHeight;
			int num2 = 0;
			this.m_Bars[num2++].fWidth = num * fWideMult;
			this.m_Bars[num2++].fWidth = num;
			this.m_Bars[num2++].fWidth = num * fWideMult;
			this.m_Bars[num2++].fWidth = num;
			this.m_Bars[num2++].fWidth = num;
			this.m_Bars[num2++].fWidth = num;
			for (int j = 0; j < length; j++)
			{
				for (int k = 0; k < 5; k++)
				{
					this.m_Bars[num2++].fWidth = num * ((PdfBarcode.INDBARS[(int)(Data[j] - '0')][k] != 0) ? fWideMult : 1f);
					this.m_Bars[num2++].fWidth = num;
				}
			}
			this.m_Bars[num2++].fWidth = num * fWideMult;
			this.m_Bars[num2++].fWidth = num;
			this.m_Bars[num2++].fWidth = num;
			this.m_Bars[num2++].fWidth = num;
			this.m_Bars[num2].fWidth = num * fWideMult;
		}
		private void CalcInterleave25(string Data, float fY, float fHeight, float fWidth, float fWideMult, bool bSpecial)
		{
			int num = Data.Length;
			for (int i = 0; i < num; i++)
			{
				if (Data[i] < '0' || Data[i] > '9')
				{
					AuxException.Throw(string.Format("Character '{0:c}' is invalid for Interleave 2 of 5.", Data[i]), PdfErrors._ERROR_INVALIDARG);
				}
			}
			string text = "";
			if (num % 2 != 0)
			{
				text += "0";
				num++;
			}
			text += Data;
			int num2 = num / 2;
			float num3;
			if (!bSpecial)
			{
				num3 = fWidth / (fWideMult * (float)(1 + 4 * num2) + (float)(6 + 6 * num2));
			}
			else
			{
				num3 = fWidth / (float)(6 + 27 * num2 + 7);
			}
			this.m_Bars[0].fBottom = fY;
			this.m_Bars[0].fTop = fY + fHeight;
			int num4 = 0;
			this.m_Bars[num4++].fWidth = num3;
			this.m_Bars[num4++].fWidth = num3 * (float)(bSpecial ? 2 : 1);
			this.m_Bars[num4++].fWidth = num3;
			this.m_Bars[num4++].fWidth = num3 * (float)(bSpecial ? 2 : 1);
			for (int j = 0; j < num2; j++)
			{
				for (int k = 0; k < 5; k++)
				{
					if (!bSpecial)
					{
						this.m_Bars[num4++].fWidth = num3 * ((PdfBarcode.INDBARS[(int)(text[j * 2] - '0')][k] != 0) ? fWideMult : 1f);
						this.m_Bars[num4++].fWidth = num3 * ((PdfBarcode.INDBARS[(int)(text[j * 2 + 1] - '0')][k] != 0) ? fWideMult : 1f);
					}
					else
					{
						this.m_Bars[num4++].fWidth = num3 * (float)((PdfBarcode.INDBARS[(int)(text[j * 2] - '0')][k] != 0) ? 4 : 1);
						this.m_Bars[num4++].fWidth = num3 * (float)((PdfBarcode.INDBARS[(int)(text[j * 2 + 1] - '0')][k] != 0) ? 5 : 2);
					}
				}
			}
			this.m_Bars[num4++].fWidth = num3 * (float)(bSpecial ? 4 : 2);
			this.m_Bars[num4++].fWidth = num3;
			this.m_Bars[num4++].fWidth = num3;
		}
		private void CalcDataLogic25(string Data, float fY, float fHeight, float fWidth, float fWideMult)
		{
			int length = Data.Length;
			for (int i = 0; i < length; i++)
			{
				if (Data[i] < '0' || Data[i] > '9')
				{
					AuxException.Throw(string.Format("Character '{0:c}' is invalid for Datalogic 2 of 5.", Data[i]), PdfErrors._ERROR_INVALIDARG);
				}
			}
			float num = fWidth / (2f * fWideMult * 1.5f + 9f + (2f * fWideMult + 3f + 1f) * (float)length);
			this.m_Bars[0].fBottom = fY;
			this.m_Bars[0].fTop = fY + fHeight;
			int num2 = 0;
			this.m_Bars[num2++].fWidth = num * fWideMult * 1.5f;
			this.m_Bars[num2++].fWidth = num;
			this.m_Bars[num2++].fWidth = num;
			this.m_Bars[num2++].fWidth = num;
			this.m_Bars[num2++].fWidth = num;
			this.m_Bars[num2++].fWidth = num;
			for (int j = 0; j < length; j++)
			{
				for (int k = 0; k < 5; k++)
				{
					this.m_Bars[num2++].fWidth = num * ((PdfBarcode.INDBARS[(int)(Data[j] - '0')][k] != 0) ? fWideMult : 1f);
				}
				this.m_Bars[num2++].fWidth = num;
			}
			this.m_Bars[num2++].fWidth = num * fWideMult * 1.5f;
			this.m_Bars[num2++].fWidth = num;
			this.m_Bars[num2++].fWidth = num;
			this.m_Bars[num2++].fWidth = num;
			this.m_Bars[num2].fWidth = num;
		}
		private void CalcPlessey(string Data, float fY, float fHeight, float fWidth, float fWideMult)
		{
			int length = Data.Length;
			for (int i = 0; i < length; i++)
			{
				if (Data[i] < '0' || Data[i] > '9')
				{
					AuxException.Throw(string.Format("Character '{0:c}' is invalid for MSI/Plessey.", Data[i]), PdfErrors._ERROR_INVALIDARG);
				}
			}
			float num = fWidth / (2f * fWideMult + 3f + (4f * fWideMult + 4f) * (float)length);
			this.m_Bars[0].fBottom = fY;
			this.m_Bars[0].fTop = fY + fHeight;
			int num2 = 0;
			this.m_Bars[num2++].fWidth = num * fWideMult;
			this.m_Bars[num2++].fWidth = num;
			for (int j = 0; j < length; j++)
			{
				int num3 = (int)(Data[j] - '0');
				for (int k = 8; k > 0; k >>= 1)
				{
					this.m_Bars[num2++].fWidth = num * (((num3 & k) != 0) ? fWideMult : 1f);
					this.m_Bars[num2++].fWidth = num * (((num3 & k) != 0) ? 1f : fWideMult);
				}
			}
			this.m_Bars[num2++].fWidth = num;
			this.m_Bars[num2++].fWidth = num * fWideMult;
			this.m_Bars[num2].fWidth = num;
		}
		private void CalcCodabar(string Data, float fY, float fHeight, float fWidth)
		{
			int arg_06_0 = Data.Length;
			char c = Data[0];
			if (c < 'A' || c > 'D' || c != Data[Data.Length - 1])
			{
				AuxException.Throw("Codabar requires matching start and stop characters, which must be A, B, C, or D.", PdfErrors._ERROR_INVALIDARG);
			}
			float num = -2f;
			for (int i = 0; i < Data.Length; i++)
			{
				int num2 = PdfBarcode.CODABARCHARS.IndexOf(Data[i]);
				if (num2 == -1)
				{
					AuxException.Throw(string.Format("Character '{0:c}' is invalid for Codabar.", Data[i]), PdfErrors._ERROR_INVALIDARG);
				}
				int num3 = num2;
				for (int j = 0; j < 7; j++)
				{
					num += (float)(j % 2 + ((PdfBarcode.CODABARS[num3][j] != 0) ? 3 : 1));
				}
				num += 2f;
			}
			float num4 = fWidth / num;
			int num5 = 0;
			this.m_Bars[0].fBottom = fY;
			this.m_Bars[0].fTop = fY + fHeight;
			for (int k = 0; k < Data.Length; k++)
			{
				int num6 = PdfBarcode.CODABARCHARS.IndexOf(Data[k]);
				for (int l = 0; l < 7; l++)
				{
					this.m_Bars[num5++].fWidth = num4 * (float)(l % 2 + ((PdfBarcode.CODABARS[num6][l] != 0) ? 3 : 1));
				}
				this.m_Bars[num5++].fWidth = num4 * 2f;
			}
		}
		private void CalcCode11(string Data, float fY, float fHeight, float fWidth, float fWideMult)
		{
			string text = string.Format("S{0}S", Data);
			int arg_12_0 = text.Length;
			float num = -1f;
			for (int i = 0; i < text.Length; i++)
			{
				int num2 = PdfBarcode.CODE11CHARS.IndexOf(text[i]);
				if (num2 == -1)
				{
					AuxException.Throw(string.Format("Character '{0:c}' is invalid for Code-11.", text[i]), PdfErrors._ERROR_INVALIDARG);
				}
				int num3 = num2;
				for (int j = 0; j < 5; j++)
				{
					num += ((PdfBarcode.CODE11BARS[num3][j] != 0) ? fWideMult : 1f);
				}
				num += 1f;
			}
			float num4 = fWidth / num;
			int num5 = 0;
			this.m_Bars[0].fBottom = fY;
			this.m_Bars[0].fTop = fY + fHeight;
			for (int k = 0; k < text.Length; k++)
			{
				int num6 = PdfBarcode.CODE11CHARS.IndexOf(text[k]);
				for (int l = 0; l < 5; l++)
				{
					this.m_Bars[num5++].fWidth = num4 * ((PdfBarcode.CODE11BARS[num6][l] != 0) ? fWideMult : 1f);
				}
				this.m_Bars[num5++].fWidth = num4;
			}
		}
		private void EncodeCode93(ref string Data, bool bCheckDigit)
		{
			string text = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%";
			string text2 = "\u001b\u001c\u001d\u001e\u001f;<=>?[\\]^_{|}~\u007f\u0080@`";
			string text3 = Data;
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < text3.Length; i++)
			{
				int num = (int)text3[i];
				int num2 = text.IndexOf((char)num);
				if (num2 == -1)
				{
					if (1 <= num && num <= 26)
					{
						stringBuilder.Append("ü");
						num += 64;
					}
					else
					{
						if (97 <= num && num <= 122)
						{
							stringBuilder.Append("þ");
							num -= 32;
						}
						else
						{
							if (33 <= num && num <= 58)
							{
								stringBuilder.Append("ý");
								num += 32;
							}
							else
							{
								if (num == 0)
								{
									stringBuilder.Append("ÿ");
									num = 85;
								}
								else
								{
									num2 = text2.IndexOf((char)num);
									if (num2 == -1)
									{
										AuxException.Throw(string.Format("Character '{0:c}' is invalid for Code-93.", (char)num), PdfErrors._ERROR_INVALIDARG);
									}
									int num3 = num2;
									stringBuilder.Append("ÿ");
									num = num3 + 65;
								}
							}
						}
					}
				}
				stringBuilder.Append((char)num);
			}
			Data = stringBuilder.ToString();
			if (bCheckDigit)
			{
				int num4 = 0;
				int num5 = 0;
				int num6 = 1;
				int num7 = 2;
				int num8;
				int num9;
				for (int j = Data.Length - 1; j >= 0; j--)
				{
					num8 = (int)Data[j];
					num9 = PdfBarcode.CODE93CHARS.IndexOf((char)num8);
					num4 += num9 * num6;
					num5 += num9 * num7;
					num6 %= 20;
					num7 %= 15;
					num6++;
					num7++;
				}
				num4 %= 47;
				num8 = (int)PdfBarcode.CODE93CHARS[num4];
				stringBuilder.Append((char)num8);
				num9 = PdfBarcode.CODE93CHARS.IndexOf((char)num8);
				num5 += num9;
				num5 %= 47;
				num8 = (int)PdfBarcode.CODE93CHARS[num5];
				stringBuilder.Append((char)num8);
			}
			Data = stringBuilder.ToString();
		}
		private void CalcCode93(string Data, float fY, float fHeight, float fWidth)
		{
			string text = string.Format("û{0}û", Data);
			float num = fWidth / (float)(text.Length * 9 + 1);
			int num2 = 0;
			this.m_Bars[0].fBottom = fY;
			this.m_Bars[0].fTop = fY + fHeight;
			for (int i = 0; i < text.Length; i++)
			{
				int num3 = PdfBarcode.CODE93CHARS.IndexOf(text[i]);
				if (num3 == -1)
				{
					AuxException.Throw(string.Format("Character '{0:c}' is invalid for Code-93.", text[i]), PdfErrors._ERROR_INVALIDARG);
				}
				int num4 = num3;
				for (int j = 0; j < 6; j++)
				{
					this.m_Bars[num2++].fWidth = num * (float)PdfBarcode.CODE93BARS[num4][j];
				}
			}
			this.m_Bars[num2].fWidth = num;
		}
		private void CalcCode128(string Data, float fY, float fHeight, float fWidth)
		{
			float num = fWidth / (float)(Data.Length * 11 + 2);
			int num2 = 0;
			this.m_Bars[0].fBottom = fY;
			this.m_Bars[0].fTop = fY + fHeight;
			for (int i = 0; i < Data.Length; i++)
			{
				int num3 = (int)Data[i];
				num3 -= 32;
				if (num3 < 0 || num3 > 106)
				{
					AuxException.Throw(string.Format("Character '{0:c}' is invalid for Code-128.", Data[i]), PdfErrors._ERROR_INVALIDARG);
				}
				for (int j = 0; j < 6; j++)
				{
					this.m_Bars[num2++].fWidth = num * (float)PdfBarcode.CODE128BARS[num3][j];
				}
			}
			this.m_Bars[num2].fWidth = num * 2f;
		}
		private bool IsNumeric(char c)
		{
			return '0' <= c && c <= '9';
		}
		private void EncodeCode128(ref string Data, bool bCompress)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int i = 0;
			int num = 0;
			byte b = 135;
			byte b2 = 136;
			byte b3 = 137;
			while (i < Data.Length)
			{
				char c = Data[i];
				if (bCompress && this.IsNumeric(c) && i + 3 < Data.Length && this.IsNumeric(Data[i + 1]) && this.IsNumeric(Data[i + 2]) && this.IsNumeric(Data[i + 3]))
				{
					num = 3;
					char value = (char)b3;
					stringBuilder.Append(value);
					while (i < Data.Length - 1 && this.IsNumeric(Data[i]) && this.IsNumeric(Data[i + 1]))
					{
						int num2 = (int)('\n' * (Data[i] - '0') + Data[i + 1] - '0');
						value = (char)(num2 + 32);
						stringBuilder.Append(value);
						i += 2;
					}
					i--;
				}
				else
				{
					if (' ' <= c && c <= '_')
					{
						switch (num)
						{
						case 0:
						case 3:
							num = 0;
							for (int j = i; j < Data.Length; j++)
							{
								if (Data[j] < ' ')
								{
									char value = (char)b;
									stringBuilder.Append(value);
									num = 1;
									break;
								}
								if (Data[j] > '_')
								{
									char value = (char)b2;
									stringBuilder.Append(value);
									num = 2;
									break;
								}
							}
							if (num == 0)
							{
								char value = (char)b;
								stringBuilder.Append(value);
								num = 1;
							}
							stringBuilder.Append(c);
							goto IL_22F;
						case 1:
						case 2:
						{
							char value = c;
							stringBuilder.Append(value);
							goto IL_22F;
						}
						}
					}
					if ('\0' <= c && c <= '\u001f')
					{
						c += '`';
						char value;
						if (num != 1)
						{
							value = (char)b;
							stringBuilder.Append(value);
							num = 1;
						}
						value = c;
						stringBuilder.Append(value);
					}
					else
					{
						if ('`' <= c && c <= '\u007f')
						{
							char value;
							if (num != 2)
							{
								value = (char)b2;
								stringBuilder.Append(value);
								num = 2;
							}
							value = c;
							stringBuilder.Append(value);
						}
						else
						{
							AuxException.Throw(string.Format("Character '{0:c}' is invalid for Code-128.", c), PdfErrors._ERROR_INVALIDARG);
						}
					}
				}
				IL_22F:
				i++;
			}
			Data = stringBuilder.ToString();
		}
		private void AddCheckCode128(ref string Data)
		{
			int num = 0;
			for (int i = 0; i < Data.Length; i++)
			{
				int num2 = (int)Data[i];
				num2 -= 32;
				if (num2 < 0 || num2 > 106)
				{
					AuxException.Throw(string.Format("Character '{0:c}' is invalid for Code-128.", Data[i]), PdfErrors._ERROR_INVALIDARG);
				}
				num += ((i != 0) ? i : 1) * num2;
			}
			num %= 103;
			char c = (char)(num + 32);
			Data += c;
		}
		private void CalcPostal(string Data, float fY, float fHeight, float fWidth)
		{
			int num = 0;
			for (int i = 0; i < Data.Length; i++)
			{
				if (Data[i] < '0' || Data[i] > '9')
				{
					AuxException.Throw(string.Format("Character '{0:c}' is invalid for US Postal Code.", Data[i]), PdfErrors._ERROR_INVALIDARG);
				}
				num += (int)(Data[i] - '0');
			}
			num %= 10;
			num = (10 - num) % 10;
			char c = (char)(num + 48);
			string text = Data + c;
			float fWidth2 = fWidth / (float)(text.Length * 10 + 1);
			int num2;
			this.m_Bars[num2 = 0].fBottom = fY;
			this.m_Bars[num2].fTop = fY + fHeight;
			this.m_Bars[num2++].fWidth = fWidth2;
			this.m_Bars[num2++].fWidth = fWidth2;
			for (int j = 0; j < text.Length; j++)
			{
				for (int k = 0; k < 5; k++)
				{
					if (PdfBarcode.POSTALBARS[(int)(text[j] - '0')][k] != 0)
					{
						this.m_Bars[num2].fTop = fY + fHeight;
					}
					else
					{
						this.m_Bars[num2].fTop = fY + fHeight / 4f;
					}
					this.m_Bars[num2++].fWidth = fWidth2;
					this.m_Bars[num2++].fWidth = fWidth2;
				}
			}
			this.m_Bars[num2].fTop = fY + fHeight;
			this.m_Bars[num2].fWidth = fWidth2;
		}
		private void CalcUKPostal(string Data, float fY, float fHeight, float fWidth, bool bCheckDigit)
		{
			float fWidth2 = fWidth / (float)(Data.Length * 8 + (bCheckDigit ? 8 : 0) + 4 - 1);
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			this.m_Bars[num].fBottom = fY + fHeight * 3f / 7f;
			this.m_Bars[num].fTop = fY + fHeight;
			this.m_Bars[num++].fWidth = fWidth2;
			this.m_Bars[num++].fWidth = fWidth2;
			int num4 = Data.Length + (bCheckDigit ? 1 : 0);
			for (int i = 0; i < num4; i++)
			{
				int num5 = 0;
				if (i < Data.Length)
				{
					if (Data[i] >= '0' && Data[i] <= '9')
					{
						num5 = (int)(Data[i] - '0');
					}
					else
					{
						if (Data[i] >= 'A' && Data[i] <= 'Z')
						{
							num5 = (int)(Data[i] - 'A' + '\n');
						}
						else
						{
							AuxException.Throw(string.Format("Character '{0:c}' is invalid for UK Postal Code.", Data[i]), PdfErrors._ERROR_INVALIDARG);
						}
					}
				}
				else
				{
					num5 = this.nCheckChar[num2 % 6][num3 % 6];
				}
				num2 += (int)PdfBarcode.UKPOSTALBARS[num5][4];
				num3 += (int)PdfBarcode.UKPOSTALBARS[num5][5];
				for (int j = 0; j < 4; j++)
				{
					int num6 = (int)PdfBarcode.UKPOSTALBARS[num5][j];
					if ((num6 & 2) != 0)
					{
						this.m_Bars[num].fBottom = fY;
					}
					else
					{
						this.m_Bars[num].fBottom = fY + fHeight * 3f / 7f;
					}
					if ((num6 & 1) != 0)
					{
						this.m_Bars[num].fTop = fY + fHeight;
					}
					else
					{
						this.m_Bars[num].fTop = fY + fHeight * 4f / 7f;
					}
					this.m_Bars[num++].fWidth = fWidth2;
					this.m_Bars[num++].fWidth = fWidth2;
				}
			}
			this.m_Bars[num].fBottom = fY;
			this.m_Bars[num].fTop = fY + fHeight;
			this.m_Bars[num++].fWidth = fWidth2;
			this.m_Bars[num++].fWidth = fWidth2;
		}
		private void CalcMailIntelligent(string Data, float fY, float fHeight, float fWidth)
		{
			int length = Data.Length;
			if (length != 20 && length != 25 && length != 29 && length != 31)
			{
				AuxException.Throw("Invalid data length for Intelligent Mail Code, must be 20, 25, 29 or 31 characters.", PdfErrors._ERROR_INVALIDARG);
			}
			for (int i = 0; i < length; i++)
			{
				if (Data[i] < '0' || Data[i] > '9')
				{
					AuxException.Throw("Invalid character for Intelligent Mail Code, must be all digits.", PdfErrors._ERROR_INVALIDARG);
				}
			}
			if (Data[1] < '0' || Data[1] > '4')
			{
				AuxException.Throw("Invalid digit #2 for Intelligent Mail Code, must be 0, 1, 2, 3 or 4.", PdfErrors._ERROR_INVALIDARG);
			}
			string tracking = Data.Substring(0, 20);
			string routing = Data.Substring(20);
			string text = BigNumber.ComputeBarcode(tracking, routing);
			float num = fWidth / 322f;
			int num2 = 0;
			for (int j = 0; j < 65; j++)
			{
				if (text[j] == 'A')
				{
					this.m_Bars[num2].fBottom = fY + fHeight / 3f;
					this.m_Bars[num2].fTop = fY + fHeight;
				}
				else
				{
					if (text[j] == 'F')
					{
						this.m_Bars[num2].fBottom = fY;
						this.m_Bars[num2].fTop = fY + fHeight;
					}
					else
					{
						if (text[j] == 'D')
						{
							this.m_Bars[num2].fBottom = fY;
							this.m_Bars[num2].fTop = fY + fHeight * 2f / 3f;
						}
						else
						{
							this.m_Bars[num2].fBottom = fY + fHeight / 3f;
							this.m_Bars[num2].fTop = fY + fHeight * 2f / 3f;
						}
					}
				}
				this.m_Bars[num2++].fWidth = num * 2f;
				if (j < 64)
				{
					this.m_Bars[num2++].fWidth = num * 3f;
				}
			}
		}
		private void DrawBars(int iNumBars, float fX)
		{
			float num = fX;
			float num2 = 0f;
			float num3 = 0f;
			bool flag = true;
			AuxRGB auxRGB = new AuxRGB();
			if (this.m_pParam.IsSet("Color"))
			{
				this.m_pParam.Color("Color", ref auxRGB);
			}
			bool flag2 = false;
			if (this.m_pParam.IsSet("tc") && this.m_pParam.IsSet("tm") && this.m_pParam.IsSet("ty") && this.m_pParam.IsSet("tk"))
			{
				float num4 = this.m_pParam.Number("tc");
				float num5 = this.m_pParam.Number("tm");
				float num6 = this.m_pParam.Number("ty");
				float num7 = this.m_pParam.Number("tk");
				if (num4 < 0f || num4 > 1f || num5 < 0f || num5 > 1f || num6 < 0f || num6 > 1f || num7 < 0f || num7 > 1f)
				{
					AuxException.Throw("A CMYK value must be between 0.0 and 1.0.", PdfErrors._ERROR_INVALIDARG);
				}
				this.m_pCanvas.SetFillColorCMYK(num4, num5, num6, num7);
				flag2 = true;
			}
			AuxRGB auxRGB2 = new AuxRGB();
			if (this.m_pParam.IsSet("BgColor"))
			{
				this.m_pParam.Color("BgColor", ref auxRGB2);
			}
			else
			{
				flag = false;
			}
			for (int i = 0; i < iNumBars; i++)
			{
				num2 = ((this.m_Bars[i].fBottom == PdfBarcode.BAR_UNSPEC) ? num2 : this.m_Bars[i].fBottom);
				num3 = ((this.m_Bars[i].fTop == PdfBarcode.BAR_UNSPEC) ? num3 : this.m_Bars[i].fTop);
				if (this.m_Bars[i].fWidth == PdfBarcode.BAR_UNSPEC)
				{
					AuxException.Throw("Internal barcode error", PdfErrors._ERROR_INVALIDARG);
				}
				if (i % 2 == 0)
				{
					if (!flag2)
					{
						this.m_pCanvas.SetFillColor(auxRGB.r, auxRGB.g, auxRGB.b);
					}
				}
				else
				{
					if (flag)
					{
						this.m_pCanvas.SetFillColor(auxRGB2.r, auxRGB2.g, auxRGB2.b);
					}
				}
				if (i % 2 == 0 || flag)
				{
					this.m_pCanvas.FillRect(num, num2, this.m_Bars[i].fWidth, num3 - num2);
				}
				num += this.m_Bars[i].fWidth;
			}
		}
		private void DrawText(int iNumText, float fX)
		{
			PdfParam pdfParam = this.m_pManager.CreateParam();
			for (int i = 0; i < iNumText; i++)
			{
				pdfParam["X"] = this.m_Text[i].fX + fX;
				pdfParam["Y"] = this.m_Text[i].fY;
				pdfParam["size"] = this.m_Text[i].fSize;
				this.m_pCanvas.DrawText(this.m_Text[i].Text, pdfParam, this.m_ptrFont);
			}
		}
		private void CenterText(string Data, float fLeftX, float fWidth, ref float fFontSize, int iIndex)
		{
			PdfRect textExtent = this.m_ptrFont.GetTextExtent(Data, fFontSize);
			float width = textExtent.Width;
			if (width > fWidth)
			{
				fFontSize *= fWidth / width;
				textExtent = this.m_ptrFont.GetTextExtent(Data, fFontSize);
				width = textExtent.Width;
			}
			this.m_Text[iIndex].fX = fLeftX + fWidth / 2f - width / 2f;
			this.m_Text[iIndex].fSize = fFontSize;
			this.m_Text[iIndex].Text = Data;
		}
		static PdfBarcode()
		{
			// Note: this type is marked as 'beforefieldinit'.
			byte[][] array = new byte[10][];
			byte[][] arg_4DC_0 = array;
			int arg_4DC_1 = 0;
			byte[] array2 = new byte[5];
			array2[2] = 1;
			array2[3] = 1;
			arg_4DC_0[arg_4DC_1] = array2;
			array[1] = new byte[]
			{
				1,
				0,
				0,
				0,
				1
			};
			array[2] = new byte[]
			{
				0,
				1,
				0,
				0,
				1
			};
			byte[][] arg_521_0 = array;
			int arg_521_1 = 3;
			byte[] array3 = new byte[5];
			array3[0] = 1;
			array3[1] = 1;
			arg_521_0[arg_521_1] = array3;
			array[4] = new byte[]
			{
				0,
				0,
				1,
				0,
				1
			};
			byte[][] arg_54F_0 = array;
			int arg_54F_1 = 5;
			byte[] array4 = new byte[5];
			array4[0] = 1;
			array4[2] = 1;
			arg_54F_0[arg_54F_1] = array4;
			byte[][] arg_566_0 = array;
			int arg_566_1 = 6;
			byte[] array5 = new byte[5];
			array5[1] = 1;
			array5[2] = 1;
			arg_566_0[arg_566_1] = array5;
			array[7] = new byte[]
			{
				0,
				0,
				0,
				1,
				1
			};
			byte[][] arg_594_0 = array;
			int arg_594_1 = 8;
			byte[] array6 = new byte[5];
			array6[0] = 1;
			array6[3] = 1;
			arg_594_0[arg_594_1] = array6;
			byte[][] arg_5AC_0 = array;
			int arg_5AC_1 = 9;
			byte[] array7 = new byte[5];
			array7[1] = 1;
			array7[3] = 1;
			arg_5AC_0[arg_5AC_1] = array7;
			PdfBarcode.INDBARS = array;
			PdfBarcode.CODABARCHARS = "0123456789-$:/.+ABCD";
			byte[][] array8 = new byte[20][];
			array8[0] = new byte[]
			{
				0,
				0,
				0,
				0,
				0,
				1,
				1
			};
			byte[][] arg_5F5_0 = array8;
			int arg_5F5_1 = 1;
			byte[] array9 = new byte[7];
			array9[4] = 1;
			array9[5] = 1;
			arg_5F5_0[arg_5F5_1] = array9;
			array8[2] = new byte[]
			{
				0,
				0,
				0,
				1,
				0,
				0,
				1
			};
			byte[][] arg_625_0 = array8;
			int arg_625_1 = 3;
			byte[] array10 = new byte[7];
			array10[0] = 1;
			array10[1] = 1;
			arg_625_0[arg_625_1] = array10;
			byte[][] arg_63D_0 = array8;
			int arg_63D_1 = 4;
			byte[] array11 = new byte[7];
			array11[2] = 1;
			array11[5] = 1;
			arg_63D_0[arg_63D_1] = array11;
			byte[][] arg_655_0 = array8;
			int arg_655_1 = 5;
			byte[] array12 = new byte[7];
			array12[0] = 1;
			array12[5] = 1;
			arg_655_0[arg_655_1] = array12;
			array8[6] = new byte[]
			{
				0,
				1,
				0,
				0,
				0,
				0,
				1
			};
			byte[][] arg_685_0 = array8;
			int arg_685_1 = 7;
			byte[] array13 = new byte[7];
			array13[1] = 1;
			array13[4] = 1;
			arg_685_0[arg_685_1] = array13;
			byte[][] arg_69D_0 = array8;
			int arg_69D_1 = 8;
			byte[] array14 = new byte[7];
			array14[1] = 1;
			array14[2] = 1;
			arg_69D_0[arg_69D_1] = array14;
			byte[][] arg_6B6_0 = array8;
			int arg_6B6_1 = 9;
			byte[] array15 = new byte[7];
			array15[0] = 1;
			array15[3] = 1;
			arg_6B6_0[arg_6B6_1] = array15;
			byte[][] arg_6CF_0 = array8;
			int arg_6CF_1 = 10;
			byte[] array16 = new byte[7];
			array16[3] = 1;
			array16[4] = 1;
			arg_6CF_0[arg_6CF_1] = array16;
			byte[][] arg_6E8_0 = array8;
			int arg_6E8_1 = 11;
			byte[] array17 = new byte[7];
			array17[2] = 1;
			array17[3] = 1;
			arg_6E8_0[arg_6E8_1] = array17;
			array8[12] = new byte[]
			{
				1,
				0,
				0,
				0,
				1,
				0,
				1
			};
			array8[13] = new byte[]
			{
				1,
				0,
				1,
				0,
				0,
				0,
				1
			};
			array8[14] = new byte[]
			{
				1,
				0,
				1,
				0,
				1,
				0,
				0
			};
			array8[15] = new byte[]
			{
				0,
				0,
				1,
				0,
				1,
				0,
				1
			};
			array8[16] = new byte[]
			{
				0,
				0,
				1,
				1,
				0,
				1,
				0
			};
			array8[17] = new byte[]
			{
				0,
				1,
				0,
				1,
				0,
				0,
				1
			};
			array8[18] = new byte[]
			{
				0,
				0,
				0,
				1,
				0,
				1,
				1
			};
			array8[19] = new byte[]
			{
				0,
				0,
				0,
				1,
				1,
				1,
				0
			};
			PdfBarcode.CODABARS = array8;
			PdfBarcode.UPCEPARITY = new byte[][]
			{
				new byte[]
				{
					1,
					1,
					1,
					0,
					0,
					0
				},
				new byte[]
				{
					1,
					1,
					0,
					1,
					0,
					0
				},
				new byte[]
				{
					1,
					1,
					0,
					0,
					1,
					0
				},
				new byte[]
				{
					1,
					1,
					0,
					0,
					0,
					1
				},
				new byte[]
				{
					1,
					0,
					1,
					1,
					0,
					0
				},
				new byte[]
				{
					1,
					0,
					0,
					1,
					1,
					0
				},
				new byte[]
				{
					1,
					0,
					0,
					0,
					1,
					1
				},
				new byte[]
				{
					1,
					0,
					1,
					0,
					1,
					0
				},
				new byte[]
				{
					1,
					0,
					1,
					0,
					0,
					1
				},
				new byte[]
				{
					1,
					0,
					0,
					1,
					0,
					1
				}
			};
			byte[][] array18 = new byte[10][];
			byte[][] arg_899_0 = array18;
			int arg_899_1 = 0;
			byte[] array19 = new byte[6];
			arg_899_0[arg_899_1] = array19;
			array18[1] = new byte[]
			{
				0,
				0,
				1,
				0,
				1,
				1
			};
			array18[2] = new byte[]
			{
				0,
				0,
				1,
				1,
				0,
				1
			};
			array18[3] = new byte[]
			{
				0,
				0,
				1,
				1,
				1,
				0
			};
			array18[4] = new byte[]
			{
				0,
				1,
				0,
				0,
				1,
				1
			};
			array18[5] = new byte[]
			{
				0,
				1,
				1,
				0,
				0,
				1
			};
			array18[6] = new byte[]
			{
				0,
				1,
				1,
				1,
				0,
				0
			};
			array18[7] = new byte[]
			{
				0,
				1,
				0,
				1,
				0,
				1
			};
			array18[8] = new byte[]
			{
				0,
				1,
				0,
				1,
				1,
				0
			};
			array18[9] = new byte[]
			{
				0,
				1,
				1,
				0,
				1,
				0
			};
			PdfBarcode.EANPARITY = array18;
			byte[][] array20 = new byte[10][];
			byte[][] arg_97F_0 = array20;
			int arg_97F_1 = 0;
			byte[] array21 = new byte[5];
			array21[0] = 1;
			array21[1] = 1;
			arg_97F_0[arg_97F_1] = array21;
			array20[1] = new byte[]
			{
				1,
				0,
				0,
				0,
				1
			};
			array20[2] = new byte[]
			{
				0,
				0,
				0,
				1,
				1
			};
			array20[3] = new byte[]
			{
				0,
				0,
				1,
				0,
				1
			};
			byte[][] arg_9DF_0 = array20;
			int arg_9DF_1 = 4;
			byte[] array22 = new byte[5];
			array22[0] = 1;
			array22[3] = 1;
			arg_9DF_0[arg_9DF_1] = array22;
			byte[][] arg_9F7_0 = array20;
			int arg_9F7_1 = 5;
			byte[] array23 = new byte[5];
			array23[2] = 1;
			array23[3] = 1;
			arg_9F7_0[arg_9F7_1] = array23;
			array20[6] = new byte[]
			{
				0,
				1,
				0,
				0,
				1
			};
			byte[][] arg_A27_0 = array20;
			int arg_A27_1 = 7;
			byte[] array24 = new byte[5];
			array24[0] = 1;
			array24[2] = 1;
			arg_A27_0[arg_A27_1] = array24;
			byte[][] arg_A3F_0 = array20;
			int arg_A3F_1 = 8;
			byte[] array25 = new byte[5];
			array25[1] = 1;
			array25[2] = 1;
			arg_A3F_0[arg_A3F_1] = array25;
			byte[][] arg_A58_0 = array20;
			int arg_A58_1 = 9;
			byte[] array26 = new byte[5];
			array26[1] = 1;
			array26[3] = 1;
			arg_A58_0[arg_A58_1] = array26;
			PdfBarcode.EXT5PARITY = array20;
			PdfBarcode.CODE11CHARS = "0123456789-S";
			byte[][] array27 = new byte[12][];
			array27[0] = new byte[]
			{
				0,
				0,
				0,
				0,
				1
			};
			array27[1] = new byte[]
			{
				1,
				0,
				0,
				0,
				1
			};
			array27[2] = new byte[]
			{
				0,
				1,
				0,
				0,
				1
			};
			byte[][] arg_ACD_0 = array27;
			int arg_ACD_1 = 3;
			byte[] array28 = new byte[5];
			array28[0] = 1;
			array28[1] = 1;
			arg_ACD_0[arg_ACD_1] = array28;
			array27[4] = new byte[]
			{
				0,
				0,
				1,
				0,
				1
			};
			byte[][] arg_AFD_0 = array27;
			int arg_AFD_1 = 5;
			byte[] array29 = new byte[5];
			array29[0] = 1;
			array29[2] = 1;
			arg_AFD_0[arg_AFD_1] = array29;
			byte[][] arg_B15_0 = array27;
			int arg_B15_1 = 6;
			byte[] array30 = new byte[5];
			array30[1] = 1;
			array30[2] = 1;
			arg_B15_0[arg_B15_1] = array30;
			array27[7] = new byte[]
			{
				0,
				0,
				0,
				1,
				1
			};
			byte[][] arg_B45_0 = array27;
			int arg_B45_1 = 8;
			byte[] array31 = new byte[5];
			array31[0] = 1;
			array31[3] = 1;
			arg_B45_0[arg_B45_1] = array31;
			byte[][] arg_B59_0 = array27;
			int arg_B59_1 = 9;
			byte[] array32 = new byte[5];
			array32[0] = 1;
			arg_B59_0[arg_B59_1] = array32;
			byte[][] arg_B6D_0 = array27;
			int arg_B6D_1 = 10;
			byte[] array33 = new byte[5];
			array33[2] = 1;
			arg_B6D_0[arg_B6D_1] = array33;
			byte[][] arg_B86_0 = array27;
			int arg_B86_1 = 11;
			byte[] array34 = new byte[5];
			array34[2] = 1;
			array34[3] = 1;
			arg_B86_0[arg_B86_1] = array34;
			PdfBarcode.CODE11BARS = array27;
			PdfBarcode.CODE93CHARS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%üÿýþû";
			PdfBarcode.CODE93BARS = new byte[][]
			{
				new byte[]
				{
					1,
					3,
					1,
					1,
					1,
					2
				},
				new byte[]
				{
					1,
					1,
					1,
					2,
					1,
					3
				},
				new byte[]
				{
					1,
					1,
					1,
					3,
					1,
					2
				},
				new byte[]
				{
					1,
					1,
					1,
					4,
					1,
					1
				},
				new byte[]
				{
					1,
					2,
					1,
					1,
					1,
					3
				},
				new byte[]
				{
					1,
					2,
					1,
					2,
					1,
					2
				},
				new byte[]
				{
					1,
					2,
					1,
					3,
					1,
					1
				},
				new byte[]
				{
					1,
					1,
					1,
					1,
					1,
					4
				},
				new byte[]
				{
					1,
					3,
					1,
					2,
					1,
					1
				},
				new byte[]
				{
					1,
					4,
					1,
					1,
					1,
					1
				},
				new byte[]
				{
					2,
					1,
					1,
					1,
					1,
					3
				},
				new byte[]
				{
					2,
					1,
					1,
					2,
					1,
					2
				},
				new byte[]
				{
					2,
					1,
					1,
					3,
					1,
					1
				},
				new byte[]
				{
					2,
					2,
					1,
					1,
					1,
					2
				},
				new byte[]
				{
					2,
					2,
					1,
					2,
					1,
					1
				},
				new byte[]
				{
					2,
					3,
					1,
					1,
					1,
					1
				},
				new byte[]
				{
					1,
					1,
					2,
					1,
					1,
					3
				},
				new byte[]
				{
					1,
					1,
					2,
					2,
					1,
					2
				},
				new byte[]
				{
					1,
					1,
					2,
					3,
					1,
					1
				},
				new byte[]
				{
					1,
					2,
					2,
					1,
					1,
					2
				},
				new byte[]
				{
					1,
					3,
					2,
					1,
					1,
					1
				},
				new byte[]
				{
					1,
					1,
					1,
					1,
					2,
					3
				},
				new byte[]
				{
					1,
					1,
					1,
					2,
					2,
					2
				},
				new byte[]
				{
					1,
					1,
					1,
					3,
					2,
					1
				},
				new byte[]
				{
					1,
					2,
					1,
					1,
					2,
					2
				},
				new byte[]
				{
					1,
					3,
					1,
					1,
					2,
					1
				},
				new byte[]
				{
					2,
					1,
					2,
					1,
					1,
					2
				},
				new byte[]
				{
					2,
					1,
					2,
					2,
					1,
					1
				},
				new byte[]
				{
					2,
					1,
					1,
					1,
					2,
					2
				},
				new byte[]
				{
					2,
					1,
					1,
					2,
					2,
					1
				},
				new byte[]
				{
					2,
					2,
					1,
					1,
					2,
					1
				},
				new byte[]
				{
					2,
					2,
					2,
					1,
					1,
					1
				},
				new byte[]
				{
					1,
					1,
					2,
					1,
					2,
					2
				},
				new byte[]
				{
					1,
					1,
					2,
					2,
					2,
					1
				},
				new byte[]
				{
					1,
					2,
					2,
					1,
					2,
					1
				},
				new byte[]
				{
					1,
					2,
					3,
					1,
					1,
					1
				},
				new byte[]
				{
					1,
					2,
					1,
					1,
					3,
					1
				},
				new byte[]
				{
					3,
					1,
					1,
					1,
					1,
					2
				},
				new byte[]
				{
					3,
					1,
					1,
					2,
					1,
					1
				},
				new byte[]
				{
					3,
					2,
					1,
					1,
					1,
					1
				},
				new byte[]
				{
					1,
					1,
					2,
					1,
					3,
					1
				},
				new byte[]
				{
					1,
					1,
					3,
					1,
					2,
					1
				},
				new byte[]
				{
					2,
					1,
					1,
					1,
					3,
					1
				},
				new byte[]
				{
					1,
					2,
					1,
					2,
					2,
					1
				},
				new byte[]
				{
					3,
					1,
					2,
					1,
					1,
					1
				},
				new byte[]
				{
					3,
					1,
					1,
					1,
					2,
					1
				},
				new byte[]
				{
					1,
					2,
					2,
					2,
					1,
					1
				},
				new byte[]
				{
					1,
					1,
					1,
					1,
					4,
					1
				}
			};
			PdfBarcode.CODE128BARS = new byte[][]
			{
				new byte[]
				{
					2,
					1,
					2,
					2,
					2,
					2
				},
				new byte[]
				{
					2,
					2,
					2,
					1,
					2,
					2
				},
				new byte[]
				{
					2,
					2,
					2,
					2,
					2,
					1
				},
				new byte[]
				{
					1,
					2,
					1,
					2,
					2,
					3
				},
				new byte[]
				{
					1,
					2,
					1,
					3,
					2,
					2
				},
				new byte[]
				{
					1,
					3,
					1,
					2,
					2,
					2
				},
				new byte[]
				{
					1,
					2,
					2,
					2,
					1,
					3
				},
				new byte[]
				{
					1,
					2,
					2,
					3,
					1,
					2
				},
				new byte[]
				{
					1,
					3,
					2,
					2,
					1,
					2
				},
				new byte[]
				{
					2,
					2,
					1,
					2,
					1,
					3
				},
				new byte[]
				{
					2,
					2,
					1,
					3,
					1,
					2
				},
				new byte[]
				{
					2,
					3,
					1,
					2,
					1,
					2
				},
				new byte[]
				{
					1,
					1,
					2,
					2,
					3,
					2
				},
				new byte[]
				{
					1,
					2,
					2,
					1,
					3,
					2
				},
				new byte[]
				{
					1,
					2,
					2,
					2,
					3,
					1
				},
				new byte[]
				{
					1,
					1,
					3,
					2,
					2,
					2
				},
				new byte[]
				{
					1,
					2,
					3,
					1,
					2,
					2
				},
				new byte[]
				{
					1,
					2,
					3,
					2,
					2,
					1
				},
				new byte[]
				{
					2,
					2,
					3,
					2,
					1,
					1
				},
				new byte[]
				{
					2,
					2,
					1,
					1,
					3,
					2
				},
				new byte[]
				{
					2,
					2,
					1,
					2,
					3,
					1
				},
				new byte[]
				{
					2,
					1,
					3,
					2,
					1,
					2
				},
				new byte[]
				{
					2,
					2,
					3,
					1,
					1,
					2
				},
				new byte[]
				{
					3,
					1,
					2,
					1,
					3,
					1
				},
				new byte[]
				{
					3,
					1,
					1,
					2,
					2,
					2
				},
				new byte[]
				{
					3,
					2,
					1,
					1,
					2,
					2
				},
				new byte[]
				{
					3,
					2,
					1,
					2,
					2,
					1
				},
				new byte[]
				{
					3,
					1,
					2,
					2,
					1,
					2
				},
				new byte[]
				{
					3,
					2,
					2,
					1,
					1,
					2
				},
				new byte[]
				{
					3,
					2,
					2,
					2,
					1,
					1
				},
				new byte[]
				{
					2,
					1,
					2,
					1,
					2,
					3
				},
				new byte[]
				{
					2,
					1,
					2,
					3,
					2,
					1
				},
				new byte[]
				{
					2,
					3,
					2,
					1,
					2,
					1
				},
				new byte[]
				{
					1,
					1,
					1,
					3,
					2,
					3
				},
				new byte[]
				{
					1,
					3,
					1,
					1,
					2,
					3
				},
				new byte[]
				{
					1,
					3,
					1,
					3,
					2,
					1
				},
				new byte[]
				{
					1,
					1,
					2,
					3,
					1,
					3
				},
				new byte[]
				{
					1,
					3,
					2,
					1,
					1,
					3
				},
				new byte[]
				{
					1,
					3,
					2,
					3,
					1,
					1
				},
				new byte[]
				{
					2,
					1,
					1,
					3,
					1,
					3
				},
				new byte[]
				{
					2,
					3,
					1,
					1,
					1,
					3
				},
				new byte[]
				{
					2,
					3,
					1,
					3,
					1,
					1
				},
				new byte[]
				{
					1,
					1,
					2,
					1,
					3,
					3
				},
				new byte[]
				{
					1,
					1,
					2,
					3,
					3,
					1
				},
				new byte[]
				{
					1,
					3,
					2,
					1,
					3,
					1
				},
				new byte[]
				{
					1,
					1,
					3,
					1,
					2,
					3
				},
				new byte[]
				{
					1,
					1,
					3,
					3,
					2,
					1
				},
				new byte[]
				{
					1,
					3,
					3,
					1,
					2,
					1
				},
				new byte[]
				{
					3,
					1,
					3,
					1,
					2,
					1
				},
				new byte[]
				{
					2,
					1,
					1,
					3,
					3,
					1
				},
				new byte[]
				{
					2,
					3,
					1,
					1,
					3,
					1
				},
				new byte[]
				{
					2,
					1,
					3,
					1,
					1,
					3
				},
				new byte[]
				{
					2,
					1,
					3,
					3,
					1,
					1
				},
				new byte[]
				{
					2,
					1,
					3,
					1,
					3,
					1
				},
				new byte[]
				{
					3,
					1,
					1,
					1,
					2,
					3
				},
				new byte[]
				{
					3,
					1,
					1,
					3,
					2,
					1
				},
				new byte[]
				{
					3,
					3,
					1,
					1,
					2,
					1
				},
				new byte[]
				{
					3,
					1,
					2,
					1,
					1,
					3
				},
				new byte[]
				{
					3,
					1,
					2,
					3,
					1,
					1
				},
				new byte[]
				{
					3,
					3,
					2,
					1,
					1,
					1
				},
				new byte[]
				{
					3,
					1,
					4,
					1,
					1,
					1
				},
				new byte[]
				{
					2,
					2,
					1,
					4,
					1,
					1
				},
				new byte[]
				{
					4,
					3,
					1,
					1,
					1,
					1
				},
				new byte[]
				{
					1,
					1,
					1,
					2,
					2,
					4
				},
				new byte[]
				{
					1,
					1,
					1,
					4,
					2,
					2
				},
				new byte[]
				{
					1,
					2,
					1,
					1,
					2,
					4
				},
				new byte[]
				{
					1,
					2,
					1,
					4,
					2,
					1
				},
				new byte[]
				{
					1,
					4,
					1,
					1,
					2,
					2
				},
				new byte[]
				{
					1,
					4,
					1,
					2,
					2,
					1
				},
				new byte[]
				{
					1,
					1,
					2,
					2,
					1,
					4
				},
				new byte[]
				{
					1,
					1,
					2,
					4,
					1,
					2
				},
				new byte[]
				{
					1,
					2,
					2,
					1,
					1,
					4
				},
				new byte[]
				{
					1,
					2,
					2,
					4,
					1,
					1
				},
				new byte[]
				{
					1,
					4,
					2,
					1,
					1,
					2
				},
				new byte[]
				{
					1,
					4,
					2,
					2,
					1,
					1
				},
				new byte[]
				{
					2,
					4,
					1,
					2,
					1,
					1
				},
				new byte[]
				{
					2,
					2,
					1,
					1,
					1,
					4
				},
				new byte[]
				{
					4,
					1,
					3,
					1,
					1,
					1
				},
				new byte[]
				{
					2,
					4,
					1,
					1,
					1,
					2
				},
				new byte[]
				{
					1,
					3,
					4,
					1,
					1,
					1
				},
				new byte[]
				{
					1,
					1,
					1,
					2,
					4,
					2
				},
				new byte[]
				{
					1,
					2,
					1,
					1,
					4,
					2
				},
				new byte[]
				{
					1,
					2,
					1,
					2,
					4,
					1
				},
				new byte[]
				{
					1,
					1,
					4,
					2,
					1,
					2
				},
				new byte[]
				{
					1,
					2,
					4,
					1,
					1,
					2
				},
				new byte[]
				{
					1,
					2,
					4,
					2,
					1,
					1
				},
				new byte[]
				{
					4,
					1,
					1,
					2,
					1,
					2
				},
				new byte[]
				{
					4,
					2,
					1,
					1,
					1,
					2
				},
				new byte[]
				{
					4,
					2,
					1,
					2,
					1,
					1
				},
				new byte[]
				{
					2,
					1,
					2,
					1,
					4,
					1
				},
				new byte[]
				{
					2,
					1,
					4,
					1,
					2,
					1
				},
				new byte[]
				{
					4,
					1,
					2,
					1,
					2,
					1
				},
				new byte[]
				{
					1,
					1,
					1,
					1,
					4,
					3
				},
				new byte[]
				{
					1,
					1,
					1,
					3,
					4,
					1
				},
				new byte[]
				{
					1,
					3,
					1,
					1,
					4,
					1
				},
				new byte[]
				{
					1,
					1,
					4,
					1,
					1,
					3
				},
				new byte[]
				{
					1,
					1,
					4,
					3,
					1,
					1
				},
				new byte[]
				{
					4,
					1,
					1,
					1,
					1,
					3
				},
				new byte[]
				{
					4,
					1,
					1,
					3,
					1,
					1
				},
				new byte[]
				{
					1,
					1,
					3,
					1,
					4,
					1
				},
				new byte[]
				{
					1,
					1,
					4,
					1,
					3,
					1
				},
				new byte[]
				{
					3,
					1,
					1,
					1,
					4,
					1
				},
				new byte[]
				{
					4,
					1,
					1,
					1,
					3,
					1
				},
				new byte[]
				{
					2,
					1,
					1,
					4,
					1,
					2
				},
				new byte[]
				{
					2,
					1,
					1,
					2,
					1,
					4
				},
				new byte[]
				{
					2,
					1,
					1,
					2,
					3,
					2
				},
				new byte[]
				{
					2,
					3,
					3,
					1,
					1,
					1
				}
			};
			byte[][] array35 = new byte[10][];
			byte[][] arg_1918_0 = array35;
			int arg_1918_1 = 0;
			byte[] array36 = new byte[5];
			array36[0] = 1;
			array36[1] = 1;
			arg_1918_0[arg_1918_1] = array36;
			array35[1] = new byte[]
			{
				0,
				0,
				0,
				1,
				1
			};
			array35[2] = new byte[]
			{
				0,
				0,
				1,
				0,
				1
			};
			byte[][] arg_1960_0 = array35;
			int arg_1960_1 = 3;
			byte[] array37 = new byte[5];
			array37[2] = 1;
			array37[3] = 1;
			arg_1960_0[arg_1960_1] = array37;
			array35[4] = new byte[]
			{
				0,
				1,
				0,
				0,
				1
			};
			byte[][] arg_198C_0 = array35;
			int arg_198C_1 = 5;
			array2 = new byte[5];
			array2[1] = 1;
			array2[3] = 1;
			arg_198C_0[arg_198C_1] = array2;
			byte[][] arg_19A0_0 = array35;
			int arg_19A0_1 = 6;
			array2 = new byte[5];
			array2[1] = 1;
			array2[2] = 1;
			arg_19A0_0[arg_19A0_1] = array2;
			array35[7] = new byte[]
			{
				1,
				0,
				0,
				0,
				1
			};
			byte[][] arg_19C8_0 = array35;
			int arg_19C8_1 = 8;
			array2 = new byte[5];
			array2[0] = 1;
			array2[3] = 1;
			arg_19C8_0[arg_19C8_1] = array2;
			byte[][] arg_19DD_0 = array35;
			int arg_19DD_1 = 9;
			array2 = new byte[5];
			array2[0] = 1;
			array2[2] = 1;
			arg_19DD_0[arg_19DD_1] = array2;
			PdfBarcode.POSTALBARS = array35;
			PdfBarcode.UKPOSTALBARS = new byte[][]
			{
				new byte[]
				{
					0,
					0,
					3,
					3,
					1,
					1
				},
				new byte[]
				{
					0,
					2,
					1,
					3,
					1,
					2
				},
				new byte[]
				{
					0,
					2,
					3,
					1,
					1,
					3
				},
				new byte[]
				{
					2,
					0,
					1,
					3,
					1,
					4
				},
				new byte[]
				{
					2,
					0,
					3,
					1,
					1,
					5
				},
				new byte[]
				{
					2,
					2,
					1,
					1,
					1,
					6
				},
				new byte[]
				{
					0,
					1,
					2,
					3,
					2,
					1
				},
				new byte[]
				{
					0,
					3,
					0,
					3,
					2,
					2
				},
				new byte[]
				{
					0,
					3,
					2,
					1,
					2,
					3
				},
				new byte[]
				{
					2,
					1,
					0,
					3,
					2,
					4
				},
				new byte[]
				{
					2,
					1,
					2,
					1,
					2,
					5
				},
				new byte[]
				{
					2,
					3,
					0,
					1,
					2,
					6
				},
				new byte[]
				{
					0,
					1,
					3,
					2,
					3,
					1
				},
				new byte[]
				{
					0,
					3,
					1,
					2,
					3,
					2
				},
				new byte[]
				{
					0,
					3,
					3,
					0,
					3,
					3
				},
				new byte[]
				{
					2,
					1,
					1,
					2,
					3,
					4
				},
				new byte[]
				{
					2,
					1,
					3,
					0,
					3,
					5
				},
				new byte[]
				{
					2,
					3,
					1,
					0,
					3,
					6
				},
				new byte[]
				{
					1,
					0,
					2,
					3,
					4,
					1
				},
				new byte[]
				{
					1,
					2,
					0,
					3,
					4,
					2
				},
				new byte[]
				{
					1,
					2,
					2,
					1,
					4,
					3
				},
				new byte[]
				{
					3,
					0,
					0,
					3,
					4,
					4
				},
				new byte[]
				{
					3,
					0,
					2,
					1,
					4,
					5
				},
				new byte[]
				{
					3,
					2,
					0,
					1,
					4,
					6
				},
				new byte[]
				{
					1,
					0,
					3,
					2,
					5,
					1
				},
				new byte[]
				{
					1,
					2,
					1,
					2,
					5,
					2
				},
				new byte[]
				{
					1,
					2,
					3,
					0,
					5,
					3
				},
				new byte[]
				{
					3,
					0,
					1,
					2,
					5,
					4
				},
				new byte[]
				{
					3,
					0,
					3,
					0,
					5,
					5
				},
				new byte[]
				{
					3,
					2,
					1,
					0,
					5,
					6
				},
				new byte[]
				{
					1,
					1,
					2,
					2,
					6,
					1
				},
				new byte[]
				{
					1,
					3,
					0,
					2,
					6,
					2
				},
				new byte[]
				{
					1,
					3,
					2,
					0,
					6,
					3
				},
				new byte[]
				{
					3,
					1,
					0,
					2,
					6,
					4
				},
				new byte[]
				{
					3,
					1,
					2,
					0,
					6,
					5
				},
				new byte[]
				{
					3,
					3,
					0,
					0,
					6,
					6
				}
			};
		}
	}
}
