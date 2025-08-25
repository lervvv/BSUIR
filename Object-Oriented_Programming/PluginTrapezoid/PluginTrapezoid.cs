using ShapeEditor;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace PluginTrapezoid
{
    public class PluginTrapezoid : IShapePlugin
    {
        public string ShapeName => "Trapezoid";
        public string Version => "4.8";

        public Dictionary<string, Type> GetParametersDescription()
        {
            return new Dictionary<string, Type>
            {
                { "X", typeof(int) },
                { "Y", typeof(int) },
                { "TopWidth", typeof(int) },
                { "BottomWidth", typeof(int) },
                { "Height", typeof(int) },
                { "ColorShape", typeof(Color) }
            };
        }

        public IShape CreateShapeWithParameters(Dictionary<string, object> parameters)
        {
            return new Trapezoid(
                (int)parameters["X"],
                (int)parameters["Y"],
                (int)parameters["TopWidth"],
                (int)parameters["BottomWidth"],
                (int)parameters["Height"],
                (Color)parameters["ColorShape"]
            );
        }
    }

    [Serializable]
    public class Trapezoid : FilledShape
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int TopWidth { get; set; }
        public int BottomWidth { get; set; }
        public int Height { get; set; }

        public Trapezoid(int x, int y, int topWidth, int bottomWidth, int height, Color color)
            : base(color) //инициализирует ColorShape в FilledShape
        {
            X = x;
            Y = y;
            TopWidth = topWidth;
            BottomWidth = bottomWidth;
            Height = height;
        }

        public override void Draw(Graphics g)
        {
            Point[] points = new Point[]
            {
                new Point(X, Y),
                new Point(X + TopWidth, Y),
                new Point(X + (TopWidth + BottomWidth) / 2, Y + Height),
                new Point(X - (BottomWidth - TopWidth) / 2, Y + Height)
            };

            using (var brush = new SolidBrush(ColorShape))
            {
                g.FillPolygon(brush, points);
            }
            g.DrawPolygon(Pens.Black, points);
        }

        public override bool ContainsPoint(Point p)
        {
            return p.X >= X && p.X <= X + TopWidth && p.Y >= Y && p.Y <= Y + Height;
        }

        public override string GetInformation()
        {
            return $"X: {X}, Y: {Y}, TopWidth: {TopWidth}, BottomWidth: {BottomWidth}, Height: {Height}, Color: {ColorShape}";
        }
    }
}