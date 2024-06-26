using System.Collections.Generic;
using System.Drawing;
using VectorDrawForms.Models;

namespace VectorDrawForms.Processors
{
    public interface IDialogProcessor : IDisplayProcessor
    {
        List<IShape> CoppiedSelection { get; }
        bool IsDragging { get; set; }
        PointF LastLocation { get; set; }
        IShape ContainsPoint(PointF point);
        void TranslateTo(PointF p);
    }
}
