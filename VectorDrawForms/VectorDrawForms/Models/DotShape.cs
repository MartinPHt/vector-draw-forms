using System;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace VectorDrawForms.Models
{
    [Serializable]
    public class DotShape : Shape
    {
        #region Constructor
        public DotShape(float x, float y) : base(new RectangleF(x, y, 8, 8))
        {
            
        }
        public DotShape(float x, float y, Color color) : this(x, y)
        {
            FillColor = color;
        }

        public DotShape(EllipseShape elipse) : base(elipse)
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
                grfx.ResetTransform();
            }
        }
    }
}
