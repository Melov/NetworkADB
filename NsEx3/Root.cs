using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using WindowsShell.ADB;
using WindowsShell.Interop;
using Microsoft.Win32;
using WindowsShell.Nspace;
using WindowsShell.Nspace.Icon;

namespace NsEx3
{
	public class Root : DefaultFolderObject
	{
		private IList items;
        internal static readonly Column ColName = new Column("Name", ColumnFormat.Left, typeof(string), new Guid("B725F130-47EF-101A-A5F1-02608C9EEBAC"), 10, 30, true, false);
        //internal static readonly Column ColName1 = new Column("Doc Title", ColumnFormat.Left, typeof(string), new Guid("F29F85E0-4FF9-1068-AB91-08002B27B3D9"), 2, 30, true, false);
        //internal static readonly Column ColDet = new Column("Details", ColumnFormat.Left, typeof(string), new Guid("C9944A21-A406-48FE-8225-AEC7E24C211B"), 8, 30, true, false);

        internal static readonly Column ColAttr = new Column("Attributes", ColumnFormat.Left, typeof(string), Guid.Empty, 1, 10, true, false);
        internal static readonly Column ColPerm1 = new Column("Owner", ColumnFormat.Left, typeof(string), Guid.Empty, 2, 10, true, false);
        internal static readonly Column ColPerm2 = new Column("Group", ColumnFormat.Left, typeof(string), Guid.Empty, 3, 10, true, false);
        internal static readonly Column ColFileSize = new Column("Size", ColumnFormat.Left, typeof(string), Guid.Empty, 4, 10, true, false);
        internal static readonly Column ColDate = new Column("Date", ColumnFormat.Left, typeof(string), Guid.Empty, 5, 10, true, false);
        internal static readonly Column ColTime = new Column("Time", ColumnFormat.Left, typeof(string), Guid.Empty, 6, 10, true, false);
        internal static readonly Column ColLinkTo = new Column("Link To", ColumnFormat.Left, typeof(string), Guid.Empty, 7, 30, true, false);

        public static Dictionary<string,Dictionary<int, System.Drawing.Icon>> ItemsIcons = new Dictionary<string, Dictionary<int, Icon>>();
        private ShellMenuItem[] _ShellMenuItems;

        public IFolderObject GetParrent()
        {
            return null;
        }

        public Root()
		{
            columns = new ColumnCollection();
            columns.Add(ColName);
            columns.Add(ColAttr);
            columns.Add(ColPerm1);
            columns.Add(ColPerm2);
            columns.Add(ColFileSize);
            columns.Add(ColDate);
            columns.Add(ColTime);
            columns.Add(ColLinkTo);
            
            //items = new ArrayList();
            //  byte[][] bt1 = new byte[1][] { new byte[1] { 0 } };
            //  byte[][] bt2 = new byte[1][] { new byte[1] { 1 } };
            // byte[][] bt3 = new byte[1][] { new byte[1] { 2 } };


            /*
            items.Add(new Item(bt1, 0, "One"));
            items.Add(new Item(bt2, 1, "Two"));
            items.Add(new Item(bt3, 2, "Three"));
             */
		}

        internal readonly ColumnCollection columns;

        public override ColumnCollection Columns
        {
            get
            {
                return columns;
            }
        }
       
	    public override ShellMenuItem[] MenuItems
	    {
	        get
	        {
	            if (_ShellMenuItems == null)
	            {

                    ShellMenuItem emSep = CreateExploreMenuItem("", "", "");
                    emSep.Separator = true;

                    ShellMenuItem emConnect = CreateConnectMenuItem();
                    emConnect.Bitmap =  (Bitmap)Properties.Resources.ResourceManager.GetObject("connect");	               
	                
                    ShellMenuItem emDisconnect = CreateDisconnectMenuItem();
                    emDisconnect.Bitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject("disconnect");
                    
                    ShellMenuItem emPreferences = CreatePreferencesMenuItem();
                    emPreferences.Bitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject("settings1");

                    ShellMenuItem emNewFolder = CreateNewFolderMenuItem();
                    emNewFolder.Bitmap = ((Icon)Properties.Resources.ResourceManager.GetObject("folder")).ToBitmap();

                    ShellMenuItem emPaste = CreatePasteMenuItem();
                    emPaste.Bitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject("paste1");

                    ShellMenuItem emConsole = CreateConsoleMenuItem();
                    emConsole.Bitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject("commandline");

	                _ShellMenuItems = new ShellMenuItem[]
	                {
	                    emPaste,
                        emSep,
	                    emNewFolder,
	                    //CreateDeleteMenuItem(new IFolderObject[]{this})
                        emSep,
	                    emConnect,
	                    emDisconnect,
                        emSep,
	                    emPreferences,
                        emConsole
	                };
	            }
	            return _ShellMenuItems;
	        }
	        set { _ShellMenuItems = value; }
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
                return FolderAttributes.Folder | FolderAttributes.HasSubFolder |FolderAttributes.HasPropSheet | FolderAttributes.CanMove | FolderAttributes.CanDelete | FolderAttributes.CanRename | FolderAttributes.CanCopy | FolderAttributes.CanLink | FolderAttributes.Browsable | FolderAttributes.DropTarget;
			}
		}

