using System;
using System.Runtime.InteropServices;

namespace WindowsShell.Interop
{
	[ComImport,
	 Guid("00000002-0000-0000-C000-000000000046"),
	 InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IMalloc
	{
		[PreserveSig] IntPtr Alloc([In] int cb);
		[PreserveSig] IntPtr Realloc([In] IntPtr pv, [In] int cb);
		[PreserveSig] void Free(IntPtr pv);
		[PreserveSig] int GetSize(IntPtr pv);
		[PreserveSig] int DidAlloc(IntPtr pv);
		[PreserveSig] void HeapMinimize();
	}
}
