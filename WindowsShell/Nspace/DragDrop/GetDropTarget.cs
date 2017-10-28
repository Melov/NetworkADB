using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using WindowsShell.Interop;
using SHDocVw;

namespace WindowsShell.Nspace.DragDrop
{
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("6D5140C1-7436-11CE-8034-00AA006009FA")]
    internal interface IServiceProvider
    {
        void QueryService([MarshalAs(UnmanagedType.LPStruct)] Guid guidService, [MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppvObject);
    }

    

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("000214E2-0000-0000-C000-000000000046")]
    internal interface IShellBrowser
    {
        void _VtblGap0_12(); // skip 12 members
        void QueryActiveShellView([MarshalAs(UnmanagedType.IUnknown)] out object ppshv);
        // the rest is not defined
    }
    /*
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("cde725b0-ccc9-4519-917e-325d72fab4ce")]
    internal interface IFolderView
    {
        void _VtblGap0_2(); // skip 2 members
        void GetFolder([MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppv);
        // the rest is not defined
    }
    */
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("cde725b0-ccc9-4519-917e-325d72fab4ce")]
    public interface IFolderView
    {
        [PreserveSig]
        int GetCurrentViewMode(ref FOLDERVIEWMODE pViewMode);

        [PreserveSig]
        int SetCurrentViewMode(FOLDERVIEWMODE ViewMode);
        
        //int GetFolder(ref Guid riid, out IPersistFolder2 ppv);
        void GetFolder([MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppv);

        [PreserveSig]
        int Item(int iItemIndex, out IntPtr ppidl);

        [PreserveSig]
        int ItemCount(uint uFlags, out int pcItems);

        [PreserveSig]
        int Items(uint uFlags, [In] ref Guid riid, out object ppv);

        [PreserveSig]
        int GetSelectionMarkedItem(out int piItem);

        [PreserveSig]
        int GetFocusedItem(out int piItem);

        [PreserveSig]
        int GetItemPosition(IntPtr pidl, out POINT ppt);

        [PreserveSig]
        int GetSpacing(ref POINT ppt);

        [PreserveSig]
        int GetDefaultSpacing(ref POINT ppt);

        [PreserveSig]
        int GetAutoArrange();

        [PreserveSig]
        int SelectItem(int iItem, int dwFlags);

        [PreserveSig]
        int SelectAndPositionItems(uint cidl, IntPtr apidl, IntPtr apt, int dwFlags);
    }

    public class GetDropTarget
    {
        internal static readonly Guid SID_STopLevelBrowser = new Guid(0x4C96BE40, 0x915C, 0x11CF, 0x99, 0xD3, 0x00, 0xAA, 0x00, 0x4A, 0xE8, 0x37);
        
        public void GetPidl()
        {
            var shellWindows = new ShellWindows();
            foreach (IWebBrowser2 win in shellWindows)
            {
                IServiceProvider sp = win as IServiceProvider;
                object sb;
                sp.QueryService(SID_STopLevelBrowser, typeof(IShellBrowser).GUID, out sb);
                IShellBrowser shellBrowser = (IShellBrowser)sb;
                object sv;
                shellBrowser.QueryActiveShellView(out sv);
                Console.WriteLine(win.LocationURL + " " + win.LocationName);
                IFolderView fv = sv as IFolderView;
                if (fv != null)
                {
                    // only folder implementation support this (IE windows do not for example)
                    object pf;
                    fv.GetFolder(typeof(IPersistFolder2).GUID, out pf);
                    IPersistFolder2 persistFolder = (IPersistFolder2)pf;

                    //fv.GetFocusedItem();

                    // get folder class, for example
                    // CLSID_ShellFSFolder for standard explorer folders
                    Guid clsid;
                    persistFolder.GetClassID(out clsid);
                    Console.WriteLine(" clsid:" + clsid);

                    // get current folder pidl
                    IntPtr pidl;
                    persistFolder.GetCurFolder(out pidl);

                    // TODO: do something with pidl

                    Marshal.FreeCoTaskMem(pidl); // free pidl's allocated memory
                }
            }
        }
    }
}
