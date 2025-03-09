using System.Drawing;

namespace laba1
{
    public abstract class FilledShape : Shape
    {
        public Color ColorShape { get; set; }

        public FilledShape(Color color)
        {
            ColorShape = color;
        }
    }
}
