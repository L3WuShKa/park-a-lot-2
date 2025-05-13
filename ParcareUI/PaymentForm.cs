using System;
using System.Drawing;
using System.Windows.Forms;

namespace ParcareUI
{
    public partial class PaymentForm : Form
    {
        public string PaymentMethod { get; private set; }
        public string Currency { get; private set; }

        public PaymentForm()
        {
            InitializeComponent();
            SetupUI();
        }

        

        private void SetupUI()
        {
            Text = "Finalizare Plata";
            Size = new Size(350, 200);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;

            var lblMethod = new Label { Text = "Metoda plata:", Location = new Point(20, 20), AutoSize = true };
            var cmbMethod = new ComboBox { Location = new Point(120, 20), Width = 200 };
            cmbMethod.Items.AddRange(new[] { "Cash", "Card", "Mobil" });
            cmbMethod.SelectedIndex = 0;

            var lblCurrency = new Label { Text = "Valuta:", Location = new Point(20, 60), AutoSize = true };
            var cmbCurrency = new ComboBox { Location = new Point(120, 60), Width = 200 };
            cmbCurrency.Items.AddRange(new[] { "RON", "EUR", "USD" });
            cmbCurrency.SelectedIndex = 0;

            var btnOK = new Button
            {
                Text = "Confirma",
                Location = new Point(80, 120),
                Size = new Size(100, 40),
                DialogResult = DialogResult.OK,
                BackColor = Color.LightGreen
            };

            var btnCancel = new Button
            {
                Text = "Anuleaza",
                Location = new Point(190, 120),
                Size = new Size(100, 40),
                DialogResult = DialogResult.Cancel,
                BackColor = Color.LightCoral
            };

            btnOK.Click += (s, e) =>
            {
                PaymentMethod = cmbMethod.SelectedItem.ToString();
                Currency = cmbCurrency.SelectedItem.ToString();
            };

            Controls.AddRange(new Control[] { lblMethod, cmbMethod, lblCurrency, cmbCurrency, btnOK, btnCancel });
        }
    }
}
