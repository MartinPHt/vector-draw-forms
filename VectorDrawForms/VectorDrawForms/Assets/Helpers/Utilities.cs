using System.Drawing;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using VectorDrawForms.Models;

namespace VectorDrawForms
{
    public static class ShapeUtility
    {
        /// <summary>
        /// Creates new <see cref="RectangleF"/> with calculated height and width of based on the provided startPoint and endPoint parameters/> 
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns>New <see cref="RectangleF"/> object with the calculated width and height.</returns>
        public static RectangleF CalculateRectangle(PointF startPoint, PointF endPoint)
        {
            //starting point
            float x1 = startPoint.X;
            float y1 = startPoint.Y;

            //end point
            float x2 = endPoint.X;
            float y2 = endPoint.Y;

            //calculate rectangle width
            float width = Math.Abs(x1 - x2);

            //calculate rectangle height
            float height = Math.Abs(y1 - y2);

            float x;
            if (x1 < x2)
                x = x1;
            else 
                x = x2;

            float y;
            if (y1 < y2)
                y = y1;
            else
                y = y2;

            return new RectangleF(x, y, width, height);
        }

        public static RectangleF CalculateGroupRectangle(IEnumerable<IShape> shapes)
        {
            //starting point
            float x = float.MaxValue;
            float y = float.MaxValue;

            //end point
            float x1 = float.MinValue;
            float y1 = float.MinValue;
            foreach (IShape shape in shapes)
            {
                if (shape.Width >= 0)
                {
                    if (x > shape.Rectangle.X)
                        x = shape.Rectangle.X;

                    if (x1 < shape.Rectangle.Right)
                        x1 = shape.Rectangle.Right;
                }
                else
                {
                    var currentX = shape.Rectangle.X - Math.Abs(shape.Rectangle.Width);
                    if (x > currentX)
                        x = currentX;

                    if (x1 < shape.Rectangle.X)
                        x1 = shape.Rectangle.X;
                }

                if (shape.Height >= 0)
                {
                    if (y > shape.Rectangle.Y)
                        y = shape.Rectangle.Y;

                    if (y1 < shape.Rectangle.Bottom)
                        y1 = shape.Rectangle.Bottom;
                }
                else
                {
                    var currentY = shape.Rectangle.Y - Math.Abs(shape.Rectangle.Height);
                    if (y > currentY)
                        y = currentY;

                    if (y1 < shape.Rectangle.Y)
                        y1 = shape.Rectangle.Y;
                }
            }

            //calculate rectangle width
            float width = Math.Abs(x - x1);

            //calculate rectangle height
            float height = Math.Abs(y - y1);
            return new RectangleF(x, y, width, height);
        }

        public static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            // top left arc  
            path.AddArc(arc, 180, 90);

            // top right arc  
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc  
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc 
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        } 

        /// <summary>
        /// Returns a deep clone of the current object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepClone<T>(this T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
    }
}
