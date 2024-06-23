using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using VectorDrawForms.Models;
using static VectorDrawForms.Models.Shape;

namespace VectorDrawForms.Processors
{
    public class DialogProcessor : DisplayProcessor, IDialogProcessor
    {
        #region Constructor

        public DialogProcessor()
        {
        }

        #endregion

        #region Properties

        private List<IShape> selections = new List<IShape>();
        /// <summary>
        /// Selected element
        /// </summary>
        public IReadOnlyCollection<IShape> Selections
        {
            get { return selections; }
        }

        private List<IShape> coppiedSelection = new List<IShape>();
        public List<IShape> CoppiedSelection
        {
            get { return coppiedSelection; }
            private set { coppiedSelection = value; }
        }


        private bool isDragging;
        /// <summary>
        /// Returns true if the dialog is currently in the "drag" state of the selected item.
        /// </summary>
        public bool IsDragging
        {
            get { return isDragging; }
            set { isDragging = value; }
        }

        private PointF lastLocation;
        /// <summary>
        /// Last position of the mouse when "drag". Used for defining the translation vector.
        /// </summary>
        public PointF LastLocation
        {
            get { return lastLocation; }
            set { lastLocation = value; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Checks if a point is in the element. Finds the "top" element ie. the one we see under the mouse.
        /// </summary>
        /// <param name="point">Indicated point</param>
        /// <returns>The shape element to which the given point belongs.</returns>
        public IShape ContainsPoint(PointF point)
        {
            for (int i = ShapeList.Count - 1; i >= 0; i--)
            {
                if (ShapeList[i].Contains(point))
                {
                    return ShapeList[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Translation of the selected element to a vector defined by the passed <see cref="PointF"/> p parameter.
        /// </summary>
        /// <param name="p">Translation vector.</param>
        public void TranslateTo(PointF p)
        {
            //Move eacch selected shape
            foreach (IShape shape in selections)
                MoveShape(shape, p);

            lastLocation = p;
        }

        /// <summary>
        /// Moves the <see cref="IShape"/> to a provided <see cref="PointF"/>
        /// </summary>
        /// <param name="shape"></param>
        private void MoveShape(IShape shape, PointF p)
        {
            if (shape is GroupShape)
            {
                var group = (GroupShape)shape;
                foreach (var subShape in group.SubShapes)
                {
                    if (subShape is GroupShape)
                        MoveShape(subShape, p);

                    subShape.Location = new PointF(subShape.Location.X + p.X - lastLocation.X, subShape.Location.Y + p.Y - lastLocation.Y);
                }

            }
            shape.Location = new PointF(shape.Location.X + p.X - lastLocation.X, shape.Location.Y + p.Y - lastLocation.Y);
        }

        /// <summary>
        /// Clears all selections from the current <see cref="DialogProcessor"/>.
        /// </summary>
        public void ClearSelection()
        {
            RemoveSelectionRange(selections);
        }

        /// <summary>
        /// Gets the shape at the given index
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public IShape GetSelectedShapeAt(int index)
        {
            return selections[index];
        }

        /// <summary>
        /// Removes the provided shape to the selections only if it is contained in the ShapesList of current <see cref="DialogProcessor"/>.
        /// </summary>
        /// <param name="shape"></param>
        public void RemoveSelection(IShape shape)
        {
            if (ShapeList.Contains(shape))
            {
                shape.IsSelected = false;
                selections.Remove(shape);
            }
        }

        /// <summary>
        /// Removes the provided shapes to the selections only if they are contained in the ShapesList of current <see cref="DialogProcessor"/>.
        /// </summary>
        /// <param name="shape"></param>
        public void RemoveSelectionRange(IEnumerable<IShape> shapes)
        {
            foreach (var shape in shapes)
            {
                if (ShapeList.Contains(shape))
                    shape.IsSelected = false;
            }
            selections = selections.Except(shapes).ToList();
        }

        /// <summary>
        /// Adds the provided shape to the selections only if it is contained in the ShapesList of current <see cref="DialogProcessor"/>.
        /// </summary>
        /// <param name="shape"></param>
        public void AddSelection(IShape shape)
        {
            if (ShapeList.Contains(shape))
            {
                shape.IsSelected = true;
                selections.Add(shape);
            }
        }

        /// <summary>
        /// Adds the provided shapes to the selections only if they are contained in the ShapesList of current <see cref="DialogProcessor"/>.
        /// </summary>
        /// <param name="shape"></param>
        public void AddSelectionRange(IEnumerable<IShape> shapes)
        {
            foreach (var shape in shapes)
            {
                if (ShapeList.Contains(shape))
                    shape.IsSelected = true;
            }
            selections.AddRange(shapes);
        }

        public void ReadFile(string path)
        {
            try
            {
                FileStream stream = new FileStream(path, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                ShapeList = (List<IShape>)formatter.Deserialize(stream);

                stream.Flush();
                stream.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error has occured while reading the file. Exception message:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void DeleteShape(IShape shape)
        {
            RemoveSelection(shape);
            ShapeList.Remove(shape);
        }

        public void MoveSelectedShapes(int pixels, MoveDirection direction)
        {
            foreach (var shape in selections)
            {
                var rectangle = shape.Rectangle;

                if (direction == MoveDirection.Up)
                    shape.Rectangle = new RectangleF(rectangle.X, rectangle.Y - pixels, rectangle.Width, rectangle.Height);
                else if (direction == MoveDirection.Down)
                    shape.Rectangle = new RectangleF(rectangle.X, rectangle.Y + pixels, rectangle.Width, rectangle.Height);
                else if (direction == MoveDirection.Right)
                    shape.Rectangle = new RectangleF(rectangle.X + pixels, rectangle.Y, rectangle.Width, rectangle.Height);
                else
                    shape.Rectangle = new RectangleF(rectangle.X - pixels, rectangle.Y, rectangle.Width, rectangle.Height);
            }
        }

        /// <summary>
        /// Clears all shapes from <see cref="DialogProcessor"/>.
        /// </summary>
        public void PrepareForCleenSheet()
        {
            ClearSelection();
            ShapeList.Clear();
        }

        /// <summary>
        /// Deletes the selected primitives
        /// </summary>
        public void DeleteSelection()
        {
            ClearSelection();
            ShapeList = ShapeList.Except(selections).ToList();
        }

        /// <summary>
        /// Clears the shapes from the <see cref="DialogProcessor"/>.
        /// </summary>
        public void ClearShapes()
        {
            ClearSelection();
            ShapeList.Clear();
        }

        /// <summary>
        /// Groups the selected shapes from the canvas.
        /// </summary>
        public void GroupSelectedShapes()
        {
            // Calculate Group Rectangle
            RectangleF rect = CalculateGroupRectangle(selections);

            //Create new Group shape from the selected shapes
            GroupShape group = new GroupShape(rect);

            //add the shapes from the current selection 
            group.SubShapes.AddRange(selections);

            //Clear selected primitives
            ClearSelection();

            //Remove the selected shapes from shape list
            ShapeList = ShapeList.Except(selections).ToList();

            //add the group to the list of primitives
            ShapeList.Add(group);

            //add the group
            AddSelection(group);
        }

        private RectangleF CalculateGroupRectangle(List<IShape> shapes)
        {
            //starting point
            float x = float.MaxValue;
            float y = float.MaxValue;

            //end point
            float x1 = float.MinValue;
            float y1 = float.MinValue;
            foreach (IShape shape in shapes)
            {
                if (x > shape.Rectangle.X)
                    x = shape.Rectangle.X;

                if (y > shape.Rectangle.Y)
                    y = shape.Rectangle.Y;

                if (x1 < shape.Rectangle.Right)
                    x1 = shape.Rectangle.Right;

                if (y1 < shape.Rectangle.Bottom)
                    y1 = shape.Rectangle.Bottom;
            }

            //calculate rectangle width
            float width = Math.Abs(x - x1);

            //calculate rectangle height
            float height = Math.Abs(y - y1);
            return new RectangleF(x, y, width, height);
        }

        /// <summary>
        /// Ungroups selected group and deselects all controls.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void UngroupSelectedShape()
        {
            if (selections.Count == 1 && selections[0] is GroupShape)
            {
                ClearSelection();
                var group = (GroupShape)selections[0];
                ShapeList.AddRange(group.SubShapes);
                ShapeList.Remove(group);
            }
            else
            {
                throw new Exception("Cannot ungroup when more than one shape is selected");
            }
        }

        public void PasteSelection()
        {
            if (coppiedSelection.Count <= 0)
                return;

            //Clear selections and coppied selections
            ClearSelection();

            List<IShape> newShapes = new List<IShape>();
            if (coppiedSelection.Count == 1)
            {
                IShape shape = coppiedSelection[0].DeepClone();
                shape.Location = new PointF(0, 0);

                ShapeList.Add(shape);
                newShapes.Add(shape);
            }
            else
            {
                // Calculate Group Rectangle
                RectangleF rect = CalculateGroupRectangle(coppiedSelection);

                //Create new Group shape from the selected shapes
                GroupShape group = new GroupShape(rect);

                //add the shapes from the current selection 
                foreach (var shape in coppiedSelection)
                    group.SubShapes.Add(shape.DeepClone());

                MoveShape(group, new PointF(group.Rectangle.X - 10, group.Rectangle.Y - 10));

                //Add to shape list
                ShapeList.AddRange(group.SubShapes);
                newShapes.AddRange(group.SubShapes);
            }

            AddSelectionRange(newShapes);
        }

        public void CopySelection()
        {
            //Clear copy buffer
            coppiedSelection.Clear();

            //Add selections to copy buffer
            coppiedSelection.AddRange(selections);
        }

        public void CutSelection()
        {
            //Clear copy buffer
            coppiedSelection.Clear();

            //Add selections to copy buffer
            coppiedSelection.AddRange(selections);

            //Remove cut shapes
            ShapeList = ShapeList.Except(selections).ToList();
        }

        /// <summary>
        /// Draws <see cref="RectangleShape"/> based on the given startPoint and endPoint, 
        /// clears the selections and selects the nelwy created rectangle.
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        public IShape DrawRectangleShape(PointF startPoint, PointF endPoint, Color strokeColor, int strokeThickness)
        {
            ClearSelection();
            RectangleShape rect = new RectangleShape(ShapeUtility.CalculateRectangle(startPoint, endPoint), strokeColor, strokeThickness);
            ShapeList.Add(rect);
            AddSelection(rect);
            return rect;
        }

        /// <summary>
        /// Draws <see cref="LineShape"/> based on the given startPoint and endPoint, 
        /// clears the selections and selects the nelwy created rectangle.
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        public IShape DrawLineShape(PointF startPoint, PointF endPoint, Color strokeColor, int strokeThickness)
        {
            ClearSelection();
            LineShape line = new LineShape(startPoint, endPoint, strokeColor, strokeThickness);
            ShapeList.Add(line);
            AddSelection(line);
            return line;
        }

        /// <summary>
        /// Draws <see cref="TriangleShape"/> based on the given startPoint and endPoint, 
        /// clears the selections and selects the nelwy created rectangle.
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        public IShape DrawTriangleShape(PointF startPoint, PointF endPoint, Color strokeColor, int strokeThickness)
        {
            ClearSelection();
            TriangleShape triangle = new TriangleShape(ShapeUtility.CalculateRectangle(startPoint, endPoint), strokeColor, strokeThickness);
            ShapeList.Add(triangle);
            AddSelection(triangle);
            return triangle;
        }

        /// <summary>
        /// Draws <see cref="EllipseShape"/> based on the given startPoint and endPoint, 
        /// clears the selections and selects the newly created rectangle.
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        public IShape DrawEllipseShape(PointF startPoint, PointF endPoint, Color strokeColor, int strokeThickness)
        {
            ClearSelection();
            EllipseShape elipse = new EllipseShape(ShapeUtility.CalculateRectangle(startPoint, endPoint), strokeColor, strokeThickness);
            ShapeList.Add(elipse);
            AddSelection(elipse);
            return elipse;
        }

        /// <summary>
        /// Draws <see cref="DotShape"/> on the given point, 
        /// clears the selections and selects the newly created rectangle.
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        public IShape DrawDotShape(PointF point, Color color)
        {
            ClearSelection();
            DotShape dot = new DotShape(point.X, point.Y, color);
            ShapeList.Add(dot);
            AddSelection(dot);
            return dot;
        }

        /// <summary>
        /// Erases all shapes that are located on the given point.
        /// </summary>
        /// <param name="location"></param>
        public void EraseShapes(Point point)
        {
            try
            {
                while (true)
                {
                    var shape = ContainsPoint(point);
                    if (shape == null)
                        break;

                    ShapeList.Remove(shape);
                }
            }
            catch { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shape"></param>
        /// <exception cref="ArgumentException"></exception>
        public void BringShapeOneLayerUp(IShape shape)
        {
            try
            {
                if (ShapeList.Contains(shape) && ShapeList.Count > 1)
                {
                    int index = ShapeList.IndexOf(shape) + 1;
                    ShapeList.Remove(shape);

                    if (index <= ShapeList.Count)
                        ShapeList.Insert(index, shape);
                    else
                        ShapeList.Add(shape);
                }
                else
                    throw new ArgumentException("The provided shape does not exist in the Shape list of the Dialog Processor.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error has occured while bringing the layer up. Exception message:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shape"></param>
        /// <exception cref="ArgumentException"></exception>
        public void BringShapeOneLayerDown(IShape shape)
        {
            try
            {
                if (ShapeList.Contains(shape) && ShapeList.Count > 1)
                {
                    int index = ShapeList.IndexOf(shape) - 1;
                    ShapeList.Remove(shape);

                    if (index >= 0)
                        ShapeList.Insert(index, shape);
                    else
                        ShapeList.Insert(0, shape);
                }
                else
                    throw new ArgumentException("The provided shape does not exist in the Shape list of the Dialog Processor.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error has occured while bringing the layer down. Exception message:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}
