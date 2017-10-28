/*
using System;
using System.Runtime.InteropServices;

namespace WindowsShell.Interop
{
	[ComImport,
	 Guid("0000010f-0000-0000-C000-000000000046"),
	 InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
	 CLSCompliant(false)]
	public interface IAdviseSink
	{
		[PreserveSig] void OnDataChange([In] ref FormatEtc pFormatetc, [In] ref StgMedium pStgmed);
		[PreserveSig] void OnViewChange([In] DeviceAspects dwAspect, [In] long lindex);
		[PreserveSig] void OnRename([In] IntPtr pmk /* pointer to IMoniker *///);
/*		[PreserveSig] void OnSave();
		[PreserveSig] void OnClose();
	}
}
*/
