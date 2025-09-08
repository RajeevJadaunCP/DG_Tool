namespace DG_Tool.WinForms.OutputFile
{
    partial class OFSatusList
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
            this.dgvBriefList = new System.Windows.Forms.DataGridView();
            this.pbRefresh = new System.Windows.Forms.GroupBox();
            this.pbReresh = new System.Windows.Forms.PictureBox();
            this.pbToCalander = new System.Windows.Forms.PictureBox();
            this.txtToDate = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFilepath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Customer = new System.Windows.Forms.Label();
            this.cbxCustomer = new System.Windows.Forms.ComboBox();
            this.cbxCircle = new System.Windows.Forms.ComboBox();
            this.pbFromCalander = new System.Windows.Forms.PictureBox();
            this.txtFromDate = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.fromCalendar = new System.Windows.Forms.MonthCalendar();
            this.toCalendar = new System.Windows.Forms.MonthCalendar();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBriefList)).BeginInit();
            this.pbRefresh.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbReresh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbToCalander)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFromCalander)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvBriefList
            // 
            this.dgvBriefList.AllowUserToAddRows = false;
            this.dgvBriefList.AllowUserToDeleteRows = false;
            this.dgvBriefList.AllowUserToResizeColumns = false;
            this.dgvBriefList.AllowUserToResizeRows = false;
            this.dgvBriefList.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dgvBriefList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBriefList.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBriefList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvBriefList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBriefList.Location = new System.Drawing.Point(-190, 175);
            this.dgvBriefList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvBriefList.Name = "dgvBriefList";
            this.dgvBriefList.ReadOnly = true;
            this.dgvBriefList.RowHeadersVisible = false;
            this.dgvBriefList.RowHeadersWidth = 51;
            this.dgvBriefList.Size = new System.Drawing.Size(1676, 437);
            this.dgvBriefList.TabIndex = 1;
            this.dgvBriefList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBriefList_CellContentClick);
            // 
            // pbRefresh
            // 
            this.pbRefresh.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pbRefresh.BackColor = System.Drawing.Color.White;
            this.pbRefresh.Controls.Add(this.pbReresh);
            this.pbRefresh.Controls.Add(this.pbToCalander);
            this.pbRefresh.Controls.Add(this.txtToDate);
            this.pbRefresh.Controls.Add(this.label5);
            this.pbRefresh.Controls.Add(this.label4);
            this.pbRefresh.Controls.Add(this.txtFilepath);
            this.pbRefresh.Controls.Add(this.label1);
            this.pbRefresh.Controls.Add(this.Customer);
            this.pbRefresh.Controls.Add(this.cbxCustomer);
            this.pbRefresh.Controls.Add(this.cbxCircle);
            this.pbRefresh.Controls.Add(this.pbFromCalander);
            this.pbRefresh.Controls.Add(this.txtFromDate);
            this.pbRefresh.Controls.Add(this.label3);
            this.pbRefresh.Location = new System.Drawing.Point(-195, 29);
            this.pbRefresh.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pbRefresh.Name = "pbRefresh";
            this.pbRefresh.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pbRefresh.Size = new System.Drawing.Size(1685, 91);
            this.pbRefresh.TabIndex = 5;
            this.pbRefresh.TabStop = false;
            // 
            // pbReresh
            // 
            this.pbReresh.Image = global::DG_Tool.Properties.Resources.refresh;
            this.pbReresh.Location = new System.Drawing.Point(1625, 30);
            this.pbReresh.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pbReresh.Name = "pbReresh";
            this.pbReresh.Size = new System.Drawing.Size(52, 39);
            this.pbReresh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbReresh.TabIndex = 7;
            this.pbReresh.TabStop = false;
            this.pbReresh.Click += new System.EventHandler(this.pbReresh_Click);
            // 
            // pbToCalander
            // 
            this.pbToCalander.Image = global::DG_Tool.Properties.Resources.calendar__1_;
            this.pbToCalander.Location = new System.Drawing.Point(1564, 53);
            this.pbToCalander.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pbToCalander.Name = "pbToCalander";
            this.pbToCalander.Size = new System.Drawing.Size(39, 30);
            this.pbToCalander.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbToCalander.TabIndex = 15;
            this.pbToCalander.TabStop = false;
            this.pbToCalander.Click += new System.EventHandler(this.pbToCalander_Click);
            // 
            // txtToDate
            // 
            this.txtToDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtToDate.Location = new System.Drawing.Point(1349, 53);
            this.txtToDate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtToDate.Name = "txtToDate";
            this.txtToDate.ReadOnly = true;
            this.txtToDate.Size = new System.Drawing.Size(201, 26);
            this.txtToDate.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(1248, 57);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 20);
            this.label5.TabIndex = 13;
            this.label5.Text = "To Date:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(708, 20);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 16);
            this.label4.TabIndex = 12;
            this.label4.Text = "File Path";
            this.label4.Visible = false;
            // 
            // txtFilepath
            // 
            this.txtFilepath.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFilepath.Location = new System.Drawing.Point(712, 39);
            this.txtFilepath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtFilepath.Name = "txtFilepath";
            this.txtFilepath.Size = new System.Drawing.Size(300, 28);
            this.txtFilepath.TabIndex = 11;
            this.txtFilepath.Visible = false;
            this.txtFilepath.TextChanged += new System.EventHandler(this.txtFilepath_TextChanged);
            this.txtFilepath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFilepath_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(363, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 16);
            this.label1.TabIndex = 10;
            this.label1.Text = "Circle";
            // 
            // Customer
            // 
            this.Customer.AutoSize = true;
            this.Customer.Location = new System.Drawing.Point(28, 21);
            this.Customer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Customer.Name = "Customer";
            this.Customer.Size = new System.Drawing.Size(64, 16);
            this.Customer.TabIndex = 9;
            this.Customer.Text = "Customer";
            // 
            // cbxCustomer
            // 
            this.cbxCustomer.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxCustomer.FormattingEnabled = true;
            this.cbxCustomer.Location = new System.Drawing.Point(32, 41);
            this.cbxCustomer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbxCustomer.Name = "cbxCustomer";
            this.cbxCustomer.Size = new System.Drawing.Size(300, 32);
            this.cbxCustomer.TabIndex = 7;
            this.cbxCustomer.SelectedIndexChanged += new System.EventHandler(this.cbxCustomer_SelectedIndexChanged);
            // 
            // cbxCircle
            // 
            this.cbxCircle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxCircle.FormattingEnabled = true;
            this.cbxCircle.Location = new System.Drawing.Point(367, 39);
            this.cbxCircle.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbxCircle.Name = "cbxCircle";
            this.cbxCircle.Size = new System.Drawing.Size(300, 32);
            this.cbxCircle.TabIndex = 8;
            this.cbxCircle.SelectedIndexChanged += new System.EventHandler(this.cbxCircle_SelectedIndexChanged);
            // 
            // pbFromCalander
            // 
            this.pbFromCalander.Image = global::DG_Tool.Properties.Resources.calendar__1_;
            this.pbFromCalander.Location = new System.Drawing.Point(1564, 20);
            this.pbFromCalander.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pbFromCalander.Name = "pbFromCalander";
            this.pbFromCalander.Size = new System.Drawing.Size(39, 30);
            this.pbFromCalander.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbFromCalander.TabIndex = 4;
            this.pbFromCalander.TabStop = false;
            this.pbFromCalander.Click += new System.EventHandler(this.pbFromCalander_Click);
            // 
            // txtFromDate
            // 
            this.txtFromDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFromDate.Location = new System.Drawing.Point(1349, 20);
            this.txtFromDate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtFromDate.Name = "txtFromDate";
            this.txtFromDate.ReadOnly = true;
            this.txtFromDate.Size = new System.Drawing.Size(201, 26);
            this.txtFromDate.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(1224, 23);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "From Date:";
            // 
            // fromCalendar
            // 
            this.fromCalendar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.fromCalendar.Location = new System.Drawing.Point(1340, 150);
            this.fromCalendar.Margin = new System.Windows.Forms.Padding(12, 11, 12, 11);
            this.fromCalendar.Name = "fromCalendar";
            this.fromCalendar.TabIndex = 4;
            this.fromCalendar.Visible = false;
            this.fromCalendar.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.fromCalendar_DateChanged);
            // 
            // toCalendar
            // 
            this.toCalendar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.toCalendar.Location = new System.Drawing.Point(1430, 150);
            this.toCalendar.Margin = new System.Windows.Forms.Padding(12, 11, 12, 11);
            this.toCalendar.Name = "toCalendar";
            this.toCalendar.TabIndex = 6;
            this.toCalendar.Visible = false;
            this.toCalendar.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.toCalendar_DateChanged);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.IndianRed;
            this.panel3.Controls.Add(this.label7);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1711, 61);
            this.panel3.TabIndex = 24;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(15, 10);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(213, 29);
            this.label7.TabIndex = 12;
            this.label7.Text = "Out File Status List";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(-195, -3);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 20);
            this.label2.TabIndex = 25;
            this.label2.Text = "Search:";
            // 
            // OFSatusList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SeaShell;
            this.ClientSize = new System.Drawing.Size(1369, 540);
            this.Controls.Add(this.toCalendar);
            this.Controls.Add(this.fromCalendar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.pbRefresh);
            this.Controls.Add(this.dgvBriefList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "OFSatusList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OFSatusList";
            ((System.ComponentModel.ISupportInitialize)(this.dgvBriefList)).EndInit();
            this.pbRefresh.ResumeLayout(false);
            this.pbRefresh.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbReresh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbToCalander)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFromCalander)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dgvBriefList;
        private System.Windows.Forms.GroupBox pbRefresh;
        private System.Windows.Forms.TextBox txtFromDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MonthCalendar fromCalendar;
        private System.Windows.Forms.PictureBox pbFromCalander;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Customer;
        private System.Windows.Forms.ComboBox cbxCustomer;
        private System.Windows.Forms.ComboBox cbxCircle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtFilepath;
        private System.Windows.Forms.PictureBox pbToCalander;
        private System.Windows.Forms.TextBox txtToDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.MonthCalendar toCalendar;
        private System.Windows.Forms.PictureBox pbReresh;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
    }
}