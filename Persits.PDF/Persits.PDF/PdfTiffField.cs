using System;
using System.Collections.Generic;
namespace Persits.PDF
{
	internal class PdfTiffField
	{
		public TiffTags m_nTag;
		public TiffFieldType m_nType;
		public int m_nCount;
		public int m_nValueOffset;
		public PdfStream m_objStream = new PdfStream();
		public List<uint> m_arrLongs = new List<uint>();
		public List<uint> m_arrLongs2 = new List<uint>();
		public List<short> m_arrSShorts = new List<short>();
		public List<int> m_arrSLongs = new List<int>();
		public List<float> m_arrFloats = new List<float>();
	}
}
