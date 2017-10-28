using System;

namespace WindowsShell.Interop
{
	[Flags]
	internal enum CommandInfoMasks
	{
		None			= 0x00000000,
		HotKey			= 0x00000020,
		Icon			= 0x00000010,
		FlagNoUi		= 0x00000400,
		NoConsole		= 0x00008000,
		AsyncOk			= 0x00100000
	}
}