		public override string GetDisplayName(NameOptions opts)
		{
			throw new NotImplementedException();
		}

		public override ShellIcon GetIcon(bool open)
		{
			throw new NotImplementedException();
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
            //Folder.ItemsIcons = ItemsIcons;

            items = new ArrayList();
            int objId = 0;

            string error = string.Empty;
            IFileObject[] all = GetAllFromFolder(ref error);

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
                }                
            }
            else
            {
                SetEmptyText(string.Empty);
            }

            //FileObject[] sFolders = GetFoldersFromFolder();
            IFileObject[] sFolders = all.ToList().FindAll(x=>x.IsFolder == true).ToArray();

            for (int i = 0; i < sFolders.Length; i++)
            {
                Folder folder = new Folder(PathData, idListAbsolute, "", sFolders[i].Name, FolderAttributes.Folder, sFolders[i], ShellView,this);
                if (!ItemsIcons.ContainsKey("folder"))
                {
                    var icons = IconHelper.GetImagesByExt("folder");
                    ItemsIcons.Add("folder", icons);                  
                }
                folder.Icons = ItemsIcons["folder"];

                items.Add(folder);
                objId++;
            }
            //FileObject[] sFiles = GetFilesFromFolder();		    
            IFileObject[] sFiles = all.ToList().FindAll(x => x.IsFolder == false).ToArray();
            for (int i = 0; i < sFiles.Length; i++)
            {
                Item it = new Item(PathData, "", "", sFiles[i].Name, sFiles[i], this, ShellView);

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
            
			return items;
		}

		public override byte[] Persist()
		{
			throw new NotImplementedException();
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

                    if (attrs != FolderAttributes.None)
                    {
                        int nDataLen = r.ReadInt32();
                        byte[] btFo = r.ReadBytes(nDataLen);
                        FileObject fo = FileObject.ToObject(btFo);

                        //Folder.ItemsIcons = ItemsIcons;

                        Folder folder = new Folder(PathData, idListAbsolute, "", path, attrs, fo, ShellView, this);
                        if (ItemsIcons.ContainsKey("folder"))
                        {
                            folder.Icons = ItemsIcons["folder"];
                        }
                        return folder;    
                    }
                    else
                    {
                        Folder folder = new Folder(PathData, idListAbsolute, "", path, attrs, null, ShellView, this);
                        if (ItemsIcons.ContainsKey("folder"))
                        {
                            folder.Icons = ItemsIcons["folder"];
                        }
                        return folder;    
                    }
                    
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

                    Item it = new Item(PathData, "", path, valueName, fo, this, ShellView);
                    
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
            //return new Item(PathData, BitConverter.ToInt32(data, 0), BitConverter.ToInt32(data, 0).ToString());
            //Item it = (Item)(items[BitConverter.ToInt32(data, 0)]);
            //it.SetPath(PathData);      
		    try
		    {
                return (IFolderObject)items[BitConverter.ToInt32(data, 0)];
		    }
		    catch (Exception)
		    {
		        return this;
		    }            
		}

        public override void NewFolder()
        {
            FolderAttributes newFolderAttrs = FolderAttributes.Folder | FolderAttributes.HasPropSheet |
                                                  FolderAttributes.CanMove | FolderAttributes.CanDelete |
                                                  FolderAttributes.CanRename | FolderAttributes.CanCopy |
                                                  FolderAttributes.CanLink | FolderAttributes.Browsable |
                                                  FolderAttributes.DropTarget;

            string spath = CreateNewFolder("", "New Folder");
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

            using (MemoryStream ms = new MemoryStream())
            {                
                BinaryWriter w = new BinaryWriter(ms, Encoding.Default);
                w.Write(0);
                w.Write(spath);
                w.Write((int) newFolderAttrs);
                IFileObject foNew = new FileObject();
                foNew.Name = spath;
                foNew.IsFolder = true;
                byte[] btFo = FileObject.ToByteArray(foNew);
                w.Write((int)btFo.Length);
                w.Write(btFo, 0, btFo.Length);
                byte[] persist = ms.ToArray();
                
                var lst = PathData.ToList();
                lst.Add(persist);

                IntPtr pNew = ItemIdList.Create(null, lst.ToArray()).Ptr;
                Shell32.SHChangeNotify(
                    ShellChangeEvents.MkDir,
                    ShellChangeFlags.IdList | ShellChangeFlags.Flush,
                    pNew,
                    IntPtr.Zero);
                Marshal.FreeCoTaskMem(pNew);
            }
            
            using (MemoryStream ms = new MemoryStream())
            {                
                BinaryWriter w = new BinaryWriter(ms, Encoding.Default);
                w.Write(0);
                w.Write(spath);
                w.Write((int)newFolderAttrs);
                byte[] persist =  ms.ToArray();

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
        }

	    public override void CopyItems(IFolderObject fo, List<string> lItems)
	    {
            FolderAttributes newFolderAttrs = FolderAttributes.Folder | FolderAttributes.HasPropSheet |
                                                  FolderAttributes.CanMove | FolderAttributes.CanDelete |
                                                  FolderAttributes.CanRename | FolderAttributes.CanCopy |
                                                  FolderAttributes.CanLink | FolderAttributes.Browsable |
                                                  FolderAttributes.DropTarget;

	        FolderAttributes newFileAttrs = FolderAttributes.HasPropSheet | FolderAttributes.CanMove |
	                                        FolderAttributes.CanDelete | FolderAttributes.CanRename |
	                                        FolderAttributes.CanCopy | FolderAttributes.CanLink |
	                                        FolderAttributes.Browsable | FolderAttributes.DropTarget;

            List<string> folders = new List<string>();
            List<string> files = new List<string>();
            CommandResult cr = Copy(fo.PathString, lItems, ref folders, ref files);
            if (!cr.IsSuccess)
            {
                cr.ShowMessage();
                return;
            }
            /*
            foreach (string folder in folders)
	        {
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryWriter w = new BinaryWriter(ms, Encoding.Default);
                    w.Write(0);
                    w.Write(folder);
                    w.Write((int)newFolderAttrs);
                    byte[] persist = ms.ToArray();

                    var lst = PathData.ToList();
                    lst.Add(persist);

                    IntPtr pNew = ItemIdList.Create(null, lst.ToArray()).Ptr;
                    Shell32.SHChangeNotify(
                        ShellChangeEvents.MkDir,
                        ShellChangeFlags.IdList | ShellChangeFlags.Flush,
                        pNew,
                        IntPtr.Zero);
                    Marshal.FreeCoTaskMem(pNew);
                }
	        }

            foreach (string file in files)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryWriter w = new BinaryWriter(ms, Encoding.Default);
                    w.Write(1);
                    w.Write(file);
                    w.Write((int)newFileAttrs);
                    byte[] persist = ms.ToArray();

                    var lst = PathData.ToList();
                    lst.Add(persist);

                    IntPtr pNew = ItemIdList.Create(null, lst.ToArray()).Ptr;
                    Shell32.SHChangeNotify(
                        ShellChangeEvents.Create,
                        ShellChangeFlags.IdList | ShellChangeFlags.Flush,
                        pNew,
                        IntPtr.Zero);
                    Marshal.FreeCoTaskMem(pNew);
                }
            }
            */
            IntPtr pCurrObj = ItemIdList.Create(null, fo.PathData).Ptr;
            Shell32.SHChangeNotify(ShellChangeEvents.UpdateDir,
                ShellChangeFlags.IdList | ShellChangeFlags.Flush,
                pCurrObj,
                IntPtr.Zero);
            Marshal.FreeCoTaskMem(pCurrObj);
	    }
	}
}
