namespace ClientFTP
{
    partial class Connection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Connection));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtIp = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.trvServer = new System.Windows.Forms.TreeView();
            this.btnMove = new System.Windows.Forms.Button();
            this.btnRename = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.lblModifiedt = new System.Windows.Forms.Label();
            this.lblSizet = new System.Windows.Forms.Label();
            this.lblModified = new System.Windows.Forms.Label();
            this.lblSize = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblFile = new System.Windows.Forms.Label();
            this.trvLocal = new System.Windows.Forms.TreeView();
            this.btnAddFolder = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(-1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1459, 73);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 19.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(594, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(271, 64);
            this.label1.TabIndex = 0;
            this.label1.Text = "FTP-CLIENT";
            // 
            // txtIp
            // 
            this.txtIp.Font = new System.Drawing.Font("Calibri", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtIp.Location = new System.Drawing.Point(88, 87);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(184, 43);
            this.txtIp.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(12, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 36);
            this.label3.TabIndex = 6;
            this.label3.Text = "Хост:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(292, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 36);
            this.label4.TabIndex = 8;
            this.label4.Text = "Порт:";
            // 
            // txtPort
            // 
            this.txtPort.Font = new System.Drawing.Font("Calibri", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtPort.Location = new System.Drawing.Point(370, 87);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(133, 43);
            this.txtPort.TabIndex = 7;
            // 
            // btnConnect
            // 
            this.btnConnect.Font = new System.Drawing.Font("Calibri", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnConnect.Location = new System.Drawing.Point(1199, 88);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(231, 48);
            this.btnConnect.TabIndex = 9;
            this.btnConnect.Text = "Подключиться";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Calibri", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(841, 94);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 36);
            this.label5.TabIndex = 13;
            this.label5.Text = "Пароль:";
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Calibri", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtPassword.Location = new System.Drawing.Point(960, 88);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(203, 43);
            this.txtPassword.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(527, 94);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 36);
            this.label6.TabIndex = 11;
            this.label6.Text = "Логин:";
            // 
            // txtLogin
            // 
            this.txtLogin.Font = new System.Drawing.Font("Calibri", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtLogin.Location = new System.Drawing.Point(624, 90);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(180, 43);
            this.txtLogin.TabIndex = 10;
            // 
            // trvServer
            // 
            this.trvServer.Location = new System.Drawing.Point(12, 167);
            this.trvServer.Name = "trvServer";
            this.trvServer.Size = new System.Drawing.Size(839, 416);
            this.trvServer.TabIndex = 14;
            this.trvServer.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // btnMove
            // 
            this.btnMove.Font = new System.Drawing.Font("Calibri", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnMove.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.btnMove.Location = new System.Drawing.Point(362, 604);
            this.btnMove.Name = "btnMove";
            this.btnMove.Size = new System.Drawing.Size(208, 48);
            this.btnMove.TabIndex = 10;
            this.btnMove.Text = "Переместить";
            this.btnMove.UseVisualStyleBackColor = true;
            this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
            // 
            // btnRename
            // 
            this.btnRename.Font = new System.Drawing.Font("Calibri", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnRename.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.btnRename.Location = new System.Drawing.Point(587, 604);
            this.btnRename.Name = "btnRename";
            this.btnRename.Size = new System.Drawing.Size(242, 48);
            this.btnRename.TabIndex = 9;
            this.btnRename.Text = "Переименовать";
            this.btnRename.UseVisualStyleBackColor = true;
            this.btnRename.Click += new System.EventHandler(this.btnRename_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("Calibri", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnDelete.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.btnDelete.Location = new System.Drawing.Point(200, 604);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(145, 48);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Удалить";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Font = new System.Drawing.Font("Calibri", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnUpdate.Location = new System.Drawing.Point(12, 604);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(172, 46);
            this.btnUpdate.TabIndex = 17;
            this.btnUpdate.Text = "Обновить";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // lblModifiedt
            // 
            this.lblModifiedt.AutoSize = true;
            this.lblModifiedt.Font = new System.Drawing.Font("Calibri", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblModifiedt.Location = new System.Drawing.Point(325, 813);
            this.lblModifiedt.Name = "lblModifiedt";
            this.lblModifiedt.Size = new System.Drawing.Size(0, 33);
            this.lblModifiedt.TabIndex = 12;
            // 
            // lblSizet
            // 
            this.lblSizet.AutoSize = true;
            this.lblSizet.Font = new System.Drawing.Font("Calibri", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSizet.Location = new System.Drawing.Point(129, 746);
            this.lblSizet.Name = "lblSizet";
            this.lblSizet.Size = new System.Drawing.Size(0, 33);
            this.lblSizet.TabIndex = 11;
            // 
            // lblModified
            // 
            this.lblModified.AutoSize = true;
            this.lblModified.Font = new System.Drawing.Font("Calibri", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblModified.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblModified.Location = new System.Drawing.Point(12, 810);
            this.lblModified.Name = "lblModified";
            this.lblModified.Size = new System.Drawing.Size(302, 36);
            this.lblModified.TabIndex = 10;
            this.lblModified.Text = "Последнее изменение:";
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Font = new System.Drawing.Font("Calibri", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSize.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblSize.Location = new System.Drawing.Point(12, 744);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(113, 36);
            this.lblSize.TabIndex = 9;
            this.lblSize.Text = "Размер:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Calibri", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(325, 816);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(0, 33);
            this.label7.TabIndex = 7;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Calibri", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(129, 749);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(0, 33);
            this.label8.TabIndex = 6;
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Font = new System.Drawing.Font("Calibri", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblFile.Location = new System.Drawing.Point(12, 679);
            this.lblFile.MaximumSize = new System.Drawing.Size(317, 93);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(261, 36);
            this.lblFile.TabIndex = 13;
            this.lblFile.Text = "Ничего не выбрано.";
            this.lblFile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trvLocal
            // 
            this.trvLocal.Location = new System.Drawing.Point(882, 167);
            this.trvLocal.Name = "trvLocal";
            this.trvLocal.Size = new System.Drawing.Size(544, 416);
            this.trvLocal.TabIndex = 18;
            // 
            // btnAddFolder
            // 
            this.btnAddFolder.Font = new System.Drawing.Font("Calibri", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnAddFolder.Location = new System.Drawing.Point(847, 604);
            this.btnAddFolder.Name = "btnAddFolder";
            this.btnAddFolder.Size = new System.Drawing.Size(227, 46);
            this.btnAddFolder.TabIndex = 21;
            this.btnAddFolder.Text = "Создать папку";
            this.btnAddFolder.UseVisualStyleBackColor = true;
            this.btnAddFolder.Click += new System.EventHandler(this.btnAddFolder_Click);
            // 
            // Connection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1452, 891);
            this.Controls.Add(this.btnAddFolder);
            this.Controls.Add(this.lblModifiedt);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblFile);
            this.Controls.Add(this.lblSizet);
            this.Controls.Add(this.trvLocal);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lblModified);
            this.Controls.Add(this.lblSize);
            this.Controls.Add(this.btnRename);
            this.Controls.Add(this.btnMove);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.trvServer);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtLogin);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtIp);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Connection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FTP-Client";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.TreeView trvServer;
        private System.Windows.Forms.Button btnMove;
        private System.Windows.Forms.Button btnRename;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.Label lblModifiedt;
        private System.Windows.Forms.Label lblSizet;
        private System.Windows.Forms.Label lblModified;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TreeView trvLocal;
        private System.Windows.Forms.Button btnAddFolder;
    }
}

