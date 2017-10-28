using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Wsi = WindowsShell.Interop;
using WindowsShell.Nspace;
using Microsoft.Win32;

namespace NsEx2
{
	public class MyItem : DefaultFolderObject
	{
        internal static readonly Column ColName = new Column("Name", ColumnFormat.Left, 20);

		private readonly int n;

		private IntPtr hfdgs;
		private IntPtr pStream;

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

        public override object GetColumnValue(Column column)
        {
            if (column == ColName)
            {
                return n.ToString();
            }
            else
            {
                return null;
            }
        }

		public MyItem(byte[][] pathData, int n)
		{
            columns = new ColumnCollection();
            columns.Add(ColName);
			this.n = n;
		    base.SetPath(n.ToString());
            base.SetPath(pathData);
		}

		public override ShellMenuItem[] MenuItems
		{
			get
			{
				return new ShellMenuItem[]
				{
					new ShellMenuItem("Copy", "copy", new EventHandler(Copy))
				};
			}
		}

		private Bitmap GetBitmap()
		{
			IntPtr hIcon = (IntPtr) Root.ExtractIcon(Process.GetCurrentProcess().Handle, @"C:\Windows\System32\Shell32.dll", n);
			Icon icon = Icon.FromHandle(hIcon);
			Bitmap bitmap = new Bitmap(icon.Width, icon.Height, PixelFormat.Format32bppArgb);

			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				graphics.DrawIcon(icon, 0, 0);
			}

			return bitmap;
		}

		public void Copy(object sender, EventArgs args)
		{
			Clipboard.SetDataObject(GetBitmap());
		}

		public override string GetDisplayName(NameOptions opts)
		{
			return n.ToString();
		}

		public override ShellIcon GetIcon(bool open)
		{
			return ShellIcon.CreateFromFile(@"C:\Windows\System32\Shell32.dll", n, true, false);
		}

		public override byte[] Persist()
		{
			return Encoding.Default.GetBytes(n.ToString());
		}

        public override IFolderObject Restore(byte[] data, bool forName = false)
		{
			throw new NotImplementedException();
		}
	}
}
