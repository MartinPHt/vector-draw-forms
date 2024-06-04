using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace VectorDrawForms.Models
{
	[Serializable]
    public class EllipseShape : Shape
    {
		#region Constructor

		public EllipseShape(RectangleF rect) : base(rect)
		{
			
        }

		public EllipseShape(RectangleF rect, Color fillColor, int strokeThickness) : this(rect)
		{
			FillColor = fillColor;
            StrokeThickness = strokeThickness;
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

            using (Matrix m = new Matrix())
            {
                m.RotateAt(RotationAngle, new PointF(Rectangle.Left + (Rectangle.Width / 2), Rectangle.Top + (Rectangle.Height / 2)));
                grfx.Transform = m;
                grfx.FillEllipse(new SolidBrush(FillColor), Rectangle);
                grfx.DrawEllipse(new Pen(StrokeColor, StrokeThickness), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
                grfx.ResetTransform();
            }
        }
	}
}
