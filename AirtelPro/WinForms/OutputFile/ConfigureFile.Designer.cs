namespace DG_Tool.WinForms.OutputFile
{
    partial class ConfigureFile
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigureFile));
            this.dgvFileSelection = new System.Windows.Forms.DataGridView();
            this.FileMasterID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileNamingConv = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileIOID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Selected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblProfileId = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblCircle = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblProfile = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.lblHeading = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFileSelection)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvFileSelection
            // 
            this.dgvFileSelection.AllowUserToAddRows = false;
            this.dgvFileSelection.AllowUserToDeleteRows = false;
            this.dgvFileSelection.AllowUserToResizeColumns = false;
            this.dgvFileSelection.AllowUserToResizeRows = false;
            this.dgvFileSelection.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvFileSelection.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvFileSelection.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvFileSelection.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFileSelection.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FileMasterID,
            this.FileName,
            this.FileDesc,
            this.FileNamingConv,
            this.FileIOID,
            this.Selected});
            this.dgvFileSelection.Location = new System.Drawing.Point(23, 143);
            this.dgvFileSelection.Name = "dgvFileSelection";
            this.dgvFileSelection.RowHeadersVisible = false;
            this.dgvFileSelection.Size = new System.Drawing.Size(984, 314);
            this.dgvFileSelection.TabIndex = 0;
            this.dgvFileSelection.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFileSelection_CellContentClick);
            // 
            // FileMasterID
            // 
            this.FileMasterID.DataPropertyName = "FileMasterID";
            this.FileMasterID.HeaderText = "File ID";
            this.FileMasterID.Name = "FileMasterID";
            this.FileMasterID.ReadOnly = true;
            // 
            // FileName
            // 
            this.FileName.DataPropertyName = "FileName";
            this.FileName.HeaderText = "File Name";
            this.FileName.Name = "FileName";
            this.FileName.ReadOnly = true;
            // 
            // FileDesc
            // 
            this.FileDesc.DataPropertyName = "FileDesc";
            this.FileDesc.HeaderText = "File Desc";
            this.FileDesc.Name = "FileDesc";
            this.FileDesc.ReadOnly = true;
            // 
            // FileNamingConv
            // 
            this.FileNamingConv.DataPropertyName = "FileNamingConv";
            this.FileNamingConv.HeaderText = "File Naming Conv";
            this.FileNamingConv.Name = "FileNamingConv";
            this.FileNamingConv.ReadOnly = true;
            // 
            // FileIOID
            // 
            this.FileIOID.DataPropertyName = "FileIOID";
            this.FileIOID.HeaderText = "File Type";
            this.FileIOID.Name = "FileIOID";
            this.FileIOID.ReadOnly = true;
            // 
            // Selected
            // 
            this.Selected.DataPropertyName = "IsSelected";
            this.Selected.HeaderText = "Select";
            this.Selected.Name = "Selected";
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.IndianRed;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(889, 456);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(119, 37);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Next";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.lblProfileId);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lblCircle);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.lblProfile);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.lblCustomer);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(23, 64);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(984, 66);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Profile Details";
            // 
            // lblProfileId
            // 
            this.lblProfileId.AutoSize = true;
            this.lblProfileId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProfileId.Location = new System.Drawing.Point(468, 30);
            this.lblProfileId.Name = "lblProfileId";
            this.lblProfileId.Size = new System.Drawing.Size(20, 16);
            this.lblProfileId.TabIndex = 7;
            this.lblProfileId.Text = "id";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(406, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "Profile Id : ";
            // 
            // lblCircle
            // 
            this.lblCircle.AutoSize = true;
            this.lblCircle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCircle.Location = new System.Drawing.Point(321, 30);
            this.lblCircle.Name = "lblCircle";
            this.lblCircle.Size = new System.Drawing.Size(50, 16);
            this.lblCircle.TabIndex = 5;
            this.lblCircle.Text = "label5";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(237, 29);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 16);
            this.label6.TabIndex = 4;
            this.label6.Text = "Circle Name : ";
            // 
            // lblProfile
            // 
            this.lblProfile.AutoSize = true;
            this.lblProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProfile.Location = new System.Drawing.Point(596, 30);
            this.lblProfile.Name = "lblProfile";
            this.lblProfile.Size = new System.Drawing.Size(50, 16);
            this.lblProfile.TabIndex = 3;
            this.lblProfile.Text = "label3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(507, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 16);
            this.label4.TabIndex = 2;
            this.label4.Text = "Profile Name : ";
            // 
            // lblCustomer
            // 
            this.lblCustomer.AutoSize = true;
            this.lblCustomer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomer.Location = new System.Drawing.Point(122, 29);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(50, 16);
            this.lblCustomer.TabIndex = 1;
            this.lblCustomer.Text = "label2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Customer Name : ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Yellow;
            this.label7.Location = new System.Drawing.Point(74, 460);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Files not included";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::DG_Tool.Properties.Resources.red_color_solid_background_1920x1080;
            this.pictureBox1.Location = new System.Drawing.Point(22, 458);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(46, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.IndianRed;
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.lblHeading);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1029, 40);
            this.panel1.TabIndex = 37;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBox2.Image = global::DG_Tool.Properties.Resources.close_button_png_30241;
            this.pictureBox2.Location = new System.Drawing.Point(990, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(39, 40);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 13;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // lblHeading
            // 
            this.lblHeading.AutoSize = true;
            this.lblHeading.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeading.ForeColor = System.Drawing.Color.White;
            this.lblHeading.Location = new System.Drawing.Point(7, 9);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Size = new System.Drawing.Size(205, 24);
            this.lblHeading.TabIndex = 0;
            this.lblHeading.Text = "Select Input Output File";
            // 
            // ConfigureFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SeaShell;
            this.ClientSize = new System.Drawing.Size(1029, 505);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dgvFileSelection);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ConfigureFile";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Input and Output Files ";
            ((System.ComponentModel.ISupportInitialize)(this.dgvFileSelection)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvFileSelection;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblCircle;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblProfile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label lblHeading;
        private System.Windows.Forms.Label lblProfileId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileMasterID;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileNamingConv;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileIOID;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Selected;
    }
}