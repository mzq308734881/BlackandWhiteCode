using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Collections;
using DevComponents.DotNetBar;
using System.Drawing.Imaging;

namespace Client
{
    public partial class FrmAI : Office2007Form
    {
        #region    //定义或初始化 全局变量
        private int _num = 0;            //当前的步数，走一步就加一
        private int _flag = 1;            //等级的选择
        private bool _begin = false;  //程序是否开始

        private bool _rbAI = true;    //是否人机对弈,默认人机对战
        private bool _rbLow = true;
        private bool _rbMiddle = false;
        private bool _rbDifficult = false;
        private bool _rbPCBlack = false;
        private bool _rbPeopleBlack = true;    //用来判断 电脑还是人执黑,默认人执黑

        private bool _over = false;     //判断游戏是否结束
        private bool _personDown = false;   //用于判断人是否落子（人机对弈时）
        private bool _isBlack = true;       //当前是否是黑方下棋
        public int[,] _xy = new int[8, 8]; //棋面
        //定义结构体类型的数组，里面存放棋子位置的二维数组
        Qipan[] _qipan = new Qipan[64];  //定义64个棋盘结构，用来储存棋面，从而实现悔棋；
        //Bitmap是从Image类继承的一个图像类，它封装了Windows位图操作的常用功能
        //Image img = Bitmap;
        Bitmap bitmap = new Bitmap(462, 426);   //位图的长和宽
        //Bitmap bitmap = new Bitmap("grid.gif");   //位图的长和宽
        Bitmap bitmap2 = new Bitmap(116, 138);

        private int _blackCount = 0, _whiteCount = 0;  //用来显示双方的棋子数
        private int x = -1, y = -1;              //当前落子的位置
        private int _regret = 3;   //悔棋的步数，如果人人对弈每个人都可以悔一步棋，人机对弈时，机器不能悔棋，直接退回两步棋
        private Bitmap blackBitmap;
        private Bitmap whiteBitmap;
        private Bitmap cBlackBitmap;
        private Bitmap cWhiteBitmap;
        private Bitmap dBlackBitmap;
        private Bitmap dWhiteBitmap;

        /// <summary>
        ///    //结构体，用于存放棋子的位置，将每一步的位置都记录在二维数组_localtion[,]中
        /// </summary>
        public struct Qipan
        {
            public int[,] _localtion;
        }
        #endregion

        #region //程序的入口
        public FrmAI()
        {
            InitializeComponent();
            //draw_QiPan();
            labBlack.Visible = labWhite.Visible = false;
            btnBack.Enabled = false;

            blackBitmap = new Bitmap("black.png");
            whiteBitmap = new Bitmap("white.png");

            cBlackBitmap = new Bitmap("CanputBlack.gif");
            cWhiteBitmap = new Bitmap("CanputWhite.gif");
            dBlackBitmap = new Bitmap("DownedBlack.png");
            dWhiteBitmap = new Bitmap("DownedWhite.png");

            listBoxAIInfo.Items.Add("默认玩家执黑子，AI等级为低级！");
        }
        // 
        #endregion
        #region 设置默认选项
        /// <summary>
        /// 保存（设置）选项信息
        /// </summary>
        /// <param name="selectStand"></param>
        private void SetInitSelectInfo(SelectStand selectStand)
        {
            //selectStand.rbAI.Checked = _rbAI;
            //selectStand.rbPeople.Checked = _rbPeople;

            selectStand.rbLow.Checked = _rbLow;
            selectStand.rbMiddle.Checked = _rbMiddle;
            selectStand.rbDifficult.Checked = _rbDifficult;

            selectStand.rbPCBlack.Checked = _rbPCBlack;
            selectStand.rbPeopleBlack.Checked = _rbPeopleBlack;
        }
        /// <summary>
        /// 得到选项信息
        /// </summary>
        /// <param name="selectStand"></param>
        private void GetSelectedInfo(SelectStand selectStand)
        {
            string str = "选择";
            _rbAI = true;
            //_rbPeople = selectStand.rbPeople.Checked;

            if (selectStand.rbPCBlack.Checked)
                str +="机器执黑，机器先走一步，";
            else
                str += "玩家执黑，";
            if (selectStand.rbLow.Checked)
                str += "AI等级为低级！";
            else if (selectStand.rbMiddle.Checked)
                str += "AI等级为中级！";
            else
                str += "AI等级为高级！";

            _rbLow = selectStand.rbLow.Checked;
            _rbMiddle = selectStand.rbMiddle.Checked;
            _rbDifficult = selectStand.rbDifficult.Checked;

            _rbPCBlack = selectStand.rbPCBlack.Checked;
            _rbPeopleBlack = selectStand.rbPeopleBlack.Checked;
            listBoxAIInfo.Items.Clear();
            listBoxAIInfo.Items.Add(str);
        }
        #endregion

        #region //选项窗口
        private void btnSelect_Click(object sender, EventArgs e)
        {
            SelectStand _selectStand = new SelectStand();
            //SetInitSelectInfo(_selectStand);  //选择的时候为历史记录
            _selectStand.ShowDialog();
            if(_selectStand._bool)
            {
                GetSelectedInfo(_selectStand);
            }
            //_rbAI = _selectStand.rbAI.Checked;  // 人机还是人人对弈的判断
            if (_rbAI == true)                  //如果人机对弈
            {
                if (_selectStand.rbLow.Checked == true)
                {
                    _flag = 1;
                }
                else if (_selectStand.rbMiddle.Checked == true)
                {
                    _flag = 2;
                }
                else
                    _flag = 3;

                _regret = 3;                    //人机对弈时，如果悔棋，退两步
            }
            else     //人人对弈时
            {
                _regret = 2;                   //人人对弈时，如果悔棋，退一步
            }
            if (_selectStand.rbPCBlack.Checked == true)  //先手的判断
            {
                _rbPeopleBlack = false;   //电脑执黑
            }
            else
            {
                _rbPeopleBlack = true;    //玩家执黑
            }

        }
        #endregion

