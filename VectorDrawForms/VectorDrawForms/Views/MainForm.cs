using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using VectorDrawForms.Models;
using VectorDrawForms.Processors;

namespace VectorDrawForms
{
    public partial class MainForm : Form
    {
        #region Fields
        /// <summary>
        /// An aggregated dialog processor in the form makes easier the manipulation of the model.
        /// </summary>
        private DialogProcessor dialogProcessor = new DialogProcessor();
        private ToolStripButton selectedToolStripButton;
        #endregion

        #region Constructor
        public MainForm()
        {
            InitializeComponent();
            PrepareInitialState();
            ChangeUIMode("UIMode");
        }
        #endregion

        #region Properties
        private string filePath;
        /// <summary>
        /// Gets the path of the current loaded file
        /// </summary>
        private string FilePath 
        { 
            get { return filePath; }
            set 
            { 
                filePath = value; 
                this.Text = string.Concat(Assembly.GetCallingAssembly().GetName().Name, $" - {Path.GetFileName(filePath)}");
            } 
        }

        /// <summary>
        /// Returns true if the canvas was redrawn at least once after the file was saved/created.
        /// </summary>
        private bool IsChangeMade { get; set; } = false;
        public static MainForm Instance { get; private set; }
        #endregion

        #region Methods
        internal static Form Initialize()
        {
            if (Instance != null)
                Instance.Dispose();

            Instance = new MainForm();
            return Instance;
        }

