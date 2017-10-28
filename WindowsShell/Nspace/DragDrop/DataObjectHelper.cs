using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows.Forms;
using WindowsShell.Interop;
using MyData.Extensions;

namespace WindowsShell.Nspace.DragDrop
{
    public static class DataObjectHelper
    {
        public static List<byte[][]> GetPidls(System.Runtime.InteropServices.ComTypes.IDataObject pDataObj)
        {
            List<byte[][]> lsRet = new List<byte[][]>();

            try
            {
                STGMEDIUM medium;
                System.Runtime.InteropServices.ComTypes.FORMATETC formatShell = new System.Runtime.InteropServices.ComTypes.FORMATETC()
                {
                    cfFormat = (Int16)DataFormats.GetFormat(NativeMethods.CFSTR_SHELLIDLIST).Id,
                    dwAspect = DVASPECT.DVASPECT_CONTENT,
                    tymed = TYMED.TYMED_HGLOBAL
                };
                System.Runtime.InteropServices.ComTypes.FORMATETC format = new System.Runtime.InteropServices.ComTypes.FORMATETC()
                {
                    cfFormat = (Int16)DataFormats.GetFormat(NativeMethods.CF_HDROP).Id,
                    dwAspect = DVASPECT.DVASPECT_CONTENT,
                    tymed = TYMED.TYMED_HGLOBAL
                };
                int ret = pDataObj.QueryGetData(ref format);
                if (ret > 0)
                {
                    pDataObj.GetData(ref formatShell, out medium);

                    IntPtr ptr;
                    ptr = medium.unionmember;

                    IntPtr ptrLock = NativeMethods.GlobalLock(ptr);

                    lsRet = ProcessCIDA_Pidl(ptr);

                    NativeMethods.GlobalUnlock(ptrLock);
                }
            }
            catch (Exception)
            {
                
            }            

            return lsRet;
        }

