namespace Lab3
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.error1TextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.error2TextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.sumErrorTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pc1NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.pc2NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pc1NumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pc2NumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox.Location = new System.Drawing.Point(12, 43);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(848, 351);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(440, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Построить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // error1TextBox
            // 
            this.error1TextBox.Location = new System.Drawing.Point(210, 400);
            this.error1TextBox.Name = "error1TextBox";
            this.error1TextBox.Size = new System.Drawing.Size(100, 20);
            this.error1TextBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 403);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Вероятность ложной тревоги";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 428);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(192, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Вероятность пропуска обнаружения";
            // 
            // error2TextBox
            // 
            this.error2TextBox.Location = new System.Drawing.Point(210, 425);
            this.error2TextBox.Name = "error2TextBox";
            this.error2TextBox.Size = new System.Drawing.Size(100, 20);
            this.error2TextBox.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 454);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(189, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Суммарная ошибка классификации";
            // 
            // sumErrorTextBox
            // 
            this.sumErrorTextBox.Location = new System.Drawing.Point(210, 451);
            this.sumErrorTextBox.Name = "sumErrorTextBox";
            this.sumErrorTextBox.Size = new System.Drawing.Size(100, 20);
            this.sumErrorTextBox.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Вероятность p(C1)";
            // 
            // pc1NumericUpDown
            // 
            this.pc1NumericUpDown.DecimalPlaces = 2;
            this.pc1NumericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.pc1NumericUpDown.Location = new System.Drawing.Point(118, 7);
            this.pc1NumericUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.pc1NumericUpDown.Name = "pc1NumericUpDown";
            this.pc1NumericUpDown.Size = new System.Drawing.Size(83, 20);
            this.pc1NumericUpDown.TabIndex = 9;
            this.pc1NumericUpDown.ValueChanged += new System.EventHandler(this.pc1NumericUpDown_ValueChanged);
            // 
            // pc2NumericUpDown
            // 
            this.pc2NumericUpDown.DecimalPlaces = 2;
            this.pc2NumericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.pc2NumericUpDown.Location = new System.Drawing.Point(327, 7);
            this.pc2NumericUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.pc2NumericUpDown.Name = "pc2NumericUpDown";
            this.pc2NumericUpDown.Size = new System.Drawing.Size(83, 20);
            this.pc2NumericUpDown.TabIndex = 11;
            this.pc2NumericUpDown.ValueChanged += new System.EventHandler(this.pc2NumericUpDown_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(221, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Вероятность p(C2)";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(872, 476);
            this.Controls.Add(this.pc2NumericUpDown);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pc1NumericUpDown);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.sumErrorTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.error2TextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.error1TextBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox);
            this.Name = "Form1";
            this.Text = "МиАПР 3";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pc1NumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pc2NumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox error1TextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox error2TextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox sumErrorTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown pc1NumericUpDown;
        private System.Windows.Forms.NumericUpDown pc2NumericUpDown;
        private System.Windows.Forms.Label label5;
    }
}

