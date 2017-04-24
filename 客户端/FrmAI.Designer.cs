namespace Client
{
    partial class FrmAI
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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAI));
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.labBlackCount = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labBlack = new System.Windows.Forms.Label();
            this.labWhite = new System.Windows.Forms.Label();
            this.labWhiteCount = new System.Windows.Forms.Label();
            this.btnNewGame = new DevComponents.DotNetBar.ButtonItem();
            this.btnGo = new DevComponents.DotNetBar.ButtonItem();
            this.btnSelect = new DevComponents.DotNetBar.ButtonItem();
            this.bar1 = new DevComponents.DotNetBar.Bar();
            this.btnBack = new DevComponents.DotNetBar.ButtonItem();
            this.btnInstruct = new DevComponents.DotNetBar.ButtonItem();
            this.listBoxAIInfo = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(1, 34);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(404, 406);
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            this.pictureBox2.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_Paint);
            this.pictureBox2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox2_MouseDown);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "");
            this.imageList1.Images.SetKeyName(3, "CanputWhite.gif");
            this.imageList1.Images.SetKeyName(4, "DownedBlack.png");
            this.imageList1.Images.SetKeyName(5, "DownedWhite.png");
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(12, 456);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 23);
            this.label2.TabIndex = 2;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(465, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "黑子成绩：";
            // 
            // labBlackCount
            // 
            this.labBlackCount.BackColor = System.Drawing.Color.Transparent;
            this.labBlackCount.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labBlackCount.ForeColor = System.Drawing.Color.Red;
            this.labBlackCount.Location = new System.Drawing.Point(560, 71);
            this.labBlackCount.Name = "labBlackCount";
            this.labBlackCount.Size = new System.Drawing.Size(53, 20);
            this.labBlackCount.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(465, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 16);
            this.label5.TabIndex = 5;
            this.label5.Text = "白子成绩：";
            // 
            // labBlack
            // 
            this.labBlack.BackColor = System.Drawing.Color.Transparent;
            this.labBlack.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labBlack.Location = new System.Drawing.Point(426, 39);
            this.labBlack.Name = "labBlack";
            this.labBlack.Size = new System.Drawing.Size(145, 31);
            this.labBlack.TabIndex = 7;
            this.labBlack.Text = "该黑方落子";
            // 
            // labWhite
            // 
            this.labWhite.BackColor = System.Drawing.Color.Transparent;
            this.labWhite.Font = new System.Drawing.Font("楷体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labWhite.Location = new System.Drawing.Point(427, 37);
            this.labWhite.Name = "labWhite";
            this.labWhite.Size = new System.Drawing.Size(144, 31);
            this.labWhite.TabIndex = 8;
            this.labWhite.Text = "该白方落子";
            // 
            // labWhiteCount
            // 
            this.labWhiteCount.BackColor = System.Drawing.Color.Transparent;
            this.labWhiteCount.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labWhiteCount.ForeColor = System.Drawing.Color.Red;
            this.labWhiteCount.Location = new System.Drawing.Point(560, 93);
            this.labWhiteCount.Name = "labWhiteCount";
            this.labWhiteCount.Size = new System.Drawing.Size(53, 20);
            this.labWhiteCount.TabIndex = 9;
            // 
            // btnNewGame
            // 
            this.btnNewGame.Name = "btnNewGame";
            this.btnNewGame.Text = "新游戏";
            this.btnNewGame.Tooltip = "默认玩家执黑子，AI等级为简单，黑子先走！";
            this.btnNewGame.Click += new System.EventHandler(this.btnNewGame_Click);
            // 
            // btnGo
            // 
            this.btnGo.BeginGroup = true;
            this.btnGo.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btnGo.Name = "btnGo";
            this.btnGo.Text = "开始";
            this.btnGo.Click += new System.EventHandler(this.butGo_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.BeginGroup = true;
            this.btnSelect.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Text = "选项";
            this.btnSelect.Tooltip = "选择执子颜色和对弈难度";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // bar1
            // 
            this.bar1.AntiAlias = true;
            this.bar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.bar1.Font = new System.Drawing.Font("华文行楷", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bar1.IsMaximized = false;
            this.bar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnNewGame,
            this.btnGo,
            this.btnBack,
            this.btnSelect,
            this.btnInstruct});
            this.bar1.Location = new System.Drawing.Point(0, 0);
            this.bar1.Name = "bar1";
            this.bar1.Size = new System.Drawing.Size(649, 33);
            this.bar1.Stretch = true;
            this.bar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.bar1.TabIndex = 10;
            this.bar1.TabStop = false;
            this.bar1.Text = "bar1";
            // 
            // btnBack
            // 
            this.btnBack.BeginGroup = true;
            this.btnBack.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btnBack.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Center;
            this.btnBack.Name = "btnBack";
            this.btnBack.Text = "悔棋";
            this.btnBack.Tooltip = "默认悔棋不超过2步";
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnInstruct
            // 
            this.btnInstruct.BeginGroup = true;
            this.btnInstruct.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btnInstruct.Name = "btnInstruct";
            this.btnInstruct.Text = "说明";
            this.btnInstruct.Tooltip = "关于黑白棋";
            this.btnInstruct.Click += new System.EventHandler(this.btnInstruct_Click);
            // 
            // listBoxAIInfo
            // 
            this.listBoxAIInfo.FormattingEnabled = true;
            this.listBoxAIInfo.HorizontalScrollbar = true;
            this.listBoxAIInfo.ItemHeight = 12;
            this.listBoxAIInfo.Location = new System.Drawing.Point(430, 124);
            this.listBoxAIInfo.Name = "listBoxAIInfo";
            this.listBoxAIInfo.Size = new System.Drawing.Size(207, 316);
            this.listBoxAIInfo.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("华文行楷", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(489, 448);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 34);
            this.label1.TabIndex = 12;
            this.label1.Text = "计科11401 明章强\r\n   2017年4月1日";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.White;
            this.label4.Font = new System.Drawing.Font("华文行楷", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(56, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(300, 19);
            this.label4.TabIndex = 13;
            this.label4.Text = "A     B      C      D     E       F     G     H ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.White;
            this.label6.Font = new System.Drawing.Font("华文行楷", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(18, 93);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(19, 133);
            this.label6.TabIndex = 14;
            this.label6.Text = "1\r\n\r\n2\r\n\r\n3\r\n\r\n4";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.White;
            this.label7.Font = new System.Drawing.Font("华文行楷", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(18, 253);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(18, 133);
            this.label7.TabIndex = 15;
            this.label7.Text = "5\r\n\r\n6\r\n\r\n7\r\n\r\n8";
            // 
            // FrmAI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 489);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBoxAIInfo);
            this.Controls.Add(this.bar1);
            this.Controls.Add(this.labWhiteCount);
            this.Controls.Add(this.labWhite);
            this.Controls.Add(this.labBlack);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.labBlackCount);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox2);
            this.DoubleBuffered = true;
            this.EnableGlass = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "FrmAI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "黑白棋(人机对弈)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmAI_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labBlackCount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labBlack;
        private System.Windows.Forms.Label labWhite;
        private System.Windows.Forms.Label labWhiteCount;
        private DevComponents.DotNetBar.ButtonItem btnNewGame;
        private DevComponents.DotNetBar.ButtonItem btnGo;
        private DevComponents.DotNetBar.ButtonItem btnSelect;
        private DevComponents.DotNetBar.Bar bar1;
        private DevComponents.DotNetBar.ButtonItem btnBack;
        private DevComponents.DotNetBar.ButtonItem btnInstruct;
        private System.Windows.Forms.ListBox listBoxAIInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
    }
}

