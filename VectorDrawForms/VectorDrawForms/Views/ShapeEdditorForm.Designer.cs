namespace VectorDrawForms
{
    partial class ShapeEditorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.buttonProceed = new System.Windows.Forms.Button();
            this.errorLabel = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.sizeGroupBox = new System.Windows.Forms.GroupBox();
            this.strokeThicknessTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.heightTextBox = new System.Windows.Forms.TextBox();
            this.widthTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.colorGroupBox = new System.Windows.Forms.GroupBox();
            this.fillColorLabel = new System.Windows.Forms.Label();
            this.strokeColorLabel = new System.Windows.Forms.Label();
            this.fillColorButton = new System.Windows.Forms.Button();
            this.strokeColorButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.rotateGroupBox = new System.Windows.Forms.GroupBox();
            this.angleTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.sizeGroupBox.SuspendLayout();
            this.colorGroupBox.SuspendLayout();
            this.rotateGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(174, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Shape Editor";
            // 
            // buttonProceed
            // 
            this.buttonProceed.Location = new System.Drawing.Point(123, 321);
            this.buttonProceed.Name = "buttonProceed";
            this.buttonProceed.Size = new System.Drawing.Size(116, 27);
            this.buttonProceed.TabIndex = 2;
            this.buttonProceed.Text = "Save";
            this.buttonProceed.UseVisualStyleBackColor = true;
            this.buttonProceed.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // errorLabel
            // 
            this.errorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errorLabel.ForeColor = System.Drawing.Color.Red;
            this.errorLabel.Location = new System.Drawing.Point(12, 163);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(388, 23);
            this.errorLabel.TabIndex = 3;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(258, 321);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(116, 27);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(78, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 14);
            this.label2.TabIndex = 5;
            this.label2.Text = "Width:";
            // 
            // sizeGroupBox
            // 
            this.sizeGroupBox.Controls.Add(this.strokeThicknessTextBox);
            this.sizeGroupBox.Controls.Add(this.label6);
            this.sizeGroupBox.Controls.Add(this.heightTextBox);
            this.sizeGroupBox.Controls.Add(this.widthTextBox);
            this.sizeGroupBox.Controls.Add(this.label3);
            this.sizeGroupBox.Controls.Add(this.label2);
            this.sizeGroupBox.Location = new System.Drawing.Point(25, 57);
            this.sizeGroupBox.Name = "sizeGroupBox";
            this.sizeGroupBox.Size = new System.Drawing.Size(230, 132);
            this.sizeGroupBox.TabIndex = 6;
            this.sizeGroupBox.TabStop = false;
            this.sizeGroupBox.Text = "Size";
            // 
            // strokeThicknessTextBox
            // 
            this.strokeThicknessTextBox.Location = new System.Drawing.Point(146, 97);
            this.strokeThicknessTextBox.Name = "strokeThicknessTextBox";
            this.strokeThicknessTextBox.Size = new System.Drawing.Size(66, 20);
            this.strokeThicknessTextBox.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(22, 99);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 14);
            this.label6.TabIndex = 9;
            this.label6.Text = "Stroke Thickness";
            // 
            // heightTextBox
            // 
            this.heightTextBox.Location = new System.Drawing.Point(146, 59);
            this.heightTextBox.Name = "heightTextBox";
            this.heightTextBox.Size = new System.Drawing.Size(66, 20);
            this.heightTextBox.TabIndex = 8;
            // 
            // widthTextBox
            // 
            this.widthTextBox.Location = new System.Drawing.Point(146, 24);
            this.widthTextBox.Name = "widthTextBox";
            this.widthTextBox.Size = new System.Drawing.Size(66, 20);
            this.widthTextBox.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(75, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 14);
            this.label3.TabIndex = 6;
            this.label3.Text = "Height:";
            // 
            // colorGroupBox
            // 
            this.colorGroupBox.Controls.Add(this.fillColorLabel);
            this.colorGroupBox.Controls.Add(this.strokeColorLabel);
            this.colorGroupBox.Controls.Add(this.fillColorButton);
            this.colorGroupBox.Controls.Add(this.strokeColorButton);
            this.colorGroupBox.Controls.Add(this.label5);
            this.colorGroupBox.Controls.Add(this.label4);
            this.colorGroupBox.Location = new System.Drawing.Point(25, 204);
            this.colorGroupBox.Name = "colorGroupBox";
            this.colorGroupBox.Size = new System.Drawing.Size(230, 97);
            this.colorGroupBox.TabIndex = 7;
            this.colorGroupBox.TabStop = false;
            this.colorGroupBox.Text = "Color";
            // 
            // fillColorLabel
            // 
            this.fillColorLabel.Location = new System.Drawing.Point(103, 62);
            this.fillColorLabel.Name = "fillColorLabel";
            this.fillColorLabel.Size = new System.Drawing.Size(75, 13);
            this.fillColorLabel.TabIndex = 14;
            this.fillColorLabel.Text = "Color";
            // 
            // strokeColorLabel
            // 
            this.strokeColorLabel.Location = new System.Drawing.Point(103, 28);
            this.strokeColorLabel.Name = "strokeColorLabel";
            this.strokeColorLabel.Size = new System.Drawing.Size(75, 13);
            this.strokeColorLabel.TabIndex = 13;
            this.strokeColorLabel.Text = "Color";
            // 
            // fillColorButton
            // 
            this.fillColorButton.Location = new System.Drawing.Point(189, 56);
            this.fillColorButton.Name = "fillColorButton";
            this.fillColorButton.Size = new System.Drawing.Size(25, 25);
            this.fillColorButton.TabIndex = 12;
            this.fillColorButton.UseVisualStyleBackColor = true;
            this.fillColorButton.Click += new System.EventHandler(this.fillColorButton_Click);
            // 
            // strokeColorButton
            // 
            this.strokeColorButton.Location = new System.Drawing.Point(189, 20);
            this.strokeColorButton.Name = "strokeColorButton";
            this.strokeColorButton.Size = new System.Drawing.Size(25, 25);
            this.strokeColorButton.TabIndex = 11;
            this.strokeColorButton.UseVisualStyleBackColor = true;
            this.strokeColorButton.Click += new System.EventHandler(this.borderColorButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(42, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 14);
            this.label5.TabIndex = 10;
            this.label5.Text = "Fill Color:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(18, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 14);
            this.label4.TabIndex = 9;
            this.label4.Text = "Stroke Color:";
            // 
            // rotateGroupBox
            // 
            this.rotateGroupBox.Controls.Add(this.angleTextBox);
            this.rotateGroupBox.Controls.Add(this.label8);
            this.rotateGroupBox.Location = new System.Drawing.Point(278, 57);
            this.rotateGroupBox.Name = "rotateGroupBox";
            this.rotateGroupBox.Size = new System.Drawing.Size(181, 129);
            this.rotateGroupBox.TabIndex = 15;
            this.rotateGroupBox.TabStop = false;
            this.rotateGroupBox.Text = "Rotate";
            // 
            // angleTextBox
            // 
            this.angleTextBox.Location = new System.Drawing.Point(94, 24);
            this.angleTextBox.Name = "angleTextBox";
            this.angleTextBox.Size = new System.Drawing.Size(66, 20);
            this.angleTextBox.TabIndex = 13;
            this.angleTextBox.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(29, 26);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 14);
            this.label8.TabIndex = 12;
            this.label8.Text = "Angle:";
            // 
            // ShapeEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 367);
            this.Controls.Add(this.rotateGroupBox);
            this.Controls.Add(this.colorGroupBox);
            this.Controls.Add(this.sizeGroupBox);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.buttonProceed);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ShapeEditorForm";
            this.Text = "VectorDraw - Shape Editor";
            this.sizeGroupBox.ResumeLayout(false);
            this.sizeGroupBox.PerformLayout();
            this.colorGroupBox.ResumeLayout(false);
            this.colorGroupBox.PerformLayout();
            this.rotateGroupBox.ResumeLayout(false);
            this.rotateGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonProceed;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox sizeGroupBox;
        private System.Windows.Forms.TextBox heightTextBox;
        private System.Windows.Forms.TextBox widthTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox colorGroupBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button fillColorButton;
        private System.Windows.Forms.Button strokeColorButton;
        private System.Windows.Forms.TextBox strokeThicknessTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label fillColorLabel;
        private System.Windows.Forms.Label strokeColorLabel;
        private System.Windows.Forms.GroupBox rotateGroupBox;
        private System.Windows.Forms.TextBox angleTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ColorDialog colorDialog;
    }
}