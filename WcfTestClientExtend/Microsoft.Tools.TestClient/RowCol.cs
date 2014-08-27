using System;

namespace Microsoft.Tools.TestClient
{
	internal class RowCol
	{
		private int col;

		private int row;

		internal int Col
		{
			get
			{
				return this.col;
			}
		}

		internal int Row
		{
			get
			{
				return this.row;
			}
		}

		public RowCol(int row, int col)
		{
			this.row = row;
			this.col = col;
		}
	}
}