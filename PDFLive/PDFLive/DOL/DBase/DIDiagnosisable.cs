/*****************************************************************************\
>	Copyright 2004 DOL for design studio.
>
>	DOLS DOL.DBase.DIDiagnosisable Class
>
>	E-mail�G	  nomad_libra.tw@yahoo.com.tw
>	E-mail�G	  jameshrsp@ms2.url.com.tw
>
\*****************************************************************************/

// DOL.DBase.DIDiagnosisable.cs: implementation of the DOL.DBase.DIDiagnosisable class.
//
///////////////////////////////////////////////////////////////////////////////

using System.Text;

namespace DOL.DBase
{
	/// <summary>
	/// DOL.DBase.DIDiagnosisable
	/// </summary>
	public interface DIDiagnosisable
	{

	/////////////////////////////////////////////////////////////////////////////////
	#region �򥻾ާ@	

		/////////////////////////////////////////////////////////////////////////////        
		/// <summary>
		/// �O�_�����Ī���
		/// </summary>
		void AssertValid();

		/////////////////////////////////////////////////////////////////////////////       
		/// <summary>
		/// �ɦL����
		/// </summary>
		/// <param name="buffer"> �ɦL���w�İ� </param>
		/// <param name="prefix"> �Y�Ƥ�r </param>
		void Dump(StringBuilder buffer, string prefix);

    #endregion

	}
}
