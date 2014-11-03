/****************************************************************************\
>	Copyright 2004 DOL for design studio.
>
>	DOLS DHtmlText Class
>
>	E-mail�G	  nomad_libra.tw@yahoo.com.tw
>	E-mail�G	  jameshrsp@ms2.url.com.tw
>
\*****************************************************************************/

// DHtmlText.cs: implementation of the DHtmlText class.
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Text;

namespace DOL.DHtml.DHtmlParser.Node
{
	/// <summary>
	/// The DHtmlText node represents a simple piece of text from the document.
	/// </summary>
    public sealed class DHtmlText : DHtmlNode, DOL.DBase.DIDiagnosisable
	{

	/////////////////////////////////////////////////////////////////////////////////
	#region �򥻾ާ@

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public DHtmlText()
        {
        }

		/////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// This constructs a new node with the given text content.
		/// </summary>
		/// <param name="text"></param>
		public DHtmlText(string text)
		{
            System.Diagnostics.Debug.Assert(text != null);  
			m_text = text;
		}

        ///////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// �غc�l
        /// </summary>
        public DHtmlText(DHtmlText obj) 
            : base(obj)
        {
            System.Diagnostics.Debug.Assert(obj != null);

            obj.AssertValid();
            m_text = obj.m_text;
        }

        /////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new DHtmlText(this);
        }

		/////////////////////////////////////////////////////////////////////////////        
		/// <summary>
		/// �O�_�����Ī���
		/// </summary>
		public override void AssertValid()
		{          			
		}

		/////////////////////////////////////////////////////////////////////////////       
		/// <summary>
		/// �ɦL����
		/// </summary>
		public override void Dump(StringBuilder buffer, string prefix)
		{
			AssertValid();
			string old = prefix;
			buffer.Append(old + "�uObject " + GetType().Name + " Dump : \n");							

			prefix += "�x�@";
            buffer.Append(prefix + "Node ID: " + this.NodeID + "\n");	

			if(m_text.Length == 0)
				buffer.Append(prefix + "Text content is empty\n");
			else if(this.IsWhiteSpace)
                buffer.Append(prefix + "Text content is white space\n");
            else
                buffer.Append(prefix + "Text content: \"" + m_text + "\"\n");
		}

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="visitor"></param>
        public override void Accept(DOL.DBase.DIBaseVisitor visitor)
        {
            DOL.DBase.DIVisitor<DHtmlText> textVisitor = visitor as DOL.DBase.DIVisitor<DHtmlText>;
            if(textVisitor != null) textVisitor.Visit(this);
        }

    #endregion	

    /////////////////////////////////////////////////////////////////////////////////
    #region �ݩʳ]�w

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        public override string NodeName
        {
            get
            {
                return "";
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        public override bool IsLeaf
        {
            get
            {
                return true;
            }
        }

		/////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// This is the text associated with this node.
		/// </summary>
		public string Text
		{
			get
			{
				return m_text;
			}
			set
			{
				System.Diagnostics.Debug.Assert(value != null);
                m_text = value;
			}
		}

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        public bool IsWhiteSpace
        {
            get
            {
                return m_text.Length == 1 && DHtmlTextProcessor.IsWhiteSpaceChar(m_text[0]);
            }
        }

    #endregion	

    /////////////////////////////////////////////////////////////////////////////////
    #region �ץX

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This will return the full HTML to represent this node (and all child nodes)
        /// </summary>
        internal override void TransformHTML(StringBuilder writer, int indentDepth)
        {
            System.Diagnostics.Debug.Assert(writer != null);
            writer.Append(DHtmlTextProcessor.TranStrToHtmlText(m_text));
		}

    #endregion	

	/////////////////////////////////////////////////////////////////////////////////
	#region	�������

		/// <summary>
		/// 
		/// </summary>
		private string m_text = "";

    #endregion	

	}
}
