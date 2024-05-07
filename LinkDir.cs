using System;
using System.Windows.Forms;

namespace KenisBank
{
    public partial class LinkDir : Form
    {
        public LinkDir()
        {
            InitializeComponent();
        }

        private void buttonOpenDir_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                textBoxDir.Text = dialog.SelectedPath;
            }
        }
    }
}
