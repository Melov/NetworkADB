using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using WindowsShell.Interop;

namespace WindowsShell.Nspace.Icon
{
    public static class IconHelper
    {
        public static IImageList list_SHIL_SMALL;
        public static IImageList list_SHIL_LARGE;
        public static IImageList list_SHIL_EXTRALARGE;
        public static IImageList list_SHIL_JUMBO;

        public static System.Drawing.Icon AddIconOverlay(System.Drawing.Icon originalIcon, System.Drawing.Icon overlay)
        {
            Image a = originalIcon.ToBitmap();
            Image b = overlay.ToBitmap();
            Bitmap bitmap = new Bitmap(overlay.Width, overlay.Height);
            Graphics canvas = Graphics.FromImage(bitmap);
            canvas.CompositingMode = CompositingMode.SourceOver;
            canvas.DrawImage(a, new Point(0, 0));
            canvas.DrawImage(b, new Point(0, 0));
            canvas.Save();
            canvas.Dispose();
            a.Dispose();
            b.Dispose();
            return System.Drawing.Icon.FromHandle(bitmap.GetHicon());
        }

        public static string GetExtension(string str)
        {
            string ext = null;
            int pos = str.LastIndexOf('.');
            if (pos > 0)
            {
                ext = str.Substring(pos);
                if (ext.Equals("."))
                {
                    ext = null;
                }
            }
            return ext;
        }

        public static void GetIcons(IFileObject[] fo, ref Dictionary<string, Dictionary<int, System.Drawing.Icon>> itemsIcons)
        {
            Dictionary<string, int> lNewExt = new Dictionary<string, int>();
            foreach (IFileObject fileObject in fo)
            {
                if (!fileObject.IsFolder)
                {
                    string ext = IconHelper.GetExtension(fileObject.Name);
                    if (!string.IsNullOrEmpty(ext) && !itemsIcons.ContainsKey(ext) && !lNewExt.ContainsKey(ext))
                    {
                        int index = IconHelper.GetIconIndex(ext);
                        lNewExt.Add(ext, index);
                    }
                }
            }

            ReleaseImageLists();
            CreateImageLists();

            if (!itemsIcons.ContainsKey("folder"))
            {
                lNewExt.Add("folder", 3);
            }

            if (!itemsIcons.ContainsKey("no"))
            {
                lNewExt.Add("no", 0);
            }

            foreach (KeyValuePair<string, int> pair in lNewExt)
            {
                itemsIcons.Add(pair.Key, GetImagesFromLists(pair.Value));
            }

            ReleaseImageLists();
        }

        public static void ReleaseImageLists()
        {
            if (list_SHIL_SMALL != null)
            {
                Marshal.FinalReleaseComObject(list_SHIL_SMALL);
                list_SHIL_SMALL = null;
            }
            if (list_SHIL_LARGE != null)
            {
                Marshal.FinalReleaseComObject(list_SHIL_LARGE);
                list_SHIL_LARGE = null;
            }
            if (list_SHIL_EXTRALARGE != null)
            {
                Marshal.FinalReleaseComObject(list_SHIL_EXTRALARGE);
                list_SHIL_EXTRALARGE = null;
            }
            if (list_SHIL_JUMBO != null)
            {
                Marshal.FinalReleaseComObject(list_SHIL_JUMBO);
                list_SHIL_JUMBO = null;
            }
        }

        public static void CreateImageLists()
        {
            Guid guil = new Guid(Shell32.IID_IImageList2);
            Guid guil2 = new Guid(Shell32.IID_IImageList);

            if (list_SHIL_SMALL == null)
                Shell32.SHGetImageList(Shell32.SHIL_SMALL, ref guil, ref list_SHIL_SMALL);
            if (list_SHIL_LARGE == null)
                Shell32.SHGetImageList(Shell32.SHIL_LARGE, ref guil, ref list_SHIL_LARGE);
            if (list_SHIL_EXTRALARGE == null)
                Shell32.SHGetImageList(Shell32.SHIL_EXTRALARGE, ref guil, ref list_SHIL_EXTRALARGE);
            if (list_SHIL_JUMBO == null)
                Shell32.SHGetImageList(Shell32.SHIL_JUMBO, ref guil, ref list_SHIL_JUMBO);
        }

