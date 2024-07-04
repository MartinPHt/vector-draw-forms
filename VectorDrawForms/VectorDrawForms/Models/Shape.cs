using System;
using System.Collections.Generic;
using System.Drawing;

namespace VectorDrawForms.Models
{
    [Serializable]
    public abstract class Shape : IShape //TODO: Create new ResizableShape class and move the resize functionality there.|
    {
        #region Constructors
        public Shape(RectangleF rect)
        {
            this.Rectangle = rect;
        }

        public Shape(IShape shape)
        {
            this.Height = shape.Height;
            this.Width = shape.Width;
            this.Location = shape.Location;
            this.Rectangle = shape.Rectangle;

            this.FillColor = shape.FillColor;
            this.StrokeColor = shape.StrokeColor;
        }
        #endregion

        #region Constants
        private const int RESIZE_RECTANGLE_HEIGHT = 8;
        private const int RESIZE_RECTANGLE_WIDTH = 8;
        #endregion

        #region Properties

        /// <summary>
        /// The element's bounding rectangle.
        /// </summary>
        private RectangleF rectangle;
        public virtual RectangleF Rectangle
        {
            get { return rectangle; }
            set
            {
                rectangle = value;
                CalculateResizeRectangles();
            }
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
            set
            {
                rectangle.Width = value;
                CalculateResizeRectangles();
            }
        }

