using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using WindowsShell.ADB;
using WindowsShell.Interop;
using WindowsShell.Nspace;

namespace WindowsShell.Dialogs
{
    public partial class PermissionsForm : Form
    {
        public const string COMMAND_REMOUNT = "mount -o remount,rw [DIR]";
        public const string COMMAND_CHANGE_PERM = "chmod {0}[OCTAL] [DIR]";
        public const string COMMAND_CHANGE_USERGROUP = "chown [OWNER].[GROUP] [DIR]";

        public const string COMMAND_USE_SU = "shell \"su -c '{0}'\"";

        private IFolderObject _folderObj;

        public enum UserList
        {
            root,
            system,
            radio,
            bluetooth,
            graphics,
            input,
            audio,
            camera,
            log,
            compass,
            mount,
            wifi,
            adb,
            install,
            media,
            dhcp,
            sdcard_rw,
            vpn,
            keystore,
            usb,
            drm,
            mdnsr,
            gps,
            media_rw,
            mtp,
            drmrpc,
            nfc,
            sdcard_r,
            clat,
            loop_radio,
            mediadrm,
            package_info,
            sdcard_pics,
            sdcard_av,
            sdcard_all,
            shell,
            cache,
            diag,
            net_bt_admin,
            net_bt,
            inet,
            net_raw,
            net_admin,
            net_bw_stats,
            net_bw_acct,
            net_bw_stack,
            misc,
            nobody            
        }

        public enum ObjType
        {
            None = 0,
            File = 1,
            Folder = 2,
            Link = 3
        }

        private char[] _validateArr = new[] { '0', '1', '2', '3', '4', '5', '6', '7' };

        private IFolderObject[] Items;

        public PermissionsForm()
        {
            InitializeComponent();

            BindUserGroups();
            SetCommands();
            cbDetails.Checked = false;
            ShowDetails();
        }

        public void BindUserGroups()
        {
            cbOwner.Items.Clear();
            cbGroup.Items.Clear();
            cbOwner.Items.Add("");
            cbGroup.Items.Add("");

            foreach (string name in Enum.GetNames(typeof(UserList)))
            {
                cbOwner.Items.Add(name);
                cbGroup.Items.Add(name);
            }
        }

        public void SetCommands()
        {
            tbRemount.Enabled = cbRemount.Checked;
            tbRemount.Text = cbRoot.Checked ? string.Format(COMMAND_USE_SU, COMMAND_REMOUNT) : "shell " + COMMAND_REMOUNT;
            tbPermissions.Text = cbRoot.Checked ? string.Format(COMMAND_USE_SU, string.Format(COMMAND_CHANGE_PERM, cbRecur.Checked ? "-R " : "")) : "shell " + string.Format(COMMAND_CHANGE_PERM, cbRecur.Checked ? "-R " : "");
            tbOwnerGroup.Text = cbRoot.Checked ? string.Format(COMMAND_USE_SU, string.Format(COMMAND_CHANGE_USERGROUP, cbRecur.Checked ? "-R " : "")) : "shell " + string.Format(COMMAND_CHANGE_USERGROUP, cbRecur.Checked ? "-R " : "");
        }

        private void GetAttributeOwnerGroup(IFolderObject item, ref string sAttr, ref string sOwner, ref string sGroup)
        {                        
            Column columnAttr = null;
            Column columnOwner = null;
            Column columnGroup = null;
            foreach (Column column in Items[0].Columns)
            {
                if (column.Name.ToLower().Equals("attributes"))
                {
                    columnAttr = column;
                }
                if (column.Name.ToLower().Equals("owner"))
                {
                    columnOwner = column;
                }
                if (column.Name.ToLower().Equals("group"))
                {
                    columnGroup = column;
                }
            }
            if (columnAttr != null)
            {
                object obj = Items[0].GetColumnValue(columnAttr);
                if (obj != null)
                {
                    sAttr = obj.ToString();
                }
            }
            if (columnOwner != null)
            {
                object obj = Items[0].GetColumnValue(columnOwner);
                if (obj != null)
                {
                    sOwner = obj.ToString();                   
                }
            }
            if (columnGroup != null)
            {
                object obj = Items[0].GetColumnValue(columnGroup);
                if (obj != null)
                {
                    sGroup = obj.ToString();                    
                }
            }
        }

