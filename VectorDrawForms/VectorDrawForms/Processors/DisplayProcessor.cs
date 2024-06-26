using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using VectorDrawForms.Models;
using System.Linq;
using static VectorDrawForms.Models.Shape;
using System.Collections.ObjectModel;
using System;

namespace VectorDrawForms.Processors
{
    public class DisplayProcessor : IDisplayProcessor
    {
        #region Constructor
        public DisplayProcessor()
        {
        }
        #endregion

        #region Properties

        private Pen selectionPen = ApplicationConstants.Instance.SelectionPen;
        public Pen SelectionPen
        {
            get { return selectionPen; }
        }

        private Pen selectionPenInverted = ApplicationConstants.Instance.SelectionPenInverted;
        public Pen SelectionPenInverted
        {
            get { return selectionPenInverted; }
        }

        private List<IShape> selections = new List<IShape>();
        /// <summary>
        /// Selected element
        /// </summary>
        public IReadOnlyList<IShape> Selections
        {
            get { return selections; }
        }

        /// <summary>
        /// List containing all elements forming the image.
        /// </summary>
        private List<IShape> shapeList = new List<IShape>();
        public List<IShape> ShapeList
        {
            get { return shapeList; }
            set { shapeList = value; }
        }

        #endregion

        #region Methods

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
            shape.IsSelected = false;
            selections.Remove(shape);
        }

