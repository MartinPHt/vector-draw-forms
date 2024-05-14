using System;
using System.Drawing;

namespace VectorDrawForms.Models
{
	[Serializable]
    public class EllipseShape : Shape
    {
		#region Constructor

		public EllipseShape(RectangleF rect) : base(rect)
		{
		}

		public EllipseShape(EllipseShape elipse) : base(elipse)
		{
		}

		#endregion

		/// <summary>
		/// Checking whether a point belongs to the elipse.
		/// </summary>
		public override bool Contains(PointF point)
		{
			double a = Width / 2;
			double b = Height / 2;
			double centerX = Location.X + a;
			double centerY = Location.Y + b;

			double dx = (point.X - centerX) / a;
			double dy = (point.Y - centerY) / b;

			return (dx * dx + dy * dy) <= 1;
		}

		/// <summary>
		/// The part visualizing the specific primitive.
		/// </summary>
		public override void DrawSelf(Graphics grfx)
		{
			base.DrawSelf(grfx);

			grfx.FillEllipse(new SolidBrush(FillColor), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
			grfx.DrawEllipse(new Pen(StrokeColor, BorderThickness), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);

		}
	}
}
