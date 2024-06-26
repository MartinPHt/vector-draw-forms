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
        /// Checks if a point is in one of the selected <see cref="Shape"/>. Finds the "top" element ie. the one we see under the mouse. 
        /// </summary>
        /// <param name="point">Indicated point</param>
        /// <returns>The shape element to which the given point belongs.</returns>
        public IShape ContainsPointInResizeRectanges(PointF point)
        {
            for (int i = Selections.Count - 1; i >= 0; i--)
            {
                if (Selections[i].ContainsInResizeRectangles(point))
                {
                    return Selections[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Checks if a point is in one of the selected <see cref="Shape"/>. Finds the "top" element ie. the one we see under the mouse. 
        /// </summary>
        /// <param name="point">Indicated point</param>
        /// <returns>The shape element to which the given point belongs.</returns>
        public IShape ContainsPointInResizeRectanges(PointF point, out ResizeRectangle resizeRectangleUsed)
        {
            for (int i = Selections.Count - 1; i >= 0; i--)
            {
                if (Selections[i].ContainsInResizeRectangles(point, out resizeRectangleUsed))
                {
                    return Selections[i];
                }
            }
            resizeRectangleUsed = ResizeRectangle.None;
            return null;
        }

        /// <summary>
        /// Translation of the selected element to a vector defined by the passed <see cref="PointF"/> p parameter.
        /// </summary>
        /// <param name="p">Translation vector.</param>
        public void TranslateTo(PointF p)
        {
            //Move eacch selected shape
            foreach (IShape shape in Selections)
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
        /// Calculates the height which is needed for the population of shapes.
        /// </summary>
        /// <returns></returns>
        public int CalculatePopulatedHeight()
        {
            float height = 0; //y
            foreach (var shape in ShapeList)
            {
                if (shape.Rectangle.Bottom > height)
                {
                    height = shape.Rectangle.Bottom;
                }
            }
            return (int)Math.Round(height);
        }

        /// <summary>
        /// Calculates the width which is needed for the population of shapes.
        /// </summary>
        /// <returns></returns>
        public int CalculatePopulatedWidth()
        {
            float width = 0; //x
            foreach (var shape in ShapeList)
            {
                if (shape.Rectangle.Right > width)
                {
                    width = shape.Rectangle.Right;
                }
            }
            return (int)Math.Round(width);
        }

        /// <summary>
        /// Reads vdfile, deserializes its content into <see cref="IShape"/> objects and populates current instance of <see cref="DialogProcessor"/>.
        /// </summary>
        /// <param name="path"></param>
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
            foreach (var shape in Selections)
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
        /// Groups the selected shapes from the canvas.
        /// </summary>
        public void GroupSelectedShapes()
        {
            // Calculate Group Rectangle
            RectangleF rect = CalculateGroupRectangle(Selections);

            //Create new Group shape from the selected shapes
            GroupShape group = new GroupShape(rect);

            //Preserve layers in the group
            for (int i = 0; i < Selections.Count; i++)
            {
                for (int j = 1; j < Selections.Count; j++)
                {
                    if (ShapeList.IndexOf(Selections[i]) > ShapeList.IndexOf(Selections[j]))
                    {
                        var temp = Selections[i];
                        Selections[i] = Selections[j];
                        Selections[j] = temp;
                    }
                }
            }

            //add the shapes from the current selection
            group.SubShapes.AddRange(Selections);

            //Remove the selected shapes from shape list
            ShapeList = ShapeList.Except(Selections).ToList();

            //Clear selected primitives
            ClearSelection();

            //add the group to the list of primitives
            ShapeList.Add(group);

            //add the group
            AddSelection(group);
        }

        private RectangleF CalculateGroupRectangle(IEnumerable<IShape> shapes)
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
            if (Selections.Count == 1 && GetSelectedShapeAt(0) is GroupShape)
            {
                var group = (GroupShape)GetSelectedShapeAt(0);
                ShapeList.AddRange(group.SubShapes);
                ShapeList.Remove(group);
                ClearSelection();
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


            //Clear selections and coppied selections
            ClearSelection();

            //Add selection
            AddSelectionRange(newShapes);
        }

        public void CopySelection()
        {
            //Clear copy buffer
            coppiedSelection.Clear();

            //Add selections to copy buffer
            coppiedSelection.AddRange(Selections);
        }

        public void CutSelection()
        {
            //Clear copy buffer
            coppiedSelection.Clear();

            //Add selections to copy buffer
            coppiedSelection.AddRange(Selections);

            //Remove cut shapes
            ShapeList = ShapeList.Except(Selections).ToList();
        }

        /// <summary>
        /// Draws <see cref="RectangleShape"/> based on the given startPoint and endPoint, 
        /// clears the selections and selects the nelwy created rectangle.
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        public IShape DrawRectangleShape(PointF startPoint, PointF endPoint, Color strokeColor, float strokeThickness)
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
        public IShape DrawLineShape(PointF startPoint, PointF endPoint, Color strokeColor, float strokeThickness)
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
        public IShape DrawTriangleShape(PointF startPoint, PointF endPoint, Color strokeColor, float strokeThickness)
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
        public IShape DrawEllipseShape(PointF startPoint, PointF endPoint, Color strokeColor, float strokeThickness)
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
