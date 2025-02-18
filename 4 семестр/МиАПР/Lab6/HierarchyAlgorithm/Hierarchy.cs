using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace HierarchyAlgorithm
{
    public abstract class Hierarchy
    {
        protected const int downSpace = 20;
        private int currentX;
        private List<Line> lines;
        private readonly int elementsCount;
        private readonly int width;
        public int XStep { get; set; }

        protected Hierarchy(int elementsCount, int width, int heght)
        {
            this.elementsCount = elementsCount;
            this.width = width;
            Height = heght;
            XStep = width/(elementsCount + 1);
            currentX = XStep/2;
        }

        protected int Height { get; private set; }
        public int YStep { get; set; }
        protected abstract int MinMaxStartValue { get; }

        public DrawingGroup GetDrawingGroup(List<List<int>> distanses,
            List<TableElement> tableElements)
        {
            lines = new List<Line>();
            var result = DrawHierachy(distanses, tableElements);
            return result;
        }

        private DrawingGroup DrawHierachy(List<List<int>> distanses,
            List<TableElement> tableElements)
        {
            CalculateLines(distanses, tableElements);
            YStep = GetYStep(lines);
            var result = GetDrawingHierarchy();
            result.Children.Add(DrawAxeses());
            return result;
        }

        protected abstract int GetYStep(List<Line> list);

        private void CalculateLines(List<List<int>> distanses, List<TableElement> tableElements)
        {
            for (var i = 0; i < elementsCount - 1; i++)
            {
                var minDifferent = GetMinDifferent(distanses, tableElements);
                var iElement = tableElements.First(x => x.Number == minDifferent.I);
                var jElement = tableElements.First(x => x.Number == minDifferent.J);
                var iPosition = GetStartPoint(iElement);
                var jPosition = GetStartPoint(jElement);

                lines.AddRange(DrawHierachyElement(iPosition, minDifferent.Distance, jPosition,
                    iElement, jElement, distanses[minDifferent.I][minDifferent.J]));

                tableElements.Remove(jElement);
                tableElements.Remove(iElement);
                iElement.Name = "";
                iElement.Position = new Point((jPosition.X + iPosition.X)/2, minDifferent.Distance);
                tableElements.Add(iElement);
                distanses = ClearDistances(distanses, minDifferent);
            }
        }

        private DrawingGroup GetDrawingHierarchy()
        {
            var result = new DrawingGroup();
            var geometryGroup = new GeometryGroup();
            var textGroup = new GeometryGroup();
            var horisontalGroup = new GeometryGroup();
            foreach (var line in lines)
            {
                line.From = new Point(line.From.X, GetCurrentY(line.From.Y) - downSpace);
                line.To = new Point(line.To.X, GetCurrentY(line.To.Y) - downSpace);
                if (Math.Abs(line.From.Y - line.To.Y) < 1)
                {
                    horisontalGroup.Children.Add(new LineGeometry(line.From, line.To));
                }
                else
                {
                    geometryGroup.Children.Add(new LineGeometry(line.From, line.To));
                }
                textGroup.Children.Add(GetTextGeometry(line.From + new Vector(-2, 5), line.XName));
                textGroup.Children.Add(GetTextGeometry(new Point(0, line.From.Y), line.YName));
            }
            result.Children.Add(new GeometryDrawing(new SolidColorBrush(Colors.Red), new Pen(new SolidColorBrush(Colors.Red),2), geometryGroup));
            result.Children.Add(new GeometryDrawing(new SolidColorBrush(Colors.Blue), new Pen(new SolidColorBrush(Colors.Blue), 2), horisontalGroup));
            result.Children.Add(new GeometryDrawing(MainBrush, MainPen, textGroup));
            return result;
        }

        private static Geometry GetTextGeometry(Point point, string textToFormat)
        {
            var text = new FormattedText(textToFormat,
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Arial"),
                10,
                Brushes.Black);
            return text.BuildGeometry(point);
        }

        protected abstract double GetCurrentY(double distance);

        private GeometryDrawing DrawAxeses()
        {
            var group = new GeometryGroup();
            group.Children.Add(new LineGeometry(new Point(0, Height - downSpace),
                new Point(width, Height - downSpace)));
            group.Children.Add(new LineGeometry(new Point(XStep/2, 0), new Point(XStep/2, Height)));
            return new GeometryDrawing(MainBrush, MainPen, group);
        }

        private List<List<int>> ClearDistances(IEnumerable<List<int>> elementDistanses,
            MinDifferent minDifferent)
        {
            var distanses = CopyDistances(elementDistanses);
            for (var i = 0; i < distanses.Count; i++)
            {
                if (i != minDifferent.I && i != minDifferent.J)
                {
                    var newDistance = GetMinMax(distanses[i][minDifferent.I],
                        distanses[i][minDifferent.J]);
                    distanses = SetDistanse(distanses, i, minDifferent.I, newDistance);
                    distanses = SetDistanse(distanses, i, minDifferent.J, 0);
                }
            }
            return SetDistanse(distanses, minDifferent.I, minDifferent.J, 0);
        }

        private static List<List<int>> CopyDistances(IEnumerable<List<int>> elementDistanses)
        {
            var distanses = new List<List<int>>();
            foreach (var elementDistanse in elementDistanses)
            {
                var newList = new int[elementDistanse.Count];
                elementDistanse.CopyTo(newList);
                distanses.Add(newList.ToList());
            }
            return distanses;
        }

        protected abstract int GetMinMax(int first, int second);

        private static List<List<int>> SetDistanse(List<List<int>> distanses, int i, int j,
            int newDistance)
        {
            distanses[i][j] = newDistance;
            distanses[j][i] = newDistance;
            return distanses;
        }

        private IEnumerable<Line> DrawHierachyElement(Point iPosition, int currentY,
            Point jPosition, TableElement iElement, TableElement jElement, int yValue)
        {
            return new List<Line>
            {
                new Line(iPosition, new Point(iPosition.X, currentY), iElement.Name, ""),
                new Line(jPosition, new Point(jPosition.X, currentY), jElement.Name, ""),
                new Line(new Point(iPosition.X, currentY), new Point(jPosition.X, currentY), "",
                    GetYString(yValue))
            };
        }

        protected abstract String GetYString(int distance);

        private Point GetStartPoint(TableElement element)
        {
            if (Math.Abs(element.Position.X) > 0.01
                && Math.Abs(element.Position.Y) > 0.01)
            {
                return element.Position;
            }
            currentX += XStep;
            return new Point(currentX - XStep, 0);
        }

        private MinDifferent GetMinDifferent(List<List<int>> distanses, List<TableElement> elements)
        {
            var minDifferent = new MinDifferent {Distance = MinMaxStartValue};
            for (var i = 0; i < distanses.Count; i++)
            {
                for (var j = i; j < distanses.Count; j++)
                {
                    if (distanses[i][j] != 0 &&
                        CompareMinMax(distanses[i][j], minDifferent.Distance))
                    {
                        minDifferent = SetMinDifferent(distanses, elements, i, j, minDifferent);
                    }
                }
            }
            return minDifferent;
        }

        private static MinDifferent SetMinDifferent(List<List<int>> distanses,
            List<TableElement> elements, int i, int j,
            MinDifferent minDifferent)
        {
            if (distanses[i][j] == minDifferent.Distance)
            {
                var iElement = elements.First(x => x.Number == i);
                var jElement = elements.First(x => x.Number == j);
                if (iElement.Name != "" || jElement.Name != "")
                {
                    minDifferent.I = i;
                    minDifferent.J = j;
                    minDifferent.Distance = distanses[i][j];
                }
            }
            else
            {
                minDifferent.I = i;
                minDifferent.J = j;
                minDifferent.Distance = distanses[i][j];
            }
            return minDifferent;
        }

        protected abstract bool CompareMinMax(int first, int second);

        private class MinDifferent
        {
            public MinDifferent()
            {
                I = 0;
                J = 0;
                Distance = Int32.MaxValue;
            }

            public int I { get; set; }
            public int J { get; set; }
            public int Distance { get; set; }
        }

        private static readonly Color MainColor = Colors.Black;
        private static readonly SolidColorBrush MainBrush = new SolidColorBrush(MainColor);
        private static readonly Pen MainPen = new Pen(MainBrush, 1);
    }
}