        public void SetData(IFolderObject[] items, IFolderObject folderObj)
        {
            _folderObj = folderObj;
            Items = items;

            if (Items.Count() == 1)
            {
                string sAttr = null;
                string sOwner = null;
                string sGroup = null;
                GetAttributeOwnerGroup(Items[0], ref sAttr, ref sOwner, ref sGroup);

                if (!cbOwner.Items.Contains(sOwner))
                {
                    cbOwner.Items.Add(sOwner);
                }
                cbOwner.Text = sOwner;

                if (!cbGroup.Items.Contains(sGroup))
                {
                    cbGroup.Items.Add(sGroup);
                }
                cbGroup.Text = sGroup;

                if (!string.IsNullOrEmpty(sAttr))
                {
                    this.Text = string.Format("P[{0}] O[{1}] G[{2}] {3}", sAttr, sOwner, sGroup,
                        string.Format("/{0}", Items[0].PathString));
                    ParseAttribute(sAttr);
                }
                else
                {
                    //clear all data
                }
            }
        }

        private void ParseAttribute(string sAttr)
        {
            ObjType type = ObjType.None;
            if (sAttr[0] == '-')
                type = ObjType.File;
            if (sAttr[0] == 'd')
                type = ObjType.Folder;
            if (sAttr[0] == 'l')
                type = ObjType.Link;


            cbOwRead.Checked = sAttr[1] == 'r';
            cbOwWrite.Checked = sAttr[2] == 'w';
            cbOwExecute.Checked = (sAttr[3] == 'x') || (sAttr[3] == 's');

            cbGrRead.Checked = sAttr[4] == 'r';
            cbGrWrite.Checked = sAttr[5] == 'w';
            cbGrExecute.Checked = (sAttr[6] == 'x') || (sAttr[6] == 's');

            cbSUID.Checked = sAttr[3] == 's' || sAttr[3] == 'S';
            cbSGID.Checked = sAttr[6] == 's' || sAttr[6] == 'S';

            cbOtRead.Checked = sAttr[7] == 'r';
            cbOtWrite.Checked = sAttr[8] == 'w';
            cbOtExecute.Checked = (sAttr[9] == 'x') || (sAttr[9] == 't');

            cbSticky.Checked = sAttr[9] == 't' || sAttr[9] == 'T';

            tbOctal.Text = GetOctal();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            lvResult.Items.Clear();

            bool bRemount = false;
            string sCommandRemount = null;

            ADBRunner runner = new ADBRunner();

            foreach (IFolderObject item in Items)
            {
                string spath = string.Format("/{0}", item.PathString);
                spath = spath.Replace(" ", "\\ ")
                    .Replace("(", "\\(")
                    .Replace(")", "\\)")
                    .Replace("'", "\\'")
                    .Replace("&", "\\&");

                string sAttr = null;
                string sOwner = null;
                string sGroup = null;
                GetAttributeOwnerGroup(Items[0], ref sAttr, ref sOwner, ref sGroup);

                if (cbApplyOwnerGroup.Checked)
                {
                    if (!string.IsNullOrEmpty(cbOwner.Text) || !string.IsNullOrEmpty(cbGroup.Text))
                    {
                        if (!cbOwner.Text.Equals(sOwner) || !cbGroup.Text.Equals(sGroup))
                        {
                            string sCommandOwnerGroup = tbOwnerGroup.Text;
                            sCommandOwnerGroup = sCommandOwnerGroup.Replace("[DIR]", spath);
                            sCommandOwnerGroup = sCommandOwnerGroup.Replace("[OWNER]", !string.IsNullOrEmpty(cbOwner.Text) ? cbOwner.Text : sOwner);
                            sCommandOwnerGroup = sCommandOwnerGroup.Replace("[GROUP]", !string.IsNullOrEmpty(cbGroup.Text) ? cbGroup.Text : sGroup);
                            bool bret = runner.LaunchProcess(ADBCommand.SelectDevice(sCommandOwnerGroup), CommandDialog.None, false);
                            AddResult(string.Format("/{0}", item.PathString), sCommandOwnerGroup, runner.strMessage);
                            if ((cbRemount.Checked) && (runner.strMessage.Contains("Read-only file system")))
                            {
                                sCommandRemount = tbRemount.Text;
                                string sParrent = spath.Substring(0, spath.LastIndexOf('/') + 1);
                                sCommandRemount = sCommandRemount.Replace("[DIR]", sParrent);
                                bret = runner.LaunchProcess(ADBCommand.SelectDevice(sCommandRemount), CommandDialog.None, false);
                                AddResult("Remount to RW", "", "");
                                AddResult(string.Format("/{0}", item.PathString), sCommandRemount, runner.strMessage);
                                if (string.IsNullOrEmpty(runner.strMessage))
                                {
                                    bRemount = true;
                                }
                                bret = runner.LaunchProcess(ADBCommand.SelectDevice(sCommandOwnerGroup), CommandDialog.None, false);
                                AddResult(string.Format("/{0}", item.PathString), sCommandOwnerGroup, runner.strMessage);
                            }   
                        }
                        else
                        {
                            AddResult(string.Format("/{0}", item.PathString), "Owner & Group not changed", "Skeep");
                        }                        
                    }                    
                }

                if (cbApplyPerm.Checked)
                {
                    string sCommandPerm = tbPermissions.Text;
                    sCommandPerm = sCommandPerm.Replace("[DIR]", spath);
                    sCommandPerm = sCommandPerm.Replace("[OCTAL]", int.Parse(GetOctal()).ToString());
                    bool bret = runner.LaunchProcess(ADBCommand.SelectDevice(sCommandPerm), CommandDialog.None, false);
                    AddResult(string.Format("/{0}", item.PathString), sCommandPerm, runner.strMessage);
                    if ((cbRemount.Checked) && (runner.strMessage.Contains("Read-only file system")))
                    {
                        sCommandRemount = tbRemount.Text;
                        string sParrent = spath.Substring(0, spath.LastIndexOf('/') + 1);
                        sCommandRemount = sCommandRemount.Replace("[DIR]", sParrent);
                        bret = runner.LaunchProcess(ADBCommand.SelectDevice(sCommandRemount), CommandDialog.None, false);
                        AddResult("Remount to RW", "", "");
                        AddResult(string.Format("/{0}", item.PathString), sCommandRemount, runner.strMessage);
                        if (string.IsNullOrEmpty(runner.strMessage))
                        {
                            bRemount = true;
                        }
                        bret = runner.LaunchProcess(ADBCommand.SelectDevice(sCommandPerm), CommandDialog.None, false);
                        AddResult(string.Format("/{0}", item.PathString), sCommandPerm, runner.strMessage);
                    }
                }
            }

            if (bRemount && !string.IsNullOrEmpty(sCommandRemount))
            {
                sCommandRemount = sCommandRemount.Replace(",rw ", ",ro ");
                bool bret = runner.LaunchProcess(ADBCommand.SelectDevice(sCommandRemount), CommandDialog.None, false);
                AddResult("Remount back to RO", sCommandRemount, runner.strMessage);
            }

            if (Items.Any())
            {
                if (_folderObj!= null && _folderObj.ShellView != null)
                {
                    
                    _folderObj.ShellView.Refresh();

                    foreach (IFolderObject item in Items)
                    {
                        List<byte[]> ls = new List<byte[]>();
                        ls.Add(item.PathData[item.PathData.Length - 1]);
                        int rt = _folderObj.ShellView.SelectItem(
                            ItemIdList.Create(null, (byte[][])ls.ToArray().Clone()).Ptr,
                            _SVSIF.SVSI_SELECT | _SVSIF.SVSI_SELECTIONMARK);                       
                    }
                    
                }
               
                /*
                IntPtr pDelObj = ItemIdList.Create(null, Items[0].PathData).Ptr;
                Shell32.SHChangeNotify(ShellChangeEvents.Delete,
                    ShellChangeFlags.IdList | ShellChangeFlags.Flush,
                    pDelObj,
                    IntPtr.Zero);
                Marshal.FreeCoTaskMem(pDelObj);
                 */
               /*
                List<byte[]> bt = ((byte[][])(Items[0].PathData.Clone())).ToList();
                //bt.RemoveAt(bt.Count - 1);
                //bt.RemoveAt(bt.Count - 1);
                IntPtr pParrent = ItemIdList.Create(null, bt.ToArray()).Ptr;
                Shell32.SHChangeNotify(ShellChangeEvents.UpdateDir,
                            ShellChangeFlags.IdList | ShellChangeFlags.Flush,
                            pParrent,
                            IntPtr.Zero);
                Marshal.FreeCoTaskMem(pParrent);   
                 */
            }            

            MessageBox.Show("Operation Complete!\r\nClick 'Details' button to view result log","Information",MessageBoxButtons.OK, MessageBoxIcon.Information);

            //DialogResult = DialogResult.OK;

            //Close();
            //-s 127.0.0.1:62001 shell "su -c 'chmod 777 /data/local/tmp'"
            //-s 127.0.0.1:62001 shell "su -c 'chown root.root /data/local/tmp'"
            //-s 127.0.0.1:62001 shell "su -c 'mount -o remount,rw /'"            
        }

