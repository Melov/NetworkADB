using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using WindowsShell.Interop;
using WindowsShell.Nspace.DragDrop;
using WindowsShell.Nspace.Link;
using MyData.Extensions;
using IDataObject = System.Runtime.InteropServices.ComTypes.IDataObject;
using IPersist = WindowsShell.Interop.IPersist;
using System.Collections.Specialized;
using System.IO;

namespace WindowsShell.Nspace
{
	[Guid("7fc15bfb-b668-47e2-807a-2c3e565e2f15"),
	 CLSCompliant(false)]
    public class NsExtension : StandardOleMarshalObject, IPersistFolder2, IPersistIDList, IShellFolder2, IShellFolderViewCB, IRemoteComputer, IDropTargetHelper, IDragSourceHelper, IQueryInfo, WindowsShell.Nspace.DragDrop.IDropTarget, IPersistFile, IShellExtInit, IContextMenu
	{
		#region IIDs

		protected static readonly Guid IID_IDataObject = new Guid("0000010e-0000-0000-C000-000000000046");
		protected static readonly Guid IID_IDropSource = new Guid("00000121-0000-0000-C000-000000000046");
		protected static readonly Guid IID_IExtractIcon = new Guid("000214FA-0000-0000-C000-000000000046");
        protected static readonly Guid IID_IExtractIconA = new Guid("000214eb-0000-0000-c000-000000000046");
        protected static readonly Guid IID_IExtractImage = new Guid("BB2E617C-0920-11d1-9A0B-00C04FC2D6C1");
		protected static readonly Guid IID_IContextMenu = new Guid("000214E4-0000-0000-C000-000000000046");
        protected static readonly Guid IID_SDefined_Unknown2 = new Guid("93f81976-6a0d-42c3-94dd-aa258a155470");
		protected static readonly Guid IID_IRemoteComputer = new Guid("000214FE-0000-0000-C000-000000000046");
		protected static readonly Guid IID_IShellFolder = new Guid("000214E6-0000-0000-C000-000000000046");
		protected static readonly Guid IID_IShellFolder2 = new Guid("93F2F68C-1D1B-11d3-A30E-00C04F79ABD1");
		protected static readonly Guid IID_IShellView = new Guid("000214E3-0000-0000-C000-000000000046");
        public static readonly Guid IID_IFolderView2 = new Guid("1af3a467-214f-4298-908e-06b03e0b39f9");

        protected static readonly Guid IID_IShellLinkA = new Guid("000214EE-0000-0000-C000-000000000046");
		protected static readonly Guid IID_IShellLinkW = new Guid("000214F9-0000-0000-C000-000000000046");
        protected static readonly Guid IID_IQueryAssociations = new Guid("c46ca590-3c3f-11d2-bee6-0000f805ca57");
        protected static readonly Guid IID_IDropTarget = new Guid("00000122-0000-0000-C000-000000000046");

        protected static readonly Guid IID_IDelegateFolder = new Guid("ADD8BA80-002B-11D0-8F0F-00C04FD7D062");
        protected static readonly Guid IID_IQueryInfo = new Guid("00021500-0000-0000-C000-000000000046");
        
        

		#endregion

		#region COM register/unregister functions

		[ComRegisterFunction]
		private static void Register(Type type)
		{            
			new NsExtensionRegistrar(type).Register();
		}

		[ComUnregisterFunction]
		private static void Unregister(Type type)
		{
			new NsExtensionRegistrar(type).Unregister();
		}

		#endregion
        /*
        public int SetItemAlloc(ref IMalloc pmalloc)
	    {
            
	        MessageBox.Show("SetItemAlloc");
	        return WinError.S_OK;
	    }
        */
		#region Fields

		protected readonly IFolderObject folderObj;
		protected readonly NsExtension parent;
        public static IShellView SHELLV;

		#endregion

		#region Constructors

		protected NsExtension(IFolderObject folderObj)
		{
			if (folderObj == null)
			{
				throw new ArgumentNullException("folderObj");
			}

			this.folderObj = folderObj;
		    this.folderObj.ShellView = SHELLV;
		}

		protected NsExtension(IFolderObject folderObj, NsExtension parent)
		{
			if (parent == null)
			{
				throw new ArgumentNullException("parent");
			}
			else if (folderObj == null)
			{
				throw new ArgumentNullException("folderObj");
			}

			this.parent = parent;
			this.folderObj = folderObj;
		}

		#endregion

		#region IPersist members

        private byte[] PidlToBytes(IntPtr pidl)
        {

            // compute the total size..

            ushort total = 0;
            ushort cb = (ushort)Marshal.ReadInt16(pidl);

            while (cb != 0)
            {
                total += cb;

                cb = (ushort)Marshal.ReadInt16(pidl, total);
            }

            // allocate...

            byte[] ret = new byte[total];


            // read...

            Marshal.Copy(pidl, ret, 0, total);

            // return...

            return ret;

        }

		public int GetClassID(out Guid pClassID)
		{
			GuidAttribute g = (GuidAttribute) Attribute.GetCustomAttribute(GetType(), typeof(GuidAttribute));
			pClassID = new Guid(g.Value);
            return WinError.S_OK;
		}

        int IPersistIDList.GetClassID(out Guid pClassID) 
	    {
            return ((IPersist)this).GetClassID(out pClassID); 
	    }

        int IPersistIDList.SetIDList(IntPtr pidl)
        {
            return ((IPersistFolder2)this).Initialize(pidl);
        }

        int IPersistIDList.GetIDList([Out] out IntPtr pidl)
        {
            return ((IPersistFolder2)this).GetCurFolder(out pidl);
        }

	    #endregion

		#region IPersistFolder members

        public int Initialize(IntPtr pidl)
        {
            folderObj.SetFullPath(ItemIdList.Create(pidl).GetItemData());
            var idListAbsolute = PidlManager.PidlToIdlist(pidl);
            //string s = PidlManager.GetPidlDisplayName(pidl.Ptr);
            //Debug.Write(s);
            //folderObj.SetIdList(PidlManager.PidlToIdlist(pidl));
            //folderObj.IdList = PidlManager.PidlToIdlist(pidl.Ptr);
            folderObj.SetIdList(idListAbsolute);
            //folderObj.SetPointer(pidl.Ptr);
            //folderObj.pid = pidl.Ptr;
            return WinError.S_OK;
        }

