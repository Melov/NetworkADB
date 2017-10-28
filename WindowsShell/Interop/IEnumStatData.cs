/*
using System;
using System.Runtime.InteropServices;

namespace WindowsShell.Interop
{
	[ComImport,
	 Guid("00000105-0000-0000-C000-000000000046"),
	 InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
	 CLSCompliant(false)]
	public interface IEnumStatData
	{
		[PreserveSig] int Next([In] ulong celt, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] StatData[] rgelt, [Out] out ulong pceltFetched);
		void Skip([In] ulong celt);
		void Reset();
		void Clone([Out, MarshalAs(UnmanagedType.Interface)] out IEnumStatData ppenum);
	}
}
*/