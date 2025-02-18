namespace Lab4
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
            this.label1 = new System.Windows.Forms.Label();
            this.classesCountNnumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.vectorsCountNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.elementsCountNumericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.teachingButton = new System.Windows.Forms.Button();
            this.teachingResultTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.getClassButton = new System.Windows.Forms.Button();
            this.classTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.classesCountNnumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vectorsCountNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.elementsCountNumericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Число классов";
            // 
            // classesCountNnumericUpDown
            // 
            this.classesCountNnumericUpDown.Location = new System.Drawing.Point(338, 7);
            this.classesCountNnumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.classesCountNnumericUpDown.Name = "classesCountNnumericUpDown";
            this.classesCountNnumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.classesCountNnumericUpDown.TabIndex = 1;
            this.classesCountNnumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Число признаков объекта";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(312, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Число объектов в обучающей выборке для каждого класса";
            // 
            // vectorsCountNumericUpDown
            // 
            this.vectorsCountNumericUpDown.Location = new System.Drawing.Point(338, 33);
            this.vectorsCountNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.vectorsCountNumericUpDown.Name = "vectorsCountNumericUpDown";
            this.vectorsCountNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.vectorsCountNumericUpDown.TabIndex = 4;
            this.vectorsCountNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // elementsCountNumericUpDown3
            // 
            this.elementsCountNumericUpDown3.Location = new System.Drawing.Point(338, 60);
            this.elementsCountNumericUpDown3.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.elementsCountNumericUpDown3.Name = "elementsCountNumericUpDown3";
            this.elementsCountNumericUpDown3.Size = new System.Drawing.Size(120, 20);
            this.elementsCountNumericUpDown3.TabIndex = 5;
            this.elementsCountNumericUpDown3.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // teachingButton
            // 
            this.teachingButton.Location = new System.Drawing.Point(192, 89);
            this.teachingButton.Name = "teachingButton";
            this.teachingButton.Size = new System.Drawing.Size(75, 23);
            this.teachingButton.TabIndex = 6;
            this.teachingButton.Text = "Обучение";
            this.teachingButton.UseVisualStyleBackColor = true;
            this.teachingButton.Click += new System.EventHandler(this.teachingButton_Click);
            // 
            // teachingResultTextBox
            // 
            this.teachingResultTextBox.Location = new System.Drawing.Point(15, 119);
            this.teachingResultTextBox.Multiline = true;
            this.teachingResultTextBox.Name = "teachingResultTextBox";
            this.teachingResultTextBox.Size = new System.Drawing.Size(199, 303);
            this.teachingResultTextBox.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(295, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Классификация";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(220, 138);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(240, 46);
            this.dataGridView1.TabIndex = 9;
            // 
            // getClassButton
            // 
            this.getClassButton.Enabled = false;
            this.getClassButton.Location = new System.Drawing.Point(276, 190);
            this.getClassButton.Name = "getClassButton";
            this.getClassButton.Size = new System.Drawing.Size(116, 23);
            this.getClassButton.TabIndex = 10;
            this.getClassButton.Text = "Классифицировать";
            this.getClassButton.UseVisualStyleBackColor = true;
            this.getClassButton.Click += new System.EventHandler(this.getClassButton_Click);
            // 
            // classTextBox
            // 
            this.classTextBox.BackColor = System.Drawing.Color.White;
            this.classTextBox.Location = new System.Drawing.Point(224, 219);
            this.classTextBox.Name = "classTextBox";
            this.classTextBox.ReadOnly = true;
            this.classTextBox.Size = new System.Drawing.Size(233, 20);
            this.classTextBox.TabIndex = 11;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 434);
            this.Controls.Add(this.classTextBox);
            this.Controls.Add(this.getClassButton);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.teachingResultTextBox);
            this.Controls.Add(this.teachingButton);
            this.Controls.Add(this.elementsCountNumericUpDown3);
            this.Controls.Add(this.vectorsCountNumericUpDown);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.classesCountNnumericUpDown);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "МиАПР 4";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.classesCountNnumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vectorsCountNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.elementsCountNumericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown classesCountNnumericUpDown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown vectorsCountNumericUpDown;
        private System.Windows.Forms.NumericUpDown elementsCountNumericUpDown3;
        private System.Windows.Forms.Button teachingButton;
        private System.Windows.Forms.TextBox teachingResultTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button getClassButton;
        private System.Windows.Forms.TextBox classTextBox;
    }
}

