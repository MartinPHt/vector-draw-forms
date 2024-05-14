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
        private List<IShape> subShapes = new List<IShape>();

        /// <summary>
        /// Containst the primitives that are included in the <see cref="GroupShape"/>.
        /// </summary>
        public List<IShape> SubShapes 
        { 
            get {  return subShapes; }
            set { subShapes = value; }
        }

        public override Color FillColor
        {
            get { return SubShapes[0].FillColor; }
            set
            {
                foreach (var shape in subShapes)
                    shape.FillColor = value;     
            }
        }

        public override Color StrokeColor
        {
            get { return SubShapes[0].StrokeColor; }
            set
            {
                foreach (var shape in subShapes)
                    shape.StrokeColor = value;     
            }
        }

        public override float StrokeThickness
        {
            get { return SubShapes[0].StrokeThickness; }
            set
            {
                foreach (var shape in subShapes)
                    shape.StrokeThickness = value;     
            }
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
