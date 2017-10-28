using System;

namespace WindowsShell.Interop
{
	[Flags]
	internal enum MenuItemInfoOptions
	{
		None		= 0x00000000,
		State       = 0x00000001,
		Id          = 0x00000002,
		SubMenu     = 0x00000004,
		CheckMarks  = 0x00000008,
		Type        = 0x00000010,
		Data        = 0x00000020,
		String      = 0x00000040,
		Bitmap      = 0x00000080,
		FType       = 0x00000100
	}
}
