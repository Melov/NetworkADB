using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace MyData.Extensions
{
    public class DataObjectEx : DataObject, System.Runtime.InteropServices.ComTypes.IDataObject
    {
        private static readonly TYMED[] ALLOWED_TYMEDS =
            new TYMED[] { 
                TYMED.TYMED_HGLOBAL,
                TYMED.TYMED_ISTREAM, 
                TYMED.TYMED_ENHMF,
                TYMED.TYMED_MFPICT,
                TYMED.TYMED_GDI};

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1 )]
        public struct FILEGROUPDESCRIPTOR
        {
            public UInt32 cItems;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 1)]
            public FILEDESCRIPTOR[] fgd;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1 )]
        public struct FILEDESCRIPTOR
        {
            public UInt32 dwFlags;
            public Guid clsid;
            public System.Drawing.Size sizel;
            public System.Drawing.Point pointl;
            public UInt32 dwFileAttributes;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
            public UInt32 nFileSizeHigh;
            public UInt32 nFileSizeLow;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public String cFileName;
        }

        public struct SelectedItem
        {
            public String FileName;            
            public DateTime WriteTime;
            public Int64 FileSize;

            public String TempContenet;
            public byte[] TempContenet1;
        }

        private SelectedItem[] m_SelectedItems;
        private Int32 m_lindex;

        public DataObjectEx(SelectedItem[] selectedItems)
        {
            m_SelectedItems = selectedItems;
        }
       
        public override object GetData(string format, bool autoConvert)
        {
            if (String.Compare(format, NativeMethods.CFSTR_FILEDESCRIPTORW, StringComparison.OrdinalIgnoreCase) == 0 && m_SelectedItems != null)
            {
                base.SetData(NativeMethods.CFSTR_FILEDESCRIPTORW, GetFileDescriptor(m_SelectedItems));
            }
            else if (String.Compare(format, NativeMethods.CFSTR_FILECONTENTS, StringComparison.OrdinalIgnoreCase) == 0)
            {
                base.SetData(NativeMethods.CFSTR_FILECONTENTS, GetFileContents(m_SelectedItems, m_lindex));
            }
            else if (String.Compare(format, NativeMethods.CFSTR_PERFORMEDDROPEFFECT, StringComparison.OrdinalIgnoreCase) == 0)
            {
                //TODO: Cleanup routines after paste has been performed
            }
            return base.GetData(format, autoConvert);
        }

        public override object GetData(string format)
        {
            if (String.Compare(format, NativeMethods.CFSTR_FILEDESCRIPTORW, StringComparison.OrdinalIgnoreCase) == 0 && m_SelectedItems != null)
            {
                base.SetData(NativeMethods.CFSTR_FILEDESCRIPTORW, GetFileDescriptor(m_SelectedItems));
            }
            else if (String.Compare(format, NativeMethods.CFSTR_FILECONTENTS, StringComparison.OrdinalIgnoreCase) == 0)
            {
                base.SetData(NativeMethods.CFSTR_FILECONTENTS, GetFileContents(m_SelectedItems, m_lindex));
            }
            else if (String.Compare(format, NativeMethods.CFSTR_PERFORMEDDROPEFFECT, StringComparison.OrdinalIgnoreCase) == 0)
            {
                //TODO: Cleanup routines after paste has been performed
            }
            return base.GetData(format);
        }

        //[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        void System.Runtime.InteropServices.ComTypes.IDataObject.GetData(ref System.Runtime.InteropServices.ComTypes.FORMATETC formatetc, out System.Runtime.InteropServices.ComTypes.STGMEDIUM medium)
        {
            if (formatetc.cfFormat == (Int16)DataFormats.GetFormat(NativeMethods.CFSTR_FILECONTENTS).Id)
                m_lindex = formatetc.lindex;

            medium = new System.Runtime.InteropServices.ComTypes.STGMEDIUM();
            if (GetTymedUseable(formatetc.tymed))
            {
                if ((formatetc.tymed & TYMED.TYMED_HGLOBAL) != TYMED.TYMED_NULL)
                {
                    medium.tymed = TYMED.TYMED_HGLOBAL;
                    medium.unionmember = NativeMethods.GlobalAlloc(NativeMethods.GHND | NativeMethods.GMEM_DDESHARE, 1);
                    if (medium.unionmember == IntPtr.Zero)
                    {
                        throw new OutOfMemoryException();
                    }
                    try
                    {
                        ((System.Runtime.InteropServices.ComTypes.IDataObject)this).GetDataHere(ref formatetc, ref medium);
                        return;
                    }
                    catch
                    {
                        NativeMethods.GlobalFree(new HandleRef((STGMEDIUM)medium, medium.unionmember));
                        medium.unionmember = IntPtr.Zero;
                        throw;                        
                    }
                }
                medium.tymed = formatetc.tymed;
                ((System.Runtime.InteropServices.ComTypes.IDataObject)this).GetDataHere(ref formatetc, ref medium);
            }
            else
            {
                Marshal.ThrowExceptionForHR(NativeMethods.DV_E_TYMED);
            }
        }

        private static Boolean GetTymedUseable(TYMED tymed)
        {
            for (Int32 i = 0; i < ALLOWED_TYMEDS.Length; i++)
            {
                if ((tymed & ALLOWED_TYMEDS[i]) != TYMED.TYMED_NULL)
                {
                    return true;
                }
            }
            return false;
        }

        private MemoryStream GetFileDescriptor(SelectedItem[] SelectedItems)
        {
            MemoryStream FileDescriptorMemoryStream = new MemoryStream();
            // Write out the FILEGROUPDESCRIPTOR.cItems value
            FileDescriptorMemoryStream.Write(BitConverter.GetBytes(SelectedItems.Length), 0, sizeof(UInt32));

            FILEDESCRIPTOR FileDescriptor = new FILEDESCRIPTOR();
            foreach (SelectedItem si in SelectedItems)
            {
                FileDescriptor.cFileName = si.FileName;
                Int64 FileWriteTimeUtc = si.WriteTime.ToFileTimeUtc();
                FileDescriptor.ftLastWriteTime.dwHighDateTime = (Int32)(FileWriteTimeUtc >> 32);
                FileDescriptor.ftLastWriteTime.dwLowDateTime = (Int32)(FileWriteTimeUtc & 0xFFFFFFFF);
                FileDescriptor.nFileSizeHigh = (UInt32)(si.FileSize >> 32);
                FileDescriptor.nFileSizeLow = (UInt32)(si.FileSize & 0xFFFFFFFF);
                FileDescriptor.dwFlags = NativeMethods.FD_WRITESTIME | NativeMethods.FD_FILESIZE | NativeMethods.FD_PROGRESSUI;

                // Marshal the FileDescriptor structure into a byte array and write it to the MemoryStream.
                Int32 FileDescriptorSize = Marshal.SizeOf(FileDescriptor);
                IntPtr FileDescriptorPointer = Marshal.AllocHGlobal(FileDescriptorSize);
                Marshal.StructureToPtr(FileDescriptor, FileDescriptorPointer, true);
                Byte[] FileDescriptorByteArray = new Byte[FileDescriptorSize];
                Marshal.Copy(FileDescriptorPointer, FileDescriptorByteArray, 0, FileDescriptorSize);
                Marshal.FreeHGlobal(FileDescriptorPointer);                                
                FileDescriptorMemoryStream.Write(FileDescriptorByteArray, 0, FileDescriptorByteArray.Length);
            }
            FileDescriptorMemoryStream.Position = 0;
            return FileDescriptorMemoryStream;
        }

        private MemoryStream GetFileContents(SelectedItem[] SelectedItems, Int32 FileNumber)
        {
            MemoryStream FileContentMemoryStream = null;
            if (SelectedItems != null && FileNumber < SelectedItems.Length)
            {
                FileContentMemoryStream = new MemoryStream();
                SelectedItem si = SelectedItems[FileNumber];

                // **************************************************************************************
                // TODO: Get the virtual file contents and place the contents in the byte array bBuffer.
                // If the contents are zero length then a single byte must be supplied to Windows
                // Explorer otherwise the transfer will fail.  If this is part of a multi-file transfer,
                // the entire transfer will fail at this point if the buffer is zero length.
                // **************************************************************************************

                Byte[] bBuffer;
                if (si.TempContenet1 != null && si.TempContenet1.Length > 0)
                {
                    bBuffer = si.TempContenet1;
                }
                else
                {
                    bBuffer = Encoding.Default.GetBytes(si.TempContenet); 
                }
                

                
                if (bBuffer.Length == 0)  // Must send at least one byte for a zero length file to prevent stoppages.
                    bBuffer = new Byte[1];
                FileContentMemoryStream.Write(bBuffer, 0, bBuffer.Length);
            }
            return FileContentMemoryStream;
        }

    }

    public class NativeMethods
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Overlapped
        {
            IntPtr intrnal;
            IntPtr internalHigh;
            int offset;
            int offsetHigh;
            IntPtr hEvent;
        }

        // declared as class
        [StructLayout(LayoutKind.Sequential)]
        public class Overlapped2
        {
            IntPtr intrnal;
            IntPtr internalHigh;
            int offset;
            int offsetHigh;
            IntPtr hEvent;
        }

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr GlobalLock(IntPtr handle);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern UIntPtr GlobalSize(IntPtr hMem);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr GlobalUnlock(IntPtr handle);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GlobalAlloc(int uFlags, int dwBytes);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GlobalFree(HandleRef handle);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern int DragQueryFile(HandleRef hDrop, int iFile, [Out] StringBuilder lpszFile, int cch);

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool ReadFile(
            HandleRef hndRef,
            StringBuilder buffer,
            int numberOfBytesToRead,
            out int numberOfBytesRead,
            ref Overlapped flag);

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "ReadFile")]
        public static extern bool ReadFile2(
            HandleRef hndRef,
            StringBuilder buffer,
            int numberOfBytesToRead,
            out int numberOfBytesRead,
            Overlapped2 flag);

        // since Overlapped is struct, null can't be passed instead, 
        // we must declare overload method if we will use this 

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool ReadFile(
            HandleRef hndRef,
            StringBuilder buffer,
            int numberOfBytesToRead,
            out int numberOfBytesRead,
            int flag);	// int instead of structure reference        

        [DllImport("ole32.dll")]
        internal static extern void ReleaseStgMedium(ref STGMEDIUM medium);

        // Clipboard formats used for cut/copy/drag operations
        public const string CFSTR_PREFERREDDROPEFFECT = "Preferred DropEffect";
        public const string CFSTR_PERFORMEDDROPEFFECT = "Performed DropEffect";
        public const string CFSTR_FILEDESCRIPTORW = "FileGroupDescriptorW";
        public const string CFSTR_FILECONTENTS = "FileContents";
        public const string CFSTR_SHELLIDLIST = "Shell IDList Array";

        // File Descriptor Flags
        public const Int32 FD_CLSID = 0x00000001;
        public const Int32 FD_SIZEPOINT = 0x00000002;
        public const Int32 FD_ATTRIBUTES = 0x00000004;
        public const Int32 FD_CREATETIME = 0x00000008;
        public const Int32 FD_ACCESSTIME = 0x00000010;
        public const Int32 FD_WRITESTIME = 0x00000020;
        public const Int32 FD_FILESIZE = 0x00000040;
        public const Int32 FD_PROGRESSUI = 0x00004000;
        public const Int32 FD_LINKUI = 0x00008000;

        // Global Memory Flags
        public const Int32 GMEM_MOVEABLE = 0x0002;
        public const Int32 GMEM_ZEROINIT = 0x0040;
        public const Int32 GHND = (GMEM_MOVEABLE | GMEM_ZEROINIT);
        public const Int32 GMEM_DDESHARE = 0x2000;

        // IDataObject constants
        public const Int32 DV_E_TYMED = unchecked((Int32)0x80040069);

        public const Int32 CF_HDROP = 15;
    }
}
