using System;
using System.Windows;

namespace HierarchyAlgorithm
{
    public class TableElement
    {
        public TableElement(int number, Point position, String name)
        {
            Number = number;
            Position = position;
            Name = name;
        }

        public TableElement(int number)
            : this(number, new Point(0, 0), string.Format("x{0}", number))
        {
        }

        public int Number { get; set; }
        public Point Position { get; set; }
        public String Name { get; set; }
    }
}