﻿using System.Collections.Generic;
using System.Drawing;
using static VectorDrawForms.Models.Shape;

namespace VectorDrawForms.Models
{
    public interface IShape
    {
        /// <summary>
		/// The element's bounding rectangle.
		/// </summary>
        RectangleF Rectangle { get; set; }

        /// <summary>
		/// Indicates weather the shape is selected or not.
		/// </summary>
        bool IsSelected { get; set; }

        /// <summary>
		/// Width of the element.
		/// </summary>
        float Width { get; set; }

        /// <summary>
		/// Height of the element.
		/// </summary>
        float Height { get; set; }

        /// <summary>
		/// Border thickness of the element.
		/// </summary>
        float StrokeThickness { get; set; }

        /// <summary>
        /// Rotation Angle of the element.
        /// </summary>
        float RotationAngle { get; set; }

        /// <summary>
		/// Top left corner of element.
		/// </summary>
		PointF Location { get; set; }

        /// <summary>
		/// Color of the element.
		/// </summary>
		Color FillColor { get; set; }

        /// <summary>
        /// Stroke of the element.
        /// </summary>
        Color StrokeColor { get; set; }

        /// <summary>
        /// Checks if point belongs to the element.
        /// </summary>
        /// <param name="point">Point</param>
        /// <returns>Returns true if the point belongs to the element and false if it does not</returns>.
        bool Contains(PointF point);

        /// <summary>
        /// Checks if point belongs to the <see cref="Shape"/>'s Resize rectangles.
        /// </summary>
        /// <param name="point">Point</param>
        /// <returns>Returns true if the point belongs to the element's resize rectangles and false if it does not</returns>
        bool ContainsInResizeRectangles(PointF point);

        /// <summary>
        /// Checks if point belongs to the <see cref="Shape"/>'s Resize rectangles.
        /// </summary>
        /// <param name="point">Point</param>
        /// <returns>Returns true if the point belongs to the element's resize rectangles and false if it does not</returns>
        bool ContainsInResizeRectangles(PointF point, out ResizeRectangle resizeRectangleUsed);

        /// <summary>
        /// Renders the element.
        /// </summary>
        /// <param name="grfx">Where to render the element.</param>
        void DrawSelf(Graphics grfx);

        /// <summary>
        /// Gets the rectangles that are used for resizing when the <see cref="Shape"/> is selected.
        /// </summary>
        /// <returns></returns>
        Dictionary<ResizeRectangle, RectangleF> ResizeRectangles { get; }

        /// <summary>
        /// Checks if the provided <see cref="PointF"/> is contained in the given <see cref="ResizeRectangle"/> of the shape.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        bool MouseOverResizeRect(PointF point, ResizeRectangle rectangle);

        /// <summary>
        /// Performs resize from the center of the <see cref="Shape"/>'s rectangle to the provided resize rectangle
        /// </summary>
        /// <param name="location"></param>
        /// <param name="bottomRight"></param>
        void PerformResize(Point location, ResizeRectangle resizeRect);
    }
}
