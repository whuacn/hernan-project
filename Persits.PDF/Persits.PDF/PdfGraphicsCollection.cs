using System;
namespace Persits.PDF
{
	public class PdfGraphicsCollection
	{
		internal PdfAnnot m_pAnnot;
		public PdfGraphics this[int Type, string State]
		{
			get
			{
				return this.m_pAnnot.GetGraphics(Type, State);
			}
			set
			{
				this.m_pAnnot.PutGraphics(Type, State, value);
			}
		}
		public PdfGraphics this[int Type]
		{
			get
			{
				return this.m_pAnnot.GetGraphics(Type, null);
			}
			set
			{
				this.m_pAnnot.PutGraphics(Type, null, value);
			}
		}
		internal PdfGraphicsCollection()
		{
		}
	}
}
