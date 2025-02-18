using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using HierarchyAlgorithm;

namespace WpfApplication
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public List<List<int>> Distanses { get; set; }
        private Hierarchy hierarchy = null;

        private void SetGreadView()
        {
            Distanses = new List<List<int>>();
            for (var i = 0; i < ElementsCountUpDown.Value; i++)
            {
                Distanses.Add(new List<int>());
                for (var j = 0; j < ElementsCountUpDown.Value; j++)
                {
                    Distanses[i].Add(0);
                }
            }
            DistancesDataGridView.ItemsSource2D = Distanses;
        }

        private void RamdomDistanceButton_Click(object sender, RoutedEventArgs e)
        {
            SetRandom();
        }

        private void SetRandom()
        {
            Distanses = new List<List<int>>(Distanses);
            var random = new Random();
            for (var i = 0; i < ElementsCountUpDown.Value; i++)
            {
                for (var j = 0; j < ElementsCountUpDown.Value; j++)
                {
                    if (i == j)
                    {
                        Distanses[j][i] = 0;
                    }
                    else
                    {
                        Distanses[j][i] =
                            random.Next(1, (ElementsCountUpDown.Value ?? 0)*4);
                        Distanses[i][j] = Distanses[j][i];
                    }
                }
            }
            DistancesDataGridView.ItemsSource2D = Distanses;
        }

        private void ElementsCountUpDown_ValueChanged(object sender,
            RoutedPropertyChangedEventArgs<object> e)
        {
            SetGreadView();
        }

        private void DrawDistanceButton_Click(object sender, RoutedEventArgs e)
        {
            Draw();
        }

        private void Draw()
        {
            var tableElements = new List<TableElement>();
            for (var i = 0; i < ElementsCountUpDown.Value; i++)
            {
                tableElements.Add(new TableElement(i));
            }
            if (!IsDistancesArrayValid(Distanses))
            {
                MessageBox.Show(
                    "Таблица расстояний не симметрична и/или на главной диагонали не стоят 0");
                return;
            }

            if (MinimumRadioButton.IsChecked ?? false)
            {
                hierarchy = new MinHierarchy(ElementsCountUpDown.Value ?? 0, 355, 400);
            }
            else if (MaximumRadioButton.IsChecked ?? false)
            {
                hierarchy = new MaxHierarchy(ElementsCountUpDown.Value ?? 0, 355, 400);
            }
            else
            {
                hierarchy = new InvertMaxHierarchy(ElementsCountUpDown.Value ?? 0, 355, 400);
            }
            ResultImage.Source =
                new DrawingImage(hierarchy.GetDrawingGroup(Distanses, tableElements));
        }

        private bool IsDistancesArrayValid(List<List<int>> distances)
        {
            var result = true;
            for (var i = 0; i < ElementsCountUpDown.Value; i++)
            {
                for (var j = 0; j < ElementsCountUpDown.Value; j++)
                {
                    if (distances[i][j] != distances[j][i]) result = false;
                    if (i == j && distances[i][j] != 0) result = false;
                }
            }
            return result;
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //Если нажата F2 -- заполнить таблицу
            if (e.Key == Key.F2)
            {
                SetRandom();
                Draw();

            }
        }

        //При выборе нового типа для графика -- отрисовать, но только если таблица уже создана
        private void MinimumRadioButton_OnChecked(object sender, RoutedEventArgs e)
        {
            if(Distanses != null && Distanses.Count > 1)
                Draw();
        }

        //Происходит при попытке изменить значение. Если изменяется значение на диагонали -- запретить
        private void DistancesDataGridView_OnBeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (e.Column.Header.ToString() == e.Row.Header.ToString())
            {
                e.Cancel = true;
            }
        }
        
        //Происходит по окончении ввода нового значения. Если ячейка не на диагонали -- заполнить симметричную так же
        private void DistancesDataGridView_OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (!e.Cancel)
            {
                Distanses[int.Parse(e.Column.Header.ToString())][int.Parse(e.Row.Header.ToString())] =
                    int.Parse( ((TextBox)e.EditingElement).Text);
                Distanses[int.Parse(e.Row.Header.ToString())][int.Parse(e.Column.Header.ToString())] =
                  int.Parse(((TextBox)e.EditingElement).Text);

                // Скоироват новые значения расстояний в новый список
               var  newDistanses = new List<List<int>>();
                for (var i = 0; i < ElementsCountUpDown.Value; i++)
                {
                    newDistanses.Add(new List<int>());
                    for (var j = 0; j < ElementsCountUpDown.Value; j++)
                    {
                        newDistanses[i].Add(Distanses[i][j]);
                    }
                }
                Distanses = newDistanses;
            }

        }

        //По гажатию Enterизменяем значене в таблице
        private void DistancesDataGridView_OnKeyUp(object sender, KeyEventArgs e)
        {
           DistancesDataGridView.ItemsSource2D = Distanses;
        }

        private void ResultImage_OnMouseMove(object sender, MouseEventArgs e)
        {
           
        }
    }
}