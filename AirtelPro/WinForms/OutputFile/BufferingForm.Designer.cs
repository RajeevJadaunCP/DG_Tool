namespace DG_Tool.WinForms.OutputFile
{
    partial class BufferingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label labelMessage;

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
            this.labelMessage = new System.Windows.Forms.Label();
            this.Ghost = new System.ComponentModel.BackgroundWorker();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelMessage
            // 
            this.labelMessage.AutoSize = true;
            this.labelMessage.Location = new System.Drawing.Point(64, 73);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(153, 13);
            this.labelMessage.TabIndex = 0;
            this.labelMessage.Text = "Please wait, generating data....";
            // 
            // Ghost
            // 
            this.Ghost.WorkerReportsProgress = true;
            this.Ghost.WorkerSupportsCancellation = true;
            this.Ghost.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Ghost_DoWork);
            this.Ghost.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.Ghost_ProgressChanged);
            this.Ghost.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Ghost_RunWorkerCompleted);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 98);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(257, 24);
            this.progressBar.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(194, 128);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // BufferingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 164);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.labelMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BufferingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Processing";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker Ghost;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button button1;
    }
}