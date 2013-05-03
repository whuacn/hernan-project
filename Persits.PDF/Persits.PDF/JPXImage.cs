using System;
namespace Persits.PDF
{
	internal class JPXImage
	{
		internal uint xSize;
		internal uint ySize;
		internal uint xOffset;
		internal uint yOffset;
		internal uint xTileSize;
		internal uint yTileSize;
		internal uint xTileOffset;
		internal uint yTileOffset;
		internal uint nComps;
		internal uint nXTiles;
		internal uint nYTiles;
		internal JPXTile[] tiles;
	}
}
