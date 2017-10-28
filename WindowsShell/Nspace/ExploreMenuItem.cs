using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WindowsShell.ADB;
using WindowsShell.Dialogs;
using WindowsShell.Interop;
using WindowsShell.Nspace.DragDrop;

namespace WindowsShell.Nspace
{
	public class ExploreMenuItem : ShellMenuItem
	{
/*
"open"        - Opens a file or a application
"openas"    - Opens dialog when no program is associated to the extension
"opennew"    - see MSDN 
"runas"    - In Windows 7 and Vista, opens the UAC dialog and in others, open the Run as... Dialog
"null"     - Specifies that the operation is the default for the selected file type.
"edit"        - Opens the default text editor for the file.    
"explore"    - Opens the Windows Explorer in the folder specified in lpDirectory.
"properties"    - Opens the properties window of the file.
"copy"        - see MSDN
"cut"        - see MSDN
"paste"    - see MSDN
"pastelink"    - pastes a shortcut
"delete"    - see MSDN
"print"    - Start printing the file with the default application.
"printto"    - see MSDN
"find"        - Start a search
*/
		public static readonly string ExploreVerb = "explore";
		public static readonly string OpenVerb = "open";
        public static readonly string CopyVerb = "copy";
        public static readonly string PasteVerb = "paste";
        public static readonly string PasteLinkVerb = "pastelink";
        public static readonly string DeleteVerb = "delete";
        public static readonly string InfoVerb = "info";
        public static readonly string RenameVerb = "rename";
        public static readonly string NewFolderVerb = "mkdir";

        public static readonly string PreferencesVerb = "preferences";
        public static readonly string ConsoleVerb = "console";
        public static readonly string ConnectVerb = "connect";
        public static readonly string DisconnectVerb = "disconnect";

        public static readonly string InstallApkVerb = "installapk";
        public static readonly string CreateScreenshotVerb = "createscreenshot";

		private readonly byte[][] fqPidl;
        private IFolderObject _folderObj;
	    private IFolderObject[] _items;

        public ExploreMenuItem(IFolderObject folderObj, byte[][] fqPidl, string text, string helpText, string verb, IFolderObject[] items = null)
			: base(null, false, false, true, helpText, false, false, text, verb)
        {
            _items = items;
            _folderObj = folderObj;
			if (fqPidl == null)
			{
				throw new ArgumentNullException("fqPidl");
			}

			this.fqPidl = fqPidl;
			Click += new EventHandler(ExploreMenuItem_Click);
		}

