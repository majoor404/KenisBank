using System;
using System.Windows.Forms;

namespace KenisBank
{
    public partial class InputStringForm : Form
    {
        public InputStringForm()
        {
            InitializeComponent();
        }

        private void InputStringForm_Shown(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
            _ = textBox1.Focus();
        }
    }
}
