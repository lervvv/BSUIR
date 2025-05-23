﻿using System.Drawing;

namespace laba1
{
    public class Rectangle : FilledShape
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Rectangle(int x, int y, int width, int height, Color color)
            : base(color) //конструктор класса для инициализации + вызов конструктора FilledShape для инициализации ColorShape
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public override void Draw(Graphics g)
        {
            using (var brush = new SolidBrush(ColorShape)) //создание кисти для заливки 
            {
                g.FillRectangle(brush, X, Y, Width, Height); //сама заливка
            }
            g.DrawRectangle(Pens.Black, X, Y, Width, Height); //отрисовка контура
        }
    }
}
