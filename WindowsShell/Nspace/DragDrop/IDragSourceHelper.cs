using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace WindowsShell.Nspace.DragDrop
{
    [ComVisible(true)]
    [ComImport]
    [Guid("DE5BF786-477A-11D2-839D-00C04FD918D0")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDragSourceHelper
    {

        void InitializeFromBitmap(

            [In, MarshalAs(UnmanagedType.Struct)] ref ShDragImage dragImage,

            [In, MarshalAs(UnmanagedType.Interface)] System.Runtime.InteropServices.ComTypes.IDataObject dataObject);

        void InitializeFromWindow(

            [In] IntPtr hwnd,

            [In] ref Win32Point pt,

            [In, MarshalAs(UnmanagedType.Interface)] System.Runtime.InteropServices.ComTypes.IDataObject dataObject);

    }
}
