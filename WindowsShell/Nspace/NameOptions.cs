using System;

namespace WindowsShell.Nspace
{
	[Flags]
	public enum NameOptions
	{
		Normal             = 0x0000,  // default (display purpose)
		InFolder           = 0x0001,  // displayed under a folder (relative)
		ForEditing         = 0x1000,  // for in-place editing
		ForAddressBar      = 0x4000,  // UI friendly parsing name (remove ugly stuff)
		ForParsing         = 0x8000   // parsing name for ParseDisplayName()
	}
}
