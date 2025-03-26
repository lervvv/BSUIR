using System.Drawing;

namespace laba1
{
    public abstract class FilledShape : Shape
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