        public int GetCurFolder(out IntPtr ppidl)
        {
            //ppidl = PidlManager.IdListToPidl(folderObj.IdList);
            //Debug.WriteLine("GetCurFolder: " + folderObj.ToString());
            ppidl = ItemIdList.Create(null,folderObj.PathData).Ptr;
            //ppidl = ItemIdList.Create(PidlManager.IdListToPidl(folderObj.IdList));
            //Malloc m = Shell32.GetMalloc();
            //ppidl = ItemIdList.Create(m, folderObj.PathData);

          /*
            if (folderObj.idListAbsolute == null)
            {
                ppidl = IntPtr.Zero;
                return WinError.S_FALSE;
            }
          
            ppidl = PidlManager.IdListToPidl(folderObj.idListAbsolute);
           */
            return WinError.S_OK;
            
/*
            //using (Malloc m = Shell32.GetMalloc())
            {
                ItemIdList lst = ItemIdList.Create(null, folderObj.PathData);
                ppidl = lst;
                ItemId[] itms = lst.GetItems();
                //ppidl = ItemIdList.Create(itms[itms.Length-1].Ptr);
                Debug.WriteLine(itms.Length - 1);
            }
            //ppidl = ItemIdList.Create(folderObj.pid);
 */
        }
	    #endregion

		#region IShellFolder members

        public int ParseDisplayName(IntPtr hwnd, IntPtr pbc, string pszDisplayName, ref uint pchEaten, out IntPtr ppidl, ref SFGAO pdwAttributes)
        {
            ppidl = ItemIdList.Create(null, folderObj.PathData).Ptr;
            return WinError.S_OK;
        }
        /*
		public void ParseDisplayName(IWin32Window hwnd, IntPtr pbc, string pszwDisplayName, IntPtr pchEaten, out ItemIdList ppidl, IntPtr attrs)
		{
			// TODO
			//throw new NotImplementedException();
		    ppidl = ItemIdList.Create(null, folderObj.PathData);
		}
        */
		public void EnumObjects(IWin32Window hwnd, EnumOptions grfFlags, out IEnumIDList ppenumIDList)
		{
			ppenumIDList = new EnumIdListImpl(grfFlags, folderObj.GetItems(hwnd));
		}
        
		public int BindToObject(ItemIdList pidl, IntPtr pbc, ref Guid riid, out IntPtr ppv)
		{           
			if (riid.Equals(NsExtension.IID_IShellFolder) ||
				riid.Equals(NsExtension.IID_IShellFolder2))
			{
				IFolderObject obj = folderObj;
                /*
                byte[][] here = folderObj.PathData;
				ArrayList oldAbs = new ArrayList(here);
                oldAbs.AddRange(pidl.GetItemData());				
				ItemIdList oldPidl = ItemIdList.Create(null, (byte[][]) oldAbs.ToArray(typeof(byte[])));
                folderObj.SetPath(oldPidl.GetItemData());
                */

                foreach (byte[] data in pidl.GetItemData())
				{
					obj = obj.Restore(data);
				}

				Type interfaceType = riid.Equals(NsExtension.IID_IShellFolder)
					? typeof(IShellFolder)
					: typeof(IShellFolder2);

				ppv = Marshal.GetComInterfaceForObject(new NsExtension(obj, this), interfaceType);

                /*
                IDelegateFolder pdfl;
			    IntPtr pdf;
			    Guid g = IID_IDelegateFolder;
                Marshal.QueryInterface(ppv, ref g, out pdf);
                pdfl = (IDelegateFolder)Marshal.GetObjectForIUnknown(pdf);
			    using (Malloc m = Shell32.GetMalloc())
			    {
                    pdfl.SetItemAlloc(m);
			    }			    
			    Marshal.Release(pdf);
                 */

			}
			else if (riid.Equals(NsExtension.IID_IRemoteComputer))
			{
				ppv = IntPtr.Zero;
				Debug.Write(pidl);
			}
			else
			{
                ppv = IntPtr.Zero;
                return WinError.E_NOINTERFACE; //throw new COMException();
			}
            return WinError.S_OK;
		}

		public int BindToStorage(ItemIdList pidl, IntPtr pbc, ref Guid riid, out IntPtr ppv)
		{
            ppv = IntPtr.Zero;
            return WinError.E_NOTIMPL;
			//throw new NotImplementedException();
		}

	    public void SetEmptyText(string text)
	    {
	        IntPtr pdf;
	        Guid gFV = IID_IFolderView2;
	        IntPtr ppv = Marshal.GetComInterfaceForObject(folderObj.ShellView, typeof (IShellView));
	        Marshal.QueryInterface(ppv, ref gFV, out pdf);
	        IFolderView2 pFV2 = (IFolderView2) Marshal.GetObjectForIUnknown(pdf);
	        pFV2.SetText(0, text);
	        Marshal.ReleaseComObject(pFV2);
	    }

	    public SortDirection GetSortDirection()
	    {
	        IntPtr pdf;
	        Guid gFV = IID_IFolderView2;
	        IntPtr ppv = Marshal.GetComInterfaceForObject(folderObj.ShellView, typeof (IShellView));
	        Marshal.QueryInterface(ppv, ref gFV, out pdf);
	        IFolderView2 pFV2 = (IFolderView2) Marshal.GetObjectForIUnknown(pdf);
	        SORTCOLUMN scc = new SORTCOLUMN();
	        int ret = pFV2.GetSortColumns(out scc, 1);
	        Marshal.ReleaseComObject(pFV2);
	        return scc.direction;
	    }

