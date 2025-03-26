using System.Collections.Generic;
using System.Drawing;

namespace laba1
{
    public class ShapeList
    {
        private List<Shape> shapes = new List<Shape>(); //список объектов типа Shape

        public void Add(Shape shape)
        {
            shapes.Add(shape);
        }

        public void Remove(Shape shape)
        {
            shapes.Remove(shape);
        }

        public void DrawAll(Graphics g)
        {
            foreach (var shape in shapes)
            {
                shape.Draw(g);
            }
        }

        public IEnumerable<Shape> GetShapes()
        {
            return shapes;
        }
    }
}
