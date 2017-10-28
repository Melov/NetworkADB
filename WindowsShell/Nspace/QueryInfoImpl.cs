using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsShell.Interop;

namespace WindowsShell.Nspace
{
    public class QueryInfoImpl : IQueryInfo
    {
        private IFolderObject _folderObj;

        public QueryInfoImpl(IFolderObject folderObj)
        {
            _folderObj = folderObj;
        }

        public int GetInfoTip(QITIPF dwFlags, out string ppwszTip)
        {
            string sValues = string.Empty;
            foreach (Column column in _folderObj.Columns)
            {
                sValues += string.Format("{0}: {1}\r\n", column.Name, _folderObj.GetColumnValue(column));
            }
            sValues = sValues.TrimEnd('\n');
            sValues = sValues.TrimEnd('\r');
            ppwszTip = sValues;            
            return WinError.S_OK;
        }

        public int GetInfoFlags(out int pdwFlags)
        {
            pdwFlags = 0;
            return WinError.S_OK;
        }
    }
}
