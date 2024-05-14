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

        public ShapeEditorForm()
        {
            InitializeComponent();
            ChangeUIMode("UIMode");
            CenterToScreen();
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
            get { return Convert.ToSingle(borderThicknessTextBox.Text); }
            set { borderThicknessTextBox.Text = value.ToString(); }
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

                    buttonProceed.BackColor = ApplicationColors.SecondaryUIDark;
                    buttonCancel.BackColor = ApplicationColors.SecondaryUIDark;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
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
                //Assign Color here
                StrokeColor = colorDialog.Color;
            }
        }

        private void fillColorButton_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                //Assign Color here
                FillColor = colorDialog.Color;
            }
        }
        #endregion
    }
}
