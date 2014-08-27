using System;

namespace Microsoft.Internal.Performance
{
	internal struct CodeMarkerExStartEnd : IDisposable
	{
		private int _end;

		private byte[] _aBuff;

		internal CodeMarkerExStartEnd(int begin, int end, byte[] aBuff)
		{
			CodeMarkers.Instance.CodeMarkerEx(begin, aBuff);
			this._end = end;
			this._aBuff = aBuff;
		}

		internal CodeMarkerExStartEnd(int begin, int end, Guid guidData) : this(begin, end, guidData.ToByteArray())
		{
		}

		internal CodeMarkerExStartEnd(int begin, int end, string stringData) : this(begin, end, CodeMarkers.StringToBytesZeroTerminated(stringData))
		{
		}

		internal CodeMarkerExStartEnd(int begin, int end, uint uintData) : this(begin, end, BitConverter.GetBytes(uintData))
		{
		}

		internal CodeMarkerExStartEnd(int begin, int end, ulong ulongData) : this(begin, end, BitConverter.GetBytes(ulongData))
		{
		}

		public void Dispose()
		{
			if (this._end != 0)
			{
				CodeMarkers.Instance.CodeMarkerEx(this._end, this._aBuff);
				this._end = 0;
				this._aBuff = null;
			}
		}
	}
}