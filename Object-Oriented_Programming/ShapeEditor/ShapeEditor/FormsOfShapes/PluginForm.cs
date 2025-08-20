using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ShapeEditor
{
    public class ParameterForm : Form
    {
        public Dictionary<string, object> Parameters { get; } = new Dictionary<string, object>();
        public event Action OnDelete;

        public ParameterForm(Dictionary<string, Type> parameters, Dictionary<string, object> initialValues = null)
        {
            Text = "Enter Parameters";
            Size = new Size(300, 100 + parameters.Count * 30);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;

            int yPos = 20;
            foreach (var param in parameters)
            {
                Label lbl = new Label { Text = $"{param.Key}:", Location = new Point(20, yPos), Width = 100 };
                Control inputControl = CreateInputControl(param.Value, yPos, param.Key);
                Controls.AddRange(new Control[] { lbl, inputControl });
                yPos += 30;
            }

            Button btnOk = new Button { Text = "OK", Location = new Point(100, yPos), DialogResult = DialogResult.OK };
            Controls.Add(btnOk);
            AcceptButton = btnOk;

            //если начальные значения переданы - редактирование
            if (initialValues != null)
            {
                foreach (var control in Controls)
                {
                    if (control is TextBox txt && txt.Tag is string paramName &&
                        initialValues.ContainsKey(paramName) && initialValues[paramName] != null)
                    {
                        txt.Text = initialValues[paramName].ToString();
                        Parameters[paramName] = initialValues[paramName];
                    }
                    else if (control is Button btn && btn.Tag is string colorParamName &&
                             colorParamName == "Color" &&
                             initialValues.ContainsKey(colorParamName) && initialValues[colorParamName] is Color color)
                    {
                        Parameters[colorParamName] = color;
                    }
                }
                Button btnDelete = new Button { Text = "Delete", Location = new Point(180, yPos), ForeColor = Color.Red };
                btnDelete.Click += (s, ev) => { OnDelete?.Invoke(); Close(); };
                Controls.Add(btnDelete);
            }
        }

        private Control CreateInputControl(Type type, int yPos, string paramName)
        {
            if (type == typeof(int))
            {
                var txt = new TextBox { Location = new Point(120, yPos - 3), Width = 150 };
                //после потери фокуса на элемент
                txt.Leave += (s, e) =>
                {
                    if (int.TryParse(txt.Text, out int value))
                        Parameters[paramName] = value; //paramName - ключ
                };
                txt.Tag = paramName; //имя параметра сохраняю в Tag
                return txt;
            }
            else if (type == typeof(Color))
            {
                var btn = new Button { Text = "Choose Color", Location = new Point(120, yPos - 3), Width = 150 };
                ColorDialog colorDialog = new ColorDialog();
                btn.Click += (s, e) =>
                {
                    if (colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        Parameters[paramName] = colorDialog.Color; //в ключ помещаю значение
                    }
                };
                btn.Tag = paramName; //имя параметра в Tag
                return btn;
            }
            return null;
        }
    }
}