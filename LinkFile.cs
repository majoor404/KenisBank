using System;
using System.IO;
using System.Windows.Forms;

namespace KenisBank
{
    public partial class LinkFile : Form
    {
        public LinkFile()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";

                if (textBox1.Text.Length > 0)
                {
                    // start dir van oude link ivm wijzigen
                    openFileDialog.InitialDirectory = Path.GetDirectoryName(textBox1.Text);
                }
                
                openFileDialog.Filter = "All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    textBox1.Text = openFileDialog.FileName;
                }
            }
        }
    }
}
