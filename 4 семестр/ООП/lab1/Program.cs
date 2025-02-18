using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace lab1
{
    public partial class Form1 : Form
    {
        private ShapeList shapeList = new ShapeList();
        


        private void Loading(object sender, EventArgs e)
        {
            

            // Добавление фигур при загрузке формы
            shapeList.Add(new Line(50, 50, 100, 100));
            shapeList.Add(new Rectangle(50, 200, 150, 100, Color.Green));
            shapeList.Add(new Ellipse(300, 200, 80, 150, Color.Blue));

            Point[] PolyLinesPoints = { new Point(400, 100), new Point(450, 50), new Point(500, 100), new Point(600, 50) };
            shapeList.Add(new PolyLine(PolyLinesPoints));

            Point[] PolygonPoints = { new Point(550, 300), new Point(650, 300), new Point(700, 200)};
            shapeList.Add(new Polygon(PolygonPoints, Color.Red));

            // Перерисовка формы
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            shapeList.DrawAll(e.Graphics);
        }
    }

    // Определения классов Shape, Line, Rectangle и Ellipse остаются здесь

    public abstract class Shape
    {
        public abstract void Draw(Graphics g);
    }

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

        public override void Draw(Graphics g)
        {
            g.DrawLine(Pens.Black, X1, Y1, X2, Y2);
        }

       
    }

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

        public class Rectangle : Shape
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Color ColorShape { get; set; }

        public Rectangle(int x, int y, int width, int height, Color color)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            ColorShape = color;
        }

        public override void Draw(Graphics g)
        {
            using (var brush = new SolidBrush(ColorShape))
            g.FillRectangle(brush, X, Y, Width, Height);
            g.DrawRectangle(Pens.Black, X, Y, Width, Height);
        }
    }

    public class Ellipse : Shape
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Color ColorShape { get; set; }

        public Ellipse(int x, int y, int width, int height, Color color)
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

    public class Polygon : Shape
    {
        public Point[] Points { get; set; }
        public Color ColorShape { get; set; }

        public Polygon(Point[] points, Color colorShape)
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

    public class ShapeList
    {
        private List<Shape> shapes = new List<Shape>();

        public void Add(Shape shape)
        {
            shapes.Add(shape);
        }

        public void DrawAll(Graphics g)
        {
            foreach (var shape in shapes)
            {
                shape.Draw(g);
            }
        }
    }

    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
