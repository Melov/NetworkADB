using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WindowsShell.ADB;

namespace WindowsShell.Nspace
{
    public class FileOperation
    {
        public string Source;
        public string Destination;
        public bool IsFolder;
        public bool IsFolderEmpty;
        public string Command;
    }

    /*
    public abstract class FileSystemManager
    {
        public virtual void SetFileName(IFolderObject folderObj, string sName)
        {
            string sOldFile = folderObj.PathString;                       
            File.Move(string.Format("{0}\\{1}", GetBaseFolder(), sOldFile), string.Format("{0}\\{1}", GetBaseFolder(), sName));            
        }

        public virtual void SetFolderName(IFolderObject folderObj, string sName)
        {
            string sOldFile = folderObj.PathString;
            Directory.Move(string.Format("{0}\\{1}", GetBaseFolder(), sOldFile), string.Format("{0}\\{1}", GetBaseFolder(), sName));
        }

        public virtual string CreateNewFolder(string path, string dirName)
        {
            path = string.Format("{0}\\{1}", GetBaseFolder(), path);
            string name = dirName;
            string current = name;
            int i = 0;
            while (Directory.Exists(Path.Combine(path, current)))
            {
                i++;
                current = String.Format("{0} {1}", name, i);
            }
            string sret = Path.Combine(path, current);
            Directory.CreateDirectory(sret);
            return current;
        }

        public virtual void DeleteItems(List<string> lsDirs, List<string> lsFiles)
        {
            foreach (string sdir in lsDirs)
            {
                Directory.Delete(sdir, true);
            }
            foreach (string sfile in lsFiles)
            {
                File.Delete(sfile);
            }
        }

        public virtual string[] GetFilesFromFolder(string folder)
        {
            if (!Directory.Exists(folder))
            {
                return null;
            }
            List<string> sFileNames = new List<string>();
            string[] sFiles = Directory.GetFiles(folder);
            foreach (string sFile in sFiles)
            {
                FileInfo fi = new FileInfo(sFile);
                sFileNames.Add(fi.Name);
            }
            return sFileNames.ToArray();
        }

        public virtual string[] GetFoldersFromFolder(string folder)
        {
            List<string> sFileNames = new List<string>();
            if (!Directory.Exists(folder))
            {
                return null;
            }
            string[] sDirs = Directory.GetDirectories(folder);
            foreach (string sDir in sDirs)
            {
                DirectoryInfo di = new DirectoryInfo(sDir);
                if (di.Exists)
                {
                    sFileNames.Add(di.Name);   
                }                
            }
            return sFileNames.ToArray();
        }

        public virtual string[] GetFilesFromFolder()
        {
            return GetFilesFromFolder(GetBaseFolder());
        }

        public virtual string[] GetFoldersFromFolder()
        {
            return GetFoldersFromFolder(GetBaseFolder());
        }

        public virtual void Copy(string sDestination, List<string> lItems, ref List<string> folders, ref List<string> files)
        {
            bool bRealFolder = false;
            
            DirectoryInfo diF = new DirectoryInfo(sDestination);
            if (diF.Exists)
            {
                bRealFolder = true;
            }
            
            string sDestinationFolder = bRealFolder ? sDestination : string.Format("{0}\\{1}", GetBaseFolder(), sDestination);

            foreach (string item in lItems)
            {
                string sName = item.Substring(item.LastIndexOf('\\') + 1);

                if (item.StartsWith("[virtualfolder]") || item.StartsWith("[virtualfile]"))
                {
                    if (item.StartsWith("[virtualfolder]"))
                    {
                        Directory.CreateDirectory(string.Format("{0}\\{1}", sDestinationFolder, sName));
                        folders.Add(sName);
                    }
                    if (item.StartsWith("[virtualfile]"))
                    {
                        File.WriteAllText(string.Format("{0}\\{1}", sDestinationFolder, sName),"");
                        files.Add(sName);
                    }
                }
                else
                {
                    DirectoryInfo diDest = new DirectoryInfo(sDestinationFolder);
                    if (!diDest.Exists)
                    {
                        Directory.CreateDirectory(sDestinationFolder);
                    }

                    FileInfo fi = new FileInfo(item);
                    bool bFile = fi.Exists;
                    DirectoryInfo di = new DirectoryInfo(item);
                    bool bDir = di.Exists;
                    if (bFile)
                    {
                        File.Copy(item, string.Format("{0}\\{1}", sDestinationFolder, sName), true);
                        files.Add(sName);
                    }
                    else
                    {
                        if (bDir)
                        {
                            Directory.CreateDirectory(string.Format("{0}\\{1}", sDestinationFolder, sName));
                            folders.Add(sName);
                        }
                    }                   
                }                
            }
        }

        public virtual string GetBaseFolder()
        {
            return @"d:";
        }
    }
    */