        /// <summary>
        /// Removes the provided shapes to the selections only if they are contained in the ShapesList of current <see cref="DialogProcessor"/>.
        /// </summary>
        /// <param name="shape"></param>
        public void RemoveSelectionRange(IEnumerable<IShape> shapes)
        {
            foreach (var shape in shapes)
            {
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

        /// <summary>
        /// Deletes the selected primitives
        /// </summary>
        public void DeleteSelection()
        {
            ShapeList = ShapeList.Except(Selections).ToList();
            ClearSelection();
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
        /// Redraws all elements in shapeList of passed <see cref="PaintEventArgs"/>.Graphics
        /// </summary>
        public void ReDraw(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Draw(e.Graphics);
        }

        /// <summary>
        /// Visualization. Loop through all items in the list and call their renderer method.
        /// </summary>
        /// <param name="grfx">Where to perform the visualization.</param>
        public virtual void Draw(Graphics grfx)
        {
            foreach (IShape item in ShapeList)
                DrawShape(grfx, item);

            if (selections.Count > 0)
                DrawSelectionElements(grfx);
        }

        /// <summary>
        /// Renders a given shape element.
        /// </summary>
        /// <param name="grfx">Where to perform the visualization.</param>
        /// <param name="item">Visualization element.</param>
        public virtual void DrawShape(Graphics grfx, IShape item)
        {
            item.DrawSelf(grfx);
        }

        /// <summary>
        /// Renders all selection elements.
        /// </summary>
        /// <param name="grfx">Where to perform the visualization.</param>
        /// <param name="item">Visualization element.</param>
        private void DrawSelectionElements(Graphics grfx)
        {
            foreach (var shape in Selections)
            {
                //Shape's rectangle & strokeThickness
                var rectangle = shape.Rectangle;
                var strokeThickness = shape.StrokeThickness;

                //Main selection dash rectangle
                grfx.DrawRectangle(selectionPen,
                    Math.Min(rectangle.X, rectangle.Right) - strokeThickness / 2 - 1,
                    Math.Min(rectangle.Y, rectangle.Bottom) - strokeThickness / 2 - 1,
                    Math.Abs(rectangle.Right - rectangle.X) + strokeThickness + 2,
                    Math.Abs(rectangle.Bottom - rectangle.Y) + strokeThickness + 2);

                //Inverted color selection dash rectangle
                grfx.DrawRectangle(selectionPenInverted,
                    Math.Min(rectangle.X, rectangle.Right) - strokeThickness / 2 - 1,
                    Math.Min(rectangle.Y, rectangle.Bottom) - strokeThickness / 2 - 1,
                    Math.Abs(rectangle.Right - rectangle.X) + strokeThickness + 2,
                    Math.Abs(rectangle.Bottom - rectangle.Y) + strokeThickness + 2);

                //Resize Rectangles

                //Top Left
                grfx.DrawRectangle(Pens.Black,
                    shape.ResizeRectangles[ResizeRectangle.TopLeft].X,
                    shape.ResizeRectangles[ResizeRectangle.TopLeft].Y,
                    shape.ResizeRectangles[ResizeRectangle.TopLeft].Width,
                    shape.ResizeRectangles[ResizeRectangle.TopLeft].Height);

                grfx.FillRectangle(Brushes.White,
                    shape.ResizeRectangles[ResizeRectangle.TopLeft].X,
                    shape.ResizeRectangles[ResizeRectangle.TopLeft].Y,
                    shape.ResizeRectangles[ResizeRectangle.TopLeft].Width,
                    shape.ResizeRectangles[ResizeRectangle.TopLeft].Height);

                //Top Right
                grfx.DrawRectangle(Pens.Black,
                    shape.ResizeRectangles[ResizeRectangle.TopRight].X,
                    shape.ResizeRectangles[ResizeRectangle.TopRight].Y,
                    shape.ResizeRectangles[ResizeRectangle.TopRight].Width,
                    shape.ResizeRectangles[ResizeRectangle.TopRight].Height);

                grfx.FillRectangle(Brushes.White,
                    shape.ResizeRectangles[ResizeRectangle.TopRight].X,
                    shape.ResizeRectangles[ResizeRectangle.TopRight].Y,
                    shape.ResizeRectangles[ResizeRectangle.TopRight].Width,
                    shape.ResizeRectangles[ResizeRectangle.TopRight].Height);

                //Top Mid
                grfx.DrawRectangle(Pens.Black,
                    shape.ResizeRectangles[ResizeRectangle.TopMid].X,
                    shape.ResizeRectangles[ResizeRectangle.TopMid].Y,
                    shape.ResizeRectangles[ResizeRectangle.TopMid].Width,
                    shape.ResizeRectangles[ResizeRectangle.TopMid].Height);

                grfx.FillRectangle(Brushes.White,
                    shape.ResizeRectangles[ResizeRectangle.TopMid].X,
                    shape.ResizeRectangles[ResizeRectangle.TopMid].Y,
                    shape.ResizeRectangles[ResizeRectangle.TopMid].Width,
                    shape.ResizeRectangles[ResizeRectangle.TopMid].Height);

                //Bottom Left
                grfx.DrawRectangle(Pens.Black,
                    shape.ResizeRectangles[ResizeRectangle.BottomLeft].X,
                    shape.ResizeRectangles[ResizeRectangle.BottomLeft].Y,
                    shape.ResizeRectangles[ResizeRectangle.BottomLeft].Width,
                    shape.ResizeRectangles[ResizeRectangle.BottomLeft].Height);

                grfx.FillRectangle(Brushes.White,
                    shape.ResizeRectangles[ResizeRectangle.BottomLeft].X,
                    shape.ResizeRectangles[ResizeRectangle.BottomLeft].Y,
                    shape.ResizeRectangles[ResizeRectangle.BottomLeft].Width,
                    shape.ResizeRectangles[ResizeRectangle.BottomLeft].Height);

                //Bottom Right
                grfx.DrawRectangle(Pens.Black,
                    shape.ResizeRectangles[ResizeRectangle.BottomRight].X,
                    shape.ResizeRectangles[ResizeRectangle.BottomRight].Y,
                    shape.ResizeRectangles[ResizeRectangle.BottomRight].Width,
                    shape.ResizeRectangles[ResizeRectangle.BottomRight].Height);

                grfx.FillRectangle(Brushes.White,
                    shape.ResizeRectangles[ResizeRectangle.BottomRight].X,
                    shape.ResizeRectangles[ResizeRectangle.BottomRight].Y,
                    shape.ResizeRectangles[ResizeRectangle.BottomRight].Width,
                    shape.ResizeRectangles[ResizeRectangle.BottomRight].Height);

                //Bottom Mid
                grfx.DrawRectangle(Pens.Black,
                    shape.ResizeRectangles[ResizeRectangle.BottomMid].X,
                    shape.ResizeRectangles[ResizeRectangle.BottomMid].Y,
                    shape.ResizeRectangles[ResizeRectangle.BottomMid].Width,
                    shape.ResizeRectangles[ResizeRectangle.BottomMid].Height);

                grfx.FillRectangle(Brushes.White,
                    shape.ResizeRectangles[ResizeRectangle.BottomMid].X,
                    shape.ResizeRectangles[ResizeRectangle.BottomMid].Y,
                    shape.ResizeRectangles[ResizeRectangle.BottomMid].Width,
                    shape.ResizeRectangles[ResizeRectangle.BottomMid].Height);

                //Mid left 
                grfx.DrawRectangle(Pens.Black,
                    shape.ResizeRectangles[ResizeRectangle.MidLeft].X,
                    shape.ResizeRectangles[ResizeRectangle.MidLeft].Y,
                    shape.ResizeRectangles[ResizeRectangle.MidLeft].Width,
                    shape.ResizeRectangles[ResizeRectangle.MidLeft].Height);

                grfx.FillRectangle(Brushes.White,
                    shape.ResizeRectangles[ResizeRectangle.MidLeft].X,
                    shape.ResizeRectangles[ResizeRectangle.MidLeft].Y,
                    shape.ResizeRectangles[ResizeRectangle.MidLeft].Width,
                    shape.ResizeRectangles[ResizeRectangle.MidLeft].Height);

                //Mid right 
                grfx.DrawRectangle(Pens.Black,
                    shape.ResizeRectangles[ResizeRectangle.MidRight].X,
                    shape.ResizeRectangles[ResizeRectangle.MidRight].Y,
                    shape.ResizeRectangles[ResizeRectangle.MidRight].Width,
                    shape.ResizeRectangles[ResizeRectangle.MidRight].Height);

                grfx.FillRectangle(Brushes.White,
                    shape.ResizeRectangles[ResizeRectangle.MidRight].X,
                    shape.ResizeRectangles[ResizeRectangle.MidRight].Y,
                    shape.ResizeRectangles[ResizeRectangle.MidRight].Width,
                    shape.ResizeRectangles[ResizeRectangle.MidRight].Height);
            }
        }
        #endregion
    }
}
