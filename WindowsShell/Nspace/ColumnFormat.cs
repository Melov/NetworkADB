using System;

namespace WindowsShell.Nspace
{
	public enum ColumnFormat
	{
		Left             = 0x0000,
		Right            = 0x0001,
		Center           = 0x0002,
		JustifyMask      = 0x0003,
		Image            = 0x0800,
		BitmapOnRight    = 0x1000,
		ColHasImages     = 0x8000
	}
}
