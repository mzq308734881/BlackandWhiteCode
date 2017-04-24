using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;


namespace Client
{
    public partial class FrmClient : Office2007Form
    {
        delegate void ButtonDelegate(Button button, bool flag);
        ButtonDelegate buttonDelegate;

        delegate void MyCheckBoxDelegate(CheckBox ckb, bool flag);
        MyCheckBoxDelegate ckbDelegate;

        private FrmPeople frmPeople;
        private TcpClient client = null;
        private StreamWriter sw;
        private StreamReader sr;
        private Service service;
        private int maxPlayingTables;
        private CheckBox[,] checkBoxGameTables;
        //所坐的游戏桌座位号,-1表示未入座
        private int side = -1;
        private bool normalExit = false;
        public FrmClient()
        {
            InitializeComponent();
            buttonDelegate = new ButtonDelegate(SetButtonItem);
            ckbDelegate = new MyCheckBoxDelegate(SetckBox);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            //string _ip = "127.0.0.1";
            string _ip = txtServerIp.Text.Trim();
            int _port = int.Parse(txtServerPort.Text.Trim());
            //string _ip = Dns.GetHostName();
            try
            {
                client = new TcpClient(_ip, _port);     //实例化当前客户端
            }
            catch
            {
                MessageBox.Show("与服务器连接失败", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            NetworkStream netStream = client.GetStream();
            sr = new StreamReader(netStream, System.Text.Encoding.UTF8);
            sw = new StreamWriter(netStream, System.Text.Encoding.UTF8);
            service = new Service(listBox1, sw);

            service.SendToServer("Login," + txtPlayer.Text.Trim());

            Thread threadReceive = new Thread(new ThreadStart(ReceiveData));
            threadReceive.Start();
        }

        /// <summary>
        /// 收消息
        /// </summary>
        private void ReceiveData()
        {
            bool exitWhile = false;
            while (exitWhile == false)
            {
                string receiveString = null;
                try
                {
                    receiveString = sr.ReadLine();
                }
                catch
                {
                    service.SetListBox("接受数据失败");

                }
                if (receiveString == null)
                {
                    if (!normalExit)
                    {
                        MessageBox.Show("与服务器失去联系,游戏无法继续");
                    }
                    normalExit = true;
                    break;
                }

                service.SetListBox(string.Format("收到:{0}", receiveString));

                string[] splitString = receiveString.Split(',');

                switch (splitString[0])
                {
                    case "Tables":
                        //字符串格式: Tables,各桌是否有人的字符串
                        //其中每位表示一个座位，1表示有人，0表示无人
                        string s = splitString[1];
                        //如果maxPlayingTables为0,说明尚未创建checkBoxGameTables
                        if (maxPlayingTables == 0)
                        {
                            maxPlayingTables = s.Length / 2;
                            checkBoxGameTables = new CheckBox[maxPlayingTables, 2];

                            for (int i = 0; i < maxPlayingTables; i++)
                            {
                                AddCheckBoxToPanel(s, i);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < maxPlayingTables; i++)
                            {
                                for (int j = 0; j < 2; j++)
                                {
                                    if (s[2 * i + j] == '0')
                                    {
                                        UpdateCheckBox(checkBoxGameTables[i, j], false);
                                    }
                                    else
                                    {
                                        UpdateCheckBox(checkBoxGameTables[i, j], true);
                                    }
                                }
                            }
                        }
                        break;
                    case "SetDot":
                        frmPeople.SetDot(int.Parse(splitString[1]), int.Parse(splitString[2]), int.Parse(splitString[3]));
                        break;
                    case "SetDotCollection":
                        string newString = receiveString.Substring(receiveString.IndexOf(',') + 1);
                        frmPeople.SetDotCollection(newString);
                        break;
                    case "SitDown":
                        frmPeople.SetTableSideText(splitString[1], splitString[2], string.Format("{0}进入", splitString[2]));
                        frmPeople.ShowTalkMessage(splitString[2].ToString()+"上线！");
                        break;
                    case "UnsetDot":
                        int x = 20 * (int.Parse(splitString[1]) + 1);
                        int y = 20 * (int.Parse(splitString[2]) + 1);
                        frmPeople.UnsetDot(x, y);
                        frmPeople.SetGradeText(splitString[3], splitString[4]);
                        break;
                    case "Win":
                        string winner = "";
                        if (int.Parse(splitString[1]) == DotColor.Black)
                        {
                            winner = "黑方出现相邻点，白方胜利!";
                        }
                        else
                        {
                            winner = "白方出现相邻点，黑方胜利!";
                        }
                        frmPeople.ShowMessage(winner);
                        frmPeople.Restart(winner);
                        break;
                    case "GetUp":
                        int tableIndex = int.Parse(splitString[2]);
                        if (side == int.Parse(splitString[1]))  //表示是否是当前座位的玩家离场
                        {
                            //自己离场
                            //btnConnect.Enabled = true;   //离场后，允许重新登录
                            SetButtonItem(btnConnect, true);
                            CheckBox ckb = checkBoxGameTables[tableIndex, int.Parse(splitString[1])];
                            //ckb.CheckState=
                            //SetckB(ckb, false);

                            //UpdateCheckBox(ckb,true);
                            side = -1;
                        }
                        else  //对方离场
                        {
                            frmPeople.SetTableSideText(splitString[1], "", string.Format("{0}退出", splitString[3]));
                            frmPeople.Restart("敌人逃跑了，我方胜利了");
                        }
                        break;
                    case "Send":
                        string userName = splitString[4].ToString();
                        string info = splitString[3].ToString();
                        frmPeople.ShowTalkMessage(userName+":"+info);
                        break;
                    case "Host":
                        bool isBlack;
                        if (int.Parse(splitString[1]) == DotColor.Black)
                        {
                            winner = "黑方发出托管请求：" + splitString[2]+ splitString[3];
                            isBlack = true;
                        }
                        else
                        {
                            winner = "白方发出托管请求：" + splitString[2]+ splitString[3];
                            isBlack = false;
                        }
                        frmPeople.ShowMessage(winner);
                        frmPeople.ShowHost(winner,isBlack);
                        //frm2.Restart(winner);
                        break;
                    case "HostGrade":
                        string grade0 = splitString[2].ToString();
                        string grade1 = splitString[3].ToString();
                        frmPeople. ShowHostGrade(grade0,grade1);
                        break;
                    case "HostDotGrid":
                        side = int.Parse(splitString[1]);
                        int row = int.Parse(splitString[2]);
                        int col = int.Parse(splitString[3]);
                        int state = int.Parse(splitString[4]);
                        frmPeople.ShowHostDotGrid(row,col,state); 
                        break;
                    case "SetHostDot":
                         side = int.Parse(splitString[3]);
                         row = int.Parse(splitString[1]);
                         col = int.Parse(splitString[2]);
                         //state = int.Parse(splitString[4]);
                        frmPeople.ShowHostDotGrid(row, col, side); 
                        break;
                    case "FreshPic":
                        side = int.Parse(splitString[1]);
                        frmPeople.FreshPic();
                        break;
                    case "CancelHost":   //取消托管
                         int  otherside = int.Parse(splitString[1]);
                        string atherPlayer = splitString[2].ToString();
                        frmPeople.CancelHost(otherside, atherPlayer);
                        break;
                }

                //string s = splitString[1];FreshPic
                //if (maxPlayingTables == 0)
                //{
                //    maxPlayingTables = s.Length / 2;
                //    checkBoxGameTables = new CheckBox[maxPlayingTables, 2];

                //    for (int i = 0; i < maxPlayingTables; i++)
                //    {
                //        AddCheckBoxToPanel(s, i);
                //    }
                //}
            }
            Application.Exit();
        }

        /// <summary>
        /// ButtonItem跨线程操作
        /// </summary>
        /// <param name="button"></param>
        /// <param name="flag"></param>
        public void SetButtonItem(Button button, bool flag)
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

        public void SetckBox(CheckBox ckb,bool flag)
        {
            if (ckb.InvokeRequired)
            {
                this.Invoke(ckbDelegate, ckb, flag);
            }
            else
            {
                ckb.Enabled = flag;
            }
        }


        delegate void ExitFormPlayingDelegate();
        private void ExitFormPlaying()
        {
            if (frmPeople.InvokeRequired)
            {
                ExitFormPlayingDelegate d = new ExitFormPlayingDelegate(ExitFormPlaying);
                this.Invoke(d);
            }
            else
            {
                frmPeople.Close();
            }
        }


        delegate void PanelCallBack(string s, int i);
        private void AddCheckBoxToPanel(string s, int i)
        {
            if (panel1.InvokeRequired)
            {
                PanelCallBack d = new PanelCallBack(AddCheckBoxToPanel);
                this.Invoke(d, s, i);
            }
            else
            {
                Label label = new Label();
                label.Location = new Point(10, 15 + i * 30);
                label.Text = string.Format("第{0}桌:", i + 1);
                label.Width = 70;
                this.panel1.Controls.Add(label);
                CreateCheckBox(i, 0, s, "黑方");
                CreateCheckBox(i, 1, s, "白方");
            }
        }

        delegate void CheckBoxDelegate(CheckBox checkbox, bool isChecked);
        private void UpdateCheckBox(CheckBox checkbox, bool isChecked)
        {
            if (checkbox.InvokeRequired)
            {
                CheckBoxDelegate d = new CheckBoxDelegate(UpdateCheckBox);
                this.Invoke(d, checkbox, isChecked);
            }
            else
            {
                if (side == -1)
                {
                    checkbox.Enabled = !isChecked;
                }
                else
                {
                    //已经坐到某游戏桌上，不允许再选其他桌
                    checkbox.Enabled = false;
                }
                //checkbox.Checked = isChecked;
            }
        }
        /// <summary>
        /// 创建桌子和座位
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="s"></param>
        /// <param name="text"></param>
        private void CreateCheckBox(int i, int j, string s, string text)
        {
            int x = j == 0 ? 100 : 200;
            checkBoxGameTables[i, j] = new CheckBox();
            checkBoxGameTables[i, j].Name = string.Format("check{0:0000}{1:0000}", i, j);
            checkBoxGameTables[i, j].Width = 60;
            checkBoxGameTables[i, j].Location = new Point(x, 10 + i * 30);
            checkBoxGameTables[i, j].Text = text;
            checkBoxGameTables[i, j].TextAlign = ContentAlignment.MiddleLeft;
            if (s[2 * i + j] == '1')
            {
                //1表示有人
                checkBoxGameTables[i, j].Enabled = false;
                checkBoxGameTables[i, j].Checked = true;
            }
            else
            {
                //0表示无人
                checkBoxGameTables[i, j].Enabled = true;
                checkBoxGameTables[i, j].Checked = false;
            }
            this.panel1.Controls.Add(checkBoxGameTables[i, j]);

            checkBoxGameTables[i, j].CheckedChanged += new EventHandler(checkBox_CheckedChanged);
        }

        /// <summary>
        /// 当选中黑棋或者白旗时立即打开对弈界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            if (checkbox.Checked)
            {
                btnConnect.Enabled = false;
                int i = int.Parse(checkbox.Name.Substring(5, 4));
                int j = int.Parse(checkbox.Name.Substring(9, 4));

                side = j;
                service.SendToServer(string.Format("SitDown,{0},{1}", i, j));
                frmPeople = new FrmPeople(i, j, sw);
                frmPeople.labPlayer.Text = txtPlayer.Text;
                frmPeople.Show();
            }
        }
    }
}