        #region  //游戏开始
        /// <summary>
        /// 开始游戏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butGo_Click(object sender, EventArgs e)
        {
            listBoxAIInfo.Items.Add("开始新游戏！");
            _xy[3, 3] = _xy[4, 4] = 1;   //设置黑子默认位置
            _xy[3, 4] = _xy[4, 3] = -1;  //设置白子默认位置
            this.Draw_QiZi();    //画棋子
            pictureBox2.Invalidate();

            Savefor();   //存储棋面，用于悔棋
            _num++; //初始化棋盘
            _begin = true;
            labBlack.Visible = true;
            btnSelect.Enabled = btnGo.Enabled = false;

            #region//如果电脑执黑 电脑先走一步
            if (_rbPeopleBlack == false)
            {
                _num = _num - 1;      //为了悔棋，否则玩家还没等可以下棋时，就已经可以悔棋了。
                if (_flag == 1)
                {
                    int row;
                    int col;
                    ComputerDownLow(out row,out col);
                    _personDown = false;
                    //this.Draw_QiZi();
                    pictureBox2.Invalidate();
                    if (canDown(_isBlack) == true)
                    {
                        listBoxAIInfo.Items.Add(string.Format("机器落子位置：({0}，{1})", row + 1, col + 1));
                        Savefor(); _num++;//储存当前棋面
                    }
                    else
                    {
                        listBoxAIInfo.Items.Add(string.Format("机器无子可下！"));
                    }
                    EndGame();
                }
                if (_flag == 2)
                {
                    int row;
                    int col;
                    ComputerDownMid(out row, out col);
                    _personDown = false;
                    //this.Draw_QiZi();
                    pictureBox2.Invalidate();
                    if (canDown(_isBlack) == true)
                    {
                        listBoxAIInfo.Items.Add(string.Format("机器落子位置：({0}，{1})", row + 1, col + 1));
                        Savefor(); _num++;//储存当前棋面
                    }
                    else
                    {
                        listBoxAIInfo.Items.Add(string.Format("机器无子可下！"));
                    }
                    EndGame();
                }

                if (_flag == 3)
                {
                    int row;
                    int col;
                    ComputerDownDiff(out row, out col);
                    _personDown = false;
                    //this.Draw_QiZi();
                    pictureBox2.Invalidate();
                    if (canDown(_isBlack) == true)
                    {
                        listBoxAIInfo.Items.Add(string.Format("机器落子位置：({0}，{1})", row + 1, col + 1));
                        Savefor(); _num++;//储存当前棋面
                    }
                    else
                    {
                        listBoxAIInfo.Items.Add(string.Format("机器无子可下！"));
                    }
                    EndGame();
                }
            }
            #endregion
        }
        #endregion

        #region //重新开始
        private void btnNewGame_Click(object sender, EventArgs e)
        {
            _num = 0;     //重新开始记录步数
            //_flag = 1;     //判断等级,默认等级为低级,重新开始时不需要修改难度等级
            _regret = 3;   //表示悔棋的步数
            //_rbPeopleBlack = true;
            _begin = false;
            labBlackCount.Text = "";
            labWhiteCount.Text = "";
            //_rbAI = true;         //是否人机对弈
            _over = false;          //判断游戏是否结束
            _personDown = true;      //用于判断人是否落子（人机对弈时）
            _isBlack = true;
            _xy = new int[8, 8];
            _qipan = new Qipan[64];
            //draw_QiPan();
            pictureBox2.Invalidate();
            labBlack.Visible = labWhite.Visible = false;
            pictureBox2.Enabled = true;
            btnSelect.Enabled = btnGo.Enabled = true;
            btnBack.Enabled = false;
            listBoxAIInfo.Items.Clear();
            listBoxAIInfo.Items.Add("默认玩家执黑子，AI等级为低级！");
        }
        #endregion

        #region //退出
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();  //结束所用进程
        }
        #endregion

