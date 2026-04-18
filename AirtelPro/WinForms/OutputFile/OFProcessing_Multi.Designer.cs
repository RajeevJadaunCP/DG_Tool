namespace DG_Tool.WinForms.OutputFile
{
    partial class OFProcessing_Multi
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
            this.ofdInputFile = new System.Windows.Forms.OpenFileDialog();
            this.ofdLicence = new System.Windows.Forms.OpenFileDialog();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbxProfile = new System.Windows.Forms.ComboBox();
            this.cbxCustomer = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnInputFile3 = new System.Windows.Forms.Button();
            this.txtInputfile3 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.btnInputFile2 = new System.Windows.Forms.Button();
            this.txtInputfile2 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelMessage = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtoutput = new System.Windows.Forms.RichTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.lblInputFile = new System.Windows.Forms.Label();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnInputFile = new System.Windows.Forms.Button();
            this.txtInputfile = new System.Windows.Forms.TextBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label9 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.license_file = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ofdInputFile
            // 
            this.ofdInputFile.FileName = "openFileDialog1";
            // 
            // ofdLicence
            // 
            this.ofdLicence.FileName = "openFileDialog1";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.IndianRed;
            this.panel3.Controls.Add(this.pictureBox2);
            this.panel3.Controls.Add(this.label10);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1163, 49);
            this.panel3.TabIndex = 46;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBox2.Image = global::DG_Tool.Properties.Resources.close_button_png_30241;
            this.pictureBox2.Location = new System.Drawing.Point(1111, 0);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(52, 49);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 14;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(15, 10);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(238, 29);
            this.label10.TabIndex = 12;
            this.label10.Text = "Multi File Processing";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.license_file);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.cbxProfile);
            this.groupBox1.Controls.Add(this.cbxCustomer);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnInputFile3);
            this.groupBox1.Controls.Add(this.txtInputfile3);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.btnInputFile2);
            this.groupBox1.Controls.Add(this.txtInputfile2);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtoutput);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.lblInputFile);
            this.groupBox1.Controls.Add(this.btnSubmit);
            this.groupBox1.Controls.Add(this.btnInputFile);
            this.groupBox1.Controls.Add(this.txtInputfile);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(28, 70);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(1100, 693);
            this.groupBox1.TabIndex = 47;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Multi File Processing";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // cbxProfile
            // 
            this.cbxProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxProfile.FormattingEnabled = true;
            this.cbxProfile.Location = new System.Drawing.Point(616, 54);
            this.cbxProfile.Margin = new System.Windows.Forms.Padding(4);
            this.cbxProfile.Name = "cbxProfile";
            this.cbxProfile.Size = new System.Drawing.Size(321, 32);
            this.cbxProfile.TabIndex = 98;
            this.cbxProfile.SelectedIndexChanged += new System.EventHandler(this.cbxProfile_SelectedIndexChanged);
            // 
            // cbxCustomer
            // 
            this.cbxCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxCustomer.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxCustomer.FormattingEnabled = true;
            this.cbxCustomer.Location = new System.Drawing.Point(296, 54);
            this.cbxCustomer.Margin = new System.Windows.Forms.Padding(4);
            this.cbxCustomer.Name = "cbxCustomer";
            this.cbxCustomer.Size = new System.Drawing.Size(277, 32);
            this.cbxCustomer.TabIndex = 97;
            this.cbxCustomer.SelectedIndexChanged += new System.EventHandler(this.cbxCustomer_SelectedIndexChanged_1);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.Red;
            this.label11.Location = new System.Drawing.Point(643, 346);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(15, 20);
            this.label11.TabIndex = 96;
            this.label11.Text = "*";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(776, -14);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(15, 20);
            this.label6.TabIndex = 93;
            this.label6.Text = "*";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Gray;
            this.label7.Location = new System.Drawing.Point(612, 27);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(163, 24);
            this.label7.TabIndex = 92;
            this.label7.Text = "EnterProfile Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(501, -13);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(15, 20);
            this.label5.TabIndex = 90;
            this.label5.Text = "*";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Gray;
            this.label4.Location = new System.Drawing.Point(299, 26);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(197, 24);
            this.label4.TabIndex = 89;
            this.label4.Text = "Enter Customer Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(457, 240);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 20);
            this.label1.TabIndex = 87;
            this.label1.Text = "*";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Gray;
            this.label2.Location = new System.Drawing.Point(291, 237);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(169, 24);
            this.label2.TabIndex = 86;
            this.label2.Text = "Selected Third File";
            // 
            // btnInputFile3
            // 
            this.btnInputFile3.BackColor = System.Drawing.Color.Gray;
            this.btnInputFile3.FlatAppearance.BorderSize = 0;
            this.btnInputFile3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInputFile3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInputFile3.ForeColor = System.Drawing.Color.White;
            this.btnInputFile3.Location = new System.Drawing.Point(776, 260);
            this.btnInputFile3.Margin = new System.Windows.Forms.Padding(4);
            this.btnInputFile3.Name = "btnInputFile3";
            this.btnInputFile3.Size = new System.Drawing.Size(153, 37);
            this.btnInputFile3.TabIndex = 85;
            this.btnInputFile3.Text = "Browse";
            this.btnInputFile3.UseVisualStyleBackColor = false;
            this.btnInputFile3.Click += new System.EventHandler(this.btnInputFile3_Click);
            // 
            // txtInputfile3
            // 
            this.txtInputfile3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInputfile3.Location = new System.Drawing.Point(295, 264);
            this.txtInputfile3.Margin = new System.Windows.Forms.Padding(4);
            this.txtInputfile3.Name = "txtInputfile3";
            this.txtInputfile3.ReadOnly = true;
            this.txtInputfile3.Size = new System.Drawing.Size(455, 29);
            this.txtInputfile3.TabIndex = 84;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.Red;
            this.label12.Location = new System.Drawing.Point(445, 168);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(15, 20);
            this.label12.TabIndex = 83;
            this.label12.Text = "*";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.Gray;
            this.label17.Location = new System.Drawing.Point(289, 164);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(158, 24);
            this.label17.TabIndex = 78;
            this.label17.Text = "Selected Sec File";
            // 
            // btnInputFile2
            // 
            this.btnInputFile2.BackColor = System.Drawing.Color.Gray;
            this.btnInputFile2.FlatAppearance.BorderSize = 0;
            this.btnInputFile2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInputFile2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInputFile2.ForeColor = System.Drawing.Color.White;
            this.btnInputFile2.Location = new System.Drawing.Point(776, 188);
            this.btnInputFile2.Margin = new System.Windows.Forms.Padding(4);
            this.btnInputFile2.Name = "btnInputFile2";
            this.btnInputFile2.Size = new System.Drawing.Size(153, 37);
            this.btnInputFile2.TabIndex = 75;
            this.btnInputFile2.Text = "Browse";
            this.btnInputFile2.UseVisualStyleBackColor = false;
            this.btnInputFile2.Click += new System.EventHandler(this.btnInputFile2_Click);
            // 
            // txtInputfile2
            // 
            this.txtInputfile2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInputfile2.Location = new System.Drawing.Point(293, 192);
            this.txtInputfile2.Margin = new System.Windows.Forms.Padding(4);
            this.txtInputfile2.Name = "txtInputfile2";
            this.txtInputfile2.ReadOnly = true;
            this.txtInputfile2.Size = new System.Drawing.Size(456, 29);
            this.txtInputfile2.TabIndex = 74;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelMessage);
            this.panel1.Location = new System.Drawing.Point(443, 414);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(377, 174);
            this.panel1.TabIndex = 67;
            this.panel1.Visible = false;
            // 
            // labelMessage
            // 
            this.labelMessage.AutoSize = true;
            this.labelMessage.Location = new System.Drawing.Point(67, 130);
            this.labelMessage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(235, 20);
            this.labelMessage.TabIndex = 3;
            this.labelMessage.Text = "Please wait, generating data....";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(299, 442);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(155, 24);
            this.label3.TabIndex = 66;
            this.label3.Text = "Output Window : ";
            // 
            // txtoutput
            // 
            this.txtoutput.Location = new System.Drawing.Point(303, 464);
            this.txtoutput.Margin = new System.Windows.Forms.Padding(4);
            this.txtoutput.Name = "txtoutput";
            this.txtoutput.Size = new System.Drawing.Size(625, 195);
            this.txtoutput.TabIndex = 65;
            this.txtoutput.Text = "";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(479, 100);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(15, 20);
            this.label8.TabIndex = 63;
            this.label8.Text = "*";
            // 
            // lblInputFile
            // 
            this.lblInputFile.AutoSize = true;
            this.lblInputFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInputFile.ForeColor = System.Drawing.Color.Gray;
            this.lblInputFile.Location = new System.Drawing.Point(291, 99);
            this.lblInputFile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInputFile.Name = "lblInputFile";
            this.lblInputFile.Size = new System.Drawing.Size(188, 24);
            this.lblInputFile.TabIndex = 57;
            this.lblInputFile.Text = "Selected Primary File";
            // 
            // btnSubmit
            // 
            this.btnSubmit.BackColor = System.Drawing.Color.Brown;
            this.btnSubmit.FlatAppearance.BorderSize = 0;
            this.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSubmit.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSubmit.ForeColor = System.Drawing.Color.White;
            this.btnSubmit.Location = new System.Drawing.Point(493, 380);
            this.btnSubmit.Margin = new System.Windows.Forms.Padding(4);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(224, 43);
            this.btnSubmit.TabIndex = 56;
            this.btnSubmit.Text = "Start Processing";
            this.btnSubmit.UseVisualStyleBackColor = false;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnInputFile
            // 
            this.btnInputFile.BackColor = System.Drawing.Color.Gray;
            this.btnInputFile.FlatAppearance.BorderSize = 0;
            this.btnInputFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInputFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInputFile.ForeColor = System.Drawing.Color.White;
            this.btnInputFile.Location = new System.Drawing.Point(776, 121);
            this.btnInputFile.Margin = new System.Windows.Forms.Padding(4);
            this.btnInputFile.Name = "btnInputFile";
            this.btnInputFile.Size = new System.Drawing.Size(153, 37);
            this.btnInputFile.TabIndex = 53;
            this.btnInputFile.Text = "Browse";
            this.btnInputFile.UseVisualStyleBackColor = false;
            this.btnInputFile.Click += new System.EventHandler(this.btnInputFile_Click);
            // 
            // txtInputfile
            // 
            this.txtInputfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInputfile.Location = new System.Drawing.Point(295, 125);
            this.txtInputfile.Margin = new System.Windows.Forms.Padding(4);
            this.txtInputfile.Name = "txtInputfile";
            this.txtInputfile.ReadOnly = true;
            this.txtInputfile.Size = new System.Drawing.Size(455, 29);
            this.txtInputfile.TabIndex = 52;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker_RunWorkerCompleted);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(431, 309);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(15, 20);
            this.label9.TabIndex = 102;
            this.label9.Text = "*";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Gray;
            this.label13.Location = new System.Drawing.Point(290, 306);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(144, 24);
            this.label13.TabIndex = 101;
            this.label13.Text = "OS License File";
            // 
            // license_file
            // 
            this.license_file.BackColor = System.Drawing.Color.Gray;
            this.license_file.FlatAppearance.BorderSize = 0;
            this.license_file.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.license_file.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.license_file.ForeColor = System.Drawing.Color.White;
            this.license_file.Location = new System.Drawing.Point(775, 329);
            this.license_file.Margin = new System.Windows.Forms.Padding(4);
            this.license_file.Name = "license_file";
            this.license_file.Size = new System.Drawing.Size(153, 37);
            this.license_file.TabIndex = 100;
            this.license_file.Text = "License_File";
            this.license_file.UseVisualStyleBackColor = false;
            this.license_file.Click += new System.EventHandler(this.license_file_Click);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(294, 333);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(455, 29);
            this.textBox1.TabIndex = 99;
            // 
            // OFProcessing_Multi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SeaShell;
            this.ClientSize = new System.Drawing.Size(1163, 784);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "OFProcessing_Multi";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OFProcessing";
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog ofdInputFile;
        private System.Windows.Forms.OpenFileDialog ofdLicence;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblInputFile;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnInputFile;
        private System.Windows.Forms.TextBox txtInputfile;
        private System.Windows.Forms.RichTextBox txtoutput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelMessage;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button btnInputFile2;
        private System.Windows.Forms.TextBox txtInputfile2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnInputFile3;
        private System.Windows.Forms.TextBox txtInputfile3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cbxCustomer;
        private System.Windows.Forms.ComboBox cbxProfile;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button license_file;
        private System.Windows.Forms.TextBox textBox1;
    }
}