using System;
using System.Drawing;
using System.Windows.Forms;

namespace ShapeEditor.FormsOfShapes
{
    public class SquareForm : Form
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; }
        public Color FillColor { get; private set; }

        public event Action OnDelete; //событие для удаления

        public SquareForm(int? x = null, int? y = null, int? width = null, Color? color = null)
        {
            Text = x.HasValue ? "Square" : "Сreating";
            Size = new Size(245, 220);
            StartPosition = FormStartPosition.CenterParent;

            Label lblX = new Label { Text = "X:", Location = new Point(50, 24), Width = 10 };
            TextBox txtX = new TextBox { Location = new Point(70, 20), Width = 100 };

            Label lblY = new Label { Text = "Y:", Location = new Point(50, 54), Width = 10 };
            TextBox txtY = new TextBox { Location = new Point(70, 50), Width = 100 };

            Label lblSide = new Label { Text = "Side:", Location = new Point(40, 84), Width = 35 };
            TextBox txtSide = new TextBox { Location = new Point(80, 80), Width = 100 };

            Button btnColor = new Button { Text = "Color", Location = new Point(75, 110) };
            ColorDialog colorDialog = new ColorDialog();
            btnColor.Click += (s, e) => { if (colorDialog.ShowDialog() == DialogResult.OK) FillColor = colorDialog.Color; };

            Button btnOk = new Button { Text = "OK", Location = x.HasValue ? new Point(20, 140) : new Point(75, 140) };

            //добавление элементов на форму
            Controls.AddRange(new Control[] { lblX, txtX, lblY, txtY, lblSide, txtSide, btnColor, btnOk });

            if (x.HasValue && y.HasValue && width.HasValue)
            {
                txtX.Text = x.ToString();
                txtY.Text = y.ToString();
                txtSide.Text = width.ToString();
                FillColor = color ?? Color.White;

                Button btnDelete = new Button { Text = "Delete", Location = new Point(130, 140), ForeColor = Color.Red };
                btnDelete.Click += (s, e) => { OnDelete?.Invoke(); Close(); };
                Controls.Add(btnDelete);
            }

            btnOk.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtX.Text) ||
                    string.IsNullOrWhiteSpace(txtY.Text) ||
                    string.IsNullOrWhiteSpace(txtSide.Text))
                {
                    MessageBox.Show("You haven't filled in all the fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    X = int.Parse(txtX.Text);
                    Y = int.Parse(txtY.Text);
                    Width = int.Parse(txtSide.Text);
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
