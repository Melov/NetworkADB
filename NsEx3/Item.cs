using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using WindowsShell.Interop;
using WindowsShell.Nspace;
using WindowsShell.Nspace.Icon;
using Microsoft.Win32;

namespace NsEx3
{
    public class Item : DefaultFolderObject
	{
		//private int n;
		private string name;
        private string path;
        private string root;
        private IFileObject fo = new FileObject();
        private IFolderObject parrent;

        internal static readonly Column ColName = new Column("Name", ColumnFormat.Left, typeof(string), new Guid("B725F130-47EF-101A-A5F1-02608C9EEBAC"), 10, 30, true, false);

        internal static readonly Column ColAttr = new Column("Attributes", ColumnFormat.Left, typeof(string), Guid.Empty, 1, 10, true, false);
        internal static readonly Column ColPerm1 = new Column("Owner", ColumnFormat.Left, typeof(string), Guid.Empty, 2, 10, true, false);
        internal static readonly Column ColPerm2 = new Column("Group", ColumnFormat.Left, typeof(string), Guid.Empty, 3, 10, true, false);
        internal static readonly Column ColFileSize = new Column("Size", ColumnFormat.Left, typeof(string), Guid.Empty, 4, 10, true, false);
        internal static readonly Column ColDate = new Column("Date", ColumnFormat.Left, typeof(string), Guid.Empty, 5, 10, true, false);
        internal static readonly Column ColTime = new Column("Time", ColumnFormat.Left, typeof(string), Guid.Empty, 6, 10, true, false);
        internal static readonly Column ColLinkTo = new Column("Link To", ColumnFormat.Left, typeof(string), Guid.Empty, 7, 30, true, false);

        public IFolderObject GetParrent()
        {
            return parrent;
        }

        public Item(byte[][] pathData, string root, string path, string name, IFileObject fo, IFolderObject parrent, IShellView sv)
        {
            base.ShellView = sv;
            this.parrent = parrent;
            columns = new ColumnCollection();
            columns.Add(ColName);
            columns.Add(ColAttr);
            columns.Add(ColPerm1);
            columns.Add(ColPerm2);
            columns.Add(ColFileSize);
            columns.Add(ColDate);
            columns.Add(ColTime);
            columns.Add(ColLinkTo);
           
            this.root = root;
            this.path = path;
            this.name = name;
            this.fo = fo;
            base.SetPath(pathData);
            if (string.IsNullOrEmpty(root))
            {
                if (string.IsNullOrEmpty(path))
                {
                    base.SetPath(string.Format("{0}", name));
                }
                else
                {
                    base.SetPath(string.Format("{0}{1}{2}", path, FD, name));   
                }                
            }
            else
            {
                base.SetPath(string.Format("{0}{1}{2}{3}{4}", root, FD,path,FD, name));                    
            }
            
		}

        internal readonly ColumnCollection columns;

        public override ColumnCollection Columns
        {
            get
            {
                return columns;
            }
        }

        public override void SetColumnValue(Column column, object value)
        {            
            if (column.Name == ColName.Name)
            {
                name = value.ToString();
            }            
            else 
            if (column.Name == ColAttr.Name)
            {
                fo.Attr = value.ToString();
            }
            else if (column.Name == ColPerm1.Name)
            {
                fo.Perm1 = value.ToString(); ;
            }
            else if (column.Name == ColPerm2.Name)
            {
                fo.Perm2 = value.ToString();
            }
            else if (column.Name == ColFileSize.Name)
            {
                fo.Size = value.ToString();
            }
            else if (column.Name == ColDate.Name)
            {
                fo.Date = value.ToString();
            }
            else if (column.Name == ColTime.Name)
            {
                fo.Time = value.ToString();
            }
            else if (column.Name == ColLinkTo.Name)
            {
                fo.Link = value.ToString();
            }
        }

        public override object GetColumnValue(Column column)
        {
            if (column == null)
                return null;

            if (column.Name == ColName.Name)
            {
                return name;
            }
            else if (column.Name == ColAttr.Name)
            {
                return fo.Attr;
            }
            else if (column.Name == ColPerm1.Name)
            {
                return fo.Perm1;
            }
            else if (column.Name == ColPerm2.Name)
            {
                return fo.Perm2;
            }
            else if (column.Name == ColFileSize.Name)
            {
                return fo.Size;
            }
            else if (column.Name == ColDate.Name)
            {
                return fo.Date;
            }
            else if (column.Name == ColTime.Name)
            {
                return fo.Time;
            }
            else if (column.Name == ColLinkTo.Name)
            {
                return fo.Link;
            }
            else
            {
                return null;
            }
        }

