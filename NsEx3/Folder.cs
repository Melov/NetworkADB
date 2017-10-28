using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using WindowsShell.ADB;
using WindowsShell.Interop;
using WindowsShell.Nspace;
using WindowsShell.Nspace.Icon;
using Microsoft.Win32;

namespace NsEx3
{
    [Serializable]
    public class SerializeData
    {
        public bool bIsFolder;
        public byte[][] pathData;
        public int n;
        public string rootName;
        public string name;
    }

    public class Folder : DefaultFolderObject
	{
        private string path;
        private string root;
        private IList items;
        private FolderAttributes attrs = FolderAttributes.None;
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

        public static Dictionary<string, Dictionary<int, System.Drawing.Icon>> ItemsIcons = new Dictionary<string, Dictionary<int, Icon>>();

        public IFolderObject GetParrent()
        {
            return parrent;
        }

        public Folder(byte[][] pathData, IdList idlist, string root, string path, IFolderObject parrent)
            : this(pathData, idlist, root, path, FolderAttributes.None, new FileObject(),null, parrent)
		{
		}

        public Folder(byte[][] pathData, IdList idlist, string root, string path, FolderAttributes attrs, IFileObject fo, IShellView sv, IFolderObject parrent)
		{
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

            base.ShellView = sv;
            this.attrs = attrs;
            this.root = root;
            this.path = path;
            this.fo = fo;
            
            if (string.IsNullOrEmpty(root))
            {
                base.SetPath(string.Format("{0}", path));
            }
            else
            {
                base.SetPath(string.Format("{0}{1}{2}", root, FD, path));    
            }
            
            base.SetPath(pathData);
            base.SetIdList(idlist);
		}

        public override ShellIcon GetIcon(bool open)
        {
            //return ShellIcon.CreateFromFile(@"C:\Windows\System32\Shell32.dll", (int)(open ? Shell32Icon.OpenFolder : Shell32Icon.Folder), true, false);                     
            return ShellIcon.CreateFromIcon(null, false, false, "folder");
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
                if (path == null)
                {
                    root = value.ToString();
                }
                else
                {
                    path = path.Substring(0, path.LastIndexOf(FD[0]) + 1) + value.ToString();                
                }                
            }            
            else 
            if (column.Name == ColAttr.Name)
            {
                fo.Attr = value.ToString();
            }
            else if (column.Name == ColPerm1.Name)
            {
                fo.Perm1 = value.ToString();
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
                return path == null
                    ? root
                    : path.Substring(path.LastIndexOf(FD[0]) + 1);
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
                ShellMenuItem emSep = CreateExploreMenuItem("", "", "");
                emSep.Separator = true;

                ShellMenuItem emNewFolder = CreateNewFolderMenuItem();
                emNewFolder.Bitmap = ((Icon)Properties.Resources.ResourceManager.GetObject("folder")).ToBitmap();

                ShellMenuItem emPaste = CreatePasteMenuItem();
                emPaste.Bitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject("paste1");

                ShellMenuItem emCopy = CreateCopyMenuItem(new IFolderObject[] { this });
                emCopy.Bitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject("copy");

                ShellMenuItem emDelete = CreateDeleteMenuItem(new IFolderObject[] {this});
                emDelete.Bitmap = ((Icon)Properties.Resources.ResourceManager.GetObject("delete")).ToBitmap();

                ShellMenuItem emRename = CreateRenameMenuItem();
                emRename.Bitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject("rename");

                ShellMenuItem emInfo = CreateInfoMenuItem(new IFolderObject[] { this });
                emInfo.Bitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject("info");

                return new ShellMenuItem[]
				{	
				    CreateExploreMenuItem(),
					CreateOpenMenuItem(),
                    emSep,
					emCopy,
                    emPaste,
                    emDelete,
                    emRename,
                    emSep,
                    emNewFolder,
                    emSep,
                    emInfo                    
				};
            }
        }

        public override ShellMenuItem[] GetMenuItems(IFolderObject[] items)
        {
            ShellMenuItem emCopy = CreateCopyMenuItem(items);
            emCopy.Bitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject("copy");

            ShellMenuItem emDelete = CreateDeleteMenuItem(items);
            emDelete.Bitmap = ((Icon)Properties.Resources.ResourceManager.GetObject("delete")).ToBitmap();

            ShellMenuItem emSep = CreateExploreMenuItem("", "", "");
            emSep.Separator = true;

            ShellMenuItem emInfo = CreateInfoMenuItem(items);
            emInfo.Bitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject("info");

            return new ShellMenuItem[]
				{										
                    emDelete,
                    emCopy,
                    emSep,
                    emInfo
				};
        }

