using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;

namespace Server
{
    class GameTable
    {
        private const int None = -1; //无棋子
        private const int Black = 0; //黑色棋子
        private const int White = 1; //白色棋子
        private int[,] grid = new int[8, 8];   //15*15的方格
        //private System.Timers.Timer timer;
        //Random rnd = new Random();
        Service service;
        public Player[] gamePlayer;
        private ListBox listbox;
        /// <summary>
        ///  0为黑方，1为白方
        /// </summary>
        private int turn = 0;


        public GameTable(ListBox listbox)
        {
            gamePlayer = new Player[2];   //两人对弈的桌子
            gamePlayer[0] = new Player();
            gamePlayer[1] = new Player();
            //timer = new System.Timers.Timer();
            //timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            //timer.Enabled = false;
            this.listbox = listbox;
            service = new Service(listbox);
            ResetGrid();
        }

        /// <summary>
        /// 初始化二维数组，无棋子
        /// </summary>
        public void ResetGrid()
        {
            for (int i = 0; i <= grid.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= grid.GetUpperBound(1); j++)
                {
                    grid[i, j] = None;
                }
            }
            gamePlayer[0].grade = 0;  //黑子和白子的初始分数为0
            gamePlayer[1].grade = 0;
        }
        //public void StartTimer()
        //{
        //    timer.Start();
        //}
        //public void StopTimer()
        //{
        //    timer.Stop();
        //}
        //void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //    int x, y;
        //    do
        //    {
        //        x = rnd.Next(15);
        //        y = rnd.Next(15);
        //    } while (grid[x, y] != None);
        //    SetDot(x, y, nextDotColor);
        //    nextDotColor = (nextDotColor + 1) % 2;

        //}

        /// <summary>
        /// 向两个用户发送点集合
        /// </summary>
        /// <param name="s"></param>
        public void SetDotCollection(string s)
        {
            service.SendToBoth(this, string.Format("SetDotCollection,{0}",s));
        }

        /// <summary>
        /// 设置棋子的坐标位置
        /// </summary>
        /// <param name="i">横坐标</param>
        /// <param name="j">纵坐标</param>
        /// <param name="side">棋子，0代表黑，1代表白</param>
        public void SetGrid(int i, int j, int side)
        {
            grid[i, j] = side;
        }

        /// <summary>
        /// 取消托管的时候，将turn的值和side的值相同
        /// </summary>
        /// <param name="_turn"></param>
        public void SetTurn(int _turn)
        {
            this.turn = _turn;
        }

