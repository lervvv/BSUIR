using System;
using System.Collections.Generic;
using System.Drawing;

namespace ShapeEditor
{
    [Serializable]
    public class ShapeList
    {
        private List<IShape> shapes = new List<IShape>(); //список объектов типа Shape

        public void Add(IShape shape)
        {
            shapes.Add(shape);
        }

        public void Remove(IShape shape)
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

        public IEnumerable<IShape> GetShapes()
        {
            return shapes;
        }

        public void Clear()
        {
            shapes.Clear();
        }

    }
}