		public override FolderAttributes Attributes
		{
			get
			{
                FolderAttributes fa = FolderAttributes.HasSubFolder | FolderAttributes.Folder | FolderAttributes.HasPropSheet | FolderAttributes.CanMove | FolderAttributes.CanDelete | FolderAttributes.CanRename | FolderAttributes.CanCopy | FolderAttributes.CanLink | FolderAttributes.Browsable | FolderAttributes.DropTarget /*| FolderAttributes.FileSystem*/;
                if (fo!= null && fo.IsLink)
			        fa |= FolderAttributes.Link;
			    return fa;
			}
		}

		public override string GetDisplayName(NameOptions opts)
		{
            if (opts == (NameOptions.ForAddressBar | NameOptions.ForParsing))
            {
                if (string.IsNullOrEmpty(root))
                {
                    return string.Format("{0}", path.Replace(FD,"\\"));                    
                }
                else
                {
                    return string.Format("{0}{1}{2}", root,"\\", path);
                }
                
            }
            return path == null ? root : path.Substring(path.LastIndexOf(FD[0]) + 1);
		}

        public void SetEmptyText(string text)
        {
            if (this.ShellView != null)
            {
                IntPtr pdf;
                Guid gFV = NsExtension.IID_IFolderView2;
                IntPtr ppv = Marshal.GetComInterfaceForObject(this.ShellView, typeof(IShellView));
                Marshal.QueryInterface(ppv, ref gFV, out pdf);
                IFolderView2 pFV2 = (IFolderView2)Marshal.GetObjectForIUnknown(pdf);
                pFV2.SetText(0, text);
                Marshal.ReleaseComObject(pFV2);   
            }            
        }

        public override IEnumerable GetItems(IWin32Window owner)
        {
            if (items == null)
            {
                items = new ArrayList();
                int objId = 0;

                string error = string.Empty;
                IFileObject[] all = GetAllFromFolder(string.Format("{0}{1}{2}", GetBaseFolder(), FD, PathString), ref error);

                IconHelper.GetIcons(all, ref ItemsIcons);

                if (!string.IsNullOrEmpty(error))
                {
                    if (error.StartsWith("error: no devices/emulators found"))
                    {
                        SetEmptyText("Please click `Connect` in Context Menu");
                    }
                    else
                    {
                        SetEmptyText(error);
                    } SetEmptyText(error);
                }
                else
                {
                    SetEmptyText(string.Empty);
                }

                //FileObject[] sFolders = GetFoldersFromFolder(string.Format("{0}{1}{2}", GetBaseFolder(), FD, PathString));
                IFileObject[] sFolders = all.ToList().FindAll(x => x.IsFolder == true).ToArray();

                if (sFolders != null)
                {
                    for (int i = 0; i < sFolders.Length; i++)
                    {
                        Folder folder = new Folder(PathData, idListAbsolute, root, path == null ? sFolders[i].Name : string.Format("{0}{1}{2}", path, FD, sFolders[i].Name), FolderAttributes.Folder, sFolders[i],ShellView,this);

                        if (!ItemsIcons.ContainsKey("folder"))
                        {
                            var icons = IconHelper.GetImagesByExt("folder");
                            ItemsIcons.Add("folder", icons);
                        }
                        folder.Icons = ItemsIcons["folder"];   

                        items.Add(folder);                        
                        objId++;
                    }       
                }

                //FileObject[] sFiles = GetFilesFromFolder(string.Format("{0}{1}{2}", GetBaseFolder(), FD, PathString));
                IFileObject[] sFiles = all.ToList().FindAll(x => x.IsFolder == false).ToArray();
                if (sFiles != null)
                {
                    for (int i = 0; i < sFiles.Length; i++)
                    {
                        Item it = new Item(PathData, root, path, sFiles[i].Name, sFiles[i], this, base.ShellView);
                        ////////////ICONS
                        string ext = IconHelper.GetExtension(sFiles[i].Name);
                        if (string.IsNullOrEmpty(ext) && !ItemsIcons.ContainsKey("no"))
                        {
                            var icons = IconHelper.GetImagesByExt(ext);
                            ItemsIcons.Add("no", icons);
                        }
                        if (string.IsNullOrEmpty(ext) && ItemsIcons.ContainsKey("no"))
                        {
                            it.Icons = ItemsIcons["no"];
                        }

                        if (!string.IsNullOrEmpty(ext) && !ItemsIcons.ContainsKey(ext))
                        {
                            var icons = IconHelper.GetImagesByExt(ext);
                            ItemsIcons.Add(ext, icons);
                        }
                        if (!string.IsNullOrEmpty(ext) && ItemsIcons.ContainsKey(ext))
                        {
                            it.Icons = ItemsIcons[ext];
                        }
                        ////////////ICONS
                        items.Add(it);
                        objId++;
                    }                   
                }                
            }            
            return items;
        }

