using System;
namespace Persits.PDF
{
	internal class JPXTile
	{
		internal bool init;
		internal uint progOrder;
		internal uint nLayers;
		internal uint multiComp;
		internal uint x0;
		internal uint y0;
		internal uint x1;
		internal uint y1;
		internal uint maxNDecompLevels;
		internal uint comp;
		internal uint res;
		internal uint precinct;
		internal uint layer;
		internal JPXTileComp[] tileComps;
	}
}
