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
    public partial class ItemToevoegen : Form
    {
        public ItemToevoegen()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            panelHoofdStuk.Visible = false;
            panelTextBlok.Visible = false;

            if (radioButton1.Checked )
            {
                panelHoofdStuk.Visible = true;
                panelHoofdStuk.BringToFront();
            }

            if (radioButton3.Checked)
            {
                panelTextBlok.Visible = true;
                panelTextBlok.BringToFront();
            }

            
            


        }

        private void ItemToevoegen_Shown(object sender, EventArgs e)
        {
            panelHoofdStuk.Location = panelTextBlok.Location = new Point(216, 12);
            panelHoofdStuk.Size = panelTextBlok.Size = new Size(1030, 722);
        }
    }
}