		public override byte[] Persist()
		{
            if (path != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryWriter w = new BinaryWriter(ms, Encoding.Default);
                    w.Write(0);
                    w.Write(path);
                    w.Write((int)attrs);
                    byte[] btFo = FileObject.ToByteArray(fo);
                    w.Write((int)btFo.Length);
                    w.Write(btFo, 0, btFo.Length);
                    return ms.ToArray();   
                }                
            }
            else
            {
                return Encoding.Default.GetBytes(root);
            }
            /*
            MemoryStream ms = new MemoryStream();
            BinaryWriter w = new BinaryWriter(ms, Encoding.Default);
            w.Write(0);
            w.Write(PathString);            
            return ms.ToArray();

            SerializeData sd = new SerializeData();
		    sd.bIsFolder = true;
            sd.pathData = PathData;
            sd.n = n;
            sd.rootName = PathString;
            sd.name = name;
            return ObjectToByteArray(sd);
          */
            //return BitConverter.GetBytes(n);
		}

        public static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            {
                bf.Serialize(ms, obj);
                return (byte[])ms.ToArray().Clone();
            }
        }
        private object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            object obj = (object)binForm.Deserialize(memStream);

            return obj;
        }

        public override IFolderObject Restore(byte[] data, bool forName = false)
		{
		    using (MemoryStream ms = new MemoryStream(data))
		    {
                BinaryReader r = new BinaryReader(ms, Encoding.Default);

                if (r.ReadInt32() == 0)	// key item
                {
                    string path = r.ReadString();
                    //Debug.WriteLine("KEY: "+path);
                    FolderAttributes attrs = (FolderAttributes)r.ReadInt32();

                    int nDataLen = r.ReadInt32();
                    byte[] btFo = r.ReadBytes(nDataLen);
                    FileObject fo = FileObject.ToObject(btFo);

                    Folder folder = new Folder(PathData, idListAbsolute, root, path, attrs, fo,ShellView,this);
                    if (ItemsIcons.ContainsKey("folder"))
                    {
                        folder.Icons = ItemsIcons["folder"];
                    } 
                    return folder;
                }
                else					// value item
                {

                    int nDataLen = r.ReadInt32();
                    byte[] btFo = r.ReadBytes(nDataLen);
                    FileObject fo = FileObject.ToObject(btFo);

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

                    Item it = new Item(PathData, root, path, valueName, fo, this, base.ShellView);
                    string ext = IconHelper.GetExtension(valueName);
                    if (string.IsNullOrEmpty(ext) && ItemsIcons.ContainsKey("no"))
                    {
                        it.Icons = ItemsIcons["no"];
                    } 
                    if (!string.IsNullOrEmpty(ext) && ItemsIcons.ContainsKey(ext))
                    {
                        it.Icons = ItemsIcons[ext];
                    } 
                    return it;
                }
		    }
            

            if (items == null)
                items = GetItems(null) as ArrayList;
            try
            {
                return (IFolderObject)items[BitConverter.ToInt32(data, 0)];
            }
            catch (Exception)
            {
                return this;
            }  
		}

		public override void SetName(IWin32Window owner, string name, NameOptions opts)
		{
		    if (path == null)
		    {
		        root = name;
		    }
		    else
		    {
                int nLastPos = path.LastIndexOf(FD[0]);
		        if (nLastPos < 0)
		        {
		            path = name;
		        }
		        else
		        {
                    string sP = path.Substring(0, nLastPos);
                    path = string.Format("{0}{1}{2}", sP,FD, name);    
		        }
		        
		    }

            SetFolderName(this, path);
		}
        
        public override void NewFolder()
        {
            string spath = CreateNewFolder(path, "New Folder");
            if (string.IsNullOrEmpty(spath))
            {
                return;
            }
            //refrash all views
            IntPtr pParrent = ItemIdList.Create(null, PathData).Ptr;
            Shell32.SHChangeNotify(ShellChangeEvents.UpdateDir,
                        ShellChangeFlags.IdList | ShellChangeFlags.Flush,
                        pParrent,
                        IntPtr.Zero);
            Marshal.FreeCoTaskMem(pParrent);
            //

            using (MemoryStream ms = new MemoryStream())
            {
                FolderAttributes newFolderAttrs = FolderAttributes.Folder | FolderAttributes.HasPropSheet | FolderAttributes.CanMove | FolderAttributes.CanDelete | FolderAttributes.CanRename | FolderAttributes.CanCopy | FolderAttributes.CanLink | FolderAttributes.Browsable | FolderAttributes.DropTarget;
                BinaryWriter w = new BinaryWriter(ms, Encoding.Default);
                w.Write(0);
                w.Write(string.Format("{0}{1}{2}", path, FD, spath));
                w.Write((int)newFolderAttrs);
                IFileObject foNew = new FileObject();
                foNew.Name = spath;
                foNew.IsFolder = true;
                byte[] btFo = FileObject.ToByteArray(foNew);
                w.Write((int)btFo.Length);
                w.Write(btFo, 0, btFo.Length);
                byte[] persist = ms.ToArray();

                IntPtr pNewFolder = ItemIdList.Create(null, new byte[][] { persist }).Ptr;
                if (ShellView != null)
                {
                    ShellView.SelectItem(pNewFolder, _SVSIF.SVSI_EDIT | _SVSIF.SVSI_FOCUSED | _SVSIF.SVSI_ENSUREVISIBLE);   
                }                
                Marshal.FreeCoTaskMem(pNewFolder);
            }

        }

        public override void RefreshItems(IFolderObject[] items)
        {
            string error = string.Empty;
            IFileObject[] all = GetAllFromFolder(string.Format("{0}{1}{2}", GetBaseFolder(), FD, this.parrent == null ? FD : this.parrent.PathString), ref error);
            

            /*
            IntPtr pDelObj1 = ItemIdList.Create(null, PathData).Ptr;
            Shell32.SHChangeNotify(ShellChangeEvents.Delete,
                ShellChangeFlags.IdList | ShellChangeFlags.Flush | ShellChangeFlags.NotifyRecursive,
                pDelObj1,
                IntPtr.Zero);
            Marshal.FreeCoTaskMem(pDelObj1);
            */
            
            foreach (IFolderObject folderObject in items)
            {
                IFileObject ifo = all.ToList().FirstOrDefault(x => x.Name.Equals(folderObject.GetDisplayName(NameOptions.Normal)));
                if (ifo != null)
                {                             
                   // IntPtr pDelObj2 = ItemIdList.Create(null, (byte[][])folderObject.PathData.Clone()).Ptr;

                    folderObject.SetColumnValue(folderObject.Columns[1], ifo.Attr);                    

                    folderObject.PathData[folderObject.PathData.Length - 1] = folderObject.Persist();
                    IntPtr pDelObj3 = ItemIdList.Create(null, (byte[][])folderObject.PathData.Clone()).Ptr;

                    Shell32.SHChangeNotify(ShellChangeEvents.Attributes,
                        ShellChangeFlags.IdList | ShellChangeFlags.Flush,
                        pDelObj3,
                        IntPtr.Zero);
                    //Marshal.FreeCoTaskMem(pDelObj2);
                    Marshal.FreeCoTaskMem(pDelObj3);
                    
                    IntPtr pDelObj = ItemIdList.Create(null, folderObject.PathData).Ptr;
                    Shell32.SHChangeNotify(ShellChangeEvents.Delete,
                        ShellChangeFlags.IdList | ShellChangeFlags.Flush | ShellChangeFlags.NotifyRecursive,
                        pDelObj,
                        IntPtr.Zero);
                    Marshal.FreeCoTaskMem(pDelObj);
                     
                }
                /*
                IntPtr pDelObj = ItemIdList.Create(null, folderObject.PathData).Ptr;
                Shell32.SHChangeNotify(ShellChangeEvents.Delete,
                    ShellChangeFlags.IdList | ShellChangeFlags.Flush | ShellChangeFlags.NotifyRecursive,
                    pDelObj,
                    IntPtr.Zero);
                Marshal.FreeCoTaskMem(pDelObj);
                 */
 
            }


            if (ShellView != null)
            {

                ShellView.Refresh();
                /*
                foreach (IFolderObject item in Items)
                {
                    List<byte[]> ls = new List<byte[]>();
                    ls.Add(item.PathData[item.PathData.Length - 1]);
                    int rt = _folderObj.ShellView.SelectItem(
                        ItemIdList.Create(null, (byte[][])ls.ToArray().Clone()).Ptr,
                        _SVSIF.SVSI_SELECT | _SVSIF.SVSI_SELECTIONMARK);

                    //_folderObj.ShellView.UIActivate()
                }
                */
            }

            if (this.parrent != null)
            {
                IntPtr pCurrObj = ItemIdList.Create(null, this.parrent.PathData).Ptr;
                Shell32.SHChangeNotify(ShellChangeEvents.UpdateDir /*| ShellChangeEvents.UpdateDir | ShellChangeEvents.UpdateItem*/,
                    ShellChangeFlags.IdList | ShellChangeFlags.Flush | ShellChangeFlags.NotifyRecursive,
                    pCurrObj,
                    IntPtr.Zero);
                Marshal.FreeCoTaskMem(pCurrObj);    
            }
            
            //refrash parrent
            IntPtr pParrent = ItemIdList.Create(null, RootPathData).Ptr;
            Shell32.SHChangeNotify(ShellChangeEvents.UpdateDir /*| ShellChangeEvents.UpdateDir | ShellChangeEvents.UpdateItem*/,
                ShellChangeFlags.IdList | ShellChangeFlags.Flush | ShellChangeFlags.NotifyRecursive,
                pParrent,
                IntPtr.Zero);
            Marshal.FreeCoTaskMem(pParrent);


            Shell32.SHChangeNotify(ShellChangeEvents.AssocChanged,
                ShellChangeFlags.Flush | ShellChangeFlags.NotifyRecursive,
                IntPtr.Zero,
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
            
            foreach (IFolderObject folderObject in items)
            {
                IntPtr pDelObj = ItemIdList.Create(null, folderObject.PathData).Ptr;
                Shell32.SHChangeNotify(ShellChangeEvents.Delete,
                    ShellChangeFlags.IdList | ShellChangeFlags.Flush,
                    pDelObj,
                    IntPtr.Zero);
                Marshal.FreeCoTaskMem(pDelObj);
            }

            IntPtr pCurrObj = ItemIdList.Create(null, PathData).Ptr;
            Shell32.SHChangeNotify(ShellChangeEvents.UpdateDir,
                ShellChangeFlags.IdList | ShellChangeFlags.Flush,
                pCurrObj,
                IntPtr.Zero);
            Marshal.FreeCoTaskMem(pCurrObj);
            //refrash parrent
            IntPtr pParrent = ItemIdList.Create(null, RootPathData).Ptr;
            Shell32.SHChangeNotify(ShellChangeEvents.UpdateDir,
                ShellChangeFlags.IdList | ShellChangeFlags.Flush,
                pParrent,
                IntPtr.Zero);
            Marshal.FreeCoTaskMem(pParrent);
        }

        public override void CopyItems(IFolderObject fo, List<string> lItems)
        {
            List<string> folders = new List<string>();
            List<string> files = new List<string>();
            CommandResult cr = Copy(fo.PathString, lItems, ref folders, ref files);
            if (!cr.IsSuccess)
            {
                cr.ShowMessage();
                return;
            }

            IntPtr pCurrObj = ItemIdList.Create(null, fo.PathData).Ptr;
            Shell32.SHChangeNotify(ShellChangeEvents.UpdateDir,
                ShellChangeFlags.IdList | ShellChangeFlags.Flush,
                pCurrObj,
                IntPtr.Zero);
            Marshal.FreeCoTaskMem(pCurrObj);
        }
	}
}
