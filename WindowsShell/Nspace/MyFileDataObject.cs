using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsShell.Nspace
{
    class MyFileDataObject : IDataObject
    {
        public event Func<object> getDataCallback;
        // Returns: The data associated with the specified format, or null.
        public object GetData(string format)
        {
            return "eeeeeeeeee";
            if (format == DataFormats.FileDrop && getDataCallback != null)
                return getDataCallback();
            return null;
        }
        public bool GetDataPresent(string format)
        {
            return true;
            return format == DataFormats.FileDrop;
        }
        public string[] GetFormats()
        {
            return new string[] { DataFormats.FileDrop };
        }
        public object GetData(Type format) { return GetData(format.ToString()); }
        public object GetData(string format, bool autoConvert) { return GetData(format); }
        public bool GetDataPresent(Type format) { return GetDataPresent(format.ToString()); }
        public bool GetDataPresent(string format, bool autoConvert) { return GetDataPresent(format); }
        public string[] GetFormats(bool autoConvert) { return GetFormats(); }
        public void SetData(object data) {  }
        public void SetData(string format, object data) { SetData(data); }
        public void SetData(Type format, object data) { SetData(data); }
        public void SetData(string format, bool autoConvert, object data) { SetData(data); }

        object IDataObject.GetData(Type format)
        {
            return "sdfsdfsdfsd";
        }

        object IDataObject.GetData(string format)
        {
            return "sdfsdfsdfsd";
        }

        object IDataObject.GetData(string format, bool autoConvert)
        {
            return "sdfsdfsdfsd";
        }

        bool IDataObject.GetDataPresent(Type format)
        {
            return true;
        }

        bool IDataObject.GetDataPresent(string format)
        {
            return true;
        }

        bool IDataObject.GetDataPresent(string format, bool autoConvert)
        {
            return true;
        }

        string[] IDataObject.GetFormats()
        {
            List<string> ls = new List<string>();
            ls.Add(DataFormats.FileDrop);
            return ls.ToArray();
        }

        string[] IDataObject.GetFormats(bool autoConvert)
        {
            List<string> ls = new List<string>();
            ls.Add(DataFormats.FileDrop);
            return ls.ToArray();
            //throw new NotImplementedException();
        }

        void IDataObject.SetData(object data)
        {
            //throw new NotImplementedException();
        }

        void IDataObject.SetData(Type format, object data)
        {
            //throw new NotImplementedException();
        }

        void IDataObject.SetData(string format, object data)
        {
           // throw new NotImplementedException();
        }

        void IDataObject.SetData(string format, bool autoConvert, object data)
        {
           // throw new NotImplementedException();
        }
    }
}
