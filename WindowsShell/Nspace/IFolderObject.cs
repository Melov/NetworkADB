using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using WindowsShell.Interop;

using MyData.Extensions;

namespace WindowsShell.Nspace
{
    public static class CodePath
    {
        public static string Code(byte[][] path)
        {
            string sRet = string.Empty;
            foreach (byte[] bytes in path)
            {
                string s = Convert.ToBase64String(bytes);
                sRet += s + "|";
            }
            return sRet.TrimEnd('|');
        }

        public static byte[][] Decode(string sPath)
        {
            List<byte[]> btRet = new List<byte[]>();
            string[] sP = sPath.Split('|');
            foreach (string s in sP)
            {
                byte[] bytes = Convert.FromBase64String(s);
                btRet.Add(bytes);
            }
            return btRet.ToArray();
        }

        public static string GetPathFromString(string sPath)
        {
            string s = sPath.Replace("[virtualfolder]", "").Replace("[virtualfile]", "");
            int np = s.IndexOf('\\');

            return s.Substring(0, np);
        }
    }

	public interface IFolderObject
	{           
        IShellView ShellView { get; set; }
		FolderAttributes Attributes { get; }
		ColumnCollection Columns { get; }
		int CompareTo(Column column, IFolderObject folderObj);
        DataObject DataObject { get; set; }
		object GetColumnValue(Column column);
        void SetColumnValue(Column column, object value);
		string GetDisplayName(NameOptions opts);
        Dictionary<int, System.Drawing.Icon> Icons { get; set; }
		ShellIcon GetIcon(bool open);
		IEnumerable GetItems(IWin32Window owner);
		ShellMenuItem[] GetMenuItems(IFolderObject[] children);
		ShellMenuItem[] MenuItems { get; set; }
        ContextMenuImpl ContextMenu { get; set; }
		byte[][] PathData { get; }
        string PathString { get; set; }
		byte[] Persist();
        IdList idListAbsolute { get; }
		string RemoteComputer { set; }
	    IFolderObject GetParrent();
		IFolderObject Restore(byte[] data, bool forName = false);
		void SetName(IWin32Window owner, string name, NameOptions opts);
		void SetPath(byte[][] pathData);
		void SetFullPath(byte[][] pathData);
	    IntPtr pid { get; set; }
        //void SetIdList(IdList lst);
        void SetIdList(IdList IdList);
        void DeleteItems(IFolderObject[] items);
        void RefreshItems(IFolderObject[] items);
        void NewFolder();
        void CopyItems(IFolderObject fo, List<string> lItems);
	}

    public interface IFileObject
    {
        string Attr{ get; set; }
        string Perm1{ get; set; }
        string Perm2{ get; set; }
        string Size{ get; set; }
        string Date{ get; set; }
        string Time{ get; set; }
        string Name{ get; set; }
        string Link{ get; set; }
        bool IsLink{ get; set; }
        bool IsFolder{ get; set; }
    }
}
