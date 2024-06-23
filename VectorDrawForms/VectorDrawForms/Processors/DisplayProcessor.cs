using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using VectorDrawForms.Models;
using System;
using System.Linq;

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
        public IReadOnlyCollection<IShape> Selections
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
                grfx.DrawRectangle(selectionPen, shape.Rectangle.X - shape.StrokeThickness / 2 - 1,
                    shape.Rectangle.Y - shape.StrokeThickness / 2 - 1,
                    shape.Rectangle.Width + shape.StrokeThickness + 2,
                    shape.Rectangle.Height + shape.StrokeThickness + 2);

                grfx.DrawRectangle(selectionPenInverted, shape.Rectangle.X - shape.StrokeThickness / 2 - 1,
                    shape.Rectangle.Y - shape.StrokeThickness / 2 - 1,
                    shape.Rectangle.Width + shape.StrokeThickness + 2,
                    shape.Rectangle.Height + shape.StrokeThickness + 2);
            }
        }
        #endregion
    }
}
