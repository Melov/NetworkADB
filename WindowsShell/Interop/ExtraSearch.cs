using System;
using System.Runtime.InteropServices;

namespace WindowsShell.Interop
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
	public struct ExtraSearch
	{
		public Guid guidSearch;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst=80)]
		public string wszFriendlyName;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst=2048)]
		public string wszUrl;
	}
}
