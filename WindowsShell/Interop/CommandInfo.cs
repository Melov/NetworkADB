using System;

namespace WindowsShell.Interop
{
    public struct CommandInfo
	{
		internal uint cbSize;         // sizeof(CMINVOKECOMMANDINFO)
		internal CommandInfoMasks fMask; // any combination of CMIC_MASK_*
		internal IntPtr hwnd;         // might be NULL (indicating no owner window)
		internal IntPtr lpVerb;       // either a string or MAKEINTRESOURCE(idOffset)
		internal string lpParameters; // might be NULL (indicating no parameter)
		internal string lpDirectory;  // might be NULL (indicating no specific directory)
		internal int nShow;           // one of SW_ values for ShowWindow() API
		internal uint dwHotKey;
		internal IntPtr hIcon;
	}
}
