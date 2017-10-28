using System;

namespace WindowsShell.Interop
{
	internal struct ShellExecuteInfo
	{
		internal uint cbSize;
		internal ShellExecuteOptions fMask;
		internal IntPtr hwnd;
		internal string lpVerb;
		internal string lpFile;
		internal string lpParameters;
		internal string lpDirectory;
		internal int nShow;
		internal IntPtr hInstanceApp;
		internal IntPtr lpIDList;
		internal string lpClass;
		internal IntPtr hkeyClass;
		internal uint dwHotKey;
		internal IntPtr hIconMonitor;
		internal IntPtr hProcess;
	}
}
