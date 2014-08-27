using System;
using System.Diagnostics;

namespace Microsoft.Tools.TestClient
{
	internal static class DiagnosticUtility
	{
		private static Microsoft.Tools.TestClient.ExceptionUtility exceptionUtility;

		internal static Microsoft.Tools.TestClient.ExceptionUtility ExceptionUtility
		{
			get
			{
				if (DiagnosticUtility.exceptionUtility == null)
				{
					DiagnosticUtility.exceptionUtility = new Microsoft.Tools.TestClient.ExceptionUtility();
				}
				return DiagnosticUtility.exceptionUtility;
			}
		}

		[Conditional("DEBUG")]
		internal static void DebugAssert(bool condition, string message)
		{
		}

		[Conditional("DEBUG")]
		internal static void DebugAssert(string message)
		{
		}
	}
}