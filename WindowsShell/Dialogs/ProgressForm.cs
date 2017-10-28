using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using WindowsShell.Nspace;

namespace WindowsShell.Dialogs
{
    public partial class ProgressForm : Form
    {
        private bool _ShowDetails = false;
        private bool ShowDetails
        {
            get { return _ShowDetails; }
            set { _ShowDetails = value; }
        }

        private int _CurrentPosition = 0;
        public int CurrentPosition
        {
            get { return _CurrentPosition; }
            set
            {
                _CurrentPosition = value;
                for (int i = 0; i < _CurrentPosition; i++)
                {
                    SetIcon(i, 1);
                    //lvFiles.Items[i].ImageIndex = 1;
                }
                SetEnsureVisible(_CurrentPosition);
                //lvFiles.Items[_CurrentPosition].EnsureVisible();

                string text = string.Format("[{0}/{1}] Details...", _CurrentPosition + 1, lvFiles.Items.Count);
                SetTextValue(btnDetails, text);
            }
        }

        private List<FileOperation> _Files = new List<FileOperation>();
        public List<FileOperation> Files
        {
            get { return _Files; }
            set
            {
                _Files = value;
                ItemsClear();
                foreach (FileOperation file in _Files)
                {
                    string sf = file.Source.Replace("//", "/").RemoveFromEnd("/.");
                    sf = PathLabel.GetPathText(sf, lvFiles.Font, new Size(lvFiles.Columns[0].Width-20, 100));
                    lvFiles.Items.Add(file.Source, sf, 0);
                }
                if (_Files.Count > 1)
                {
                    btnDetails.Visible = true;
                    lvFiles.Visible = true;
                }
            }
        }

        private Process proc = null;

        PathLabel sl = new PathLabel();
        PathLabel dl = new PathLabel();


        public ProgressForm()
        {
            InitializeComponent();

            pbS.Visible = false;

            this.picBox.Image = global::WindowsShell.Properties.Resources.copy;

            dl.Anchor = this.lbDestination.Anchor;
            dl.AutoEllipsis = this.lbDestination.AutoEllipsis;
            dl.Location = this.lbDestination.Location;
            dl.Name = this.lbDestination.Name + "1";
            dl.Size = this.lbDestination.Size;
            dl.TabIndex = this.lbDestination.TabIndex;
            dl.Visible = true;
            dl.AutoSize = false;

            sl.Anchor = this.lbSource.Anchor;
            sl.AutoEllipsis = this.lbSource.AutoEllipsis;
            sl.Location = this.lbSource.Location;
            sl.Name = this.lbSource.Name + "1";
            sl.Size = this.lbSource.Size;
            sl.TabIndex = this.lbSource.TabIndex;
            sl.Visible = true;
            sl.AutoSize = false;

            this.Controls.Add(dl);
            this.Controls.Add(sl);
            this.lbSource.Visible = false;
            this.lbDestination.Visible = false;
        }

        public void InternalCloseDialog()
        {
            SetClose();
        }

