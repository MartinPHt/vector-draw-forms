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
                    var ellipse = shape as EllipseShape;
                    if (ellipse.Contains(point))
                        return true;
                }
                else if (shape is GroupShape)
                {
                    var group = shape as GroupShape;
                    if (group.Contains(point))
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
                    rect.DrawSelf(grfx);
                }
                else if (shape is EllipseShape)
                {
                    var ellipse = (EllipseShape)shape;
                    ellipse.DrawSelf(grfx);
                }
                else if (shape is GroupShape)
                {
                    var group = (GroupShape)shape;
                    group.DrawSelf(grfx);
                }
            }
        }
        #endregion
    }
}
