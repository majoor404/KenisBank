using System;
using System.Windows.Forms;

namespace KenisBank
{
    // Progress feedback verbeteren
    public class ProgressManager : IDisposable
    {
        public dynamic MainForm { get; private set; }
        private int value;

        public ProgressManager(Form Main, string message, int maximum)
        {
            MainForm = Main;
            MainForm.progressBar.Maximum = maximum;
            MainForm.progressBar.Value = 0;
            MainForm.progressBar.Visible = true;
            MainForm.WaarBenMeeBezigLabel.Text = message;
            MainForm.WaarBenMeeBezigLabel.Visible = true;
            value = 0;
            MainForm.progressBar.Location.X = MainForm.WaarBenMeeBezigLabel.Location.X + MainForm.WaarBenMeeBezigLabel.Width + 20;
            MainForm.progressBar.Width = MainForm.Width - MainForm.progressBar.Location.X - 40;
            MainForm.PanelDetail.Visible = false;
        }

        public void Update()
        {
            value++;
            MainForm.progressBar.Value = Math.Min(value, MainForm.progressBar.Maximum);
            MainForm.progressBar.PerformStep();
            MainForm.progressBar.Refresh();
            Application.DoEvents();
        }

        public void Dispose()
        {
            MainForm.progressBar.Visible = false;
            MainForm.WaarBenMeeBezigLabel.Visible = false;
        }
    }
}