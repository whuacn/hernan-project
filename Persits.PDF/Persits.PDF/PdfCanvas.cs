using System;
using System.Collections.Generic;
using System.Text;
namespace Persits.PDF
{
	public class PdfCanvas
	{
		internal string m_bstrHtmlTag;
		private List<AuxSnippetRange> m_arrRanges = new List<AuxSnippetRange>();
		private List<AuxSnippetRange> m_arrStack = new List<AuxSnippetRange>();
		private AuxSnippetRange m_pLastDivRange;
		private float m_fX;
		private float m_fY;
		private float m_fBottom;
		private bool m_bNewParagraph;
		private float m_fSpacing;
		private bool m_bBottomReached;
		private int m_nLastFromIndex;
		private float m_fSubIndent = 0.35f;
		private float m_fMaxPrevSubSize;
		internal PdfManager m_pManager;
		internal PdfDocument m_pDoc;
		internal PdfIndirectObj m_pContentsObj;
		internal PdfIndirectObj m_pResourceObj;
		internal PdfIndirectObj m_pFontDictObj;
		internal PdfIndirectObj m_pXObjectDictObj;
		internal PdfIndirectObj m_pCSDictObj;
		internal bool m_bPathEmpty;
		internal PdfPage m_pPage;
		internal float m_fLineWidth = 1f;
		internal int m_nLineCap;
		internal int m_nLineJoin;
		internal float m_fMiter = -1f;
		internal string m_bstrText;
		internal PdfFont m_pFont;
		internal int m_nSpaceWidth;
		internal float m_fSize;
		internal bool m_bReverseHebrewArabic;
		internal int m_nAlignment;
		internal float m_fTextWidth;
		internal bool m_bFirstLine = true;
		internal float m_fNextLineOffset;
		internal int m_nTabCount;
		internal float[] m_arrTabWidths = new float[100];
		internal int[] m_arrTabIndexes = new int[100];
		internal float m_fVertialExtent;
		internal bool m_bCMYK;
		public string HtmlTag
		{
			get
			{
				return this.m_bstrHtmlTag;
			}
		}
		public string Value
		{
			get
			{
				if (this.m_pContentsObj.m_objStream.Length > 0)
				{
					return Encoding.UTF8.GetString(this.m_pContentsObj.m_objStream.m_objMemStream.ToArray());
				}
				return "";
			}
		}
		public float LineWidth
		{
			get
			{
				return this.m_fLineWidth;
			}
			set
			{
				if (value <= 0f)
				{
					AuxException.Throw("LineWidth must be a positive number.", PdfErrors._ERROR_INVALIDARG);
				}
				this.m_fLineWidth = value;
				this.AppendFormatted("w", new float[]
				{
					value
				});
			}
		}
		public int LineCap
		{
			get
			{
				return this.m_nLineCap;
			}
			set
			{
				if (value < 0 || value > 2)
				{
					AuxException.Throw("LineCap must be set to 0 (Butt), 1 (Round), or 2 (Projecting square).", PdfErrors._ERROR_INVALIDARG);
				}
				this.m_nLineCap = value;
				this.AppendFormatted("J", new float[]
				{
					(float)value
				});
			}
		}
		public int LineJoin
		{
			get
			{
				return this.m_nLineJoin;
			}
			set
			{
				if (value < 0 || value > 2)
				{
					AuxException.Throw("LineJoin must be set to 0 (Miter), 1 (Round), or 2 (Bevel).", PdfErrors._ERROR_INVALIDARG);
				}
				this.m_nLineJoin = value;
				this.AppendFormatted("j", new float[]
				{
					(float)value
				});
			}
		}
		public float MiterLimit
		{
			get
			{
				return this.m_fMiter;
			}
			set
			{
				if (value < 0f || value > 2f)
				{
					AuxException.Throw("Miter limit must be greater or equal to 1.", PdfErrors._ERROR_INVALIDARG);
				}
				this.m_fMiter = value;
				this.AppendFormatted("M", new float[]
				{
					value
				});
			}
		}
		internal int DrawHtml(string Text, PdfParam pParam, float fX, float fY, PdfFont pFont, float fSize)
		{
			this.ClearSnippets();
			this.m_bstrHtmlTag = "";
			this.m_fMaxPrevSubSize = 0f;
			int length = Text.Length;
			float fR = 0f;
			float fG = 0f;
			float fB = 0f;
			if (pParam.IsSet("Color"))
			{
				pParam.Color("Color", out fR, out fG, out fB);
			}
			char[] array = new char[length + 10];
			uint num = 0u;
			int i = 0;
			this.m_bNewParagraph = true;
			AuxSnippetRange auxSnippetRange = new AuxSnippetRange(pFont, pFont.m_bstrFamily, pFont.m_bstrFace, fSize, fR, fG, fB);
			AuxSnippetRange item = new AuxSnippetRange(auxSnippetRange);
			this.m_arrStack.Add(item);
			while (i < length)
			{
				if (Text[i] == '<')
				{
					string bstrTag;
					if (this.ParseTag(Text, length, ref i, out bstrTag))
					{
						AuxSnippetRange auxSnippetRange2 = new AuxSnippetRange(auxSnippetRange);
						auxSnippetRange2.m_nOriginalFrom = i;
						auxSnippetRange2.m_nFrom = (int)num;
						if (!auxSnippetRange.IsEmpty())
						{
							this.m_arrRanges.Add(auxSnippetRange);
						}
						this.AnalyzeTag(bstrTag, array, ref num, ref auxSnippetRange2);
						auxSnippetRange = auxSnippetRange2;
					}
					else
					{
						array[(int)num++] = '<';
						auxSnippetRange.m_nLength++;
						auxSnippetRange.m_nOriginalLength++;
						this.m_bNewParagraph = true;
					}
				}
				else
				{
					if (Text[i] == '&')
					{
						int num2 = i;
						char c;
						if (this.ParseSymbol(Text, length, ref i, out c))
						{
							array[(int)((UIntPtr)(num++))] = c;
						}
						else
						{
							array[(int)((UIntPtr)(num++))] = '&';
						}
						auxSnippetRange.m_nLength++;
						auxSnippetRange.m_nOriginalLength += i - num2;
						for (int j = num2; j < i - 1; j++)
						{
							auxSnippetRange.m_arrSkippedChars.Add(j - auxSnippetRange.m_nOriginalFrom);
						}
						this.m_bNewParagraph = false;
					}
					else
					{
						if (PdfCanvas.IsSpace(Text[i]))
						{
							while (i < Text.Length)
							{
								if (!PdfCanvas.IsSpace(Text[i]))
								{
									break;
								}
								if (!this.m_bNewParagraph && array[(int)((UIntPtr)(num - 1u))] != ' ')
								{
									array[(int)((UIntPtr)(num++))] = ' ';
									auxSnippetRange.m_nLength++;
								}
								else
								{
									auxSnippetRange.m_arrSkippedChars.Add(i - auxSnippetRange.m_nOriginalFrom - (this.m_bNewParagraph ? 0 : 1));
								}
								i++;
								auxSnippetRange.m_nOriginalLength++;
							}
						}
						else
						{
							array[(int)((UIntPtr)(num++))] = Text[i++];
							auxSnippetRange.m_nLength++;
							auxSnippetRange.m_nOriginalLength++;
							this.m_bNewParagraph = false;
						}
					}
				}
			}
			string text = new string(array, 0, (int)num);
			if (!auxSnippetRange.IsEmpty())
			{
				this.m_arrRanges.Add(auxSnippetRange);
			}
			PdfFonts fonts = this.m_pDoc.Fonts;
			foreach (AuxSnippetRange current in this.m_arrRanges)
			{
				string text2 = auxSnippetRange.m_bstrBaseFont;
				if (auxSnippetRange.m_nBold > 0)
				{
					text2 += " Bold";
				}
				if (auxSnippetRange.m_nItalic > 0)
				{
					text2 += " Italic";
				}
				current.m_ptrFont = fonts.LoadStandardFont(current.m_bstrBaseFont, current.m_nBold != 0, current.m_nItalic != 0);
				if (current.m_ptrFont == null)
				{
					try
					{
						current.m_ptrFont = fonts.LoadTrueTypeFont(current.m_bstrBaseFont, current.m_nBold > 0, current.m_nItalic > 0);
					}
					catch (Exception)
					{
					}
					if (current.m_ptrFont == null)
					{
						try
						{
							current.m_ptrFont = fonts[text2];
						}
						catch (Exception)
						{
						}
						if (current.m_ptrFont == null)
						{
							try
							{
								text2 = current.m_bstrFace2;
								current.m_ptrFont = fonts[text2];
							}
							catch (Exception)
							{
							}
							if (current.m_ptrFont == null)
							{
								string text3 = "HTML rendering error: cannot obtain font \"";
								text3 += text2;
								text3 += "\".";
								AuxException.Throw(text3, PdfErrors._ERROR_HTMLFONT);
							}
						}
					}
				}
			}
			this.RenderText(text.Trim(), pParam);
			int result = 0;
			AuxSnippetRange auxSnippetRange3 = null;
			if (this.m_bBottomReached)
			{
				int num3 = this.m_nLastFromIndex - 1;
				if (num3 < 0)
				{
					return result;
				}
				while (num3 >= 0 && PdfCanvas.IsSpace(text[num3]))
				{
					num3--;
				}
				foreach (AuxSnippetRange current2 in this.m_arrRanges)
				{
					auxSnippetRange3 = current2;
					if (num3 >= current2.m_nFrom && num3 < current2.m_nFrom + current2.m_nLength)
					{
						if (text[current2.m_nFrom] == '\r')
						{
							num3 -= 2;
						}
						if (current2.m_nLength > 2 && text[current2.m_nFrom + 2] == '\r')
						{
							num3 -= 2;
						}
						int num4 = num3 - current2.m_nFrom;
						if (num4 < 0)
						{
							break;
						}
						if (current2.m_arrSkippedChars.Count == 0)
						{
							result = current2.m_nOriginalFrom + num4 + 1;
							break;
						}
						int num5 = 0;
						int num6 = 0;
						int num7 = 0;
						while (true)
						{
							if (num6 < current2.m_arrSkippedChars.Count && num5 == current2.m_arrSkippedChars[num6])
							{
								while (num6 < current2.m_arrSkippedChars.Count && num5 == current2.m_arrSkippedChars[num6])
								{
									num5 = current2.m_arrSkippedChars[num6] + 1;
									num6++;
								}
							}
							num7++;
							num5++;
							if (num7 >= current2.m_nLength)
							{
								break;
							}
							if (num7 >= num4)
							{
								goto Block_57;
							}
						}
						if (current2.m_nLength == 1)
						{
							result = current2.m_nOriginalFrom + current2.m_nOriginalLength;
							break;
						}
						break;
						Block_57:
						while (num6 < current2.m_arrSkippedChars.Count && num5 == current2.m_arrSkippedChars[num6])
						{
							num5 = current2.m_arrSkippedChars[num6] + 1;
							num6++;
						}
						if (num5 >= current2.m_nOriginalLength)
						{
							num5--;
						}
						result = current2.m_nOriginalFrom + num5 + 1;
						break;
					}
				}
				if (auxSnippetRange3 != null && auxSnippetRange3.m_ptrFont != null)
				{
					string bstrHtmlTag = string.Format("<FONT STYLE=\"font-family: '{0}'; font-size: {1:f}pt; font-weight: {2}; font-style: {3}; text-decoration: {4}; color: #H{5:x02}{6:x02}{7:x02}\">{8}{9}{10}", new object[]
					{
						auxSnippetRange3.m_ptrFont.m_bstrFamily,
						auxSnippetRange3.m_fSize,
						(auxSnippetRange3.m_nBold > 0) ? "bold" : "normal",
						(auxSnippetRange3.m_nItalic > 0) ? "italic" : "normal",
						(auxSnippetRange3.m_nUnderlined > 0) ? "underline" : "none",
						(int)(auxSnippetRange3.m_fR * 255f),
						(int)(auxSnippetRange3.m_fG * 255f),
						(int)(auxSnippetRange3.m_fB * 255f),
						(auxSnippetRange3.m_nCenter > 0) ? "<CENTER>" : "",
						(auxSnippetRange3.m_nRight > 0) ? "<DIV ALIGN=\"RIGHT\">" : "",
						(auxSnippetRange3.m_nJustify > 0) ? "<DIV ALIGN=\"JUSTIFY\">" : ""
					});
					this.m_bstrHtmlTag = bstrHtmlTag;
				}
			}
			else
			{
				result = length;
			}
			return result;
		}
		private void RenderText(string bstrText, PdfParam pParam)
		{
			float fX = pParam.Number("X");
			this.m_fY = pParam.Number("Y");
			this.m_fX = fX;
			this.EndText();
			this.m_bFirstLine = true;
			this.m_fSize = 0f;
			this.m_bBottomReached = false;
			this.m_nLastFromIndex = 0;
			int num = 0;
			int fromIndex = 0;
			int num2 = -1;
			float num3 = 0f;
			float curWidth = 0f;
			int length = bstrText.Length;
			float num4 = 16777215f;
			if (pParam.IsSet("Width"))
			{
				num4 = pParam.Number("Width");
			}
			this.m_bstrText = bstrText;
			this.m_fSpacing = 1f;
			if (pParam.IsSet("Spacing"))
			{
				this.m_fSpacing = pParam.Number("Spacing");
			}
			this.m_fTextWidth = num4;
			this.m_bReverseHebrewArabic = pParam.Bool("ReverseHebrewArabic");
			this.m_fBottom = -1f;
			if (pParam.IsSet("Height") && pParam.Number("Height") > 0f)
			{
				this.m_fBottom = this.m_fY - pParam.Number("Height");
			}
			while (num < length && !this.m_bBottomReached)
			{
				float num5;
				float num6;
				this.GetCharInfo(num, out num5, out num6);
				float num7 = num5 * num6;
				if (num7 > num4)
				{
					break;
				}
				if (bstrText[num] == ' ')
				{
					num2 = num;
					curWidth = num3;
				}
				if (bstrText[num] == '\r' || bstrText[num] == '\n')
				{
					this.DrawTextLine(fromIndex, num - 1, num3, false);
					num++;
					if (bstrText[num - 1] == '\r' && bstrText[num] == '\n')
					{
						num++;
					}
					fromIndex = num;
					num2 = -1;
					num3 = 0f;
				}
				else
				{
					num3 += num7;
					if (num3 > num4)
					{
						if (num2 < 0)
						{
							this.DrawTextLine(fromIndex, num - 1, num3 - num7, false);
							fromIndex = num++;
							num3 = num7;
						}
						else
						{
							this.DrawTextLine(fromIndex, num2 - 1, curWidth, true);
							bool flag = num == num2;
							while (num2 < length && bstrText[++num2] == ' ')
							{
							}
							if (flag && bstrText[num2] == '\r')
							{
								num2 += 2;
							}
							num = (fromIndex = num2);
							num3 = 0f;
							num2 = -1;
						}
					}
					else
					{
						num++;
						if (num == length)
						{
							this.DrawTextLine(fromIndex, num - 1, num3, false);
						}
					}
				}
			}
			if (this.m_fVertialExtent > 0f)
			{
				this.m_fVertialExtent += 0.1f;
			}
		}
		private void DrawTextLine(int FromIndex, int ToIndex, float CurWidth, bool bSoftBreak)
		{
			List<AuxRect> list = new List<AuxRect>();
			List<AuxRect> list2 = new List<AuxRect>();
			while (ToIndex >= 0 && this.m_bstrText[ToIndex] == ' ')
			{
				float num;
				float num2;
				this.GetCharInfo(ToIndex, out num, out num2);
				ToIndex--;
				CurWidth -= num * num2;
			}
			float num3 = 0f;
			float num4;
			PdfFont pdfFont;
			int num5;
			uint num6;
			string text;
			this.GetCharInfo(FromIndex, out num4, out num4, out pdfFont, out num5, out num6, out num4, out num4, out num4, out text);
			if ((num6 & 2u) > 0u)
			{
				num3 = (this.m_fTextWidth - CurWidth) / 2f;
			}
			if ((num6 & 4u) > 0u)
			{
				num3 = this.m_fTextWidth - CurWidth;
			}
			float num7 = 0f;
			if ((num6 & 32u) > 0u && bSoftBreak)
			{
				int num8 = 0;
				for (int i = FromIndex; i <= ToIndex; i++)
				{
					if (this.m_bstrText[i] == ' ' || this.m_bstrText[i] == '\u00a0')
					{
						num8++;
					}
				}
				if (num8 > 0)
				{
					num7 = -1000f * (this.m_fTextWidth - CurWidth) / (float)num8;
				}
			}
			float num9 = 0f;
			float num10 = 0f;
			float num11 = this.m_fX + num3;
			float num12 = (FromIndex >= ToIndex) ? this.m_fSize : 0f;
			int j = FromIndex;
			float num13 = 0f;
			while (j <= ToIndex)
			{
				float num14;
				float num15;
				int num16;
				uint num17;
				this.GetCharInfo(j, out num14, out num15, out pdfFont, out num16, out num17, out num14, out num14, out num14, out text);
				if (num12 < num15)
				{
					num12 = num15;
				}
				if ((num17 & 8u) > 0u && num13 < num15 * this.m_fSubIndent)
				{
					num13 = num15 * this.m_fSubIndent;
				}
				if ((num17 & 16u) > 0u && this.m_fMaxPrevSubSize < num15 * this.m_fSubIndent)
				{
					this.m_fMaxPrevSubSize = num15 * this.m_fSubIndent;
				}
				j += num16;
			}
			num12 += num13;
			this.m_fY -= (this.m_bFirstLine ? num12 : (num12 * this.m_fSpacing));
			if (this.m_fBottom != -1f && (double)this.m_fY - 0.2 * (double)num12 < (double)this.m_fBottom)
			{
				this.m_bBottomReached = true;
				this.m_nLastFromIndex = FromIndex;
				return;
			}
			this.m_fVertialExtent += (this.m_bFirstLine ? num12 : (num12 * this.m_fSpacing)) + 0.2f * num12 * this.m_fSpacing;
			this.BeginText();
			this.TextTo(this.m_fX + num3, this.m_fY);
			float r = 0f;
			float g = 0f;
			float b = 0f;
			j = FromIndex;
			new PdfStream();
			while (j <= ToIndex)
			{
				string text3;
				do
				{
					float num14;
					float num15;
					int num16;
					uint num17;
					PdfFont pdfFont2;
					string text2;
					this.GetCharInfo(j, out num14, out num15, out pdfFont2, out num16, out num17, out r, out g, out b, out text2);
					pdfFont2.CreateIndirectObject(this.m_pResourceObj, this.m_pFontDictObj);
					if (num16 > ToIndex - j + 1)
					{
						num16 = ToIndex - j + 1;
					}
					this.SelectFont(this.m_pDoc.m_szFontID, num15);
					text3 = this.m_bstrText.Substring(j, num16);
					if (text3.Length > 0)
					{
						PdfStream pdfStream = new PdfStream();
						float num18;
						pdfFont2.UnicodeToGlyph(text3.ToString(), ref pdfStream, out num18);
						if (!this.m_bCMYK)
						{
							this.SetFillColor(r, g, b);
						}
						if ((num17 & 8u) > 0u)
						{
							this.TextRise(num15 * this.m_fSubIndent);
						}
						if ((num17 & 16u) > 0u)
						{
							this.TextRise(-num15 * this.m_fSubIndent);
						}
						if (num7 != 0f)
						{
							num10 = num15 * this.HandleJustifyAlignment(pdfStream, num7 / num15, pdfFont2);
							num9 += num10;
						}
						else
						{
							this.Append(pdfStream.ToBytes());
							this.Append(" Tj");
						}
						if ((num17 & 24u) > 0u)
						{
							this.TextRise(0f);
						}
						if ((num17 & 1u) > 0u)
						{
							AuxRect auxRect = new AuxRect(num9 - num10 + num11, this.m_fY, num18 * num15 + num10, num15, r, g, b);
							if ((num17 & 24u) > 0u)
							{
								auxRect.m_y = num15 * this.m_fSubIndent * (float)(((num17 & 8u) > 0u) ? 1 : -1);
							}
							else
							{
								auxRect.m_y = 0f;
							}
							list.Add(auxRect);
						}
						if (this.m_pPage != null && text2 != null && text2.Length > 0)
						{
							list2.Add(new AuxRect(num9 - num10 + num11, this.m_fY, num18 * num15 + num10, num15, r, g, b)
							{
								m_anchor = text2
							});
						}
						num11 += num18 * num15;
						text3 = "";
					}
					j += num16;
				}
				while (j <= ToIndex);
				if (this.m_bReverseHebrewArabic)
				{
					this.ReverseHebrewArabic(ref text3);
				}
			}
			this.m_bFirstLine = false;
			this.EndText();
			foreach (AuxRect current in list)
			{
				if (!this.m_bCMYK)
				{
					this.SetFillColor(current.m_r, current.m_g, current.m_b);
				}
				this.FillRect(current.m_x, current.m_y + this.m_fY - num12 / 9f, current.m_width, -num12 / 20f);
			}
			foreach (AuxRect current2 in list2)
			{
				PdfAction action = this.m_pDoc.CreateAction("type=URI", current2.m_anchor);
				PdfAnnots annots = this.m_pPage.Annots;
				string param = string.Format("Type=Link; Border=0; x={0:f}, y={1:f}, width={2:f}, height={3:f}", new object[]
				{
					current2.m_x,
					(double)this.m_fY - 0.2 * (double)current2.m_height,
					current2.m_width,
					0.8 * (double)current2.m_height
				});
				PdfAnnot pdfAnnot = annots.Add(null, param, null, null);
				pdfAnnot.SetAction(action);
			}
			this.m_fY -= num12 * this.m_fSpacing * 0.2f;
			this.m_fVertialExtent += this.m_fMaxPrevSubSize;
			this.m_fY -= this.m_fMaxPrevSubSize;
			this.m_fMaxPrevSubSize = 0f;
			this.m_fSize = num12;
		}
		private float HandleJustifyAlignment(PdfStream objString, float fAdjustment, PdfFont pFont)
		{
			if (objString.Length <= 2)
			{
				return 0f;
			}
			float num = 0f;
			PdfOutput pdfOutput = new PdfOutput(50);
			PdfNumber pdfNumber = new PdfNumber(null, (double)fAdjustment);
			pdfNumber.WriteOut(pdfOutput);
			this.AppendNoLF("[");
			if (objString[0] == 40)
			{
				this.AppendNoLF("(");
				for (int i = 1; i < objString.Length; i++)
				{
					this.Append(new byte[]
					{
						objString[i]
					});
					if (objString[i] == 32)
					{
						this.AppendNoLF(") ");
						this.Append(pdfOutput.ToBytes());
						this.AppendNoLF(" (");
						num += fAdjustment;
					}
				}
			}
			else
			{
				PdfStream pdfStream = new PdfStream();
				float num2;
				pFont.UnicodeToGlyph(" ", ref pdfStream, out num2);
				int num3 = pdfStream.Length - 2;
				this.AppendNoLF("<");
				for (int j = 1; j < objString.Length - 1; j += num3)
				{
					if (objString[j] == 10)
					{
						j++;
					}
					if (j >= objString.Length - 1)
					{
						break;
					}
					byte[] array = new byte[num3];
					Array.Copy(objString.ToBytes(), j, array, 0, num3);
					this.Append(array);
					if (objString.Compare(j, pdfStream, 1, num3))
					{
						this.AppendNoLF("> ");
						this.Append(pdfOutput.ToBytes());
						this.AppendNoLF(" <");
						num += fAdjustment;
					}
				}
				this.AppendNoLF(">");
			}
			this.Append("] TJ");
			return -num / 1000f;
		}
		private void GetCharInfo(int nCurIndex, out float pCWidth, out float pSize)
		{
			pSize = 0f;
			pCWidth = 0f;
			foreach (AuxSnippetRange current in this.m_arrRanges)
			{
				if (nCurIndex >= current.m_nFrom && nCurIndex < current.m_nFrom + current.m_nLength)
				{
					PdfFont ptrFont = current.m_ptrFont;
					ushort num;
					ushort num2;
					ptrFont.GetCharInfo(this.m_bstrText[nCurIndex], out num, out num2);
					pSize = current.m_fSize;
					pCWidth = (float)num2 / (float)ptrFont.m_nUnitsPerEm;
					break;
				}
			}
		}
		private void GetCharInfo(int nCurIndex, out float pCWidth, out float pSize, out PdfFont ppFont, out int pRangeLength, out uint pFlags, out float pR, out float pG, out float pB, out string pAnchor)
		{
			pCWidth = 0f;
			pSize = 0f;
			pRangeLength = 0;
			ppFont = null;
			pFlags = 0u;
			pR = (pG = (pB = 0f));
			pAnchor = "";
			foreach (AuxSnippetRange current in this.m_arrRanges)
			{
				if (nCurIndex >= current.m_nFrom && nCurIndex < current.m_nFrom + current.m_nLength)
				{
					PdfFont ptrFont = current.m_ptrFont;
					ushort num;
					ushort num2;
					ptrFont.GetCharInfo(this.m_bstrText[nCurIndex], out num, out num2);
					pSize = current.m_fSize;
					pCWidth = (float)num2 / (float)ptrFont.m_nUnitsPerEm;
					pRangeLength = current.m_nLength - (nCurIndex - current.m_nFrom);
					ppFont = ptrFont;
					pFlags = 0u;
					if (current.m_nUnderlined > 0)
					{
						pFlags |= 1u;
					}
					if (current.m_nCenter > 0)
					{
						pFlags |= 2u;
					}
					if (current.m_nRight > 0)
					{
						pFlags |= 4u;
					}
					if (current.m_nSub > 0)
					{
						pFlags |= 8u;
					}
					if (current.m_nSub < 0)
					{
						pFlags |= 16u;
					}
					if (current.m_nJustify > 0)
					{
						pFlags |= 32u;
					}
					pR = current.m_fR;
					pG = current.m_fG;
					pB = current.m_fB;
					pAnchor = current.m_bstrAnchor;
					break;
				}
			}
		}
		private void AnalyzeTag(string bstrTag, char[] wszOutText, ref uint pOutPtr, ref AuxSnippetRange pRange)
		{
			AuxHtml auxHtml = new AuxHtml();
			if (string.Compare(bstrTag, "p", true) == 0)
			{
				pRange.m_nLength += this.AddCRLF(wszOutText, ref pOutPtr, 2);
				this.m_bNewParagraph = true;
				return;
			}
			if (string.Compare(bstrTag, "br", true) == 0)
			{
				pRange.m_nLength += this.AddCRLF(wszOutText, ref pOutPtr, 1);
				this.m_bNewParagraph = true;
				return;
			}
			if (string.Compare(bstrTag, "b", true) == 0)
			{
				pRange.m_nBold++;
				return;
			}
			if (string.Compare(bstrTag, "/b", true) == 0)
			{
				pRange.m_nBold--;
				if (pRange.m_nBold <= 0)
				{
					pRange.m_nBold = 0;
					return;
				}
			}
			else
			{
				if (string.Compare(bstrTag, "i", true) == 0)
				{
					pRange.m_nItalic++;
					return;
				}
				if (string.Compare(bstrTag, "/i", true) == 0)
				{
					pRange.m_nItalic--;
					if (pRange.m_nItalic <= 0)
					{
						pRange.m_nItalic = 0;
						return;
					}
				}
				else
				{
					if (string.Compare(bstrTag, "u", true) == 0)
					{
						pRange.m_nUnderlined++;
						return;
					}
					if (string.Compare(bstrTag, "/u", true) == 0)
					{
						pRange.m_nUnderlined--;
						if (pRange.m_nUnderlined <= 0)
						{
							pRange.m_nUnderlined = 0;
							return;
						}
					}
					else
					{
						if (string.Compare(bstrTag, "sub", true) == 0)
						{
							pRange.m_nSub--;
							return;
						}
						if (string.Compare(bstrTag, "/sub", true) == 0)
						{
							if (pRange.m_nSub < 0)
							{
								pRange.m_nSub++;
								return;
							}
						}
						else
						{
							if (string.Compare(bstrTag, "sup", true) == 0)
							{
								pRange.m_nSub++;
								return;
							}
							if (string.Compare(bstrTag, "/sup", true) == 0)
							{
								if (pRange.m_nSub > 0)
								{
									pRange.m_nSub--;
									return;
								}
							}
							else
							{
								if (string.Compare(bstrTag, 0, "!--", 0, 3, true) == 0)
								{
									return;
								}
								if (string.Compare(bstrTag, 0, "font", 0, 4, true) == 0)
								{
									AuxSnippetRange item = new AuxSnippetRange(pRange);
									this.m_arrStack.Add(item);
									auxHtml.Set(bstrTag);
									auxHtml.Parse(pRange);
									return;
								}
								if (string.Compare(bstrTag, 0, "/font", 0, 5, true) == 0)
								{
									AuxSnippetRange auxSnippetRange = this.m_arrStack[this.m_arrStack.Count - 1];
									pRange.CopyFrom(auxSnippetRange);
									if (this.m_arrStack.Count > 1)
									{
										this.m_arrStack.Remove(auxSnippetRange);
										return;
									}
								}
								else
								{
									if (string.Compare(bstrTag, "center", true) == 0 || string.Compare(bstrTag, "/center", true) == 0)
									{
										pRange.m_nLength += this.AddCRLF(wszOutText, ref pOutPtr, 1);
										this.m_bNewParagraph = true;
										if (bstrTag[0] != '/')
										{
											pRange.m_nCenter++;
											return;
										}
										pRange.m_nCenter--;
										if (pRange.m_nCenter <= 0)
										{
											pRange.m_nCenter = 0;
											return;
										}
									}
									else
									{
										if (string.Compare(bstrTag, 0, "div", 0, 3, true) == 0)
										{
											pRange.m_nLength += this.AddCRLF(wszOutText, ref pOutPtr, 1);
											this.m_bNewParagraph = true;
											if (this.m_pLastDivRange != null)
											{
												this.m_pLastDivRange = null;
											}
											this.m_pLastDivRange = new AuxSnippetRange(pRange);
											auxHtml.Set(bstrTag);
											auxHtml.Parse(pRange);
											return;
										}
										if (string.Compare(bstrTag, "/div", true) == 0)
										{
											pRange.m_nLength += this.AddCRLF(wszOutText, ref pOutPtr, 1);
											this.m_bNewParagraph = true;
											if (this.m_pLastDivRange != null)
											{
												pRange.CopyFrom(this.m_pLastDivRange);
												this.m_pLastDivRange = null;
												return;
											}
										}
										else
										{
											if (string.Compare(bstrTag, 0, "a", 0, 1, true) == 0)
											{
												auxHtml.Set(bstrTag);
												auxHtml.Parse(pRange);
												return;
											}
											if (string.Compare(bstrTag, "/a", true) == 0)
											{
												pRange.m_bstrAnchor = "";
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		private int AddCRLF(char[] wszOutText, ref uint pOutPtr, int nHowMany)
		{
			uint num = pOutPtr;
			if (num >= 4u && wszOutText[(int)((UIntPtr)(num - 4u))] == '\r' && wszOutText[(int)((UIntPtr)(num - 3u))] == '\n' && wszOutText[(int)((UIntPtr)(num - 2u))] == '\r' && wszOutText[(int)((UIntPtr)(num - 1u))] == '\n')
			{
				return 0;
			}
			if (num > 2u && wszOutText[(int)((UIntPtr)(num - 2u))] == '\r' && wszOutText[(int)((UIntPtr)(num - 1u))] == '\n')
			{
				nHowMany--;
			}
			if (nHowMany < 0)
			{
				return 0;
			}
			for (int i = 0; i < nHowMany; i++)
			{
				wszOutText[(int)((UIntPtr)(num++))] = '\r';
				wszOutText[(int)((UIntPtr)(num++))] = '\n';
			}
			pOutPtr = (uint)(pOutPtr + 2 * nHowMany);
			return 2 * nHowMany;
		}
		private bool ParseSymbol(string Text, int nLen, ref int nPtr, out char pCh)
		{
			pCh = '\0';
			if (nPtr + 1 >= nLen || Text[nPtr + 1] != '#')
			{
				int num = HtmlCSS.SymbolMap.Length;
				for (int i = 0; i < num; i++)
				{
					if (string.Compare(Text.Substring(nPtr), 0, HtmlCSS.SymbolMap[i].symbol, 0, HtmlCSS.SymbolMap[i].symbol.Length) == 0)
					{
						pCh = HtmlCSS.SymbolMap[i].code;
						nPtr += HtmlCSS.SymbolMap[i].symbol.Length;
						return true;
					}
				}
				nPtr++;
				return false;
			}
			int num2 = nPtr + 2;
			uint num3 = 0u;
			while (num2 < nLen && Text[num2] >= '0' && Text[num2] <= '9')
			{
				num3 = 10u * num3 + (uint)(Text[num2++] - '0');
			}
			if (num2 < nLen && Text[num2] == ';' && num3 < 65535u)
			{
				pCh = (char)num3;
				nPtr = num2 + 1;
				return true;
			}
			nPtr++;
			return false;
		}
		public static bool IsSpace(char ch)
		{
			return ch == ' ' || ch == '\r' || ch == '\n' || ch == '\t';
		}
		private void ClearSnippets()
		{
			this.m_arrStack.Clear();
			this.m_arrRanges.Clear();
			this.m_pLastDivRange = null;
		}
		private bool ParseTag(string Text, int nLen, ref int nPtr, out string Tag)
		{
			Tag = "";
			nPtr++;
			if (nPtr < nLen && (Text[nPtr] == '/' || Text[nPtr] == '!' || (Text[nPtr] >= 'a' && Text[nPtr] <= 'z') || (Text[nPtr] >= 'A' && Text[nPtr] <= 'Z')))
			{
				StringBuilder stringBuilder = new StringBuilder();
				while (nPtr < nLen && Text[nPtr] != '>')
				{
					stringBuilder.Append(Text[nPtr++]);
				}
				if (nPtr < nLen)
				{
					nPtr++;
				}
				Tag = stringBuilder.ToString();
				return true;
			}
			return false;
		}
		internal PdfCanvas()
		{
			this.m_pResourceObj = null;
			this.m_pContentsObj = null;
			this.m_pManager = null;
			this.m_pFontDictObj = null;
			this.m_pXObjectDictObj = null;
			this.m_pCSDictObj = null;
			this.m_bPathEmpty = true;
			this.m_nTabCount = 0;
			this.m_arrTabIndexes[0] = -1;
			this.m_arrTabWidths[0] = 0f;
		}
		internal void Append(byte[] sz)
		{
			if (this.m_pContentsObj == null)
			{
				return;
			}
			this.m_pContentsObj.m_objStream.Append(sz);
		}
		internal void Append(string sz)
		{
			this.Append(Encoding.UTF8.GetBytes(sz));
			this.Append(new byte[]
			{
				10
			});
		}
		internal void AppendNoLF(string sz)
		{
			this.Append(Encoding.UTF8.GetBytes(sz));
		}
		public void Clear()
		{
			this.m_pContentsObj.m_objStream.Alloc(10);
		}
		internal void TextTo(float x, float y)
		{
			this.AppendFormatted("Td", new float[]
			{
				x,
				y
			});
		}
		internal void TextRise(float fSize)
		{
			this.AppendFormatted("Ts", new float[]
			{
				fSize
			});
		}
		internal void CTM(double a, double b, double c, double d, double e, double f)
		{
			this.AppendFormatted("cm", new float[]
			{
				(float)a,
				(float)b,
				(float)c,
				(float)d,
				(float)e,
				(float)f
			});
		}
		internal void CTM(float a, float b, float c, float d, float e, float f)
		{
			this.AppendFormatted("cm", new float[]
			{
				a,
				b,
				c,
				d,
				e,
				f
			});
		}
		internal void Tm(float a, float b, float c, float d, float e, float f)
		{
			this.AppendFormatted("Tm", new float[]
			{
				a,
				b,
				c,
				d,
				e,
				f
			});
		}
		internal void DoImage(string ID)
		{
			this.Append("/" + ID + " Do");
		}
		public void BeginText()
		{
			this.Append("BT");
		}
		internal void EndText()
		{
			this.Append("ET");
		}
		internal void SelectFont(string szFont, float fSize)
		{
			this.AppendNoLF("/" + szFont + " ");
			this.AppendFormatted("Tf", new float[]
			{
				fSize
			});
		}
		public void DrawLine(float x1, float y1, float x2, float y2)
		{
			this.MoveTo(x1, y1);
			this.LineTo(x2, y2);
			this.Stroke();
		}
		public void MoveTo(float x, float y)
		{
			this.AppendFormatted("m", new float[]
			{
				x,
				y
			});
			this.m_bPathEmpty = false;
		}
		public void LineTo(float x, float y)
		{
			this.CheckEmpty();
			this.AppendFormatted("l", new float[]
			{
				x,
				y
			});
			this.m_bPathEmpty = false;
		}
		public void Stroke()
		{
			this.CheckEmpty();
			this.Append("S");
			this.m_bPathEmpty = true;
		}
		public void Fill()
		{
			this.Fill(false);
		}
		public void Fill(bool EvenOdd)
		{
			this.CheckEmpty();
			this.AppendNoLF("f");
			if (EvenOdd)
			{
				this.AppendNoLF("*");
			}
			this.AppendNoLF("\n");
			this.m_bPathEmpty = true;
		}
		public void FillStroke()
		{
			this.FillStroke(false);
		}
		public void FillStroke(bool EvenOdd)
		{
			this.CheckEmpty();
			this.AppendNoLF("B");
			if (EvenOdd)
			{
				this.AppendNoLF("*");
			}
			this.AppendNoLF("\n");
			this.m_bPathEmpty = true;
		}
		public void SetParams(object prm)
		{
			PdfParam pdfParam = this.m_pManager.VariantToParam(prm);
			AuxRGB auxRGB = new AuxRGB();
			if (pdfParam.IsSet("Color"))
			{
				auxRGB.Set((uint)pdfParam.Long("Color"));
				this.SetColor(auxRGB.r, auxRGB.g, auxRGB.b);
			}
			if (pdfParam.IsSet("r") && pdfParam.IsSet("g") && pdfParam.IsSet("b"))
			{
				this.SetColor(pdfParam.Number("r"), pdfParam.Number("g"), pdfParam.Number("b"));
			}
			if (pdfParam.IsSet("c") && pdfParam.IsSet("m") && pdfParam.IsSet("y") && pdfParam.IsSet("k"))
			{
				this.SetColorCMYK(pdfParam.Number("c"), pdfParam.Number("m"), pdfParam.Number("y"), pdfParam.Number("k"));
			}
			if (pdfParam.IsSet("FillColor"))
			{
				auxRGB.Set((uint)pdfParam.Long("FillColor"));
				this.SetFillColor(auxRGB.r, auxRGB.g, auxRGB.b);
			}
			if (pdfParam.IsSet("fr") && pdfParam.IsSet("fg") && pdfParam.IsSet("fb"))
			{
				this.SetFillColor(pdfParam.Number("fr"), pdfParam.Number("fg"), pdfParam.Number("fb"));
			}
			if (pdfParam.IsSet("fc") && pdfParam.IsSet("fm") && pdfParam.IsSet("fy") && pdfParam.IsSet("fk"))
			{
				this.SetFillColorCMYK(pdfParam.Number("fc"), pdfParam.Number("fm"), pdfParam.Number("fy"), pdfParam.Number("fk"));
			}
			if (pdfParam.IsSet("LineWidth"))
			{
				this.LineWidth = pdfParam.Number("LineWidth");
			}
			if (pdfParam.IsSet("LineCap"))
			{
				this.LineCap = pdfParam.Long("LineCap");
			}
			if (pdfParam.IsSet("LineJoin"))
			{
				this.LineJoin = pdfParam.Long("LineJoin");
			}
			if (pdfParam.IsSet("MiterLimit"))
			{
				this.MiterLimit = pdfParam.Number("MiterLimit");
			}
			if (pdfParam.IsSet("Dash1"))
			{
				int num = 0;
				if (pdfParam.IsSet("DashPhase"))
				{
					num = pdfParam.Long("DashPhase");
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("[");
				int num2 = 1;
				do
				{
					string name = string.Format("Dash{0}", num2);
					if (!pdfParam.IsSet(name))
					{
						break;
					}
					int num3 = pdfParam.Long(name);
					if (num3 <= 0)
					{
						break;
					}
					stringBuilder.Append(string.Format(" {0}", num3));
					num2++;
				}
				while (num2 <= 10);
				stringBuilder.Append("] ");
				stringBuilder.Append(string.Format("{0} d", num));
				this.Append(stringBuilder.ToString());
			}
		}
		public void SetColor(float r, float g, float b)
		{
			if (r < 0f || r > 1f || g < 0f || g > 1f || b < 0f || b > 1f)
			{
				AuxException.Throw("The R, G, B values must be in the range 0.0 to 1.0", PdfErrors._ERROR_OUTOFRANGE);
			}
			this.AppendFormatted("RG", new float[]
			{
				r,
				g,
				b
			});
		}
		public void SetFillColor(float r, float g, float b)
		{
			if (r < 0f || r > 1f || g < 0f || g > 1f || b < 0f || b > 1f)
			{
				AuxException.Throw("The R, G, B values must be in the range 0.0 to 1.0", PdfErrors._ERROR_OUTOFRANGE);
			}
			this.AppendFormatted("rg", new float[]
			{
				r,
				g,
				b
			});
		}
		public void SetColorCMYK(float c, float m, float y, float k)
		{
			if (c < 0f || c > 1f || m < 0f || m > 1f || y < 0f || y > 1f || k < 0f || k > 1f)
			{
				AuxException.Throw("The C, M, Y, K values must be in the range 0.0 to 1.0", PdfErrors._ERROR_OUTOFRANGE);
			}
			this.AppendFormatted("K", new float[]
			{
				c,
				m,
				y,
				k
			});
		}
		public void SetFillColorCMYK(float c, float m, float y, float k)
		{
			if (c < 0f || c > 1f || m < 0f || m > 1f || y < 0f || y > 1f || k < 0f || k > 1f)
			{
				AuxException.Throw("The C, M, Y, K values must be in the range 0.0 to 1.0", PdfErrors._ERROR_OUTOFRANGE);
			}
			this.AppendFormatted("k", new float[]
			{
				c,
				m,
				y,
				k
			});
		}
		public void DrawRect(float X, float Y, float Width, float Height)
		{
			if (!this.m_bPathEmpty)
			{
				this.AbandonPath();
			}
			this.AddRect(X, Y, Width, Height);
			this.Stroke();
		}
		public void FillRect(float X, float Y, float Width, float Height)
		{
			if (!this.m_bPathEmpty)
			{
				this.AbandonPath();
			}
			this.AddRect(X, Y, Width, Height);
			this.Fill();
		}
		public void AddRect(float X, float Y, float Width, float Height)
		{
			this.AppendFormatted("re", new float[]
			{
				X,
				Y,
				Width,
				Height
			});
			this.m_bPathEmpty = false;
		}
		public void ClosePath()
		{
			this.Append("h");
		}
		public void AbandonPath()
		{
			this.Append("n");
			this.m_bPathEmpty = true;
		}
		public void AddCurve(float x1, float y1, float x2, float y2, float x3, float y3)
		{
			this.CheckEmpty();
			this.AppendFormatted("c", new float[]
			{
				x1,
				y1,
				x2,
				y2,
				x3,
				y3
			});
		}
		public void AddEllipse(float x, float y, float Rx, float Ry)
		{
			this.MoveTo(x + Rx, y);
			this.AddCurve(x + Rx, y + Ry * 0.5523f, x + Rx * 0.5523f, y + Ry, x, y + Ry);
			this.AddCurve(x - Rx * 0.5523f, y + Ry, x - Rx, y + Ry * 0.5523f, x - Rx, y);
			this.AddCurve(x - Rx, y - Ry * 0.5523f, x - Rx * 0.5523f, y - Ry, x, y - Ry);
			this.AddCurve(x + Rx * 0.5523f, y - Ry, x + Rx, y - Ry * 0.5523f, x + Rx, y);
		}
		public void DrawEllipse(float X, float Y, float Rx, float Ry)
		{
			if (!this.m_bPathEmpty)
			{
				this.AbandonPath();
			}
			this.AddEllipse(X, Y, Rx, Ry);
			this.Stroke();
		}
		public void FillEllipse(float X, float Y, float Rx, float Ry)
		{
			if (!this.m_bPathEmpty)
			{
				this.AbandonPath();
			}
			this.AddEllipse(X, Y, Rx, Ry);
			this.Fill();
		}
		public void Clip()
		{
			this.Clip(false);
		}
		public void Clip(bool EvenOdd)
		{
			this.CheckEmpty();
			this.AppendNoLF("W");
			if (EvenOdd)
			{
				this.AppendNoLF("*");
			}
			this.AppendNoLF("\n");
			this.AbandonPath();
			this.m_bPathEmpty = true;
		}
		public void SaveState()
		{
			this.Append("q");
		}
		public void RestoreState()
		{
			this.Append("Q");
		}
		public void SetCTM(float a, float b, float c, float d, float e, float f)
		{
			this.CTM(a, b, c, d, e, f);
		}
		public void SetTM(float a, float b, float c, float d, float e, float f)
		{
			this.Tm(a, b, c, d, e, f);
		}
		internal void AppendFormatted(string Command, float[] args)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < args.Length; i++)
			{
				float num = args[i];
				PdfNumber pdfNumber = new PdfNumber(null, (double)num);
				stringBuilder.Append(pdfNumber.ToString());
				stringBuilder.Append(" ");
			}
			stringBuilder.Append(Command);
			this.Append(stringBuilder.ToString());
		}
		internal void CheckEmpty()
		{
			if (this.m_bPathEmpty)
			{
				AuxException.Throw("Path undefined.", PdfErrors._ERROR_PATHNOTSTARTED);
			}
		}
		public void DrawGraphics(PdfGraphics Graphics, object Param)
		{
			this.DrawImage(new PdfImage
			{
				m_pDoc = this.m_pDoc,
				m_pManager = this.m_pManager,
				m_bstrID = Graphics.m_bstrID,
				m_pImageObj = Graphics.m_pXObj,
				m_fWidth = 1f,
				m_fHeight = 1f
			}, Param);
		}
		public void DrawImage(PdfImage Image, object Param)
		{
			if (Image == null)
			{
				AuxException.Throw("The Image object is empty.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			PdfParam pdfParam = this.m_pManager.VariantToParam(Param);
			if (!pdfParam.IsSet("X") || !pdfParam.IsSet("Y"))
			{
				AuxException.Throw("X and/or Y parameters not set.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			float e = pdfParam.Number("X");
			float f = pdfParam.Number("Y");
			if (this.m_pResourceObj != null)
			{
				PdfDict pdfDict;
				if (this.m_pXObjectDictObj == null)
				{
					if (this.m_pResourceObj.m_nType == enumIndirectType.pdfIndirectResource)
					{
						pdfDict = this.m_pResourceObj.m_objAttributes;
					}
					else
					{
						pdfDict = this.m_pResourceObj.AddDict("Resources");
					}
					this.m_pResourceObj.m_bModified = true;
				}
				else
				{
					pdfDict = this.m_pXObjectDictObj.m_objAttributes;
					this.m_pXObjectDictObj.m_bModified = true;
				}
				PdfDict pdfDict2 = (this.m_pXObjectDictObj != null) ? this.m_pXObjectDictObj.m_objAttributes : ((PdfDict)pdfDict.GetObjectByName("XObject"));
				if (pdfDict2 == null)
				{
					pdfDict2 = new PdfDict("XObject");
					pdfDict.Add(pdfDict2);
				}
				pdfDict2.Add(new PdfReference(Image.m_bstrID, Image.m_pImageObj));
			}
			this.SaveState();
			this.CTM(1f, 0f, 0f, 1f, e, f);
			if (pdfParam.IsSet("Angle"))
			{
				float num = pdfParam.Number("Angle");
				float num2 = 0.01745329f;
				this.CTM(Math.Cos((double)(num * num2)), Math.Sin((double)(num * num2)), -Math.Sin((double)(num * num2)), Math.Cos((double)(num * num2)), 0.0, 0.0);
			}
			float num3 = Image.m_fDefaultScaleX;
			float num4 = Image.m_fDefaultScaleY;
			if (pdfParam.IsSet("ScaleX"))
			{
				num3 *= pdfParam.Number("ScaleX");
			}
			if (pdfParam.IsSet("ScaleY"))
			{
				num4 *= pdfParam.Number("ScaleY");
			}
			if (num3 == 0f || num4 == 0f)
			{
				AuxException.Throw("ScaleX and/or ScaleY arguments are 0.", PdfErrors._ERROR_INVALIDARG);
			}
			this.CTM(Image.m_fWidth * num3, 0f, 0f, Image.m_fHeight * num4, 0f, 0f);
			this.DoImage(Image.m_bstrID);
			this.RestoreState();
		}
		public int DrawTable(PdfTable Table, object Param)
		{
			if (Table == null)
			{
				AuxException.Throw("Table argument is empty.", PdfErrors._ERROR_OUTOFRANGE);
			}
			return Table.Draw(this, Param);
		}
		internal void AppendText(int FromIndex, int ToIndex, float CurWidth, ref float pCurYPos)
		{
			while (ToIndex >= 0 && this.m_bstrText[ToIndex] == ' ')
			{
				ToIndex--;
				CurWidth -= (float)this.m_nSpaceWidth * this.m_fSize;
			}
			string text;
			if (ToIndex - FromIndex + 1 < 0)
			{
				text = "";
			}
			else
			{
				text = this.m_bstrText.Substring(FromIndex, ToIndex - FromIndex + 1);
			}
			if (this.m_bReverseHebrewArabic)
			{
				this.ReverseHebrewArabic(ref text);
			}
			float num = 0f;
			if (this.m_nAlignment > 0)
			{
				num = (this.m_fTextWidth - CurWidth) / (float)this.m_pFont.m_nUnitsPerEm;
				if (this.m_nAlignment == 2)
				{
					num /= 2f;
				}
			}
			this.TextTo(num, this.m_bFirstLine ? (-this.m_fSize) : this.m_fNextLineOffset);
			pCurYPos += (this.m_bFirstLine ? (-this.m_fSize) : this.m_fNextLineOffset);
			this.m_bFirstLine = false;
			if (text.Length > 0)
			{
				if (this.m_nTabCount == 0)
				{
					PdfStream pdfStream = new PdfStream();
					float num2;
					this.m_pFont.UnicodeToGlyph(text.ToString(), ref pdfStream, out num2);
					this.Append(pdfStream.m_objMemStream.ToArray());
					this.Append(" Tj");
				}
				else
				{
					this.m_arrTabIndexes[this.m_nTabCount + 1] = text.Length;
					this.m_arrTabWidths[this.m_nTabCount + 1] = 0f;
					float num3 = 0f;
					float num4 = 0f;
					for (int i = 1; i <= this.m_nTabCount + 1; i++)
					{
						StringBuilder stringBuilder = new StringBuilder();
						int num5 = this.m_arrTabIndexes[i] - this.m_arrTabIndexes[i - 1] - 1;
						if (num5 > 0)
						{
							stringBuilder.Append(text.ToString().Substring(this.m_arrTabIndexes[i - 1] + 1, num5));
							PdfStream pdfStream2 = new PdfStream();
							this.m_pFont.UnicodeToGlyph(stringBuilder.ToString(), ref pdfStream2, out num4);
							this.Append(pdfStream2.m_objMemStream.ToArray());
							this.Append(" Tj");
							num3 += num4 * this.m_fSize;
							this.TextTo(num4 * this.m_fSize, 0f);
						}
						num3 += this.m_arrTabWidths[i];
						this.TextTo(this.m_arrTabWidths[i], 0f);
					}
					this.TextTo(-num3, 0f);
				}
			}
			if (num > 0f)
			{
				this.TextTo(-num, 0f);
			}
			this.m_nTabCount = 0;
		}
		public int DrawText(string Text, object Param, PdfFont Font)
		{
			PdfParam pdfParam = this.m_pManager.VariantToParam(Param);
			if (Font == null)
			{
				AuxException.Throw("The Font object is empty.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			this.m_pFont = Font;
			this.m_fVertialExtent = 0f;
			this.m_bFirstLine = true;
			string text;
			this.ReshapeArabic(Text, out text);
			int length = text.Length;
			if (length <= 0)
			{
				return 0;
			}
			int result = length;
			if (!pdfParam.IsSet("X"))
			{
				AuxException.Throw("X parameter expected.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			if (!pdfParam.IsSet("Y"))
			{
				AuxException.Throw("Y parameter expected.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			float num = pdfParam.Number("X");
			float num2 = pdfParam.Number("Y");
			int num3 = 0;
			float num4 = this.m_pDoc.m_fDefaultFontSize;
			this.m_pFont.CreateIndirectObject(this.m_pResourceObj, this.m_pFontDictObj);
			if (pdfParam.IsSet("Size"))
			{
				num4 = pdfParam.Number("Size");
			}
			this.BeginText();
			this.SelectFont(this.m_pDoc.m_szFontID, num4);
			if (pdfParam.IsSet("Angle"))
			{
				float num5 = pdfParam.Number("Angle");
				float num6 = 0.0174528f;
				this.Tm((float)Math.Cos((double)(num5 * num6)), (float)Math.Sin((double)(num5 * num6)), -(float)Math.Sin((double)(num5 * num6)), (float)Math.Cos((double)(num5 * num6)), num, num2);
			}
			else
			{
				this.TextTo(num, num2);
			}
			if (pdfParam.IsSet("Color"))
			{
				float num7;
				float num8;
				float num9;
				pdfParam.Color("Color", out num7, out num8, out num9);
				if (num7 < 0f || num7 > 1f || num8 < 0f || num8 > 1f || num9 < 0f || num9 > 1f)
				{
					AuxException.Throw("An RGB value must be between 0.0 and 1.0.", PdfErrors._ERROR_INVALIDFONTARG);
				}
				this.SetFillColor(num7, num8, num9);
			}
			if (pdfParam.IsSet("tc") && pdfParam.IsSet("tm") && pdfParam.IsSet("ty") && pdfParam.IsSet("tk"))
			{
				float num10 = pdfParam.Number("tc");
				float num11 = pdfParam.Number("tm");
				float num12 = pdfParam.Number("ty");
				float num13 = pdfParam.Number("tk");
				if (num10 < 0f || num10 > 1f || num11 < 0f || num11 > 1f || num12 < 0f || num12 > 1f || num13 < 0f || num13 > 1f)
				{
					AuxException.Throw("A CMYK value must be between 0.0 and 1.0.", PdfErrors._ERROR_INVALIDFONTARG);
				}
				this.SetFillColorCMYK(num10, num11, num12, num13);
				this.m_bCMYK = true;
			}
			if (pdfParam.IsSet("Rendering"))
			{
				long num14 = (long)pdfParam.Long("Rendering");
				if (num14 < 0L || num14 > 7L)
				{
					AuxException.Throw("The Rendering value must be between 0 and 7.", PdfErrors._ERROR_INVALIDFONTARG);
				}
				this.AppendFormatted("Tr", new float[]
				{
					(float)num14
				});
			}
			if (pdfParam.IsTrue("HTML"))
			{
				return this.DrawHtml(Text, pdfParam, num, num2, Font, num4);
			}
			if (pdfParam.IsSet("Alignment"))
			{
				num3 = pdfParam.Long("Alignment");
				if (num3 < 0 || num3 > 2)
				{
					AuxException.Throw("Valid Alignment values are 0 (left), 1 (right) or 2 (center).", PdfErrors._ERROR_INVALIDFONTARG);
				}
				if (num3 > 0 && !pdfParam.IsSet("Width"))
				{
					AuxException.Throw("If Alignment is set to 1 (right) or 2 (center), Width must also be specified.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
				}
			}
			float num15 = num2;
			int i = 0;
			int num16 = 0;
			int num17 = -1;
			float num18 = 0f;
			float curWidth = 0f;
			float num19 = 16777215f;
			if (pdfParam.IsSet("Width"))
			{
				num19 = pdfParam.Number("Width") * (float)this.m_pFont.m_nUnitsPerEm;
			}
			this.m_bstrText = text;
			this.m_fNextLineOffset = -num4;
			if (pdfParam.IsSet("Spacing"))
			{
				this.m_fNextLineOffset *= pdfParam.Number("Spacing");
			}
			int num20 = 10;
			if (pdfParam.IsSet("Tab"))
			{
				num20 = pdfParam.Long("Tab");
			}
			ushort num21;
			ushort num22;
			this.m_pFont.GetCharInfo(' ', out num21, out num22);
			float num23 = num4 * (float)num20 * (float)num22;
			this.m_nTabCount = 0;
			this.m_fSize = num4;
			this.m_nSpaceWidth = (int)num22;
			this.m_nAlignment = num3;
			this.m_fTextWidth = num19;
			this.m_bReverseHebrewArabic = pdfParam.Bool("ReverseHebrewArabic");
			float num24 = -1f;
			if (pdfParam.IsSet("Height"))
			{
				num24 = pdfParam.Number("Height");
			}
			float num25 = num24 + num4 * (float)this.m_pFont.m_nDescent / (float)this.m_pFont.m_nUnitsPerEm;
			while (i < length)
			{
				if (num24 != -1f && num2 - num15 - (this.m_bFirstLine ? (-this.m_fSize) : this.m_fNextLineOffset) > num25)
				{
					result = num16;
					break;
				}
				ushort num26;
				this.m_pFont.GetCharInfo(text[i], out num21, out num26);
				int num27 = (int)((float)num26 * num4);
				if ((float)num27 > num19)
				{
					break;
				}
				if (text[i] == ' ' || text[i] == '\t')
				{
					num17 = i;
					curWidth = num18;
				}
				if (text[i] == '\r' || text[i] == '\n')
				{
					this.AppendText(num16, i - 1, num18, ref num15);
					i++;
					if (text[i - 1] == '\r' && text[i] == '\n')
					{
						i++;
					}
					num16 = i;
					num17 = -1;
					num18 = 0f;
				}
				else
				{
					if (text[i] == '\t' && !this.m_bReverseHebrewArabic)
					{
						num27 = (int)(num23 * (num18 / num23 + 1f) - num18);
						this.m_arrTabIndexes[++this.m_nTabCount] = i - num16;
						this.m_arrTabWidths[this.m_nTabCount] = (float)num27 / (float)this.m_pFont.m_nUnitsPerEm;
					}
					num18 += (float)num27;
					if (num18 > num19)
					{
						if (num17 < 0)
						{
							this.AppendText(num16, i - 1, num18 - (float)num27, ref num15);
							num16 = i;
							num18 = 0f;
						}
						else
						{
							this.AppendText(num16, num17 - 1, curWidth, ref num15);
							bool flag = i == num17;
							while (num17 < length - 1 && text[++num17] == ' ')
							{
							}
							if (flag && (text[num17] == '\r' || text[num17] == '\n'))
							{
								num17++;
								if (num17 < length && text[num17 - 1] == '\r' && text[num17] == '\n')
								{
									num17++;
								}
							}
							i = (num16 = num17);
							num18 = 0f;
							num17 = -1;
						}
					}
					else
					{
						i++;
						if (i == length)
						{
							this.AppendText(num16, i - 1, num18, ref num15);
						}
					}
				}
			}
			this.m_fVertialExtent = num2 - num15 - num4 * (float)this.m_pFont.m_nDescent / (float)this.m_pFont.m_nUnitsPerEm + 0.1f;
			this.EndText();
			return result;
		}
		public void FillWithPattern(PdfGraphics Graphics)
		{
			this.FillWithPattern(Graphics, false);
		}
		public void FillWithPattern(PdfGraphics Graphics, bool EvenOdd)
		{
			if (Graphics == null)
			{
				AuxException.Throw("The Graphics object is empty.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			if (!Graphics.m_bPattern)
			{
				AuxException.Throw("The specified graphics object is not a pattern. Use the parameter Pattern=true when calling CreateGraphics.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			if (Graphics.m_pXObj.m_objAttributes.GetObjectByName("Resources") == null)
			{
				Graphics.m_pXObj.AddDict("Resources");
			}
			if (this.m_pResourceObj != null)
			{
				PdfDict pdfDict;
				if (this.m_pResourceObj.m_nType == enumIndirectType.pdfIndirectResource)
				{
					pdfDict = this.m_pResourceObj.m_objAttributes;
				}
				else
				{
					pdfDict = this.m_pResourceObj.AddDict("Resources");
				}
				this.m_pResourceObj.m_bModified = true;
				PdfDict pdfDict2 = (PdfDict)pdfDict.GetObjectByName("Pattern");
				if (pdfDict2 == null)
				{
					pdfDict2 = new PdfDict("Pattern");
					pdfDict.Add(pdfDict2);
				}
				pdfDict2.Add(new PdfReference(Graphics.m_bstrID, Graphics.m_pXObj));
			}
			this.Append("/Pattern cs");
			this.Append(string.Format("/{0} scn", Graphics.m_bstrID));
			this.Fill(EvenOdd);
		}
		public void DrawBarcode(string Data, object Param)
		{
			PdfParam pParam = this.m_pManager.VariantToParam(Param);
			PdfBarcode pdfBarcode = new PdfBarcode();
			pdfBarcode.m_pManager = this.m_pManager;
			pdfBarcode.m_pDoc = this.m_pDoc;
			this.SaveState();
			pdfBarcode.Draw(Data, this, pParam);
			this.RestoreState();
		}
		public PdfParam DrawBarcode2D(byte[] BinaryData, object Param)
		{
			return this.DrawBarcode2DHelper(null, BinaryData, Param);
		}
		public PdfParam DrawBarcode2D(string TextData, object Param)
		{
			return this.DrawBarcode2DHelper(TextData, null, Param);
		}
		internal PdfParam DrawBarcode2DHelper(string TextData, byte[] BinaryData, object Param)
		{
			PdfParam pdfParam = this.m_pManager.VariantToParam(Param);
			int num = 1;
			if (pdfParam.IsSet("Type"))
			{
				num = pdfParam.Long("Type");
			}
			if (num < 1 || num > 3)
			{
				AuxException.Throw("Valid Type values are 1 (PDF417), 2 (DataMatrix) and 3 (QR Code.)", PdfErrors._ERROR_INVALIDARG);
			}
			PdfBarcode417 pdfBarcode = new PdfBarcode417();
			pdfBarcode.m_pManager = this.m_pManager;
			pdfBarcode.m_pDoc = this.m_pDoc;
			if (TextData != null && TextData.Length >= 5420)
			{
				AuxException.Throw("TextData argument is too long.", PdfErrors._ERROR_INVALIDARG);
			}
			if (TextData == null && BinaryData == null)
			{
				AuxException.Throw("Neither text nor binary data for the barcode is specified.", PdfErrors._ERROR_INVALIDARG);
			}
			if (num == 2)
			{
				return this.DrawBarcodeMatrix(TextData, BinaryData, this, pdfParam);
			}
			if (num == 3)
			{
				return this.DrawBarcodeQR(TextData, BinaryData, this, pdfParam);
			}
			this.SaveState();
			pdfBarcode.Draw(TextData, BinaryData, this, pdfParam);
			this.RestoreState();
			PdfParam pdfParam2 = new PdfParam();
			pdfParam2.AddItem("Width", pdfBarcode.m_fBarWidth * (float)pdfBarcode.m_nBitColumns);
			pdfParam2.AddItem("Height", pdfBarcode.m_fYHeight * (float)pdfBarcode.m_nCodeRows);
			pdfParam2.AddItem("ErrorLevel", (float)pdfBarcode.m_nErrorLevel);
			pdfParam2.AddItem("Columns", (float)pdfBarcode.m_nCodeColumns);
			pdfParam2.AddItem("Rows", (float)pdfBarcode.m_nCodeRows);
			return pdfParam2;
		}
		internal PdfParam DrawBarcodeMatrix(string TextData, byte[] pBinaryData, PdfCanvas pCanvas, PdfParam pParam)
		{
			PdfBarcodeMatrix pdfBarcodeMatrix = new PdfBarcodeMatrix();
			pdfBarcodeMatrix.m_pManager = this.m_pManager;
			pdfBarcodeMatrix.m_pDoc = this.m_pDoc;
			if (pBinaryData == null)
			{
				pdfBarcodeMatrix.m_pData = Encoding.UTF8.GetBytes(TextData);
				pdfBarcodeMatrix.m_nDataLen = pdfBarcodeMatrix.m_pData.Length;
			}
			else
			{
				pdfBarcodeMatrix.m_pData = pBinaryData;
				pdfBarcodeMatrix.m_nDataLen = pBinaryData.Length;
			}
			this.SaveState();
			pdfBarcodeMatrix.Draw(pCanvas, pParam);
			this.RestoreState();
			PdfParam pdfParam = new PdfParam();
			pdfParam.AddItem("Width", pdfBarcodeMatrix.m_fBarWidth * (float)pdfBarcodeMatrix.m_nWidth);
			pdfParam.AddItem("Height", pdfBarcodeMatrix.m_fBarWidth * (float)pdfBarcodeMatrix.m_nHeight);
			pdfParam.AddItem("Columns", (float)pdfBarcodeMatrix.m_nWidth);
			pdfParam.AddItem("Rows", (float)pdfBarcodeMatrix.m_nHeight);
			return pdfParam;
		}
		internal PdfParam DrawBarcodeQR(string TextData, byte[] pBinaryData, PdfCanvas pCanvas, PdfParam pParam)
		{
			PdfBarcodeQR pdfBarcodeQR = new PdfBarcodeQR();
			pdfBarcodeQR.m_pManager = this.m_pManager;
			pdfBarcodeQR.m_pDoc = this.m_pDoc;
			if (pBinaryData == null)
			{
				byte[] bytes = Encoding.UTF8.GetBytes(TextData);
				if (bytes.Length > TextData.Length)
				{
					byte[] array = new byte[bytes.Length + 3];
					Array.Copy(new byte[]
					{
						239,
						187,
						191
					}, 0, array, 0, 3);
					Array.Copy(bytes, 0, array, 3, bytes.Length);
					pdfBarcodeQR.m_pData = array;
					pdfBarcodeQR.m_nDataLen = array.Length;
				}
				else
				{
					pdfBarcodeQR.m_pData = bytes;
					pdfBarcodeQR.m_nDataLen = bytes.Length;
				}
			}
			else
			{
				pdfBarcodeQR.m_pData = pBinaryData;
				pdfBarcodeQR.m_nDataLen = pBinaryData.Length;
				pdfBarcodeQR.m_nEightBit = 1;
			}
			this.SaveState();
			pdfBarcodeQR.Draw(pCanvas, pParam);
			this.RestoreState();
			PdfParam pdfParam = new PdfParam();
			pdfParam.AddItem("Width", pdfBarcodeQR.m_fBarWidth * (float)pdfBarcodeQR.m_nWidth);
			pdfParam.AddItem("Height", pdfBarcodeQR.m_fBarWidth * (float)pdfBarcodeQR.m_nHeight);
			pdfParam.AddItem("Columns", (float)pdfBarcodeQR.m_nWidth);
			pdfParam.AddItem("Rows", (float)pdfBarcodeQR.m_nHeight);
			return pdfParam;
		}
		public void SetColorSpace(PdfColorSpace ColorSpace)
		{
			this.SetColorSpaceHelper(ColorSpace, true);
		}
		public void SetFillColorSpace(PdfColorSpace ColorSpace)
		{
			this.SetColorSpaceHelper(ColorSpace, false);
		}
		internal void SetColorSpaceHelper(PdfColorSpace pColorSpace, bool bStroke)
		{
			if (pColorSpace == null)
			{
				AuxException.Throw("The ColorSpace object is empty.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			pColorSpace.Validate();
			this.Append("/" + pColorSpace.m_bstrID + " " + (bStroke ? "CS" : "cs"));
			if (this.m_pResourceObj != null && pColorSpace.m_pCSObj != null)
			{
				PdfDict pdfDict;
				if (this.m_pCSDictObj == null)
				{
					if (this.m_pResourceObj.m_nType == enumIndirectType.pdfIndirectResource)
					{
						pdfDict = this.m_pResourceObj.m_objAttributes;
					}
					else
					{
						pdfDict = this.m_pResourceObj.AddDict("Resources");
					}
					this.m_pResourceObj.m_bModified = true;
				}
				else
				{
					pdfDict = this.m_pCSDictObj.m_objAttributes;
					this.m_pCSDictObj.m_bModified = true;
				}
				PdfDict pdfDict2 = (this.m_pCSDictObj != null) ? this.m_pCSDictObj.m_objAttributes : ((PdfDict)pdfDict.GetObjectByName("ColorSpace"));
				if (pdfDict2 == null)
				{
					pdfDict2 = new PdfDict("ColorSpace");
					pdfDict.Add(pdfDict2);
				}
				pdfDict2.Add(new PdfReference(pColorSpace.m_bstrID, pColorSpace.m_pCSObj));
			}
		}
		internal void SetColorExHelper(object Param, bool bStroke)
		{
			new AuxRGB();
			PdfParam pdfParam = this.m_pManager.VariantToParam(Param);
			pdfParam.IsTrue("Fill");
			double[] array = new double[10];
			int num = 0;
			for (int i = 0; i < 10; i++)
			{
				num = i;
				if (!pdfParam.GetNumberIfSet("c%d", i + 1, ref array[i]))
				{
					break;
				}
				if (array[i] < 0.0 || array[i] > 1.0)
				{
					AuxException.Throw("A color component value must be in the range [0, 1].", PdfErrors._ERROR_PARAMNOTSPECIFIED);
				}
			}
			if (num == 0)
			{
				AuxException.Throw("No color components are set.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			for (int j = 0; j < num; j++)
			{
				this.AppendFormatted("", new float[]
				{
					(float)array[j]
				});
			}
			this.Append(bStroke ? "SCN" : "scn");
		}
		public void SetColorEx(object Param)
		{
			this.SetColorExHelper(Param, true);
		}
		public void SetFillColorEx(object Param)
		{
			this.SetColorExHelper(Param, false);
		}
		internal void ReshapeArabic(string Text, out string bstrText)
		{
			int length = Text.Length;
			bool flag = false;
			arabic_level level = arabic_level.ar_standard;
			for (int i = 0; i < length; i++)
			{
				if (Text[i] >= '؀' && Text[i] <= 'ۿ')
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				char[] array = new char[2 * length];
				PdfArabic pdfArabic = new PdfArabic();
				pdfArabic.arabic_reshape(ref length, Text.ToCharArray(), array, level);
				bstrText = new string(array, 0, length);
				return;
			}
			bstrText = Text;
		}
		private bool IsNextHebrewAr(ref string Text, int nLen, int i)
		{
			bool flag = false;
			int num = i;
			while (num < nLen && Text[num] == ' ')
			{
				num++;
			}
			if (num == nLen - 1)
			{
				return i != 0 && PdfCanvas.IsHebrewAr(Text[i - 1]);
			}
			return PdfCanvas.IsHebrewAr(Text[num]) || flag;
		}
		private void ReverseHebrewArabic(ref string Text)
		{
			char[] array = Text.ToString().ToCharArray();
			int length = Text.Length;
			if (length == 0)
			{
				return;
			}
			PdfCanvas.IsHebrewAr(Text[0]);
			int num = length - 1;
			int i = 0;
			while (i < length)
			{
				char c = Text[i];
				if (c == ')' || c == '(')
				{
					array[num--] = ((c == '(') ? ')' : '(');
					i++;
				}
				else
				{
					if (c == ' ')
					{
						array[num--] = c;
						i++;
					}
					else
					{
						if (PdfCanvas.IsHebrewAr(c) || PdfCanvas.IsNeutralChar(c))
						{
							array[num--] = c;
							i++;
						}
						else
						{
							int num2 = i;
							int num3 = i - 1;
							while (i < length && !PdfCanvas.IsHebrewAr(c) && ((!PdfCanvas.IsNeutralChar(c) && c != ' ') || !this.IsNextHebrewAr(ref Text, length, i)))
							{
								num3++;
								i++;
								if (i < length)
								{
									c = Text[i];
								}
							}
							for (int j = num3; j >= num2; j--)
							{
								array[num--] = Text[j];
							}
						}
					}
				}
			}
			Text = new string(array);
		}
		private static bool IsHebrewAr(char c)
		{
			return (c >= '֐' && c <= '׿') || (c >= '؀' && c <= 'ۿ') || (c >= 'ﭐ' && c <= '﷿') || (c >= 'ﹰ' && c <= '﻿');
		}
		private static bool IsNeutralChar(char c)
		{
			string text = ",.;:'\"/?\\{}[]|+=-_^%&$#@!~\r\n";
			for (int i = 0; i < text.Length; i++)
			{
				if (c == text[i])
				{
					return true;
				}
			}
			return false;
		}
	}
}
