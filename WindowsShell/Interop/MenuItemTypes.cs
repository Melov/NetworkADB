using System;

namespace WindowsShell.Interop
{
	[Flags]
	internal enum MenuItemTypes
	{
		String          = 0x00000000,
		Bitmap          = 0x00000004,
		MenuBarBreak    = 0x00000020,
		MenuBreak       = 0x00000040,
		OwnerDraw       = 0x00000100,
		RadioCheck      = 0x00000200,
		Separator       = 0x00000800,
		RightOrder      = 0x00002000,
		RightJustify    = 0x00004000
	}
}
