using System;
using System.Windows;

namespace HierarchyAlgorithm
{
    public class Line
    {
        public Line(Point from, Point to, String xName, String yName)
        {
            From = from;
            To = to;
            XName = xName;
            YName = yName;
        }

        public Point From { get; set; }
        public Point To { get; set; }
        public String XName { get; set; }
        public String YName { get; set; }
    }
}