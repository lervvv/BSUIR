namespace Lab6
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
            this.distancesDataGridView = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.elementsCountNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.drawButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.randomValuesButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.maximumRadioButton = new System.Windows.Forms.RadioButton();
            this.minimumRadioButton = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.distancesDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.elementsCountNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // distancesDataGridView
            // 
            this.distancesDataGridView.AllowUserToAddRows = false;
            this.distancesDataGridView.AllowUserToDeleteRows = false;
            this.distancesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.distancesDataGridView.Location = new System.Drawing.Point(12, 56);
            this.distancesDataGridView.Name = "distancesDataGridView";
            this.distancesDataGridView.Size = new System.Drawing.Size(405, 281);
            this.distancesDataGridView.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(182, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Расстояния";
            // 
            // elementsCountNumericUpDown
            // 
            this.elementsCountNumericUpDown.Location = new System.Drawing.Point(115, 12);
            this.elementsCountNumericUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.elementsCountNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.elementsCountNumericUpDown.Name = "elementsCountNumericUpDown";
            this.elementsCountNumericUpDown.Size = new System.Drawing.Size(79, 20);
            this.elementsCountNumericUpDown.TabIndex = 2;
            this.elementsCountNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.elementsCountNumericUpDown.ValueChanged += new System.EventHandler(this.elementsCountNumericUpDown_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Число элементов";
            // 
            // drawButton
            // 
            this.drawButton.Location = new System.Drawing.Point(199, 356);
            this.drawButton.Name = "drawButton";
            this.drawButton.Size = new System.Drawing.Size(75, 23);
            this.drawButton.TabIndex = 4;
            this.drawButton.Text = "Построить";
            this.drawButton.UseVisualStyleBackColor = true;
            this.drawButton.Click += new System.EventHandler(this.drawButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(441, 14);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(544, 443);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // randomValuesButton
            // 
            this.randomValuesButton.Location = new System.Drawing.Point(217, 9);
            this.randomValuesButton.Name = "randomValuesButton";
            this.randomValuesButton.Size = new System.Drawing.Size(75, 23);
            this.randomValuesButton.TabIndex = 6;
            this.randomValuesButton.Text = "Заполнить";
            this.randomValuesButton.UseVisualStyleBackColor = true;
            this.randomValuesButton.Click += new System.EventHandler(this.randomValuesButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.maximumRadioButton);
            this.groupBox1.Controls.Add(this.minimumRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(15, 343);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(168, 85);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Критерий классификации";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // maximumRadioButton
            // 
            this.maximumRadioButton.AutoSize = true;
            this.maximumRadioButton.Location = new System.Drawing.Point(6, 42);
            this.maximumRadioButton.Name = "maximumRadioButton";
            this.maximumRadioButton.Size = new System.Drawing.Size(79, 17);
            this.maximumRadioButton.TabIndex = 1;
            this.maximumRadioButton.TabStop = true;
            this.maximumRadioButton.Text = "Максимум";
            this.maximumRadioButton.UseVisualStyleBackColor = true;
            // 
            // minimumRadioButton
            // 
            this.minimumRadioButton.AutoSize = true;
            this.minimumRadioButton.Checked = true;
            this.minimumRadioButton.Location = new System.Drawing.Point(6, 19);
            this.minimumRadioButton.Name = "minimumRadioButton";
            this.minimumRadioButton.Size = new System.Drawing.Size(73, 17);
            this.minimumRadioButton.TabIndex = 0;
            this.minimumRadioButton.TabStop = true;
            this.minimumRadioButton.Text = "Минимум";
            this.minimumRadioButton.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(997, 469);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.randomValuesButton);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.drawButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.elementsCountNumericUpDown);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.distancesDataGridView);
            this.Name = "Form1";
            this.Text = "МиАПР 6";
            ((System.ComponentModel.ISupportInitialize)(this.distancesDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.elementsCountNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView distancesDataGridView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown elementsCountNumericUpDown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button drawButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button randomValuesButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton maximumRadioButton;
        private System.Windows.Forms.RadioButton minimumRadioButton;
    }
}

