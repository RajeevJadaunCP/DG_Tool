namespace DG_Tool.WinForms.Configure
{
    partial class ViewMasterFiles
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
            this.dgvMasterFiles = new System.Windows.Forms.DataGridView();
            this.btnAddNewFile = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pbCancel = new System.Windows.Forms.PictureBox();
            this.label10 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMasterFiles)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCancel)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvMasterFiles
            // 
            this.dgvMasterFiles.AllowUserToAddRows = false;
            this.dgvMasterFiles.AllowUserToDeleteRows = false;
            this.dgvMasterFiles.AllowUserToResizeColumns = false;
            this.dgvMasterFiles.AllowUserToResizeRows = false;
            this.dgvMasterFiles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMasterFiles.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMasterFiles.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMasterFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMasterFiles.Location = new System.Drawing.Point(17, 108);
            this.dgvMasterFiles.Name = "dgvMasterFiles";
            this.dgvMasterFiles.ReadOnly = true;
            this.dgvMasterFiles.RowHeadersVisible = false;
            this.dgvMasterFiles.Size = new System.Drawing.Size(931, 374);
            this.dgvMasterFiles.TabIndex = 0;
            this.dgvMasterFiles.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMasterFiles_CellContentClick);
            // 
            // btnAddNewFile
            // 
            this.btnAddNewFile.BackColor = System.Drawing.Color.IndianRed;
            this.btnAddNewFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddNewFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddNewFile.ForeColor = System.Drawing.Color.White;
            this.btnAddNewFile.Location = new System.Drawing.Point(830, 58);
            this.btnAddNewFile.Name = "btnAddNewFile";
            this.btnAddNewFile.Size = new System.Drawing.Size(118, 35);
            this.btnAddNewFile.TabIndex = 1;
            this.btnAddNewFile.Text = "Add New File";
            this.btnAddNewFile.UseVisualStyleBackColor = false;
            this.btnAddNewFile.Click += new System.EventHandler(this.btnAddNewFile_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.IndianRed;
            this.panel3.Controls.Add(this.pbCancel);
            this.panel3.Controls.Add(this.label10);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(967, 40);
            this.panel3.TabIndex = 47;
            // 
            // pbCancel
            // 
            this.pbCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.pbCancel.Image = global::DG_Tool.Properties.Resources.close_button_png_30241;
            this.pbCancel.Location = new System.Drawing.Point(928, 0);
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
            this.label10.Size = new System.Drawing.Size(158, 24);
            this.label10.TabIndex = 12;
            this.label10.Text = "View Master Files";
            // 
            // ViewMasterFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SeaShell;
            this.ClientSize = new System.Drawing.Size(967, 498);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.btnAddNewFile);
            this.Controls.Add(this.dgvMasterFiles);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ViewMasterFiles";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ViewMasterFiles";
            ((System.ComponentModel.ISupportInitialize)(this.dgvMasterFiles)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCancel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvMasterFiles;
        private System.Windows.Forms.Button btnAddNewFile;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox pbCancel;
        private System.Windows.Forms.Label label10;
    }
}