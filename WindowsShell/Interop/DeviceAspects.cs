using System;

namespace WindowsShell.Interop
{
	[Flags]
	public enum DeviceAspects
	{
		Content		= 1,
		Thumbnail	= 2,
		Icon		= 4,
		DocPrint	= 8
	}
}
