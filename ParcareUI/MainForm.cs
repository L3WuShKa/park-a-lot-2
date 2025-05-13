using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Administrator;
using ParcareMare;
using Masina;       
using LocParcare;

namespace ParcareUI
{
    public partial class MainForm : Form
    {
        private const int VerticalSpacing = 40;
        private const int HorizontalSpacing = 20;
        private const int ControlWidth = 200;
        private const int ButtonWidth = 120;
        private const int ButtonHeight = 40;

        private readonly toata_parcarea _parcare = new toata_parcarea(10);
        private readonly IAdminAccess _admin = new Admin();
        private readonly ErrorProvider _errorProvider = new ErrorProvider();

        public MainForm()
        {
            InitializeComponent();  
            SetupUI();
        }

       
        private void SetupUI()
        {
            this.Text = "Sistem Management Parcare";
            this.Size = new Size(900, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            var tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Appearance = TabAppearance.Normal,
                Padding = new Point(15, 15),
                ItemSize = new Size(120, 30),
                SizeMode = TabSizeMode.Fixed
            };

            // Tab „Înregistrare Vehicul”
            var tabUser = new TabPage("Înregistrare Vehicul");
            SetupUserTab(tabUser);
            tabControl.TabPages.Add(tabUser);

            // Tab „Administrare”
            var tabAdmin = new TabPage("Administrare");
            SetupAdminTab(tabAdmin);
            tabControl.TabPages.Add(tabAdmin);

            // Tab „Editare Vehicul”
            var tabEdit = new TabPage("Editare Vehicul");
            SetupEditTab(tabEdit);
            tabControl.TabPages.Add(tabEdit);

            this.Controls.Add(tabControl);
        }

        private void SetupUserTab(TabPage tab)
        {
            tab.BackColor = Color.FromArgb(240, 240, 240);
            tab.Padding = new Padding(20);

            var lblTitlu = new Label
            {
                Text = "Înregistrare Vehicul Nou",
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Height = 50,
                Margin = new Padding(0, 0, 0, 20)
            };

            var panelContainer = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(20)
            };

            int currentY = 80;

            var lblNr = CreateLabel("Număr înmatriculare:", HorizontalSpacing, currentY);
            var txtNr = CreateTextBox(HorizontalSpacing + 180, currentY);
            currentY += VerticalSpacing;

            var lblProp = CreateLabel("Proprietar:", HorizontalSpacing, currentY);
            var txtProp = CreateTextBox(HorizontalSpacing + 180, currentY);
            currentY += VerticalSpacing;

            var lblMarca = CreateLabel("Marca:", HorizontalSpacing, currentY);
            var txtMarca = CreateTextBox(HorizontalSpacing + 180, currentY);
            currentY += VerticalSpacing;

            var lblCuloare = CreateLabel("Culoare:", HorizontalSpacing, currentY);
            var txtCuloare = CreateTextBox(HorizontalSpacing + 180, currentY);
            currentY += VerticalSpacing;

            var grpTip = new GroupBox
            {
                Text = "Tip Parcare",
                Location = new Point(HorizontalSpacing, currentY),
                Size = new Size(360, 60),
                Font = new Font("Arial", 9)
            };
            var radNormal = new RadioButton { Text = "Standard", Location = new Point(20, 20), Checked = true };
            var radVIP = new RadioButton { Text = "VIP", Location = new Point(180, 20) };
            grpTip.Controls.Add(radNormal);
            grpTip.Controls.Add(radVIP);
            currentY += 80;

            var btnPret = CreateButton("Verifică Preț", HorizontalSpacing, currentY, Color.LightBlue);
            var btnInregistrare = CreateButton("Înregistrează", HorizontalSpacing + 140, currentY, Color.LightGreen);
            var btnEliberare = CreateButton("Eliberează", HorizontalSpacing + 280, currentY, Color.LightCoral);

