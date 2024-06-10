using System;
using System.Configuration;
using System.Reflection;
using System.Windows.Forms;

namespace VectorDrawForms.Views
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            PrepareInitialState();
            ChangeUIMode("UIMode");
            CenterToScreen();
        }

        private void PrepareInitialState()
        {
            pictureBox.Image = Properties.Resources.AppImage;
            versionLabel.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private void ChangeUIMode(string key)
        {
            try
            {
                var uiMode = ConfigurationManager.AppSettings[key];
                if (uiMode == UIMode.Light.ToString())
                {
                    this.ForeColor = ApplicationColors.MainUIDark;
                    this.BackColor = ApplicationColors.MainUILight;
                }
                else
                {
                    this.ForeColor = ApplicationColors.MainUILight;
                    this.BackColor = ApplicationColors.MainUIDark;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
