using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using WindowsShell.Nspace;

namespace WindowsShell.Interop
{
    /// <summary>
    /// The SIZE structure specifies the width and height of a rectangle.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SIZE
    {
        /// <summary>
        /// Specifies the rectangle's width. The units depend on which function uses this.
        /// </summary>
        public int cx;

        /// <summary>
        /// Specifies the rectangle's height. The units depend on which function uses this.
        /// </summary>
        public int cy;

        /// <summary>
        /// Simple constructor for SIZE structs.
        /// </summary>
        /// <param name="cx">The initial width of the SIZE structure.</param>
        /// <param name="cy">The initial height of the SIZE structure.</param>
        public SIZE(int cx, int cy)
        {
            this.cx = cx;
            this.cy = cy;
        }
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("BB2E617C-0920-11d1-9A0B-00C04FC2D6C1")]
    public interface IExtractImage
    {
        /// <summary>
        /// Gets a path to the image that is to be extracted.
        /// </summary>
        /// <param name="pszPathBuffer">The buffer used to return the path description. This value identifies the image so you can avoid loading the same one more than once.</param>
        /// <param name="cch">The size of pszPathBuffer in characters.</param>
        /// <param name="pdwPriority">Not used.</param>
        /// <param name="prgSize">A pointer to a SIZE structure with the desired width and height of the image. Must not be NULL.</param>
        /// <param name="dwRecClrDepth">The recommended color depth in units of bits per pixel. Must not be NULL.</param>
        /// <param name="pdwFlags">Flags that specify how the image is to be handled.</param>
        [PreserveSig]
        int GetLocation(out StringBuilder pszPathBuffer, int cch, ref int pdwPriority, ref SIZE prgSize, int dwRecClrDepth, ref int pdwFlags);

        /// <summary>
        /// Requests an image from an object, such as an item in a Shell folder.
        /// </summary>
        /// <param name="phBmpThumbnail">The buffer to hold the bitmapped image.</param>
        [PreserveSig]
        int Extract(out IntPtr phBmpThumbnail);
    }
}
