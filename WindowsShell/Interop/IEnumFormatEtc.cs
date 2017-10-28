/*
using System;
using System.Runtime.InteropServices;

namespace WindowsShell.Interop
{
	[ComImport,
	Guid("00000103-0000-0000-C000-000000000046"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
	CLSCompliant(false)]
	public interface IEnumFormatEtc
	{
		[PreserveSig] int Next([In] ulong celt, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] FormatEtc[] rgelt, [Out] out ulong pceltFetched);
		void Skip([In] ulong celt);
		void Reset();
		void Clone([Out, MarshalAs(UnmanagedType.Interface)] out IEnumFormatEtc ppenum);
	}
}
*/