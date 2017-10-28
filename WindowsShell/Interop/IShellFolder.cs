using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using WindowsShell.Nspace;

namespace WindowsShell.Interop
{
	[ComImport,
	 Guid("000214E6-0000-0000-C000-000000000046"),
	 InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
	 CLSCompliant(false)]
	public interface IShellFolder
	{
        [PreserveSig]
        int ParseDisplayName(IntPtr hwnd, IntPtr pbc, string pszDisplayName, ref uint pchEaten, out IntPtr ppidl, ref SFGAO pdwAttributes);

		//void ParseDisplayName([In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(Win32WindowMarshaler))] IWin32Window hwnd, [In] IntPtr pbc, [In, MarshalAs(UnmanagedType.LPWStr)] string pwszDisplayName, [In] IntPtr pchEaten, [Out, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(ItemIdListMarshaler))] out ItemIdList ppidl, [In] IntPtr attrs);
		void EnumObjects([In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(Win32WindowMarshaler))] IWin32Window hwnd, [In] EnumOptions grfFlags, [Out, MarshalAs(UnmanagedType.Interface)] out IEnumIDList ppenumIDList);
        [PreserveSig]
        int BindToObject([In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ItemIdListMarshaler))] ItemIdList pidl, [In] IntPtr pbc, [In] ref Guid riid, [Out] out IntPtr ppv);
        [PreserveSig] int BindToStorage([In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ItemIdListMarshaler))] ItemIdList pidl, [In] IntPtr pbc, [In] ref Guid riid, [Out] out IntPtr ppv);
        [PreserveSig]
        int CompareIDs([In] IntPtr lParam, [In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ItemIdListMarshaler))] ItemIdList pidl1, [In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ItemIdListMarshaler))] ItemIdList pidl2);
        [PreserveSig] int CreateViewObject([In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Win32WindowMarshaler))] IWin32Window hwndOwner, [In] ref Guid riid, [Out] out IntPtr ppv);
		void GetAttributesOf([In] uint cidl, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] IntPtr[] apidl, [In, Out] ref FolderAttributes rgfInOut);
        [PreserveSig]
        int GetUIObjectOf([In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Win32WindowMarshaler))] IWin32Window hwndOwner, [In] uint cidl, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] IntPtr[] apidl, [In] ref Guid riid, [In, Out] IntPtr rgfReserved, [Out] out IntPtr ppv);
        void GetDisplayNameOf([In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ItemIdListMarshaler))] ItemIdList pidl, [In] NameOptions uFlags, [Out] out STRRET pName);
		void SetNameOf([In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(Win32WindowMarshaler))] IWin32Window hwnd, [In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(ItemIdListMarshaler))] ItemIdList pidl, [In, MarshalAs(UnmanagedType.LPWStr)] string pszName, [In] NameOptions uFlags, [In, Out] ref IntPtr ppidlOut);
	}
}
