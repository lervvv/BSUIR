using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ShapeEditor.FormsOfShapes
{
    public class PolygonForm : Form
    {
        public List<Point> Points { get; private set; } = new List<Point>();
        public Color FillColor { get; private set; }

        public event Action OnDelete; //событие для удаления

        public PolygonForm(List<Point> points = null, Color? color = null)
        {
            Text = points != null ? "Polygon" : "Creating";
            Size = new Size(300, 320);
            StartPosition = FormStartPosition.CenterParent;

            //таблица для ввода координат
            DataGridView dgvPoints = new DataGridView
            {
                Location = new Point(20, 10),
                Size = new Size(243, 200),
                ColumnCount = 2,
                AllowUserToAddRows = true //для добавления новых строк
            };
            dgvPoints.Columns[0].Name = "X";
            dgvPoints.Columns[1].Name = "Y";

            Button btnColor = new Button { Text = "Color", Location = new Point(100, 220) };
            ColorDialog colorDialog = new ColorDialog();
            btnColor.Click += (s, e) => { if (colorDialog.ShowDialog() == DialogResult.OK) FillColor = colorDialog.Color; };

            Button btnOk = new Button { Text = "OK", Location = points != null ? new Point(40, 250) : new Point(100, 250) };

            Controls.AddRange(new Control[] { dgvPoints, btnOk, btnColor });

            if (points != null)
            {
                foreach (var point in points)
                    dgvPoints.Rows.Add(point.X, point.Y);

                Button btnDelete = new Button { Text = "Delete", Location = new Point(160, 250), ForeColor = Color.Red };
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
