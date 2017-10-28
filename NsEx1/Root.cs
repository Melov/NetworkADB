using System;
using System.Collections;

using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using WindowsShell.Nspace;

namespace NsEx1
{
	public class Root : DefaultFolderObject
	{
		public Root()
		{
		}

		public override FolderAttributes Attributes
		{
			get
			{
                return FolderAttributes.Folder | FolderAttributes.HasSubFolder | FolderAttributes.HasPropSheet | FolderAttributes.CanMove | FolderAttributes.CanDelete | FolderAttributes.CanRename | FolderAttributes.CanCopy | FolderAttributes.CanLink | FolderAttributes.Browsable | FolderAttributes.DropTarget;
			}
		}

		public override ColumnCollection Columns
		{
			get
			{
				return KeyItem.columns;
			}
		}


		public override string GetDisplayName(NameOptions opts)
		{
			throw new NotImplementedException();
		}

		public override IEnumerable GetItems(IWin32Window owner)
		{
			IList items = new ArrayList();
			items.Add(new KeyItem(PathData, idListAbsolute, Registry.ClassesRoot, null));
            items.Add(new KeyItem(PathData, idListAbsolute, Registry.CurrentConfig, null));
            items.Add(new KeyItem(PathData, idListAbsolute, Registry.CurrentUser, null));
            items.Add(new KeyItem(PathData, idListAbsolute, Registry.LocalMachine, null));
            items.Add(new KeyItem(PathData, idListAbsolute, Registry.Users, null));
			return items;
		}

		public override byte[] Persist()
		{
			throw new NotImplementedException();
		}

        public override IFolderObject Restore(byte[] data, bool forName = false)
		{
			string s = Encoding.Default.GetString(data);

			switch (s)
			{
				case "HKEY_CLASSES_ROOT":
                    return new KeyItem(PathData, idListAbsolute, Registry.ClassesRoot, null);

				case "HKEY_CURRENT_CONFIG":
                    return new KeyItem(PathData, idListAbsolute, Registry.CurrentConfig, null);

				case "HKEY_CURRENT_USER":
                    return new KeyItem(PathData, idListAbsolute, Registry.CurrentUser, null);

				case "HKEY_LOCAL_MACHINE":
                    return new KeyItem(PathData, idListAbsolute, Registry.LocalMachine, null);

				case "HKEY_USERS":
                    return new KeyItem(PathData, idListAbsolute, Registry.Users, null);

				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}
