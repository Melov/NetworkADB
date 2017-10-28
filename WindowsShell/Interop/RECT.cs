﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace WindowsShell.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public RECT(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }


        public int left, top, right, bottom;

        public int Width()
        {
            return right - left;
        }

        public int Height()
        {
            return bottom - top;
        }

        public void Offset(int x, int y)
        {
            left += x;
            right += x;
            top += y;
            bottom += y;
        }

        public void Set(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        public bool IsEmpty()
        {
            return Width() == 0 && Height() == 0;
        }
    }
}