	    public int CompareIDs(IntPtr lParam, ItemIdList pidl1, ItemIdList pidl2)
		{           
            
            //  Get the low short from the lParam, this is the sorting option.
            short sortingRule = (short)(lParam.ToInt64() & 0x000000FF);
            SCHIDS modifiers = (SCHIDS)((lParam.ToInt64() >> 16) & 0x000000FF);

            /*
	        SortDirection sd = SortDirection.Default;
            if (folderObj.ShellView != null)
            {
                sd = GetSortDirection();
            }            
            */

			IEnumerator enum1 = pidl1.GetItemData().GetEnumerator();
			IFolderObject item1 = folderObj;

			IEnumerator enum2 = pidl2.GetItemData().GetEnumerator();
			IFolderObject item2 = folderObj;

			// this assumes enum1 and enum2 contain one item each, *or*
			// that each item in the path's CompareTo method can take
			// a Column from *this* folder object; not necessarily their
			// parent's folder object
			
			ColumnCollection columns = folderObj.Columns;

            Column column = sortingRule >= 0 && sortingRule < columns.Count ? columns[sortingRule] : null;

			while (enum1.MoveNext())
			{
				if (enum2.MoveNext())
				{
				    byte[] p1 = (byte[]) enum1.Current;
                    byte[] p2 = (byte[]) enum2.Current;
                    if (p1.Length == 4 || p2.Length == 4)
                    {
                        int i = 0;
                        return -1;
                    }
                    item1 = item1.Restore(p1);
                    item2 = item2.Restore(p2);

                    //Debug.WriteLine("CompareIDs:" + lParam+"   " + sortingRule + "   " + modifiers + ">    " + item1.PathString + "     " + item2.PathString);
				    
					int result = item1.CompareTo(column, item2);
				    /*
                    string s1 = item1.PathString;
                    string s2 = item2.PathString;
                    List<string> ss= new List<string>();
                    ss.Add(s1);
                    ss.Add(s2);
                    ss = sd == SortDirection.Ascending ? ss.OrderBy(x => x).ToList() : ss.OrderByDescending(x => x).ToList();
                    */
                    //int result = (ss[0].Equals(s1) ? -1 : 1);
				    bool bFolder1 = ((item1.Attributes & FolderAttributes.Folder) == FolderAttributes.Folder);
                    bool bFolder2 = ((item2.Attributes & FolderAttributes.Folder) == FolderAttributes.Folder);
                    if ((bFolder1 && !bFolder2))
                        return -1;
                    if ((!bFolder1 && bFolder2))
                        return 1;

					if (result != 0)
					{
					    int retRez = result;
                        //if (sd == SortDirection.Descending)
                            //if (retRez == -1)
                             //   retRez = retRez * (- 1);
                        /*
                        if (sd == SortDirection.Descending)
                            if (retRez == 1)
                                retRez = retRez * (-1);*/
                        return retRez;
					}
				}
				else
				{
					return -1;
				}
			}

		    if (enum2.MoveNext())
		    {
                return 1;
		    }

			return 0;
		}

        

		public int CreateViewObject(IWin32Window hwndOwner, ref Guid riid, out IntPtr ppv)
		{
			if (NsExtension.IID_IShellView.Equals(riid))
			{
                SFV_CREATE csfv = new SFV_CREATE();
                csfv.cbSize = (uint)Marshal.SizeOf(typeof(SFV_CREATE));
                csfv.psfvcb = this as IShellFolderViewCB;
			    csfv.pshf = this as IShellFolder;
			    

                //List<ShellDetailColumn> lsr = new List<ShellDetailColumn>();
			    //DefaultNamespaceFolderView def = new DefaultNamespaceFolderView(lsr, null);
			    //IShellView isv = def.CreateShellView(this as IShellFolder);
                //ShellViewHost wh = new ShellViewHost(new Control());
			    IShellView isv;
                int result = Shell32.SHCreateShellFolderView(ref csfv, out isv);
                Marshal.ThrowExceptionForHR(result);
                ppv = Marshal.GetComInterfaceForObject(isv, typeof(IShellView));
				if (ppv == IntPtr.Zero)
				{
					throw new COMException();
				}
                folderObj.ShellView = isv;
			    SHELLV = isv;

			    SetEmptyText("Please click `Connect` in Context Menu");
			}
            else if (riid.Equals(NsExtension.IID_IQueryInfo))
            {
                IFolderObject obj = null;
                obj = folderObj;
                QueryInfoImpl impl = new QueryInfoImpl(obj);
                ppv = Marshal.GetComInterfaceForObject(impl, typeof(IQueryInfo));
            }
			else if (NsExtension.IID_IContextMenu.Equals(riid))
			{
				ppv = Marshal.GetComInterfaceForObject(new ContextMenuImpl(null, folderObj), typeof(IContextMenu));
			}/*
            else if (NsExtension.IID_SDefined_Unknown2.Equals(riid))
            {
                ppv = Marshal.GetComInterfaceForObject(new ContextMenuImpl(null, folderObj), typeof(IContextMenu));
            }*/
            else if (NsExtension.IID_IShellLinkW.Equals(riid))
            {
                IShellLink link = (IShellLink)new ShellLink();
                IFolderObject obj = null;
                obj = folderObj;
                IntPtr path = ItemIdList.Create(null, obj.PathData).Ptr;
                link.SetIDList(path);
                ppv = Marshal.GetComInterfaceForObject(link, typeof(IShellLink));
            }   
            else if (riid.Equals(NsExtension.IID_IDropTarget))
            {
                IFolderObject obj = null;
                obj = folderObj;//.Restore(ItemId.Create(apidl[0]).GetData());

                MyDropTarget dropTarget = new MyDropTarget(obj);

                ppv = Marshal.GetComInterfaceForObject(dropTarget, typeof(WindowsShell.Nspace.DragDrop.IDropTarget));

                if (ppv == IntPtr.Zero)
                {
                    throw new COMException();
                }
            }
			else
			{
				//throw new COMException();
                //Debug.WriteLine("CreateViewObject: NOT: " + riid);
			    ppv = IntPtr.Zero;
			    return WinError.E_NOINTERFACE;
			}
		    return WinError.S_OK;
		}

        

		public void GetAttributesOf(uint cidl, IntPtr[] apidl, ref FolderAttributes rgfInOut)
		{
			for (int i = 0; i < cidl && rgfInOut != (FolderAttributes) 0; i++)
			{
				ItemId item = ItemId.Create(apidl[i]);
				rgfInOut &= folderObj.Restore(item.GetData()).Attributes;
			}
		}

	    

