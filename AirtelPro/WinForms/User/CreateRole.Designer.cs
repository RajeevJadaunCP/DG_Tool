namespace DG_Tool.WinForms.User
{
    partial class CreateRole
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
            this.txtRole = new System.Windows.Forms.TextBox();
            this.Role = new System.Windows.Forms.Label();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.lblHeading = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.msgRoleName = new System.Windows.Forms.Label();
            this.dgvRoleList = new System.Windows.Forms.DataGridView();
            this.RolesID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RoleName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CretedOn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsActive = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoleList)).BeginInit();
            this.SuspendLayout();
            // 
            // txtRole
            // 
            this.txtRole.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRole.Location = new System.Drawing.Point(170, 46);
            this.txtRole.MaxLength = 20;
            this.txtRole.Name = "txtRole";
            this.txtRole.Size = new System.Drawing.Size(172, 23);
            this.txtRole.TabIndex = 0;
            this.txtRole.TextChanged += new System.EventHandler(this.txtRole_TextChanged);
            this.txtRole.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtRole_KeyPress);
            // 
            // Role
            // 
            this.Role.AutoSize = true;
            this.Role.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Role.Location = new System.Drawing.Point(86, 49);
            this.Role.Name = "Role";
            this.Role.Size = new System.Drawing.Size(78, 17);
            this.Role.TabIndex = 1;
            this.Role.Text = "Role Name";
            // 
            // btnSubmit
            // 
            this.btnSubmit.BackColor = System.Drawing.Color.IndianRed;
            this.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubmit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSubmit.ForeColor = System.Drawing.Color.White;
            this.btnSubmit.Location = new System.Drawing.Point(373, 39);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(110, 36);
            this.btnSubmit.TabIndex = 5;
            this.btnSubmit.Text = "SUBMIT";
            this.btnSubmit.UseVisualStyleBackColor = false;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // lblHeading
            // 
            this.lblHeading.AutoSize = true;
            this.lblHeading.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeading.ForeColor = System.Drawing.Color.White;
            this.lblHeading.Location = new System.Drawing.Point(7, 9);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Size = new System.Drawing.Size(109, 24);
            this.lblHeading.TabIndex = 0;
            this.lblHeading.Text = "Create Role";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.IndianRed;
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.lblHeading);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(787, 40);
            this.panel1.TabIndex = 35;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBox2.Image = global::DG_Tool.Properties.Resources.close_button_png_30241;
            this.pictureBox2.Location = new System.Drawing.Point(748, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(39, 40);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 13;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.msgRoleName);
            this.groupBox1.Controls.Add(this.txtRole);
            this.groupBox1.Controls.Add(this.Role);
            this.groupBox1.Controls.Add(this.btnSubmit);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(110, 57);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(562, 100);
            this.groupBox1.TabIndex = 36;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add New Role";
            // 
            // msgRoleName
            // 
            this.msgRoleName.AutoSize = true;
            this.msgRoleName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msgRoleName.Location = new System.Drawing.Point(167, 72);
            this.msgRoleName.Name = "msgRoleName";
            this.msgRoleName.Size = new System.Drawing.Size(60, 13);
            this.msgRoleName.TabIndex = 6;
            this.msgRoleName.Text = "Role Name";
            this.msgRoleName.Visible = false;
            // 
            // dgvRoleList
            // 
            this.dgvRoleList.AllowUserToAddRows = false;
            this.dgvRoleList.AllowUserToDeleteRows = false;
            this.dgvRoleList.AllowUserToResizeColumns = false;
            this.dgvRoleList.AllowUserToResizeRows = false;
            this.dgvRoleList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRoleList.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRoleList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvRoleList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRoleList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RolesID,
            this.RoleName,
            this.CreatedBy,
            this.CretedOn,
            this.IsActive});
            this.dgvRoleList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvRoleList.Location = new System.Drawing.Point(15, 173);
            this.dgvRoleList.Name = "dgvRoleList";
            this.dgvRoleList.RowHeadersVisible = false;
            this.dgvRoleList.Size = new System.Drawing.Size(760, 317);
            this.dgvRoleList.TabIndex = 37;
            this.dgvRoleList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRoleList_CellContentClick);
            // 
            // RolesID
            // 
            this.RolesID.DataPropertyName = "RoleId";
            this.RolesID.HeaderText = "RoleId";
            this.RolesID.Name = "RolesID";
            // 
            // RoleName
            // 
            this.RoleName.DataPropertyName = "RoleName";
            this.RoleName.HeaderText = "Role Name";
            this.RoleName.Name = "RoleName";
            // 
            // CreatedBy
            // 
            this.CreatedBy.DataPropertyName = "CreatedBy";
            this.CreatedBy.HeaderText = "Created By";
            this.CreatedBy.Name = "CreatedBy";
            // 
            // CretedOn
            // 
            this.CretedOn.DataPropertyName = "CreatedOn";
            this.CretedOn.HeaderText = "Created Date";
            this.CretedOn.Name = "CretedOn";
            // 
            // IsActive
            // 
            this.IsActive.DataPropertyName = "IsActive";
            this.IsActive.HeaderText = "Status";
            this.IsActive.Name = "IsActive";
            // 
            // CreateRole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SeaShell;
            this.ClientSize = new System.Drawing.Size(787, 502);
            this.Controls.Add(this.dgvRoleList);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CreateRole";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CreateRole";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoleList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtRole;
        private System.Windows.Forms.Label Role;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label lblHeading;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvRoleList;
        private System.Windows.Forms.Label msgRoleName;
        private System.Windows.Forms.DataGridViewTextBoxColumn RolesID;
        private System.Windows.Forms.DataGridViewTextBoxColumn RoleName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreatedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn CretedOn;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsActive;
    }
}