            btnInregistrare.Click += (s, e) =>
            {
                if (ValidateInputs(txtNr, txtProp, txtMarca, txtCuloare))
                {
                    var result = _parcare.InregistreazaMasinaInteractiv(
                        txtNr.Text, txtProp.Text, txtMarca.Text, txtCuloare.Text, radVIP.Checked);
                    MessageBox.Show(result, "Rezultat Înregistrare", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };
            btnPret.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtNr.Text))
                {
                    _errorProvider.SetError(txtNr, "Introduceți numărul de înmatriculare");
                    return;
                }
                var pret = _parcare.AfiseazaPretCurentInteractiv(txtNr.Text);
                MessageBox.Show(pret, "Preț Curent", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            btnEliberare.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtNr.Text))
                {
                    _errorProvider.SetError(txtNr, "Introduceți numărul de înmatriculare");
                    return;
                }
                using (var formPlata = new PaymentForm())
                {
                    if (formPlata.ShowDialog() == DialogResult.OK)
                    {
                        var result = _parcare.ElibereazaSiPlatesteInteractiv(
                            txtNr.Text, formPlata.PaymentMethod, formPlata.Currency);
                        MessageBox.Show(result, "Rezultat Plata", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            };

            panelContainer.Controls.AddRange(new Control[]
            {
                lblNr, txtNr, lblProp, txtProp,
                lblMarca, txtMarca, lblCuloare, txtCuloare,
                grpTip, btnPret, btnInregistrare, btnEliberare
            });

            tab.Controls.Add(lblTitlu);
            tab.Controls.Add(panelContainer);
        }

        private void SetupAdminTab(TabPage tab)
        {
            tab.BackColor = Color.FromArgb(240, 240, 240);
            tab.Padding = new Padding(20);

            var panelContainer = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            var panelPassword = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(40)
            };

            var panelAdminFunctions = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(20),
                Visible = false
            };

            var lblPasswordPrompt = new Label { Text = "Pentru acces la funcțiile administrative,", Location = new Point(20, 40), AutoSize = true, Font = new Font("Arial", 10) };
            var lblPasswordPrompt2 = new Label { Text = "vă rugăm introduceți parola:", Location = new Point(20, 70), AutoSize = true, Font = new Font("Arial", 10) };
            var txtPassword = new TextBox { Location = new Point(20, 100), Width = 250, PasswordChar = '*', Font = new Font("Arial", 10) };
            var btnLogin = CreateButton("Autentificare", 20, 140, Color.LightGreen);

            btnLogin.Click += (s, e) =>
            {
                if (_admin.Authenticate(txtPassword.Text))
                {
                    panelPassword.Visible = false;
                    panelAdminFunctions.Visible = true;
                }
                else
                {
                    MessageBox.Show("Parolă incorectă!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            panelPassword.Controls.AddRange(new Control[]
            {
                lblPasswordPrompt, lblPasswordPrompt2, txtPassword, btnLogin
            });

            var txtLog = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.Fill,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                Font = new Font("Consolas", 9),
                BackColor = Color.White,
                Margin = new Padding(10)
            };

            var panelButtons = new Panel
            {
                Dock = DockStyle.Top,
                Height = 180,
                BackColor = Color.FromArgb(240, 240, 240),
                Padding = new Padding(10)
            };

            var btnLog = CreateAdminButton("Log Securitate", 20, 20);
            var btnStergeLog = CreateAdminButton("Șterge Log", 220, 20);
            var btnMasini = CreateAdminButton("Mașini Parcate", 420, 20);
            var btnPlati = CreateAdminButton("Istoric Plăți", 20, 80);
            var btnCauta = CreateAdminButton("Caută Mașină", 220, 80);
            var btnIesire = CreateAdminButton("Ieșire Admin", 420, 80);

            btnLog.Click += (s, e) => txtLog.Text = GetAdminOutput(_admin.AfiseazaLogSecuritate);
            btnStergeLog.Click += (s, e) => { _admin.StergeLogSecuritate(); txtLog.Text = "Log-urile au fost șterse cu succes!"; };
            btnMasini.Click += (s, e) => txtLog.Text = GetAdminOutput(_admin.AfiseazaMasiniInParcare);
            btnPlati.Click += (s, e) => txtLog.Text = GetAdminOutput(_admin.AfiseazaIstoricPlati);
            btnCauta.Click += (s, e) =>
            {
                using (var searchForm = new SearchForm())
                {
                    if (searchForm.ShowDialog() == DialogResult.OK)
                        txtLog.Text = GetAdminOutput(() => _admin.CautaMasina(searchForm.SearchText));
                }
            };
            btnIesire.Click += (s, e) =>
            {
                panelAdminFunctions.Visible = false;
                panelPassword.Visible = true;
                txtPassword.Text = "";
                txtLog.Text = "";
            };

            panelButtons.Controls.AddRange(new Control[]
            {
                btnLog, btnStergeLog, btnMasini, btnPlati, btnCauta, btnIesire
            });

            panelAdminFunctions.Controls.Add(txtLog);
            panelAdminFunctions.Controls.Add(panelButtons);

            panelContainer.Controls.Add(panelAdminFunctions);
            panelContainer.Controls.Add(panelPassword);
            tab.Controls.Add(panelContainer);
        }

