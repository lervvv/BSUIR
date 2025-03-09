using System.Drawing;

namespace laba1
{
    public class PolyLine : Shape
    {
        public Point[] Points { get; set; }
        public Color ColorShape { get; set; }

        public PolyLine(Point[] point)
        {
            Points = point;
        }

        public override void Draw(Graphics g)
        {
            g.DrawLines(Pens.Black, Points);
        }
    }
}
