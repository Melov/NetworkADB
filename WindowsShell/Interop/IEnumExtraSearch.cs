using System;
using System.Runtime.InteropServices;

namespace WindowsShell.Interop
{
	[ComImport,
	 Guid("0E700BE1-9DB6-11d1-A1CE-00C04FD75D13"),
	 InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
	 CLSCompliant(false)]
	public interface IEnumExtraSearch
	{
		[PreserveSig] int Next([In] ulong celt, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] ExtraSearch[] rgelt, [In] IntPtr pceltFetched);
		void Skip([In] ulong celt);
		void Reset();
		void Clone([Out] out IEnumExtraSearch ppenum);
	}
}
