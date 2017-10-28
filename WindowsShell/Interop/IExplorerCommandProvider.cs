using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace WindowsShell.Interop
{
    [ComImport,
     Guid("64961751-0835-43c0-8ffe-d57686530e64"),
     InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
     CLSCompliant(false)]
    public interface IExplorerCommandProvider
    {/*
        [PreserveSig]
        int  GetCommands ([in] IUnknown *punkSite,[in] REFIID riid,[out, iid_is (riid)] void **ppv);
  [PreserveSig]
        int  GetCommand ([in] REFGUID rguidCommandId,[in] REFIID riid,[out, iid_is (riid)] void **ppv);        
      */
    }
}
