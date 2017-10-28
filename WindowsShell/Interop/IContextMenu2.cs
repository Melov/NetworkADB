﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using WindowsShell.Nspace;

namespace WindowsShell.Interop
{
    /// <summary>
    /// Exposes methods that either create or merge a shortcut (context) menu associated with a Shell object. Extends IContextMenu by adding a method that allows client objects to handle messages associated with owner-drawn menu items.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("000214f4-0000-0000-c000-000000000046")]
    internal interface IContextMenu2 : IContextMenu
    {
        #region IContextMenu overrides
        [PreserveSig]
        new int QueryContextMenu(IntPtr hMenu, uint indexMenu, int idCmdFirst, int idCmdLast, ContextMenuOptions uFlags);
        new void InvokeCommand(IntPtr pici);
        new void GetCommandString(int idcmd, CommandStringOptions uflags, int reserved, StringBuilder commandstring, int cch);

        #endregion

        /// <summary>
        /// Enables client objects of the IContextMenu interface to handle messages associated with owner-drawn menu items.
        /// </summary>
        /// <param name="uMsg">The message to be processed. In the case of some messages, such as WM_INITMENUPOPUP, WM_DRAWITEM, WM_MENUCHAR, or WM_MEASUREITEM, the client object being called may provide owner-drawn menu items.</param>
        /// <param name="wParam">Additional message information. The value of this parameter depends on the value of the uMsg parameter.</param>
        /// <param name="lParam">Additional message information. The value of this parameter depends on the value of the uMsg parameter.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int HandleMenuMsg(uint uMsg, IntPtr wParam, IntPtr lParam);
    }
}
