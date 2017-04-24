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
        #region    //������ʼ�� ȫ�ֱ���
        private int _num = 0;            //��ǰ�Ĳ�������һ���ͼ�һ
        private int _flag = 1;            //�ȼ���ѡ��
        private bool _begin = false;  //�����Ƿ�ʼ

        private bool _rbAI = true;    //�Ƿ��˻�����,Ĭ���˻���ս
        private bool _rbLow = true;
        private bool _rbMiddle = false;
        private bool _rbDifficult = false;
        private bool _rbPCBlack = false;
        private bool _rbPeopleBlack = true;    //�����ж� ���Ի�����ִ��,Ĭ����ִ��

        private bool _over = false;     //�ж���Ϸ�Ƿ����
        private bool _personDown = false;   //�����ж����Ƿ����ӣ��˻�����ʱ��
        private bool _isBlack = true;       //��ǰ�Ƿ��Ǻڷ�����
        public int[,] _xy = new int[8, 8]; //����
        //����ṹ�����͵����飬����������λ�õĶ�ά����
        Qipan[] _qipan = new Qipan[64];  //����64�����̽ṹ�������������棬�Ӷ�ʵ�ֻ��壻
        //Bitmap�Ǵ�Image��̳е�һ��ͼ���࣬����װ��Windowsλͼ�����ĳ��ù���
        //Image img = Bitmap;
        Bitmap bitmap = new Bitmap(462, 426);   //λͼ�ĳ��Ϳ�
        //Bitmap bitmap = new Bitmap("grid.gif");   //λͼ�ĳ��Ϳ�
        Bitmap bitmap2 = new Bitmap(116, 138);

        private int _blackCount = 0, _whiteCount = 0;  //������ʾ˫����������
        private int x = -1, y = -1;              //��ǰ���ӵ�λ��
        private int _regret = 3;   //����Ĳ�����������˶���ÿ���˶����Ի�һ���壬�˻�����ʱ���������ܻ��壬ֱ���˻�������
        private Bitmap blackBitmap;
        private Bitmap whiteBitmap;
        private Bitmap cBlackBitmap;
        private Bitmap cWhiteBitmap;
        private Bitmap dBlackBitmap;
        private Bitmap dWhiteBitmap;

        /// <summary>
        ///    //�ṹ�壬���ڴ�����ӵ�λ�ã���ÿһ����λ�ö���¼�ڶ�ά����_localtion[,]��
        /// </summary>
        public struct Qipan
        {
            public int[,] _localtion;
        }
        #endregion

        #region //��������
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

            listBoxAIInfo.Items.Add("Ĭ�����ִ���ӣ�AI�ȼ�Ϊ�ͼ���");
        }
        // 
        #endregion
        #region ����Ĭ��ѡ��
        /// <summary>
        /// ���棨���ã�ѡ����Ϣ
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
        /// �õ�ѡ����Ϣ
        /// </summary>
        /// <param name="selectStand"></param>
        private void GetSelectedInfo(SelectStand selectStand)
        {
            string str = "ѡ��";
            _rbAI = true;
            //_rbPeople = selectStand.rbPeople.Checked;

            if (selectStand.rbPCBlack.Checked)
                str +="����ִ�ڣ���������һ����";
            else
                str += "���ִ�ڣ�";
            if (selectStand.rbLow.Checked)
                str += "AI�ȼ�Ϊ�ͼ���";
            else if (selectStand.rbMiddle.Checked)
                str += "AI�ȼ�Ϊ�м���";
            else
                str += "AI�ȼ�Ϊ�߼���";

            _rbLow = selectStand.rbLow.Checked;
            _rbMiddle = selectStand.rbMiddle.Checked;
            _rbDifficult = selectStand.rbDifficult.Checked;

            _rbPCBlack = selectStand.rbPCBlack.Checked;
            _rbPeopleBlack = selectStand.rbPeopleBlack.Checked;
            listBoxAIInfo.Items.Clear();
            listBoxAIInfo.Items.Add(str);
        }
        #endregion

        #region //ѡ���
        private void btnSelect_Click(object sender, EventArgs e)
        {
            SelectStand _selectStand = new SelectStand();
            //SetInitSelectInfo(_selectStand);  //ѡ���ʱ��Ϊ��ʷ��¼
            _selectStand.ShowDialog();
            if(_selectStand._bool)
            {
                GetSelectedInfo(_selectStand);
            }
            //_rbAI = _selectStand.rbAI.Checked;  // �˻��������˶��ĵ��ж�
            if (_rbAI == true)                  //����˻�����
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

                _regret = 3;                    //�˻�����ʱ��������壬������
            }
            else     //���˶���ʱ
            {
                _regret = 2;                   //���˶���ʱ��������壬��һ��
            }
            if (_selectStand.rbPCBlack.Checked == true)  //���ֵ��ж�
            {
                _rbPeopleBlack = false;   //����ִ��
            }
            else
            {
                _rbPeopleBlack = true;    //���ִ��
            }

        }
        #endregion

        #region  //��Ϸ��ʼ
        /// <summary>
        /// ��ʼ��Ϸ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butGo_Click(object sender, EventArgs e)
        {
            listBoxAIInfo.Items.Add("��ʼ����Ϸ��");
            _xy[3, 3] = _xy[4, 4] = 1;   //���ú���Ĭ��λ��
            _xy[3, 4] = _xy[4, 3] = -1;  //���ð���Ĭ��λ��
            this.Draw_QiZi();    //������
            pictureBox2.Invalidate();

            Savefor();   //�洢���棬���ڻ���
            _num++; //��ʼ������
            _begin = true;
            labBlack.Visible = true;
            btnSelect.Enabled = btnGo.Enabled = false;

            #region//�������ִ�� ��������һ��
            if (_rbPeopleBlack == false)
            {
                _num = _num - 1;      //Ϊ�˻��壬������һ�û�ȿ�������ʱ�����Ѿ����Ի����ˡ�
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
                        listBoxAIInfo.Items.Add(string.Format("��������λ�ã�({0}��{1})", row + 1, col + 1));
                        Savefor(); _num++;//���浱ǰ����
                    }
                    else
                    {
                        listBoxAIInfo.Items.Add(string.Format("�������ӿ��£�"));
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
                        listBoxAIInfo.Items.Add(string.Format("��������λ�ã�({0}��{1})", row + 1, col + 1));
                        Savefor(); _num++;//���浱ǰ����
                    }
                    else
                    {
                        listBoxAIInfo.Items.Add(string.Format("�������ӿ��£�"));
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
                        listBoxAIInfo.Items.Add(string.Format("��������λ�ã�({0}��{1})", row + 1, col + 1));
                        Savefor(); _num++;//���浱ǰ����
                    }
                    else
                    {
                        listBoxAIInfo.Items.Add(string.Format("�������ӿ��£�"));
                    }
                    EndGame();
                }
            }
            #endregion
        }
        #endregion

        #region //���¿�ʼ
        private void btnNewGame_Click(object sender, EventArgs e)
        {
            _num = 0;     //���¿�ʼ��¼����
            //_flag = 1;     //�жϵȼ�,Ĭ�ϵȼ�Ϊ�ͼ�,���¿�ʼʱ����Ҫ�޸��Ѷȵȼ�
            _regret = 3;   //��ʾ����Ĳ���
            //_rbPeopleBlack = true;
            _begin = false;
            labBlackCount.Text = "";
            labWhiteCount.Text = "";
            //_rbAI = true;         //�Ƿ��˻�����
            _over = false;          //�ж���Ϸ�Ƿ����
            _personDown = true;      //�����ж����Ƿ����ӣ��˻�����ʱ��
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
            listBoxAIInfo.Items.Add("Ĭ�����ִ���ӣ�AI�ȼ�Ϊ�ͼ���");
        }
        #endregion

        #region //�˳�
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();  //�������ý���
        }
        #endregion

        #region //�������
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBack_Click(object sender, EventArgs e)
        {
            if (_rbAI == true)  //�˻���ս���˻������壬ֻ���˻���ս��ʱ���������
            {
                #region //�˻�һ�����ص�֮ǰ���
                _num = _num - _regret;
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (_num == 0)   //˵��ǡ������_regret��
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
            else  //���˶�ս�����Ի���һ��
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
            listBoxAIInfo.Items.Add("��һ��壡");
            _regret--;
        }
        #endregion

        #region //��ʾ���λ��
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Start();
            Point p = PointToClient(MousePosition);
            label2.Text = p.X.ToString() + "  " + p.Y.ToString();
        }
        #endregion

        #region //���ӳ���
        /// <summary>
        /// //���ӳ���
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
            {  //�õ�����λ��
                row = x = (x - 41) % 40 > 0 ? (int)((x - 41) / 40) : (int)((x - 41) / 40) - 1;
                col = y = (y - 74) % 40 > 0 ? ((int)((y - 74) / 40)) : (int)((y - 74) / 40) - 1;
                if (_xy[x, y] == 0)   //��ʾ�հ�λ��
                {
                    if (this._isBlack)
                    {
                        _xy[x, y] = 1;//��ʾ����
                        canEat = Eaten(x, y);
                        if (canEat == false)
                        {
                            _xy[x, y] = 0;
                            // MessageBox.Show("�˴���������");
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
                            //  MessageBox.Show("�˴���������"); 
                        }//��ʾ����
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
                    // MessageBox.Show("�˴��������ӣ���������");
                    return false;
                }
            }
            else
                return false;
        }
        #endregion

        #region ������  
        /// <summary>
        ///��ʼ������
        /// </summary>
        public void draw_QiPan()
        {
            Graphics g = Graphics.FromImage(bitmap);
            SolidBrush orgSoildBrush = new SolidBrush(Color.White);  //��ˢ��ɫΪ��ɫ
            Pen LinePen = new Pen(Color.Black);   //�߱���Ϊ��ɫ
            g.FillRectangle(orgSoildBrush, 0, 0, 600, 600);   //�����ɫ
            for (int x_ = 74; x_ <= 394; x_ += 40)
            {
                g.DrawLine(LinePen, x_, 74, x_, 394);  //������ͬ��9������
            }
            for (int y_ = 74; y_ <= 394; y_ += 40)
            {
                g.DrawLine(LinePen, 74, y_, 394, y_);  //������ͬ��9������
            }
            this.pictureBox2.Image = bitmap;   //���ý�������ӵ�pictureBox���
            if (_num <= 0)   //û��ʼ�߲��������
            {
                btnBack.Enabled = false;
            }
            else
                btnBack.Enabled = true;
        }
        #endregion

        #region //�����ӳ���
        /// <summary>
        /// ������
        /// </summary>
        public void Draw_QiZi()
        {
            //draw_QiPan();
            int x1, y1;  //���ӵ�����
            int _blackTempCount = 0;           //������ʱ����������
            int _whiteTempCount = 0;         //������ʱ����������
            Image img = pictureBox2.Image;
            //���ԭͼƬ���������ظ�ʽ֮�еģ�����Ҫת����bmp��ʽ

            Bitmap bmp = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);  //�õ�����
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    x1 = 40 * i + 74;
                    y1 = 40 * j + 74;
                    if (_xy[i, j] == 1)  //����
                    {
                        Point point = new Point(x1, y1);
                        g.DrawImage(blackBitmap, point);  //������
                        _blackTempCount++;
                        // MessageBox.Show(i.ToString() + "  " + j.ToString());
                    }
                    else if (_xy[i, j] == -1)  //����
                    {
                        // MessageBox.Show(i.ToString() + "  " + j.ToString());
                        Point point = new Point(x1, y1);
                        g.DrawImage(whiteBitmap, point);  //������
                        _whiteTempCount++;
                    }
                    else  //�˴��Ȳ��ǰ����ֲ��Ǻ���
                    {
                        if (_isBlack == true)//��ǰ�Ƿ�ڷ�����
                        {
                            _xy[i, j] = 1;
                        }
                        else
                        {
                            _xy[i, j] = -1;
                        }
                        if (Eaten(i, j) == true)
                        {
                            if (_isBlack == true)   //��ǰ�Ƿ�ڷ�����
                            {
                                Point point = new Point(x1, y1);
                                g.DrawImage(cBlackBitmap, point);  //���������ִ����
                            }
                            else
                            {
                                Point point = new Point(x1, y1);
                                g.DrawImage(cWhiteBitmap, point); //����������ִ����
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

        #region//��������
        /// <summary>
        /// //��������,���ڻ���
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

        #region //�����������ж��Լ�������Ϸ
        /// <summary>
        /// �����������ж��Լ�������Ϸ
        /// </summary>
        public void EndGame()
        {
            #region  // �����Ƿ�������
            bool can = true;//�ж�ĳ���Ƿ�������µı�־
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

            #region //�ж��Ƿ��������
            bool canChase = true;    //�Ƿ�˫����������� 
            can = canDown(_isBlack);
            if (win == false)
            {

                if (can == false)
                {
                    if (_isBlack == true)
                    {
                        MessageBox.Show("�ڷ��޵ط������ӣ��׷��ɼ�������");
                        listBoxAIInfo.Items.Add(string.Format("�ڷ��޵ط������ӣ��׷��ɼ�������"));
                        _personDown = !_personDown;
                    }
                    else
                    {
                        MessageBox.Show("�׷��޵ط������ӣ��ڷ��ɼ�������");
                        listBoxAIInfo.Items.Add(string.Format("�׷��޵ط������ӣ��ڷ��ɼ�������"));
                        _personDown = !_personDown;
                    }
                    _isBlack = !_isBlack;
                    Draw_QiZi();
                }
                if (canDown(_isBlack) == false) //����Է�Ҳ�׵ط������� ����Ϸ����
                {
                    canChase = false;
                }
            }
            #endregion

            #region  //�ж���Ӯ
            if (win == true || canChase == false)
            {

                if (_blackCount > _whiteCount)
                {
                    MessageBox.Show("��Ϸ����,�ڷ���ʤ");
                    listBoxAIInfo.Items.Add(string.Format("��Ϸ����,�ڷ���ʤ"));
                }
                else if (_blackCount < _whiteCount)
                {
                    MessageBox.Show("��Ϸ����,�׷���ʤ");
                    listBoxAIInfo.Items.Add(string.Format("��Ϸ����,�׷���ʤ"));
                }
                else
                {
                    MessageBox.Show(_blackCount + "  " + _whiteCount + "��Ϸ����,ƽ��");
                    listBoxAIInfo.Items.Add(string.Format("ƽ�֣���Ϸ������"));
                }

                pictureBox2.Enabled = false;
                labWhite.Visible = false;
                labBlack.Visible = false;
                btnSelect.Enabled = btnBack.Enabled = false;
                _over = true;
                return;
            }
            #endregion

            #region ��ʾ��˭����
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
            #region  // �����Ƿ�������
            bool can = true;//�ж�ĳ���Ƿ�������µı�־
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

            #region //�ж��Ƿ��������
            bool canChase = true;    //�Ƿ�˫����������� 
            can = canDown(_isBlack);
            if (win == false)
            {

                if (can == false)
                {
                    if (_isBlack == true)
                    {
                        MessageBox.Show("�ڷ��޵ط������ӣ��׷��ɼ�������");
                        listBoxAIInfo.Items.Add(string.Format("�ڷ��޵ط������ӣ��׷��ɼ�������"));
                        _personDown = !_personDown;
                    }
                    else
                    {
                        MessageBox.Show("�׷��޵ط������ӣ��ڷ��ɼ�������");
                        listBoxAIInfo.Items.Add(string.Format("�׷��޵ط������ӣ��ڷ��ɼ�������"));
                        _personDown = !_personDown;
                    }
                    _isBlack = !_isBlack;
                    Draw_QiZi();
                }
                if (canDown(_isBlack) == false) //����Է�Ҳ�׵ط������� ����Ϸ����
                {
                    canChase = false;
                }
            }
            #endregion

            #region  //�ж���Ӯ
            if (win == true || canChase == false)
            {

                if (_blackCount > _whiteCount)
                {
                    MessageBox.Show("��Ϸ����,�ڷ���ʤ");
                    listBoxAIInfo.Items.Add(string.Format("��Ϸ����,�ڷ���ʤ��"));
                }
                else if (_blackCount < _whiteCount)
                {
                    MessageBox.Show("��Ϸ����,�׷���ʤ");
                    listBoxAIInfo.Items.Add(string.Format("��Ϸ����,�׷���ʤ��"));
                }
                else
                {
                    MessageBox.Show(_blackCount + "  " + _whiteCount + "��Ϸ����,ƽ��");
                    listBoxAIInfo.Items.Add(string.Format("ƽ��,��Ϸ������"));
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

        #region //ˢ�����̼��ж��Ƿ��˻�����
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            int row;
            int col;
            if (_begin == false)
            {
                return;
            }
            Point p = PointToClient(MousePosition);
            bool luozi = this.In_QiZi(p.X, p.Y, out row, out col);//�жϵ���Ƿ���Ч

            pictureBox2.Invalidate();
            if (luozi == true)
            {
                listBoxAIInfo.Items.Add(string.Format("�������λ�ã�({0}��{1})", row + 1, col + 1));
                Savefor(); _num++;
            }//���浱ǰ����

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
                        listBoxAIInfo.Items.Add(string.Format("��������λ�ã�({0}��{1})", row1 + 1, col1 + 1));
                        Savefor(); _num++;//���浱ǰ����
                    }
                    else
                    {
                        listBoxAIInfo.Items.Add(string.Format("�������ӿ��£�"));
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
                        listBoxAIInfo.Items.Add(string.Format("��������λ�ã�({0}��{1})", row2 + 1, col2 + 1));
                        Savefor(); _num++;//���浱ǰ����
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
                        listBoxAIInfo.Items.Add(string.Format("��������λ�ã�({0}��{1})", row3 + 1, col3 + 1));
                        Savefor(); _num++;     //���浱ǰ����
                    }
                    else
                    {
                        listBoxAIInfo.Items.Add(string.Format("�������ӿ��£�"));
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

        #region  //���ӳ���
        /// <summary>
        /// //���ӳ���
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Eat(int x, int y)
        {
            #region //�жϵ�ǰ���ӵ���ɫ
            int a, b; //a,b �������ӵ���ɫ 1Ϊ�ڣ�-1Ϊ��
            if (_xy[x, y] == 1)
            {
                a = 1; b = -1;
            }
            else
            {
                a = -1; b = 1;
            }
            #endregion
            #region //���ӳ���

            #region  //���ҳ���
            int i = 0;
            for (i = x + 1; i < 8;) //��_xy[x, y]=1 Ϊ����b=-1��������ұ߰��ӵĸ��������ұ�Ϊ������ͣ�¡�xy[x, y]=-1 Ϊ����b=1
            {   //��b���ӵ���ɫ������_xy[x, y]�෴
                if (_xy[i, y] == b)  //���Ҽ������ӵĸ���
                { ++i; continue; }
                else
                    break;
            }
            if (i > x + 1 && i < 8 && _xy[i, y] == a)//�������ӣ�_xy[i, y] ��a��ͬ����ɫ
            {
                for (int j = x + 1; j <= i - 1; j++) //���ܳԶԷ�������_xy[x, y]�ұߵ����б�����Լ���ͬ����ɫ
                {
                    _xy[j, y] = a;
                }
            }
            #endregion

            #region  //�������
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

            #region  //���ϳ���
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

            #region  //���³���
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

            #region //�����ϳ���
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

            #region   //�����³���
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

            #region //�����ϳ���
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

            #region //�����³���
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

        #region //�ж��Ƿ����ӿ���
        /// <summary>
        /// �ж��Ƿ����ӿ���
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

        #region //�ж��Ƿ��ܹ�����
        /// <summary>
        /// �ж��Ƿ��ܹ�����
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Eaten(int x, int y)
        {
            bool _canEat = false;
            #region //�жϵ�ǰ���ӵ���ɫ
            int a, b; //a,b �������ӵ���ɫ 1Ϊ�ڣ�-1Ϊ��
            if (_xy[x, y] == 1)
            {
                a = 1; b = -1;
            }
            else
            {
                a = -1; b = 1;
            }
            #endregion
            #region //���ӳ���

            #region  //���ҳ���
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

            #region  //�������
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

            #region  //���ϳ���
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

            #region  //���³���
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

            #region //�����ϳ���
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

            #region   //�����³���
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

            #region //�����ϳ���
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

            #region //�����³���
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

        #region  //�˻�����ʱ�������ж��Ĳ����Ӷ�
        /// <summary>
        ///�˻�����ʱ�������ж��Ĳ����Ӷ�
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Eatnum(int x, int y)
        {
            int num = 0;
            #region //�жϵ�ǰ���ӵ���ɫ
            int a, b; //a,b �������ӵ���ɫ 1Ϊ�ڣ�-1Ϊ��
            if (_xy[x, y] == 1)
            {
                a = 1; b = -1;
            }
            else
            {
                a = -1; b = 1;
            }
            #endregion
            #region //���ӳ���

            #region  //���ҳ���
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

            #region  //�������
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

            #region  //���ϳ���
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

            #region  //���³���
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

            #region //�����ϳ���
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

            #region   //�����³���
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

            #region //�����ϳ���
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

            #region //�����³���
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

        #region //�˻�����ʱ�����Ե�ѡ��
        #region //�м�
        /// <summary>
        /// �м�
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

        #region //�ͼ�
        /// <summary>
        /// //�ͼ�
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

        #region //�߼�   
        /// <summary>
        /// ///�߼�  
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
            #region   //��ǰ���Ե����ѡ�� �൱���м����Ե�ѡ��
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
            int mm = 1000;             //��������
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


            #region  //���Կ����Լ���һ���󣬶Է�����õ��߷���Ȼ��ѡ�����п������У��Է���õ��߷�
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


            #region  //�ۺ���������ѡ��  ѡ����õ�
            //�нǵ�ռ�ǣ��޽ǵķ�ֹ�Է�ռ��
            //�бߵ�ռ�ߣ��ޱߵķ�ֹ�Է�ռ��
            //�ͼ��㷨
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

        #region  //�˻�����ʱ�����ڿ�����һ��������
        /// <summary>
        /// �˻�����ʱ�����ڿ�����һ��������
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

        #region //�˻�����ʱ�����������ӳ���
        /// <summary>
        /// //�˻�����ʱ�����������ӳ���
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void ComputerIn_QiZi(int x, int y)
        {
            if (_xy[x, y] == 0)
            {
                if (this._isBlack)
                {
                    _xy[x, y] = 1;//��ʾ����
                    Eat(x, y); this.x = x; this.y = y;
                }
                else
                {
                    _xy[x, y] = -1;   //��ʾ����
                    Eat(x, y); this.x = x; this.y = y;
                }
                this._isBlack = !this._isBlack;  //���������ת������

            }
        }

        #endregion
        #region//˵��
        private void btnInstruct_Click(object sender, EventArgs e)
        {
            Introduction inr = new Introduction();
            inr.Show();
        }

        /// <summary>
        /// �رմ����¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmAI_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!_over) //��ʾû�н���
            {
                if(MessageBoxEx.Show("��Ϸ��δ�������Ƿ�ȷ���˳���","��ʾ��",MessageBoxButtons.OKCancel)==DialogResult.OK)
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
        /// ��갴��ʱ��Ӧ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            //if (!_isAIOrPeople)
            //{
            //    //i������j������ y��ӦΪ�� x��ӦΪ�� 
            //    int x = e.X / 40;
            //    int y = e.Y / 40;
            //    ////�ڰ����߼�
            //    if (!(x < 1 || x > 8 || y < 1 || y > 8))
            //    {
            //        if (grid[x - 1, y - 1] == DotColor.None) //��������
            //        {
            //            //int color = grid[x - 1, y - 1];
            //            service.SendToServer(string.Format("SetDot,{0},{1},{2},{3}", tableIndex, x - 1, y - 1, side));
            //        }
            //    }
            //}
        }

        /// <summary>
        /// ���ƽ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            int _blackTempCount = 0;           //������ʱ����������
            int _whiteTempCount = 0;         //������ʱ����������
            Graphics g = e.Graphics;
            for (int i = 0; i <= _xy.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= _xy.GetUpperBound(1); j++)
                {
                    if (_xy[i, j] != 0)
                    {
                        if (_xy[i, j] == 1)
                        {
                            //i������j������ y��ӦΪ�� x��ӦΪ�� 
                            g.DrawImage(blackBitmap, (i + 1) * 40, (j + 1) * 40);
                        }
                        else
                        {
                            g.DrawImage(whiteBitmap, (i + 1) * 40, (j + 1) * 40);
                        }
                    }

                    if (_xy[i, j] == 1)  //����
                    {
                        //Point point = new Point(x1, y1);
                        //g.DrawImage(imageList1.Images[0], point);  //������
                        _blackTempCount++;
                        // MessageBox.Show(i.ToString() + "  " + j.ToString());
                    }
                    else if (_xy[i, j] == -1)  //����
                    {
                        // MessageBox.Show(i.ToString() + "  " + j.ToString());
                        //Point point = new Point(x1, y1);
                        //g.DrawImage(imageList1.Images[1], point);  //������
                        _whiteTempCount++;
                    }
                    else  //�˴��Ȳ��ǰ����ֲ��Ǻ���
                    {
                        if (_isBlack == true)//��ǰ�Ƿ�ڷ�����
                        {
                            _xy[i, j] = 1;
                        }
                        else
                        {
                            _xy[i, j] = -1;
                        }
                        if (Eaten(i, j) == true)
                        {
                            if (_isBlack == true)   //��ǰ�Ƿ�ڷ�����
                            {
                                //Point point = new Point(x1, y1);
                                g.DrawImage(cBlackBitmap, (i + 1) * 40, (j + 1) * 40);  //���������ִ����
                            }
                            else
                            {
                                //Point point = new Point(x1, y1);
                                g.DrawImage(cWhiteBitmap, (i + 1) * 40, (j + 1) * 40); //����������ִ����
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