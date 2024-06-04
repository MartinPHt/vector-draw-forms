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
        /// The main User Interface dark color
        /// </summary>
        public static Color MainUIDark { get { return Color.FromArgb(50, 50, 50); } }

        /// <summary>
        /// The secondary User Interface dark color
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

        /// <summary>
        /// ToolStripMenu Hovered dark color
        /// </summary>
        public static Brush ToolStripMenuCheckedDark { get { return new SolidBrush(Color.FromArgb(67, 67, 67)); } }

        /// <summary>
        /// ToolStripMenu Hovered dark color
        /// </summary>
        public static Brush ToolStripMenuHoveredDark { get { return new SolidBrush(Color.FromArgb(76, 76, 76)); } }

        /// <summary>
        /// ToolStripMenu Hovered dark color
        /// </summary>
        public static Brush ToolStripMenuCheckedLight { get { return new SolidBrush(Color.FromArgb(210, 210, 210)); } }

        /// <summary>
        /// ToolStripMenu Hovered dark color
        /// </summary>
        public static Brush ToolStripMenuHoveredLight { get { return new SolidBrush(Color.FromArgb(230, 230, 230)); } }
    }
}
