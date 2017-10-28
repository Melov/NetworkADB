using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace WindowsShell.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CIDA
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public uint[] aoffset;
    }
}
