namespace DG_Tool.WinForms.OutputFile
{
    partial class OFProcessing
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelMessage = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtoutput = new System.Windows.Forms.RichTextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblInputFile = new System.Windows.Forms.Label();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnLicence = new System.Windows.Forms.Button();
            this.txtLicence = new System.Windows.Forms.TextBox();
            this.btnInputFile = new System.Windows.Forms.Button();
            this.txtInputfile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbxProfile = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.cbxCircle = new System.Windows.Forms.ComboBox();
            this.cbxCustomer = new System.Windows.Forms.ComboBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.button1 = new System.Windows.Forms.Button();
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
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(767, 49);
            this.panel3.TabIndex = 46;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBox2.Image = global::DG_Tool.Properties.Resources.close_button_png_30241;
            this.pictureBox2.Location = new System.Drawing.Point(715, 0);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            this.label10.Size = new System.Drawing.Size(225, 29);
            this.label10.TabIndex = 12;
            this.label10.Text = "Out File Processing";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtoutput);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.lblInputFile);
            this.groupBox1.Controls.Add(this.btnSubmit);
            this.groupBox1.Controls.Add(this.btnLicence);
            this.groupBox1.Controls.Add(this.txtLicence);
            this.groupBox1.Controls.Add(this.btnInputFile);
            this.groupBox1.Controls.Add(this.txtInputfile);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cbxProfile);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lblCustomer);
            this.groupBox1.Controls.Add(this.cbxCircle);
            this.groupBox1.Controls.Add(this.cbxCustomer);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(31, 78);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(699, 706);
            this.groupBox1.TabIndex = 47;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Out File Processing Details";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelMessage);
            this.panel1.Location = new System.Drawing.Point(179, 432);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            this.label3.Location = new System.Drawing.Point(35, 460);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(155, 24);
            this.label3.TabIndex = 66;
            this.label3.Text = "Output Window : ";
            // 
            // txtoutput
            // 
            this.txtoutput.Location = new System.Drawing.Point(39, 482);
            this.txtoutput.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtoutput.Name = "txtoutput";
            this.txtoutput.Size = new System.Drawing.Size(625, 195);
            this.txtoutput.TabIndex = 65;
            this.txtoutput.Text = "";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(261, 298);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(15, 20);
            this.label9.TabIndex = 64;
            this.label9.Text = "*";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(235, 214);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(15, 20);
            this.label8.TabIndex = 63;
            this.label8.Text = "*";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(139, 119);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(15, 20);
            this.label7.TabIndex = 62;
            this.label7.Text = "*";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(436, 39);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(15, 20);
            this.label5.TabIndex = 61;
            this.label5.Text = "*";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(175, 39);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(15, 20);
            this.label6.TabIndex = 60;
            this.label6.Text = "*";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Gray;
            this.label4.Location = new System.Drawing.Point(72, 298);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(192, 24);
            this.label4.TabIndex = 58;
            this.label4.Text = "Selected Licence FIle";
            // 
            // lblInputFile
            // 
            this.lblInputFile.AutoSize = true;
            this.lblInputFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInputFile.ForeColor = System.Drawing.Color.Gray;
            this.lblInputFile.Location = new System.Drawing.Point(72, 209);
            this.lblInputFile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInputFile.Name = "lblInputFile";
            this.lblInputFile.Size = new System.Drawing.Size(166, 24);
            this.lblInputFile.TabIndex = 57;
            this.lblInputFile.Text = "Selected Input File";
            // 
            // btnSubmit
            // 
            this.btnSubmit.BackColor = System.Drawing.Color.Brown;
            this.btnSubmit.FlatAppearance.BorderSize = 0;
            this.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSubmit.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSubmit.ForeColor = System.Drawing.Color.White;
            this.btnSubmit.Location = new System.Drawing.Point(229, 398);
            this.btnSubmit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(224, 43);
            this.btnSubmit.TabIndex = 56;
            this.btnSubmit.Text = "Start Processing";
            this.btnSubmit.UseVisualStyleBackColor = false;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnLicence
            // 
            this.btnLicence.BackColor = System.Drawing.Color.Gray;
            this.btnLicence.FlatAppearance.BorderSize = 0;
            this.btnLicence.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLicence.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLicence.ForeColor = System.Drawing.Color.White;
            this.btnLicence.Location = new System.Drawing.Point(473, 320);
            this.btnLicence.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLicence.Name = "btnLicence";
            this.btnLicence.Size = new System.Drawing.Size(153, 36);
            this.btnLicence.TabIndex = 55;
            this.btnLicence.Text = "Browse";
            this.btnLicence.UseVisualStyleBackColor = false;
            this.btnLicence.Click += new System.EventHandler(this.btnLicence_Click);
            // 
            // txtLicence
            // 
            this.txtLicence.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLicence.Location = new System.Drawing.Point(76, 320);
            this.txtLicence.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtLicence.Name = "txtLicence";
            this.txtLicence.ReadOnly = true;
            this.txtLicence.Size = new System.Drawing.Size(365, 29);
            this.txtLicence.TabIndex = 54;
            // 
            // btnInputFile
            // 
            this.btnInputFile.BackColor = System.Drawing.Color.Gray;
            this.btnInputFile.FlatAppearance.BorderSize = 0;
            this.btnInputFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInputFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInputFile.ForeColor = System.Drawing.Color.White;
            this.btnInputFile.Location = new System.Drawing.Point(473, 235);
            this.btnInputFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            this.txtInputfile.Location = new System.Drawing.Point(76, 235);
            this.txtInputfile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtInputfile.Name = "txtInputfile";
            this.txtInputfile.ReadOnly = true;
            this.txtInputfile.Size = new System.Drawing.Size(365, 29);
            this.txtInputfile.TabIndex = 52;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(77, 114);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 24);
            this.label1.TabIndex = 51;
            this.label1.Text = "Profile";
            // 
            // cbxProfile
            // 
            this.cbxProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxProfile.FormattingEnabled = true;
            this.cbxProfile.Location = new System.Drawing.Point(76, 140);
            this.cbxProfile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbxProfile.Name = "cbxProfile";
            this.cbxProfile.Size = new System.Drawing.Size(556, 32);
            this.cbxProfile.TabIndex = 50;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(381, 37);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 24);
            this.label2.TabIndex = 49;
            this.label2.Text = "Circle";
            // 
            // lblCustomer
            // 
            this.lblCustomer.AutoSize = true;
            this.lblCustomer.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomer.Location = new System.Drawing.Point(81, 37);
            this.lblCustomer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(91, 24);
            this.lblCustomer.TabIndex = 48;
            this.lblCustomer.Text = "Customer";
            // 
            // cbxCircle
            // 
            this.cbxCircle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxCircle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxCircle.FormattingEnabled = true;
            this.cbxCircle.Location = new System.Drawing.Point(385, 60);
            this.cbxCircle.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbxCircle.Name = "cbxCircle";
            this.cbxCircle.Size = new System.Drawing.Size(247, 32);
            this.cbxCircle.TabIndex = 47;
            this.cbxCircle.SelectedIndexChanged += new System.EventHandler(this.cbxCircle_SelectedIndexChanged);
            // 
            // cbxCustomer
            // 
            this.cbxCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxCustomer.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxCustomer.FormattingEnabled = true;
            this.cbxCustomer.Location = new System.Drawing.Point(76, 60);
            this.cbxCustomer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbxCustomer.Name = "cbxCustomer";
            this.cbxCustomer.Size = new System.Drawing.Size(247, 32);
            this.cbxCustomer.TabIndex = 46;
            this.cbxCustomer.SelectedIndexChanged += new System.EventHandler(this.cbxCustomer_SelectedIndexChanged);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker_RunWorkerCompleted);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Brown;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(622, 398);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(69, 43);
            this.button1.TabIndex = 50;
            this.button1.Text = "Multi Processing";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // OFProcessing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SeaShell;
            this.ClientSize = new System.Drawing.Size(767, 788);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "OFProcessing";
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
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblInputFile;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnLicence;
        private System.Windows.Forms.TextBox txtLicence;
        private System.Windows.Forms.Button btnInputFile;
        private System.Windows.Forms.TextBox txtInputfile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbxProfile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.ComboBox cbxCircle;
        private System.Windows.Forms.ComboBox cbxCustomer;
        private System.Windows.Forms.RichTextBox txtoutput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelMessage;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button button1;
    }
}