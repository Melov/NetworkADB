using System;
using System.Runtime.InteropServices;

namespace WindowsShell.Interop
{
	[ComImport,
     Guid("1AC3D9F0-175C-11d1-95BE-00609797EA4F"),
	 InterfaceType(ComInterfaceType.InterfaceIsIUnknown),CLSCompliant(false)]
    public interface IPersistFolder2 : IPersistFolder
	{
        [PreserveSig]
        new int GetClassID([Out] out Guid pClassID);
        [PreserveSig]
        new int Initialize([In] IntPtr pidl);
        [PreserveSig]
        int GetCurFolder([Out] out IntPtr pidl);
	}
}
