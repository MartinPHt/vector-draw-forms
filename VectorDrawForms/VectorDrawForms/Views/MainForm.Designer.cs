namespace VectorDrawForms
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
            this.groupMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.groupSelectionMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ungroupSelectionMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearCanvasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableDisableDarkModeSettingsButton = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenu = new System.Windows.Forms.ToolStrip();
            this.coordinatesLabel = new System.Windows.Forms.Label();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.selectedShapesCountLabel = new System.Windows.Forms.Label();
            this.canvas = new VectorDrawForms.Views.DoubleBufferedPanel();
            this.selectionToolButton = new System.Windows.Forms.ToolStripButton();
            this.rectangleToolButton = new System.Windows.Forms.ToolStripButton();
            this.elipseToolButton = new System.Windows.Forms.ToolStripButton();
            this.triangleToolButton = new System.Windows.Forms.ToolStripButton();
            this.dotToolButton = new System.Windows.Forms.ToolStripButton();
            this.editToolButton = new System.Windows.Forms.ToolStripButton();
            this.groupToolButton = new System.Windows.Forms.ToolStripButton();
            this.removeShapeToolButton = new System.Windows.Forms.ToolStripButton();
            this.lineToolButton = new System.Windows.Forms.ToolStripButton();
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
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.groupMenuButton,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.editSelectionToolStripMenuItem,
            this.deleteSelectionToolStripMenuItem,
            this.clearCanvasToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.DropDownOpened += new System.EventHandler(this.editToolStripMenuItem_DropDownOpened);
            // 
            // groupMenuButton
            // 
            this.groupMenuButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.groupSelectionMenuButton,
            this.ungroupSelectionMenuButton});
            this.groupMenuButton.Name = "groupMenuButton";
            this.groupMenuButton.Size = new System.Drawing.Size(236, 22);
            this.groupMenuButton.Text = "Group";
            this.groupMenuButton.DropDownOpened += new System.EventHandler(this.groupMenuButton_DropDownOpened);
            // 
            // groupSelectionMenuButton
            // 
            this.groupSelectionMenuButton.Name = "groupSelectionMenuButton";
            this.groupSelectionMenuButton.Size = new System.Drawing.Size(246, 22);
            this.groupSelectionMenuButton.Text = "Group selection                Ctrl + G";
            this.groupSelectionMenuButton.Click += new System.EventHandler(this.groupSelectionMenuButton_Click);
            // 
            // ungroupSelectionMenuButton
            // 
            this.ungroupSelectionMenuButton.Name = "ungroupSelectionMenuButton";
            this.ungroupSelectionMenuButton.Size = new System.Drawing.Size(246, 22);
            this.ungroupSelectionMenuButton.Text = "Ungroup selection           Ctrl + U";
            this.ungroupSelectionMenuButton.Click += new System.EventHandler(this.ungroupSelectionToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.copyToolStripMenuItem.Text = "Copy                               Ctrl + C";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.pasteToolStripMenuItem.Text = "Paste                               Ctrl + V";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // editSelectionToolStripMenuItem
            // 
            this.editSelectionToolStripMenuItem.Name = "editSelectionToolStripMenuItem";
            this.editSelectionToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.editSelectionToolStripMenuItem.Text = "Edit selection                 Ctrl + E";
            this.editSelectionToolStripMenuItem.Click += new System.EventHandler(this.editSelectionCtrlEToolStripMenuItem_Click);
            // 
            // deleteSelectionToolStripMenuItem
            // 
            this.deleteSelectionToolStripMenuItem.Name = "deleteSelectionToolStripMenuItem";
            this.deleteSelectionToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.deleteSelectionToolStripMenuItem.Text = "Delete selection             del";
            this.deleteSelectionToolStripMenuItem.Click += new System.EventHandler(this.deleteSelectionToolStripMenuItem_Click);
            // 
            // clearCanvasToolStripMenuItem
            // 
            this.clearCanvasToolStripMenuItem.Name = "clearCanvasToolStripMenuItem";
            this.clearCanvasToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.clearCanvasToolStripMenuItem.Text = "Clear Canvas";
            this.clearCanvasToolStripMenuItem.Click += new System.EventHandler(this.clearCanvasToolStripMenuItem_Click);
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
            this.selectionToolButton,
            this.rectangleToolButton,
            this.elipseToolButton,
            this.triangleToolButton,
            this.lineToolButton,
            this.dotToolButton,
            this.editToolButton,
            this.groupToolButton,
            this.removeShapeToolButton});
            this.toolMenu.Location = new System.Drawing.Point(0, 24);
            this.toolMenu.Name = "toolMenu";
            this.toolMenu.Size = new System.Drawing.Size(45, 546);
            this.toolMenu.TabIndex = 1;
            this.toolMenu.Text = "toolStrip1";
            this.toolMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolMenu_ItemClicked);
            // 
            // coordinatesLabel
            // 
            this.coordinatesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.coordinatesLabel.AutoSize = true;
            this.coordinatesLabel.Location = new System.Drawing.Point(51, 551);
            this.coordinatesLabel.Name = "coordinatesLabel";
            this.coordinatesLabel.Size = new System.Drawing.Size(25, 13);
            this.coordinatesLabel.TabIndex = 4;
            this.coordinatesLabel.Text = "0, 0";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(871, 551);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Selected Shapes: ";
            // 
            // selectedShapesCountLabel
            // 
            this.selectedShapesCountLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.selectedShapesCountLabel.AutoSize = true;
            this.selectedShapesCountLabel.Location = new System.Drawing.Point(970, 551);
            this.selectedShapesCountLabel.Name = "selectedShapesCountLabel";
            this.selectedShapesCountLabel.Size = new System.Drawing.Size(13, 13);
            this.selectedShapesCountLabel.TabIndex = 6;
            this.selectedShapesCountLabel.Text = "0";
            // 
            // canvas
            // 
            this.canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canvas.Location = new System.Drawing.Point(45, 24);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(959, 546);
            this.canvas.TabIndex = 3;
            this.canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.ViewPortPaint);
            this.canvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ViewPortMouseDown);
            this.canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ViewPortMouseMove);
            this.canvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ViewPortMouseUp);
            // 
            // selectionToolButton
            // 
            this.selectionToolButton.AutoSize = false;
            this.selectionToolButton.CheckOnClick = true;
            this.selectionToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.selectionToolButton.Image = ((System.Drawing.Image)(resources.GetObject("selectionToolButton.Image")));
            this.selectionToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.selectionToolButton.Name = "selectionToolButton";
            this.selectionToolButton.Size = new System.Drawing.Size(30, 30);
            this.selectionToolButton.Text = "Selection Tool";
            this.selectionToolButton.ToolTipText = "Selection Tool";
            // 
            // rectangleToolButton
            // 
            this.rectangleToolButton.AutoSize = false;
            this.rectangleToolButton.CheckOnClick = true;
            this.rectangleToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.rectangleToolButton.Image = ((System.Drawing.Image)(resources.GetObject("rectangleToolButton.Image")));
            this.rectangleToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.rectangleToolButton.Name = "rectangleToolButton";
            this.rectangleToolButton.Size = new System.Drawing.Size(30, 30);
            this.rectangleToolButton.Text = "Rectangle Tool";
            this.rectangleToolButton.ToolTipText = "Rectangle Tool";
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
            this.elipseToolButton.Text = "Elipse Tool";
            // 
            // triangleToolButton
            // 
            this.triangleToolButton.AutoSize = false;
            this.triangleToolButton.CheckOnClick = true;
            this.triangleToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.triangleToolButton.Image = ((System.Drawing.Image)(resources.GetObject("triangleToolButton.Image")));
            this.triangleToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.triangleToolButton.Name = "triangleToolButton";
            this.triangleToolButton.Size = new System.Drawing.Size(30, 30);
            this.triangleToolButton.Text = "Triangle Tool";
            // 
            // dotToolButton
            // 
            this.dotToolButton.AutoSize = false;
            this.dotToolButton.CheckOnClick = true;
            this.dotToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.dotToolButton.Image = ((System.Drawing.Image)(resources.GetObject("dotToolButton.Image")));
            this.dotToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.dotToolButton.Name = "dotToolButton";
            this.dotToolButton.Size = new System.Drawing.Size(30, 30);
            this.dotToolButton.Text = "toolStripButton1";
            this.dotToolButton.ToolTipText = "Create a dot";
            // 
            // editToolButton
            // 
            this.editToolButton.AutoSize = false;
            this.editToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.editToolButton.Image = ((System.Drawing.Image)(resources.GetObject("editToolButton.Image")));
            this.editToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.editToolButton.Name = "editToolButton";
            this.editToolButton.Size = new System.Drawing.Size(30, 30);
            this.editToolButton.Text = "Paint Tool";
            this.editToolButton.ToolTipText = "Edit Tool";
            this.editToolButton.Click += new System.EventHandler(this.editToolButton_Click);
            // 
            // groupToolButton
            // 
            this.groupToolButton.AutoSize = false;
            this.groupToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.groupToolButton.Image = ((System.Drawing.Image)(resources.GetObject("groupToolButton.Image")));
            this.groupToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.groupToolButton.Name = "groupToolButton";
            this.groupToolButton.Size = new System.Drawing.Size(30, 30);
            this.groupToolButton.Text = "Group Tool";
            this.groupToolButton.ToolTipText = "Groop Tool";
            this.groupToolButton.Click += new System.EventHandler(this.groupTool_Click);
            // 
            // removeShapeToolButton
            // 
            this.removeShapeToolButton.AutoSize = false;
            this.removeShapeToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.removeShapeToolButton.Image = ((System.Drawing.Image)(resources.GetObject("removeShapeToolButton.Image")));
            this.removeShapeToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeShapeToolButton.Name = "removeShapeToolButton";
            this.removeShapeToolButton.Size = new System.Drawing.Size(30, 30);
            this.removeShapeToolButton.Text = "Shape Remove Tool";
            this.removeShapeToolButton.Click += new System.EventHandler(this.removeShapeToolButton_Click);
            // 
            // lineToolButton
            // 
            this.lineToolButton.AutoSize = false;
            this.lineToolButton.CheckOnClick = true;
            this.lineToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.lineToolButton.Image = ((System.Drawing.Image)(resources.GetObject("lineToolButton.Image")));
            this.lineToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.lineToolButton.Name = "lineToolButton";
            this.lineToolButton.Size = new System.Drawing.Size(30, 30);
            this.lineToolButton.Text = "Line Tool";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 570);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.selectedShapesCountLabel);
            this.Controls.Add(this.coordinatesLabel);
            this.Controls.Add(this.canvas);
            this.Controls.Add(this.toolMenu);
            this.Controls.Add(this.mainMenu);
            this.KeyPreview = true;
            this.MainMenuStrip = this.mainMenu;
            this.Name = "MainForm";
            this.Text = "Vector Draw";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
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
        private System.Windows.Forms.ToolStripButton rectangleToolButton;
        private System.Windows.Forms.ToolStripButton selectionToolButton;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
        private Views.DoubleBufferedPanel canvas;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enableDisableDarkModeSettingsButton;
        private System.Windows.Forms.Label coordinatesLabel;
        private System.Windows.Forms.ToolStripButton elipseToolButton;
        private System.Windows.Forms.ToolStripButton editToolButton;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.ToolStripButton groupToolButton;
        private System.Windows.Forms.ToolStripMenuItem newFileMenuButton;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton removeShapeToolButton;
        private System.Windows.Forms.ToolStripMenuItem clearCanvasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem groupMenuButton;
        private System.Windows.Forms.ToolStripMenuItem groupSelectionMenuButton;
        private System.Windows.Forms.ToolStripMenuItem ungroupSelectionMenuButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label selectedShapesCountLabel;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton dotToolButton;
        private System.Windows.Forms.ToolStripMenuItem editSelectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteSelectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton triangleToolButton;
        private System.Windows.Forms.ToolStripButton lineToolButton;
    }
}