		public int GetUIObjectOf(IWin32Window hwndOwner, uint cidl, IntPtr[] apidl, ref Guid riid, IntPtr rgfReserved, out IntPtr ppv)
		{            
		    if (riid.Equals(NsExtension.IID_IDropTarget))
		    {
		        IFolderObject obj = null;
		        obj = folderObj.Restore(ItemId.Create(apidl[0]).GetData());

                MyDropTarget dropTarget = new MyDropTarget(obj);                

		        ppv = Marshal.GetComInterfaceForObject(dropTarget, typeof (WindowsShell.Nspace.DragDrop.IDropTarget));

		        if (ppv == IntPtr.Zero)
		        {
		            throw new COMException();
		        }
		    }
		    else if (riid.Equals(NsExtension.IID_IExtractIcon) && cidl == 1)
		    {
		        if (cidl != 1)
		        {
		            throw new ArgumentOutOfRangeException("cidl", cidl, "Expected exactly one PIDL to retrieve IExtractIcon");
		        }

		        ppv =
		            Marshal.GetComInterfaceForObject(
		                new ExtractIconImpl(folderObj.Restore(ItemIdList.Create(apidl[0]).GetItems()[0].GetData())),
		                typeof (IExtractIcon));
		    }
            else if (riid.Equals(NsExtension.IID_IExtractIconA) && cidl == 1)
            {
                if (cidl != 1)
                {
                    throw new ArgumentOutOfRangeException("cidl", cidl, "Expected exactly one PIDL to retrieve IExtractIcon");
                }

                ppv =
                    Marshal.GetComInterfaceForObject(
                        new ExtractIconImpl(folderObj.Restore(ItemIdList.Create(apidl[0]).GetItems()[0].GetData())),
                        typeof(IExtractIcon));
            }
                /*
            else if (riid.Equals(NsExtension.IID_IExtractImage) && cidl == 1)
            {
                IFolderObject obj = null;
                obj = folderObj.Restore(ItemId.Create(apidl[0]).GetData());

                ExtractImageImpl extractImage = new ExtractImageImpl(obj);

                ppv = Marshal.GetComInterfaceForObject(extractImage, typeof(IExtractImage));

                if (ppv == IntPtr.Zero)
                {
                    throw new COMException();
                }    
            }*/
		    else if (riid.Equals(NsExtension.IID_IDataObject))
		    {
		       // if (cidl != 1)
		        {
		           // throw new ArgumentOutOfRangeException("cidl", cidl, "Expected exactly one PIDL to retrieve IDataObject");
                    DataObject dataObject = new DataObject();
                    List<string> file_list = new List<string>();
                    StringCollection sc = new StringCollection();
                    foreach (IntPtr intPtr in apidl)
                    {
                        IFolderObject obj = folderObj.Restore(ItemIdList.Create(intPtr).GetItemData()[0]);
                                                
                        string sAdd = ((obj.Attributes & FolderAttributes.Folder) == FolderAttributes.Folder)
                            ? "[virtualfolder]"
                            : "[virtualfile]";
                        file_list.Add(string.Format("{0}{1}\\{2}", sAdd, CodePath.Code(obj.PathData), obj.PathString));                        
                        sc.Add(string.Format("{0}{1}\\{2}", sAdd, CodePath.Code(obj.PathData), obj.PathString));                        
                    }
                    //dataObject.SetData(DataFormats.FileDrop, file_list.ToArray());
                    dataObject.SetData(DataFormats.FileDrop, (System.String[])file_list.ToArray());
                    dataObject.SetData(typeof(StringCollection), sc);
                    IntPtr pUnk = Marshal.GetIUnknownForObject(dataObject);
                    Marshal.ThrowExceptionForHR(Marshal.QueryInterface(pUnk, ref riid, out ppv));   
		        }
                /*
		        else
		        {
                    IFolderObject obj = folderObj.Restore(ItemIdList.Create(apidl[0]).GetItemData()[0]);
                    DataObject dataObject = obj.DataObject;
                    IntPtr pUnk = Marshal.GetIUnknownForObject(dataObject);
                    Marshal.ThrowExceptionForHR(Marshal.QueryInterface(pUnk, ref riid, out ppv));   
		        }
                 */
		        
		    }		    
			else if (riid.Equals(NsExtension.IID_IContextMenu))
			{
				ArrayList fos = new ArrayList();

				for (uint i = 0; i < cidl; i++)
				{
					fos.Add(folderObj.Restore(ItemId.Create(apidl[i]).GetData()));
				}
			    ContextMenuImpl impl = new ContextMenuImpl(folderObj, (IFolderObject[]) fos.ToArray(typeof (IFolderObject)));
				ppv = Marshal.GetComInterfaceForObject(impl, typeof(IContextMenu));
			}
            else if(riid.Equals(NsExtension.IID_IQueryAssociations))
            {
                //  If we've been asked for a query associations, it should only be for a single PIDL.
                if (apidl.Length != 1)
                {                    
                    ppv = IntPtr.Zero;
                    return WinError.E_FAIL;
                }
                //var item = GetChildItem(idLists[0]);
                var isFolder = true;//item is IShellNamespaceFolder;

                if (isFolder)
                {
                    //  todo perhaps a good class name would simply be the 
                    //  name of the item type? or an attribute that uses the classname as a 
                    //  fallback.
                    var associations = new ASSOCIATIONELEMENT[]
                    {
                        new ASSOCIATIONELEMENT
                            {
                                ac = ASSOCCLASS.ASSOCCLASS_PROGID_STR,
                                hkClass = IntPtr.Zero,
                                pszClass = "FolderViewSampleType"
                            },
                        new ASSOCIATIONELEMENT
                            {
                                ac = ASSOCCLASS.ASSOCCLASS_FOLDER,
                                hkClass = IntPtr.Zero,
                                pszClass = "FolderViewSampleType"
                            }
                    };
                    Shell32.AssocCreateForClasses(associations, (uint)associations.Length, riid, out ppv);

                }
                else
                {
                    var associations = new ASSOCIATIONELEMENT[]
                    {
                        new ASSOCIATIONELEMENT
                            {
                                ac = ASSOCCLASS.ASSOCCLASS_PROGID_STR,
                                hkClass = IntPtr.Zero,
                                pszClass = "FolderViewSampleType"
                            }
                    };
                    Shell32.AssocCreateForClasses(associations, (uint)associations.Length, riid, out ppv);
                }
            }
            else if (riid.Equals(NsExtension.IID_IQueryInfo))
            {
                IFolderObject obj = null;
                obj = folderObj.Restore(ItemId.Create(apidl[0]).GetData());
                QueryInfoImpl impl = new QueryInfoImpl(obj);
                ppv = Marshal.GetComInterfaceForObject(impl, typeof (IQueryInfo));
            }
            else if (riid.Equals(NsExtension.IID_IShellLinkW))
            {
                IShellLink link = (IShellLink)new ShellLink();
                IFolderObject obj = null;
                obj = folderObj.Restore(ItemId.Create(apidl[0]).GetData());
                IntPtr path = ItemIdList.Create(null, obj.PathData).Ptr;
                link.SetIDList(path);
                ppv = Marshal.GetComInterfaceForObject(link, typeof(IShellLink));
            }
            //  We have a set of child pidls (i.e. length one). We can now offer interfaces such as:
            /*
             * IContextMenu	The cidl parameter can be greater than or equal to one.
IContextMenu2	The cidl parameter can be greater than or equal to one.
IDataObject	The cidl parameter can be greater than or equal to one.
IDropTarget	The cidl parameter can only be one.
IExtractIcon	The cidl parameter can only be one.
IQueryInfo	The cidl parameter can only be one.
             * */

            //  IID_IExtractIconW
            //  IID_IDataObject
            //  IID_IQueryAssociations
            //  Currently, we don't offer any extra child item UI objects.

            /*
             else if (riid.Equals(NsExtension.IID_IShellLinkW))
             {
                 
             }*/
            else
            {
                Debug.WriteLine("GetUIObjectOf: " + riid);
                ppv = IntPtr.Zero;
                return WinError.E_NOINTERFACE;
                //throw new COMException();
            }
		    return WinError.S_OK;
		}

