using ShapeEditor;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace PluginHexagon
{
    public class PluginHexagon : IShapePlugin
    {
        public string ShapeName => "Hexagon";
        public string Version => "4.8";

        public Dictionary<string, Type> GetParametersDescription()
        {
            return new Dictionary<string, Type>
            {
                { "CenterX", typeof(int) },
                { "CenterY", typeof(int) },
                { "Radius", typeof(int) },
                { "ColorShape", typeof(Color) }
            };
        }

        public IShape CreateShapeWithParameters(Dictionary<string, object> parameters)
        {
            return new Hexagon(
                (int)parameters["CenterX"],
                (int)parameters["CenterY"],
                (int)parameters["Radius"],
                (Color)parameters["ColorShape"]
            );
        }
    }

    [Serializable]
    public class Hexagon : FilledShape
    {
        public int CenterX { get; set; }
        public int CenterY { get; set; }
        public int Radius { get; set; }

        public Hexagon(int centerX, int centerY, int radius, Color color)
            : base(color)
        {
            CenterX = centerX;
            CenterY = centerY;
            Radius = radius;
        }

        public override void Draw(Graphics g)
        {
            Point[] points = CalculateHexagonPoints();

            using (var brush = new SolidBrush(ColorShape))
            {
                g.FillPolygon(brush, points);
            }
            g.DrawPolygon(Pens.Black, points);
        }

        private Point[] CalculateHexagonPoints()
        {
            Point[] points = new Point[6];
            for (int i = 0; i < 6; i++)
            {
                double angle = 2 * Math.PI / 6 * i;
                int x = CenterX + (int)(Radius * Math.Cos(angle));
                int y = CenterY + (int)(Radius * Math.Sin(angle));
                points[i] = new Point(x, y);
            }
            return points;
        }

        public override bool ContainsPoint(Point p)
        {
            int dx = p.X - CenterX;
            int dy = p.Y - CenterY;
            return dx * dx + dy * dy <= Radius * Radius;
        }

        public override string GetInformation()
        {
            return $"Center: ({CenterX}, {CenterY}), Radius: {Radius}, Color: {ColorShape}";
        }
    }
}