using System.Drawing;

namespace laba1
{
    public class Polygon : FilledShape
    {
        public Point[] Points { get; set; }
        public Color ColorShape { get; set; }

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
        public override bool ContainsPoint(Point p)
        {
            bool inside = false;
            int n = Points.Length;
            for (int i = 0, j = n - 1; i < n; j = i++)
            {
                if (((Points[i].Y > p.Y) != (Points[j].Y > p.Y)) &&
                    (p.X < (Points[j].X - Points[i].X) * (p.Y - Points[i].Y) / (Points[j].Y - Points[i].Y) + Points[i].X))
                {
                    inside = !inside;
                }
            }
            return inside;
        }
        public override string GetInformation()
        {
            string details = "Points: ";
            foreach (var point in Points)
            {
                details += $"({point.X}, {point.Y}) ";
            }
            details += $"Color: {ColorShape}";
            return details.Trim();
        }
    }
}
