using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace Algorithms
{
    public class KMeans : AlgorithmBase
    {
        private readonly Random random = new Random();

        private bool isRecalculateNecessary = false;

        public KMeans(IEnumerable<Point> points, int classesCount)
        {
            Points = new List<Point>(points);
            Classes = GetClassesWithRandomCenters(classesCount);
        }

        public KMeans(IEnumerable<Point> points, IEnumerable<PointsClass> classes)
        {
            Points = new List<Point>(points);
            Classes = new List<PointsClass>(classes);
        }

        private List<PointsClass> GetClassesWithRandomCenters(int classesCount)
        {
            var result = new List<PointsClass>();
            var selectedCenters = new List<Point>();
            for (int i = 0; i < classesCount; i++)
            {
                result.Add(new PointsClass(GetNextRandomCenter(selectedCenters)));
            }
            return result;
        }

        private Point GetNextRandomCenter(List<Point> selectedCenters)
        {

            Point centerCandidate;
            do
            {
                centerCandidate = this.Points[random.Next(this.Points.Count)];
            } while (selectedCenters.Contains(centerCandidate));
            return centerCandidate;
        }

        public List<PointsClass> GetReadyClasses()
        {
            do
            {
                isRecalculateNecessary = false;
                ClearClasses();
                AddPointsToClasses();
                ChangeClassCenters();
            } while (isRecalculateNecessary);
            return Classes;
        }

        private void ChangeClassCenters()
        {

            Parallel.ForEach(Classes, kMeansClass =>
            {
                Point bestClassCenter = kMeansClass.GetBestClassCenter();

                if (bestClassCenter != kMeansClass.Center)
                {
                    kMeansClass.Center = bestClassCenter;
                    isRecalculateNecessary = true;
                }
            });
        }





    }
}
