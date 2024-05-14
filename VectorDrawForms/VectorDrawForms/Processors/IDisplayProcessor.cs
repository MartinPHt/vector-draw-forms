using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using VectorDrawForms.Models;

namespace VectorDrawForms.Processors
{
    public interface IDisplayProcessor
    {
        List<IShape> ShapeList { get; set; }

        void Draw(Graphics grfx);

        void ReDraw(object sender, PaintEventArgs e);

        void DrawShape(Graphics grfx, IShape item);
    }
}
