using Microsoft.Win32;
using System;
using System.Collections;
using System.IO;
namespace Persits.PDF
{
	public class PdfFonts : IEnumerable
	{
		internal PdfManager m_pManager;
		internal PdfDocument m_pDoc;
		internal static string[] m_arrFontNames = new string[]
		{
			"Courier",
			"Courier-Bold",
			"Courier-Oblique",
			"Courier-BoldOblique",
			"Helvetica",
			"Helvetica-Bold",
			"Helvetica-Oblique",
			"Helvetica-BoldOblique",
			"Times-Roman",
			"Times-Bold",
			"Times-Italic",
			"Times-BoldItalic",
			"Symbol",
			"ZapfDingbats"
		};
		public PdfFont this[int index]
		{
			get
			{
				if (index < 1 && index > this.m_pDoc.m_ptrFonts.Count)
				{
					AuxException.Throw("Index out of range.", PdfErrors._ERROR_OUTOFRANGE);
				}
				return this.m_pDoc.m_ptrFonts[index];
			}
		}
		public PdfFont this[string Face]
		{
			get
			{
				return this[Face, pdfCharSets.pdfAnsi];
			}
		}
		public PdfFont this[string Face, pdfCharSets CharSet]
		{
			get
			{
				foreach (PdfFont current in this.m_pDoc.m_ptrFonts)
				{
					if (string.Compare(current.Face, Face, true) == 0 && current.CharSet == CharSet)
					{
						PdfFont result = current;
						return result;
					}
				}
				if (CharSet == pdfCharSets.pdfAnsi)
				{
					for (int i = 0; i < PdfFonts.m_arrFontNames.Length; i++)
					{
						if (string.Compare(PdfFonts.m_arrFontNames[i], Face, true) == 0)
						{
							PdfFont pdfFont = new PdfFont();
							pdfFont.m_pManager = this.m_pManager;
							pdfFont.m_pDoc = this.m_pDoc;
							pdfFont.m_bstrFace = PdfFonts.m_arrFontNames[i];
							pdfFont.m_bstrFontName = pdfFont.m_bstrFace;
							switch (i)
							{
							case 1:
							case 2:
							case 3:
								pdfFont.m_bstrFamily = "Courier";
								goto IL_119;
							case 5:
							case 6:
							case 7:
								pdfFont.m_bstrFamily = "Helvetica";
								goto IL_119;
							case 9:
							case 10:
							case 11:
								pdfFont.m_bstrFamily = "Times-Roman";
								goto IL_119;
							}
							pdfFont.m_bstrFamily = pdfFont.m_bstrFace;
							IL_119:
							pdfFont.m_bstrType = "Type1";
							this.m_pDoc.m_ptrFonts.Add(pdfFont);
							pdfFont.SetIndex();
							pdfFont.m_nFirstChar = 0;
							pdfFont.m_nLastChar = 255;
							pdfFont.m_pWidths = new ushort[256];
							Array.Copy(PdfFont.m_arrFontWidths[i], pdfFont.m_pWidths, PdfFont.m_arrFontWidths[i].Length);
							pdfFont.m_nDescent = (int)PdfFont.m_arrDescents[i];
							pdfFont.m_nCapHeight = (uint)PdfFont.m_arrCapHeights[i];
							pdfFont.m_nVerticalExtent = (int)PdfFont.m_arrVerticalExtents[i];
							pdfFont.m_nAscent = (int)(PdfFont.m_arrVerticalExtents[i] + (ushort)PdfFont.m_arrDescents[i]);
							pdfFont.m_rectFontBox.Bottom = (float)pdfFont.m_nDescent;
							pdfFont.m_rectFontBox.Top = (float)pdfFont.m_nAscent;
							if (i == 12 || i == 13)
							{
								pdfFont.m_bDingbatOrSymbol = true;
							}
							return pdfFont;
						}
					}
				}
				foreach (PdfFont current2 in this.m_pDoc.m_ptrFonts)
				{
					if (current2.m_bstrFaceAsObtainedFromTTFFile != null && string.Compare(current2.m_bstrFaceAsObtainedFromTTFFile, Face, true) == 0)
					{
						PdfFont result = current2;
						return result;
					}
				}
				return this.LoadTrueTypeFont(Face);
			}
		}
		public int Count
		{
			get
			{
				return this.m_pDoc.m_ptrFonts.Count;
			}
		}
		internal PdfFonts()
		{
		}
		public IEnumerator GetEnumerator()
		{
			return this.m_pDoc.m_ptrFonts.GetEnumerator();
		}
		internal PdfFont LoadStandardFont(string Face, bool bBold, bool bItalic)
		{
			if (Face.Length <= 0)
			{
				return null;
			}
			string text = null;
			if (string.Compare(Face, "Times Roman", true) == 0 || string.Compare(Face, "Times-Roman", true) == 0)
			{
				text = ((bBold || bItalic) ? "Times-" : "Times-Roman");
				if (bBold)
				{
					text += "Bold";
				}
				if (bItalic)
				{
					text += "Italic";
				}
			}
			else
			{
				if (string.Compare(Face, "Helvetica", true) == 0)
				{
					text = ((bBold || bItalic) ? "Helvetica-" : "Helvetica");
					if (bBold)
					{
						text += "Bold";
					}
					if (bItalic)
					{
						text += "Oblique";
					}
				}
				else
				{
					if (string.Compare(Face, "Courier", true) == 0)
					{
						text = ((bBold || bItalic) ? "Courier-" : "Courier");
						if (bBold)
						{
							text += "Bold";
						}
						if (bItalic)
						{
							text += "Oblique";
						}
					}
					else
					{
						if (string.Compare(Face, "Symbol", true) == 0 && !bBold && !bItalic)
						{
							text = "Symbol";
						}
						else
						{
							if (string.Compare(Face, "ZapfDingbats", true) == 0 && !bBold && !bItalic)
							{
								text = "ZapfDingbats";
							}
						}
					}
				}
			}
			if (text == null)
			{
				return null;
			}
			return this[text];
		}
		internal PdfFont LoadTrueTypeFont(string Face, bool bBold, bool bItalic)
		{
			PdfFonts.BuildInstalledFontTable(this.m_pManager);
			string key = string.Format("{0}{1}{2}", Face, bBold, bItalic).ToLower();
			if (!this.m_pManager.m_mapFontPaths.ContainsKey(key))
			{
				return this.LoadTrueTypeFont(Face);
			}
			string strPath = this.m_pManager.m_mapFontPaths[key];
			return this.LoadFromFile(strPath);
		}
		internal PdfFont LoadTrueTypeFont(string Face)
		{
			string strPath = this.ObtainFontPath(Face);
			return this.LoadFromFile(strPath);
		}
		public PdfFont LoadFromFile(string strPath)
		{
			foreach (PdfFont current in this.m_pDoc.m_ptrFonts)
			{
				if (current.m_bstrPath != null && string.Compare(current.m_bstrPath, strPath, true) == 0)
				{
					return current;
				}
			}
			PdfFont pdfFont = new PdfFont();
			pdfFont.m_pManager = this.m_pManager;
			pdfFont.m_pDoc = this.m_pDoc;
			pdfFont.SetFace(Path.GetFileNameWithoutExtension(strPath));
			pdfFont.m_bstrPath = strPath;
			pdfFont.m_bstrType = "Type0";
			pdfFont.m_bstrEncoding = "Identity-H";
			pdfFont.Parse(strPath, null);
			this.m_pDoc.m_ptrFonts.Add(pdfFont);
			pdfFont.SetIndex();
			pdfFont.SetFace(pdfFont.m_bstrFace);
			pdfFont.m_bParsed = true;
			if (pdfFont.m_bstrFace != null)
			{
				string key = string.Format("{0}{1}{2}", pdfFont.m_bstrFace, (pdfFont.m_nMacStyle & 1) != 0, (pdfFont.m_nMacStyle & 2) != 0).ToLower();
				if (!this.m_pManager.m_mapFontPaths.ContainsKey(key))
				{
					this.m_pManager.m_mapFontPaths.Add(key, strPath);
				}
			}
			return pdfFont;
		}
		internal PdfFont LoadFromResource()
		{
			foreach (PdfFont current in this.m_pDoc.m_ptrFonts)
			{
				if (current.m_bBarcodeFont)
				{
					return current;
				}
			}
			byte[] barcodef = AspPDF.barcodef;
			PdfFont pdfFont = new PdfFont();
			pdfFont.m_pManager = this.m_pManager;
			pdfFont.m_pDoc = this.m_pDoc;
			pdfFont.SetFace("BarcodeFont");
			pdfFont.m_bBarcodeFont = true;
			pdfFont.m_streamBarcodeFont = new PdfStream();
			pdfFont.m_streamBarcodeFont.Set(barcodef);
			pdfFont.m_streamBarcodeFont.DecodeFlate();
			pdfFont.m_bstrType = "Type0";
			pdfFont.m_bstrEncoding = "Identity-H";
			this.m_pDoc.m_ptrFonts.Add(pdfFont);
			pdfFont.SetIndex();
			pdfFont.m_bstrPath = "***";
			pdfFont.Parse(null, pdfFont.m_streamBarcodeFont.ToBytes());
			return pdfFont;
		}
		internal string ObtainFontPath(string Face)
		{
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Fonts");
			string arg = Environment.GetEnvironmentVariable("windir") + "\\Fonts\\";
			string[] valueNames = registryKey.GetValueNames();
			string[] array = valueNames;
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				string strA = text;
				int num = text.IndexOf('(');
				if (num > 0)
				{
					strA = text.Substring(0, num - 1);
				}
				if (string.Compare(strA, Face, true) == 0)
				{
					return arg + registryKey.GetValue(text);
				}
			}
			registryKey.Close();
			AuxException.Throw(string.Format("Font \"{0}\" could not be found.", Face), PdfErrors._ERROR_FONTFILENAME);
			return "";
		}
		internal static void BuildInstalledFontTable(PdfManager pManager)
		{
			if (pManager.m_bFontTableBuilt)
			{
				return;
			}
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Fonts");
			string arg = Environment.GetEnvironmentVariable("windir") + "\\Fonts\\";
			string[] valueNames = registryKey.GetValueNames();
			PdfFont pdfFont = new PdfFont();
			string[] array = valueNames;
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				int num = text.IndexOf("(TrueType)");
				if (num > 0)
				{
					text.Substring(0, num - 1);
					string text2 = arg + registryKey.GetValue(text);
					pdfFont.m_bstrFamily = null;
					pdfFont.m_bstrFace = null;
					pdfFont.ParseToDetermineBoldItalic(text2);
					string bstrFamily = pdfFont.m_bstrFamily;
					if (bstrFamily != null)
					{
						string key = string.Format("{0}{1}{2}", bstrFamily, (pdfFont.m_nMacStyle & 1) != 0, (pdfFont.m_nMacStyle & 2) != 0).ToLower();
						if (!pManager.m_mapFontPaths.ContainsKey(key))
						{
							pManager.m_mapFontPaths.Add(key, text2);
						}
					}
				}
			}
			registryKey.Close();
			pManager.m_bFontTableBuilt = true;
		}
	}
}
