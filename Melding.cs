using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Melding
{
    public partial class FormMelding : Form
    {
        private int meldingX, meldingY;
        private readonly int schermbreed, schermhoog;
        private int close;
        private statusForm status;

        private enum statusForm { start, show, eind};
        public enum Type { Info, Cal, Err, Klaar, Edit, Save , Note};

        public FormMelding(Type type, string regel1, string regel2)
        {
            InitializeComponent();
            schermbreed = Screen.PrimaryScreen.WorkingArea.Width;
            schermhoog = Screen.PrimaryScreen.WorkingArea.Height;

            label1.Text = regel1;
            label2.Text = regel2;

            switch (type)
            {
                case Type.Info:
                    pictureBox.Image = KenisBank.Properties.Resources.info;
                    panelColor.BackColor = Color.FromArgb(0, 147, 241);
                    break;
                case Type.Cal:
                    pictureBox.Image = KenisBank.Properties.Resources.calendar;
                    panelColor.BackColor = Color.FromArgb(226, 209, 203);
                    break;
                case Type.Err:
                    pictureBox.Image = KenisBank.Properties.Resources.caution;
                    panelColor.BackColor = Color.FromArgb(255, 179, 32);
                    break;
                case Type.Klaar:
                    pictureBox.Image = KenisBank.Properties.Resources.finish;
                    panelColor.BackColor = Color.FromArgb(80, 56, 55);
                    break;
                case Type.Edit:
                    pictureBox.Image = KenisBank.Properties.Resources.pencil;
                    panelColor.BackColor = Color.FromArgb(169, 204, 102);
                    break;
                case Type.Save:
                    pictureBox.Image = KenisBank.Properties.Resources.usb;
                    panelColor.BackColor = Color.FromArgb(80, 203, 203);
                    break;
                case Type.Note:
                    pictureBox.Image = KenisBank.Properties.Resources.notes;
                    panelColor.BackColor = Color.FromArgb(255, 189, 66);
                    break;
                default:
                    pictureBox.Image = KenisBank.Properties.Resources.info;
                    panelColor.BackColor = Color.FromArgb(0, 147, 241);
                    break;
            }
        }

        private void FormMelding_Load(object sender, EventArgs e)
        {
            meldingX = schermbreed - Width - 10;
            meldingY = schermhoog;
            Location = new Point(meldingX, meldingY);
        }

        private void FormMelding_MouseClick(object sender, MouseEventArgs e)
        {
            timer.Enabled = false;
            Close();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Application.DoEvents();
            if (status == statusForm.start)
                {
                    meldingY -= 4;
                    Location = new Point(meldingX, meldingY);
                    this.Refresh();
                    if (meldingY < schermhoog - Height - 10)
                    {
                        status = statusForm.show;
                        close = 110;
                    }
                }
                if (status == statusForm.show)
                {
                    close--;
                    if (close < 0)
                        status = statusForm.eind;
                }
                if (status == statusForm.eind)
                {
                    meldingY += 4;
                    Location = new Point(meldingX, meldingY);
                    this.Refresh();
                    if (meldingY > schermhoog)
                    {
                        timer.Enabled = false;
                        Close();
                        System.GC.Collect();
                    }
                }
        }

        private void FormMelding_Shown(object sender, EventArgs e)
        {
            status = statusForm.start;
            timer.Enabled = true;
        }
    }
}
