using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using WindowsShell.ADB;
using WindowsShell.Interop;
using WindowsShell.Nspace.Link;
using MyData.Extensions;

namespace WindowsShell.Nspace
{
    [Serializable]
    public abstract class DefaultFolderObject : FileSystemManager, IFolderObject
    {
        private Dictionary<int, System.Drawing.Icon> _icons;
        private DataObject _dataObject;
        private ContextMenuImpl _contextMenu;
        private ShellMenuItem[] _ShellMenuItems;
        private IShellView _ShellView;
		private byte[][] rootPath = null;
		private byte[][] pathData = null;
	    private string pathString = string.Empty;
        private IdList _IdList = null;
        private IntPtr _pid = IntPtr.Zero;
        private IFolderObject parrent;
		public DefaultFolderObject() {}

        public IFolderObject GetParrent()
        {
            return parrent;
        }

        public virtual IShellView ShellView
        {
            get
            {
                return _ShellView;
            }
            set { _ShellView = value; }
        }

		#region IFolderObject Members

        public virtual IdList idListAbsolute
        {
            get
            {                
                var combined = new List<ShellId>(_IdList.Ids);               
                return IdList.Create(combined);
            }           
        }

        public virtual IntPtr pid
        {
            get
            {
                return _pid;
            }
            set
            {
                _pid = value;
            }
        }

		public virtual FolderAttributes Attributes
		{
			get
			{
				return FolderAttributes.None;
			}
		}

		public virtual ColumnCollection Columns
		{
			get
			{
				return new ColumnCollection();
			}
		}

		public virtual int CompareTo(Column column, IFolderObject obj)
		{
			// get the values for this item and the other item in the
			// specified column

			object value1 = GetColumnValue(column);
			object value2 = obj.GetColumnValue(column);

			if (column != null && column.Comparer != null)
			{
				// the column provides a comparer
				return column.Comparer.Compare(value1, value2);
			}
			else if (value1 == null && value2 != null)
			{
				return 1;	// sort empty column values to the bottom
			}
			else if (value2 == null && value1 != null)
			{
				return -1;	// sort empty column values to the bottom
			}
			else if (value1 == null && value2 == null ||
				!(value1 is IComparable && value2 is IComparable))
			{
				// compare the display names as a last resort
				return GetDisplayName(NameOptions.Normal).CompareTo(obj.GetDisplayName(NameOptions.Normal));
			}
			else 
			{
				// the values implement IComparable, so work with that;
				// hopefully the values are compatible
				return ((IComparable) value1).CompareTo(value2);
			}
		}

        public virtual DataObject DataObject
		{
			get
			{
                /*
                string sAdd = ((Attributes & FolderAttributes.Folder) == FolderAttributes.Folder)
                            ? "[virtualfolder]"
                            : "[virtualfile]";

                DataObject dobj = new DataObject();
                List<string> file_list = new List<string>();
                file_list.Add(string.Format("{0}{1}\\{2}", sAdd, CodePath.Code(PathData), PathString));      
                //file_list.Add(string.Format("{0}\\{1}", sAdd, PathString));
                dobj.SetData(DataFormats.FileDrop, file_list.ToArray());
                 */

                /*
                var virtualFileDataObject = new VirtualFileDataObject(
               null,
               (vfdo) =>
               {
                   if (DragDropEffects.Move == vfdo.PerformedDropEffect)
                   {
                       // Hide the element that was moved (or cut)
                       // BeginInvoke ensures UI operations happen on the right thread
                       //Dispatcher.BeginInvoke((Action)(() => VirtualFile.Visibility = Visibility.Hidden));
                   }
               });

                // Provide a virtual file (generated on demand) containing the letters 'a'-'z'
                virtualFileDataObject.SetData(new VirtualFileDataObject.FileDescriptor[]
            {
                new VirtualFileDataObject.FileDescriptor
                {
                    Name = "Alphabet.txt",
                    Length = 26,
                    ChangeTimeUtc = DateTime.Now.AddDays(-1),
                    StreamContents = stream =>
                        {
                            var contents = Enumerable.Range('a', 26).Select(i => (byte)i).ToArray();
                            stream.Write(contents, 0, contents.Length);
                        }
                },
            });
                 */
                /*
                List<DataObjectEx.SelectedItem> lsDi = new List<DataObjectEx.SelectedItem>();
                DataObjectEx.SelectedItem itm = new DataObjectEx.SelectedItem();
			    itm.FileName = GetDisplayName(NameOptions.Normal) + ".reg";
                itm.WriteTime = DateTime.Now;
			    itm.TempContenet = PathString;
                itm.FileSize = itm.TempContenet.Length;
                lsDi.Add(itm);

                DataObjectEx.SelectedItem itm1 = new DataObjectEx.SelectedItem();
                itm1.FileName = GetDisplayName(NameOptions.Normal) + ".lnk";
                itm1.WriteTime = DateTime.Now;
                LinkMayker lb = new LinkMayker();
                IntPtr path = ItemIdList.Create(null, PathData).Ptr;
                itm1.TempContenet1 = lb.MaleLink("virtual link", path, PathString);
			    itm1.FileSize = itm1.TempContenet1.Length;//itm.TempContenet.Length;
                lsDi.Add(itm1);

                DataObjectEx dobj = new DataObjectEx(lsDi.ToArray());
                dobj.SetData(NativeMethods.CFSTR_FILEDESCRIPTORW, null);
                dobj.SetData(NativeMethods.CFSTR_FILECONTENTS, null);
                dobj.SetData(NativeMethods.CFSTR_PERFORMEDDROPEFFECT, null);
                */
                return _dataObject;
			}
            set { _dataObject = value; }
		}

