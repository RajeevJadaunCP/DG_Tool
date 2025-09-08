namespace AirtelPro.WinForms.OutputFile
{
    partial class FileLotMaster
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
            this.dgvFileLotList = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.Customer = new System.Windows.Forms.Label();
            this.cbxCustomer = new System.Windows.Forms.ComboBox();
            this.cbxCircle = new System.Windows.Forms.ComboBox();
            this.pbFromCalander = new System.Windows.Forms.PictureBox();
            this.txtFileReceivedDate = new System.Windows.Forms.TextBox();
            this.receivedDateCalendar = new System.Windows.Forms.MonthCalendar();
            this.label2 = new System.Windows.Forms.Label();
            this.pbReresh = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pbCancel = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFileLotList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFromCalander)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbReresh)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCancel)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvFileLotList
            // 
            this.dgvFileLotList.AllowUserToAddRows = false;
            this.dgvFileLotList.AllowUserToDeleteRows = false;
            this.dgvFileLotList.AllowUserToResizeColumns = false;
            this.dgvFileLotList.AllowUserToResizeRows = false;
            this.dgvFileLotList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvFileLotList.BackgroundColor = System.Drawing.Color.White;
            this.dgvFileLotList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFileLotList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvFileLotList.Location = new System.Drawing.Point(0, 134);
            this.dgvFileLotList.Name = "dgvFileLotList";
            this.dgvFileLotList.ReadOnly = true;
            this.dgvFileLotList.Size = new System.Drawing.Size(1242, 316);
            this.dgvFileLotList.TabIndex = 0;
            this.dgvFileLotList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFileLotList_CellContentClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(489, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Circle";
            // 
            // Customer
            // 
            this.Customer.AutoSize = true;
            this.Customer.Location = new System.Drawing.Point(73, 69);
            this.Customer.Name = "Customer";
            this.Customer.Size = new System.Drawing.Size(51, 13);
            this.Customer.TabIndex = 13;
            this.Customer.Text = "Customer";
            // 
            // cbxCustomer
            // 
            this.cbxCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxCustomer.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxCustomer.FormattingEnabled = true;
            this.cbxCustomer.Location = new System.Drawing.Point(76, 85);
            this.cbxCustomer.Name = "cbxCustomer";
            this.cbxCustomer.Size = new System.Drawing.Size(226, 26);
            this.cbxCustomer.TabIndex = 11;
            this.cbxCustomer.SelectedIndexChanged += new System.EventHandler(this.cbxCustomer_SelectedIndexChanged);
            // 
            // cbxCircle
            // 
            this.cbxCircle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxCircle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxCircle.FormattingEnabled = true;
            this.cbxCircle.Location = new System.Drawing.Point(492, 85);
            this.cbxCircle.Name = "cbxCircle";
            this.cbxCircle.Size = new System.Drawing.Size(226, 26);
            this.cbxCircle.TabIndex = 12;
            this.cbxCircle.SelectedIndexChanged += new System.EventHandler(this.cbxCircle_SelectedIndexChanged);
            // 
            // pbFromCalander
            // 
            this.pbFromCalander.Image = global::AirtelPro.Properties.Resources.calendar__1_;
            this.pbFromCalander.Location = new System.Drawing.Point(1100, 87);
            this.pbFromCalander.Name = "pbFromCalander";
            this.pbFromCalander.Size = new System.Drawing.Size(29, 24);
            this.pbFromCalander.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbFromCalander.TabIndex = 16;
            this.pbFromCalander.TabStop = false;
            this.pbFromCalander.Click += new System.EventHandler(this.pbFromCalander_Click);
            // 
            // txtFileReceivedDate
            // 
            this.txtFileReceivedDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFileReceivedDate.Location = new System.Drawing.Point(902, 87);
            this.txtFileReceivedDate.Name = "txtFileReceivedDate";
            this.txtFileReceivedDate.ReadOnly = true;
            this.txtFileReceivedDate.Size = new System.Drawing.Size(189, 23);
            this.txtFileReceivedDate.TabIndex = 15;
            // 
            // receivedDateCalendar
            // 
            this.receivedDateCalendar.Location = new System.Drawing.Point(902, 119);
            this.receivedDateCalendar.Name = "receivedDateCalendar";
            this.receivedDateCalendar.TabIndex = 17;
            this.receivedDateCalendar.Visible = false;
            this.receivedDateCalendar.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.receivedDateCalendar_DateSelected);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(899, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Date";
            // 
            // pbReresh
            // 
            this.pbReresh.Image = global::AirtelPro.Properties.Resources.refresh;
            this.pbReresh.Location = new System.Drawing.Point(1148, 87);
            this.pbReresh.Name = "pbReresh";
            this.pbReresh.Size = new System.Drawing.Size(25, 24);
            this.pbReresh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbReresh.TabIndex = 19;
            this.pbReresh.TabStop = false;
            this.pbReresh.Click += new System.EventHandler(this.pbReresh_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.IndianRed;
            this.panel3.Controls.Add(this.pbCancel);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1242, 40);
            this.panel3.TabIndex = 25;
            // 
            // pbCancel
            // 
            this.pbCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.pbCancel.Image = global::AirtelPro.Properties.Resources.close_button_png_30241;
            this.pbCancel.Location = new System.Drawing.Point(1203, 0);
            this.pbCancel.Name = "pbCancel";
            this.pbCancel.Size = new System.Drawing.Size(39, 40);
            this.pbCancel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbCancel.TabIndex = 13;
            this.pbCancel.TabStop = false;
            this.pbCancel.Click += new System.EventHandler(this.pbCancel_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(11, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(132, 24);
            this.label7.TabIndex = 12;
            this.label7.Text = "File Lot Master";
            // 
            // FileLotMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1242, 450);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.pbReresh);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.receivedDateCalendar);
            this.Controls.Add(this.pbFromCalander);
            this.Controls.Add(this.txtFileReceivedDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Customer);
            this.Controls.Add(this.cbxCustomer);
            this.Controls.Add(this.cbxCircle);
            this.Controls.Add(this.dgvFileLotList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FileLotMaster";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FileLotMaster";
            ((System.ComponentModel.ISupportInitialize)(this.dgvFileLotList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFromCalander)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbReresh)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCancel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvFileLotList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Customer;
        private System.Windows.Forms.ComboBox cbxCustomer;
        private System.Windows.Forms.ComboBox cbxCircle;
        private System.Windows.Forms.PictureBox pbFromCalander;
        private System.Windows.Forms.TextBox txtFileReceivedDate;
        private System.Windows.Forms.MonthCalendar receivedDateCalendar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pbReresh;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox pbCancel;
        private System.Windows.Forms.Label label7;
    }
}