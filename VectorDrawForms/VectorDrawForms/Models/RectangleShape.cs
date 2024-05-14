using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace VectorDrawForms.Models
{
	[Serializable]
	public class RectangleShape : Shape
	{
		#region Constructor
		public RectangleShape(RectangleF rect) : base(rect)
		{
		}

		public RectangleShape(RectangleShape rectangle) : base(rectangle)
		{
		}
		#endregion

		/// <summary>
		/// Checking whether a point belongs to the rectangle.
		/// </summary>
		public override bool Contains(PointF point)
		{
			if (base.Contains(point))
				return true;
			else
				return false;
		}

        /// <summary>
        /// The part visualizing the specific primitive.
        /// </summary>
        public override void DrawSelf(Graphics grfx)
        {
			using (Matrix m = new Matrix())
			{
				m.RotateAt(RotationAngle, new PointF(Rectangle.Left + (Rectangle.Width / 2), Rectangle.Top + (Rectangle.Height / 2)));
				grfx.Transform = m;
                grfx.FillRectangle(new SolidBrush(FillColor), Rectangle);
                grfx.DrawRectangle(new Pen(StrokeColor, StrokeThickness), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
				grfx.ResetTransform();
            }
        }
    }
}
