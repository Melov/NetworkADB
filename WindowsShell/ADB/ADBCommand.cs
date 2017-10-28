using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsShell.Nspace;

namespace WindowsShell.ADB
{
    public enum enCopyType
    {
        Pull =1, //device-com
        Push = 2, //comp-device
        Copy =3 //dev-dev
    }

    public class CommandResult
    {
        public string Message;
        public bool IsSuccess;

        public CommandResult(string message, bool isSuccess)
        {
            this.Message = message;
            this.IsSuccess = isSuccess;
        }

        public void ShowMessage()
        {
            string sRet = string.Empty;
            foreach (var str in Message.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!str.StartsWith("["))
                {
                    sRet += str + "\r\n";
                }
            }
            if (!string.IsNullOrEmpty(sRet))
            {
                MessageBox.Show(sRet, IsSuccess ? "Information" : "Warning", MessageBoxButtons.OK, IsSuccess ? MessageBoxIcon.Information : MessageBoxIcon.Warning);   
            }            
        }
    }

    public class ADBCommand
    {
        private string COMMAND_CONNECT = "connect {0}";
        private string COMMAND_CONNECT_DEV = "-s {0} shell";
        private string COMMAND_DISCONNECT = "disconnect";
        private string COMMAND_KILL_SERVER = "kill-server";
        private string COMMAND_LIST_DIR = "shell ls {0} -la";
        private string COMMAND_DEVICES = "devices";
        private string COMMAND_NEWDIR = "shell mkdir \"{0}\"";
        private string COMMAND_DELETE = "shell rm -rf \"{0}\"";
        private string COMMAND_RENAME = "shell mv \"{0}\" \"{1}\"";
        private string COMMAND_INSTALL = "install \"{0}\"";
        private string MESSAGE_CONNECT_ERROR = "Please Set Device Address in Properties";

        private string MESSAGE_NODEVICE = "no devices/emulators found";

        public static string SelectDevice(string sIn)
        {
            string sRet = sIn;
            DeviseAddr addr = new DeviseAddr();
            if (!string.IsNullOrEmpty(addr.ConnectionType) && addr.ConnectionType.Equals("usb") &&
                !string.IsNullOrEmpty(addr.UsbDevice))
            {
                sRet = string.Format("-s {0} {1}", addr.UsbDevice, sIn);
            }
            if (!string.IsNullOrEmpty(addr.ConnectionType) && addr.ConnectionType.Equals("ip") &&
                !string.IsNullOrEmpty(addr.Address))
            {
                sRet = string.Format("-s {0} {1}", addr.Address, sIn);
            }
            return sRet;
        }

        public string CurrentDevice()
        {
            DeviseAddr addr = new DeviseAddr();
            string sDev = string.Empty;
            if (!string.IsNullOrEmpty(addr.ConnectionType) && addr.ConnectionType.Equals("usb") && !string.IsNullOrEmpty(addr.UsbDevice))
            {
                sDev = addr.UsbDevice;
            }
            else
            {
                sDev = addr.ToString();
            }

            return sDev;
        }

        public CommandResult Connect()
        {
            DeviseAddr addr = new DeviseAddr();
            if (addr.IsValid())
            {
                ADBRunner runner = new ADBRunner();
                string sCommand = string.Format(COMMAND_CONNECT, addr.Address);
                if (!string.IsNullOrEmpty(addr.ConnectionType) && addr.ConnectionType.Equals("usb") && !string.IsNullOrEmpty(addr.UsbDevice))
                {
                    sCommand = string.Format(COMMAND_CONNECT_DEV, addr.UsbDevice);
                    return new CommandResult("You select '" + addr.UsbDevice + "' device", true);    
                }
                else
                {
                    bool bret = runner.LaunchProcess(sCommand, CommandDialog.Connect, true);
                    return new CommandResult(runner.strMessage, bret);      
                }                
            }
            return new CommandResult(MESSAGE_CONNECT_ERROR, false);  
        }

        public CommandResult Disconnect(bool bOnly = false)
        {
            ADBRunner runner = new ADBRunner();
            bool bret = runner.LaunchProcess(COMMAND_DISCONNECT,CommandDialog.Disconnect);
            string sRet = runner.strMessage;
            if (!bOnly)
            {
                bret = runner.LaunchProcess(COMMAND_KILL_SERVER, CommandDialog.KillServer);
                sRet += runner.strMessage;   
            }            
            return new CommandResult(sRet, bret);   
        }

        public CommandResult Devices()
        {
            ADBRunner runner = new ADBRunner();
            bool bret = runner.LaunchProcess(COMMAND_DEVICES, CommandDialog.Devices);
            return new CommandResult(runner.strMessage.Replace("List of devices attached\r\n", "").Replace("\t"," "), bret);
        }

        public CommandResult Install(string sApk)
        {
            CommandResult rez = Devices();
            if (!rez.IsSuccess)
            {
                return rez;
            }
            else
            {
                if (!string.IsNullOrEmpty(rez.Message))
                {
                    ADBRunner runner = new ADBRunner();
                    bool bret = runner.LaunchProcess(SelectDevice(string.Format(COMMAND_INSTALL, sApk)), CommandDialog.Install, true);
                    return new CommandResult(runner.strMessage, bret);      
                }
                else
                {
                    return new CommandResult("No connected devices.\r\n" + "Please click `Connect` in Context Menu", false);      
                }
            }            
        }

        public CommandResult CreateDirectory(string dir)
        {
            string dirValid = dir.Replace(" ", "\\ ")
                .Replace("(", "\\(")
                .Replace(")", "\\)")
                .Replace("'", "\\'")
                .Replace("&", "\\&");
            ADBRunner runner = new ADBRunner();
            bool bret = runner.LaunchProcess(SelectDevice(string.Format(COMMAND_NEWDIR, dirValid)), CommandDialog.None);
            if (runner.strMessage.Contains("Read-only file system") || runner.strMessage.Contains("Permission denied"))
            {
                bret = false;
            }
            return new CommandResult(runner.strMessage, bret);
        }

        public CommandResult Rename(string source, string destination)
        {
            ADBRunner runner = new ADBRunner();
            string sourceValid = source.Replace(" ", "\\ ").Replace("(", "\\(").Replace(")", "\\)").Replace("'", "\\'").Replace("&", "\\&");
            string destinationValid = destination.Replace(" ", "\\ ").Replace("(", "\\(").Replace(")", "\\)").Replace("'", "\\'").Replace("&", "\\&");
            bool bret = runner.LaunchProcess(SelectDevice(string.Format(COMMAND_RENAME, sourceValid, destinationValid)), CommandDialog.None);
            return new CommandResult(runner.strMessage, bret);
        }

        public CommandResult DeleteObject(string path)
        {
            ADBRunner runner = new ADBRunner();
            string pathValid = path.Replace(" ", "\\ ").Replace("(", "\\(").Replace(")", "\\)").Replace("'", "\\'").Replace("&", "\\&");
            bool bret = runner.LaunchProcess(SelectDevice(string.Format(COMMAND_DELETE, pathValid)), CommandDialog.None);
            return new CommandResult(runner.strMessage, bret);
        }

        public CommandResult DeleteObjects(List<FileOperation> lstItems, bool showProgress = false)
        {
            foreach (FileOperation fileOperation in lstItems)            
            {
                string pathValid = fileOperation.Source.Replace(" ", "\\ ").Replace("(", "\\(").Replace(")", "\\)").Replace("'", "\\'").Replace("&", "\\&");
                fileOperation.Command = SelectDevice(string.Format(COMMAND_DELETE, pathValid));
            }

            ADBRunner runner = new ADBRunner();
            bool bret = runner.LaunchProcess(lstItems, CommandDialog.Delete, showProgress);
            return new CommandResult(runner.strMessage, bret);
        }

        public CommandResult ListDirectory(string dir)
        {
            ADBRunner runner = new ADBRunner();
            string sd = dir.Replace(" ", "\\ ").Replace("(", "\\(").Replace(")", "\\)").Replace("'", "\\'").Replace("&", "\\&") + "/";

            bool bret = runner.LaunchProcess(SelectDevice(string.Format(COMMAND_LIST_DIR, sd)), CommandDialog.ListDir);
            string sret = runner.strMessage;
            if (bret)
            {                
                if (runner.strMessage.StartsWith("error: "))
                {
                    bret = false;
                    sret = runner.strMessage;
                    string sAdbErr = runner.strMessage.Replace("error: ", "");
                    if (sAdbErr.Contains("more than one device/emulator"))
                    {
                        bool bret1 = runner.LaunchProcess(COMMAND_DEVICES, CommandDialog.Devices);
                        if (bret1)
                        {
                            runner.strMessage = runner.strMessage.Replace("List of devices attached\r\n", "").Replace("\t"," ");
                            sret = sAdbErr + "--------------\r\n" + runner.strMessage;
                        }
                    }
                }
            }
            
            return new CommandResult(sret, bret);   
        }

        public CommandResult CreateScreenShot(string dir)
        {
            string spull = string.Format("{0}\\{1}", dir,
                string.Format("screen-{0:yyyy-MM-dd_hh-mm-ss-tt}.png", DateTime.Now));

            ADBRunner runner = new ADBRunner();
            bool bret = runner.LaunchProcess(SelectDevice("shell screencap -p /sdcard/screen.png"), CommandDialog.CreateScreenShot, true);
            if (bret)
            {
                if (!runner.strMessage.Contains(MESSAGE_NODEVICE))
                {
                    CommandResult crc = Copy("/sdcard/screen.png", spull, enCopyType.Pull, true);
                    bret = crc.IsSuccess;//runner.LaunchProcess("pull /sdcard/screen.png" + " " + spull, CommandDialog.None);
                    if (bret)
                    {
                        bret = runner.LaunchProcess(SelectDevice("shell rm /sdcard/screen.png"), CommandDialog.None);
                    }
                    else
                    {
                        return new CommandResult(crc.Message, bret);
                    }   
                }
                else
                {
                    return new CommandResult(runner.strMessage, false);
                }                
            }
            else
            {
                return new CommandResult(runner.strMessage, bret);
            }

            return new CommandResult("ScreenShot created successfully", bret);
        }

        public CommandResult Copy(string source, string destination, enCopyType ct, bool showProgress = false)
        {
            ADBRunner runner = new ADBRunner();
            string sourceValid = string.Format("\"{0}\"", source);
            string destinationValid =
                destination/*.Replace(":", "_")
                    .Replace("*", "_")
                    .Replace("?", "_")
                    .Replace("<", "_")
                    .Replace(">", "_")
                    .Replace("|", "_")
                    .Replace("\"", "_")*/;
            string type = ct == enCopyType.Pull ? "pull" : "push";
            if (ct == enCopyType.Copy)
            {
                type = "shell cp";
                showProgress = false;
            }
            string runCommand = string.Format("{0} {1} {2}", type, showProgress ? "-p" : string.Empty, sourceValid) + " " + string.Format("\"{0}\"", destinationValid);
            bool bret = runner.LaunchProcess(SelectDevice(runCommand), CommandDialog.Copy, showProgress, destination);
            return new CommandResult(runner.strMessage, bret);
        }

        public CommandResult Copy(List<FileOperation> lstItems, enCopyType ct, bool showProgress = false)
        {
            foreach (FileOperation fileOperation in lstItems)
            {
                string sourceValid = string.Format("\"{0}\"", fileOperation.Source);
                string destinationValid =
                fileOperation.Destination/*.Replace(":", "_")*/
                    .Replace("*", "_")
                    .Replace("?", "_")
                    .Replace("<", "_")
                    .Replace(">", "_")
                    .Replace("|", "_")
                    .Replace("\"", "_");
                string type = ct == enCopyType.Pull ? "pull" : "push";

                bool bProgress = showProgress;

                if (ct == enCopyType.Copy)
                {
                    type = "shell cp -a";
                    bProgress = false;
                    destinationValid = destinationValid.Replace(" ", "\\ ")
                            .Replace("(", "\\(")
                            .Replace(")", "\\)")
                            .Replace("'", "\\'")
                            .Replace("&", "\\&");
                    sourceValid = sourceValid.Replace(" ", "\\ ")
                            .Replace("(", "\\(")
                            .Replace(")", "\\)")
                            .Replace("'", "\\'")
                            .Replace("&", "\\&");
                }

                string runCommand = string.Format("{0}{1} {2}", type, bProgress ? " -p" : string.Empty, sourceValid) + " " + string.Format("\"{0}\"", destinationValid);

                if (ct == enCopyType.Push)
                {
                    if (fileOperation.IsFolder && fileOperation.IsFolderEmpty)
                    {
                        string dirValid = fileOperation.Destination.Replace(" ", "\\ ")
                            .Replace("(", "\\(")
                            .Replace(")", "\\)")
                            .Replace("'", "\\'")
                            .Replace("&", "\\&");

                        runCommand = string.Format(COMMAND_NEWDIR, dirValid);
                    }
                }

                fileOperation.Command = SelectDevice(runCommand);
            }

            ADBRunner runner = new ADBRunner();
            bool bret = runner.LaunchProcess(lstItems, CommandDialog.Copy, showProgress);
            return new CommandResult(runner.strMessage, bret);
        }
    }
}
