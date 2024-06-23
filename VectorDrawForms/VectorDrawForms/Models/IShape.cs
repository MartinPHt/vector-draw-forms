using System.Drawing;

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
        /// <returns>Returns true if the point belongs to the element and false if it does not</returns>
        bool Contains(PointF point);

        /// <summary>
        /// Renders the element.
        /// </summary>
        /// <param name="grfx">Where to render the element.</param>
        void DrawSelf(Graphics grfx);
    }
}
