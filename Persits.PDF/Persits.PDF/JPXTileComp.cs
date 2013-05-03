using System;
namespace Persits.PDF
{
	internal class JPXTileComp
	{
		internal bool sgned;
		internal uint prec;
		internal uint hSep;
		internal uint vSep;
		internal uint style;
		internal uint nDecompLevels;
		internal uint codeBlockW;
		internal uint codeBlockH;
		internal uint codeBlockStyle;
		internal uint transform;
		internal uint quantStyle;
		internal uint[] quantSteps;
		internal uint nQuantSteps;
		internal uint x0;
		internal uint y0;
		internal uint x1;
		internal uint y1;
		internal uint w;
		internal uint cbW;
		internal uint cbH;
		internal int[] data;
		internal int[] buf;
		internal JPXResLevel[] resLevels;
		internal JPXTileComp copy()
		{
			return new JPXTileComp
			{
				sgned = this.sgned,
				prec = this.prec,
				hSep = this.hSep,
				vSep = this.vSep,
				style = this.style,
				nDecompLevels = this.nDecompLevels,
				codeBlockW = this.codeBlockW,
				codeBlockH = this.codeBlockH,
				codeBlockStyle = this.codeBlockStyle,
				transform = this.transform,
				quantStyle = this.quantStyle,
				nQuantSteps = this.nQuantSteps,
				x0 = this.x0,
				y0 = this.y0,
				x1 = this.x1,
				y1 = this.y1,
				w = this.w,
				cbW = this.cbW,
				cbH = this.cbH
			};
		}
	}
}
