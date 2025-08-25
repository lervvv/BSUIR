using System;
using System.Windows.Forms;

namespace ShapeEditor
{
    internal static class Program
    {
        [STAThread] //указывает, что используется однопоточная модель
        static void Main()
        {
            Application.EnableVisualStyles();  //для стилей
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm()); //отображение формы
        }
    }
}
