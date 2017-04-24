using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using DevComponents.DotNetBar;

namespace Server
{
    public partial class FormServer : Office2007Form
    {
        IPAddress localAddress;
        //private int port = 51888;   //端口号
        private int port;
        private TcpListener myListener;   //监听客户端
        private Service service;
        List<User> userList = new List<User>();

        private GameTable[] gameTable;
        private int maxTables;   //最大桌子数
        private int maxUsers;    //得到最大用户数

        public FormServer()
        {
            InitializeComponent();
            service = new Service(listbox);
        }

        private void FormServer_Load(object sender, EventArgs e)
        {
            IPAddress[] addrIP = Dns.GetHostAddresses(Dns.GetHostName());
            localAddress = addrIP[0];   //得到本地IP
            //localAddress = IPAddress.TryParse(txtIP.Text) ;
            //localAddress =txtIP.Text.Trim();
            port = int.Parse(txtPort.Text.Trim());   //得到端口号
            buttonStop.Enabled = false;
        }

        /// <summary>
        /// 开启监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStart_Click(object sender, EventArgs e)
        {
            IPAddress localAddress = null;
            //用于判断IP和端口是否正确
            if(!IPAddress.TryParse(txtIP.Text, out localAddress)||!int.TryParse(txtPort.Text,out port))
            {
                MessageBox.Show("IP或端口格式错误！");
                return;
            }
            //System.Windows.Forms.MessageBox.Show(localAddress.ToString());

            if (int.TryParse(textBoxMaxTables.Text, out maxTables) == false  //得到最大的桌子数
                || int.TryParse(textBoxMaxUsers.Text, out maxUsers) == false)
            {
                MessageBox.Show("请输入在规定范围内的正整数");
                return;
            }
            //得到最大桌子数
            gameTable = new GameTable[maxTables];
            for (int i = 0; i < maxTables; i++)
            {
                gameTable[i] = new GameTable(this.listbox);
            }
            try
            {
            myListener = new TcpListener(localAddress, port);
            myListener.Start();
            service.SetListBox(string.Format("开始在{0}:{1}监听客户连接", localAddress, port));
            ThreadStart ts = new ThreadStart(ListenClientConnect);
            Thread myThread = new Thread(ts);
            myThread.Start();
            buttonStart.Enabled = false;
            buttonStop.Enabled = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show("异常："+ex.Message);
            }
        }

        /// <summary>
        /// 监听用户连接
        /// </summary>
        private void ListenClientConnect()
        {
            while (true)
            {
                TcpClient newClient = null;
                try
                {
                    newClient = myListener.AcceptTcpClient();    //接收客户端的请求
                }
                catch
                {
                    break;
                }
                User user = new User(newClient);
                userList.Add(user);   //连接的用户List中添加用户
                service.SetListBox(string.Format("{0}进入", newClient.Client.RemoteEndPoint));   //得到正在连接用户的IP和端口
                service.SetListBox(string.Format("当前连接用户数:{0}",userList.Count));
                
                //线程执行带参方法，将接收消息的方法放在线程里
                ParameterizedThreadStart pts = new ParameterizedThreadStart(ReceiveData);
                Thread threadReceive = new Thread(pts);
                threadReceive.Start(user);
            }
        }

        /// <summary>
        /// 断开服务端
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStop_Click(object sender, EventArgs e)
        {
            service.SetListBox(string.Format("目前连接用户数:{0}", userList.Count));
            service.SetListBox("开始停止服务，并依次使用户退出!");
            myListener.Stop();    //关闭监听器
            buttonStart.Enabled = true;
            buttonStop.Enabled = false;

        }