        delegate void ItemsClearCallback();
        private void ItemsClear()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.lvFiles.InvokeRequired)
            {
                ItemsClearCallback d = new ItemsClearCallback(ItemsClear);
                try
                {
                    this.Invoke(d, new object[] {  });
                }
                catch (Exception)
                {

                }
            }
            else
            {
                lvFiles.Items.Clear();
            }
        }

        delegate void SetEnsureVisibleCallback(int item);
        private void SetEnsureVisible(int item)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.lvFiles.InvokeRequired)
            {
                SetEnsureVisibleCallback d = new SetEnsureVisibleCallback(SetEnsureVisible);
                try
                {
                    this.Invoke(d, new object[] { item });
                }
                catch (Exception)
                {
                    
                }                
            }
            else
            {
                lvFiles.Items[item].EnsureVisible();                
            }
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

        delegate void SetHeightCallback(int value);
        private void SetFormHeightValue(int value)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.InvokeRequired)
            {
                SetHeightCallback d = new SetHeightCallback(SetFormHeightValue);
                try
                {
                    this.Invoke(d, new object[] { value });
                }
                catch (Exception)
                {
                    
                }                
            }
            else
            {
                this.Height = value;
            }
        }

        delegate void SetTextCallback(Control c , string value);
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

        delegate void SetProgressCallback(int percent);
        private void SetValue(int value)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.pbS.InvokeRequired)
            {
                SetProgressCallback d = new SetProgressCallback(SetValue);
                try
                {
                    this.Invoke(d, new object[] { value });
                }
                catch (Exception)
                {
                    
                }                
            }
            else
            {
                this.pbS.Value = value;
            }
        }

        delegate void SetProgressCallback1(int percent);
        private void SetValue1(int value)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.pb.InvokeRequired)
            {
                SetProgressCallback1 d = new SetProgressCallback1(SetValue1);
                try
                {
                    this.Invoke(d, new object[] { value });
                }
                catch (Exception)
                {
                    
                }                
            }
            else
            {
                this.pb.Value = value;
            }
        }

        delegate void SetProgressVisibleCallback(bool visible);
        public void SetProgressVisible(bool visible)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.pbS.InvokeRequired)
            {
                SetProgressVisibleCallback d = new SetProgressVisibleCallback(SetProgressVisible);
                try
                {
                    this.Invoke(d, new object[] { visible });
                }
                catch (Exception)
                {
                    
                }                
            }
            else
            {
                this.pbS.Visible = visible;
            }
        }

        delegate void SetIconCallback(int item, int value);
        private void SetIcon(int item, int value)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.lvFiles.InvokeRequired)
            {
                SetIconCallback d = new SetIconCallback(SetIcon);
                try
                {
                    this.Invoke(d, new object[] { item, value });
                }
                catch (Exception)
                {
                    
                }                
            }
            else
            {
                lvFiles.Items[item].ImageIndex = value;
            }
        }
        

        public void SetProgress(string sSource, int percent, string sDestination = "")
        {
            //try
            {
                if (!string.IsNullOrEmpty(sSource))
                {
                    sSource = sSource.Trim();
                    sSource = sSource.Replace("//", "/");
                }
                else
                {
                    SetValue(0);
                }
                
                if (!string.IsNullOrEmpty(sSource) && sSource.EndsWith("%"))
                {                    
                    int lastP = sSource.LastIndexOf(" ");
                    if (lastP > 0)
                    {
                        string sPercents = sSource.Substring(lastP + 1);
                        sPercents = sPercents.Trim().Replace("%", "");
                        int nPercentSub = int.Parse(sPercents);
                        sSource = sSource.Substring(0, lastP).Trim().TrimEnd(':');
                        SetProgressVisible(true);

                        SetValue(nPercentSub);
                        //pbS.Value = nPercentSub;
                        //pbS.Invalidate();
                    }
                }
                else
                {
                    //if (!string.IsNullOrEmpty(sSource))
                    {
                       SetProgressVisible(false);
                    }
                }

                if (!string.IsNullOrEmpty(sDestination))
                {
                    SetTextValue(dl, sDestination);
                }

                SetTextValue(sl,sSource);
                SetValue1(percent);

                SetTextValue(this, string.Format("[{0}%] Copying...", percent));
            }
            //catch (Exception)
            {
                
            }            
        }

        public void SetProcess(Process proc, string sFolder = "")
        {
            this.proc = proc;
            if (!string.IsNullOrEmpty(sFolder))
            {
                SetProgressVisible(sFolder.Equals("1"));
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (proc != null)
            {
                try
                {
                    //proc.CloseMainWindow();                    
                    proc.Kill();                
                }
                catch (Exception)
                {
                    //e.Message;
                }                
            }
            InternalCloseDialog();
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

        private void btnDetails_Click(object sender, EventArgs e)
        {
            ShowDetails = !ShowDetails;

            if (ShowDetails)
            {
                SetFormHeightValue(Height + 120);
                //Height += 120;
            }
            else
            {
                SetFormHeightValue(Height - 120);                
                //Height -= 120;
            }
        } 
    }
}
