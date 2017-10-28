using System;
using System.Runtime.InteropServices;

namespace WindowsShell.Interop
{
	// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/shellcc/platform/shell/reference/ifaces/ishellfolderviewcb/ishellfolderviewcb.asp
	[ComImport,
	 Guid("2047E320-F2A9-11CE-AE65-08002B2E1262"),
	 InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IShellFolderViewCB
	{
        [PreserveSig]
        int MessageSFVCB([In] ShellFolderViewMessage uMsg, ref IntPtr wParam, ref IntPtr lParam, ref IntPtr plResult);
	}
}
