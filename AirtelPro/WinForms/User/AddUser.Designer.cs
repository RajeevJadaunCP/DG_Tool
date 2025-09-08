namespace DG_Tool.WinForms.User
{
    partial class AddUser
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
            this.txtName = new System.Windows.Forms.TextBox();
            this.cbxEmpId = new System.Windows.Forms.ComboBox();
            this.lblEmpId = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtLoginId = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbxRole = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
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
            this.lblIsPasswordMatched = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.msgPassword = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtConfirmPassword = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.msglblFullname = new System.Windows.Forms.Label();
            this.msglblLoginId = new System.Windows.Forms.Label();
            this.lblOR = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.lblHeading = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblEmpCode = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.Location = new System.Drawing.Point(40, 178);
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
            this.cbxEmpId.Location = new System.Drawing.Point(40, 29);
            this.cbxEmpId.Name = "cbxEmpId";
            this.cbxEmpId.Size = new System.Drawing.Size(176, 26);
            this.cbxEmpId.TabIndex = 1;
            this.cbxEmpId.SelectedIndexChanged += new System.EventHandler(this.cbxEmpId_SelectedIndexChanged);
            // 
            // lblEmpId
            // 
            this.lblEmpId.AutoSize = true;
            this.lblEmpId.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmpId.Location = new System.Drawing.Point(37, 9);
            this.lblEmpId.Name = "lblEmpId";
            this.lblEmpId.Size = new System.Drawing.Size(114, 18);
            this.lblEmpId.TabIndex = 2;
            this.lblEmpId.Text = "Employee Code";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.Red;
            this.label12.Location = new System.Drawing.Point(385, 319);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(11, 13);
            this.label12.TabIndex = 27;
            this.label12.Text = "*";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.Red;
            this.label13.Location = new System.Drawing.Point(288, 95);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(11, 13);
            this.label13.TabIndex = 29;
            this.label13.Text = "*";
            // 
            // txtLoginId
            // 
            this.txtLoginId.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLoginId.Location = new System.Drawing.Point(40, 111);
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
            this.label14.Location = new System.Drawing.Point(254, 92);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(39, 18);
            this.label14.TabIndex = 28;
            this.label14.Text = "Role";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(37, 91);
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
            this.cbxRole.Location = new System.Drawing.Point(257, 111);
            this.cbxRole.Name = "cbxRole";
            this.cbxRole.Size = new System.Drawing.Size(176, 26);
            this.cbxRole.TabIndex = 5;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.Red;
            this.label11.Location = new System.Drawing.Point(110, 317);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(11, 13);
            this.label11.TabIndex = 26;
            this.label11.Text = "*";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(37, 159);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 18);
            this.label3.TabIndex = 7;
            this.label3.Text = "Full Name";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(301, 233);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(11, 13);
            this.label10.TabIndex = 25;
            this.label10.Text = "*";
            // 
            // txtEmail
            // 
            this.txtEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.Location = new System.Drawing.Point(40, 251);
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
            this.label9.Location = new System.Drawing.Point(77, 233);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(11, 13);
            this.label9.TabIndex = 24;
            this.label9.Text = "*";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmail.Location = new System.Drawing.Point(37, 232);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(45, 18);
            this.lblEmail.TabIndex = 9;
            this.lblEmail.Text = "Email";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(109, 162);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(11, 13);
            this.label8.TabIndex = 23;
            this.label8.Text = "*";
            // 
            // txtMobile
            // 
            this.txtMobile.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMobile.Location = new System.Drawing.Point(257, 250);
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
            this.label7.Location = new System.Drawing.Point(94, 95);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(11, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "*";
            // 
            // msgEmailValidation
            // 
            this.msgEmailValidation.AutoSize = true;
            this.msgEmailValidation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msgEmailValidation.Location = new System.Drawing.Point(40, 277);
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
            this.label1.Location = new System.Drawing.Point(150, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(11, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "*";
            // 
            // msgMobile
            // 
            this.msgMobile.AutoSize = true;
            this.msgMobile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msgMobile.Location = new System.Drawing.Point(254, 277);
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
            this.btnSubmit.Location = new System.Drawing.Point(156, 395);
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
            this.label4.Location = new System.Drawing.Point(254, 231);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 18);
            this.label4.TabIndex = 13;
            this.label4.Text = "Mobile";
            // 
            // lblIsPasswordMatched
            // 
            this.lblIsPasswordMatched.AutoSize = true;
            this.lblIsPasswordMatched.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIsPasswordMatched.Location = new System.Drawing.Point(254, 360);
            this.lblIsPasswordMatched.Name = "lblIsPasswordMatched";
            this.lblIsPasswordMatched.Size = new System.Drawing.Size(35, 13);
            this.lblIsPasswordMatched.TabIndex = 19;
            this.lblIsPasswordMatched.Text = "label5";
            this.lblIsPasswordMatched.Visible = false;
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(40, 334);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.ReadOnly = true;
            this.txtPassword.Size = new System.Drawing.Size(176, 24);
            this.txtPassword.TabIndex = 14;
            this.txtPassword.Text = "User@1234567";
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // msgPassword
            // 
            this.msgPassword.AutoSize = true;
            this.msgPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msgPassword.Location = new System.Drawing.Point(40, 360);
            this.msgPassword.Name = "msgPassword";
            this.msgPassword.Size = new System.Drawing.Size(35, 13);
            this.msgPassword.TabIndex = 18;
            this.msgPassword.Text = "label4";
            this.msgPassword.Visible = false;
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
            // txtConfirmPassword
            // 
            this.txtConfirmPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConfirmPassword.Location = new System.Drawing.Point(257, 334);
            this.txtConfirmPassword.Name = "txtConfirmPassword";
            this.txtConfirmPassword.ReadOnly = true;
            this.txtConfirmPassword.Size = new System.Drawing.Size(176, 24);
            this.txtConfirmPassword.TabIndex = 15;
            this.txtConfirmPassword.Text = "User@1234567";
            this.txtConfirmPassword.TextChanged += new System.EventHandler(this.txtConfirmPassword_TextChanged);
            // 
            // panel2
            // 
            this.panel2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.lblEmpCode);
            this.panel2.Controls.Add(this.msglblFullname);
            this.panel2.Controls.Add(this.msglblLoginId);
            this.panel2.Controls.Add(this.lblOR);
            this.panel2.Controls.Add(this.txtName);
            this.panel2.Controls.Add(this.cbxEmpId);
            this.panel2.Controls.Add(this.lblEmpId);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Controls.Add(this.label13);
            this.panel2.Controls.Add(this.txtLoginId);
            this.panel2.Controls.Add(this.label14);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.cbxRole);
            this.panel2.Controls.Add(this.label11);
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
            this.panel2.Controls.Add(this.lblIsPasswordMatched);
            this.panel2.Controls.Add(this.txtPassword);
            this.panel2.Controls.Add(this.msgPassword);
            this.panel2.Controls.Add(this.txtConfirmPassword);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Location = new System.Drawing.Point(25, 60);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(481, 447);
            this.panel2.TabIndex = 35;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // msglblFullname
            // 
            this.msglblFullname.AutoSize = true;
            this.msglblFullname.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msglblFullname.Location = new System.Drawing.Point(37, 205);
            this.msglblFullname.Name = "msglblFullname";
            this.msglblFullname.Size = new System.Drawing.Size(35, 13);
            this.msglblFullname.TabIndex = 32;
            this.msglblFullname.Text = "label4";
            this.msglblFullname.Visible = false;
            // 
            // msglblLoginId
            // 
            this.msglblLoginId.AutoSize = true;
            this.msglblLoginId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msglblLoginId.Location = new System.Drawing.Point(39, 137);
            this.msglblLoginId.Name = "msglblLoginId";
            this.msglblLoginId.Size = new System.Drawing.Size(35, 13);
            this.msglblLoginId.TabIndex = 31;
            this.msglblLoginId.Text = "label4";
            this.msglblLoginId.Visible = false;
            // 
            // lblOR
            // 
            this.lblOR.AutoSize = true;
            this.lblOR.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOR.Location = new System.Drawing.Point(105, 70);
            this.lblOR.Name = "lblOR";
            this.lblOR.Size = new System.Drawing.Size(23, 13);
            this.lblOR.TabIndex = 30;
            this.lblOR.Text = "OR";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(254, 315);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(132, 18);
            this.label5.TabIndex = 17;
            this.label5.Text = "Confirm Password";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(40, 315);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 18);
            this.label6.TabIndex = 16;
            this.label6.Text = "Password";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.Image = global::DG_Tool.Properties.Resources.close_button_png_30241;
            this.pictureBox2.Location = new System.Drawing.Point(489, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(39, 37);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 13;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // lblHeading
            // 
            this.lblHeading.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lblHeading.AutoSize = true;
            this.lblHeading.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeading.ForeColor = System.Drawing.Color.White;
            this.lblHeading.Location = new System.Drawing.Point(7, 9);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Size = new System.Drawing.Size(89, 24);
            this.lblHeading.TabIndex = 0;
            this.lblHeading.Text = "Add User";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.IndianRed;
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.lblHeading);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(531, 40);
            this.panel1.TabIndex = 34;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 521);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(531, 31);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Note:";
            // 
            // lblEmpCode
            // 
            this.lblEmpCode.AutoSize = true;
            this.lblEmpCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmpCode.Location = new System.Drawing.Point(40, 58);
            this.lblEmpCode.Name = "lblEmpCode";
            this.lblEmpCode.Size = new System.Drawing.Size(35, 13);
            this.lblEmpCode.TabIndex = 33;
            this.lblEmpCode.Text = "label4";
            this.lblEmpCode.Visible = false;
            // 
            // AddUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SeaShell;
            this.ClientSize = new System.Drawing.Size(531, 552);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AddUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AddUser";
            this.Load += new System.EventHandler(this.AddUser_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.ComboBox cbxEmpId;
        private System.Windows.Forms.Label lblEmpId;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtLoginId;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbxRole;
        private System.Windows.Forms.Label label11;
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
        private System.Windows.Forms.Label lblIsPasswordMatched;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label msgPassword;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtConfirmPassword;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label lblHeading;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblOR;
        private System.Windows.Forms.Label msglblLoginId;
        private System.Windows.Forms.Label msglblFullname;
        private System.Windows.Forms.Label lblEmpCode;
    }
}