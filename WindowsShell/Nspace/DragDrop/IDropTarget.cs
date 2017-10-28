using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using WindowsShell.Interop;

namespace WindowsShell.Nspace.DragDrop
{
    [ComImport,
     Guid("00000122-0000-0000-C000-000000000046"),
     InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
     CLSCompliant(false)]
    public interface IDropTarget
    {
        [PreserveSig]
        int DragEnter([In, MarshalAs(UnmanagedType.Interface)]System.Runtime.InteropServices.ComTypes.IDataObject pDataObj,
        [In, MarshalAs(UnmanagedType.U4)] int grfKeyState, [In] Win32Point pt, [In,
          Out] ref int pdwEffect);

        [PreserveSig]
        int DragOver([In, MarshalAs(UnmanagedType.U4)] int grfKeyState, [In]
Win32Point pt, [In, Out] ref int pdwEffect);

        [PreserveSig]
        int DragLeave();

        [PreserveSig]
        int Drop([In, MarshalAs(UnmanagedType.Interface)]System.Runtime.InteropServices.ComTypes.IDataObject pDataObj,
        [In, MarshalAs(UnmanagedType.U4)] int grfKeyState, [In] Win32Point pt, [In,
         Out] ref int pdwEffect);
    };
}
