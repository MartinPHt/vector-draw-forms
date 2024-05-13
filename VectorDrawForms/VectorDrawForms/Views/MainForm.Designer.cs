﻿namespace VectorDrawForms
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newFileMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableDisableDarkModeSettingsButton = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenu = new System.Windows.Forms.ToolStrip();
            this.selectionTool = new System.Windows.Forms.ToolStripButton();
            this.drawRectangleButton = new System.Windows.Forms.ToolStripButton();
            this.elipseToolButton = new System.Windows.Forms.ToolStripButton();
            this.paintToolButton = new System.Windows.Forms.ToolStripButton();
            this.groupToolButton = new System.Windows.Forms.ToolStripButton();
            this.coordinatesLabel = new System.Windows.Forms.Label();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.canvas = new VectorDrawForms.Views.DoubleBufferedPanel();
            this.mainMenu.SuspendLayout();
            this.toolMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.imageToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(1004, 24);
            this.mainMenu.TabIndex = 0;
            this.mainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newFileMenuButton,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newFileMenuButton
            // 
            this.newFileMenuButton.Name = "newFileMenuButton";
            this.newFileMenuButton.Size = new System.Drawing.Size(112, 22);
            this.newFileMenuButton.Text = "New";
            this.newFileMenuButton.Click += new System.EventHandler(this.newFileMenuButton_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.saveAsToolStripMenuItem.Text = "Save as";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // imageToolStripMenuItem
            // 
            this.imageToolStripMenuItem.Name = "imageToolStripMenuItem";
            this.imageToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.imageToolStripMenuItem.Text = "Image";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableDisableDarkModeSettingsButton});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // enableDisableDarkModeSettingsButton
            // 
            this.enableDisableDarkModeSettingsButton.Name = "enableDisableDarkModeSettingsButton";
            this.enableDisableDarkModeSettingsButton.Size = new System.Drawing.Size(170, 22);
            this.enableDisableDarkModeSettingsButton.Text = "Enable Dark Mode";
            this.enableDisableDarkModeSettingsButton.Click += new System.EventHandler(this.enableDisableDarkModeSettingsButton_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem1});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem1
            // 
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem1.Text = "About";
            // 
            // toolMenu
            // 
            this.toolMenu.AutoSize = false;
            this.toolMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectionTool,
            this.drawRectangleButton,
            this.elipseToolButton,
            this.paintToolButton,
            this.groupToolButton});
            this.toolMenu.Location = new System.Drawing.Point(0, 24);
            this.toolMenu.Name = "toolMenu";
            this.toolMenu.Size = new System.Drawing.Size(45, 546);
            this.toolMenu.TabIndex = 1;
            this.toolMenu.Text = "toolStrip1";
            this.toolMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolMenu_ItemClicked);
            // 
            // selectionTool
            // 
            this.selectionTool.AutoSize = false;
            this.selectionTool.CheckOnClick = true;
            this.selectionTool.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.selectionTool.Image = ((System.Drawing.Image)(resources.GetObject("selectionTool.Image")));
            this.selectionTool.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.selectionTool.Name = "selectionTool";
            this.selectionTool.Size = new System.Drawing.Size(30, 30);
            this.selectionTool.Text = "toolStripButton2";
            this.selectionTool.ToolTipText = "Selection Tool";
            // 
            // drawRectangleButton
            // 
            this.drawRectangleButton.AutoSize = false;
            this.drawRectangleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.drawRectangleButton.Image = ((System.Drawing.Image)(resources.GetObject("drawRectangleButton.Image")));
            this.drawRectangleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.drawRectangleButton.Name = "drawRectangleButton";
            this.drawRectangleButton.Size = new System.Drawing.Size(30, 30);
            this.drawRectangleButton.Text = "toolStripButton1";
            this.drawRectangleButton.ToolTipText = "Draw a rectangle";
            this.drawRectangleButton.Click += new System.EventHandler(this.drawRectangleButton_Click);
            // 
            // elipseToolButton
            // 
            this.elipseToolButton.AutoSize = false;
            this.elipseToolButton.CheckOnClick = true;
            this.elipseToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.elipseToolButton.Image = ((System.Drawing.Image)(resources.GetObject("elipseToolButton.Image")));
            this.elipseToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.elipseToolButton.Name = "elipseToolButton";
            this.elipseToolButton.Size = new System.Drawing.Size(30, 30);
            this.elipseToolButton.Text = "ElipseTool";
            this.elipseToolButton.Click += new System.EventHandler(this.elipseToolButton_Click);
            // 
            // paintToolButton
            // 
            this.paintToolButton.AutoSize = false;
            this.paintToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.paintToolButton.Image = ((System.Drawing.Image)(resources.GetObject("paintToolButton.Image")));
            this.paintToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.paintToolButton.Name = "paintToolButton";
            this.paintToolButton.Size = new System.Drawing.Size(30, 30);
            this.paintToolButton.Text = "toolStripButton1";
            this.paintToolButton.ToolTipText = "Paint Tool";
            this.paintToolButton.Click += new System.EventHandler(this.paintToolButton_Click);
            // 
            // groupToolButton
            // 
            this.groupToolButton.AutoSize = false;
            this.groupToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.groupToolButton.Image = ((System.Drawing.Image)(resources.GetObject("groupToolButton.Image")));
            this.groupToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.groupToolButton.Name = "groupToolButton";
            this.groupToolButton.Size = new System.Drawing.Size(30, 30);
            this.groupToolButton.Text = "toolStripButton1";
            this.groupToolButton.ToolTipText = "Groop Tool";
            this.groupToolButton.Click += new System.EventHandler(this.groupTool_Click);
            // 
            // coordinatesLabel
            // 
            this.coordinatesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.coordinatesLabel.AutoSize = true;
            this.coordinatesLabel.Location = new System.Drawing.Point(51, 549);
            this.coordinatesLabel.Name = "coordinatesLabel";
            this.coordinatesLabel.Size = new System.Drawing.Size(25, 13);
            this.coordinatesLabel.TabIndex = 4;
            this.coordinatesLabel.Text = "0, 0";
            // 
            // canvas
            // 
            this.canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canvas.Location = new System.Drawing.Point(45, 24);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(959, 546);
            this.canvas.TabIndex = 3;
            this.canvas.Load += new System.EventHandler(this.viewPort_Load);
            this.canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.ViewPortPaint);
            this.canvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ViewPortMouseDown);
            this.canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ViewPortMouseMove);
            this.canvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ViewPortMouseUp);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 570);
            this.Controls.Add(this.coordinatesLabel);
            this.Controls.Add(this.canvas);
            this.Controls.Add(this.toolMenu);
            this.Controls.Add(this.mainMenu);
            this.MainMenuStrip = this.mainMenu;
            this.Name = "MainForm";
            this.Text = "Vector Draw";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.toolMenu.ResumeLayout(false);
            this.toolMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolMenu;
        private System.Windows.Forms.ToolStripButton drawRectangleButton;
        private System.Windows.Forms.ToolStripButton selectionTool;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
        private Views.DoubleBufferedPanel canvas;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enableDisableDarkModeSettingsButton;
        private System.Windows.Forms.Label coordinatesLabel;
        private System.Windows.Forms.ToolStripButton elipseToolButton;
        private System.Windows.Forms.ToolStripButton paintToolButton;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.ToolStripButton groupToolButton;
        private System.Windows.Forms.ToolStripMenuItem newFileMenuButton;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
    }
}

