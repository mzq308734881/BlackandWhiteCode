using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class User
    {
        /// <summary>
        /// 只读变量，只允许一次赋值
        /// </summary>
        public readonly TcpClient client;    //readOnly  为运行时常量，程序运行时进行赋值，赋值完成后便无法更改，因此也有人称其为只读变量。 
        /// <summary>
        /// 从字节流读取数据
        /// </summary>
        public readonly StreamReader sr;
        /// <summary>
        /// 向字节流写入数据
        /// </summary>
        public readonly StreamWriter sw;
        /// <summary>
        /// 用户名
        /// </summary>
        public string userName;
        public User(TcpClient client)
        {
            this.client = client;
            this.userName = "";
            NetworkStream netStream = client.GetStream();
            sr = new StreamReader(netStream, System.Text.Encoding.UTF8);
            sw = new StreamWriter(netStream, System.Text.Encoding.UTF8);
        }
    }
}
