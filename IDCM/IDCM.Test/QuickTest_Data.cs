using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.Data;
using System.Threading;

namespace IDCM.Test
{
    public partial class QuickTest_Data : Form
    {
        public QuickTest_Data()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 数据源连接快速检测事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //System.IO.File.Delete("F:/Test.mrc");
            //冒烟测试部分
            WorkSpaceManager wsm = new WorkSpaceManager("F:/Test.mrc", "jsdjkfhsdf");
            if (wsm.connect())
            {
                if(wsm.prepare())
                {
                    string connStr = wsm.getValidConnectStr();
                    long sid = DataSupporter.nextSeqID(wsm);
                    
                    this.richTextBox1.Text = "数据源连接快速检测\n#冒烟测试部分 通过。\n";
                    if (wsm.getLastError() != null)
                    {
                        this.richTextBox1.Text += "[lastError] " + wsm.getLastError().ToString();
                    }
                }
                else
                    this.richTextBox1.Text += "[lastError] " + wsm.getLastError().ToString();
                wsm.disconnect();
            }else
                this.richTextBox1.Text += "[lastError] " + wsm.getLastError().ToString();

            //重建连接并进行线程池请求测试
            if (wsm.connect())
            {
                if (wsm.prepare())
                {
                    ParameterizedThreadStart pts = new ParameterizedThreadStart(DBQueryTest);
                    Thread[] threads = new Thread[10];
                    int tx = 0;
                    while (tx < threads.Length)
                    {
                        threads[tx]=new Thread(pts);
                        tx++;
                    }
                    string ts = DateTime.Now.Ticks.ToString();
                    while (tx > 0)
                    {
                        tx--;
                        threads[tx].Start(new object[] { wsm, tx.ToString(), ts });
                    }
                    this.richTextBox1.Text += "线程池请求测试部分 通过。\n";
                }
            }
        }

        public void DBQueryTest(object wsmObj)
        {
            object[] pas=(wsmObj as object[]);
            for (int i = 0; i < 10; i++)
            {
                WorkSpaceManager wsm = pas[0] as WorkSpaceManager;
                long cc = DataSupporter.CountSQLQuery(wsm, "select count(*) from CustomTColDef");
                Console.WriteLine("@cc" + (pas[1] as string) + "=" + cc);
            }
            TimeSpan ts = new TimeSpan(DateTime.Now.Ticks-long.Parse(pas[2] as string));
            Console.WriteLine("线程" + (pas[1] as string) + "耗时=" + ts.TotalMilliseconds + "ms");
        }
    }
}
