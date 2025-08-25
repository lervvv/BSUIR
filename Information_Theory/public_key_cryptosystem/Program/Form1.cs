using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace public_key_cryptosystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //вычисление первообразных
        private void btnList_Click(object sender, EventArgs e)
        {
            bool isNumber = int.TryParse(txtP1.Text, out int p);
            if (!isNumber || !IsPrime(p)) 
            {
                MessageBox.Show("Значение P должно быть простым числом.", "Ошибка");
                return;
            }
            List<int> primitive = new List<int>();
            int phi = p - 1;
            var divisors = GetPrimeDivisors(phi);
            for (int g = 2; g < p; g++)
            {
                bool isPrimitive = true;
                foreach (int q in divisors)
                {
                    int power = phi / q;
                    if (ModPow(g, power, p) == 1)
                    {
                        isPrimitive = false;
                        break;
                    }
                }
                if (isPrimitive)
                    primitive.Add(g);
            }
            txtList.Clear();
            foreach (var g in  primitive)
            {
                txtList.AppendText(g.ToString() + "  ");
            }
        }

        //проверка простое ли число
        static bool IsPrime(int p)
        {
            if (p <= 1) return false;
            if (p == 2) return true;
            if (p % 2 == 0) return false;
            for (int i = 3; i * i < p; i += 2)
            {
                if (p % i == 0) return false;
            }
            return true;
        }

        //поиск простых делителей числа
        static List<int> GetPrimeDivisors(int n)
        {
            List<int> divisors = new List<int>();
            int d = 2;
            while (d * d <= n)
            {
                if (n % d == 0)
                {
                    divisors.Add(d);
                    while (n % d == 0)
                        n /= d;
                }
                d++;
            }
            if (n > 1)
                divisors.Add(n);
            return divisors;
        }

        //возведение в степень по модулю
        static int ModPow(int baseValue, int exponent, int mod)
        {
            int result = 1;
            baseValue %= mod;
            while (exponent > 0)
            {
                if ((exponent & 1) == 1)
                    result = (result * baseValue) % mod;
                baseValue = (baseValue * baseValue) % mod;
                exponent >>= 1;
            }
            return result;
        }

        //поиск НОД для определения взаимно простых чисел
        static int GCD(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        //поиск обратного по модулю с использованием расширенного алгоритма Евклида
        private int ModInverse(int a, int mod)
        {
            int m0 = mod, t, q;
            int x0 = 0, x1 = 1;

            if (mod == 1)
                return 0;

            while (a > 1)
            {
                q = a / mod;
                t = mod;

                //mod - остаток от деления
                mod = a % mod;
                a = t;

                t = x0;
                x0 = x1 - q * x0;
                x1 = t;
            }

            if (x1 < 0)
                x1 += m0;

            return x1;
        }


        private void btnFile1_Click(object sender, EventArgs e)
        {
            //диалоговое окно для выбора файла
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Все файлы|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //чтение содержимого файла в массив байтов
                    byte[] fileBytes = System.IO.File.ReadAllBytes(openFileDialog.FileName);

                    //формирование строки в 10 сс
                    StringBuilder decimalString = new StringBuilder(fileBytes.Length * 4); //3 цифры + пробел

                    foreach (byte b in fileBytes)
                    {
                        decimalString.Append(b.ToString()).Append(' ');
                    }

                    txtM.Clear();
                    txtM.AppendText(decimalString.ToString());
                    txtResult1.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при чтении файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnFile2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Все файлы|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string filePath = openFileDialog.FileName;
                    txtC.Clear();
                    txtResult2.Text = "";

                    byte[] fileBytes = File.ReadAllBytes(filePath);

                    if (fileBytes.Length % 2 != 0)
                    {
                        MessageBox.Show("Длина файла не кратна 2 байтам. Возможно, это не ushort-данные.", "Предупреждение");
                    }

                    StringBuilder builder = new StringBuilder();

                    for (int i = 0; i + 1 < fileBytes.Length; i += 2)
                    {
                        ushort number = BitConverter.ToUInt16(fileBytes, i);
                        builder.Append(number).Append(" ");
                    }

                    txtC.Text = builder.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при чтении файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btnEncr_Click(object sender, EventArgs e)
        {
            bool isNumber = int.TryParse(txtP1.Text.Trim(), out int p);
            if (!isNumber || !IsPrime(p))
            {
                MessageBox.Show("Значение P должно быть простым числом.", "Ошибка");
                return;
            }
            if (txtList.Text == "" || txtG.Text == "")
            {
                MessageBox.Show("Выберите значение g из списка первообразных!", "Ошибка");
                return;
            }
            string[] lines = txtList.Text.Split(new[] { "\r\n", "\r", "\n", " "  }, StringSplitOptions.RemoveEmptyEntries);
            List<int> primitive = new List<int>();
            foreach (string line in lines)
            {
                if (int.TryParse(line.Trim(), out int val))
                    primitive.Add(val);
            }
            //проверка, что значение g содержится в списке
            if (!int.TryParse(txtG.Text.Trim(), out int g) || !primitive.Contains(g))
            {
                MessageBox.Show("Введённое значение g не найдено в списке первообразных корней!", "Ошибка");
                return;
            }
            if (!int.TryParse(txtX1.Text.Trim(), out int x) || x <= 1 || x >= p - 1)
            {
                MessageBox.Show("Введённое значение х не соответствует условию!", "Ошибка");
                return;
            }
            if (!int.TryParse(txtK.Text.Trim(), out int k) || k <= 1 || k >= p - 1 || GCD(k, p - 1) != 1)
            {
                MessageBox.Show("Введённое значение k не соответствует условию!", "Ошибка");
                return;
            }
            if (txtM.Text.Trim() == "")
            {
                MessageBox.Show("Откройте файл для шифрования", "Ошибка");
                return;
            }
            try
            {
                int y = ModPow(g, x, p);
                int a = ModPow(g, k, p);
                string[] numbers = txtM.Text.Split(new[] { " ", "\r\n", "\n", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                List<(int a, int b)> encrypted = new List<(int a, int b)>();
                foreach (string num in numbers)
                {
                    if (int.TryParse(num, out int m))
                    {
                        if (m < 0 || m >= p)
                        {
                            MessageBox.Show($"Сообщение m={m} не входит в диапазон [0, p-1]", "Ошибка");
                            return;
                        }

                        int yk = ModPow(y, k, p);
                        int b = (yk * m) % p;

                        encrypted.Add((a, b));
                    }
                }
                txtResult1.Text = string.Join("  ", encrypted.Select(pair => $"({pair.a}, {pair.b})"));
            }
            catch
            {
                MessageBox.Show("Ошибка при шифровании!", "Ошибка");
                return;
            }
        }

        private void btnDecr_Click(object sender, EventArgs e)
        {
            bool isNumber = int.TryParse(txtP2.Text.Trim(), out int p);
            if (!isNumber || !IsPrime(p))
            {
                MessageBox.Show("Значение P должно быть простым числом.", "Ошибка");
                return;
            }
            if (!int.TryParse(txtX2.Text.Trim(), out int x) || x <= 1 || x >= p - 1)
            {
                MessageBox.Show("Введённое значение х не соответствует условию!", "Ошибка");
                return;
            }
            if (txtC.Text.Trim() == "")
            {
                MessageBox.Show("Откройте файл для дешифрования", "Ошибка");
                return;
            }
            try
            {
                string[] parts = txtC.Text.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length % 2 != 0)
                {
                    MessageBox.Show("Неверный формат шифртекста! Ожидалась чётная последовательность чисел (a b a b ...).", "Ошибка");
                    return;
                }

                List<int> decrypted = new List<int>();

                for (int i = 0; i < parts.Length; i += 2)
                {
                    if (int.TryParse(parts[i], out int a) && int.TryParse(parts[i + 1], out int b))
                    {
                        //a^x mod p
                        int ax = ModPow(a, x, p);

                        //обратный элемент (ax)^(-1) mod p
                        int axInv = ModInverse(ax, p);
                        if (axInv == -1)
                        {
                            MessageBox.Show($"Не удалось найти обратный элемент по модулю для: {ax}", "Ошибка");
                            return;
                        }

                        //m = b * (a^x)^(-1) mod p
                        int m = (b * axInv) % p;
                        decrypted.Add(m);
                    }
                    else
                    {
                        MessageBox.Show($"Ошибка парсинга чисел: {parts[i]}, {parts[i + 1]}", "Ошибка");
                        return;
                    }
                }

                txtResult2.Text = string.Join(" ", decrypted);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при дешифровании: {ex.Message}", "Ошибка");
            }
        }

        private void btnSave(string txt)
        {
            if (txt == "")
            {
                MessageBox.Show("Данные для сохранения отсутствуют.", "Ошибка");
                return;
            }
            //диалоговое окно для сохранения файла
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Текстовый файл (*.txt)|*.txt|Бинарный файл (*.bin)|*.bin|Изображение (*.png)|*.png|Аудио WAV (*.wav)|*.wav|Видео MP4 (*.mp4)|*.mp4|Все файлы (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string filePath = saveFileDialog.FileName;
                    string extension = Path.GetExtension(filePath).ToLower();
                    byte[] dataToSave = Encoding.UTF8.GetBytes(txtResult1.Text); //преобразование в байты

                    switch (extension)
                    {
                        case ".txt":
                            ResultToText(txt, filePath);
                            break;
                        case ".bin":
                            ResultToBinaryText(txt, filePath);
                            break;
                        case ".png":
                            SaveAsImage(txt, filePath);
                            break;
                        case ".mp4":
                            SaveAsVideo(txt, filePath);
                            break;
                        default:
                            MessageBox.Show("Неизвестный формат файла.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка при сохранении файла.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }        

        private void btnSave1_Click(object sender, EventArgs e)
        {
            string cleaned = txtResult1.Text.Replace("(", "")
                          .Replace(")", "")
                          .Replace(",", "");
            btnSave(cleaned);
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            btnSave(txtResult2.Text);
        }

        private void ResultToText(string text, string filePath)
        {
            string[] parts = text.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            List<byte> byteList = new List<byte>();

            foreach (string part in parts)
            {
                if (int.TryParse(part, out int number))
                {
                    byteList.Add((byte)number);
                }
            }
            string Text = Encoding.UTF8.GetString(byteList.ToArray());
            File.WriteAllText(filePath, Text, Encoding.UTF8);
        }

        private void ResultToBinaryText(string text, string filePath)
        {
            string[] parts = text.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {
                foreach (string part in parts)
                {
                    if (ushort.TryParse(part, out ushort number))
                    {
                        writer.Write(number); //сохранение как 2 байта
                    }
                    else
                    {
                        MessageBox.Show($"Не удалось преобразовать '{part}' в число", "Ошибка");
                        return;
                    }
                }
            }
        }

        private void SaveAsImage(string text, string filePath)
        {
            try
            {
                string[] byteStrings = text.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                List<byte> byteList = new List<byte>();

                foreach (string byteString in byteStrings)
                {
                    if (int.TryParse(byteString, out int number))
                    {
                        if (number >= 0 && number <= 255)
                        {
                            byteList.Add((byte)number);
                        }
                        else
                        {
                            MessageBox.Show($"Число {number} выходит за пределы диапазона байта!", "Ошибка");
                            return;
                        }
                    }
                }

                if (byteList.Count > 100) //примерно минимум для изображения
                {
                    File.WriteAllBytes(filePath, byteList.ToArray());
                }
                else
                {
                    MessageBox.Show("Недостаточно данных для изображения.", "Ошибка");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении изображения: {ex.Message}", "Ошибка");
            }
        }


        private void SaveAsVideo(string text, string filePath)
        {
            try
            {
                string[] byteStrings = text.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                List<byte> byteList = new List<byte>();

                foreach (string byteString in byteStrings)
                {
                    if (int.TryParse(byteString, out int number))
                    {
                        if (number >= 0 && number <= 255)
                        {
                            byteList.Add((byte)number);
                        }
                        else
                        {
                            MessageBox.Show($"Число {number} выходит за пределы диапазона байта!", "Ошибка");
                            return;
                        }
                    }
                }

                if (byteList.Count > 1000) //примерно минимум MP4-файла
                {
                    File.WriteAllBytes(filePath, byteList.ToArray());
                }
                else
                {
                    MessageBox.Show("Недостаточно данных для корректного видео!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении видео: {ex.Message}", "Ошибка");
            }
        }

    }
}
