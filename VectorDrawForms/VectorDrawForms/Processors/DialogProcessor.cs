using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using VectorDrawForms.Models;

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
        public List<IShape> Selections
        {
            get { return selections; }
            private set { selections = value; }
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
            //Unhandled exception
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

        public void ReadFile(string path)
        {
            try
            {
                FileStream stream = new FileStream(path, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                ShapeList = (List<IShape>)formatter.Deserialize(stream);

                //Dispose fileStream
                stream.Flush();
                stream.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error has occured while reading the file. Exception message:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// Clears all shapes from <see cref="DialogProcessor"/>.
        /// </summary>
        public void PrepareForCleenSheet()
        {
            selections.Clear();
            ShapeList.Clear();
        }

        /// <summary>
        /// Deletes the selected primitives
        /// </summary>
        public void DeleteSelection()
        {
            ShapeList = ShapeList.Except(selections).ToList();
            selections.Clear();
        }

        /// <summary>
        /// Clears the shapes from the <see cref="DialogProcessor"/>.
        /// </summary>
        public void ClearShapes()
        {
            ShapeList.Clear();
            selections.Clear();
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

            //Remove the selected shapes from shape list
            ShapeList = ShapeList.Except(selections).ToList();

            //add the group to the list of primitives
            ShapeList.Add(group);

            //Clear selected primitives
            selections.Clear();

            //add the group
            selections.Add(group);
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
                var group = (GroupShape)selections[0];
                ShapeList.AddRange(group.SubShapes);
                ShapeList.Remove(group);
                selections.Clear();
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


            if (coppiedSelection.Count == 1)
            {
                IShape shape = coppiedSelection[0].DeepClone();
                shape.Location = new PointF(0, 0);
                ShapeList.Add(shape);
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
            }

            //Clear selections and coppied selections
            selections.Clear();
        }

        public void CopySelection()
        {
            //Clear copy buffer
            coppiedSelection.Clear();

            //Add selections to copy buffer
            coppiedSelection.AddRange(selections);
        }

        /// <summary>
        /// Draws <see cref="RectangleShape"/> based on the given startPoint and endPoint, 
        /// clears the selections and selects the nelwy created rectangle.
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        public IShape DrawRectangleShape(PointF startPoint, PointF endPoint, Color fillColor, int strokeThickness)
        {
            RectangleShape rect = new RectangleShape(ShapeUtility.CalculateRectangle(startPoint, endPoint), fillColor, strokeThickness);
            ShapeList.Add(rect);
            selections.Clear();
            selections.Add(rect);
            return rect;
        }

        /// <summary>
        /// Draws <see cref="LineShape"/> based on the given startPoint and endPoint, 
        /// clears the selections and selects the nelwy created rectangle.
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        public IShape DrawLineShape(PointF startPoint, PointF endPoint, Color fillColor, int strokeThickness)
        {
            LineShape line = new LineShape(startPoint, endPoint, fillColor, strokeThickness);
            ShapeList.Add(line);
            selections.Clear();
            selections.Add(line);
            return line;
        }

        /// <summary>
        /// Draws <see cref="TriangleShape"/> based on the given startPoint and endPoint, 
        /// clears the selections and selects the nelwy created rectangle.
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        public IShape DrawTriangleShape(PointF startPoint, PointF endPoint, Color fillColor, int strokeThickness)
        {
            TriangleShape triangle = new TriangleShape(ShapeUtility.CalculateRectangle(startPoint, endPoint), fillColor, strokeThickness);
            ShapeList.Add(triangle);
            selections.Clear();
            selections.Add(triangle);
            return triangle;
        }

        /// <summary>
        /// Draws <see cref="EllipseShape"/> based on the given startPoint and endPoint, 
        /// clears the selections and selects the newly created rectangle.
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        public IShape DrawEllipseShape(PointF startPoint, PointF endPoint, Color fillColor, int strokeThickness)
        {
            EllipseShape elipse = new EllipseShape(ShapeUtility.CalculateRectangle(startPoint, endPoint), fillColor, strokeThickness);
            ShapeList.Add(elipse);
            selections.Clear();
            selections.Add(elipse);
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
            DotShape dot = new DotShape(point.X, point.Y, color);
            ShapeList.Add(dot);
            selections.Clear();
            selections.Add(dot);
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
        #endregion
    }
}
