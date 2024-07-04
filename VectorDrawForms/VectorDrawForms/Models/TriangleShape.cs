using System;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace VectorDrawForms.Models
{
    [Serializable]
    internal class TriangleShape : Shape
    {
        #region Constructor
        public TriangleShape(RectangleF rect) : base(rect)
        {
        }

        public TriangleShape(RectangleF rect, Color strokeColor, float strokeThickness) : this(rect)
        {
            StrokeColor = strokeColor;
            StrokeThickness = strokeThickness;
        }

        public TriangleShape(TriangleShape rectangle) : base(rectangle)
        {
        }
        #endregion

        #region Properties
        public PointF TopPoint 
        { 
            get 
            { 
                return new PointF(Rectangle.X + Rectangle.Width / 2, Rectangle.Y); 
            } 
        }
        public PointF LeftBottomPoint
        {
            get
            {
                return new PointF(Rectangle.X, Rectangle.Y + Rectangle.Height);
            }
        }
        public PointF RightBottomPoint
        {
            get
            {
                return new PointF(Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height);
            }
        }
        #endregion

        /// <summary>
        /// Checking whether a point belongs to the rectangle.
        /// </summary>
        public override bool Contains(PointF point)
        {
            float denominator = ((LeftBottomPoint.Y - RightBottomPoint.Y) * (TopPoint.X - RightBottomPoint.X) + (RightBottomPoint.X - LeftBottomPoint.X) * (TopPoint.Y - RightBottomPoint.Y));
            float a = ((LeftBottomPoint.Y - RightBottomPoint.Y) * (point.X - RightBottomPoint.X) + (RightBottomPoint.X - LeftBottomPoint.X) * (point.Y - RightBottomPoint.Y)) / denominator;
            float b = ((RightBottomPoint.Y - TopPoint.Y) * (point.X - RightBottomPoint.X) + (TopPoint.X - RightBottomPoint.X) * (point.Y - RightBottomPoint.Y)) / denominator;
            float c = 1 - a - b;

            // The point is inside the triangle if a, b and c are all between 0 and 1
            return 0 <= a && a <= 1 && 0 <= b && b <= 1 && 0 <= c && c <= 1;
        }

        public PointF[] CalculateTriangelPoints(RectangleF rect)
        {
            return new PointF[]
            { 
                // Upper point
                new PointF(rect.X + rect.Width /2, rect.Y),

                //left point
                new PointF(rect.X, rect.Y + rect.Height),

                //right point
                new PointF(rect.X + rect.Width, rect.Y + rect.Height),
            };
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
                grfx.FillPolygon(new SolidBrush(FillColor), CalculateTriangelPoints(Rectangle));
                grfx.DrawPolygon(new Pen(StrokeColor, StrokeThickness), CalculateTriangelPoints(Rectangle));
                grfx.ResetTransform();
            }
        }
    }
}
