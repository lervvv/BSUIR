using System;
using System.Drawing;
using System.Windows.Forms;

namespace laba1
{
    public class RectangleForm : Form
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Color FillColor { get; private set; }

        public event Action OnDelete; //событие для удаления

        public RectangleForm(int? x = null, int? y = null, int? width = null, int? height = null, Color? color = null)
        {
            Text = x.HasValue? "Rectangle" : "Сreating";
            Size = new Size(245, 250);
            StartPosition = FormStartPosition.CenterParent;

            Label lblX = new Label { Text = "X:", Location = new Point(50, 24), Width = 10 };
            TextBox txtX = new TextBox { Location = new Point(70, 20), Width = 100 };

            Label lblY = new Label { Text = "Y:", Location = new Point(50, 54), Width = 10 };
            TextBox txtY = new TextBox { Location = new Point(70, 50), Width = 100 };

            Label lblWidth = new Label { Text = "Width:", Location = new Point(40, 84) , Width = 35 };
            TextBox txtWidth = new TextBox { Location = new Point(80, 80), Width = 100 };

            Label lblHeight = new Label { Text = "Height:", Location = new Point(40, 114), Width = 35 };
            TextBox txtHeight = new TextBox { Location = new Point(80, 110), Width = 100 };

            Button btnColor = new Button { Text = "Color", Location = new Point(75, 140) };
            ColorDialog colorDialog = new ColorDialog();
            btnColor.Click += (s, e) => 
            { 
                if (colorDialog.ShowDialog() == DialogResult.OK) 
                    FillColor = colorDialog.Color; 
            };

            Button btnOk = new Button { Text = "OK", Location = x.HasValue ? new Point(20, 170) : new Point(75, 170) };

            //добавление элементов на форму
            Controls.AddRange(new Control[] { lblX, txtX, lblY, txtY, lblWidth, txtWidth, lblHeight, txtHeight, btnColor, btnOk });

            if (x.HasValue && y.HasValue && width.HasValue && height.HasValue)
            {
                txtX.Text = x.ToString();
                txtY.Text = y.ToString();
                txtWidth.Text = width.ToString();
                txtHeight.Text = height.ToString();
                FillColor = color ?? Color.White;

                Button btnDelete = new Button { Text = "Delete", Location = new Point(130, 170), ForeColor = Color.Red };
                btnDelete.Click += (s, e) => { OnDelete?.Invoke(); Close(); };
                Controls.Add(btnDelete);
            }

            btnOk.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtX.Text) ||
                    string.IsNullOrWhiteSpace(txtY.Text) ||
                    string.IsNullOrWhiteSpace(txtWidth.Text) ||
                    string.IsNullOrWhiteSpace(txtHeight.Text))
                {
                    MessageBox.Show("You haven't filled in all the fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    X = int.Parse(txtX.Text);
                    Y = int.Parse(txtY.Text);
                    Width = int.Parse(txtWidth.Text);
                    Height = int.Parse(txtHeight.Text);
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
