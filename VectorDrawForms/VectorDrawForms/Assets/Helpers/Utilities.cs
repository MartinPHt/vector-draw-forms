using System.Drawing;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace VectorDrawForms
{
    public static class Utilities
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
