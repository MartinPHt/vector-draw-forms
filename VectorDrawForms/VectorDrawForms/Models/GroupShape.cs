using System;
using System.Collections.Generic;
using System.Drawing;

namespace VectorDrawForms.Models
{
    [Serializable]
    public class GroupShape : Shape
    {
        #region Fields
        public List<Shape> subShapes = new List<Shape>();
        #endregion

        #region Constructor
        public GroupShape(RectangleF rect) : base(rect)
        {
        }

        public GroupShape(GroupShape group) : base(group)
        {
        }
        #endregion

        /// <summary>
        /// Checking whether a point belongs to the elipse.
        /// </summary>
        public override bool Contains(PointF point)
        {
            foreach (var shape in subShapes)
            {
                if (shape is RectangleShape)
                {
                    if (base.Contains(point))
                        return true;
                }
                else if (shape is ElipseShape)
                {
                    double a = shape.Width / 2;
                    double b = shape.Height / 2;
                    double centerX = shape.Location.X + a;
                    double centerY = shape.Location.Y + b;

                    double dx = (point.X - centerX) / a;
                    double dy = (point.Y - centerY) / b;

                    if ((dx * dx + dy * dy) <= 1)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// The part visualizing the specific primitive.
        /// </summary>
        public override void DrawSelf(Graphics grfx)
        {
            base.DrawSelf(grfx);

            foreach (var shape in subShapes)
            {
                if (shape is RectangleShape)
                {
                    grfx.FillRectangle(new SolidBrush(FillColor), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
                    grfx.DrawRectangle(new Pen(StrokeColor), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
                }
                else if (shape is ElipseShape)
                {
                    grfx.FillEllipse(new SolidBrush(FillColor), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
                    grfx.DrawEllipse(new Pen(StrokeColor), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
                }
            }
        }
    }
}
