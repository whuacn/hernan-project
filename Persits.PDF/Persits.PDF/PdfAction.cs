using System;
namespace Persits.PDF
{
	public class PdfAction
	{
		internal ActionType m_nType;
		internal PdfManager m_pManager;
		internal PdfDocument m_pDoc;
		internal PdfIndirectObj m_pActionObj;
		internal bool m_bDestSet;
		internal bool m_bGoToR;
		public bool IsValid
		{
			get
			{
				bool result = true;
				if (!this.m_bDestSet && (this.m_nType == ActionType.ActionGoTo || this.m_nType == ActionType.ActionGoToR))
				{
					result = false;
				}
				return result;
			}
		}
		internal PdfAction()
		{
			this.m_pManager = null;
			this.m_pDoc = null;
			this.m_pActionObj = null;
			this.m_nType = ActionType.ActionNotSet;
			this.m_bDestSet = false;
			this.m_bGoToR = false;
		}
		internal void Create(PdfParam pParam, string Value)
		{
			if (!pParam.IsSet("Type"))
			{
				AuxException.Throw("You must specify action type.", PdfErrors._ERROR_INVALIDARG);
			}
			this.m_nType = (ActionType)pParam.Long("Type");
			this.m_pActionObj = this.m_pDoc.AddNewIndirectObject(enumIndirectType.pdfIndirectAction);
			this.m_pActionObj.AddName("Type", "Action");
			switch (this.m_nType)
			{
			case ActionType.ActionGoTo:
				this.HandleGoTo();
				return;
			case ActionType.ActionGoToR:
				this.HandleGoToR(pParam, Value);
				return;
			case ActionType.ActionLaunch:
				this.HandleLaunch(pParam, Value);
				return;
			case ActionType.ActionURI:
				this.HandleURI(pParam, Value);
				return;
			case ActionType.ActionNamed:
				this.HandleNamed(pParam, Value);
				return;
			case ActionType.ActionSubmitForm:
			case ActionType.ActionResetForm:
				this.HandleForm(this.m_nType, pParam, Value);
				return;
			case ActionType.ActionJavaScript:
				this.HandleJavaScript(pParam, Value);
				return;
			}
			AuxException.Throw("Action type not supported.", PdfErrors._ERROR_INVALIDARG);
		}
		private void HandleGoTo()
		{
			this.m_pActionObj.AddName("S", "GoTo");
		}
		private void HandleGoToR(PdfParam pParam, string Value)
		{
			this.m_pActionObj.AddName("S", "GoToR");
			this.m_pActionObj.AddString("F", Value);
			this.m_bGoToR = true;
		}
		private void HandleLaunch(PdfParam pParam, string Value)
		{
			this.m_pActionObj.AddName("S", "Launch");
			PdfDict pdfDict = this.m_pActionObj.AddDict("F");
			pdfDict.Add(new PdfString("F", Value));
			if (pParam.IsTrue("NewWindow"))
			{
				this.m_pActionObj.AddBool("NewWindow", true);
			}
		}
		private void HandleURI(PdfParam pParam, string Value)
		{
			this.m_pActionObj.AddName("S", "URI");
			this.m_pActionObj.AddString("URI", Value);
			if (pParam.IsTrue("IsMap"))
			{
				this.m_pActionObj.AddBool("IsMap", true);
			}
		}
		private void HandleNamed(PdfParam pParam, string Value)
		{
			this.m_pActionObj.AddName("S", "Named");
			this.m_pActionObj.AddName("N", Value);
		}
		private void HandleForm(ActionType Type, PdfParam pParam, string Value)
		{
			this.m_pActionObj.AddName("S", (Type == ActionType.ActionResetForm) ? "ResetForm" : "SubmitForm");
			if (Type == ActionType.ActionSubmitForm)
			{
				PdfDict pdfDict = this.m_pActionObj.AddDict("F");
				pdfDict.Add(new PdfName("FS", "URL"));
				pdfDict.Add(new PdfString("F", Value));
				int num = 0;
				if (pParam.IsTrue("HTML"))
				{
					num |= 4;
				}
				if (pParam.IsTrue("Get"))
				{
					num |= 8;
				}
				if (pParam.IsTrue("Coordinates"))
				{
					num |= 16;
				}
				if (pParam.IsTrue("XML"))
				{
					num |= 32;
				}
				if (pParam.IsTrue("IncludeAppend"))
				{
					num |= 64;
				}
				if (pParam.IsTrue("IncludeAnnots"))
				{
					num |= 128;
				}
				if (pParam.IsTrue("PDF"))
				{
					num |= 256;
				}
				if (pParam.IsTrue("ExcludeNonUserAnnots"))
				{
					num |= 1024;
				}
				if (num > 0)
				{
					this.m_pActionObj.AddInt("Flags", num);
				}
			}
		}
		private void HandleJavaScript(PdfParam pParam, string Value)
		{
			this.m_pActionObj.AddName("S", "JavaScript");
			if (Value.Length > 50)
			{
				PdfIndirectObj pdfIndirectObj = this.m_pDoc.AddNewIndirectObject(enumIndirectType.pdfIndirectStream);
				PdfString pStream = new PdfString(null, Value);
				pdfIndirectObj.m_objStream.Set(pStream);
				pdfIndirectObj.m_objStream.EncodeFlate();
				pdfIndirectObj.AddName("Filter", "FlateDecode");
				this.m_pActionObj.AddReference("JS", pdfIndirectObj);
				return;
			}
			this.m_pActionObj.AddString("JS", Value);
		}
		public void SetDest(PdfDest Dest)
		{
			if (Dest == null)
			{
				AuxException.Throw("Destination argument is empty.", PdfErrors._ERROR_INVALIDARG);
			}
			PdfArray pArray = this.m_pActionObj.AddArray("D");
			Dest.Populate(pArray, this.m_bGoToR);
			this.m_bDestSet = true;
		}
		public void AddNextAction(PdfAction NextAction)
		{
			if (NextAction == null)
			{
				AuxException.Throw("Next action argument is empty.", PdfErrors._ERROR_INVALIDARG);
			}
			if (NextAction == this)
			{
				AuxException.Throw("Next action cannot be pointing to self.", PdfErrors._ERROR_INVALIDARG);
			}
			PdfArray pdfArray = this.m_pActionObj.AddArray("Next");
			pdfArray.Add(new PdfReference(null, NextAction.m_pActionObj));
		}
	}
}
