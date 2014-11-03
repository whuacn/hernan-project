/*****************************************************************************\
>	Copyright 2004 DOL for design studio.
>
>	DOLS DHtmlProcessingInstruction Class
>
>	E-mail�G	  nomad_libra.tw@yahoo.com.tw
>	E-mail�G	  jameshrsp@ms2.url.com.tw
>
\*****************************************************************************/

// DHtmlProcessingInstruction: implementation of the DHtmlProcessingInstruction class.
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Text;

namespace DOL.DHtml.DHtmlParser.Node
{
	/// <summary>
    /// The DHtmlProcessingInstruction node represents a simple piece of processing instruction from the document.
	/// </summary>
    public sealed class DHtmlProcessingInstruction : DHtmlNode, DOL.DBase.DIDiagnosisable
	{

	/////////////////////////////////////////////////////////////////////////////////
	#region �򥻾ާ@

        /////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
        public DHtmlProcessingInstruction()
        {
        }

		/////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
        public DHtmlProcessingInstruction(string value)
		{
            System.Diagnostics.Debug.Assert(value != null);
            m_value = value;
		}

        ///////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// �غc�l
        /// </summary>
        public DHtmlProcessingInstruction(DHtmlProcessingInstruction obj) 
            : base(obj)
        {
            System.Diagnostics.Debug.Assert(obj != null) ;

            obj.AssertValid();
            m_value = obj.m_value;
        }

        /////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new DHtmlProcessingInstruction(this);
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

            if(m_value.Length == 0)
                buffer.Append(prefix + "Processing Instruction is empty\n");
            else
            {
                buffer.Append(prefix + "Processing Instruction content:");
                buffer.Append("\n====================================================================================================================================\n");
                buffer.Append(m_value);
                buffer.Append("\n====================================================================================================================================\n");
            }
		}

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="visitor"></param>
        public override void Accept(DOL.DBase.DIBaseVisitor visitor)
        {
            DOL.DBase.DIVisitor<DHtmlProcessingInstruction> piVisitor = visitor as DOL.DBase.DIVisitor<DHtmlProcessingInstruction>;
            if(piVisitor != null) piVisitor.Visit(this);
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
        /// This is the name associated with this node.
        /// </summary>
        public string Value
        {
            get
            {
                return m_value;
            }
            set
            {
                System.Diagnostics.Debug.Assert(value != null) ;
                m_value = value;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This is the text associated with this node.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "<?" + m_value + "?>";
        }

    #endregion	

    /////////////////////////////////////////////////////////////////////////////////
    #region �ץX

		/////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  �ץX�� HTML �r��
		/// </summary>
        /// 
        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This will return the full HTML to represent this node (and all child nodes)
        /// </summary>
        internal override void TransformHTML(StringBuilder writer, int indentDepth)
        {
            System.Diagnostics.Debug.Assert(writer != null);

            for(int count = 0; count < indentDepth; ++count)
                writer.Append("\t");

            writer.Append("<?");
            writer.Append(m_value);
            writer.Append("?>\n");
        }

    #endregion	

	/////////////////////////////////////////////////////////////////////////////////
	#region	�������

		/// <summary>
		/// 
		/// </summary>
        private string m_value = "";

    #endregion	

	}
}
