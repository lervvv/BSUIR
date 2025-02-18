using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HierarchyAlgorithm;

namespace Lab6
{
    public partial class Form1 : Form
    {
        public Form1()
        { 
            InitializeComponent();
            SetGreadView();
        }

        private void SetGreadView()
        {
            distancesDataGridView.RowCount = (int) elementsCountNumericUpDown.Value;
            distancesDataGridView.ColumnCount = (int)elementsCountNumericUpDown.Value;
            for (int i = 0; i < (int)elementsCountNumericUpDown.Value; i++)
            {
                distancesDataGridView.Columns[i].HeaderText = "x" + (i + 1);
                distancesDataGridView.Rows[i].HeaderCell.Value = (i + 1).ToString();
                distancesDataGridView.Columns[i].Width = 50;
            }
        }

        private void elementsCountNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            SetGreadView();
        }

        private int[][] GetDistances()
        {
            var result = new int[(int)elementsCountNumericUpDown.Value][];
            for (int i = 0; i < (int)elementsCountNumericUpDown.Value; i++)
            {
                result[i] = new int[(int)elementsCountNumericUpDown.Value];
                for (int j = 0; j < (int) elementsCountNumericUpDown.Value; j++)
                {
                    if (distancesDataGridView[j, i].Value != null)
                        result[i][j] = Int32.Parse(distancesDataGridView[j, i].Value.ToString());
                    else
                        result[i][j] = 0;
                }
            }
            return result;
        }

        private bool IsDistancesArrayValid(int[][] distances)
        {
            bool result = true;
            for (int i = 0; i < (int)elementsCountNumericUpDown.Value; i++)
            {
                for (int j = 0; j < (int)elementsCountNumericUpDown.Value; j++)
                {
                    if (distances[i][j] != distances[j][i]) result = false;
                    if (i == j && distances[i][j] != 0) result = false;
                }
            }
            return result;
        }

        private void randomValuesButton_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            for (int i = 0; i < (int)elementsCountNumericUpDown.Value; i++)
            {
                for (int j = 0; j < (int)elementsCountNumericUpDown.Value; j++)
                {
                    if (i == j)
                    {
                        distancesDataGridView[j,i].Value = 0;
                    }
                    else
                    {
                        distancesDataGridView[j,i].Value =
                            random.Next(1,(int) elementsCountNumericUpDown.Value*4);
                        distancesDataGridView[i,j].Value = distancesDataGridView[j,i].Value;
                    }
                }
            }
        }

        private void drawButton_Click(object sender, EventArgs e)
        {
            int[][] distances = GetDistances();
            var tableElements = new List<TableElement>();
            for (int i = 0; i < (int)elementsCountNumericUpDown.Value; i++)
            {
                tableElements.Add(new TableElement(i));
            }
            if (!IsDistancesArrayValid(distances))
            {
                MessageBox.Show(
                    "Таблица расстояний не симметрична и/или на главной диагонали не стоят 0");
                return;
            }
            
            /*Hierarchy hierarchy;
            if (minimumRadioButton.Checked)
            {
                hierarchy = new MinHierarchy((int) elementsCountNumericUpDown.Value,
                    pictureBox1.Width,
                    pictureBox1.Height);
            }
            else
            {
                hierarchy = new MaxHierarchy((int)elementsCountNumericUpDown.Value,
                    pictureBox1.Width,
                    pictureBox1.Height);
            }
            pictureBox1.Image = hierarchy.GetDrawingGroup(distances, tableElements);*/
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
