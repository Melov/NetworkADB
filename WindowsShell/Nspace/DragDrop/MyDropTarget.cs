using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;
using WindowsShell.Interop;
using MyData.Extensions;

namespace WindowsShell.Nspace.DragDrop
{
    public class MyDropTarget : IDropTarget
    {
        IFolderObject _folderObj;
        public MyDropTarget(IFolderObject folderObj)
        {
            _folderObj = folderObj;
        }
        
        public int DragEnter(System.Runtime.InteropServices.ComTypes.IDataObject pDataObj, int grfKeyState, Win32Point pt, ref int pdwEffect)
        {
            //List<string> files = DataObjectHelper.GetFiles(pDataObj);
            //_folderObj.PathString

            bool bFolder = ((_folderObj.Attributes & FolderAttributes.Folder) == FolderAttributes.Folder);


            pdwEffect = bFolder ? (int)SFGAO.SFGAO_CANCOPY : 0;
            return WinError.S_OK;
        }

        public int DragOver(int grfKeyState, Win32Point pt, ref int pdwEffect)
        {
            return WinError.S_OK;
        }

        public int DragLeave()
        {
            return WinError.S_OK;
        }

        public int Drop(System.Runtime.InteropServices.ComTypes.IDataObject pDataObj, int grfKeyState, Win32Point pt, ref int pdwEffect)
        {            
            bool bFolder = ((_folderObj.Attributes & FolderAttributes.Folder) == FolderAttributes.Folder);
            if (!bFolder)
                return WinError.S_OK;       

            List<string> files = DataObjectHelper.GetFiles(pDataObj);
            
            string sr = string.Empty;
            foreach (string file in files)
            {
                sr += file + "\r\n";
            }
            sr += "to\r\n" + _folderObj.PathString;
            //MessageBox.Show(sr);
            Debug.WriteLine(sr);
            _folderObj.CopyItems(_folderObj, files);

            pdwEffect = (int)SFGAO.SFGAO_CANCOPY;
            return WinError.S_OK;            
        }
    }
}
