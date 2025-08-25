using System.Collections.Generic;
using System;

namespace ShapeEditor
{
    public interface IShapePlugin
    {
        string ShapeName { get; }
        string Version { get; }
        Dictionary<string, Type> GetParametersDescription();
        IShape CreateShapeWithParameters(Dictionary<string, object> parameters);
    }
}
