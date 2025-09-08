namespace DG_Tool.WinForms.Customer
{
    partial class ViewCustomerProfile
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvCustomerProfileFile = new System.Windows.Forms.DataGridView();
            this.CustProfileFileID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileIOID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileNamingConv = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileStructureName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FilePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileExtn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Encrypt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EncryptKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StatusID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvProfileAttribute = new System.Windows.Forms.DataGridView();
            this.HeaderName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PFName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.lblHeading = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblCreatedBy = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblProfile = new System.Windows.Forms.Label();
            this.lblCircle = new System.Windows.Forms.Label();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblProfileId = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.lblProfile1 = new System.Windows.Forms.Label();
            this.lblCircle1 = new System.Windows.Forms.Label();
            this.lblCustomer1 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomerProfileFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProfileAttribute)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvCustomerProfileFile
            // 
            this.dgvCustomerProfileFile.AllowUserToAddRows = false;
            this.dgvCustomerProfileFile.AllowUserToDeleteRows = false;
            this.dgvCustomerProfileFile.AllowUserToOrderColumns = true;
            this.dgvCustomerProfileFile.AllowUserToResizeColumns = false;
            this.dgvCustomerProfileFile.AllowUserToResizeRows = false;
            this.dgvCustomerProfileFile.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCustomerProfileFile.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Brown;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCustomerProfileFile.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCustomerProfileFile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCustomerProfileFile.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CustProfileFileID,
            this.FileName,
            this.FileIOID,
            this.FileDesc,
            this.FileNamingConv,
            this.FileStructureName,
            this.FilePath,
            this.FileExtn,
            this.Encrypt,
            this.EncryptKey,
            this.StatusID});
            this.dgvCustomerProfileFile.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvCustomerProfileFile.Location = new System.Drawing.Point(0, 408);
            this.dgvCustomerProfileFile.Name = "dgvCustomerProfileFile";
            this.dgvCustomerProfileFile.RowHeadersVisible = false;
            this.dgvCustomerProfileFile.Size = new System.Drawing.Size(1238, 271);
            this.dgvCustomerProfileFile.TabIndex = 6;
            this.dgvCustomerProfileFile.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCustomerProfileFile_CellContentClick);
            // 
            // CustProfileFileID
            // 
            this.CustProfileFileID.DataPropertyName = "CustProfileFIleID";
            this.CustProfileFileID.HeaderText = "File ID";
            this.CustProfileFileID.Name = "CustProfileFileID";
            // 
            // FileName
            // 
            this.FileName.DataPropertyName = "FileName";
            this.FileName.HeaderText = "File Name";
            this.FileName.Name = "FileName";
            // 
            // FileIOID
            // 
            this.FileIOID.DataPropertyName = "FileIOID";
            this.FileIOID.HeaderText = "File IO ID";
            this.FileIOID.Name = "FileIOID";
            // 
            // FileDesc
            // 
            this.FileDesc.DataPropertyName = "FileDesc";
            this.FileDesc.HeaderText = "File Desc";
            this.FileDesc.Name = "FileDesc";
            // 
            // FileNamingConv
            // 
            this.FileNamingConv.DataPropertyName = "FileNamingConv";
            this.FileNamingConv.HeaderText = "Naming Conv";
            this.FileNamingConv.Name = "FileNamingConv";
            // 
            // FileStructureName
            // 
            this.FileStructureName.DataPropertyName = "FileStructueName";
            this.FileStructureName.HeaderText = "File Structure";
            this.FileStructureName.Name = "FileStructureName";
            // 
            // FilePath
            // 
            this.FilePath.DataPropertyName = "FilePath";
            this.FilePath.HeaderText = "FIle Path";
            this.FilePath.Name = "FilePath";
            // 
            // FileExtn
            // 
            this.FileExtn.DataPropertyName = "FileExtn";
            this.FileExtn.HeaderText = "File Extn";
            this.FileExtn.Name = "FileExtn";
            // 
            // Encrypt
            // 
            this.Encrypt.DataPropertyName = "Encrypt";
            this.Encrypt.HeaderText = "Encrypt";
            this.Encrypt.Name = "Encrypt";
            // 
            // EncryptKey
            // 
            this.EncryptKey.DataPropertyName = "EncryptKey";
            this.EncryptKey.HeaderText = "Encryption Key";
            this.EncryptKey.Name = "EncryptKey";
            // 
            // StatusID
            // 
            this.StatusID.DataPropertyName = "StatusID";
            this.StatusID.HeaderText = "Status";
            this.StatusID.Name = "StatusID";
            // 
            // dgvProfileAttribute
            // 
            this.dgvProfileAttribute.AllowUserToAddRows = false;
            this.dgvProfileAttribute.AllowUserToDeleteRows = false;
            this.dgvProfileAttribute.AllowUserToResizeColumns = false;
            this.dgvProfileAttribute.AllowUserToResizeRows = false;
            this.dgvProfileAttribute.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.dgvProfileAttribute.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProfileAttribute.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvProfileAttribute.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvProfileAttribute.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProfileAttribute.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.HeaderName,
            this.PFName});
            this.dgvProfileAttribute.Location = new System.Drawing.Point(814, 144);
            this.dgvProfileAttribute.Name = "dgvProfileAttribute";
            this.dgvProfileAttribute.RowHeadersVisible = false;
            this.dgvProfileAttribute.Size = new System.Drawing.Size(412, 250);
            this.dgvProfileAttribute.TabIndex = 12;
            // 
            // HeaderName
            // 
            this.HeaderName.DataPropertyName = "HeaderName";
            this.HeaderName.HeaderText = "Header Name";
            this.HeaderName.Name = "HeaderName";
            // 
            // PFName
            // 
            this.PFName.DataPropertyName = "PFName";
            this.PFName.HeaderText = "Profile Name";
            this.PFName.Name = "PFName";
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.SeaShell;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(811, 124);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(117, 17);
            this.label7.TabIndex = 13;
            this.label7.Text = "ProfileAttribute";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.IndianRed;
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.lblHeading);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1238, 40);
            this.panel1.TabIndex = 38;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBox2.Image = global::DG_Tool.Properties.Resources.close_button_png_30241;
            this.pictureBox2.Location = new System.Drawing.Point(1199, 0);
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
            this.lblHeading.Size = new System.Drawing.Size(148, 24);
            this.lblHeading.TabIndex = 0;
            this.lblHeading.Text = "Customer Profile";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.lblCreatedBy);
            this.groupBox1.Controls.Add(this.lblStatus);
            this.groupBox1.Controls.Add(this.lblProfile);
            this.groupBox1.Controls.Add(this.lblCircle);
            this.groupBox1.Controls.Add(this.lblCustomer);
            this.groupBox1.Location = new System.Drawing.Point(12, 128);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(793, 270);
            this.groupBox1.TabIndex = 39;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Profile Details";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(23, 181);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 17);
            this.label2.TabIndex = 21;
            this.label2.Text = "Created By :";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(23, 224);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 17);
            this.label3.TabIndex = 20;
            this.label3.Text = "Status :";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(23, 129);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 17);
            this.label4.TabIndex = 19;
            this.label4.Text = "Profile Name :";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(23, 77);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 17);
            this.label5.TabIndex = 18;
            this.label5.Text = "Circle :";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(23, 29);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 17);
            this.label6.TabIndex = 17;
            this.label6.Text = "Customer :";
            // 
            // lblCreatedBy
            // 
            this.lblCreatedBy.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCreatedBy.AutoSize = true;
            this.lblCreatedBy.BackColor = System.Drawing.Color.White;
            this.lblCreatedBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCreatedBy.Location = new System.Drawing.Point(137, 181);
            this.lblCreatedBy.Name = "lblCreatedBy";
            this.lblCreatedBy.Size = new System.Drawing.Size(88, 17);
            this.lblCreatedBy.TabIndex = 16;
            this.lblCreatedBy.Text = "Created By";
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblStatus.AutoSize = true;
            this.lblStatus.BackColor = System.Drawing.Color.White;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(137, 224);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(54, 17);
            this.lblStatus.TabIndex = 15;
            this.lblStatus.Text = "Status";
            // 
            // lblProfile
            // 
            this.lblProfile.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblProfile.AutoSize = true;
            this.lblProfile.BackColor = System.Drawing.Color.White;
            this.lblProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProfile.Location = new System.Drawing.Point(137, 128);
            this.lblProfile.Name = "lblProfile";
            this.lblProfile.Size = new System.Drawing.Size(101, 17);
            this.lblProfile.TabIndex = 14;
            this.lblProfile.Text = "Profile Name";
            // 
            // lblCircle
            // 
            this.lblCircle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCircle.AutoSize = true;
            this.lblCircle.BackColor = System.Drawing.Color.White;
            this.lblCircle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCircle.Location = new System.Drawing.Point(137, 77);
            this.lblCircle.Name = "lblCircle";
            this.lblCircle.Size = new System.Drawing.Size(49, 17);
            this.lblCircle.TabIndex = 13;
            this.lblCircle.Text = "Circle";
            // 
            // lblCustomer
            // 
            this.lblCustomer.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCustomer.AutoSize = true;
            this.lblCustomer.BackColor = System.Drawing.Color.White;
            this.lblCustomer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomer.Location = new System.Drawing.Point(137, 29);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(76, 17);
            this.lblCustomer.TabIndex = 12;
            this.lblCustomer.Text = "Customer";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.panel2.Controls.Add(this.lblProfileId);
            this.panel2.Controls.Add(this.label19);
            this.panel2.Controls.Add(this.lblProfile1);
            this.panel2.Controls.Add(this.lblCircle1);
            this.panel2.Controls.Add(this.lblCustomer1);
            this.panel2.Controls.Add(this.label14);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label13);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.label15);
            this.panel2.Location = new System.Drawing.Point(0, 62);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1238, 43);
            this.panel2.TabIndex = 40;
            // 
            // lblProfileId
            // 
            this.lblProfileId.AutoSize = true;
            this.lblProfileId.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProfileId.Location = new System.Drawing.Point(415, 14);
            this.lblProfileId.Name = "lblProfileId";
            this.lblProfileId.Size = new System.Drawing.Size(59, 17);
            this.lblProfileId.TabIndex = 26;
            this.lblProfileId.Text = "Circle: ";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(348, 14);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(77, 17);
            this.label19.TabIndex = 25;
            this.label19.Text = "Profile ID : ";
            // 
            // lblProfile1
            // 
            this.lblProfile1.AutoSize = true;
            this.lblProfile1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProfile1.Location = new System.Drawing.Point(543, 14);
            this.lblProfile1.Name = "lblProfile1";
            this.lblProfile1.Size = new System.Drawing.Size(59, 17);
            this.lblProfile1.TabIndex = 24;
            this.lblProfile1.Text = "Circle: ";
            // 
            // lblCircle1
            // 
            this.lblCircle1.AutoSize = true;
            this.lblCircle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCircle1.Location = new System.Drawing.Point(244, 14);
            this.lblCircle1.Name = "lblCircle1";
            this.lblCircle1.Size = new System.Drawing.Size(59, 17);
            this.lblCircle1.TabIndex = 23;
            this.lblCircle1.Text = "Circle: ";
            // 
            // lblCustomer1
            // 
            this.lblCustomer1.AutoSize = true;
            this.lblCustomer1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomer1.Location = new System.Drawing.Point(91, 14);
            this.lblCustomer1.Name = "lblCustomer1";
            this.lblCustomer1.Size = new System.Drawing.Size(59, 17);
            this.lblCustomer1.TabIndex = 22;
            this.lblCustomer1.Text = "Circle: ";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(494, 14);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(56, 17);
            this.label14.TabIndex = 21;
            this.label14.Text = "Profile: ";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(199, 14);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(51, 17);
            this.label12.TabIndex = 20;
            this.label12.Text = "Circle: ";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(21, 14);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(76, 17);
            this.label10.TabIndex = 19;
            this.label10.Text = "Customer: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(526, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 17);
            this.label1.TabIndex = 18;
            this.label1.Text = "label4";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(467, 114);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(56, 17);
            this.label11.TabIndex = 17;
            this.label11.Text = "Profile :";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(339, 114);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 17);
            this.label8.TabIndex = 16;
            this.label8.Text = "label4";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(280, 114);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(51, 17);
            this.label13.TabIndex = 15;
            this.label13.Text = "Circle :";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(166, 114);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(46, 17);
            this.label9.TabIndex = 14;
            this.label9.Text = "label3";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(83, 114);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(76, 17);
            this.label15.TabIndex = 13;
            this.label15.Text = "Customer :";
            // 
            // ViewCustomerProfile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SeaShell;
            this.ClientSize = new System.Drawing.Size(1238, 679);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.dgvProfileAttribute);
            this.Controls.Add(this.dgvCustomerProfileFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ViewCustomerProfile";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ViewCustomerProfile";
            this.Load += new System.EventHandler(this.ViewCustomerProfile_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomerProfileFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProfileAttribute)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dgvCustomerProfileFile;
        private System.Windows.Forms.DataGridView dgvProfileAttribute;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label lblHeading;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblCreatedBy;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblProfile;
        private System.Windows.Forms.Label lblCircle;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.DataGridViewTextBoxColumn HeaderName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PFName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustProfileFileID;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileIOID;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileNamingConv;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileStructureName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FilePath;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileExtn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Encrypt;
        private System.Windows.Forms.DataGridViewTextBoxColumn EncryptKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn StatusID;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblProfileId;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label lblProfile1;
        private System.Windows.Forms.Label lblCircle1;
        private System.Windows.Forms.Label lblCustomer1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label15;
    }
}