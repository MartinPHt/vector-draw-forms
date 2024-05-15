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
            set { selections = value; }
        }

        private List<IShape> coppiedSelection = new List<IShape>();
        public List<IShape> CoppiedSelection
        {
            get { return coppiedSelection; }
            set { coppiedSelection = value; }
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
        private Rectangle GenerateRandomRectangleForShape(
            int maxHeight = 200,
            int maxWidth = 200,
            int minHeight = 50,
            int minWidth = 50)
        {
            Random rnd = new Random();

            //size
            int height = rnd.Next(minHeight, maxHeight);
            int width = rnd.Next(minWidth, maxWidth);

            //starting point
            int x = rnd.Next(0, MainForm.Instance.Size.Width - width);
            int y = rnd.Next(0, MainForm.Instance.Size.Height - height);

            //return new Rectangle
            return new Rectangle(x, y, width, height);
        }

        /// <summary>
        /// Adds a rectangle primitive to an arbitrary location on the client area.
        /// </summary>
        public void AddRandomRectangle()
        {
            RectangleShape rect = new RectangleShape(GenerateRandomRectangleForShape());
            rect.FillColor = Color.Transparent;
            ShapeList.Add(rect);
        }

        /// <summary>
        /// Adds an elipse primitive to an arbitrary location on the client area.
        /// </summary>
        public void AddRandomElipse()
        {
            EllipseShape elipse = new EllipseShape(GenerateRandomRectangleForShape());
            elipse.FillColor = Color.Transparent;
            ShapeList.Add(elipse);
        }

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
                shape.Rectangle = new RectangleF(0, 0, shape.Rectangle.Width, shape.Rectangle.Height);
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

                //Add to shape list
                ShapeList.Add(group);
            }

            //Clear selections and coppied selections
            selections.Clear();
            coppiedSelection.Clear();
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
        /// clears the selections and selects the nely created rectangle.
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        public IShape DrawRectangleShape(PointF startPoint, PointF endPoint)
        {
            RectangleShape rect = new RectangleShape(Utilities.CalculateRectangle(startPoint, endPoint));
            rect.FillColor = Color.Transparent;
            ShapeList.Add(rect);
            selections.Clear();
            selections.Add(rect);
            return rect;
        }

        /// <summary>
        /// Draws <see cref="EllipseShape"/> based on the given startPoint and endPoint, 
        /// clears the selections and selects the nely created rectangle.
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        public IShape DrawEllipseShape(PointF startPoint, PointF endPoint)
        {
            EllipseShape elipse = new EllipseShape(Utilities.CalculateRectangle(startPoint, endPoint));
            elipse.FillColor = Color.Transparent;
            ShapeList.Add(elipse);
            selections.Clear();
            selections.Add(elipse);
            return elipse;
        }
        #endregion
    }
}
