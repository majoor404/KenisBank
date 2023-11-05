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
    public partial class ZoekForm : Form
    {
        public ZoekForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Index word 2 keer per week ververst.\nU kunt ook nu index verversen.");
        }
    }
}
