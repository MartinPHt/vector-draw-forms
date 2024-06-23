using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorDrawForms.Models
{
    public interface IGroupShape : IShape
    {
        List<IShape> SubShapes { get; }
    }
}
