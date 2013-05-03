using System;
namespace Persits.PDF
{
	internal struct PSObject
	{
		internal PSObjectType type;
		internal bool booln;
		internal int intg;
		internal double real;
		internal PSOp op;
		internal int blk;
	}
}
