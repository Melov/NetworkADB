using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsShell.Dialogs
{
    public partial class ConnectForm : Form
    {
        public ConnectForm()
        {
            InitializeComponent();
        }
        
        delegate void SetTextCallback(Control c, string value);
        private void SetTextValue(Control c, string value)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (c.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetTextValue);
                try
                {
                    this.Invoke(d, new object[] { c, value });
                }
                catch (Exception)
                {

                }
            }
            else
            {
                c.Text = value;
            }
        }

        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }

        public void SetIP(string sIP)
        {
            SetTextValue(lbIP,sIP);
        }


        public void InternalCloseDialog()
        {
            SetClose();
        }

        delegate void SetCloseCallback();
        private void SetClose()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.InvokeRequired)
            {
                SetCloseCallback d = new SetCloseCallback(SetClose);
                try
                {
                    this.Invoke(d, new object[] { });
                }
                catch (Exception)
                {
                    
                }                
            }
            else
            {
                this.Close();
            }
        }
    }

}
