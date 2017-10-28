using System;
using System.Runtime.InteropServices;

namespace WindowsShell.Interop
{
	[ComImport,
	 Guid("000214F2-0000-0000-C000-000000000046"),
	 InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
	 CLSCompliant(false)]
	public interface IEnumIDList
	{
		[PreserveSig] int Next([In] uint celt, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] IntPtr[] rgelt, [In] IntPtr pceltFetched);
		void Skip([In] uint celt);
		void Reset();
		void Clone([Out] out IEnumIDList ppenum);
	}
}
