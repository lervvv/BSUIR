using System.Drawing;
using System.Windows.Forms;

namespace laba1
{
    public class MainForm : Form
    {
        private ShapeList shapeList = new ShapeList();

        public MainForm()
        {
            this.ClientSize = new Size(800, 600);

            shapeList.Add(new Line(50, 50, 50, 100));
            shapeList.Add(new Rectangle(200, 30, 200, 80, Color.CornflowerBlue));
            shapeList.Add(new Square(450, 30, 80, Color.DarkMagenta));
            shapeList.Add(new Ellipse(50, 200, 80, 120, Color.Firebrick));
            shapeList.Add(new Circle(200, 200, 80, Color.Coral));
            
            Point[] PolyLinesPoints = { new Point(100, 50), new Point(130, 100), new Point(160, 30), new Point(160, 100) };
            shapeList.Add(new PolyLine(PolyLinesPoints));

            Point[] PolygonPoints = { new Point(500, 200), new Point(400, 250), new Point(500, 150)};
            shapeList.Add(new Polygon(PolygonPoints, Color.BlanchedAlmond));

            this.Paint += MainForm_Paint; //добавляем метод для отрисовки
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            shapeList.DrawAll(e.Graphics); //отрисовка фигур из контейнера
        }
    }
}
