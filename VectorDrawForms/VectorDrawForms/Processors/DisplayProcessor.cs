using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using VectorDrawForms.Models;

namespace VectorDrawForms.Processors
{
	public class DisplayProcessor
	{
		#region Constructor

		public DisplayProcessor()
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// List containing all elements forming the image.
		/// </summary>
		private List<Shape> shapeList = new List<Shape>();
		public List<Shape> ShapeList
		{
			get { return shapeList; }
			set { shapeList = value; }
		}

		#endregion

		#region Drawing

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
			foreach (Shape item in ShapeList)
			{
				DrawShape(grfx, item);
			}
		}

		/// <summary>
		/// Renders a given image element.
		/// </summary>
		/// <param name="grfx">Where to perform the visualization.</param>
		/// <param name="item">Visualization element.</param>
		public virtual void DrawShape(Graphics grfx, Shape item)
		{
			item.DrawSelf(grfx);
		}

		#endregion
	}
}
