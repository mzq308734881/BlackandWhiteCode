using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace Client
{

    public partial class FrmPeople : Office2007Form
    {
        #region 通信全局变量
        private bool _isHost;
        public string _player;  //当前用户
        private int tableIndex;   //桌子号索引，从第零个开始
        private int side;     //0表示黑棋，1表示白棋
        private int[,] grid = new int[8, 8];  //棋盘格子
        private string _whoDo;        //表示归谁下子
        private Service service;              //服务
        private Service serviceTalk;      //服务
        delegate void LabelDelegate(Label label, string str);
        delegate void ButtonDelegate(Button button, bool flag);
        delegate void ButtonItemDelegate(ButtonItem button, bool flag);
        delegate void PictureBoxDelegate(PictureBox pic, bool flag);
        LabelDelegate labelDelegate;
        ButtonDelegate buttonDelegate;
        ButtonItemDelegate buttonItemDelegate;
        PictureBoxDelegate pictureBoxDelegate;
        private Bitmap blackBitmap;
        private Bitmap whiteBitmap;
        private Bitmap cBlackBitmap;
        private Bitmap cWhiteBitmap;
        private Bitmap dBlackBitmap;
        private Bitmap dWhiteBitmap;
        private Bitmap gridBitmap;
        #endregion

        public bool _isAIOrPeople = false;

        #region    //定义或初始化 全局变量
        private int _num = 0;            //当前的步数，走一步就加一
        private int _flag = 1;            //等级的选择，默认等级为1
        private bool _begin = false;  //程序是否开始

        private bool _rbAI = true;    //是否人机对弈,默认人机对战
        private bool _rbLow = true;
        private bool _rbMiddle = false;
        private bool _rbDifficult = false;
        //private bool _rbPCBlack = false;
        //private bool _rbPeopleBlack = true;    //用来判断 电脑还是人执黑,默认人执黑

        private bool _over = false;//判断游戏是否结束
        private bool _personDown = false;   //用于判断人是否落子（人机对弈时）
        private bool _isBlack = true;  //当前是否是黑方下棋
        public int[,] _xy = new int[8, 8]; //棋面
        //定义结构体类型的数组，里面存放棋子位置的二维数组
        Qipan[] _qipan = new Qipan[64];  //定义64个棋盘结构，用来储存棋面，从而实现悔棋；
        //Bitmap是从Image类继承的一个图像类，它封装了Windows位图操作的常用功能
        //Image img = Bitmap;
        Bitmap bitmap = new Bitmap(462, 426);   //位图的长和宽
        Bitmap bitmap2 = new Bitmap(116, 138);

        private int _blackCount = 0, _whiteCount = 0;  //用来显示双方的棋子数
        private int x = -1, y = -1;              //当前落子的位置
        private int _regret = 3;   //悔棋的步数，如果人人对弈每个人都可以悔一步棋，人机对弈时，机器不能悔棋，直接退回两步棋

        /// <summary>
        ///结构体，用于存放棋子的位置，将每一步的位置都记录在二维数组_localtion[,]中
        /// </summary>
        public struct Qipan
        {
            public int[,] _localtion;
        }
        #endregion

        /// <summary>
        /// 该构造方法表示人人对弈
        /// </summary>
        /// <param name="TableIndex"></param>
        /// <param name="Side"></param>
        /// <param name="sw"></param>
        public FrmPeople(int TableIndex, int Side, StreamWriter sw)
        {
            InitializeComponent();
            service = new Service(listBoxMsg, sw);
            serviceTalk = new Service(listTalkMsg, sw);
            this.tableIndex = TableIndex;
            this.side = Side;
            labelDelegate = new LabelDelegate(SetLabel);
            buttonDelegate = new ButtonDelegate(SetButton);
            buttonItemDelegate = new ButtonItemDelegate(SetButtonItem);
            pictureBoxDelegate = new PictureBoxDelegate(SetPicBox);
            blackBitmap = new Bitmap("black.png");
            whiteBitmap = new Bitmap("white.png");

            cBlackBitmap = new Bitmap("CanputBlack.gif");
            cWhiteBitmap = new Bitmap("CanputWhite.gif");
            dBlackBitmap = new Bitmap("DownedBlack.png");
            dWhiteBitmap = new Bitmap("DownedWhite.png");

            gridBitmap = new Bitmap("DownedWhite.png");

            btnHost.Enabled = false;
            btnCancelHost.Enabled = false;
            labPlayer.Text = _player;
            _isHost = false;

            //try   //上线提醒
            //{
            //    service.SendToServer(string.Format("Online,{0},{1},{2},{3}", tableIndex, side, txtSendMsg.Text, labPlayer.Text));
            //    txtSendMsg.Text = "";
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("发送消息失败：" + ex.Message);
            //}
            //draw_QiPan();
            //labBlack.Visible = labWhite.Visible = false;
        }

        /// <summary>
        /// 人机模式
        /// </summary>
        public FrmPeople()
        {
            InitializeComponent();
            //draw_QiPan();
            //labBlack.Visible = labWhite.Visible = false;
            blackBitmap = new Bitmap("black.png");
            whiteBitmap = new Bitmap("white.png");

            cBlackBitmap = new Bitmap("CanputBlack.gif");
            cWhiteBitmap = new Bitmap("CanputWhite.gif");
            dBlackBitmap = new Bitmap("DownedBlack.png");
            dWhiteBitmap = new Bitmap("DownedWhite.png");

            gridBitmap = new Bitmap("DownedWhite.png");
        }
        #region 设置默认选项
        /// <summary>
        /// 保存（设置）选项信息
        /// </summary>
        /// <param name="_frmSelectHostHard"></param>
        private void SetInitSelectInfo(FrmSelectHostHard _frmSelectHostHard)
        {
            //selectStand.rbAI.Checked = _rbAI;
            //selectStand.rbPeople.Checked = _rbPeople;

            _frmSelectHostHard.rbLow.Checked = _rbLow;
            _frmSelectHostHard.rbMiddle.Checked = _rbMiddle;
            _frmSelectHostHard.rbDifficult.Checked = _rbDifficult;
        }
        /// <summary>
        /// 得到选项信息
        /// </summary>
        /// <param name="_frmSelectHostHard"></param>
        private void GetSelectedInfo(FrmSelectHostHard _frmSelectHostHard)
        {
            _rbAI = true;
            //_rbPeople = selectStand.rbPeople.Checked;

            _rbLow = _frmSelectHostHard.rbLow.Checked;
            _rbMiddle = _frmSelectHostHard.rbMiddle.Checked;
            _rbDifficult = _frmSelectHostHard.rbDifficult.Checked;

        }
        #endregion

        #region //选项窗口
        private void btnSelect_Click(object sender, EventArgs e)
        {
            FrmSelectHostHard frmSelectHostHard = new FrmSelectHostHard();
            SetInitSelectInfo(frmSelectHostHard);
            frmSelectHostHard.ShowDialog();
            GetSelectedInfo(frmSelectHostHard);
            frmSelectHostHard.TitleText += ("选择托管等级");
            //_rbAI = _selectStand.rbAI.Checked;  // 人机还是人人对弈的判断
            if (_rbAI == true)                  //如果人机对弈
            {
                if (frmSelectHostHard.rbLow.Checked == true)
                {
                    _flag = 1;
                }
                else if (frmSelectHostHard.rbMiddle.Checked == true)
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
            //if (frmSelectHostHard.rbPCBlack.Checked == true)    //先手的判断
            //{
            //    _rbPeopleBlack = false;   //电脑执黑
            //}
            //else
            //{
            //    _rbPeopleBlack = true;    //玩家执黑
            //}
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
            _xy[3, 3] = _xy[4, 4] = 1;   //设置黑子默认位置
            _xy[3, 4] = _xy[4, 3] = -1;  //设置白子默认位置
            this.Draw_QiZi();    //画棋子
            pictureBox2.Invalidate();


            Savefor();   //存储棋面，用于悔棋
            _num++; //初始化棋盘
            _begin = true;
            //labBlack.Visible = true;
            //btnSelect.Enabled = false;

            //#region//如果电脑执黑 电脑先走一步
            //if (_rbPeopleBlack == false)
            //{
            //    _num = _num - 1;      //为了悔棋，否则玩家还没等可以下棋时，就已经可以悔棋了。
            //    if (_flag == 1)
            //    {

            //        ComputerDownLow();
            //        _personDown = false;
            //        //this.Draw_QiZi();
            //        pictureBox2.Invalidate();
            //        if (canDown(_isBlack) == true)
            //        {
            //            Savefor(); _num++;//储存当前棋面
            //        }
            //        EndGame();
            //    }
            //    if (_flag == 2)
            //    {
            //        ComputerDownMid();
            //        _personDown = false;
            //        //this.Draw_QiZi();
            //        pictureBox2.Invalidate();
            //        if (canDown(_isBlack) == true)
            //        {
            //            Savefor(); _num++;//储存当前棋面
            //        }
            //        EndGame();
            //    }

            //    if (_flag == 3)
            //    {
            //        ComputerDownDiff();
            //        _personDown = false;
            //        //this.Draw_QiZi();
            //        pictureBox2.Invalidate();
            //        if (canDown(_isBlack) == true)
            //        {
            //            Savefor(); _num++;//储存当前棋面
            //        }
            //        EndGame();
            //    }
            //}
            //#endregion
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
            labAIGrade0.Text = "";
            labAIGrade1.Text = "";
            //_rbAI = true; //是否人机对弈
            _over = false;//判断游戏是否结束
            _personDown = true;//用于判断人是否落子（人机对弈时）
            _isBlack = true;
            _xy = new int[8, 8];
            _qipan = new Qipan[64];
            //draw_QiPan();
            //labBlack.Visible = labWhite.Visible = false;
            pictureBox2.Enabled = true;
            btnSelect.Enabled = true;
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
            if ((x >= 262 & x <= 582) & (y >= 74 & y < 394))
            {  //得到整数位置
                row = x = (x - 262) % 40 > 0 ? (int)((x - 262) / 40) : (int)((x - 262) / 40) - 1;
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

        private bool In_QiZi(int x, int y)
        {
            bool canEat = true;
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
                g.DrawLine(LinePen, x_, 74, x_, 384);  //横轴相同画9条竖线
            }
            for (int y_ = 74; y_ <= 394; y_ += 40)
            {
                g.DrawLine(LinePen, 74, y_, 394, y_);  //纵轴相同画9条横线
            }
            this.pictureBox2.Image = bitmap;   //设置将棋盘添加到pictureBox面板
            //if (_num <= 0)   //没开始走不允许悔棋
            //{
            //    btnBack.Enabled = false;
            //}
            //else
            //    btnBack.Enabled = true;
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
                        //g.DrawImage(imageList1.Images[0], point);  //画黑子
                        _blackTempCount++;
                        // MessageBox.Show(i.ToString() + "  " + j.ToString());
                    }
                    else if (_xy[i, j] == -1)  //白子
                    {
                        // MessageBox.Show(i.ToString() + "  " + j.ToString());
                        Point point = new Point(x1, y1);
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

            _blackCount = _blackTempCount;
            _whiteCount = _whiteTempCount;
            SetLabel(labAIGrade0, _blackTempCount.ToString());
            SetLabel(labAIGrade1, _whiteTempCount.ToString());
            //labAIGrade0.Text = _blackTempCount.ToString();
            //labAIGrade1.Text = _whiteTempCount.ToString();
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
                        _personDown = !_personDown;
                    }
                    else
                    {
                        MessageBox.Show("白方无地方可落子，黑方可继续落子");
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
                }
                else if (_blackCount < _whiteCount)
                {
                    MessageBox.Show("游戏结束,白方获胜");
                }
                else
                    MessageBox.Show(_blackCount + "  " + _whiteCount + "游戏结束,平局");
                //pictureBox2.Enabled = false;
                SetPicBox(pictureBox2, false);
                //labWhite.Visible = false;
                //labBlack.Visible = false;
                btnSelect.Enabled = false;
                _over = true;
                return;
            }
            #endregion

            #region 显示该谁下子
            //if (_isBlack)
            //{
            //    labBlack.Visible = true;
            //    labWhite.Visible = false;

            //}
            //else
            //{
            //    labBlack.Visible = false;
            //    labWhite.Visible = true;
            //}

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
                        _personDown = !_personDown;
                    }
                    else
                    {
                        MessageBox.Show("白方无地方可落子，黑方可继续落子");
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
                }
                else if (_blackCount < _whiteCount)
                {
                    MessageBox.Show("游戏结束,白方获胜");
                }
                else
                    MessageBox.Show(_blackCount + "  " + _whiteCount + "游戏结束,平局");
                //pictureBox2.Enabled = false;   //跨线程操作，有问题
                SetPicBox(pictureBox2, false);
                //labWhite.Visible = false;
                //labBlack.Visible = false;
                btnSelect.Enabled = false;
                _over = true;
                return;
            }
            #endregion
        }
        #endregion

        #region //刷新棋盘及判断是否人机对弈
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (_isHost)
            {
                int row;
                int col;
                if (_begin == false)
                {
                    return;
                }
                Point p = PointToClient(MousePosition);
                bool luozi = this.In_QiZi(p.X, p.Y, out row, out col);//判断点击是否有效
                if (_isHost && luozi)
                {
                    ////i代表行j代表列 y轴应为列 x轴应为行 
                    //int x = e.X / 40;
                    //int y = e.Y / 40;
                    ////黑白棋逻辑
                    if (!(row < 0 || row > 8 || col < 0 || col > 8))
                    {
                        if (grid[row, col] == -1) //无子区域
                        {
                            //int color = grid[x - 1, y - 1];
                            service.SendToServer(string.Format("SetHostDot,{0},{1},{2},{3}", tableIndex, row, col, side));
                        }
                    }
                }
                pictureBox2.Invalidate();
                if (luozi == true)
                { Savefor(); _num++; }//储存当前棋面

                EndGame();
                while (_personDown == true && _over == false && _rbAI == true && luozi == true)
                {
                    //if (_flag == 1)
                    //{
                    //    ComputerDownLow();
                    //    _personDown = false;
                    //    //this.Draw_QiZi();
                    //    pictureBox2.Invalidate();
                    //    if (canDown(_isBlack) == true)
                    //    {
                    //        Savefor(); _num++;//储存当前棋面
                    //    }
                    //    EndGame();
                    //}

                    ComputerDownMid();
                    _personDown = false;
                    //this.Draw_QiZi();
                    pictureBox2.Invalidate();
                    if (canDown(_isBlack) == true)
                    {
                        Savefor(); _num++;//储存当前棋面
                    }
                    EndGame();


                    //if (_flag == 3)
                    //{
                    //    ComputerDownDiff();
                    //    _personDown = false;
                    //    //this.Draw_QiZi();
                    //    pictureBox2.Invalidate();
                    //    if (canDown(_isBlack) == true)
                    //    {
                    //        Savefor(); _num++;     //储存当前棋面
                    //    }
                    //    EndGame();
                    //}
                }
                luozi = false;
                //if(_isHost)   //发送托管的成绩
                //{
                //    service.SendToServer(string.Format("HostGrade,{0},{1},{2},{3}", tableIndex, side, labAIGrade0.Text, labAIGrade1.Text));
                //}

            }
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
        public void ComputerDownMid()
        {
            int a = 0;
            if (canDown(_isBlack) == false)
            {
                _isBlack = !_isBlack;
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
                                ComputerIn_QiZi(i, j); return;
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
                ComputerIn_QiZi(x1, y1); return;
            }
            else
            {
                ComputerIn_QiZi(x, y); return;
            }
        }
        #endregion

        #region //低级
        /// <summary>
        /// //低级
        /// </summary>
        public void ComputerDownLow()
        {
            int a = 0;
            if (canDown(_isBlack) == false)
            {
                _isBlack = !_isBlack;
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
            ComputerIn_QiZi(x, y); return;
        }


        #endregion

        #region //高级   
        /// <summary>
        /// ///高级  
        /// </summary>
        public void ComputerDownDiff()
        {
            int a;
            bool isDown = _isBlack;
            if (canDown(_isBlack) == false)
            {
                _isBlack = !_isBlack;
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
                ComputerIn_QiZi(x1, y1); return;
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
                    ComputerIn_QiZi(x, y); return;
                }
                else
                {
                    ComputerIn_QiZi(x2, y2); return;
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
                    ComputerIn_QiZi(x, y); return;
                }
                else
                {
                    ComputerIn_QiZi(x3, y3); return;
                }
            }
            #endregion
            ComputerIn_QiZi(x, y); return;
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
        #endregion

        #region// 通信对弈模式
        /// <summary>
        /// Lable跨线程操作
        /// </summary>
        /// <param name="label"></param>
        /// <param name="str"></param>
        public void SetLabel(Label label, string str)
        {
            if (label.InvokeRequired)
            {
                this.Invoke(labelDelegate, label, str);
            }
            else
            {
                label.Text = str;
            }
        }

        /// <summary>
        /// Button跨线程操作
        /// </summary>
        /// <param name="button"></param>
        /// <param name="flag"></param>
        public void SetButton(Button button, bool flag)
        {
            if (button.InvokeRequired)
            {
                this.Invoke(buttonDelegate, button, flag);
            }
            else
            {
                button.Enabled = flag;
            }
        }

        /// <summary>
        /// ButtonItem跨线程操作
        /// </summary>
        /// <param name="button"></param>
        /// <param name="flag"></param>
        public void SetButtonItem(ButtonItem buttonItem, bool flag)
        {
            if (buttonItem.InvokeRequired)
            {
                this.Invoke(buttonItemDelegate, buttonItem, flag);
            }
            else
            {
                buttonItem.Enabled = flag;
            }
        }

        /// <summary>
        /// PictureBox 委托跨线程操作
        /// </summary>
        /// <param name="picBox"></param>
        /// <param name="flag"></param>
        public void SetPicBox(PictureBox picBox, bool flag)
        {
            if (picBox.InvokeRequired)
            {
                this.Invoke(pictureBoxDelegate, picBox, flag);
            }
            else
            {
                picBox.Enabled = flag;
            }
        }

        public void SetDot(int i, int j, int dotColor)
        {
            service.SetListBox(string.Format("{0},{1},{2}", i, j, dotColor));
            grid[i, j] = dotColor;
            pictureBox2.Invalidate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newString"></param>
        public void SetDotCollection(string newString)
        {
            service.SetListBox("落子："+newString);  //显示落子情况
            string[] sunit = newString.Split(',');
            //后三个值为双方棋子个数及轮到次序
            for (int i = 0; i < sunit.Length - 3; i = i + 3)
            {
                grid[int.Parse(sunit[i]), int.Parse(sunit[i + 1])] = int.Parse(sunit[i + 2]);
            }
            this.SetGradeText(sunit[sunit.Length - 3], sunit[sunit.Length - 2]);      //显示分数
            _whoDo = sunit[sunit.Length - 1];
            this.SetLabel(labelTurn, string.Format("该{0}下子了", sunit[sunit.Length - 1] == "0" ? "黑方" : "白方"));
            pictureBox2.Invalidate();
        }

        /// <summary>
        /// 重新启动
        /// </summary>
        /// <param name="str"></param>
        public void Restart(string str)
        {
            MessageBox.Show(str, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            ResetGrid();
            SetButtonItem(btnStart, true);
        }

        /// <summary>
        /// 初始化二维数组
        /// </summary>
        private void ResetGrid()
        {
            for (int i = 0; i <= grid.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= grid.GetUpperBound(1); j++)
                {
                    grid[i, j] = DotColor.None;
                }
            }
            pictureBox2.Invalidate();
        }
        public void UnsetDot(int x, int y)
        {
            grid[x / 40 - 1, y / 40 - 1] = DotColor.None;
            pictureBox2.Invalidate();
        }

        public void SetGradeText(string str0, string str1)
        {
            if (side == DotColor.Black)
            {
                SetLabel(labelGrade0, str0);
                SetLabel(labelGrade1, str1);
            }
            else
            {
                SetLabel(labelGrade1, str0);
                SetLabel(labelGrade0, str1);
            }
        }

        /// <summary>
        /// 列表信息
        /// </summary>
        /// <param name="str"></param>
        public void ShowMessage(string str)
        {
            service.SetListBox(str);
        }

        /// <summary>
        /// Talk列表信息
        /// </summary>
        /// <param name="str"></param>
        public void ShowTalkMessage(string str)
        {
            serviceTalk.SetListBox(str);
        }

        /// <summary>
        /// 显示托管成绩
        /// </summary>
        /// <param name="str0"></param>
        /// <param name="str1"></param>
        public void ShowHostGrade(string str0, string str1)
        {
            //if(_isHost)  
            //{
            SetLabel(labelGrade0, str1);   //当托管的时候发送托管成绩给对方
            SetLabel(labelGrade1, str0);
            //}
        }

        /// <summary>
        /// 得到托管后的界面
        /// </summary>
        /// <param name="xy"></param>
        public void ShowHostDotGrid(int x, int y, int side)
        {
            //_xy = xy;
            //if (side == 0)
            //    _xy[x, y] = 1;
            //else if (side == 1)
            //    _xy[x, y] = -1;
            //else
            //    _xy[x, y] = 0;

            bool luozi = this.In_QiZi(x, y);//判断点击是否有效
            pictureBox2.Invalidate();
            if (luozi == true)
            { Savefor(); _num++; }//储存当前棋面

            EndGame();
            //_isBlack = isBlack;            //判断该先白棋还是改下黑棋
            //ComputerDownMid();   //设置托管人机等级为一般 
            //if (_flag == 1)
            //{
            //    ComputerDownLow();
            //    _personDown = false;
            //    //this.Draw_QiZi();
            //    pictureBox2.Invalidate();
            //    if (canDown(_isBlack) == true)
            //    {
            //        Savefor(); _num++;//储存当前棋面
            //    }
            //    EndGame();
            //}

            if (side == 0)  //说明传过来的是黑子，则机器为白子
                _isBlack = false;
            else if (side == 1)
                _isBlack = true;  //说明传过来的是白子，则机器为黑子

            ComputerDownMid();
            _personDown = false;
            //this.Draw_QiZi();
            pictureBox2.Invalidate();
            if (canDown(_isBlack) == true)
            {
                Savefor(); _num++;//储存当前棋面
            }
            EndGame();

            //if (_flag == 3)
            //{
            //    ComputerDownDiff();
            //    _personDown = false;
            //    //this.Draw_QiZi();
            //    pictureBox2.Invalidate();
            //    if (canDown(_isBlack) == true)
            //    {
            //        Savefor(); _num++;     //储存当前棋面
            //    }
            //    EndGame();
            //}         
            luozi = false;
        }

        /// <summary>
        /// 取消托管,重新回到人人对弈模式
        /// </summary>
        public void CancelHost(int otherside, string otherPlayer)
        {
            string turn;
            //this.side = otherside;
            _isAIOrPeople = false;
            _begin = true;
            _personDown = true;
            _over = false;
            _rbAI = false;
            _isHost = false;   //取消托管
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (_xy[i, j] == 1)
                        grid[i, j] = 0;
                    else if (_xy[i, j] == 0)
                        grid[i, j] = -1;
                    else
                        grid[i, j] = 1;
                }
            }
            if (otherside == 0)
            {
                otherPlayer = "黑子：" + otherPlayer;
                turn = "白子";
            }
            else
            {
                otherPlayer = "白子：" + otherPlayer;
                turn = "黑子";
            }
            service.SetListBox(otherPlayer + "【取消托管】");
            SetLabel(labHost, "");     //表示取消了托管,将提示消息清空
            SetLabel(labelTurn,string.Format( "{1}托管取消,该{0}下子",turn, otherPlayer));
            pictureBox2.Invalidate();
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        public void FreshPic()
        {
            pictureBox2.Invalidate();
        }
        /// <summary>
        /// 托管信息
        /// </summary>
        /// <param name="str"></param>
        public void ShowHost(string str, bool isBlack)
        {
            //labHost.Text = str;
            _isHost = true;
            SetLabel(labHost, str);   //显示托管信息
            _xy = new int[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (grid[i, j] == 0)  //表示黑棋子
                        _xy[i, j] = 1;
                    else if (grid[i, j] == 1)  //表示白棋子
                        _xy[i, j] = -1;
                    else if (grid[i, j] == -1)  //表示黑棋子
                        _xy[i, j] = 0;
                }
            }
            _isAIOrPeople = true;
            _begin = true;
            _personDown = true;
            _over = false;
            _rbAI = true;
            if (_whoDo == "0" && isBlack)   //该黑子下棋了且是黑子托管,则机器替黑子先走一步
            {
                Savefor(); _num++; //储存当前棋面
                EndGame();
                _isBlack = isBlack;            //判断该先白棋还是改下黑棋
                ComputerDownMid();   //设置托管人机等级为一般
                _personDown = false;
                pictureBox2.Invalidate();
                if (canDown(_isBlack) == true)
                {
                    Savefor(); _num++;//储存当前棋面
                }
                EndGame();
                pictureBox2.Invalidate();  //界面重绘
                SetLabel(labelTurn, "黑子托管该白方落子！");
                //labelTurn.Text = "黑子托管该白方落子！";
            }
            else if (_whoDo == "1" && isBlack)  //该白子下棋了且是黑子托管，则机器为黑子，等白子走一步后再走
            {
                _isBlack = !isBlack;
                SetLabel(labelTurn, "黑子托管该白方落子！");
                //labelTurn.Text = "黑子托管该白方落子！";
            }
            else if (_whoDo == "1" && !isBlack)  //该白子下棋了且是白子托管，则机器为白子，则机器先走一步
            {
                Savefor(); _num++; //储存当前棋面
                EndGame();
                _isBlack = isBlack;       //判断该先白棋还是改下黑棋
                ComputerDownMid();
                _personDown = false;
                pictureBox2.Invalidate();
                if (canDown(_isBlack) == true)
                {
                    Savefor(); _num++;//储存当前棋面
                }
                EndGame();
                pictureBox2.Invalidate();  //界面重绘
                SetLabel(labelTurn, "白子托管该黑方落子！");
                //labelTurn.Text = "白子托管该黑方落子！";
            }
            else   //该黑子下棋了且是白子托管，则机器为白子，等黑子走一步后再走
            {
                _isBlack = !isBlack;   //判断该先白棋还是改下黑棋
                SetLabel(labelTurn, "白子托管该黑方落子！");
                //labelTurn.Text = "白子托管该黑方落子！";
            }
        }

        /// <summary>
        /// 得到对手和自己的信息
        /// </summary>
        /// <param name="sideString"></param>
        /// <param name="labelSideString"></param>
        /// <param name="listBoxString"></param>
        public void SetTableSideText(string sideString, string labelSideString, string listBoxString)
        {
            string s = "白方";
            if (sideString == "0")
            {
                s = "黑方";
            }
            if (sideString == side.ToString())
            {
                SetLabel(labelSideOwn0, "我是" + s + labelSideString);
            }
            else
            {
                SetLabel(labelSideOther1, s + labelSideString);
            }
            service.SetListBox(listBoxString);
        }
        #endregion

        #region// 事件
        private void btnStart_Click(object sender, EventArgs e)
        {
            service.SendToServer(string.Format("Start,{0},{1}", tableIndex, side));
            this.btnStart.Enabled = false;
            //btnSelect.Enabled = false;
            btnHost.Enabled = true;
        }
        /// <summary>
        /// 退出事件，当退出的时候没有点托管，默认对方胜利
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormPlaying_FormClosing(object sender, FormClosingEventArgs e)
        {
            service.SendToServer(string.Format("GetUp,{0},{1}", tableIndex, side));
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            for (int i = 0; i <= grid.GetUpperBound(0); i++)
            {

                for (int j = 0; j <= grid.GetUpperBound(1); j++)
                {
                    grid[i, j] = DotColor.None;
                }
            }
        }

        /// <summary>
        /// 鼠标按下时响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            btnHost.Enabled = true;
            if (!_isAIOrPeople)
            {
                //i代表行j代表列 y轴应为列 x轴应为行 
                int x = e.X / 40;
                int y = e.Y / 40;
                ////黑白棋逻辑
                if (!(x < 1 || x > 8 || y < 1 || y > 8))
                {
                    if (grid[x - 1, y - 1] == DotColor.None) //无子区域
                    {
                        //int color = grid[x - 1, y - 1];
                        service.SendToServer(string.Format("SetDot,{0},{1},{2},{3}", tableIndex, x - 1, y - 1, side));
                    }
                }
            }
            //if(_isHost)
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
            //            service.SendToServer(string.Format("SetHostDot,{0},{1},{2},{3}", tableIndex, x - 1, y - 1, side));
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 绘制界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            if (!_isAIOrPeople)   //表示人人对弈
            {
                Graphics g = e.Graphics;
                for (int i = 0; i <= grid.GetUpperBound(0); i++)
                {
                    for (int j = 0; j <= grid.GetUpperBound(1); j++)
                    {
                        if (grid[i, j] != DotColor.None)
                        {
                            if (grid[i, j] == DotColor.Black)
                            {
                                //i代表行j代表列 y轴应为列 x轴应为行 
                                g.DrawImage(blackBitmap, (i + 1) * 40, (j + 1) * 40);
                            }
                            else
                            {
                                g.DrawImage(whiteBitmap, (i + 1) * 40, (j + 1) * 40);
                            }
                        }
                    }
                }
            }
            else  //表示托管
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
                labAIGrade0.Text = _blackTempCount.ToString();
                labAIGrade1.Text = _whiteTempCount.ToString();

                if (_isHost)   //发送托管的成绩，和界面效果
                {
                    if (side == 0)
                    {
                        labelGrade0.Text = labAIGrade0.Text;
                        labelGrade1.Text = labAIGrade1.Text;
                    }
                    else
                    {
                        labelGrade0.Text = labAIGrade1.Text;
                        labelGrade1.Text = labAIGrade0.Text;
                    }
                    service.SendToServer(string.Format("HostGrade,{0},{1},{2},{3}", tableIndex, side, labAIGrade0.Text, labAIGrade1.Text));
                    //service.SendToServer(string.Format("HostDotGrid,{0},{1},{2}", tableIndex, side,_xy));
                    //for(int i=0;i<8;i++)
                    //{
                    //    for(int j=0;j<8;j++)
                    //    {
                    //        if(_xy[i,j]==1)
                    //        service.SendToServer(string.Format("HostDotGrid,{0},{1},{2},{3},{4}", tableIndex, x , y,0 , side));  //发送黑子的位置
                    //        else if(_xy[i, j] == 0)
                    //            service.SendToServer(string.Format("HostDotGrid,{0},{1},{2},{3},{4}", tableIndex, x, y, -1, side));  //发送黑子的位置
                    //        else
                    //            service.SendToServer(string.Format("HostDotGrid,{0},{1},{2},{3},{4}", tableIndex, x, y, 1, side));  //发送黑子的位置
                    //    }
                    //}
                    //service.SendToServer(string.Format("FreshPic,{0},{1}", tableIndex, side));
                }
            }
        }

        /// <summary>
        /// 托管
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHost_Click(object sender, EventArgs e)
        {
            if (labHost.Text.Trim() == "")  //说明对方没有托管，或者取消了托管，则允许托管
            {
                this.pictureBox2.Enabled = false;   //黑方或者白方点了托管后
                  //发送托管信息
                service.SendToServer(string.Format("Host,{0},{1},{2}", tableIndex, side, txtSendMsg.Text));
                //_isHost = true;
                _xy = new int[8, 8];
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (grid[i, j] == 0)  //表示黑棋子
                            _xy[i, j] = 1;
                        else if (grid[i, j] == 1)  //表示白棋子
                            _xy[i, j] = -1;
                        else if (grid[i, j] == -1)  //表示黑棋子
                            _xy[i, j] = 0;
                    }
                }
                _isAIOrPeople = true;
                _begin = true;
                _personDown = true;
                _over = false;
                _rbAI = true;

                if (side == 0 && _whoDo == "0")   //黑子点的托管，该黑子下棋了,则机器替黑子先走一步
                {
                    _isBlack = true;             //判断该先白棋还是改下黑棋
                    Savefor(); _num++;      //储存当前棋面
                    EndGame0();

                    ComputerDownMid();       //设置托管人机等级为一般
                    _personDown = false;
                    pictureBox2.Invalidate();
                    if (canDown(_isBlack) == true)
                    {
                        Savefor(); _num++;//储存当前棋面
                    }
                    EndGame0();
                    pictureBox2.Invalidate();  //界面重绘
                }
                else if (side == 1 && _whoDo == "0")  //白棋托管，该黑棋走，则不动
                {
                    _isBlack = true;
                }
                else if (side == 1 && _whoDo == "1")  //白棋托管，该白棋走，则机器为白子，则机器先走一步
                {
                    _isBlack = false;       //判断该先白棋还是改下黑棋
                    Savefor(); _num++; //储存当前棋面
                    EndGame0();
                    ComputerDownMid();
                    _personDown = false;
                    pictureBox2.Invalidate();
                    if (canDown(_isBlack) == true)
                    {
                        Savefor(); _num++;//储存当前棋面
                    }
                    EndGame0();
                    pictureBox2.Invalidate();  //界面重绘
                }
                else   //黑棋托管该白棋走
                {
                    _isBlack = !_isBlack;   //判断该先白棋还是改下黑棋
                }
                btnHost.Enabled = false;   //自己点了托管后
                btnSelect.Enabled = false;
                btnCancelHost.Enabled = true;  //点了托管后允许取消托管
            }
            else
            {
                btnCancelHost.Enabled = false;
                MessageBox.Show("对方已经托管电脑操作，不允许在我方托管请求！");
                return;
            }
        }
        /// <summary>
        /// 发送聊天信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butSendTalk_Click(object sender, EventArgs e)
        {
            try
            {
                string sendMsg = txtSendMsg.Text+" ";
                service.SendToServer(string.Format("Send,{0},{1},{2},{3}", tableIndex, side, sendMsg, labPlayer.Text));
                txtSendMsg.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("发送消息失败：" + ex.Message);
            }
        }

        /// <summary>
        /// enter发送事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmPeople_Enter(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Enter响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSendMsg_Enter(object sender, EventArgs e)
        {
            try
            {
                string sendMsg = txtSendMsg.Text + " ";
                service.SendToServer(string.Format("Send,{0},{1},{2},{3}", tableIndex, side, sendMsg, labPlayer.Text));
                txtSendMsg.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("发送消息失败：" + ex.Message);
            }
        }



        private void txtSendMsg_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                try
                {
                    string sendMsg = txtSendMsg.Text + " ";
                    service.SendToServer(string.Format("Send,{0},{1},{2},{3}", tableIndex, side, sendMsg, labPlayer.Text));
                    txtSendMsg.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("发送消息失败：" + ex.Message);
                }
            }
        }

        /// <summary>
        /// 清除聊天信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            listTalkMsg.Items.Clear();
        }
        /// <summary>
        /// 取消托管
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelHost_Click(object sender, EventArgs e)
        {
            btnCancelHost.Enabled = false;
            btnSelect.Enabled = true;
            btnHost.Enabled = true;
            pictureBox2.Enabled = true;
            //if (_isHost)
            //{
                _isAIOrPeople = false;
                _begin = true;
                _personDown = true;
                _over = false;
                _rbAI = false;
                _isHost = false;   //取消托管
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (_xy[i, j] == 1)
                            grid[i, j] = 0;
                        else if (_xy[i, j] == 0)
                            grid[i, j] = -1;
                        else
                            grid[i, j] = 1;
                        //取消托管的时候向更新服务端的Grid 二维矩阵
                        service.SendToServer(string.Format("FreshGrid,{0},{1},{2},{3},{4}", tableIndex, side, i, j, grid[i, j]));
                    }
                }
                pictureBox2.Invalidate();
                //发送消息给对方，让对方取消托管
                service.SendToServer(string.Format("CancelHost,{0},{1}", tableIndex, side));
            //}
        }
        #endregion
    }

    class DotColor
    {
        public const int None = -1;
        public const int Black = 0;
        public const int White = 1;
    }
}
