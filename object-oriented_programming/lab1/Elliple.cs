using System.Drawing;

namespace laba1
{
    public class Ellipse : FilledShape
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Ellipse(int x, int y, int width, int height, Color color)
            : base (color)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            ColorShape = color;
        }

        public override void Draw(Graphics g)
        {
            using (Brush brush = new SolidBrush(ColorShape))
            {
                g.FillEllipse(brush, X, Y, Width, Height);
                g.DrawEllipse(Pens.Black, X, Y, Width, Height);
            }
        }
    }
}
