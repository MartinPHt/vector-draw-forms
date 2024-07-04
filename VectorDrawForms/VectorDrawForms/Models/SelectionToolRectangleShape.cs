using System;
using System.Drawing;

namespace VectorDrawForms.Models
{
    public class SelectionToolRectangleShape : Shape, ISelectionToolRectangleShape
    {
        #region Region
        public SelectionToolRectangleShape(RectangleF rect) : base(rect)
        {
        }

        public SelectionToolRectangleShape(IShape shape) : base(shape)
        {
        }
        #endregion

        #region Methods
        public override void DrawSelf(Graphics grfx)
        {
            grfx.DrawRectangle(
                    ApplicationConstants.Instance.SelectionPen,
                    Math.Min(Rectangle.X, Rectangle.Right),
                    Math.Min(Rectangle.Y, Rectangle.Bottom),
                    Math.Abs(Rectangle.Right - Rectangle.X),
                    Math.Abs(Rectangle.Bottom - Rectangle.Y));
        }
        #endregion
    }
}
