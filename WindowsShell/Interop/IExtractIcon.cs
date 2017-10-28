using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace WindowsShell.Interop
{
	[ComImport,
	 Guid("000214FA-0000-0000-C000-000000000046"),
	 InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
	 CLSCompliant(false)]
	public interface IExtractIcon
	{
		[PreserveSig] int GetIconLocation([In] ExtractIconOptions uFlags, [In] IntPtr szIconFile, [In] uint cchMax, [Out] out int piIndex, [Out] out ExtractIconFlags pwFlags);
		[PreserveSig] int Extract([In, MarshalAs(UnmanagedType.LPWStr)] string pszFile, uint nIconIndex, [Out, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(IconMarshaler))] out Icon phiconLarge, [Out, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(IconMarshaler))] out Icon phiconSmall, [In] uint nIconSize);
	}
}
