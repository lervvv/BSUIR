using Algorithms;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;


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
            IEnumerable<Point> points = GetRandomPoints(PointsCountInput.Value ?? 0,(int)this.ActualHeight - imageTop,
                (int)this.ActualWidth);
            var maxMin = new Algorithms.MaxMin(points);
            List<Algorithms.PointsClass> result = maxMin.GetReadyClasses();
            Draw(result);


            MessageBox.Show("Нажмите OK, для прохода алгоритмом k-means");

            var kMeans = new Algorithms.KMeans(points,result);
            result = kMeans.GetReadyClasses();
            Draw(result);
        }

        private void Draw(List<PointsClass> result)
        {
            var drawingGroup = new DrawingGroup();
            int colorStep = 256*256*256/(result.Count);
            for (int i = 0; i < result.Count; i++)
            {
                DrawClass(i, colorStep, drawingGroup, result[i]);
            }


            MainImage.Source = new DrawingImage(drawingGroup);
        }

        private void DrawClass( int i, int colorStep, DrawingGroup drawingGroup, PointsClass kMeansClass)
        {
            var ellipses = new GeometryGroup();

            foreach (var point in kMeansClass.Points)
            {
                int pointSize = point == kMeansClass.Center ? 7 : 1;
                ellipses.Children.Add(new EllipseGeometry(point, pointSize, pointSize));
            }

            Color classColor = GetColorFromNumber(i*colorStep);
            var brush = new SolidColorBrush(classColor);
            var geometryDrawing = new GeometryDrawing(brush, new Pen(brush, 1), ellipses);
            geometryDrawing.Geometry = ellipses;
            drawingGroup.Children.Add(geometryDrawing);
        }

        private Color GetColorFromNumber(int number)
        {
            byte blue = (byte)  (number & 0x000000FF);
            byte green = (byte) ((number & 0x0000FF00) >> 8);
            byte red = (byte)   ((number & 0x00FF0000) >> 16);
            return  Color.FromRgb(red,green,blue);
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
