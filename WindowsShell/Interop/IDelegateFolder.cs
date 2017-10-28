using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace WindowsShell.Interop
{
    //http://www.codeproject.com/Articles/1840/Namespace-Extensions-the-IDelegateFolder-mystery
    //http://read.pudn.com/downloads133/sourcecode/windows/network/565261/ftp%E6%BA%90%E7%A0%81/www.cnzz.cn/ftpfoldr.cpp__.htm
    //http://www.codeproject.com/Articles/3551/C-does-Shell-Part
    [ComImport,
     Guid("ADD8BA80-002B-11D0-8F0F-00C04FD7D062"),
     InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
     CLSCompliant(false)]
    public interface IDelegateFolder
    {
        [PreserveSig]
        int SetItemAlloc([In] ref IMalloc pmalloc);        
    }
}
