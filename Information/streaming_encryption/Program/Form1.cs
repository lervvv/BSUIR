using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace streaming_encryption
{
    public partial class Form1 : Form
    {
        const int LFSR_SIZE = 23;
        bool[] lfsr = new bool[LFSR_SIZE];
        public Form1()
        {
            InitializeComponent();
        }

        //инициализация LFSR начальным состоянием регистра
        private void InitializeLFSR(string initialState)
        {
            for (int i = 0; i < LFSR_SIZE; i++)
            {
                lfsr[i] = initialState[i] == '1'; //заполнение 0 или 1
            }
        }

        private StringBuilder GenerateKey(int length)
        {
            StringBuilder keyStates = new StringBuilder();
            for (int i = 0; i < length * 8; i++)
            {
                keyStates.Append(lfsr[0] ? "1" : "0");
                //XOR и сдвиг
                bool newBit = lfsr[0] ^ lfsr[18];
                for (int j = 0; j < LFSR_SIZE - 1; j++)
                {
                    lfsr[j] = lfsr[j + 1];
                }
                lfsr[LFSR_SIZE - 1] = newBit;
            }
            return keyStates;
        }

        //шифрование данных
        private byte[] EncryptDecrypt(byte[] input, StringBuilder key)
        {
            byte[] result = new byte[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                string byteString = key.ToString(i * 8, 8);
                byte keyByte = Convert.ToByte(byteString, 2);
                result[i] = (byte)(input[i] ^ keyByte);
            }
            return result;
        }

        private void EncrDecr(RichTextBox txt, RichTextBox txtresult, RichTextBox txtkey)
        {
            try
            {
                string m = txt.Text.Replace(" ", "");
                if (!m.All(c => c == '0' || c == '1'))
                {
                    MessageBox.Show("Некорректные символы в данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtkey.Text = "";
                    txtresult.Text = "";
                    return;
                }
                if (m.Length % 8 != 0)
                {
                    MessageBox.Show("Длина данных должна быть кратна 8", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtkey.Text = "";
                    txtresult.Text = "";
                    return;
                }

                //преобразование строки в массив байтов
                byte[] inputBytes = Enumerable.Range(0, m.Length / 8)
                                              .Select(i => Convert.ToByte(m.Substring(i * 8, 8), 2))
                                              .ToArray();

                string first = txtfirst.Text;
                if (!first.All(c => c == '0' || c == '1'))
                {
                    MessageBox.Show("Некорректные символы в регистре", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtkey.Text = "";
                    txtresult.Text = "";
                    return;
                }
                if (first.Length != LFSR_SIZE)
                {
                    MessageBox.Show("Начальное состояние регистра должно быть длиной 23 бита.", "Ошибка");
                    txtkey.Text = "";
                    txtresult.Text = "";
                    return;
                }

                InitializeLFSR(first);
                StringBuilder keyStates = GenerateKey(inputBytes.Length);
                byte[] encryptedBytes = EncryptDecrypt(inputBytes, keyStates);

                //преобразование зашифрованных данных в строку
                string encryptedBinary = string.Join(" ", encryptedBytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
                txtresult.Text = encryptedBinary;

                txtkey.Text = string.Concat(keyStates);
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnencrypt_Click(object sender, EventArgs e)
        {
            if(txtm.Text == "")
            {
                MessageBox.Show("Откройте файл для шифрования.", "Ошибка");
            }
            else
            {
                EncrDecr(txtm, txtresult, txtkey);
            }
        }

        private void btndecrypt_Click(object sender, EventArgs e)
        {
            if (txtm.Text == "")
            {
                MessageBox.Show("Откройте файл для дешифрования.", "Ошибка");
            }
            else
            {
                EncrDecr(txtm, txtresult, txtkey);
            }
        }

        private void ReadingFile(RichTextBox txt, Label l)
        {
            //диалоговое окно для выбора файла
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Все файлы|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    l.Text = Path.GetFileName(openFileDialog.FileName);
                    //чтение содержимого файла в массив байтов
                    byte[] fileBytes = System.IO.File.ReadAllBytes(openFileDialog.FileName);

                    //формирование строки
                    StringBuilder binaryString = new StringBuilder(fileBytes.Length * 9); //8 бит + пробел

                    foreach (byte b in fileBytes)
                    {
                        binaryString.Append(Convert.ToString(b, 2).PadLeft(8, '0')).Append(' ');
                    }

                    txt.Clear();
                    txt.AppendText(binaryString.ToString());
                    txtkey.Text = "";
                    txtresult.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при чтении файла.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnfile1_Click(object sender, EventArgs e)
        {
            ReadingFile(txtm, filename);
        }

        private void SaveResult(RichTextBox txt)
        {
            if (txtresult.Text == "")
            {
                MessageBox.Show("Данные для сохранения отсутствуют.", "Ошибка");
                return;
            }
            //диалоговое окно для сохранения файла
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Бинарный файл (*.bin)|*.bin|Изображение (*.png)|*.png|Аудио WAV (*.wav)|*.wav|Видео MP4 (*.mp4)|*.mp4|Все файлы (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string filePath = saveFileDialog.FileName;
                    string extension = Path.GetExtension(filePath).ToLower();
                    byte[] dataToSave = Encoding.UTF8.GetBytes(txt.Text); //преобразование в байты

                    switch (extension)
                    {
                        case ".bin":
                            SaveAsBinaryText(txt.Text, filePath);
                            txtresult.Text = "";
                            txtkey.Text = "";
                            txtm.Text = "";
                            filename.Text = "Имя файла:";
                            break;
                        case ".png":
                            SaveAsImage(txt.Text, filePath);
                            txtresult.Text = "";
                            txtkey.Text = "";
                            txtm.Text = "";
                            filename.Text = "Имя файла:";
                            break;
                        case ".wav":
                            SaveAsAudio(txt.Text, filePath);
                            txtresult.Text = "";
                            txtkey.Text = "";
                            txtm.Text = "";
                            filename.Text = "Имя файла:";
                            break;
                        case ".mp4":
                            SaveAsVideo(txt.Text, filePath);
                            txtresult.Text = "";
                            txtkey.Text = "";
                            txtm.Text = "";
                            filename.Text = "Имя файла:";
                            break;
                        default:
                            MessageBox.Show("Неизвестный формат файла.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при сохранении файла.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnsave1_Click(object sender, EventArgs e)
        {
            SaveResult(txtresult);
        }

        private void SaveAsBinaryText(string text, string filePath)
        {
            string[] byteStrings = text.Split(' ');
            List<byte> byteList = new List<byte>();

            foreach (string byteString in byteStrings)
            {
                if (byteString.Length == 8) //проверяем, что здесь 8 бит
                {
                    byteList.Add(Convert.ToByte(byteString, 2)); //преобразуем в байт
                }
            }
            File.WriteAllBytes(filePath, byteList.ToArray());
        }

        private void SaveAsImage(string text, string filePath)
        {
            try
            {
                string[] byteStrings = text.Split(' ');
                List<byte> byteList = new List<byte>();

                foreach (string byteString in byteStrings)
                {
                    if (byteString.Length == 8) //проверка 8 бит
                    {
                        byteList.Add(Convert.ToByte(byteString, 2)); //преобразуем в байт
                    }
                }
                if (byteList.Count > 100) //примерный минимум для валидного изображения
                {
                    File.WriteAllBytes(filePath, byteList.ToArray());
                    return;
                }
            }
            catch { }
        }

        private void SaveAsAudio(string text, string filePath)
        {
            int sampleRate = 44100; //частота дискретизации (Гц)
            short bitsPerSample = 16; //16-битный звук
            short channels = 1; //моно

            //длина аудиоданных
            byte[] soundData = Encoding.ASCII.GetBytes(text);
            int dataLength = soundData.Length;

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
                //WAV-заголовок
                writer.Write(Encoding.ASCII.GetBytes("RIFF"));
                writer.Write(36 + dataLength); //размер файла
                writer.Write(Encoding.ASCII.GetBytes("WAVE"));
                writer.Write(Encoding.ASCII.GetBytes("fmt "));
                writer.Write(16); //подраздел (PCM)
                writer.Write((short)1); //PCM = 1
                writer.Write(channels);
                writer.Write(sampleRate);
                writer.Write(sampleRate * channels * bitsPerSample / 8);
                writer.Write((short)(channels * bitsPerSample / 8));
                writer.Write(bitsPerSample);
                writer.Write(Encoding.ASCII.GetBytes("data"));
                writer.Write(dataLength);
                writer.Write(soundData); //запись данных
            }
        }

        private void SaveAsVideo(string text, string filePath)
        {
            string[] byteStrings = text.Split(' ');
            List<byte> byteList = new List<byte>();

            foreach (string byteString in byteStrings)
            {
                if (byteString.Length == 8) //проверка 8 бит
                {
                    byteList.Add(Convert.ToByte(byteString, 2)); //преобразуем в байт
                }
            }
            if (byteList.Count > 1000) //минимальный размер MP4-файла
            {
                File.WriteAllBytes(filePath, byteList.ToArray());
            }
            else
            {
                MessageBox.Show("Недостаточно данных для корректного видео!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
