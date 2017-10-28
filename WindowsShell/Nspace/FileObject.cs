using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace WindowsShell.Nspace
{
    public enum enGetFileType
    {
        FilesOnly = 0,
        FoldersOnly = 1,
        FilesAndFolders = 2
    }
    
    [Serializable]
    public class FileObject : IFileObject
    {
        public string Attr;
        public string Perm1;
        public string Perm2;
        public string Size;
        public string Date;
        public string Time;
        public string Name;
        public string Link;
        public bool IsLink;
        public bool IsFolder;

        public static byte[] ToByteArray(IFileObject obj)
        {
            /*
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
            */
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryWriter w = new BinaryWriter(ms, Encoding.Default);
                w.Write(obj.Attr ?? string.Empty);
                w.Write(obj.Perm1 ?? string.Empty);
                w.Write(obj.Perm2 ?? string.Empty);
                w.Write(obj.Size ?? string.Empty);
                w.Write(obj.Date ?? string.Empty);
                w.Write(obj.Time ?? string.Empty);
                w.Write(obj.Name ?? string.Empty);
                w.Write(obj.Link ?? string.Empty);
                w.Write(obj.IsLink);
                w.Write(obj.IsFolder);

                return ms.ToArray();
            }
        }

        public static FileObject ToObject(byte[] arrBytes)
        {
            /*
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                object obj = binForm.Deserialize(memStream);
                return (FileObject)obj;
            }
             */

            FileObject fo = new FileObject();
            using (MemoryStream ms = new MemoryStream(arrBytes))
            {
                BinaryReader r = new BinaryReader(ms, Encoding.Default);
                fo.Attr = r.ReadString();
                fo.Perm1 = r.ReadString();
                fo.Perm2 = r.ReadString();
                fo.Size = r.ReadString();
                fo.Date = r.ReadString();
                fo.Time = r.ReadString();
                fo.Name = r.ReadString();
                fo.Link = r.ReadString();
                fo.IsLink = r.ReadBoolean();
                fo.IsFolder = r.ReadBoolean();

                return fo;
            }
        }

        string IFileObject.Attr
        {
            get { return Attr; }
            set { Attr = value; }
        }

        string IFileObject.Perm1
        {
            get { return Perm1; }
            set { Perm1 = value; }
        }

        string IFileObject.Perm2
        {
            get { return Perm2; }
            set { Perm2 = value; }
        }

        string IFileObject.Size
        {
            get { return Size; }
            set { Size = value; }
        }

        string IFileObject.Date
        {
            get { return Date; }
            set { Date = value; }
        }

        string IFileObject.Time
        {
            get { return Time; }
            set { Time = value; }
        }

        string IFileObject.Name
        {
            get { return Name; }
            set { Name = value; }
        }

        string IFileObject.Link
        {
            get { return Link; }
            set { Link = value; }
        }

        bool IFileObject.IsLink
        {
            get { return IsLink; }
            set { IsLink = value; }
        }

        bool IFileObject.IsFolder
        {
            get { return IsFolder; }
            set { IsFolder = value; }
        }
    }
}
