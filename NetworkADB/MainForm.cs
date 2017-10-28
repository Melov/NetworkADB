using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using NetworkADB.Properties;

namespace NetworkADB
{
    public partial class MainForm : Form
    {
        [DllImport("user32.dll")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(int hWnd, uint Msg, int wParam, int lParam);

        public MainForm()
        {
            InitializeComponent();
        }

        private void UnpackFiles()
        {
            string sPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\";

            string adbExe = sPath + "adb.exe";
            FileInfo fiAE = new FileInfo(adbExe);
            if (!fiAE.Exists)
            {
                File.WriteAllBytes(adbExe, Resources.adb1);
            }
            string adbd1 = sPath + "adbwinapi.dll";
            FileInfo fiD1 = new FileInfo(adbd1);
            if (!fiD1.Exists)
            {
                File.WriteAllBytes(adbd1, Resources.AdbWinApi);
            }
            string adbd2 = sPath + "adbwinusbapi.dll";
            FileInfo fiD2 = new FileInfo(adbd2);
            if (!fiD2.Exists)
            {
                File.WriteAllBytes(adbd2, Resources.AdbWinUsbApi);
            }
            string ra = sPath + "regasm.exe";
            FileInfo fiRa = new FileInfo(ra);
            if (!fiRa.Exists)
            {
                File.WriteAllBytes(ra, Resources.RegAsm);
            }

            string ws = sPath + "windowsshell.dll";
            FileInfo fiws = new FileInfo(ws);
            if (!fiws.Exists)
            {
                File.WriteAllBytes(ws, Resources.WindowsShell);
            }
            string ext = sPath + "nsex3.dll";
            FileInfo fiext = new FileInfo(ext);
            if (!fiext.Exists)
            {
                File.WriteAllBytes(ext, Resources.NsEx3);
            }
        }

        private void RegUnregDlls(bool bReg)
        {
            string sPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\";
            string sExe = sPath + "regasm.exe";
            string sDll1 = sPath + "nsex3.dll";
            string sDll2 = sPath + "windowsshell.dll";
            if (sDll1.IndexOf(" ") > 0)
                sDll1 = "\"" + sDll1 + "\"";
            if (sDll2.IndexOf(" ") > 0)
                sDll2 = "\"" + sDll2 + "\"";

            var proc = System.Diagnostics.Process.Start(sExe, bReg ? sDll1 + " /codebase" : "/u " + sDll1);
            proc.WaitForExit();
            proc.Close();
            proc = System.Diagnostics.Process.Start(sExe, bReg ? sDll2 + " /codebase" : "/u " + sDll2);
            proc.WaitForExit();
            proc.Close();
        }

        private void DeleteFiles()
        {
            string sPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\";

            string adbExe = sPath + "adb.exe";
            FileInfo fiAE = new FileInfo(adbExe);
            if (fiAE.Exists)
            {
                try
                {
                    File.Delete(adbExe);
                }
                catch (Exception)
                {
                    Process[] prcChecker = Process.GetProcessesByName("adb");
                    if (prcChecker.Length > 0)
                    {
                        foreach (Process p in prcChecker)
                        {
                            p.Kill();
                        }
                    }
                    try
                    {
                        File.Delete(adbExe);
                    }
                    catch (Exception)
                    {
                        
                    }
                }                
            }
            string adbd1 = sPath + "adbwinapi.dll";
            FileInfo fiD1 = new FileInfo(adbd1);
            if (fiD1.Exists)
            {
                try
                {
                    File.Delete(adbd1);
                }
                catch (Exception)
                {
                }                
            }
            string adbd2 = sPath + "adbwinusbapi.dll";
            FileInfo fiD2 = new FileInfo(adbd2);
            if (fiD2.Exists)
            {
                try
                {
                    File.Delete(adbd2);
                }
                catch (Exception)
                {
                }                
            }
            string ra = sPath + "regasm.exe";
            FileInfo fiRa = new FileInfo(ra);
            if (fiRa.Exists)
            {
                try
                {
                    File.Delete(ra);
                }
                catch (Exception)
                {
                }                 
            }

            string ws = sPath + "windowsshell.dll";
            FileInfo fiws = new FileInfo(ws);
            if (fiws.Exists)
            {
                try
                {
                    File.Delete(ws);
                }
                catch (Exception)
                {
                }                
            }
            string ext = sPath + "nsex3.dll";
            FileInfo fiext = new FileInfo(ext);
            if (fiext.Exists)
            {
                try
                {
                    File.Delete(ext);
                }
                catch (Exception)
                {
                }                 
            }
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {            
            ReloadExplorer();
            UnpackFiles();
            RegUnregDlls(true);
            RunExplorer(true);
            
            //RunExplorer(false);
        }

        private void btnUninstall_Click(object sender, EventArgs e)
        {
            ReloadExplorer();
            RegUnregDlls(false);
            DeleteFiles();
            RunExplorer(true);
        }

        private void lnkNick_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://4pda.ru/forum/index.php?showtopic=787749&st=60");
        }

        private void KillExplorer()
        {
            int hwnd;
            hwnd = FindWindow("Progman", null);
            PostMessage(hwnd, /*WM_QUIT*/ 0x12, 0, 0);
        }

        private void ReloadExplorer()
        {
            RegistryKey regKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default).OpenSubKey(@"Software\Microsoft\Windows NT\CurrentVersion\Winlogon", RegistryKeyPermissionCheck.ReadWriteSubTree);
            if (regKey.GetValue("AutoRestartShell").ToString() == "1")
                regKey.SetValue("AutoRestartShell", 0, RegistryValueKind.DWord);

            Process[] prcChecker = Process.GetProcessesByName("explorer");
            if (prcChecker.Length > 0)
            {
                foreach (Process p in prcChecker)
                {
                    p.ForceKill();
                }
            }

            regKey.SetValue("AutoRestartShell", 1);
        }

        private void RunExplorer(bool bClear)
        {
            if (!bClear)
            {
                string myComputerPath = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
                if (string.IsNullOrEmpty(myComputerPath))
                {
                    myComputerPath = "::{20d04fe0-3aea-1069-a2d8-08002b30309d}";
                }

                Process.Start(Path.Combine(Environment.GetEnvironmentVariable("windir"), "explorer.exe"));//, "/root," + myComputerPath);//+ "\\::{57852db1-0d17-4e7f-b381-35627bed7eac}");    
            }
            else
            {
                Process.Start(Path.Combine(Environment.GetEnvironmentVariable("windir"), "explorer.exe"));
            }
            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Process.Start("https://forum.xda-developers.com/android/software-hacking/network-adb-extension-windows-explorer-t3519741");
        }
    }
}
