using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using WindowsShell.Nspace;

namespace WindowsShell.Interop
{
	[ComVisible(false),
	 CLSCompliant(false)]
	public sealed class Shell32
	{
        public static int OFASI_EDIT = 1;

		internal static Malloc GetMalloc()
		{
			IntPtr pMalloc;
			Marshal.ThrowExceptionForHR(Shell32.SHGetMalloc(out pMalloc));
			return new Malloc(pMalloc);
		}

        [DllImport("shell32.dll", SetLastError = true)]
        internal static extern int ExtractIcon(IntPtr hInst, string lpszExeFileName, int nIconIndex);

		// ms-help://MS.MSDNQTR.2003FEB.1033/shellcc/platform/shell/reference/functions/shellexecuteex.htm
		[DllImport("shell32.dll", SetLastError=true)]
		internal static extern int ShellExecuteEx(ref ShellExecuteInfo lpExecInfo);

		[DllImport("shell32.dll")]
		internal static extern int SHGetMalloc(out IntPtr ppMalloc);

		// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/shellcc/platform/shell/reference/Functions/SHCreateShellFolderView.asp
		[DllImport("shell32.dll")]
        internal static extern int SHCreateShellFolderView(ref SFV_CREATE pcsfv, out IShellView ppsv);

		// ms-help://MS.MSDNQTR.2003FEB.1033/shellcc/platform/shell/reference/functions/shchangenotify.htm
		[DllImport("shell32.dll")]
		public static extern void SHChangeNotify(ShellChangeEvents wEventId, ShellChangeFlags uFlags, IntPtr dwItem1, IntPtr dwItem2);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int SHGetIDListFromObject(IntPtr iUnknown, [Out, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ItemIdListMarshaler))] out ItemIdList pidl);

        [DllImport("shell32.dll")]
        public static extern int SHOpenFolderAndSelectItems(IntPtr pidlFolder, uint cidl, IntPtr[] apidl, int dwFlags);

        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttribs, out SHFILEINFO psfi, uint cbFileInfo, SHGFI uFlags);

        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(IntPtr pszPath, uint dwFileAttribs, out SHFILEINFO psfi, uint cbFileInfo, SHGFI uFlags);

        [DllImport("shell32.dll")]
        public static extern int AssocCreateForClasses(ASSOCIATIONELEMENT[] rgClasses, uint cClasses, Guid riid, out IntPtr ppv);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr ILCreateFromPath([In, MarshalAs(UnmanagedType.LPWStr)] string pszPath);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern IntPtr ILCreateFromPathW(string pszPath);

        [DllImport("shell32.dll", ExactSpelling = true)]
        public static extern void ILFree(IntPtr pidlList);

        [DllImport("shell32.dll", CharSet = CharSet.None)]
        public static extern int ILGetSize(IntPtr pidl);

        [DllImport("shell32.dll")]
        public static extern IntPtr ILCombine(IntPtr pidl1, IntPtr pidl2);

        [DllImport("shell32.dll")]
        public static extern IntPtr ILClone(IntPtr pidl);

        [DllImport("shell32.dll")]
        public static extern bool ILIsEqual(IntPtr pidl1, IntPtr pidl2);

        [DllImport("shell32.dll")]
        public static extern Int32 SHBindToParent(
          IntPtr pidls,
          [MarshalAs(UnmanagedType.LPStruct)] Guid riid,
          out IntPtr ppv,
          ref IntPtr ppidlLast);

        [DllImport("shell32.dll")]
        public static extern Int32 SHGetSpecialFolderLocation(IntPtr hwndOwner, int nFolder, ref IntPtr ppidl);

        /// <summary>
        /// Makes a copy of a string in newly allocated memory.
        /// </summary>
        /// <param name="pszSource">A pointer to the null-terminated string to be copied.</param>
        /// <param name="ppwsz">A pointer to an allocated Unicode string that contains the result. SHStrDup allocates memory for this string with CoTaskMemAlloc. You should free the string with CoTaskMemFree when it is no longer needed. In the case of failure, this value is NULL.</param>
        /// <returns>Returns S_OK if successful, or a COM error value otherwise.</returns>
        [DllImport("shlwapi.dll", EntryPoint = "SHStrDupW")]
        public static extern int SHStrDup([MarshalAs(UnmanagedType.LPWStr)] string pszSource, out IntPtr ppwsz);

        [DllImport("shell32.dll", EntryPoint = "#727")]
        public extern static int SHGetImageList(int iImageList, ref Guid riid, ref IImageList ppv);

        [DllImport("shell32.dll")]
        public static extern int SHGetIconOverlayIndexW(string pszIconPath, int iIconIndex);

		private Shell32() {}

        /*
        private const int MaxPath = 256;
        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            private const int Namesize = 80;
            public readonly IntPtr hIcon;
            private readonly int iIcon;
            private readonly uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MaxPath)]
            private readonly string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Namesize)]
            private readonly string szTypeName;
        };
        */

	    public const int IDO_SHGIOI_LINK = 0x0FFFFFFE;
        public const int IDO_SHGIOI_SHARE = 0x0FFFFFFF;
        public const int IDO_SHGIOI_SLOWFILE = 0x0FFFFFFD;
        public const int IDO_SHGIOI_DEFAULT = 0x0FFFFFFC;

        public const uint SHGFI_ICON = 0x000000100;     // get icon
        public const uint SHGFI_LINKOVERLAY = 0x000008000;     // put a link overlay on icon
        public const uint SHGFI_LARGEICON = 0x000000000;     // get large icon
        public const uint SHGFI_SMALLICON = 0x000000001;     // get small icon
        public const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;     // use passed dwFileAttribute
        public const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;

        public const string IID_IImageList = "46EB5926-582E-4017-9FDF-E8998DAA0950";
        public const string IID_IImageList2 = "192B9D83-50FC-457B-90A0-2B82A8B5DAE1";

        public const int SHIL_LARGE = 0x0;
        public const int SHIL_SMALL = 0x1;
        public const int SHIL_EXTRALARGE = 0x2;
        public const int SHIL_SYSSMALL = 0x3;
        public const int SHIL_JUMBO = 0x4;
        public const int SHIL_LAST = 0x4;

        public const int ILD_TRANSPARENT = 0x00000001;
        public const int ILD_IMAGE = 0x00000020;

        //http://stackoverflow.com/questions/4877260/copy-file-from-truecrypt-volume-to-clipboard/4927766#4927766
        private static MemoryStream CreateShellIDList(List<string> filenames)
        {
            // first convert all files into pidls list
            int pos = 0;
            byte[][] pidls = new byte[filenames.Count][];
            foreach (var filename in filenames)
            {
                // Get pidl based on name
                IntPtr pidl = ILCreateFromPath(filename);
                int pidlSize = ILGetSize(pidl);
                // Copy over to our managed array
                pidls[pos] = new byte[pidlSize];
                Marshal.Copy(pidl, pidls[pos++], 0, pidlSize);
                ILFree(pidl);
            }

            // Determine where in CIDA we will start pumping PIDLs
            int pidlOffset = 4 * (filenames.Count + 2);
            // Start the CIDA stream stream
            var memStream = new MemoryStream();
            var sw = new BinaryWriter(memStream);
            // Initialize CIDA witha count of files
            sw.Write(filenames.Count);
            // Calcualte and write relative offsets of every pidl starting with root
            sw.Write(pidlOffset);
            pidlOffset += 4; // root is 4 bytes
            foreach (var pidl in pidls)
            {
                sw.Write(pidlOffset);
                pidlOffset += pidl.Length;
            }

            // Write the root pidl (0) followed by all pidls
            sw.Write(0);
            foreach (var pidl in pidls) sw.Write(pidl);
            // stream now contains the CIDA
            return memStream;
        }
	}

    [StructLayout(LayoutKind.Sequential)]
    public struct SFV_CREATE
    {
        /// <summary>
        /// The size of the SFV_CREATE structure, in bytes.
        /// </summary>
        public uint cbSize;

        /// <summary>
        /// The IShellFolder interface of the folder for which to create the view.
        /// </summary>
        public IShellFolder pshf;

        /// <summary>
        /// A pointer to the parent IShellView interface. This parameter may be NULL. This parameter is used only when the view created by SHCreateShellFolderView is hosted in a common dialog box.
        /// </summary>
        public object psvOuter;

        /// <summary>
        /// A pointer to the IShellFolderViewCB interface that handles the view's callbacks when various events occur. This parameter may be NULL.
        /// </summary>
        public IShellFolderViewCB psfvcb;
    }
}
