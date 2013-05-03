using System;
namespace Persits.PDF
{
	internal enum PeekValues : uint
	{
		PEEK_NOTATAG,
		PEEK_COMMENT,
		PEEK_NONFONT,
		PEEK_FONT = 4u,
		PEEK_CLOSE = 32768u
	}
}
