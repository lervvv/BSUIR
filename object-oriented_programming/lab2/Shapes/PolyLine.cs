using System;
using System.Drawing;

namespace laba1
{
    public class PolyLine : Shape
    {
        public Point[] Points { get; set; }

        public PolyLine(Point[] point)
        {
            Points = point;
        }

        public void Draw(Graphics g)
        {
            g.DrawLines(Pens.Black, Points);
        }

        public bool ContainsPoint(Point p)
        {
            const double MAXSIZE = 5.0; //макс порог попадания в пикселях

            for (int i = 0; i < Points.Length - 1; i++) //перебор всех координат ломаной
            {
                double area = Math.Abs((Points[i + 1].X - Points[i].X) * (p.Y - Points[i].Y) -
                                       (p.X - Points[i].X) * (Points[i + 1].Y - Points[i].Y)) /
                              Math.Sqrt((Points[i + 1].X - Points[i].X) * (Points[i + 1].X - Points[i].X) +
                                        (Points[i + 1].Y - Points[i].Y) * (Points[i + 1].Y - Points[i].Y));

                if (area <= MAXSIZE)
                    return true;
            }
            return false;
        }

        public string GetInformation()
        {
            string details = "Points: ";
            foreach (var point in Points)
            {
                details += $"({point.X}, {point.Y}) ";
            }
            return details.Trim();
        }
    }
}
