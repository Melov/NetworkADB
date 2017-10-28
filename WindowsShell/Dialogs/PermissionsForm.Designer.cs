namespace WindowsShell.Dialogs
{
    partial class PermissionsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PermissionsForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.cbOwRead = new System.Windows.Forms.CheckBox();
            this.cbOwWrite = new System.Windows.Forms.CheckBox();
            this.cbOwExecute = new System.Windows.Forms.CheckBox();
            this.cbGrRead = new System.Windows.Forms.CheckBox();
            this.cbGrWrite = new System.Windows.Forms.CheckBox();
            this.cbGrExecute = new System.Windows.Forms.CheckBox();
            this.cbOtRead = new System.Windows.Forms.CheckBox();
            this.cbOtExecute = new System.Windows.Forms.CheckBox();
            this.cbOtWrite = new System.Windows.Forms.CheckBox();
            this.cbSticky = new System.Windows.Forms.CheckBox();
            this.cbSGID = new System.Windows.Forms.CheckBox();
            this.cbSUID = new System.Windows.Forms.CheckBox();
            this.cbOwner = new System.Windows.Forms.ComboBox();
            this.cbGroup = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.btnAll = new System.Windows.Forms.Button();
            this.btnNone = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancell = new System.Windows.Forms.Button();
            this.tbOctal = new System.Windows.Forms.MaskedTextBox();
            this.cbRoot = new System.Windows.Forms.CheckBox();
            this.cbRecur = new System.Windows.Forms.CheckBox();
            this.cbRemount = new System.Windows.Forms.CheckBox();
            this.tbRemount = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.tbPermissions = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.tbOwnerGroup = new System.Windows.Forms.TextBox();
            this.cbApplyPerm = new System.Windows.Forms.CheckBox();
            this.gbP = new System.Windows.Forms.GroupBox();
            this.gbO = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbApplyOwnerGroup = new System.Windows.Forms.CheckBox();
            this.lvResult = new System.Windows.Forms.ListView();
            this.Path = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Command = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Result = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cbDetails = new System.Windows.Forms.CheckBox();
            this.gbP.SuspendLayout();
            this.gbO.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Read";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(93, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Write";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(141, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Execute";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Owner";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Group";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 77);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Other";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(44, 94);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "SUID";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(93, 94);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(33, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "SGID";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(141, 94);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Sticky";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 21);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(38, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "Owner";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(7, 49);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(36, 13);
            this.label11.TabIndex = 10;
            this.label11.Text = "Group";
            // 
            // cbOwRead
            // 
            this.cbOwRead.AutoSize = true;
            this.cbOwRead.Location = new System.Drawing.Point(47, 38);
            this.cbOwRead.Name = "cbOwRead";
            this.cbOwRead.Size = new System.Drawing.Size(15, 14);
            this.cbOwRead.TabIndex = 11;
            this.cbOwRead.Tag = "4";
            this.cbOwRead.UseVisualStyleBackColor = true;
            this.cbOwRead.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
            // 
            // cbOwWrite
            // 
            this.cbOwWrite.AutoSize = true;
            this.cbOwWrite.Location = new System.Drawing.Point(96, 38);
            this.cbOwWrite.Name = "cbOwWrite";
            this.cbOwWrite.Size = new System.Drawing.Size(15, 14);
            this.cbOwWrite.TabIndex = 12;
            this.cbOwWrite.Tag = "2";
            this.cbOwWrite.UseVisualStyleBackColor = true;
            this.cbOwWrite.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
            // 
            // cbOwExecute
            // 
            this.cbOwExecute.AutoSize = true;
            this.cbOwExecute.Location = new System.Drawing.Point(144, 38);
            this.cbOwExecute.Name = "cbOwExecute";
            this.cbOwExecute.Size = new System.Drawing.Size(15, 14);
            this.cbOwExecute.TabIndex = 13;
            this.cbOwExecute.Tag = "1";
            this.cbOwExecute.UseVisualStyleBackColor = true;
            this.cbOwExecute.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
            // 
            // cbGrRead
            // 
            this.cbGrRead.AutoSize = true;
            this.cbGrRead.Location = new System.Drawing.Point(47, 57);
            this.cbGrRead.Name = "cbGrRead";
            this.cbGrRead.Size = new System.Drawing.Size(15, 14);
            this.cbGrRead.TabIndex = 14;
            this.cbGrRead.Tag = "4";
            this.cbGrRead.UseVisualStyleBackColor = true;
            this.cbGrRead.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
            // 
            // cbGrWrite
            // 
            this.cbGrWrite.AutoSize = true;
            this.cbGrWrite.Location = new System.Drawing.Point(96, 58);
            this.cbGrWrite.Name = "cbGrWrite";
            this.cbGrWrite.Size = new System.Drawing.Size(15, 14);
            this.cbGrWrite.TabIndex = 15;
            this.cbGrWrite.Tag = "2";
            this.cbGrWrite.UseVisualStyleBackColor = true;
            this.cbGrWrite.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
            // 
            // cbGrExecute
            // 
            this.cbGrExecute.AutoSize = true;
            this.cbGrExecute.Location = new System.Drawing.Point(144, 58);
            this.cbGrExecute.Name = "cbGrExecute";
            this.cbGrExecute.Size = new System.Drawing.Size(15, 14);
            this.cbGrExecute.TabIndex = 16;
            this.cbGrExecute.Tag = "1";
            this.cbGrExecute.UseVisualStyleBackColor = true;
            this.cbGrExecute.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
            // 
            // cbOtRead
            // 
            this.cbOtRead.AutoSize = true;
            this.cbOtRead.Location = new System.Drawing.Point(47, 77);
            this.cbOtRead.Name = "cbOtRead";
            this.cbOtRead.Size = new System.Drawing.Size(15, 14);
            this.cbOtRead.TabIndex = 17;
            this.cbOtRead.Tag = "4";
            this.cbOtRead.UseVisualStyleBackColor = true;
            this.cbOtRead.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
            // 
            // cbOtExecute
            // 
            this.cbOtExecute.AutoSize = true;
            this.cbOtExecute.Location = new System.Drawing.Point(144, 78);
            this.cbOtExecute.Name = "cbOtExecute";
            this.cbOtExecute.Size = new System.Drawing.Size(15, 14);
            this.cbOtExecute.TabIndex = 19;
            this.cbOtExecute.Tag = "1";
            this.cbOtExecute.UseVisualStyleBackColor = true;
            this.cbOtExecute.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
            // 
            // cbOtWrite
            // 
            this.cbOtWrite.AutoSize = true;
            this.cbOtWrite.Location = new System.Drawing.Point(96, 78);
            this.cbOtWrite.Name = "cbOtWrite";
            this.cbOtWrite.Size = new System.Drawing.Size(15, 14);
            this.cbOtWrite.TabIndex = 18;
            this.cbOtWrite.Tag = "2";
            this.cbOtWrite.UseVisualStyleBackColor = true;
            this.cbOtWrite.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
            // 
            // cbSticky
            // 
            this.cbSticky.AutoSize = true;
            this.cbSticky.Location = new System.Drawing.Point(144, 111);
            this.cbSticky.Name = "cbSticky";
            this.cbSticky.Size = new System.Drawing.Size(15, 14);
            this.cbSticky.TabIndex = 22;
            this.cbSticky.Tag = "1";
            this.cbSticky.UseVisualStyleBackColor = true;
            this.cbSticky.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
            // 
            // cbSGID
            // 
            this.cbSGID.AutoSize = true;
            this.cbSGID.Location = new System.Drawing.Point(96, 111);
            this.cbSGID.Name = "cbSGID";
            this.cbSGID.Size = new System.Drawing.Size(15, 14);
            this.cbSGID.TabIndex = 21;
            this.cbSGID.Tag = "2";
            this.cbSGID.UseVisualStyleBackColor = true;
            this.cbSGID.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
            // 
            // cbSUID
            // 
            this.cbSUID.AutoSize = true;
            this.cbSUID.Location = new System.Drawing.Point(47, 111);
            this.cbSUID.Name = "cbSUID";
            this.cbSUID.Size = new System.Drawing.Size(15, 14);
            this.cbSUID.TabIndex = 20;
            this.cbSUID.Tag = "4";
            this.cbSUID.UseVisualStyleBackColor = true;
            this.cbSUID.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
            // 
            // cbOwner
            // 
            this.cbOwner.FormattingEnabled = true;
            this.cbOwner.Location = new System.Drawing.Point(48, 19);
            this.cbOwner.Name = "cbOwner";
            this.cbOwner.Size = new System.Drawing.Size(112, 21);
            this.cbOwner.TabIndex = 23;
            // 
            // cbGroup
            // 
            this.cbGroup.FormattingEnabled = true;
            this.cbGroup.Location = new System.Drawing.Point(48, 46);
            this.cbGroup.Name = "cbGroup";
            this.cbGroup.Size = new System.Drawing.Size(112, 21);
            this.cbGroup.TabIndex = 24;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 134);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(32, 13);
            this.label12.TabIndex = 26;
            this.label12.Text = "Octal";
            // 
            // btnAll
            // 
            this.btnAll.Location = new System.Drawing.Point(195, 36);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(52, 23);
            this.btnAll.TabIndex = 27;
            this.btnAll.Text = "All";
            this.btnAll.UseVisualStyleBackColor = true;
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // btnNone
            // 
            this.btnNone.Location = new System.Drawing.Point(195, 65);
            this.btnNone.Name = "btnNone";
            this.btnNone.Size = new System.Drawing.Size(52, 23);
            this.btnNone.TabIndex = 28;
            this.btnNone.Text = "None";
            this.btnNone.UseVisualStyleBackColor = true;
            this.btnNone.Click += new System.EventHandler(this.btnNone_Click);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(430, 252);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(52, 23);
            this.btnApply.TabIndex = 29;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancell
            // 
            this.btnCancell.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancell.Location = new System.Drawing.Point(488, 252);
            this.btnCancell.Name = "btnCancell";
            this.btnCancell.Size = new System.Drawing.Size(52, 23);
            this.btnCancell.TabIndex = 30;
            this.btnCancell.Text = "Close";
            this.btnCancell.UseVisualStyleBackColor = true;
            this.btnCancell.Click += new System.EventHandler(this.btnCancell_Click);
            // 
            // tbOctal
            // 
            this.tbOctal.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite;
            this.tbOctal.Location = new System.Drawing.Point(47, 131);
            this.tbOctal.Mask = "0000";
            this.tbOctal.Name = "tbOctal";
            this.tbOctal.PromptChar = '0';
            this.tbOctal.Size = new System.Drawing.Size(64, 20);
            this.tbOctal.TabIndex = 31;
            this.tbOctal.TextChanged += new System.EventHandler(this.tbOctal_TextChanged);
            this.tbOctal.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbOctal_KeyDown);
            // 
            // cbRoot
            // 
            this.cbRoot.AutoSize = true;
            this.cbRoot.Checked = true;
            this.cbRoot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRoot.Location = new System.Drawing.Point(13, 20);
            this.cbRoot.Name = "cbRoot";
            this.cbRoot.Size = new System.Drawing.Size(79, 17);
            this.cbRoot.TabIndex = 32;
            this.cbRoot.Text = "Has ROOT";
            this.cbRoot.UseVisualStyleBackColor = true;
            this.cbRoot.CheckedChanged += new System.EventHandler(this.cbRoot_CheckedChanged);
            // 
            // cbRecur
            // 
            this.cbRecur.AutoSize = true;
            this.cbRecur.Location = new System.Drawing.Point(13, 43);
            this.cbRecur.Name = "cbRecur";
            this.cbRecur.Size = new System.Drawing.Size(156, 17);
            this.cbRecur.TabIndex = 33;
            this.cbRecur.Text = "Recursively set Permissions";
            this.cbRecur.UseVisualStyleBackColor = true;
            this.cbRecur.CheckedChanged += new System.EventHandler(this.cbRecur_CheckedChanged);
            // 
            // cbRemount
            // 
            this.cbRemount.AutoSize = true;
            this.cbRemount.Checked = true;
            this.cbRemount.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRemount.Location = new System.Drawing.Point(13, 66);
            this.cbRemount.Name = "cbRemount";
            this.cbRemount.Size = new System.Drawing.Size(238, 17);
            this.cbRemount.TabIndex = 34;
            this.cbRemount.Text = "Remount to RW if RO [Read-only file system]";
            this.cbRemount.UseVisualStyleBackColor = true;
            this.cbRemount.CheckedChanged += new System.EventHandler(this.cbRemount_CheckedChanged);
            // 
            // tbRemount
            // 
            this.tbRemount.Location = new System.Drawing.Point(13, 102);
            this.tbRemount.Name = "tbRemount";
            this.tbRemount.Size = new System.Drawing.Size(238, 20);
            this.tbRemount.TabIndex = 35;
            this.tbRemount.Text = "shell \"su -c \'mount -o remount,rw [DIR]\'\"";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(10, 86);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(128, 13);
            this.label13.TabIndex = 36;
            this.label13.Text = "Remount ADB Command:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(10, 125);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(180, 13);
            this.label14.TabIndex = 37;
            this.label14.Text = "Change Permissions ADB Command:";
            // 
            // tbPermissions
            // 
            this.tbPermissions.Location = new System.Drawing.Point(13, 141);
            this.tbPermissions.Name = "tbPermissions";
            this.tbPermissions.Size = new System.Drawing.Size(238, 20);
            this.tbPermissions.TabIndex = 38;
            this.tbPermissions.Text = "shell \"su -c \'chmod [OCTAL] [DIR]\'\"";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(10, 164);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(190, 13);
            this.label15.TabIndex = 39;
            this.label15.Text = "Change Owner/Group ADB Command:";
            // 
            // tbOwnerGroup
            // 
            this.tbOwnerGroup.Location = new System.Drawing.Point(13, 180);
            this.tbOwnerGroup.Name = "tbOwnerGroup";
            this.tbOwnerGroup.Size = new System.Drawing.Size(238, 20);
            this.tbOwnerGroup.TabIndex = 40;
            this.tbOwnerGroup.Text = "shell \"su -c \'chown [OWNER].[GROUP] [DIR]\'\"";
            // 
            // cbApplyPerm
            // 
            this.cbApplyPerm.AutoSize = true;
            this.cbApplyPerm.Checked = true;
            this.cbApplyPerm.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbApplyPerm.Location = new System.Drawing.Point(208, 1);
            this.cbApplyPerm.Name = "cbApplyPerm";
            this.cbApplyPerm.Size = new System.Drawing.Size(52, 17);
            this.cbApplyPerm.TabIndex = 41;
            this.cbApplyPerm.Text = "Apply";
            this.cbApplyPerm.UseVisualStyleBackColor = true;
            this.cbApplyPerm.CheckedChanged += new System.EventHandler(this.cbApplyPerm_CheckedChanged);
            // 
            // gbP
            // 
            this.gbP.Controls.Add(this.label4);
            this.gbP.Controls.Add(this.label1);
            this.gbP.Controls.Add(this.label2);
            this.gbP.Controls.Add(this.label3);
            this.gbP.Controls.Add(this.label5);
            this.gbP.Controls.Add(this.label6);
            this.gbP.Controls.Add(this.label7);
            this.gbP.Controls.Add(this.label8);
            this.gbP.Controls.Add(this.label9);
            this.gbP.Controls.Add(this.cbOwRead);
            this.gbP.Controls.Add(this.cbOwWrite);
            this.gbP.Controls.Add(this.cbOwExecute);
            this.gbP.Controls.Add(this.tbOctal);
            this.gbP.Controls.Add(this.cbGrRead);
            this.gbP.Controls.Add(this.cbGrWrite);
            this.gbP.Controls.Add(this.label12);
            this.gbP.Controls.Add(this.btnNone);
            this.gbP.Controls.Add(this.cbGrExecute);
            this.gbP.Controls.Add(this.btnAll);
            this.gbP.Controls.Add(this.cbSGID);
            this.gbP.Controls.Add(this.cbOtRead);
            this.gbP.Controls.Add(this.cbOtWrite);
            this.gbP.Controls.Add(this.cbOtExecute);
            this.gbP.Controls.Add(this.cbSUID);
            this.gbP.Controls.Add(this.cbSticky);
            this.gbP.Location = new System.Drawing.Point(6, 3);
            this.gbP.Name = "gbP";
            this.gbP.Size = new System.Drawing.Size(260, 162);
            this.gbP.TabIndex = 42;
            this.gbP.TabStop = false;
            this.gbP.Text = "Permissions";
            // 
            // gbO
            // 
            this.gbO.Controls.Add(this.cbOwner);
            this.gbO.Controls.Add(this.label10);
            this.gbO.Controls.Add(this.label11);
            this.gbO.Controls.Add(this.cbGroup);
            this.gbO.Location = new System.Drawing.Point(6, 171);
            this.gbO.Name = "gbO";
            this.gbO.Size = new System.Drawing.Size(260, 75);
            this.gbO.TabIndex = 43;
            this.gbO.TabStop = false;
            this.gbO.Text = "Owner/Group";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbRoot);
            this.groupBox3.Controls.Add(this.cbRecur);
            this.groupBox3.Controls.Add(this.cbRemount);
            this.groupBox3.Controls.Add(this.tbRemount);
            this.groupBox3.Controls.Add(this.tbOwnerGroup);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.tbPermissions);
            this.groupBox3.Location = new System.Drawing.Point(272, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(268, 243);
            this.groupBox3.TabIndex = 44;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Settings";
            // 
            // cbApplyOwnerGroup
            // 
            this.cbApplyOwnerGroup.AutoSize = true;
            this.cbApplyOwnerGroup.Checked = true;
            this.cbApplyOwnerGroup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbApplyOwnerGroup.Location = new System.Drawing.Point(208, 169);
            this.cbApplyOwnerGroup.Name = "cbApplyOwnerGroup";
            this.cbApplyOwnerGroup.Size = new System.Drawing.Size(52, 17);
            this.cbApplyOwnerGroup.TabIndex = 42;
            this.cbApplyOwnerGroup.Text = "Apply";
            this.cbApplyOwnerGroup.UseVisualStyleBackColor = true;
            this.cbApplyOwnerGroup.CheckedChanged += new System.EventHandler(this.cbApplyOwnerGroup_CheckedChanged);
            // 
            // lvResult
            // 
            this.lvResult.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Path,
            this.Command,
            this.Result});
            this.lvResult.FullRowSelect = true;
            this.lvResult.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvResult.HideSelection = false;
            this.lvResult.Location = new System.Drawing.Point(6, 282);
            this.lvResult.Name = "lvResult";
            this.lvResult.ShowItemToolTips = true;
            this.lvResult.Size = new System.Drawing.Size(534, 107);
            this.lvResult.TabIndex = 45;
            this.lvResult.UseCompatibleStateImageBehavior = false;
            this.lvResult.View = System.Windows.Forms.View.Details;
            // 
            // Path
            // 
            this.Path.Text = "Path";
            this.Path.Width = 200;
            // 
            // Command
            // 
            this.Command.Text = "Command";
            this.Command.Width = 200;
            // 
            // Result
            // 
            this.Result.Text = "Result";
            this.Result.Width = 100;
            // 
            // cbDetails
            // 
            this.cbDetails.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbDetails.AutoSize = true;
            this.cbDetails.Checked = true;
            this.cbDetails.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDetails.Location = new System.Drawing.Point(6, 252);
            this.cbDetails.Name = "cbDetails";
            this.cbDetails.Size = new System.Drawing.Size(58, 23);
            this.cbDetails.TabIndex = 43;
            this.cbDetails.Text = "Details...";
            this.cbDetails.UseVisualStyleBackColor = true;
            this.cbDetails.CheckedChanged += new System.EventHandler(this.cbDetails_CheckedChanged);
            // 
            // PermissionsForm
            // 
            this.AcceptButton = this.btnApply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancell;
            this.ClientSize = new System.Drawing.Size(545, 396);
            this.Controls.Add(this.cbApplyOwnerGroup);
            this.Controls.Add(this.cbDetails);
            this.Controls.Add(this.lvResult);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.cbApplyPerm);
            this.Controls.Add(this.gbO);
            this.Controls.Add(this.gbP);
            this.Controls.Add(this.btnCancell);
            this.Controls.Add(this.btnApply);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PermissionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Permissions & Owner/Group Settings";
            this.gbP.ResumeLayout(false);
            this.gbP.PerformLayout();
            this.gbO.ResumeLayout(false);
            this.gbO.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox cbOwRead;
        private System.Windows.Forms.CheckBox cbOwWrite;
        private System.Windows.Forms.CheckBox cbOwExecute;
        private System.Windows.Forms.CheckBox cbGrRead;
        private System.Windows.Forms.CheckBox cbGrWrite;
        private System.Windows.Forms.CheckBox cbGrExecute;
        private System.Windows.Forms.CheckBox cbOtRead;
        private System.Windows.Forms.CheckBox cbOtExecute;
        private System.Windows.Forms.CheckBox cbOtWrite;
        private System.Windows.Forms.CheckBox cbSticky;
        private System.Windows.Forms.CheckBox cbSGID;
        private System.Windows.Forms.CheckBox cbSUID;
        private System.Windows.Forms.ComboBox cbOwner;
        private System.Windows.Forms.ComboBox cbGroup;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnAll;
        private System.Windows.Forms.Button btnNone;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancell;
        private System.Windows.Forms.MaskedTextBox tbOctal;
        private System.Windows.Forms.CheckBox cbRoot;
        private System.Windows.Forms.CheckBox cbRecur;
        private System.Windows.Forms.CheckBox cbRemount;
        private System.Windows.Forms.TextBox tbRemount;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tbPermissions;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox tbOwnerGroup;
        private System.Windows.Forms.CheckBox cbApplyPerm;
        private System.Windows.Forms.GroupBox gbP;
        private System.Windows.Forms.GroupBox gbO;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox cbApplyOwnerGroup;
        private System.Windows.Forms.ListView lvResult;
        private System.Windows.Forms.ColumnHeader Path;
        private System.Windows.Forms.ColumnHeader Command;
        private System.Windows.Forms.ColumnHeader Result;
        private System.Windows.Forms.CheckBox cbDetails;
    }
}