        public void GetDisplayNameOf(ItemIdList pidl, NameOptions uFlags, out STRRET pName)
        {   
			byte[] buf = pidl.GetItems()[0].GetData();
			IFolderObject obj = folderObj.Restore(buf, true);
            
			string name = obj.GetDisplayName(uFlags);

            if (uFlags == (NameOptions.ForAddressBar | NameOptions.ForParsing))
            {
                ItemIdList lst1 = ItemIdList.Create(null, folderObj.PathData[0]);
                ItemIdList lst2 = ItemIdList.Create(null, folderObj.PathData.Take(2).ToArray());

                string s1 = PidlManager.GetPidlDisplayName(lst1.Ptr);
                string s2 = PidlManager.GetPidlDisplayName(lst2.Ptr);

                Marshal.FreeCoTaskMem(lst1.Ptr);
                Marshal.FreeCoTaskMem(lst2.Ptr);

                name = string.Format("{0}\\{1}\\{2}", s1, s2, name);
            }
            
            pName = STRRET.CreateUnicode(name);
			//pName = StrRet.CreateDefault(name);
		}

		public void SetNameOf(IWin32Window hwnd, ItemIdList pidl, string pszName, NameOptions uFlags, ref IntPtr ppidlOut)
		{
			try
			{
				ArrayList relative = new ArrayList();
				IFolderObject fo = folderObj;
                
				foreach (byte[] data in pidl.GetItemData())
				{
					relative.Add(data);
					fo = fo.Restore(data);
				}

				byte[] oldId = (byte[]) relative[relative.Count-1];
				relative.RemoveAt(relative.Count - 1);

				fo.SetName(hwnd, pszName, uFlags);

				byte[] newId = fo.Persist();

				if (ppidlOut != IntPtr.Zero)
				{
					//using (Malloc m = Shell32.GetMalloc())
					{
						ArrayList newRelativePidl = new ArrayList(relative);
						newRelativePidl.Add(newId);
						ppidlOut = ItemIdList.Create(null, (byte[][]) newRelativePidl.ToArray(typeof(byte[]))).Ptr;
					}
				}

				byte[][] here = folderObj.PathData;

				ArrayList oldAbs = new ArrayList(here);
				oldAbs.AddRange(relative);
				oldAbs.Add(oldId);

				ArrayList newAbs = new ArrayList(here);
				newAbs.AddRange(relative);
				newAbs.Add(newId);

				//using (Malloc m = Shell32.GetMalloc())
				{
					ItemIdList oldPidl = ItemIdList.Create(null, (byte[][]) oldAbs.ToArray(typeof(byte[])));
                    ItemIdList newPidl = ItemIdList.Create(null, (byte[][])newAbs.ToArray(typeof(byte[])));
                    
                    Shell32.SHChangeNotify(
						((fo.Attributes & FolderAttributes.Folder) == FolderAttributes.Folder)
							? ShellChangeEvents.RenameFolder
							: ShellChangeEvents.RenameItem,
                        ShellChangeFlags.IdList | ShellChangeFlags.Flush,
						oldPidl.Ptr,
                        newPidl.Ptr);
                    Marshal.FreeCoTaskMem(oldPidl.Ptr);
                    Marshal.FreeCoTaskMem(newPidl.Ptr);

                    // refresh current folder
				    IntPtr pCur = ItemIdList.Create(null, folderObj.PathData).Ptr;
                    Shell32.SHChangeNotify(ShellChangeEvents.UpdateDir,
                        ShellChangeFlags.IdList | ShellChangeFlags.Flush,
                        pCur,
                        IntPtr.Zero);
                    Marshal.FreeCoTaskMem(pCur);
                    
                    
					//m.Free(oldPidl.Ptr);
					//m.Free(newPidl.Ptr);
				}
			}
			catch
			{
				ppidlOut = IntPtr.Zero;
			}
		}

		#endregion

		#region IShellFolder2 members

		/* IShellFolder2 */

		public int GetDefaultSearchGUID(out Guid pqguid)
		{
            pqguid = Guid.Empty;
            return WinError.E_NOTIMPL;
			//throw new NotImplementedException();
		}

		public int EnumSearches(out IEnumExtraSearch ppenum)
		{
            ppenum = null;
            return WinError.E_NOTIMPL;
			//throw new NotImplementedException();
		}

		public void GetDefaultColumn(uint dwRes, out ulong pSort, out ulong pDisplay)
		{
			ColumnCollection columns = folderObj.Columns;
			pSort = 0;
			pDisplay = 0;

			for (int i = 0; i < columns.Count; i++)
			{
				if (columns[i] == columns.DefaultDisplayColumn)
				{
					pDisplay = (ulong) i;
				}
				
				if (columns[i] == columns.DefaultSortColumn)
				{
					pSort = (ulong) i;
				}
			}
		}

		public int GetDefaultColumnState(uint iColumn, out ColumnStates pcsFlags)
		{
		    if ((int) iColumn < folderObj.Columns.Count)
		    {
                Column column = folderObj.Columns[(int)iColumn];
                pcsFlags = ColumnStates.None;

                if (column.Slow)
                {
                    pcsFlags |= ColumnStates.Slow;
                }

                if (column.DefaultVisible)
                {
                    pcsFlags |= ColumnStates.OnByDefault;
                }

                IList numericTypes = new ArrayList(new Type[]
			{
				typeof(sbyte),	typeof(byte),
				typeof(short),	typeof(ushort),
				typeof(int),	typeof(uint),
				typeof(long),	typeof(ulong),
				typeof(decimal)
			});

                if (numericTypes.Contains(column.Type))
                {
                    pcsFlags |= ColumnStates.TypeInt;
                }
                else if (column.Type == typeof(DateTime))
                {
                    pcsFlags |= ColumnStates.TypeDate;
                }
                else
                {
                    pcsFlags |= ColumnStates.TypeStr;
                }               
		    }
		    else
		    {
                pcsFlags = ColumnStates.None;
                return WinError.E_INVALIDARG;
		    }

            return WinError.S_OK;
		}

