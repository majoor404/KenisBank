using System;
using System.Windows.Forms;

namespace KenisBank
{
    public partial class Hoofdstuk : Form
    {
        public Hoofdstuk()
        {
            InitializeComponent();
        }

        private void Hoofdstuk_Shown(object sender, EventArgs e)
        {
            _ = textBox1.Focus();
        }
    }
}
