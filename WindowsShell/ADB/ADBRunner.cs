using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using WindowsShell.Dialogs;
using WindowsShell.Nspace;
using Microsoft.Win32;

namespace WindowsShell.ADB
{
    public enum CommandDialog
    {
        None = -1,
        Connect = 0,
        Disconnect =1,
        KillServer = 2,
        Devices = 3,
        ListDir = 4,
        Copy = 5,
        CreateScreenShot = 6,
        Delete = 7,
        Install = 8
    }

    public class ADBRunner
    {
        public static string ADB_EXE = "adb.exe";
        public string strMessage;

        private static ProgressForm pf;
        private static ConnectForm cf;
        private static OperationForm of;

        void build_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            string log = e.Data;

            if (!string.IsNullOrEmpty(log))
            {
                log = Encoding.UTF8.GetString(Encoding.Default.GetBytes(log));
                strMessage += (log.TrimStart('\t') + "\r\n");

                if (pf != null && pf.Visible)
                {
                    //Debug.WriteLine(log);
                    if (log.StartsWith("[") && log.Contains("%]"))
                    {
                        int percents = int.Parse(log.Substring(1, 3).Trim());
                        string sFile = log.Substring(7);
                        pf.SetProgress(sFile, percents);
                    }                    
                }
                else if (of != null && of.Visible && of.OperationType == OperationForm.TypeOp.Install)
                {
                    if (log.StartsWith("[") && log.Contains("%]"))
                    {
                        int percents = int.Parse(log.Substring(1, 3).Trim());
                        of.SetFormText("["+percents+"%]");
                    }
                }
            }
        }

        private void ThreadProcCopy()
        {
            if (pf != null)
            {
                pf.Visible = false;
                pf.ShowDialog();   
            }            
        }
        private void ThreadProcConnect()
        {
            if (cf != null)
            {
                cf.Visible = false;
                cf.ShowDialog();
            }
        }
        private void ThreadProcOperation()
        {
            if (of != null)
            {
                of.Visible = false;
                of.ShowDialog();
            }
        }

        public static string FindADB()
        {
            string sRet = string.Empty;

            DeviseAddr addr = new DeviseAddr();
            if (!string.IsNullOrEmpty(addr.ADBPath))
            {
                sRet = addr.ADBPath;
                return sRet;
            }

            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            FileInfo fi = new FileInfo(string.Format("{0}\\{1}", assemblyFolder, ADB_EXE));
            if (fi.Exists)
            {
                sRet = assemblyFolder + "\\";
            }
            else
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Android Studio"))
                {
                    if (key != null)
                    {
                        Object o = key.GetValue("SdkPath");
                        if (o != null)
                        {
                            string sPath = string.Format("{0}\\platform-tools\\{1}", o, ADB_EXE);
                            FileInfo fiReg = new FileInfo(sPath);
                            if (fiReg.Exists)
                            {
                                sRet = o + "\\platform-tools\\";
                            }
                        }
                    }
                }
            }

