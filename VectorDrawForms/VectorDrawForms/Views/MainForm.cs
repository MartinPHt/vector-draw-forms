using System;
using System.Configuration;
using System.IO;
using System.Linq;
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
            CenterToScreen();
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

        private bool isChangeMade = false;
        /// <summary>
        /// Returns true if the canvas was redrawn at least once after the file was saved/created.
        /// </summary>
        public bool IsChangeMade
        {
            get { return isChangeMade; }
            private set
            {
                isChangeMade = value;

                // Indicate that changes were made
                try
                {
                    bool fileHasStar = this.Text[this.Text.Length - 1] == '*';

                    if (value)
                    {
                        if (!fileHasStar)
                            this.Text += "*";
                    }
                    else
                    {
                        if (fileHasStar)
                            this.Text = Text.Substring(0, Text.Length - 1);
                    }
                }
                catch { }
            }
        }
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
        /// Handles the dragging of selected primitives on the canvas
        /// </summary>
        /// <param name="e"></param>
        private void HandleDragging(MouseEventArgs e)
        {
            if (dialogProcessor.IsDragging)
            {
                dialogProcessor.TranslateTo(e.Location);
                RedrawCanvas();
            }

            coordinatesLabel.Text = string.Format("{0}, {1}", e.Location.X, e.Location.Y);
        }

        /// <summary>
        /// Opens <see cref="SaveFileDialog"/> window and creates file based on user's directory and file name input.
        /// </summary>
        private void HandleSaveAs()
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
                MessageBox.Show($"Error has occured while saving to {FilePath} file. Exception message:" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handle save file. If file does not exist, Opens <see cref="SaveFileDialog"/> window and creates file based on user's directory.
        /// </summary>
        private void HandleSaveFile()
        {
            try
            {
                if (!File.Exists(FilePath))
                    HandleSaveAs();
                else
                    SaveToFile(FilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error has occured while saving to {FilePath} file. Exception message:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles pasting of coppied shapes
        /// </summary>
        private void HandlePasteShape()
        {
            try
            {
                dialogProcessor.PasteSelection();
                RedrawCanvas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Couldn't paste shape. Exception message:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HandleCopyShape()
        {
            try
            {
                dialogProcessor.CopySelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Couldn't Copy shape. Exception message:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HandleDeleteShape()
        {
            try
            {
                if (dialogProcessor.Selections.Count == 0)
                {
                    MessageBox.Show("You have to select a shape in order to use the Remove Shape Tool", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var confirmResult = MessageBox.Show($"Do you want to delete the selection?", "VectorDraw", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmResult == DialogResult.Yes)
                {
                    dialogProcessor.DeleteSelection();
                    RedrawCanvas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error has occured during Remove Shape Tool's execution. Exception message:" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                //Go back to Selection Tool
                selectionToolButton.PerformClick();
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

                var confirmResult = MessageBox.Show($"Do you want to save changes to {Path.GetFileName(FilePath)}", "VectorDraw",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (confirmResult == DialogResult.Yes)
                {
                    if (!File.Exists(FilePath))
                        HandleSaveAs();
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
                MessageBox.Show($"Unexpexted error has occured. Exception message:" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            //Update selected shapes count label everytime the canvas is redrawn
            selectedShapesCountLabel.Text = dialogProcessor.Selections.Count.ToString();
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
                    selectionToolButton.Image = Properties.Resources.PointerDark;
                    drawRectangleButton.Image = Properties.Resources.RectangleDark;
                    elipseToolButton.Image = Properties.Resources.ElipseDark;
                    editToolButton.Image = Properties.Resources.BrushDark;
                    groupToolButton.Image = Properties.Resources.GroupDark;
                    removeShapeToolButton.Image = Properties.Resources.BinDark;
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
                    selectionToolButton.Image = Properties.Resources.PointerLight;
                    drawRectangleButton.Image = Properties.Resources.RectangleLight;
                    elipseToolButton.Image = Properties.Resources.ElipseLight;
                    editToolButton.Image = Properties.Resources.BrushLight;
                    groupToolButton.Image = Properties.Resources.GroupLight;
                    removeShapeToolButton.Image = Properties.Resources.BinLight;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error has occured while changing window light/dark mode. Exception message:" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region Event Handling Methods (Functionality, Buttons)
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

            //Go back to Selection Tool
            selectionToolButton.PerformClick();
        }

        private void viewPort_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Invoked when canvas.Invalidate() is called.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            if (selectionToolButton.Checked)
            {
                IShape selectedShape = dialogProcessor.ContainsPoint(e.Location);

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

                //Indicate dragging and update last location in dialog processor
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

        /// <summary>
        /// Invoked when the mouse moves on top of the canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewPortMouseMove(object sender, MouseEventArgs e)
        {
            HandleDragging(e);
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

            if (selectedToolStripButton == selectionToolButton)
                canvas.Cursor = Cursors.Default;
            else
                canvas.Cursor = Cursors.Cross;
        }

        private void elipseToolButton_Click(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomElipse();
            RedrawCanvas();

            //Go back to Selection Tool
            selectionToolButton.PerformClick();
        }

        private void paintToolButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (
                    dialogProcessor.Selections == null
                    || dialogProcessor.Selections.Count != 1
                    || dialogProcessor.Selections.OfType<GroupShape>().Count() != 0
                    )
                {
                    MessageBox.Show("To open Edit Tool you have to select only one non group shape with the Selection Tool first.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var shape = dialogProcessor.Selections[0];
                var dialog
                    = new ShapeEditorForm(shape.Width, shape.Height, shape.RotationAngle, shape.StrokeThickness, shape.StrokeColor, shape.FillColor);

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    shape.Width = dialog.ShapeWidth;
                    shape.Height = dialog.ShapeHeight;
                    shape.StrokeThickness = dialog.StrokeThickness;
                    shape.StrokeColor = dialog.StrokeColor;
                    shape.FillColor = dialog.FillColor;
                    shape.RotationAngle = dialog.RotationAngle;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error has occured during Edit Tool's execution. Exception message:" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                //Go back to Selection Tool
                selectionToolButton.PerformClick();
                RedrawCanvas();
            }
        }

        private void groupTool_Click(object sender, EventArgs e)
        {
            try
            {
                //Ensure that there are selected shapes
                if (dialogProcessor.Selections.Count < 1)
                {
                    MessageBox.Show("To use Group Tool you have to select at least one item with the Selection Tool first.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (dialogProcessor.Selections.Count > 1)
                {
                    dialogProcessor.GroupSelectedShapes();
                    RedrawCanvas();
                }
                else if (dialogProcessor.Selections[0] is GroupShape)
                {
                    dialogProcessor.UngroupSelectedShape();
                    RedrawCanvas();
                }
                else
                {
                    MessageBox.Show("Cannot use Group Tool on one shape. Select at least two shapes with the Selection Tool first.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpexted error has occured during Group Tool's execution. Exception message:" + ex.Message, "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            finally
            {
                //Go back to Selection Tool
                selectionToolButton.PerformClick();
            }
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
            HandleSaveAs();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HandleSaveFile();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!EnsureUnsavedWorkIsNotLost())
                e.Cancel = true;
        }

        private void removeShapeToolButton_Click(object sender, EventArgs e)
        {
            HandleDeleteShape();
        }

        private void clearCanvasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (EnsureUnsavedWorkIsNotLost())
                {
                    dialogProcessor.ClearShapes();
                    RedrawCanvas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error has occured during Remove Shape Tool's execution. Exception message:" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ungroupSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dialogProcessor.Selections.OfType<GroupShape>().ToList().Count < 0)
                {
                    MessageBox.Show("Cannot ungroup because there is no group selected", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    return;
                }

                dialogProcessor.UngroupSelectedShape();
                RedrawCanvas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error has occured during ungrouping of shapes. Exception message:" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void groupSelectionMenuButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (dialogProcessor.Selections.Count <= 1)
                {
                    MessageBox.Show("Cannot use Group Tool. Select at least two shapes with the Selection Tool first.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    return;
                }

                dialogProcessor.GroupSelectedShapes();
                RedrawCanvas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error has occured during grouping of shapes. Exception message:" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if Ctrl and S keys are pressed
            if (e.Control && e.KeyCode == Keys.S)
            {
                HandleSaveFile();
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                HandleCopyShape();
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                if (dialogProcessor.CoppiedSelection.Count > 0)
                    HandlePasteShape();

            }
            else if (!e.Control && e.KeyCode == Keys.Delete)
            {
                HandleDeleteShape();
            }
        }
        #endregion

        #region UI Helper Event Handlers (Enable/Disable controls)
        private void groupMenuButton_DropDownOpened(object sender, EventArgs e)
        {
            try
            {
                //Handle Group disable/enable
                if (dialogProcessor.Selections.Count > 1)
                {
                    groupSelectionMenuButton.Enabled = true;
                }
                else
                {
                    groupSelectionMenuButton.Enabled = false;
                }

                //Handle Ungroup disable/enable
                if (dialogProcessor.Selections.Count > 0
                    && dialogProcessor.Selections.All(n => n is GroupShape))
                {
                    ungroupSelectionMenuButton.Enabled = true;
                }
                else
                {
                    ungroupSelectionMenuButton.Enabled = false;
                }
            }
            catch { }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HandleCopyShape();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HandlePasteShape();
        }
        #endregion
    }

    internal enum UIMode
    {
        Light,
        Dark
    }
}
