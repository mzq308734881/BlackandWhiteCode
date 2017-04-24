namespace Client
{
    partial class SelectStand
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rbDifficult = new System.Windows.Forms.RadioButton();
            this.rbMiddle = new System.Windows.Forms.RadioButton();
            this.rbLow = new System.Windows.Forms.RadioButton();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rbPeopleBlack = new System.Windows.Forms.RadioButton();
            this.rbPCBlack = new System.Windows.Forms.RadioButton();
            this.btnExit = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.rbDifficult);
            this.groupBox2.Controls.Add(this.rbMiddle);
            this.groupBox2.Controls.Add(this.rbLow);
            this.groupBox2.Location = new System.Drawing.Point(76, 21);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(126, 151);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 125);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "（默认为低级）";
            // 
            // rbDifficult
            // 
            this.rbDifficult.AutoSize = true;
            this.rbDifficult.Location = new System.Drawing.Point(39, 85);
            this.rbDifficult.Name = "rbDifficult";
            this.rbDifficult.Size = new System.Drawing.Size(47, 16);
            this.rbDifficult.TabIndex = 3;
            this.rbDifficult.Text = "高级";
            this.rbDifficult.UseVisualStyleBackColor = true;
            // 
            // rbMiddle
            // 
            this.rbMiddle.AutoSize = true;
            this.rbMiddle.Location = new System.Drawing.Point(39, 63);
            this.rbMiddle.Name = "rbMiddle";
            this.rbMiddle.Size = new System.Drawing.Size(47, 16);
            this.rbMiddle.TabIndex = 2;
            this.rbMiddle.Text = "中级";
            this.rbMiddle.UseVisualStyleBackColor = true;
            // 
            // rbLow
            // 
            this.rbLow.AutoSize = true;
            this.rbLow.Checked = true;
            this.rbLow.Location = new System.Drawing.Point(39, 41);
            this.rbLow.Name = "rbLow";
            this.rbLow.Size = new System.Drawing.Size(47, 16);
            this.rbLow.TabIndex = 1;
            this.rbLow.TabStop = true;
            this.rbLow.Text = "低级";
            this.rbLow.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(110, 202);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.rbPeopleBlack);
            this.groupBox3.Controls.Add(this.rbPCBlack);
            this.groupBox3.Location = new System.Drawing.Point(267, 21);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(126, 151);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 125);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "（默认为人执黑）";
            // 
            // rbPeopleBlack
            // 
            this.rbPeopleBlack.AutoSize = true;
            this.rbPeopleBlack.Checked = true;
            this.rbPeopleBlack.Location = new System.Drawing.Point(25, 41);
            this.rbPeopleBlack.Name = "rbPeopleBlack";
            this.rbPeopleBlack.Size = new System.Drawing.Size(71, 16);
            this.rbPeopleBlack.TabIndex = 1;
            this.rbPeopleBlack.TabStop = true;
            this.rbPeopleBlack.Text = "玩家执黑";
            this.rbPeopleBlack.UseVisualStyleBackColor = true;
            // 
            // rbPCBlack
            // 
            this.rbPCBlack.AutoSize = true;
            this.rbPCBlack.Location = new System.Drawing.Point(25, 87);
            this.rbPCBlack.Name = "rbPCBlack";
            this.rbPCBlack.Size = new System.Drawing.Size(71, 16);
            this.rbPCBlack.TabIndex = 0;
            this.rbPCBlack.Text = "电脑执黑";
            this.rbPCBlack.UseVisualStyleBackColor = true;
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(275, 202);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // SelectStand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(447, 254);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox2);
            this.DoubleBuffered = true;
            this.EnableGlass = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "SelectStand";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选项";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.RadioButton rbMiddle;
        public System.Windows.Forms.RadioButton rbLow;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox groupBox3;
        public System.Windows.Forms.RadioButton rbPCBlack;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.RadioButton rbDifficult;
        public System.Windows.Forms.RadioButton rbPeopleBlack;
        private System.Windows.Forms.Button btnExit;
    }
}