using System;
using System.Configuration;
using System.Windows.Forms;
using System.Drawing;

namespace VectorDrawForms
{
    public partial class ShapeEditorForm : Form
    {
        #region Constructors
        public ShapeEditorForm(
            float shapeWidth,
            float shapeHeight,
            float rotationAngle,
            float strokeThickness,
            Color strokeColor,
            Color fillColor
            )
            : this()
        {
            ShapeWidth = shapeWidth;
            ShapeHeight = shapeHeight;
            RotationAngle = rotationAngle;
            StrokeThickness = strokeThickness;
            StrokeColor = strokeColor;
            FillColor = fillColor;
        }

        public ShapeEditorForm(
            float strokeThickness,
            Color strokeColor,
            Color fillColor
            )
            : this()
        {
            widthTextBox.Text = "N/A";
            widthTextBox.Enabled = false;
            heightTextBox.Text = "N/A";
            heightTextBox.Enabled = false;
            angleTextBox.Text = "N/A";
            angleTextBox.Enabled = false;
        }

        public ShapeEditorForm()
        {
            InitializeComponent();
            ChangeUIMode("UIMode");
            CenterToScreen();

            //Set defaukt stroke color
            StrokeColor = Color.Gray;
        }
        #endregion

        #region Properties
        public float ShapeWidth
        {
            get { return Convert.ToSingle(widthTextBox.Text); }
            set { widthTextBox.Text = value.ToString(); }
        }

        public float ShapeHeight
        {
            get { return Convert.ToSingle(heightTextBox.Text); }
            set { heightTextBox.Text = value.ToString(); }
        }

        public float StrokeThickness
        {
            get { return Convert.ToSingle(strokeThicknessTextBox.Text); }
            set { strokeThicknessTextBox.Text = value.ToString(); }
        }

        public float RotationAngle
        {
            get { return Convert.ToSingle(angleTextBox.Text); }
            set { angleTextBox.Text = value.ToString(); }
        }

        private Color strokeColor;
        public Color StrokeColor
        {
            get { return strokeColor; }
            set
            {
                strokeColor = value;
                strokeColorLabel.Text = value.ToString();
                strokeColorButton.BackColor = value;
            }
        }
        private Color fillColor;
        public Color FillColor
        {
            get { return fillColor; }
            set
            {
                fillColor = value;
                fillColorLabel.Text = value.ToString();
                fillColorButton.BackColor = value;
            }
        }
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
                    buttonCancel.BackColor = SystemColors.Control;
                }
                else
                {
                    this.ForeColor = ApplicationColors.MainUILight;
                    this.BackColor = ApplicationColors.MainUIDark;

                    sizeGroupBox.ForeColor = ApplicationColors.MainUILight;
                    rotateGroupBox.ForeColor = ApplicationColors.MainUILight;
                    colorGroupBox.ForeColor = ApplicationColors.MainUILight;

                    buttonProceed.BackColor = ApplicationColors.SecondaryUIDark;
                    buttonCancel.BackColor = ApplicationColors.SecondaryUIDark;

                    widthTextBox.ForeColor = Color.White;
                    widthTextBox.BackColor = ApplicationColors.SecondaryUIDark;

                    heightTextBox.ForeColor = Color.White;
                    heightTextBox.BackColor = ApplicationColors.SecondaryUIDark;

                    strokeThicknessTextBox.ForeColor = Color.White;
                    strokeThicknessTextBox.BackColor = ApplicationColors.SecondaryUIDark;

                    angleTextBox.ForeColor = Color.White;
                    angleTextBox.BackColor = ApplicationColors.SecondaryUIDark;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            string errorMessage = null;

            if (widthTextBox.Enabled && !float.TryParse(widthTextBox.Text, out _))
                errorMessage = "You have to provide a proper floating point value for width to proceed.";

            else if (heightTextBox.Enabled && !float.TryParse(heightTextBox.Text, out _))
                errorMessage = "You have to provide a proper floating point value for height to proceed.";

            else if (strokeThicknessTextBox.Enabled && !float.TryParse(strokeThicknessTextBox.Text, out _))
                errorMessage = "You have to provide a proper floating point value for stroke thickness";

            else if(angleTextBox.Enabled && !float.TryParse(angleTextBox.Text, out _))
                errorMessage = "You have to provide a proper floating point value for angle.";

            if (errorMessage != null)
            {
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Close();
            DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
            DialogResult = DialogResult.Cancel;
        }

        private void borderColorButton_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                StrokeColor = colorDialog.Color;
            }
        }

        private void fillColorButton_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                FillColor = colorDialog.Color;
            }
        }
        #endregion
    }
}
