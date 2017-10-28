using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using WindowsShell.Interop;

namespace WindowsShell.Nspace
{
    class ExtractImageImpl : IExtractImage
    {        
        public enum IEIFLAG
        {
            ASYNC = 0x0001, // ask the extractor if it supports ASYNC extract (free threaded)
            CACHE = 0x0002, // returned from the extractor if it does NOT cache the thumbnail
            ASPECT = 0x0004, // passed to the extractor to beg it to render to the aspect ratio of the supplied rect
            OFFLINE = 0x0008, // if the extractor shouldn't hit the net to get any content neede for the rendering
            GLEAM = 0x0010, // does the image have a gleam ? this will be returned if it does
            SCREEN = 0x0020, // render as if for the screen (this is exlusive with IEIFLAG_ASPECT )
            ORIGSIZE = 0x0040, // render to the approx size passed, but crop if neccessary
            NOSTAMP = 0x0080, // returned from the extractor if it does NOT want an icon stamp on the thumbnail
            NOBORDER = 0x0100, // returned from the extractor if it does NOT want an a border around the thumbnail
            QUALITY = 0x0200 // passed to the Extract method to indicate that a slower, higher quality image is desired, re-compute the thumbnail
        }

        #region ExtractImage Private Fields

        private Size m_size = Size.Empty;
        private string m_filename = String.Empty;

        #endregion

        private IFolderObject folderObj;

        public ExtractImageImpl(IFolderObject obj)
        {
            folderObj = obj;
        }

        public int GetLocation(out StringBuilder pszPathBuffer, int cch, ref int pdwPriority, ref SIZE prgSize, int dwRecClrDepth, ref int pdwFlags)
        {
            pszPathBuffer = new StringBuilder();
            pszPathBuffer.Append(m_filename);
            
            m_size = new Size(prgSize.cx, prgSize.cy);

            if (((IEIFLAG)pdwFlags & IEIFLAG.ASYNC) != 0)
                return WinError.E_PENDING;

            //pdwFlags = pdwFlags | (int) IEIFLAG.CACHE | (int) IEIFLAG.ASYNC;

            return WinError.S_OK;
        }

        public int Extract(out IntPtr phBmpThumbnail)
        {
           // ShellIcon icon = folderObj.GetIcon(false);

            Bitmap bmp;
            Bitmap bmpNew = new Bitmap(m_size.Width, m_size.Height,PixelFormat.Format32bppArgb);
/*
            if (icon is ShellIcon.FromFile)
            {
                ShellIcon.FromFile fif = icon as ShellIcon.FromFile;
                IntPtr hIcon = (IntPtr) Shell32.ExtractIcon(IntPtr.Zero, fif.Filename, fif.Index);
			    Icon iconret = Icon.FromHandle(hIcon);
                bmp = iconret.ToBitmap();
            }
            else //(icon is ShellIcon.FromIcon)
            {
                ShellIcon.FromIcon fi = icon as ShellIcon.FromIcon;
                bmp = fi.Icon.ToBitmap();
            }
            */
            using (Graphics g = Graphics.FromImage(bmpNew))
            {
                //using (Pen p = new Pen(Color.Black))
                {
                   // g.DrawImage(bmp, 0, 0);
                    g.Clear(Color.Red);
                    g.DrawString(folderObj.GetDisplayName(NameOptions.Normal), SystemFonts.DefaultFont, new SolidBrush(Color.Black), 0, 0);
                    
                    
                    //g.DrawLine(p, 0, 0, m_size.Width, m_size.Height);
                }
            }

            phBmpThumbnail = bmpNew.GetHbitmap();

            return WinError.S_OK;
        }
    }
}
