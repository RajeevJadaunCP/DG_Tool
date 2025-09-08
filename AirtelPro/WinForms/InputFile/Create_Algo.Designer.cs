namespace DG_Tool.WinForms.InputFile
{
    partial class Create_Algo
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnSave = new System.Windows.Forms.Button();
            this.dgvFileSelection = new System.Windows.Forms.DataGridView();
            this.FileMasterID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FieldName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Selected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.headerCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFileSelection)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.IndianRed;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(861, 387);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(130, 38);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Submit";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dgvFileSelection
            // 
            this.dgvFileSelection.AllowUserToAddRows = false;
            this.dgvFileSelection.AllowUserToDeleteRows = false;
            this.dgvFileSelection.AllowUserToResizeColumns = false;
            this.dgvFileSelection.AllowUserToResizeRows = false;
            this.dgvFileSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvFileSelection.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvFileSelection.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvFileSelection.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFileSelection.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FileMasterID,
            this.FieldName,
            this.FileName,
            this.FileDesc,
            this.Selected});
            this.dgvFileSelection.Location = new System.Drawing.Point(33, 27);
            this.dgvFileSelection.Name = "dgvFileSelection";
            this.dgvFileSelection.ReadOnly = true;
            this.dgvFileSelection.RowHeadersVisible = false;
            this.dgvFileSelection.Size = new System.Drawing.Size(958, 354);
            this.dgvFileSelection.TabIndex = 3;
            this.dgvFileSelection.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFileSelection_CellContentClick);
            // 
            // FileMasterID
            // 
            this.FileMasterID.DataPropertyName = "FieldID";
            this.FileMasterID.HeaderText = "FieldID";
            this.FileMasterID.Name = "FileMasterID";
            this.FileMasterID.ReadOnly = true;
            this.FileMasterID.Width = 151;
            // 
            // FieldName
            // 
            this.FieldName.DataPropertyName = "FieldName";
            this.FieldName.HeaderText = "FieldName";
            this.FieldName.Name = "FieldName";
            this.FieldName.ReadOnly = true;
            this.FieldName.Width = 152;
            // 
            // FileName
            // 
            this.FileName.DataPropertyName = "AlgoName";
            this.FileName.HeaderText = "AlgoName";
            this.FileName.Name = "FileName";
            this.FileName.ReadOnly = true;
            this.FileName.Width = 151;
            // 
            // FileDesc
            // 
            this.FileDesc.DataPropertyName = "AlgoDes";
            this.FileDesc.HeaderText = "AlgoDesc";
            this.FileDesc.Name = "FileDesc";
            this.FileDesc.ReadOnly = true;
            this.FileDesc.Width = 152;
            // 
            // Selected
            // 
            this.Selected.DataPropertyName = "IsSelected";
            this.Selected.HeaderText = "Select          ";
            this.Selected.Name = "Selected";
            this.Selected.ReadOnly = true;
            this.Selected.Width = 151;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.BackColor = System.Drawing.Color.IndianRed;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(33, 387);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(159, 38);
            this.button1.TabIndex = 5;
            this.button1.Text = "Create New Algo";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // headerCheckBox
            // 
            this.headerCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.headerCheckBox.AutoSize = true;
            this.headerCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headerCheckBox.Location = new System.Drawing.Point(857, 5);
            this.headerCheckBox.Name = "headerCheckBox";
            this.headerCheckBox.Size = new System.Drawing.Size(92, 20);
            this.headerCheckBox.TabIndex = 6;
            this.headerCheckBox.Text = "Select All";
            this.headerCheckBox.UseVisualStyleBackColor = true;
            // 
            // Create_Algo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SeaShell;
            this.ClientSize = new System.Drawing.Size(1025, 443);
            this.Controls.Add(this.headerCheckBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dgvFileSelection);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "Create_Algo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create_Algo";
            ((System.ComponentModel.ISupportInitialize)(this.dgvFileSelection)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgvFileSelection;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileMasterID;
        private System.Windows.Forms.DataGridViewTextBoxColumn FieldName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileDesc;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Selected;
        private System.Windows.Forms.CheckBox headerCheckBox;
    }
}