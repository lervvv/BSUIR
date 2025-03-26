using System.Drawing;

namespace laba1
{
    public class Circle : Ellipse
    {
        public Circle(int x, int y, int radius, Color color)
            : base(x, y, radius * 2, radius * 2, color) { }
    }
}
