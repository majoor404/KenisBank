using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                textBoxDir.Text = dialog.SelectedPath;
            }
        }
    }
}
