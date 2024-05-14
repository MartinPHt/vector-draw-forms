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
    public class DialogProcessor : DisplayProcessor
    {
        #region Constructor

        public DialogProcessor()
        {
        }

        #endregion

        #region Properties

        private List<Shape> selections = new List<Shape>();
        /// <summary>
        /// Selected element
        /// </summary>
        public List<Shape> Selections
        {
            get { return selections; }
            set { selections = value; }
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
            rect.FillColor = Color.White;
            ShapeList.Add(rect);
        }

        /// <summary>
        /// Adds an elipse primitive to an arbitrary location on the client area.
        /// </summary>
        public void AddRandomElipse()
        {
            EllipseShape elipse = new EllipseShape(GenerateRandomRectangleForShape());
            elipse.FillColor = Color.White;
            ShapeList.Add(elipse);
        }

        /// <summary>
        /// Checks if a point is in the element. Finds the "top" element ie. the one we see under the mouse.
        /// </summary>
        /// <param name="point">Указана точка</param>
        /// <returns>The shape element to which the given point belongs.</returns>
        public Shape ContainsPoint(PointF point)
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
            foreach (Shape shape in selections)
            {
                if (shape is GroupShape)
                {
                    var group = (GroupShape)shape;

                    foreach (var subShape in group.SubShapes)
                        subShape.Location = new PointF(subShape.Location.X + p.X - lastLocation.X, subShape.Location.Y + p.Y - lastLocation.Y);                    
                }
                else
                {
                    shape.Location = new PointF(shape.Location.X + p.X - lastLocation.X, shape.Location.Y + p.Y - lastLocation.Y);
                }

            }

            lastLocation = p;
        }

        public void ReadFile(string path)
        {
            try
            {
                FileStream stream = new FileStream(path, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                ShapeList = (List<Shape>)formatter.Deserialize(stream);

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
        /// Writes the model to a given file.
        /// </summary>
        /// <param name="name"></param>
        public void SaveToFile(string path)
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, ShapeList);

            //Dispose fileStream
            stream.Flush();
            stream.Close();
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
            //starting point
            float x = float.MaxValue;
            float y = float.MaxValue;

            //end point
            float x1 = float.MinValue;
            float y1 = float.MinValue;
            foreach (Shape item in selections)
            {
                if (x > item.Rectangle.X)
                    x = item.Rectangle.X;

                if (y > item.Rectangle.Y)
                    y = item.Rectangle.Y;

                if (x1 < item.Rectangle.Right)
                    x1 = item.Rectangle.Right;

                if (y1 < item.Rectangle.Bottom)
                    y1 = item.Rectangle.Bottom;
            }

            //calculate rectangle width
            float width = Math.Abs(x - x1);

            //calculate rectangle height
            float height = Math.Abs(y - y1);


            RectangleF rect = new RectangleF(x, y, width, height);
            //RectangleF rect = new Rectangle();

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
        #endregion
    }
}
