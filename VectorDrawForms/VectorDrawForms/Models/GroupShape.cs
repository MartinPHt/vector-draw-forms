using System;
using System.Collections.Generic;
using System.Drawing;

namespace VectorDrawForms.Models
{
    [Serializable]
    public class GroupShape : Shape
    {
        #region Constructor
        public GroupShape(RectangleF rect) : base(rect)
        {
        }

        public GroupShape(GroupShape group) : base(group)
        {
        }
        #endregion

        #region Properties
        private List<Shape> subShapes = new List<Shape>();

        /// <summary>
        /// Containst the primitives that are included in the <see cref="GroupShape"/>.
        /// </summary>
        public List<Shape> SubShapes 
        { 
            get {  return subShapes; }
            set { subShapes = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Checking whether a point belongs to the group.
        /// </summary>
        public override bool Contains(PointF point)
        {
            foreach (var shape in subShapes)
            {
                if (shape is RectangleShape)
                {
                    if (shape.Contains(point))
                        return true;
                }
                else if (shape is EllipseShape)
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
                    var rect = (RectangleShape)shape;
                    grfx.FillRectangle(new SolidBrush(rect.FillColor), rect.Rectangle.X, rect.Rectangle.Y, rect.Rectangle.Width, rect.Rectangle.Height);
                    grfx.DrawRectangle(new Pen(rect.StrokeColor), rect.Rectangle.X, rect.Rectangle.Y, rect.Rectangle.Width, rect.Rectangle.Height);
                }
                else if (shape is EllipseShape)
                {
                    var ellipse = (EllipseShape)shape;
                    grfx.FillEllipse(new SolidBrush(ellipse.FillColor), ellipse.Rectangle.X, ellipse.Rectangle.Y, ellipse.Rectangle.Width, ellipse.Rectangle.Height);
                    grfx.DrawEllipse(new Pen(ellipse.StrokeColor), ellipse.Rectangle.X, ellipse.Rectangle.Y, ellipse.Rectangle.Width, ellipse.Rectangle.Height);
                }
            }
        }
        #endregion
    }
}
