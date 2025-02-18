using System;
using System.Collections.Generic;
using System.Windows;

namespace Algorithms
{
    public abstract class AlgorithmBase
    {

        protected List<Point> Points;

        protected List<PointsClass> Classes;

        protected void ClearClasses()
        {
            foreach (PointsClass pointsClass in Classes)
            {
                pointsClass.Points = new List<Point> { pointsClass.Center };
            }
        }

        protected void AddPointsToClasses()
        {
            foreach (var point in Points)
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

        private PointsClass GetMinDifferentClass(Point point)
        {
            double minDifferent = Double.MaxValue;
            PointsClass minDifferentClass = null;

            foreach (PointsClass pointsClass in Classes)
            {
                if (point == pointsClass.Center) return null;
                double different = GetInstance(pointsClass, point);
                if (different < minDifferent)
                {
                    minDifferent = different;
                    minDifferentClass = pointsClass;
                }
            }
            return minDifferentClass;
        }

        private double GetInstance(PointsClass pointClass, Point point)
        {
            return GetPointsInstance(pointClass.Center, point);
        }

        protected double GetPointsInstance(Point point1, Point point2)
        {
            return Math.Sqrt(Math.Pow( point1.X - point2.X,2) + Math.Pow( point1.Y - point2.Y,2));
        }
    }
}
