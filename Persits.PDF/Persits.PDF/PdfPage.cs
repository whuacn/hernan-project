using System;
using System.Collections.Generic;
using System.Text;
namespace Persits.PDF
{
	public class PdfPage
	{
		internal PdfIndirectObj m_pPageObj;
		internal PdfIndirectObj m_pContentsObj;
		internal PdfIndirectObj m_pBgContentsObj;
		internal PdfIndirectObj m_pResourceObj;
		internal PdfIndirectObj m_pParentObj;
		internal PdfIndirectObj m_pFontDictObj;
		internal PdfIndirectObj m_pXObjectDictObj;
		internal PdfIndirectObj m_pAnnotArrayObj;
		internal PdfIndirectObj m_pCSDictObj;
		internal PdfManager m_pManager;
		internal PdfDocument m_pDoc;
		internal PdfRect m_ptrMediaBox;
		internal PdfRect m_ptrCropBox;
		internal bool m_bContentExtracted;
		internal float m_fVertialExtent;
		internal int m_nIndex;
		internal bool m_bReverseHebrewArabic;
		internal int m_nRotate;
		internal bool m_bExisting;
		internal PdfCanvas m_ptrCanvas;
		internal PdfCanvas m_ptrBgCanvas;
		internal List<PdfAnnot> m_arrAnnots = new List<PdfAnnot>();
		internal PdfDict m_ptrFontDict;
		internal StringBuilder m_bstrContentText = new StringBuilder();
		internal string m_bstrContents = "";
		internal PdfImage m_pThumb;
		public float Width
		{
			get
			{
				return this.m_ptrMediaBox.Width;
			}
			set
			{
				this.m_ptrMediaBox.Width = value;
				this.m_ptrMediaBox.AddArray("MediaBox", this.m_pPageObj);
			}
		}
		public float Height
		{
			get
			{
				return this.m_ptrMediaBox.Height;
			}
			set
			{
				this.m_ptrMediaBox.Height = value;
				this.m_ptrMediaBox.AddArray("MediaBox", this.m_pPageObj);
			}
		}
		public PdfCanvas Background
		{
			get
			{
				if (this.m_ptrBgCanvas == null)
				{
					this.m_pBgContentsObj = this.m_pDoc.AddNewIndirectObject(enumIndirectType.pdfIndirectContents);
					PdfObject objectByName = this.m_pPageObj.m_objAttributes.GetObjectByName("Contents");
					if (objectByName == null)
					{
						this.m_pPageObj.AddReference("Contents", this.m_pBgContentsObj);
					}
					else
					{
						if (objectByName.m_nType == enumType.pdfArray)
						{
							((PdfArray)objectByName).Insert(new PdfReference(null, this.m_pBgContentsObj), 0);
						}
						else
						{
							if (objectByName.m_nType == enumType.pdfRef)
							{
								PdfArray pdfArray = new PdfArray("Contents");
								PdfRef pdfRef = new PdfRef((PdfRef)objectByName);
								pdfRef.m_bstrType = null;
								pdfArray.Add(new PdfReference(null, this.m_pBgContentsObj));
								pdfArray.Add(pdfRef);
								this.m_pPageObj.m_objAttributes.Add(pdfArray);
							}
							if (objectByName.m_nType == enumType.pdfReference)
							{
								PdfArray pdfArray2 = new PdfArray("Contents");
								PdfReference pdfReference = new PdfReference((PdfReference)objectByName);
								pdfReference.m_bstrType = null;
								pdfArray2.Add(new PdfReference(null, this.m_pBgContentsObj));
								pdfArray2.Add(pdfReference);
								this.m_pPageObj.m_objAttributes.Add(pdfArray2);
							}
						}
					}
					this.m_ptrBgCanvas = this.CreateCanvas(this.m_pBgContentsObj);
					this.m_pPageObj.m_bModified = true;
				}
				return this.m_ptrBgCanvas;
			}
		}
		public PdfCanvas Canvas
		{
			get
			{
				if (this.m_ptrCanvas == null)
				{
					this.m_pContentsObj = this.m_pDoc.AddNewIndirectObject(enumIndirectType.pdfIndirectContents);
					PdfObject objectByName = this.m_pPageObj.m_objAttributes.GetObjectByName("Contents");
					if (objectByName == null)
					{
						this.m_pPageObj.AddReference("Contents", this.m_pContentsObj);
					}
					else
					{
						if (objectByName.m_nType == enumType.pdfArray)
						{
							((PdfArray)objectByName).Add(new PdfReference(null, this.m_pContentsObj));
						}
						else
						{
							if (objectByName.m_nType == enumType.pdfRef)
							{
								PdfArray pdfArray = new PdfArray("Contents");
								pdfArray.Add(new PdfRef((PdfRef)objectByName)
								{
									m_bstrType = null
								});
								pdfArray.Add(new PdfReference(null, this.m_pContentsObj));
								this.m_pPageObj.m_objAttributes.Add(pdfArray);
							}
						}
					}
					this.m_ptrCanvas = this.CreateCanvas(this.m_pContentsObj);
					this.m_pPageObj.m_bModified = true;
				}
				return this.m_ptrCanvas;
			}
		}
		public int Index
		{
			get
			{
				int num = 1;
				foreach (PdfPage current in this.m_pDoc.m_ptrPages)
				{
					if (current == this)
					{
						return num;
					}
					num++;
				}
				return 0;
			}
		}
		public PdfPage NextPage
		{
			get
			{
				int index = this.Index;
				PdfPages pages = this.m_pDoc.Pages;
				if (index + 1 <= pages.Count)
				{
					return pages[index + 1];
				}
				return pages.Add(this.Width, this.Height);
			}
		}
		public PdfAnnots Annots
		{
			get
			{
				return new PdfAnnots
				{
					m_pManager = this.m_pManager,
					m_pDoc = this.m_pDoc,
					m_pPage = this,
					m_parrAnnots = this.m_arrAnnots
				};
			}
		}
		public int Rotate
		{
			get
			{
				return this.m_nRotate;
			}
			set
			{
				if (value % 90 != 0)
				{
					AuxException.Throw("Rotate value must be a multiple of 90.", PdfErrors._ERROR_INVALIDARG);
				}
				this.m_nRotate = value;
				this.m_pPageObj.AddInt("Rotate", this.m_nRotate);
				this.m_pPageObj.m_bModified = true;
			}
		}
		public PdfRect MediaBox
		{
			get
			{
				this.m_ptrMediaBox.m_bstrName = "MediaBox";
				this.m_ptrMediaBox.m_pHostObj = this.m_pPageObj;
				return this.m_ptrMediaBox;
			}
		}
		public PdfRect CropBox
		{
			get
			{
				this.m_ptrCropBox.m_bstrName = "CropBox";
				this.m_ptrCropBox.m_pHostObj = this.m_pPageObj;
				return this.m_ptrCropBox;
			}
		}
		public PdfImage Thumb
		{
			get
			{
				if (this.m_pThumb == null)
				{
					AuxException.Throw("Thumb property not set.", PdfErrors._ERROR_PROPNOTSET);
				}
				return this.m_pThumb;
			}
			set
			{
				if (value == null)
				{
					AuxException.Throw("Thumb value cannot be null.", PdfErrors._ERROR_INVALIDARG);
				}
				this.m_pThumb = value;
				this.m_pPageObj.AddReference("Thumb", this.m_pThumb.m_pImageObj);
				this.m_pPageObj.m_bModified = true;
			}
		}
		public PdfAnnot CreatePushbutton(string Name, string Caption, object Param, PdfFont Font, PdfAnnot Parent)
		{
			PdfParam pParam = this.m_pManager.VariantToParam(Param);
			PdfAnnot pdfAnnot = this.CreateFieldHelper(FieldType.FieldButton, FieldType.FieldButtonPushbutton, Parent, Name, pParam);
			pdfAnnot.CreatePushbutton(Caption, pParam, Font);
			return pdfAnnot;
		}
		public PdfAnnot CreateCheckbox(string Name, object Param, PdfAnnot Parent)
		{
			PdfParam pParam = this.m_pManager.VariantToParam(Param);
			PdfAnnot pdfAnnot = this.CreateFieldHelper(FieldType.FieldButton, FieldType.FieldButtonCheckbox, Parent, Name, pParam);
			pdfAnnot.CreateCheckbox(pParam);
			return pdfAnnot;
		}
		public PdfAnnot CreateRadiobutton(string Name, string OptionName, object Param, PdfAnnot Parent)
		{
			if (OptionName == null || OptionName.Length == 0)
			{
				AuxException.Throw("The OptionName argument must be specified.", PdfErrors._ERROR_INVALIDARG);
			}
			PdfParam pParam = this.m_pManager.VariantToParam(Param);
			PdfAnnot pdfAnnot = this.CreateFieldHelper(FieldType.FieldButton, (Parent == null) ? FieldType.FieldButtonRadio : FieldType.FieldButtonCheckbox, Parent, (Parent == null) ? Name : null, pParam);
			pdfAnnot.CreateRadioButton(pParam, OptionName, Parent == null);
			return pdfAnnot;
		}
		public PdfAnnot CreateTextbox(string Name, string Text, object Param, PdfFont Font, PdfAnnot Parent)
		{
			PdfParam pParam = this.m_pManager.VariantToParam(Param);
			PdfAnnot pdfAnnot = this.CreateFieldHelper(FieldType.FieldText, FieldType.FieldText, Parent, Name, pParam);
			pdfAnnot.CreateTextbox(Text, pParam, Font);
			return pdfAnnot;
		}
		public PdfAnnot CreateChoice(string Name, string Options, string Selection, object Param, PdfFont Font, PdfAnnot Parent)
		{
			PdfParam pParam = this.m_pManager.VariantToParam(Param);
			PdfAnnot pdfAnnot = this.CreateFieldHelper(FieldType.FieldChoice, FieldType.FieldChoice, Parent, Name, pParam);
			pdfAnnot.CreateChoice(Options, Selection, pParam, Font);
			return pdfAnnot;
		}
		internal PdfAnnot CreateFieldHelper(FieldType Type, FieldType SpecificType, PdfAnnot Parent, string Name, PdfParam pParam)
		{
			pParam.Add("Type=Widget");
			PdfAnnots annots = this.Annots;
			PdfAnnot pdfAnnot = annots.Add(null, pParam, Name, null);
			string[] array = new string[]
			{
				"Tx",
				"Btn",
				"Ch",
				"Sig"
			};
			pdfAnnot.m_pAnnotObj.AddName("FT", array[(int)Type]);
			PdfForm form = this.m_pDoc.Form;
			form.ValidateName(Name);
			pdfAnnot.m_bstrFieldName = Name;
			if (Parent == null)
			{
				if (SpecificType == FieldType.FieldButtonRadio)
				{
					PdfArray pdfArray = (PdfArray)this.m_pPageObj.m_objAttributes.GetObjectByName("Annots");
					if (pdfArray != null)
					{
						pdfArray.RemoveLast();
					}
					pdfAnnot.m_pAnnotObj.m_objAttributes.RemoveByName("Type");
					pdfAnnot.m_pAnnotObj.m_objAttributes.RemoveByName("Subtype");
					pdfAnnot.m_pAnnotObj.m_objAttributes.RemoveByName("Rect");
				}
				if (SpecificType != FieldType.FieldButtonPushbutton)
				{
					PdfArray pdfArray2 = form.m_pFormObj.AddArray("Fields");
					pdfArray2.m_nItemsPerLine = 10;
					pdfArray2.Add(new PdfReference(null, pdfAnnot.m_pAnnotObj));
					form.m_pFormObj.m_bModified = true;
					form.m_arrFields.Add(pdfAnnot);
				}
			}
			if (Parent != null)
			{
				pdfAnnot.m_pAnnotObj.AddReference("Parent", Parent.m_pAnnotObj);
				PdfArray pdfArray2 = Parent.m_pAnnotObj.AddArray("Kids");
				pdfArray2.Add(new PdfReference(null, pdfAnnot.m_pAnnotObj));
			}
			uint num = 0u;
			if (pParam.IsTrue("ReadOnly"))
			{
				num |= 1u;
			}
			if (pParam.IsTrue("Required"))
			{
				num |= 2u;
			}
			if (pParam.IsTrue("NoExport"))
			{
				num |= 4u;
			}
			if (Name != null)
			{
				pdfAnnot.m_pAnnotObj.AddString("T", Name);
			}
			switch (SpecificType)
			{
			case FieldType.FieldText:
				if (pParam.IsTrue("Multiline"))
				{
					num |= 4096u;
				}
				if (pParam.IsTrue("Password"))
				{
					num |= 8192u;
				}
				if (pParam.IsTrue("FileSelect"))
				{
					num |= 1048576u;
				}
				if (pParam.IsTrue("DoNotSpellCheck"))
				{
					num |= 4194304u;
				}
				if (pParam.IsTrue("DoNotScroll"))
				{
					num |= 8388608u;
					goto IL_357;
				}
				goto IL_357;
			case FieldType.FieldChoice:
				if (pParam.IsTrue("Combo"))
				{
					num |= 131072u;
				}
				if (pParam.IsTrue("Edit"))
				{
					num |= 262144u;
				}
				if (pParam.IsTrue("Sort"))
				{
					num |= 524288u;
				}
				if (pParam.IsTrue("MultiSelect"))
				{
					num |= 2097152u;
				}
				if (pParam.IsTrue("DoNotSpellCheck"))
				{
					num |= 4194304u;
					goto IL_357;
				}
				goto IL_357;
			case FieldType.FieldSignature:
				form.m_pFormObj.AddInt("SigFlags", 3);
				form.m_pFormObj.m_bModified = true;
				goto IL_357;
			case FieldType.FieldButtonPushbutton:
				num |= 65536u;
				goto IL_357;
			case FieldType.FieldButtonRadio:
				num |= 32768u;
				if (pParam.IsTrue("NoToggleToOff"))
				{
					num |= 16384u;
					goto IL_357;
				}
				goto IL_357;
			case FieldType.FieldButtonCheckbox:
				goto IL_357;
			}
			AuxException.Throw("Field type not implemented.", PdfErrors._ERROR_INVALIDARG);
			IL_357:
			if (num > 0u)
			{
				pdfAnnot.m_pAnnotObj.AddInt("Ff", (int)num);
			}
			int num2 = 3;
			if (pParam.IsSet("Highlight"))
			{
				num2 = pParam.Long("Highlight");
				if (num2 < 0 || num2 > 3)
				{
					num2 = 3;
				}
			}
			string[] array2 = new string[]
			{
				"N",
				"I",
				"O",
				"P"
			};
			pdfAnnot.m_pAnnotObj.AddName("H", array2[num2]);
			if (pParam.IsSet("FontSize"))
			{
				pdfAnnot.m_nFontSize = pParam.Long("FontSize");
			}
			return pdfAnnot;
		}
		internal PdfPage()
		{
			this.m_pPageObj = null;
			this.m_pContentsObj = null;
			this.m_pBgContentsObj = null;
			this.m_pResourceObj = null;
			this.m_pParentObj = null;
			this.m_pFontDictObj = null;
			this.m_pXObjectDictObj = null;
			this.m_pAnnotArrayObj = null;
			this.m_pCSDictObj = null;
			this.m_pManager = null;
			this.m_pDoc = null;
			this.m_ptrBgCanvas = null;
			this.m_nIndex = 0;
			this.m_bReverseHebrewArabic = false;
			this.m_fVertialExtent = 0f;
			this.m_nRotate = 0;
			this.m_bExisting = false;
			this.m_bContentExtracted = false;
			float fTop = 792f;
			float fRight = 612f;
			float fBottom = 0f;
			float fLeft = 0f;
			this.m_ptrMediaBox = new PdfRect();
			this.m_ptrCropBox = new PdfRect();
			this.m_ptrCropBox.Set(fLeft, fBottom, fRight, fTop);
			this.m_ptrMediaBox.Set(fLeft, fBottom, fRight, fTop);
		}
		internal PdfCanvas CreateCanvas(PdfIndirectObj pContentsObj)
		{
			return new PdfCanvas
			{
				m_pManager = this.m_pManager,
				m_pDoc = this.m_pDoc,
				m_pContentsObj = pContentsObj,
				m_pResourceObj = (this.m_pResourceObj != null) ? this.m_pResourceObj : this.m_pPageObj,
				m_pFontDictObj = this.m_pFontDictObj,
				m_pXObjectDictObj = this.m_pXObjectDictObj,
				m_pCSDictObj = this.m_pCSDictObj,
				m_pPage = this
			};
		}
		public PdfDest CreateDest()
		{
			return this.CreateDest(null);
		}
		public PdfDest CreateDest(object Param)
		{
			PdfParam pParam = this.m_pManager.VariantToParam(Param);
			PdfDest pdfDest = new PdfDest();
			pdfDest.m_pManager = this.m_pManager;
			pdfDest.m_pDoc = this.m_pDoc;
			pdfDest.Create(this, pParam);
			return pdfDest;
		}
		internal void FromFontNameToObjID(string Name, ref int pObjNum, ref int pGenNum)
		{
			pObjNum = -1;
			pGenNum = -1;
			if (this.m_ptrFontDict == null)
			{
				return;
			}
			for (int i = 0; i < this.m_ptrFontDict.Size; i++)
			{
				PdfObject @object = this.m_ptrFontDict.GetObject(i);
				if (@object.m_nType == enumType.pdfRef)
				{
					PdfRef pdfRef = (PdfRef)@object;
					if (string.Compare(pdfRef.m_bstrType, Name) == 0)
					{
						pObjNum = pdfRef.m_nObjNum;
						pGenNum = pdfRef.m_nGenNum;
						return;
					}
				}
			}
		}
		public string ExtractText(object Param)
		{
			if (this.m_pDoc.m_bEncrypted && (this.m_pDoc.m_nPermissions & pdfPermissions.pdfCopy) == (pdfPermissions)0u && !this.m_pDoc.m_bOwnerPasswordSpecified)
			{
				AuxException.Throw("Content extraction is disabled for this document, owner password is required.", PdfErrors._ERROR_DISABLED);
			}
			PdfParam pdfParam = this.m_pManager.VariantToParam(Param);
			bool bSpace = pdfParam.IsTrue("Space");
			int nCodePage = 0;
			if (pdfParam.IsSet("CodePage"))
			{
				nCodePage = pdfParam.Long("CodePage");
			}
			this.ExtractHelper(bSpace, nCodePage);
			if (pdfParam.IsTrue("All"))
			{
				return this.m_bstrContents;
			}
			return this.m_bstrContentText.ToString();
		}
		public string ExtractText()
		{
			return this.ExtractText(null);
		}
		private void ExtractHelper(bool bSpace, int nCodePage)
		{
			if (this.m_pDoc.m_pInput != null)
			{
				this.m_bstrContents = "";
				this.m_bstrContentText = new StringBuilder();
				try
				{
					this.m_pDoc.m_pInput.ParseContentStreams(this, bSpace, nCodePage);
				}
				catch (Exception ex)
				{
					this.m_pDoc.m_pInput.Close();
					throw ex;
				}
			}
		}
		private bool IsSpace(byte ch)
		{
			return ch == 32 || ch == 13 || ch == 10 || ch == 9;
		}
		public void ResetCoordinates()
		{
			if (this.m_pDoc.m_pInput == null)
			{
				return;
			}
			float[] array = new float[30];
			int num = 0;
			PdfStream pdfStream = new PdfStream();
			AuxMatrix auxMatrix = new AuxMatrix();
			try
			{
				this.m_pDoc.m_pInput.ParseContentStreamsForPreview(this, ref pdfStream);
				pdfStream.AppendSpace();
				PdfInput pdfInput = new PdfInput(pdfStream.ToBytes(), this.m_pDoc, null);
				pdfInput.Next();
				int num2 = 0;
				while (pdfInput.m_dwPtr < pdfInput.m_dwBytesRead)
				{
					PdfObject pdfObject = pdfInput.ParseObject(null, 0, 0);
					enumType nType = pdfObject.m_nType;
					if (nType != enumType.pdfNumber)
					{
						if (nType != enumType.pdfOther)
						{
							num = 0;
						}
						else
						{
							string bstrOriginalValue = ((PdfOther)pdfObject).m_bstrOriginalValue;
							if (bstrOriginalValue == "BI")
							{
								while (pdfInput.m_dwPtr < pdfInput.m_dwBytesRead - 3 && (!this.IsSpace(pdfInput.m_szBuffer[pdfInput.m_dwPtr]) || pdfInput.m_szBuffer[pdfInput.m_dwPtr + 1] != 69 || pdfInput.m_szBuffer[pdfInput.m_dwPtr + 2] != 73 || !this.IsSpace(pdfInput.m_szBuffer[pdfInput.m_dwPtr + 3])))
								{
									pdfInput.m_dwPtr++;
								}
								pdfInput.Next();
							}
							if (bstrOriginalValue == "cm" && num2 == 0)
							{
								AuxMatrix auxMatrix2 = new AuxMatrix();
								auxMatrix2.Init(array[0], array[1], array[2], array[3], array[4], array[5]);
								auxMatrix2.Multiply(auxMatrix);
								auxMatrix.Init(auxMatrix2);
							}
							if (bstrOriginalValue == "q")
							{
								num2++;
							}
							if (bstrOriginalValue == "Q")
							{
								num2--;
							}
							num = 0;
						}
					}
					else
					{
						array[num++] = (float)((PdfNumber)pdfObject).m_fValue;
					}
				}
			}
			catch (Exception ex)
			{
				this.m_pDoc.m_pInput.Close();
				throw ex;
			}
			if (!auxMatrix.IsUnit() && auxMatrix.Inverse())
			{
				this.Canvas.SetCTM(auxMatrix.m_mat[0][0], auxMatrix.m_mat[0][1], auxMatrix.m_mat[1][0], auxMatrix.m_mat[1][1], auxMatrix.m_mat[2][0], auxMatrix.m_mat[2][1]);
			}
		}
		public PdfPreview ToImage()
		{
			return this.ToImage(null);
		}
		public PdfPreview ToImage(object Param)
		{
			AuxException.Throw("This method is currently not supported.", PdfErrors._ERROR_EXISTINGONLY);
			if (!this.m_pDoc.m_bExisting || this.m_pDoc.m_pInput == null)
			{
				AuxException.Throw("A preview can only be obtained on an existing document.", PdfErrors._ERROR_EXISTINGONLY);
			}
			PdfPreview pdfPreview = new PdfPreview();
			pdfPreview.m_pManager = this.m_pManager;
			pdfPreview.m_pDoc = this.m_pDoc;
			pdfPreview.m_pPage = this;
			PdfParam pParam = this.m_pManager.VariantToParam(Param);
			pdfPreview.Create(pParam);
			return pdfPreview;
		}
	}
}
