using System.Windows.Forms;

namespace DG_Tool.WinForms.Authentication
{
	partial class LoginPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginPage));
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtPassword = new RupeshDesign.RSTextBoxNew();
            this.txtUsername = new RupeshDesign.RSTextBoxNew();
            this.rsButton1 = new RupeshDesign.RSButton();
            this.label6 = new System.Windows.Forms.Label();
            this.linkLabel4 = new System.Windows.Forms.LinkLabel();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.txtYears = new System.Windows.Forms.Label();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtVersion = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.rsButton2 = new RupeshDesign.RSButton();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.SeaShell;
            this.panel1.Controls.Add(this.txtPassword);
            this.panel1.Controls.Add(this.txtUsername);
            this.panel1.Controls.Add(this.rsButton1);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.linkLabel4);
            this.panel1.Controls.Add(this.linkLabel3);
            this.panel1.Controls.Add(this.txtYears);
            this.panel1.Controls.Add(this.linkLabel2);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.linkLabel1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(436, 575);
            this.panel1.TabIndex = 39;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseUp);
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.White;
            this.txtPassword.BorderColor = System.Drawing.Color.Gray;
            this.txtPassword.BorderRadius = 10;
            this.txtPassword.BorderWidth = 1;
            this.txtPassword.CornerRadius = 10;
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.IconScalingMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.txtPassword.IconSize = new System.Drawing.Size(16, 16);
            this.txtPassword.LeftIcon = global::DG_Tool.Properties.Resources.passwordIcon;
            this.txtPassword.LeftIconPadding = 10;
            this.txtPassword.LeftRightPadding = ((uint)(10u));
            this.txtPassword.Location = new System.Drawing.Point(51, 262);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.RightIcon = global::DG_Tool.Properties.Resources.hide;
            this.txtPassword.RightIconPadding = 5;
            this.txtPassword.Size = new System.Drawing.Size(315, 54);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.RightIconClick += new System.EventHandler(this.txtPassword_RightIconClick);
            // 
            // txtUsername
            // 
            this.txtUsername.BackColor = System.Drawing.Color.White;
            this.txtUsername.BorderColor = System.Drawing.Color.Gray;
            this.txtUsername.BorderRadius = 10;
            this.txtUsername.BorderWidth = 1;
            this.txtUsername.CornerRadius = 10;
            this.txtUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsername.IconScalingMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.txtUsername.IconSize = new System.Drawing.Size(16, 16);
            this.txtUsername.LeftIcon = global::DG_Tool.Properties.Resources.userIcon;
            this.txtUsername.LeftIconPadding = 10;
            this.txtUsername.LeftRightPadding = ((uint)(10u));
            this.txtUsername.Location = new System.Drawing.Point(51, 172);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(4);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.RightIcon = null;
            this.txtUsername.RightIconPadding = 5;
            this.txtUsername.Size = new System.Drawing.Size(315, 54);
            this.txtUsername.TabIndex = 1;
            this.txtUsername.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtUsername.UseSystemPasswordChar = false;
            // 
            // rsButton1
            // 
            this.rsButton1.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.rsButton1.BackgroundColor = System.Drawing.Color.MediumSlateBlue;
            this.rsButton1.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.rsButton1.BorderRadius = 10;
            this.rsButton1.BorderSize = 0;
            this.rsButton1.FlatAppearance.BorderSize = 0;
            this.rsButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rsButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rsButton1.ForeColor = System.Drawing.Color.White;
            this.rsButton1.Location = new System.Drawing.Point(51, 367);
            this.rsButton1.Margin = new System.Windows.Forms.Padding(4);
            this.rsButton1.Name = "rsButton1";
            this.rsButton1.Size = new System.Drawing.Size(315, 49);
            this.rsButton1.TabIndex = 3;
            this.rsButton1.Text = "Login";
            this.rsButton1.TextColor = System.Drawing.Color.White;
            this.rsButton1.UseVisualStyleBackColor = false;
            this.rsButton1.Click += new System.EventHandler(this.rsButton1_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(212, 540);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(13, 18);
            this.label6.TabIndex = 14;
            this.label6.Text = "|";
            // 
            // linkLabel4
            // 
            this.linkLabel4.AutoSize = true;
            this.linkLabel4.Location = new System.Drawing.Point(227, 543);
            this.linkLabel4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkLabel4.Name = "linkLabel4";
            this.linkLabel4.Size = new System.Drawing.Size(92, 16);
            this.linkLabel4.TabIndex = 13;
            this.linkLabel4.TabStop = true;
            this.linkLabel4.Text = "Privacy Policy";
            // 
            // linkLabel3
            // 
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.Location = new System.Drawing.Point(96, 543);
            this.linkLabel3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(109, 16);
            this.linkLabel3.TabIndex = 12;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "Terms of Service";
            // 
            // txtYears
            // 
            this.txtYears.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtYears.Location = new System.Drawing.Point(51, 476);
            this.txtYears.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.txtYears.Name = "txtYears";
            this.txtYears.Size = new System.Drawing.Size(320, 59);
            this.txtYears.TabIndex = 11;
            this.txtYears.Text = ".";
            this.txtYears.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(209, 322);
            this.linkLabel2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(147, 16);
            this.linkLabel2.TabIndex = 4;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "Forget Your Password?";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(51, 239);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 19);
            this.label4.TabIndex = 50;
            this.label4.Text = "Password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(51, 149);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 19);
            this.label3.TabIndex = 40;
            this.label3.Text = "Username";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(269, 446);
            this.linkLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(55, 16);
            this.linkLabel1.TabIndex = 5;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Sign Up";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(81, 443);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(180, 19);
            this.label2.TabIndex = 2;
            this.label2.Text = "Don\'t have an account?";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(163, 92);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 29);
            this.label1.TabIndex = 10;
            this.label1.Text = "Login";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::DG_Tool.Properties.Resources.colorplast_logo_hd;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(51, 16);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(320, 62);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = global::DG_Tool.Properties.Resources.DGtool;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Controls.Add(this.txtVersion);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.rsButton2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(436, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(631, 575);
            this.panel2.TabIndex = 20;
            this.panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel2_MouseDown);
            this.panel2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel2_MouseMove);
            this.panel2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel2_MouseUp);
            // 
            // txtVersion
            // 
            this.txtVersion.AutoSize = true;
            this.txtVersion.BackColor = System.Drawing.Color.Transparent;
            this.txtVersion.ForeColor = System.Drawing.Color.White;
            this.txtVersion.Location = new System.Drawing.Point(95, 548);
            this.txtVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(10, 16);
            this.txtVersion.TabIndex = 13;
            this.txtVersion.Text = ".";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Algerian", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.IndianRed;
            this.label7.Location = new System.Drawing.Point(24, 521);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(172, 22);
            this.label7.TabIndex = 12;
            this.label7.Text = "Data Gen Tool";
            // 
            // rsButton2
            // 
            this.rsButton2.BackColor = System.Drawing.Color.Red;
            this.rsButton2.BackgroundColor = System.Drawing.Color.Red;
            this.rsButton2.BackgroundImage = global::DG_Tool.Properties.Resources.close;
            this.rsButton2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.rsButton2.BorderColor = System.Drawing.Color.Red;
            this.rsButton2.BorderRadius = 0;
            this.rsButton2.BorderSize = 2;
            this.rsButton2.FlatAppearance.BorderSize = 0;
            this.rsButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rsButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rsButton2.ForeColor = System.Drawing.Color.White;
            this.rsButton2.Location = new System.Drawing.Point(579, 5);
            this.rsButton2.Margin = new System.Windows.Forms.Padding(4);
            this.rsButton2.Name = "rsButton2";
            this.rsButton2.Size = new System.Drawing.Size(47, 37);
            this.rsButton2.TabIndex = 19;
            this.rsButton2.TextColor = System.Drawing.Color.White;
            this.rsButton2.UseVisualStyleBackColor = false;
            this.rsButton2.Click += new System.EventHandler(this.rsButton2_Click);
            // 
            // LoginPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 575);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "LoginPage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private Panel panel1;

		// Token: 0x040001D6 RID: 470
		private Panel panel2;

		// Token: 0x040001D7 RID: 471
		private PictureBox pictureBox1;

		// Token: 0x040001D8 RID: 472
		private Label label1;

		// Token: 0x040001D9 RID: 473
		private Label label2;

		// Token: 0x040001DA RID: 474
		private LinkLabel linkLabel1;

		// Token: 0x040001DB RID: 475
		private Label label4;

		// Token: 0x040001DC RID: 476
		private Label label3;

		// Token: 0x040001E1 RID: 481
		private Label txtYears;

		// Token: 0x040001E2 RID: 482
		private LinkLabel linkLabel2;

		// Token: 0x040001E3 RID: 483
		private LinkLabel linkLabel4;

		// Token: 0x040001E4 RID: 484
		private LinkLabel linkLabel3;

		// Token: 0x040001E5 RID: 485
		private Label label6;
		private RupeshDesign.RSTextBoxNew txtUsername;
		private RupeshDesign.RSButton rsButton1;
		private RupeshDesign.RSTextBoxNew txtPassword;
		private RupeshDesign.RSButton rsButton2;
		private Label label7;
		private Label txtVersion;
    }
}