        /// <summary>
        /// 服务端接收消息的方法
        /// </summary>
        /// <param name="obj"></param>
        private void ReceiveData(object obj)
        {
            User user = (User)obj;
            TcpClient client = user.client;

            bool normalExit = false;

            bool exitWhile = false;
            while (exitWhile == false)
            {

                string receiveString = null;
                try
                {
                    receiveString = user.sr.ReadLine();   //得到客户端发送的数据流
                }
                catch
                {
                    service.SetListBox("接受数据失败");
                }
                if (receiveString == null)
                {
                    if (normalExit == false)
                    {
                        if (client.Connected)   //表示客户端掉线
                        {
                            service.SetListBox(string.Format("与{0}失去了联系,已终止接受该用户信息", client.Client.RemoteEndPoint));
                        }
                        RemoveClientfromPlayer(user);
                    }
                    break;
                }

                //将字符串显示出来
                service.SetListBox(string.Format("来自{0}:{1}", user.userName, receiveString));

                string[] splitString = receiveString.Split(',');   //包括操作和二维数组的消息
                string sendString = "";   //发出去的消息


                //basilwang 2008-09-06 prepare to analysis the SitDown command 
                //new to this version myGame3
                int tableIndex = -1;      //桌号
                int side = -1;               //座位号，0或者1
                int anotherSide = -1; //对方座位号，0或者1


                switch (splitString[0])   //取出操作的类型
                {
                    case "Login":   //操作类型，用户名，和用户IP和端口
                        user.userName = string.Format("[{0}--{1}]", splitString[1], client.Client.RemoteEndPoint);
                        //获得玩家姓名和IP端口号
                        //将桌子游戏座位的状态发送给客户端
                        sendString = "Tables," + this.GetOnlineString();

                        service.SendToOne(user, sendString);
                        break;

                    case "SitDown":  //表示点击座位坐下
                        tableIndex = int.Parse(splitString[1]);     //得到桌子号索引，从第0号桌子开始
                        side = int.Parse(splitString[2]);              //得到座位号，0代表黑子，1代表白子
                        gameTable[tableIndex].gamePlayer[side].user = user;
                        gameTable[tableIndex].gamePlayer[side].someone = true;  //表示有人
                        service.SetListBox(string.Format(
                            "用户：{0}，在第{1}桌第{2}座入座", user.userName, tableIndex + 1, side + 1));
                        //得到对家座位号
                        anotherSide = (side + 1) % 2;
                        //判断对方是否有人
                        if (gameTable[tableIndex].gamePlayer[anotherSide].someone)
                        {
                            //先告诉该用户对家已经入座
                            //发送格式:SitDown,座位号,用户名
                            sendString = string.Format("SitDown,{0},{1}", anotherSide,
                                gameTable[tableIndex].gamePlayer[anotherSide].user.userName);
                            service.SendToOne(user, sendString);
                        }
                        //同时告诉两个用户该用户入座(也可能对方无人)
                        //发送格式:SitDown,座位号,用户名
                        sendString = string.Format("SitDown,{0},{1}", side, user.userName);
                        service.SendToBoth(gameTable[tableIndex], sendString);

                        //重新将游戏室各桌情况发送给所有用户
                        service.SendToAll(userList, "Tables," + this.GetOnlineString());
                        break;
                    case "Start":
                        //格式：Start,桌号,座位号
                        //该用户单击了开始按钮
                        tableIndex = int.Parse(splitString[1]);
                        side = int.Parse(splitString[2]);
                        gameTable[tableIndex].gamePlayer[side].started = true;  //表示已经开始
                        if (side == 0)
                        {
                            anotherSide = 1;  //对手
                            sendString = "Message,黑方已开始。";
                        }
                        else
                        {
                            anotherSide = 0;  //对手
                            sendString = "Message,白方已开始。";
                        }
                        service.SendToBoth(gameTable[tableIndex], sendString);

                        //如果对手也开始了，就将棋子信息显示在棋盘上
                        if (gameTable[tableIndex].gamePlayer[anotherSide].started == true)
                        {
                            gameTable[tableIndex].ResetGrid();  //初始化二维数组
                            //gameTable[tableIndex].StartTimer();
                            //格式 行，列，值，行，列，值 ....黑子数，白子数，该谁走
                            string s="3,3,0,3,4,1,4,3,1,4,4,0,2,2,0";
                            gameTable[tableIndex].SetDotCollection(s);
                            gameTable[tableIndex].SetGrid(3, 3, 0);   //设置棋子的位置，0代表黑子，1代表白子
                            gameTable[tableIndex].SetGrid(3, 4, 1);
                            gameTable[tableIndex].SetGrid(4, 3, 1);
                            gameTable[tableIndex].SetGrid(4, 4, 0);
                        }
                        break;
                    case "FreshGrid":
                        //格式：Start,桌号,座位号
                        //该用户单击了开始按钮
                        tableIndex = int.Parse(splitString[1]);
                        side = int.Parse(splitString[2]);
                        int x1 = int.Parse(splitString[3]);
                        int y1 = int.Parse(splitString[4]);
                        int state1 = int.Parse(splitString[5]);
                        gameTable[tableIndex].gamePlayer[side].started = true;  //表示已经开始
                        if (side == 0)
                        {
                            anotherSide = 1;  //对手
                            sendString = "Message,黑方已开始。";
                        }
                        else
                        {
                            anotherSide = 0;  //对手
                            sendString = "Message,白方已开始。";
                        }
                        //service.SendToBoth(gameTable[tableIndex], sendString);

                        //如果对手也开始了，就将棋子信息显示在棋盘上
                        if (gameTable[tableIndex].gamePlayer[anotherSide].started == true)
                        {
                            //gameTable[tableIndex].ResetGrid();  //初始化二维数组
                            //gameTable[tableIndex].StartTimer();
                            //格式 行，列，值，行，列，值 ....黑子数，白子数，该谁走
                            //string s = "3,3,0,3,4,1,4,3,1,4,4,0,2,2,0";
                            //gameTable[tableIndex].SetDotCollection(s);
                            gameTable[tableIndex].SetGrid(x1, y1, state1);   //设置棋子的位置，0代表黑子，1代表白子
                            //gameTable[tableIndex].SetGrid(3, 4, 1);
                            //gameTable[tableIndex].SetGrid(4, 3, 1);
                            //gameTable[tableIndex].SetGrid(4, 4, 0);
                        }
                        break;
                    case "SetDot":   //下棋子
                        tableIndex = int.Parse(splitString[1]);
                        side = int.Parse(splitString[4]);  //得到座位号
                        int xi = int.Parse(splitString[2]);
                        int xj = int.Parse(splitString[3]);
                        anotherSide = (side + 1) % 2;
                        gameTable[tableIndex].SetDot(xi, xj, side);
                        break;
                    case "SetHostDot":   //下棋子
                        tableIndex = int.Parse(splitString[1]);
                        side = int.Parse(splitString[4]);  //得到座位号
                        anotherSide = (side + 1) % 2;
                        int x = int.Parse(splitString[2]);
                        int y = int.Parse(splitString[3]);
                        //gameTable[tableIndex].SetDot(x, y, side);
                        sendString = string.Format("SetHostDot,{0},{1},{2}", x, y, side);
                        service.SendToOne(gameTable[tableIndex].gamePlayer[anotherSide].user, sendString);
                        break;
                    case "GetUp":  //表示离开座位
                        tableIndex = int.Parse(splitString[1]);
                        side = int.Parse(splitString[2]);
                        service.SetListBox(string.Format("{0}离座,返回游戏室", user.userName));
                        service.SendToBoth(gameTable[tableIndex], string.Format("GetUp,{0},{1},{2}", side,tableIndex, user.userName));
                        gameTable[tableIndex].gamePlayer[side].someone = false;  //设置为没人
                        gameTable[tableIndex].gamePlayer[side].started = false;  
                        gameTable[tableIndex].gamePlayer[side].grade = 0;
                        anotherSide = (side + 1) % 2;
                        if (gameTable[tableIndex].gamePlayer[anotherSide].someone)
                        {
                            gameTable[tableIndex].gamePlayer[anotherSide].started = false;  
                            gameTable[tableIndex].gamePlayer[anotherSide].grade = 0;  //分数为0
                        }
                        //重新将所有桌子的座位信息发送给送油用户
                        service.SendToAll(userList, "Tables," + this.GetOnlineString());
                        break;
                    case "Logout":  //表示关闭客户端
                        service.SetListBox(string.Format("{0}退出游戏室", user.userName));
                        normalExit = true;
                        exitWhile = true;
                        break;
                    case "SetDotCollection":  //点集合
                        //排除splitString[0]
                        tableIndex = int.Parse(splitString[1]);
                        side = int.Parse(splitString[2]);
                        gameTable[tableIndex].gamePlayer[side].user = user;
                        gameTable[tableIndex].gamePlayer[side].someone = true;
                        string s1 = receiveString.Substring(receiveString.IndexOf(',') + 1);
                        gameTable[tableIndex].SetDotCollection(s1);  //向tableIndex号桌子的两个用户发送点的集合
                        break;
                    //case "UnsetDot":
                    //    tableIndex = int.Parse(splitString[1]);
                    //    side = int.Parse(splitString[2]);
                    //    int xi = int.Parse(splitString[3]);
                    //    int xj = int.Parse(splitString[4]);
                    //    int color = int.Parse(splitString[5]);
                    //    gameTable[tableIndex].UnsetDot(xi, xj, color);
                    //    break;
                    case "Send":   //单纯发送消息
                        tableIndex = int.Parse(splitString[1]);
                        side = int.Parse(splitString[2]);
                        gameTable[tableIndex].gamePlayer[side].someone = true;  //表示有人
                        //service.SetListBox(string.Format(
                        //    "用户：{0}，在第{1}桌第{2}座入座", user.userName, tableIndex + 1, side + 1));
                        //得到对家座位号
                        gameTable[tableIndex].gamePlayer[side].user = user;  //得到发送者信息
                        string info = splitString[3];  //发送的消息Msg   
                        string player= splitString[4];   //玩家
                        anotherSide = (side + 1) % 2;
                        if (gameTable[tableIndex].gamePlayer[anotherSide].someone)  //表示对面有人
                        {
                            //先告诉该用户对家已经入座
                            //发送格式:SitDown,座位号,用户名
                            sendString = string.Format("Send,{0},{1},{2},{3}", side,
                                gameTable[tableIndex].gamePlayer[side].user.userName+":", info, player);
                            service.SendToBoth(gameTable[tableIndex], sendString);
                            //service.SendToOne(gameTable[tableIndex].gamePlayer[anotherSide].user, sendString);
                        }
                        else  //表示对面没人了，然后就给自己回个消息说对面人离开了
                        {
                            sendString = string.Format("Send,{0},{1},{2},{3}", side, gameTable[tableIndex].gamePlayer[side].user.userName = ":","发送失败，用户【"+ gameTable[tableIndex].gamePlayer[side].user.userName+"】已离开！", player);
                            service.SendToOne(gameTable[tableIndex].gamePlayer[side].user, sendString);
                        }
                        break;
                    case "Host":   //单纯发送消息
                        tableIndex = int.Parse(splitString[1]);     //得到桌子号索引，从第0号桌子开始
                        side = int.Parse(splitString[2]);              //得到座位号，0代表黑子，1代表白子
                        gameTable[tableIndex].gamePlayer[side].someone = true;  //表示有人
                        //service.SetListBox(string.Format(
                        //    "用户：{0}，在第{1}桌第{2}座入座", user.userName, tableIndex + 1, side + 1));
                        //得到对家座位号
                        anotherSide = (side + 1) % 2;
                        //判断对方是否有人
                        if (gameTable[tableIndex].gamePlayer[anotherSide].someone)  //表示对面有人
                        {
                            //先告诉该用户对家已经入座
                            //发送格式:SitDown,座位号,用户名
                            sendString = string.Format("Host,{0},{1},{2}", side,
                                gameTable[tableIndex].gamePlayer[side].user.userName,"正在AI托管");
                            service.SendToOne(gameTable[tableIndex].gamePlayer[anotherSide].user, sendString);
                        }
                        break;
                    case "HostGrade":   //单纯发送成绩
                        tableIndex = int.Parse(splitString[1]);     //得到桌子号索引，从第0号桌子开始
                        side = int.Parse(splitString[2]);              //得到座位号，0代表黑子，1代表白子
                        gameTable[tableIndex].gamePlayer[side].someone = true;  //表示有人
                        //service.SetListBox(string.Format(
                        //    "用户：{0}，在第{1}桌第{2}座入座", user.userName, tableIndex + 1, side + 1));
                        //得到对家座位号
                        anotherSide = (side + 1) % 2;
                        //判断对方是否有人
                        if (gameTable[tableIndex].gamePlayer[anotherSide].someone)  //表示对面有人
                        {
                            //先告诉该用户对家已经入座
                            //发送格式:SitDown,座位号,用户名
                            sendString = string.Format("HostGrade,{0},{1},{2}", side,
                                splitString[3].ToString(), splitString[4].ToString());
                            service.SendToOne(gameTable[tableIndex].gamePlayer[anotherSide].user, sendString);
                        }
                        break;
                    case "HostDotGrid":
                        tableIndex = int.Parse(splitString[1]);
                        side = int.Parse(splitString[5]);  //得到座位号
                        anotherSide = (side + 1) % 2;
                         x = int.Parse(splitString[2]);
                         y = int.Parse(splitString[3]);
                        int state= int.Parse(splitString[4]);   //表示黑子还是白子
                        //gameTable[tableIndex].SetDot(x, y, side);
                        sendString = string.Format("HostDotGrid,{0},{1},{2},{3}", side,x, y,state);
                        service.SendToOne(gameTable[tableIndex].gamePlayer[anotherSide].user, sendString);
                        //service.SendToOne(this, string.Format("SetDotCollection,{0},{1},{2},{3}", sTran, this.gamePlayer[side].grade, this.gamePlayer[otherside].grade, turn));
                        break;
                    case "FreshPic":   //单纯刷新界面
                        tableIndex = int.Parse(splitString[1]);     //得到桌子号索引，从第0号桌子开始
                        side = int.Parse(splitString[2]);              //得到座位号，0代表黑子，1代表白子
                        gameTable[tableIndex].gamePlayer[side].someone = true;  //表示有人
                        //service.SetListBox(string.Format(
                        //    "用户：{0}，在第{1}桌第{2}座入座", user.userName, tableIndex + 1, side + 1));
                        //得到对家座位号
                        anotherSide = (side + 1) % 2;
                        //判断对方是否有人
                        if (gameTable[tableIndex].gamePlayer[anotherSide].someone)  //表示对面有人
                        {
                            //先告诉该用户对家已经入座
                            //发送格式:SitDown,座位号,用户名
                            sendString = string.Format("FreshPic,{0}", side);
                            service.SendToOne(gameTable[tableIndex].gamePlayer[anotherSide].user, sendString);
                        }
                        break;
                    case "CancelHost":   //取消托管
                        tableIndex = int.Parse(splitString[1]);     //得到桌子号索引，从第0号桌子开始
                        side = int.Parse(splitString[2]);              //得到座位号，0代表黑子，1代表白子
                        gameTable[tableIndex].gamePlayer[side].someone = true;  //表示有人
                        //service.SetListBox(string.Format(
                        //    "用户：{0}，在第{1}桌第{2}座入座", user.userName, tableIndex + 1, side + 1));
                        //得到对家座位号
                        anotherSide = (side + 1) % 2;
                        //判断对方是否有人
                        if (gameTable[tableIndex].gamePlayer[anotherSide].someone)  //表示对面有人
                        {
                            //先告诉该用户对家已经入座
                            //发送格式:SitDown,座位号,用户名CancelHost
                            sendString = string.Format("CancelHost,{0},{1}", anotherSide, gameTable[tableIndex].gamePlayer[anotherSide].user.userName);
                            service.SendToOne(gameTable[tableIndex].gamePlayer[anotherSide].user, sendString);

                            gameTable[tableIndex].SetTurn(anotherSide);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 将该用户从游戏桌上移除
        /// </summary>
        /// <param name="user"></param>
        private void RemoveClientfromPlayer(User user)
        {
            for (int i = 0; i < gameTable.Length; i++)  //多行
            {
                for (int j = 0; j < 2; j++)     //两列
                {
                    if (gameTable[i].gamePlayer[j].user != null)  //表示该用户在桌子里
                    {
                        if (gameTable[i].gamePlayer[j].user == user)  //找到掉线用户
                        {
                            StopPlayer(i, j);    //表示第i个桌子的，第j号棋子，不是黑子就是白子
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        private void StopPlayer(int i, int j)
        {
            gameTable[i].gamePlayer[j].someone = false;
            gameTable[i].gamePlayer[j].started = false;
            gameTable[i].gamePlayer[j].grade = 0;    //将该用户的成绩清零
            int otherSide = (j + 1) % 2;   //得到黑子或者白子
            if (gameTable[i].gamePlayer[otherSide].user.client.Connected)  //表示是否已近连接
            {
                gameTable[i].gamePlayer[otherSide].started = false;
                gameTable[i].gamePlayer[otherSide].grade = 0;
                if (gameTable[i].gamePlayer[otherSide].user.client.Connected)
                {
                    //发送第几号桌子的第几个玩家下线
                    service.SendToOne(gameTable[i].gamePlayer[otherSide].user, string.Format("Lost,{0},{1}", j, gameTable[i].gamePlayer[j].user.userName));
                }
            }
        }

        /// <summary>
        /// 得到所有桌子座位的状态，每个桌子占用，第一位代表黑子，第二位代表白子状态
        /// </summary>
        /// <returns></returns>
        private string GetOnlineString()
        {
            string str = "";
            for (int i = 0; i < gameTable.Length; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    //表示第i号桌子第j个玩家的状态，1表示有人，0表示没人
                    str += gameTable[i].gamePlayer[j].someone == true ? "1" : "0";
                }
            }
            return str;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (myListener != null)
            {
                buttonStop_Click(null, null);
            }
        }
    }
}