        public static Dictionary<int, System.Drawing.Icon> GetImagesFromLists(int index)
        {
            Dictionary<int, System.Drawing.Icon> dic = new Dictionary<int, System.Drawing.Icon>();

            int indexOverlay = 29;

            System.Drawing.Icon icon_s;
            System.Drawing.Icon icon_s_o;
            System.Drawing.Icon icon_l;
            System.Drawing.Icon icon_l_o;
            System.Drawing.Icon icon_e;
            System.Drawing.Icon icon_e_o;
            System.Drawing.Icon icon_j;
            System.Drawing.Icon icon_j_o;

            IntPtr hIcon = IntPtr.Zero;
            //list_SHIL_SMALL.SetOverlayImage(index, indexOverlay);
            list_SHIL_SMALL.GetIcon((int)index, Shell32.ILD_TRANSPARENT | Shell32.ILD_IMAGE, ref hIcon);
            System.Drawing.Icon orig = (System.Drawing.Icon)System.Drawing.Icon.FromHandle(hIcon);
            icon_s = (System.Drawing.Icon)orig;
            //orig.Dispose();
            //User32.DestroyIcon(hIcon);            

            hIcon = IntPtr.Zero;
            //list_SHIL_LARGE.SetOverlayImage(index, indexOverlay);
            list_SHIL_LARGE.GetIcon((int)index, Shell32.ILD_TRANSPARENT | Shell32.ILD_IMAGE, ref hIcon);
            orig = (System.Drawing.Icon)System.Drawing.Icon.FromHandle(hIcon);
            icon_l = (System.Drawing.Icon)orig;
            //orig.Dispose();
            //User32.DestroyIcon(hIcon);

            hIcon = IntPtr.Zero;
            //list_SHIL_EXTRALARGE.SetOverlayImage(index, indexOverlay);
            list_SHIL_EXTRALARGE.GetIcon((int)index, Shell32.ILD_TRANSPARENT | Shell32.ILD_IMAGE, ref hIcon);
            orig = (System.Drawing.Icon)System.Drawing.Icon.FromHandle(hIcon);
            icon_e = (System.Drawing.Icon)orig;
            //orig.Dispose();
            //User32.DestroyIcon(hIcon);

            hIcon = IntPtr.Zero;
            //list_SHIL_JUMBO.SetOverlayImage(index, indexOverlay);
            list_SHIL_JUMBO.GetIcon((int)index, Shell32.ILD_TRANSPARENT | Shell32.ILD_IMAGE, ref hIcon);
            orig = (System.Drawing.Icon)System.Drawing.Icon.FromHandle(hIcon);
            icon_j = (System.Drawing.Icon)orig;
            //orig.Dispose();
            //User32.DestroyIcon(hIcon);

            /////////overlays
            hIcon = IntPtr.Zero;
            list_SHIL_SMALL.GetIcon((int)indexOverlay, Shell32.ILD_TRANSPARENT | Shell32.ILD_IMAGE, ref hIcon);
            orig = (System.Drawing.Icon)System.Drawing.Icon.FromHandle(hIcon);
            icon_s_o = (System.Drawing.Icon)orig;
            //orig.Dispose();
            //User32.DestroyIcon(hIcon);

            hIcon = IntPtr.Zero;
            list_SHIL_LARGE.GetIcon((int)indexOverlay, Shell32.ILD_TRANSPARENT | Shell32.ILD_IMAGE, ref hIcon);
            orig = (System.Drawing.Icon)System.Drawing.Icon.FromHandle(hIcon);
            icon_l_o = (System.Drawing.Icon)orig;
            //orig.Dispose();
            //User32.DestroyIcon(hIcon);

            hIcon = IntPtr.Zero;
            list_SHIL_EXTRALARGE.GetIcon((int)indexOverlay, Shell32.ILD_TRANSPARENT | Shell32.ILD_IMAGE, ref hIcon);
            orig = (System.Drawing.Icon)System.Drawing.Icon.FromHandle(hIcon);
            icon_e_o = (System.Drawing.Icon)orig;
            //orig.Dispose();
            //User32.DestroyIcon(hIcon);

            hIcon = IntPtr.Zero;
            list_SHIL_JUMBO.GetIcon((int)indexOverlay, Shell32.ILD_TRANSPARENT | Shell32.ILD_IMAGE, ref hIcon);
            orig = (System.Drawing.Icon)System.Drawing.Icon.FromHandle(hIcon);
            icon_j_o = (System.Drawing.Icon)orig;
            //orig.Dispose();
            //User32.DestroyIcon(hIcon);

            dic.Add(16, icon_s);
            dic.Add(32, icon_l);
            dic.Add(48, icon_e);
            dic.Add(256, icon_j);

            dic.Add(1000 + 16, icon_s_o);
            dic.Add(1000 + 32, icon_l_o);
            dic.Add(1000 + 48, icon_e_o);
            dic.Add(1000 + 256, icon_j_o);

            return dic;
        }

