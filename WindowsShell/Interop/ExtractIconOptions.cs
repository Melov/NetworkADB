using System;

namespace WindowsShell.Interop
{
	[Flags]
	public enum ExtractIconOptions
	{
		OpenIcon =		0x0001,	// allows containers to specify an "open" look	}
		ForShell =		0x0002,	// icon is to be displayed in a ShellFolder}
		Async =			0x0020,	// this is an async extract, return E_PENDING
		DefaultIcon =	0x0040,	// get the default icon location if the final one takes too long to get
		ForShortcut =	0x0080	// the icon is for a shortcut to the object
	}
}