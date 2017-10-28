using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsShell.Nspace;

namespace WindowsShell.ADB
{    
    public class CommandResultHelper
    {
        public List<FileObject> GetItems(string sMessage, enGetFileType type)
        {
            List<FileObject> lsFiles = new List<FileObject>();

            bool isNewStruct = false;
            bool isChecked = false;
            foreach (var str in sMessage.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (string.IsNullOrEmpty(str.Trim()))
                    continue;
                string sret = str.Replace("\r", "");
                if (sret == "opendir failed, Permission denied")
                    continue;
                if (sret.EndsWith(": Permission denied"))
                    continue;
                if (sret.StartsWith("total "))
                    continue;

                string[] sArr = sret.Split(' ');

                if (sArr[0] == "lstat")
                {
                    string sPath = sArr[1].Trim('\'');
                    string[] sp = sPath.Split('/');
                    string name = sp[sp.Length - 1];
                }
                else
                {
                    List<string> ls = new List<string>();
                    foreach (string s in sArr)
                    {
                        if (!string.IsNullOrEmpty(s))
                        {
                            ls.Add(s);
                        }
                    }

                    if (!isChecked && ls.Count == 8 && ls[7].Equals("."))
                    {
                        isNewStruct = true;                        
                    }
                    isChecked = true;

                    int iterator = 0;
                    string attr = ls[iterator];
                    iterator++;
                    string count = string.Empty;
                    
                    if (isNewStruct)
                    {
                        count = ls[iterator];
                        iterator++;
                    }

                    string perm1 = ls[iterator];
                    iterator++;
                    string perm2 = ls[iterator];
                    iterator++;
                    string size;
                    string date;
                    string time;
                    string file;
                    string link = string.Empty;
                    int currIndex = 0;

                    if (!isNewStruct && ls[3].Contains("-"))
                    {
                        size = "";
                        date = ls[iterator];
                        iterator++;
                        time = ls[iterator];
                        iterator++;
                        file = ls[iterator];
                        currIndex = iterator;
                    }
                    else
                    {
                        size = ls[iterator];
                        iterator++;
                        date = ls[iterator];
                        iterator++;
                        time = ls[iterator];
                        iterator++;
                        file = ls[iterator];
                        currIndex = iterator;
                    }
                    if (ls.Count > currIndex + 1)
                    {
                        /*
                        for (int i = currIndex + 1; i < ls.Count; i++)
                        {
                            file += " " + ls[i];
                        }
                        */

                        string stT = sret;
                        if (!string.IsNullOrEmpty(attr.Trim()))
                            stT = ReplaceFirst(stT, attr, "");
                        if (!string.IsNullOrEmpty(perm1.Trim()))
                            stT = ReplaceFirst(stT, perm1, "");
                        if (!string.IsNullOrEmpty(perm2.Trim()))
                            stT = ReplaceFirst(stT, perm2, "");
                        if (!string.IsNullOrEmpty(size.Trim()))
                            stT = ReplaceFirst(stT, size, "");
                        if (!string.IsNullOrEmpty(date.Trim()))
                            stT = ReplaceFirst(stT, date, "");
                        if (!string.IsNullOrEmpty(time.Trim()))
                            stT = ReplaceFirst(stT, time, "");
                        if (!string.IsNullOrEmpty(count.Trim()))
                            stT = ReplaceFirst(stT, count, "");

                        stT = stT.Trim();
                        file = stT;
                        //int n = sret.LastIndexOf(file);
                        //file += sret.Substring(n + file.Length);
                        if (file.Contains(" -> "))
                        {
                            int ni = file.LastIndexOf(" -> ");                            
                            link = file.Substring(ni + " -> ".Length);
                            file = file.Substring(0, ni);
                        }
                    }

                    FileObject obj = new FileObject();
                    obj.Attr = attr;
                    obj.Perm1 = perm1;
                    obj.Perm2 = perm2;
                    obj.Size = size;
                    obj.Date = date;
                    obj.Time = time;
                    obj.Name = file;
                    obj.Link = link;
                    obj.IsLink = link.Length > 0;
                    obj.IsFolder = !attr.StartsWith("-");//!(size.Length > 0);

                    if (isNewStruct && (file.Equals(".") || file.Equals("..")))
                        continue;

                    if (type == enGetFileType.FilesAndFolders)
                        lsFiles.Add(obj);
                    else if (type == enGetFileType.FilesOnly)
                    {
                        if (!obj.IsFolder)
                            lsFiles.Add(obj);
                    }
                    else if (type == enGetFileType.FoldersOnly)
                    {
                        if (obj.IsFolder)
                            lsFiles.Add(obj);
                    }
                    /*
                    if (size.Length > 0)
                    {
                        //file
                        if (link.Length > 0)
                        {
                            //link to file
                        }
                        else
                        {
                            //real file
                        }
                       
                        if (bFiles)
                        {
                            lsFiles.Add(file);
                        }
                    }
                    else
                    {
                        //folder
                        if (link.Length > 0)
                        {
                            //link to folder
                        }
                        else
                        {
                            //real folder
                        }

                        if (!bFiles)
                        {
                            lsFiles.Add(file);
                        }
                    }
                     */
                }
            }

            return lsFiles;
        }

        public string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
    }
}
