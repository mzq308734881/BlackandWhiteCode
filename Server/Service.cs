using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Windows.Forms;

namespace Server
{
    class Service
    {
        private ListBox listbox;
        private delegate void SetListBoxCallback(string str);  //定义委托方法包含一个参数
        private SetListBoxCallback setListBoxCallback;
        public Service(ListBox listbox)
        {
            this.listbox = listbox;
            setListBoxCallback = new SetListBoxCallback(SetListBox);
        }
        public void SetListBox(string str)  //用于委托的方法
        {
            if (listbox.InvokeRequired)  //当前线程不是创建该控件的线程时为True
            {
                listbox.Invoke(setListBoxCallback, str);
            }
            else   //当前线程是创建该控件的线程
            {
                listbox.Items.Add(str);
                listbox.SelectedIndex = listbox.Items.Count - 1;  //选中最后一个
                listbox.ClearSelected();
            }
        }

        /// <summary>
        /// 向玩家单独发消息
        /// </summary>
        /// <param name="user">客户端</param>
        /// <param name="str"></param>
        public void SendToOne(User user, string str)
        {
            try
            {
                user.sw.WriteLine(str);
                user.sw.Flush();
                SetListBox(string.Format("向{0}发送{1}", user.userName, str));
            }
            catch
            {
                SetListBox(string.Format("向{0}发送信息失败", user.userName));
            }
        }

        /// <summary>
        /// 向两个棋手都发送消息
        /// </summary>
        /// <param name="gameTable"></param>
        /// <param name="str"></param>
        public void SendToBoth(GameTable gameTable, string str)
        {
            for (int i = 0; i < 2; i++)
            {
                if (gameTable.gamePlayer[i].someone == true)
                {
                    SendToOne(gameTable.gamePlayer[i].user, str);
                }
            }
        }

        /// <summary>
        /// 群发
        /// </summary>
        /// <param name="userList"></param>
        /// <param name="str"></param>
        public void SendToAll(List<User> userList, string str)
        {
            for (int i = 0; i < userList.Count; i++)
            {
                SendToOne(userList[i], str);
            }
        }
    }
}
