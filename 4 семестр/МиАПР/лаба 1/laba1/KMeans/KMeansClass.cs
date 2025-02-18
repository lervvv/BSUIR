using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace KMeans
{
    public class KMeansClass
    {
        public List<Point> Points { get; set; }

        public Point Center { get; set; }

        public KMeansClass(Point center)
        {
            Points = new List<Point>();
            Points.Add(center);
            Center = center;
        }


        public Point GetBestClassCenter()
        {
            var bestCenter = new Point(this.Points.Average(x => x.X), this.Points.Average(x => x.Y));
            double minDifferent = Double.MaxValue;
            var minDifferentPoint = new Point();
            foreach (Point centerCandidate in this.Points)
            {
                double differen = GetPointsInstance(bestCenter, centerCandidate);
                if (differen < minDifferent)
                {
                    minDifferent = differen;
                    minDifferentPoint = centerCandidate;
                }
            }
            return minDifferentPoint;
        }

        private double GetPointsInstance(Point point1, Point point2)
        {
            double xDifferent = point1.X - point2.X;
            double yDifferent = point1.Y - point2.Y;
            return Math.Sqrt(xDifferent * xDifferent + yDifferent * yDifferent);
        }
    }
}
