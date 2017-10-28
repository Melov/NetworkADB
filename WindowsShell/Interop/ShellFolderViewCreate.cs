using System;
using System.Runtime.InteropServices;

namespace WindowsShell.Interop
{
	// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/shellcc/platform/shell/reference/structures/SFV_CREATE.asp
	[ComVisible(false),
	 CLSCompliant(false)]
    [StructLayout(LayoutKind.Sequential)]
	public struct ShellFolderViewCreate
	{
		// creates a SFVC with no callback; according to MSDN this member can
		// be null
		public ShellFolderViewCreate(IShellFolder pshf)
			: this(pshf, null)
		{
		}

		public ShellFolderViewCreate(IShellFolder pshf, IShellFolderViewCB psfvcb)
		{
			cbSize = (uint) Marshal.SizeOf(typeof(ShellFolderViewCreate));
			this.pshf = pshf;
			this.psvOuter = null;
			this.psfvcb = psfvcb;
		}

		public uint cbSize;
		public IShellFolder pshf;
		public /* IShellView */ object psvOuter;
		public IShellFolderViewCB psfvcb;
	}
}