        private void SetupEditTab(TabPage tab)
        {
            tab.BackColor = Color.FromArgb(240, 240, 240);
            tab.Padding = new Padding(20);

            // ComboBox selecție
            var cmbSelect = new ComboBox
            {
                Location = new Point(20, 20),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbSelect.Items.AddRange(_parcare.GetNumereMasini());
            tab.Controls.Add(cmbSelect);

            // ListBox
            var lst = new ListBox
            {
                Location = new Point(240, 20),
                Size = new Size(300, 200)
            };
            lst.Items.AddRange(_parcare.GetMasini().Select(m => m.ToString()).ToArray());
            tab.Controls.Add(lst);

            // DataGridView
            var dgv = new DataGridView
            {
                Location = new Point(560, 20),
                Size = new Size(300, 200),
                ReadOnly = true,
                AutoGenerateColumns = true,
                DataSource = _parcare.GetMasini()
            };
            tab.Controls.Add(dgv);

            // Controale editare
            int y = 240;
            var lblNr = CreateLabel("Număr:", 20, y);
            var txtNr = CreateTextBox(120, y); txtNr.ReadOnly = true;
            y += VerticalSpacing;

            var lblProp = CreateLabel("Proprietar:", 20, y);
            var txtProp = CreateTextBox(120, y);
            y += VerticalSpacing;

            var lblMarca = CreateLabel("Marca:", 20, y);
            var txtMarca = CreateTextBox(120, y);
            y += VerticalSpacing;

            var lblCuloare = CreateLabel("Culoare:", 20, y);
            var txtCuloare = CreateTextBox(120, y);
            y += VerticalSpacing;

            var grpTip = new GroupBox { Text = "Tip Parc.", Location = new Point(20, y), Size = new Size(260, 60) };
            var radStandard = new RadioButton { Text = "Standard", Location = new Point(10, 20), Checked = true };
            var radVIP = new RadioButton { Text = "VIP", Location = new Point(130, 20) };
            grpTip.Controls.Add(radStandard);
            grpTip.Controls.Add(radVIP);
            tab.Controls.AddRange(new Control[] { lblNr, txtNr, lblProp, txtProp,
                                                  lblMarca, txtMarca, lblCuloare, txtCuloare, grpTip });
            y += 80;

            var lblIntrare = CreateLabel("Data Intrare:", 20, y);
            var dtpIntrare = new DateTimePicker
            {
                Location = new Point(120, y),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd-MM-yyyy HH:mm",
                Enabled = false
            };
            tab.Controls.AddRange(new Control[] { lblIntrare, dtpIntrare });
            y += VerticalSpacing;

            var lblActual = CreateLabel("Actualizare:", 20, y);
            var dtpActual = new DateTimePicker
            {
                Location = new Point(120, y),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd-MM-yyyy HH:mm",
                Enabled = false
            };
            tab.Controls.AddRange(new Control[] { lblActual, dtpActual });
            y += VerticalSpacing;

            var btnLoad = new Button { Text = "Încarcă", Size = new Size(100, ButtonHeight), Location = new Point(20, y) };
            var btnUpdate = new Button { Text = "Actualizează", Size = new Size(120, ButtonHeight), Location = new Point(140, y) };
            tab.Controls.AddRange(new Control[] { btnLoad, btnUpdate });

            btnLoad.Click += (_, __) => //practic se incarca masina selectata (din combo) in textbox-uri 
            {
                if (cmbSelect.SelectedItem == null) return;
                var nr = cmbSelect.SelectedItem.ToString();
                var m = _parcare.GetMasina(nr);
                var loc = _parcare.GetLoc(nr);
                if (m == null || loc == null) return;

                txtNr.Text = m.NumarInmatriculare;
                txtProp.Text = m.Proprietar;
                txtMarca.Text = m.Marca;
                txtCuloare.Text = m.Culoare;
                radVIP.Checked = loc.VIP;
                radStandard.Checked = !loc.VIP;
                dtpIntrare.Value = loc.DataOcupare;
                dtpActual.Value = m.DataActualizare;
            };

            btnUpdate.Click += (_, __) =>
            {
                if (string.IsNullOrEmpty(txtNr.Text)) return;
                var masinaNoua = new DetaliiMasina(
                    txtNr.Text,
                    txtProp.Text,
                    txtMarca.Text,
                    txtCuloare.Text
                );
                masinaNoua.DataIntrare = dtpIntrare.Value;
                masinaNoua.DataActualizare = DateTime.Now;

                bool ok = _parcare.UpdateMasina(masinaNoua, radVIP.Checked);
                if (ok)
                {
                    MessageBox.Show("Actualizare cu succes!", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    cmbSelect.Items.Clear();
                    cmbSelect.Items.AddRange(_parcare.GetNumereMasini());
                    lst.Items.Clear();
                    lst.Items.AddRange(_parcare.GetMasini().Select(x => x.ToString()).ToArray());
                    dgv.DataSource = null;
                    dgv.DataSource = _parcare.GetMasini();
                    dtpActual.Value = masinaNoua.DataActualizare;
                }
                else
                {
                    MessageBox.Show("Mașina nu a fost găsită.", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
        }

        #region Helper Methods
        private Label CreateLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Arial", 10)
            };
        }

        private TextBox CreateTextBox(int x, int y)
        {
            return new TextBox
            {
                Location = new Point(x, y),
                Width = ControlWidth,
                Font = new Font("Arial", 10)
            };
        }

        private Button CreateButton(string text, int x, int y, Color backColor)
        {
            return new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(ButtonWidth, ButtonHeight),
                BackColor = backColor,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 9, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
        }

        private Button CreateAdminButton(string text, int x, int y)
        {
            return new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(180, 50),
                BackColor = GetButtonColor(text),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 9, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
        }

        private Color GetButtonColor(string buttonText)
        {
            switch (buttonText)
            {
                case "Log Securitate": return Color.LightBlue;
                case "Șterge Log": return Color.LightCoral;
                case "Mașini Parcate": return Color.LightGreen;
                case "Istoric Plăți": return Color.LightGoldenrodYellow;
                case "Caută Mașină": return Color.LightSalmon;
                case "Ieșire Admin": return Color.LightGray;
                default: return Color.LightGray;
            }
        }

        private bool ValidateInputs(params TextBox[] textBoxes)
        {
            bool isValid = true;
            foreach (var txt in textBoxes)
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    _errorProvider.SetError(txt, "Câmp obligatoriu");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txt, "");
                }
            }
            return isValid;
        }

        private string GetAdminOutput(Action action)
        {
            using (var writer = new StringWriter())
            {
                var original = Console.Out;
                Console.SetOut(writer);
                action.Invoke();
                Console.SetOut(original);
                return writer.ToString();
            }
        }
        #endregion
    }
}
