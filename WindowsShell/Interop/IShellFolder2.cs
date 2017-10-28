using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WindowsShell.Nspace;

namespace WindowsShell.Interop
{
	[ComImport,
	 Guid("93F2F68C-1D1B-11d3-A30E-00C04F79ABD1"),
	 InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
	 CLSCompliant(false)]
	public interface IShellFolder2 : IShellFolder
	{
		/* IShellFolder members */

		//new void ParseDisplayName([In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(Win32WindowMarshaler))] IWin32Window hwnd, [In] IntPtr pbc, [In, MarshalAs(UnmanagedType.LPWStr)] string pwszDisplayName, [In] IntPtr pchEaten, [Out, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(ItemIdListMarshaler))] out ItemIdList ppidl, [In] IntPtr attrs);
        [PreserveSig]
        new int ParseDisplayName(IntPtr hwnd, IntPtr pbc, string pszDisplayName, ref uint pchEaten, out IntPtr ppidl, ref SFGAO pdwAttributes);
		new void EnumObjects([In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(Win32WindowMarshaler))] IWin32Window hwnd, [In] EnumOptions grfFlags, [Out, MarshalAs(UnmanagedType.Interface)] out IEnumIDList ppenumIDList);
        [PreserveSig]
        int BindToObject([In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ItemIdListMarshaler))] ItemIdList pidl, [In] IntPtr pbc, [In] ref Guid riid, [Out] out IntPtr ppv);
        [PreserveSig] int BindToStorage([In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ItemIdListMarshaler))] ItemIdList pidl, [In] IntPtr pbc, [In] ref Guid riid, [Out] out IntPtr ppv);
        [PreserveSig]
        new int CompareIDs([In] IntPtr lParam, [In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ItemIdListMarshaler))] ItemIdList pidl1, [In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ItemIdListMarshaler))] ItemIdList pidl2);
        [PreserveSig] new int CreateViewObject([In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Win32WindowMarshaler))] IWin32Window hwndOwner, [In] ref Guid riid, [Out] out IntPtr ppv);
		new void GetAttributesOf([In] uint cidl, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] IntPtr[] apidl, [In, Out] ref FolderAttributes rgfInOut);
        [PreserveSig]
        new int GetUIObjectOf([In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Win32WindowMarshaler))] IWin32Window hwndOwner, [In] uint cidl, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] IntPtr[] apidl, [In] ref Guid riid, [In, Out] IntPtr rgfReserved, [Out] out IntPtr ppv);
        new void GetDisplayNameOf([In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ItemIdListMarshaler))] ItemIdList pidl, [In] NameOptions uFlags, [Out] out STRRET pName);
		new void SetNameOf([In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(Win32WindowMarshaler))] IWin32Window hwnd, [In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(ItemIdListMarshaler))] ItemIdList pidl, [In, MarshalAs(UnmanagedType.LPWStr)] string pszName, [In] NameOptions uFlags, [In, Out] ref IntPtr ppidlOut);

		/* IShellFolder2 members */

        [PreserveSig]
        int GetDefaultSearchGUID([Out] out Guid pqguid);
        [PreserveSig]
        int EnumSearches([Out] out IEnumExtraSearch ppenum);
		void GetDefaultColumn([In] uint dwRes, [Out] out ulong pSort, [Out] out ulong pDisplay);
        [PreserveSig]
        int GetDefaultColumnState([In] uint iColumn, [Out] out ColumnStates pcsFlags);
        [PreserveSig]
        int GetDetailsEx([In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ItemIdListMarshaler))] ItemIdList pidl, [In] ref ColumnId pscid, [Out, MarshalAs(UnmanagedType.Struct)] out object pv);
        [PreserveSig]
        int GetDetailsOf([In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ItemIdListMarshaler))] ItemIdList pidl, [In] uint iColumn, [Out] out ShellDetails psd);
        int MapColumnToSCID([In] uint iColumn, [In] ref ColumnId pscid);
	}
}
