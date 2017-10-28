using System;

namespace WindowsShell.Interop
{
	[Flags]
	internal enum MenuItemStates
	{
		None			= 0x00000000,
		Grayed          = 0x00000003,
		Disabled        = 0x00000003,
		Checked         = 0x00000008,
		HiLite          = 0x00000080,
		Enabled         = 0x00000000,
		Unchecked       = 0x00000000,
		Unhilite        = 0x00000000,
		Default         = 0x00001000
	}
}
