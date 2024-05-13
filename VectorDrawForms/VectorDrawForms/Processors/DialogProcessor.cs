using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
            int height = rnd.Next(minHeight, maxHeight);
            int width = rnd.Next(minWidth, maxWidth);
            int x = rnd.Next(0, MainForm.Instance.Size.Width - width);
            int y = rnd.Next(0, MainForm.Instance.Size.Height - height);
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
            ElipseShape elipse = new ElipseShape(GenerateRandomRectangleForShape());
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
            foreach (Shape item in selections)
                item.Location = new PointF(item.Location.X + p.X - lastLocation.X, item.Location.Y + p.Y - lastLocation.Y);

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
        #endregion
    }
}
