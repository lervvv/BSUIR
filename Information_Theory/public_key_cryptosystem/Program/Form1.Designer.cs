namespace public_key_cryptosystem
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
            this.btnSave1 = new System.Windows.Forms.Button();
            this.txtK = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtResult1 = new System.Windows.Forms.RichTextBox();
            this.btnFile1 = new System.Windows.Forms.Button();
            this.txtM = new System.Windows.Forms.RichTextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnEncr = new System.Windows.Forms.Button();
            this.txtX1 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnList = new System.Windows.Forms.Button();
            this.txtList = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtG = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtP1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSave2 = new System.Windows.Forms.Button();
            this.btnFile2 = new System.Windows.Forms.Button();
            this.txtResult2 = new System.Windows.Forms.RichTextBox();
            this.btnDecr = new System.Windows.Forms.Button();
            this.txtC = new System.Windows.Forms.RichTextBox();
            this.txtX2 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtP2 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(-1, -2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1276, 74);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(428, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(450, 45);
            this.label1.TabIndex = 0;
            this.label1.Text = "Криптосистема Эль-Гамаля";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Window;
            this.panel2.Controls.Add(this.btnSave1);
            this.panel2.Controls.Add(this.txtK);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.txtResult1);
            this.panel2.Controls.Add(this.btnFile1);
            this.panel2.Controls.Add(this.txtM);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.btnEncr);
            this.panel2.Controls.Add(this.txtX1);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.btnList);
            this.panel2.Controls.Add(this.txtList);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.txtG);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.txtP1);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(14, 82);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(615, 704);
            this.panel2.TabIndex = 1;
            // 
            // btnSave1
            // 
            this.btnSave1.Font = new System.Drawing.Font("Comic Sans MS", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSave1.Location = new System.Drawing.Point(221, 647);
            this.btnSave1.Name = "btnSave1";
            this.btnSave1.Size = new System.Drawing.Size(182, 40);
            this.btnSave1.TabIndex = 19;
            this.btnSave1.Text = "Сохранить";
            this.btnSave1.UseVisualStyleBackColor = true;
            this.btnSave1.Click += new System.EventHandler(this.btnSave1_Click);
            // 
            // txtK
            // 
            this.txtK.Location = new System.Drawing.Point(347, 295);
            this.txtK.Name = "txtK";
            this.txtK.Size = new System.Drawing.Size(191, 31);
            this.txtK.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(330, 259);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(236, 33);
            this.label7.TabIndex = 17;
            this.label7.Text = "Случайное число k";
            // 
            // txtResult1
            // 
            this.txtResult1.Location = new System.Drawing.Point(18, 545);
            this.txtResult1.Name = "txtResult1";
            this.txtResult1.Size = new System.Drawing.Size(577, 81);
            this.txtResult1.TabIndex = 16;
            this.txtResult1.Text = "";
            // 
            // btnFile1
            // 
            this.btnFile1.Font = new System.Drawing.Font("Comic Sans MS", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnFile1.Location = new System.Drawing.Point(388, 402);
            this.btnFile1.Name = "btnFile1";
            this.btnFile1.Size = new System.Drawing.Size(182, 40);
            this.btnFile1.TabIndex = 15;
            this.btnFile1.Text = "Открыть файл";
            this.btnFile1.UseVisualStyleBackColor = true;
            this.btnFile1.Click += new System.EventHandler(this.btnFile1_Click);
            // 
            // txtM
            // 
            this.txtM.Location = new System.Drawing.Point(29, 384);
            this.txtM.Name = "txtM";
            this.txtM.Size = new System.Drawing.Size(335, 85);
            this.txtM.TabIndex = 2;
            this.txtM.Text = "";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.Location = new System.Drawing.Point(66, 348);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(260, 33);
            this.label9.TabIndex = 14;
            this.label9.Text = "Исходное сообщение";
            // 
            // btnEncr
            // 
            this.btnEncr.Font = new System.Drawing.Font("Comic Sans MS", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnEncr.Location = new System.Drawing.Point(221, 490);
            this.btnEncr.Name = "btnEncr";
            this.btnEncr.Size = new System.Drawing.Size(182, 40);
            this.btnEncr.TabIndex = 12;
            this.btnEncr.Text = "Зашифровать";
            this.btnEncr.UseVisualStyleBackColor = true;
            this.btnEncr.Click += new System.EventHandler(this.btnEncr_Click);
            // 
            // txtX1
            // 
            this.txtX1.Location = new System.Drawing.Point(72, 295);
            this.txtX1.Name = "txtX1";
            this.txtX1.Size = new System.Drawing.Size(191, 31);
            this.txtX1.TabIndex = 11;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(54, 259);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(221, 33);
            this.label8.TabIndex = 10;
            this.label8.Text = "Закрытый ключ x";
            // 
            // btnList
            // 
            this.btnList.Font = new System.Drawing.Font("Comic Sans MS", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnList.Location = new System.Drawing.Point(350, 94);
            this.btnList.Name = "btnList";
            this.btnList.Size = new System.Drawing.Size(233, 42);
            this.btnList.TabIndex = 7;
            this.btnList.Text = "Вычислить список g";
            this.btnList.UseVisualStyleBackColor = true;
            this.btnList.Click += new System.EventHandler(this.btnList_Click);
            // 
            // txtList
            // 
            this.txtList.Location = new System.Drawing.Point(29, 203);
            this.txtList.Name = "txtList";
            this.txtList.Size = new System.Drawing.Size(296, 31);
            this.txtList.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(23, 165);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(302, 33);
            this.label6.TabIndex = 5;
            this.label6.Text = "Список первообразных g";
            // 
            // txtG
            // 
            this.txtG.Location = new System.Drawing.Point(388, 203);
            this.txtG.Name = "txtG";
            this.txtG.Size = new System.Drawing.Size(191, 31);
            this.txtG.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(428, 165);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(109, 33);
            this.label5.TabIndex = 3;
            this.label5.Text = "Выбор g";
            // 
            // txtP1
            // 
            this.txtP1.Location = new System.Drawing.Point(69, 111);
            this.txtP1.Name = "txtP1";
            this.txtP1.Size = new System.Drawing.Size(191, 31);
            this.txtP1.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(39, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(258, 33);
            this.label4.TabIndex = 1;
            this.label4.Text = "Случайное простое p";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Comic Sans MS", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(203, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(185, 38);
            this.label2.TabIndex = 0;
            this.label2.Text = "Шифрование";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Window;
            this.panel3.Controls.Add(this.btnSave2);
            this.panel3.Controls.Add(this.btnFile2);
            this.panel3.Controls.Add(this.txtResult2);
            this.panel3.Controls.Add(this.btnDecr);
            this.panel3.Controls.Add(this.txtC);
            this.panel3.Controls.Add(this.txtX2);
            this.panel3.Controls.Add(this.label12);
            this.panel3.Controls.Add(this.txtP2);
            this.panel3.Controls.Add(this.label11);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label10);
            this.panel3.Location = new System.Drawing.Point(646, 82);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(615, 704);
            this.panel3.TabIndex = 2;
            // 
            // btnSave2
            // 
            this.btnSave2.Font = new System.Drawing.Font("Comic Sans MS", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSave2.Location = new System.Drawing.Point(225, 647);
            this.btnSave2.Name = "btnSave2";
            this.btnSave2.Size = new System.Drawing.Size(182, 40);
            this.btnSave2.TabIndex = 22;
            this.btnSave2.Text = "Сохранить";
            this.btnSave2.UseVisualStyleBackColor = true;
            this.btnSave2.Click += new System.EventHandler(this.btnSave2_Click);
            // 
            // btnFile2
            // 
            this.btnFile2.Font = new System.Drawing.Font("Comic Sans MS", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnFile2.Location = new System.Drawing.Point(402, 295);
            this.btnFile2.Name = "btnFile2";
            this.btnFile2.Size = new System.Drawing.Size(182, 40);
            this.btnFile2.TabIndex = 22;
            this.btnFile2.Text = "Открыть файл";
            this.btnFile2.UseVisualStyleBackColor = true;
            this.btnFile2.Click += new System.EventHandler(this.btnFile2_Click);
            // 
            // txtResult2
            // 
            this.txtResult2.Location = new System.Drawing.Point(19, 484);
            this.txtResult2.Name = "txtResult2";
            this.txtResult2.Size = new System.Drawing.Size(577, 121);
            this.txtResult2.TabIndex = 21;
            this.txtResult2.Text = "";
            // 
            // btnDecr
            // 
            this.btnDecr.Font = new System.Drawing.Font("Comic Sans MS", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnDecr.Location = new System.Drawing.Point(221, 418);
            this.btnDecr.Name = "btnDecr";
            this.btnDecr.Size = new System.Drawing.Size(190, 40);
            this.btnDecr.TabIndex = 20;
            this.btnDecr.Text = "Расшифровать";
            this.btnDecr.UseVisualStyleBackColor = true;
            this.btnDecr.Click += new System.EventHandler(this.btnDecr_Click);
            // 
            // txtC
            // 
            this.txtC.Location = new System.Drawing.Point(35, 248);
            this.txtC.Name = "txtC";
            this.txtC.Size = new System.Drawing.Size(335, 136);
            this.txtC.TabIndex = 20;
            this.txtC.Text = "";
            // 
            // txtX2
            // 
            this.txtX2.Location = new System.Drawing.Point(368, 126);
            this.txtX2.Name = "txtX2";
            this.txtX2.Size = new System.Drawing.Size(191, 31);
            this.txtX2.TabIndex = 21;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label12.Location = new System.Drawing.Point(128, 206);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(157, 33);
            this.label12.TabIndex = 21;
            this.label12.Text = "Шифротекст";
            // 
            // txtP2
            // 
            this.txtP2.Location = new System.Drawing.Point(57, 126);
            this.txtP2.Name = "txtP2";
            this.txtP2.Size = new System.Drawing.Size(191, 31);
            this.txtP2.TabIndex = 21;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label11.Location = new System.Drawing.Point(350, 90);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(221, 33);
            this.label11.TabIndex = 20;
            this.label11.Text = "Закрытый ключ x";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Comic Sans MS", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(189, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(218, 38);
            this.label3.TabIndex = 1;
            this.label3.Text = "Дешифрование";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label10.Location = new System.Drawing.Point(27, 90);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(258, 33);
            this.label10.TabIndex = 20;
            this.label10.Text = "Случайное простое p";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1274, 798);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cryptosystem";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtP1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnList;
        private System.Windows.Forms.TextBox txtList;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtG;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtX1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnEncr;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.RichTextBox txtResult1;
        private System.Windows.Forms.Button btnFile1;
        private System.Windows.Forms.RichTextBox txtM;
        private System.Windows.Forms.TextBox txtK;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnSave1;
        private System.Windows.Forms.Button btnSave2;
        private System.Windows.Forms.Button btnFile2;
        private System.Windows.Forms.RichTextBox txtResult2;
        private System.Windows.Forms.Button btnDecr;
        private System.Windows.Forms.RichTextBox txtC;
        private System.Windows.Forms.TextBox txtX2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtP2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
    }
}

