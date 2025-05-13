using System;
using System.Drawing;
using System.Windows.Forms;

namespace ParcareUI
{
    public partial class PasswordForm : Form
    {
        public string Password { get; private set; }

        public PasswordForm()
        {
            InitializeComponent();
            InitializeForm();
        }



        private void InitializeForm()
        {
            this.Text = "Autentificare Administrator";
            this.ClientSize = new Size(350, 200);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(240, 240, 240);

            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            var lblPassword = new Label
            {
                Text = "Introduceti parola admin:",
                Location = new Point(50, 30),
                AutoSize = true,
                Font = new Font("Arial", 10)
            };

            var txtPassword = new TextBox
            {
                Location = new Point(50, 60),
                Width = 250,
                PasswordChar = '*',
                Font = new Font("Arial", 10)
            };

            var panelButtons = new Panel
            {
                Location = new Point(50, 110),
                Size = new Size(250, 40)
            };

            var btnOK = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Size = new Size(100, 30),
                BackColor = Color.LightGreen,
                FlatStyle = FlatStyle.Flat
            };

            var btnCancel = new Button
            {
                Text = "Anuleaza",
                DialogResult = DialogResult.Cancel,
                Location = new Point(130, 0),
                Size = new Size(100, 30),
                BackColor = Color.LightCoral,
                FlatStyle = FlatStyle.Flat
            };

            btnOK.Click += (s, e) =>
            {
                this.Password = txtPassword.Text;
            };

            panelButtons.Controls.Add(btnOK);
            panelButtons.Controls.Add(btnCancel);

            panel.Controls.Add(lblPassword);
            panel.Controls.Add(txtPassword);
            panel.Controls.Add(panelButtons);

            this.Controls.Add(panel);
        }
    }
}
