using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using WindowsShell.Nspace;

namespace NsEx2
{
	public class Root : DefaultFolderObject
	{
		[DllImport("shell32.dll", SetLastError=true)]
		internal static extern int ExtractIcon(IntPtr hInst, string lpszExeFileName, int nIconIndex);
        internal static readonly Column ColName = new Column("Name", ColumnFormat.Left, 20);
		public Root()
		{
            columns = new ColumnCollection();
            columns.Add(ColName);
		}

        internal readonly ColumnCollection columns;

        public override ColumnCollection Columns
        {
            get
            {
                return columns;
            }
        }

		public override FolderAttributes Attributes
		{
            get
            {
                return FolderAttributes.CanRename | FolderAttributes.CanCopy | FolderAttributes.CanLink | FolderAttributes.Browsable | FolderAttributes.DropTarget;
            }
		}

		public override string GetDisplayName(NameOptions opts)
		{
			throw new NotImplementedException();
		}

		public override IEnumerable GetItems(IWin32Window owner)
		{
			IList items = new ArrayList();
			int nIcons = Root.ExtractIcon(Process.GetCurrentProcess().Handle, @"C:\Windows\System32\Shell32.dll", -1);
			int result = Marshal.GetLastWin32Error();

			for (int i = 0; i < nIcons; i++)
			{
				items.Add(new MyItem(PathData, i));
			}

			return items;
		}

		public override byte[] Persist()
		{
			throw new NotImplementedException();
		}

        public override IFolderObject Restore(byte[] data, bool forName = false)
		{
			return new MyItem(PathData, int.Parse(Encoding.Default.GetString(data)));
		}
	}
}
