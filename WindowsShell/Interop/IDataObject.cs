/*
using System;
using System.Runtime.InteropServices;

namespace WindowsShell.Interop
{
	[ComImport,
	 Guid("0000010e-0000-0000-C000-000000000046"),
	 InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
	 CLSCompliant(false)]
	public interface IDataObject
	{
		void GetData([In] ref FormatEtc pformatetcIn, [Out] out StgMedium pmedium);
		void GetDataHere([In] ref FormatEtc pformatetc, [In, Out] ref StgMedium pmedium);
		void QueryGetData([In] ref FormatEtc pformatetc);
		void GetCanonicalFormatEtc([In] ref FormatEtc pformatectIn, [Out] out FormatEtc pformatetcOut);
		void SetData([In] ref FormatEtc pformatetc, [In] ref StgMedium pmedium, [In] bool fRelease);
		void EnumFormatEtc([In] uint direction, [Out, MarshalAs(UnmanagedType.Interface)] out IEnumFormatEtc ppenumFormatEtc);
		void DAdvise([In] ref FormatEtc pformatetc, [In] uint advf, [In, MarshalAs(UnmanagedType.Interface)] IAdviseSink pAdvSink, [Out] out uint pdwConnection);
		void DUnadvise([In] uint dwConnection);
		void EnumDAdvise([Out] out IEnumStatData ppenumAdvise);
	}
}
*/
