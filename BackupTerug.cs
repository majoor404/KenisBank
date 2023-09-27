using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KenisBank
{
    public partial class BackupTerug : Form
    {
        public BackupTerug()
        {
            InitializeComponent();
        }

        private void buttonBackup1_Click(object sender, EventArgs e)
        {
            string opslagnaam = $"Data\\{PaginaNaam.Text}.xml";
            string backup1 = $"Data\\{PaginaNaam.Text}_backup1.xml";
            string opslag0 = $"Data\\{PaginaNaam.Text}_backup0.xml";

            //van org naar 0
            File.Copy(opslagnaam, opslag0, true);

            //van 1 naar org
            if (File.Exists(backup1))
                File.Copy(backup1, opslagnaam, true);

            Close();
        }

        private void buttonBackup2_Click(object sender, EventArgs e)
        {
            string opslagnaam = $"Data\\{PaginaNaam.Text}.xml";
            string backup2 = $"Data\\{PaginaNaam.Text}_backup2.xml";
            string opslag0 = $"Data\\{PaginaNaam.Text}_backup0.xml";

            //van org naar 0
            File.Copy(opslagnaam, opslag0, true);

            //van 1 naar org
            if (File.Exists(backup2))
                File.Copy(backup2, opslagnaam, true);

            Close();
        }
    }
}
