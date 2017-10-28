using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using WindowsShell.Nspace;

namespace NsEx1
{
	public class ValueItem : DefaultFolderObject
	{
		internal RegistryKey root;
		private string path;
		private ShellMenuItem[] menuItems;
		private ShellMenuItem mnuCopyValue;
		private string valueName;
		private object val = null;
        private FolderAttributes attrs = FolderAttributes.None;

	    public override FolderAttributes Attributes
	    {
	        get
	        {
	            if (attrs == FolderAttributes.None)
	            {
	                try
	                {
                        attrs = FolderAttributes.Browsable | FolderAttributes.CanLink | FolderAttributes.CanCopy | FolderAttributes.DropTarget;
	                }
	                catch
	                {
	                    attrs = FolderAttributes.Ghosted;
	                }
	            }

	            return attrs;
	        }
	    }

	    public ValueItem(byte[][] pathData,RegistryKey root, string path, string valueName)
		{
			if (root == null)
			{
				throw new ArgumentNullException("root");
			}
			else if (valueName == null)
			{
				throw new ArgumentNullException("valueName");
			}

			this.root = root;
			this.path = path;            
			this.valueName = valueName;
            base.SetPath(pathData);
            base.SetPath(string.Format("{0}\\{1}\\{2}", root.Name, path, valueName));
		}

		public override byte[] Persist()
		{
			MemoryStream ms = new MemoryStream();
			BinaryWriter w = new BinaryWriter(ms, Encoding.Default);
			w.Write(1);
			w.Write(path != null);

			if (path != null)
			{
				w.Write(path);
			}

			w.Write(valueName);

			return ms.ToArray();
		}

		public override object GetColumnValue(Column column)
		{
			if (column == KeyItem.ColName)
			{
				return valueName;
			}
			else if (column == KeyItem.ColType)
			{
				using (RegistryKey key = OpenKey())
				{
					object @value = key.GetValue(valueName);
					
					if (@value == null)
					{
						return null;
					}
					else
					{
						return @value.GetType().Name;
					}
				}
			}
			else if (column == KeyItem.ColValue)
			{
				using (RegistryKey key = OpenKey())
				{
					object @value = key.GetValue(valueName);
					return value;
				}
			}
			else
			{
				return null;
			}
		}

		public override string GetDisplayName(NameOptions opts)
		{
            if (opts == (NameOptions.ForAddressBar | NameOptions.ForParsing))
            {
                return string.Format("{0}\\{1}\\{2}", root.Name, path, valueName);
            }
			return valueName;
		}

		public override ShellIcon GetIcon(bool open)
		{
			if (val == null)
			{
				using (RegistryKey key = OpenKey())
				{
					val = key.GetValue(valueName);
				}
			}

			return ShellIcon.CreateFromFile(
				@"C:\Windows\Regedit.exe",
				val is string ? -205 : -206,
				true,
				false);
		}

		public override ShellMenuItem[] MenuItems
		{
			get
			{
				if (menuItems == null)
				{
					mnuCopyValue = new ShellMenuItem("Copy Value", "copyvalue", new EventHandler(CopyValue_Click));
					mnuCopyValue.HelpText = "Copies the registry key value to the system clipboard";

					menuItems = new ShellMenuItem[] { mnuCopyValue };
				}

				return menuItems;
			}
		}

        public override IFolderObject Restore(byte[] data, bool forName = false)
		{
			throw new NotImplementedException(); // no children
		}

		public override string ToString()
		{
			return GetDisplayName(NameOptions.Normal);
		}

		public override void SetName(IWin32Window owner, string name, NameOptions opts)
		{
			throw new NotSupportedException();
		}

		private RegistryKey OpenKey()
		{
			return path == null ? root : root.OpenSubKey(path);
		}

		private void CopyValue_Click(object sender, EventArgs e)
		{
			using (RegistryKey key = OpenKey())
			{
				Clipboard.SetDataObject(key.GetValue(valueName));
			}
		}
	}
}
