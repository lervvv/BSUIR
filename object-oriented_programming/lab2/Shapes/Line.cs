using System;
using System.Drawing;

namespace laba1
{
    public class Line : Shape
    {
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }

        public Line(int x1, int y1, int x2, int y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        public void Draw(Graphics g)
        {
            g.DrawLine(Pens.Black, X1, Y1, X2, Y2);
        }

        public bool ContainsPoint(Point p)
        {
            const double MAXSIZE = 5.0; //макс порог попадания в пикселях

            double area = Math.Abs((X2 - X1) * (p.Y - Y1) - (p.X - X1) * (Y2 - Y1)) /
                          Math.Sqrt((X2 - X1) * (X2 - X1) + (Y2 - Y1) * (Y2 - Y1));

            return area <= MAXSIZE;
        }
        public string GetInformation()
        {
            return $"X: {X1}, Y: {Y1}, X2: {X2}, Y2: {Y2}";
        }

    }
}
