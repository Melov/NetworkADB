using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using iop = System.Runtime.InteropServices;

namespace WindowsShell.Nspace.Link
{
    [ComImport]
    [Guid("00021401-0000-0000-C000-000000000046")]
    internal class ShellLink
    {
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("000214F9-0000-0000-C000-000000000046")]
    internal interface IShellLink
    {
        void GetPath([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, out IntPtr pfd, int fFlags);
        void GetIDList(out IntPtr ppidl);
        void SetIDList(IntPtr pidl);
        void GetDescription([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cchMaxName);
        void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
        void GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);
        void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
        void GetArguments([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);
        void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
        void GetHotkey(out short pwHotkey);
        void SetHotkey(short wHotkey);
        void GetShowCmd(out int piShowCmd);
        void SetShowCmd(int iShowCmd);
        void GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, int cchIconPath, out int piIcon);
        void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
        void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);
        void Resolve(IntPtr hwnd, int fFlags);
        void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("0000010c-0000-0000-C000-000000000046")]
    public interface IPersist
    {
        void GetClassID(out Guid pClassID);
    };

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("00000109-0000-0000-C000-000000000046")]
    public interface IPersistStream : IPersist
    {
        new void GetClassID(out Guid pClassID);

        [PreserveSig]
        int IsDirty();
        void Load([In] IStream pStm);
        void Save([In] IStream pStm, [In, MarshalAs(UnmanagedType.Bool)] bool fClearDirty);
        void GetSizeMax(out long pcbSize);
    };

    public class ComStreamWrapper : System.IO.Stream
    {
        private IStream mSource;
        private IntPtr mInt64;

        public ComStreamWrapper()
        {
            
        }
        public ComStreamWrapper(IStream source)
        {
            mSource = source;
            mInt64 = iop.Marshal.AllocCoTaskMem(8);
        }

        ~ComStreamWrapper()
        {
            iop.Marshal.FreeCoTaskMem(mInt64);
        }

        public override bool CanRead { get { return true; } }
        public override bool CanSeek { get { return true; } }
        public override bool CanWrite { get { return true; } }

        public override void Flush()
        {
            mSource.Commit(0);
        }

        public override long Length
        {
            get
            {
                System.Runtime.InteropServices.ComTypes.STATSTG stat;
                mSource.Stat(out stat, 1);
                return stat.cbSize;
            }
        }

        public override long Position
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (offset != 0) throw new NotImplementedException();
            mSource.Read(buffer, count, mInt64);
            return iop.Marshal.ReadInt32(mInt64);
        }

        public override long Seek(long offset, System.IO.SeekOrigin origin)
        {
            mSource.Seek(offset, (int)origin, mInt64);
            return iop.Marshal.ReadInt64(mInt64);
        }

        public override void SetLength(long value)
        {
            mSource.SetSize(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (offset != 0) throw new NotImplementedException();
            mSource.Write(buffer, count, IntPtr.Zero);
        }
    }

    
    public class IStreamWrapper : Stream
    {
        IStream stream;

        public IStreamWrapper(IStream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            this.stream = stream;
        }

        ~IStreamWrapper()
        {
            Close();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (offset != 0)
                throw new NotSupportedException("only 0 offset is supported");
            if (buffer.Length < count)
                throw new NotSupportedException("buffer is not large enough");

            IntPtr bytesRead = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)));
            try
            {
                stream.Read(buffer, count, bytesRead);
                return Marshal.ReadInt32(bytesRead);
            }
            finally
            {
                Marshal.FreeCoTaskMem(bytesRead);
            }
        }


        public override void Write(byte[] buffer, int offset, int count)
        {
            if (offset != 0)
                throw new NotSupportedException("only 0 offset is supported");
            stream.Write(buffer, count, IntPtr.Zero);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            IntPtr address = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)));
            try
            {
                stream.Seek(offset, (int)origin, address);
                return Marshal.ReadInt32(address);
            }
            finally
            {
                Marshal.FreeCoTaskMem(address);
            }
        }


        public override long Length
        {
            get
            {
                System.Runtime.InteropServices.ComTypes.STATSTG statstg;
                stream.Stat(out statstg, 1 /* STATSFLAG_NONAME*/ );
                return statstg.cbSize;
            }
        }

        public override long Position
        {
            get { return Seek(0, SeekOrigin.Current); }
            set { Seek(value, SeekOrigin.Begin); }
        }


        public override void SetLength(long value)
        {
            stream.SetSize(value);
        }

        public override void Close()
        {
            stream.Commit(0);
            // Marshal.ReleaseComObject(stream);
            stream = null;
            GC.SuppressFinalize(this);
        }

        public override void Flush()
        {
            stream.Commit(0);
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }
    }

    public class LinkMayker
    {
        public byte[] MaleLink(string description, IntPtr ppath, string path)
        {
            IShellLink link = (IShellLink)new ShellLink();
            //link.SetDescription(description);
            //link.SetPath("Компьютер\\Registry3\\"+path);
            //link.SetWorkingDirectory("");
            //link.SetRelativePath("Компьютер\\Registry3\\" + path,0);
            link.SetIDList(ppath);
            // save it
            IPersistFile file = (IPersistFile)link;
            //string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);         
            string sftmp = Path.GetTempFileName();
            file.Save(sftmp, false);
            byte[] br = File.ReadAllBytes(sftmp);
            File.Delete(sftmp);
            return br;
        }
        public void MakeLink(string description, IntPtr ppath, string path)
        {
            IShellLink link = (IShellLink)new ShellLink();          
            link.SetIDList(ppath);            
            IPersistFile file = (IPersistFile)link;
            file.Save(path, false);          
        } 
    }
}
