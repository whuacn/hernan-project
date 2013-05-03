using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
namespace Persits.PDF
{
	public class PdfDocument : IDisposable
	{
		private static string[] m_wszCatalogEntries = new string[]
		{
			"Names",
			"Dests",
			"ViewerPreferences",
			"PageLayout",
			"PageMode",
			"Outlines",
			"Threads",
			"OpenAction",
			"AA",
			"URI",
			"AcroForm",
			"Metadata",
			"StructTreeRoot",
			"MarkInfo",
			"Lang",
			"SpiderInfo",
			"OutputIntents"
		};
		internal List<PdfPoint> m_arrObjIDs = new List<PdfPoint>();
		internal List<PdfIndirectObj> m_arrObjPTRs = new List<PdfIndirectObj>();
		internal List<PdfDocument> m_ptrAppendedDocs = new List<PdfDocument>();
		internal pdfPageLayout m_nPageLayout;
		internal pdfPageMode m_nPageMode;
		internal PdfAction m_ptrOpenAction;
		internal PdfDest m_ptrOpenActionDest;
		internal int m_nSignatureSize;
		internal X509Certificate2 m_pSignerCert;
		internal PdfManager m_pManager;
		internal List<PdfIndirectObj> m_ptrIndirectObjects = new List<PdfIndirectObj>();
		internal List<PdfPage> m_ptrPages = new List<PdfPage>();
		internal List<PdfFont> m_ptrFonts = new List<PdfFont>();
		internal string m_bstrTitle;
		internal string m_bstrAuthor;
		internal string m_bstrSubject;
		internal string m_bstrKeywords;
		internal string m_bstrCreator;
		internal string m_bstrProducer;
		internal PdfIndirectObj m_pCatalogObj;
		internal PdfIndirectObj m_pPageRootObj;
		internal PdfIndirectObj m_pInfoObj;
		internal PdfIndirectObj m_pDefaultFontObj;
		internal PdfIndirectObj m_pEncDict;
		internal PdfIndirectObj m_pMetaDataObj;
		internal PdfString m_objID = new PdfString();
		internal PdfString m_objID2 = new PdfString();
		internal PdfXRef m_objXRef = new PdfXRef();
		internal PdfXRef m_objExistingXRef = new PdfXRef();
		internal bool m_bEncrypted;
		internal bool m_bExisting;
		internal bool m_bCompressedXRef;
		internal bool m_bInfoSet;
		internal PdfStream m_pKey;
		internal string m_bstrVersion = "1.3";
		internal int m_nMasterObjectCount;
		internal PdfDict m_pTrailer;
		internal int m_dwLastXrefPos;
		internal bool m_bSaved;
		internal string m_bstrSavedPath;
		internal DateTime m_dtModDate = default(DateTime);
		internal DateTime m_dtCreationDate = default(DateTime);
		internal pdfPermissions m_nPermissions = (pdfPermissions)4294967292u;
		internal PdfInput m_pInput;
		internal PdfDict m_listFontNames = new PdfDict();
		internal PdfDict m_listXObjectNames = new PdfDict();
		internal PdfDict m_listCSNames = new PdfDict();
		internal string m_szFontID = "F0";
		internal float m_fDefaultFontSize = 10f;
		internal PdfOutline m_ptrOutline;
		internal PdfForm m_ptrForm;
		internal List<PdfFont> m_ptrExistingFonts = new List<PdfFont>();
		internal bool m_bIsMac;
		internal int m_nMacBinarySize;
		internal string m_bstrOriginalVersion;
		internal string m_bstrPath;
		internal bool m_bUserPassword;
		internal bool m_bOwnerPassword;
		internal bool m_bOwnerPasswordSpecified;
		internal string m_bstrMetaData;
		internal byte[] m_pBinaryDoc;
		internal string m_bstrFilename;
		internal string m_bstrOriginalFilename;
		internal bool m_bSigned;
		internal float m_fLowestYCoordinate;
		internal int m_nLowestPageNumber;
		internal bool m_bImportInfoSet;
		public string Producer
		{
			get
			{
				return this.m_bstrProducer;
			}
			set
			{
				this.m_bstrProducer = value;
				this.m_pInfoObj.m_bModified = (this.m_bInfoSet = true);
				this.m_pInfoObj.AddString("Producer", this.m_bstrProducer);
			}
		}
		public string Title
		{
			get
			{
				return this.m_bstrTitle;
			}
			set
			{
				this.m_bstrTitle = value;
				this.m_pInfoObj.m_bModified = (this.m_bInfoSet = true);
				this.m_pInfoObj.AddString("Title", this.m_bstrTitle);
			}
		}
		public string Author
		{
			get
			{
				return this.m_bstrAuthor;
			}
			set
			{
				this.m_bstrAuthor = value;
				this.m_pInfoObj.m_bModified = (this.m_bInfoSet = true);
				this.m_pInfoObj.AddString("Author", this.m_bstrAuthor);
			}
		}
		public string Subject
		{
			get
			{
				return this.m_bstrSubject;
			}
			set
			{
				this.m_bstrSubject = value;
				this.m_pInfoObj.m_bModified = (this.m_bInfoSet = true);
				this.m_pInfoObj.AddString("Subject", this.m_bstrSubject);
			}
		}
		public string Creator
		{
			get
			{
				return this.m_bstrCreator;
			}
			set
			{
				this.m_bstrCreator = value;
				this.m_pInfoObj.m_bModified = (this.m_bInfoSet = true);
				this.m_pInfoObj.AddString("Creator", this.m_bstrCreator);
			}
		}
		public string Keywords
		{
			get
			{
				return this.m_bstrKeywords;
			}
			set
			{
				this.m_bstrKeywords = value;
				this.m_pInfoObj.m_bModified = (this.m_bInfoSet = true);
				this.m_pInfoObj.AddString("Keywords", this.m_bstrKeywords);
			}
		}
		public DateTime CreationDate
		{
			get
			{
				return this.m_dtCreationDate;
			}
			set
			{
				this.m_dtCreationDate = value;
				this.m_pInfoObj.m_bModified = (this.m_bInfoSet = true);
				this.m_pInfoObj.AddDate("CreationDate", this.m_dtCreationDate);
			}
		}
		public DateTime ModDate
		{
			get
			{
				return this.m_dtModDate;
			}
			set
			{
				this.m_dtModDate = value;
				this.m_pInfoObj.m_bModified = (this.m_bInfoSet = true);
				this.m_pInfoObj.AddDate("ModDate", this.m_dtModDate);
			}
		}
		public PdfPages Pages
		{
			get
			{
				if (this.m_pInput != null)
				{
					try
					{
						this.m_pInput.ParseAllPages();
					}
					catch (Exception ex)
					{
						this.m_pInput.Close();
						throw ex;
					}
				}
				return this.GetPages();
			}
		}
		public string Path
		{
			get
			{
				return this.m_bstrSavedPath;
			}
		}
		public string ID
		{
			get
			{
				PdfStream pdfStream = new PdfStream(this.m_objID);
				return pdfStream.ToString();
			}
		}
		public PdfFonts Fonts
		{
			get
			{
				if (this.m_pInput != null)
				{
					this.m_pInput.ParseAllPages();
				}
				return new PdfFonts
				{
					m_pDoc = this,
					m_pManager = this.m_pManager
				};
			}
		}
		public pdfPageLayout PageLayout
		{
			get
			{
				return this.m_nPageLayout;
			}
			set
			{
				if (value < pdfPageLayout.pdfSinglePage || value > pdfPageLayout.pdfTwoColumnRight)
				{
					AuxException.Throw("Invalid value, must be 1 (SinglePage), 2 (OneColumn), 3 (TwoColumnLeft) or 4 (TwoColumnRight).", PdfErrors._ERROR_INVALIDARG);
				}
				this.m_nPageLayout = value;
				string[] array = new string[]
				{
					"SinglePage",
					"OneColumn",
					"TwoColumnLeft",
					"TwoColumnRight"
				};
				this.m_pCatalogObj.AddName("PageLayout", array[this.m_nPageLayout - pdfPageLayout.pdfSinglePage]);
				this.m_pCatalogObj.m_bModified = true;
			}
		}
		public pdfPageMode PageMode
		{
			get
			{
				return this.m_nPageMode;
			}
			set
			{
				if (value < pdfPageMode.pdfUseNone || value > pdfPageMode.pdfFullScreen)
				{
					AuxException.Throw("Invalid value, must be 1 (UseNone), 2 (UseOutlines), 3 (UseThumbs) or 4 (FullScreen).", PdfErrors._ERROR_INVALIDARG);
				}
				string[] array = new string[]
				{
					"UseNone",
					"UseOutlines",
					"UseThumbs",
					"FullScreen"
				};
				this.m_nPageMode = value;
				this.m_pCatalogObj.AddName("PageMode", array[this.m_nPageMode - pdfPageMode.pdfUseNone]);
				this.m_pCatalogObj.m_bModified = true;
			}
		}
		public object OpenAction
		{
			get
			{
				if (this.m_ptrOpenAction == null && this.m_ptrOpenActionDest == null)
				{
					AuxException.Throw("OpenAction property not set.", PdfErrors._ERROR_PROPNOTSET);
				}
				if (this.m_ptrOpenAction != null)
				{
					return this.m_ptrOpenAction;
				}
				if (this.m_ptrOpenActionDest != null)
				{
					return this.m_ptrOpenActionDest;
				}
				return null;
			}
			set
			{
				if (value == null)
				{
					AuxException.Throw("OpenAction cannot be null.", PdfErrors._ERROR_INVALIDARG);
				}
				string fullName = value.GetType().FullName;
				if (fullName != "Persits.PDF.PdfAction" && fullName != "Persits.PDF.PdfDest")
				{
					AuxException.Throw("You must specify either an instance of PdfAction or PdfDest.", PdfErrors._ERROR_INVALIDARG);
				}
				this.m_pCatalogObj.m_objAttributes.RemoveByName("OpenAction");
				this.m_pCatalogObj.m_bModified = true;
				this.m_ptrOpenAction = null;
				this.m_ptrOpenActionDest = null;
				if (fullName == "Persits.PDF.PdfDest")
				{
					this.m_ptrOpenActionDest = (PdfDest)value;
					PdfArray pArray = this.m_pCatalogObj.AddArray("OpenAction");
					this.m_ptrOpenActionDest.Populate(pArray, false);
				}
				if (fullName == "Persits.PDF.PdfAction")
				{
					this.m_ptrOpenAction = (PdfAction)value;
					this.m_pCatalogObj.AddReference("OpenAction", this.m_ptrOpenAction.m_pActionObj);
				}
			}
		}
		public string Version
		{
			get
			{
				return this.m_bstrVersion;
			}
			set
			{
				if (value.Length < 3)
				{
					AuxException.Throw("Invalid version value.", PdfErrors._ERROR_OUTOFRANGE);
				}
				this.m_bstrVersion = value;
			}
		}
		public bool Encrypted
		{
			get
			{
				return this.m_bEncrypted;
			}
		}
		public int KeyLength
		{
			get
			{
				if (this.m_pKey == null)
				{
					return 0;
				}
				if (this.m_pKey.m_nPtr != 1)
				{
					return this.m_pKey.Length * 8;
				}
				return -this.m_pKey.Length * 8;
			}
		}
		public bool Mac
		{
			get
			{
				return this.m_bIsMac;
			}
		}
		public int Permissions
		{
			get
			{
				if (this.m_bEncrypted)
				{
					return (int)this.m_nPermissions;
				}
				return 0;
			}
		}
		public string OriginalPath
		{
			get
			{
				return this.m_bstrPath;
			}
		}
		public string Filename
		{
			get
			{
				return this.m_bstrFilename;
			}
		}
		public string OriginalFilename
		{
			get
			{
				return this.m_bstrOriginalFilename;
			}
		}
		public bool UserPassword
		{
			get
			{
				return this.m_bUserPassword;
			}
		}
		public bool OwnerPassword
		{
			get
			{
				return this.m_bOwnerPassword;
			}
		}
		public bool OwnerPasswordUsed
		{
			get
			{
				return this.m_bOwnerPasswordSpecified;
			}
		}
		public string OriginalVersion
		{
			get
			{
				return this.m_bstrOriginalVersion;
			}
		}
		public string MetaData
		{
			get
			{
				this.MetaDataHelper();
				return this.m_bstrMetaData;
			}
			set
			{
				if (this.m_pMetaDataObj == null)
				{
					this.m_pMetaDataObj = this.AddNewIndirectObject(enumIndirectType.pdfIndirectStream);
					this.m_pMetaDataObj.AddName("Type", "Metadata");
					this.m_pMetaDataObj.AddName("Subtype", "XML");
					this.m_pCatalogObj.m_objAttributes.RemoveByName("Metadata");
					this.m_pCatalogObj.AddReference("Metadata", this.m_pMetaDataObj);
					this.m_pCatalogObj.m_bModified = true;
				}
				PdfString pdfString = new PdfString(null, value);
				if (!pdfString.m_bAnsi)
				{
					this.m_pMetaDataObj.m_objStream.Append(new byte[]
					{
						255,
						254
					});
				}
				this.m_pMetaDataObj.m_objStream.Append(pdfString);
				this.m_bstrMetaData = value;
			}
		}
		public bool CompressedXRef
		{
			get
			{
				return this.m_bCompressedXRef;
			}
			set
			{
				this.m_bCompressedXRef = value;
			}
		}
		public PdfOutline Outline
		{
			get
			{
				if (this.m_pInput != null)
				{
					try
					{
						this.m_pInput.ParseOutlines();
					}
					catch (Exception ex)
					{
						this.m_pInput.Close();
						throw ex;
					}
				}
				return this.GetOutline();
			}
		}
		public PdfForm Form
		{
			get
			{
				if (this.m_pInput != null)
				{
					try
					{
						this.m_pInput.ParseAllPages();
						this.m_pInput.ParseAcroForm();
					}
					catch (Exception ex)
					{
						this.m_pInput.Close();
						throw ex;
					}
				}
				return this.GetForm();
			}
		}
		public PdfParam ImportInfo
		{
			get
			{
				if (!this.m_bImportInfoSet)
				{
					AuxException.Throw("This property can only be called after a successful call to ImportFromUrl.", PdfErrors._ERROR_INVALIDARG);
				}
				PdfParam pdfParam = new PdfParam();
				pdfParam["Y"] = this.m_fLowestYCoordinate;
				pdfParam["Page"] = (float)this.m_nLowestPageNumber;
				return pdfParam;
			}
		}
		public void AppendDocument(PdfDocument Doc)
		{
			if (Doc == null)
			{
				AuxException.Throw("Document argument is empty.", PdfErrors._ERROR_INVALIDARG);
			}
			if (Doc == this)
			{
				AuxException.Throw("Document cannot be appended to self.", PdfErrors._ERROR_INVALIDARG);
			}
			foreach (PdfDocument current in this.m_ptrAppendedDocs)
			{
				if (Doc == current)
				{
					AuxException.Throw("The same document cannot be appended twice.", PdfErrors._ERROR_INVALIDARG);
				}
			}
			if (Doc.m_bEncrypted && !Doc.m_bOwnerPasswordSpecified)
			{
				AuxException.Throw("This document cannot be appended, owner password is required.", PdfErrors._ERROR_DISABLED);
			}
			this.m_ptrAppendedDocs.Add(Doc);
			if (Doc.m_pCatalogObj.m_objAttributes.GetObjectByName("AcroForm") != null)
			{
				PdfForm arg_A3_0 = this.Form;
			}
		}
		private void HandleAppendedDocuments(int nObjNumAdjustment, ref int nObjectCount)
		{
			if (this.m_ptrAppendedDocs.Count <= 0)
			{
				return;
			}
			PdfPages arg_15_0 = this.Pages;
			int num = 0;
			foreach (PdfDocument current in this.m_ptrAppendedDocs)
			{
				int num2 = 0;
				int size = current.m_objExistingXRef.Size;
				for (int i = 0; i < size; i++)
				{
					PdfXRefEntry pdfXRefEntry = (PdfXRefEntry)current.m_objExistingXRef.GetObject(i);
					current.RetrieveIndirectObj(pdfXRefEntry, this, nObjNumAdjustment);
					if (pdfXRefEntry.m_nObjNumber != 0)
					{
						num++;
						num2++;
					}
				}
				PdfPages arg_85_0 = current.Pages;
				PdfArray pdfArray = this.m_pPageRootObj.AddArray("Kids");
				PdfObject objectByName = current.m_pCatalogObj.m_objAttributes.GetObjectByName("Pages");
				if (objectByName != null && objectByName.m_nType == enumType.pdfRef)
				{
					PdfObject pdfObject = objectByName.Copy();
					pdfObject.m_bstrType = null;
					this.AdjustIndirectRefs(pdfObject, nObjNumAdjustment);
					pdfArray.Add(pdfObject);
					PdfIndirectObj pdfIndirectObj = this.FindExistingIndirectObj(((PdfRef)pdfObject).m_nObjNum, ((PdfRef)pdfObject).m_nGenNum);
					if (pdfIndirectObj != null)
					{
						pdfIndirectObj.AddReference("Parent", this.m_pPageRootObj);
					}
				}
				for (int j = 0; j < PdfDocument.m_wszCatalogEntries.Length; j++)
				{
					if (this.m_pCatalogObj.m_objAttributes.GetObjectByName(PdfDocument.m_wszCatalogEntries[j]) == null)
					{
						PdfObject objectByName2 = current.m_pCatalogObj.m_objAttributes.GetObjectByName(PdfDocument.m_wszCatalogEntries[j]);
						if (objectByName2 != null)
						{
							this.AdjustIndirectRefs(objectByName2, nObjNumAdjustment);
							this.m_pCatalogObj.m_objAttributes.Add(objectByName2.Copy());
							this.m_pCatalogObj.m_bModified = true;
						}
					}
				}
				this.MergeFormFields2(current, nObjNumAdjustment);
				PdfObject objectByName3 = current.m_pPageRootObj.m_objAttributes.GetObjectByName("Count");
				if (objectByName3 != null && objectByName3.m_nType == enumType.pdfNumber)
				{
					this.m_pPageRootObj.ChangeInt("Count", (int)((PdfNumber)objectByName3).m_fValue);
				}
				else
				{
					if (objectByName3 != null && objectByName3.m_nType == enumType.pdfRef)
					{
						PdfIndirectObj pdfIndirectObj2 = this.FindExistingIndirectObj(((PdfRef)objectByName3).m_nObjNum + nObjNumAdjustment, ((PdfRef)objectByName3).m_nGenNum);
						if (pdfIndirectObj2 != null && pdfIndirectObj2.m_pSimpleValue != null)
						{
							this.m_pPageRootObj.ChangeInt("Count", (int)((PdfNumber)pdfIndirectObj2.m_pSimpleValue).m_fValue);
						}
					}
				}
				this.m_pPageRootObj.m_bModified = true;
				nObjNumAdjustment += current.m_objExistingXRef.m_nMaxNum;
			}
			nObjectCount += num;
		}
		private void BuildLeafFieldList(int nObjNum, int nGenNum, ref List<int> arrObjNum, ref List<int> arrGenNum)
		{
			PdfIndirectObj pdfIndirectObj = this.FindExistingIndirectObj(nObjNum, nGenNum);
			if (pdfIndirectObj == null)
			{
				return;
			}
			PdfObject objectByName = pdfIndirectObj.m_objAttributes.GetObjectByName("Kids");
			if (objectByName == null)
			{
				arrObjNum.Add(nObjNum);
				arrGenNum.Add(nGenNum);
				return;
			}
			if (objectByName.m_nType != enumType.pdfArray)
			{
				return;
			}
			PdfArray pdfArray = (PdfArray)objectByName;
			for (int i = 0; i < pdfArray.Size; i++)
			{
				PdfObject @object = pdfArray.GetObject(i);
				if (@object != null && @object.m_nType == enumType.pdfRef)
				{
					this.BuildLeafFieldList(((PdfRef)@object).m_nObjNum, ((PdfRef)@object).m_nGenNum, ref arrObjNum, ref arrGenNum);
				}
			}
		}
		private void MergeFormFields2(PdfDocument pDoc, int nObjNumAdjustment)
		{
			if (pDoc.m_pCatalogObj.m_objAttributes.GetObjectByName("AcroForm") == null)
			{
				return;
			}
			PdfForm form = this.Form;
			PdfForm form2 = pDoc.Form;
			PdfArray pdfArray = (PdfArray)form.m_pFormObj.m_objAttributes.GetObjectByName("Fields");
			PdfArray pdfArray2 = (PdfArray)form2.m_pFormObj.m_objAttributes.GetObjectByName("Fields");
			if (pdfArray == null || pdfArray2 == null)
			{
				return;
			}
			for (int i = 0; i < pdfArray2.Size; i++)
			{
				PdfObject @object = pdfArray2.GetObject(i);
				this.AdjustIndirectRefs(@object, nObjNumAdjustment);
				pdfArray.Add(@object.Copy());
			}
			form.m_pFormObj.m_bModified = true;
			List<string> list = new List<string>();
			List<int> list2 = new List<int>();
			List<int> list3 = new List<int>();
			for (int j = 0; j < pdfArray.Size; j++)
			{
				PdfObject object2 = pdfArray.GetObject(j);
				if (object2 != null && object2.m_nType == enumType.pdfRef)
				{
					this.BuildLeafFieldList(((PdfRef)object2).m_nObjNum, ((PdfRef)object2).m_nGenNum, ref list2, ref list3);
				}
			}
			for (int k = 0; k < list2.Count; k++)
			{
				PdfIndirectObj pdfIndirectObj = this.FindExistingIndirectObj(list2[k], list3[k]);
				if (pdfIndirectObj != null)
				{
					PdfObject objectByName = pdfIndirectObj.m_objAttributes.GetObjectByName("T");
					if (objectByName != null && objectByName.m_nType == enumType.pdfString)
					{
						string text = ((PdfString)objectByName).ToString();
						if (text.Length != 0)
						{
							while (!this.IsUnique(ref list, text))
							{
								text += "_";
							}
							list.Add(text);
							pdfIndirectObj.m_objAttributes.Add(new PdfString("T", text));
						}
					}
				}
			}
			for (int l = 0; l < pdfArray.Size; l++)
			{
				PdfObject object3 = pdfArray.GetObject(l);
				PdfIndirectObj pdfIndirectObj2 = this.FindExistingIndirectObj(((PdfRef)object3).m_nObjNum, ((PdfRef)object3).m_nGenNum);
				if (pdfIndirectObj2 != null)
				{
					PdfObject objectByName2 = pdfIndirectObj2.m_objAttributes.GetObjectByName("T");
					if (objectByName2 != null && objectByName2.m_nType == enumType.pdfString)
					{
						string text2 = ((PdfString)objectByName2).ToString();
						if (text2.Length != 0)
						{
							while (!this.IsUnique(ref list, text2))
							{
								text2 += "_";
							}
							list.Add(text2);
							pdfIndirectObj2.m_objAttributes.Add(new PdfString("T", text2));
						}
					}
				}
			}
		}
		private bool IsUnique(ref List<string> arrNames, string Name)
		{
			foreach (string current in arrNames)
			{
				if (string.Compare(current, Name, true) == 0)
				{
					return false;
				}
			}
			return true;
		}
		private void RetrieveIndirectObj(PdfXRefEntry pXRefEntry, PdfDocument pMasterDoc, int nAdjustment)
		{
			if (this.m_pInput == null)
			{
				AuxException.Throw("For a document to be appended to another document, the former must be based on an existing PDF file.", PdfErrors._ERROR_APPEND);
			}
			if (pXRefEntry.m_cType == 102)
			{
				if (pXRefEntry.m_nObjNumber == 0)
				{
					return;
				}
				pMasterDoc.m_objXRef.AddEntry(pXRefEntry.m_nOffset + nAdjustment, pXRefEntry.m_nObjNumber + nAdjustment, pXRefEntry.m_nGenNumber, 102);
				return;
			}
			else
			{
				if (pXRefEntry.m_nOffset == 0 && pXRefEntry.m_nObjNumber > 0)
				{
					pMasterDoc.m_objXRef.AddEntry(0, pXRefEntry.m_nObjNumber + nAdjustment, pXRefEntry.m_nGenNumber, 102);
					return;
				}
				if (this.m_pInput != null)
				{
					try
					{
						PdfIndirectObj pdfIndirectObj = this.m_pInput.ParseGenericIndirectObj(pXRefEntry.m_nOffset, pMasterDoc, nAdjustment);
						if (pdfIndirectObj.m_pSimpleValue != null)
						{
							this.AdjustIndirectRefs(pdfIndirectObj.m_pSimpleValue, nAdjustment);
						}
						else
						{
							this.AdjustIndirectRefs(pdfIndirectObj.m_objAttributes, nAdjustment);
						}
					}
					catch (Exception ex)
					{
						this.m_pInput.Close();
						throw ex;
					}
				}
				return;
			}
		}
		private void AdjustIndirectRefs(PdfObject pObj, int nAdjustment)
		{
			if (pObj.m_nType == enumType.pdfRef)
			{
				((PdfRef)pObj).m_nObjNum += nAdjustment;
				return;
			}
			if (pObj.m_nType == enumType.pdfDictionary || pObj.m_nType == enumType.pdfArray)
			{
				PdfObjectList pdfObjectList = (PdfObjectList)pObj;
				for (int i = 0; i < pdfObjectList.Size; i++)
				{
					this.AdjustIndirectRefs(pdfObjectList.GetObject(i), nAdjustment);
				}
			}
		}
		internal PdfIndirectObj FindExistingIndirectObj(int nObjNum, int nGenNum)
		{
			foreach (PdfIndirectObj current in this.m_ptrIndirectObjects)
			{
				if (current.m_bExisting && current.m_nObjNumber == nObjNum && current.m_nGenNumber == nGenNum)
				{
					return current;
				}
			}
			return null;
		}
		public PdfDocument ExtractPages(object Param)
		{
			if (this.m_pInput == null)
			{
				AuxException.Throw("This document must be based on an existing PDF file.", PdfErrors._ERROR_EXTRACT);
			}
			if (this.m_bEncrypted && !this.m_bOwnerPasswordSpecified)
			{
				AuxException.Throw("Page extraction cannot be performed, the owner password is required.", PdfErrors._ERROR_DISABLED);
			}
			PdfParam pdfParam = this.m_pManager.VariantToParam(Param);
			PdfPages pages = this.Pages;
			int count = pages.Count;
			this.m_arrObjIDs.Clear();
			List<int> list = new List<int>();
			int num = 1;
			while (true)
			{
				string name = string.Format("Page{0}", num);
				if (!pdfParam.IsSet(name))
				{
					break;
				}
				int num2 = (int)pdfParam.Number(name);
				if (num2 < 1 || num2 > count)
				{
					AuxException.Throw(string.Format("Page index {0} is out of range, must be between 1 and {1}.", num2, count), PdfErrors._ERROR_INVALID_INDEX);
				}
				foreach (int current in list)
				{
					if (current == num2)
					{
						AuxException.Throw(string.Format("Page index {0} is used more than once.", num2), PdfErrors._ERROR_INVALID_INDEX);
					}
				}
				list.Add(num2);
				num++;
			}
			if (list.Count == 0)
			{
				AuxException.Throw("At least one page number must be specified.", PdfErrors._ERROR_INVALID_INDEX);
			}
			this.m_arrObjIDs.Clear();
			int i = 0;
			PdfDocument pdfDocument = new PdfDocument();
			pdfDocument.m_pManager = this.m_pManager;
			pdfDocument.Init(null);
			int num3 = 5;
			PdfIndirectObj pdfIndirectObj = pdfDocument.AddNewIndirectObject(enumIndirectType.pdfIndirectPageRoot);
			pdfIndirectObj.AddName("Type", "Pages");
			int num4 = this.m_objExistingXRef.m_nMaxNum + 1;
			using (List<int>.Enumerator enumerator2 = list.GetEnumerator())
			{
				IL_455:
				while (enumerator2.MoveNext())
				{
					int current2 = enumerator2.Current;
					PdfPage pdfPage = this.Pages[current2];
					this.AddObjectID(pdfPage.m_pPageObj.m_nObjNumber, pdfPage.m_pPageObj.m_nGenNumber);
					while (i < this.m_arrObjIDs.Count)
					{
						int x = this.m_arrObjIDs[i].x;
						int y = this.m_arrObjIDs[i].y;
						int offset = this.m_objExistingXRef.GetOffset(x, y);
						try
						{
							PdfIndirectObj pdfIndirectObj2 = this.m_pInput.ParseGenericIndirectObj(offset, pdfDocument, i + num3 - x);
							if (pdfIndirectObj2.m_pSimpleValue != null)
							{
								this.TraverseObject(pdfIndirectObj2.m_pSimpleValue, num3);
							}
							else
							{
								this.TraverseObject(pdfIndirectObj2.m_objAttributes, num3);
							}
						}
						catch (Exception ex)
						{
							this.m_pInput.Close();
							throw ex;
						}
						i++;
					}
					int nObjNum = num3 + pdfPage.m_pPageObj.m_nObjNumber - this.AddObjectID(pdfPage.m_pPageObj.m_nObjNumber, pdfPage.m_pPageObj.m_nGenNumber);
					int nGenNum = pdfPage.m_pPageObj.m_nGenNumber;
					PdfIndirectObj pdfIndirectObj3 = null;
					PdfIndirectObj pdfIndirectObj4 = pdfPage.m_pPageObj;
					while (true)
					{
						PdfIndirectObj pdfIndirectObj5 = pdfDocument.FindExistingIndirectObj(nObjNum, nGenNum);
						if (pdfIndirectObj5 == null)
						{
							goto IL_455;
						}
						PdfObject objectByName = pdfIndirectObj5.m_objAttributes.GetObjectByName("Kids");
						if (objectByName != null)
						{
							pdfIndirectObj4 = pdfDocument.AddNewIndirectObject(pdfIndirectObj5.m_nType);
							pdfIndirectObj4.m_bExisting = true;
							pdfIndirectObj4.m_objAttributes.CopyItems(pdfIndirectObj5.m_objAttributes);
							pdfIndirectObj4.m_nObjNumber = num4++;
							int num5 = this.AddObjectID(pdfIndirectObj4.m_nObjNumber, pdfIndirectObj4.m_nGenNumber);
							pdfIndirectObj4.m_nObjNumber -= num5 - num3;
							i++;
							pdfIndirectObj4.m_objAttributes.RemoveByName("Kids");
							pdfIndirectObj4.m_objAttributes.RemoveByName("Count");
							PdfArray pdfArray = pdfIndirectObj4.AddArray("Kids");
							pdfArray.Add(new PdfRef(null, pdfIndirectObj3.m_nObjNumber, pdfIndirectObj3.m_nGenNumber));
							pdfIndirectObj4.ChangeInt("Count", 1);
							pdfIndirectObj3.m_objAttributes.Add(new PdfRef("Parent", pdfIndirectObj4.m_nObjNumber, pdfIndirectObj4.m_nGenNumber));
							pdfIndirectObj3 = pdfIndirectObj4;
						}
						else
						{
							pdfIndirectObj3 = pdfIndirectObj5;
						}
						PdfObject objectByName2 = pdfIndirectObj5.m_objAttributes.GetObjectByName("Parent");
						if (objectByName2 == null || objectByName2.m_nType != enumType.pdfRef)
						{
							break;
						}
						nObjNum = ((PdfRef)objectByName2).m_nObjNum;
						nGenNum = ((PdfRef)objectByName2).m_nGenNum;
					}
					PdfArray pdfArray2 = pdfIndirectObj.AddArray("Kids");
					pdfArray2.Add(new PdfRef(null, pdfIndirectObj4.m_nObjNumber, pdfIndirectObj4.m_nGenNumber));
					pdfIndirectObj.ChangeInt("Count", 1);
					pdfIndirectObj4.AddReference("Parent", pdfIndirectObj);
				}
			}
			pdfDocument.m_pCatalogObj.AddReference("Pages", pdfIndirectObj);
			return pdfDocument;
		}
		internal void TraverseObject(PdfObject pObj, int nAdjustment)
		{
			if (pObj.m_nType == enumType.pdfRef)
			{
				int num = this.AddObjectID(((PdfRef)pObj).m_nObjNum, ((PdfRef)pObj).m_nGenNum);
				((PdfRef)pObj).m_nObjNum -= num - nAdjustment;
				return;
			}
			if (pObj.m_nType == enumType.pdfDictionary || pObj.m_nType == enumType.pdfArray)
			{
				bool flag = false;
				if (pObj.m_nType == enumType.pdfDictionary)
				{
					PdfDict pdfDict = (PdfDict)pObj;
					PdfObject objectByName = pdfDict.GetObjectByName("Type");
					if (objectByName != null && objectByName.m_nType == enumType.pdfName && ((PdfName)objectByName).m_bstrName == "Pages")
					{
						flag = true;
					}
				}
				PdfObjectList pdfObjectList = (PdfObjectList)pObj;
				for (int i = 0; i < pdfObjectList.Size; i++)
				{
					PdfObject @object = pdfObjectList.GetObject(i);
					if (!flag || !(@object.m_bstrType == "Kids"))
					{
						this.TraverseObject(pdfObjectList.GetObject(i), nAdjustment);
					}
				}
			}
		}
		internal int AddObjectID(int nObjNum, int nGenNum)
		{
			int num = 0;
			foreach (PdfPoint current in this.m_arrObjIDs)
			{
				if (current.x == nObjNum && current.y == nGenNum)
				{
					return nObjNum - num;
				}
				num++;
			}
			PdfPoint pdfPoint = new PdfPoint();
			pdfPoint.x = nObjNum;
			pdfPoint.y = nGenNum;
			this.m_arrObjIDs.Add(pdfPoint);
			return nObjNum - this.m_arrObjIDs.Count + 1;
		}
		public PdfGraphics CreateGraphicsFromPage(PdfDocument Doc, int PageIndex)
		{
			if (Doc == null)
			{
				AuxException.Throw("The Document argument cannot be null.", PdfErrors._ERROR_INVALIDARG);
			}
			if (Doc == this)
			{
				AuxException.Throw("The Document argument must be a separate document.", PdfErrors._ERROR_INVALIDARG);
			}
			if (!Doc.m_bExisting)
			{
				AuxException.Throw("This method can only be called on existing documents.", PdfErrors._ERROR_INVALIDARG);
			}
			if (Doc.m_bEncrypted && !Doc.m_bOwnerPasswordSpecified)
			{
				AuxException.Throw("A page cannot be imported from this document, owner password is required.", PdfErrors._ERROR_INVALIDARG);
			}
			this.m_arrObjIDs.Clear();
			this.m_arrObjPTRs.Clear();
			PdfPages arg_6B_0 = this.Pages;
			PdfPage pdfPage = Doc.Pages[PageIndex];
			PdfRef pObj = new PdfRef(null, pdfPage.m_pPageObj.m_nObjNumber, pdfPage.m_pPageObj.m_nGenNumber);
			PdfIndirectObj pdfIndirectObj;
			try
			{
				pdfIndirectObj = this.RetrieveIndirObject(Doc, pObj);
				Doc.m_pInput.ParseContentStreamsForPreview(pdfPage, ref pdfIndirectObj.m_objStream);
			}
			catch (Exception ex)
			{
				Doc.m_pInput.Close();
				throw ex;
			}
			PdfGraphics pdfGraphics = new PdfGraphics();
			pdfGraphics.m_pManager = this.m_pManager;
			pdfGraphics.m_pDoc = this;
			pdfIndirectObj.m_nType = enumIndirectType.pdfIndirectXObject;
			pdfGraphics.m_pXObj = pdfIndirectObj;
			PdfImage pdfImage = new PdfImage();
			pdfImage.m_pDoc = this;
			pdfImage.m_pManager = this.m_pManager;
			pdfImage.SetIndex();
			pdfGraphics.m_bstrID = pdfImage.m_bstrID;
			PdfObject pdfObject = null;
			PdfObject pdfObject2 = null;
			if (pdfIndirectObj.m_objAttributes.GetObjectByName("Resources") == null || pdfIndirectObj.m_objAttributes.GetObjectByName("MediaBox") == null)
			{
				PdfIndirectObj pdfIndirectObj2 = pdfIndirectObj;
				while (true)
				{
					PdfObject objectByName = pdfIndirectObj2.m_objAttributes.GetObjectByName("Parent");
					if (objectByName == null || objectByName.m_nType != enumType.pdfReference)
					{
						break;
					}
					pdfIndirectObj2 = (PdfIndirectObj)((PdfReference)objectByName).m_pPtr;
					if (pdfObject2 == null)
					{
						pdfObject2 = pdfIndirectObj2.m_objAttributes.GetObjectByName("Resources");
					}
					if (pdfObject == null)
					{
						pdfObject = pdfIndirectObj2.m_objAttributes.GetObjectByName("MediaBox");
					}
				}
				if (pdfObject2 != null)
				{
					pdfIndirectObj.m_objAttributes.Add(pdfObject2.Copy());
				}
				if (pdfObject != null)
				{
					pdfIndirectObj.m_objAttributes.Add(pdfObject.Copy());
				}
			}
			pdfIndirectObj.m_objAttributes.RemoveByName("Parent");
			pdfIndirectObj.m_objAttributes.RemoveByName("Type");
			PdfObject objectByName2 = pdfIndirectObj.m_objAttributes.GetObjectByName("MediaBox");
			if (objectByName2 != null)
			{
				PdfObject pdfObject3 = objectByName2.Copy();
				pdfObject3.m_bstrType = "BBox";
				pdfIndirectObj.m_objAttributes.Add(pdfObject3);
				pdfIndirectObj.m_objAttributes.RemoveByName("MediaBox");
			}
			pdfIndirectObj.m_objAttributes.Add(new PdfName("Type", "XObject"));
			pdfIndirectObj.m_objAttributes.Add(new PdfName("Subtype", "Form"));
			pdfIndirectObj.m_objAttributes.Add(new PdfNumber("FormType", 1.0));
			return pdfGraphics;
		}
		internal PdfIndirectObj AddObjectIDandPtr(int nObjNum, int nGenNum)
		{
			int count = this.m_arrObjIDs.Count;
			int index = nObjNum - this.AddObjectID(nObjNum, nGenNum);
			if (this.m_arrObjIDs.Count > count)
			{
				return null;
			}
			return this.m_arrObjPTRs[index];
		}
		internal PdfIndirectObj RetrieveIndirObject(PdfDocument pDoc, PdfObject pObj)
		{
			PdfIndirectObj pdfIndirectObj = null;
			if (pObj.m_nType == enumType.pdfRef)
			{
				PdfIndirectObj pdfIndirectObj2 = this.AddObjectIDandPtr(((PdfRef)pObj).m_nObjNum, ((PdfRef)pObj).m_nGenNum);
				if (pdfIndirectObj2 != null)
				{
					return pdfIndirectObj2;
				}
				int offset = pDoc.m_objExistingXRef.GetOffset(((PdfRef)pObj).m_nObjNum, ((PdfRef)pObj).m_nGenNum);
				pdfIndirectObj = pDoc.m_pInput.ParseGenericIndirectObj(offset, this, this.m_objExistingXRef.Size + 1);
				pdfIndirectObj.m_bExisting = false;
				pdfIndirectObj.m_bModified = true;
				pdfIndirectObj.m_nObjNumber = -1;
				this.m_arrObjPTRs.Add(pdfIndirectObj);
			}
			else
			{
				if (pObj.m_nType == enumType.pdfDictionary || pObj.m_nType == enumType.pdfArray)
				{
					PdfObjectList pdfObjectList = (PdfObjectList)pObj;
					int num = -1;
					PdfObject pdfObject = null;
					PdfIndirectObj ptr = null;
					for (int i = 0; i < pdfObjectList.Size; i++)
					{
						pdfObject = pdfObjectList.GetObject(i);
						if (pdfObject.m_nType == enumType.pdfRef)
						{
							ptr = this.RetrieveIndirObject(pDoc, pdfObject);
							num = i;
							break;
						}
						if (pdfObject.m_nType == enumType.pdfArray || pdfObject.m_nType == enumType.pdfDictionary)
						{
							this.RetrieveIndirObject(pDoc, pdfObject);
						}
					}
					if (num != -1)
					{
						pdfObjectList.Insert(new PdfReference(pdfObject.m_bstrType, ptr), num);
						pdfObjectList.RemoveByIndex(num + 1);
						this.RetrieveIndirObject(pDoc, pObj);
					}
				}
			}
			if (pdfIndirectObj != null)
			{
				if (pdfIndirectObj.m_pSimpleValue != null)
				{
					this.RetrieveIndirObject(pDoc, pdfIndirectObj.m_pSimpleValue);
				}
				else
				{
					PdfObject objectByName = pdfIndirectObj.m_objAttributes.GetObjectByName("Type");
					if (objectByName != null && objectByName.m_nType == enumType.pdfName && (((PdfName)objectByName).m_bstrName == "Page" || ((PdfName)objectByName).m_bstrName == "Pages"))
					{
						pdfIndirectObj.m_objAttributes.RemoveByName("Kids");
						pdfIndirectObj.m_objAttributes.RemoveByName("Contents");
					}
					this.RetrieveIndirObject(pDoc, pdfIndirectObj.m_objAttributes);
				}
			}
			return pdfIndirectObj;
		}
		public void SetViewerPrefs(object Param)
		{
			PdfParam pdfParam = this.m_pManager.VariantToParam(Param);
			PdfDict pdfDict = this.m_pCatalogObj.AddDict("ViewerPreferences");
			string[] array = new string[]
			{
				"HideToolbar",
				"HideMenubar",
				"HideWindowUI",
				"FitWindow",
				"CenterWindow",
				"DisplayDocTitle"
			};
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string name = array2[i];
				if (pdfParam.IsSet(name))
				{
					bool value = pdfParam.Bool(name);
					pdfDict.Add(new PdfBool(name, value));
				}
			}
			int num = -1;
			if (pdfParam.IsSet("NonFullScreenPageMode"))
			{
				num = pdfParam.Long("NonFullScreenPageMode");
				if (num < 1)
				{
					num = 1;
				}
				if (num > 3)
				{
					num = 3;
				}
			}
			if (num > 0)
			{
				pdfDict.Add(new PdfNumber("NonFullScreenPageMode", (double)num));
			}
			if (pdfParam.IsSet("PrintScaling") && !pdfParam.Bool("PrintScaling"))
			{
				pdfDict.Add(new PdfName("PrintScaling", "None"));
			}
			this.m_pCatalogObj.m_bModified = true;
		}
		internal PdfPages GetPages()
		{
			return new PdfPages
			{
				m_pDoc = this,
				m_pManager = this.m_pManager
			};
		}
		internal void MetaDataHelper()
		{
			if (this.m_pInput != null)
			{
				try
				{
					this.m_pInput.ParseMetaData();
				}
				catch (Exception ex)
				{
					this.m_pInput.Close();
					throw ex;
				}
			}
		}
		internal bool IsFontParsed(PdfRef pRef)
		{
			foreach (PdfFont current in this.m_ptrExistingFonts)
			{
				if (current.m_nObjNum == pRef.m_nObjNum && current.m_nGenNum == pRef.m_nGenNum)
				{
					return true;
				}
			}
			return false;
		}
		internal bool IsFontName(string Name)
		{
			foreach (PdfFont current in this.m_ptrExistingFonts)
			{
				if (current.m_bstrNameInResource == Name)
				{
					return true;
				}
			}
			return false;
		}
		internal bool IsToUnicodeParsed(PdfRef pRef)
		{
			foreach (PdfFont current in this.m_ptrExistingFonts)
			{
				if (current.m_nToUnicodeObjNum == pRef.m_nObjNum && current.m_nToUnicodeGenNum == pRef.m_nGenNum)
				{
					return true;
				}
			}
			return false;
		}
		internal PdfFont IsEncodingParsed(PdfRef pRef)
		{
			foreach (PdfFont current in this.m_ptrExistingFonts)
			{
				if (current.m_nEncodingObjNum == pRef.m_nObjNum && current.m_nEncodingGenNum == pRef.m_nGenNum)
				{
					return current;
				}
			}
			return null;
		}
		internal PdfFont AddExistingFont(PdfDict pFontDict, string NameInResource, int nObjNum, int nGenNum)
		{
			PdfFont pdfFont = new PdfFont();
			pdfFont.m_pManager = this.m_pManager;
			pdfFont.m_pDoc = this;
			pdfFont.m_nObjNum = nObjNum;
			pdfFont.m_nGenNum = nGenNum;
			pdfFont.m_bstrNameInResource = NameInResource;
			this.m_ptrExistingFonts.Add(pdfFont);
			PdfObject objectByName = pFontDict.GetObjectByName("Subtype");
			if (objectByName != null && objectByName.m_nType == enumType.pdfName)
			{
				pdfFont.m_bstrType = ((PdfName)objectByName).m_bstrName;
			}
			pdfFont.m_pFontObj = this.AddExistingIndirectObject(enumIndirectType.pdfIndirectResource, nObjNum, nGenNum, pFontDict);
			pdfFont.m_pFontObj.m_bModified = false;
			if (pdfFont.m_bstrType == "TrueType" || pdfFont.m_bstrType == "Type1")
			{
				objectByName = pFontDict.GetObjectByName("Name");
				if (objectByName != null && objectByName.m_nType == enumType.pdfName)
				{
					pdfFont.m_bstrFace = ((PdfName)objectByName).m_bstrName;
				}
				objectByName = pFontDict.GetObjectByName("FirstChar");
				if (objectByName != null && objectByName.m_nType == enumType.pdfNumber)
				{
					pdfFont.m_nFirstChar = (int)((PdfNumber)objectByName).m_fValue;
				}
				objectByName = pFontDict.GetObjectByName("LastChar");
				if (objectByName != null && objectByName.m_nType == enumType.pdfNumber)
				{
					pdfFont.m_nLastChar = (int)((PdfNumber)objectByName).m_fValue;
				}
				objectByName = pFontDict.GetObjectByName("Widths");
				if (objectByName != null && objectByName.m_nType == enumType.pdfArray)
				{
					int size = ((PdfArray)objectByName).Size;
					pdfFont.m_pWidths = new ushort[size];
					for (int i = 0; i < size; i++)
					{
						PdfObject @object = ((PdfArray)objectByName).GetObject(i);
                        pdfFont.m_pWidths[i] = @object == null || @object.m_nType != enumType.pdfNumber ? (ushort)0 : (ushort)((PdfNumber)@object).m_fValue;
					}
				}
				objectByName = pFontDict.GetObjectByName("FontDescriptor");
				if (objectByName != null && objectByName.m_nType == enumType.pdfDictionary)
				{
					PdfDict pdfDict = (PdfDict)objectByName;
					objectByName = pdfDict.GetObjectByName("FontBBox");
					if (objectByName != null && objectByName.m_nType == enumType.pdfArray && ((PdfArray)objectByName).Size == 4)
					{
						PdfArray pdfArray = (PdfArray)objectByName;
						pdfFont.m_rectFontBox.Left = ((pdfArray.GetObject(0).m_nType == enumType.pdfNumber) ? ((float)((PdfNumber)pdfArray.GetObject(0)).m_fValue) : 0f);
						pdfFont.m_rectFontBox.Bottom = ((pdfArray.GetObject(1).m_nType == enumType.pdfNumber) ? ((float)((PdfNumber)pdfArray.GetObject(1)).m_fValue) : 0f);
						pdfFont.m_rectFontBox.Right = ((pdfArray.GetObject(2).m_nType == enumType.pdfNumber) ? ((float)((PdfNumber)pdfArray.GetObject(2)).m_fValue) : 0f);
						pdfFont.m_rectFontBox.Top = ((pdfArray.GetObject(3).m_nType == enumType.pdfNumber) ? ((float)((PdfNumber)pdfArray.GetObject(3)).m_fValue) : 0f);
					}
				}
			}
			if (pdfFont.m_bstrType == "Type1")
			{
				string text = null;
				objectByName = pFontDict.GetObjectByName("BaseFont");
				if (objectByName != null && objectByName.m_nType == enumType.pdfName)
				{
					text = ((PdfName)objectByName).m_bstrName;
				}
				if (text != null)
				{
					for (int j = 0; j < PdfFonts.m_arrFontNames.Length; j++)
					{
						if (string.Compare(PdfFonts.m_arrFontNames[j], text, true) == 0)
						{
							pdfFont.m_nFirstChar = 0;
							pdfFont.m_nLastChar = 255;
							pdfFont.m_pWidths = new ushort[256];
							Array.Copy(PdfFont.m_arrFontWidths[j], pdfFont.m_pWidths, PdfFont.m_arrFontWidths[j].Length);
							pdfFont.m_nDescent = (int)PdfFont.m_arrDescents[j];
							pdfFont.m_nAscent = (int)(PdfFont.m_arrVerticalExtents[j] + (ushort)PdfFont.m_arrDescents[j]);
							pdfFont.m_rectFontBox.Bottom = (float)pdfFont.m_nDescent;
							pdfFont.m_rectFontBox.Top = (float)pdfFont.m_nAscent;
							pdfFont.m_bstrEncoding = "PDFDocEncoding";
							break;
						}
					}
				}
			}
			return pdfFont;
		}
		internal void ConvertAndAppend(int nFontObjNum, int nFontGenNum, PdfString objString, string Delimiter, int nCodePage, ref StringBuilder sb)
		{
			string value;
			foreach (PdfFont current in this.m_ptrExistingFonts)
			{
				if (current.m_nObjNum == nFontObjNum && current.m_nGenNum == nFontGenNum)
				{
					value = current.ConvertToUnicode(objString, nCodePage);
					if (Delimiter != null)
					{
						sb.Append(Delimiter);
					}
					sb.Append(value);
					return;
				}
			}
			value = objString.ToString();
			if (Delimiter != null)
			{
				sb.Append(Delimiter);
			}
			sb.Append(value);
		}
		public PdfAnnot Sign(X509Certificate2 SignerCert, string Name, string Reason, string Location, object Param)
		{
			if (this.m_bSigned)
			{
				AuxException.Throw("Sign method cannot be called multiple times.", PdfErrors._ERROR_INVALIDARG);
			}
			if (SignerCert == null)
			{
				AuxException.Throw("SignerCert argument not specified.", PdfErrors._ERROR_INVALIDARG);
			}
			if (!SignerCert.HasPrivateKey)
			{
				AuxException.Throw("The specified certificate does not have a private key.", PdfErrors._ERROR_INVALIDARG);
			}
			this.m_pSignerCert = SignerCert;
			PdfParam pdfParam = this.m_pManager.VariantToParam(Param);
			int index = 1;
			if (pdfParam.IsTrue("Visible"))
			{
				if (pdfParam.IsSet("PageIndex"))
				{
					index = pdfParam.Long("PageIndex");
				}
			}
			else
			{
				pdfParam.Add("x=0; y=0; width=0; height=0");
			}
			if (this.Pages.Count == 0)
			{
				AuxException.Throw("Document contains no pages.", PdfErrors._ERROR_SIGNFAILED);
			}
			PdfPage pdfPage = this.Pages[index];
			PdfAnnot pdfAnnot = pdfPage.CreateFieldHelper(FieldType.FieldSignature, FieldType.FieldSignature, null, Name, pdfParam);
			PdfIndirectObj pdfIndirectObj = this.AddNewIndirectObject(enumIndirectType.pdfIndirectSignature);
			pdfAnnot.m_pAnnotObj.AddReference("V", pdfIndirectObj);
			pdfIndirectObj.AddName("Type", "Sig");
			pdfIndirectObj.AddName("Filter", pdfParam.IsTrue("Windows") ? "Adobe.PPKMS" : "VeriSign.PPKVS");
			pdfIndirectObj.AddName("SubFilter", "adbe.pkcs7.detached");
			if (Name != null)
			{
				pdfIndirectObj.AddString("Name", Name);
			}
			if (Reason != null)
			{
				pdfIndirectObj.AddString("Reason", Reason);
			}
			if (Location != null)
			{
				pdfIndirectObj.AddString("Location", Location);
			}
			pdfIndirectObj.AddDate("M", DateTime.Now);
			pdfIndirectObj.AddInt("R", 65537);
			if (pdfParam.IsSet("BinarySize"))
			{
				this.m_nSignatureSize = pdfParam.Long("BinarySize") * 2;
			}
			else
			{
				byte[] array = this.SignBinaryMessage(SignerCert, new byte[]
				{
					65
				});
				this.m_nSignatureSize = array.Length * 2;
			}
			PdfName pdfName = new PdfName("Contents", null);
			pdfName.m_bstrName = new string('A', this.m_nSignatureSize + 1);
			pdfIndirectObj.m_objAttributes.Add(pdfName);
			PdfArray pdfArray = pdfIndirectObj.AddArray("ByteRange");
			pdfArray.Add(new PdfNumber(null, 99999.0));
			pdfArray.Add(new PdfNumber(null, 99999.0));
			pdfArray.Add(new PdfNumber(null, 99999.0));
			pdfArray.Add(new PdfNumber(null, 99999.0));
			pdfArray.Add(new PdfNumber(null, 99999.0));
			pdfArray.Add(new PdfNumber(null, 99999.0));
			pdfArray.Add(new PdfNumber(null, 99999.0));
			pdfArray.Add(new PdfNumber(null, 99999.0));
			this.m_bSigned = true;
			return pdfAnnot;
		}
		internal void HandleSignature(int nSigLocation)
		{
			AuxFile auxFile = new AuxFile();
			auxFile.OpenForWriting(this.m_bstrSavedPath);
			int size = auxFile.Size;
			auxFile.SetPointer(nSigLocation - 60);
			int num = this.m_nSignatureSize + 2;
			int num2 = 73;
			string text = string.Format("{0} {1} {2} {3} ]", new object[]
			{
				0,
				nSigLocation - num2 - num,
				nSigLocation - num2,
				size - nSigLocation + num2
			});
			if (text.Length > 48)
			{
				AuxException.Throw("Unexpected error when inserting signature.", PdfErrors._ERROR_SIGNATURE, nSigLocation);
			}
			text = text.PadRight(48);
			auxFile.Write(Encoding.UTF8.GetBytes(text));
			auxFile.Close();
			byte[] array = this.SignFileParts(this.m_bstrSavedPath, 0, nSigLocation - num2 - num, nSigLocation - num2, size - nSigLocation + num2);
			if (this.m_nSignatureSize / 2 < array.Length)
			{
				AuxException.Throw(string.Format("Requested signature size (BinarySize parameter) is smaller than necessary ({0} bytes).", array.Length), PdfErrors._ERROR_SIGNATURE, nSigLocation);
			}
			PdfStream pdfStream = new PdfStream();
			pdfStream.Append(array);
			if (this.m_nSignatureSize / 2 > array.Length)
			{
				byte[] buffer = new byte[this.m_nSignatureSize / 2 - array.Length];
				pdfStream.Append(buffer);
			}
			string s = "<" + pdfStream.ToString() + ">";
			auxFile.OpenForWriting(this.m_bstrSavedPath);
			auxFile.SetPointer(nSigLocation - num2 - num);
			auxFile.Write(Encoding.UTF8.GetBytes(s));
			auxFile.Close();
		}
		internal byte[] SignBinaryMessage(X509Certificate2 SignerCert, byte[] message)
		{
			ContentInfo contentInfo = new ContentInfo(message);
			SignedCms signedCms = new SignedCms(contentInfo, true);
			CmsSigner signer = new CmsSigner(SignerCert);
			signedCms.ComputeSignature(signer, true);
			return signedCms.Encode();
		}
		internal byte[] SignFileParts(string Path, int nFrom1, int nLength1, int nFrom2, int nLength2)
		{
			AuxFile auxFile = new AuxFile();
			auxFile.Open(Path);
			byte[] array = new byte[nLength1 + nLength2];
			auxFile.SetPointer(nFrom1);
			auxFile.Read(array, 0, nLength1);
			auxFile.SetPointer(nFrom2);
			auxFile.Read(array, nLength1, nLength2);
			auxFile.Close();
			return this.SignBinaryMessage(this.m_pSignerCert, array);
		}
		public PdfSignature VerifySignature()
		{
			if (!this.m_bExisting)
			{
				AuxException.Throw("VerifySignature method can only be called on an existing document.", PdfErrors._ERROR_VERIFYSIG);
			}
			if (this.m_pBinaryDoc != null)
			{
				AuxException.Throw("VerifySignature method cannot be called on memory documents. Use OpenDocument(string) instead of OpenDocument(byte []).", PdfErrors._ERROR_VERIFYSIG);
			}
			PdfForm form = this.Form;
			PdfSignature pdfSignature = new PdfSignature();
			pdfSignature.m_pManager = this.m_pManager;
			pdfSignature.m_pDoc = this;
			foreach (PdfAnnot current in form.m_arrFields)
			{
				PdfObject objectByName = current.m_pAnnotObj.m_objAttributes.GetObjectByName("V");
				if (objectByName != null && (objectByName.m_nType == enumType.pdfDictionary || objectByName.m_nType == enumType.pdfRef))
				{
					if (objectByName.m_nType == enumType.pdfDictionary)
					{
						pdfSignature.Populate((PdfDict)objectByName, current.m_pAnnotObj.m_nObjNumber, current.m_pAnnotObj.m_nGenNumber);
					}
					else
					{
						if (this.m_pInput == null)
						{
							AuxException.Throw("Document file is closed.", PdfErrors._ERROR_VERIFYSIG);
						}
						try
						{
							this.m_pInput.ParseSignature((PdfRef)objectByName, pdfSignature);
						}
						catch (Exception ex)
						{
							this.m_pInput.Close();
							throw ex;
						}
					}
				}
			}
			if (!pdfSignature.m_bPopulated)
			{
				return null;
			}
			pdfSignature.m_bStatus = this.VerifySignatureFileParts(pdfSignature.m_objContents.ToBytes(), this.m_bstrPath, pdfSignature.m_dwFrom1, pdfSignature.m_dwLength1, pdfSignature.m_dwFrom2, pdfSignature.m_dwLength2);
			return pdfSignature;
		}
		internal bool VerifySignatureFileParts(byte[] Signature, string Path, int nFrom1, int nLength1, int nFrom2, int nLength2)
		{
			AuxFile auxFile = new AuxFile();
			auxFile.Open(Path);
			byte[] array = new byte[nLength1 + nLength2];
			auxFile.SetPointer(nFrom1);
			auxFile.Read(array, 0, nLength1);
			auxFile.SetPointer(nFrom2);
			auxFile.Read(array, nLength1, nLength2);
			auxFile.Close();
			ContentInfo contentInfo = new ContentInfo(array);
			SignedCms signedCms = new SignedCms(contentInfo, true);
			signedCms.Decode(Signature);
			try
			{
				signedCms.CheckSignature(true);
			}
			catch (CryptographicException)
			{
				return false;
			}
			return true;
		}
		internal PdfDocument()
		{
			this.m_pCatalogObj = null;
			this.m_pPageRootObj = null;
			this.m_pInfoObj = null;
			this.m_pDefaultFontObj = null;
			this.m_pEncDict = null;
			this.m_pMetaDataObj = null;
			this.m_pKey = null;
			this.m_pTrailer = null;
			this.m_bEncrypted = false;
			this.m_bExisting = false;
			this.m_bCompressedXRef = false;
			this.m_bInfoSet = false;
			this.m_bstrTitle = "";
			this.m_bstrAuthor = "";
			this.m_bstrSubject = "";
			this.m_bstrKeywords = "";
			this.m_bstrCreator = "";
			this.m_nMasterObjectCount = 1;
			this.m_dwLastXrefPos = 0;
		}
		public void Dispose()
		{
			this.Close();
		}
		internal PdfIndirectObj AddNewIndirectObject(enumIndirectType Type)
		{
			PdfIndirectObj pdfIndirectObj = new PdfIndirectObj();
			pdfIndirectObj.m_nType = Type;
			pdfIndirectObj.m_pManager = this.m_pManager;
			this.m_ptrIndirectObjects.Add(pdfIndirectObj);
			return pdfIndirectObj;
		}
		internal PdfIndirectObj AddExistingIndirectObject(enumIndirectType Type, int ObjNum, int GenNum, PdfObject pObj)
		{
			foreach (PdfIndirectObj current in this.m_ptrIndirectObjects)
			{
				if (current.m_nObjNumber == ObjNum && current.m_nGenNumber == GenNum)
				{
					return current;
				}
			}
			PdfIndirectObj pdfIndirectObj = this.AddNewIndirectObject(Type);
			pdfIndirectObj.m_bExisting = true;
			pdfIndirectObj.m_bModified = false;
			pdfIndirectObj.m_nObjNumber = ObjNum;
			pdfIndirectObj.m_nGenNumber = GenNum;
			if (pObj.m_nType == enumType.pdfDictionary)
			{
				pdfIndirectObj.m_objAttributes.CopyItems((PdfDict)pObj);
			}
			else
			{
				pdfIndirectObj.m_pSimpleValue = pObj.Copy();
			}
			return pdfIndirectObj;
		}
		internal void RemoveIndirectObject(PdfIndirectObj pObj)
		{
			if (pObj == null)
			{
				return;
			}
			foreach (PdfObjectList current in pObj.m_arrBackPointers)
			{
				current.RemoveReference(pObj);
			}
			if (pObj.m_pParentArrayOfKids != null)
			{
				pObj.m_pParentArrayOfKids.RemoveRef(pObj.m_nObjNumber, pObj.m_nGenNumber);
				if (pObj.m_pParentArrayOfKids.m_pParent != null)
				{
					((PdfIndirectObj)pObj.m_pParentArrayOfKids.m_pParent).m_bModified = true;
				}
			}
			foreach (PdfIndirectObj current2 in this.m_ptrIndirectObjects)
			{
				if (current2 == pObj)
				{
					if (current2.m_bExisting)
					{
						current2.m_bModified = true;
						current2.m_bDeleted = true;
						break;
					}
					this.m_ptrIndirectObjects.Remove(current2);
					break;
				}
			}
		}
		internal void Init(string ID)
		{
			this.m_bstrProducer = "PDFLive 4.0 - Hernán Javier Hegykozi (hernanjh@gmail.com)";
			this.m_pCatalogObj = this.AddNewIndirectObject(enumIndirectType.pdfIndirectCatalog);
			this.m_pCatalogObj.AddName("Type", "Catalog");
			this.m_pPageRootObj = this.AddNewIndirectObject(enumIndirectType.pdfIndirectPageRoot);
			this.m_pPageRootObj.AddName("Type", "Pages");
			this.m_pCatalogObj.AddReference("Pages", this.m_pPageRootObj);
			this.m_pInfoObj = this.AddNewIndirectObject(enumIndirectType.pdfIndirectInfo);
			this.Producer = this.m_bstrProducer;
			this.m_objXRef.AddEntry(0, 0, 65535, 102);
			if (ID != null)
			{
				this.m_objID.Set(ID);
				this.m_objID2.Set(ID);
			}
			else
			{
				this.m_objID.SetRandomID();
			}
			this.m_objID2.Set(this.m_objID);
		}
		internal bool Open(string Path, byte[] Blob, string Password)
		{
			try
			{
				if (Blob == null)
				{
					this.m_pInput = new PdfInput(Path, this, Password);
					this.m_bstrPath = Path;
				}
				else
				{
					this.m_pInput = new PdfInput(Blob, this, Password);
					this.m_pBinaryDoc = Blob;
				}
				this.m_pInput.Parse();
			}
			catch (Exception ex)
			{
				if (this.m_pInput != null)
				{
					this.m_pInput.Close();
				}
				if (ex.Message.IndexOf("Invalid password") >= 0)
				{
					return false;
				}
				throw ex;
			}
			return true;
		}
		public void Close()
		{
			if (this.m_pInput != null)
			{
				this.m_pInput.Close();
			}
		}
		public string Save(string Path)
		{
			return this.Save(Path, true);
		}
		public string Save(string Path, bool Overwrite)
		{
			if (this.m_bSaved)
			{
				AuxException.Throw("A document cannot be saved more than once .", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			AuxFile auxFile = new AuxFile(Path);
			if (!Overwrite)
			{
				auxFile.CreateUniqueFileName();
			}
			this.m_bstrSavedPath = auxFile.m_bstrPath;
			PdfOutputToFile objOutput = new PdfOutputToFile(65536, auxFile.m_bstrPath);
			this.Dump(objOutput);
			this.m_bSaved = true;
			this.m_bstrOriginalFilename = auxFile.ExtractFileName();
			this.m_bstrFilename = auxFile.m_bstrFileName;
			return auxFile.m_bstrFileName;
		}
		public byte[] SaveToMemory()
		{
			if (this.m_bSaved)
			{
				AuxException.Throw("A document cannot be saved more than once .", PdfErrors._ERROR_PARAMNOTSPECIFIED);
			}
			PdfOutput pdfOutput = new PdfOutput(65536);
			this.Dump(pdfOutput);
			this.m_bSaved = true;
			return pdfOutput.ToBytes();
		}
		public void SaveHttp(string DispHeader)
		{
			this.SaveHttp(DispHeader, "application/pdf");
		}
		public void SaveHttp(string DispHeader, string ContentType)
		{
			PdfOutputToHttp objOutput = new PdfOutputToHttp(65536, DispHeader, ContentType);
			this.Dump(objOutput);
			this.m_bSaved = true;
		}
		internal void Dump(PdfOutput objOutput)
		{
			int num = 0;
			objOutput.Open();
			if (this.m_bEncrypted)
			{
				objOutput.m_bEncrypt = true;
				objOutput.m_pMasterKey = new PdfStream(this.m_pKey);
				objOutput.m_pEncKey = new PdfStream(this.m_pKey);
			}
			if (!this.m_bExisting)
			{
				objOutput.Write("%PDF-", ref num);
				objOutput.Write(this.m_bstrVersion, ref num);
				objOutput.Write("\n%Çì\u008f¢\n", ref num);
			}
			else
			{
				this.DumpExistingFile(objOutput);
				objOutput.Write("\n", ref num);
			}
			if (this.m_objExistingXRef.m_nMaxNum > 0)
			{
				this.m_nMasterObjectCount = this.m_objExistingXRef.m_nMaxNum + 1;
			}
			int num2 = 0;
			foreach (PdfIndirectObj current in this.m_ptrIndirectObjects)
			{
				if (!current.m_bExisting)
				{
					current.m_nObjNumber = this.m_nMasterObjectCount++;
					num2++;
				}
			}
			foreach (PdfFont current2 in this.m_ptrFonts)
			{
				current2.CreateSubset();
			}
			this.HandleAppendedDocuments(this.m_nMasterObjectCount - 1, ref num2);
			foreach (PdfIndirectObj current3 in this.m_ptrIndirectObjects)
			{
				if (current3.m_bModified)
				{					
                    this.m_objXRef.AddEntry(objOutput.CurrentLocation(), current3.m_nObjNumber, current3.m_nGenNumber, current3.m_bDeleted ? (byte)102 : (byte)110);
                    if (!current3.m_bEncryptable)
					{
						objOutput.m_bEncrypt = false;
					}
					objOutput.SetObjectID((long)current3.m_nObjNumber, (long)current3.m_nGenNumber);
					if (!current3.m_bDeleted)
					{
						current3.WriteOut(objOutput);
					}
					if (!current3.m_bEncryptable)
					{
						objOutput.m_bEncrypt = true;
					}
				}
			}
			if (this.m_objXRef.Size == 0)
			{
				objOutput.Flush();
				objOutput.Close();
				return;
			}
			if (!this.m_bExisting)
			{
				this.m_objXRef.CheckForHoles();
			}
			int num3 = objOutput.CurrentLocation();
			if (this.m_bCompressedXRef)
			{
				this.DumpCompressedXRef(objOutput, num3, this.m_nMasterObjectCount, num2);
				return;
			}
			this.m_objXRef.WriteOut(objOutput);
			objOutput.Write("trailer\n", ref num);
			PdfDict pdfDict = new PdfDict(true);
			pdfDict.Add(new PdfNumber("Size", (double)(this.m_objExistingXRef.NextObjNumber() + num2)));
			if (!this.m_bExisting)
			{
				pdfDict.Add(new PdfReference("Root", this.m_ptrIndirectObjects[0]));
				if (this.m_bInfoSet)
				{
					pdfDict.Add(new PdfReference("Info", this.m_pInfoObj));
				}
			}
			else
			{
				pdfDict.Add(new PdfRef((PdfRef)this.m_pTrailer.GetObjectByName("Root")));
				PdfRef pdfRef = (PdfRef)this.m_pTrailer.GetObjectByName("Info");
				if (pdfRef != null)
				{
					pdfDict.Add(new PdfRef(pdfRef));
				}
				else
				{
					if (this.m_bInfoSet)
					{
						pdfDict.Add(new PdfReference("Info", this.m_pInfoObj));
					}
				}
			}
			if (this.m_dwLastXrefPos > 0)
			{
				pdfDict.Add(new PdfNumber("Prev", (double)this.m_dwLastXrefPos));
			}
			PdfArray pdfArray = new PdfArray("ID");
			pdfArray.Add(new PdfString(null, this.m_objID));
			pdfArray.Add(new PdfString(null, this.m_objID2));
			pdfDict.Add(pdfArray);
			if (this.m_pEncDict != null)
			{
				pdfDict.Add(new PdfReference("Encrypt", this.m_pEncDict));
			}
			objOutput.m_bEncrypt = false;
			pdfDict.WriteOut(objOutput);
			objOutput.Write("startxref\n", ref num);
			objOutput.Write(num3, ref num);
			objOutput.Write("\n%%EOF\n", ref num);
			objOutput.Flush();
			objOutput.Close();
			if (this.m_bSigned)
			{
				this.HandleSignature(objOutput.m_nSignatureLocation);
			}
		}
		public void Encrypt(string OwnerPassword, string UserPassword, int KeyLength)
		{
			this.Encrypt(OwnerPassword, UserPassword, KeyLength, (pdfPermissions)4294967292u);
		}
		public void Encrypt(string OwnerPassword, string UserPassword)
		{
			this.Encrypt(OwnerPassword, UserPassword, 40, (pdfPermissions)4294967292u);
		}
		public void Encrypt(string OwnerPassword, string UserPassword, int KeyLength, pdfPermissions Permissions)
		{
			if (this.m_bEncrypted)
			{
				AuxException.Throw("Document already encrypted.", PdfErrors._ERROR_DOUBLECALLTOMETHOD);
			}
			if (this.m_bExisting)
			{
				AuxException.Throw("An existing PDF document cannot be encrypted directly. Create an empty document with Pdf.CreateDocument, append your existing document to it with Doc.AppendDocument, then call Doc.Encrypt.", PdfErrors._ERROR_DOUBLECALLTOMETHOD);
			}
			int num = KeyLength;
			string text = OwnerPassword;
			bool bAES = false;
			if (num == -128)
			{
				num = 128;
				bAES = true;
			}
			if (num != 40 && num != 128)
			{
				AuxException.Throw("Key Length must be set to 40 (RC4), 128 (RC4) or -128 (AES).", PdfErrors._ERROR_INVALIDARGUMENT);
			}
			uint num2 = (uint)(Permissions | (pdfPermissions)4294963392u);
			num2 = num2 >> 2 << 2;
			if (text.Length == 0)
			{
				text = UserPassword;
			}
			this.m_bEncrypted = true;
			this.m_pEncDict = this.AddNewIndirectObject(enumIndirectType.pdfIndirectEncDict);
			this.m_pKey = this.CreateEncryptionDict(text, UserPassword, num, num2, this.m_pEncDict, bAES);
		}
		internal PdfStream CreateEncryptionDict(string OwnerPassword, string UserPassword, int nKeyLength, uint nPermissions, PdfIndirectObj pDictObj, bool bAES)
		{
			bool flag = false;
			if (nKeyLength > 40)
			{
				flag = true;
			}
			PdfStream pdfStream = new PdfStream();
			pdfStream.MD5FromPaddedString(OwnerPassword, flag ? 50 : 0);
			PdfStream pdfStream2 = new PdfStream();
			pdfStream2.PadString(UserPassword);
			pdfStream2.Encrypt(pdfStream, nKeyLength / 8);
			if (flag)
			{
				for (int i = 1; i <= 19; i++)
				{
					byte[] array = new byte[nKeyLength / 8];
					for (int j = 0; j < nKeyLength / 8; j++)
					{
                        array[j] = (byte)((uint)pdfStream[j] ^ (uint)(byte)i);
					}
					pdfStream2.Encrypt(array);
				}
			}
			PdfStream pdfStream3 = new PdfStream();
			pdfStream3.PadString(UserPassword);
			PdfHash pdfHash = new PdfHash();
			pdfHash.Init();
			pdfHash.Update(pdfStream3);
			pdfHash.Update(pdfStream2);
			byte[] bytes = BitConverter.GetBytes(nPermissions);
			pdfHash.Update(bytes);
			pdfHash.Update(this.m_objID);
			pdfHash.Final();
			if (flag)
			{
				for (int k = 0; k < 50; k++)
				{
					pdfHash.HashItself();
				}
			}
			PdfStream pdfStream4 = new PdfStream();
			pdfStream4.PadString("");
			PdfStream pdfStream5 = new PdfStream();
			if (flag)
			{
				PdfHash pdfHash2 = new PdfHash();
				pdfHash2.Init();
				pdfHash2.Update(pdfStream4);
				pdfHash2.Update(this.m_objID);
				pdfHash2.Final();
				pdfHash2.Encrypt(pdfHash, nKeyLength / 8);
				for (int l = 1; l <= 19; l++)
				{
					byte[] array2 = new byte[nKeyLength / 8];
					for (int m = 0; m < nKeyLength / 8; m++)
					{
                        array2[m] = (byte)((uint)pdfHash[m] ^ (uint)(byte)l);
					}
					pdfHash2.Encrypt(array2);
				}
				byte[] buffer = new byte[]
				{
					49,
					65,
					89,
					38,
					83,
					88,
					151,
					147,
					35,
					132,
					98,
					100,
					51,
					131,
					39,
					149
				};
				pdfHash2.Append(buffer);
				pdfStream5.Set(pdfHash2);
			}
			else
			{
				pdfStream4.Encrypt(pdfHash, nKeyLength / 8);
				pdfStream5.Set(pdfStream4);
			}
			pDictObj.AddName("Filter", "Standard");
			pDictObj.AddInt("V", bAES ? 4 : ((nKeyLength > 40) ? 2 : 1));
			pDictObj.AddInt("R", bAES ? 4 : (flag ? 3 : 2));
			pDictObj.AddInt("P", (int)nPermissions);
			pDictObj.AddString("O", pdfStream2);
			pDictObj.AddString("U", pdfStream5);
			if (flag)
			{
				pDictObj.AddInt("Length", nKeyLength);
			}
			if (bAES)
			{
				pDictObj.AddName("StrF", "StdCF");
				pDictObj.AddName("StmF", "StdCF");
				PdfDict pdfDict = pDictObj.AddDict("CF");
				PdfDict pdfDict2 = new PdfDict("StdCF");
				pdfDict2.Add(new PdfName("AuthEvent", "DocOpen"));
				pdfDict2.Add(new PdfName("CFM", "AESV2"));
				pdfDict.Add(pdfDict2);
			}
			pDictObj.m_bEncryptable = false;
			this.m_nPermissions = (pdfPermissions)nPermissions;
			this.m_bEncrypted = true;
			byte[] sourceArray = pdfHash.m_objMemStream.ToArray();
			byte[] array3 = new byte[nKeyLength / 8];
			Array.Copy(sourceArray, array3, nKeyLength / 8);
			PdfStream pdfStream6 = new PdfStream();
			pdfStream6.Append(array3);
			if (bAES)
			{
				pdfStream6.m_nPtr = 1;
			}
			return pdfStream6;
		}
		public PdfImage OpenImage(string Path, object Param)
		{
			return this.OpenImageHelper(Path, null, Param);
		}
		public PdfImage OpenImage(byte[] Blob, object Param)
		{
			return this.OpenImageHelper(null, Blob, Param);
		}
		public PdfImage OpenImage(string Path)
		{
			return this.OpenImageHelper(Path, null, null);
		}
		public PdfImage OpenImage(byte[] Blob)
		{
			return this.OpenImageHelper(null, Blob, null);
		}
		internal PdfImage OpenImageHelper(string Path, byte[] Blob, object Param)
		{
			PdfPages arg_06_0 = this.Pages;
			PdfParam pdfParam = this.m_pManager.VariantToParam(Param);
			PdfImage pdfImage = new PdfImage();
			pdfImage.m_pManager = this.m_pManager;
			pdfImage.m_pDoc = this;
			if (pdfParam.IsSet("Invert"))
			{
				pdfImage.m_bInvert = pdfParam.Bool("Invert");
			}
			if (pdfParam.IsSet("Interpolate"))
			{
				pdfImage.m_bInterpolate = pdfParam.Bool("Interpolate");
			}
			if (pdfParam.IsSet("Index"))
			{
				pdfImage.m_nTiffIndex = pdfParam.Long("Index");
			}
			if (!pdfImage.Open(Path, Blob))
			{
				return null;
			}
			return pdfImage;
		}
		public PdfGraphics CreateGraphics(object Param)
		{
			PdfParam pParam = this.m_pManager.VariantToParam(Param);
			PdfGraphics pdfGraphics = new PdfGraphics();
			pdfGraphics.m_pManager = this.m_pManager;
			pdfGraphics.m_pDoc = this;
			pdfGraphics.Create(pParam);
			return pdfGraphics;
		}
		public PdfTable CreateTable(object Param)
		{
			PdfParam pParam = this.m_pManager.VariantToParam(Param);
			PdfTable pdfTable = new PdfTable();
			pdfTable.m_pManager = this.m_pManager;
			pdfTable.m_pDoc = this;
			pdfTable.Create(pParam);
			return pdfTable;
		}
		internal void DumpExistingFile(PdfOutput objOutput)
		{
			int num = 0;
			if (this.m_pBinaryDoc == null)
			{
				AuxFile auxFile = new AuxFile();
				byte[] array = new byte[10000];
				auxFile.Open(this.m_bstrPath);
				bool flag = true;
				while (true)
				{
					int num2 = auxFile.Read(array);
					if (num2 == 0)
					{
						break;
					}
					if (flag && num2 > 7 && array[7] < (byte)this.m_bstrVersion[2])
					{
						array[7] = (byte)this.m_bstrVersion[2];
					}
					int num3 = 0;
					if (flag && this.m_bIsMac)
					{
						num3 = this.m_nMacBinarySize;
					}
					objOutput.Write(array, num3, num2 - num3, ref num);
					flag = false;
				}
				auxFile.Close();
				return;
			}
			int num4 = this.m_pBinaryDoc.Length;
			int nStart = 0;
			if (this.m_bIsMac)
			{
				nStart = this.m_nMacBinarySize;
				num4 -= this.m_nMacBinarySize;
			}
			if (this.m_pBinaryDoc[7] < (byte)this.m_bstrVersion[2])
			{
				this.m_pBinaryDoc[7] = (byte)this.m_bstrVersion[2];
			}
			objOutput.Write(this.m_pBinaryDoc, nStart, num4, ref num);
		}
		internal void DumpCompressedXRef(PdfOutput objOutput, int nXrefStart, int nMasterObjCount, int nNewObjCount)
		{
			int num = 0;
			PdfIndirectObj pdfIndirectObj = this.AddNewIndirectObject(enumIndirectType.pdfIndirectStream);
			pdfIndirectObj.m_nObjNumber = nMasterObjCount;
			pdfIndirectObj.m_bEncryptable = false;
			pdfIndirectObj.AddName("Type", "XRef");
			PdfArray pdfArray = new PdfArray("ID");
			pdfArray.Add(new PdfString(null, this.m_objID));
			pdfArray.Add(new PdfString(null, this.m_objID2));
			pdfIndirectObj.m_objAttributes.Add(pdfArray);
			pdfIndirectObj.m_objAttributes.Add(new PdfNumber("Size", (double)(this.m_objExistingXRef.NextObjNumber() + nNewObjCount)));
			if (!this.m_bExisting)
			{
				pdfIndirectObj.m_objAttributes.Add(new PdfReference("Root", this.m_ptrIndirectObjects[0]));
				if (this.m_bInfoSet)
				{
					pdfIndirectObj.m_objAttributes.Add(new PdfReference("Info", this.m_pInfoObj));
				}
			}
			else
			{
				pdfIndirectObj.m_objAttributes.Add(new PdfRef((PdfRef)this.m_pTrailer.GetObjectByName("Root")));
				PdfRef pdfRef = (PdfRef)this.m_pTrailer.GetObjectByName("Info");
				if (pdfRef != null)
				{
					pdfIndirectObj.m_objAttributes.Add(new PdfRef(pdfRef));
				}
				else
				{
					if (this.m_bInfoSet)
					{
						pdfIndirectObj.m_objAttributes.Add(new PdfReference("Info", this.m_pInfoObj));
					}
				}
			}
			if (this.m_dwLastXrefPos > 0)
			{
				pdfIndirectObj.m_objAttributes.Add(new PdfNumber("Prev", (double)this.m_dwLastXrefPos));
			}
			if (this.m_pEncDict != null)
			{
				pdfIndirectObj.m_objAttributes.Add(new PdfReference("Encrypt", this.m_pEncDict));
			}
			this.m_objXRef.GenerateCompressedXRef(pdfIndirectObj.m_objAttributes, pdfIndirectObj.m_objStream);
			objOutput.m_bEncrypt = false;
			pdfIndirectObj.WriteOut(objOutput);
			objOutput.Write("startxref\n", ref num);
			objOutput.Write(nXrefStart, ref num);
			objOutput.Write("\n%%EOF\n", ref num);
			objOutput.Flush();
			objOutput.Close();
			if (this.m_bSigned)
			{
				this.HandleSignature(objOutput.m_nSignatureLocation);
			}
		}
		public PdfDest CreateDest(PdfPage Page)
		{
			return this.CreateDest(Page, null);
		}
		public PdfDest CreateDest(PdfPage Page, object Param)
		{
			PdfParam pParam = this.m_pManager.VariantToParam(Param);
			PdfDest pdfDest = new PdfDest();
			pdfDest.m_pManager = this.m_pManager;
			pdfDest.m_pDoc = this;
			pdfDest.Create(Page, pParam);
			return pdfDest;
		}
		public PdfAction CreateAction(object Param, string Value)
		{
			PdfParam pParam = this.m_pManager.VariantToParam(Param);
			PdfAction pdfAction = new PdfAction();
			pdfAction.m_pManager = this.m_pManager;
			pdfAction.m_pDoc = this;
			pdfAction.Create(pParam, Value);
			return pdfAction;
		}
		internal PdfOutline GetOutline()
		{
			if (this.m_ptrOutline != null)
			{
				return this.m_ptrOutline;
			}
			PdfOutline pdfOutline = new PdfOutline();
			pdfOutline.m_pManager = this.m_pManager;
			pdfOutline.m_pDoc = this;
			pdfOutline.Create();
			this.m_ptrOutline = pdfOutline;
			return pdfOutline;
		}
		internal PdfAnnot FindAnnot(PdfRef pRef)
		{
			foreach (PdfPage pdfPage in this.Pages)
			{
				foreach (PdfAnnot pdfAnnot in pdfPage.Annots)
				{
					if (pdfAnnot.m_pAnnotObj.m_nObjNumber == pRef.m_nObjNum && pdfAnnot.m_pAnnotObj.m_nGenNumber == pRef.m_nGenNum)
					{
						PdfAnnot result = pdfAnnot;
						return result;
					}
				}
			}
			PdfForm form = this.Form;
			foreach (PdfAnnot pdfAnnot2 in form.Fields)
			{
				if (pdfAnnot2.m_pAnnotObj.m_nObjNumber == pRef.m_nObjNum && pdfAnnot2.m_pAnnotObj.m_nGenNumber == pRef.m_nGenNum)
				{
					PdfAnnot result = pdfAnnot2;
					return result;
				}
			}
			return null;
		}
		internal PdfForm GetForm()
		{
			if (this.m_ptrForm != null)
			{
				return this.m_ptrForm;
			}
			PdfForm pdfForm = new PdfForm();
			pdfForm.m_pManager = this.m_pManager;
			pdfForm.m_pDoc = this;
			pdfForm.Create();
			this.m_ptrForm = pdfForm;
			return pdfForm;
		}
		public string ImportFromUrl(string Url, object Param)
		{
			return this.ImportFromUrl(Url, Param, null, null);
		}
		public string ImportFromUrl(string Url)
		{
			return this.ImportFromUrl(Url, null, null, null);
		}
		public string ImportFromUrl(string Url, object Param, string Username, string Password)
		{
			PdfParam pParam = this.m_pManager.VariantToParam(Param);
			HtmlManager htmlManager = new HtmlManager();
			htmlManager.m_pManager = this.m_pManager;
			htmlManager.m_pDocument = this;
			htmlManager.m_pParam = pParam;
			htmlManager.OpenUrl(Url, Username, Password);
			htmlManager.AssignCoordinates();
			htmlManager.Render();
			this.m_fLowestYCoordinate = htmlManager.m_fLowestYCoordinate;
			this.m_nLowestPageNumber = htmlManager.m_nLowestPageNumber;
			this.m_bImportInfoSet = htmlManager.m_bImportInfoSet;
			return htmlManager.m_bstrLog.ToString();
		}
		public PdfColorSpace CreateColorSpace(string Name)
		{
			return this.CreateColorSpace(Name, null);
		}
		public PdfColorSpace CreateColorSpace(string Name, object Param)
		{
			if (Name == null || Name.Length == 0)
			{
				AuxException.Throw("The Name argument cannot be empty.", PdfErrors._ERROR_INVALIDARG);
			}
			string text = "";
			string[] array = new string[]
			{
				"DeviceRGB",
				"DeviceCMYK",
				"DeviceGray",
				"CalGray",
				"CalRGB",
				"Lab",
				"ICCBased",
				"Indexed",
				"Pattern",
				"Separation",
				"DeviceN"
			};
			long[] array2 = new long[]
			{
				3L,
				4L,
				1L,
				1L,
				3L,
				3L,
				0L,
				1L,
				0L,
				1L,
				0L
			};
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if (string.Compare(array[i], Name, true) == 0)
				{
					text = array[i];
					num = i;
					break;
				}
			}
			if (text.Length == 0)
			{
				AuxException.Throw("Invalid colorspace name. Should be DeviceGray, DeviceRGB, DeviceCMYK, CalGray, CalRGB, Lab, ICCBased, Indexed, Pattern, Separation or DeviceN.", PdfErrors._ERROR_INVALIDARG);
			}
			PdfPages arg_E6_0 = this.Pages;
			PdfParam pParam = this.m_pManager.VariantToParam(Param);
			PdfColorSpace pdfColorSpace = new PdfColorSpace();
			pdfColorSpace.m_pManager = this.m_pManager;
			pdfColorSpace.m_pDoc = this;
			pdfColorSpace.m_bstrName = text;
			pdfColorSpace.m_bStandard = (num < 3);
			pdfColorSpace.m_nComponents = array2[num];
			pdfColorSpace.Create(pParam);
			return pdfColorSpace;
		}
		public PdfFunction CreateFunction(object Param)
		{
			PdfParam pParam = this.m_pManager.VariantToParam(Param);
			PdfFunction pdfFunction = new PdfFunction();
			pdfFunction.m_pManager = this.m_pManager;
			pdfFunction.m_pDoc = this;
			pdfFunction.Create(pParam);
			return pdfFunction;
		}
	}
}
