using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace WindowsShell.Nspace.DragDrop
{
    [StructLayout(LayoutKind.Sequential)]

    public struct Win32Point
    {

        public int x;

        public int y;

    }

    [StructLayout(LayoutKind.Sequential)]

    public struct Win32Size
    {

        public int cx;

        public int cy;

    }

    [StructLayout(LayoutKind.Sequential)]

    public struct ShDragImage
    {

        public Win32Size sizeDragImage;

        public Win32Point ptOffset;

        public IntPtr hbmpDragImage;

        public int crColorKey;

    }

    public struct FORMATETC
    {

        public short cfFormat;

        public IntPtr ptd;

        public DVASPECT dwAspect;

        public int lindex;

        public TYMED tymed;

    }
}