        #region //悔棋程序
        /// <summary>
        /// 悔棋程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBack_Click(object sender, EventArgs e)
        {
            if (_rbAI == true)  //人机对战，退回两步棋，只有人机对战的时候允许悔棋
            {
                #region //退回一步，回到之前棋局
                _num = _num - _regret;
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (_num == 0)   //说明恰好走了_regret步
                        {
                            _xy[i, j] = _qipan[0]._localtion[i, j];
                        }
                        else if (_num > 0)
                        {
                            _xy[i, j] = _qipan[_num]._localtion[i, j];
                        }
                    }
                }
                for (int k = _num + 1; k < 61 && k > 0; k++)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            _qipan[k]._localtion = new int[8, 8];
                        }
                    }
                } 
                #endregion
                Draw_QiZi();
                _num++;
            }
            else  //人人对战，各自悔棋一步
            {
                _num = _num - _regret;
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (_num == 0)
                        {
                            _xy[i, j] = _qipan[0]._localtion[i, j];
                        }
                        else
                        {
                            _xy[i, j] = _qipan[_num]._localtion[i, j];
                        }
                    }
                }
                for (int k = _num + 1; k < 61 && k > 0; k++)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            _qipan[k]._localtion = new int[8, 8];
                        }
                    }
                }
                _isBlack = !_isBlack;
                Draw_QiZi();
                _num++;
            }
            listBoxAIInfo.Items.Add("玩家悔棋！");
            _regret--;
        }
        #endregion

        #region //显示鼠标位置
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Start();
            Point p = PointToClient(MousePosition);
            label2.Text = p.X.ToString() + "  " + p.Y.ToString();
        }
        #endregion

        #region //落子程序
        /// <summary>
        /// //落子程序
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private bool In_QiZi(int x, int y, out int row, out int col)
        {
            bool canEat = true;
            row = -1;
            col = -1;
            if ((x >= 41 & x <= 361) & (y >= 74 & y < 394))
            {  //得到整数位置
                row = x = (x - 41) % 40 > 0 ? (int)((x - 41) / 40) : (int)((x - 41) / 40) - 1;
                col = y = (y - 74) % 40 > 0 ? ((int)((y - 74) / 40)) : (int)((y - 74) / 40) - 1;
                if (_xy[x, y] == 0)   //表示空白位置
                {
                    if (this._isBlack)
                    {
                        _xy[x, y] = 1;//表示黑子
                        canEat = Eaten(x, y);
                        if (canEat == false)
                        {
                            _xy[x, y] = 0;
                            // MessageBox.Show("此处不能落子");
                            return false;
                        }
                        //MessageBox.Show(x + "  " + y);
                        else
                        {
                            Eat(x, y); _personDown = true; this.x = x; this.y = y;
                        }
                    }
                    else
                    {
                        _xy[x, y] = -1;
                        canEat = Eaten(x, y);
                        if (canEat == false)
                        {
                            _xy[x, y] = 0;
                            return false;
                            //  MessageBox.Show("此处不能落子"); 
                        }//表示白子
                        else
                        {
                            Eat(x, y); _personDown = true; this.x = x; this.y = y;
                        }
                    }

                    this._isBlack = !this._isBlack;
                    //service.SendToServer(string.Format("SetDotCollection,{0},{1},{2},{3},{4}", tableIndex, side,true, x, y));
                    return true;
                }
                else
                {
                    // MessageBox.Show("此处已有棋子，不能落子");
                    return false;
                }
            }
            else
                return false;
        }
        #endregion

        #region 画棋盘  
        /// <summary>
        ///初始化棋盘
        /// </summary>
        public void draw_QiPan()
        {
            Graphics g = Graphics.FromImage(bitmap);
            SolidBrush orgSoildBrush = new SolidBrush(Color.White);  //笔刷底色为蓝色
            Pen LinePen = new Pen(Color.Black);   //线边线为黑色
            g.FillRectangle(orgSoildBrush, 0, 0, 600, 600);   //填充颜色
            for (int x_ = 74; x_ <= 394; x_ += 40)
            {
                g.DrawLine(LinePen, x_, 74, x_, 394);  //横轴相同画9条竖线
            }
            for (int y_ = 74; y_ <= 394; y_ += 40)
            {
                g.DrawLine(LinePen, 74, y_, 394, y_);  //纵轴相同画9条横线
            }
            this.pictureBox2.Image = bitmap;   //设置将棋盘添加到pictureBox面板
            if (_num <= 0)   //没开始走不允许悔棋
            {
                btnBack.Enabled = false;
            }
            else
                btnBack.Enabled = true;
        }
        #endregion

        #region //画棋子程序
        /// <summary>
        /// 画棋子
        /// </summary>
        public void Draw_QiZi()
        {
            //draw_QiPan();
            int x1, y1;  //棋子的坐标
            int _blackTempCount = 0;           //黑子临时棋子数计数
            int _whiteTempCount = 0;         //白子临时棋子数计数
            Image img = pictureBox2.Image;
            //如果原图片是索引像素格式之列的，则需要转换成bmp格式

            Bitmap bmp = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);  //得到棋盘
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    x1 = 40 * i + 74;
                    y1 = 40 * j + 74;
                    if (_xy[i, j] == 1)  //黑子
                    {
                        Point point = new Point(x1, y1);
                        g.DrawImage(blackBitmap, point);  //画黑子
                        _blackTempCount++;
                        // MessageBox.Show(i.ToString() + "  " + j.ToString());
                    }
                    else if (_xy[i, j] == -1)  //白子
                    {
                        // MessageBox.Show(i.ToString() + "  " + j.ToString());
                        Point point = new Point(x1, y1);
                        g.DrawImage(whiteBitmap, point);  //画黑子
                        _whiteTempCount++;
                    }
                    else  //此处既不是白子又不是黑子
                    {
                        if (_isBlack == true)//当前是否黑方下子
                        {
                            _xy[i, j] = 1;
                        }
                        else
                        {
                            _xy[i, j] = -1;
                        }
                        if (Eaten(i, j) == true)
                        {
                            if (_isBlack == true)   //当前是否黑方下子
                            {
                                Point point = new Point(x1, y1);
                                g.DrawImage(cBlackBitmap, point);  //红框代表玩家执黑子
                            }
                            else
                            {
                                Point point = new Point(x1, y1);
                                g.DrawImage(cWhiteBitmap, point); //蓝框代表玩家执黑子
                            }
                        }
                        _xy[i, j] = 0;
                    }
                }
            }
            if (x != -1 && y != -1)
            {
                if (_xy[this.x, this.y] == 1)
                {
                    x1 = 40 * this.x + 74;
                    y1 = 40 * this.y + 74;
                    Point point = new Point(x1, y1);
                    g.DrawImage(dBlackBitmap, point);
                }

                else if (_xy[this.x, this.y] == -1)
                {
                    x1 = 40 * this.x + 74;
                    y1 = 40 * this.y + 74;
                    Point point = new Point(x1, y1);
                    g.DrawImage(dWhiteBitmap, point);
                }
            }
            pictureBox2.Invalidate();

            _blackCount = _blackTempCount;
            _whiteCount = _whiteTempCount;
            labBlackCount.Text = _blackTempCount.ToString();
            labWhiteCount.Text = _whiteTempCount.ToString();
            //this.pictureBox2.Image = bitmap;
        }
        #endregion

        #region//储存棋面
        /// <summary>
        /// //储存棋面,用于悔棋
        /// </summary>
        public void Savefor()
        {
            _qipan[_num]._localtion = new int[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    _qipan[_num]._localtion[i, j] = _xy[i, j];
                }
            }
        }
        #endregion

        #region //结束条件的判断以及结束游戏
        /// <summary>
        /// 结束条件的判断以及结束游戏
        /// </summary>
        public void EndGame()
        {
            #region  // 棋子是否下完了
            bool can = true;//判断某方是否有棋可下的标志
            bool win = true;
            int f1, f2;
            for (f1 = 0; f1 < 8; f1++)
            {
                for (f2 = 0; f2 < 8; f2++)
                {
                    if (_xy[f1, f2] == 0)
                    {
                        win = false; break;
                    }
                }
            }
            #endregion

            #region //判断是否有棋可下
            bool canChase = true;    //是否双方都无棋可下 
            can = canDown(_isBlack);
            if (win == false)
            {

                if (can == false)
                {
                    if (_isBlack == true)
                    {
                        MessageBox.Show("黑方无地方可落子，白方可继续落子");
                        listBoxAIInfo.Items.Add(string.Format("黑方无地方可落子，白方可继续落子"));
                        _personDown = !_personDown;
                    }
                    else
                    {
                        MessageBox.Show("白方无地方可落子，黑方可继续落子");
                        listBoxAIInfo.Items.Add(string.Format("白方无地方可落子，黑方可继续落子"));
                        _personDown = !_personDown;
                    }
                    _isBlack = !_isBlack;
                    Draw_QiZi();
                }
                if (canDown(_isBlack) == false) //如果对方也米地方可落子 则游戏结束
                {
                    canChase = false;
                }
            }
            #endregion

            #region  //判断输赢
            if (win == true || canChase == false)
            {

                if (_blackCount > _whiteCount)
                {
                    MessageBox.Show("游戏结束,黑方获胜");
                    listBoxAIInfo.Items.Add(string.Format("游戏结束,黑方获胜"));
                }
                else if (_blackCount < _whiteCount)
                {
                    MessageBox.Show("游戏结束,白方获胜");
                    listBoxAIInfo.Items.Add(string.Format("游戏结束,白方获胜"));
                }
                else
                {
                    MessageBox.Show(_blackCount + "  " + _whiteCount + "游戏结束,平局");
                    listBoxAIInfo.Items.Add(string.Format("平局，游戏结束！"));
                }

                pictureBox2.Enabled = false;
                labWhite.Visible = false;
                labBlack.Visible = false;
                btnSelect.Enabled = btnBack.Enabled = false;
                _over = true;
                return;
            }
            #endregion

            #region 显示该谁下子
            if (_isBlack)
            {
                labBlack.Visible = true;
                labWhite.Visible = false;

            }
            else
            {
                labBlack.Visible = false;
                labWhite.Visible = true;
            }

            #endregion
        }

        private void EndGame0()
        {
            #region  // 棋子是否下完了
            bool can = true;//判断某方是否有棋可下的标志
            bool win = true;
            int f1, f2;
            for (f1 = 0; f1 < 8; f1++)
            {
                for (f2 = 0; f2 < 8; f2++)
                {
                    if (_xy[f1, f2] == 0)
                    {
                        win = false; break;
                    }
                }
            }
            #endregion

            #region //判断是否有棋可下
            bool canChase = true;    //是否双方都无棋可下 
            can = canDown(_isBlack);
            if (win == false)
            {

                if (can == false)
                {
                    if (_isBlack == true)
                    {
                        MessageBox.Show("黑方无地方可落子，白方可继续落子");
                        listBoxAIInfo.Items.Add(string.Format("黑方无地方可落子，白方可继续落子"));
                        _personDown = !_personDown;
                    }
                    else
                    {
                        MessageBox.Show("白方无地方可落子，黑方可继续落子");
                        listBoxAIInfo.Items.Add(string.Format("白方无地方可落子，黑方可继续落子"));
                        _personDown = !_personDown;
                    }
                    _isBlack = !_isBlack;
                    Draw_QiZi();
                }
                if (canDown(_isBlack) == false) //如果对方也米地方可落子 则游戏结束
                {
                    canChase = false;
                }
            }
            #endregion

            #region  //判断输赢
            if (win == true || canChase == false)
            {

                if (_blackCount > _whiteCount)
                {
                    MessageBox.Show("游戏结束,黑方获胜");
                    listBoxAIInfo.Items.Add(string.Format("游戏结束,黑方获胜！"));
                }
                else if (_blackCount < _whiteCount)
                {
                    MessageBox.Show("游戏结束,白方获胜");
                    listBoxAIInfo.Items.Add(string.Format("游戏结束,白方获胜！"));
                }
                else
                {
                    MessageBox.Show(_blackCount + "  " + _whiteCount + "游戏结束,平局");
                    listBoxAIInfo.Items.Add(string.Format("平局,游戏结束！"));
                }
                pictureBox2.Enabled = false;
                labWhite.Visible = false;
                labBlack.Visible = false;
                btnSelect.Enabled = btnBack.Enabled = false;
                _over = true;
                return;
            }
            #endregion
        }

        #endregion

        #region //刷新棋盘及判断是否人机对弈
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            int row;
            int col;
            if (_begin == false)
            {
                return;
            }
            Point p = PointToClient(MousePosition);
            bool luozi = this.In_QiZi(p.X, p.Y, out row, out col);//判断点击是否有效

            pictureBox2.Invalidate();
            if (luozi == true)
            {
                listBoxAIInfo.Items.Add(string.Format("玩家落子位置：({0}，{1})", row + 1, col + 1));
                Savefor(); _num++;
            }//储存当前棋面

            EndGame();
            while (_personDown == true && _over == false && _rbAI == true && luozi == true)
            {
                if (_flag == 1)
                {
                    int row1;
                    int col1;
                    ComputerDownLow(out  row1,out  col1);
                    _personDown = false;
                    //this.Draw_QiZi();
                    pictureBox2.Invalidate();
                    if (canDown(_isBlack) == true)
                    {
                        listBoxAIInfo.Items.Add(string.Format("机器落子位置：({0}，{1})", row1 + 1, col1 + 1));
                        Savefor(); _num++;//储存当前棋面
                    }
                    else
                    {
                        listBoxAIInfo.Items.Add(string.Format("机器无子可下！"));
                    }
                    EndGame();
                }
                if (_flag == 2)
                {
                    int row2;
                    int col2;
                    ComputerDownMid(out row2, out col2);
                    _personDown = false;
                    //this.Draw_QiZi();
                    pictureBox2.Invalidate();
                    if (canDown(_isBlack) == true)
                    {
                        listBoxAIInfo.Items.Add(string.Format("机器落子位置：({0}，{1})", row2 + 1, col2 + 1));
                        Savefor(); _num++;//储存当前棋面
                    }
                    EndGame();
                }

                if (_flag == 3)
                {
                    int row3;
                    int col3;
                    ComputerDownDiff(out row3, out col3);
                    _personDown = false;
                    //this.Draw_QiZi();
                    pictureBox2.Invalidate();
                    if (canDown(_isBlack) == true)
                    {
                        listBoxAIInfo.Items.Add(string.Format("机器落子位置：({0}，{1})", row3 + 1, col3 + 1));
                        Savefor(); _num++;     //储存当前棋面
                    }
                    else
                    {
                        listBoxAIInfo.Items.Add(string.Format("机器无子可下！"));
                    }
                    EndGame();
                }
            }
            luozi = false;
            if (_regret < 3)
            {
                _regret++;
            }
            btnBack.Enabled = true;
        }
        #endregion

        #region  //吃子程序
        /// <summary>
        /// //吃子程序
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Eat(int x, int y)
        {
            #region //判断当前下子的颜色
            int a, b; //a,b 代表棋子的颜色 1为黑，-1为白
            if (_xy[x, y] == 1)
            {
                a = 1; b = -1;
            }
            else
            {
                a = -1; b = 1;
            }
            #endregion
            #region //吃子程序

            #region  //向右吃子
            int i = 0;
            for (i = x + 1; i < 8;) //若_xy[x, y]=1 为黑子b=-1，则计算右边白子的个数，若右边为黑子则停下。xy[x, y]=-1 为黑子b=1
            {   //则b棋子的颜色总是与_xy[x, y]相反
                if (_xy[i, y] == b)  //向右计算棋子的个数
                { ++i; continue; }
                else
                    break;
            }
            if (i > x + 1 && i < 8 && _xy[i, y] == a)//相邻棋子，_xy[i, y] 与a相同的颜色
            {
                for (int j = x + 1; j <= i - 1; j++) //若能吃对方棋子则将_xy[x, y]右边的所有变成与自己相同的颜色
                {
                    _xy[j, y] = a;
                }
            }
            #endregion

            #region  //向左吃子
            i = 0;
            for (i = x - 1; i >= 0;)
            {
                if (_xy[i, y] == b)
                { --i; continue; }
                else
                    break;
            }
            if (i < x - 1 && i >= 0 && _xy[i, y] == a)
            {
                //MessageBox.Show(x.ToString()+"  "+i.ToString());
                for (int j = x - 1; j > i; j--)
                {
                    _xy[j, y] = a;
                }
            }
            #endregion

            #region  //向上吃子
            i = 0;
            for (i = y - 1; i >= 0;)
            {
                if (_xy[x, i] == b)
                { --i; continue; }
                else
                    break;
            }
            if (i < y - 1 && i >= 0 && _xy[x, i] == a)
            {
                //MessageBox.Show(x.ToString()+"  "+i.ToString());
                for (int j = y - 1; j > i; j--)
                {
                    _xy[x, j] = a;
                }
            }
            #endregion

            #region  //向下吃子
            i = 0;
            for (i = y + 1; i < 8;)
            {
                if (_xy[x, i] == b)
                { ++i; continue; }
                else
                    break;
            }
            if (i > y + 1 && i < 8 && _xy[x, i] == a)
            {
                for (int j = y + 1; j <= i - 1; j++)
                {
                    _xy[x, j] = a;
                }
            }
            #endregion

            #region //向右上吃子
            int k = 0;
            for (i = x + 1, k = y - 1; i < 8 && k >= 0;)
            {
                if (_xy[i, k] == b)
                { ++i; --k; continue; }
                else
                    break;
            }
            if (i > x + 1 && k < y - 1 && i < 8 && k >= 0 && _xy[i, k] == a)
            {
                for (int j = x + 1, j1 = y - 1; j <= i - 1 && j1 > k; j++, j1--)
                {
                    _xy[j, j1] = a;
                }
            }
            #endregion

            #region   //向左下吃子
            k = 0;
            for (i = y + 1, k = x - 1; i < 8 && k >= 0;)
            {
                if (_xy[k, i] == b)
                { ++i; --k; continue; }
                else
                    break;
            }
            if (i > y + 1 && k < x - 1 && i < 8 && k >= 0 && _xy[k, i] == a)
            {
                for (int j = y + 1, j1 = x - 1; j <= i - 1 && j1 > k; j++, j1--)
                {
                    _xy[j1, j] = a;
                }
            }
            #endregion

            #region //向左上吃子
            k = 0;
            for (i = y - 1, k = x - 1; i >= 0 && k >= 0;)
            {
                if (_xy[k, i] == b)
                { --i; --k; continue; }
                else
                    break;
            }
            if (i < y - 1 && k < x - 1 && i >= 0 && k >= 0 && _xy[k, i] == a)
            {
                for (int j = y - 1, j1 = x - 1; j > i && j1 > k; j--, j1--)
                {
                    _xy[j1, j] = a;
                }
            }
            #endregion

            #region //向右下吃子
            k = 0;
            for (i = y + 1, k = x + 1; i < 8 && k < 8;)
            {
                if (_xy[k, i] == b)
                { ++i; ++k; continue; }
                else
                    break;
            }
            if (i > y + 1 && k > x + 1 && i < 8 && k < 8 && _xy[k, i] == a)
            {
                for (int j = y + 1, j1 = x + 1; j < i && j1 < k; j++, j1++)
                {
                    _xy[j1, j] = a;
                }
            }
            #endregion

            #endregion
        }
        #endregion

        #region //判断是否有子可下
        /// <summary>
        /// 判断是否有子可下
        /// </summary>
        /// <param name="isBlack"></param>
        /// <returns></returns>
        public bool canDown(bool isBlack)
        {
            bool _can = false;
            int a = 0;
            if (isBlack == true)
            {
                a = 1;
            }
            else
                a = -1;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (_xy[i, j] == 0)
                    {
                        _xy[i, j] = a;
                        if (Eaten(i, j) == true)
                        {
                            _can = true;
                            _xy[i, j] = 0;
                            break;
                        }
                        else
                        {
                            _xy[i, j] = 0;
                        }

                    }
                }
            }
            return _can;



        }
        #endregion

        #region //判断是否能够吃子
        /// <summary>
        /// 判断是否能够吃子
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Eaten(int x, int y)
        {
            bool _canEat = false;
            #region //判断当前下子的颜色
            int a, b; //a,b 代表棋子的颜色 1为黑，-1为白
            if (_xy[x, y] == 1)
            {
                a = 1; b = -1;
            }
            else
            {
                a = -1; b = 1;
            }
            #endregion
            #region //吃子程序

            #region  //向右吃子
            int i = 0;
            for (i = x + 1; i < 8;)
            {
                if (_xy[i, y] == b)
                { ++i; continue; }
                else
                    break;
            }
            if (i > x + 1 && i < 8 && _xy[i, y] == a)
            {
                _canEat = true;
            }
            #endregion

            #region  //向左吃子
            i = 0;
            for (i = x - 1; i >= 0;)
            {
                if (_xy[i, y] == b)
                { --i; continue; }
                else
                    break;
            }
            if (i < x - 1 && i >= 0 && _xy[i, y] == a)
            {

                _canEat = true;
            }
            #endregion

            #region  //向上吃子
            i = 0;
            for (i = y - 1; i >= 0;)
            {
                if (_xy[x, i] == b)
                { --i; continue; }
                else
                    break;
            }
            if (i < y - 1 && i >= 0 && _xy[x, i] == a)
            {
                _canEat = true;
            }
            #endregion

            #region  //向下吃子
            i = 0;
            for (i = y + 1; i < 8;)
            {
                if (_xy[x, i] == b)
                { ++i; continue; }
                else
                    break;
            }
            if (i > y + 1 && i < 8 && _xy[x, i] == a)
            {
                _canEat = true;
            }
            #endregion

            #region //向右上吃子
            int k = 0;
            for (i = x + 1, k = y - 1; i < 8 && k >= 0;)
            {
                if (_xy[i, k] == b)
                { ++i; --k; continue; }
                else
                    break;
            }
            if (i > x + 1 && k < y - 1 && i < 8 && k >= 0 && _xy[i, k] == a)
            {
                _canEat = true;
            }
            #endregion

            #region   //向左下吃子
            k = 0;
            for (i = y + 1, k = x - 1; i < 8 && k >= 0;)
            {
                if (_xy[k, i] == b)
                { ++i; --k; continue; }
                else
                    break;
            }
            if (i > y + 1 && k < x - 1 && i < 8 && k >= 0 && _xy[k, i] == a)
            {
                _canEat = true;
            }
            #endregion

            #region //向左上吃子
            k = 0;
            for (i = y - 1, k = x - 1; i >= 0 && k >= 0;)
            {
                if (_xy[k, i] == b)
                { --i; --k; continue; }
                else
                    break;
            }
            if (i < y - 1 && k < x - 1 && i >= 0 && k >= 0 && _xy[k, i] == a)
            {
                _canEat = true;
            }
            #endregion

            #region //向右下吃子
            k = 0;
            for (i = y + 1, k = x + 1; i < 8 && k < 8;)
            {
                if (_xy[k, i] == b)
                { ++i; ++k; continue; }
                else
                    break;
            }
            if (i > y + 1 && k > x + 1 && i < 8 && k < 8 && _xy[k, i] == a)
            {
                _canEat = true;
            }
            #endregion

            #endregion
            return _canEat;
        }

        #endregion

        #region  //人机对弈时，电脑判断哪步吃子多
        /// <summary>
        ///人机对弈时，电脑判断哪步吃子多
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Eatnum(int x, int y)
        {
            int num = 0;
            #region //判断当前下子的颜色
            int a, b; //a,b 代表棋子的颜色 1为黑，-1为白
            if (_xy[x, y] == 1)
            {
                a = 1; b = -1;
            }
            else
            {
                a = -1; b = 1;
            }
            #endregion
            #region //吃子程序

            #region  //向右吃子
            int i = 0;
            for (i = x + 1; i < 8;)
            {
                if (_xy[i, y] == b)
                { ++i; continue; }
                else
                    break;
            }
            if (i > x + 1 && i < 8 && _xy[i, y] == a)
            {
                for (int j = x + 1; j <= i - 1; j++)
                {
                    num++;
                }
            }
            #endregion

            #region  //向左吃子
            i = 0;
            for (i = x - 1; i >= 0;)
            {
                if (_xy[i, y] == b)
                { --i; continue; }
                else
                    break;
            }
            if (i < x - 1 && i >= 0 && _xy[i, y] == a)
            {
                //MessageBox.Show(x.ToString()+"  "+i.ToString());
                for (int j = x - 1; j > i; j--)
                {
                    num++;
                }
            }
            #endregion

            #region  //向上吃子
            i = 0;
            for (i = y - 1; i >= 0;)
            {
                if (_xy[x, i] == b)
                { --i; continue; }
                else
                    break;
            }
            if (i < y - 1 && i >= 0 && _xy[x, i] == a)
            {
                //MessageBox.Show(x.ToString()+"  "+i.ToString());
                for (int j = y - 1; j > i; j--)
                {
                    num++;
                }
            }
            #endregion

            #region  //向下吃子
            i = 0;
            for (i = y + 1; i < 8;)
            {
                if (_xy[x, i] == b)
                { ++i; continue; }
                else
                    break;
            }
            if (i > y + 1 && i < 8 && _xy[x, i] == a)
            {
                for (int j = y + 1; j <= i - 1; j++)
                {
                    num++;
                }
            }
            #endregion

            #region //向右上吃子
            int k = 0;
            for (i = x + 1, k = y - 1; i < 8 && k >= 0;)
            {
                if (_xy[i, k] == b)
                { ++i; --k; continue; }
                else
                    break;
            }
            if (i > x + 1 && k < y - 1 && i < 8 && k >= 0 && _xy[i, k] == a)
            {
                for (int j = x + 1, j1 = y - 1; j <= i - 1 && j1 > k; j++, j1--)
                {
                    num++;
                }
            }
            #endregion

            #region   //向左下吃子
            k = 0;
            for (i = y + 1, k = x - 1; i < 8 && k >= 0;)
            {
                if (_xy[k, i] == b)
                { ++i; --k; continue; }
                else
                    break;
            }
            if (i > y + 1 && k < x - 1 && i < 8 && k >= 0 && _xy[k, i] == a)
            {
                for (int j = y + 1, j1 = x - 1; j <= i - 1 && j1 > k; j++, j1--)
                {
                    num++;
                }
            }
            #endregion

            #region //向左上吃子
            k = 0;
            for (i = y - 1, k = x - 1; i >= 0 && k >= 0;)
            {
                if (_xy[k, i] == b)
                { --i; --k; continue; }
                else
                    break;
            }
            if (i < y - 1 && k < x - 1 && i >= 0 && k >= 0 && _xy[k, i] == a)
            {
                for (int j = y - 1, j1 = x - 1; j > i && j1 > k; j--, j1--)
                {
                    num++;
                }
            }
            #endregion

            #region //向右下吃子
            k = 0;
            for (i = y + 1, k = x + 1; i < 8 && k < 8;)
            {
                if (_xy[k, i] == b)
                { ++i; ++k; continue; }
                else
                    break;
            }
            if (i > y + 1 && k > x + 1 && i < 8 && k < 8 && _xy[k, i] == a)
            {
                for (int j = y + 1, j1 = x + 1; j < i && j1 < k; j++, j1++)
                {
                    num++;
                }
            }
            #endregion
            return num;
            #endregion
        }
        #endregion

        #region //人机对弈时，电脑的选择
        #region //中级
        /// <summary>
        /// 中级
        /// </summary>
        public void ComputerDownMid(out int  row,out int  col)
        {
            int a = 0;
            if (canDown(_isBlack) == false)
            {
                _isBlack = !_isBlack;
                row = -1;
                col = -1;
                return;

            }
            int x = 0, y = 0, x1 = 0, y1 = 0;
            int max = 0, max2 = 0;
            int[,] location = new int[8, 8];
            if (_isBlack == true)
            {
                a = 1;
            }
            else
                a = -1;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (_xy[i, j] == 0)
                    {
                        _xy[i, j] = a;
                        if (Eaten(i, j) == true)
                        {
                            location[i, j] = Eatnum(i, j);
                            _xy[i, j] = 0;
                            if ((i == 0 && j == 0) || (i == 0 && j == 7) || (i == 7 && j == 7) || (i == 7 && j == 0))
                            {
                                ComputerIn_QiZi(i, j);
                                row = i;
                                col = j;
                                return;
                            }

                        }
                        else
                        {
                            _xy[i, j] = 0;
                        }

                    }
                }
            }

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (location[i, j] > max || (i == 0 || j == 0 || i == 7 || j == 7))
                    {
                        if ((i == 0 || j == 0 || i == 7 || j == 7) && location[i, j] > max2)
                        {
                            max2 = location[i, j]; x1 = i; y1 = j;
                        }
                        else if (location[i, j] > max)
                        {
                            max = location[i, j]; x = i; y = j;
                        }
                    }
                }
            }
            if (max2 != 0)
            {
                ComputerIn_QiZi(x1, y1);
                row = x1;
                col = y1;
                return;
            }
            else
            {
                ComputerIn_QiZi(x, y);
                row = x;
                col = y;
                return;
            }
        }
        #endregion

        #region //低级
        /// <summary>
        /// //低级
        /// </summary>
        public void ComputerDownLow( out int row,out int col)
        {
            int a = 0;
            if (canDown(_isBlack) == false)
            {
                _isBlack = !_isBlack;
                row = -1;
                col = -1;
                return;

            }
            int x = 0, y = 0;
            int max = 0;
            int[,] location = new int[8, 8];
            if (_isBlack == true)
            {
                a = 1;
            }
            else
                a = -1;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (_xy[i, j] == 0)
                    {
                        _xy[i, j] = a;
                        if (Eaten(i, j) == true)
                        {
                            location[i, j] = Eatnum(i, j);
                            _xy[i, j] = 0;
                        }
                        else
                        {
                            _xy[i, j] = 0;
                        }

                    }
                }
            }

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {

                    if (location[i, j] > max)
                    {
                        max = location[i, j]; x = i; y = j;
                    }

                }
            }
            ComputerIn_QiZi(x, y);
            row = x;
            col = y;
            return;
        }


        #endregion

        #region //高级   
        /// <summary>
        /// ///高级  
        /// </summary>
        public void ComputerDownDiff( out int row,out int col )
        {
            int a;
            bool isDown = _isBlack;
            if (canDown(_isBlack) == false)
            {
                _isBlack = !_isBlack;
                row = -1;
                col = -1;
                return;
            }
            if (_isBlack == true)
                a = 1;
            else
                a = -1;
            int[,] temp = new int[8, 8];
            int x = -1, y = -1;
            int x1 = -1, y1 = -1;
            int x2 = -1, y2 = -1;
            int x3 = -1, y3 = -1;
            int max = 0, max2 = 0, max3 = 0;
            int[,] location = new int[8, 8];
            int mark = 10000;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    temp[i, j] = _xy[i, j];
                }
            }
            #region   //当前电脑的最好选择 相当于中级电脑的选择
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {

                    if (_xy[i, j] == 0)
                    {
                        _xy[i, j] = a;
                        if (Eaten(i, j) == true)
                        {
                            location[i, j] = Eatnum(i, j);

                            if (((i == 0 && j == 0) || (i == 0 && j == 7) || (i == 7 && j == 7) || (i == 7 && j == 0)) && location[i, j] > max)//
                            {
                                max = location[i, j];
                                x1 = i; y1 = j;

                            }

                            _xy[i, j] = 0;
                        }

                        else
                        {
                            _xy[i, j] = 0;
                        }

                    }
                }
            }
            int mm = 1000;             //用于下面
            int lm = 1000;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {

                    if (location[i, j] > 0)
                    {
                        if ((i == 0 || j == 0 || i == 7 || j == 7))
                        {
                            ComputerIn_QiZi(i, j);
                            int tmp = Mark();
                            if (tmp < mm)
                            {
                                max2 = 1;
                                mm = tmp;
                                x2 = i; y2 = j;
                            }
                            for (int m = 0; m < 8; m++)
                            {
                                for (int n = 0; n < 8; n++)
                                {
                                    _xy[m, n] = temp[m, n];
                                }
                            }


                        }
                        else
                        {
                            ComputerIn_QiZi(i, j);
                            int tmp = Mark();
                            if (tmp < lm)
                            {
                                max3 = 1;
                                lm = tmp;
                                x3 = i; y3 = j;
                            }
                            for (int m = 0; m < 8; m++)
                            {
                                for (int n = 0; n < 8; n++)
                                {
                                    _xy[m, n] = temp[m, n];
                                }
                            }
                        }
                    }
                }
            }

            #endregion


            #region  //电脑考虑自己走一步后，对方的最好的走法，然后选择所有可走棋中，对方最不好的走法
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {

                    if (_xy[i, j] == 0)
                    {
                        _xy[i, j] = a;
                        if (Eaten(i, j) == true)
                        {
                            location[i, j] = Eatnum(i, j);
                            _xy[i, j] = 0;
                            ComputerIn_QiZi(i, j);
                            int ma = Mark();
                            if (ma < mark)
                            {
                                mark = ma;
                                x = i; y = j;
                                // MessageBox.Show(mark.ToString() + "  " + x.ToString() + "  " + y.ToString());
                            }

                        }

                        for (int m = 0; m < 8; m++)
                        {
                            for (int n = 0; n < 8; n++)
                            {
                                _xy[m, n] = temp[m, n];
                            }
                        }

                    }
                }
            }

            #endregion


            #region  //综合以上两种选择  选择最好的
            //有角的占角，无角的防止对方占角
            //有边的占边，无边的防止对方占边
            //低级算法
            for (int m = 0; m < 8; m++)
            {
                for (int n = 0; n < 8; n++)
                {
                    _xy[m, n] = temp[m, n];
                }
            }

            if (max != 0)
            {
                ComputerIn_QiZi(x1, y1);
                row = x1;
                col = y1;
                return;
            }
            else if (max2 != 0)
            {
                ComputerIn_QiZi(x2, y2);
                int ma = Mark();

                for (int m = 0; m < 8; m++)
                {
                    for (int n = 0; n < 8; n++)
                    {
                        _xy[m, n] = temp[m, n];
                    }
                }
                if (ma >= 500)
                {
                    ComputerIn_QiZi(x, y);
                    row = x;
                    col = y;
                    return;
                }
                else
                {
                    ComputerIn_QiZi(x2, y2);
                    row = x2;
                    col = y2;
                    return;
                }
            }

            else if (max3 != 0)
            {

                ComputerIn_QiZi(x3, y3);
                int ma = Mark();

                for (int m = 0; m < 8; m++)
                {
                    for (int n = 0; n < 8; n++)
                    {
                        _xy[m, n] = temp[m, n];
                    }
                }
                if (ma >= 500 || ma >= 100)
                {
                    ComputerIn_QiZi(x, y);
                    row = x;
                    col = y;
                    return;
                }
                else
                {
                    ComputerIn_QiZi(x3, y3);
                    row = x3;
                    col = y3;
                    return;
                }
            }
            #endregion
            ComputerIn_QiZi(x, y);
            row = x;
            col = y;
            return;
        }
        #endregion


        #endregion

        #region  //人机对弈时，对于考虑下一步的评分
        /// <summary>
        /// 人机对弈时，对于考虑下一步的评分
        /// </summary>
        /// <returns></returns>
        public int Mark()
        {
            int a;
            if (canDown(_isBlack) == false)
            {
                _isBlack = !_isBlack;
                return 0;

            }
            if (_isBlack == true)
                a = 1;
            else
                a = -1;
            //int x = 0, y = 0;
            //int x1 = 0, y1 = 0;
            //int x2 = 0, y2 = 0;
            int max = 0, max1 = 0, max2 = 0, max3 = 0, max4 = 0;
            int[,] location = new int[8, 8];

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (_xy[i, j] == 0)
                    {
                        _xy[i, j] = a;
                        if (Eaten(i, j) == true)
                        {
                            location[i, j] = Eatnum(i, j);

                            if (((i == 0 && j == 0) || (i == 0 && j == 7) || (i == 7 && j == 7) || (i == 7 && j == 0)))//&&location[i,j]>max1
                            {
                                max1 = location[i, j];
                                max3 = max1 + 500;

                            }
                            _xy[i, j] = 0;
                        }
                        else
                        {
                            _xy[i, j] = 0;
                        }
                    }
                }
            }

            if (max3 != 0)
            { _isBlack = !_isBlack; return max3; }

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {

                    if ((location[i, j] > 0) || (i == 0 || j == 0 || i == 7 || j == 7))
                    {
                        if ((i == 0 || j == 0 || i == 7 || j == 7) && (location[i, j] > max))
                        {
                            max = location[i, j];
                            max2 = max + 100;

                        }
                        else if (location[i, j] > max4)
                        {
                            max4 = location[i, j];
                        }
                    }
                }
            }
            if (max2 != 0)
            {
                _isBlack = !_isBlack; return max2;
            }
            else
            {
                _isBlack = !_isBlack; return max4;
            }
        }
        #endregion

        #region //人机对弈时，机器的落子程序
        /// <summary>
        /// //人机对弈时，机器的落子程序
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void ComputerIn_QiZi(int x, int y)
        {
            if (_xy[x, y] == 0)
            {
                if (this._isBlack)
                {
                    _xy[x, y] = 1;//表示黑子
                    Eat(x, y); this.x = x; this.y = y;
                }
                else
                {
                    _xy[x, y] = -1;   //表示白子
                    Eat(x, y); this.x = x; this.y = y;
                }
                this._isBlack = !this._isBlack;  //机器下完后转到人下

            }
        }

        #endregion
        #region//说明
        private void btnInstruct_Click(object sender, EventArgs e)
        {
            Introduction inr = new Introduction();
            inr.Show();
        }

        /// <summary>
        /// 关闭窗口事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmAI_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!_over) //表示没有结束
            {
                if(MessageBoxEx.Show("游戏尚未结束，是否确定退出？","提示：",MessageBoxButtons.OKCancel)==DialogResult.OK)
                {
                    BizLogic.frmMain.Visible = true;
                }
                else
                {
                    e.Cancel=true;
                    return;
                }
            }
        }
        #endregion

        /// <summary>
        /// 鼠标按下时响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            //if (!_isAIOrPeople)
            //{
            //    //i代表行j代表列 y轴应为列 x轴应为行 
            //    int x = e.X / 40;
            //    int y = e.Y / 40;
            //    ////黑白棋逻辑
            //    if (!(x < 1 || x > 8 || y < 1 || y > 8))
            //    {
            //        if (grid[x - 1, y - 1] == DotColor.None) //无子区域
            //        {
            //            //int color = grid[x - 1, y - 1];
            //            service.SendToServer(string.Format("SetDot,{0},{1},{2},{3}", tableIndex, x - 1, y - 1, side));
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 绘制界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            int _blackTempCount = 0;           //黑子临时棋子数计数
            int _whiteTempCount = 0;         //白子临时棋子数计数
            Graphics g = e.Graphics;
            for (int i = 0; i <= _xy.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= _xy.GetUpperBound(1); j++)
                {
                    if (_xy[i, j] != 0)
                    {
                        if (_xy[i, j] == 1)
                        {
                            //i代表行j代表列 y轴应为列 x轴应为行 
                            g.DrawImage(blackBitmap, (i + 1) * 40, (j + 1) * 40);
                        }
                        else
                        {
                            g.DrawImage(whiteBitmap, (i + 1) * 40, (j + 1) * 40);
                        }
                    }

                    if (_xy[i, j] == 1)  //黑子
                    {
                        //Point point = new Point(x1, y1);
                        //g.DrawImage(imageList1.Images[0], point);  //画黑子
                        _blackTempCount++;
                        // MessageBox.Show(i.ToString() + "  " + j.ToString());
                    }
                    else if (_xy[i, j] == -1)  //白子
                    {
                        // MessageBox.Show(i.ToString() + "  " + j.ToString());
                        //Point point = new Point(x1, y1);
                        //g.DrawImage(imageList1.Images[1], point);  //画黑子
                        _whiteTempCount++;
                    }
                    else  //此处既不是白子又不是黑子
                    {
                        if (_isBlack == true)//当前是否黑方下子
                        {
                            _xy[i, j] = 1;
                        }
                        else
                        {
                            _xy[i, j] = -1;
                        }
                        if (Eaten(i, j) == true)
                        {
                            if (_isBlack == true)   //当前是否黑方下子
                            {
                                //Point point = new Point(x1, y1);
                                g.DrawImage(cBlackBitmap, (i + 1) * 40, (j + 1) * 40);  //红框代表玩家执黑子
                            }
                            else
                            {
                                //Point point = new Point(x1, y1);
                                g.DrawImage(cWhiteBitmap, (i + 1) * 40, (j + 1) * 40); //蓝框代表玩家执黑子
                            }
                        }
                        _xy[i, j] = 0;
                    }
                }
            }

            if (x != -1 && y != -1)
            {
                if (_xy[this.x, this.y] == 1)
                {
                    //x1 = 40 * this.x + 64;
                    //y1 = 40 * this.y + 64;
                    g.DrawImage(dBlackBitmap, (this.x + 1) * 40, (this.y + 1) * 40);
                }

                else if (_xy[this.x, this.y] == -1)
                {
                    g.DrawImage(dWhiteBitmap, (this.x + 1) * 40, (this.y + 1) * 40);
                }
            }

            _blackCount = _blackTempCount;
            _whiteCount = _whiteTempCount;
            labBlackCount.Text = _blackTempCount.ToString();
            labWhiteCount.Text = _whiteTempCount.ToString();

            if (_regret == 0)
            {
                btnBack.Enabled = false;
                return;
            }
        }
    }
}