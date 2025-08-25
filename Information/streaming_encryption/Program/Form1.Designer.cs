namespace streaming_encryption
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btndecrypt = new System.Windows.Forms.Button();
            this.filename = new System.Windows.Forms.Label();
            this.txtresult = new System.Windows.Forms.RichTextBox();
            this.txtkey = new System.Windows.Forms.RichTextBox();
            this.txtm = new System.Windows.Forms.RichTextBox();
            this.btnsave1 = new System.Windows.Forms.Button();
            this.btnfile1 = new System.Windows.Forms.Button();
            this.btnencrypt = new System.Windows.Forms.Button();
            this.txtfirst = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(1, -1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1367, 56);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(325, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(361, 33);
            this.label1.TabIndex = 0;
            this.label1.Text = "Потоковое шифрование";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel2.Controls.Add(this.btndecrypt);
            this.panel2.Controls.Add(this.filename);
            this.panel2.Controls.Add(this.txtresult);
            this.panel2.Controls.Add(this.txtkey);
            this.panel2.Controls.Add(this.txtm);
            this.panel2.Controls.Add(this.btnsave1);
            this.panel2.Controls.Add(this.btnfile1);
            this.panel2.Controls.Add(this.btnencrypt);
            this.panel2.Controls.Add(this.txtfirst);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.ForeColor = System.Drawing.Color.Black;
            this.panel2.Location = new System.Drawing.Point(-9, 50);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1035, 826);
            this.panel2.TabIndex = 3;
            // 
            // btndecrypt
            // 
            this.btndecrypt.Location = new System.Drawing.Point(549, 357);
            this.btndecrypt.Name = "btndecrypt";
            this.btndecrypt.Size = new System.Drawing.Size(190, 41);
            this.btndecrypt.TabIndex = 21;
            this.btndecrypt.Text = "Расшифровать";
            this.btndecrypt.UseVisualStyleBackColor = true;
            this.btndecrypt.Click += new System.EventHandler(this.btndecrypt_Click);
            // 
            // filename
            // 
            this.filename.AutoSize = true;
            this.filename.Location = new System.Drawing.Point(566, 301);
            this.filename.Name = "filename";
            this.filename.Size = new System.Drawing.Size(130, 25);
            this.filename.TabIndex = 27;
            this.filename.Text = "Имя файла:";
            // 
            // txtresult
            // 
            this.txtresult.Location = new System.Drawing.Point(41, 627);
            this.txtresult.Name = "txtresult";
            this.txtresult.Size = new System.Drawing.Size(951, 123);
            this.txtresult.TabIndex = 26;
            this.txtresult.Text = "";
            // 
            // txtkey
            // 
            this.txtkey.Location = new System.Drawing.Point(41, 458);
            this.txtkey.Name = "txtkey";
            this.txtkey.Size = new System.Drawing.Size(951, 123);
            this.txtkey.TabIndex = 25;
            this.txtkey.Text = "";
            // 
            // txtm
            // 
            this.txtm.Location = new System.Drawing.Point(41, 59);
            this.txtm.Name = "txtm";
            this.txtm.Size = new System.Drawing.Size(951, 125);
            this.txtm.TabIndex = 24;
            this.txtm.Text = "";
            // 
            // btnsave1
            // 
            this.btnsave1.Location = new System.Drawing.Point(464, 765);
            this.btnsave1.Name = "btnsave1";
            this.btnsave1.Size = new System.Drawing.Size(143, 42);
            this.btnsave1.TabIndex = 12;
            this.btnsave1.Text = "Сохранить";
            this.btnsave1.UseVisualStyleBackColor = true;
            this.btnsave1.Click += new System.EventHandler(this.btnsave1_Click);
            // 
            // btnfile1
            // 
            this.btnfile1.Location = new System.Drawing.Point(352, 297);
            this.btnfile1.Name = "btnfile1";
            this.btnfile1.Size = new System.Drawing.Size(197, 41);
            this.btnfile1.TabIndex = 11;
            this.btnfile1.Text = "Прочитать файл";
            this.btnfile1.UseVisualStyleBackColor = true;
            this.btnfile1.Click += new System.EventHandler(this.btnfile1_Click);
            // 
            // btnencrypt
            // 
            this.btnencrypt.Location = new System.Drawing.Point(327, 357);
            this.btnencrypt.Name = "btnencrypt";
            this.btnencrypt.Size = new System.Drawing.Size(177, 42);
            this.btnencrypt.TabIndex = 10;
            this.btnencrypt.Text = "Зашифровать";
            this.btnencrypt.UseVisualStyleBackColor = true;
            this.btnencrypt.Click += new System.EventHandler(this.btnencrypt_Click);
            // 
            // txtfirst
            // 
            this.txtfirst.Location = new System.Drawing.Point(196, 243);
            this.txtfirst.Name = "txtfirst";
            this.txtfirst.Size = new System.Drawing.Size(624, 31);
            this.txtfirst.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(380, 426);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(290, 29);
            this.label7.TabIndex = 5;
            this.label7.Text = "Сформированный ключ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(459, 595);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(132, 29);
            this.label6.TabIndex = 4;
            this.label6.Text = "Результат";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(322, 205);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(380, 29);
            this.label5.TabIndex = 3;
            this.label5.Text = "Начальное состояние регистра";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(412, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(196, 29);
            this.label4.TabIndex = 2;
            this.label4.Text = "Исходный файл";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(1030, 869);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtfirst;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnsave1;
        private System.Windows.Forms.Button btnfile1;
        private System.Windows.Forms.Button btnencrypt;
        private System.Windows.Forms.RichTextBox txtm;
        private System.Windows.Forms.RichTextBox txtkey;
        private System.Windows.Forms.RichTextBox txtresult;
        private System.Windows.Forms.Label filename;
        private System.Windows.Forms.Button btndecrypt;
    }
}

