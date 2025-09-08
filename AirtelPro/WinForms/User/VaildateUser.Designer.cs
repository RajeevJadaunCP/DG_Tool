namespace DG_Tool.WinForms.User
{
    partial class VaildateUser
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
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblHeading = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.msglblFullName = new System.Windows.Forms.Label();
            this.msglblLoginId = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.cbxEmpId = new System.Windows.Forms.ComboBox();
            this.lblEmpId = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtLoginId = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbxRole = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtMobile = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.msgEmailValidation = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.msgMobile = new System.Windows.Forms.Label();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::DG_Tool.Properties.Resources.close_button_png_30241;
            this.pictureBox2.Location = new System.Drawing.Point(433, 1);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(39, 37);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 13;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.IndianRed;
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.lblHeading);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(481, 40);
            this.panel1.TabIndex = 37;
            // 
            // lblHeading
            // 
            this.lblHeading.AutoSize = true;
            this.lblHeading.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeading.ForeColor = System.Drawing.Color.White;
            this.lblHeading.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblHeading.Location = new System.Drawing.Point(7, 9);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Size = new System.Drawing.Size(89, 24);
            this.lblHeading.TabIndex = 0;
            this.lblHeading.Text = "Add User";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.msglblFullName);
            this.panel2.Controls.Add(this.msglblLoginId);
            this.panel2.Controls.Add(this.txtName);
            this.panel2.Controls.Add(this.cbxEmpId);
            this.panel2.Controls.Add(this.lblEmpId);
            this.panel2.Controls.Add(this.label13);
            this.panel2.Controls.Add(this.txtLoginId);
            this.panel2.Controls.Add(this.label14);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.cbxRole);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.txtEmail);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.lblEmail);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.txtMobile);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.msgEmailValidation);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.msgMobile);
            this.panel2.Controls.Add(this.btnSubmit);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Location = new System.Drawing.Point(21, 54);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(432, 393);
            this.panel2.TabIndex = 38;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // msglblFullName
            // 
            this.msglblFullName.AutoSize = true;
            this.msglblFullName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msglblFullName.Location = new System.Drawing.Point(18, 212);
            this.msglblFullName.Name = "msglblFullName";
            this.msglblFullName.Size = new System.Drawing.Size(35, 13);
            this.msglblFullName.TabIndex = 31;
            this.msglblFullName.Text = "label4";
            this.msglblFullName.Visible = false;
            // 
            // msglblLoginId
            // 
            this.msglblLoginId.AutoSize = true;
            this.msglblLoginId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msglblLoginId.Location = new System.Drawing.Point(18, 138);
            this.msglblLoginId.Name = "msglblLoginId";
            this.msglblLoginId.Size = new System.Drawing.Size(35, 13);
            this.msglblLoginId.TabIndex = 30;
            this.msglblLoginId.Text = "label4";
            this.msglblLoginId.Visible = false;
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.Location = new System.Drawing.Point(18, 185);
            this.txtName.MaxLength = 25;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(393, 24);
            this.txtName.TabIndex = 6;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            this.txtName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtName_KeyPress);
            // 
            // cbxEmpId
            // 
            this.cbxEmpId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxEmpId.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxEmpId.FormattingEnabled = true;
            this.cbxEmpId.Location = new System.Drawing.Point(18, 29);
            this.cbxEmpId.Name = "cbxEmpId";
            this.cbxEmpId.Size = new System.Drawing.Size(176, 26);
            this.cbxEmpId.TabIndex = 1;
            this.cbxEmpId.SelectedIndexChanged += new System.EventHandler(this.cbxEmpId_SelectedIndexChanged);
            // 
            // lblEmpId
            // 
            this.lblEmpId.AutoSize = true;
            this.lblEmpId.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmpId.Location = new System.Drawing.Point(15, 9);
            this.lblEmpId.Name = "lblEmpId";
            this.lblEmpId.Size = new System.Drawing.Size(114, 18);
            this.lblEmpId.TabIndex = 2;
            this.lblEmpId.Text = "Employee Code";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.Red;
            this.label13.Location = new System.Drawing.Point(266, 95);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(11, 13);
            this.label13.TabIndex = 29;
            this.label13.Text = "*";
            // 
            // txtLoginId
            // 
            this.txtLoginId.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLoginId.Location = new System.Drawing.Point(18, 111);
            this.txtLoginId.MaxLength = 25;
            this.txtLoginId.Name = "txtLoginId";
            this.txtLoginId.ReadOnly = true;
            this.txtLoginId.Size = new System.Drawing.Size(176, 24);
            this.txtLoginId.TabIndex = 3;
            this.txtLoginId.TextChanged += new System.EventHandler(this.txtLoginId_TextChanged);
            this.txtLoginId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLoginId_KeyPress);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(232, 92);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(39, 18);
            this.label14.TabIndex = 28;
            this.label14.Text = "Role";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(15, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 18);
            this.label2.TabIndex = 4;
            this.label2.Text = "Login Id";
            // 
            // cbxRole
            // 
            this.cbxRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxRole.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxRole.FormattingEnabled = true;
            this.cbxRole.Location = new System.Drawing.Point(235, 111);
            this.cbxRole.Name = "cbxRole";
            this.cbxRole.Size = new System.Drawing.Size(176, 26);
            this.cbxRole.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(15, 166);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 18);
            this.label3.TabIndex = 7;
            this.label3.Text = "Username";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(280, 244);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(11, 13);
            this.label10.TabIndex = 25;
            this.label10.Text = "*";
            // 
            // txtEmail
            // 
            this.txtEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.Location = new System.Drawing.Point(18, 262);
            this.txtEmail.MaxLength = 50;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(176, 24);
            this.txtEmail.TabIndex = 8;
            this.txtEmail.TextChanged += new System.EventHandler(this.txtEmail_TextChanged);
            this.txtEmail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtEmail_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(55, 244);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(11, 13);
            this.label9.TabIndex = 24;
            this.label9.Text = "*";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmail.Location = new System.Drawing.Point(15, 243);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(45, 18);
            this.lblEmail.TabIndex = 9;
            this.lblEmail.Text = "Email";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(87, 169);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(11, 13);
            this.label8.TabIndex = 23;
            this.label8.Text = "*";
            // 
            // txtMobile
            // 
            this.txtMobile.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMobile.Location = new System.Drawing.Point(235, 261);
            this.txtMobile.MaxLength = 10;
            this.txtMobile.Name = "txtMobile";
            this.txtMobile.Size = new System.Drawing.Size(176, 24);
            this.txtMobile.TabIndex = 10;
            this.txtMobile.TextChanged += new System.EventHandler(this.txtMobile_TextChanged);
            this.txtMobile.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMobile_KeyPress);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(72, 95);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(11, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "*";
            // 
            // msgEmailValidation
            // 
            this.msgEmailValidation.AutoSize = true;
            this.msgEmailValidation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msgEmailValidation.Location = new System.Drawing.Point(18, 288);
            this.msgEmailValidation.Name = "msgEmailValidation";
            this.msgEmailValidation.Size = new System.Drawing.Size(35, 13);
            this.msgEmailValidation.TabIndex = 11;
            this.msgEmailValidation.Text = "label4";
            this.msgEmailValidation.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(125, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(11, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "*";
            // 
            // msgMobile
            // 
            this.msgMobile.AutoSize = true;
            this.msgMobile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msgMobile.Location = new System.Drawing.Point(232, 288);
            this.msgMobile.Name = "msgMobile";
            this.msgMobile.Size = new System.Drawing.Size(35, 13);
            this.msgMobile.TabIndex = 12;
            this.msgMobile.Text = "label5";
            this.msgMobile.Visible = false;
            // 
            // btnSubmit
            // 
            this.btnSubmit.BackColor = System.Drawing.Color.IndianRed;
            this.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubmit.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSubmit.ForeColor = System.Drawing.Color.White;
            this.btnSubmit.Location = new System.Drawing.Point(125, 336);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(158, 38);
            this.btnSubmit.TabIndex = 20;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = false;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(232, 242);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 18);
            this.label4.TabIndex = 13;
            this.label4.Text = "Mobile";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ForeColor = System.Drawing.Color.Red;
            this.label15.Location = new System.Drawing.Point(123, 12);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(202, 13);
            this.label15.TabIndex = 0;
            this.label15.Text = "Fields with asterisk (⋆) sign are mandatory";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 454);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(474, 31);
            this.groupBox1.TabIndex = 36;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Note:";
            // 
            // VaildateUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SeaShell;
            this.ClientSize = new System.Drawing.Size(474, 485);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "VaildateUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VaildateUser";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblHeading;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.ComboBox cbxEmpId;
        private System.Windows.Forms.Label lblEmpId;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtLoginId;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbxRole;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtMobile;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label msgEmailValidation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label msgMobile;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label msglblLoginId;
        private System.Windows.Forms.Label msglblFullName;
    }
}