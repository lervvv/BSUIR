using ShapeEditor.FormsOfShapes;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace ShapeEditor
{
    public class MainForm : Form
    {
        private Panel panel1;
        private Button btnAdd;
        private Panel panel2;
        private Button btnRead;
        private Button btnSave;
        private ShapeList shapeList = new ShapeList();
        private Button btnPlugin;
        private List<IShapePlugin> plugins = new List<IShapePlugin>();

        public MainForm()
        {
            InitializeComponent();
            this.ClientSize = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "ShapeEditor";

            //обработчик события Paint для панели2
            this.panel2.Paint += panel2_Paint;
            //метод отрисовки при изменении
            this.Paint += MainForm_Paint;

            //shapeList.Add(new Rectangle(200, 30, 200, 80, Color.CornflowerBlue));
            //shapeList.Add(new Ellipse(50, 200, 80, 120, Color.Firebrick));
        }
        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            shapeList.DrawAll(e.Graphics); //отрисовка фигур из контейнера
        }

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRead = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnPlugin = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panel1.Controls.Add(this.btnPlugin);
            this.panel1.Controls.Add(this.btnRead);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Location = new System.Drawing.Point(-1, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 63);
            this.panel1.TabIndex = 0;
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(490, 9);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(146, 43);
            this.btnRead.TabIndex = 3;
            this.btnRead.Text = "New file";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(642, 9);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(146, 43);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save ";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(18, 9);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(146, 43);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel2.Location = new System.Drawing.Point(-1, 65);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(810, 441);
            this.panel2.TabIndex = 1;
            this.panel2.Click += new System.EventHandler(this.panel2_Click);
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // btnPlugin
            // 
            this.btnPlugin.Location = new System.Drawing.Point(327, 10);
            this.btnPlugin.Name = "btnPlugin";
            this.btnPlugin.Size = new System.Drawing.Size(146, 43);
            this.btnPlugin.TabIndex = 4;
            this.btnPlugin.Text = "Add plugin";
            this.btnPlugin.UseVisualStyleBackColor = true;
            this.btnPlugin.Click += new System.EventHandler(this.btnPlugin_Click);
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(774, 429);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            using (Form selectForm = new Form())
            {
                selectForm.Text = "Choose a shape";
                selectForm.Size = new Size(300, 150);
                selectForm.StartPosition = FormStartPosition.CenterParent;

                ComboBox cmbShapes = new ComboBox
                {
                    Location = new Point(30, 20),
                    Width = 200,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                cmbShapes.Items.AddRange(new object[] {
                    "Line",
                    "Polyline",
                    "Rectangle",
                    "Square",
                    "Polygon",
                    "Circle",
                    "Ellipse"
                });
                // Фигуры из плагинов
                foreach (var plugin in plugins)
                {
                    cmbShapes.Items.Add($"[P] {plugin.ShapeName}");
                }
                Button btnOk = new Button
                {
                    Text = "OK",
                    Location = new Point(100, 60),
                    DialogResult = DialogResult.OK,
                };

                selectForm.Controls.AddRange(new Control[] { cmbShapes, btnOk });
                selectForm.AcceptButton = btnOk;

                if (selectForm.ShowDialog() == DialogResult.OK && cmbShapes.SelectedItem != null)
                {
                    string selectedShape = cmbShapes.SelectedItem.ToString();
                    //проверка, если выбран плагин
                    if (selectedShape.StartsWith("[P] "))
                    {
                        string pluginName = selectedShape.Substring(4); //имя плагина начинается с 4 позиции
                        //поиск плагина в списке загруженных
                        var plugin = plugins.FirstOrDefault(p => p.ShapeName == pluginName);

                        if (plugin != null)
                        {
                            var paramForm = new ParameterForm(plugin.GetParametersDescription());
                            if (paramForm.ShowDialog() == DialogResult.OK)
                            {
                                IShape newShape = plugin.CreateShapeWithParameters(paramForm.Parameters);
                                shapeList.Add(newShape);
                                panel2.Invalidate();
                            }
                        }
                    }
                    else
                    { 
                        Form shapeForm = null;  //для ввода параметров
                        IShape newShape = null;  //новая фигура

                        //открытие нужной формы в зависимости от фигуры
                        switch (selectedShape)
                        {
                            case "Line":
                                shapeForm = new LineForm();
                                break;
                            case "Polyline":
                                shapeForm = new PolylineForm();
                                break;
                            case "Rectangle":
                                shapeForm = new RectangleForm();
                                break;                        
                            case "Square":
                                shapeForm = new SquareForm();
                                break;
                            case "Circle":
                                shapeForm = new CircleForm();
                                break;
                            case "Ellipse":
                                shapeForm = new EllipseForm();
                                break;
                            case "Polygon":
                                shapeForm = new PolygonForm();
                                break;
                        }
                        if (shapeForm != null && shapeForm.ShowDialog() == DialogResult.OK)
                        {
                            //получение данных
                            switch (shapeForm)
                            {
                                case LineForm lineForm:
                                    newShape = new Line(lineForm.X1, lineForm.Y1, lineForm.X2, lineForm.Y2);
                                    break;
                                case RectangleForm rectForm:
                                    newShape = new Rectangle(rectForm.X, rectForm.Y, rectForm.Width, rectForm.Height, rectForm.FillColor);
                                    break;
                                case PolylineForm polyForm:
                                    newShape = new PolyLine(polyForm.Points.ToArray());
                                    break;
                                case SquareForm sqForm:
                                    newShape = new Square(sqForm.X, sqForm.Y, sqForm.Width, sqForm.FillColor);
                                    break;
                                case CircleForm crclForm:
                                    newShape = new Circle(crclForm.X, crclForm.Y, crclForm.Radius, crclForm.FillColor);
                                    break;
                                case EllipseForm ellForm:
                                    newShape = new Ellipse(ellForm.X, ellForm.Y, ellForm.Width, ellForm.Height, ellForm.FillColor);
                                    break;
                                case PolygonForm polygonForm:
                                    newShape = new Polygon(polygonForm.Points.ToArray(), polygonForm.FillColor);
                                    break;
                                default:
                                    newShape = null;
                                    break;
                            }
                            if (newShape != null)
                            {
                                shapeList.Add(newShape);
                                panel2.Invalidate();
                            }
                        }                        
                    }
                }
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            shapeList.DrawAll(e.Graphics);
        }
        private void panel2_Click(object sender, System.EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e; //координаты клика
            foreach (var shape in shapeList.GetShapes()) //фигуры из списка
            {
                if (shape.ContainsPoint(me.Location))
                {
                    //является ли фигура плагином
                    var plugin = plugins.FirstOrDefault(p => p.ShapeName == shape.GetType().Name);

                    if (plugin != null)
                    {
                        //обработка для любой фигуры плагина
                        HandlePluginShape(shape, plugin);
                        return;
                    }
                    switch (shape)
                    {
                        case Square sq:
                            SquareForm sqForm = new SquareForm(sq.X, sq.Y, sq.Width, sq.ColorShape);
                            sqForm.OnDelete += () =>
                            {
                                shapeList.Remove(sq);
                                panel2.Invalidate();
                            };
                            //если ок, обновление параметров
                            if (sqForm.ShowDialog() == DialogResult.OK)
                            {
                                sq.X = sqForm.X;
                                sq.Y = sqForm.Y;
                                sq.Width = sqForm.Width;
                                sq.Height = sqForm.Width;
                                sq.ColorShape = sqForm.FillColor;
                                panel2.Invalidate();
                            }
                            break;
                        case Rectangle rect:
                            RectangleForm rectForm = new RectangleForm(rect.X, rect.Y, rect.Width, rect.Height, rect.ColorShape);
                            rectForm.OnDelete += () =>
                            {
                                shapeList.Remove(rect);
                                panel2.Invalidate();
                            };
                            if (rectForm.ShowDialog() == DialogResult.OK)
                            {
                                rect.X = rectForm.X;
                                rect.Y = rectForm.Y;
                                rect.Width = rectForm.Width;
                                rect.Height = rectForm.Height;
                                rect.ColorShape = rectForm.FillColor;
                                panel2.Invalidate();
                            }
                            break;
                        case Line ln:
                            LineForm lnForm = new LineForm(ln.X1, ln.Y1, ln.Y1, ln.Y2);
                            lnForm.OnDelete += () =>
                            {
                                shapeList.Remove(ln);
                                panel2.Invalidate();
                            };
                            if (lnForm.ShowDialog() == DialogResult.OK)
                            {
                                ln.X1 = lnForm.X1;
                                ln.Y1 = lnForm.Y1;
                                ln.X2 = lnForm.X2;
                                ln.Y2 = lnForm.Y2;
                                panel2.Invalidate();
                            }
                            break;
                        case Circle crcl:
                            int radius = crcl.Width / 2; //получаем текущий радиус
                            CircleForm crclForm = new CircleForm(crcl.X, crcl.Y, radius, crcl.ColorShape);
                            crclForm.OnDelete += () =>
                            {
                                shapeList.Remove(crcl);
                                panel2.Invalidate();
                            };
                            if (crclForm.ShowDialog() == DialogResult.OK)
                            {
                                crcl.X = crclForm.X;
                                crcl.Y = crclForm.Y;
                                radius = crclForm.Radius;
                                crcl.Width = radius * 2;
                                crcl.Height = radius * 2;
                                crcl.ColorShape = crclForm.FillColor;
                                panel2.Invalidate();
                            }
                            break;
                        case Ellipse ell:
                            EllipseForm ellForm = new EllipseForm(ell.X, ell.Y, ell.Width, ell.Height, ell.ColorShape);
                            ellForm.OnDelete += () =>
                            {
                                shapeList.Remove(ell);
                                panel2.Invalidate();
                            };
                            if (ellForm.ShowDialog() == DialogResult.OK)
                            {
                                ell.X = ellForm.X;
                                ell.Y = ellForm.Y;
                                ell.Width = ellForm.Width;
                                ell.Height= ellForm.Height;
                                ell.ColorShape = ellForm.FillColor;
                                panel2.Invalidate();
                            }
                            break;
                        case PolyLine pl:
                            PolylineForm plForm = new PolylineForm(pl.Points.ToList());
                            plForm.OnDelete += () =>
                            {
                                shapeList.Remove(pl);
                                panel2.Invalidate();
                            };
                            if (plForm.ShowDialog() == DialogResult.OK)
                            {
                                pl.Points = plForm.Points.ToArray();
                                panel2.Invalidate();
                            }
                            break;
                        case Polygon plg:
                            PolygonForm plgForm = new PolygonForm(plg.Points.ToList(), plg.ColorShape);
                            plgForm.OnDelete += () =>
                            {
                                shapeList.Remove(plg);
                                panel2.Invalidate();
                            };
                            if (plgForm.ShowDialog() == DialogResult.OK)
                            {
                                plg.Points = plgForm.Points.ToArray();
                                plg.ColorShape = plgForm.FillColor;
                                panel2.Invalidate();
                            }
                            break;
                    }
                    return;

                }
            }
        }

        private void HandlePluginShape(IShape shape, IShapePlugin plugin)
        {
            //словарь для хранения параметров
            var properties = new Dictionary<string, object>();
            var type = shape.GetType();
            // цикл по свойствам фигуры (публичным, не статическим, не унаследованным
            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                //можно и прочитать и записать
                if (prop.CanRead && prop.CanWrite)
                {
                    //добавление в словарь свойство = значение
                    properties[prop.Name] = prop.GetValue(shape);
                }
            }
            //если фигура наследует заливку
            if (shape is FilledShape filledShape)
            {
                properties["ColorShape"] = filledShape.ColorShape;
            }
            //форма с параметрами
            var paramForm = new ParameterForm(plugin.GetParametersDescription(), properties);
    
            paramForm.OnDelete += () =>
            {
                shapeList.Remove(shape);
                panel2.Invalidate();
            };

            if (paramForm.ShowDialog() == DialogResult.OK)
            {
                //обновление свойств фигуры
                foreach (var param in paramForm.Parameters)
                {
                    //если параметр Color и заливка поддерживается
                    if (param.Key == "Color" && shape is FilledShape)
                    {
                        ((FilledShape)shape).ColorShape = (Color)param.Value;
                        continue;
                    }
                    var prop = shape.GetType().GetProperty(param.Key);
                    if (prop != null && prop.CanWrite)
                    {
                        prop.SetValue(shape, param.Value);
                    }
                }
                panel2.Invalidate();
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Binary files (*.dat)|*.dat|Text files (*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                if (filePath.EndsWith(".dat"))
                {
                    try
                    {
                        //файловый поток для записи
                        using (FileStream fs = new FileStream(filePath, FileMode.Create))
                        {
                            //создается сериализатор
                            BinaryFormatter bf = new BinaryFormatter();
                            //для обработки типов из плагинов
                            bf.Binder = new PluginAwareBinder();
                            //список для хранения информации о типах
                            var typeInfoList = new List<Tuple<string, string>>();
                            foreach (var shape in shapeList.GetShapes())
                            {
                                Type type = shape.GetType();
                                typeInfoList.Add(new Tuple<string, string>(
                                    type.AssemblyQualifiedName,
                                    type.Assembly.Location));
                            }
                            //структура 
                            var saveData = Tuple.Create(
                            shapeList.GetShapes().ToList(), //список всех фигур
                            plugins.Select(p => p.GetType().Assembly.Location).ToList() //список путей к плагинам
                            );

                            //сериализация данных в файл
                            bf.Serialize(fs, saveData);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}", "Error");
                    }
                }
                else
                {
                    //открывается поток для записи текста
                    using (StreamWriter sw = new StreamWriter(filePath))
                    {
                        foreach (var shape in shapeList.GetShapes())
                        {
                            sw.WriteLine($"{shape.GetType().Name}: {shape.GetInformation()}");
                        }
                    }
                }
            }
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Binary files (*.dat)|*.dat";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                try
                {
                    //поток FileStream открывает файл для чтения
                    using (FileStream fs = new FileStream(filePath, FileMode.Open))
                    {
                        //создание десериализатора
                        BinaryFormatter bf = new BinaryFormatter();
                        bf.Binder = new PluginAwareBinder();

                        //десериализация с фигурами и путями к плагинам
                        var saveData = bf.Deserialize(fs) as Tuple<List<IShape>, List<string>>;

                        if (saveData != null)
                        {
                            shapeList.Clear();
                            foreach (var shape in saveData.Item1)
                            {
                                shapeList.Add(shape);
                            }
                            panel2.Invalidate();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }
        private void btnPlugin_Click(object sender, EventArgs e)
        {
            string requiredVersion = "4.8";
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "D:\\4 семестр\\ООП\\4\\ShapeEditor\\Plugins";
                openFileDialog.Filter = "DLL files (*.dll)|*.dll";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {                    
                    try
                    {
                        //загрузка сборки в память для последующей работы с ней
                        Assembly assembly = Assembly.LoadFrom(openFileDialog.FileName);
                        //перебор всех классов в загруженной dll
                        foreach (Type type in assembly.GetTypes())
                        {
                            //проверка , что тип реализует интерфейс IShapePlugin и не является интерфейсом
                            if (typeof(IShapePlugin).IsAssignableFrom(type) && !type.IsInterface)
                            {
                                //создание объекта плагина
                                IShapePlugin plugin = (IShapePlugin)Activator.CreateInstance(type);
                                if (plugins.Any(p => p.ShapeName == plugin.ShapeName))
                                {
                                    MessageBox.Show($"The plugin with the name '{plugin.ShapeName}' has already been uploaded.");
                                    continue;
                                }
                                if (plugin.Version != requiredVersion)
                                {
                                    MessageBox.Show($"The plugin '{plugin.ShapeName}' has an unsupported version ({plugin.Version}). Required: {requiredVersion}.");
                                    continue;
                                }
                                //добавление в список
                                plugins.Add(plugin);
                                MessageBox.Show($"Plugin '{plugin.ShapeName}' uploaded successfully.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error {openFileDialog.FileName}: {ex.Message}", "Error");
                    }
                }
            }            
        }
    }
}