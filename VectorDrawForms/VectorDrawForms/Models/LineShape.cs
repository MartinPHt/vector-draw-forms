using System;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace VectorDrawForms.Models
{
    [Serializable]
    public class LineShape : Shape
    {
        #region Fields
        private readonly PointF initialStartPoint;
        #endregion

        #region Constructor
        public LineShape(PointF startPoint, PointF endPoint, bool isPreviewShape = false) : base(ShapeUtility.CalculateRectangle(startPoint, endPoint))
        {
            this.IsPreviewShape = isPreviewShape;
            initialStartPoint = startPoint;

            if ((startPoint.X < endPoint.X && startPoint.Y < endPoint.Y) || (startPoint.X > endPoint.X && startPoint.Y > endPoint.Y)) 
                lineDirection = Direction.Default;
            else
                lineDirection = Direction.Inverted;
        }

        public LineShape(PointF startPoint, PointF endPoint, Color color, float strokeThickness, bool isPreviewShape = false) : this(startPoint, endPoint, isPreviewShape)
        {
            StrokeColor = color;
            StrokeThickness = strokeThickness;
        }

        public LineShape(LineShape lineShape) : base(lineShape)
        {
            lineDirection = lineShape.LineDirection;
        }
        #endregion

        #region Properties
        public override RectangleF Rectangle 
        { 
            get => base.Rectangle;
            set 
            { 
                base.Rectangle = value; 

            } 
        }

        /// <summary>
        /// The start point of the line.
        /// </summary>
        public PointF LineStartPoint
        {
            get
            {
                if (lineDirection == Direction.Default)
                    return StartPointRect;
                else
                    return BottomLeftPointRect;
            }
        }

        /// <summary>
        /// The end point of the line.
        /// </summary>
        public PointF LineEndPoint
        {
            get
            {
                if (lineDirection == Direction.Default)
                    return EndPointRect;
                else
                    return TopRightPointRect;
            }
        }

        private Direction lineDirection;

        public Direction LineDirection
        {
            get { return lineDirection; }
            set { lineDirection = value; }
        }


        /// <summary>
        /// Indicates that the current shape is being drawn and is just a preview
        /// </summary>
        private bool IsPreviewShape { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Checking whether a point belongs to the rectangle.
        /// </summary>
        public override bool Contains(PointF point)
        {
            // Define a small tolerance value to account for floating-point imprecision
            const float tolerance = 15.0f; 

            // Calculate the distance from the point to the line
            float distance = DistanceFromPointToLine(point, LineStartPoint, LineEndPoint);

            // Check if the distance is within the tolerance
            if (distance > tolerance)
                return false;

            // Additionally, check if the point is within the bounds of the line segment
            bool withinXBounds = (point.X >= Math.Min(LineStartPoint.X, LineEndPoint.X) && point.X <= Math.Max(LineStartPoint.X, LineEndPoint.X));
            bool withinYBounds = (point.Y >= Math.Min(LineStartPoint.Y, LineEndPoint.Y) && point.Y <= Math.Max(LineStartPoint.Y, LineEndPoint.Y));

            return withinXBounds && withinYBounds;
        }

        private float DistanceFromPointToLine(PointF point, PointF lineStart, PointF lineEnd)
        {
            float dx = lineEnd.X - lineStart.X;
            float dy = lineEnd.Y - lineStart.Y;

            // If the line is actually a point
            if (dx == 0 && dy == 0)
            {
                return DistanceBetweenPoints(point, lineStart);
            }

            float t = ((point.X - lineStart.X) * dx + (point.Y - lineStart.Y) * dy) / (dx * dx + dy * dy);

            // Find the projection of the point onto the line
            PointF closestPoint;
            if (t < 0)
            {
                closestPoint = lineStart;
            }
            else if (t > 1)
            {
                closestPoint = lineEnd;
            }
            else
            {
                closestPoint = new PointF(lineStart.X + t * dx, lineStart.Y + t * dy);
            }

            // Return the distance between the point and the closest point on the line
            return DistanceBetweenPoints(point, closestPoint);
        }

        private float DistanceBetweenPoints(PointF p1, PointF p2)
        {
            float dx = p1.X - p2.X;
            float dy = p1.Y - p2.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public override void DrawSelf(Graphics grfx)
        {
            using (Matrix m = new Matrix())
            {
                m.RotateAt(RotationAngle, new PointF(Rectangle.Left + (Rectangle.Width / 2), Rectangle.Top + (Rectangle.Height / 2)));
                grfx.Transform = m;
                if (!IsPreviewShape)
                {
                    grfx.DrawLine(new Pen(StrokeColor, StrokeThickness), LineStartPoint, LineEndPoint);
                }
                else
                {
                    PointF lineEndPoint;
                    if (initialStartPoint == BottomLeftPointRect)                    
                        lineEndPoint = TopRightPointRect;                   
                    else if (initialStartPoint == TopRightPointRect)
                        lineEndPoint = BottomLeftPointRect;                  
                    else if (initialStartPoint == StartPointRect)
                        lineEndPoint = EndPointRect;                   
                    else
                        lineEndPoint = StartPointRect;

                    grfx.DrawLine(new Pen(StrokeColor, StrokeThickness), initialStartPoint, lineEndPoint);
                }
                grfx.ResetTransform();
            }
        }

        public enum Direction
        {
            Default,
            Inverted
        }
        #endregion
    }
}
