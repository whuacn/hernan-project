using System;
namespace Persits.PDF
{
	internal class PdfIdentityFunction : PdfFunctionTop
	{
		internal PdfIdentityFunction()
		{
			this.m = 8;
			this.n = 32;
			for (int i = 0; i < 8; i++)
			{
				this.domain[i, 0] = 0.0;
				this.domain[i, 1] = 1.0;
			}
			this.hasRange = false;
		}
		internal override void transform(double[] inv, double[] outv)
		{
			for (int i = 0; i < 32; i++)
			{
				outv[i] = inv[i];
			}
		}
		internal override PdfFunctionTop copy()
		{
			return new PdfIdentityFunction();
		}
		internal override bool isOk()
		{
			return true;
		}
	}
}
