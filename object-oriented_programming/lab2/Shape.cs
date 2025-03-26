using System.Drawing;

namespace laba1
{
    public interface Shape
    {
        void Draw(Graphics g);
        bool ContainsPoint(Point p);
        string GetInformation();
    }
}