        public static Dictionary<int, System.Drawing.Icon> GetImagesByExt(string ext)
        {
            

            int index = 0;//Unknown File Type
            if (!string.IsNullOrEmpty(ext))
            {
                if (ext.Equals("folder"))
                {
                    index = 3;
                }
                else
                {
                    index = IconHelper.GetIconIndex(ext);
                }
            }

            //index = 29;
            //http://www.codeproject.com/Articles/2405/Retrieving-shell-icons
            int indexOverlay = 29;//Shell32.SHGetIconOverlayIndexW(null, Shell32.IDO_SHGIOI_SHARE);            

            //IntPtr hIconO = (IntPtr)Shell32.ExtractIcon(Process.GetCurrentProcess().Handle, @"C:\Windows\System32\Shell32.dll", 29);
            //System.Drawing.Icon iconO = System.Drawing.Icon.FromHandle(hIconO);

            

            /*
            IImageList list_SHIL_SMALL = null;
            IImageList list_SHIL_LARGE = null;
            IImageList list_SHIL_EXTRALARGE = null;
            IImageList list_SHIL_JUMBO = null;
             */
            
            ReleaseImageLists();
            CreateImageLists();

            Dictionary<int, System.Drawing.Icon> dic = GetImagesFromLists(index);

            //icon_s = AddIconOverlay(icon_s, icon_s_o);
            //icon_l = AddIconOverlay(icon_l, icon_l_o);
            //icon_e = AddIconOverlay(icon_e, icon_e_o);
            //icon_j = AddIconOverlay(icon_j, icon_j_o);

            ReleaseImageLists();
          
            return dic;
        }

        public static int GetIconIndex(string pszFile)
        {
            SHFILEINFO sfi = new SHFILEINFO();
            Shell32.SHGetFileInfo(pszFile
                , 0
                , out sfi
                , (uint)System.Runtime.InteropServices.Marshal.SizeOf(sfi)
                , (SHGFI.SHGFI_SYSICONINDEX | SHGFI.SHGFI_LARGEICON | SHGFI.SHGFI_USEFILEATTRIBUTES));
            return sfi.iIcon;
        }

        // 256*256
        public static IntPtr GetJumboIcon(int iImage)
        {
            IImageList spiml = null;
            Guid guil = new Guid(Shell32.IID_IImageList2);//or IID_IImageList

            Shell32.SHGetImageList(Shell32.SHIL_JUMBO, ref guil, ref spiml);
            IntPtr hIcon = IntPtr.Zero;
            spiml.GetIcon(iImage, Shell32.ILD_TRANSPARENT | Shell32.ILD_IMAGE, ref hIcon); //

            return hIcon;
        }

        // 48X48
        public static IntPtr GetXLIcon(int iImage)
        {
            IImageList spiml = null;
            Guid guil = new Guid(Shell32.IID_IImageList2);//or IID_IImageList

            Shell32.SHGetImageList(Shell32.SHIL_EXTRALARGE, ref guil, ref spiml);
            IntPtr hIcon = IntPtr.Zero;
            spiml.GetIcon(iImage, Shell32.ILD_TRANSPARENT | Shell32.ILD_IMAGE, ref hIcon); //

            return hIcon;
        }

        public static System.Drawing.Icon GetFileIcon(string name, User32.IconSize size, bool linkOverlay)
        {
            SHFILEINFO shfi = new SHFILEINFO();
            uint flags = Shell32.SHGFI_ICON | Shell32.SHGFI_USEFILEATTRIBUTES;

            if (true == linkOverlay) flags += Shell32.SHGFI_LINKOVERLAY;


            /* Check the size specified for return. */
            if (User32.IconSize.Small == size)
            {
                flags += Shell32.SHGFI_SMALLICON; // include the small icon flag
            }
            else
            {
                flags += Shell32.SHGFI_LARGEICON;  // include the large icon flag
            }

            Shell32.SHGetFileInfo(name,
                Shell32.FILE_ATTRIBUTE_NORMAL,
                out shfi,
                (uint)System.Runtime.InteropServices.Marshal.SizeOf(shfi),
                (SHGFI)flags);


            // Copy (clone) the returned icon to a new object, thus allowing us 
            // to call DestroyIcon immediately
            System.Drawing.Icon icon = (System.Drawing.Icon)
                                 System.Drawing.Icon.FromHandle(shfi.hIcon).Clone();
            User32.DestroyIcon(shfi.hIcon); // Cleanup
            return icon;
        }
    }
}
