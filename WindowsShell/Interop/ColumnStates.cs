using System;

namespace WindowsShell.Interop
{
	[Flags]
	public enum ColumnStates
	{
		None			= 0x0,
		TypeStr			= 0x1,
		TypeInt			= 0x2,
		TypeDate		= 0x3,
		TypeMask		= 0xf,
		OnByDefault		= 0x10,
		Slow			= 0x20,
		Extended		= 0x40,
		SecondaryUi		= 0x80,
		Hidden			= 0x100,
		PreferVarCmp	= 0x200
	}
}
