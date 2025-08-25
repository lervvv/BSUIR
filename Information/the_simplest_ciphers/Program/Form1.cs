using System;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Text;

namespace laba1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string ColumnEncrMethod(string m, string key1, string key2)
        {
            //фильтрация только русских символов
            m = new string(m.Where(IsRussianLetter).ToArray());
            key1 = new string(key1.Where(IsRussianLetter).ToArray());
            key2 = new string(key2.Where(IsRussianLetter).ToArray());

            if (m == "" || (key1 == "" && key2 == "")) return "";
            string res = m;

            //с использованием key1
            if (key1 != "") res = ColumnEncryptWithKey(res, key1);
            //с использованием key2, если есть
            if (key2 != "") res = ColumnEncryptWithKey(res, key2);
            return res;
        }

        private static bool IsRussianLetter(char c)
        {
            return (c >= 'А' && c <= 'Я') || (c >= 'а' && c <= 'я') || c == 'Ё' || c == 'ё';
        }
        private static int GetRussianLetterOrder(char c)
        {
            //приводим к нижнему регистру
            c = char.ToLower(c);
            //'ё' после 'е'
            if (c == 'ё') return 'е' + 1;
            if (c > 'е') return c + 1;
            return (int)c;
        }

        private string ColumnEncryptWithKey(string m, string key)
        {
            int columns = key.Length;
            int rows = (int)Math.Ceiling((double)m.Length / columns);
            char[,] arr = new char[rows, columns];

            //заполняем массив текстом m
            for (int i = 0, k = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                {
                    if (k < m.Length)
                    {
                        arr[i, j] = m[k];
                        k++;
                    }
                    else arr[i, j] = '\0';
                }

            //расставляем индексы столбцов в правильном порядке
            int[] order = new int[columns];
            for (int i = 0; i < columns; i++)
            {
                order[i] = i;
            }
            Array.Sort(order, (a, b) => GetRussianLetterOrder(key[a]).CompareTo(GetRussianLetterOrder(key[b])));

            //формируем результат
            StringBuilder res = new StringBuilder();
            for (int j = 0; j < columns; j++)
            {
                int newcol = order[j];
                for (int i = 0; i < rows; i++)
                {
                    if (arr[i, newcol] != '\0')
                    {
                        res.Append(arr[i, newcol]);
                    }
                }
            }
            return res.ToString();
        } 

        private void encrypt_Click(object sender, EventArgs e)
        {
            string m = tm1.Text;
            string key1 = tkey1.Text;
            string key2 = tkey2.Text;
            tencr1.Text = ColumnEncrMethod(m, key1, key2);
        }

        private void fileread1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                //фильтр файлов
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.Title = "Выберите файл для чтения";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    string m = File.ReadAllText(filePath);

                    tm1.Text = m;
                    tkey1.Text = "";
                    tkey2.Text = "";
                }
            }
        }

        private void save1_Click(object sender, EventArgs e)
        {
            if (tencr1.Text != "")
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    //фильтр файлов
                    openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                    openFileDialog.Title = "Выберите файл для сохранения";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = openFileDialog.FileName;
                        File.WriteAllText(filePath, tencr1.Text);
                    }
                }
            }
            else MessageBox.Show("У вас нет данных для сохранения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private string ColumnDencrMethod(string t, string key1, string key2)
        {
            //фильтрация только русских символов
            t = new string(t.Where(IsRussianLetter).ToArray());
            key1 = new string(key1.Where(IsRussianLetter).ToArray());
            key2 = new string(key2.Where(IsRussianLetter).ToArray());
            if (t == "" || (key1 == "" && key2 == "")) return "";
            string res = t;

            //с использованием key1
            if (key1 != "") res = ColumnDecryptWithKey(res, key1);
            //с использованием key2, если есть
            if (key2 != "") res = ColumnDecryptWithKey(res, key2);
            return res;
        }

        private string ColumnDecryptWithKey(string cipher, string key)
        {
            int columns = key.Length;
            int rows = (int)Math.Ceiling((double)cipher.Length / columns);
            char[,] cipherMat = new char[rows, columns];

            //определяем количество пустых ячеек
            int emptyCells = (rows * columns) - cipher.Length;

            //расставляем индексы столбцов в правильном порядке
            int[] order = new int[columns];
            for (int i = 0; i < columns; i++)
            {
                order[i] = i;
            }
            Array.Sort(order, (a, b) => GetRussianLetterOrder(key[a]).CompareTo(GetRussianLetterOrder(key[b])));

            //заполнение массива символами шифротекста по столбцам
            int k = 0;
            for (int z = 0; z < columns; z++)
            {
                int j = order[z];
                //определяем количество строк в ряду для заполнения
                int rowsincol = rows - (j >= columns - emptyCells ? 1 : 0);
                for (int i = 0; i < rowsincol; i++)
                {
                    if (k < cipher.Length)
                    {
                        cipherMat[i, j] = cipher[k];
                        k++;
                    }
                    else cipherMat[i, j] = '\0';
                }                
            }
            //формирование расшифрованного результата
            string res = "";
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                {
                    if (cipherMat[i, j] != '\0') res += cipherMat[i, j];
                }
            return res;
        }

        private void decrypt_Click(object sender, EventArgs e)
        {
            string t = tt1.Text;
            string key1 = tkey3.Text;
            string key2 = tkey4.Text;
            tm2.Text = ColumnDencrMethod(t, key1, key2);
        }

        private void fileread2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                //фильтр файлов
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"; 
                openFileDialog.Title = "Выберите файл для чтения";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    string m = File.ReadAllText(filePath);

                    tt1.Text = m;
                    tkey3.Text = "";
                    tkey4.Text = "";
                }
            }
        }

        private void save2_Click(object sender, EventArgs e)
        {
            if (tm2.Text != "")
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    //фильтр файлов
                    openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                    openFileDialog.Title = "Выберите файл для сохранения";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = openFileDialog.FileName;
                        File.WriteAllText(filePath, tm2.Text);
                    }
                }
            }
            else MessageBox.Show("У вас нет данных для сохранения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void encrypt1_Click(object sender, EventArgs e)
        {
            string m = tm3.Text;
            string key1 = tkey5.Text;
            tencr2.Text = VigenerEncrypt(m, key1);
        }

        private static string alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";

        private static int GetIndex(char c)
        {
            //возвращает индекс символа в строке alphabet
            return alphabet.IndexOf(c);
        }

        private string VigenerEncrypt(string m, string key1)
        {
            m = new string(m.Where(IsRussianLetter).ToArray()).ToLower();
            key1 = new string(key1.Where(IsRussianLetter).ToArray()).ToLower();
            string res = "";
            for (int i = 0; i < m.Length; i++)
            {
                key1 += m[i];
                int plain = GetIndex(m[i]);
                int key = GetIndex(key1[i]);
                int encr = (plain + key) % 33;
                char symbol = alphabet[encr];
                res += symbol;
            }
            return res;
        }

        private void decrypt1_Click(object sender, EventArgs e)
        {
            string t = tt2.Text;
            string key1 = tkey6.Text;
            tm4.Text = VigenerDecrypt(t, key1);
        }

        private string VigenerDecrypt(string t, string key1)
        {
            t = new string(t.Where(IsRussianLetter).ToArray()).ToLower();
            key1 = new string(key1.Where(IsRussianLetter).ToArray()).ToLower();
            if (key1 == "") return "";
            string res = "";
            for (int i = 0; i < t.Length; i++)
            {
                int cipher = GetIndex(t[i]);
                int key = GetIndex(key1[i]);
                int encr = (cipher - key + 33) % 33;
                char symbol = alphabet[encr];
                res += symbol;
                key1 += symbol;
            }
            return res;
        }

        private void fileread3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                //фильтр файлов
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.Title = "Выберите файл для чтения";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    string m = File.ReadAllText(filePath);

                    tm3.Text = m;
                }
            }
        }

        private void fileread4_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                //фильтр файлов
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.Title = "Выберите файл для чтения";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    string m = File.ReadAllText(filePath);

                    tt2.Text = m;
                }
            }
        }

        private void save3_Click(object sender, EventArgs e)
        {
            if (tencr2.Text != "")
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                    openFileDialog.Title = "Выберите файл для сохранения";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = openFileDialog.FileName;
                        File.WriteAllText(filePath, tencr2.Text);
                    }
                }
            }
            else MessageBox.Show("У вас нет данных для сохранения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        private void save4_Click(object sender, EventArgs e)
        {
            if (tm4.Text != "")
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                    openFileDialog.Title = "Выберите файл для сохранения";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = openFileDialog.FileName;
                        File.WriteAllText(filePath, tm4.Text);
                    }
                }
            }
            else MessageBox.Show("У вас нет данных для сохранения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