        /// <summary>
        /// 下棋子，和吃棋子，
        /// </summary>
        /// <param name="i">行</param>
        /// <param name="j">列</param>
        /// <param name="side">白棋子还是黑棋子</param>
        public void SetDot(int i, int j, int side)
        {
            //存放需要改变颜色的棋子数组 格式 行,列,值
            List<string> dotCollection1 = new List<string>();
            List<string> dotCollection2 = new List<string>();
            List<string> dotCollection3 = new List<string>();
            List<string> dotCollection4 = new List<string>();
            List<string> dotCollection5 = new List<string>();
            List<string> dotCollection6 = new List<string>();
            List<string> dotCollection7 = new List<string>();
            List<string> dotCollection8 = new List<string>();   //不可能一下子改变8个棋子
            if (side != turn)
                return;
            else
            {
                //处理一般情况
                //考虑单列的情况（单列向上或向下查找）
                //1.单列中小于目标行的情况,目标行最小为第1行（下标为0），比较行此时没有
                //假设目标行为第5行（下标为4）则比较行k1依次为3，2，1，0
                //如果目标行为第1行（下标为0）则不需要比较小于目标行的情况
                //2.单列中大于目标行的情况,目标行最大为第8行（下标为7），比较行此时没有
                //假设目标行为第5行（下标为4）则比较行k2依次为5,6,7
                //如果目标行为第8行（下标为7）则不需要比较大于目标行的情况
                #region 上下左右
                for (int k1 = i - 1; k1 >= 0; k1--)
                {
                    //从最靠近目标行的行数开始，逐个检验同列中棋子颜色，遇到相同颜色或者无子即退出，连续不同存入
                    //如果检测位置无子，则晴空dotCollection，即无法改变dotCollection棋子颜色
                    //如果有同颜色的子，则退出
                    //如果有不同颜色的子且不为第1行，则记录，否则清空列表。假设在第1列第1-4行为黑子，白方在第1列第5行打算落下白子，
                    //从向上的方向比较，当检测到第1行时，该方向列表中已有3个子，但第一行为黑子，无法反转1-4行黑子，所以清空已有列表。
                    if(grid[k1,j]==None)  //第j列，先向上查找，再向下查找
                    {
                        dotCollection1.Clear();  //表示棋子上面没有与自己相同的的棋子，说明没有夹在两个相同棋子之间，则清除所有不继续网上查找
                        break;
                    }
                    if (grid[k1, j] != side)  //表示与自己不同颜色的棋子
                    {
                        if (k1 != 0)   //没到上边界
                        {
                            dotCollection1.Add(string.Format("{0},{1},{2}", k1, j, side));
                        }
                        else  //表示到达了上边界了，但上边界的棋子与自己的颜色不同，则也清除所有记录
                        {
                            dotCollection1.Clear();
                            break;
                        }
                        //grid[k1, j] = side;
                    }
                    else
                    {
                        break;  //表示遇到与自己相同的棋子了 退出
                    }
                }
                for (int k2 = i + 1; k2 <= 7; k2++)  //向下
                {
                    //从最靠近目标行的行数开始，逐个检验同列中棋子颜色，遇到相同颜色或者无子即退出，连续不同存入
                    //如果检测位置无子，则晴空dotCollection，即无法改变dotCollection棋子颜色
                    //如果有子，则记录
                    if (grid[k2, j] == None)
                    {
                        dotCollection2.Clear();
                        break;
                    }
                    if (grid[k2, j] != side)
                    {
                        if (k2 != 7)
                        {
                            dotCollection2.Add(string.Format("{0},{1},{2}", k2, j, side));
                            //grid[k2, j] = side;
                        }
                        else
                        {
                            dotCollection2.Clear();
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                //考虑单行的情况
                //1.单列中小于目标列的情况,目标列最小为第3列（下标为2），比较列最小为第2列（下标为1）
                //2.单列中大于目标列的情况,目标列最大为第6列（下标为5），比较列最大为第7列（下标为6）
                for (int k3 = j - 1; k3 >= 0; k3--)   //向左查找
                {
                    //从最靠近目标行的行数开始，逐个检验同列中棋子颜色，遇到相同即退出，连续不同存入
                    if (grid[i,k3] == None)
                    {
                        dotCollection3.Clear();
                        break;
                    }
                    if (grid[i, k3] != side)
                    {
                        if (k3 != 0)
                        {
                            dotCollection3.Add(string.Format("{0},{1},{2}", i, k3, side));
                            //grid[i, k3] = side;
                        }
                        else
                        {
                            dotCollection3.Clear();
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                for (int k4 = j + 1; k4 <= 7; k4++)  //向右查找
                {
                    //从最靠近目标行的行数开始，逐个检验同列中棋子颜色，遇到相同即退出，连续不同存入
                    if (grid[i, k4] == None)
                    {
                        dotCollection4.Clear();
                        break;
                    }
                    if (grid[i, k4] != side)
                    {
                        if (k4 != 7)
                        {
                            dotCollection4.Add(string.Format("{0},{1},{2}", i, k4, side));
                            //grid[i, k4] = side;
                        }
                        else
                        {
                            dotCollection4.Clear();
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                #endregion
                #region 左上 左下 右上 右下
                //左上
                for (int k5 = i - 1,m5=j-1; k5 >= 0&&m5>=0; k5--,m5--)
                {

                    if (grid[k5, m5] == None)
                    {
                        dotCollection5.Clear();
                        break;
                    }
                    if (grid[k5, m5] != side)
                    {
                        if (k5 != 0 && m5!=0)
                        {
                            dotCollection5.Add(string.Format("{0},{1},{2}", k5, m5, side));
                        }
                        else
                        {
                            dotCollection5.Clear();
                            break;
                        }
                        //grid[k1, j] = side;
                    }
                    else
                    {
                        break;
                    }
                }
                //右下
                for (int k6 = i + 1,m6=j+1; k6 <= 7&&m6<=7; k6++,m6++)
                {
                    //从最靠近目标行的行数开始，逐个检验同列中棋子颜色，遇到相同颜色或者无子即退出，连续不同存入
                    //如果检测位置无子，则晴空dotCollection，即无法改变dotCollection棋子颜色
                    //如果有子，则记录
                    if (grid[k6, m6] == None)
                    {
                        dotCollection6.Clear();
                        break;
                    }
                    if (grid[k6, m6] != side)
                    {
                        if (k6 != 7&&m6!=7)
                        {
                            dotCollection6.Add(string.Format("{0},{1},{2}", k6, m6, side));
                            //grid[k2, j] = side;
                        }
                        else
                        {
                            dotCollection6.Clear();
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                //左下
                for (int k7 = j - 1,m7=i+1; k7 >= 0&&m7<=7; k7--,m7++)
                {
                    //从最靠近目标行的行数开始，逐个检验同列中棋子颜色，遇到相同即退出，连续不同存入
                    if (grid[m7, k7] == None)
                    {
                        dotCollection7.Clear();
                        break;
                    }
                    if (grid[m7, k7] != side)
                    {
                        if (k7 != 0&&m7!=7)
                        {
                            dotCollection7.Add(string.Format("{0},{1},{2}", m7, k7, side));
                            //grid[i, k3] = side;
                        }
                        else
                        {
                            dotCollection7.Clear();
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                //右上
                for (int k8 = j + 1,m8=i-1; k8 <= 7&&m8>=0; k8++,m8--)
                {
                    //从最靠近目标行的行数开始，逐个检验同列中棋子颜色，遇到相同即退出，连续不同存入
                    if (grid[m8, k8] == None)
                    {
                        dotCollection8.Clear();
                        break;
                    }
                    if (grid[m8, k8] != side)
                    {
                        if (k8 != 7&&m8!=0)
                        {
                            dotCollection8.Add(string.Format("{0},{1},{2}", m8, k8, side));
                            //grid[i, k4] = side;
                        }
                        else
                        {
                            dotCollection8.Clear();
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                #endregion

                #region 吃掉对方的棋子，即将对方棋子的颜色变成自己的颜色
                foreach (string dot in dotCollection1)
                {
                    string[] dotset = dot.Split(',');
                    grid[int.Parse(dotset[0]), int.Parse(dotset[1])] = int.Parse(dotset[2]);

                }
                foreach (string dot in dotCollection2)
                {
                    string[] dotset = dot.Split(',');
                    grid[int.Parse(dotset[0]), int.Parse(dotset[1])] = int.Parse(dotset[2]);

                }
                foreach (string dot in dotCollection3)
                {
                    string[] dotset = dot.Split(',');
                    grid[int.Parse(dotset[0]), int.Parse(dotset[1])] = int.Parse(dotset[2]);

                }
                foreach (string dot in dotCollection4)
                {
                    string[] dotset = dot.Split(',');
                    grid[int.Parse(dotset[0]), int.Parse(dotset[1])] = int.Parse(dotset[2]);

                }
                foreach (string dot in dotCollection5)
                {
                    string[] dotset = dot.Split(',');
                    grid[int.Parse(dotset[0]), int.Parse(dotset[1])] = int.Parse(dotset[2]);

                }
                foreach (string dot in dotCollection6)
                {
                    string[] dotset = dot.Split(',');
                    grid[int.Parse(dotset[0]), int.Parse(dotset[1])] = int.Parse(dotset[2]);

                }
                foreach (string dot in dotCollection7)
                {
                    string[] dotset = dot.Split(',');
                    grid[int.Parse(dotset[0]), int.Parse(dotset[1])] = int.Parse(dotset[2]);

                }
                foreach (string dot in dotCollection8)
                {
                    string[] dotset = dot.Split(',');
                    grid[int.Parse(dotset[0]), int.Parse(dotset[1])] = int.Parse(dotset[2]);

                } 
                #endregion

                //dotCollection为空，说明此处无法下子
                if (dotCollection1.Count == 0 && dotCollection2.Count == 0 && dotCollection3.Count == 0 && dotCollection4.Count == 0
                    && dotCollection5.Count == 0 && dotCollection6.Count == 0 && dotCollection7.Count == 0 && dotCollection8.Count == 0)
                {
                    //MessageBox.Show("您没有位置可下了！");
                    return;
                }
                else
                {
                    //先将下子的位置设值
                    grid[i, j] = side;
                    service.SendToBoth(this, string.Format("SetDot,{0},{1},{2}", i, j, side));
                    string sTran="";
                    foreach (string sunit in dotCollection1)
                    {
                        sTran += sunit + ",";
                    }
                    foreach (string sunit in dotCollection2)
                    {
                        sTran += sunit + ",";
                    }
                    foreach (string sunit in dotCollection3)
                    {
                        sTran += sunit + ",";
                    }
                    foreach (string sunit in dotCollection4)
                    {
                        sTran += sunit + ",";
                    }
                    foreach (string sunit in dotCollection5)
                    {
                        sTran += sunit + ",";
                    }
                    foreach (string sunit in dotCollection6)
                    {
                        sTran += sunit + ",";
                    }
                    foreach (string sunit in dotCollection7)
                    {
                        sTran += sunit + ",";
                    }
                    foreach (string sunit in dotCollection8)
                    {
                        sTran += sunit + ",";
                    }
                    sTran = sTran.Substring(0, sTran.Length - 1);
                    int otherside=(side+1)%2;     //取余，0或1
                    for (int ii = 0; ii <= grid.GetUpperBound(0); ii++)
                    {
                        for (int jj = 0; jj <= grid.GetUpperBound(1); jj++)
                        {                         
                            if (grid[ii, jj] == side)
                                this.gamePlayer[side].grade++;
                            if (grid[ii,jj]==otherside)
                                this.gamePlayer[otherside].grade++;
                        }
                    }
                    turn = (turn + 1) % 2;
                    service.SendToBoth(this, string.Format("SetDotCollection,{0},{1},{2},{3}", sTran,this.gamePlayer[side].grade,this.gamePlayer[otherside].grade,turn));
                    this.gamePlayer[side].grade = 0;
                    this.gamePlayer[otherside].grade = 0;
                }
            }
            //grid[i, j] = side;
            //service.SendToBoth(this, string.Format("SetDot,{0},{1},{2}", i, j, dotColor));
            ////判断是否有相邻点
            //int k1, k2;//k1:循环初值,k2：循环中值
            //if (i == 0)
            //{
            //    k1 = k2 = 1;
            //}
            //else if (i == grid.GetUpperBound(0))
            //{
            //    k1 = k2 = grid.GetUpperBound(0) - 1;
            //}
            //else
            //{
            //    k1 = i - 1; k2 = i + 1;
            //}
            //for (int x = k1; x <= k2; x += 2)
            //{
            //    if (grid[x, j] == dotColor)
            //    {
            //        ShowWin(dotColor);
            //    }
            //}
            //if (j == 0)
            //{
            //    k1 = k2 = 1;
            //}
            //else if (j == grid.GetUpperBound(1))
            //{
            //    k1 = k2 = grid.GetUpperBound(1) - 1;
            //}
            //else
            //{
            //    k1 = j - 1; k2 = j + 1;
            //}
            //for (int y = k1; y <= k2; y += 2)
            //{
            //    if (grid[i, y] == dotColor)
            //    {
            //        ShowWin(dotColor);
            //    }
            //}
        }
        private void ShowWin(int dotColor)
        {
            //timer.Enabled = false;
            gamePlayer[0].started = false;
            gamePlayer[1].started = false;
            this.ResetGrid();
            service.SendToBoth(this,String.Format("Win,{0},{1},{2}",dotColor,gamePlayer[0].grade,gamePlayer[1].grade));

        }
    }
}
