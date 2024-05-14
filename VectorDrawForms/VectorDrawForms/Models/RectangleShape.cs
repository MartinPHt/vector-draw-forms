using System;
using System.Drawing;

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
			base.DrawSelf(grfx);

			grfx.FillRectangle(new SolidBrush(FillColor), Rectangle);
			grfx.DrawRectangle(new Pen(StrokeColor, BorderThickness), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
		}
	}
}