        /// <summary>
        /// Height of the element.
        /// </summary>
        public virtual float Height
        {
            get { return Rectangle.Height; }
            set
            {
                rectangle.Height = value;
                CalculateResizeRectangles();
            }
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
            set
            {
                rectangle.Location = value;
                CalculateResizeRectangles();
            }
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

        public Dictionary<ResizeRectangle, RectangleF> ResizeRectangles { get; private set; } = new Dictionary<ResizeRectangle, RectangleF>();
        #endregion

        #region Methods
        /// <summary>
        /// Checks if point belongs to the <see cref="Shape"/>.
        /// </summary>
        /// <param name="point">Point</param>
        /// <returns>Returns true if the point belongs to the element and false if it does not</returns>
        public virtual bool Contains(PointF point)
        {
            return Rectangle.Contains(point.X, point.Y);
        }

        /// <summary>
        /// Checks if point belongs to the <see cref="Shape"/>'s Resize rectangles.
        /// </summary>
        /// <param name="point">Point</param>
        /// <returns>Returns true if the point belongs to the element's resize rectangles and false if it does not</returns>
        public virtual bool ContainsInResizeRectangles(PointF point)
        {
            return MouseOverResizeRect(point, ResizeRectangle.TopLeft)
                || MouseOverResizeRect(point, ResizeRectangle.TopRight)
                || MouseOverResizeRect(point, ResizeRectangle.TopMid)
                || MouseOverResizeRect(point, ResizeRectangle.BottomLeft)
                || MouseOverResizeRect(point, ResizeRectangle.BottomRight)
                || MouseOverResizeRect(point, ResizeRectangle.BottomMid)
                || MouseOverResizeRect(point, ResizeRectangle.MidLeft)
                || MouseOverResizeRect(point, ResizeRectangle.MidRight);
        }

        /// <summary>
        /// Checks if point belongs to the <see cref="Shape"/>'s Resize rectangles.
        /// </summary>
        /// <param name="point">Point</param>
        /// <returns>Returns true if the point belongs to the element's resize rectangles and false if it does not</returns>
        public virtual bool ContainsInResizeRectangles(PointF point, out ResizeRectangle resizeRectangleUsed)
        {
            bool contained;
            if (MouseOverResizeRect(point, ResizeRectangle.TopLeft))
            {
                resizeRectangleUsed = ResizeRectangle.TopLeft;
                contained = true;
            }
            else if (MouseOverResizeRect(point, ResizeRectangle.TopRight)) 
            {
                resizeRectangleUsed = ResizeRectangle.TopRight;
                contained = true;
            }
            else if(MouseOverResizeRect(point, ResizeRectangle.TopMid)) 
            {
                resizeRectangleUsed = ResizeRectangle.TopMid;
                contained = true;
            }
            else if(MouseOverResizeRect(point, ResizeRectangle.BottomLeft)) 
            {
                resizeRectangleUsed = ResizeRectangle.BottomLeft;
                contained = true;
            }
            else if(MouseOverResizeRect(point, ResizeRectangle.BottomRight)) 
            {
                resizeRectangleUsed = ResizeRectangle.BottomRight;
                contained = true;
            }
            else if(MouseOverResizeRect(point, ResizeRectangle.BottomMid))
            {
                resizeRectangleUsed = ResizeRectangle.BottomMid;
                contained = true;
            }
            else if(MouseOverResizeRect(point, ResizeRectangle.MidLeft))
            {
                resizeRectangleUsed = ResizeRectangle.MidLeft;
                contained = true;
            }
            else if(MouseOverResizeRect(point, ResizeRectangle.MidRight))
            {
                resizeRectangleUsed = ResizeRectangle.MidRight;
                contained = true;
            }
            else
            {
                resizeRectangleUsed = ResizeRectangle.None;
                contained = false;
            }

            return contained;
        }

        /// <summary>
        /// Renders the element.
        /// </summary>
        /// <param name="grfx">Where to render the element.</param>
        public abstract void DrawSelf(Graphics grfx);

        public void CalculateResizeRectangles()
        {
            //points
            var x_Left = rectangle.X - ApplicationConstants.Instance.SelectionPen.Width - RESIZE_RECTANGLE_WIDTH / 2;
            var x_Right = rectangle.Right - RESIZE_RECTANGLE_WIDTH / 2;
            var x_Mid = rectangle.X + rectangle.Width / 2 - RESIZE_RECTANGLE_WIDTH / 2;
            var y_Top = rectangle.Y - ApplicationConstants.Instance.SelectionPen.Width - RESIZE_RECTANGLE_HEIGHT / 2;
            var y_Bottom = rectangle.Bottom - RESIZE_RECTANGLE_HEIGHT / 2;
            var y_Mid = rectangle.Y + rectangle.Height / 2 - RESIZE_RECTANGLE_HEIGHT / 2;

            ResizeRectangles = new Dictionary<ResizeRectangle, RectangleF>
            {
                { ResizeRectangle.TopLeft, new RectangleF(x_Left, y_Top, RESIZE_RECTANGLE_WIDTH, RESIZE_RECTANGLE_HEIGHT) },
                { ResizeRectangle.TopRight, new RectangleF(x_Right, y_Top, RESIZE_RECTANGLE_WIDTH, RESIZE_RECTANGLE_HEIGHT) },
                { ResizeRectangle.TopMid, new RectangleF(x_Mid, y_Top, RESIZE_RECTANGLE_WIDTH, RESIZE_RECTANGLE_HEIGHT) },
                { ResizeRectangle.BottomLeft, new RectangleF(x_Left, y_Bottom, RESIZE_RECTANGLE_WIDTH, RESIZE_RECTANGLE_HEIGHT) },
                { ResizeRectangle.BottomRight, new RectangleF(x_Right, y_Bottom, RESIZE_RECTANGLE_WIDTH, RESIZE_RECTANGLE_HEIGHT) },
                { ResizeRectangle.BottomMid, new RectangleF(x_Mid, y_Bottom, RESIZE_RECTANGLE_WIDTH, RESIZE_RECTANGLE_HEIGHT) },
                { ResizeRectangle.MidLeft, new RectangleF(x_Left, y_Mid, RESIZE_RECTANGLE_WIDTH, RESIZE_RECTANGLE_HEIGHT) },
                { ResizeRectangle.MidRight, new RectangleF(x_Right, y_Mid, RESIZE_RECTANGLE_WIDTH, RESIZE_RECTANGLE_HEIGHT) },
            };
        }

        /// <summary>
        /// Checks if the provided <see cref="PointF"/> is contained in the given <see cref="ResizeRectangle"/> of the shape.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public bool MouseOverResizeRect(PointF point, ResizeRectangle rectangle)
        {
            return ResizeRectangles[rectangle].Contains(point.X, point.Y);
        }

        public void PerformResize(Point location, ResizeRectangle resizeRect)
        {
            if (resizeRect == ResizeRectangle.BottomRight)
            {
                Rectangle = RectangleF.FromLTRB(
                    Rectangle.X, 
                    Rectangle.Y, 
                    location.X, 
                    location.Y);
            }
            else if (resizeRect == ResizeRectangle.TopRight)
            {
                Rectangle = RectangleF.FromLTRB(
                    Rectangle.X, 
                    location.Y, 
                    location.X, 
                    Rectangle.Bottom);
            }
            else if (resizeRect == ResizeRectangle.MidRight)
            {
                Rectangle = new RectangleF(
                    Rectangle.X, 
                    Rectangle.Y, 
                    Rectangle.Width + location.X - Rectangle.Right, 
                    Rectangle.Height);
            }
            else if (resizeRect == ResizeRectangle.TopLeft)
            {
                Rectangle = RectangleF.FromLTRB(
                    location.X,
                    location.Y,
                    Rectangle.Right,
                    Rectangle.Bottom);
            }
            else if (resizeRect == ResizeRectangle.BottomLeft)
            {
                Rectangle = RectangleF.FromLTRB(
                    location.X,
                    Rectangle.Y,
                    Rectangle.Right,
                    location.Y);
            }
            else if (resizeRect == ResizeRectangle.BottomMid)
            {
                Rectangle = new RectangleF(
                    Rectangle.X,
                    Rectangle.Y,
                    Rectangle.Width,
                    Rectangle.Height + location.Y - Rectangle.Bottom);
            }
            else if (resizeRect == ResizeRectangle.TopMid)
            {
                Rectangle = new RectangleF(
                    Rectangle.X,
                    location.Y,
                    Rectangle.Width,
                    Rectangle.Height + Rectangle.Y - location.Y);
            }
            else if (resizeRect == ResizeRectangle.MidLeft)
            {
                Rectangle = new RectangleF(
                    location.X,
                    Rectangle.Y,
                    Rectangle.Width + Rectangle.X - location.X,
                    Rectangle.Height);
            }
        }
        #endregion

        #region Enums
        public enum MoveDirection
        {
            Up,
            Down,
            Left,
            Right
        }

        public enum ResizeRectangle
        {
            None,
            TopLeft,
            TopRight,
            TopMid,
            BottomLeft,
            BottomRight,
            BottomMid,
            MidLeft,
            MidRight
        }
        #endregion
    }
}
