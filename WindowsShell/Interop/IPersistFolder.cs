using System;
using System.Runtime.InteropServices;

namespace WindowsShell.Interop
{
	[ComImport,
	 Guid("000214EA-0000-0000-C000-000000000046"),
	 InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IPersistFolder : IPersist
	{
        [PreserveSig]
        new int GetClassID([Out] out Guid pClassID);
        [PreserveSig]
        int Initialize([In] IntPtr pidl);
	}
}