            return string.Format("{0}{1}", sRet, ADB_EXE);
        }

        public bool LaunchProcess(List<FileOperation> lstItems, CommandDialog cd, bool bShowProgress = false)
        {
            string sAdb = FindADB();
            if (string.IsNullOrEmpty(sAdb))
            {
                strMessage = "Can not find adb.exe";
                return false;
            }

            Thread thCopy = null;
            if (bShowProgress)
            {
                if (cd == CommandDialog.Copy)
                {
                    if (pf == null)
                    {
                        pf = new ProgressForm();
                        pf.Files = lstItems;
                    }

                    thCopy = new Thread(() => ThreadProcCopy());
                    thCopy.Start();    
                }
                if (cd == CommandDialog.Delete)
                {
                    if (of == null)
                    {
                        of = new OperationForm(OperationForm.TypeOp.Delete);
                        of.SetOperation("Deleting");
                    }

                    thCopy = new Thread(() => ThreadProcOperation());
                    thCopy.Start();   
                }
            }            

            int iFile = 0;
            bool bRet = true;
            foreach (FileOperation fileOperation in lstItems)
            {
                Process build = new Process();

                if (cd == CommandDialog.Copy)
                {
                    if (bShowProgress && pf != null)
                    {
                        pf.SetProgressVisible(false);
                        pf.CurrentPosition = iFile;
                        pf.SetProcess(build, fileOperation.IsFolder ? "1" : "0");
                        pf.SetProgress(fileOperation.Source, 0, fileOperation.Destination);
                    }                   
                }                

                build.StartInfo.WorkingDirectory = Path.GetDirectoryName(sAdb);
                build.StartInfo.FileName = sAdb;
                build.StartInfo.Arguments = fileOperation.Command;

                build.StartInfo.UseShellExecute = false;
                build.StartInfo.RedirectStandardOutput = true;
                build.StartInfo.RedirectStandardError = true;
                build.StartInfo.CreateNoWindow = true;
                build.ErrorDataReceived += build_ErrorDataReceived;
                build.OutputDataReceived += build_ErrorDataReceived;
                build.EnableRaisingEvents = true;
                build.Start();
                build.BeginOutputReadLine();
                build.BeginErrorReadLine();
                build.WaitForExit();

                if (bShowProgress)
                {
                    if (cd == CommandDialog.Copy)
                    {
                        if (build.ExitCode < 0)
                        {
                            if (!string.IsNullOrEmpty(fileOperation.Destination))
                            {
                                if (File.Exists(fileOperation.Destination))
                                {
                                    File.Delete(fileOperation.Destination);
                                }
                                else if (Directory.Exists(fileOperation.Destination))
                                {
                                    //Directory.Delete(sDestination, true);
                                }
                            }

                            if (pf != null)
                            {
                                pf.SetProgress(string.Empty, 0);
                                pf.InternalCloseDialog();
                                pf = null;   
                            }                            

                            break;
                        }
                        else
                        {
                            if (pf != null)
                            {
                                pf.SetProgress(string.Empty, 0);   
                            }                            
                        }                       
                    }                    
                }

                if (strMessage != null && (strMessage.Contains("adb: error:") || strMessage.Contains("Read-only file system") || strMessage.Contains(": Permission denied")))
                {
                    bRet = false;
                    break;
                }

                iFile++;
            }

            if (bShowProgress)
            {
                if (cd == CommandDialog.Copy)
                {
                    if (pf != null)
                    {
                        pf.SetProgress(string.Empty, 0);
                        pf.InternalCloseDialog();
                        pf = null;      
                    }                    
                }
                if (cd == CommandDialog.Delete)
                {
                    if (of != null)
                    {                        
                        of.InternalCloseDialog();
                        of = null;
                    }  
                }
            }

            return bRet;
        }

        public bool LaunchProcess(string args, CommandDialog cd, bool bShowProgress = false, string sDestination = "")
        {
            try
            {                
                strMessage = string.Empty;
                Process build = new Process();

                Thread thDialog = null;
                if (bShowProgress)
                {
                    if (cd == CommandDialog.Copy)
                    {
                        if (pf == null)
                        {
                            pf = new ProgressForm();
                        }
                        pf.SetProcess(build);
                        pf.SetProgress(string.Empty, 0, sDestination);
                        thDialog = new Thread(() => ThreadProcCopy());
                        thDialog.Start();                       
                    }
                    else if (cd == CommandDialog.Connect)
                    {
                        if (cf == null)
                        {
                            cf = new ConnectForm();
                        }
                        DeviseAddr addr = new DeviseAddr();
                        if (!string.IsNullOrEmpty(addr.ConnectionType) && addr.ConnectionType.Equals("usb") &&
                            !string.IsNullOrEmpty(addr.UsbDevice))
                        {
                            cf.SetIP(addr.UsbDevice);
                        }
                        else
                        {
                            cf.SetIP(addr.ToString());    
                        }
                        
                        thDialog = new Thread(() => ThreadProcConnect());
                        thDialog.Start(); 
                    }
                    else if (cd == CommandDialog.CreateScreenShot)
                    {
                        if (of == null)
                        {
                            of = new OperationForm(OperationForm.TypeOp.CreateScreenShot);
                        }
                        of.SetOperation("Creating ScreenShot");
                        thDialog = new Thread(() => ThreadProcOperation());
                        thDialog.Start();
                    }
                    else if (cd == CommandDialog.Install)
                    {
                        if (of == null)
                        {
                            of = new OperationForm(OperationForm.TypeOp.Install);
                        }
                        of.SetOperation("Install " + args.Substring(args.LastIndexOf("\\") + 1).TrimEnd('\"'));
                        thDialog = new Thread(() => ThreadProcOperation());
                        thDialog.Start();
                    }
                    else if (cd == CommandDialog.Delete)
                    {
                        if (of == null)
                        {
                            of = new OperationForm(OperationForm.TypeOp.Delete);
                        }
                        of.SetOperation("Deleting");
                        thDialog = new Thread(() => ThreadProcOperation());
                        thDialog.Start();
                    }
                }

                build.StartInfo.WorkingDirectory = Path.GetDirectoryName(FindADB());

                if (string.IsNullOrEmpty(build.StartInfo.WorkingDirectory))
                {
                    if (bShowProgress && pf != null)
                    {
                        pf.SetProgress(string.Empty, 0);
                        pf.InternalCloseDialog();
                        pf = null;
                    }
                    if (bShowProgress && cf != null)
                    {
                        cf.InternalCloseDialog();
                        cf = null;
                    }
                    if (bShowProgress && of != null)
                    {
                        of.InternalCloseDialog();
                        of = null;
                    }
                    strMessage = "Can not find adb.exe";
                    return false;
                }

                build.StartInfo.FileName = FindADB();
                build.StartInfo.Arguments = args;

                build.StartInfo.UseShellExecute = false;
                build.StartInfo.RedirectStandardOutput = true;
                build.StartInfo.RedirectStandardError = true;
                build.StartInfo.CreateNoWindow = true;
                build.ErrorDataReceived += build_ErrorDataReceived;
                build.OutputDataReceived += build_ErrorDataReceived;
                build.EnableRaisingEvents = true;
                build.Start();
                build.BeginOutputReadLine();
                build.BeginErrorReadLine();
                build.WaitForExit();

                if (bShowProgress)
                {
                    if (cd == CommandDialog.Copy)
                    {
                        if (build.ExitCode < 0)
                        {
                            if (!string.IsNullOrEmpty(sDestination))
                            {
                                if (File.Exists(sDestination))
                                {
                                    File.Delete(sDestination);
                                }
                                else if (Directory.Exists(sDestination))
                                {
                                    //Directory.Delete(sDestination, true);
                                }
                            }
                        }

                        if (pf != null)
                        {
                            pf.SetProgress(string.Empty, 0);
                            pf.InternalCloseDialog();
                            pf = null;
                        }                        
                    }
                    else if (cd == CommandDialog.Connect)
                    {
                        cf.InternalCloseDialog();
                        cf = null;
                    }
                    else if (cd == CommandDialog.CreateScreenShot || cd == CommandDialog.Delete || cd == CommandDialog.Install)
                    {
                        of.InternalCloseDialog();
                        of = null;
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                if (bShowProgress)
                {
                    if (cd == CommandDialog.Copy)
                    {
                        if (pf != null)
                        {
                            pf.SetProgress(string.Empty, 0);
                            pf.InternalCloseDialog();
                            pf = null;
                        }
                    }
                    else if (cd == CommandDialog.Connect)
                    {
                        cf.InternalCloseDialog();
                        cf = null;
                    }
                    else if (cd == CommandDialog.CreateScreenShot || cd == CommandDialog.Delete)
                    {
                        of.InternalCloseDialog();
                        of = null;
                    } 
                }

                strMessage = e.Message;
                return false;
            }            
        }
    }
}
