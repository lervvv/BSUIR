using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KMeans;


namespace Lab1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int imageTop = 40;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<Point> points = GetRandomPoints(PointsCountInput.Value ?? 0, (int)this.ActualHeight - imageTop,
                (int)this.ActualWidth);
            var kMeans = new KMeans.KMeans(points, ClassesCountInput.Value ?? 0);
            List<KMeans.KMeansClass> result = kMeans.GetReadyClasses().ToList();

            var drawingGroup = new DrawingGroup();
            int colorStep = 256 * 256 * 256 / (ClassesCountInput.Value) ?? 0;
            for (int i = 0; i < result.Count; i++)
            {
                DrawClass(i, colorStep, drawingGroup, result[i]);
            }


            MainImage.Source = new DrawingImage(drawingGroup);
        }

        private void DrawClass(int i, int colorStep, DrawingGroup drawingGroup, KMeansClass kMeansClass)
        {
            var ellipses = new GeometryGroup();

            foreach (var point in kMeansClass.Points)
            {
                int pointSize = point == kMeansClass.Center ? 7 : 1;
                ellipses.Children.Add(new EllipseGeometry(point, pointSize, pointSize));
            }

            Color classColor = GetColorFromNumber(i * colorStep);
            var brush = new SolidColorBrush(classColor);
            var geometryDrawing = new GeometryDrawing(brush, new Pen(brush, 1), ellipses);
            geometryDrawing.Geometry = ellipses;
            drawingGroup.Children.Add(geometryDrawing);
        }

        private Color GetColorFromNumber(int number)
        {
            byte blue = (byte)(number & 0x000000FF);
            byte green = (byte)((number & 0x0000FF00) >> 8);
            byte red = (byte)((number & 0x00FF0000) >> 16);
            return Color.FromRgb(red, green, blue);
        }

        private List<Point> GetRandomPoints(int pointsCount, int height, int width)
        {
            var result = new List<Point>(pointsCount);
            var random = new Random();
            for (int i = 0; i < pointsCount; i++)
            {
                result.Add(new Point(random.Next(width), random.Next(height)));
            }
            return result;
        }
    }
}
