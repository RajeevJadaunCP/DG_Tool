namespace DG_Tool.WinForms.Configure
{
    partial class AddMasterFiles
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.pbCancel = new System.Windows.Forms.PictureBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtFileNamingConvension = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtEncryptionKey = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbxIsEncryption = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtFileExtension = new System.Windows.Forms.TextBox();
            this.FileStructure = new System.Windows.Forms.Label();
            this.cbxFileStructure = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnBrowseFile = new System.Windows.Forms.Button();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbxFileIOID = new System.Windows.Forms.ComboBox();
            this.txtFileDescription = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCancel)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.IndianRed;
            this.panel3.Controls.Add(this.pbCancel);
            this.panel3.Controls.Add(this.label10);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(694, 40);
            this.panel3.TabIndex = 48;
            // 
            // pbCancel
            // 
            this.pbCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.pbCancel.Image = global::DG_Tool.Properties.Resources.close_button_png_30241;
            this.pbCancel.Location = new System.Drawing.Point(655, 0);
            this.pbCancel.Name = "pbCancel";
            this.pbCancel.Size = new System.Drawing.Size(39, 40);
            this.pbCancel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbCancel.TabIndex = 13;
            this.pbCancel.TabStop = false;
            this.pbCancel.Click += new System.EventHandler(this.pbCancel_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(11, 8);
            this.label10.Name = "label10";
            this.label10.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label10.Size = new System.Drawing.Size(142, 24);
            this.label10.TabIndex = 12;
            this.label10.Text = "Add Master File";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.txtFileNamingConvension);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.btnSubmit);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtEncryptionKey);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cbxIsEncryption);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtFileExtension);
            this.groupBox1.Controls.Add(this.FileStructure);
            this.groupBox1.Controls.Add(this.cbxFileStructure);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.btnBrowseFile);
            this.groupBox1.Controls.Add(this.txtFilePath);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cbxFileIOID);
            this.groupBox1.Controls.Add(this.txtFileDescription);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtFileName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(40, 64);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(616, 482);
            this.groupBox1.TabIndex = 49;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Create New Master File";
            // 
            // txtFileNamingConvension
            // 
            this.txtFileNamingConvension.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtFileNamingConvension.Location = new System.Drawing.Point(344, 139);
            this.txtFileNamingConvension.MaxLength = 10;
            this.txtFileNamingConvension.Name = "txtFileNamingConvension";
            this.txtFileNamingConvension.Size = new System.Drawing.Size(185, 23);
            this.txtFileNamingConvension.TabIndex = 39;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label8.Location = new System.Drawing.Point(341, 119);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(160, 17);
            this.label8.TabIndex = 38;
            this.label8.Text = "File Naming Convension";
            // 
            // btnSubmit
            // 
            this.btnSubmit.BackColor = System.Drawing.Color.IndianRed;
            this.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubmit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnSubmit.ForeColor = System.Drawing.Color.White;
            this.btnSubmit.Location = new System.Drawing.Point(247, 422);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(140, 33);
            this.btnSubmit.TabIndex = 37;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = false;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label5.Location = new System.Drawing.Point(341, 341);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 17);
            this.label5.TabIndex = 36;
            this.label5.Text = "Encryption Key";
            // 
            // txtEncryptionKey
            // 
            this.txtEncryptionKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtEncryptionKey.Location = new System.Drawing.Point(344, 361);
            this.txtEncryptionKey.MaxLength = 10;
            this.txtEncryptionKey.Name = "txtEncryptionKey";
            this.txtEncryptionKey.Size = new System.Drawing.Size(185, 23);
            this.txtEncryptionKey.TabIndex = 35;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label7.Location = new System.Drawing.Point(87, 341);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 17);
            this.label7.TabIndex = 34;
            this.label7.Text = "Encryption";
            // 
            // cbxIsEncryption
            // 
            this.cbxIsEncryption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxIsEncryption.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.cbxIsEncryption.FormattingEnabled = true;
            this.cbxIsEncryption.Items.AddRange(new object[] {
            "---Plese Select---",
            "Yes",
            "No"});
            this.cbxIsEncryption.Location = new System.Drawing.Point(90, 361);
            this.cbxIsEncryption.Name = "cbxIsEncryption";
            this.cbxIsEncryption.Size = new System.Drawing.Size(185, 24);
            this.cbxIsEncryption.TabIndex = 33;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label6.Location = new System.Drawing.Point(341, 276);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 17);
            this.label6.TabIndex = 32;
            this.label6.Text = "File Extension";
            // 
            // txtFileExtension
            // 
            this.txtFileExtension.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtFileExtension.Location = new System.Drawing.Point(344, 296);
            this.txtFileExtension.MaxLength = 10;
            this.txtFileExtension.Name = "txtFileExtension";
            this.txtFileExtension.Size = new System.Drawing.Size(185, 23);
            this.txtFileExtension.TabIndex = 31;
            // 
            // FileStructure
            // 
            this.FileStructure.AutoSize = true;
            this.FileStructure.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.FileStructure.Location = new System.Drawing.Point(87, 276);
            this.FileStructure.Name = "FileStructure";
            this.FileStructure.Size = new System.Drawing.Size(92, 17);
            this.FileStructure.TabIndex = 30;
            this.FileStructure.Text = "File Structure";
            // 
            // cbxFileStructure
            // 
            this.cbxFileStructure.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxFileStructure.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.cbxFileStructure.FormattingEnabled = true;
            this.cbxFileStructure.Location = new System.Drawing.Point(90, 296);
            this.cbxFileStructure.Name = "cbxFileStructure";
            this.cbxFileStructure.Size = new System.Drawing.Size(185, 24);
            this.cbxFileStructure.TabIndex = 29;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label4.Location = new System.Drawing.Point(87, 194);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 17);
            this.label4.TabIndex = 28;
            this.label4.Text = "File Path";
            // 
            // btnBrowseFile
            // 
            this.btnBrowseFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnBrowseFile.Location = new System.Drawing.Point(425, 212);
            this.btnBrowseFile.Name = "btnBrowseFile";
            this.btnBrowseFile.Size = new System.Drawing.Size(104, 31);
            this.btnBrowseFile.TabIndex = 27;
            this.btnBrowseFile.Text = "Browse";
            this.btnBrowseFile.UseVisualStyleBackColor = true;
            this.btnBrowseFile.Click += new System.EventHandler(this.btnBrowseFile_Click);
            // 
            // txtFilePath
            // 
            this.txtFilePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtFilePath.Location = new System.Drawing.Point(90, 214);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(315, 23);
            this.txtFilePath.TabIndex = 26;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label3.Location = new System.Drawing.Point(87, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 17);
            this.label3.TabIndex = 25;
            this.label3.Text = "FileIOID";
            // 
            // cbxFileIOID
            // 
            this.cbxFileIOID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxFileIOID.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.cbxFileIOID.FormattingEnabled = true;
            this.cbxFileIOID.Items.AddRange(new object[] {
            "----Select Type----",
            "Input",
            "Output"});
            this.cbxFileIOID.Location = new System.Drawing.Point(90, 138);
            this.cbxFileIOID.Name = "cbxFileIOID";
            this.cbxFileIOID.Size = new System.Drawing.Size(185, 24);
            this.cbxFileIOID.TabIndex = 24;
            // 
            // txtFileDescription
            // 
            this.txtFileDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtFileDescription.Location = new System.Drawing.Point(344, 60);
            this.txtFileDescription.MaxLength = 50;
            this.txtFileDescription.Name = "txtFileDescription";
            this.txtFileDescription.Size = new System.Drawing.Size(185, 23);
            this.txtFileDescription.TabIndex = 23;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label2.Location = new System.Drawing.Point(341, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 17);
            this.label2.TabIndex = 22;
            this.label2.Text = "File Description";
            // 
            // txtFileName
            // 
            this.txtFileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtFileName.Location = new System.Drawing.Point(90, 60);
            this.txtFileName.MaxLength = 50;
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(185, 23);
            this.txtFileName.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label1.Location = new System.Drawing.Point(87, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 17);
            this.label1.TabIndex = 20;
            this.label1.Text = "File Name";
            // 
            // AddMasterFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SeaShell;
            this.ClientSize = new System.Drawing.Size(694, 574);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AddMasterFiles";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AddMasterFiles";
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCancel)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox pbCancel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtFileNamingConvension;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtEncryptionKey;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbxIsEncryption;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtFileExtension;
        private System.Windows.Forms.Label FileStructure;
        private System.Windows.Forms.ComboBox cbxFileStructure;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnBrowseFile;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbxFileIOID;
        private System.Windows.Forms.TextBox txtFileDescription;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label label1;
    }
}