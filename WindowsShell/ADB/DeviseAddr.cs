using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WindowsShell.ADB
{
    public class DeviseAddr
    {
        public string IP;
        public string PORT;
        public string Address;

        public string UsbDevice;
        public string ConnectionType;
        public string ADBPath;

        public DeviseAddr()
        {
            string sSF = GetSettingsFile();
            FileInfo fi = new FileInfo(sSF);
            if (fi.Exists)
            {
                string sretFile = File.ReadAllText(sSF);
                string sret = GetLine(sretFile, 1);
                this.Address = sret;
                string[] sip = sret.Split(':');
                this.IP = sip[0];
                this.PORT = sip[1];

                string sUsbDevice = GetLine(sretFile, 2);
                string sType = GetLine(sretFile, 3);
                string sADBPath = GetLine(sretFile, 4);

                UsbDevice = sUsbDevice;
                ConnectionType = sType;
                ADBPath = sADBPath;
            }
        }
        public DeviseAddr(string IP, string PORT, string Address)
        {
            this.IP = IP;
            this.PORT = PORT;
            this.Address = Address;
        }
        public DeviseAddr(string IP, string PORT)
        {
            this.IP = IP;
            this.PORT = PORT;
            this.Address = string.Format("{0}:{1}", IP, PORT);
        }
        public DeviseAddr(string Address)
        {
            this.Address = Address;
            string[] sip = Address.Split(':');
            this.IP = sip[0];
            this.PORT = sip[1];
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", IP, PORT);
        }

        public string GetSettingsFile()
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string sRet = string.Format("{0}\\{1}", assemblyFolder, "settings.txt");
            return sRet;
        }

        string GetLine(string text, int lineNo)
        {
            string[] lines = text.Replace("\r", "").Split('\n');
            return lines.Length >= lineNo ? lines[lineNo - 1] : null;
        }

        public void Save(bool isIp, string device, string adbPath)
        {
            string sf = GetSettingsFile();
            string sWrite = string.Format("{0}:{1}", IP, PORT);
            sWrite += "\r\n" + device;
            sWrite += "\r\n" + (isIp ? "ip" : "usb");
            sWrite += "\r\n" + adbPath;
            File.WriteAllText(sf, sWrite);
        }

        public bool IsValid()
        {
            bool bret = false;
            if (!string.IsNullOrEmpty(IP) && !string.IsNullOrEmpty(PORT) && !string.IsNullOrEmpty(Address))
            {
                bret = true;
            }
            return bret;
        }
    }
}