		public int GetDetailsEx(ItemIdList pidl, ref ColumnId pscid, out object pv)
		{
            if (pidl.Ptr == IntPtr.Zero)
            {
                pv = null;
                return WinError.E_INVALIDARG;
            }
		    IFolderObject obj = null;
            pv = GetItemColumnValue(pidl, pscid, ref obj);
            
            if (pscid.guid.Equals(Guid.Parse("c9944a21-a406-48fe-8225-aec7e24c211b")) && pscid.pid == 13)
		    {
               // pv = "prop:~System.ItemNameDisplay;~System.LayoutPattern.PlaceHolder;~System.LayoutPattern.PlaceHolder;~System.LayoutPattern.PlaceHolder;System.DateModified";
                //pv = "prop:~System.ItemNameDisplay;System.ItemTypeText;";
                pv = "prop:~System.ItemNameDisplay;~System.LayoutPattern.PlaceHolder;~System.LayoutPattern.PlaceHolder;~System.LayoutPattern.PlaceHolder;System.ItemTypeText";
		    }
            if (pscid.guid.Equals(Guid.Parse("c9944a21-a406-48fe-8225-aec7e24c211b")) && pscid.pid == 502)
            {
                pv = "";
            }             
            if (pscid.guid.Equals(Guid.Parse("c9944a21-a406-48fe-8225-aec7e24c211b")) && pscid.pid == 500)
            {
                pv = "delta";
            }

            if (pscid.guid.Equals(Guid.Parse("B725F130-47EF-101A-A5F1-02608C9EEBAC")) && pscid.pid == 14)
            {
                pv = "111";
            }
            if (pscid.guid.Equals(Guid.Parse("B725F130-47EF-101A-A5F1-02608C9EEBAC")) && pscid.pid == 4)
            {
                bool bFolder = ((obj.Attributes & FolderAttributes.Folder) == FolderAttributes.Folder);
                bool bLink = ((obj.Attributes & FolderAttributes.Link) == FolderAttributes.Link);
                if (bLink)
                {
                    pv = "Link";   
                }
                else
                {
                    if (bFolder)
                    {
                        pv = "Folder"; 
                    }
                    else
                    {
                        pv = "File"; 
                    }
                }   
            }
        //https://msdn.microsoft.com/en-us/library/windows/desktop/bb760783(v=vs.85).aspx
            return WinError.S_OK;
		}

        private string GetItemColumnValue(ItemIdList pidl, ColumnId propertyKey, ref IFolderObject obj)
        {
            obj = folderObj.Restore(pidl.GetItemData()[0]);
            
            object @value = obj.GetColumnValue(folderObj.Columns[0]);
            return @value == null ? string.Empty : @value.ToString();
        }

		public int GetDetailsOf(ItemIdList pidl, uint iColumn, out ShellDetails psd)
		{
		    if (folderObj.Columns.Count > 0 && (int) iColumn < folderObj.Columns.Count)
		    {
		        Column column = folderObj.Columns[(int) iColumn];
		        psd = new ShellDetails();
		        psd.cxChar = column.Width;
		        psd.fmt = column.Format;

		        if (pidl == null)
		        {
		            psd.str = STRRET.CreateUnicode(column.Name);
		        }
		        else
		        {
		            IFolderObject obj = folderObj.Restore(pidl.GetItemData()[0]);
		            object @value = obj.GetColumnValue(column);
		            psd.str = STRRET.CreateUnicode(@value == null ? string.Empty : @value.ToString());
		        }
		    }
		    else
		    {

		        ShellDetails sd = new ShellDetails();
		        sd.str = new STRRET {uType = STRRET.STRRETTYPE.STRRET_WSTR, data = IntPtr.Zero};
		        psd = sd;
		        return WinError.E_FAIL;

		        //throw new NotImplementedException();
		    }
		    return WinError.S_OK;
		}

		public int MapColumnToSCID(uint iColumn, ref ColumnId pscid)
		{
		    if ((int) iColumn < folderObj.Columns.Count)
		    {
                Column column = folderObj.Columns[(int)iColumn];

                //if (column.PropertyIdentifier < 0)
                {
                    //throw new ArgumentOutOfRangeException("iColumn", iColumn, "Column does not implement a property set");
                    //pscid = new ColumnId();
                 //   return WinError.E_FAIL;
                    //throw new ArgumentOutOfRangeException("iColumn", iColumn, "Column does not implement a property set");
                }

                pscid.guid = column.FormatIdentifier;
                pscid.pid = (uint)column.PropertyIdentifier;
                return WinError.S_OK;    
		    }
		    else
		    {
                return WinError.E_FAIL;
		    }			
		}

		#endregion

		#region IShellFolderViewCB members

		/* IShellFolderViewCB */

        int IShellFolderViewCB.MessageSFVCB(ShellFolderViewMessage uMsg, ref IntPtr wParam, ref IntPtr lParam, ref IntPtr plResult)
		{
            //Debug.WriteLine("MessageSFVCB: " + uMsg);
			switch (uMsg)
			{
				case ShellFolderViewMessage.GetNotify:
					//using (Malloc m = Shell32.GetMalloc())
			    {
			        wParam = /*PidlManager.IdListToPidl(folderObj.IdList);//*/ItemIdList.Create(null, folderObj.PathData).Ptr;
                    lParam = new IntPtr((int)(ShellChangeEvents.RenameFolder | ShellChangeEvents.RenameItem | ShellChangeEvents.UpdateDir ));
					}
					break;
                    
                case ShellFolderViewMessage.FsNotify:
                    //using (Malloc m = Shell32.GetMalloc())
			    {
                    wParam = /*PidlManager.IdListToPidl(folderObj.IdList);*/ItemIdList.Create(null, folderObj.PathData).Ptr;
                        //lParam = new IntPtr((int)(ShellChangeEvents.RenameFolder | ShellChangeEvents.RenameItem));
                    }
                    break;
                case ShellFolderViewMessage.GetButtons:
			    {
                    TBBUTTON  tb = new TBBUTTON();
			        break;
			    }
				default:
					//throw new NotImplementedException();                    
                    return WinError.E_NOTIMPL;
			}
            return WinError.S_OK;
		}

		#endregion

		#region IRemoteComputer Members

		void IRemoteComputer.Initialize(string pszMachine, bool bEnumerating)
		{
			folderObj.RemoteComputer = pszMachine;
		}

		#endregion

	    public void DragEnter(IntPtr hwndTarget, IDataObject dataObject, ref Win32Point pt, int effect)
	    {
	        MessageBox.Show("1");
            throw new NotImplementedException();
	    }

	    public void DragLeave()
	    {
            MessageBox.Show("1");
	        throw new NotImplementedException();
	    }

	    public void DragOver(ref Win32Point pt, int effect)
	    {
            MessageBox.Show("1");
	        throw new NotImplementedException();
	    }

	    public void Drop(IDataObject dataObject, ref Win32Point pt, int effect)
	    {
            MessageBox.Show("1");
	        throw new NotImplementedException();
	    }