        public override ShellMenuItem[] MenuItems
        {
            get
            {
                ShellMenuItem emCopy = CreateCopyMenuItem(new IFolderObject[] { this });
                emCopy.Bitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject("copy");

                ShellMenuItem emDelete = CreateDeleteMenuItem(new IFolderObject[] { this });
                emDelete.Bitmap = ((Icon)Properties.Resources.ResourceManager.GetObject("delete")).ToBitmap();

                ShellMenuItem emRename = CreateRenameMenuItem();
                emRename.Bitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject("rename");

                ShellMenuItem emSep = CreateExploreMenuItem("", "", "");
                emSep.Separator = true;

                ShellMenuItem emInfo = CreateInfoMenuItem(new IFolderObject[] { this });
                emInfo.Bitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject("info");

                return new ShellMenuItem[]
				{					
					emCopy,
                    emDelete,
                    emRename,
                    emSep,
                    emInfo
				};
            }
        }

		public override FolderAttributes Attributes
		{
			get
			{
                FolderAttributes fa = FolderAttributes.HasPropSheet | FolderAttributes.CanMove | FolderAttributes.CanDelete | FolderAttributes.CanRename | FolderAttributes.CanCopy | FolderAttributes.CanLink | FolderAttributes.Browsable | FolderAttributes.DropTarget /*| FolderAttributes.FileSystem*/;
                if (fo.IsLink)
                    fa |= FolderAttributes.Link;
			    return fa;
			}
		}

		public override string GetDisplayName(NameOptions opts)
		{
            if (opts == (NameOptions.ForAddressBar | NameOptions.ForParsing))
            {
                return string.Format("{0}{1}{2}{3}{4}", root, "\\", path, "\\", name);
            }
            return name;
		}

        public override ShellIcon GetIcon(bool open)
        {
            string ext = IconHelper.GetExtension(name);
            
            //Icon ic = IconHelper.GetFileIcon(ext, User32.IconSize.Small, false);
            //IntPtr hIcon = IconHelper.GetXLIcon(IconHelper.GetIconIndex(ext));
            //Icon ic = (Icon)Icon.FromHandle(hIcon).Clone();
            //User32.DestroyIcon(hIcon); // don't forget to cleanup
            return ShellIcon.CreateFromIcon(null, false, false, ext);
        }

		public override byte[] Persist()
		{
		    using (MemoryStream ms = new MemoryStream())
		    {
                BinaryWriter w = new BinaryWriter(ms, Encoding.Default);
                w.Write(1);

                byte[] btFo = FileObject.ToByteArray(fo);
                w.Write((int)btFo.Length);
                w.Write(btFo, 0, btFo.Length);

                w.Write(path != null);

                if (path != null)
                {
                    w.Write(path);
                }

                w.Write(name);

                return ms.ToArray();    
		    }
            
            /*
            SerializeData sd = new SerializeData();
            sd.bIsFolder = false;
            sd.pathData = PathData;
            sd.n = n;
            sd.rootName = PathString;
            sd.name = name;
            return Folder.ObjectToByteArray(sd);
          */
           // return BitConverter.GetBytes(n);
		}

        public override IFolderObject Restore(byte[] data, bool forName = false)
		{
			throw new NotImplementedException();
		}

		public override void SetName(IWin32Window owner, string name, NameOptions opts)
		{
			this.name = name;
            string sNewFile = name;
		    if (!string.IsNullOrEmpty(path))
		    {
		        sNewFile = string.Format("{0}{1}{2}", path,FD, name);
		    }
            SetFileName(this, sNewFile);
		}

        public override void RefreshItems(IFolderObject[] items)
        {
            Shell32.SHChangeNotify(ShellChangeEvents.UpdateDir,
                ShellChangeFlags.IdList | ShellChangeFlags.Flush,
                ItemIdList.Create(null, PathData).Ptr,
                IntPtr.Zero);
        }

        public override void DeleteItems(IFolderObject[] items)
        {
            List<string> lsFiles = new List<string>();
            List<string> lsDirs = new List<string>();
            foreach (IFolderObject folderObject in items)
            {
                string spath = string.Format("{0}{1}{2}", GetBaseFolder(), FD, folderObject.PathString);
                if ((folderObject.Attributes & FolderAttributes.Folder) == FolderAttributes.Folder)
                {
                    lsDirs.Add(spath);
                }
                else
                {
                    lsFiles.Add(spath);
                }
            }

            DeleteItems(lsDirs, lsFiles);

            Shell32.SHChangeNotify(ShellChangeEvents.UpdateDir,
                ShellChangeFlags.IdList | ShellChangeFlags.Flush,
                ItemIdList.Create(null, PathData).Ptr,
                IntPtr.Zero);
        }
	}
}
