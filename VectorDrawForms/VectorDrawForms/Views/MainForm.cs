using System;
using System.Configuration;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using VectorDrawForms.Models;
using VectorDrawForms.Processors;
using VectorDrawForms.Assets.Helpers;
using VectorDrawForms.Views;

namespace VectorDrawForms
{
    public partial class MainForm : Form
    {
        #region Fields
        private ToolStripButton selectedToolStripButton;
        private Button addButton;

        //Drawing fiels
        private bool isDrawingPerformed = false;
        private PointF previewShapeStartPoint;
        private IShape currentDrawnShape = null;
        private int createdCanvases = 0;
        #endregion

        #region Constructor
        public MainForm()
        {
            InitializeComponent();
            PrepareInitialState();
            InitializeTabControl();
            ChangeUIMode("UIMode");
            CenterToScreen();
        }
        #endregion

        #region Properties
        private DialogProcessor CurrentDialogProcessor
        {
            get { return GetCurrentDialogProcessor(); }
        }

        private DoubleBufferedPanel CurrentCanvas
        {
            get { return GetCurrentCanvas(); }
        }

        private TabPage SelectedTab
        {
            get { return tabControl.SelectedTab; }
        }

        /// <summary>
        /// Full file path of the selected tab
        /// </summary>
        private string SelectedTabFilePath
        {
            get
            {
                try
                {
                    return SelectedTab.Tag.ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                try
                {
                    SelectedTab.Tag = value;
                    SelectedTab.Text = Path.GetFileName(value);
                }
                catch { }
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
                    bool fileHasStar = tabControl.SelectedTab.Text[tabControl.SelectedTab.Text.Length - 1] == '*';

                    if (value)
                    {
                        if (!fileHasStar)
                            tabControl.SelectedTab.Text += "*";
                    }
                    else
                    {
                        if (fileHasStar)
                            tabControl.SelectedTab.Text = tabControl.SelectedTab.Text.Substring(0, tabControl.SelectedTab.Text.Length - 1);
                    }
                }
                catch { }
            }
        }

        private Color selectedColor = Color.Gray;
        public Color SelectedColor
        {
            get { return selectedColor; }
            private set
            {
                selectedColor = value;
                colorPicker.BackColor = selectedColor;
            }
        }

        public static MainForm Instance { get; private set; }

