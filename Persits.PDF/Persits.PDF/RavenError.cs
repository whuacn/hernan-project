using System;
namespace Persits.PDF
{
	internal enum RavenError
	{
		ravenOk,
		ravenErrNoCurPt,
		ravenErrEmptyPath,
		ravenErrBogusPath,
		ravenErrNoSave,
		ravenErrOpenFile,
		ravenErrNoGlyph,
		ravenErrModeMismatch,
		ravenErrSingularMatrix
	}
}
