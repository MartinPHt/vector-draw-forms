using System.Drawing;

namespace VectorDrawForms
{
    public static class ApplicationColors
    {
        /// <summary>
        /// The main User Interface light color
        /// </summary>
        public static Color MainUILight { get { return Color.FromArgb(245, 246, 250); } }

        /// <summary>
        /// The main User Interface light color
        /// </summary>
        public static Color MainUIDark { get { return Color.FromArgb(50, 50, 50); } }

        /// <summary>
        /// The main User Interface light color
        /// </summary>
        public static Color SecondaryUIDark { get { return Color.FromArgb(85, 85, 85); } }

        /// <summary>
        /// The default shape stroke color
        /// </summary>
        public static Color ShapeStroke { get { return Color.Gray; } }

        /// <summary>
        /// The default shape fill color
        /// </summary>
        public static Color ShapeFill { get { return Color.Transparent; } }
    }
}
