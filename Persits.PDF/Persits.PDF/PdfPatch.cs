using System;
namespace Persits.PDF
{
	internal class PdfPatch
	{
		internal double[,] x = new double[4, 4];
		internal double[,] y = new double[4, 4];
		internal PdfColor[,] color = new PdfColor[2, 2];
		internal PdfPatch()
		{
			this.color[0, 0] = new PdfColor();
			this.color[0, 1] = new PdfColor();
			this.color[1, 0] = new PdfColor();
			this.color[1, 1] = new PdfColor();
		}
		internal PdfPatch(PdfPatch p)
		{
			Array.Copy(p.x, this.x, 16);
			Array.Copy(p.y, this.y, 16);
			this.color = new PdfColor[2, 2];
			this.color[0, 0] = new PdfColor();
			this.color[0, 1] = new PdfColor();
			this.color[1, 0] = new PdfColor();
			this.color[1, 1] = new PdfColor();
			Array.Copy(p.color[0, 0].c, this.color[0, 0].c, 32);
			Array.Copy(p.color[0, 1].c, this.color[0, 1].c, 32);
			Array.Copy(p.color[1, 0].c, this.color[1, 0].c, 32);
			Array.Copy(p.color[1, 1].c, this.color[1, 1].c, 32);
		}
	}
}
