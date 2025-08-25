using System;
using System.Drawing;

namespace ShapeEditor
{
    public interface IShape
    {
        void Draw(Graphics g);
        bool ContainsPoint(Point p);
        string GetInformation();
    }
}
