using System;
using System.Drawing;
using System.Windows.Forms;

namespace ParcareUI
{
    public partial class SearchForm : Form
    {
        public string SearchText { get; private set; }

        public SearchForm()
        {
            InitializeComponent();
            InitializeForm();
        }

       

        private void InitializeForm()
        {
            this.Text = "Cautare Masina";
            this.ClientSize = new Size(300, 150);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 240, 240);

            var lblSearch = new Label
            {
                Text = "Numar inmatriculare:",
                Location = new Point(20, 20),
                AutoSize = true
            };

            var txtSearch = new TextBox
            {
                Location = new Point(20, 50),
                Width = 250
            };

            var btnOK = new Button
            {
                Text = "Cauta",
                DialogResult = DialogResult.OK,
                Location = new Point(80, 90),
                Size = new Size(80, 30),
                BackColor = Color.LightBlue
            };

            var btnCancel = new Button
            {
                Text = "Anuleaza",
                DialogResult = DialogResult.Cancel,
                Location = new Point(170, 90),
                Size = new Size(80, 30),
                BackColor = Color.LightCoral
            };

            btnOK.Click += (s, e) => SearchText = txtSearch.Text;

            this.Controls.Add(lblSearch);
            this.Controls.Add(txtSearch);
            this.Controls.Add(btnOK);
            this.Controls.Add(btnCancel);
        }
    }
}
