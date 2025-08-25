using System;
using System.Drawing;

namespace ShapeEditor
{
    [Serializable]
    public abstract class FilledShape : IShape
    {
        public Color ColorShape { get; set; }
        public abstract void Draw(Graphics g);
        public abstract bool ContainsPoint(Point p);
        public abstract string GetInformation();

        public FilledShape(Color color)
        {
            ColorShape = color;
        }
    }
}
