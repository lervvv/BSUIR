using System;
using System.Drawing;
using System.Windows.Forms;

namespace laba1.FormsOfShapes
{
    public class LineForm : Form
    {
        public int X1 { get; private set; }
        public int Y1 { get; private set; }
        public int X2 { get; private set; }
        public int Y2 { get; private set; }

        public event Action OnDelete;

        public LineForm(int? x1 = null, int? y1 = null, int? x2 = null, int? y2 = null)
        {
            Text = x1.HasValue ? "Line" : "Сreating";
            Size = new Size(245, 250);
            StartPosition = FormStartPosition.CenterParent;

            Label lblX1 = new Label { Text = "X1:", Location = new Point(45, 24), Width = 20 };
            TextBox txtX1 = new TextBox { Location = new Point(70, 20), Width = 100 };

            Label lblY1 = new Label { Text = "Y1:", Location = new Point(45, 54), Width = 20 };
            TextBox txtY1 = new TextBox { Location = new Point(70, 50), Width = 100 };

            Label lblX2 = new Label { Text = "X2:", Location = new Point(45, 84), Width = 20 };
            TextBox txtX2 = new TextBox { Location = new Point(70, 80), Width = 100 };

            Label lblY2 = new Label { Text = "Y2:", Location = new Point(45, 114), Width = 20 };
            TextBox txtY2 = new TextBox { Location = new Point(70, 110), Width = 100 };

            Button btnOk = new Button { Text = "OK", Location = x1.HasValue? new Point(20, 170) : new Point(75, 170) };

            //добавление элементов на форму
            Controls.AddRange(new Control[] { lblX1, txtX1, lblY1, txtY1, lblX2, txtX2, lblY2, txtY2, btnOk });

            if (x1.HasValue && y1.HasValue && x2.HasValue && y2.HasValue)
            {
                txtX1.Text = x1.ToString();
                txtY1.Text = y1.ToString();
                txtX2.Text = x2.ToString();
                txtY2.Text = y2.ToString();

                Button btnDelete = new Button { Text = "Delete", Location = new Point(130, 170), ForeColor = Color.Red };
                btnDelete.Click += (s, e) => { OnDelete?.Invoke(); Close(); };
                Controls.Add(btnDelete);
            }

            btnOk.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtX1.Text) ||
                    string.IsNullOrWhiteSpace(txtY1.Text) ||
                    string.IsNullOrWhiteSpace(txtX2.Text) ||
                    string.IsNullOrWhiteSpace(txtY2.Text))
                {
                    MessageBox.Show("You haven't filled in all the fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    X1 = int.Parse(txtX1.Text);
                    Y1 = int.Parse(txtY1.Text);
                    X2 = int.Parse(txtX2.Text);
                    Y2 = int.Parse(txtY2.Text);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                catch
                {
                    MessageBox.Show("Enter the correct numeric values!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
        }
    }
}
