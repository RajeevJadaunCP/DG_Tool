namespace DG_Tool
{
    partial class CustomerProfileList
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
            this.dataGridViewCustomer = new System.Windows.Forms.DataGridView();
            this.ProfileID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CirclName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProfileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InputFileCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OutFileCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InputFileProcessed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Customer = new System.Windows.Forms.Label();
            this.cbxCustomer = new System.Windows.Forms.ComboBox();
            this.cbxCircle = new System.Windows.Forms.ComboBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblHeading = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCustomer)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewCustomer
            // 
            this.dataGridViewCustomer.AllowUserToAddRows = false;
            this.dataGridViewCustomer.AllowUserToDeleteRows = false;
            this.dataGridViewCustomer.AllowUserToResizeColumns = false;
            this.dataGridViewCustomer.AllowUserToResizeRows = false;
            this.dataGridViewCustomer.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dataGridViewCustomer.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewCustomer.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Brown;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewCustomer.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewCustomer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCustomer.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProfileID,
            this.CustName,
            this.CirclName,
            this.ProfileName,
            this.InputFileCount,
            this.OutFileCount,
            this.InputFileProcessed});
            this.dataGridViewCustomer.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewCustomer.Location = new System.Drawing.Point(10, 189);
            this.dataGridViewCustomer.Name = "dataGridViewCustomer";
            this.dataGridViewCustomer.RowHeadersVisible = false;
            this.dataGridViewCustomer.Size = new System.Drawing.Size(1226, 355);
            this.dataGridViewCustomer.TabIndex = 0;
            this.dataGridViewCustomer.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewCustomer_CellContentClick);
            // 
            // ProfileID
            // 
            this.ProfileID.DataPropertyName = "ProfileId";
            this.ProfileID.HeaderText = "Profile Id";
            this.ProfileID.Name = "ProfileID";
            // 
            // CustName
            // 
            this.CustName.DataPropertyName = "CustomerName";
            this.CustName.HeaderText = "Customer Name";
            this.CustName.Name = "CustName";
            // 
            // CirclName
            // 
            this.CirclName.DataPropertyName = "CircleName";
            this.CirclName.HeaderText = "Circle Name";
            this.CirclName.Name = "CirclName";
            // 
            // ProfileName
            // 
            this.ProfileName.DataPropertyName = "ProfileName";
            this.ProfileName.HeaderText = "Profile Name";
            this.ProfileName.Name = "ProfileName";
            // 
            // InputFileCount
            // 
            this.InputFileCount.DataPropertyName = "No_of_InputFile";
            this.InputFileCount.HeaderText = "No of Input File";
            this.InputFileCount.Name = "InputFileCount";
            // 
            // OutFileCount
            // 
            this.OutFileCount.DataPropertyName = "No_of_OutputFile";
            this.OutFileCount.HeaderText = "No of Output File";
            this.OutFileCount.Name = "OutFileCount";
            // 
            // InputFileProcessed
            // 
            this.InputFileProcessed.DataPropertyName = "Input_File_Processed";
            this.InputFileProcessed.HeaderText = "Input File Processed";
            this.InputFileProcessed.Name = "InputFileProcessed";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox2.BackColor = System.Drawing.Color.Snow;
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.Customer);
            this.groupBox2.Controls.Add(this.cbxCustomer);
            this.groupBox2.Controls.Add(this.cbxCircle);
            this.groupBox2.Controls.Add(this.txtSearch);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(10, 71);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1225, 92);
            this.groupBox2.TabIndex = 39;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Search Details";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(780, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 16);
            this.label4.TabIndex = 7;
            this.label4.Text = "Profile";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(448, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "Circle";
            // 
            // Customer
            // 
            this.Customer.AutoSize = true;
            this.Customer.Location = new System.Drawing.Point(110, 25);
            this.Customer.Name = "Customer";
            this.Customer.Size = new System.Drawing.Size(72, 16);
            this.Customer.TabIndex = 5;
            this.Customer.Text = "Customer";
            // 
            // cbxCustomer
            // 
            this.cbxCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxCustomer.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxCustomer.FormattingEnabled = true;
            this.cbxCustomer.Location = new System.Drawing.Point(110, 46);
            this.cbxCustomer.Name = "cbxCustomer";
            this.cbxCustomer.Size = new System.Drawing.Size(226, 26);
            this.cbxCustomer.TabIndex = 2;
            this.cbxCustomer.SelectedIndexChanged += new System.EventHandler(this.cbxCustomer_SelectedIndexChanged);
            // 
            // cbxCircle
            // 
            this.cbxCircle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxCircle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxCircle.FormattingEnabled = true;
            this.cbxCircle.Location = new System.Drawing.Point(448, 43);
            this.cbxCircle.Name = "cbxCircle";
            this.cbxCircle.Size = new System.Drawing.Size(226, 26);
            this.cbxCircle.TabIndex = 3;
            this.cbxCircle.SelectedIndexChanged += new System.EventHandler(this.cbxCircle_SelectedIndexChanged);
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.Location = new System.Drawing.Point(780, 43);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(226, 24);
            this.txtSearch.TabIndex = 4;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSearch_KeyPress);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.IndianRed;
            this.panel1.Controls.Add(this.lblHeading);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1249, 40);
            this.panel1.TabIndex = 42;
            // 
            // lblHeading
            // 
            this.lblHeading.AutoSize = true;
            this.lblHeading.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeading.ForeColor = System.Drawing.Color.White;
            this.lblHeading.Location = new System.Drawing.Point(7, 9);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Size = new System.Drawing.Size(180, 24);
            this.lblHeading.TabIndex = 0;
            this.lblHeading.Text = "Customer Profile List";
            // 
            // CustomerProfileList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SeaShell;
            this.ClientSize = new System.Drawing.Size(1249, 554);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.dataGridViewCustomer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CustomerProfileList";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AllCustomers";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCustomer)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewCustomer;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label Customer;
        private System.Windows.Forms.ComboBox cbxCustomer;
        private System.Windows.Forms.ComboBox cbxCircle;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProfileID;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CirclName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProfileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn InputFileCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn OutFileCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn InputFileProcessed;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblHeading;
    }
}