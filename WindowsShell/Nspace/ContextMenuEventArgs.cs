using System;
using WindowsShell.Interop;

namespace WindowsShell.Nspace
{
	internal class ContextMenuEventArgs : EventArgs
	{
		private readonly CommandInfo ci;

		internal ContextMenuEventArgs(CommandInfo ci)
		{
			this.ci = ci;
		}

		internal CommandInfo CommandInfo
		{
			get
			{
				return ci;
			}
		}
	}
}
