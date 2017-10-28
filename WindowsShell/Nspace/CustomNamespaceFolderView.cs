using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsShell.Interop;

namespace WindowsShell.Nspace
{
    public sealed class CustomNamespaceFolderView : ShellNamespaceFolderView
    {
        public CustomNamespaceFolderView(UserControl customView)
        {
            this.customView = customView;
        }

        private readonly UserControl customView;


        internal override IShellView CreateShellView(IShellFolder folder)
        {
            return new ShellViewHost(customView);
        }
    }
}
