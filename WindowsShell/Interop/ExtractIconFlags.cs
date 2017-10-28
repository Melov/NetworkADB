using System;

namespace WindowsShell.Interop
{
	[Flags]
	public enum ExtractIconFlags
	{
		None		=	0x0000,
		SimulateDoc =	0x0001,	// simulate this document icon for this	}
		PerInstance =	0x0002,	// icons from this class are per instance (each file has its own)}
		PerClass =		0x0004,	// icons from this class per class (shared for all files of this type)
		NotFilename =	0x0008,	// location is not a filename, must call ::ExtractIcon
		DontCache =		0x0010	// this icon should not be cached
	}
}
