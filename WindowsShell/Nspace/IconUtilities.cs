using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace WindowsShell.Nspace
{
    internal static class IconUtilities
    {
        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);
        /*
                public static ImageSource ToImageSource(this Icon icon)
                {
                    Bitmap bitmap = icon.ToBitmap();
                    IntPtr hBitmap = bitmap.GetHbitmap();

                    ImageSource wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(
                        hBitmap,
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());

                    if (!DeleteObject(hBitmap))
                    {
                        throw new Win32Exception();
                    }

                    return wpfBitmap;
                }
         */
    }
 
}
