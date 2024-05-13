using System;
using System.Configuration;
using System.Windows.Forms;
using System.Drawing;

namespace VectorDrawForms
{
    public partial class PaintToolConfirmationForm : Form
    {
        #region Constructor
        public PaintToolConfirmationForm()
        {
            InitializeComponent();
            ChangeUIMode("UIMode");
        }
        #endregion

        #region Properties
        public string SelectedItem { get { return comboBox.Text; } }
        #endregion

        #region Methods
        private void ChangeUIMode(string key)
        {
            try
            {
                var uiMode = ConfigurationManager.AppSettings[key];
                if (uiMode == UIMode.Light.ToString())
                {
                    this.ForeColor = ApplicationColors.MainUIDark;
                    this.BackColor = ApplicationColors.MainUILight;

                    buttonProceed.BackColor = SystemColors.Control;
                }
                else
                {
                    this.ForeColor = ApplicationColors.MainUILight;
                    this.BackColor = ApplicationColors.MainUIDark;

                    buttonProceed.BackColor = ApplicationColors.SecondaryUIDark;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            errorLabel.Text = string.Empty;
        }

        private void buttonProceed_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox.Text))
            {
                errorLabel.Text = "You have to provide a property to proceed!";
                return;
            }

            Close();
            DialogResult = DialogResult.OK;
        }
    }
}
