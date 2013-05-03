using System;
namespace Persits.PDF
{
	internal class RavenPipe
	{
		internal int x;
		internal int y;
		internal RavenPattern pattern;
		internal byte aInput;
		internal bool usesShape;
		internal RavenColorPtr cSrc;
		internal RavenColor cSrcVal = new RavenColor();
		internal RavenColorPtr alpha0Ptr;
		internal RavenColorPtr softMaskPtr;
		internal RavenColorPtr destColorPtr;
		internal int destColorMask;
		internal RavenColorPtr destAlphaPtr;
		internal byte shape;
		internal bool noTransparency;
		internal RavenPipeResultColorCtrl resultColorCtrl;
		internal bool nonIsolatedGroup;
		internal PipeFunc run;
	}
}
