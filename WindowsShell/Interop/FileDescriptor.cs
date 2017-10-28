/*
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace WindowsShell.Interop
{
	[CLSCompliant(false)]
	public struct FileDescriptor
	{
		public static unsafe IntPtr AllocateFileGroupDescriptor(params FileDescriptor[] fds)
		{
			IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(uint)) + fds.Length * Marshal.SizeOf(typeof(FileDescriptor)));
			uint* cItems = (uint*) ptr;
			*cItems = (uint) fds.Length;

			IntPtr ptr2 = (IntPtr)((int) ptr + Marshal.SizeOf(typeof(uint)));
			
			foreach (FileDescriptor fd in fds)
			{
				Marshal.StructureToPtr(fd, ptr2, false);
				ptr2 = (IntPtr) ((int) ptr2 + Marshal.SizeOf(typeof(FileDescriptor)));
			}

			return ptr;
		}

		public FileDescriptorFlags dwFlags;
		public Guid clsid;
		public SizeL sizel;
		public PointL pointl;
		public FileAttributes dwFileAttributes;
		public long ftCreationTime;
		public long ftLastAccessTime;
		public long ftLastWriteTime;
		public uint nFileSizeHigh;
		public uint nFileSizeLow;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst=260)]
		public string cFileName;
	}
}
*/