namespace Client
{
    partial class FrmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.styleManager1 = new DevComponents.DotNetBar.StyleManager(this.components);
            this.labAI = new DevComponents.DotNetBar.LabelX();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labPeople = new DevComponents.DotNetBar.LabelX();
            this.labAuthor = new DevComponents.DotNetBar.LabelX();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // styleManager1
            // 
            this.styleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.Office2007VistaGlass;
            this.styleManager1.MetroColorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255))))), System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(115)))), ((int)(((byte)(199))))));
            // 
            // labAI
            // 
            this.labAI.AutoSize = true;
            this.labAI.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labAI.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labAI.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labAI.Font = new System.Drawing.Font("华文行楷", 21.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labAI.ForeColor = System.Drawing.Color.White;
            this.labAI.Location = new System.Drawing.Point(16, 241);
            this.labAI.Name = "labAI";
            this.labAI.Size = new System.Drawing.Size(142, 40);
            this.labAI.TabIndex = 1;
            this.labAI.Text = "人机对弈";
            this.labAI.TextAlignment = System.Drawing.StringAlignment.Center;
            this.labAI.Click += new System.EventHandler(this.labAI_Click);
            this.labAI.MouseEnter += new System.EventHandler(this.labAI_MouseEnter);
            this.labAI.MouseLeave += new System.EventHandler(this.labAI_MouseLeave);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(432, 522);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // labPeople
            // 
            this.labPeople.AutoSize = true;
            this.labPeople.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labPeople.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labPeople.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labPeople.Font = new System.Drawing.Font("华文行楷", 21.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labPeople.ForeColor = System.Drawing.Color.White;
            this.labPeople.Location = new System.Drawing.Point(274, 243);
            this.labPeople.Name = "labPeople";
            this.labPeople.Size = new System.Drawing.Size(142, 40);
            this.labPeople.TabIndex = 1;
            this.labPeople.Text = "玩家对弈";
            this.labPeople.TextAlignment = System.Drawing.StringAlignment.Center;
            this.labPeople.Click += new System.EventHandler(this.labPeople_Click);
            this.labPeople.MouseEnter += new System.EventHandler(this.labPeople_MouseEnter);
            this.labPeople.MouseLeave += new System.EventHandler(this.labPeople_MouseLeave);
            // 
            // labAuthor
            // 
            this.labAuthor.AutoSize = true;
            this.labAuthor.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labAuthor.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labAuthor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labAuthor.Font = new System.Drawing.Font("华文行楷", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labAuthor.ForeColor = System.Drawing.Color.Black;
            this.labAuthor.Location = new System.Drawing.Point(206, 473);
            this.labAuthor.Name = "labAuthor";
            this.labAuthor.Size = new System.Drawing.Size(208, 28);
            this.labAuthor.TabIndex = 8;
            this.labAuthor.Text = "计科11401  明章强、余程";
            this.labAuthor.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.ClientSize = new System.Drawing.Size(432, 522);
            this.Controls.Add(this.labAuthor);
            this.Controls.Add(this.labPeople);
            this.Controls.Add(this.labAI);
            this.Controls.Add(this.pictureBox1);
            this.DoubleBuffered = true;
            this.EnableGlass = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "黑白棋";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.StyleManager styleManager1;
        private DevComponents.DotNetBar.LabelX labAI;
        private System.Windows.Forms.PictureBox pictureBox1;
        private DevComponents.DotNetBar.LabelX labPeople;
        private DevComponents.DotNetBar.LabelX labAuthor;
    }
}