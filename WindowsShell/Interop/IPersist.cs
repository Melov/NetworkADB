using System;
using System.Runtime.InteropServices;

namespace WindowsShell.Interop
{
	[ComImport,
	 Guid("0000010c-0000-0000-C000-000000000046"),
	 InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IPersist
	{
        [PreserveSig]
		int GetClassID([Out] out Guid pClassID);
	}
}
