namespace DG_Tool.WinForms.User.Admin
{
    partial class ProfileAttributeList
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtProfileAttributeHD = new System.Windows.Forms.TextBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.dgvProfileAttributeList = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProfileAttributeHDID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HeaderName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.lblHeading = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProfileAttributeList)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // txtProfileAttributeHD
            // 
            this.txtProfileAttributeHD.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProfileAttributeHD.Location = new System.Drawing.Point(297, 75);
            this.txtProfileAttributeHD.Name = "txtProfileAttributeHD";
            this.txtProfileAttributeHD.Size = new System.Drawing.Size(242, 23);
            this.txtProfileAttributeHD.TabIndex = 0;
            this.txtProfileAttributeHD.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtProfileAttributeHD_KeyPress);
            // 
            // btnSubmit
            // 
            this.btnSubmit.BackColor = System.Drawing.Color.IndianRed;
            this.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSubmit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSubmit.ForeColor = System.Drawing.Color.White;
            this.btnSubmit.Location = new System.Drawing.Point(563, 73);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(151, 29);
            this.btnSubmit.TabIndex = 1;
            this.btnSubmit.Text = "Add Profile Attribute";
            this.btnSubmit.UseVisualStyleBackColor = false;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // dgvProfileAttributeList
            // 
            this.dgvProfileAttributeList.AllowUserToAddRows = false;
            this.dgvProfileAttributeList.AllowUserToDeleteRows = false;
            this.dgvProfileAttributeList.AllowUserToResizeColumns = false;
            this.dgvProfileAttributeList.AllowUserToResizeRows = false;
            this.dgvProfileAttributeList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvProfileAttributeList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvProfileAttributeList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProfileAttributeList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.ProfileAttributeHDID,
            this.HeaderName});
            this.dgvProfileAttributeList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvProfileAttributeList.Location = new System.Drawing.Point(0, 131);
            this.dgvProfileAttributeList.Name = "dgvProfileAttributeList";
            this.dgvProfileAttributeList.RowHeadersVisible = false;
            this.dgvProfileAttributeList.Size = new System.Drawing.Size(934, 319);
            this.dgvProfileAttributeList.TabIndex = 2;
            this.dgvProfileAttributeList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProfileAttributeList_CellContentClick);
            // 
            // ID
            // 
            this.ID.DataPropertyName = "Id";
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            // 
            // ProfileAttributeHDID
            // 
            this.ProfileAttributeHDID.DataPropertyName = "ProfileAttributeHDID";
            this.ProfileAttributeHDID.HeaderText = "Attribute HDID";
            this.ProfileAttributeHDID.Name = "ProfileAttributeHDID";
            // 
            // HeaderName
            // 
            this.HeaderName.DataPropertyName = "HeaderName";
            this.HeaderName.HeaderText = "Header Name";
            this.HeaderName.Name = "HeaderName";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(317, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Profile Attribute HD";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.IndianRed;
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.lblHeading);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(934, 40);
            this.panel1.TabIndex = 39;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBox2.Image = global::DG_Tool.Properties.Resources.close_button_png_30241;
            this.pictureBox2.Location = new System.Drawing.Point(895, 0);
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
            this.lblHeading.Size = new System.Drawing.Size(195, 24);
            this.lblHeading.TabIndex = 0;
            this.lblHeading.Text = "Create Profile Attribute";
            // 
            // ProfileAttributeList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SeaShell;
            this.ClientSize = new System.Drawing.Size(934, 450);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvProfileAttributeList);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.txtProfileAttributeHD);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ProfileAttributeList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create Profile Attribute";
            ((System.ComponentModel.ISupportInitialize)(this.dgvProfileAttributeList)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtProfileAttributeHD;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.DataGridView dgvProfileAttributeList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProfileAttributeHDID;
        private System.Windows.Forms.DataGridViewTextBoxColumn HeaderName;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label lblHeading;
    }
}