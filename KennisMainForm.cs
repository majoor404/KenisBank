using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KenisBank
{
    public partial class KennisMainForm : Form
    {
        Regel regels = new Regel();
        public KennisMainForm()
        {
            InitializeComponent();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {

        }

        private void KennisMainForm_Shown(object sender, EventArgs e)
        {
            // laad main.xml
            regels.Laad(@"Data\main.xml");
        }


    }
}