		public virtual object GetColumnValue(Column column)
		{            
		    return null;
		}

        public virtual void SetColumnValue(Column column, object value)
        {
            return;
        }

		public abstract string GetDisplayName(NameOptions opts);

		public virtual ShellIcon GetIcon(bool open)
		{
			return null;
		}

        public virtual Dictionary<int, System.Drawing.Icon> Icons
        {
            get { return _icons; }
            set { _icons = value; }
        }

		public virtual IEnumerable GetItems(IWin32Window owner)
		{
			return null;
		}

		public virtual ShellMenuItem[] GetMenuItems(IFolderObject[] items)
		{
			return null;
		}

        public virtual void DeleteItems(IFolderObject[] items)
        {
            
        }

        public virtual void RefreshItems(IFolderObject[] items)
        {

        }

        public virtual void CopyItems(IFolderObject fo, List<string> lItems)
        {
            if (fo == null)
            {
                List<string> folders = new List<string>();
                List<string> files = new List<string>();

                string sRealDestination = lItems[lItems.Count - 1];
                lItems.RemoveAt(lItems.Count - 1);

                if (lItems.Count > 0)
                {
                    foreach (string item in lItems)
                    {
                        string sObject = item;
                        byte[][] btp = CodePath.Decode(CodePath.GetPathFromString(sObject));

                        LinkMayker lb = new LinkMayker();
                        IntPtr path = ItemIdList.Create(null, btp).Ptr;
                        string sFullName = sObject.Substring(sObject.IndexOf('\\') + 1);
                        lb.MakeLink("Virtual Link", path, sRealDestination + "\\" + sFullName.Substring(sFullName.LastIndexOf(FD[0]) + 1) + ".lnk");
                        Marshal.FreeCoTaskMem(path);

                        //Copy(sRealDestination, lItems, ref folders, ref files);         
                    }                    
                }                
            }
            else
            {
                List<string> folders = new List<string>();
                List<string> files = new List<string>();

                CommandResult cr = Copy(fo.PathString, lItems, ref folders, ref files);
                if (!cr.IsSuccess)
                {
                    cr.ShowMessage();
                }
            }            
        }

        public virtual void NewFolder()
        {

        }

		public virtual ShellMenuItem[] MenuItems
		{
			get
			{
                return _ShellMenuItems;
			}
		    set
		    {
                _ShellMenuItems = value;
		    }
		}

	    public virtual string PathString
	    {
            get
            {
                return pathString;
            }
	        set { pathString = value; }
	    }

		public virtual byte[][] PathData
		{
			get
			{
				if (rootPath == null)
				{
					throw new InvalidOperationException("Path not initialized with SetPath");
				}
				else if (pathData == null)
				{
					pathData = new byte[rootPath.Length + 1][];
					Array.Copy(rootPath, 0, pathData, 0, rootPath.Length);
					pathData[pathData.Length-1] = Persist();
				}

				return (byte[][]) pathData.Clone();
			}
		}