        /// <summary>
        /// Opens <see cref="SaveFileDialog"/> window and creates file based on user's directory and file name input.
        /// </summary>
        private void ExecuteSaveAs()
        {
            try
            {
                var dialog = new SaveFileDialog();
                dialog.InitialDirectory = Environment.CurrentDirectory;
                dialog.Title = "Save as";
                dialog.DefaultExt = "file";
                dialog.CheckPathExists = true;
                dialog.FileName = Path.GetFileName(FilePath);

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    FilePath = dialog.FileName;
                    SaveToFile(FilePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error has occured while saving to {FilePath} file. Exception message:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Wrapper. Ensures that any unsaved work is not lost by opening a <see cref="MessageBox"/> asking if it should save changes before moving on.
        /// Returns true to indicate that any following actions can proceed and false to stop all following actions. <br></br> <br></br>
        /// 
        /// <example> Example:
        /// <code>
        ///     if (EnsureUnsavedWorkIsNotLost())
        ///     {
        ///         //following action:
        ///         RedrawCanvas();
        ///      }
        /// </code>
        /// </example>
        /// 
        /// </summary>
        /// <returns>true if there are changes and user agreed to either save the changes by pressing "Yes" or to lose it by pressing "No". 
        /// <br></br>false if the dialog was closed or canceled by pressing "Cancel". Done if the user does not want to prceed
        /// </returns>
        private bool EnsureUnsavedWorkIsNotLost()
        {
            try
            {
                // If change has not been made, return true to indicate that following operations can proceed
                if (!IsChangeMade)
                    return true;

                var confirmResult = MessageBox.Show($"Do you want to save changes to {Path.GetFileName(FilePath)}", "VectorDraw", MessageBoxButtons.YesNoCancel);
                if (confirmResult == DialogResult.Yes)
                {
                    if (!File.Exists(FilePath))
                        ExecuteSaveAs();
                    else
                        SaveToFile(FilePath);

                    return true;
                }
                else if (confirmResult == DialogResult.No)
                {
                    // Don't save to a file and proceed. 
                    return true;
                }
                else
                {
                    // Return false to indicate that the user doesn't want to proceed.
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpexted error has occured. Exception message:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Return false to indicate an error in UnsavedWork handling.
                return false;
            }
        }

        /// <summary>
        /// Saves the canvas to the provided path and resets the IsChangeMade property to false.
        /// </summary>
        /// <param name="path"></param>
        private void SaveToFile(string path)
        {
            dialogProcessor.SaveToFile(path);
            IsChangeMade = false;
        }

        /// <summary>
        /// Invalidates the entire surface of the canvas, causes it to be redrawn and indicates change being made.
        /// Use when there is a change in the model.
        /// </summary>
        private void RedrawCanvas()
        {
            canvas.Invalidate();
            IsChangeMade = true;
        }

        /// <summary>
        /// Prepares the state of the <see cref="MainForm"/> on initialization
        /// </summary>
        private void PrepareInitialState()
        {
            //Prepare for initial load.
            //Untitled will be used as a name for the inital file
            FilePath = "Untitled";
        }

        private void ChangeUIMode(string key)
        {
            try
            {
                var uiMode = ConfigurationManager.AppSettings[key];
                if (uiMode == UIMode.Light.ToString())
                {
                    enableDisableDarkModeSettingsButton.Text = "Enable Dark Mode";
                    this.ForeColor = ApplicationColors.MainUIDark;
                    this.BackColor = ApplicationColors.MainUILight;

                    //Assign light color mode to main menu
                    mainMenu.ForeColor = ApplicationColors.MainUIDark;
                    mainMenu.BackColor = ApplicationColors.MainUILight;

                    //Assign light color mode to tool menu
                    toolMenu.ForeColor = ApplicationColors.MainUIDark;
                    toolMenu.BackColor = ApplicationColors.MainUILight;

                    //Change images to the opposite color
                    selectionTool.Image = Properties.Resources.PointerDark;
                    drawRectangleButton.Image = Properties.Resources.RectangleDark;
                    elipseToolButton.Image = Properties.Resources.ElipseDark;
                    paintToolButton.Image = Properties.Resources.BrushDark;
                    groupToolButton.Image = Properties.Resources.GroupDark;
                }
                else
                {
                    enableDisableDarkModeSettingsButton.Text = "Disable Dark Mode";
                    this.ForeColor = ApplicationColors.MainUILight;
                    this.BackColor = ApplicationColors.MainUIDark;

                    //Assign dark color mode to main menu
                    mainMenu.ForeColor = ApplicationColors.MainUILight;
                    mainMenu.BackColor = ApplicationColors.SecondaryUIDark;

                    //Assign dark color mode to tool menu
                    toolMenu.ForeColor = ApplicationColors.MainUILight;
                    toolMenu.BackColor = ApplicationColors.SecondaryUIDark;

                    //Change images to the opposite color
                    selectionTool.Image = Properties.Resources.PointerLight;
                    drawRectangleButton.Image = Properties.Resources.RectangleLight;
                    elipseToolButton.Image = Properties.Resources.ElipseLight;
                    paintToolButton.Image = Properties.Resources.BrushLight;
                    groupToolButton.Image = Properties.Resources.GroupLight;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error has occured while changing window light/dark mode. Exception message:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region Event Handling Methods
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Close the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void drawRectangleButton_Click(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomRectangle();
            RedrawCanvas();
            
        }

        private void viewPort_Load(object sender, EventArgs e)
        {

        }

        private void ViewPortPaint(object sender, PaintEventArgs e)
        {
            dialogProcessor.ReDraw(sender, e);
        }

        private void ViewPortMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                dialogProcessor.Selections.Clear();
                return;
            }

            if (selectionTool.Checked)
            {
                Shape selectedShape = dialogProcessor.ContainsPoint(e.Location);

                //Check if the new selection is null
                if (selectedShape != null)
                {
                    //Check if the left mouse key is pressed and if ctrl was pressed when the mouse click occured
                    if ((ModifierKeys & Keys.Control) == Keys.Control)
                    {
                        //Add the new selection if it is not in the selections list
                        if (!dialogProcessor.Selections.Contains(selectedShape))
                            dialogProcessor.Selections.Add(selectedShape);
                    }
                    else
                    {
                        //Clear the selection since ctrl wasn't pressed.
                        dialogProcessor.Selections.Clear();

                        //Add the selected shape if there is any
                        dialogProcessor.Selections.Add(selectedShape);
                    }
                }
                else
                {
                    //Clear selection since selectedShape is null
                    dialogProcessor.Selections.Clear();
                }

                dialogProcessor.IsDragging = true;
                dialogProcessor.LastLocation = e.Location;
                RedrawCanvas();
            }
            else if (elipseToolButton.Checked)
            {
                //if (actiiveDrawing)
                //{
                //    switch (drawIndex)
                //    {
                //        case 0: // point
                //            points.Add(new Models.Point(currentPossition));
                //            break;
                //        default:
                //            break;
                //    }
                //    viewPort.Invalidate();
                //}
            }
        }

        private void ViewPortMouseMove(object sender, MouseEventArgs e)
        {
            if (dialogProcessor.IsDragging)
            {
                dialogProcessor.TranslateTo(e.Location);
                RedrawCanvas();
            }

            coordinatesLabel.Text = string.Format("{0}, {1}", e.Location.X, e.Location.Y);
        }

        private void ViewPortMouseUp(object sender, MouseEventArgs e)
        {
            dialogProcessor.IsDragging = false;
        }

        private void enableDisableDarkModeSettingsButton_Click(object sender, EventArgs e)
        {
            try
            {
                string key = "UIMode";
                if (ConfigurationManager.AppSettings[key] == UIMode.Light.ToString())
                    ConfigurationManager.AppSettings[key] = UIMode.Dark.ToString();
                else
                    ConfigurationManager.AppSettings[key] = UIMode.Light.ToString();

                ChangeUIMode(key);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (selectedToolStripButton != null)
                selectedToolStripButton.Checked = false;

            selectedToolStripButton = e.ClickedItem as ToolStripButton;

            if (selectedToolStripButton == selectionTool)
                canvas.Cursor = Cursors.Default;
            else
                canvas.Cursor = Cursors.Cross;
        }

        private void elipseToolButton_Click(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomElipse();
            RedrawCanvas();
        }

        private void paintToolButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (dialogProcessor.Selections == null || dialogProcessor.Selections.Count == 0)
                {
                    MessageBox.Show("To open Paint Tool you have to select an item with the Selection Tool first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var confirmationDialog = new PaintToolConfirmationForm();
                if (confirmationDialog.ShowDialog() == DialogResult.OK)
                {
                    if (colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        foreach (var item in dialogProcessor.Selections)
                        {
                            if (confirmationDialog.SelectedItem == "Border Color")
                                item.StrokeColor = colorDialog.Color;
                            else if (confirmationDialog.SelectedItem == "Fill Color")
                                item.FillColor = colorDialog.Color;
                        }

                        RedrawCanvas();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error has occured during Paint Tool's exevution. Exception message:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void groupTool_Click(object sender, EventArgs e)
        {
            //Izchisli obhwashtashtiq prawoygylnik,
            //suzdai nova grupa
            //dobavi grupata v spisuka s primitivi
            //SubShape = Selection
        }

        private void newFileMenuButton_Click(object sender, EventArgs e)
        {
            if (EnsureUnsavedWorkIsNotLost())
            {
                //Return to initial state
                FilePath = "Untitled";
                dialogProcessor.PrepareForCleenSheet();
                RedrawCanvas();

                //Indicate that change is not made
                IsChangeMade = false;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.InitialDirectory = Environment.CurrentDirectory;
            dialog.Title = "Open";
            dialog.DefaultExt = "file";
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FilePath = dialog.FileName;
                dialogProcessor.ReadFile(dialog.FileName);
            }

            RedrawCanvas();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExecuteSaveAs();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(FilePath))
                    ExecuteSaveAs();
                else
                    SaveToFile(FilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error has occured while saving to {FilePath} file. Exception message:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }           
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!EnsureUnsavedWorkIsNotLost())           
                e.Cancel = true;
        }
        #endregion
    }

    internal enum UIMode
    {
        Light,
        Dark
    }
}