        private void AddResult(string sPath, string sCommand, string sResult)
        {
            ListViewItem lvi = lvResult.Items.Add(sPath, sPath, -1);
            lvi.SubItems.Add(sCommand);
            lvi.SubItems.Add(string.IsNullOrEmpty(sResult) ? "Ok" : sResult);
        }

        private void btnCancell_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void SetAll(bool bChecked)
        {
            cbOwRead.Checked = bChecked;
            cbOwWrite.Checked = bChecked;
            cbOwExecute.Checked = bChecked;

            cbGrRead.Checked = bChecked;
            cbGrWrite.Checked = bChecked;
            cbGrExecute.Checked = bChecked;

            cbOtRead.Checked = bChecked;
            cbOtWrite.Checked = bChecked;
            cbOtExecute.Checked = bChecked;

            cbSUID.Checked = false;
            cbSGID.Checked = false;
            cbSticky.Checked = false;
        }

        private string GetOctal()
        {
            int i1 = cbSUID.Checked ? int.Parse(cbSUID.Tag.ToString()) : 0;
            int i2 = cbSGID.Checked ? int.Parse(cbSGID.Tag.ToString()) : 0;
            int i3 = cbSticky.Checked ? int.Parse(cbSticky.Tag.ToString()) : 0;

            int iow1 = cbOwRead.Checked ? int.Parse(cbOwRead.Tag.ToString()) : 0;
            int iow2 = cbOwWrite.Checked ? int.Parse(cbOwWrite.Tag.ToString()) : 0;
            int iow3 = cbOwExecute.Checked ? int.Parse(cbOwExecute.Tag.ToString()) : 0;

            int igr1 = cbGrRead.Checked ? int.Parse(cbGrRead.Tag.ToString()) : 0;
            int igr2 = cbGrWrite.Checked ? int.Parse(cbGrWrite.Tag.ToString()) : 0;
            int igr3 = cbGrExecute.Checked ? int.Parse(cbGrExecute.Tag.ToString()) : 0;

            int iot1 = cbOtRead.Checked ? int.Parse(cbOtRead.Tag.ToString()) : 0;
            int iot2 = cbOtWrite.Checked ? int.Parse(cbOtWrite.Tag.ToString()) : 0;
            int iot3 = cbOtExecute.Checked ? int.Parse(cbOtExecute.Tag.ToString()) : 0;

            return string.Format("{0}{1}{2}{3}", i1 + i2 + i3, iow1 + iow2 + iow3, igr1 + igr2 + igr3, iot1 + iot2 + iot3);
        }

