using DG_Tool.HelperClass;
using System;using CardPrintingApplication;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DG_Tool.WinForms.OutputFile
{
    public partial class BufferingForm : Form
    {
        private Panel panel;
        private System.Windows.Forms.Timer timer;
        private int angle;
        public List<int> Hdids = null;
        public OFSatusList of = new OFSatusList();
        public BufferingForm(int lot)
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 100; 
            timer.Tick += Timer_Tick;
            timer.Start();
            Panel bufferingPanel = new Panel
            {
                Location = new Point(110, 12),
                Size = new Size(50, 50)
            };
            bufferingPanel.Paint += BufferingPanel_Paint;
            Controls.Add(bufferingPanel);
            InitializeComponent();
            Hdids = of.DataGenProcessHDFileByHDID(lot);
            progressBar.Value = 0;
            progressBar.Maximum = Hdids.Count;
            Ghost.RunWorkerAsync();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            angle = (angle + 10) % 360; 
            Invalidate(true);
        }

        private void BufferingPanel_Paint(object sender, PaintEventArgs e)
        {
            DrawBufferingCircle(e.Graphics, ((Panel)sender).ClientRectangle, angle);
        }

        private void DrawBufferingCircle(Graphics g, Rectangle bounds, int angle)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int circleRadius = Math.Min(bounds.Width, bounds.Height) / 2 - 10;
            Point center = new Point(bounds.Width / 2, bounds.Height / 2);
            int numSegments = 12;
            int segmentRadius = circleRadius / 6;

            for (int i = 0; i < numSegments; i++)
            {
                float segmentAngle = (360f / numSegments) * i + angle;
                double radians = segmentAngle * Math.PI / 180;
                Point segmentCenter = new Point(
                    center.X + (int)(Math.Cos(radians) * circleRadius),
                    center.Y + (int)(Math.Sin(radians) * circleRadius)
                );

                int alpha = (int)(255 * (i + 1) / (float)numSegments);
                using (Brush brush = new SolidBrush(Color.FromArgb(alpha, Color.Black)))
                {
                    g.FillEllipse(brush, segmentCenter.X - segmentRadius, segmentCenter.Y - segmentRadius, segmentRadius * 2, segmentRadius * 2);
                }
            }
        }

        private void Ghost_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value += 1;
        }

        private void Ghost_DoWork(object sender, DoWorkEventArgs e)
        {
            
            foreach (int Hdid in Hdids)
            {
                LogMaster.addlog($"Reprocessing Started for {Hdid} HDID.");
                of.btnProcessAll_Click(Hdid);
                Ghost.ReportProgress(Hdid);
                LogMaster.addlog($"{Hdid} HDID is Reprocessed Succesfully.");
            }
            LogMaster.addlog($"**All HDIDs is Reprocessed Succesfully.**");
        }

        private void BufferingForm_Load(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Ghost.CancelAsync();
            this.Close();
        }

        private void Ghost_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            using (SqlConnection con = new SqlConnection(of.connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE FileLotMaster SET [DataGenProcessStatus]= 15 WHERE ID = (SELECT MAX(ID) FROM FileLotMaster)", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
            this.Close();
        }
    }
}
