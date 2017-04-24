namespace Client
{
    partial class FrmPeople
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPeople));
            this.labPlayer0 = new System.Windows.Forms.Label();
            this.labPlayer = new System.Windows.Forms.Label();
            this.listBoxMsg = new System.Windows.Forms.ListBox();
            this.labelTurn = new System.Windows.Forms.Label();
            this.labelGrade1 = new System.Windows.Forms.Label();
            this.labelGrade0 = new System.Windows.Forms.Label();
            this.labelSideOther1 = new System.Windows.Forms.Label();
            this.labelgrid1 = new System.Windows.Forms.Label();
            this.labelgrid0 = new System.Windows.Forms.Label();
            this.labelSideOwn0 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.labAIGrade1 = new System.Windows.Forms.Label();
            this.labAIGrade0 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.labHost = new System.Windows.Forms.Label();
            this.bar1 = new DevComponents.DotNetBar.Bar();
            this.btnStart = new DevComponents.DotNetBar.ButtonItem();
            this.btnHost = new DevComponents.DotNetBar.ButtonItem();
            this.btnCancelHost = new DevComponents.DotNetBar.ButtonItem();
            this.btnSelect = new DevComponents.DotNetBar.ButtonItem();
            this.btnInstruct = new DevComponents.DotNetBar.ButtonItem();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.txtSendMsg = new System.Windows.Forms.RichTextBox();
            this.btnSendMsg = new System.Windows.Forms.Button();
            this.listTalkMsg = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // labPlayer0
            // 
            this.labPlayer0.BackColor = System.Drawing.Color.White;
            this.labPlayer0.Font = new System.Drawing.Font("华文行楷", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labPlayer0.Location = new System.Drawing.Point(357, 408);
            this.labPlayer0.Name = "labPlayer0";
            this.labPlayer0.Size = new System.Drawing.Size(58, 17);
            this.labPlayer0.TabIndex = 0;
            this.labPlayer0.Text = "玩家：";
            this.labPlayer0.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labPlayer
            // 
            this.labPlayer.BackColor = System.Drawing.Color.White;
            this.labPlayer.Font = new System.Drawing.Font("华文行楷", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labPlayer.ForeColor = System.Drawing.Color.Red;
            this.labPlayer.Location = new System.Drawing.Point(408, 408);
            this.labPlayer.Name = "labPlayer";
            this.labPlayer.Size = new System.Drawing.Size(104, 17);
            this.labPlayer.TabIndex = 1;
            this.labPlayer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // listBoxMsg
            // 
            this.listBoxMsg.FormattingEnabled = true;
            this.listBoxMsg.HorizontalScrollbar = true;
            this.listBoxMsg.ItemHeight = 12;
            this.listBoxMsg.Location = new System.Drawing.Point(640, 173);
            this.listBoxMsg.Name = "listBoxMsg";
            this.listBoxMsg.Size = new System.Drawing.Size(237, 268);
            this.listBoxMsg.TabIndex = 7;
            // 
            // labelTurn
            // 
            this.labelTurn.Location = new System.Drawing.Point(632, 34);
            this.labelTurn.Name = "labelTurn";
            this.labelTurn.Size = new System.Drawing.Size(141, 28);
            this.labelTurn.TabIndex = 17;
            // 
            // labelGrade1
            // 
            this.labelGrade1.ForeColor = System.Drawing.Color.Red;
            this.labelGrade1.Location = new System.Drawing.Point(712, 147);
            this.labelGrade1.Name = "labelGrade1";
            this.labelGrade1.Size = new System.Drawing.Size(116, 12);
            this.labelGrade1.TabIndex = 16;
            // 
            // labelGrade0
            // 
            this.labelGrade0.ForeColor = System.Drawing.Color.Red;
            this.labelGrade0.Location = new System.Drawing.Point(712, 96);
            this.labelGrade0.Name = "labelGrade0";
            this.labelGrade0.Size = new System.Drawing.Size(118, 12);
            this.labelGrade0.TabIndex = 15;
            // 
            // labelSideOther1
            // 
            this.labelSideOther1.Location = new System.Drawing.Point(632, 126);
            this.labelSideOther1.Name = "labelSideOther1";
            this.labelSideOther1.Size = new System.Drawing.Size(245, 12);
            this.labelSideOther1.TabIndex = 14;
            // 
            // labelgrid1
            // 
            this.labelgrid1.Location = new System.Drawing.Point(663, 148);
            this.labelgrid1.Name = "labelgrid1";
            this.labelgrid1.Size = new System.Drawing.Size(41, 12);
            this.labelgrid1.TabIndex = 13;
            this.labelgrid1.Text = "成绩：";
            // 
            // labelgrid0
            // 
            this.labelgrid0.Location = new System.Drawing.Point(665, 96);
            this.labelgrid0.Name = "labelgrid0";
            this.labelgrid0.Size = new System.Drawing.Size(41, 12);
            this.labelgrid0.TabIndex = 12;
            this.labelgrid0.Text = "成绩：";
            // 
            // labelSideOwn0
            // 
            this.labelSideOwn0.Location = new System.Drawing.Point(632, 69);
            this.labelSideOwn0.Name = "labelSideOwn0";
            this.labelSideOwn0.Size = new System.Drawing.Size(245, 12);
            this.labelSideOwn0.TabIndex = 11;
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
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // labAIGrade1
            // 
            this.labAIGrade1.Location = new System.Drawing.Point(806, 133);
            this.labAIGrade1.Name = "labAIGrade1";
            this.labAIGrade1.Size = new System.Drawing.Size(67, 12);
            this.labAIGrade1.TabIndex = 37;
            this.labAIGrade1.Visible = false;
            // 
            // labAIGrade0
            // 
            this.labAIGrade0.BackColor = System.Drawing.Color.Transparent;
            this.labAIGrade0.Location = new System.Drawing.Point(806, 108);
            this.labAIGrade0.Name = "labAIGrade0";
            this.labAIGrade0.Size = new System.Drawing.Size(67, 12);
            this.labAIGrade0.TabIndex = 33;
            this.labAIGrade0.Visible = false;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(63, 449);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 31;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(221, 34);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(405, 407);
            this.pictureBox2.TabIndex = 30;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            this.pictureBox2.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox2_Paint);
            this.pictureBox2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox2_MouseDown);
            // 
            // labHost
            // 
            this.labHost.ForeColor = System.Drawing.Color.Red;
            this.labHost.Location = new System.Drawing.Point(224, 444);
            this.labHost.Name = "labHost";
            this.labHost.Size = new System.Drawing.Size(405, 23);
            this.labHost.TabIndex = 39;
            this.labHost.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bar1
            // 
            this.bar1.AntiAlias = true;
            this.bar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.bar1.Font = new System.Drawing.Font("华文行楷", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bar1.IsMaximized = false;
            this.bar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnStart,
            this.btnHost,
            this.btnCancelHost,
            this.btnSelect,
            this.btnInstruct});
            this.bar1.Location = new System.Drawing.Point(0, 0);
            this.bar1.Name = "bar1";
            this.bar1.Size = new System.Drawing.Size(885, 33);
            this.bar1.Stretch = true;
            this.bar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.bar1.TabIndex = 40;
            this.bar1.TabStop = false;
            this.bar1.Text = "bar1";
            // 
            // btnStart
            // 
            this.btnStart.Name = "btnStart";
            this.btnStart.Text = "开始";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnHost
            // 
            this.btnHost.BeginGroup = true;
            this.btnHost.Name = "btnHost";
            this.btnHost.Text = "托管";
            this.btnHost.Click += new System.EventHandler(this.btnHost_Click);
            // 
            // btnCancelHost
            // 
            this.btnCancelHost.BeginGroup = true;
            this.btnCancelHost.Name = "btnCancelHost";
            this.btnCancelHost.Text = "取消托管";
            this.btnCancelHost.Click += new System.EventHandler(this.btnCancelHost_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.BeginGroup = true;
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Text = "选项";
            this.btnSelect.Tooltip = "设置AI托管难度";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnInstruct
            // 
            this.btnInstruct.BeginGroup = true;
            this.btnInstruct.Name = "btnInstruct";
            this.btnInstruct.Text = "说明";
            this.btnInstruct.Click += new System.EventHandler(this.btnInstruct_Click);
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(0, 19);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(90, 23);
            this.labelX1.TabIndex = 41;
            this.labelX1.Text = "消息对话框：";
            // 
            // txtSendMsg
            // 
            this.txtSendMsg.Location = new System.Drawing.Point(0, 272);
            this.txtSendMsg.Multiline = false;
            this.txtSendMsg.Name = "txtSendMsg";
            this.txtSendMsg.Size = new System.Drawing.Size(203, 79);
            this.txtSendMsg.TabIndex = 43;
            this.txtSendMsg.Text = "";
            this.txtSendMsg.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSendMsg_KeyUp);
            // 
            // btnSendMsg
            // 
            this.btnSendMsg.Location = new System.Drawing.Point(119, 363);
            this.btnSendMsg.Name = "btnSendMsg";
            this.btnSendMsg.Size = new System.Drawing.Size(75, 23);
            this.btnSendMsg.TabIndex = 44;
            this.btnSendMsg.Text = "发送";
            this.btnSendMsg.UseVisualStyleBackColor = true;
            this.btnSendMsg.Click += new System.EventHandler(this.butSendTalk_Click);
            // 
            // listTalkMsg
            // 
            this.listTalkMsg.FormattingEnabled = true;
            this.listTalkMsg.HorizontalScrollbar = true;
            this.listTalkMsg.ItemHeight = 12;
            this.listTalkMsg.Location = new System.Drawing.Point(0, 48);
            this.listTalkMsg.Name = "listTalkMsg";
            this.listTalkMsg.Size = new System.Drawing.Size(203, 220);
            this.listTalkMsg.TabIndex = 42;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listTalkMsg);
            this.groupBox2.Controls.Add(this.btnClear);
            this.groupBox2.Controls.Add(this.btnSendMsg);
            this.groupBox2.Controls.Add(this.txtSendMsg);
            this.groupBox2.Controls.Add(this.labelX1);
            this.groupBox2.Location = new System.Drawing.Point(12, 39);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(203, 402);
            this.groupBox2.TabIndex = 45;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "聊天室";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(12, 363);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 44;
            this.btnClear.Text = "清除";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("华文行楷", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(662, 450);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(210, 17);
            this.label1.TabIndex = 46;
            this.label1.Text = "计科11401 明章强  2017年4月1日";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.White;
            this.label4.Font = new System.Drawing.Font("华文行楷", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(272, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(300, 19);
            this.label4.TabIndex = 47;
            this.label4.Text = "A     B      C      D     E       F     G     H ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.White;
            this.label7.Font = new System.Drawing.Font("华文行楷", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(237, 252);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(18, 133);
            this.label7.TabIndex = 49;
            this.label7.Text = "5\r\n\r\n6\r\n\r\n7\r\n\r\n8";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.White;
            this.label6.Font = new System.Drawing.Font("华文行楷", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(237, 92);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(19, 133);
            this.label6.TabIndex = 48;
            this.label6.Text = "1\r\n\r\n2\r\n\r\n3\r\n\r\n4";
            // 
            // FrmPeople
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(243)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(885, 475);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labPlayer);
            this.Controls.Add(this.labPlayer0);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.bar1);
            this.Controls.Add(this.labHost);
            this.Controls.Add(this.labAIGrade1);
            this.Controls.Add(this.labAIGrade0);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.labelTurn);
            this.Controls.Add(this.labelGrade1);
            this.Controls.Add(this.labelGrade0);
            this.Controls.Add(this.labelSideOther1);
            this.Controls.Add(this.labelgrid1);
            this.Controls.Add(this.labelgrid0);
            this.Controls.Add(this.labelSideOwn0);
            this.Controls.Add(this.listBoxMsg);
            this.DoubleBuffered = true;
            this.EnableGlass = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "FrmPeople";
            this.Text = "黑白棋(玩家对弈)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPlaying_FormClosing);
            this.Load += new System.EventHandler(this.Form2_Load);
            this.Enter += new System.EventHandler(this.FrmPeople_Enter);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labPlayer0;
        public System.Windows.Forms.Label labPlayer;
        private System.Windows.Forms.ListBox listBoxMsg;
        private System.Windows.Forms.Label labelTurn;
        private System.Windows.Forms.Label labelGrade1;
        private System.Windows.Forms.Label labelGrade0;
        private System.Windows.Forms.Label labelSideOther1;
        private System.Windows.Forms.Label labelgrid1;
        private System.Windows.Forms.Label labelgrid0;
        private System.Windows.Forms.Label labelSideOwn0;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label labAIGrade1;
        private System.Windows.Forms.Label labAIGrade0;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label labHost;
        private DevComponents.DotNetBar.Bar bar1;
        private DevComponents.DotNetBar.ButtonItem btnStart;
        private DevComponents.DotNetBar.ButtonItem btnHost;
        private DevComponents.DotNetBar.ButtonItem btnSelect;
        private DevComponents.DotNetBar.ButtonItem btnInstruct;
        private DevComponents.DotNetBar.LabelX labelX1;
        private System.Windows.Forms.RichTextBox txtSendMsg;
        private System.Windows.Forms.Button btnSendMsg;
        private System.Windows.Forms.ListBox listTalkMsg;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClear;
        private DevComponents.DotNetBar.ButtonItem btnCancelHost;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
    }
}