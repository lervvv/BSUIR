using System.Drawing;

namespace laba1
{
    public class Polygon : FilledShape
    {
        public Point[] Points { get; set; }

        public Polygon(Point[] points, Color colorShape) 
            : base(colorShape)
        {
            Points = points;
            ColorShape = colorShape;
        }

        public override void Draw(Graphics g)
        {
            using (Brush brush = new SolidBrush(ColorShape))
            {
                g.FillPolygon(brush, Points);
                g.DrawPolygon(Pens.Black, Points);
            }

        }
    }
}
