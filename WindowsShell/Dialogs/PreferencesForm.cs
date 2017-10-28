using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using WindowsShell.ADB;

namespace WindowsShell.Dialogs
{    
    public partial class PreferencesForm : Form
    {        
        public PreferencesForm()
        {
            InitializeComponent();

            DeviseAddr addr = new DeviseAddr();
            if (addr.IsValid())
            {
                tbIP.Text = addr.IP;
                tbPort.Text = addr.PORT;
            }

            if (!string.IsNullOrEmpty(addr.ADBPath))
            {
                tbADB.Text = addr.ADBPath;
            }
            else
            {
                tbADB.Text = ADBRunner.FindADB();    
            }

            if (!string.IsNullOrEmpty(addr.ConnectionType))
            {
                rbUSB.Checked = addr.ConnectionType.Equals("usb");
            }

            cbDevices.Items.Clear();
            ADBCommand commandDev = new ADBCommand();
            CommandResult retDev = commandDev.Devices();
            if (retDev.IsSuccess)
            {
                foreach (var str in retDev.Message.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        string[] s = str.Split(' ');
                        if (s.Any())
                        {
                            cbDevices.Items.Add(s[0]);       
                        }
                    }                    
                }
            }

            if (!string.IsNullOrEmpty(addr.UsbDevice))
            {
                if (cbDevices.Items.Contains(addr.UsbDevice))
                {
                    cbDevices.SelectedItem = addr.UsbDevice;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DeviseAddr addr = new DeviseAddr(tbIP.Text, tbPort.Text);
            if (addr.IsValid())
            {
                addr.Save(rbIP.Checked, cbDevices.SelectedItem != null ? cbDevices.SelectedItem.ToString() : "", tbADB.Text);
            }
            else
            {
                DeviseAddr addr1 = new DeviseAddr("192.168.100.1", "5555");
                addr1.Save(rbIP.Checked, cbDevices.SelectedItem != null ? cbDevices.SelectedItem.ToString() : "", tbADB.Text);
            }

            Close();
        }

        private void PreferencesForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void rbUSB_CheckedChanged(object sender, EventArgs e)
        {
            SetTypeConnect();
        }

        private void rbIP_CheckedChanged(object sender, EventArgs e)
        {
            SetTypeConnect();
        }

        private void SetTypeConnect()
        {
            tbIP.Enabled = !rbUSB.Checked;
            tbPort.Enabled = !rbUSB.Checked;
            cbDevices.Enabled = rbUSB.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "adb.exe";
            if (string.IsNullOrEmpty(tbADB.Text))
            {
                openFileDialog1.InitialDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
            else
            {
                openFileDialog1.InitialDirectory = Path.GetDirectoryName(tbADB.Text);
            }

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileInfo fi = new FileInfo(openFileDialog1.FileName);
                if (fi.Exists)
                {
                    tbADB.Text = openFileDialog1.FileName;   
                }
                else
                {
                    MessageBox.Show("File not found!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
