using System;
using System.Drawing;

namespace VectorDrawForms.Models
{
    [Serializable]
    public class Shape : IShape
    {
        #region Constructors
        public Shape()
        {
        }

        public Shape(RectangleF rect)
        {
            rectangle = rect;
        }

        public Shape(IShape shape)
        {
            this.Height = shape.Height;
            this.Width = shape.Width;
            this.Location = shape.Location;
            this.rectangle = shape.Rectangle;

            this.FillColor = shape.FillColor;
            this.StrokeColor = shape.StrokeColor;
        }
        #endregion

        #region Properties

        /// <summary>
        /// The element's bounding rectangle.
        /// </summary>
        private RectangleF rectangle;
        public virtual RectangleF Rectangle
        {
            get { return rectangle; }
            set { rectangle = value; }
        }

        /// <summary>
        /// Gets the point of the upper-left corner of the element's rectangle
        /// </summary>
        public PointF StartPointRect
        {
            get { return new PointF(Rectangle.X, Rectangle.Y); }
        }

        /// <summary>
        /// Gets the point of the upper-right corner of the element's rectangle
        /// </summary>
        public PointF TopRightPointRect
        {
            get { return new PointF(Rectangle.X + Rectangle.Width, Rectangle.Y); }
        }

        /// <summary>
        /// Gets the point of the bottom-left corner of the element's rectangle
        /// </summary>
        public PointF BottomLeftPointRect
        {
            get { return new PointF(Rectangle.X, Rectangle.Y + Rectangle.Height); }
        }

        /// <summary>
        /// Gets the point of the bottom-right corner of the element's rectangle
        /// </summary>
        public PointF EndPointRect
        {
            get { return new PointF(Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height); }
        }

        /// <summary>
        /// Width of the element.
        /// </summary>
        public virtual float Width
        {
            get { return Rectangle.Width; }
            set { rectangle.Width = value; }
        }

        /// <summary>
        /// Height of the element.
        /// </summary>
        public virtual float Height
        {
            get { return Rectangle.Height; }
            set { rectangle.Height = value; }
        }

        private float rotationAngle;
        /// <summary>
        /// Rotation angle of the element.
        /// </summary>
        public virtual float RotationAngle
        {
            get { return rotationAngle; }
            set { rotationAngle = value; }
        }

        /// <summary>
        /// Top left corner of element.
        /// </summary>
        public virtual PointF Location
        {
            get { return Rectangle.Location; }
            set { rectangle.Location = value; }
        }

        /// <summary>
        /// Color of the element.
        /// </summary>
        private Color fillColor;
        public virtual Color FillColor
        {
            get { return fillColor; }
            set { fillColor = value; }
        }

        /// <summary>
        /// Stroke of the element.
        /// </summary>
        private Color strokeColor = Color.Gray;
        public virtual Color StrokeColor
        {
            get { return strokeColor; }
            set { strokeColor = value; }
        }

        /// <summary>
        /// Stroke thickness of the element.
        /// </summary>
        private float strokeThickness = 2;
        public virtual float StrokeThickness
        {
            get { return strokeThickness; }
            set { strokeThickness = value; }
        }

        /// <summary>
        /// Indicates weather the shape is selected or not.
        /// </summary>
        private bool isSelected;
        public virtual bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }

        /// <summary>
        /// The <see cref="Pen"/> which is used when selection rectangle is drawn.
        /// </summary>
        private Pen selectionPen = ApplicationConstants.Instance.SelectionPen;
        public virtual Pen SelectionPen
        {
            get { return selectionPen; }
            set {  selectionPen = value; }
        }
        #endregion

        /// <summary>
        /// Checks if point belongs to the element.
        /// </summary>
        /// <param name="point">Point</param>
        /// <returns>Returns true if the point belongs to the element and false if it does not</returns>
        public virtual bool Contains(PointF point)
        {
            return Rectangle.Contains(point.X, point.Y);
        }

        /// <summary>
        /// Renders the element.
        /// </summary>
        /// <param name="grfx">Where to render the element.</param>
        public virtual void DrawSelf(Graphics grfx)
        {
            if (IsSelected)
            {
                grfx.DrawRectangle(selectionPen, Rectangle.X - StrokeThickness / 2 - 1, 
                    Rectangle.Y - StrokeThickness / 2 - 1, 
                    Rectangle.Width + StrokeThickness + 2, 
                    Rectangle.Height + StrokeThickness + 2);
            }
        }

        public enum MoveDirection
        {
            Up,
            Down,
            Left,
            Right
        }
    }
}