	    public void Show(bool show)
	    {
            MessageBox.Show("1");
	        throw new NotImplementedException();
	    }

	    public void InitializeFromBitmap(ref ShDragImage dragImage, IDataObject dataObject)
	    {
            MessageBox.Show("1");
	        throw new NotImplementedException();
	    }

	    public void InitializeFromWindow(IntPtr hwnd, ref Win32Point pt, IDataObject dataObject)
	    {
            MessageBox.Show("1");
	        throw new NotImplementedException();
	    }

        public int GetInfoTip(QITIPF dwFlags, out string ppwszTip)
        {
            throw new NotImplementedException();
        }

        public int GetInfoFlags(out int pdwFlags)
        {
            throw new NotImplementedException();
        }



        public int DragEnter(IDataObject pDataObj, int grfKeyState, Win32Point pt, ref int pdwEffect)
        {
            //throw new NotImplementedException();
            return WinError.S_OK;
        }

        public int DragOver(int grfKeyState, Win32Point pt, ref int pdwEffect)
        {
            //throw new NotImplementedException();
            return WinError.S_OK;
        }

        int DragDrop.IDropTarget.DragLeave()
        {
            //throw new NotImplementedException();
            return WinError.S_OK;
        }

        public int Drop(IDataObject pDataObj, int grfKeyState, Win32Point pt, ref int pdwEffect)
        {
            throw new NotImplementedException();
        }

        void IPersistFile.GetClassID(out Guid pClassID)
        {
            throw new NotImplementedException();
        }

        public int IsDirty()
        {
            throw new NotImplementedException();
        }

        public void Load(string pszFileName, int dwMode)
        {
            //throw new NotImplementedException();
            folderObj.PathString = pszFileName;
            Debug.WriteLine(pszFileName);
        }

        public void Save(string pszFileName, bool fRemember)
        {
            throw new NotImplementedException();
        }

        public void SaveCompleted(string pszFileName)
        {
            throw new NotImplementedException();
        }

        public void GetCurFile(out IntPtr ppszFileName)
        {
            throw new NotImplementedException();
        }

       

        public void Initialize(IntPtr pidlFolder, IntPtr pDataObj, IntPtr hKeyProgID)
        {
            string folderPath = string.Empty;
            if (pidlFolder != IntPtr.Zero)
            {                
                var stringBuilder = new StringBuilder(260);
                if (User32.SHGetPathFromIDListW(pidlFolder, stringBuilder))
                {
                    //  Set parent folder path.
                    folderPath = stringBuilder.ToString();

                    ItemIdList itmId = ItemIdList.Create(pidlFolder);
                    folderObj.SetFullPath(itmId.GetItemData());
                    folderObj.SetPath(itmId.GetItemData());
                    folderObj.PathString = folderPath;

                    if (folderObj.MenuItems != null)
                    {
                        List<ShellMenuItem> menuItems = new List<ShellMenuItem>();
                        ShellMenuItem smiLink = new ExploreMenuItem(folderObj, folderObj.PathData, "ADB - Create Screenshot", "ADB - Create Screenshot", ExploreMenuItem.CreateScreenshotVerb, null);
                        smiLink.Bitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject("screenshot");
                        menuItems.Add(smiLink);                        
                        folderObj.MenuItems = menuItems.ToArray();  
                    }
                    //Marshal.FreeCoTaskMem(itmId.Ptr);
                }
            }
            if (pDataObj != IntPtr.Zero)
            {
                //  Create the IDataObject from the provided pDataObj.
                var dataObjectIn = (IDataObject)Marshal.GetObjectForIUnknown(pDataObj);
                List<string> files = DataObjectHelper.GetFiles(dataObjectIn);

                DataObject dataObject = new DataObject();
                List<string> file_list = new List<string>();
                StringCollection sc = new StringCollection();
                foreach (string sf in files)
                {                   
                    file_list.Add(sf);
                    sc.Add(sf);
                }                
                dataObject.SetData(DataFormats.FileDrop, (System.String[])file_list.ToArray());
                dataObject.SetData(typeof(StringCollection), sc);
                folderObj.DataObject = dataObject;

                //context menu on file
                if (pidlFolder == IntPtr.Zero && files.Count == 1 && !files[0].StartsWith("[virtual"))
                {
                    folderObj.PathString = files[0];
                    IntPtr pidlList = Shell32.ILCreateFromPathW(folderObj.PathString);
                    if (pidlList != IntPtr.Zero)
                    {
                        try
                        {
                            ItemIdList itmId = ItemIdList.Create(pidlList);
                            folderObj.SetFullPath(itmId.GetItemData());
                            folderObj.SetPath(itmId.GetItemData());


                            List<ShellMenuItem> menuItems = new List<ShellMenuItem>();
                            bool bFile = false;
                            bool bDir = false;
                            FileInfo fi = new FileInfo(files[0]);
                            if (fi.Exists)
                            {
                                bFile = true;
                            }
                            DirectoryInfo di = new DirectoryInfo(files[0]);
                            if (di.Exists)
                            {
                                bDir = true;
                            }

                            if (bFile && files[0].ToLower().EndsWith(".apk"))
                            {
                                ShellMenuItem smiLink = new ExploreMenuItem(folderObj, folderObj.PathData, "ADB - Install APK", "ADB - Install APK", ExploreMenuItem.InstallApkVerb, null);
                                smiLink.Bitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject("android1");
                                menuItems.Add(smiLink);
                            }
                            if (bDir)
                            {
                                ShellMenuItem smiLink = new ExploreMenuItem(folderObj, folderObj.PathData, "ADB - Create Screenshot", "ADB - Create Screenshot", ExploreMenuItem.CreateScreenshotVerb, null);
                                smiLink.Bitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject("screenshot");
                                menuItems.Add(smiLink);
                            }

                            folderObj.MenuItems = menuItems.ToArray();
                        }
                        finally
                        {
                            Shell32.ILFree(pidlList);
                        }   
                    }
                    else
                    {
                        //may by this folder
                        List<byte[][]> lsPidls = DataObjectHelper.GetPidls(dataObjectIn);
                        if (lsPidls.Count == 1)
                        {
                            Guid g;
                            int n = GetClassID(out g);
                            folderObj.SetFullPath(lsPidls[0]);
                            folderObj.SetPath(lsPidls[0]);
                            if (folderObj.MenuItems != null)
                            {
                                folderObj.MenuItems = new ShellMenuItem[0];
                            }
                        }
                    }                   
                }

                if (pidlFolder != IntPtr.Zero && !string.IsNullOrEmpty(folderPath) && !folderPath.StartsWith("[virtual") && files.Count > 0 && files[0].StartsWith("[virtual"))
                {                    
                    List<ShellMenuItem> menuItems = new List<ShellMenuItem>();
                    ShellMenuItem smiLink = new ExploreMenuItem(folderObj, folderObj.PathData, "Create Link", "Create Link", ExploreMenuItem.PasteLinkVerb, null);
                    smiLink.Bitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject("link");
                    ShellMenuItem smiLinkC = new ExploreMenuItem(folderObj, folderObj.PathData, "Copy", "Copy", ExploreMenuItem.PasteVerb, null);
                    smiLinkC.Bitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject("copy1");
                    smiLinkC.Default = true;
                    menuItems.Add(smiLinkC);
                    menuItems.Add(smiLink);
                    
                    folderObj.MenuItems = menuItems.ToArray();                    

                    

                    //files.Add(folderPath);
                    //folderObj.CopyItems(null, files);    
                }                
                //  Add the set of files to the selected file paths.
                //selectedItemPaths = dataObject.GetFileList();
            }
            //throw new NotImplementedException();
        }

