using System;
using System.Drawing;

namespace ShapeEditor
{
    [Serializable]
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
        public override bool ContainsPoint(Point p)
        {
            //вычисляем центр эллипса
            double centerX = X + Width / 2.0;
            double centerY = Y + Height / 2.0;

            double normX = (p.X - centerX) / (Width / 2.0);
            double normY = (p.Y - centerY) / (Height / 2.0);

            //по формуле вычисляем находится ли точка в нем
            return (normX * normX + normY * normY) <= 1;
        }
        public override string GetInformation()
        {
            return $"X: {X}, Y: {Y}, Width: {Width}, Height: {Height}, Color: {ColorShape}";
        }
    }
}