        private void SetOctal(string sOctal)
        {
            if (sOctal.Length == 4)
            {
                int i1 = int.Parse(sOctal[0].ToString());
                int i2 = int.Parse(sOctal[1].ToString());
                int i3 = int.Parse(sOctal[2].ToString());
                int i4 = int.Parse(sOctal[3].ToString());

                cbSUID.CheckedChanged -= cb_CheckedChanged;
                cbSGID.CheckedChanged -= cb_CheckedChanged;
                cbSticky.CheckedChanged -= cb_CheckedChanged;
                cbOwRead.CheckedChanged -= cb_CheckedChanged;
                cbOwWrite.CheckedChanged -= cb_CheckedChanged;
                cbOwExecute.CheckedChanged -= cb_CheckedChanged;
                cbGrRead.CheckedChanged -= cb_CheckedChanged;
                cbGrWrite.CheckedChanged -= cb_CheckedChanged;
                cbGrExecute.CheckedChanged -= cb_CheckedChanged;
                cbOtRead.CheckedChanged -= cb_CheckedChanged;
                cbOtWrite.CheckedChanged -= cb_CheckedChanged;
                cbOtExecute.CheckedChanged -= cb_CheckedChanged;

                cbSUID.Checked = (i1 & int.Parse(cbSUID.Tag.ToString())) != 0;
                cbSGID.Checked = (i1 & int.Parse(cbSGID.Tag.ToString())) != 0;
                cbSticky.Checked = (i1 & int.Parse(cbSticky.Tag.ToString())) != 0;

                cbOwRead.Checked = (i2 & int.Parse(cbOwRead.Tag.ToString())) != 0;
                cbOwWrite.Checked = (i2 & int.Parse(cbOwWrite.Tag.ToString())) != 0;
                cbOwExecute.Checked = (i2 & int.Parse(cbOwExecute.Tag.ToString())) != 0;

                cbGrRead.Checked = (i3 & int.Parse(cbGrRead.Tag.ToString())) != 0;
                cbGrWrite.Checked = (i3 & int.Parse(cbGrWrite.Tag.ToString())) != 0;
                cbGrExecute.Checked = (i3 & int.Parse(cbGrExecute.Tag.ToString())) != 0;

                cbOtRead.Checked = (i4 & int.Parse(cbOtRead.Tag.ToString())) != 0;
                cbOtWrite.Checked = (i4 & int.Parse(cbOtWrite.Tag.ToString())) != 0;
                cbOtExecute.Checked = (i4 & int.Parse(cbOtExecute.Tag.ToString())) != 0;

                cbSUID.CheckedChanged += cb_CheckedChanged;
                cbSGID.CheckedChanged += cb_CheckedChanged;
                cbSticky.CheckedChanged += cb_CheckedChanged;
                cbOwRead.CheckedChanged += cb_CheckedChanged;
                cbOwWrite.CheckedChanged += cb_CheckedChanged;
                cbOwExecute.CheckedChanged += cb_CheckedChanged;
                cbGrRead.CheckedChanged += cb_CheckedChanged;
                cbGrWrite.CheckedChanged += cb_CheckedChanged;
                cbGrExecute.CheckedChanged += cb_CheckedChanged;
                cbOtRead.CheckedChanged += cb_CheckedChanged;
                cbOtWrite.CheckedChanged += cb_CheckedChanged;
                cbOtExecute.CheckedChanged += cb_CheckedChanged;
            }            
        }

