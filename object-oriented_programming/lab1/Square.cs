using System.Drawing;

namespace laba1
{
    public class Square : Rectangle
    {
        public Square(int x, int y, int sideLength, Color color)
        : base(x, y, sideLength, sideLength, color) { } //вызов конструктора базового класса прямоугольника
    }
}
