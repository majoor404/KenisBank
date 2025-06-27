using System;
using System.Drawing;
using System.Windows.Forms;

namespace KenisBank
{
    public class KennisPanel : Panel
    {
        // een kennis panel bevat een
        // 1) HoofdStuk, dus label
        // 2) Text
        // 3) Link , LinkLabel
        // 4) Pagina, Button

        public type kType { get; set; }
        public string kText { get; set; }
        public string kUrl { get; set; }
        private Panel kPanel { get; set; }
        public int kId { get; set; }

        private KennisPanel() { }
        public KennisPanel(Panel MainPanel, type Type, string Text, string Url)
        {
            kPanel = MainPanel;
            kType = Type;
            kText = Text;
            kUrl = Url;

            kId = KennisMainForm.MaakID();
            Tag = kId;

            switch (kType)
            {
                case type.Hoofdstuk:
                    MaakHoofdStuk(kId);
                    break;
                case type.TekstBlok:
                    MaakTekstBlok(kId);
                    break;
                case type.LinkDir:
                    MaakLinkDir(kId);
                    break;
                case type.LinkFile:
                    MaakLinkDir(kId);
                    break;
                case type.PaginaNaam:
                    MaakPaginaKnop(kId);
                    break;
                case type.Leeg:
                    kText = string.Empty;
                    MaakTekstBlok(kId);
                    break;
            }
        }

        private void MaakNewPanel(int kId)
        {
            Dock = DockStyle.Top;
            BorderStyle = BorderStyle.None;
            AutoSize = true;

            Click += new EventHandler(kKlik);


            kPanel.Controls.Add(this);
            kPanel.Controls.SetChildIndex(this, 0);
            Tag = kId;
            MouseHover += MouseHoverPanel;

            this.MouseDown += KennisPanel_MouseDown;
            this.AllowDrop = true;
            this.DragEnter += KennisPanel_DragEnter;
            this.DragDrop += KennisPanel_DragDrop;
        }

        private void MouseHoverPanel(object sender, EventArgs e)
        {
            System.Windows.Forms.Panel lb = (System.Windows.Forms.Panel)sender;
            int tag = (int)lb.Tag;
            KennisMainForm.mainForm.DummyBut.Text = tag.ToString();
            
        }

        private void kKlik(object sender, EventArgs e)
        {
            KennisMainForm.KleurGeselecteerdePanel(kId);
        }

        private void kLinkKlik(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(e.Button == MouseButtons.Right) {return;}
            KennisMainForm.Label_LinkKlik(kId);
        }

        private void kPaginaKlik(object sender, EventArgs e)
        {
            KennisMainForm.PaginaKlik(kId);
        }

        private void MaakHoofdStuk(int kId)
        {
            MaakNewPanel(kId);

            System.Windows.Forms.Label label = new System.Windows.Forms.Label();
            Point org = new Point(label.Location.X, label.Location.Y);
            org.X += 30;
            label.Location = org;
            label.AutoSize = true;
            label.Font = new Font("Microsoft Sans Serif", 18, FontStyle.Bold);
            label.Text = kText;
            label.Tag = kId;
            label.MouseHover += MouseHoverLabelofText;

            Controls.Add(label);
            //Refresh();
        }

        private void MouseHoverLabelofText(object sender, EventArgs e)
        {
            System.Windows.Forms.Label lb = (System.Windows.Forms.Label)sender;
            int tag = (int)lb.Tag;
            KennisMainForm.mainForm.DummyBut.Text = tag.ToString();
        }

        private void MaakTekstBlok(int kId)
        {
            MaakNewPanel(kId);

            //SuspendLayout();

            // split string at new line
            string[] result = kText.Split('\n');
            int regeloffset = 0;
            foreach (string str in result)
            {
                System.Windows.Forms.Label label = new System.Windows.Forms.Label();
                Point org = new Point(label.Location.X, label.Location.Y);
                org.X += 30;
                org.Y += regeloffset * 20;
                regeloffset++;
                label.Location = org;
                label.AutoSize = true;
                label.MaximumSize = new Size(kPanel.Width - 60, 0);
                label.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
                label.Text = str;
                label.Tag = kId;
                label.MouseHover += MouseHoverLabelofText;
                Controls.Add(label);
            }
            //ResumeLayout();
        }

        private void MaakLinkDir(int kId)
        {
            MaakNewPanel(kId);

            System.Windows.Forms.LinkLabel label = new System.Windows.Forms.LinkLabel();

            Point org = new Point(label.Location.X, label.Location.Y);
            org.X += 30;
            label.Location = org;
            label.AutoSize = true;
            //label.Width = kPanel.Width - 100;
            label.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
            label.Text = kText;
            label.BorderStyle = BorderStyle.None;
            label.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(kLinkKlik);
            label.Tag = kId;
            label.MouseHover += MouseHoverLabelofText;
            Controls.Add(label);
            //Refresh();
        }

        private void MaakPaginaKnop(int kId)
        {
            MaakNewPanel(kId);

            System.Windows.Forms.Button but = new System.Windows.Forms.Button();

            Point org = new Point(but.Location.X, but.Location.Y);

            if (kPanel.Width > 235)
            {
                org.X += 30;
                but.Width = 500;
                but.Height = 30;
            }
            else
            {
                org.X += 10;
                but.Width = 200;
                but.Height = 30;
            }
            but.Location = org;
            but.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
            but.Text = kText;
            but.Click += new EventHandler(kPaginaKlik);
            but.Tag = kId;
            but.MouseHover += MouseHoverButton;
            Controls.Add(but);
            //Refresh();
        }

        private void MouseHoverButton(object sender, EventArgs e)
        {
            Button lb = (Button)sender;
            int tag = (int)lb.Tag;
            KennisMainForm.mainForm.DummyBut.Text = tag.ToString();
        }

        private void KennisPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (KennisMainForm.mainForm.editModeAanToolStripMenuItem.Checked && e.Button == MouseButtons.Left)
            {
                DoDragDrop(this, DragDropEffects.Move);
            }
        }

        private void KennisPanel_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(KennisPanel)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private void KennisPanel_DragDrop(object sender, DragEventArgs e)
        {
            if (!KennisMainForm.mainForm.editModeAanToolStripMenuItem.Checked)
                return;

            var draggedPanel = e.Data.GetData(typeof(KennisPanel)) as KennisPanel;
            var targetPanel = this;

            if (draggedPanel == null || targetPanel == null || draggedPanel == targetPanel)
                return;

            // Haal indexen op
            var form = KennisMainForm.mainForm;
            int fromIndex = form.GetIndexVanId(draggedPanel.kId);
            int toIndex = form.GetIndexVanId(targetPanel.kId);

            if (fromIndex >= 0 && toIndex >= 0 && fromIndex != toIndex)
            {
                form.MovePanel(fromIndex, toIndex);
            }
        }
    }
}
