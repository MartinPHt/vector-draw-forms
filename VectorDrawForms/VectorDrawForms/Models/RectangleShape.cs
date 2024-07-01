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

		public RectangleShape(RectangleF rect, Color strokeColor, float strokeThickness) : this(rect)
		{
			StrokeColor = strokeColor;
            StrokeThickness = strokeThickness;
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
			if (Rectangle.Width >= 0 && Rectangle.Height >= 0)
			{
				return Rectangle.Contains(point.X, point.Y);
			}
			else
			{
                bool containedOnAbscissa = false;
                bool containedOnOrdinate = false;
                if (Rectangle.Width < 0 && Rectangle.Height < 0)
                {
                    containedOnAbscissa = point.X <= Rectangle.X && point.X >= Rectangle.X - Math.Abs(Rectangle.Width);
                    containedOnOrdinate = point.Y <= Rectangle.Y && point.Y >= Rectangle.Y - Math.Abs(Rectangle.Height);
                }
                else if (Rectangle.Width < 0)
                {
                    containedOnAbscissa = point.X <= Rectangle.X && point.X >= Rectangle.X - Math.Abs(Rectangle.Width);
                    containedOnOrdinate = point.Y >= Rectangle.Y && point.Y <= Rectangle.Bottom;
                }
                else
                {
                    containedOnAbscissa = point.X >= Rectangle.X && point.X <= Rectangle.Right;
                    containedOnOrdinate = point.Y <= Rectangle.Y && point.Y >= Rectangle.Y - Math.Abs(Rectangle.Height);
                }

                return containedOnAbscissa && containedOnOrdinate;
            }
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

                grfx.FillRectangle(
					new SolidBrush(FillColor), 
					Math.Min(Rectangle.X, Rectangle.Right),
					Math.Min(Rectangle.Y, Rectangle.Bottom),
					Math.Abs(Rectangle.Right - Rectangle.X),
					Math.Abs(Rectangle.Bottom - Rectangle.Y));

                grfx.DrawRectangle(
					new Pen(StrokeColor, StrokeThickness),
					Math.Min(Rectangle.X, Rectangle.Right),
					Math.Min(Rectangle.Y, Rectangle.Bottom),
					Math.Abs(Rectangle.Right - Rectangle.X),
					Math.Abs(Rectangle.Bottom - Rectangle.Y));

				grfx.ResetTransform();
            }
        }
    }
}
