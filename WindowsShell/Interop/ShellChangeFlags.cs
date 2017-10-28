using System;

namespace WindowsShell.Interop
{
	public enum ShellChangeFlags
	{
		IdList		= 0x0000,
		PathA       = 0x0001,
		PrinterA    = 0x0002,
		Dword       = 0x0003,
		PathW       = 0x0005,
		PrinterW    = 0x0006,
		Type        = 0x00FF,
		Flush       = 0x1000,
		FlushNoWait = 0x2000,
        NotifyRecursive	= 0x10000,        
	}
}
