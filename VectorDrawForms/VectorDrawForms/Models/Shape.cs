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
			// shape.Rectangle.Inflate(shape.BorderWidth, shape.BorderWidth);
		}

		public enum RotateDirection
		{
			Left,
			Right,
		}
	}
}
