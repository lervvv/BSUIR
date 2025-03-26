using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace laba1.FormsOfShapes
{
    public class PolylineForm : Form
    {
        public List<Point> Points { get; private set; } = new List<Point>();

        public event Action OnDelete; //событие для удаления

        public PolylineForm(List<Point> points = null)
        {
            Text = points != null ? "Polyline" : "Creating";
            Size = new Size(300, 300);
            StartPosition = FormStartPosition.CenterParent;

            //таблица для ввода координат
            DataGridView dgvPoints = new DataGridView
            {
                Location = new Point(10, 10),
                Size = new Size(260, 200),
                ColumnCount = 2,
                AllowUserToAddRows = true //для возможности добавления новых строк
            };
            dgvPoints.Columns[0].Name = "X";
            dgvPoints.Columns[1].Name = "Y";

            Button btnOk = new Button { Text = "OK", Location = points != null ? new Point(20, 220) : new Point(100, 220) };

            Controls.AddRange(new Control[] { dgvPoints, btnOk });

            if (points != null)
            {
                foreach (var point in points)
                    dgvPoints.Rows.Add(point.X, point.Y);

                Button btnDelete = new Button { Text = "Delete", Location = new Point(150, 220), ForeColor = Color.Red };
                btnDelete.Click += (s, e) => { OnDelete?.Invoke(); Close(); };
                Controls.Add(btnDelete);
            }

            btnOk.Click += (s, e) =>
            {
                Points.Clear();
                try
                {
                    foreach (DataGridViewRow row in dgvPoints.Rows)
                    {
                        if (row.IsNewRow) continue; //пропуск пустой строки

                        int x = int.Parse(row.Cells[0].Value.ToString());
                        int y = int.Parse(row.Cells[1].Value.ToString());
                        Points.Add(new Point(x, y));
                    }

                    if (Points.Count < 2)
                    {
                        MessageBox.Show("Enter at least two points!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    DialogResult = DialogResult.OK;
                    Close();
                }
                catch
                {
                    MessageBox.Show("Invalid input! Enter only numbers.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
        }
    }
}
