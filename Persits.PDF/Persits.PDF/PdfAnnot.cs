using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
namespace Persits.PDF
{
	public class PdfAnnot
	{
		internal const int MAX_STATE_COUNT = 2;
		internal static string[] m_arrTypes = new string[]
		{
			"Text",
			"Link",
			"FreeText",
			"Line",
			"Square",
			"Circle",
			"Highlight",
			"Underline",
			"StrikeOut",
			"Squiggly",
			"Stamp",
			"FileAttachment",
			"Sound",
			"Movie",
			"Widget"
		};
		internal PdfManager m_pManager;
		internal PdfPage m_pPage;
		internal PdfDocument m_pDoc;
		internal PdfIndirectObj m_pAnnotObj;
		internal PdfIndirectObj m_pFilespecObj;
		internal PdfIndirectObj m_pEmbeddedFileObj;
		internal string m_bstrType;
		internal string m_bstrContents;
		internal string m_bstrName;
		internal string m_bstrFieldType;
		internal string m_bstrFieldName;
		internal string m_bstrFieldAlternateName;
		internal string m_bstrFieldValue;
		internal string m_bstrFieldDefaultValue;
		internal string m_bstrFieldActiveState;
		internal string m_bstrFieldOnValue;
		internal string m_bstrFieldDefaultAppearance;
		internal float m_fX1;
		internal float m_fY1;
		internal float m_fX2;
		internal float m_fY2;
		internal int m_nIndex;
		internal bool m_bExisting;
		internal DateTime m_dtModified;
		internal bool m_bActionSet;
		internal bool m_bDestSet;
		internal int m_nFieldFlags;
		internal int m_nRotate;
		internal int m_nFlags;
		internal float m_fBorderWidth;
		internal int m_nAlignment;
		internal bool m_bIsPushbutton;
		internal bool m_bIsCheckbox;
		internal bool m_bIsChoice;
		internal bool m_bIsRadiobutton;
		internal bool m_bIsText;
		internal int m_nFontSize;
		internal PdfGraphics[][] m_arrGraphics;
		internal string[] m_arrStateNames;
		internal Dictionary<string, string> m_mapOptions = new Dictionary<string, string>();
		internal List<PdfAnnot> m_arrKids = new List<PdfAnnot>();
		internal PdfDict m_ptrApperanceDict;
		internal List<string> m_arrDisplayValues = new List<string>();
		public string Type
		{
			get
			{
				return this.m_bstrType;
			}
		}
		public int Index
		{
			get
			{
				return this.m_nIndex;
			}
		}
		public string Name
		{
			get
			{
				return this.m_bstrName;
			}
			set
			{
				this.m_bstrName = value;
				this.m_pAnnotObj.AddName("Name", this.m_bstrName);
				this.m_pAnnotObj.m_bModified = true;
			}
		}
		public DateTime Modified
		{
			get
			{
				return this.m_dtModified;
			}
			set
			{
				this.m_dtModified = value;
				this.m_pAnnotObj.AddDate("M", this.m_dtModified);
				this.m_pAnnotObj.m_bModified = true;
			}
		}
		private float Width
		{
			get
			{
				if (this.m_nRotate != 0 && this.m_nRotate != 180)
				{
					return Math.Abs(this.m_fY2 - this.m_fY1);
				}
				return Math.Abs(this.m_fX2 - this.m_fX1);
			}
		}
		private float Height
		{
			get
			{
				if (this.m_nRotate != 0 && this.m_nRotate != 180)
				{
					return Math.Abs(this.m_fX2 - this.m_fX1);
				}
				return Math.Abs(this.m_fY2 - this.m_fY1);
			}
		}
		private float Size
		{
			get
			{
				if (this.Width <= this.Height)
				{
					return this.Width;
				}
				return this.Height;
			}
		}
		public string FieldOnValue
		{
			get
			{
				return this.m_bstrFieldOnValue;
			}
		}
		public string FieldType
		{
			get
			{
				return this.m_bstrFieldType;
			}
			set
			{
				this.m_bstrFieldType = value;
			}
		}
		public int FieldFlags
		{
			get
			{
				return this.m_nFieldFlags;
			}
			set
			{
				this.m_nFieldFlags = value;
				this.m_pAnnotObj.AddInt("Ff", value);
				this.m_pAnnotObj.m_bModified = true;
			}
		}
		public int Flags
		{
			get
			{
				return this.m_nFlags;
			}
			set
			{
				this.m_nFlags = value;
				this.m_pAnnotObj.AddInt("F", value);
				this.m_pAnnotObj.m_bModified = true;
			}
		}
		public string FieldName
		{
			get
			{
				return this.m_bstrFieldName;
			}
			set
			{
				this.m_bstrFieldName = value;
				this.m_pAnnotObj.AddString("T", value);
				this.m_pAnnotObj.m_bModified = true;
			}
		}
		public string FieldAlternateName
		{
			get
			{
				return this.m_bstrFieldAlternateName;
			}
			set
			{
				this.m_bstrFieldAlternateName = value;
				this.m_pAnnotObj.AddString("TU", value);
				this.m_pAnnotObj.m_bModified = true;
			}
		}
		public string FieldValue
		{
			get
			{
				return this.m_bstrFieldValue;
			}
			set
			{
				this.m_bstrFieldValue = value;
				if (this.m_bIsRadiobutton || this.m_bIsCheckbox)
				{
					this.m_pAnnotObj.AddName("V", value);
				}
				else
				{
					this.m_pAnnotObj.AddString("V", value);
				}
				this.m_pAnnotObj.m_bModified = true;
			}
		}
		public string FieldDefaultValue
		{
			get
			{
				return this.m_bstrFieldDefaultValue;
			}
			set
			{
				this.m_bstrFieldDefaultValue = value;
				if (this.m_bIsRadiobutton || this.m_bIsCheckbox)
				{
					this.m_pAnnotObj.AddName("DV", value);
				}
				else
				{
					this.m_pAnnotObj.AddString("DV", value);
				}
				this.m_pAnnotObj.m_bModified = true;
			}
		}
		public string FieldActiveState
		{
			get
			{
				return this.m_bstrFieldActiveState;
			}
			set
			{
				this.m_bstrFieldActiveState = value;
				this.m_pAnnotObj.AddName("AS", value);
				this.m_pAnnotObj.m_bModified = true;
			}
		}
		public string Contents
		{
			get
			{
				return this.m_bstrContents;
			}
			set
			{
				this.m_bstrContents = value;
				if (this.m_bstrContents != null)
				{
					this.m_pAnnotObj.AddString("Contents", this.m_bstrContents);
				}
				else
				{
					this.m_pAnnotObj.m_objAttributes.RemoveByName("Contents");
				}
				this.m_pAnnotObj.m_bModified = true;
			}
		}
		public PdfGraphicsCollection Graphics
		{
			get
			{
				return new PdfGraphicsCollection
				{
					m_pAnnot = this
				};
			}
		}
		public PdfAnnot Parent
		{
			get
			{
				if (!this.m_bExisting)
				{
					AuxException.Throw("The Parent property is not implemented for new annotations.", PdfErrors._ERROR_INVALIDARG);
				}
				PdfObject objectByName = this.m_pAnnotObj.m_objAttributes.GetObjectByName("Parent");
				if (objectByName == null)
				{
					return null;
				}
				if (objectByName.m_nType == enumType.pdfRef)
				{
					return this.m_pDoc.FindAnnot((PdfRef)objectByName);
				}
				return null;
			}
		}
		public PdfAnnots Children
		{
			get
			{
				if (!this.m_bExisting)
				{
					AuxException.Throw("The Children property is not implemented for new annotations.", PdfErrors._ERROR_INVALIDARG);
				}
				this.ClearKidsCollection();
				PdfAnnots pdfAnnots = new PdfAnnots();
				pdfAnnots.m_bFormFields = true;
				pdfAnnots.m_pManager = this.m_pManager;
				pdfAnnots.m_pDoc = this.m_pDoc;
				pdfAnnots.m_pPage = null;
				pdfAnnots.m_parrAnnots = this.m_arrKids;
				PdfObject objectByName = this.m_pAnnotObj.m_objAttributes.GetObjectByName("Kids");
				if (objectByName == null)
				{
					return pdfAnnots;
				}
				if (objectByName.m_nType != enumType.pdfArray)
				{
					return pdfAnnots;
				}
				PdfArray pdfArray = (PdfArray)objectByName;
				for (int i = 0; i < pdfArray.Size; i++)
				{
					PdfObject @object = pdfArray.GetObject(i);
					if (@object.m_nType == enumType.pdfRef)
					{
						PdfAnnot pdfAnnot = this.m_pDoc.FindAnnot((PdfRef)@object);
						if (pdfAnnot == null)
						{
							if (this.m_pDoc.m_pInput != null)
							{
								try
								{
									this.m_pDoc.m_pInput.ParseFormField((PdfRef)@object, pdfArray);
								}
								catch (Exception ex)
								{
									this.m_pDoc.m_pInput.Close();
									throw ex;
								}
							}
							pdfAnnot = this.m_pDoc.FindAnnot((PdfRef)@object);
						}
						if (pdfAnnot != null)
						{
							this.m_arrKids.Add(pdfAnnot);
							if (pdfAnnot.m_pAnnotObj != null && pdfAnnot.m_pAnnotObj.m_objAttributes.GetObjectByName("Ff") == null)
							{
								pdfAnnot.m_nFieldFlags = this.m_nFieldFlags;
								pdfAnnot.m_bIsPushbutton = ((this.m_nFieldFlags & 65536) > 0);
								if (this.m_bstrFieldType != null && string.Compare(this.m_bstrFieldType, "Btn") == 0)
								{
									pdfAnnot.m_bIsRadiobutton = (!this.m_bIsPushbutton && (this.m_nFieldFlags & 32768) > 0);
									pdfAnnot.m_bIsCheckbox = (!this.m_bIsPushbutton && (this.m_nFieldFlags & 32768) == 0);
								}
							}
							PdfObject objectByName2 = this.m_pAnnotObj.m_objAttributes.GetObjectByName("DA");
							if (objectByName2 != null && objectByName2.m_nType == enumType.pdfString && pdfAnnot.m_pAnnotObj != null && pdfAnnot.m_pAnnotObj.m_objAttributes.GetObjectByName("DA") == null)
							{
								pdfAnnot.m_pAnnotObj.m_objAttributes.Add(((PdfString)objectByName2).Copy());
							}
						}
					}
				}
				return pdfAnnots;
			}
		}
		public PdfRect Rect
		{
			get
			{
				PdfRect pdfRect = new PdfRect();
				pdfRect.Set(this.m_fX1, this.m_fY1, this.m_fX2, this.m_fY2);
				return pdfRect;
			}
		}
		internal PdfAnnot()
		{
			this.m_fX1 = (this.m_fY1 = (this.m_fX2 = (this.m_fY2 = 0f)));
			this.m_nIndex = 0;
			this.m_pAnnotObj = (this.m_pFilespecObj = (this.m_pEmbeddedFileObj = null));
			this.m_bExisting = false;
			this.m_dtModified = DateTime.MinValue;
			this.m_bActionSet = (this.m_bDestSet = false);
			this.m_nFontSize = 10;
			this.m_nFieldFlags = 0;
			this.m_nFlags = 0;
			this.m_nAlignment = -1;
			this.m_bIsPushbutton = (this.m_bIsCheckbox = (this.m_bIsChoice = (this.m_bIsRadiobutton = (this.m_bIsText = false))));
			this.m_nRotate = 0;
			this.m_fBorderWidth = 1f;
			this.m_arrGraphics = new PdfGraphics[3][];
			for (int i = 0; i <= 2; i++)
			{
				this.m_arrGraphics[i] = new PdfGraphics[2];
			}
			this.m_arrStateNames = new string[2];
		}
		internal void Create(string strContents, PdfParam pParam, string Name, string Path)
		{
			if (!pParam.IsSet("X") || !pParam.IsSet("Y") || !pParam.IsSet("Width") || !pParam.IsSet("Height"))
			{
				AuxException.Throw("The following parameters are required: X, Y, Width, Height.", PdfErrors._ERROR_INVALIDARG);
			}
			float num = pParam.Number("X");
			float num2 = pParam.Number("Y");
			float num3 = pParam.Number("Width");
			float num4 = pParam.Number("Height");
			if (this.m_pPage.m_nRotate != 0)
			{
				this.m_nRotate = this.m_pPage.m_nRotate;
				if (this.m_nRotate == 90 || this.m_nRotate == 270)
				{
					float num5 = num4;
					num4 = num3;
					num3 = num5;
				}
				if (this.m_nRotate == 90)
				{
					float num5 = num;
					num = this.m_pPage.Width - num2 - num3;
					num2 = num5;
				}
				if (this.m_nRotate == 270)
				{
					float num5 = num2;
					num2 = this.m_pPage.Height - num - num4;
					num = num5;
				}
				if (this.m_nRotate == 180)
				{
					num = this.m_pPage.Width - num - num3;
					num2 = this.m_pPage.Height - num2 - num4;
				}
			}
			this.m_fX1 = num;
			this.m_fY1 = num2;
			this.m_fX2 = num + num3;
			this.m_fY2 = num2 + num4;
			AnnotType annotType = AnnotType.AnnotText;
			this.m_bstrType = "Text";
			if (pParam.IsSet("Type"))
			{
				annotType = (AnnotType)pParam.Long("Type");
				if (annotType < AnnotType.AnnotText || annotType >= (AnnotType)PdfAnnot.m_arrTypes.Length)
				{
					AuxException.Throw("Type parameter out of range.", PdfErrors._ERROR_OUTOFRANGE);
				}
				this.m_bstrType = PdfAnnot.m_arrTypes[(int)annotType];
			}
			this.m_pAnnotObj = this.m_pDoc.AddNewIndirectObject(enumIndirectType.pdfIndirectAnnot);
			this.m_pAnnotObj.AddName("Type", "Annot");
			this.m_pAnnotObj.AddName("Subtype", this.m_bstrType);
			PdfArray pdfArray = this.m_pAnnotObj.AddArray("Rect");
			pdfArray.Add(new PdfNumber(null, (double)num));
			pdfArray.Add(new PdfNumber(null, (double)num2));
			pdfArray.Add(new PdfNumber(null, (double)(num + num3)));
			pdfArray.Add(new PdfNumber(null, (double)(num2 + num4)));
			this.Contents = strContents;
			uint num6 = 0u;
			if (pParam.IsTrue("Invisible"))
			{
				num6 |= 1u;
			}
			if (pParam.IsTrue("Hidden"))
			{
				num6 |= 2u;
			}
			if (pParam.IsTrue("Print"))
			{
				num6 |= 4u;
			}
			if (pParam.IsTrue("NoZoom"))
			{
				num6 |= 8u;
			}
			if (pParam.IsTrue("NoRotate"))
			{
				num6 |= 16u;
			}
			if (pParam.IsTrue("NoView"))
			{
				num6 |= 32u;
			}
			if (pParam.IsTrue("ReadOnly"))
			{
				num6 |= 64u;
			}
			if (num6 > 0u)
			{
				this.m_pAnnotObj.AddInt("F", (int)num6);
			}
			this.m_pAnnotObj.AddReference("P", this.m_pPage.m_pPageObj);
			if (pParam.IsSet("Color"))
			{
				AuxRGB auxRGB = new AuxRGB();
				auxRGB.Set((uint)pParam.Long("Color"));
				PdfArray pdfArray2 = this.m_pAnnotObj.AddArray("C");
				pdfArray2.Add(new PdfNumber(null, (double)auxRGB.r));
				pdfArray2.Add(new PdfNumber(null, (double)auxRGB.g));
				pdfArray2.Add(new PdfNumber(null, (double)auxRGB.b));
			}
			if (pParam.IsSet("Border") || pParam.IsSet("BorderRadius"))
			{
				int num7 = 1;
				int num8 = 0;
				if (pParam.IsSet("Border"))
				{
					num7 = pParam.Long("Border");
				}
				if (pParam.IsSet("BorderRadius"))
				{
					num8 = pParam.Long("BorderRadius");
				}
				PdfArray pdfArray3 = this.m_pAnnotObj.AddArray("Border");
				pdfArray3.Add(new PdfNumber(null, (double)num8));
				pdfArray3.Add(new PdfNumber(null, (double)num8));
				pdfArray3.Add(new PdfNumber(null, (double)num7));
			}
			if (this.m_nRotate != 0)
			{
				PdfDict pdfDict = this.m_pAnnotObj.AddDict("MK");
				pdfDict.Add(new PdfNumber("R", (double)this.m_nRotate));
			}
			switch (annotType)
			{
			case AnnotType.AnnotText:
				this.HandleText(pParam, Name);
				return;
			case AnnotType.AnnotLink:
				this.HandleLink(pParam);
				return;
			case AnnotType.AnnotFreeText:
				this.HandleFreeText(pParam);
				return;
			case AnnotType.AnnotLine:
				this.HandleLine(pParam);
				return;
			case AnnotType.AnnotSquare:
			case AnnotType.AnnotCircle:
				this.HandleSquare(pParam);
				return;
			case AnnotType.AnnotHighlight:
			case AnnotType.AnnotUnderline:
			case AnnotType.AnnotStrikeOut:
			case AnnotType.AnnotSquiggly:
				this.HandleHighlight(pParam);
				return;
			case AnnotType.AnnotStamp:
				this.HandleStamp(pParam, Name);
				return;
			case AnnotType.AnnotFileAttachment:
				this.HandleFileAttachment(pParam, "FS", Name, Path);
				return;
			case AnnotType.AnnotSound:
				this.HandleSound(pParam, Name, Path);
				return;
			case AnnotType.AnnotMovie:
				this.HandleMovie(pParam, Name, Path);
				return;
			default:
				return;
			}
		}
		private void HandleText(PdfParam pParam, string bstrArg)
		{
			if (pParam.IsTrue("Open"))
			{
				this.m_pAnnotObj.AddBool("Open", true);
			}
			string[] array = new string[]
			{
				"Comment",
				"Help",
				"Insert",
				"Key",
				"NewParagraph",
				"Note",
				"Paragraph"
			};
			if (bstrArg != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (string.Compare(bstrArg, array[i], true) == 0)
					{
						this.m_pAnnotObj.AddName("Name", array[i]);
						this.m_bstrName = array[i];
						return;
					}
				}
			}
		}
		private void HandleLink(PdfParam pParam)
		{
			if (pParam.IsSet("HighlightMode"))
			{
				string value;
				switch (pParam.Long("HighlightMode"))
				{
				case 1:
					value = "I";
					goto IL_51;
				case 2:
					value = "O";
					goto IL_51;
				case 3:
					value = "P";
					goto IL_51;
				}
				value = "N";
				IL_51:
				this.m_pAnnotObj.AddName("H", value);
			}
		}
		private void HandleFreeText(PdfParam pParam)
		{
			if (pParam.IsSet("Alignment"))
			{
				int num = pParam.Long("Alignment");
				if (num < 0)
				{
					num = 0;
				}
				if (num > 2)
				{
					num = 2;
				}
				if (num == 1)
				{
					num = 2;
				}
				else
				{
					if (num == 2)
					{
						num = 1;
					}
				}
				this.m_pAnnotObj.AddInt("Q", num);
			}
		}
		private void HandleLine(PdfParam pParam)
		{
			if (pParam.IsSet("X1") && pParam.IsSet("X2") && pParam.IsSet("Y1") && pParam.IsSet("Y2"))
			{
				float num = pParam.Number("X1");
				float num2 = pParam.Number("Y1");
				float num3 = pParam.Number("X2");
				float num4 = pParam.Number("Y2");
				PdfArray pdfArray = this.m_pAnnotObj.AddArray("");
				pdfArray.Add(new PdfNumber(null, (double)num));
				pdfArray.Add(new PdfNumber(null, (double)num2));
				pdfArray.Add(new PdfNumber(null, (double)num3));
				pdfArray.Add(new PdfNumber(null, (double)num4));
			}
			else
			{
				AuxException.Throw("You must specify X1, Y1, X2, Y2 for a Line annotation.", PdfErrors._ERROR_INVALIDARG);
			}
			string[] array = new string[]
			{
				"None",
				"Square",
				"Circle",
				"Diamond",
				"OpenArrow",
				"ClosedArrow"
			};
			int num5 = 0;
			int num6 = 0;
			if (pParam.IsSet("Beginning"))
			{
				num5 = pParam.Long("Beginning");
			}
			if (pParam.IsSet("Ending"))
			{
				num6 = pParam.Long("Ending");
			}
			if (num5 < 0 || num5 > 5 || num6 < 0 || num6 > 5)
			{
				AuxException.Throw("Beginning and Ending parameters must be set to one of the following values: 0 (None), 1 (Square), 2 (Circle), 3 (Diamond), 4 (Open Arrow), 5 (Closed Arrow).", PdfErrors._ERROR_INVALIDARG);
			}
			PdfArray pdfArray2 = this.m_pAnnotObj.AddArray("LE");
			pdfArray2.Add(new PdfName(null, array[num5]));
			pdfArray2.Add(new PdfName(null, array[num6]));
			this.HandleInnerColor(pParam);
			this.HandleBorderStyle(pParam);
		}
		private void HandleSquare(PdfParam pParam)
		{
			this.HandleInnerColor(pParam);
			this.HandleBorderStyle(pParam);
		}
		private void HandleHighlight(PdfParam pParam)
		{
			if (pParam.IsSet("X1") && pParam.IsSet("Y1") && pParam.IsSet("X2") && pParam.IsSet("Y2") && pParam.IsSet("X3") && pParam.IsSet("Y3") && pParam.IsSet("X4") && pParam.IsSet("Y4"))
			{
				float num = pParam.Number("X1");
				float num2 = pParam.Number("Y1");
				float num3 = pParam.Number("X2");
				float num4 = pParam.Number("Y2");
				float num5 = pParam.Number("X3");
				float num6 = pParam.Number("Y3");
				float num7 = pParam.Number("X4");
				float num8 = pParam.Number("Y4");
				PdfArray pdfArray = this.m_pAnnotObj.AddArray("QuadPoints");
				pdfArray.Add(new PdfNumber(null, (double)num));
				pdfArray.Add(new PdfNumber(null, (double)num2));
				pdfArray.Add(new PdfNumber(null, (double)num3));
				pdfArray.Add(new PdfNumber(null, (double)num4));
				pdfArray.Add(new PdfNumber(null, (double)num5));
				pdfArray.Add(new PdfNumber(null, (double)num6));
				pdfArray.Add(new PdfNumber(null, (double)num7));
				pdfArray.Add(new PdfNumber(null, (double)num8));
				return;
			}
			AuxException.Throw("You must specify X1, Y1, X2, Y2, X3, Y3, X4, Y4 for a Highlight, Underline, Squggly, and StrikeOut annotations.", PdfErrors._ERROR_INVALIDARG);
		}
		private void HandleStamp(PdfParam pParam, string bstrName)
		{
			string[] array = new string[]
			{
				"Approved",
				"AsIs",
				"Confidential",
				"Departmental",
				"Draft",
				"Experimental",
				"Expired",
				"Final",
				"ForComment",
				"ForPublicRelease",
				"NotApproved",
				"NotForPublicRelease",
				"Sold",
				"TopSecret"
			};
			if (bstrName != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (string.Compare(bstrName, array[i], true) == 0)
					{
						this.m_pAnnotObj.AddName("Name", array[i]);
						this.m_bstrName = array[i];
						return;
					}
				}
			}
		}
		private void HandleFileAttachment(PdfParam pParam, string DictName, string bstrName, string Path)
		{
			if (Path == null)
			{
				AuxException.Throw("Path for file annotation not specified.", PdfErrors._ERROR_INVALIDARG);
			}
			AuxFile auxFile = new AuxFile();
			auxFile.Open(Path);
			this.m_pEmbeddedFileObj = this.m_pDoc.AddNewIndirectObject(enumIndirectType.pdfIndirectEmbeddedFile);
			this.m_pEmbeddedFileObj.AddName("Type", "EmbeddedFile");
			byte[] array = new byte[auxFile.Size];
			auxFile.Read(array);
			this.m_pEmbeddedFileObj.m_objStream.Set(array);
			int num = array.Length;
			PdfDict pdfDict = this.m_pEmbeddedFileObj.AddDict("Params");
			pdfDict.Add(new PdfNumber("Size", (double)num));
			PdfHash pdfHash = new PdfHash();
			pdfHash.Compute(this.m_pEmbeddedFileObj.m_objStream);
			pdfDict.Add(new PdfString("CheckSum", pdfHash));
			DateTime dtDate;
			DateTime dtDate2;
			auxFile.GetFileDates(out dtDate, out dtDate2);
			pdfDict.Add(new PdfDate("CreationDate", dtDate));
			pdfDict.Add(new PdfDate("ModDate", dtDate2));
			int num2 = this.m_pEmbeddedFileObj.m_objStream.Encode(enumEncoding.PdfEncFlate);
			if (num2 != 0)
			{
				AuxException.Throw("Compression of file attachment failed.", PdfErrors._ERROR_COMPRESSIONFAILED);
			}
			this.m_pEmbeddedFileObj.AddName("Filter", "FlateDecode");
			this.m_pEmbeddedFileObj.AddInt("Length", this.m_pEmbeddedFileObj.m_objStream.Length);
			this.m_pFilespecObj = this.m_pDoc.AddNewIndirectObject(enumIndirectType.pdfIndirectFilespec);
			this.m_pFilespecObj.AddName("Type", "Filespec");
			string text = auxFile.ExtractFileName();
			text = AuxFile.ConvertToAnsi(text);
			this.m_pFilespecObj.AddString("F", text);
			PdfDict pdfDict2 = this.m_pFilespecObj.AddDict("EF");
			pdfDict2.Add(new PdfReference("F", this.m_pEmbeddedFileObj));
			this.m_pAnnotObj.AddReference(DictName, this.m_pFilespecObj);
			string[] array2 = new string[]
			{
				"Graph",
				"Paperclip",
				"PushPin",
				"Tag"
			};
			if (bstrName != null)
			{
				for (int i = 0; i < array2.Length; i++)
				{
					if (string.Compare(bstrName, array2[i], true) == 0)
					{
						this.m_pAnnotObj.AddName("Name", array2[i]);
						this.m_bstrName = array2[i];
						return;
					}
				}
			}
		}
		private void HandleSound(PdfParam pParam, string bstrArg, string Path)
		{
			AuxException.Throw("Sound annotation not currently supported.", PdfErrors._ERROR_INVALIDARG);
		}
		private void HandleMovie(PdfParam pParam, string bstrArg, string Path)
		{
			AuxException.Throw("Movie annotation not currently supported.", PdfErrors._ERROR_INVALIDARG);
		}
		private void HandleBorderStyle(PdfParam pParam)
		{
			if (pParam.IsSet("LineWidth") || pParam.IsSet("Style"))
			{
				float num = 1f;
				if (pParam.IsSet("LineWidth"))
				{
					num = pParam.Number("LineWidth");
				}
				int num2 = 0;
				if (pParam.IsSet("LineStyle"))
				{
					num2 = pParam.Long("LineStyle");
				}
				if (num2 < 0 || num2 > 4)
				{
					num2 = 0;
				}
				string[] array = new string[]
				{
					"S",
					"D",
					"B",
					"I",
					"U"
				};
				PdfDict pdfDict = this.m_pAnnotObj.AddDict("BS");
				pdfDict.Add(new PdfNumber("W", (double)num));
				pdfDict.Add(new PdfName("S", array[num2]));
				if (pParam.IsSet("Dash1"))
				{
					PdfArray pdfArray = new PdfArray("D");
					pdfArray.Add(new PdfNumber(null, (double)pParam.Number("Dash1")));
					if (pParam.IsSet("Dash2"))
					{
						pdfArray.Add(new PdfNumber(null, (double)pParam.Number("Dash2")));
					}
					pdfDict.Add(pdfArray);
				}
			}
		}
		private void HandleInnerColor(PdfParam pParam)
		{
			if (pParam.IsSet("InnerColor"))
			{
				AuxRGB auxRGB = new AuxRGB();
				auxRGB.Set((uint)pParam.Long("InnerColor"));
				PdfArray pdfArray = this.m_pAnnotObj.AddArray("IC");
				pdfArray.Add(new PdfNumber(null, (double)auxRGB.r));
				pdfArray.Add(new PdfNumber(null, (double)auxRGB.g));
				pdfArray.Add(new PdfNumber(null, (double)auxRGB.b));
			}
		}
		public void SetDest(PdfDest Dest)
		{
			if (this.m_bActionSet)
			{
				AuxException.Throw("Destination cannot be set if Action is already set.", PdfErrors._ERROR_INVALIDARG);
			}
			if (Dest == null)
			{
				AuxException.Throw("Dest argument is empty.", PdfErrors._ERROR_INVALIDARG);
			}
			if (this.m_bstrType == "Link")
			{
				PdfArray pArray = this.m_pAnnotObj.AddArray("Dest");
				Dest.Populate(pArray, false);
				this.m_bDestSet = true;
				return;
			}
			AuxException.Throw("A destination may only be set on a Link annotation.", PdfErrors._ERROR_INVALIDARG);
		}
		public void SetAction(PdfAction Action)
		{
			if (this.m_bDestSet)
			{
				AuxException.Throw("Action cannot be set if Destination is already set.", PdfErrors._ERROR_INVALIDARG);
			}
			if (Action == null)
			{
				AuxException.Throw("Destination argument is empty.", PdfErrors._ERROR_INVALIDARG);
			}
			if (!Action.IsValid)
			{
				AuxException.Throw("This type of Action requires a destination.", PdfErrors._ERROR_INVALIDARG);
			}
			this.m_pAnnotObj.AddReference("A", Action.m_pActionObj);
			this.m_bActionSet = true;
		}
		internal PdfGraphics GetGraphics(int Type, string State)
		{
			int num;
			this.ValidateGraphicsArgs(Type, State, out num);
			if (this.m_arrGraphics[Type][num] == null)
			{
				PdfParam pdfParam = new PdfParam();
				pdfParam["Left"] = 0f;
				pdfParam["Bottom"] = 0f;
				pdfParam["Right"] = this.Width;
				pdfParam["Top"] = this.Height;
				if (this.m_nRotate != 0)
				{
					float value = 1f;
					float value2 = 0f;
					float value3 = 0f;
					float value4 = 1f;
					float value5 = 0f;
					float value6 = 0f;
					int nRotate = this.m_nRotate;
					if (nRotate != 90)
					{
						if (nRotate != 180)
						{
							if (nRotate == 270)
							{
								value = 0f;
								value2 = -1f;
								value3 = 1f;
								value4 = 0f;
								value5 = 0f;
								value6 = Math.Abs(this.m_fY2 - this.m_fY1);
							}
						}
						else
						{
							value = -1f;
							value2 = 0f;
							value3 = 0f;
							value4 = -1f;
							value5 = Math.Abs(this.m_fX2 - this.m_fX1);
							value6 = Math.Abs(this.m_fY2 - this.m_fY1);
						}
					}
					else
					{
						value = 0f;
						value2 = 1f;
						value3 = -1f;
						value4 = 0f;
						value5 = Math.Abs(this.m_fX2 - this.m_fX1);
						value6 = 0f;
					}
					pdfParam["a"] = value;
					pdfParam["b"] = value2;
					pdfParam["c"] = value3;
					pdfParam["d"] = value4;
					pdfParam["e"] = value5;
					pdfParam["f"] = value6;
				}
				PdfGraphics pGraph = this.m_pDoc.CreateGraphics(pdfParam);
				this.PutGraphics(Type, State, pGraph);
			}
			return this.m_arrGraphics[Type][num];
		}
		internal void PutGraphics(int Type, string State, PdfGraphics pGraph)
		{
			if (pGraph == null)
			{
				AuxException.Throw("Empty Graphics object specified.", PdfErrors._ERROR_INVALIDARG);
			}
			int num;
			this.ValidateGraphicsArgs(Type, State, out num);
			this.m_arrGraphics[Type][num] = pGraph;
			string[] array = new string[]
			{
				"N",
				"D",
				"R"
			};
			PdfDict pdfDict = this.m_pAnnotObj.AddDict("AP");
			this.m_pAnnotObj.m_bModified = true;
			for (int i = 0; i < 3; i++)
			{
				if (this.m_arrStateNames[0] == null)
				{
					pdfDict.Add(new PdfReference(array[Type], pGraph.m_pXObj));
				}
				else
				{
					PdfObject objectByName = pdfDict.GetObjectByName(array[Type]);
					if (objectByName == null)
					{
						PdfDict pdfDict2 = new PdfDict(array[Type]);
						pdfDict2.Add(new PdfReference(this.m_arrStateNames[num], pGraph.m_pXObj));
						pdfDict.Add(pdfDict2);
					}
					else
					{
						if (objectByName.m_nType != enumType.pdfDictionary)
						{
							return;
						}
						((PdfDict)objectByName).Add(new PdfReference(this.m_arrStateNames[num], pGraph.m_pXObj));
					}
				}
			}
		}
		internal void ValidateGraphicsArgs(int Type, string State, out int pState)
		{
			if (Type < 0 || Type > 2)
			{
				AuxException.Throw("Valid values for Type argument are 0 (Normal), 1 (Down) and 2 (Rollover).", PdfErrors._ERROR_OUTOFRANGE);
			}
			pState = 0;
			if (State == null || State.Length == 0)
			{
				return;
			}
			for (int i = 0; i < 2; i++)
			{
				if (this.m_arrStateNames[i] != null && this.m_arrStateNames[i] == State)
				{
					pState = i;
					return;
				}
			}
			for (int j = 0; j < 2; j++)
			{
				if (this.m_arrStateNames[j] == null)
				{
					this.m_arrStateNames[j] = State;
					pState = j;
					return;
				}
			}
			AuxException.Throw("State name not found.", PdfErrors._ERROR_OUTOFRANGE);
		}
		internal PdfCanvas AddEmptyGraphics(int nType, string State)
		{
			PdfGraphics graphics = this.GetGraphics(nType, State);
			return graphics.Canvas;
		}
		private void ClearKidsCollection()
		{
			this.m_arrKids.Clear();
		}
		public void SetRect(float X, float Y, float Width, float Height)
		{
			this.m_pAnnotObj.m_objAttributes.RemoveByName("Rect");
			PdfArray pdfArray = this.m_pAnnotObj.AddArray("Rect");
			pdfArray.Add(new PdfNumber(null, (double)X));
			pdfArray.Add(new PdfNumber(null, (double)Y));
			pdfArray.Add(new PdfNumber(null, (double)(X + Width)));
			pdfArray.Add(new PdfNumber(null, (double)(Y + Height)));
			this.m_fX1 = X;
			this.m_fY1 = Y;
			this.m_fX2 = X + Width;
			this.m_fY2 = Y + Height;
			this.m_pAnnotObj.m_bModified = true;
		}
		public void SetFieldValue(string Text, PdfFont Font)
		{
			if (!this.m_bExisting)
			{
				AuxException.Throw("This method can only be called on existing documents.", PdfErrors._ERROR_INVALIDARG);
			}
			if (this.m_bIsRadiobutton || this.m_bIsCheckbox)
			{
				this.m_pAnnotObj.AddName("AS", Text);
			}
			if (this.m_bIsRadiobutton || this.m_bIsCheckbox)
			{
				this.FieldValue = Text;
				this.FieldDefaultValue = Text;
				return;
			}
			if ((this.m_nFieldFlags & 131072) != 0)
			{
				this.m_pAnnotObj.m_objAttributes.RemoveByName("I");
			}
			bool flag = (this.m_nFieldFlags & 16777216) != 0;
			int num = 0;
			if (flag)
			{
				PdfObject objectByName = this.m_pAnnotObj.m_objAttributes.GetObjectByName("MaxLen");
				if (objectByName != null && objectByName.m_nType == enumType.pdfNumber)
				{
					num = (int)((PdfNumber)objectByName).m_fValue;
					if (num <= 0)
					{
						flag = false;
					}
				}
				else
				{
					flag = false;
				}
			}
			byte[] array = null;
			PdfObject objectByName2 = this.m_pAnnotObj.m_objAttributes.GetObjectByName("AP");
			if (objectByName2 != null && objectByName2.m_nType == enumType.pdfDictionary)
			{
				PdfDict pdfDict = (PdfDict)objectByName2;
				PdfObject objectByName3 = pdfDict.GetObjectByName("N");
				if (objectByName3 != null && objectByName3.m_nType == enumType.pdfRef && this.m_pDoc.m_pInput != null)
				{
					PdfStream pdfStream = new PdfStream();
					try
					{
						this.m_pDoc.m_pInput.ParseStream((PdfRef)objectByName3, ref pdfStream, 0);
					}
					catch (Exception ex)
					{
						this.m_pDoc.m_pInput.Close();
						throw ex;
					}
					int num2 = this.FindString(pdfStream, "/Tx BMC");
					if (num2 < 0)
					{
						array = pdfStream.ToBytes();
					}
					else
					{
						int num3 = this.FindString(pdfStream, "EMC");
						if (num3 >= 0 && num3 > num2)
						{
							array = new byte[num2 + pdfStream.Length - num3 - 3];
							Array.Copy(pdfStream.ToBytes(), array, num2);
							Array.Copy(pdfStream.ToBytes(), 3, array, num2, pdfStream.Length - num3 - 3);
						}
					}
				}
			}
			float num4 = 0f;
			float r = 0f;
			float g = 0f;
			float b = 0f;
			PdfObject objectByName4 = this.m_pAnnotObj.m_objAttributes.GetObjectByName("DA");
			if (objectByName4 != null && objectByName4.m_nType == enumType.pdfString)
			{
				string text = ((PdfString)objectByName4).ToString();
				if (text.Length > 0)
				{
					int num5 = text.IndexOf(" Tf");
					int num6 = num5;
					if (num5 > -1)
					{
						num5--;
						while (num5 >= 0 && text[num5] != ' ')
						{
							num5--;
						}
						string s = text.Substring(num5 + 1, num6 - num5 - 1);
						try
						{
							num4 = float.Parse(s, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
						}
						catch (Exception)
						{
						}
						int num7 = text.IndexOf("/");
						if (num7 > -1 && num7 < num5 && num5 - num7 - 1 < 49)
						{
							text.Substring(num7 + 1, num5 - num7 - 1);
						}
					}
					num5 = text.IndexOf(" g");
					int num8 = num5;
					if (num5 > -1)
					{
						num5--;
						while (num5 >= 0 && (text[num5] < 'a' || text[num5] > 'z') && (text[num5] < 'A' || text[num5] > 'Z'))
						{
							num5--;
						}
						if (num5 != 0)
						{
							num5++;
						}
						string s2 = text.Substring(num5 + 1, num8 - num5 - 1);
						float num9 = 0f;
						try
						{
							num9 = float.Parse(s2, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
						}
						catch (Exception)
						{
						}
						if (num9 >= 0f && num9 <= 1f)
						{
							b = (g = (r = num9));
						}
					}
					num5 = text.IndexOf(" rg");
					if (num5 > -1)
					{
						num5--;
						while (num5 >= 0 && (text[num5] < 'a' || text[num5] > 'z') && (text[num5] < 'A' || text[num5] > 'Z'))
						{
							num5--;
						}
						if (num5 != 0)
						{
							num5++;
						}
						float num10 = 0f;
						float num11 = 0f;
						float num12 = 0f;
						string[] array2 = text.Substring(num5 + 1).Split(new char[]
						{
							' '
						});
						try
						{
							if (array2.Length > 0)
							{
								num10 = float.Parse(array2[0], NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
							}
							if (array2.Length > 1)
							{
								num11 = float.Parse(array2[1], NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
							}
							if (array2.Length > 2)
							{
								num12 = float.Parse(array2[2], NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
							}
						}
						catch (Exception)
						{
						}
						if (num10 >= 0f && num10 <= 1f && num11 >= 0f && num11 <= 1f && num12 >= 0f && num12 <= 1f)
						{
							r = num10;
							g = num11;
							b = num12;
						}
					}
				}
			}
			if (this.m_bIsRadiobutton || this.m_bIsCheckbox)
			{
				this.m_pAnnotObj.AddName("DV", Text);
				this.m_pAnnotObj.AddName("V", Text);
			}
			else
			{
				this.m_pAnnotObj.m_objAttributes.Add(new PdfBigEndianString("DV", Text));
				this.m_pAnnotObj.m_objAttributes.Add(new PdfBigEndianString("V", Text));
			}
			string text2 = Text;
			if (Font != null && (Font.m_bstrType == "TrueType" || Font.m_bstrType == "Type1"))
			{
				PdfFont.EncodeText(text2, Font.m_bstrEncoding, out text2);
			}
			if (num4 == 0f)
			{
				num4 = 10f;
				this.CalculateFontSizeForAutoField(text2, Font, ref num4);
			}
			int num13 = this.m_nAlignment;
			if (num13 == -1)
			{
				PdfAnnot parent = this.Parent;
				if (parent != null)
				{
					num13 = parent.m_nAlignment;
				}
				if (num13 == -1)
				{
					num13 = 0;
				}
			}
			if (num13 > 0)
			{
				num13 = 3 - num13;
			}
			PdfGraphics pdfGraphics = this.Graphics[0, ""];
			PdfCanvas canvas = pdfGraphics.Canvas;
			canvas.Append(array);
			canvas.Append(" ");
			canvas.Append("/Tx BMC ");
			canvas.SaveState();
			canvas.SetFillColor(r, g, b);
			float num14 = this.Height;
			if ((this.m_nFieldFlags & 4096) == 0 && Font != null)
			{
				num14 = this.Height / 2f - (Font.m_rectFontBox.Top + Font.m_rectFontBox.Bottom) * num4 / 2000f + num4;
				float arg_664_0 = -num4 * (float)Font.m_nDescent / 1000f;
			}
			canvas.AddRect(this.m_fBorderWidth, this.m_fBorderWidth, this.Width - 2f * this.m_fBorderWidth, this.Height - 2f * this.m_fBorderWidth);
			canvas.Clip();
			PdfParam pdfParam = new PdfParam();
			float num15 = Math.Max(2f, 2f * this.m_fBorderWidth);
			pdfParam["X"] = ((num13 == 0) ? num15 : 0f);
			pdfParam["Y"] = num14;
			pdfParam["Width"] = this.Width - ((num13 == 2) ? 0f : num15);
			pdfParam["Alignment"] = (float)num13;
			pdfParam["Size"] = num4;
			pdfParam["Spacing"] = 1.2f;
			if (flag)
			{
				this.HandleCombField(text2, canvas, Font, num14, num, num4);
			}
			else
			{
				canvas.DrawText(text2, pdfParam, Font);
			}
			canvas.RestoreState();
			canvas.Append("EMC");
			this.m_pAnnotObj.m_bModified = true;
		}
		public void SetFieldValueEx(string Text)
		{
			if (this.m_bIsRadiobutton || this.m_bIsCheckbox)
			{
				this.SetFieldValue(Text, null);
				return;
			}
			string text = "";
			if (this.m_bstrFieldDefaultAppearance == null)
			{
				this.m_bstrFieldDefaultAppearance = this.m_pDoc.m_ptrForm.m_bstrDefaultAppearance;
			}
			if (this.m_bstrFieldDefaultAppearance != null && this.m_bstrFieldDefaultAppearance.Length > 0)
			{
				int num = this.m_bstrFieldDefaultAppearance.IndexOf(" Tf");
				int num2 = num;
				if (num > -1)
				{
					num--;
					while (num >= 0 && this.m_bstrFieldDefaultAppearance[num] != ' ')
					{
						num--;
					}
					string s = this.m_bstrFieldDefaultAppearance.Substring(num + 1, num2 - num - 1);
					try
					{
						float.Parse(s, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
					}
					catch (Exception)
					{
					}
					int num3 = this.m_bstrFieldDefaultAppearance.IndexOf("/");
					if (num3 > -1 && num3 < num && num - num3 - 1 < 49)
					{
						text = this.m_bstrFieldDefaultAppearance.Substring(num3 + 1, num - num3 - 1);
					}
				}
			}
			if (text.Length == 0)
			{
				AuxException.Throw("Default font name could not be obtained for this field. Use SetFieldValue instead.", PdfErrors._ERROR_INVALIDARG);
			}
			PdfFont pdfFont = null;
			foreach (PdfFont current in this.m_pDoc.m_ptrExistingFonts)
			{
				if (current.m_bstrNameInResource != null && current.m_bstrNameInResource.Length > 0 && string.Compare(current.m_bstrNameInResource, text, true) == 0)
				{
					pdfFont = current;
					break;
				}
			}
			if (pdfFont == null)
			{
				AuxException.Throw("Font object could not be located for this field. Use SetFieldValue instead.", PdfErrors._ERROR_INVALIDARG);
			}
			pdfFont.m_szID = pdfFont.m_bstrNameInResource;
			this.SetFieldValue(Text, pdfFont);
		}
		private void CalculateFontSizeForAutoField(string Text, PdfFont pFont, ref float pSize)
		{
			float num = this.Height;
			PdfParam pdfParam = new PdfParam();
			if ((this.m_nFieldFlags & 4096) != 0)
			{
				num = 12f;
				float paragraphHeight;
				do
				{
					num *= 0.95f;
					pdfParam["Width"] = this.Width;
					pdfParam["Size"] = num;
					pdfParam["Spacing"] = 1.2f;
					try
					{
						paragraphHeight = pFont.GetParagraphHeight(Text, pdfParam);
					}
					catch
					{
						return;
					}
				}
				while (paragraphHeight >= this.Height);
			}
			else
			{
				while (true)
				{
					num *= 0.95f;
					try
					{
						PdfRect textExtent = pFont.GetTextExtent(Text, num);
						if (textExtent.Width >= this.Width || textExtent.Height >= this.Height)
						{
							continue;
						}
					}
					catch
					{
						return;
					}
					break;
				}
			}
			pSize = num;
		}
		private void HandleCombField(string Text, PdfCanvas pCanvas, PdfFont pFont, float fY, int nMaxLen, float fSize)
		{
			int num = Text.Length;
			if (num > nMaxLen)
			{
				num = nMaxLen;
			}
			float num2 = this.Width / (float)nMaxLen;
			float num3 = 0f;
			for (int i = 0; i < num; i++)
			{
				string text = Text.Substring(i, 1);
				PdfParam pdfParam = new PdfParam();
				pdfParam["X"] = num3;
				pdfParam["Y"] = fY;
				pdfParam["Size"] = fSize;
				pdfParam["Width"] = num2;
				pdfParam["Alignment"] = 2f;
				pCanvas.DrawText(text, pdfParam, pFont);
				num3 += num2;
			}
		}
		private void ConvertUnicodeToAnsi(string Text, string wszFontName, ref PdfStream pOut)
		{
			int length = Text.Length;
			byte[] array = new byte[length];
			PdfFont pdfFont = null;
			foreach (PdfFont current in this.m_pDoc.m_ptrExistingFonts)
			{
				if (current.m_bstrNameInResource != null && string.Compare(current.m_bstrNameInResource, wszFontName, true) == 0)
				{
					pdfFont = current;
					break;
				}
			}
			string text;
			PdfFont.EncodeText(Text, "PDFDocEncoding", out text);
			for (int i = 0; i < length; i++)
			{
				array[i] = (byte)text[i];
			}
			for (int j = 0; j < length; j++)
			{
				char unicodeChar = Text[j];
				byte b;
				if (pdfFont != null && pdfFont.LookupDifferenceItem(unicodeChar, out b))
				{
					array[j] = b;
				}
				else
				{
					if (this.m_pDoc.m_ptrForm != null && this.m_pDoc.m_ptrForm.m_ptrEncoding != null)
					{
						PdfFont ptrEncoding = this.m_pDoc.m_ptrForm.m_ptrEncoding;
						if (ptrEncoding != null && ptrEncoding.LookupDifferenceItem(unicodeChar, out b))
						{
							array[j] = b;
						}
					}
				}
			}
			pOut.Set(array);
		}
		internal int FindString(PdfStream stream, string szString)
		{
			string @string = Encoding.UTF8.GetString(stream.ToBytes());
			return @string.IndexOf(szString);
		}
		public void SetFieldImage(PdfImage Image)
		{
			this.SetFieldImage(Image, null);
		}
		public void SetFieldImage(PdfImage Image, object Param)
		{
			if (!this.m_bExisting)
			{
				AuxException.Throw("This method can only be called on existing documents.", PdfErrors._ERROR_INVALIDARG);
			}
			if ((this.m_nFieldFlags & 65536) == 0)
			{
				AuxException.Throw("This does not appear to be an image field.", PdfErrors._ERROR_INVALIDARG);
			}
			PdfParam pdfParam = this.m_pManager.VariantToParam(Param);
			PdfRect rect = this.Rect;
			double num = 1.0;
			pdfParam.GetNumberIfSet("Mode", 0, ref num);
			int num2 = (int)num;
			float width = rect.Width;
			float height = rect.Height;
			PdfParam pdfParam2 = this.m_pManager.CreateParam();
			pdfParam2["Left"] = 0f;
			pdfParam2["Bottom"] = 0f;
			pdfParam2["Right"] = width;
			pdfParam2["Top"] = height;
			PdfGraphics pdfGraphics = this.m_pDoc.CreateGraphics(pdfParam2);
			PdfCanvas canvas = pdfGraphics.Canvas;
			canvas.SaveState();
			canvas.AddRect(1f, 1f, width - 1f, height - 1f);
			canvas.Append("W\nn");
			canvas.SaveState();
			canvas.AddRect(2f, 2f, width - 2f, height - 2f);
			canvas.Append("W\nn");
			float num3 = 1f;
			float num4 = 1f;
			if (num2 == 1 || num2 == 2)
			{
				num3 = (width - 4f) / Image.m_fWidth / 72f * Image.m_fResolutionX;
				num4 = (height - 4f) / Image.m_fHeight / 72f * Image.m_fResolutionY;
				if (num2 == 2)
				{
					num4 = (num3 = ((num3 < num4) ? num3 : num4));
				}
			}
			PdfParam pdfParam3 = this.m_pManager.CreateParam();
			pdfParam3["x"] = 2f;
			pdfParam3["y"] = height - 2f - Image.m_fHeight * num4 * 72f / Image.m_fResolutionY;
			pdfParam3["ScaleX"] = num3;
			pdfParam3["ScaleY"] = num4;
			canvas.DrawImage(Image, pdfParam3);
			canvas.RestoreState();
			canvas.RestoreState();
			this.PutGraphics(0, null, pdfGraphics);
			if (!pdfParam.IsSet("ReadOnly") || pdfParam.IsTrue("ReadOnly"))
			{
				this.FieldFlags = (this.m_nFieldFlags | 1);
			}
		}
		public void CreatePushbutton(string Caption, PdfParam pParam, PdfFont Font)
		{
			PdfCanvas pdfCanvas = this.AddEmptyGraphics(0, null);
			PdfCanvas pdfCanvas2 = this.AddEmptyGraphics(1, null);
			float r = 0.784f;
			float g = 0.784f;
			float b = 0.784f;
			if (pParam.IsSet("BgColor"))
			{
				pParam.Color("BgColor", out r, out g, out b);
			}
			AuxRGB pColor = null;
			if (pParam.IsSet("Color"))
			{
				float rr;
				float gg;
				float bb;
				pParam.Color("Color", out rr, out gg, out bb);
				pColor = new AuxRGB(rr, gg, bb);
			}
			float num = 0.784f;
			pdfCanvas.SetFillColor(r, g, b);
			pdfCanvas.FillRect(0f, 0f, this.Width, this.Height);
			pdfCanvas.SetColor(0f, 0f, 0f);
			pdfCanvas.LineWidth = 1f;
			pdfCanvas.MoveTo(0f, 0.5f);
			pdfCanvas.LineTo(this.Width - 0.5f, 0.5f);
			pdfCanvas.LineTo(this.Width - 0.5f, this.Height);
			pdfCanvas.Stroke();
			pdfCanvas.SetColor(num * 1.14f, num * 1.14f, num * 1.14f);
			pdfCanvas.MoveTo(0.5f, 1f);
			pdfCanvas.LineTo(0.5f, this.Height - 0.5f);
			pdfCanvas.LineTo(this.Width - 1f, this.Height - 0.5f);
			pdfCanvas.Stroke();
			pdfCanvas.SetColor(num * 0.76f, num * 0.76f, num * 0.76f);
			pdfCanvas.MoveTo(1f, 1.5f);
			pdfCanvas.LineTo(this.Width - 1.5f, 1.5f);
			pdfCanvas.LineTo(this.Width - 1.5f, this.Height - 1f);
			pdfCanvas.Stroke();
			pdfCanvas2.SetFillColor(r, g, b);
			pdfCanvas2.FillRect(0f, 0f, this.Width, this.Height);
			pdfCanvas2.SetColor(0f, 0f, 0f);
			pdfCanvas2.DrawRect(0.5f, 0.5f, this.Width - 1f, this.Height - 1f);
			pdfCanvas2.SetColor(num * 0.76f, num * 0.76f, num * 0.76f);
			pdfCanvas2.DrawRect(1.5f, 1.5f, this.Width - 3f, this.Height - 3f);
			this.DrawTextHelper(pdfCanvas, Caption, "Times-Roman", Font, this.Width, this.Height, 0f, 0f, this.m_nFontSize, pColor);
			this.DrawTextHelper(pdfCanvas2, Caption, "Times-Roman", Font, this.Width, this.Height, 1f, -1f, this.m_nFontSize, pColor);
			this.m_bIsPushbutton = true;
		}
		public void CreateCheckbox(PdfParam pParam)
		{
			PdfCanvas pdfCanvas = this.AddEmptyGraphics(0, "Yes");
			PdfCanvas pdfCanvas2 = this.AddEmptyGraphics(0, "Off");
			PdfCanvas pdfCanvas3 = this.AddEmptyGraphics(1, "Yes");
			PdfCanvas pdfCanvas4 = this.AddEmptyGraphics(1, "Off");
			bool flag = false;
			if (pParam.IsSet("State"))
			{
				flag = (pParam.Long("State") == 1);
			}
			this.m_pAnnotObj.AddName("AS", flag ? "Yes" : "Off");
			this.m_pAnnotObj.AddName("DV", flag ? "Yes" : "Off");
			this.m_pAnnotObj.AddName("V", flag ? "Yes" : "Off");
			float size = this.Size;
			pdfCanvas.SaveState();
			pdfCanvas.SetColor(0f, 0f, 0f);
			pdfCanvas.DrawRect(0.5f, 0.5f, size - 1f, size - 1f);
			this.DrawTextHelper(pdfCanvas, "4", "ZapfDingbats", null, size, size, 0f, 0f, this.m_nFontSize, null);
			pdfCanvas.RestoreState();
			pdfCanvas2.SaveState();
			pdfCanvas2.SetColor(0f, 0f, 0f);
			pdfCanvas2.DrawRect(0.5f, 0.5f, size - 1f, size - 1f);
			pdfCanvas2.RestoreState();
			pdfCanvas3.SaveState();
			pdfCanvas3.SetColor(0f, 0f, 0f);
			pdfCanvas3.SetFillColor(0.625f, 0.625f, 0.625f);
			pdfCanvas3.FillRect(0.5f, 0.5f, size - 1f, size - 1f);
			pdfCanvas3.DrawRect(0.5f, 0.5f, size - 1f, size - 1f);
			this.DrawTextHelper(pdfCanvas3, "4", "ZapfDingbats", null, size, size, 0f, 0f, this.m_nFontSize, null);
			pdfCanvas3.RestoreState();
			pdfCanvas4.SaveState();
			pdfCanvas4.SetFillColor(0.625f, 0.625f, 0.625f);
			pdfCanvas4.FillRect(0.5f, 0.5f, size - 1f, size - 1f);
			pdfCanvas4.DrawRect(0.5f, 0.5f, size - 1f, size - 1f);
			pdfCanvas4.RestoreState();
			this.m_bIsCheckbox = true;
		}
		public void CreateRadioButton(PdfParam pParam, string OptionName, bool bTopLevel)
		{
			if (bTopLevel)
			{
				this.m_pAnnotObj.AddName("V", OptionName);
				this.m_pAnnotObj.AddName("DV", OptionName);
				this.m_bstrFieldValue = OptionName;
				this.m_bstrFieldDefaultValue = OptionName;
				return;
			}
			PdfCanvas pdfCanvas = this.AddEmptyGraphics(0, OptionName);
			PdfCanvas pdfCanvas2 = this.AddEmptyGraphics(0, "Off");
			PdfCanvas pdfCanvas3 = this.AddEmptyGraphics(1, OptionName);
			PdfCanvas pdfCanvas4 = this.AddEmptyGraphics(1, "Off");
			bool flag = false;
			if (pParam.IsSet("State"))
			{
				flag = (pParam.Long("State") == 1);
			}
			this.m_pAnnotObj.AddName("AS", flag ? OptionName : "Off");
			this.m_pAnnotObj.AddName("DV", flag ? OptionName : "Off");
			float size = this.Size;
			pdfCanvas.SaveState();
			pdfCanvas.SetColor(0f, 0f, 0f);
			pdfCanvas.SetFillColor(0f, 0f, 0f);
			pdfCanvas.DrawEllipse(size / 2f, size / 2f, size / 2f - 1f, size / 2f - 1f);
			pdfCanvas.FillEllipse(size / 2f, size / 2f, size / 4f, size / 4f);
			pdfCanvas.RestoreState();
			pdfCanvas2.SaveState();
			pdfCanvas2.SetColor(0f, 0f, 0f);
			pdfCanvas2.DrawEllipse(size / 2f, size / 2f, size / 2f - 1f, size / 2f - 1f);
			pdfCanvas2.RestoreState();
			pdfCanvas3.SaveState();
			pdfCanvas3.SetFillColor(0.625f, 0.625f, 0.625f);
			pdfCanvas3.SetColor(0f, 0f, 0f);
			pdfCanvas3.FillEllipse(size / 2f, size / 2f, size / 2f - 1f, size / 2f - 1f);
			pdfCanvas3.DrawEllipse(size / 2f, size / 2f, size / 2f - 1f, size / 2f - 1f);
			pdfCanvas3.SetFillColor(0f, 0f, 0f);
			pdfCanvas3.FillEllipse(size / 2f, size / 2f, size / 4f, size / 4f);
			pdfCanvas3.RestoreState();
			pdfCanvas4.SaveState();
			pdfCanvas4.SetFillColor(0.625f, 0.625f, 0.625f);
			pdfCanvas4.SetColor(0f, 0f, 0f);
			pdfCanvas4.FillEllipse(size / 2f, size / 2f, size / 2f - 1f, size / 2f - 1f);
			pdfCanvas4.DrawEllipse(size / 2f, size / 2f, size / 2f - 1f, size / 2f - 1f);
			pdfCanvas4.RestoreState();
			this.m_bIsRadiobutton = true;
		}
		public void CreateTextbox(string Text, PdfParam pParam, PdfFont Font)
		{
			PdfFont pdfFont;
			if (Font == null)
			{
				pdfFont = this.m_pDoc.Fonts["Times-Roman"];
			}
			else
			{
				pdfFont = Font;
			}
			if (Text != null && Text.Length > 0)
			{
				this.m_pAnnotObj.AddString("V", Text);
				this.m_pAnnotObj.AddString("DV", Text);
				this.m_bstrFieldValue = Text;
				this.m_bstrFieldDefaultValue = Text;
			}
			PdfDict pdfDict = this.m_pAnnotObj.AddDict("MK");
			pdfDict.Add(new PdfArray("BC"));
			int num = 0;
			if (pParam.IsSet("Alignment"))
			{
				num = pParam.Long("Alignment");
				if (num == 2)
				{
					num = 1;
				}
				else
				{
					if (num == 1)
					{
						num = 2;
					}
				}
				this.m_pAnnotObj.AddInt("Q", num);
			}
			if (pParam.IsSet("MaxLen"))
			{
				this.m_pAnnotObj.AddInt("MaxLen", pParam.Long("MaxLen"));
			}
			string value = string.Format("/{0} {1} Tf 0 0 0 rg ", pdfFont.m_szID, this.m_nFontSize);
			this.m_pAnnotObj.AddString("DA", value);
			PdfCanvas pdfCanvas = this.AddEmptyGraphics(0, null);
			pdfCanvas.SaveState();
			this.DrawBackgroundHelper(pdfCanvas, pParam);
			PdfParam pdfParam = new PdfParam();
			float value2;
			if (pParam.IsTrue("Multiline"))
			{
				value2 = this.Height - 2f;
			}
			else
			{
				value2 = this.Height / 2f - (float)this.m_nFontSize * ((float)pdfFont.m_nVerticalExtent / 2f + (float)pdfFont.m_nDescent) / (float)pdfFont.m_nUnitsPerEm + (float)this.m_nFontSize;
			}
			pdfParam["x"] = ((num == 0) ? 2f : 0f);
			pdfParam["y"] = value2;
			pdfParam["width"] = this.Width - ((num == 2) ? 2f : 0f);
			pdfParam["height"] = this.Height;
			pdfParam["size"] = (float)this.m_nFontSize;
			pdfParam["color"] = 0f;
			pdfParam["alignment"] = (float)((num == 0) ? 0 : (3 - num));
			pdfCanvas.Append("/Tx BMC");
			pdfCanvas.AddRect(0f, 0f, this.Width, this.Height);
			pdfCanvas.Clip(false);
			pdfCanvas.DrawText(Text, pdfParam, pdfFont);
			pdfCanvas.Append("EMC");
			this.DrawBorderHelper(pdfCanvas, pParam);
			pdfCanvas.RestoreState();
			this.AddFontToFormResource(pdfFont);
			this.m_bIsText = true;
		}
		public void CreateChoice(string Options, string Selection, PdfParam pParam, PdfFont Font)
		{
			if (Options == null || Options.Length == 0)
			{
				AuxException.Throw("Options argument must be specified.", PdfErrors._ERROR_INVALIDARG);
			}
			PdfFont pdfFont = null;
			if (Font == null)
			{
				pdfFont = this.m_pDoc.Fonts["Times-Roman"];
			}
			else
			{
				pdfFont = Font;
			}
			string value = string.Format("/{0} {1} Tf 0 0 0 rg ", pdfFont.m_szID, this.m_nFontSize);
			this.m_pAnnotObj.AddString("DA", value);
			PdfArray pArray = this.m_pAnnotObj.AddArray("Opt");
			this.SplitString(Options, pArray, true);
			if (Selection != null && Selection.Length > 0)
			{
				PdfArray pArray2 = this.m_pAnnotObj.AddArray("V");
				this.SplitString(Selection, pArray2, false);
				PdfArray pArray3 = this.m_pAnnotObj.AddArray("DV");
				this.SplitString(Selection, pArray3, false);
				this.m_bstrFieldValue = Selection;
				this.m_bstrFieldDefaultValue = Selection;
			}
			if (pParam.IsSet("TopIndex"))
			{
				this.m_pAnnotObj.AddInt("TI", pParam.Long("TopIndex"));
			}
			PdfDict pdfDict = this.m_pAnnotObj.AddDict("MK");
			pdfDict.Add(new PdfArray("BC"));
			PdfParam pdfParam = new PdfParam();
			pdfParam["x"] = 2f;
			pdfParam["y"] = this.Height - 2f;
			pdfParam["width"] = this.Width;
			pdfParam["height"] = this.Height;
			pdfParam["size"] = (float)this.m_nFontSize;
			pdfParam["color"] = 0f;
			PdfCanvas pdfCanvas = this.AddEmptyGraphics(0, null);
			pdfCanvas.SaveState();
			this.DrawBackgroundHelper(pdfCanvas, pParam);
			pdfCanvas.Append("/Tx BMC");
			pdfCanvas.AddRect(0f, 0f, this.Width, this.Height);
			pdfCanvas.Clip(false);
			string text = null;
			if (pParam.IsTrue("Combo"))
			{
				text = Selection;
			}
			else
			{
				foreach (string current in this.m_arrDisplayValues)
				{
					text += current;
					text += "\r\n";
				}
			}
			pdfCanvas.DrawText(text, pdfParam, pdfFont);
			pdfCanvas.Append("EMC");
			this.DrawBorderHelper(pdfCanvas, pParam);
			pdfCanvas.RestoreState();
			this.AddFontToFormResource(pdfFont);
			this.m_bIsChoice = true;
		}
		internal void DrawBackgroundHelper(PdfCanvas Canvas, PdfParam pParam)
		{
			if (pParam.IsSet("BGColor"))
			{
				AuxRGB auxRGB = new AuxRGB();
				pParam.Color("BGColor", ref auxRGB);
				Canvas.SetFillColor(auxRGB.r, auxRGB.g, auxRGB.b);
				Canvas.FillRect(0f, 0f, this.Width, this.Height);
				PdfDict pdfDict = this.m_pAnnotObj.AddDict("MK");
				pdfDict.AddRGBArray("BG", auxRGB);
			}
		}
		internal void DrawBorderHelper(PdfCanvas Canvas, PdfParam pParam)
		{
			if (pParam.IsSet("Border"))
			{
				float num = pParam.Number("Border");
				Canvas.LineWidth = num;
				AuxRGB auxRGB = new AuxRGB();
				if (pParam.IsSet("BorderColor"))
				{
					pParam.Color("BorderColor", ref auxRGB);
					Canvas.SetColor(auxRGB.r, auxRGB.g, auxRGB.b);
				}
				PdfDict pdfDict = this.m_pAnnotObj.AddDict("MK");
				if (num > 0f)
				{
					pdfDict.AddRGBArray("BC", auxRGB);
					Canvas.DrawRect(num / 2f, num / 2f, this.Width - num, this.Height - num);
				}
			}
		}
		internal void DrawTextHelper(PdfCanvas Canvas, string Text, string FontName, PdfFont Font, float fWidth, float fHeight, float dx, float dy, int size, AuxRGB pColor)
		{
			PdfFont pdfFont;
			if (Font == null)
			{
				pdfFont = this.m_pDoc.Fonts[FontName];
			}
			else
			{
				pdfFont = Font;
			}
			PdfRect textExtent = pdfFont.GetTextExtent(Text, (float)size);
			float value = (fWidth - textExtent.m_fRight) / 2f + dx;
			float value2 = (fHeight + (float)(2 * this.m_nFontSize) - textExtent.m_fTop) / 2f + dy;
			PdfParam pdfParam = new PdfParam();
			pdfParam["x"] = value;
			pdfParam["y"] = value2;
			pdfParam["size"] = (float)size;
			if (pColor != null)
			{
				Canvas.SetFillColor(pColor.r, pColor.g, pColor.b);
			}
			else
			{
				Canvas.SetFillColor(0f, 0f, 0f);
			}
			Canvas.DrawText(Text, pdfParam, pdfFont);
			this.AddFontToFormResource(pdfFont);
		}
		private void SplitString(string Str, PdfArray pArray, bool bSplitFurther)
		{
			int num = (Str != null) ? Str.Length : 0;
			int num2;
			for (int i = 0; i < num; i = num2 + 2)
			{
				num2 = Str.IndexOf("##", i);
				if (num2 == -1)
				{
					num2 = num;
				}
				string text = Str.Substring(i, num2 - i);
				int num3 = text.IndexOf("%%");
				if (bSplitFurther && num3 != -1)
				{
					string text2 = text.Substring(0, num3);
					string value = text.Substring(num3 + 2);
					PdfArray pdfArray = new PdfArray(null);
					pdfArray.Add(new PdfString(null, value));
					pdfArray.Add(new PdfString(null, text2));
					pArray.Add(pdfArray);
					this.m_mapOptions[text2] = value;
					this.m_arrDisplayValues.Add(text2);
				}
				else
				{
					if (!bSplitFurther && this.m_mapOptions.Count > 0 && text.Length > 0)
					{
						text = this.m_mapOptions[text];
					}
					if (text.Length > 0)
					{
						pArray.Add(new PdfString(null, text));
					}
					if (bSplitFurther)
					{
						this.m_arrDisplayValues.Add(text);
					}
				}
			}
		}
		private void AddFontToFormResource(PdfFont Font)
		{
			PdfForm form = this.m_pDoc.Form;
			form.AddFontToResources(Font, this.m_nFontSize);
		}
	}
}