		public virtual void ExploreMenuItem_Click(object sender, EventArgs e)
		{
		    if (Verb.Equals(ConsoleVerb))
		    {
		        ConsoleForm cf = new ConsoleForm();
		        cf.ShowDialog();
		    }else
            if (Verb.Equals(PreferencesVerb))
            {
                ContextMenuEventArgs c = (ContextMenuEventArgs)e;
                
                //NativeWindow nativeWindow = new NativeWindow();
                //nativeWindow.AssignHandle(c.CommandInfo.hwnd);

                PreferencesForm pf = new PreferencesForm();
                pf.ShowDialog();
            }
            else if (Verb.Equals(ConnectVerb))
            {
                
                DeviseAddr addr = new DeviseAddr();
                if (!string.IsNullOrEmpty(addr.ConnectionType) && addr.ConnectionType.Equals("usb") &&
                    !string.IsNullOrEmpty(addr.UsbDevice))
                {
                    //ADBCommand commandD = new ADBCommand();
                    //CommandResult retD = commandD.Disconnect(true);   
                }
                else
                {
                    ADBCommand commandD = new ADBCommand();
                    CommandResult retD = commandD.Disconnect();   
                }                

                ADBCommand command = new ADBCommand();
                CommandResult ret = command.Connect();
                ret.ShowMessage();
                ADBCommand commandDev = new ADBCommand();
                CommandResult retDev = commandDev.Devices();
                if (retDev.IsSuccess)
                {
                    Dictionary<string,string> dicDev = new Dictionary<string, string>();
                    foreach (var str in retDev.Message.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            string[] s = str.Split(' ');
                            if (s.Any())
                            {
                                string sDevType = string.Empty;
                                if (s.Length > 1)
                                {
                                    sDevType = s[1];
                                }
                                if (!dicDev.ContainsKey(s[0]))
                                {
                                    dicDev.Add(s[0], sDevType);                                   
                                }                                
                            }
                        }
                    }

                    if (dicDev.Count == 0)
                    {
                        MessageBox.Show("List of devices attached - is empty",
                            "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    if (!dicDev.ContainsKey(commandDev.CurrentDevice()))
                    {
                        MessageBox.Show("List of devices attached - does not contain selected device",
                            "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    if (dicDev.ContainsKey(commandDev.CurrentDevice()) && dicDev[commandDev.CurrentDevice()].Equals("unauthorized"))
                    {
                        MessageBox.Show("Please authorize this computer on dialog in device,\r\nAnd after that, click Ok button to continue",
                            "Warning! " + retDev.Message.Replace("\t"," - "), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    IntPtr pParrent = ItemIdList.Create(null, _folderObj.PathData).Ptr;
                    Shell32.SHChangeNotify(ShellChangeEvents.UpdateDir,
                                ShellChangeFlags.IdList | ShellChangeFlags.Flush,
                                pParrent,
                                IntPtr.Zero);
                    Marshal.FreeCoTaskMem(pParrent);
                }
            }
            else if (Verb.Equals(DisconnectVerb))
            {
                ADBCommand command = new ADBCommand();
                CommandResult ret = command.Disconnect();
                ret.ShowMessage();

                IntPtr pParrent = ItemIdList.Create(null, _folderObj.PathData).Ptr;
                Shell32.SHChangeNotify(ShellChangeEvents.UpdateDir,
                            ShellChangeFlags.IdList | ShellChangeFlags.Flush,
                            pParrent,
                            IntPtr.Zero);
                Marshal.FreeCoTaskMem(pParrent);
            }
		    else if (Verb.Equals(InstallApkVerb))
		    {
                ADBCommand command = new ADBCommand();
                CommandResult ret = command.Install(_folderObj.PathString);
                ret.ShowMessage();		        
		    }
            else if (Verb.Equals(CreateScreenshotVerb))
            {
                ADBCommand command = new ADBCommand();
                CommandResult ret = command.CreateScreenShot(_folderObj.PathString);
                ret.ShowMessage();
               // MessageBox.Show("Create Screenshot to: " + _folderObj.PathString);
            }
            else if (Verb.Equals(InfoVerb))
            {                               
                StringCollection sc = new StringCollection();
               
                PermissionsForm pf = new PermissionsForm();
                pf.SetData(_items, _folderObj);
                pf.ShowDialog();
                /*
                if (_folderObj.GetParrent() != null)
                {
                    _folderObj.GetParrent().RefreshItems(_items);
                }
                 */
                //_folderObj.RefreshItems(_items);
                
            }
		    else if (Verb.Equals(NewFolderVerb))
		    {
                _folderObj.NewFolder();
		    }
		    else if (Verb.Equals(RenameVerb))
		    {                
		        using (Malloc m = Shell32.GetMalloc())
		        {
                    byte[][] clone = (byte[][])fqPidl.Clone();
		            List<byte[]> lsn = clone.ToList();
                    lsn.RemoveAt(lsn.Count-1);
		            clone = lsn.ToArray();
                    ItemIdList itemIdList = ItemIdList.Create(m, fqPidl);
                    ItemIdList itemIdListFolder = ItemIdList.Create(m, clone);
                    Shell32.SHOpenFolderAndSelectItems(itemIdListFolder.Ptr, 1, new[] { itemIdList.Ptr }, Shell32.OFASI_EDIT);
		        }
		    }
		    else if (Verb.Equals(CopyVerb))
		    {
                DataObject dobj = new DataObject();
                List<string> file_list = new List<string>();
                StringCollection sc = new StringCollection();
                foreach (IFolderObject folderObject in _items)
		        {
                    string sAdd = ((folderObject.Attributes & FolderAttributes.Folder) == FolderAttributes.Folder)
                            ? "[virtualfolder]"
                            : "[virtualfile]";
                    file_list.Add(string.Format("{0}{1}\\{2}", sAdd, CodePath.Code(folderObject.PathData), folderObject.PathString));
		            sc.Add(string.Format("{0}{1}\\{2}", sAdd, CodePath.Code(folderObject.PathData), folderObject.PathString));
                    //sc.Add(folderObject.PathString);
		            //file_list.Add(string.Format("{0}\\{1}", sAdd, folderObject.PathString));                    
		        }
                dobj.SetData(DataFormats.FileDrop, (System.String[])file_list.ToArray());
                dobj.SetData(typeof(StringCollection), sc);
                Clipboard.SetDataObject(dobj, false);
                //Clipboard.SetFileDropList(sc);
		    }
            else if (Verb.Equals(PasteLinkVerb))
            {
                List<string> lFiles = new List<string>();
                if (_folderObj.DataObject != null)
                {
                    DataObject dobj = _folderObj.DataObject;
                    StringCollection z = (StringCollection)dobj.GetData(typeof(StringCollection));
                    foreach (string s in z)
                    {
                        if (s.StartsWith("[virtualfolder]") || s.StartsWith("[virtualfile]"))
                            lFiles.Add(s);
                    }

                    lFiles.Add(_folderObj.PathString);
                    _folderObj.CopyItems(null, lFiles);

                    _folderObj.DataObject = null;
                }
                else
                if (Clipboard.ContainsData("DataObject"))
                {
                    DataObject dobj = (DataObject)Clipboard.GetDataObject();
                    StringCollection z = (StringCollection)dobj.GetData(typeof(StringCollection));
                    if (z != null)
                    {
                        foreach (string s in z)
                        {
                            if (s.StartsWith("[virtualfolder]"))
                            {
                                lFiles.Add(s);   
                            }                            
                        }

                        lFiles.Add(_folderObj.PathString);
                        _folderObj.CopyItems(null, lFiles);
                    }
                }  
            }
            else if (Verb.Equals(PasteVerb))
            {
                List<string> lFiles = new List<string>();
                //only for external
                if (Clipboard.ContainsFileDropList())
                {                    
                    StringCollection files = Clipboard.GetFileDropList();
                    foreach (string file in files)
                    {
                        if (!file.StartsWith("[virtualfolder]") && !file.StartsWith("[virtualfile]"))
                            lFiles.Add(file);
                    }                    
                }

                if (_folderObj.DataObject != null)
                {
                    DataObject dobj = _folderObj.DataObject;
                    StringCollection z = (StringCollection)dobj.GetData(typeof(StringCollection));
                    foreach (string s in z)
                    {
                        lFiles.Add(s);
                    }
                    _folderObj.DataObject = null;
                }else
                if (Clipboard.ContainsData("DataObject"))
                {
                    DataObject dobj = (DataObject)Clipboard.GetDataObject();
                    StringCollection z = (StringCollection)dobj.GetData(typeof(StringCollection));
                    if (z != null)
                    {
                        foreach (string s in z)
                        {
                            lFiles.Add(s);
                        }
                        //lFiles = DataObjectHelper.GetFiles(dobj);   
                    }                    
                }                                

                string sr = string.Empty;
                foreach (string file in lFiles)
                {
                    sr += file + "\r\n";
                }
                sr += "to\r\n" + _folderObj.PathString;
                //MessageBox.Show(sr);
                Debug.WriteLine(sr);
                _folderObj.CopyItems(_folderObj, lFiles);
            }
            else if (Verb.Equals(DeleteVerb))
            {
                _folderObj.DeleteItems(_items);
            }
		    else
		    {
		        using (Malloc m = Shell32.GetMalloc())
		        {

		            ContextMenuEventArgs c = (ContextMenuEventArgs) e;

		            ShellExecuteInfo sei = new ShellExecuteInfo();
		            sei.cbSize = (uint) Marshal.SizeOf(typeof (ShellExecuteInfo));
		            sei.fMask = ShellExecuteOptions.IdList | ShellExecuteOptions.ClassName;
		            ItemIdList itemIdList = ItemIdList.Create(m, fqPidl);
		            sei.lpIDList = itemIdList.Ptr;
		            sei.lpClass = "folder";
		            sei.hwnd = c.CommandInfo.hwnd;
		            sei.nShow = c.CommandInfo.nShow;
		            sei.lpVerb = Verb;

		            int result = Shell32.ShellExecuteEx(ref sei);

		            //m.Free(itemIdList.Ptr);

		            if (result == 0)
		            {
		                int lastError = Marshal.GetLastWin32Error();
		                throw new Exception("ShellExecuteEx failed; last error = " + lastError);
		            }
		        }
		    }
		}
	}
}
