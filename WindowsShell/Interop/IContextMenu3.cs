using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using WindowsShell.Nspace;

namespace WindowsShell.Interop
{
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("BCFCE0A0-EC17-11d0-8D10-00A0C90F2719")]
    internal interface IContextMenu3 : IContextMenu2
    {
        #region IContextMenu and IContextMenu2 overrides

        new int QueryContextMenu(IntPtr hMenu, uint indexMenu, int idCmdFirst, int idCmdLast, ContextMenuOptions uFlags);
        new int InvokeCommand(IntPtr pici);
        new int GetCommandString(int idcmd, CommandStringOptions uflags, int reserved, StringBuilder commandstring, int cch);
        new int HandleMenuMsg(uint uMsg, IntPtr wParam, IntPtr lParam);

        #endregion

        /// <summary>
        /// Allows client objects of the IContextMenu3 interface to handle messages associated with owner-drawn menu items.
        /// </summary>
        /// <param name="uMsg">The message to be processed. In the case of some messages, such as WM_INITMENUPOPUP, WM_DRAWITEM, WM_MENUCHAR, or WM_MEASUREITEM, the client object being called may provide owner-drawn menu items.</param>
        /// <param name="wParam">Additional message information. The value of this parameter depends on the value of the uMsg parameter.</param>
        /// <param name="lParam">Additional message information. The value of this parameter depends on the value of the uMsg parameter.</param>
        /// <param name="plResult">The address of an LRESULT value that the owner of the menu will return from the message. This parameter can be NULL.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int HandleMenuMsg2(uint uMsg, IntPtr wParam, IntPtr lParam, ref IntPtr plResult);
    }
}
