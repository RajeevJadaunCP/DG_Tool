namespace AirtelPro.WinForms.OutputFile
{
    partial class DataGenProcessList
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
            this.dataGVProcessList = new System.Windows.Forms.DataGridView();
            this.Customer = new System.Windows.Forms.Label();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.txtCircle = new System.Windows.Forms.Label();
            this.lblCircle = new System.Windows.Forms.Label();
            this.lbl3 = new System.Windows.Forms.Label();
            this.lblProfile = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnProcessAll = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGVProcessList)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGVProcessList
            // 
            this.dataGVProcessList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGVProcessList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGVProcessList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGVProcessList.Location = new System.Drawing.Point(0, 212);
            this.dataGVProcessList.Name = "dataGVProcessList";
            this.dataGVProcessList.Size = new System.Drawing.Size(1334, 266);
            this.dataGVProcessList.TabIndex = 0;
            this.dataGVProcessList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGVProcessList_CellContentClick);
            // 
            // Customer
            // 
            this.Customer.AutoSize = true;
            this.Customer.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Customer.Location = new System.Drawing.Point(191, 35);
            this.Customer.Name = "Customer";
            this.Customer.Size = new System.Drawing.Size(82, 18);
            this.Customer.TabIndex = 1;
            this.Customer.Text = "Customer :";
            // 
            // lblCustomer
            // 
            this.lblCustomer.AutoSize = true;
            this.lblCustomer.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomer.Location = new System.Drawing.Point(283, 35);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(46, 18);
            this.lblCustomer.TabIndex = 2;
            this.lblCustomer.Text = "label2";
            // 
            // txtCircle
            // 
            this.txtCircle.AutoSize = true;
            this.txtCircle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCircle.Location = new System.Drawing.Point(415, 35);
            this.txtCircle.Name = "txtCircle";
            this.txtCircle.Size = new System.Drawing.Size(54, 18);
            this.txtCircle.TabIndex = 3;
            this.txtCircle.Text = "Circle :";
            // 
            // lblCircle
            // 
            this.lblCircle.AutoSize = true;
            this.lblCircle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCircle.Location = new System.Drawing.Point(474, 35);
            this.lblCircle.Name = "lblCircle";
            this.lblCircle.Size = new System.Drawing.Size(46, 18);
            this.lblCircle.TabIndex = 4;
            this.lblCircle.Text = "label4";
            // 
            // lbl3
            // 
            this.lbl3.AutoSize = true;
            this.lbl3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl3.Location = new System.Drawing.Point(647, 35);
            this.lbl3.Name = "lbl3";
            this.lbl3.Size = new System.Drawing.Size(58, 18);
            this.lbl3.TabIndex = 5;
            this.lbl3.Text = "Profile :";
            // 
            // lblProfile
            // 
            this.lblProfile.AutoSize = true;
            this.lblProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProfile.Location = new System.Drawing.Point(711, 35);
            this.lblProfile.Name = "lblProfile";
            this.lblProfile.Size = new System.Drawing.Size(46, 18);
            this.lblProfile.TabIndex = 6;
            this.lblProfile.Text = "label6";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Customer);
            this.groupBox1.Controls.Add(this.lblProfile);
            this.groupBox1.Controls.Add(this.lblCustomer);
            this.groupBox1.Controls.Add(this.lbl3);
            this.groupBox1.Controls.Add(this.txtCircle);
            this.groupBox1.Controls.Add(this.lblCircle);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1334, 82);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Customer Details";
            // 
            // btnProcessAll
            // 
            this.btnProcessAll.Location = new System.Drawing.Point(1197, 171);
            this.btnProcessAll.Name = "btnProcessAll";
            this.btnProcessAll.Size = new System.Drawing.Size(130, 31);
            this.btnProcessAll.TabIndex = 8;
            this.btnProcessAll.Text = "Process All";
            this.btnProcessAll.UseVisualStyleBackColor = true;
            // 
            // DataGenProcessList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1334, 478);
            this.Controls.Add(this.btnProcessAll);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dataGVProcessList);
            this.Name = "DataGenProcessList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DataGenProcessList";
            ((System.ComponentModel.ISupportInitialize)(this.dataGVProcessList)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGVProcessList;
        private System.Windows.Forms.Label Customer;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.Label txtCircle;
        private System.Windows.Forms.Label lblCircle;
        private System.Windows.Forms.Label lbl3;
        private System.Windows.Forms.Label lblProfile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnProcessAll;
    }
}