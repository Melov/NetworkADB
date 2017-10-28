using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace WindowsShell.Interop
{
    [ComImport, Guid("00000121-0000-0000-C000-000000000046"),
InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IDropSource
    {
        [PreserveSig]
        uint QueryContinueDrag(
        [MarshalAs(UnmanagedType.Bool)] bool fEscapePressed,
        uint grfKeyState);

        [PreserveSig]
        uint GiveFeedback(
        uint dwEffect);
    }
}