        public int QueryContextMenu(IntPtr hmenu, uint indexMenu, uint idCmdFirst, uint idCmdLast, ContextMenuOptions uFlags)
        {
            bool bNormal = (uFlags & ContextMenuOptions.Normal) == ContextMenuOptions.Normal;
            bool bDefaultOnly = (uFlags & ContextMenuOptions.DefaultOnly) == ContextMenuOptions.DefaultOnly;
            bool bVerbsOnly = (uFlags & ContextMenuOptions.VerbsOnly) == ContextMenuOptions.VerbsOnly;
            bool bExplore = (uFlags & ContextMenuOptions.Explore) == ContextMenuOptions.Explore;
            bool bNoVerbs = (uFlags & ContextMenuOptions.NoVerbs) == ContextMenuOptions.NoVerbs;
            bool bCanRename = (uFlags & ContextMenuOptions.CanRename) == ContextMenuOptions.CanRename;
            bool bNoDefault = (uFlags & ContextMenuOptions.NoDefault) == ContextMenuOptions.NoDefault;
            bool bIncludeStatic = (uFlags & ContextMenuOptions.IncludeStatic) == ContextMenuOptions.IncludeStatic;
            bool bExtendedVerbs = (uFlags & ContextMenuOptions.ExtendedVerbs) == ContextMenuOptions.ExtendedVerbs;
            bool bReserved = (uFlags & ContextMenuOptions.Reserved) == ContextMenuOptions.Reserved;

            Debug.WriteLine(string.Format("QueryContextMenu: \r\nNormal:{0}\r\nDefaultOnly:{1}\r\nbVerbsOnly:{2}\r\nExplore:{3}\r\nNoVerbs:{4}\r\nCanRename:{5}\r\nNoDefault:{6}\r\nIncludeStatic:{7}\r\nExtendedVerbs:{8}\r\nReserved:{9}", bNormal, bDefaultOnly, bVerbsOnly, bExplore, bNoVerbs, bCanRename, bNoDefault, bIncludeStatic, bExtendedVerbs, bReserved));

            try
            {
                if (folderObj.MenuItems != null)
                {
                    if (folderObj.MenuItems.Count() == 2) //delete only if move
                    {
                        int items = User32.GetMenuItemCount(hmenu);
                        for (int i = 0; i < items; i++)
                        {
                            User32.RemoveMenu(hmenu, /*(uint)i*/0, User32.MF_BYPOSITION);
                        }
                    }

                    MenuOrderEnumerator m = new MenuOrderEnumerator(folderObj.MenuItems, uFlags);

                    while (m.MoveNext())
                    {
                        ((ShellMenuItem)m.Current).CreateMenuItem(hmenu, indexMenu++, (int)idCmdFirst + m.CurrentId, m.CurrentIsDefault & !bNoDefault, true);
                    }

                    return folderObj.MenuItems.Count();
                }       
            }
            catch (Exception)
            {
                
            }                 
            
            return WinError.S_OK;
        }

        public void InvokeCommand(ref CommandInfo lpici)
        {
            if (folderObj.MenuItems != null)
            {
                if (User32.IsIntResource(lpici.lpVerb))
                {
                    int id = (int)lpici.lpVerb;
                    if (id < folderObj.MenuItems.Count())
                    {
                        folderObj.MenuItems[id].PerformClick(Win32Window.Create(lpici.hwnd), lpici);
                    }
                }
                else
                {
                    string verb = Marshal.PtrToStringAnsi(lpici.lpVerb);
                    for (int id = 0; id < folderObj.MenuItems.Length; id++)
                    {
                        if (folderObj.MenuItems[id].Verb == verb)
                        {
                            folderObj.MenuItems[id].PerformClick(Win32Window.Create(lpici.hwnd), lpici);
                        }
                    }
                }   
            }            
            
            //throw new NotImplementedException();
        }

        public void GetCommandString(int idCmd, CommandStringOptions uFlags, IntPtr pwReserved, byte[] pszName, uint cchMax)
        {
            if (idCmd > folderObj.MenuItems.Length - 1)
            {
                idCmd = 0;
                //return;
            }


            ShellMenuItem menuItem = folderObj.MenuItems[idCmd];
            string text;

            switch (uFlags)
            {
                case CommandStringOptions.HelpTextA:
                case CommandStringOptions.HelpTextW:
                    text = menuItem.HelpText;
                    break;

                case CommandStringOptions.VerbA:
                case CommandStringOptions.VerbW:
                    text = menuItem.Verb;
                    break;

                case CommandStringOptions.ValidateA:
                case CommandStringOptions.ValidateW:
                    if (idCmd < 0 || idCmd >= folderObj.MenuItems.Length)
                    {
                        Marshal.ThrowExceptionForHR(1); // S_FALSE
                    }
                    throw new Exception(); // unreachable

                default:
                    throw new ArgumentOutOfRangeException("uFlags", uFlags.ToString());
            }

            if (text == null)
            {
                text = string.Empty;
            }

            byte[] buf;

            if ((uFlags & CommandStringOptions.Unicode) == CommandStringOptions.Unicode)
            {
                buf = Encoding.Unicode.GetBytes(text);
            }
            else
            {
                buf = Encoding.ASCII.GetBytes(text);
            }

            int cch = Math.Min(buf.Length, pszName.Length - 1);

            if (cch > 0)
            {
                Array.Copy(buf, 0, pszName, 0, cch);
            }
            else
            {
                // null terminate the buffer
                pszName[0] = 0;
            }
        }       
    }
}
