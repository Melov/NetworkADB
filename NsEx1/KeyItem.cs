using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using WindowsShell.Nspace;
using Microsoft.Win32;

namespace NsEx1
{
	public class KeyItem : DefaultFolderObject
	{
        internal static readonly Column ColName = new Column("Name", ColumnFormat.Left, typeof(string), new Guid("B725F130-47EF-101A-A5F1-02608C9EEBAC"), 10, 30, true, false);
		internal static readonly Column ColType = new Column("Type", ColumnFormat.Left, 10);
		internal static readonly Column ColValue = new Column("Value", ColumnFormat.Left, 20);
        /*
//        internal static readonly Column ColName = new Column("Name", ColumnFormat.Left, typeof(string), new Guid("B725F130-47EF-101A-A5F1-02608C9EEBAC"), 10, 30, true, false);

        internal static readonly Column ColDet1 = new Column("1111", ColumnFormat.Left, typeof(string), new Guid("C9944A21-A406-48FE-8225-AEC7E24C211B"), 500, 30, true, false);
        internal static readonly Column ColDet2 = new Column("1111", ColumnFormat.Left, typeof(string), new Guid("C9944A21-A406-48FE-8225-AEC7E24C211B"), 13, 30, true, false);

        internal static readonly Column ColDet3 = new Column("1111", ColumnFormat.Left, typeof(string), new Guid("b725f130-47ef-101a-a5f1-02608c9eebac"), 14, 30, true, false);
        internal static readonly Column ColDet4 = new Column("1111", ColumnFormat.Left, typeof(string), new Guid("b725f130-47ef-101a-a5f1-02608c9eebac"), 13, 30, true, false);
        */
		internal static readonly ColumnCollection columns;

		static KeyItem()
		{
			columns = new ColumnCollection();
			columns.Add(ColName);
			columns.Add(ColType);
			columns.Add(ColValue);
            //columns.Add(ColDet);
            /*
            columns.Add(ColDet1);
            columns.Add(ColDet2);
            columns.Add(ColDet3);
            columns.Add(ColDet4);
            */
			columns.DefaultDisplayColumn = ColName;
			columns.DefaultSortColumn = ColName;
		}

		internal static int Compare(object a, object b)
		{
			if (a is KeyItem && !(b is KeyItem))
			{
				return -1;
			}
			else if (b is KeyItem && !(a is KeyItem))
			{
				return 1;
			}
			else
			{
				return a.ToString().CompareTo(b.ToString());
			}
		}

		private RegistryKey root;
		private string path;
		private FolderAttributes attrs = FolderAttributes.None;
		private IList items = null;

        public KeyItem(byte[][] pathData, IdList idlist, RegistryKey root, string path)
            : this(pathData, idlist, root, path, FolderAttributes.None)
		{
		}

		protected KeyItem(byte[][] pathData, IdList idlist, RegistryKey root, string path, FolderAttributes attrs)
		{
			if (root == null)
			{
				throw new ArgumentNullException("root");
			}

			this.root = root;
			this.path = path;
			this.attrs = attrs;

			base.SetPath(pathData);
            base.SetPath(string.Format("{0}\\{1}", root.Name, path));
		    base.SetIdList(idlist);
		}

		public override FolderAttributes Attributes
		{
			get
			{
				if (attrs == FolderAttributes.None)
				{
					try
					{
						using (RegistryKey subkey = OpenKey())
						{
                            attrs = FolderAttributes.Folder | FolderAttributes.Browsable | FolderAttributes.CanLink | FolderAttributes.CanCopy | FolderAttributes.DropTarget;

							if (subkey.SubKeyCount > 0)
							{
								attrs |= FolderAttributes.HasSubFolder;
							}
						}
					}
					catch
					{
						attrs = FolderAttributes.Ghosted;
					}
				}

				return attrs;
			}
		}

		public override ColumnCollection Columns
		{
			get
			{
				return columns;
			}
		}

		public override object GetColumnValue(Column column)
		{
			if (column == ColName)
			{
				return path == null
					? root.Name
					: path.Substring(path.LastIndexOf('\\')+1);
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
                return string.Format("{0}\\{1}",root.Name, path);
		    }
			return path == null
				? root.Name
				: path.Substring(path.LastIndexOf('\\')+1);
		}

		public override ShellIcon GetIcon(bool open)
		{
			return ShellIcon.CreateFromFile(@"C:\Windows\System32\Shell32.dll", (int) (open ? Shell32Icon.OpenFolder : Shell32Icon.Folder), true, false);
		}

		public override IEnumerable GetItems(IWin32Window owner)
		{
			if (items == null)
			{
				items = new ArrayList();

				try
				{
					using (RegistryKey key = OpenKey())
					{
						foreach (string subkey in key.GetSubKeyNames())
						{
							items.Add(new KeyItem(PathData, idListAbsolute, root, path == null ? subkey : string.Format(@"{0}\{1}", path, subkey)));
						}

						foreach (string valueName in key.GetValueNames())
						{
                            items.Add(new ValueItem(PathData, root, path, valueName));
						}
					}
				}
				catch
				{
				}
			}

			return items;
		}

		public override ShellMenuItem[] MenuItems
		{
			get
			{
				return new ShellMenuItem[]
				{
					CreateExploreMenuItem(),
					CreateOpenMenuItem()
				};
			}
		}


		public override byte[] Persist()
		{
			if (path != null)
			{
				MemoryStream ms = new MemoryStream();
				BinaryWriter w = new BinaryWriter(ms, Encoding.Default);
				w.Write(0);
				Debug.Assert(path != null);
				w.Write(path);
				w.Write((int) attrs);
				return ms.ToArray();
			}
			else
			{
				return Encoding.Default.GetBytes(root.Name);
			}
		}

        public override IFolderObject Restore(byte[] data, bool forName = false)
		{
			MemoryStream ms = new MemoryStream(data);
			BinaryReader r = new BinaryReader(ms, Encoding.Default);
			
			if (r.ReadInt32() == 0)	// key item
			{
				string path = r.ReadString();
                //Debug.WriteLine("KEY: "+path);
				FolderAttributes attrs = (FolderAttributes) r.ReadInt32();
				return new KeyItem(PathData,idListAbsolute, root, path, attrs);
			}
			else					// value item
			{
			    string path = null;
			    string valueName = string.Empty;
			    try
			    {
                   // Debug.WriteLine("Value: " + data);
                    if (r.BaseStream.Position < r.BaseStream.Length)
                        path = r.ReadBoolean() ? r.ReadString() : null;
                    if (r.BaseStream.Position < r.BaseStream.Length)
				        valueName = r.ReadString();
			    }
			    catch (Exception)
			    {
			        
			    }
                return new ValueItem(PathData, root, path, valueName);
			}
		}

		public override string ToString()
		{
			return GetDisplayName(NameOptions.Normal);
		}

		private RegistryKey OpenKey()
		{
            //Debug.WriteLine("OpenKey: " + path);
			return path == null ? root : root.OpenSubKey(path);
		}
	}
}
