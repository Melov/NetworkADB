using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsShell.Interop;

namespace WindowsShell.Nspace
{
    public abstract class ShellNamespaceFolderView
    {
        internal abstract IShellView CreateShellView(IShellFolder folder);
    }
}
