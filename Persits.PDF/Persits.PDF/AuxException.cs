using System;
using System.Reflection;
namespace Persits.PDF
{
	internal class AuxException
	{
		public static void Throw(string err)
		{
			AuxException.Throw(err, (PdfErrors)0);
		}
		public static void Throw(string err, PdfErrors code)
		{
			if (code == PdfErrors._ERROR_PREVIEW_PARSE)
			{
				throw new PdfException(err);
			}
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			AssemblyName name = executingAssembly.GetName();
			Version arg_1F_0 = name.Version;
			string err2 = string.Format("AspPDF.NET {2} has generated error #{1}: [ {0} ]", err, (int)code, name.Version.ToString());
			throw new PdfException(err2);
		}
		public static void Throw(string err, PdfErrors code, int nOffset)
		{
			if (nOffset == -1)
			{
				AuxException.Throw(err, code);
				return;
			}
			string err2 = err + string.Format(" Offset: {0} (Hex {1:x})", nOffset, nOffset);
			AuxException.Throw(err2, code);
		}
	}
}