        private bool ValidateOctal(string sOctal)
        {
            bool bRet = false;
            if (sOctal.Length == 4)
            {
                bRet = true;
                int pos = 0;
                foreach (char c in sOctal)
                {
                    if (!_validateArr.Contains(c))
                    {                        
                        bRet = false;
                        break;
                    }
                    pos++;
                }               
            }
            return bRet;
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            SetAll(true);
        }

        private void btnNone_Click(object sender, EventArgs e)
        {
            SetAll(false);
        }

        private void cb_CheckedChanged(object sender, EventArgs e)
        {
            tbOctal.Text = GetOctal();
        }

        private void tbOctal_TextChanged(object sender, EventArgs e)
        {
            if (!ValidateOctal(tbOctal.Lines[0]))
            {
                tbOctal.Text = "0000";
            }
            else
            {
                SetOctal(tbOctal.Lines[0]);
            }
        }

        private void tbOctal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 56 || e.KeyValue == 57 || e.KeyValue == 104 || e.KeyValue == 105)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void cbRoot_CheckedChanged(object sender, EventArgs e)
        {
            SetCommands();
        }

        private void cbRecur_CheckedChanged(object sender, EventArgs e)
        {
            SetCommands();
        }

        private void cbRemount_CheckedChanged(object sender, EventArgs e)
        {
            SetCommands();
        }

        private void ShowDetails()
        {
            this.Size = cbDetails.Checked ? new Size(551, 426) : new Size(551, 311);
        }
        private void cbDetails_CheckedChanged(object sender, EventArgs e)
        {
            ShowDetails();
        }

        private void cbApplyPerm_CheckedChanged(object sender, EventArgs e)
        {
            gbP.Enabled = cbApplyPerm.Checked;
            btnApply.Enabled = cbApplyPerm.Checked || cbApplyOwnerGroup.Checked;
        }

        private void cbApplyOwnerGroup_CheckedChanged(object sender, EventArgs e)
        {
            gbO.Enabled = cbApplyOwnerGroup.Checked;
            btnApply.Enabled = cbApplyPerm.Checked || cbApplyOwnerGroup.Checked;
        }
    }
}
