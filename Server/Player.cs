using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    class Player
    {
        /// <summary>
        ///用户,客户端信息，包括用户名，读出和写入数据流
        /// </summary>
        public User user;  
        /// <summary>
        /// 是否开始
        /// </summary>
        public bool started;  
        /// <summary>
        ///分数
        /// </summary>
        public int grade;  
        /// <summary>
        /// //表示黑子或者白子位置的状态
        /// </summary>
        public bool someone;  
        public Player()
        {
            someone = false;
            started = false;
            grade = 0;
            user = null;
        }
    }
}