        public static List<string> GetFiles(System.Runtime.InteropServices.ComTypes.IDataObject pDataObj)
        {
            System.Runtime.InteropServices.ComTypes.FORMATETC formatV = new System.Runtime.InteropServices.ComTypes.FORMATETC()
            {
                cfFormat = (Int16)DataFormats.GetFormat(NativeMethods.CFSTR_FILEDESCRIPTORW).Id,
                dwAspect = DVASPECT.DVASPECT_CONTENT,
                tymed = TYMED.TYMED_HGLOBAL
            };
            System.Runtime.InteropServices.ComTypes.FORMATETC formatShell = new System.Runtime.InteropServices.ComTypes.FORMATETC()
            {
                cfFormat = (Int16)DataFormats.GetFormat(NativeMethods.CFSTR_SHELLIDLIST).Id,
                dwAspect = DVASPECT.DVASPECT_CONTENT,
                tymed = TYMED.TYMED_HGLOBAL
            };
            System.Runtime.InteropServices.ComTypes.FORMATETC format = new System.Runtime.InteropServices.ComTypes.FORMATETC()
            {
                cfFormat = (Int16)DataFormats.GetFormat(NativeMethods.CF_HDROP).Id,
                dwAspect = DVASPECT.DVASPECT_CONTENT,
                tymed = TYMED.TYMED_HGLOBAL
            };

            STGMEDIUM medium;

            bool bVirtualData = true;
            bool bVirtualShell = false;
            try
            {
                pDataObj.GetData(ref formatV, out medium);
            }
            catch (Exception)
            {
                if (pDataObj is System.Windows.Forms.DataObject)
                {
                    List<string> files1 = new List<string>();
                    //medium = new STGMEDIUM();
                    System.Windows.Forms.DataObject dob = (System.Windows.Forms.DataObject)pDataObj;
                    StringCollection z = (StringCollection)dob.GetData(typeof(StringCollection));
                    foreach (string s in z)
                    {
                        files1.Add(s);
                    }
                    /*
                    if (dob.ContainsFileDropList())
                    {                        
                        StringCollection c = dob.GetFileDropList();
                        foreach (string s in c)
                        {
                            files1.Add(s);
                        }                        
                    }
                    else
                    {
                        string objFormat = dob.GetFormats().FirstOrDefault(x => x.Equals("FileDrop"));
                        if (!string.IsNullOrEmpty(objFormat))
                        {
                            object obj = dob.GetData("FileDrop");
                            if (obj is string)
                                files1.Add((string)obj);
                        }                        
                    }
                     */
                    return files1;
                }
                else
                {
                    
                    int ret = pDataObj.QueryGetData(ref format);
                    if (ret > 0)
                    {
                        bVirtualShell = true;

                        Debug.WriteLine("!!!!SPECIAL FOLDER!!!!");
                        pDataObj.GetData(ref formatShell, out medium); 
                    }                    
                    else
                    {
                        try
                        {
                            pDataObj.GetData(ref format, out medium);      
                        }
                        catch (Exception)
                        {
                            bVirtualShell = true;

                            Debug.WriteLine("!!!!SPECIAL FOLDER!!!!");
                            pDataObj.GetData(ref formatShell, out medium);
                        }                        
                    }
                    
                    /*
                    if (ret != 0)
                    {                                               
                        Debug.WriteLine("------- GET FORMATS -------");
                        IEnumFORMATETC en = pDataObj.EnumFormatEtc(DATADIR.DATADIR_GET);
                        System.Runtime.InteropServices.ComTypes.FORMATETC[] fc = new System.Runtime.InteropServices.ComTypes.FORMATETC[10];
                        int[] fetched = new int[10];
                        int ret1 = en.Next(10, fc, fetched);
                        for (int i = 0; i < fetched[0]; i++)
                        {
                            DataFormats.Format f = DataFormats.GetFormat(fc[i].cfFormat);

                            string sn = f.Name;
                            Debug.WriteLine("FORMAT: " + sn);
                        }
                        Debug.WriteLine("---------------------------");

                        pDataObj.GetData(ref formatShell, out medium);                        
                        IntPtr ptr1;
                        ptr1 = medium.unionmember;
                        IntPtr ptrLock = NativeMethods.GlobalLock(ptr1);
                        

                        List<IntPtr> lsUnk = ProcessCIDA(ptrLock);
                        foreach (IntPtr intPtr in lsUnk)
                        {
                            foreach (Environment.SpecialFolder suit in Enum.GetValues(typeof(Environment.SpecialFolder)))
                            {
                                var desktopPIDL = IntPtr.Zero;
                                int r = Shell32.SHGetSpecialFolderLocation(IntPtr.Zero, (int)suit, ref desktopPIDL);

                                string spath = Environment.GetFolderPath(suit);
                               

                                string s1 = PidlManager.GetPidlDisplayName(desktopPIDL);
                                string s2 = PidlManager.GetPidlDisplayName(intPtr);
                                
                                bool be = Shell32.ILIsEqual(intPtr, desktopPIDL);
                                if (be || s1.Equals(s2))
                                {
                                    ItemIdList iptm = ItemIdList.Create(desktopPIDL);
                                    byte[][] b = iptm.GetItemData();
                                    string s = "finded!!!:" + suit;

                                    ItemIdList iptm1 = ItemIdList.Create(intPtr);
                                    byte[][] b1 = iptm1.GetItemData();
                                }
                            }

                            Shell32.ILFree(intPtr);  
                        }                      

                        IntPtr ptrUnLock = NativeMethods.GlobalUnlock(ptrLock);

                    }
                    else
                    {
                        pDataObj.GetData(ref format, out medium);       
                    }
                    */
                    
                }
                
                bVirtualData = false;
            }

            IntPtr ptr;
            ptr = medium.unionmember;

            List<string> files = new List<string>();

            if (bVirtualData)
            {
                GetVirtualFileNamesFromDataObject(ptr, ref files);
            }
            else
            {
                if (!bVirtualShell)
                {
                    GetFileNamesFromDataObject(ptr, ref files);   
                }
                else
                {
                    GetSystemVirtualFileNamesFromDataObject(ptr, ref files);   
                }
            }

            NativeMethods.ReleaseStgMedium(ref medium);

           
            return files;
        }

