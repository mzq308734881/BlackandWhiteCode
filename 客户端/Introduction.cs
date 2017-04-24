using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace Client
{
    public partial class Introduction : Office2007Form
    {
        public Introduction()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //默认IE打开
            System.Diagnostics.Process.Start("iexplore.exe", "http://download.csdn.net/download/qq751172625/2908475");  
        }
    }
}