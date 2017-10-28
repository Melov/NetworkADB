using System;
using System.Runtime.InteropServices;
using System.Text;
using WindowsShell.Nspace;

namespace WindowsShell.Interop
{
	[ComImport,
	 Guid("000214E4-0000-0000-C000-000000000046"),
	 InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IContextMenu
	{
		[PreserveSig] int QueryContextMenu([In] IntPtr hmenu, [In] uint indexMenu, [In] uint idCmdFirst, [In] uint idCmdLast, [In] ContextMenuOptions uFlags);
		void InvokeCommand([In] ref CommandInfo lpici);
		void GetCommandString([In] int idCmd, [In] CommandStringOptions uFlags, [In] IntPtr pwReserved, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=4)] byte[] pszName, [In] uint cchMax);
	}
}
