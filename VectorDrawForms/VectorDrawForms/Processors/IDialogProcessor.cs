using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using VectorDrawForms.Models;

namespace VectorDrawForms.Processors
{
    public interface IDialogProcessor : IDisplayProcessor
    {
        IReadOnlyCollection<IShape> Selections { get; }
        List<IShape> CoppiedSelection { get; }
        bool IsDragging { get; set; }
        PointF LastLocation { get; set; }
        IShape ContainsPoint(PointF point);
        void TranslateTo(PointF p);
    }
}
