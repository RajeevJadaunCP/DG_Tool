namespace DG_Tool.WinForms.Configure
{
    partial class CreateDataToolParameter
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.txtParameterName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtParameterDescription = new System.Windows.Forms.RichTextBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.dgvParameterList = new System.Windows.Forms.DataGridView();
            this.ParameterID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ParameterCharVal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ParameterIntVal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ParameterName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ParameterDetails = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pbCancel = new System.Windows.Forms.PictureBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParameterList)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCancel)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(95, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Parameter Name";
            // 
            // txtParameterName
            // 
            this.txtParameterName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtParameterName.Location = new System.Drawing.Point(98, 45);
            this.txtParameterName.MaxLength = 25;
            this.txtParameterName.Name = "txtParameterName";
            this.txtParameterName.Size = new System.Drawing.Size(244, 23);
            this.txtParameterName.TabIndex = 1;
            this.txtParameterName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtParameterName_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(95, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Parameter Description";
            // 
            // txtParameterDescription
            // 
            this.txtParameterDescription.Location = new System.Drawing.Point(98, 114);
            this.txtParameterDescription.Name = "txtParameterDescription";
            this.txtParameterDescription.Size = new System.Drawing.Size(244, 96);
            this.txtParameterDescription.TabIndex = 6;
            this.txtParameterDescription.Text = "";
            this.txtParameterDescription.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtParameterDescription_KeyPress);
            // 
            // btnSubmit
            // 
            this.btnSubmit.BackColor = System.Drawing.Color.Brown;
            this.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSubmit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSubmit.ForeColor = System.Drawing.Color.White;
            this.btnSubmit.Location = new System.Drawing.Point(396, 173);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(161, 37);
            this.btnSubmit.TabIndex = 7;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = false;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // dgvParameterList
            // 
            this.dgvParameterList.AllowUserToAddRows = false;
            this.dgvParameterList.AllowUserToDeleteRows = false;
            this.dgvParameterList.AllowUserToResizeColumns = false;
            this.dgvParameterList.AllowUserToResizeRows = false;
            this.dgvParameterList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvParameterList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvParameterList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvParameterList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ParameterID,
            this.ParameterCharVal,
            this.ParameterIntVal,
            this.ParameterName,
            this.ParameterDetails});
            this.dgvParameterList.Location = new System.Drawing.Point(12, 275);
            this.dgvParameterList.Name = "dgvParameterList";
            this.dgvParameterList.ReadOnly = true;
            this.dgvParameterList.RowHeadersVisible = false;
            this.dgvParameterList.Size = new System.Drawing.Size(912, 246);
            this.dgvParameterList.TabIndex = 8;
            this.dgvParameterList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvParameterList_CellContentClick);
            // 
            // ParameterID
            // 
            this.ParameterID.DataPropertyName = "ParameterID";
            this.ParameterID.HeaderText = "Parameter Id";
            this.ParameterID.Name = "ParameterID";
            this.ParameterID.ReadOnly = true;
            // 
            // ParameterCharVal
            // 
            this.ParameterCharVal.DataPropertyName = "ParameterCharVal";
            this.ParameterCharVal.HeaderText = "Char Value";
            this.ParameterCharVal.Name = "ParameterCharVal";
            this.ParameterCharVal.ReadOnly = true;
            // 
            // ParameterIntVal
            // 
            this.ParameterIntVal.DataPropertyName = "ParameterIntVal";
            this.ParameterIntVal.HeaderText = "Int Value";
            this.ParameterIntVal.Name = "ParameterIntVal";
            this.ParameterIntVal.ReadOnly = true;
            // 
            // ParameterName
            // 
            this.ParameterName.DataPropertyName = "ParameterName";
            this.ParameterName.HeaderText = "Parameter Name";
            this.ParameterName.Name = "ParameterName";
            this.ParameterName.ReadOnly = true;
            // 
            // ParameterDetails
            // 
            this.ParameterDetails.DataPropertyName = "ParameterDetails";
            this.ParameterDetails.HeaderText = "Parameter Details";
            this.ParameterDetails.Name = "ParameterDetails";
            this.ParameterDetails.ReadOnly = true;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.IndianRed;
            this.panel3.Controls.Add(this.pbCancel);
            this.panel3.Controls.Add(this.label10);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(936, 40);
            this.panel3.TabIndex = 47;
            // 
            // pbCancel
            // 
            this.pbCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.pbCancel.Image = global::DG_Tool.Properties.Resources.close_button_png_30241;
            this.pbCancel.Location = new System.Drawing.Point(897, 0);
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
            this.label10.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label10.Size = new System.Drawing.Size(168, 24);
            this.label10.TabIndex = 12;
            this.label10.Text = "View Configuration";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtParameterName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnSubmit);
            this.groupBox1.Controls.Add(this.txtParameterDescription);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(124, 46);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(628, 223);
            this.groupBox1.TabIndex = 48;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parameter Details";
            // 
            // CreateDataToolParameter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SeaShell;
            this.ClientSize = new System.Drawing.Size(936, 536);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.dgvParameterList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CreateDataToolParameter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "View Configuration";
            ((System.ComponentModel.ISupportInitialize)(this.dgvParameterList)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCancel)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtParameterName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox txtParameterDescription;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.DataGridView dgvParameterList;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox pbCancel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParameterID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParameterCharVal;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParameterIntVal;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParameterName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParameterDetails;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}