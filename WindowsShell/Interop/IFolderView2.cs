using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using WindowsShell.Interop;
using WindowsShell.Nspace.DragDrop;

namespace WindowsShell.Interop
{
    
    //https://github.com/shellscape/Shellscape.Common/blob/aa5465929e842e4bcc88c29c1cc369122c307bc6/Microsoft/Windows%20API/Shell/Interop/ExplorerBrowser/ExplorerBrowserCOMInterfaces.cs
    [ComImport, SuppressUnmanagedCodeSecurity, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("1af3a467-214f-4298-908e-06b03e0b39f9")]
    public interface IFolderView2 
    {
        [PreserveSig]
        int GetCurrentViewMode(ref int pViewMode);
        [PreserveSig]
        int SetCurrentViewMode(int ViewMode);
        [PreserveSig]
        int GetFolder(ref Guid riid, out IPersistFolder2 ppv);
        [PreserveSig]
        int Item(int iItemIndex, out IntPtr ppidl);
        [PreserveSig]
        int ItemCount(uint uFlags, out int pcItems);
        [PreserveSig]
        int Items(uint uFlags, ref Guid riid, out IEnumIDList ppv);
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
        int SelectItem(int iItem, uint dwFlags);
        [PreserveSig]
        int SelectAndPositionItems(uint cidl, IntPtr apidl, IntPtr apt, int dwFlags);
        [PreserveSig]
        int SetGroupBy(ref int key, bool fAscending);
        [PreserveSig]
        int GetGroupBy(out int pkey, out bool pfAscending);
        [PreserveSig]
        /* NOT DECLARED */
        int SetViewProperty(IntPtr pidl, ref int propkey, ref object propvar); // ?
        [PreserveSig]
        /* NOT DECLARED */
        int GetViewProperty(IntPtr pidl, ref int propkey, out object propvar); // ?
        [PreserveSig]
        int SetTileViewProperties(IntPtr pidl, [MarshalAs(UnmanagedType.LPWStr)] string pszPropList);
        [PreserveSig]
        int SetExtendedTileViewProperties(IntPtr pidl, [MarshalAs(UnmanagedType.LPWStr)] string pszPropList);
        [PreserveSig]
        int SetText(int iType, [MarshalAs(UnmanagedType.LPWStr)] string pwszText);
        [PreserveSig]
        int SetCurrentFolderFlags(int dwMask, int dwFlags);
        [PreserveSig]
        int GetCurrentFolderFlags(out int pdwFlags);
        [PreserveSig]
        int GetSortColumnCount(out int pcColumns);
        [PreserveSig]
        /* NOT DECLARED */
        int SetSortColumns(/*ref SORTCOLUMN rgSortColumns, int cColumns*/);
        [PreserveSig]
        /* NOT DECLARED */
        int GetSortColumns(out SORTCOLUMN cc, int cColumns);
        [PreserveSig]
        int GetItem(int iItem, ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppv);
        [PreserveSig]
        int GetVisibleItem(int iStart, bool fPrevious, out int piItem);
        [PreserveSig]
        int GetSelectedItem(int iStart, out int piItem);
        [PreserveSig]
        /* NOT DECLARED */
        int GetSelection(/* bool fNoneImpliesFolder, out IShellItemArray ppsia */);
        [PreserveSig]
        int GetSelectionState(IntPtr pidl, out int pdwFlags);
        [PreserveSig]
        int InvokeVerbOnSelection([MarshalAs(UnmanagedType.LPWStr)] string pszVerb);
        [PreserveSig]
        int SetViewModeAndIconSize(int uViewMode, int iImageSize);
        [PreserveSig]
        int GetViewModeAndIconSize(out int puViewMode, out int piImageSize);
        [PreserveSig]
        int SetGroupSubsetCount(uint cVisibleRows);
        [PreserveSig]
        int GetGroupSubsetCount(out uint pcVisibleRows);
        [PreserveSig]
        int SetRedraw(bool fRedrawOn);
        [PreserveSig]
        int IsMoveInSameFolder();
        [PreserveSig]
        int DoRename();
    }    
    
    [StructLayout(LayoutKind.Sequential)]
    public struct SORTCOLUMN
    {
        public PROPERTYKEY propkey;
        public SortDirection direction;

        public SORTCOLUMN(PROPERTYKEY propkey, SortDirection direction)
        {
            this.propkey = propkey;
            this.direction = direction;
        }
    }

    public enum SortDirection
    {
        /// <summary>
        /// A default value for sort direction, this value should not be used;
        /// instead use Descending or Ascending.
        /// </summary>
        Default = 0,

        /// <summary>
        /// The items are sorted in descending order. Whether the sort is alphabetical, numerical, 
        /// and so on, is determined by the data type of the column indicated in propkey.
        /// </summary>
        Descending = -1,

        /// <summary>
        /// The items are sorted in ascending order. Whether the sort is alphabetical, numerical, 
        /// and so on, is determined by the data type of the column indicated in propkey.
        /// </summary>
        Ascending = 1,
    }
}
