using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace Client
{
    public partial class FrmMain : OfficeForm
    {
        public FrmMain()
        {
            InitializeComponent();
            labAI.Parent = pictureBox1;
            labPeople.Parent = pictureBox1;
            labAuthor.Parent = pictureBox1;
        }

        private void labPeople_MouseEnter(object sender, EventArgs e)
        {
            //移动变化颜色 
            this.labPeople.ForeColor = Color.Red;
        }

        private void labPeople_MouseLeave(object sender, EventArgs e)
        {
            //离开变化颜色 
            this.labPeople.ForeColor = Color.White;
        }

        private void labAI_MouseEnter(object sender, EventArgs e)
        {
            this.labAI.ForeColor = Color.Red;
            //this.labAI.Font = new Font(this.Font.FontFamily, 20);
        }

        private void labAI_MouseLeave(object sender, EventArgs e)
        {
            this.labAI.ForeColor = Color.White;
        }

        /// <summary>
        /// 进入人机对弈模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labAI_Click(object sender, EventArgs e)
        {
            BizLogic.frmMain = this;
            BizLogic.frmMain.Hide();
            FrmAI frmAI = new FrmAI();
            frmAI.ShowDialog();

            //this.Hide();
        }

        /// <summary>
        /// 进入联机对弈模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labPeople_Click(object sender, EventArgs e)
        {
            BizLogic.frmMain = this;
            BizLogic.frmMain.Hide();
            FrmClient frmClient = new FrmClient();
            frmClient.ShowDialog();
        }
    }
}
