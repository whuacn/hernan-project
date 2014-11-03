/*****************************************************************************\
>	Copyright 2004 DOL for design studio.
>
>	DOLS DCssProperty Class
>
>	E-mail�G	  nomad_libra.tw@yahoo.com.tw
>	E-mail�G	  jameshrsp@ms2.url.com.tw
>
\*****************************************************************************/

// DCssProperty.cs: implementation of the DCssProperty class.
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Text;

namespace DOL.DHtml.DCssResolver
{
	/// <summary>
    /// The DCssProperty object represents a named value
	/// </summary>
    public sealed class DCssProperty : DOL.DBase.DIDiagnosisable, ICloneable
	{

	/////////////////////////////////////////////////////////////////////////////////
	#region �򥻾ާ@	

		///////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// �غc�l
		/// </summary>
		public DCssProperty()
		{
		}

        ///////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// �غc�l
        /// </summary>
        public DCssProperty(string name, string value)
        {
            System.Diagnostics.Debug.Assert(name != null && DHtmlTextProcessor.ExistWhiteSpaceChar(name) == false);
            System.Diagnostics.Debug.Assert(value != null);

            m_propertyName = name.ToLower();
            m_propertyValue = value.Trim(DHtmlTextProcessor.WhiteSpaceCharsArray);
        }

        ///////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// �غc�l
        /// </summary>
        public DCssProperty(DCssProperty obj)
        {
            System.Diagnostics.Debug.Assert(obj != null);

            obj.AssertValid();
            m_propertyName = obj.m_propertyName;
            m_propertyValue = obj.m_propertyValue;
        }

        /////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new DCssProperty(this);
        }

		/////////////////////////////////////////////////////////////////////////////        
		/// <summary>
		/// �O�_�����Ī���
		/// </summary>
		public void AssertValid()
		{
            System.Diagnostics.Debug.Assert(m_propertyName != null && DHtmlTextProcessor.ExistWhiteSpaceChar(m_propertyName) == false, "Member variable, m_propertyName is invalid");
            System.Diagnostics.Debug.Assert(m_propertyValue != null, "Member variable, m_propertyValue is invalid");       	
		}

		/////////////////////////////////////////////////////////////////////////////       
		/// <summary>
		/// �ɦL����
		/// </summary>
		public void Dump(StringBuilder buffer, string prefix)
		{
            AssertValid();
            string old = prefix;
            buffer.Append(old + "�uObject " + GetType().Name + " Dump : \n");

            prefix += "�x�@";
            buffer.Append(prefix + "Property: \"" + this.CSS + "\"\n");
		}

    #endregion	

    /////////////////////////////////////////////////////////////////////////////////
    #region �ݩʳ]�w

		///////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// �ݩʦW��
		/// </summary>
		public string Name
		{
			get
			{
                return m_propertyName;
			}
			set
			{
                System.Diagnostics.Debug.Assert(value != null);
                System.Diagnostics.Debug.Assert(DHtmlTextProcessor.ExistWhiteSpaceChar(value) != true);

                m_propertyName = value.ToLower(); 
			}
		}

		///////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// �ݩʭ�
		/// </summary>
		public string Value
		{
			get
			{
                return m_propertyValue;
			}
			set
			{                
                System.Diagnostics.Debug.Assert(value != null);
                m_propertyValue = value.Trim(DHtmlTextProcessor.WhiteSpaceCharsArray); 
			}
		}

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This is the text associated with this node.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.CSS;
        }

    #endregion	

    /////////////////////////////////////////////////////////////////////////////////
    #region �ץX

		///////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// �ץX�� CSS �r��
		/// </summary>		
        public string CSS
		{
            get
            {
                StringBuilder writer = new StringBuilder();
                TransformCSS(writer);

                return writer.ToString();
            }
		}

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This will return the full CSS to represent this property
        /// </summary>
        internal void TransformCSS(StringBuilder writer)
        {
            System.Diagnostics.Debug.Assert(writer != null);

            writer.Append(m_propertyName);
            writer.Append(":");
            writer.Append(m_propertyValue);
            writer.Append(";");
        }
	
    #endregion	

	/////////////////////////////////////////////////////////////////////////////////
	#region �������

		/// <summary>
		/// �ݩʦW��
		/// </summary>
        private string m_propertyName = "";
		/// <summary>
		/// �ݩʭ�
		/// </summary>
        private string m_propertyValue = "";

    #endregion

	}

}
