using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace KenisBank
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void About_Shown(object sender, EventArgs e)
        {
            DateTime buildDate = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;
            label1.Text = buildDate.ToString();
            try
            {
                textBox2.Clear();
                string[] lines = File.ReadAllLines("Data\\versie.ini");
                textBox2.Text = string.Join(Environment.NewLine, lines);
                textBox2.SelectionStart = textBox2.TextLength;
                textBox2.ScrollToCaret();
            }
            catch (Exception ex)
            {
                Logger.Log(ex, "About_Shown: failed to load versie.ini");
            };
        }


    }
}