        public int StrokeThickness
        {
            get
            {
                try
                {
                    return int.Parse(newShapeStrokeThicknessTextBox.Text);
                }
                catch
                {
                    return 2;
                }
            }
        }
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
            CurrentDialogProcessor.TranslateTo(e.Location);
            RedrawCanvas();
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
                dialog.Filter = "Png Image (.png)|*.png|File|*.vdfile";
                dialog.DefaultExt = "vdfile";
                dialog.CheckPathExists = true;
                dialog.FileName = SelectedTab.Tag.ToString();

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    SelectedTabFilePath = dialog.FileName;
                    SaveToFile(SelectedTabFilePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error has occured while saving to {SelectedTabFilePath} file. Exception message:" + ex.Message, "Error",
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
                if (!File.Exists(SelectedTabFilePath))
                    HandleSaveAs();
                else
                    SaveToFile(SelectedTabFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error has occured while saving to {SelectedTabFilePath} file. Exception message:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles pasting of coppied shapes
        /// </summary>
        private void HandlePasteShape()
        {
            try
            {
                CurrentDialogProcessor.PasteSelection();
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
                CurrentDialogProcessor.CopySelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Couldn't Copy shape. Exception message:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HandleCutShape()
        {
            try
            {
                CurrentDialogProcessor.CutSelection();
                RedrawCanvas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Couldn't Cut shape. Exception message:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HandleDeleteShape()
        {
            try
            {
                if (CurrentDialogProcessor.Selections.Count == 0)
                {
                    MessageBox.Show("You have to select a shape in order to use the Remove Shape Tool", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var confirmResult = MessageBox.Show($"Do you want to delete the selection?", "VectorDraw", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmResult == DialogResult.Yes)
                {
                    CurrentDialogProcessor.DeleteSelection();
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

                var confirmResult = MessageBox.Show($"Do you want to save changes to {Path.GetFileName(SelectedTabFilePath)}", "VectorDraw",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (confirmResult == DialogResult.Yes)
                {
                    if (!File.Exists(SelectedTabFilePath))
                        HandleSaveAs();
                    else
                        SaveToFile(SelectedTabFilePath);

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
            var extension = Path.GetExtension(path);
            if (extension == ".png")
            {
                SaveShapesToPNG(path);
            }
            else
            {
                FileStream stream = new FileStream(path, FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, CurrentDialogProcessor.ShapeList);

                //Dispose fileStream
                stream.Flush();
                stream.Close();

                IsChangeMade = false;
            }
        }

        /// <summary>
        /// Invalidates the entire surface of the current canvas, causes it to be redrawn and indicates change being made.
        /// Use when there is a change in the model.
        /// </summary>
        private void RedrawCanvas()
        {
            CurrentCanvas.Invalidate();
            IsChangeMade = true;

            //Update selected shapes count label everytime the canvas is redrawn
            selectedShapesCountLabel.Text = CurrentDialogProcessor.Selections.Count.ToString();
        }

        private void ClearSelections()
        {
            var dialogProcessor = CurrentDialogProcessor;
            dialogProcessor.Selections.Clear();
            selectedShapesCountLabel.Text = dialogProcessor.Selections.Count.ToString();
        }

        /// <summary>
        /// Prepares the state of the <see cref="MainForm"/> on initialization
        /// </summary>
        private void PrepareInitialState()
        {
            //Prepare for initial load
            this.Text = Assembly.GetCallingAssembly().GetName().Name;
        }

        private void InitializeTabControl()
        {
            tabControl.Dock = DockStyle.Fill;
            tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            tabControl.Padding = new Point(12, 4);

            //Add button
            addButton = new Button();
            addButton.Text = "+";
            addButton.Size = new Size(24, 24);
            addButton.Click += new EventHandler(AddButton_Click);
            this.Controls.Add(addButton);

            CreateNewTabPage();
        }

        private void CreateNewTabPage()
        {
            TabPage tabPage = new TabPage()
            {
                Text = $"Untitled{createdCanvases + 1}",
                Tag = $"Untitled{createdCanvases + 1}",
            };

            var canvas = new DoubleBufferedPanel()
            {
                Dock = DockStyle.Fill,
                Location = new Point(45, 24),
                TabIndex = 4,
                Name = $"canvas{createdCanvases}",
            };

            canvas.Paint += new PaintEventHandler(ViewPortPaint);
            canvas.MouseDown += new MouseEventHandler(ViewPortMouseDown);
            canvas.MouseMove += new MouseEventHandler(ViewPortMouseMove);
            canvas.MouseUp += new MouseEventHandler(ViewPortMouseUp);

            tabPage.Controls.Add(canvas);
            tabControl.Controls.Add(tabPage);

            createdCanvases++;
            PositionAddButton();
        }

        private void PositionAddButton()
        {
            if (tabControl.TabPages.Count > 0)
            {
                Rectangle lastTabRect = tabControl.GetTabRect(tabControl.TabPages.Count - 1);
                addButton.Location = new Point(lastTabRect.Right + toolMenu.Width + 2, tabControl.Top - 1);
            }
            else
            {
                addButton.Location = new Point(toolMenu.Right + 1, tabControl.Top - 1);
            }
            addButton.BringToFront();
        }

        private void LoadFileOnNewTabPage(string fullFileName)
        {
            TabPage tabPage = new TabPage()
            {
                Text = Path.GetFileName(fullFileName),
                Tag = fullFileName
            };

            var canvas = new DoubleBufferedPanel()
            {
                Dock = DockStyle.Fill,
                Location = new Point(45, 24),
                TabIndex = 4,
                Name = $"canvas{createdCanvases}",
            };

            canvas.Paint += new PaintEventHandler(ViewPortPaint);
            canvas.MouseDown += new MouseEventHandler(ViewPortMouseDown);
            canvas.MouseMove += new MouseEventHandler(ViewPortMouseMove);
            canvas.MouseUp += new MouseEventHandler(ViewPortMouseUp);
            canvas.DialogProcessor.ReadFile(fullFileName);

            tabPage.Controls.Add(canvas);
            tabControl.Controls.Add(tabPage);

            createdCanvases++;
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

                    //canvas.BackColor = Color.White;
                    coordinatesLabel.BackColor = Color.White;
                    label1.BackColor = Color.White;
                    selectedShapesCountLabel.BackColor = Color.White;
                    newShapeStrokeThicknessTextBox.ForeColor = ApplicationColors.SecondaryUIDark;
                    newShapeStrokeThicknessTextBox.BackColor = Color.White;

                    //Assign light color mode to main menu
                    mainMenu.ForeColor = ApplicationColors.MainUIDark;
                    mainMenu.BackColor = ApplicationColors.MainUILight;

                    //Assign light color mode to tool menu
                    toolMenu.ForeColor = ApplicationColors.MainUIDark;
                    toolMenu.BackColor = ApplicationColors.MainUILight;

                    //Change images to the opposite color
                    selectionToolButton.Image = Properties.Resources.PointerDark;
                    rectangleToolButton.Image = Properties.Resources.RectangleDark;
                    elipseToolButton.Image = Properties.Resources.ElipseDark;
                    editToolButton.Image = Properties.Resources.BrushDark;
                    bucketToolButton.Image = Properties.Resources.BucketDark;
                    groupToolButton.Image = Properties.Resources.GroupDark;
                    removeShapeToolButton.Image = Properties.Resources.BinDark;
                    dotToolButton.Image = Properties.Resources.DotDark;
                    triangleToolButton.Image = Properties.Resources.TriangleDark;
                    lineToolButton.Image = Properties.Resources.LineDark;
                    eraserToolButton.Image = Properties.Resources.EraserDark;

                    //Update toolStrip renderer
                    toolMenu.Renderer = new ApplicationToolStripRenderer(ApplicationColors.ToolStripMenuHoveredLight, ApplicationColors.ToolStripMenuCheckedLight);
                }
                else
                {
                    enableDisableDarkModeSettingsButton.Text = "Disable Dark Mode";
                    this.ForeColor = ApplicationColors.MainUILight;
                    this.BackColor = ApplicationColors.MainUIDark;

                    //canvas.BackColor = ApplicationColors.MainUIDark;
                    coordinatesLabel.BackColor = ApplicationColors.MainUIDark;
                    label1.BackColor = ApplicationColors.MainUIDark;
                    selectedShapesCountLabel.BackColor = ApplicationColors.MainUIDark;
                    newShapeStrokeThicknessTextBox.ForeColor = Color.White;
                    newShapeStrokeThicknessTextBox.BackColor = ApplicationColors.SecondaryUIDark;

                    //Assign dark color mode to main menu
                    mainMenu.ForeColor = ApplicationColors.MainUILight;
                    mainMenu.BackColor = ApplicationColors.SecondaryUIDark;

                    //Assign dark color mode to tool menu
                    toolMenu.ForeColor = ApplicationColors.MainUILight;
                    toolMenu.BackColor = ApplicationColors.SecondaryUIDark;

                    //Change images to the opposite color
                    selectionToolButton.Image = Properties.Resources.PointerLight;
                    rectangleToolButton.Image = Properties.Resources.RectangleLight;
                    elipseToolButton.Image = Properties.Resources.ElipseLight;
                    editToolButton.Image = Properties.Resources.BrushLight;
                    bucketToolButton.Image = Properties.Resources.BucketLight;
                    groupToolButton.Image = Properties.Resources.GroupLight;
                    removeShapeToolButton.Image = Properties.Resources.BinLight;
                    dotToolButton.Image = Properties.Resources.DotLight;
                    triangleToolButton.Image = Properties.Resources.TriangleLight;
                    lineToolButton.Image = Properties.Resources.LineLight;
                    eraserToolButton.Image = Properties.Resources.EraserLight;

                    //Update toolStrip renderer
                    toolMenu.Renderer = new ApplicationToolStripRenderer(ApplicationColors.ToolStripMenuHoveredDark, ApplicationColors.ToolStripMenuCheckedDark);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error has occured while changing window light/dark mode. Exception message:" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SaveShapesToPNG(string filePath)
        {
            //Get current Dialog processor and canvas
            var canvas = CurrentCanvas;

            // Create a Bitmap with the size of the User Control
            Bitmap bitmap = new Bitmap(canvas.Width, canvas.Height);

            // Create a Graphics object to draw on the Bitmap
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                foreach (var shape in CurrentDialogProcessor.ShapeList)
                {
                    shape.DrawSelf(graphics);
                }
            }

            // Save the Bitmap as a PNG file
            bitmap.Save(filePath, ImageFormat.Png);

            // Dispose of the resources of the Bitmap
            bitmap.Dispose();
        }

        private void GroupSelection()
        {
            try
            {
                if (CurrentDialogProcessor.Selections.Count <= 1)
                {
                    MessageBox.Show("Cannot use Group Tool. Select at least two shapes with the Selection Tool first.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    return;
                }

                CurrentDialogProcessor.GroupSelectedShapes();
                RedrawCanvas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error has occured during grouping of shapes. Exception message:" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void UngroupSelection()
        {
            try
            {
                if (CurrentDialogProcessor.Selections.OfType<GroupShape>().ToList().Count < 0)
                {
                    MessageBox.Show("Cannot ungroup because there is no group selected", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    return;
                }

                CurrentDialogProcessor.UngroupSelectedShape();
                RedrawCanvas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error has occured during ungrouping of shapes. Exception message:" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Handles the disposal of the preview shape draw and returns the form to its normal state
        /// </summary>
        private void DisposeShapePreview()
        {
            try
            {
                isDrawingPerformed = false;

                if (CurrentDialogProcessor.ShapeList.Contains(currentDrawnShape))
                    CurrentDialogProcessor.ShapeList.Remove(currentDrawnShape);

                previewShapeStartPoint = Point.Empty;
                currentDrawnShape = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpexted error has occured while disposing shape preview. Exception message: {ex.Message}.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Opens <see cref="ShapeEditorForm"/> and handles the edit of the shape and redrawing of canvas
        /// </summary>
        private void HandleEditShape()
        {
            try
            {
                var dialogProcessor = CurrentDialogProcessor;
                if (dialogProcessor.Selections == null || dialogProcessor.Selections.Count <= 0)
                {
                    MessageBox.Show("To open Edit Tool you have to select at least shape with the Selection Tool first.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (dialogProcessor.Selections.OfType<GroupShape>().Count() <= 0 && dialogProcessor.Selections.OfType<DotShape>().Count() <= 0)
                {
                    if (dialogProcessor.Selections.Count == 1)
                    {
                        var firstShape = dialogProcessor.Selections[0];
                        var dialog
                            = new ShapeEditorForm(firstShape.Width, firstShape.Height, firstShape.RotationAngle, firstShape.StrokeThickness, firstShape.StrokeColor, firstShape.FillColor);

                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            foreach (var shape in dialogProcessor.Selections)
                            {
                                shape.Width = dialog.ShapeWidth;
                                shape.Height = dialog.ShapeHeight;
                                shape.StrokeThickness = dialog.StrokeThickness;
                                shape.StrokeColor = dialog.StrokeColor;
                                shape.FillColor = dialog.FillColor;
                                shape.RotationAngle = dialog.RotationAngle;
                            }
                        }
                    }
                    else
                    {
                        var dialog = new ShapeEditorForm();

                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            foreach (var shape in dialogProcessor.Selections)
                            {
                                shape.Width = dialog.ShapeWidth;
                                shape.Height = dialog.ShapeHeight;
                                shape.StrokeThickness = dialog.StrokeThickness;
                                shape.StrokeColor = dialog.StrokeColor;
                                shape.FillColor = dialog.FillColor;
                                shape.RotationAngle = dialog.RotationAngle;
                            }
                        }
                    }
                }
                else
                {
                    var firstShape = dialogProcessor.Selections[0];
                    var dialog
                        = new ShapeEditorForm(firstShape.StrokeThickness, firstShape.StrokeColor, firstShape.FillColor);

                    if (dialog.ShowDialog() == DialogResult.OK)
                        UpdateMultipleShapes(dialogProcessor.Selections, dialog.StrokeColor, dialog.FillColor, dialog.StrokeThickness);
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

        private DialogProcessor GetCurrentDialogProcessor()
        {
            try
            {
                return GetCurrentCanvas().DialogProcessor;
            }
            catch
            {
                return null;
            }
        }
        private DoubleBufferedPanel GetCurrentCanvas()
        {
            try
            {
                return tabControl.SelectedTab.Controls.OfType<DoubleBufferedPanel>().First();
            }
            catch
            {
                return null;
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

        /// <summary>
        /// Invoked when canvas.Invalidate() is called.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewPortPaint(object sender, PaintEventArgs e)
        {
            CurrentDialogProcessor.ReDraw(sender, e);
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            CreateNewTabPage();
            tabControl.SelectedIndex = tabControl.Controls.Count - 1;
            PositionAddButton();
        }

        private void ViewPortMouseDown(object sender, MouseEventArgs e)
        {
            var dialogProcessor = CurrentDialogProcessor;

            // Do nothing if the pressed button is not the left mouse button
            if (e.Button != MouseButtons.Left)
                return;

            //Clear selections if selected shape is null
            IShape selectedShape = dialogProcessor.ContainsPoint(e.Location);
            if (selectedShape == null)
                ClearSelections();

            if (selectionToolButton.Checked)
            {
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
                        ClearSelections();

                        //Add the selected shape if there is any
                        dialogProcessor.Selections.Add(selectedShape);
                    }

                    //Indicate dragging and update last location in dialog processor
                    dialogProcessor.IsDragging = true;
                    dialogProcessor.LastLocation = e.Location;
                    RedrawCanvas();
                }
            }
            else if (bucketToolButton.Checked)
            {
                if (selectedShape != null)
                {
                    selectedShape.FillColor = SelectedColor;
                    RedrawCanvas();
                }
            }
            else if (lineToolButton.Checked)
            {
                previewShapeStartPoint = e.Location;
                currentDrawnShape = new LineShape(previewShapeStartPoint, previewShapeStartPoint, Color.Transparent, StrokeThickness, true);
                currentDrawnShape.StrokeColor = Color.LightGray;
                dialogProcessor.ShapeList.Add(currentDrawnShape);
                isDrawingPerformed = true;
                RedrawCanvas();
            }
            else if (rectangleToolButton.Checked)
            {
                previewShapeStartPoint = e.Location;
                currentDrawnShape = new RectangleShape(new RectangleF(previewShapeStartPoint.X, previewShapeStartPoint.Y, 0, 0), Color.Transparent, StrokeThickness);
                currentDrawnShape.StrokeColor = Color.LightGray;
                dialogProcessor.ShapeList.Add(currentDrawnShape);
                isDrawingPerformed = true;
                RedrawCanvas();
            }
            else if (elipseToolButton.Checked)
            {
                previewShapeStartPoint = e.Location;
                currentDrawnShape = new EllipseShape(new RectangleF(previewShapeStartPoint.X, previewShapeStartPoint.Y, 0, 0), Color.Transparent, StrokeThickness);
                currentDrawnShape.StrokeColor = Color.LightGray;
                dialogProcessor.ShapeList.Add(currentDrawnShape);
                isDrawingPerformed = true;
                RedrawCanvas();
            }
            else if (triangleToolButton.Checked)
            {
                previewShapeStartPoint = e.Location;
                currentDrawnShape = new TriangleShape(new RectangleF(previewShapeStartPoint.X, previewShapeStartPoint.Y, 0, 0), Color.Transparent, StrokeThickness);
                currentDrawnShape.StrokeColor = Color.LightGray;
                dialogProcessor.ShapeList.Add(currentDrawnShape);
                isDrawingPerformed = true;
                RedrawCanvas();
            }
            else if (eraserToolButton.Checked)
            {
                dialogProcessor.EraseShapes(e.Location);
                RedrawCanvas();
            }
        }

        /// <summary>
        /// Invoked when the mouse moves on top of the canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewPortMouseMove(object sender, MouseEventArgs e)
        {
            //Update coordinates
            coordinatesLabel.Text = string.Format("{0}, {1}", e.Location.X, e.Location.Y);

            if (eraserToolButton.Checked && e.Button == MouseButtons.Left)
            {
                CurrentDialogProcessor.EraseShapes(e.Location);
                RedrawCanvas();
                return;
            }

            // Handle dragging
            if (CurrentDialogProcessor.IsDragging)
            {
                HandleDragging(e);
                return;
            }

            if (isDrawingPerformed)
            {
                //Update the drawn shape
                currentDrawnShape.Rectangle = ShapeUtility.CalculateRectangle(previewShapeStartPoint, e.Location);
                RedrawCanvas();
                return;
            }
        }

        private void ViewPortMouseUp(object sender, MouseEventArgs e)
        {
            var dialogProcessor = CurrentDialogProcessor;
            var endPoint = e.Location;

            if (selectionToolButton.Checked)
            {
                dialogProcessor.IsDragging = false;
            }
            else if (lineToolButton.Checked)
            {
                dialogProcessor.DrawLineShape(previewShapeStartPoint, endPoint, SelectedColor, StrokeThickness);
                DisposeShapePreview();
                RedrawCanvas();
            }
            else if (rectangleToolButton.Checked)
            {
                dialogProcessor.DrawRectangleShape(previewShapeStartPoint, endPoint, SelectedColor, StrokeThickness);
                DisposeShapePreview();
                RedrawCanvas();
            }
            else if (elipseToolButton.Checked)
            {
                dialogProcessor.DrawEllipseShape(previewShapeStartPoint, endPoint, SelectedColor, StrokeThickness);
                DisposeShapePreview();
                RedrawCanvas();
            }
            else if (triangleToolButton.Checked)
            {
                dialogProcessor.DrawTriangleShape(previewShapeStartPoint, endPoint, SelectedColor, StrokeThickness);
                DisposeShapePreview();
                RedrawCanvas();
            }
            else if (dotToolButton.Checked)
            {
                dialogProcessor.DrawDotShape(endPoint, SelectedColor);
                DisposeShapePreview();
                RedrawCanvas();
            }
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
                CurrentCanvas.Cursor = Cursors.Default;
            else
                CurrentCanvas.Cursor = Cursors.Cross;
        }

        private void editToolButton_Click(object sender, EventArgs e)
        {
            HandleEditShape();
        }

        private void UpdateMultipleShapes(List<IShape> shapes, Color strokeColor, Color fillColor, float strokeThickness)
        {
            foreach (var shape in shapes)
            {
                shape.StrokeColor = strokeColor;
                shape.FillColor = fillColor;
                shape.StrokeThickness = strokeThickness;
                if (shape is GroupShape)
                {
                    var group = shape as GroupShape;
                    UpdateMultipleShapes(group.SubShapes, strokeColor, fillColor, strokeThickness);
                }
            }
        }

        private void groupTool_Click(object sender, EventArgs e)
        {
            try
            {
                var dialogProcessor = CurrentDialogProcessor;

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
            CreateNewTabPage();
            tabControl.SelectedIndex = tabControl.Controls.Count - 1;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                InitialDirectory = Environment.CurrentDirectory,
                Title = "Open",
                DefaultExt = "vdfile",
                Filter = "File|*.vdfile",
                CheckFileExists = true,
                CheckPathExists = true,
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LoadFileOnNewTabPage(dialog.FileName);
                tabControl.SelectedIndex = tabControl.Controls.Count - 1;
            }
            PositionAddButton();
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
                    CurrentDialogProcessor.ClearShapes();
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
            UngroupSelection();
        }

        private void groupSelectionMenuButton_Click(object sender, EventArgs e)
        {
            GroupSelection();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            var dialogProcessor = CurrentDialogProcessor;

            // Check if Ctrl and S keys are pressed
            if (e.Control && e.KeyCode == Keys.S)
            {
                HandleSaveFile();
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                HandleCopyShape();
            }
            else if (e.Control && e.KeyCode == Keys.X)
            {
                HandleCutShape();
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                if (dialogProcessor.CoppiedSelection.Count > 0)
                    HandlePasteShape();
            }
            else if (e.Control && e.KeyCode == Keys.G)
            {
                GroupSelection();
            }
            else if (e.Control && e.KeyCode == Keys.U)
            {
                UngroupSelection();
            }
            else if (e.Control && e.KeyCode == Keys.E)
            {
                HandleEditShape();
            }
            else if (!e.Control && e.KeyCode == Keys.Delete)
            {
                HandleDeleteShape();
            }
            else if (e.Control && e.KeyCode == Keys.Delete)
            {
                if (EnsureUnsavedWorkIsNotLost())
                {
                    tabControl.TabPages.Remove(SelectedTab);
                    PositionAddButton();
                }
            }
            else if (e.Control && e.KeyCode == Keys.Up)
            {
                if (dialogProcessor.Selections.Count == 1)
                {
                    dialogProcessor.BringShapeOneLayerUp(dialogProcessor.Selections[0]);
                    RedrawCanvas();
                }
            }
            else if (e.Control && e.KeyCode == Keys.Down)
            {
                if (dialogProcessor.Selections.Count == 1)
                {
                    dialogProcessor.BringShapeOneLayerDown(dialogProcessor.Selections[0]);
                    RedrawCanvas();
                }
            }
        }

        private void deleteSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HandleDeleteShape();
        }

        private void colorPicker_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                SelectedColor = colorDialog.Color;
            }
        }

        private void newShapeStrokeThicknessTextBox_MouseEnter(object sender, EventArgs e)
        {
            var textBox = (sender as TextBox);
            if (textBox != null)
            {
                toolTip.Show("Stroke Thickness", textBox);
            }
        }

        private void colorPicker_MouseEnter(object sender, EventArgs e)
        {
            var button = (sender as Button);
            if (button != null)
            {
                toolTip.Show("Color Picker", button);
            }
        }

        private void newShapeStrokeThicknessTextBox_TextChanged(object sender, EventArgs e)
        {
            var textBox = (sender as TextBox);
            if (textBox != null && textBox.Text != string.Empty)
            {
                if (!int.TryParse(textBox.Text, out _))
                {
                    MessageBox.Show("Please enter a valid numeric value!", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    textBox.Text = Regex.Replace(textBox.Text, "[^0-9]", "");
                }

                if (textBox.Text.Length >= 3)
                    textBox.Text = textBox.Text.Substring(0, textBox.Text.Length - 1);

            }
            textBox.SelectionStart = textBox.TextLength;
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HandleCutShape();
        }

        private void moveShapeLayerUpCtrlUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialogProcessor = CurrentDialogProcessor;

            if (dialogProcessor.Selections.Count == 1)
            {
                dialogProcessor.BringShapeOneLayerUp(dialogProcessor.Selections[0]);
                RedrawCanvas();
            }
        }

        private void moveLayerDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialogProcessor = CurrentDialogProcessor;

            if (dialogProcessor.Selections.Count == 1)
            {
                dialogProcessor.BringShapeOneLayerDown(dialogProcessor.Selections[0]);
                RedrawCanvas();
            }
        }

        private void closeTabCtrlDelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EnsureUnsavedWorkIsNotLost())
            {
                tabControl.TabPages.Remove(SelectedTab);
                PositionAddButton();
            }
        }
        #endregion

        #region UI Helper Event Handlers (Enable/Disable controls)
        private void groupMenuButton_DropDownOpened(object sender, EventArgs e)
        {
            try
            {
                var dialogProcessor = CurrentDialogProcessor;

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

        private void editSelectionCtrlEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HandleEditShape();
        }

        private void editToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            if (tabControl.TabPages.Count > 0)
                closeTabToolStripMenuItem.Enabled = true;
            else
                closeTabToolStripMenuItem.Enabled = false;

            var dialogProcessor = CurrentDialogProcessor;
            if (dialogProcessor != null)
            {
                //Handle Edit and Delete
                if (dialogProcessor.Selections.Count > 0)
                {
                    cutToolStripMenuItem.Enabled = true;
                    copyToolStripMenuItem.Enabled = true;
                    editSelectionToolStripMenuItem.Enabled = true;
                    deleteSelectionToolStripMenuItem.Enabled = true;
                }
                else
                {
                    cutToolStripMenuItem.Enabled = false;
                    copyToolStripMenuItem.Enabled = false;
                    editSelectionToolStripMenuItem.Enabled = false;
                    deleteSelectionToolStripMenuItem.Enabled = false;
                }

                if (dialogProcessor.Selections.Count == 1)
                {
                    moveShapeLayerUpToolStripMenuItem.Enabled = true;
                    moveLayerDownToolStripMenuItem.Enabled = true;
                }
                else
                {
                    moveShapeLayerUpToolStripMenuItem.Enabled = false;
                    moveLayerDownToolStripMenuItem.Enabled = false;
                }

                if (dialogProcessor.CoppiedSelection.Count > 0)
                    pasteToolStripMenuItem.Enabled = true;
                else
                    pasteToolStripMenuItem.Enabled = false;
            }
            else
            {
                cutToolStripMenuItem.Enabled = false;
                copyToolStripMenuItem.Enabled = false;
                clearCanvasToolStripMenuItem.Enabled = false;
                editSelectionToolStripMenuItem.Enabled = false;
                deleteSelectionToolStripMenuItem.Enabled = false;
                moveShapeLayerUpToolStripMenuItem.Enabled = false;
                moveLayerDownToolStripMenuItem.Enabled = false;
                pasteToolStripMenuItem.Enabled = false;
            }
        }
        #endregion
    }

    internal enum UIMode
    {
        Light,
        Dark
    }
}
