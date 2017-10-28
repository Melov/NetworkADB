using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsShell.ADB;

namespace WindowsShell.Dialogs
{
    public partial class ConsoleForm : Form
    {
        public string strMessage;
        AutoCompleteStringCollection autoComplete = new AutoCompleteStringCollection();
        
        public ConsoleForm()
        {
            InitializeComponent();

            tbConsole.BackColor = System.Drawing.SystemColors.Window;
            toolStripStatusLabel1.Text = ADBRunner.FindADB();            
        }

        private void ConsoleForm_Load(object sender, EventArgs e)
        {
            tbCommand.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            tbCommand.AutoCompleteSource = AutoCompleteSource.CustomSource;            
            tbCommand.AutoCompleteCustomSource = autoComplete;

            tbCommand.Focus();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                autoComplete.Add(tbCommand.Text);
                AppendText(tbCommand.Text, Color.DarkBlue, true);
                RunCommandNew(tbCommand.Text);
            }
        }

        private void tbConsole_TextChanged(object sender, EventArgs e)
        {          
            tbConsole.SelectionStart = tbConsole.Text.Length;         
            tbConsole.ScrollToCaret();
        }

        private void AppendText(string text, Color color, bool newLine = false)
        {
            if (newLine)
            {
                text += Environment.NewLine;
            }

            tbConsole.SelectionStart = tbConsole.TextLength;
            tbConsole.SelectionLength = 0;

            tbConsole.SelectionColor = color;
            tbConsole.AppendText(text);
            tbConsole.SelectionColor = tbConsole.ForeColor;
        }

        private void RunCommandNew(string sCommand)
        {
            try
            {
                if (!consoleControl1.IsProcessRunning)
                {
                    consoleControl1.ClearOutput();                   
                    consoleControl1.Focus();
                    consoleControl1.StartProcess(ADBRunner.FindADB(), sCommand);   
                }                

                if (consoleControl1.IsProcessRunning)
                {
                    if (!consoleControl1.Focused)
                        consoleControl1.Focus();
                    tbCommand.Enabled = false;
                }
                else
                {
                    tbCommand.Enabled = true;
                    tbCommand.Focus();
                }
            }
            catch (Exception)
            {
                
            }            
        }

        private void RunCommand(string sCommand)
        {            
            strMessage = string.Empty;            
            Process build = new Process();
            string strADB = ADBRunner.FindADB();
            build.StartInfo.WorkingDirectory = Path.GetDirectoryName(strADB);
            build.StartInfo.FileName = strADB;
            build.StartInfo.Arguments = sCommand;

            build.StartInfo.UseShellExecute = false;
            build.StartInfo.RedirectStandardOutput = true;
            //build.StandardInput.AutoFlush = true;
            build.StartInfo.RedirectStandardInput = true;
            build.StartInfo.RedirectStandardError = true;
            build.StartInfo.CreateNoWindow = true;
            build.ErrorDataReceived += build_ErrorDataReceived;
            build.OutputDataReceived += build_ErrorDataReceived;
            build.EnableRaisingEvents = true;            
            build.Start();
            
            StreamWriter inputWriter = build.StandardInput;
            //StreamReader outputReader = build.StandardOutput;

            build.BeginOutputReadLine();
            build.BeginErrorReadLine();            

            foreach (ProcessThread thread in build.Threads)
                if (thread.ThreadState == ThreadState.Wait
                    && thread.WaitReason == ThreadWaitReason.UserRequest)
                {
                    inputWriter.WriteLine("exit");
                    //build.StandardInput.Close(); 
                    /*
                    string ss = outputReader.ReadToEnd();
                    ss = Encoding.UTF8.GetString(Encoding.Default.GetBytes(ss));
                    AppendText(ss, tbConsole.ForeColor, true);
                    */
                }
            
            build.WaitForExit();
        }

        void build_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            string log = e.Data;

            if (!string.IsNullOrEmpty(log))
            {
                log = Encoding.UTF8.GetString(Encoding.Default.GetBytes(log));
                AppendText(log, tbConsole.ForeColor, true);
            }
        }

        private void lnkADB_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://developer.android.com/studio/command-line/adb.html");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (consoleControl1.IsProcessRunning)
                {                    
                    if (tbCommand.Enabled)
                    {
                        tbCommand.Enabled = false;
                        if (!consoleControl1.Focused)
                            consoleControl1.Focus();
                    }
                        
                }
                else
                {
                    tbCommand.Enabled = true;
                    //tbCommand.Focus();
                }
            }
            catch (Exception)
            {
                
            }            
        }
    }
}
