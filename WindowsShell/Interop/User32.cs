using System;
using System.Runtime.InteropServices;
using System.Text;

namespace WindowsShell.Interop
{
    public sealed class User32
	{
        //public static uint MF_BYPOSITION = 0x400;
        public static uint MF_REMOVE = 0x1000;
       // public static uint MF_GRAYED = 0x1;

        /* Menu flags for Add/Check/EnableMenuItem() */
public static uint MF_BYCOMMAND   =     0x0000;
public static uint MF_BYPOSITION   =    0x0400;

public static uint MF_SEPARATOR   =     0x0800;

public static uint MF_ENABLED     =     0x0000;
public static uint MF_GRAYED       =    0x0001;
public static uint MF_DISABLED     =    0x0002;
    
public static uint MF_UNCHECKED   =     0x0000;
public static uint MF_CHECKED      =    0x0008;
public static uint MF_USECHECKBITMAPS = 0x0200;

public static uint MF_STRING    =       0x0000;
public static uint MF_BITMAP    =       0x0004;
public static uint MF_OWNERDRAW  =      0x0100;

public static uint MF_POPUP      =      0x0010;
public static uint MF_MENUBARBREAK =    0x0020;
public static uint MF_MENUBREAK   =     0x0040;

public static uint MF_UNHILITE   =      0x0000;
public static uint MF_HILITE     =      0x0080;

public static uint MF_SYSMENU     =     0x2000;
public static uint MF_HELP       =      0x4000;
public static uint MF_MOUSESELECT  =    0x8000;


        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SHGetPathFromIDListW(IntPtr pidl, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder pszPath);

        [DllImport("user32.dll")]
        public static extern int GetMenuItemCount(IntPtr hMenu);

        [DllImport("user32.dll")]
        public static extern uint GetMenuItemID(IntPtr hMenu, int nPos);

        [DllImport("user32.dll")]
        public static extern bool RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        [DllImport("user32.dll")]
        public static extern IntPtr GetSubMenu(IntPtr hMenu, int nPos);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool AppendMenu(IntPtr hMenu, MenuFlags uFlags, uint uIDNewItem, string lpNewItem);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetMenuItemInfo(IntPtr hMenu, uint uItem, bool fByPosition, ref MenuItemInfo lpmii);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError=true)]
		internal static extern bool InsertMenuItem(IntPtr hMenu, uint uItem, bool fByPosition, ref MenuItemInfo lpmii);

		[DllImport("user32.dll", SetLastError=true)]
		internal static extern bool SetMenuItemInfo(IntPtr hMenu, uint uItem, bool fByPosition, ref MenuItemInfo lpmii);

        [DllImport("user32.dll")]
        public static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        [DllImport("user32.dll", SetLastError = true)] // SETLAST by us
        public static extern int SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

		internal static bool IsIntResource(IntPtr p)
		{
			return (((uint) p) >> 16) == 0;
		}

		private User32() {}

        /// <summary>
        /// Provides access to function required to delete handle. This method is used internally
        /// and is not required to be called separately.
        /// </summary>
        /// <param name="hIcon">Pointer to icon handle.</param>
        /// <returns>N/A</returns>
        [DllImport("User32.dll")]
        public static extern int DestroyIcon(IntPtr hIcon);

        [Flags]
        public enum MenuFlags : uint
        {
            MF_STRING = 0,
            MF_BYPOSITION = 0x400,
            MF_SEPARATOR = 0x800,
            MF_REMOVE = 0x1000,
            MF_POPUP = 0x00000010,
        }

        public enum IconSize
        {
            /// <summary>
            /// Specify large icon - 32 pixels by 32 pixels.
            /// </summary>
            Large = 0,
            /// <summary>
            /// Specify small icon - 16 pixels by 16 pixels.
            /// </summary>
            Small = 1
        }
	}
}
