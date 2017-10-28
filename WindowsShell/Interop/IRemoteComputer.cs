using System;
using System.Runtime.InteropServices;

namespace WindowsShell.Interop
{
	[ComImport,
	 Guid("000214FE-0000-0000-C000-000000000046"),
	 InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IRemoteComputer
	{
        [PreserveSig]
        void Initialize([In] string pszMachine, [In] bool bEnumerating);
	}
}
