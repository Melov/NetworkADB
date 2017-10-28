using System;
using System.Runtime.InteropServices;

namespace WindowsShell.Interop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct MenuItemInfo
	{
		internal uint cbSize;
		internal MenuItemInfoOptions fMask;
		internal MenuItemTypes fType;
		internal MenuItemStates fState;
		internal uint wID;
		internal IntPtr hSubMenu;
		internal IntPtr hbmpChecked;
		internal IntPtr hbmpUnchecked;
		internal IntPtr dwItemData;
		internal string dwTypeData;
		internal uint cch;
		internal IntPtr hbmpItem;        
	}
}
