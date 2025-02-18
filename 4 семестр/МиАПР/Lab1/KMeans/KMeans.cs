using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KMeans
{
    public class KMeans
    {
        private Random random = new Random();

        private List<Point> points;
        
        private ConcurrentBag<KMeansClass> classes;

        private bool isRecalculateNecessary = false;

        public KMeans(IEnumerable<Point> points, int classesCount)
        {
            this.points = new List<Point>(points);
            classes = GetClassesWithRandomCenters(classesCount);
        }

        private ConcurrentBag<KMeansClass> GetClassesWithRandomCenters(int classesCount)
        {
            var result = new ConcurrentBag<KMeansClass>();
            var selectedCenters = new List<Point>();
            for (int i = 0; i < classesCount; i++)
            {
                result.Add(new KMeansClass(GetNextRandomCenter(selectedCenters)));
            }
            return result;
        }

        private Point GetNextRandomCenter(List<Point> selectedCenters)
        {

            Point centerCandidate;
            do
            {
                centerCandidate = this.points[random.Next(this.points.Count)];
            } while (selectedCenters.Contains(centerCandidate));
            return centerCandidate;
        }

        public ConcurrentBag<KMeansClass> GetReadyClasses()
        {
            do
            {
                isRecalculateNecessary = false;
                ClearClasses();
                AddPointsToClasses();
                ChangeClassCenters();
            } while (isRecalculateNecessary);
            return classes;
        }

        private void ClearClasses()
        {
            foreach (KMeansClass kMeansClass in classes)
            {
                kMeansClass.Points = new List<Point> { kMeansClass.Center };
            }
        }

        private void AddPointsToClasses()
        {
            foreach (var point in points)
            {
                AddPointToClass(point);
            }
        }

        private void AddPointToClass(Point point)
        {
            var minDifferentClass = GetMinDifferentClass(point);
            if (minDifferentClass != null)
            {
                minDifferentClass.Points.Add(point);
            }
        }

        private void ChangeClassCenters()
        {

            Parallel.ForEach(classes, kMeansClass =>
            {
                Point bestClassCenter = kMeansClass.GetBestClassCenter();
                if (bestClassCenter != kMeansClass.Center)
                {
                    kMeansClass.Center = bestClassCenter;
                    isRecalculateNecessary = true;
                }
            });
        }

        private KMeansClass GetMinDifferentClass(Point point)
        {
            double minDifferent = Double.MaxValue;
            KMeansClass minDifferentClass = null;

            foreach (KMeansClass kMeansClass in classes)
            {
                if (point == kMeansClass.Center) return null;
                double different = GetInstance(kMeansClass, point);
                if (different < minDifferent)
                {
                    minDifferent = different;
                    minDifferentClass = kMeansClass;
                }
            }
            return minDifferentClass;
        }

        private double GetInstance(KMeansClass kMeansClass, Point point)
        {
            return GetPointsInstance(kMeansClass.Center, point);
        }

        private double GetPointsInstance(Point point1, Point point2)
        {
            double xDifferent = point1.X - point2.X;
            double yDifferent = point1.Y - point2.Y;
            return Math.Sqrt(xDifferent * xDifferent + yDifferent * yDifferent);
        }

    }
}
