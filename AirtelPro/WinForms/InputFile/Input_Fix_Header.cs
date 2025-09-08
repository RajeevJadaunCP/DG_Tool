using System;using CardPrintingApplication;
using System.IO;
using System.Windows.Forms;

namespace DG_Tool.WinForms.InputFile
{
    public partial class Input_Fix_Header : Form
    {
        public Input_Fix_Header()
        {
            InitializeComponent();
        }

        private void Input_Fix_Header_Load(object sender, EventArgs e)
        {
            //string inputfilepath = "X:\\Code\\Rupesh\\INPUT_FILES\\Test_Input_File_Postpaid_DL.txt";

            //verticalTextBox1.Text = inputfilepath;

            //string text = File.ReadAllText(inputfilepath);
            //richTextBox1.Text = text;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "File Browser";
            fdlg.InitialDirectory = @"d:\";
            fdlg.Filter = "All files (*.*)|*.*|All files (*.*)|*.*";
            fdlg.FilterIndex = 2;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                verticalTextBox1.Text = fdlg.FileName;

                string text = File.ReadAllText(fdlg.FileName);
                richTextBox1.Text = text;

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialogfdlg = new SaveFileDialog();
            saveFileDialogfdlg.Title = "Save File";
            saveFileDialogfdlg.InitialDirectory = @"d:\";
            saveFileDialogfdlg.Filter = "txt files (*.txt*)|*.txt*";
            saveFileDialogfdlg.FilterIndex = 2;
            saveFileDialogfdlg.RestoreDirectory = true;

            if (saveFileDialogfdlg.ShowDialog() == DialogResult.OK)
            {
                TextWriter txt = new StreamWriter(saveFileDialogfdlg.FileName+@".txt");
                txt.Write(richTextBox2.Text);
                txt.Close();
                MessageBox.Show("Header File Created!");
                richTextBox1.Clear();
                richTextBox2.Clear();
                verticalTextBox1.Text = "Please Select Input File";
            }
        }

    }
}