    public abstract class FileSystemManager
    {
        public static string FD = "/";

        public virtual void SetFileName(IFolderObject folderObj, string sName)
        {
            string sOldFile = folderObj.PathString;
            string sOld = string.Format("{0}/{1}", GetBaseFolder(), sOldFile);
            string sNew = string.Format("{0}/{1}", GetBaseFolder(), sName);

            ADBCommand command = new ADBCommand();
            CommandResult rez = command.Rename(sOld, sNew);
        }

        public virtual void SetFolderName(IFolderObject folderObj, string sName)
        {
            string sOldFile = folderObj.PathString;
            string sOld = string.Format("{0}/{1}", GetBaseFolder(), sOldFile);
            string sNew = string.Format("{0}/{1}", GetBaseFolder(), sName);

            ADBCommand command = new ADBCommand();
            CommandResult rez = command.Rename(sOld, sNew);
        }

        public virtual string CreateNewFolder(string path, string dirName)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = GetBaseFolder();
            }
            else
            {
                path = string.Format("{0}/{1}", GetBaseFolder(), path);    
            }
            
            string name = dirName;
            string current = name;
            int i = 0;

            FileObject[] fo = GetFoldersFromFolder(string.Format("{0}/.", path).Replace("//","/"));

            while (fo.ToList().FirstOrDefault(x => x.Name == current) != null)
            {
                i++;
                current = String.Format("{0} {1}", name, i);
            }

            string sret = path.EndsWith("/") ? (path + current) : (path + "/" + current);

            ADBCommand command = new ADBCommand();
            CommandResult rez = command.CreateDirectory(sret);

            if (!rez.IsSuccess)
            {
                rez.ShowMessage();
                current = null;
            }

