using System;
using System.Drawing;

namespace ShapeEditor
{
    [Serializable]
    public class Square : Rectangle
    {
        public Square(int x, int y, int sideLength, Color color)
        : base(x, y, sideLength, sideLength, color) { } //вызов конструктора базового класса прямоугольника
    }
}
