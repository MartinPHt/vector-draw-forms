using System.Collections.Generic;
using System.Drawing;
using VectorDrawForms.Models;

namespace VectorDrawForms.Processors
{
    public interface IDialogProcessor : IDisplayProcessor
    {
        List<IShape> Selections { get; set; }
        bool IsDragging { get; set; }
        PointF LastLocation { get; set; }
        IShape ContainsPoint(PointF point);
        void TranslateTo(PointF p);
    }
}