            return current;
        }

        public virtual void DeleteItems(List<string> lsDirs, List<string> lsFiles)
        {
            /*
            foreach (string dir in lsDirs)
            {
                ADBCommand command = new ADBCommand();
                CommandResult rez = command.DeleteObject(dir);    
            }
            foreach (string file in lsFiles)
            {
                ADBCommand command = new ADBCommand();
                CommandResult rez = command.DeleteObject(file);    
            } 
            */
            List<FileOperation> lstItems = new List<FileOperation>();
            foreach (string dir in lsDirs)
            {
                FileOperation fo = new FileOperation();
                fo.Source = dir;
                lstItems.Add(fo);
            }
            foreach (string file in lsFiles)
            {
                FileOperation fo = new FileOperation();
                fo.Source = file;
                lstItems.Add(fo);
            }
            ADBCommand command = new ADBCommand();
            CommandResult rez = command.DeleteObjects(lstItems, true);    
        }

        public virtual FileObject[] GetFilesFromFolder(string folder)
        {
            List<FileObject> sFileNames = new List<FileObject>();
           
            ADBCommand command = new ADBCommand();
            CommandResult rez = command.ListDirectory(folder);
            if (rez.IsSuccess)
            {
                CommandResultHelper helper = new CommandResultHelper();
                sFileNames = helper.GetItems(rez.Message, enGetFileType.FilesOnly);
            }
   
            return sFileNames.ToArray();
        }

        public virtual FileObject[] GetFoldersFromFolder(string folder)
        {
            List<FileObject> sFileNames = new List<FileObject>();            
          
            ADBCommand command = new ADBCommand();
            CommandResult rez = command.ListDirectory(folder);
            if (rez.IsSuccess)
            {
                CommandResultHelper helper = new CommandResultHelper();
                sFileNames = helper.GetItems(rez.Message, enGetFileType.FoldersOnly);
            }

            return sFileNames.ToArray();
        }

        public virtual FileObject[] GetAllFromFolder(string folder, ref string error)
        {
            List<FileObject> sFileNames = new List<FileObject>();
            /*
            FileObject fo = new FileObject();
            fo.Name = "test";
            fo.IsFolder = true;
            fo.Attr = "-rwxrwx--x";
            fo.Perm1 = "root";
            fo.IsLink = true;
            sFileNames.Add(fo);

            FileObject fo1 = new FileObject();
            fo1.Name = "test1";
            fo1.IsFolder = true;

            sFileNames.Add(fo1);

            FileObject fo2 = new FileObject();
            fo2.Name = "test.txt";
            fo2.IsFolder = false;
            fo2.Size = "100";
            sFileNames.Add(fo2);

            FileObject fo3 = new FileObject();
            fo3.Name = "test1.doc";
            fo3.IsFolder = false;
            fo2.Size = "200";
            fo2.IsLink = true;
            sFileNames.Add(fo3);
            */
            ADBCommand command = new ADBCommand();
            CommandResult rez = command.ListDirectory(folder);
            if (rez.IsSuccess)
            {
                try
                {
                    CommandResultHelper helper = new CommandResultHelper();
                    sFileNames = helper.GetItems(rez.Message, enGetFileType.FilesAndFolders);   
                }
                catch (Exception)
                {
                    
                }
                if (sFileNames.Count == 0)
                {
                    if (rez.Message.EndsWith(" Permission denied\r\n"))
                    {
                        error = rez.Message;
                    }
                }
            }
            else
            {
                error = rez.Message;
            }

            return sFileNames.ToArray();
        }

        public virtual FileObject[] GetFilesFromFolder()
        {
            return GetFilesFromFolder(GetBaseFolder());
        }

        public virtual FileObject[] GetFoldersFromFolder()
        {
            return GetFoldersFromFolder(GetBaseFolder());
        }

        public virtual IFileObject[] GetAllFromFolder(ref string error)
        {
            return GetAllFromFolder(GetBaseFolder(), ref error);
        }

        public virtual CommandResult Copy(string sDestination, List<string> lItems, ref List<string> folders, ref List<string> files)
        {
            if (lItems.Count > 0)
            {
                bool bRealFolder = false;

                try
                {
                    DirectoryInfo diF = new DirectoryInfo(sDestination);
                    if (diF.Exists)
                    {
                        bRealFolder = true;
                    }
                }
                catch (Exception)
                {
                    
                }                

                string sDestinationFolder = bRealFolder ? sDestination : string.Format("{0}/{1}", GetBaseFolder(), sDestination).Replace("//","/");

                string itemCheck = lItems[0];
                if (itemCheck.StartsWith("[virtualfolder]") || itemCheck.StartsWith("[virtualfile]"))
                {
                    List<FileOperation> lstItems = new List<FileOperation>();                   
                    foreach (string item in lItems)
                    {                        
                        string sFullName = item.Substring(item.IndexOf('\\') + 1);
                        string sItem = string.Format("{0}{1}{2}", GetBaseFolder(), FD, sFullName);

                        string sName = sFullName.Substring(sFullName.LastIndexOf(FD[0]) + 1);
                        sName = sName.Replace(":", "_");

                        string sDest = string.Empty;
                        if (bRealFolder)
                        {
                            sDest = string.Format("{0}{1}{2}", sDestinationFolder, "\\", sName);
                        }
                        else
                        {
                            sDest = string.Format("{0}", sDestinationFolder);
                        }

                        //string sDest = string.Format("{0}{1}{2}", sDestinationFolder, bRealFolder? "\\" : "/", sName);

                        if (item.StartsWith("[virtualfolder]"))
                        {
                            FileOperation fo = new FileOperation();
                            fo.Source = sItem.Replace("//", "/") + (bRealFolder ? "/" : "");
                            fo.Destination = sDest;
                            fo.IsFolder = true;

                            lstItems.Add(fo);

                            folders.Add(sName);
                        }
                        if (item.StartsWith("[virtualfile]"))
                        {
                            FileOperation fo = new FileOperation();
                            fo.Source = sItem.Replace("//","/");
                            fo.Destination = sDest;
                            fo.IsFolder = false;

                            lstItems.Add(fo);

                            files.Add(sName);
                        }
                    }
                    ADBCommand command = new ADBCommand();
                    CommandResult rez = command.Copy(lstItems, bRealFolder ? enCopyType.Pull : enCopyType.Copy, true);
                    return rez;
                }
                else
                {
                    List<FileOperation> lstItems = new List<FileOperation>();
                    foreach (string item in lItems)
                    {
                        string sName = item.Substring(item.LastIndexOf('\\') + 1);

                        FileInfo fi = new FileInfo(item);
                        bool bFile = fi.Exists;
                        DirectoryInfo di = new DirectoryInfo(item);
                        bool bDir = di.Exists;
                        string sPath = string.Empty;

                        bool isFolderEmpty = false;

                        if (bFile)
                        {
                            sPath = string.Format("{0}/{1}", sDestinationFolder, sName).Replace("//", "/");
                            files.Add(sName);
                        }
                        else
                        {
                            if (bDir)
                            {
                                isFolderEmpty = IsDirectoryEmpty(item);
                                sPath = string.Format("{0}/{1}", sDestinationFolder, sName).Replace("//", "/");
                                folders.Add(sName);
                            }
                        }    

                        FileOperation fo = new FileOperation();
                        fo.IsFolder = bDir;
                        fo.Destination = sPath;
                        fo.Source = item;
                        fo.IsFolderEmpty = isFolderEmpty;
                        lstItems.Add(fo);
                    }

                    ADBCommand command = new ADBCommand();
                    CommandResult rez = command.Copy(lstItems, enCopyType.Push, true);
                    return rez;
                }
            }
           return null;
        }

        public bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        public virtual string GetBaseFolder()
        {
            return "/";
        }
        
    }
}
