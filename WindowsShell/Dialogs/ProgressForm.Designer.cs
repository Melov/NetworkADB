using System.Windows.Forms;
using WindowsShell.Dialogs;

namespace WindowsShell.Dialogs
{
    partial class ProgressForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressForm));
            this.pb = new System.Windows.Forms.ProgressBar();
            this.lbSource = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.picBox = new System.Windows.Forms.PictureBox();
            this.lbDestination = new System.Windows.Forms.Label();
            this.pbS = new System.Windows.Forms.ProgressBar();
            this.btnDetails = new System.Windows.Forms.Button();
            this.lvFiles = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pb
            // 
            this.pb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pb.Location = new System.Drawing.Point(12, 77);
            this.pb.Name = "pb";
            this.pb.Size = new System.Drawing.Size(291, 11);
            this.pb.Step = 1;
            this.pb.TabIndex = 0;
            // 
            // lbSource
            // 
            this.lbSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSource.AutoEllipsis = true;
            this.lbSource.Location = new System.Drawing.Point(12, 91);
            this.lbSource.Name = "lbSource";
            this.lbSource.Size = new System.Drawing.Size(291, 16);
            this.lbSource.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(233, 127);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(70, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // picBox
            // 
            this.picBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picBox.Location = new System.Drawing.Point(24, 0);
            this.picBox.Name = "picBox";
            this.picBox.Size = new System.Drawing.Size(268, 55);
            this.picBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picBox.TabIndex = 3;
            this.picBox.TabStop = false;
            // 
            // lbDestination
            // 
            this.lbDestination.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbDestination.AutoEllipsis = true;
            this.lbDestination.Location = new System.Drawing.Point(12, 58);
            this.lbDestination.Name = "lbDestination";
            this.lbDestination.Size = new System.Drawing.Size(291, 16);
            this.lbDestination.TabIndex = 4;
            // 
            // pbS
            // 
            this.pbS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbS.Location = new System.Drawing.Point(12, 110);
            this.pbS.Name = "pbS";
            this.pbS.Size = new System.Drawing.Size(291, 11);
            this.pbS.Step = 1;
            this.pbS.TabIndex = 5;
            this.pbS.Visible = false;
            // 
            // btnDetails
            // 
            this.btnDetails.Location = new System.Drawing.Point(12, 127);
            this.btnDetails.Name = "btnDetails";
            this.btnDetails.Size = new System.Drawing.Size(107, 23);
            this.btnDetails.TabIndex = 6;
            this.btnDetails.Text = "[0/0] Details...";
            this.btnDetails.UseVisualStyleBackColor = true;
            this.btnDetails.Visible = false;
            this.btnDetails.Click += new System.EventHandler(this.btnDetails_Click);
            // 
            // lvFiles
            // 
            this.lvFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName});
            this.lvFiles.FullRowSelect = true;
            this.lvFiles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvFiles.Location = new System.Drawing.Point(12, 163);
            this.lvFiles.MultiSelect = false;
            this.lvFiles.Name = "lvFiles";
            this.lvFiles.ShowGroups = false;
            this.lvFiles.Size = new System.Drawing.Size(291, 100);
            this.lvFiles.SmallImageList = this.imageList1;
            this.lvFiles.TabIndex = 7;
            this.lvFiles.UseCompatibleStateImageBehavior = false;
            this.lvFiles.View = System.Windows.Forms.View.Details;
            this.lvFiles.Visible = false;
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 270;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "uncheck.png");
            this.imageList1.Images.SetKeyName(1, "check.png");
            // 
            // ProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(314, 159);
            this.Controls.Add(this.lvFiles);
            this.Controls.Add(this.btnDetails);
            this.Controls.Add(this.pbS);
            this.Controls.Add(this.lbDestination);
            this.Controls.Add(this.picBox);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lbSource);
            this.Controls.Add(this.pb);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "[0%] Copying...";
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar pb;
        private Label lbSource;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.PictureBox picBox;
        private Label lbDestination;
        private System.Windows.Forms.ProgressBar pbS;
        private Button btnDetails;
        private ListView lvFiles;
        private ImageList imageList1;
        private ColumnHeader colName;
    }
}