        private static List<byte[][]> ProcessCIDA_Pidl(IntPtr p)
        {
            List<byte[][]> lsret = new List<byte[][]>();

            UInt32 cidl = (UInt32)Marshal.ReadInt32(p);            
            int offset = sizeof(UInt32);
            IntPtr parentpidl = (IntPtr)((int)p + (UInt32)Marshal.ReadInt32(p, offset));

            for (int i = 1; i <= cidl; ++i)
            {
                offset += sizeof(UInt32);
                IntPtr relpidl = (IntPtr)((int)p + (UInt32)Marshal.ReadInt32(p, offset));
                IntPtr abspidl = Shell32.ILCombine(parentpidl, relpidl);
                IdList idl = PidlManager.PidlToIdlist(abspidl);
                lsret.Add(idl.Ids.Select(x=>x.RawId).ToArray());
                Shell32.ILFree(abspidl);                
            }

            return lsret;
        }

        private static List<string> ProcessCIDA(IntPtr p)
        {            
            List<string> lFiles = new List<string>();
            // Get number of items.
            UInt32 cidl = (UInt32)Marshal.ReadInt32(p);
            // Get parent folder.
            int offset = sizeof(UInt32);
            IntPtr parentpidl = (IntPtr)((int)p + (UInt32)Marshal.ReadInt32(p, offset));
            StringBuilder path = new StringBuilder(256);
            bool br = User32.SHGetPathFromIDListW(parentpidl, path);
            string sParrent = PidlManager.GetPidlDisplayName(parentpidl);                  
            
            // Get subitems.
            for (int i = 1; i <= cidl; ++i)
            {
                offset += sizeof(UInt32);
                IntPtr relpidl = (IntPtr)((int)p + (UInt32)Marshal.ReadInt32(p, offset));
                IntPtr abspidl = Shell32.ILCombine(parentpidl, relpidl);
                bool br1 = User32.SHGetPathFromIDListW(abspidl, path);                
                string sFile = PidlManager.GetPidlDisplayName(abspidl);                  
                                
                Shell32.ILFree(abspidl);
                if (br1)
                {
                    lFiles.Add(path.ToString());
                }
                else
                {
                    lFiles.Add(string.Format("{0}\\{1}", sParrent, sFile));   
                }                
            }
            return lFiles;
        }

        private static void GetSystemVirtualFileNamesFromDataObject(IntPtr ptr, ref List<string> files)
        {
            IntPtr ptrLock = NativeMethods.GlobalLock(ptr);

            files = ProcessCIDA(ptr);

            NativeMethods.GlobalUnlock(ptrLock);
        }

        private static void GetVirtualFileNamesFromDataObject(IntPtr ptr, ref List<string> files)
        {
            IntPtr ptrLock = NativeMethods.GlobalLock(ptr);

            int offset = 0;
            int fileCount = Marshal.ReadInt32(ptrLock, offset);
            IntPtr ptrParse = new IntPtr(ptrLock.ToInt32() + sizeof(Int32));
            for (int i = 0; i < fileCount; i++)
            {
                DataObjectEx.FILEDESCRIPTOR des = (DataObjectEx.FILEDESCRIPTOR)Marshal.PtrToStructure(ptrParse, typeof(DataObjectEx.FILEDESCRIPTOR));
                files.Add(des.cFileName);
                if (i < fileCount)
                    ptrParse = new IntPtr(ptrParse.ToInt32() + Marshal.SizeOf(des));
            }

            NativeMethods.GlobalUnlock(ptrLock);
        }

        private static void GetFileNamesFromDataObject(IntPtr ptr, ref List<string> files)
        {
            int fileCount = NativeMethods.DragQueryFile(new HandleRef(null, ptr), -1, null, 0);
            try
            {
                for (int x = 0; x < fileCount; ++x)
                {
                    int size = NativeMethods.DragQueryFile(new HandleRef(null, ptr), x, null, 0);
                    if (size > 0)
                    {
                        StringBuilder fileName = new StringBuilder(size + 1);
                        if (
                            NativeMethods.DragQueryFile(new HandleRef(null, ptr), x, fileName, fileName.Capacity) >
                            0)
                            files.Add(fileName.ToString());
                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
