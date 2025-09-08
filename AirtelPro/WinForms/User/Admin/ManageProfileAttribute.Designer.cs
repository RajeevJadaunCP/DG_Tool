namespace DG_Tool.WinForms.User.Admin
{
    partial class ManageProfileAttribute
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
            this.dgvAttributeItems = new System.Windows.Forms.DataGridView();
            this.btnAddNewItem = new System.Windows.Forms.Button();
            this.txtProfileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblAttribute = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.lblHeading = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttributeItems)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvAttributeItems
            // 
            this.dgvAttributeItems.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAttributeItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAttributeItems.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvAttributeItems.Location = new System.Drawing.Point(0, 174);
            this.dgvAttributeItems.Name = "dgvAttributeItems";
            this.dgvAttributeItems.RowHeadersVisible = false;
            this.dgvAttributeItems.Size = new System.Drawing.Size(800, 329);
            this.dgvAttributeItems.TabIndex = 0;
            // 
            // btnAddNewItem
            // 
            this.btnAddNewItem.BackColor = System.Drawing.Color.IndianRed;
            this.btnAddNewItem.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddNewItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddNewItem.ForeColor = System.Drawing.Color.White;
            this.btnAddNewItem.Location = new System.Drawing.Point(497, 110);
            this.btnAddNewItem.Name = "btnAddNewItem";
            this.btnAddNewItem.Size = new System.Drawing.Size(138, 28);
            this.btnAddNewItem.TabIndex = 1;
            this.btnAddNewItem.Text = "Add New Item";
            this.btnAddNewItem.UseVisualStyleBackColor = false;
            this.btnAddNewItem.Click += new System.EventHandler(this.btnAddNewItem_Click);
            // 
            // txtProfileName
            // 
            this.txtProfileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProfileName.Location = new System.Drawing.Point(232, 113);
            this.txtProfileName.Name = "txtProfileName";
            this.txtProfileName.Size = new System.Drawing.Size(243, 23);
            this.txtProfileName.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(33, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Atrribute :";
            // 
            // lblAttribute
            // 
            this.lblAttribute.AutoSize = true;
            this.lblAttribute.BackColor = System.Drawing.Color.Yellow;
            this.lblAttribute.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAttribute.Location = new System.Drawing.Point(107, 75);
            this.lblAttribute.Name = "lblAttribute";
            this.lblAttribute.Size = new System.Drawing.Size(46, 17);
            this.lblAttribute.TabIndex = 4;
            this.lblAttribute.Text = "label2";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.IndianRed;
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.lblHeading);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 40);
            this.panel1.TabIndex = 39;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBox2.Image = global::DG_Tool.Properties.Resources.close_button_png_30241;
            this.pictureBox2.Location = new System.Drawing.Point(761, 0);
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
            this.lblHeading.Size = new System.Drawing.Size(209, 24);
            this.lblHeading.TabIndex = 0;
            this.lblHeading.Text = "Manage Profile Attribute";
            // 
            // ManageProfileAttribute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SeaShell;
            this.ClientSize = new System.Drawing.Size(800, 503);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblAttribute);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtProfileName);
            this.Controls.Add(this.btnAddNewItem);
            this.Controls.Add(this.dgvAttributeItems);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ManageProfileAttribute";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ManageProfileAttribute";
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttributeItems)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvAttributeItems;
        private System.Windows.Forms.Button btnAddNewItem;
        private System.Windows.Forms.TextBox txtProfileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblAttribute;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label lblHeading;
    }
}