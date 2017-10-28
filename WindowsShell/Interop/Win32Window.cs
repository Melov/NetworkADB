using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowsShell.Interop
{
	[ComVisible(false)]
	internal sealed class Win32Window : IWin32Window
	{
		internal static IWin32Window Create(IntPtr hwnd)
		{
			return hwnd == IntPtr.Zero ? null : new Win32Window(hwnd);
		}

		private readonly IntPtr hwnd;

		private Win32Window(IntPtr hwnd)
		{
			if (hwnd == IntPtr.Zero)
			{
				throw new ArgumentNullException("hwnd");
			}

			this.hwnd = hwnd;
		}

		IntPtr IWin32Window.Handle
		{
			get
			{
				return hwnd;
			}
		}
	}
}
