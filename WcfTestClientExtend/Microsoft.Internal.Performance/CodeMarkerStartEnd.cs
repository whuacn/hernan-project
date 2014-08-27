using System;

namespace Microsoft.Internal.Performance
{
	internal struct CodeMarkerStartEnd : IDisposable
	{
		private int _end;

		internal CodeMarkerStartEnd(int begin, int end)
		{
			CodeMarkers.Instance.CodeMarker(begin);
			this._end = end;
		}

		public void Dispose()
		{
			if (this._end != 0)
			{
				CodeMarkers.Instance.CodeMarker(this._end);
				this._end = 0;
			}
		}
	}
}