		public abstract byte[] Persist();

        public abstract IFolderObject Restore(byte[] data, bool forName = false);

		public virtual string RemoteComputer
		{
			set { throw new NotSupportedException(); }
		}

		public virtual void SetName(IWin32Window owner, string name, NameOptions opts)
		{
			throw new NotSupportedException();
		}

		public virtual void SetPath(byte[][] data)
		{
			if (rootPath != null)
			{
				//throw new InvalidOperationException("Path already initialized");
			}

			rootPath = (byte[][]) data.Clone();
		}

        public virtual void SetPath(string sPath)
        {
            pathString = sPath;
        }

        public virtual void SetIdList(IdList IdList)
        {
            var combined = new List<ShellId>(IdList.Ids);
            _IdList = IdList.Create(combined);
        }

	    public virtual void SetPointer(IntPtr pid)
	    {
	        this.pid = pid;
	    }
		public virtual void SetFullPath(byte[][] data)
		{
			if (rootPath != null)
			{
				throw new InvalidOperationException("Path already initialized");
			}

		    if (data.Length > 0)
		    {
                rootPath = new byte[data.Length - 1][];
                Array.Copy(data, 0, rootPath, 0, data.Length - 1);   
		    }			

			pathData = (byte[][]) data.Clone();
		}

		#endregion

		protected virtual ExploreMenuItem CreateExploreMenuItem()
		{
			return CreateExploreMenuItem("Explore", "Explore", ExploreMenuItem.ExploreVerb);
		}

		protected virtual ExploreMenuItem CreateOpenMenuItem()
		{
			return CreateExploreMenuItem("Open", "Open", ExploreMenuItem.OpenVerb);
		}

        protected virtual ExploreMenuItem CreateCopyMenuItem(IFolderObject[] items)
        {
            return CreateExploreMenuItem("Copy", "Copy", ExploreMenuItem.CopyVerb, items);
        }

        protected virtual ExploreMenuItem CreateRenameMenuItem()
        {
            return CreateExploreMenuItem("Rename", "Rename", ExploreMenuItem.RenameVerb);
        }

        protected virtual ExploreMenuItem CreatePasteMenuItem()
        {
            return CreateExploreMenuItem("Paste", "Paste", ExploreMenuItem.PasteVerb);
        }

        protected virtual ExploreMenuItem CreateDeleteMenuItem(IFolderObject[] items)
        {
            return CreateExploreMenuItem("Delete", "Delete", ExploreMenuItem.DeleteVerb, items);
        }

        protected virtual ExploreMenuItem CreateInfoMenuItem(IFolderObject[] items)
        {
            return CreateExploreMenuItem("Info", "Info", ExploreMenuItem.InfoVerb, items);
        }

        protected virtual ExploreMenuItem CreateNewFolderMenuItem()
        {
            return CreateExploreMenuItem("New Folder", "New Folder", ExploreMenuItem.NewFolderVerb);
        }

        protected virtual ExploreMenuItem CreatePreferencesMenuItem()
        {
            return CreateExploreMenuItem("Preferences", "Preferences", ExploreMenuItem.PreferencesVerb);
        }
        protected virtual ExploreMenuItem CreateConsoleMenuItem()
        {
            return CreateExploreMenuItem("ADB Console", "ADB Console", ExploreMenuItem.ConsoleVerb);
        }
        protected virtual ExploreMenuItem CreateConnectMenuItem()
        {
            return CreateExploreMenuItem("Connect", "Connect", ExploreMenuItem.ConnectVerb);
        }
        protected virtual ExploreMenuItem CreateDisconnectMenuItem()
        {
            return CreateExploreMenuItem("Disconnect", "Disconnect", ExploreMenuItem.DisconnectVerb);
        }

        protected virtual ExploreMenuItem CreateExploreMenuItem(string text, string helpText, string verb, IFolderObject[] items = null)
		{
            return new ExploreMenuItem(this, PathData, text, helpText, verb, items);
		}

		protected virtual byte[][] RootPathData
		{
			get
			{
				return (byte[][]) rootPath.Clone();
			}
		}


        public ContextMenuImpl ContextMenu
        {
            get { return _contextMenu; }
            set { _contextMenu = value; }
        }
    }
}
